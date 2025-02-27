using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;

public class BossManager : AIManager
{
    [Header("BOSS ID")]
    public int bossID = 0;

    [Header("STATUS")]
    public NetworkVariable<bool> hasBeenDefeated = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> hasBeenAwakened = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> bossFightIsActive = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("FOG WALL")]
    [SerializeField] private List<FogWallsInteractable> fogWalls;

    [Header("SLEEP")]
    [SerializeField] private string sleepAnim = "Sleep_01";
    [SerializeField] private string awakenAnim = "Awaken_01";

    [Header("PHASE SHIFT")]
    public float hpForPhaseShift = 70;
    [SerializeField] private string phaseShiftAnim = "Phase_Change_01";

    [Header("BOSS STATES")]
    [SerializeField] private BossSleepState sleep;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        bossFightIsActive.OnValueChanged += OnBossFightIsActiveChanged;
        OnBossFightIsActiveChanged(false, bossFightIsActive.Value);

        if (IsOwner)
        {
            sleep = Instantiate(sleep);
            currentState = sleep;
        }

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

            StartCoroutine(GetFogWallFromObjManager());

            if (hasBeenAwakened.Value)
            {
                for (int i = 0; i < fogWalls.Count; i++)
                {
                    fogWalls[i].isActive.Value = true;
                }
            }

            if (hasBeenDefeated.Value)
            {
                for (int i = 0; i < fogWalls.Count; i++)
                {
                    fogWalls[i].isActive.Value = false;
                }

                aiNetworkManager.isActive.Value = false;
            }
        }

        if (!hasBeenAwakened.Value)
        {
            anim.Play(sleepAnim);
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        bossFightIsActive.OnValueChanged -= OnBossFightIsActiveChanged;
    }

    private IEnumerator GetFogWallFromObjManager()
    {
        while (WorldObjectManager.Instance.fogWalls.Count == 0)
            yield return new WaitForEndOfFrame();

        fogWalls = new();

        foreach (var fogWall in WorldObjectManager.Instance.fogWalls)
        {
            if (fogWall.fogWallID == bossID)
                fogWalls.Add(fogWall);
        }
    }

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnim = false)
    {
        if (IsOwner)
        {
            characterNetworkManager.currentHealth.Value = 0;
            isDead.Value = true;
            bossFightIsActive.Value = false;

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

    public void WakeBoss()
    {
        if (IsOwner)
        {
            if (!hasBeenAwakened.Value)
            {
                characterAnimatorManager.PlayTargetActionAnim(awakenAnim, true);
            }

            bossFightIsActive.Value = true;
            hasBeenAwakened.Value = true;
            currentState = idle;

            if (!WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {
                WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.Add(bossID, true);
            }
            else
            {
                WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.Remove(bossID);
                WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.Add(bossID, true);
            }

            for (int i = 0; i < fogWalls.Count; i++)
            {
                fogWalls[i].isActive.Value = true;
            }
        }
    }

    private void OnBossFightIsActiveChanged(bool oldStatus, bool newStatus)
    {
        if (bossFightIsActive.Value)
        {
            //WorldSoundFXManager.Instance.PlayBossTrack(bossIntroClip, bossFightLoopClip);

            GameObject bossHPBar = Instantiate(
                PlayerUIManager.Instance.playerUIHudManager.bossHpBarObj,
                PlayerUIManager.Instance.playerUIHudManager.bossHpBarParent);

            BossHPBar bossHpBar = bossHPBar.GetComponentInChildren<BossHPBar>();
            bossHpBar.EnableBossHPBar(this);
        }
        else
        {
            //WorldSoundFXManager.Instance.StopBossMusic();
        }
    }
}
