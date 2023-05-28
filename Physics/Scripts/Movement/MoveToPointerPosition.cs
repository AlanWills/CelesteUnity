using Celeste.Input;
using Celeste.Maths;
using Celeste.Parameters;
using Celeste.Tools.Attributes.GUI;
using System;
using UnityEngine;

namespace Celeste.Physics.Movement
{
    [AddComponentMenu("Celeste/Input/Movement/Move To Pointer Position")]
    public class MoveToPointerPosition : MonoBehaviour
    {
        #region Properties and Fields

        private float MovementSpeed => movementSpeedParameterised ? movementSpeedParameter.Value : movementSpeed;

        [SerializeField] private InputState inputState;
        [SerializeField] private Rigidbody2D bodyToMove;
        [SerializeField] private bool movementSpeedParameterised;
        [SerializeField, HideIf(nameof(movementSpeedParameterised))] private float movementSpeed = 1;
        [SerializeField, ShowIf(nameof(movementSpeedParameterised))] private FloatValue movementSpeedParameter;

        [NonSerialized] private Vector3 worldPosition;

        #endregion

        #region Unity Methods

        private void FixedUpdate()
        {
            float threshold = MovementSpeed * Time.fixedDeltaTime;

            if ((bodyToMove.position - worldPosition.ToVector2()).magnitude < threshold)
            {
                bodyToMove.velocity = Vector2.zero;
                enabled = false;
            }
        }

        #endregion

        #region Callbacks

        public void OnPointerClicked()
        {
            worldPosition = inputState.PointerWorldPosition;
            bodyToMove.transform.LookAtWorldPosition2D(worldPosition);
            bodyToMove.SetLocalForwardVelocity(MovementSpeed);
            enabled = true;
        }

        #endregion
    }
}
