using Unity.Netcode;
using UnityEngine;

public class AICharacterSpawner : MonoBehaviour
{
    [Header("CHARACTER")]
    [SerializeField] private GameObject characterGameObject;
    [SerializeField] private GameObject instantiatedGameObject;

    private void Awake()
    {

    }

    private void Start()
    {
        WorldAIManager.Instance.SpawnCharacter(this);
        gameObject.SetActive(false);
    }

    public void AttemptSpawnCharacters()
    {
        if (characterGameObject != null)
        {
            instantiatedGameObject = Instantiate(characterGameObject);
            instantiatedGameObject.transform.position = transform.position;
            instantiatedGameObject.transform.rotation = transform.rotation;
            instantiatedGameObject.GetComponent<NetworkObject>().Spawn();
        }
    }
}
