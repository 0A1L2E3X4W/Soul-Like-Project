using System.Linq;
using UnityEngine;

public class WorldActionManager : MonoBehaviour
{
    public static WorldActionManager Instance;

    [Header("WEAPON ITEM ACTIONS")]
    public WeaponItemAction[] weaponItemAction;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < weaponItemAction.Length; i++)
        {
            weaponItemAction[i].actionID = i;
        }
    }

    public WeaponItemAction GetWeaponItemAction(int ID)
    {
        return weaponItemAction.FirstOrDefault(action => action.actionID == ID);
    }
}
