using Celeste.DataStructures;
using Celeste.Logic;
using Celeste.Objects;
using Celeste.Tools.Attributes.GUI;
using System;
using UnityEngine;

namespace Celeste.Narrative.Choices
{
    [Serializable]
    public enum InvalidBehaviour
    {
        Hide,
        ShowDisabled
    }

    public abstract class Choice : ScriptableObject, IChoice, ICopyable<Choice>
    {
        #region Properties and Fields

        public string ID
        {
            get { return id; }
            set
            {
                id = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        public Condition[] Conditions
        {
            set { ArrayExtensions.ResizeAndCopyFrom(ref conditions, value); }
        }

        public InvalidBehaviour InvalidBehaviour
        {
            get { return invalidBehaviour; }
        }

        [SerializeField] private string id;
        [SerializeField] private Condition[] conditions;
        [SerializeField, HideIfNullOrEmpty(nameof(conditions))] private InvalidBehaviour invalidBehaviour = InvalidBehaviour.Hide;

        #endregion

        public virtual void CopyFrom(Choice original)
        {
            id = original.id;
            invalidBehaviour = original.invalidBehaviour;
            Conditions = original.conditions;
        }

        public virtual bool IsValid()
        {
            for (int i = 0, n = conditions != null ? conditions.Length : 0; i < n; ++i)
            {
                if (!conditions[i].IsMet)
                {
                    return false;
                }
            }

            return true;
        }

        public virtual void OnSelected() { }
    }
}
