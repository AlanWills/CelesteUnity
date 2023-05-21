using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CelesteEditor.Tilemaps
{
    [CustomEditor(typeof(Tilemap))]
    public class TilemapEditor : Editor
    {
        #region Properties and Fields

        public Tilemap Tilemap
        {
            get { return target as Tilemap; }
        }

        private TileBase originalTile;
        private TileBase newTile;
        private Tilemap copyFrom;
        private TileBase applyTile;

        #endregion

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Clear", GUILayout.ExpandWidth(false)))
            {
                Tilemap.ClearAllTiles();
            }

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Replace", GUILayout.ExpandWidth(false)))
            {
                BoundsInt tilemapDims = Tilemap.cellBounds;

                for (int y = tilemapDims.yMin; y < tilemapDims.yMax; ++y)
                {
                    for (int x = tilemapDims.xMin; x < tilemapDims.xMax; ++x)
                    {
                        if (Tilemap.GetTile(new Vector3Int(x, y, 0)) == originalTile)
                        {
                            Tilemap.SetTile(new Vector3Int(x, y, 0), newTile);
                        }
                    }
                }
            }

            originalTile = EditorGUILayout.ObjectField(originalTile, typeof(TileBase), false) as TileBase;
            newTile = EditorGUILayout.ObjectField(newTile, typeof(TileBase), false) as TileBase;

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Apply", GUILayout.ExpandWidth(false)))
            {
                BoundsInt tilemapDims = copyFrom.cellBounds;

                for (int y = tilemapDims.yMin; y < tilemapDims.yMax; ++y)
                {
                    for (int x = tilemapDims.xMin; x < tilemapDims.xMax; ++x)
                    {
                        if (copyFrom.HasTile(new Vector3Int(x, y, 0)))
                        {
                            Tilemap.SetTile(new Vector3Int(x, y, 0), applyTile);
                        }
                    }
                }
            }

            copyFrom = EditorGUILayout.ObjectField(copyFrom, typeof(Tilemap), true) as Tilemap;
            applyTile = EditorGUILayout.ObjectField(applyTile, typeof(TileBase), false) as TileBase;

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Copy From", GUILayout.ExpandWidth(false)))
            {
                BoundsInt tilemapDims = copyFrom.cellBounds;

                for (int y = tilemapDims.yMin; y < tilemapDims.yMax; ++y)
                {
                    for (int x = tilemapDims.xMin; x < tilemapDims.xMax; ++x)
                    {
                        if (copyFrom.HasTile(new Vector3Int(x, y, 0)))
                        {
                            Tilemap.SetTile(new Vector3Int(x, y, 0), copyFrom.GetTile(new Vector3Int(x, y, 0)));
                        }
                    }
                }
            }

            copyFrom = EditorGUILayout.ObjectField(copyFrom, typeof(Tilemap), true) as Tilemap;

            EditorGUILayout.EndHorizontal();

            base.OnInspectorGUI();
        }
    }
}
