using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    //Singleton
    public static CameraController instance;

    public Transform cameraTransform;
    private Camera camera;

    [Header("Movement-Configuration")]
    [SerializeField][Range (0,2)] private float movementSpeed;
    [SerializeField][Range (0,10)] private float movementTime;
    [SerializeField][Range (0,2)] private float normalSpeed;
    [SerializeField][Range (0,10)] private float fastSpeed;

    [Header("Position-Configuration")]
    [SerializeField] private Vector3 actualPosition;

    [Header("Rotation-Configuration")]
    [SerializeField] private Quaternion actualRotation;
    [SerializeField][Range(0, 2)] private float rotationSpeed;

    [Header("Zoom-Configuration")]
    [SerializeField] private Vector3 actualZoom;
    [SerializeField] private float multiplierZoom;
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;

    InputController inputController;
    InputAction inputAction;
    InputAction UIAction;


    private void Awake()
    {
        instance = this;
        inputController = new InputController();
    }

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponentInChildren<Camera>();
        actualPosition = transform.position;
        actualRotation = transform.rotation;
        actualZoom = camera.transform.localPosition;
        cameraTransform = camera.transform;
    }

    private void OnEnable()
    {
        inputAction = inputController.Player.Move;
        UIAction = inputController.UI.ScrollWheel;
        inputController.Player.Enable();
        inputController.UI.Enable();
    }

    private void OnDisable()
    {
        inputController.Player.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        MovementManager();
        MouseController();
    }

    void MouseController()
    {
        Vector2 scrollValue = UIAction.ReadValue<Vector2>();

        if (scrollValue.y > 0)
        {
            actualZoom += Vector3.up * multiplierZoom;
            actualZoom.y = Mathf.Clamp(actualZoom.y, minZoom, maxZoom);
        }
        if (scrollValue.y < 0)
        {
            actualZoom -= Vector3.up * multiplierZoom;
            actualZoom.y = Mathf.Clamp(actualZoom.y, minZoom, maxZoom);
        }

    }

    void MovementManager()
    {
        Vector2 action = inputAction.ReadValue<Vector2>();


        #region Movement
        if (action.y > 0)
        {
            actualPosition += transform.forward * movementSpeed;
        }
        if (action.y < 0)
        {
            actualPosition -= transform.forward * movementSpeed;
        }
        if (action.x > 0)
        {
            actualPosition += transform.right * movementSpeed;
        }
        if (action.x < 0)
        {
            actualPosition -= transform.right * movementSpeed;
        }

        #endregion

        Vector2 rotationAction =inputController.Player.Rotation.ReadValue<Vector2>();

        if (rotationAction.x < 0)
        {
            actualRotation *= Quaternion.Euler(Vector3.up * -rotationSpeed);
        }
        if (rotationAction.x > 0)
        {
            actualRotation *= Quaternion.Euler(Vector3.up * rotationSpeed);
        }


        transform.position = Vector3.Lerp(transform.position, actualPosition , Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, actualRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition,actualZoom, Time.deltaTime * movementTime);
    }
}
