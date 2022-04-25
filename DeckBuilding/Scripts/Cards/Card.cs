using Celeste.DataStructures;
using Celeste.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Cards
{
    [CreateAssetMenu(fileName = nameof(Card), menuName = "Celeste/Deck Building/Cards/Card")]
    public class Card : ScriptableObject, IGuid
    {
        #region Properties and Fields

        public int Guid
        {
            get { return guid; }
            set
            {
                guid = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        public int NumComponents
        {
            get { return components.Count; }
        }

        [SerializeField] private int guid;
        [SerializeField] private List<Component> components = new List<Component>();

        #endregion

        public Component GetComponent(int index)
        {
            return components.Get(index);
        }

        public void AddComponent(Type type)
        {
            this.AddSubAsset(type, components);
        }

        public void RemoveComponent(int componentIndex)
        {
            this.RemoveSubAsset(componentIndex, components);
        }

        public bool HasComponent<T>() where T : Component
        {
            return components.Find(x => x is T);
        }
    }
}
