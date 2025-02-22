using Unity.Netcode;
using UnityEngine;

public class NetworkObjectSpawner : MonoBehaviour
{
    [Header("OBJECTS")]
    [SerializeField] private GameObject networkGameObj;
    [SerializeField] private GameObject instantiatedGameObject;

    private void Awake()
    {

    }

    private void Start()
    {
        WorldObjectManager.Instance.SpawnObject(this);
        gameObject.SetActive(false);
    }

    public void AttemptSpawnCharacters()
    {
        if (networkGameObj != null)
        {
            instantiatedGameObject = Instantiate(networkGameObj);
            instantiatedGameObject.transform.position = transform.position;
            instantiatedGameObject.transform.rotation = transform.rotation;
            instantiatedGameObject.GetComponent<NetworkObject>().Spawn();
        }
    }
}
