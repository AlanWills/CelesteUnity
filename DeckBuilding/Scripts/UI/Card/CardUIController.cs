using UnityEngine;
using UnityEngine.EventSystems;

namespace Celeste.DeckBuilding.UI
{
    [AddComponentMenu("Celeste/Deck Building/UI/Card UI Controller")]
    public class CardUIController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region Properties and Fields

        [SerializeField] private bool interactable = true;

        [Header("UI")]
        [SerializeField] private Animator animator;
        [SerializeField] private InfoUIController infoController;
        [SerializeField] private CostUIController costController;
        [SerializeField] private ArmourUIController armourController;
        [SerializeField] private HealthUIController healthController;
        [SerializeField] private StrengthUIController strengthController;
        [SerializeField] private DamageUIController damageController;

        private CardRuntime card;
        private bool canPlay = false;
        private bool dragging = false;
        private Vector2 startDragLocalPosition;

        private static int FACE_UP_HASH = Animator.StringToHash("FaceUp");
        private static int PLAYABLE_HASH = Animator.StringToHash("Playable");

        #endregion

        public void Hookup(CardRuntime card)
        {
            this.card = card;
            this.card.OnCanPlayChanged.AddListener(OnCanPlayChanged);
            this.card.OnFaceUpChanged.AddListener(OnFaceUpChanged);
            
            infoController.Hookup(card);
            costController.Hookup(card);
            armourController.Hookup(card);
            healthController.Hookup(card);
            strengthController.Hookup(card);
            damageController.Hookup(card);
        }

        private void OnEnable()
        {
            // Because these values are driven by an animator, setting them in Hookup will do nothing
            // When the animator is enabled the parameters will be set to their default values and our changes will be ignored
            // Consequently, we initialze the values in OnEnable to ensure correct UI behaviour
            UpdateCanPlay(card.CanPlay);
            UpdateFaceUp(card.IsFaceUp);
        }

        private void OnDisable()
        {
            card.OnCanPlayChanged.RemoveListener(OnCanPlayChanged);
            card.OnFaceUpChanged.AddListener(OnFaceUpChanged);
        }

        public bool IsForCard(CardRuntime card)
        {
            return this.card == card;
        }

        #region UI

        private void UpdateCanPlay(bool newCanPlay)
        {
            canPlay = newCanPlay && interactable;
            animator.SetBool(PLAYABLE_HASH, canPlay);
        }

        private void UpdateFaceUp(bool isFaceUp)
        {
            animator.SetBool(FACE_UP_HASH, isFaceUp);
        }

        #endregion

        #region IDrag

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (canPlay)
            {
                dragging = true;
                startDragLocalPosition = transform.localPosition;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (dragging)
            {
                transform.position = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (dragging)
            {
                dragging = false;

                if (transform.localPosition.y <= 150 || !card.TryPlay())
                {
                    // Reset the card position
                    transform.localPosition = startDragLocalPosition;
                }
            }
        }

        #endregion

        #region Callbacks

        private void OnCanPlayChanged(bool newCanPlay)
        {
            UpdateCanPlay(newCanPlay);
        }

        private void OnFaceUpChanged(bool isFaceUp)
        {
            UpdateFaceUp(isFaceUp);
        }

        #endregion
    }
}