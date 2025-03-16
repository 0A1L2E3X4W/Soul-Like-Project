using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldItemDatabase : MonoBehaviour
{
    public static WorldItemDatabase Instance;

    public WeaponItem unarmedWeapon;

    [Header("WEAPONS")]
    [SerializeField] private List<WeaponItem> weapons = new();

    [Header("HEAD EQUIPMENTS")]
    [SerializeField] private List<HeadEquipmentItem> headEquips = new();

    [Header("BODY EQUIPMENTS")]
    [SerializeField] private List<BodyEquipmentItem> bodyEquips = new();

    [Header("LEGS EQUIPMENTS")]
    [SerializeField] private List<LegsEquipmentItem> legsEquips = new();

    [Header("HAND EQUIPMENTS")]
    [SerializeField] private List<HandEquipmentItem> handEquips = new();

    [Header("ITEMS")]
    private List<Item> items = new();

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

        foreach (var weapon in weapons)
        {
            items.Add(weapon);
        }

        // HEAD
        foreach (var item in headEquips)
        {
            items.Add(item);
        }

        // BODY
        foreach (var item in bodyEquips)
        {
            items.Add(item);
        }

        // LEG
        foreach (var item in legsEquips)
        {
            items.Add(item);
        }

        // HAND
        foreach (var item in handEquips)
        {
            items.Add(item);
        }

        for (int i = 0; i < items.Count; i++)
        {
            items[i].itemID = i;
        }
    }

    public WeaponItem GetWeaponByID(int ID)
    {
        return weapons.FirstOrDefault(weapon => weapon.itemID == ID);
    }

    public HeadEquipmentItem GetHeadEquipmentByID(int ID)
    {
        return headEquips.FirstOrDefault(equipment => equipment.itemID == ID);
    }

    public LegsEquipmentItem GetLegsEquipmentByID(int ID)
    {
        return legsEquips.FirstOrDefault(equipment => equipment.itemID == ID);
    }

    public HandEquipmentItem GetHandEquipmentByID(int ID)
    {
        return handEquips.FirstOrDefault(equipment => equipment.itemID == ID);
    }

    public BodyEquipmentItem GetBodyEquipmentByID(int ID)
    {
        return bodyEquips.FirstOrDefault(equipment => equipment.itemID == ID);
    }
}
