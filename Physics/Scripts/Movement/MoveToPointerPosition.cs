using Celeste.Input;
using Celeste.Maths;
using Celeste.Parameters;
using System;
using UnityEngine;

namespace Celeste.Physics.Movement
{
    [AddComponentMenu("Celeste/Input/Movement/Move To Pointer Position")]
    public class MoveToPointerPosition : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private InputState inputState;
        [SerializeField] private Rigidbody2D bodyToMove;
        [SerializeField] private FloatReference movementSpeed;

        [NonSerialized] private Vector3 worldPosition;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (movementSpeed == null)
            {
                movementSpeed = ScriptableObject.CreateInstance<FloatReference>();
                movementSpeed.name = "MovementSpeed";
                movementSpeed.IsConstant = true;
                movementSpeed.Value = 1;
            }
#endif
        }

        private void FixedUpdate()
        {
            float threshold = movementSpeed.Value * Time.fixedDeltaTime;

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
            bodyToMove.SetLocalForwardVelocity(movementSpeed.Value);
            enabled = true;
        }

        #endregion
    }
}
