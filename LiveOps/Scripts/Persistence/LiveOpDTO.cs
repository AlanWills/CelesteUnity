using Celeste.Components.Persistence;
using Celeste.Tools.Attributes.GUI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.LiveOps.Persistence
{
    [Serializable]
    public class LiveOpDTO
    {
        public long type;
        public long subType;
        [Timestamp] public long startTimestamp;
        public bool isRecurring;
        public long repeatsAfter;
        [HideInInspector] public int state;
        public List<ComponentDTO> components;

        public LiveOpDTO(LiveOp liveOp)
        {
            type = liveOp.Type;
            subType = liveOp.SubType;
            startTimestamp = liveOp.StartTimestamp;
            isRecurring = false;
            repeatsAfter = -1;
            state = (int)liveOp.State;

            components = new List<ComponentDTO>(liveOp.NumComponents);

            for (int i = 0, n = liveOp.NumComponents; i < n; i++)
            {
                var component = liveOp.GetComponent(i);
                components.Add(new ComponentDTO(component));
            }
        }

        public LiveOpDTO(LiveOpTemplate template)
        {
            type = template.Type;
            subType = template.SubType;
            startTimestamp = template.StartTimestamp;
            isRecurring = template.IsRecurring;
            repeatsAfter = template.RepeatsAfter;
            state = (int)LiveOpState.Unknown;
            components = new List<ComponentDTO>(template.NumComponents);

            for (int i = 0, n = template.NumComponents; i < n; i++)
            {
                components.Add(new ComponentDTO(template.GetComponent(i)));
            }
        }
    }
}
