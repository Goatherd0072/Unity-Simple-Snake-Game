using UnityEngine;

namespace Snake.UI
{
    public class UIPanel : MonoBehaviour
    {
        private CanvasGroup canvasGroup;

        public virtual void Init()
        {
            if (TryGetComponent<CanvasGroup>(out var cG))
            {
                canvasGroup = cG;
            }
            else
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        public virtual void Show()
        {
            canvasGroup.alpha = 1;
        }

        public virtual void Hide()
        {
            canvasGroup.alpha = 0;
        }

    }
}