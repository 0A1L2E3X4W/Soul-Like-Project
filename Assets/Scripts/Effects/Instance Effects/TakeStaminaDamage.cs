using UnityEngine;

[CreateAssetMenu(menuName = "Character Effect/Instance Effects/Take Stamina Damage")]
public class TakeStaminaDamage : InstanceCharacterEffect
{
    [Header("EFFECT SETTINGS")]
    public float staminaDamage;

    public override void ProcessEffect(CharacterManager character)
    {
        CalcuStaminaDamage(character);
    }

    private void CalcuStaminaDamage(CharacterManager character)
    {
        if (character.IsOwner)
        {
            Debug.Log("STAMINA EFFECT DAMAGE : " + staminaDamage);
            character.characterNetworkManager.currentStamina.Value -= staminaDamage;
        }
    }
}
