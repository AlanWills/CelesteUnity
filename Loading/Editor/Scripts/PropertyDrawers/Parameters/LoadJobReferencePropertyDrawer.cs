using Celeste.Loading.Parameters;
using CelesteEditor.PropertyDrawers.Parameters;
using UnityEditor;

namespace CelesteEditor.Loading.PropertyDrawers.Parameters
{
    [CustomPropertyDrawer(typeof(LoadJobReference))]
    public class LoadJobReferencePropertyDrawer : ParameterReferencePropertyDrawer
    {
    }
}
