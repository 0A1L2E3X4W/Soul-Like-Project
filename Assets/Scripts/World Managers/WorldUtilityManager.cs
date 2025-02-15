using UnityEngine;

public class WorldUtilityManager : MonoBehaviour
{
    public static WorldUtilityManager Instance;

    [Header("LAYERS")]
    [SerializeField] private LayerMask characterLayer;
    [SerializeField] private LayerMask environmentLayer;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public LayerMask GetCharacterLayers() { return characterLayer; }

    public LayerMask GetEnvironmentLayers() { return environmentLayer; }

    public bool AbleAtkTarget(CharacterGroup atkCharacter, CharacterGroup targetCharacter)
    {
        if (atkCharacter == CharacterGroup.team01)
        {
            switch (targetCharacter)
            {
                case CharacterGroup.team01: return false;
                case CharacterGroup.team02: return true;
                default:
                    break;
            }
        }
        else if (atkCharacter == CharacterGroup.team02)
        {
            switch (targetCharacter)
            {
                case CharacterGroup.team01: return true;
                case CharacterGroup.team02: return false;
                default:
                    break;
            }
        }

        return false;
    }

    public float GetAngleOfTarget(Transform characterTransform, Vector3 targetsDir)
    {
        targetsDir.y = 0f;
        float viewableAngle = Vector3.Angle(characterTransform.forward, targetsDir);
        Vector3 cross = Vector3.Cross(characterTransform.forward, targetsDir);

        if (cross.y < 0f) { viewableAngle = -viewableAngle; }

        return viewableAngle;
    }
}
