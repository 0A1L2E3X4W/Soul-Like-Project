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
}
