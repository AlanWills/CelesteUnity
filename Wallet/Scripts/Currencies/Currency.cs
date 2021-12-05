using Celeste.Events;
using Celeste.Objects;
using System.Collections;
using UnityEngine;

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

        public IntEvent OnQuantityChanged
        {
            get { return onQuantityChanged; }
        }

        [SerializeField] private int guid;
        [SerializeField] private int startingQuantity;

        [Header("Events")]
        [SerializeField] private IntEvent onQuantityChanged;

        #endregion
    }
}