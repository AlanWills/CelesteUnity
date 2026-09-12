using Celeste.Parameters;
using System;
using UnityEngine;

namespace Celeste.FSM.Nodes.Viewport
{
    public enum LookAxis
    {
        X,
        Y,
        Z
    }

    [Serializable]
    [CreateNodeMenu("Celeste/Viewport/Look At")]
    [NodeWidth(250)]
    public class LookAtNode : FSMNode
    {
        #region Properties and Fields

        public Vector3Reference targetPosition;
        [SerializeField] private CameraReference camera;
        public LookAxis lookAxis;
        public float time = 0;


        private float currentTime = 0;
        private Vector3 currentStartingPosition;
        private Vector3 currentTargetPosition;

        #endregion

        #region Add/Remove/Copy

        protected override void OnAddToGraph()
        {
            base.OnAddToGraph();

            if (targetPosition == null)
            {
                targetPosition = CreateParameter<Vector3Reference>($"{name}_targetPosition");
            }

            if (camera == null)
            {
                camera = CreateParameter<CameraReference>($"{name}_camera");
            }
        }

        protected override void OnRemoveFromGraph()
        {
            base.OnRemoveFromGraph();

            RemoveParameter(targetPosition);
            RemoveParameter(camera);
        }

        protected override void OnCopyInGraph(FSMNode original)
        {
            base.OnCopyInGraph(original);

            LookAtNode lookAtNode = original as LookAtNode;
            targetPosition = CreateParameter(lookAtNode.targetPosition);
            camera = CreateParameter(lookAtNode.camera);
        }

        #endregion

        #region FSM Runtime

        protected override void OnEnter()
        {
            base.OnEnter();

            currentTime = 0;
            currentStartingPosition = camera.Value.transform.position;

            if (lookAxis == LookAxis.X)
            {
                currentTargetPosition = targetPosition.Value;
                currentTargetPosition.x = currentStartingPosition.x;
            }
            else if (lookAxis == LookAxis.Y)
            {
                currentTargetPosition = targetPosition.Value;
                currentTargetPosition.y = currentStartingPosition.y;
            }
            else if (lookAxis == LookAxis.Z)
            {
                currentTargetPosition = targetPosition.Value;
                currentTargetPosition.z = currentStartingPosition.z;
            }
        }

        protected override FSMNode OnUpdate()
        {
            currentTime = Math.Min(time, currentTime + Time.deltaTime);
            camera.Value.transform.position = Vector3.Lerp(currentStartingPosition, currentTargetPosition, time != 0 ? currentTime / time : 1);

            return currentTime < time ? this : base.OnUpdate();
        }

        #endregion
    }
}
