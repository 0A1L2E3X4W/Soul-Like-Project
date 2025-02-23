using UnityEngine;

public class EventTriggerBossFight : MonoBehaviour
{
    [Header("BOSS")]
    [SerializeField] int bossID;

    private void OnTriggerEnter(Collider other)
    {
        BossManager boss = WorldAIManager.Instance.GetBossByID(bossID);

        if (boss != null)
        {
            boss.WakeBoss();
        }
    }
}
