using Celeste.Rewards.Catalogue;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Rewards.Catalogue
{
    [CustomEditor(typeof(Reward))]
    public class RewardEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Initialize"))
            {
                (target as Reward).Initialize();
            }

            base.OnInspectorGUI();
        }
    }
}
