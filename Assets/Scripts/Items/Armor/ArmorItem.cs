using UnityEngine;

public class ArmorItem : EquipmentItem
{
    [Header("EQUIP ABSORPTION")]
    public float physicalDamageAbsorption;
    public float magicalDamageAbsorption;
    public float fireDamageAbsorption;
    public float holyDamageAbsorption;
    public float lightningDamageAbsorption;

    [Header("EQUIP RESISTANCE")]
    public float immunity;  // ROT & POISON
    public float robustness;// BLEED & FROST
    public float focus;     // MADNESS & SLEEP
    public float vitality;  // DEATH CURSE

    [Header("POISE")]
    public float poise;

    public EquipmentModel[] equipmentModels;
}
