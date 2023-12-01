using UnityEngine;
using UnityEngine.UI;

namespace _Core._Scripts.UI
{
    public class ClosePopup : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _button;

        private void Awake()
        {
            if(_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
            if(_button == null) _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(HidePopup);
        }
        
        private void OnDisable()
        {
            _button.onClick.RemoveListener(HidePopup);
        }

        private void HidePopup()
        {
            if(_canvasGroup == null) return;
            
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}
