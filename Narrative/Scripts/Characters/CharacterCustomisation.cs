using UnityEditor;
using UnityEngine;

namespace Celeste.Narrative.Characters
{
    public abstract class CharacterCustomisation : ScriptableObject
    {
        #region Properties and Fields

        public int Guid
        {
            get { return guid; }
        }

        [SerializeField] private int guid;

        #endregion
    }
}