using Celeste.Events;
using Celeste.Objects;
using Celeste.Parameters;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Wallet
{
    [CreateAssetMenu(fileName = nameof(Currency), menuName = "Celeste/Wallet/Currency")]
    public class Currency : ScriptableObject, IGuid
    {
        #region Properties and Fields

        public int Guid
        {
            get { return guid; }
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

        [SerializeField] private int guid;
        [SerializeField] private int startingQuantity;

        [Header("Runtime")]
        [SerializeField] private IntValue quantity;

        #endregion

        public void AddOnQuantityChangedCallback(Action<int> callback)
        {
            quantity.AddOnValueChangedCallback(callback);
        }

        public void RemoveOnQuantityChangedCallback(Action<int> callback)
        {
            quantity.RemoveOnValueChangedCallback(callback);
        }
    }
}