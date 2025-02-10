using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    [Header("MANAGER")]
    private CharacterManager character;

    [Header("STAMINA REGENRERATION")]
    [SerializeField] float staminaRegenerationDelay = 0.8f;
    [SerializeField] int staminaRegenerationAmount = 2;
    private float staminaRegenerationTimer = 0f;
    private float staminaTickTimer = 0f;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Start()
    {

    }

    public int CalcuHealthBasedOnVitalityLV(int vitality)
    {
        float health = 0f;

        health = vitality * 15f;

        return Mathf.RoundToInt(health);
    }

    public int CalcuStaminaBasedOnEnduranceLV(int endurance)
    {
        float stamina = 0f;

        stamina = endurance * 10f;

        return Mathf.RoundToInt(stamina);
    }

    public virtual void RegenerateStamina()
    {
        if (!character.IsOwner)
            return;

        if (character.characterNetworkManager.isSprinting.Value)
            return;

        if (character.isPerformingAction)
            return;

        staminaRegenerationTimer += Time.deltaTime;

        if (staminaRegenerationTimer >= staminaRegenerationDelay)
        {
            if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
            {
                staminaTickTimer += Time.deltaTime;

                if (staminaTickTimer >= 0.1f)
                {
                    staminaTickTimer = 0f;
                    character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                }
            }
        }
    }

    public virtual void ResetStaminaRegenerationTimer(float previousStaminaVal, float currentStaminaVal)
    {
        if (currentStaminaVal < previousStaminaVal)
        {
            staminaRegenerationTimer = 0;
        }
    }
}
