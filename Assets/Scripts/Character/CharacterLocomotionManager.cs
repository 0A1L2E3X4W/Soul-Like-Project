using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour
{
    [Header("MANAGER")]
    private CharacterManager character;

    [Header("GROUND CHECK & JUMPING")]
    [SerializeField] private float groundCheckSphereRadius = 0.2f;
    [SerializeField] protected float groundYVelocity = -20f;
    [SerializeField] protected float fallStartYVelocity = -5f;
    [SerializeField] protected float gravityForce = -40f;
    [SerializeField] protected Vector3 yVelocity;
    [SerializeField] private LayerMask groundLayer;

    protected bool fallingVelocityHasBeenSet = false;
    protected float floatTimer = 0f;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleGroundCheck();

        if (character.isGrounded)
        {
            if (yVelocity.y < 0f)
            {
                floatTimer = 0f;
                fallingVelocityHasBeenSet = false;
                yVelocity.y = groundYVelocity;
            }
        }
        else
        {
            if (!character.characterNetworkManager.isJumping.Value && !fallingVelocityHasBeenSet)
            {
                fallingVelocityHasBeenSet = true;
                yVelocity.y = fallStartYVelocity;
            }

            floatTimer += Time.deltaTime;
            character.anim.SetFloat("FloatTimer", floatTimer);
            yVelocity.y += gravityForce * Time.deltaTime;
        }

        character.characterController.Move(yVelocity * Time.deltaTime);
    }

    protected void HandleGroundCheck()
    {
        character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
    }

    protected void OnDrawGizmosSelected()
    {
        if (character == null)
            return;

        Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
    }
}
