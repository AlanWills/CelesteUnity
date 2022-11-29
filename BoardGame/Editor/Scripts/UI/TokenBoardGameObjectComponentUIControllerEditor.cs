using Celeste.BoardGame.UI;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.BoardGame.UI
{
    [CustomEditor(typeof(TokenBoardGameObjectComponentUIController))]
    public class TokenBoardGameObjectComponentUIControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Flip"))
            {
                (target as TokenBoardGameObjectComponentUIController).Flip();
            }

            base.OnInspectorGUI();
        }
    }
}
