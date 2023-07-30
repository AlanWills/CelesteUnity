using Celeste.Events;
using Celeste.Objects;
using Celeste.Parameters;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Wallet
{
    [CreateAssetMenu(fileName = nameof(Currency), menuName = "Celeste/Wallet/Currency")]
    public class Currency : ScriptableObject, IIntGuid
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

        public int StartingQuantity
        {
            get { return startingQuantity; }
        }

        public int Quantity
        {
            get { return quantity.Value; }
            set { quantity.Value = value; }
        }

        public Sprite Icon => icon;
        public string GlyphName => $"\"{glyphName}\"";
        public bool IsPersistent => isPersistent;

        [SerializeField] private int guid;
        [SerializeField] private int startingQuantity;
        [SerializeField] private Sprite icon;
        [SerializeField] private string glyphName;
        [SerializeField] private bool isPersistent = true;

        [Header("Runtime")]
        [SerializeField] private IntValue quantity;

        #endregion

        public void AddOnQuantityChangedCallback(UnityAction<ValueChangedArgs<int>> callback)
        {
            quantity.AddValueChangedCallback(callback);
        }

        public void RemoveOnQuantityChangedCallback(UnityAction<ValueChangedArgs<int>> callback)
        {
            quantity.RemoveValueChangedCallback(callback);
        }

        public void RemoveAllQuantityChangedCallbacks()
        {
            quantity.RemoveAllValueChangedCallbacks();
        }
    }
}