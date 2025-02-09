using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour
{
    [Header("MANAGER")]
    private CharacterManager character;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        
    }
}
