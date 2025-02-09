using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [Header("COMPONENTS")]
    protected Slider slider;
    protected RectTransform rectTransform;

    //[Header("BAR OPTIONS")]
    //[SerializeField] protected bool scaleBarLengthWithStats = false;
    //[SerializeField] protected float widthScaleMultiplier = 1f;

    protected virtual void Awake()
    {
        slider = GetComponent<Slider>();
        rectTransform = GetComponent<RectTransform>();
    }

    protected virtual void Start()
    {

    }

    public virtual void SetStat(int newVal)
    {
        slider.value = newVal;
    }

    public virtual void SetMaxStat(int maxVal)
    {
        slider.maxValue = maxVal;
        slider.value = maxVal;

        //if (scaleBarLengthWithStats)
        //{
        //    rectTransform.sizeDelta = new(maxVal * widthScaleMultiplier, rectTransform.sizeDelta.y);

        //    PlayerUIManager.Instance.playerUIHudManager.RefreshHUD();
        //}
    }
}
