using Celeste.Tilemaps.WaveFunctionCollapse;
using CelesteEditor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Tilemaps.WaveFunctionCollapse
{
    [CustomEditor(typeof(TilemapSolverComponent))]
    public class TilemapSolverComponentEditor : Editor
    {
        #region Properties and Fields

        private Vector2Int tilemapBounds = new Vector2Int();
        private Vector2Int location = new Vector2Int();

        #endregion

        private void OnEnable()
        {
            TilemapSolverComponent tilemapSolver = target as TilemapSolverComponent;
            if (tilemapSolver.tilemap != null && tilemapBounds == Vector2.zero)
            {
                // Need to take into account the boundary
                tilemapBounds.x = tilemapSolver.tilemap.cellBounds.size.x - 1;
                tilemapBounds.y = tilemapSolver.tilemap.cellBounds.size.y - 1;
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            TilemapSolverComponent tilemapSolverComponent = target as TilemapSolverComponent;

            EditorGUILayout.BeginHorizontal();

            tilemapBounds = EditorGUILayout.Vector2IntField("Tilemap Bounds", tilemapBounds);
            if (GUILayout.Button("Apply", GUILayout.ExpandWidth(false)))
            {
                // Compensate for boundary
                tilemapSolverComponent.tilemap.size = new Vector3Int(tilemapBounds.x + 1, tilemapBounds.y + 1, 1);
                tilemapSolverComponent.tilemap.ResizeBounds();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            location = EditorGUILayout.Vector2IntField("Location", location);
            if (GUILayout.Button("Analyse", GUILayout.ExpandWidth(false)))
            {
                LogExtensions.Clear();
                tilemapSolverComponent.Analyse(location);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Reset", GUILayout.ExpandWidth(false)))
            {
                LogExtensions.Clear();
                tilemapSolverComponent.ResetTilemap();
                EditorUtility.SetDirty(tilemapSolverComponent);
            }

            if (GUILayout.Button("Set Up From Tilemap", GUILayout.ExpandWidth(false)))
            {
                LogExtensions.Clear();
                tilemapSolverComponent.SetUpFromTilemap();
            }

            if (GUILayout.Button("Solve", GUILayout.ExpandWidth(false)))
            {
                LogExtensions.Clear();
                tilemapSolverComponent.Solve();
                EditorUtility.SetDirty(tilemapSolverComponent);
            }

            if (GUILayout.Button("Solve Step", GUILayout.ExpandWidth(false)))
            {
                LogExtensions.Clear();
                tilemapSolverComponent.SolveStep();
                EditorUtility.SetDirty(tilemapSolverComponent);
            }

            if (GUILayout.Button("Show Collapsed", GUILayout.ExpandWidth(false)))
            {
                LogExtensions.Clear();
                tilemapSolverComponent.collapsedTilemap.ClearAllTiles();

                for (int row = 0; row < tilemapSolverComponent.tilemapSolver.Solution.Count; ++row)
                {
                    List<TilePossibilities> rowPossibilities = tilemapSolverComponent.tilemapSolver.Solution[row];

                    for (int column = 0; column < rowPossibilities.Count; ++column)
                    {
                        if (rowPossibilities[column].HasCollapsed)
                        {
                            tilemapSolverComponent.collapsedTilemap.SetTile(new Vector3Int(column, row, 0), tilemapSolverComponent.tilemapSolver.nullTilePreview);
                        }
                    }
                }
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}
