using UnityEngine;
using UnityEngine.InputSystem;

namespace Voyager
{
    public class MovementControl2 : MonoBehaviour
    {
        // Child Object. Requires a Rigidbody component
        public GameObject childObject = null;
        // Target position for the child object. Doesn't need a Rigidbody component
        public GameObject targetIndicator = null;
        // Camera to use for the child object
        public GameObject childCamera = null;

        // Body direction is [Left/Right, Up/Down, Forward/Backward]
        // Body rotation is [negative pitch, yaw, negative roll]

        /*
         *  Rotation speed is in radians per second
         *  The target will be rotated using a PID controller (without I term)
         */
        public float inputRotationSpeed = 0.8f;
        public float rotationP = 0.02f;
        public float rotationD = 0.02f;
        public float targetRotationSpeed = 0.05f;

        /*
         *  Movement speed is in units per second
         */
        public float movementSpeed = 5f;

        // P and D values as in per second
        private static float calculatePD(float p, float d,
            float currentError,
            float lastError, float deltaTime)
        {
            return p * currentError + d * (currentError - lastError);
        }

        private float lastAngleError = 0f;

        private bool mouseLocked = false;
        private void lockMouse()
        {
            if (!mouseLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                mouseLocked = true;
            }
        }

        private void unlockMouse()
        {
            if (mouseLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                mouseLocked = false;
            }
        }

        private void Start()
        {
            if (childObject == null)
            {
                Debug.LogError("Child object not set");
            }
            if (targetIndicator == null)
            {
                Debug.LogError("Target indicator not set");
            }
            lockMouse();

        }

        private bool isValidFloat(float f)
        {
            return !float.IsNaN(f) && !float.IsInfinity(f);
        }

        private void rotateTargetCamera(Vector3 inputRotate)
        {
            // Rotate target indicator relative to camera
            Quaternion inputRot = Quaternion.Euler(childCamera.transform.TransformVector(inputRotate * inputRotationSpeed));
            targetIndicator.transform.rotation = inputRot * targetIndicator.transform.rotation;
            childCamera.transform.rotation = inputRot * childCamera.transform.rotation;
        }
        private void rotateCamera(Vector3 inputRotate)
        {
            // Rotate camera
            Quaternion inputRot = Quaternion.Euler(childCamera.transform.TransformVector(inputRotate * inputRotationSpeed));
            childCamera.transform.rotation = inputRot * childCamera.transform.rotation;
        }
        
        private void snapPositionToChild()
        {
            // Snap position to child
            targetIndicator.transform.position = childObject.transform.position;
            childCamera.transform.position = childObject.transform.position;
        }

        private void snapCameraToTarget()
        {
            childCamera.transform.rotation = targetIndicator.transform.rotation;
        }

        private void snapTargetToCamera()
        {
            targetIndicator.transform.rotation = childCamera.transform.rotation;
            //targetIndicator.transform.rotation *= childCamera.transform.rotation;
            
        }

        private void stop(float inputStop)
        {
            // Stop
            if (inputStop > 0)
            {
                float currentSpeed = childObject.GetComponent<Rigidbody>().velocity.magnitude;
                if (currentSpeed > 0)
                {
                    currentSpeed *= 0.01f;
                    childObject.GetComponent<Rigidbody>().AddForce(Vector3.ClampMagnitude(Vector3.Scale(childObject.GetComponent<Rigidbody>().velocity,
                        new Vector3(-currentSpeed, -currentSpeed, -currentSpeed)), movementSpeed),
                        ForceMode.Acceleration);
                }
            }
        }

        private void move(Vector3 inputMove)
        {
            // Move child relative to camera
            inputMove *= movementSpeed;
            childObject.GetComponent<Rigidbody>().AddForce(childCamera.transform.TransformVector(inputMove),
                ForceMode.Acceleration);
            //childObject.GetComponent<Rigidbody>().AddRelativeForce(inputMove, ForceMode.Acceleration);
        }

        private void updateRotation()
        {
            // Calculate rotation error angle
            Vector3 axis;
            float angle;
            Quaternion errorQuat = targetIndicator.transform.rotation * Quaternion.Inverse(childObject.transform.rotation);


            errorQuat.ToAngleAxis(out angle, out axis);

            angle = Mathf.DeltaAngle(0f, angle);
            if (!isValidFloat(axis.x) || !isValidFloat(axis.y) || !isValidFloat(axis.z))
            {
                lastAngleError = 0;
                return;
            }
            //Debug.Log("RotationError: " + angle + " " + axis);

            // Calculate PID
            angle = calculatePD(rotationP, rotationD, angle, lastAngleError, Time.deltaTime);

            angle = Mathf.DeltaAngle(0f, angle);

            //Debug.Log("RotationCorrection: " + angle + " " + axis);

            // Update last error
            lastAngleError = angle;
            Vector3 targetAngularVelocity = axis * angle;
            Vector3 currentAngularVelocity = childObject.GetComponent<Rigidbody>().angularVelocity;
            // Calculate torque to apply
            Vector3 torque = targetAngularVelocity - currentAngularVelocity;
            // Apply torque
            childObject.GetComponent<Rigidbody>().AddTorque(torque, ForceMode.Acceleration);
            //hildObject.transform.Rotate(axis, angle, Space.World);
        }

        void Update()
        {
            if (childObject == null || targetIndicator == null)
            {
                return;
            }



            PlayerInput input = GetComponent<PlayerInput>();
            // Normalized
            Vector3 inputMove = input.actions["Move"].ReadValue<Vector3>();
            // X,Y normalized. z clamped.
            Vector3 inputRotate = new Vector3(
                Mathf.Clamp(-input.actions["Rotate"].ReadValue<Vector2>().y / 10, -1, 1),
                Mathf.Clamp(input.actions["Rotate"].ReadValue<Vector2>().x / 10, -1, 1),
                -input.actions["Roll"].ReadValue<float>());
            float inputFocus = input.actions["Focus"].ReadValue<float>();
            float inputStop = input.actions["Stop"].ReadValue<float>();
            bool inputTab = input.actions["Tab"].triggered;
            bool inputMiddle = input.actions["Middle"].ReadValue<float>() > 0;
            bool inputEscape = input.actions["Escape"].triggered;
            bool inputClick = input.actions["Click"].triggered;
            if (inputEscape)
            {
                unlockMouse();
            } else if (inputClick)
            {
                lockMouse();
            }

            //Debug.Log(inputMove);
            //Debug.Log(inputRotate);
            
            // stop
            stop(inputStop);
            // move
            move(inputMove);
            // Copy position to all children
            snapPositionToChild();
            updateRotation();
            // tab snap
            if (inputMiddle) snapTargetToCamera();
            if (inputTab) snapCameraToTarget();
            // rotate target or camera based on focus
            if (mouseLocked)
            {
                if (inputFocus == 1f)
                {
                    rotateTargetCamera(inputRotate);
                }
                else
                {
                    rotateCamera(inputRotate);
                }
            }
        }
    }
}