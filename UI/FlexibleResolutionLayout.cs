using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.UI
{
    [AddComponentMenu("Celeste/UI/Flexible Resolution Layout")]
    [ExecuteInEditMode]
    public class FlexibleResolutionLayout : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private int constraintCount = 1;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            if (gridLayoutGroup == null)
            {
                gridLayoutGroup = GetComponent<GridLayoutGroup>();
            }
        }

        private void Awake()
        {
            EnforceConstraint();
        }

        private void OnEnable()
        {
            EnforceConstraint();
        }

        #endregion

        private void EnforceConstraint()
        {
            gridLayoutGroup.constraint = Screen.height > Screen.width ?
                GridLayoutGroup.Constraint.FixedColumnCount : GridLayoutGroup.Constraint.FixedRowCount;
            gridLayoutGroup.constraintCount = constraintCount;
        }
    }
}
