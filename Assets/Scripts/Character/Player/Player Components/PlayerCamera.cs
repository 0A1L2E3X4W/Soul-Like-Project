using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance;

    [Header("MANAGER")]
    public PlayerManager player;

    [Header("CAMERA OBJECTS")]
    public Camera cameraObj;
    public Transform cameraPivotTransform;

    [Header("CAMERA SETTINGS")]
    [SerializeField] private float horizontalRotateSpeed = 220f;
    [SerializeField] private float verticalRotateSpeed = 180f;
    [SerializeField] private float minPivot = -30f;
    [SerializeField] private float maxPivot = 30f;
    [Space]
    [SerializeField] private float cameraCollisionRadius = 0.2f;
    [SerializeField] private LayerMask collideWithLayers;
    [Space]
    [SerializeField] private float cameraSmoothSpeed = 1f;

    [Header("CAMERA VALUES")]
    [SerializeField] private float horizontalLookAngle;
    [SerializeField] private float verticalLookAngle;

    [Header("RESOURCES")]
    private Vector3 cameraVelocity;
    private Vector3 cameraObjPosition;
    private float cameraZPosition;
    private float targetCameraZPosition;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        cameraZPosition = cameraObj.transform.localPosition.z;
    }

    public void HandleAllCameraActions()
    {
        if (player != null)
        {
            HandleFollowTarget();
            HandleRotation();
            HandleCollisions();
        }
    }

    private void HandleFollowTarget()
    {
        Vector3 targetCameraPos = Vector3.SmoothDamp(
            transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetCameraPos;
    }

    private void HandleRotation()
    {
        horizontalLookAngle += (PlayerInputManager.Instance.cameraHorizontalInput * horizontalRotateSpeed) * Time.deltaTime;
        verticalLookAngle -= (PlayerInputManager.Instance.cameraVerticalInput * verticalRotateSpeed) * Time.deltaTime;
        verticalLookAngle = Mathf.Clamp(verticalLookAngle, minPivot, maxPivot);

        Vector3 cameraRotation;
        Quaternion targetRotation;

        cameraRotation = Vector3.zero;
        cameraRotation.y = horizontalLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;

        cameraRotation = Vector3.zero;
        cameraRotation.x = verticalLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleCollisions()
    {
        targetCameraZPosition = cameraZPosition;
        RaycastHit hit;
        Vector3 dir = cameraObj.transform.position - cameraPivotTransform.position;
        dir.Normalize();

        if (Physics.SphereCast(
            cameraPivotTransform.position, cameraCollisionRadius, dir, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
        {
            float distanceFromHitObj = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetCameraZPosition = -(distanceFromHitObj - cameraCollisionRadius);
        }

        if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
        {
            targetCameraZPosition = -cameraCollisionRadius;
        }

        cameraObjPosition.z = Mathf.Lerp(cameraObj.transform.localPosition.z, targetCameraZPosition, cameraCollisionRadius);
        cameraObj.transform.localPosition = cameraObjPosition;
    }
}
