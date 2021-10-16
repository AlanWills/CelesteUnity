using Celeste.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace CelesteEditor.Objects
{
    public class GuidValidator
    {
        #region Properties and Fields

        public string ErrorString { get; private set; }

        private SerializedProperty itemsProperty;
        private Dictionary<int, IGuid> guids = new Dictionary<int, IGuid>();

        #endregion

        public GuidValidator(SerializedProperty itemsProperty)
        {
            this.itemsProperty = itemsProperty;
        }

        public void Validate()
        {
            guids.Clear();

            for (int i = 0, n = itemsProperty.arraySize; i < n; ++i)
            {
                IGuid item = itemsProperty.GetArrayElementAtIndex(i).objectReferenceValue as IGuid;
                if (item == null)
                {
                    continue;
                }

                if (guids.ContainsKey(item.Guid))
                {
                    ErrorString = $"Found duplicate Guid {item.Guid} on {item.name} and {guids[item.Guid].name}.";
                    return;
                }

                guids.Add(item.Guid, item);
            }

            ErrorString = "All guids unique.";
        }
    }
}
