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
        public float inputRotationSpeed = 0.3f;
        public float rotationP = 0.02f;
        public float rotationD = 0.02f;
        public float targetRotationSpeed = 0.05f;

        public Vector3 maxForce;
        public Vector3 maxNegForce;

        public bool qeAsRoll = true;

        private bool readInput = true;
        public void OnPausePlayerInput()
        {
            readInput = false;
        }
        public void OnResumePlayerInput()
        {
            readInput = true;
        }

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

        private Vector2 mouseDelta = Vector2.zero;

        public void OnRotate(InputValue value)
        {
            if (!mouseLocked) return;
            Vector2 delta = value.Get<Vector2>();
            mouseDelta += delta;
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
            if (maxForce.x < 0f || maxForce.y < 0f || maxForce.z < 0f)
            {
                Debug.LogError("Max force must be positive");
            }
            if (maxNegForce.x > 0f || maxNegForce.y > 0f || maxNegForce.z > 0f)
            {
                Debug.LogError("Max negative force must be negative");
            }

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
            //childCamera.transform.rotation = inputRot * childCamera.transform.rotation;
            targetIndicator.transform.rotation = inputRot * targetIndicator.transform.rotation;
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

        private void moveAndStop(Vector3 inputMove, float inputStop)
        {
            Vector3 stopForce = Vector3.zero;
            // Stop
            if (inputStop > 0)
            {
                    stopForce = -childObject.GetComponent<Rigidbody>().velocity;
                    stopForce = childObject.transform.InverseTransformVector(stopForce);
            }
            
            // Move child relative to camera
            Vector3 moveForce = new Vector3(0, 0, 0);
            moveForce.x = inputMove.x == 0 ? 0 : inputMove.x > 0 ? maxForce.x : maxNegForce.x;
            moveForce.y = inputMove.y == 0 ? 0 : inputMove.y > 0 ? maxForce.y : maxNegForce.y;
            moveForce.z = inputMove.z == 0 ? 0 : inputMove.z > 0 ? maxForce.z : maxNegForce.z;

            stopForce.x = stopForce.x > 0 ? Mathf.Min(maxForce.x, stopForce.x) : Mathf.Max(maxNegForce.x, stopForce.x);
            stopForce.y = stopForce.y > 0 ? Mathf.Min(maxForce.y, stopForce.y) : Mathf.Max(maxNegForce.y, stopForce.y);
            stopForce.z = stopForce.z > 0 ? Mathf.Min(maxForce.z, stopForce.z) : Mathf.Max(maxNegForce.z, stopForce.z);

            if (moveForce.x == 0f) moveForce.x = stopForce.x * inputStop;
            if (moveForce.y == 0f) moveForce.y = stopForce.y * inputStop;
            if (moveForce.z == 0f) moveForce.z = stopForce.z * inputStop;

            // Transform the vector from relative to cam into world space
            //inputMove = childObject.transform.InverseTransformVector(childCamera.transform.TransformVector(inputMove));
            // Clamp the input based on the max force we can apply on one of the axis
            /*float rx = inputMove.x / maxForce.x;
            float ry = inputMove.y / maxForce.y;
            float rz = inputMove.z / maxForce.z;
            float nrx = inputMove.x / maxNegForce.x;
            float nry = inputMove.y / maxNegForce.y;
            float nrz = inputMove.z / maxNegForce.z;
            float max = Mathf.Max(rx, ry, rz, nrx, nry, nrz);
            if (max > 1)
            {
                //Debug.Log("Max force exceeded. Max ratio: " + max);
                inputMove /= max;
            }*/ // Note: This is not needed for now because the value is already scaled to maxForce
            childObject.GetComponent<Rigidbody>().AddForce(childObject.transform.TransformVector(moveForce),
                ForceMode.Acceleration);

            // Send Message OnForce for other scripts to use if needed
            SendMessage("OnForce", moveForce, SendMessageOptions.DontRequireReceiver);
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
            bool inputEscape = input.actions["Escape"].triggered;
            bool inputClick = input.actions["Click"].triggered;
            bool inputTab = input.actions["Tab"].triggered;
            if (inputTab) snapCameraToTarget();


            if (inputEscape)
            {
                unlockMouse();
            }
            else if (inputClick)
            {
                lockMouse();
            }
            qeAsRoll = !input.actions["Click"].IsPressed();

            if (!readInput)
            {
                snapPositionToChild();
                updateRotation();
                return;
            }



            // Normalized
            Vector3 inputMove = input.actions["Move"].ReadValue<Vector3>();
            // X,Y normalized. z clamped.
            Vector3 inputRotate;
            if (qeAsRoll)
            {
                inputRotate = new Vector3(
                    Mathf.Clamp(-mouseDelta.y, -20, 20),
                    Mathf.Clamp(mouseDelta.x, -20, 20),
                    -input.actions["Roll"].ReadValue<float>());
            }
            else
            {
                inputRotate = new Vector3(
                    Mathf.Clamp(-mouseDelta.y, -20, 20),
                    input.actions["Roll"].ReadValue<float>(),
                    -Mathf.Clamp(mouseDelta.x, -20, 20));

            }
            mouseDelta = Vector2.zero;
            float inputFocus = input.actions["Focus"].ReadValue<float>();
            float inputStop = input.actions["Stop"].ReadValue<float>();
            bool inputMiddle = input.actions["Middle"].ReadValue<float>() > 0;


            //Debug.Log(inputMove);
            //Debug.Log(inputRotate);
            
            // move and stop
            moveAndStop(inputMove, inputStop);
            // Copy position to all children
            snapPositionToChild();
            updateRotation();
            // tab snap
            if (inputMiddle) snapTargetToCamera();
            // rotate target or camera based on focus
            if (mouseLocked)
            {
                if (inputFocus == 1f)
                {
                    rotateCamera(inputRotate);
                }
                else
                {
                    rotateTargetCamera(inputRotate);
                }
            }
        }
    }
}