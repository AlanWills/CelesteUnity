using Celeste.Logic;
using Celeste.Objects;
using Celeste.Tools.Attributes.GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Narrative.Choices
{
    [Serializable]
    public enum InvalidBehaviour
    {
        Hide,
        ShowDisabled
    }

    public class Choice : ScriptableObject, IChoice, ICopyable<Choice>
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

        public InvalidBehaviour InvalidBehaviour
        {
            get { return invalidBehaviour; }
        }

        [SerializeField] private string id;
        [SerializeField] private Condition condition;
        [SerializeField, HideIfNull(nameof(condition))] private InvalidBehaviour invalidBehaviour = InvalidBehaviour.Hide;

        #endregion

        public virtual void CopyFrom(Choice original)
        {
            id = original.id;
            invalidBehaviour = original.invalidBehaviour;
        }

        public virtual bool IsValid()
        {
            return condition == null || condition.Check();
        }

        public virtual void OnSelected() { }
    }
}
