using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the camera's movement, direction and height. It also temporary raycasts for clicking on gameobjects.
/// </summary>
public class CameraController : MonoBehaviour
{
    public Pawn pawn;
    private Camera _camera;
    private Transform cameraParent;
    public float cameraSpeed = 10f;
    public float cameraRotationSpeed = 180f;
    const float RAYCAST_DISTANCE = 500f;
    LayerMask clickMask;
    LayerMask cameraMask;
    private Clickable selected;

    [SerializeField]
    Transform target;
    Vector3 targetOffset = Vector3.up * 10f;
    const float SPRINT_MULTIPLIER = 1.5f;
    const float CAMERA_MAX_HEIGHT = 100f;
    const float CAMERA_MIN_HEIGHT = 2f;
    const float CAMERA_MIN_DISTANCE = -50f;
    const float CAMERA_MAX_DISTANCE = -30f;
    const float CAMERA_HEIGHT_STEP = 2.5f;
    const float CAMERA_DISTANCE_STEP = 1f;
    float targetCameraHeight = 30f;
    float targetCameraDistance = -40f;

    private void Start()
    {
        _camera = Camera.main;
        cameraParent = _camera.transform.parent;
        clickMask = 1 << 6;
        cameraMask = 6;
    }

    void Update()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(target.position + targetOffset, Vector3.down, out hitInfo, RAYCAST_DISTANCE, ~clickMask))
        {
            transform.position = hitInfo.point;
        }

        Vector3 movement = (transform.forward * movementInput.y + transform.right * movementInput.x) * cameraSpeed * Time.deltaTime * ((sprint) ? SPRINT_MULTIPLIER : 1f) / GameSpeed.TimeScale;
        movement = Quaternion.Euler(transform.forward) * movement;
        transform.position += movement;

        transform.Rotate(Vector3.up, rotationInput * cameraRotationSpeed * Time.deltaTime);

        Vector3 camPos = cameraParent.localPosition;
        camPos.y = Mathf.Lerp(camPos.y, targetCameraHeight, Time.deltaTime * 2.5f);
        camPos.z = Mathf.Lerp(camPos.z, targetCameraDistance, Time.deltaTime * 2.5f);
        cameraParent.localPosition = camPos;
        cameraParent.LookAt(target, Vector3.up);
    }

    Vector2 movementInput;
    float rotationInput = 0;
    bool sprint = false;
    public void a_Move(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// React to Q or E keys to rotate camera
    /// </summary>
    public void a_RotateCamera(InputAction.CallbackContext context)
    {
        rotationInput = context.ReadValue<float>();
    }

    /// <summary>
    /// React to a mouse click and triggers a raycast
    /// </summary>
    public void a_Click(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            RaycastHit hitInfo;
            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out hitInfo, RAYCAST_DISTANCE))
            {
                Clickable clickable = hitInfo.collider.GetComponent<Clickable>();
                if (clickable)
                {
                    if (selected != clickable)
                    {
                        if (selected)
                            selected.Deselect();

                        clickable.Select();
                    }

                    selected = clickable;
                    UIManager.singleton.UpdateSelectionPanel(true, selected);
                }
            }
            else if (selected)
            {
                selected.Deselect();
                selected = null;
                UIManager.singleton.UpdateSelectionPanel(false, null);
            }
        }
    }

    /// <summary>
    /// React to a scroll input and set the target for a smooth movment in Update()
    /// </summary>
    public void a_Scroll(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            float scrollValue = context.ReadValue<float>();

            if (scrollValue < 0)
            {
                targetCameraHeight += CAMERA_HEIGHT_STEP;
                targetCameraDistance -= CAMERA_DISTANCE_STEP;
            }
            else if (scrollValue > 0)
            {
                targetCameraHeight -= CAMERA_HEIGHT_STEP;
                targetCameraDistance += CAMERA_DISTANCE_STEP;
            }

            targetCameraHeight = Mathf.Clamp(targetCameraHeight, CAMERA_MIN_HEIGHT, CAMERA_MAX_HEIGHT);
            targetCameraDistance = Mathf.Clamp(targetCameraDistance, CAMERA_MIN_DISTANCE, CAMERA_MAX_DISTANCE);
        }
    }

    /// <summary>
    /// Increase camera movement speed when key is pressed
    /// </summary>
    public void a_CameraSprint(InputAction.CallbackContext context)
    {
        if (context.started)
            sprint = true;
        else if (context.canceled)
            sprint = false;
    }

    /// <summary>
    /// Set time scale
    /// </summary>
    public void a_ChangeGameSpeed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameSpeed.SetGameSpeed((int)context.ReadValue<float>());
        }
    }
}