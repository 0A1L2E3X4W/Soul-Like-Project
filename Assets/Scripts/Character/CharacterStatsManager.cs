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

    [Header("BLOCKING ABSORPTIONS")]
    public float blockingPhysicalAbsorption;
    public float blockingMagicAbsorption;
    public float blockingFireAbsorption;
    public float blockingLightningAbsorption;
    public float blockingHolyAbsorption;
    public float blockingStability = 0;

    [Header("POISE")]
    public float totalPoiseDamage = 0;          //  HOW MUCH POISE DAMAGE WE HAVE TAKEN
    public float offensivePoiseBonus = 0;       //  THE POISE BONUS GAINED FROM USING WEAPONS (HEAVY WEAPONS HAVE A MUCH LARGER BONUS)
    public float basePoiseDefense = 0;          //  THE POISE BONUS GAINED FROM ARMOR/TALISMANS ETC, ETC...
    public float defaultPoiseResetTime = 8;     //  THE TIME IT TAKES FOR POISE DAMAGE TO RESET (MUST NOT BE HIT IN THE TIME OR IT WILL RESET)
    public float poiseResetTimer = 0;           //  THE CURRENT TIMER FOR POISE RESET

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandlePoiseResetTimer();
    }

    // STATS
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

    // REGENERATE STAMINA
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

    // POISE
    protected virtual void HandlePoiseResetTimer()
    {
        if (poiseResetTimer > 0)
        {
            poiseResetTimer -= Time.deltaTime;
        }
        else
        {
            totalPoiseDamage = 0;
        }
    }
}
