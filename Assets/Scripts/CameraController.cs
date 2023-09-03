using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Singleton
    public static CameraController instance;

    public Transform cameraTransform;
    private Camera camera;

    [Header("Movement-Configuration")]
    [SerializeField][Range (0,10)] private float movementSpeed;
    [SerializeField][Range (0,10)] private float movementTime;
    [SerializeField][Range (0,10)] private float normalSpeed;
    [SerializeField][Range (0,10)] private float fastSpeed;

    [Header("Position-Configuration")]
    [SerializeField] private Vector3 actualPosition;

    [Header("Rotation-Configuration")]
    [SerializeField] private Quaternion actualRotation;

    [Header("Zoom-Configuration")]
    [SerializeField] private Vector3 actualZoom;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        actualPosition = transform.position;
        actualRotation = transform.rotation;
        actualZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MouseController()
    {

    }

    void MovementManager()
    {

    }
}
