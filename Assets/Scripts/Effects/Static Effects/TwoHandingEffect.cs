using UnityEngine;

[CreateAssetMenu(menuName = "Character Effect/Static Effect/Two Handing Effect")]
public class TwoHandingEffect : StaticEffect
{
    [Header("STATS")]
    [SerializeField] int strengthGainedFormWeapon;

    public override void ProcessStaticEffect(CharacterManager character)
    {
        base.ProcessStaticEffect(character);

        if (character.IsOwner)
        {
            strengthGainedFormWeapon = Mathf.RoundToInt(character.characterNetworkManager.strength.Value / 2);
            Debug.Log(strengthGainedFormWeapon);
            character.characterNetworkManager.strengthModifer.Value += strengthGainedFormWeapon;
        }
    }

    public override void RemoveStaticEffect(CharacterManager character)
    {
        base.RemoveStaticEffect(character);

        if (character.IsOwner)
        {
            strengthGainedFormWeapon = Mathf.RoundToInt(character.characterNetworkManager.strength.Value / 2);
            character.characterNetworkManager.strengthModifer.Value -= strengthGainedFormWeapon;
        }
    }
}
