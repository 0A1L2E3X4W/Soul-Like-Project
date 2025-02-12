using UnityEngine;

public class DestoryAfterTime : MonoBehaviour
{
    [Header("UTILITY SETTINGS")]
    [SerializeField] float timeUntilDestroyed = 4f;

    private void Awake()
    {
        Destroy(gameObject, timeUntilDestroyed);
    }
}
