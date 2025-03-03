using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    [Header("MANAGER")]
    private PlayerManager player;

    [Header("INTERACTABLE OBJ")]
    private List<Interactable> currentInteractActions;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    private void Start()
    {
        currentInteractActions = new();
    }

    private void FixedUpdate()
    {
        if (!player.IsOwner)
            return;

        if (!PlayerUIManager.Instance.menuWindowIsOpen && !PlayerUIManager.Instance.popUpWindowIsOpen)
            CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        if (currentInteractActions.Count == 0)
            return;

        if (currentInteractActions[0] == null)
        {
            currentInteractActions.RemoveAt(0);
            return;
        }

        if (currentInteractActions[0] != null)
            PlayerUIManager.Instance.playerUIPopUpManager.SendPlayerMessagePopUp(currentInteractActions[0].interactableText);
    }

    private void RefreshInteractionList()
    {
        for (int i = currentInteractActions.Count - 1; i > -1; i--)
        {
            if (currentInteractActions[i] == null)
                currentInteractActions.RemoveAt(i);
        }
    }

    public void AddInteractionToList(Interactable interableObj)
    {
        RefreshInteractionList();

        if (!currentInteractActions.Contains(interableObj))
            currentInteractActions.Add(interableObj);
    }

    public void RemoveInteractionFromList(Interactable interableObj)
    {
        if (currentInteractActions.Contains(interableObj))
            currentInteractActions.Remove(interableObj);

        RefreshInteractionList();
    }

    public void Interact()
    {
        if (currentInteractActions.Count <= 0)
            return;

        if (currentInteractActions[0] != null)
        {
            currentInteractActions[0].Interact(player);
            RefreshInteractionList();
        }
    }
}
