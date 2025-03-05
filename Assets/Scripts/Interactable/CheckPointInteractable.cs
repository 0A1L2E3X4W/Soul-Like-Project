using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class CheckPointInteractable : Interactable
{
    [Header("[ CHECK POINT ]")]
    [Header("CHECK POINT INFO")]
    [SerializeField] int checkpointID;

    [Header("ACTIVE")]
    public NetworkVariable<bool> isActivated = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("INTERACTION TEXT")]
    [SerializeField] string deactivatedText = "RESTORE LOST GRACE";
    [SerializeField] string activatedText = "REST";

    [Header("VFX")]
    [SerializeField] GameObject activatedParticles;

    protected override void Start()
    {
        base.Start();

        if (IsOwner)
        {
            if (WorldSaveGameManager.Instance.currentCharacterData.checkpoint.ContainsKey(checkpointID))
            {
                isActivated.Value = WorldSaveGameManager.Instance.currentCharacterData.checkpoint[checkpointID];
            }
            else
            {
                isActivated.Value = false;
            }
        }

        if (isActivated.Value)
        {
            interactableText = activatedText;
        }
        else
        {
            interactableText = deactivatedText;
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsOwner)
            OnIsActiveChanged(false, isActivated.Value);

        isActivated.OnValueChanged += OnIsActiveChanged;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        isActivated.OnValueChanged -= OnIsActiveChanged;
    }

    private void RestoreCheckPoint(PlayerManager player)
    {
        isActivated.Value = true;

        if (WorldSaveGameManager.Instance.currentCharacterData.checkpoint.ContainsKey(checkpointID))
            WorldSaveGameManager.Instance.currentCharacterData.checkpoint.Remove(checkpointID);

        WorldSaveGameManager.Instance.currentCharacterData.checkpoint.Add(checkpointID, true);

        player.playerAnimatorManager.PlayTargetActionAnim("Activate_Checkpoint_01", true);

        PlayerUIManager.Instance.playerUIPopUpManager.SendCheckpointPopUp(deactivatedText);

        StartCoroutine(WaitAndRestoreColliders());
    }

    private void RestAtCheckPoint(PlayerManager player)
    {
        PlayerUIManager.Instance.playerUIPopUpManager.SendCheckpointPopUp(activatedText);

        interactableCollider.enabled = true;
        player.playerNetworkManager.currentHealth.Value = player.playerNetworkManager.maxHealth.Value;
        player.playerNetworkManager.currentStamina.Value = player.playerNetworkManager.maxStamina.Value;

        WorldAIManager.Instance.ResetAllCharacters();
    }

    private IEnumerator WaitAndRestoreColliders()
    {
        yield return new WaitForSeconds(2);

        interactableCollider.enabled = true;
    }

    private void OnIsActiveChanged(bool oldstatus, bool newStatus)
    {
        if (isActivated.Value)
        {
            activatedParticles.SetActive(true);
            interactableText = activatedText;
        }
        else
        {
            interactableText = deactivatedText;
        }
    }

    public override void Interact(PlayerManager player)
    {
        base.Interact(player);

        if (!isActivated.Value)
        {
            RestoreCheckPoint(player);
        }
        else
        {
            RestAtCheckPoint(player);
        }
    }
}
