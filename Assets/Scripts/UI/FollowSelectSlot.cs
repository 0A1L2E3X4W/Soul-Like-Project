using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ALEX
{
    public class FollowSelectSlot : MonoBehaviour
    {
        [Header("CONTROL COMPONENTS")]
        [SerializeField] private GameObject currentSelected;
        [SerializeField] private GameObject previousSelected;
        [SerializeField] private RectTransform currentSelectedTransform;

        [SerializeField] private RectTransform contentPanel;
        [SerializeField] private ScrollRect scrollRect;

        private void Update()
        {
            currentSelected = EventSystem.current.currentSelectedGameObject;

            if (currentSelected != null)
            {
                if (currentSelected != previousSelected)
                {
                    previousSelected = currentSelected;
                    currentSelectedTransform = currentSelected.GetComponent<RectTransform>();
                    SnapTo(currentSelectedTransform);
                }
            }
        }

        private void SnapTo(RectTransform target)
        {
            Canvas.ForceUpdateCanvases();

            Vector2 newPos = (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position)
                - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);

            newPos.x = 0f;

            contentPanel.anchoredPosition = newPos;
        }
    }
}
