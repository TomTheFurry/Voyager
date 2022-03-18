using UnityEngine;
using UnityEngine.InputSystem;

namespace Voyager
{
    public class MovementControl : MonoBehaviour
    {
        // Child Object. Requires a Rigidbody component
        public GameObject childObject = null;
        // Target position for the child object. Doesn't need a Rigidbody component
        public GameObject targetIndicator = null;

        // Body direction is [Left/Right, Up/Down, Forward/Backward]
        // Body rotation is [negative pitch, yaw, negative roll]

        /*
         *  Rotation speed is in radians per second
         *  The target will be rotated using a PID controller (without I term)
         */
        public float inputRotationSpeed = 0.1f;
        public float rotationP = 0.01f;
        public float rotationD = 0.01f;
        public float targetRotationSpeed = 0.05f;

        /*
         *  Movement speed is in units per second
         */
        public float movementSpeed = 10f;

        // P and D values as in per second
        private static float calculatePD(float p, float d,
            float currentError,
            float lastError, float deltaTime)
        {
            return p * currentError + d * (currentError - lastError);
        }

        private float lastAngleError = 0f;

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

            // Lock the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

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

            float inputStop = input.actions["Stop"].ReadValue<float>();

            //Debug.Log(inputMove);
            Debug.Log(inputRotate);

            // Move
            inputMove *= movementSpeed;
            childObject.GetComponent<Rigidbody>().AddRelativeForce(inputMove, ForceMode.Acceleration);
            // Stop
            if (inputStop > 0)
            {
                float currentSpeed = childObject.GetComponent<Rigidbody>().velocity.magnitude;
                if (currentSpeed > 0)
                {
                    currentSpeed *= 0.01f;
                    childObject.GetComponent<Rigidbody>().AddForce(Vector3.ClampMagnitude(Vector3.Scale(childObject.GetComponent<Rigidbody>().velocity,
                        new Vector3(-currentSpeed,-currentSpeed,-currentSpeed)), movementSpeed),
                        ForceMode.Acceleration);
                }

            }


            // Copy position to target indicator
            targetIndicator.transform.position = childObject.transform.position;

            // Rotate target indicator
            Quaternion inputRot = Quaternion.Euler(targetIndicator.transform.TransformVector(inputRotate * inputRotationSpeed));

            targetIndicator.transform.rotation = inputRot * targetIndicator.transform.rotation;

            // Calculate rotation error angle
            Vector3 axis;
            float angle;
            Quaternion errorQuat = targetIndicator.transform.rotation * Quaternion.Inverse(childObject.transform.rotation);

            errorQuat.ToAngleAxis(out angle, out axis);

            angle = Mathf.DeltaAngle(0f, angle);

            Debug.Log("RotationError: " + angle + " " + axis);

            // Calculate PID
            angle = calculatePD(rotationP, rotationD, angle, lastAngleError, Time.deltaTime);

            angle = Mathf.DeltaAngle(0f, angle);

            Debug.Log("RotationCorrection: " + angle + " " + axis);

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
    }
}