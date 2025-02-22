using Unity.Netcode;
using UnityEngine;

public class FogWallsInteractable : Interactable
{
    [Header("ID")]
    public int fogWallID;

    [Header("FOG WALLS")]
    [SerializeField] GameObject[] fogWallObjs;

    [Header("ACTIVE")]
    public NetworkVariable<bool> isActive = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

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
}
