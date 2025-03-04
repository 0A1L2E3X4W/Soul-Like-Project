using Unity.Netcode;
using UnityEngine;

public class Interactable : NetworkBehaviour
{
    [Header("[ INTERACTABLE ]")]
    [Header("SETTINGS")]
    public string interactableText;
    [SerializeField] protected Collider interactableCollider;
    [SerializeField] protected bool hostOnlyInteract = true;

    protected virtual void Awake()
    {
        if (interactableCollider == null)
            interactableCollider = GetComponent<Collider>();
    }

    protected virtual void Start()
    {

    }

    public virtual void Interact(PlayerManager player)
    {
        Debug.Log("Interacted");

        if (!player.IsOwner)
            return;

        interactableCollider.enabled = false;

        player.playerInteractionManager.RemoveInteractionFromList(this);
        PlayerUIManager.Instance.playerUIPopUpManager.CloseAllPopUpWindows();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        PlayerManager player = other.GetComponent<PlayerManager>();

        if (player != null)
        {
            if (!player.playerNetworkManager.IsHost && hostOnlyInteract)
                return;

            if (!player.IsOwner)
                return;

            player.playerInteractionManager.AddInteractionToList(this);
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        PlayerManager player = other.GetComponent<PlayerManager>();

        if (player != null)
        {
            if (!player.playerNetworkManager.IsHost && hostOnlyInteract)
                return;

            if (!player.IsOwner)
                return;

            player.playerInteractionManager.RemoveInteractionFromList(this);

            PlayerUIManager.Instance.playerUIPopUpManager.CloseAllPopUpWindows();
        }
    }
}
