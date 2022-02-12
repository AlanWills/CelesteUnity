using Celeste.Localisation.Parameters;
using Celeste.Parameters;
using CelesteEditor.PropertyDrawers.Parameters;
using UnityEditor;

namespace CelesteEditor.Localisation.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(LocalisationKeyCategoryReference))]
    public class LocalisationKeyCategoryReferencePropertyDrawer : ParameterReferencePropertyDrawer
    {
    }
}
