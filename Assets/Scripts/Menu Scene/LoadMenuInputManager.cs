using UnityEngine;

public class LoadMenuInputManager : MonoBehaviour
{
    [Header("INPUT SYSTEM")]
    private PlayerControls playerControls;

    [Header("TITLE SCREEN INPUTS")]
    [SerializeField] private bool deleteCharacterSlot = false;

    private void Update()
    {
        if (deleteCharacterSlot)
        {
            deleteCharacterSlot = false;
            TitleScreenManager.Instance.AttemptDeleteCharacterSlots();
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new();
            playerControls.UI.X.performed += i => deleteCharacterSlot = true;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
