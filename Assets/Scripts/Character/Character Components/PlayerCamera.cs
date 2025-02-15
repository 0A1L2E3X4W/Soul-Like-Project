using System.Collections;
using System.Collections.Generic;
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

    [Header("LOCK ON SETTINGS")]
    [SerializeField] private float lockOnRadius = 20f;
    [SerializeField] private float minViewAngle = -60f;
    [SerializeField] private float maxViewAngle = 50f;
    [SerializeField] private float lockOnTargetFollowSpeed = 0.2f;
    [SerializeField] private float unlockedOnCameraHeight = 1.5f;
    [SerializeField] private float lockedOnCameraHeight = 2f;
    [SerializeField] private float setCameraHeightSpeed = 1.65f;

    [Header("LOCK ON TARGETS PARAMS")]
    public CharacterManager nearestLockOnTarget;
    public CharacterManager leftLockOnTarget;
    public CharacterManager rightLockOnTarget;
    private List<CharacterManager> availableTargets = new();
    private Coroutine cameraLockOnHeightCoroutine;

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
        if (player.playerNetworkManager.isLockedOn.Value)
        {
            Vector3 rotationDir = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - transform.position;
            rotationDir.Normalize();
            rotationDir.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(rotationDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lockOnTargetFollowSpeed);

            rotationDir = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - cameraPivotTransform.position;
            rotationDir.Normalize();

            targetRotation = Quaternion.LookRotation(rotationDir);
            cameraPivotTransform.transform.rotation = Quaternion.Slerp(cameraPivotTransform.rotation, targetRotation, lockOnTargetFollowSpeed);

            horizontalLookAngle = transform.eulerAngles.y;
            verticalLookAngle = transform.eulerAngles.x;
        }
        else
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

    // LOCK ON
    public void HandleLocateLockOnTarget()
    {
        float shortestDistance = Mathf.Infinity;
        float shortestDistanceFromRight = Mathf.Infinity;
        float shortestDistanceFromLeft = -Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(player.transform.position, lockOnRadius, WorldUtilityManager.Instance.GetCharacterLayers());

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager lockOnTarget = colliders[i].GetComponent<CharacterManager>();

            if (lockOnTarget != null)
            {
                Vector3 lockOnTargetDir = lockOnTarget.transform.position - player.transform.position;
                float distanceFromTarget = Vector3.Distance(player.transform.position, lockOnTarget.transform.position);
                float viewAngle = Vector3.Angle(lockOnTargetDir, cameraObj.transform.forward);

                if (lockOnTarget.isDead.Value)
                    continue;

                if (lockOnTarget.transform.root == player.transform.root)
                    continue;

                if (viewAngle > minViewAngle && viewAngle < maxViewAngle)
                {
                    RaycastHit hit;

                    if (Physics.Linecast(player.playerCombatManager.lockOnTransform.position,
                        lockOnTarget.characterCombatManager.lockOnTransform.position, out hit,
                        WorldUtilityManager.Instance.GetEnvironmentLayers()))
                    {
                        continue;
                    }
                    else
                    {
                        availableTargets.Add(lockOnTarget);
                    }
                }
            }
        }

        for (int k = 0; k < availableTargets.Count; k++)
        {
            if (availableTargets[k] != null)
            {
                float distanceFromTarget = Vector3.Distance(player.transform.position, availableTargets[k].transform.position);
                Vector3 lockTargetDir = availableTargets[k].transform.position - player.transform.position;

                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTargets[k];
                }

                if (player.playerNetworkManager.isLockedOn.Value)
                {
                    Vector3 relativeEnemyPos = player.transform.InverseTransformPoint(availableTargets[k].transform.position);

                    var distanceFromLeftTarget = relativeEnemyPos.x;
                    var distanceFromRightTarget = relativeEnemyPos.x;

                    if (availableTargets[k] != player.playerCombatManager.currentTarget)
                        continue;

                    if (relativeEnemyPos.x <= 0.00 && distanceFromLeftTarget > shortestDistanceFromLeft)
                    {
                        shortestDistanceFromLeft = distanceFromLeftTarget;
                        leftLockOnTarget = availableTargets[k];
                    }
                    else if (relativeEnemyPos.x >= 0.00 && distanceFromRightTarget < shortestDistanceFromLeft)
                    {
                        shortestDistanceFromRight = distanceFromLeftTarget;
                        rightLockOnTarget = availableTargets[k];
                    }
                }
            }
            else
            {
                ClearLockOnTargets();
                player.playerNetworkManager.isLockedOn.Value = false;
            }
        }
    }

    public void SetLockedOnCameraHeight()
    {
        if (cameraLockOnHeightCoroutine != null)
            StopCoroutine(cameraLockOnHeightCoroutine);

        cameraLockOnHeightCoroutine = StartCoroutine(SetCameraLockedOnHeight());
    }

    public void ClearLockOnTargets()
    {
        nearestLockOnTarget = null;
        leftLockOnTarget = null;
        rightLockOnTarget = null;

        availableTargets.Clear();
    }

    public IEnumerator WaitFindNewTarget()
    {
        while (player.isPerformingAction)
        {
            yield return null;
        }

        ClearLockOnTargets();

        HandleLocateLockOnTarget();

        if (nearestLockOnTarget != null)
        {
            player.playerCombatManager.SetTarget(nearestLockOnTarget);
            player.playerNetworkManager.isLockedOn.Value = true;
        }

        yield return null;
    }

    public IEnumerator SetCameraLockedOnHeight()
    {
        float duration = 1f;
        float timer = 0f;

        Vector3 velocity = Vector3.zero;
        Vector3 newLockedCameraHeight = new(cameraPivotTransform.transform.localPosition.x, lockedOnCameraHeight);
        Vector3 newUnlockedCameraHeight = new(cameraPivotTransform.transform.localPosition.x, unlockedOnCameraHeight);

        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (player != null)
            {
                if (player.playerCombatManager.currentTarget != null)
                {
                    cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(
                        cameraPivotTransform.transform.localPosition, newLockedCameraHeight, ref velocity, setCameraHeightSpeed);
                    cameraPivotTransform.transform.localRotation = Quaternion.Slerp(
                        cameraPivotTransform.transform.localRotation, Quaternion.Euler(0, 0, 0), lockOnTargetFollowSpeed);
                }
                else
                {
                    cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(
                       cameraPivotTransform.transform.localPosition, newUnlockedCameraHeight, ref velocity, setCameraHeightSpeed);
                }
            }

            yield return null;
        }

        if (player != null)
        {
            if (player.playerCombatManager.currentTarget != null)
            {
                cameraPivotTransform.transform.localPosition = newLockedCameraHeight;
                cameraPivotTransform.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                cameraPivotTransform.transform.localPosition = newUnlockedCameraHeight;
            }
        }

        yield return null;
    }
}
