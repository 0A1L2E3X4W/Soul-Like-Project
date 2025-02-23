using UnityEngine;

public class CharacterFootStepSFXMaker : MonoBehaviour
{
    [Header("MANAGER")]
    private CharacterManager character;

    [Header("AUDIO")]
    private AudioSource audioSource;
    private GameObject steppedOnObj;

    [Header("STATUS")]
    private bool hasTouchedGround;
    private bool hasPlayedFootStepSFX;
    [SerializeField] private float distanceToGround = 0.05f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        character = GetComponentInParent<CharacterManager>();
    }

    private void FixedUpdate()
    {
        CheckFootSteps();
    }

    private void CheckFootSteps()
    {
        if (character == null)
            return;

        if (!character.characterNetworkManager.isMoving.Value)
            return;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, character.transform.TransformDirection(Vector3.down),
            out hit, distanceToGround, WorldUtilityManager.Instance.GetEnvironmentLayers()))
        {
            hasTouchedGround = true;

            if (!hasPlayedFootStepSFX)
                steppedOnObj = hit.transform.gameObject;
        }
        else
        {
            hasTouchedGround = false;
            hasPlayedFootStepSFX = false;
            steppedOnObj = null;
        }

        if (hasTouchedGround && !hasPlayedFootStepSFX)
        {
            hasPlayedFootStepSFX = true;
            PlayFootStepSFX();
        }
    }

    private void PlayFootStepSFX()
    {
        character.characterSoundFXManager.PlayFootStepSFX();
    }
}
