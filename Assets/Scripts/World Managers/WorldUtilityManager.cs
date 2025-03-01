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
        if (atkCharacter == CharacterGroup.Player)
        {
            switch (targetCharacter)
            {
                case CharacterGroup.Player: return false;
                case CharacterGroup.Undead: return true;
                default:
                    break;
            }
        }
        else if (atkCharacter == CharacterGroup.Undead)
        {
            switch (targetCharacter)
            {
                case CharacterGroup.Player: return true;
                case CharacterGroup.Undead: return false;
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
