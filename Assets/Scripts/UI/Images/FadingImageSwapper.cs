using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Images {
    [RequireComponent(typeof(EventTrigger))]
    public class FadingImageSwapper : MonoBehaviour {
        private Image _default;
        private Image _hover;
        private EventTrigger _eventTrigger;


        public void Awake() {
            var defaultImage = transform.FindChild("Default");
            if (defaultImage != null) {
                _default = defaultImage.GetComponent<Image>();
            } else {
              Debug.LogWarning("FadingImageSwapper requires a child component named \"Default\".");  
            }

            var hoverImage = transform.FindChild("Hover");
            if (hoverImage != null) {
                _hover = hoverImage.GetComponent<Image>();
            } else {
                Debug.LogWarning("FadingImageSwapper requires a child component named \"Hover\"");
            }

            _eventTrigger = GetComponent<EventTrigger>();
        }

        public void Start() {
            var hoverEntry = new EventTrigger.Entry {eventID = EventTriggerType.PointerEnter};
            hoverEntry.callback.AddListener((eventData) => { ShowHoverState(); });
            _eventTrigger.triggers.Add(hoverEntry);


            var exitEntry = new EventTrigger.Entry {eventID = EventTriggerType.PointerExit};
            exitEntry.callback.AddListener((eventData) => { ShowDefaultState(); });
            _eventTrigger.triggers.Add(exitEntry);

            _hover.DOFade(0, 0);
        }

        public void ShowHoverState() {
            _hover.DOFade(1, 0.3f);
            _default.DOFade(0, 0.3f);
        }

        public void ShowDefaultState() {
            _hover.DOFade(0, 0.3f);
            _default.DOFade(1, 0.3f);
        }
    }
}
