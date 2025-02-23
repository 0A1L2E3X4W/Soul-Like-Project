using System.Collections.Generic;
using UnityEngine;

public class StompDamageCollider : DamageCollider
{
    [Header("MANAGER")]
    [SerializeField] BossGiantManager giantCharacter;

    protected override void Awake()
    {
        base.Awake();

        giantCharacter = GetComponentInParent<BossGiantManager>();
    }

    public void StompAttack()
    {
        GameObject stompVFX = Instantiate(giantCharacter.giantCombatManager.stompImpactVFX, transform);

        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            giantCharacter.giantCombatManager.stompAttackAOERadius,
            WorldUtilityManager.Instance.GetCharacterLayers());

        List<CharacterManager> charactersDamaged = new();

        foreach (var collider in colliders)
        {
            CharacterManager character = collider.GetComponentInParent<CharacterManager>();

            if (character != null)
            {
                if (charactersDamaged.Contains(character))
                    continue;

                if (character == giantCharacter)
                    continue;

                charactersDamaged.Add(character);

                if (character.IsOwner)
                {
                    TakeDamage damageEffect = Instantiate(WorldEffectsManager.Instance.takeDamageEffect);
                    damageEffect.physicalDamage = giantCharacter.giantCombatManager.stompDamage;
                    damageEffect.poiseDamage = giantCharacter.giantCombatManager.stompDamage;

                    character.characterEffectsManager.ProcessInstanceEffect(damageEffect);
                }
            }
        }
    }
}
