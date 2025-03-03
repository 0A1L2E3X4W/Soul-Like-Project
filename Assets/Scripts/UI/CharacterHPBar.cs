using TMPro;
using UnityEngine;

public class CharacterHPBar : StatusBar
{
    [Header("MANAGERS")]
    private CharacterManager character;
    private AIManager aiCharacter;
    private PlayerManager playerCharacter;

    [Header("HP BAR SETTINGS")]
    [SerializeField] bool displayNameOnHpBar = false;
    [SerializeField] float timeBeforeHide = 3f;
    [SerializeField] float hideTimer = 0f;
    [SerializeField] int currentDamageTaken = 0;
    [HideInInspector] public int oldHpVal = 0;

    [Header("BAR STATS")]
    [SerializeField] TextMeshProUGUI characterName;
    [SerializeField] TextMeshProUGUI characterDamage;

    protected override void Awake()
    {
        base.Awake();

        character = GetComponentInParent<CharacterManager>();

        if (character != null)
        {
            aiCharacter = character as AIManager;
            playerCharacter = character as PlayerManager;
        }
    }

    protected override void Start()
    {
        base.Start();

        gameObject.SetActive(false);
    }

    public override void SetStat(int newVal)
    {
        if (displayNameOnHpBar)
        {
            characterName.enabled = true;

            if (aiCharacter != null)
                characterName.text = aiCharacter.characterName;

            if (playerCharacter != null)
                characterName.text = playerCharacter.playerNetworkManager.characterName.Value.ToString();
        }

        slider.maxValue = character.characterNetworkManager.maxHealth.Value;

        currentDamageTaken = Mathf.RoundToInt(currentDamageTaken + (oldHpVal - newVal));

        if (currentDamageTaken < 0)
        {
            currentDamageTaken = Mathf.Abs(currentDamageTaken);
            characterDamage.text = "+ " + currentDamageTaken.ToString();
        }
        else
        {
            characterDamage.text = "- " + currentDamageTaken.ToString();
        }

        slider.value = newVal;

        if (character.characterNetworkManager.currentHealth.Value != character.characterNetworkManager.maxHealth.Value)
        {
            hideTimer = timeBeforeHide;
            gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);

        if (hideTimer > 0)
        {
            hideTimer -= Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        currentDamageTaken = 0;
    }
}
