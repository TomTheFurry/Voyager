using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(FuelControl), typeof(HealthSystem))]
public class MovementControl2 : MonoBehaviour
{
    private FuelControl fuelControl;
    private HealthSystem healthSystem;
    private bool isSlowingPlayer = false;

    private void OnEnable()
    {
        if (mouseLocked) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            mouseLocked = true;
        }
    }

    private void OnDisable()
    {
        if (mouseLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            mouseLocked = false;
        }
    }

    public void SlowPlayer()
    {
        isSlowingPlayer = true;
    }

    // Child Object. Requires a Rigidbody component
    public GameObject childObject = null;
    // Target position for the child object. Doesn't need a Rigidbody component
    public Transform targetIndicator = null;
    // Camera to use for the child object
    public GameObject childCamera = null;
    public Camera childActualCamera = null;
    public Camera fpCamera = null;
    private bool inFp = false;

    public UnityEvent onEnterFp;
    public UnityEvent onLeaveFp;

    public UnityEvent<Vector3> onForceApplied;

    // Body direction is [Left/Right, Up/Down, Forward/Backward]
    // Body rotation is [negative pitch, yaw, negative roll]

    /*
        *  Rotation speed is in radians per second
        *  The target will be rotated using a PID controller (without I term)
        */
    public float inputRotationSpeed = 0.3f;
    public PDController rotationPD = new PDController();
    public float targetRotationSpeed = 0.05f;

    public float zoomMin = 1.0f;
    public float zoomMax = 20.0f;
    public float zoomSpeed = 0.1f;
    public float zoomTarget = 5.0f;
    public float camOffset = 1.0f;
    public PDController zoomPD = new PDController();

    public Vector3 maxForce;
    public Vector3 maxNegForce;

    public bool qeAsRoll = true;

    public EmissionHookup[] upEmissionHookup;
    public EmissionHookup[] downEmissionHookup;
    public EmissionHookup[] leftEmissionHookup;
    public EmissionHookup[] rightEmissionHookup;
    public EmissionHookup[] forwardEmissionHookup;
    public EmissionHookup[] backwardEmissionHookup;
        
    private bool readSpaceshipInput = true;
    public void DisableSpaceshipInput()
    {
        readSpaceshipInput = false;
    }
    public void EnableSpaceshipInput()
    {
        readSpaceshipInput = true;
    }

    // P and D values as in per second
    private static float calculatePD(float p, float d,
        float currentError,
        float lastError, float deltaTime)
    {
        return p * currentError + d * (currentError - lastError);
    }

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
        fuelControl = GetComponent<FuelControl>();
        healthSystem = GetComponent<HealthSystem>();

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
        targetIndicator.rotation = inputRot * targetIndicator.rotation;
        childCamera.transform.rotation = inputRot * childCamera.transform.rotation;
    }
    private void rotateCamera(Vector3 inputRotate)
    {
        // Rotate camera
        Quaternion inputRot = Quaternion.Euler(childCamera.transform.TransformVector(inputRotate * inputRotationSpeed));
        //childCamera.transform.rotation = inputRot * childCamera.transform.rotation;
        targetIndicator.rotation = inputRot * targetIndicator.rotation;
    }
        
    private void snapPositionToChild()
    {
        // Snap position to child
        targetIndicator.position = childObject.transform.position;
        childCamera.transform.position = childObject.transform.position;
    }

    private void snapCameraToTarget()
    {
        childCamera.transform.rotation = targetIndicator.rotation;
    }

    private void snapTargetToCamera()
    {
        targetIndicator.rotation = childCamera.transform.rotation;
        //targetIndicator.transform.rotation *= childCamera.transform.rotation;
    }

    private static void setEmission(EmissionHookup[] hookups, float force, float max) {
        float percent = Mathf.Clamp01(force / max);
        for (int i = 0; i < hookups.Length; i++)
        {
            hookups[i].SetEmissionRate(percent);
        }
    }
    private void setAllEmissionHookups(Vector3 value)
    {
        setEmission(upEmissionHookup, value.y, maxForce.y);
        setEmission(downEmissionHookup, value.y, maxNegForce.y);
        setEmission(leftEmissionHookup, value.x, maxNegForce.x);
        setEmission(rightEmissionHookup, value.x, maxForce.x);
        setEmission(forwardEmissionHookup, value.z, maxForce.z);
        setEmission(backwardEmissionHookup, value.z, maxNegForce.z);
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
        // Send event OnForceApplied for other scripts to use if needed
        setAllEmissionHookups(moveForce);
        fuelControl.RecordForce(moveForce);
        onForceApplied.Invoke(moveForce);

    }
    private void quickStop()
    {
        Vector3 stopForce = Vector3.zero;
        // Stop
        stopForce = -childObject.GetComponent<Rigidbody>().velocity;
        stopForce = childObject.transform.InverseTransformVector(stopForce);
        stopForce *= 0.1f;
        // Note: This is not needed for now because the value is already scaled to maxForce
        childObject.GetComponent<Rigidbody>().AddForce(childObject.transform.TransformVector(stopForce),
            ForceMode.Acceleration);
        // Send event OnForceApplied for other scripts to use if needed
        setAllEmissionHookups(stopForce);
        onForceApplied.Invoke(stopForce);

    }

    private void updateRotation()
    {
        // Calculate rotation error angle
        Vector3 axis;
        float angle;
        Quaternion errorQuat = targetIndicator.rotation * Quaternion.Inverse(childObject.transform.rotation);


        errorQuat.ToAngleAxis(out angle, out axis);

        angle = Mathf.DeltaAngle(0f, angle);
        if (!isValidFloat(axis.x) || !isValidFloat(axis.y) || !isValidFloat(axis.z))
        {
            rotationPD.reset();
            return;
        }
        //Debug.Log("RotationError: " + angle + " " + axis);

        //Debug.Log("RotationCorrection: " + angle + " " + axis);
        Vector3 targetAngularVelocity = axis * angle * (float)rotationPD.p;
        Vector3 currentAngularVelocity = childObject.GetComponent<Rigidbody>().angularVelocity * (float)rotationPD.d;
        // Calculate torque to apply
        Vector3 torque = targetAngularVelocity - currentAngularVelocity;
        // Apply torque
        childObject.GetComponent<Rigidbody>().AddTorque(torque, ForceMode.VelocityChange);
        //hildObject.transform.Rotate(axis, angle, Space.World);
    }

    private void updateZoom(float inputZoom)
    {
        zoomTarget = Mathf.Clamp(zoomTarget + inputZoom * zoomSpeed, zoomMin, zoomMax);
            
        float zoomTrueTarget = zoomTarget;


        float currentZoom = -childActualCamera.transform.localPosition.z;
        // Calculate zoom error
        float zoomError = zoomTrueTarget - currentZoom;

        // Calculate PID
        float value = currentZoom + (float)zoomPD.tick(zoomError);

        // Raycast to check if camera is not colliding with something
        Vector3 back = childActualCamera.transform.TransformDirection(Vector3.back);
        RaycastHit hit;
        // Raycast but ignore player & UI layerp
        if (Physics.SphereCast(childCamera.transform.position, camOffset, back, out hit, value,
            ~LayerMask.GetMask("Player", "UI", "Ignore Raycast")))
        {
            //Debug.Log("Zoom collision: " + hit.distance);
            value = hit.distance - camOffset;
        }

        // Apply zoom
        childActualCamera.transform.localPosition = new Vector3(0, 0, -value);
        
        if (zoomTarget == zoomMin && !inFp)
        {
            childActualCamera.gameObject.SetActive(false);
            fpCamera.gameObject.SetActive(true);
            onEnterFp.Invoke();
            inFp = true;
        } else if (zoomTarget != zoomMin && inFp)
        {
            fpCamera.gameObject.SetActive(false);
            childActualCamera.gameObject.SetActive(true);
            onLeaveFp.Invoke();
            inFp = false;
        }
    }

    public void spaceshipTerminated()
    {
        readSpaceshipInput = false;
        zoomTarget = 5f;
        
        // Raycast to check if camera is not colliding with something
        Vector3 back = childActualCamera.transform.TransformDirection(Vector3.back);
        RaycastHit hit;
        // Raycast but ignore player & UI layer
        if (Physics.SphereCast(childCamera.transform.position, camOffset, back, out hit, zoomTarget,
            ~LayerMask.GetMask("Player", "UI", "Ignore Raycast")))
        {
            //Debug.Log("Zoom collision: " + hit.distance);
            zoomTarget = hit.distance - camOffset;
        }

        // Apply zoom
        childActualCamera.transform.localPosition = new Vector3(0, 0, -zoomTarget);
        zoomPD.reset();

        if (zoomTarget != zoomMin && inFp)
        {
            fpCamera.gameObject.SetActive(false);
            childActualCamera.gameObject.SetActive(true);
            onLeaveFp.Invoke();
        }
    }

    public void toggleFullScreen()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        Screen.fullScreen = !Screen.fullScreen;
        Debug.Log("Toggled fullscreen to " + Screen.fullScreen);
    }

    public void OnFullScreen(InputValue value)
    {
        if (value.isPressed)
        {
            toggleFullScreen();
        }
    }

    float _inputZoom = 0;
    void Update()
    {
        PlayerInput input = GetComponent<PlayerInput>();

        if (childObject == null || targetIndicator == null || Time.timeScale == 0)
        {
            return;
        }

        bool inputMiddle = input.actions["Middle"].triggered;
        if (inputMiddle && readSpaceshipInput)
        {
            if (mouseLocked) unlockMouse(); else lockMouse();
        }

        _inputZoom += -input.actions["Zoom"].ReadValue<float>();
    }

    void FixedUpdate()
    {
        PlayerInput input = GetComponent<PlayerInput>();

        if (childObject == null || targetIndicator == null || Time.timeScale == 0)
        {
            return;
        }


        float inputZoom = _inputZoom;
        _inputZoom = 0f;

        float inputStop = input.actions["Stop"].ReadValue<float>();
        
        qeAsRoll = input.actions["Focus"].ReadValue<float>() != 1.0f;

        if (!readSpaceshipInput)
        {
            if (isSlowingPlayer) quickStop();

            snapPositionToChild();
            //updateRotation();
            updateZoom(mouseLocked ? inputZoom : 0);
            setAllEmissionHookups(Vector3.zero);
            onForceApplied.Invoke(Vector3.zero);
            return;
        }

        // Normalized
        Vector3 inputMove = input.actions["Move"].ReadValue<Vector3>();
        // X,Y normalized. z clamped.
        Vector3 inputRotate;
        if (!mouseLocked)
        {
            inputRotate = Vector3.zero;
        }
        else
        {

            if (qeAsRoll)
            {
                inputRotate = new Vector3(
                    Mathf.Clamp(-mouseDelta.y, -20, 20),
                    Mathf.Clamp(mouseDelta.x, -20, 20),
                    0);//-input.actions["Roll"].ReadValue<float>());
            }
            else
            {
                inputRotate = new Vector3(
                    Mathf.Clamp(-mouseDelta.y, -20, 20),
                    0,//input.actions["Roll"].ReadValue<float>(),
                    -Mathf.Clamp(mouseDelta.x, -20, 20));

            }
        }
        mouseDelta = Vector2.zero;
        float inputFocus = 0f; // input.actions["Focus"].ReadValue<float>();


        //Debug.Log(inputMove);
        //Debug.Log(inputRotate);

        // move and stop
        moveAndStop(inputMove, inputStop);
        // Copy position to all children
        snapPositionToChild();
        updateRotation();
        if (mouseLocked)
        {
            updateZoom(inputZoom);
        }
        else
        {
            updateZoom(0);
        }
        // tab snap
        // if (inputMiddle) snapTargetToCamera();
            
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