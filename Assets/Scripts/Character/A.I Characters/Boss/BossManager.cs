using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class BossManager : AIManager
{
    [Header("BOSS ID")]
    public int bossID = 0;

    [Header("STATUS")]
    public NetworkVariable<bool> hasBeenDefeated = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> hasBeenAwakened = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            if (!WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {
                WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.Add(bossID, false);
                WorldSaveGameManager.Instance.currentCharacterData.bossesDefeated.Add(bossID, false);
            }
            else
            {
                hasBeenDefeated.Value = WorldSaveGameManager.Instance.currentCharacterData.bossesDefeated[bossID];
                hasBeenAwakened.Value = WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened[bossID];
            }

            if (hasBeenDefeated.Value)
            {
                aiNetworkManager.isActive.Value = false;
            }
        }
    }

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnim = false)
    {
        if (IsOwner)
        {
            characterNetworkManager.currentHealth.Value = 0;
            isDead.Value = true;

            if (!manuallySelectDeathAnim)
            {
                characterAnimatorManager.PlayTargetActionAnim("Death_01", true);
            }
        }

        hasBeenDefeated.Value = true;

        if (!WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
        {
            WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.Add(bossID, true);
            WorldSaveGameManager.Instance.currentCharacterData.bossesDefeated.Add(bossID, true);
        }
        else
        {
            WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.Remove(bossID);
            WorldSaveGameManager.Instance.currentCharacterData.bossesDefeated.Remove(bossID);

            WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.Add(bossID, true);
            WorldSaveGameManager.Instance.currentCharacterData.bossesDefeated.Add(bossID, true);
        }

        WorldSaveGameManager.Instance.SaveGame();

        yield return new WaitForSeconds(5);
    }
}
