using UnityEngine;

public class Item : ScriptableObject
{
    [Header("Item Info")]
    public int itemID;
    public string itemName;
    public Sprite itemIcon;
    [TextArea] public string itemText;
}
