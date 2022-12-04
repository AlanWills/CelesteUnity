using Celeste.Components.Persistence;
using Celeste.Tools.Attributes.GUI;
using System;
using System.Collections.Generic;

namespace Celeste.LiveOps.Persistence
{
    [Serializable]
    public class LiveOpDTO
    {
        #region Properties and Fields

        public bool IsValid
        {
            get
            {
                return components != null && components.Count > 0;
            }
        }

        public long type;
        public long subType;
        [Timestamp] public long startTimestamp;
        public bool isRecurring;
        public long repeatsAfter;
        [ReadOnly] public LiveOpState state;
        public List<ComponentDTO> components = new List<ComponentDTO>();

        #endregion

        public LiveOpDTO(LiveOp liveOp)
        {
            type = liveOp.Type;
            subType = liveOp.SubType;
            startTimestamp = liveOp.StartTimestamp;
            isRecurring = liveOp.IsRecurring;
            repeatsAfter = -1;
            state = liveOp.State;
            components.Capacity = liveOp.NumComponents;

            for (int i = 0, n = liveOp.NumComponents; i < n; i++)
            {
                var component = liveOp.GetComponent(i);
                components.Add(ComponentDTO.From(component));
            }
        }

        public LiveOpDTO(LiveOpTemplate template)
        {
            type = template.Type;
            subType = template.SubType;
            startTimestamp = template.StartTimestamp;
            isRecurring = template.IsRecurring;
            repeatsAfter = template.RepeatsAfter;
            state = LiveOpState.ComingSoon;
            components.Capacity = template.NumComponents;

            for (int i = 0, n = template.NumComponents; i < n; i++)
            {
                components.Add(ComponentDTO.From(template.GetComponent(i), template.GetComponentData(i)));
            }
        }
    }
}
