using DatingBros.PhoneSimulation.UI;
using UnityEditor;

namespace CelesteEditor.UI.Layout
{
    [CustomEditor(typeof(MaxSizeContentSizeFitter), false)]
    public class MaxSizeContentSizeFitterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // This is so dumb, but Unity does it's own drawing of ContentSizeFitters for all child classes, so I have to hard override it
            DrawDefaultInspector();
        }
    }
}