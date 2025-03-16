using UnityEngine;

[CreateAssetMenu(menuName = "Equipment Model")]
public class EquipmentModel : ScriptableObject
{
    [Header("EQUIPMENT TYPE")]
    public EquipmentModelType equipmentModelType;

    [Header("EQUIPMENT NAME")]
    public string maleEquipmentName;
    public string femaleEquipmentName;

    public void LoadModel(PlayerManager player, bool isMale)
    {
        if (isMale)
        {
            LoadMaleModel(player);
        }
        else
        {
            LoadFemaleModel(player);
        }
    }

    private void LoadMaleModel(PlayerManager player)
    {
        switch (equipmentModelType)
        {
            case EquipmentModelType.FullHelmet:

                foreach (var model in player.playerEquipmentManager.maleFullHelmets)
                {
                    if (model.gameObject.name == maleEquipmentName)
                    {
                        model.gameObject.SetActive(true);
                    }
                }
                break;

            case EquipmentModelType.OpenHelmet:
                break;
            case EquipmentModelType.Hood:
                break;
            case EquipmentModelType.HelmetAcessorie:
                break;
            case EquipmentModelType.FaceCover:
                break;
            case EquipmentModelType.Torso:
                break;
            case EquipmentModelType.Back:
                break;
            case EquipmentModelType.RightShoulder:
                break;
            case EquipmentModelType.RightElbow:
                break;
            case EquipmentModelType.RightUpperArm:
                break;
            case EquipmentModelType.RightLowArm:
                break;
            case EquipmentModelType.RightHand:
                break;
            case EquipmentModelType.LeftShoulder:
                break;
            case EquipmentModelType.LeftElbow:
                break;
            case EquipmentModelType.LeftUpperArm:
                break;
            case EquipmentModelType.LeftLowArm:
                break;
            case EquipmentModelType.LeftHand:
                break;
            case EquipmentModelType.Hips:
                break;
            case EquipmentModelType.HipsAttachment:
                break;
            case EquipmentModelType.RightLeg:
                break;
            case EquipmentModelType.RightKnee:
                break;
            case EquipmentModelType.LeftLeg:
                break;
            case EquipmentModelType.LeftKnee:
                break;
            default:
                break;
        }
    }

    private void LoadFemaleModel(PlayerManager player)
    {

    }
}
