using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class FogWallsInteractable : Interactable
{
    [Header("[ FOG WALL ]")]
    [Header("ID")]
    public int fogWallID;

    [Header("FOG WALLS")]
    [SerializeField] GameObject[] fogWallObjs;

    [Header("COLLITIONS")]
    [SerializeField] Collider fogWallCollider;

    [Header("ACTIVE")]
    public NetworkVariable<bool> isActive = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("SFX")]
    private AudioSource fogWallAudioSource;
    [SerializeField] AudioClip fogWallSFX;

    protected override void Awake()
    {
        base.Awake();

        fogWallAudioSource = GetComponent<AudioSource>();
    }

    public override void Interact(PlayerManager player)
    {
        base.Interact(player);

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.left);
        player.transform.rotation = targetRotation;

        AllowPlayerThroughFogWallColliderServerRpc(player.NetworkObjectId);
        player.playerAnimatorManager.PlayTargetActionAnim("Pass_Through_01", true);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        OnIsActiveChanged(false, isActive.Value);
        isActive.OnValueChanged += OnIsActiveChanged;
        WorldObjectManager.Instance.AddFogWallToList(this);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        isActive.OnValueChanged -= OnIsActiveChanged;
        WorldObjectManager.Instance.RemoveFogWallFromList(this);
    }

    public virtual void OnIsActiveChanged(bool oldStatus, bool newStatus)
    {
        if (isActive.Value)
        {
            foreach (var obj in fogWallObjs)
            {
                obj.SetActive(true);
            }
        }
        else
        {
            foreach (var obj in fogWallObjs)
            {
                obj.SetActive(false);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void AllowPlayerThroughFogWallColliderServerRpc(ulong playerObjID)
    {
        if (IsServer)
        {
            AllowPlayerThroughFogWallColliderClientRpc(playerObjID);
        }
    }

    [ClientRpc]
    private void AllowPlayerThroughFogWallColliderClientRpc(ulong playerObjID)
    {
        PlayerManager player = NetworkManager.Singleton.SpawnManager.SpawnedObjects[playerObjID].GetComponent<PlayerManager>();

        //fogWallAudioSource.PlayOneShot(fogWallSFX);

        if (player != null)
            StartCoroutine(DisableColliderForTime(player));
    }

    private IEnumerator DisableColliderForTime(PlayerManager player)
    {
        Physics.IgnoreCollision(player.characterController, fogWallCollider, true);

        yield return new WaitForSeconds(3);
        Physics.IgnoreCollision(player.characterController, fogWallCollider, false);
    }
}
