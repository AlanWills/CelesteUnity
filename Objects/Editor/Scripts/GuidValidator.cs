using Celeste.Objects;
using System.Collections.Generic;
using UnityEditor;

namespace CelesteEditor.Objects
{
    public class GuidValidator
    {
        #region Properties and Fields

        public string ErrorString { get; private set; }

        private SerializedProperty itemsProperty;
        private Dictionary<int, IIntGuid> guids = new Dictionary<int, IIntGuid>();

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
                IIntGuid item = itemsProperty.GetArrayElementAtIndex(i).objectReferenceValue as IIntGuid;
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
