using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    public static TitleScreenManager Instance;

    [Header("SAVE SLOTS")]
    public CharacterSlot currentSelectedSlot = CharacterSlot.NullSlot;

    [Header("MENUS")]
    [SerializeField] private GameObject titleScreenMainMenu;
    [SerializeField] private GameObject titleScreenLoadMenu;

    [Header("BUTTONS")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button loadMenuReturnButton;

    [Header("NO FREE SLOT POP UP")]
    [SerializeField] private GameObject noFreeSlotsPopUp;
    [SerializeField] private Button noFreeSlotsConfirmButton;

    [Header("DELETE CHARACTER SLOT POP UP")]
    [SerializeField] private GameObject deleteCharacterSlotPopUp;
    [SerializeField] private Button deleteSlotConfirmButton;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    public void StartNetworkAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartNewGame()
    {
        WorldSaveGameManager.Instance.AttemptCreateNewGame();
    }

    // LOAD MENU
    public void OpenLoadGameMenu()
    {
        titleScreenMainMenu.SetActive(false);
        titleScreenLoadMenu.SetActive(true);

        loadMenuReturnButton.Select();
    }

    public void CloseLoadGameMenu()
    {
        titleScreenLoadMenu.SetActive(false);
        titleScreenMainMenu.SetActive(true);

        loadGameButton.Select();
    }

    // NO FREE SLOT POP UP
    public void DisplayNoFreeSlotPopUp()
    {
        noFreeSlotsPopUp.SetActive(true);
        noFreeSlotsConfirmButton.Select();
    }

    public void CloseNoFreeSlotPopUp()
    {
        noFreeSlotsPopUp.SetActive(false);
        newGameButton.Select();
    }

    // DELETE CHARACTER SLOT
    public void AttemptDeleteCharacterSlots()
    {
        if (currentSelectedSlot != CharacterSlot.NullSlot)
        {
            deleteCharacterSlotPopUp.SetActive(true);
            deleteSlotConfirmButton.Select();
        }
    }

    public void DeleteCharacterSlots()
    {
        deleteCharacterSlotPopUp.SetActive(false);
        WorldSaveGameManager.Instance.DeleteGame(currentSelectedSlot);

        titleScreenLoadMenu.SetActive(false);
        titleScreenLoadMenu.SetActive(true);

        loadMenuReturnButton.Select();
    }

    public void CloseDeleteSlotPopUp()
    {
        deleteCharacterSlotPopUp.SetActive(false);
        loadMenuReturnButton.Select();
    }

    // CHARACTER SLOT
    public void SelectCharacterSlots(CharacterSlot characterSlot)
    {
        currentSelectedSlot = characterSlot;
    }

    public void SelectNullSlot()
    {
        currentSelectedSlot = CharacterSlot.NullSlot;
    }
}
