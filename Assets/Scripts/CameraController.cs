using System;
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
    [SerializeField] private Vector3 dragPosition;
    [SerializeField] private Vector3 actualDragPosition;

    [Header("Position-Configuration")]
    [SerializeField] private Vector3 actualPosition;

    [Header("Rotation-Configuration")]
    [SerializeField] private Quaternion actualRotation;
    [SerializeField][Range(0, 2)] private float rotationSpeed;
    [SerializeField] private Vector3 inicialRotPosition;
    [SerializeField] private Vector3 actualRotPosition;
    [SerializeField] bool mouseClicked= false;

    [Header("Zoom-Configuration")]
    [SerializeField] private Vector3 actualZoom;
    [SerializeField] private float multiplierZoom;
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;

    InputController inputController;
    InputAction inputAction;
    InputAction scrollAction;
    InputAction clickAction;


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
        scrollAction = inputController.UI.ScrollWheel;
        clickAction = inputController.UI.Click;

        inputController.Player.Enable();
        inputController.UI.Enable();

        inputController.UI.Click.performed += OnClicked;
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
        Vector2 scrollValue = scrollAction.ReadValue<Vector2>();

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

        float clickValue = clickAction.ReadValue<float>();

        if (!mouseClicked)
        {
            //rotacion
            inicialRotPosition = Mouse.current.position.ReadValue();
        }
        if (mouseClicked)
        {

            actualRotPosition = Mouse.current.position.ReadValue();

            Vector3 dif = inicialRotPosition - actualRotPosition;
            inicialRotPosition = actualRotPosition;
            actualRotation *= Quaternion.Euler(Vector3.up * - dif.x / 5);
            }
        if (!mouseClicked)
        {
            //drag & drop
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            float hit;

            if (plane.Raycast(ray, out hit))
            {
                dragPosition = ray.GetPoint(hit);

            }
        }
        if (mouseClicked)
        {
            //drag & drop
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            float hit;

            if (plane.Raycast(ray, out hit))
            {
                actualDragPosition = ray.GetPoint(hit);
                actualPosition = transform.position + dragPosition - actualDragPosition;
            }

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

    private void OnClicked(InputAction.CallbackContext inputContext)
    {
        var phase = inputContext.phase;

        switch (phase)
        {
            case InputActionPhase.Started:
                break;
            case InputActionPhase.Performed:
                mouseClicked = !mouseClicked;
                break;
            case InputActionPhase.Canceled:
                break;

        }
    }
}
