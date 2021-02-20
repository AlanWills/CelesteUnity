using Celeste.Tilemaps.WaveFunctionCollapse;
using CelesteEditor.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Tilemaps.WaveFunctionCollapse
{
    [CustomEditor(typeof(TilemapSolver))]
    public class TilemapEditor : Editor
    {
        #region Properties and Fields

        private TilemapSolver TilemapSolver
        {
            get { return target as TilemapSolver; }
        }

        #endregion

        #region Editor Methods

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Find Tile Descriptions", GUILayout.ExpandWidth(false)))
            {
                target.FindAssets<TileDescription>("tileDescriptions");
            }

            if (GUILayout.Button("Check Symmetric Rules", GUILayout.ExpandWidth(false)))
            {
                TilemapSolver.CheckSymmetricRules();
            }

            if (GUILayout.Button("Fix Null", GUILayout.ExpandWidth(false)))
            {
                foreach (TileDescription tileDescription in TilemapSolver.tileDescriptions)
                {
                    foreach (Rule rule in tileDescription.Rules)
                    {
                        if (rule.otherTile == null)
                        {
                            rule.otherTile = TilemapSolver.nullTile;
                            EditorUtility.SetDirty(rule);
                        }
                    }
                }

                AssetDatabase.SaveAssets();
            }

            EditorGUILayout.EndHorizontal();
            
            base.OnInspectorGUI();
        }

        #endregion
    }
}
