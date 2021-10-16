using Celeste.Tilemaps.WaveFunctionCollapse;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Celeste.Tilemaps.WaveFunctionCollapse
{
    [AddComponentMenu("Celeste/Tilemaps/Wave Function Collapse/Tilemap Solver Component")]
    public class TilemapSolverComponent : MonoBehaviour
    {
        #region Properties and Fields

        public Tilemap tilemap;
        public Tilemap collapsedTilemap;
        public TilemapSolver tilemapSolver;
        public uint maxRetryCount = 3;

        private Coroutine solveCoroutine;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            if (tilemap == null)
            {
                tilemap = GetComponent<Tilemap>();

#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        #endregion

        public void ResetTilemap()
        {
            tilemapSolver.Reset(tilemap);
        }

        public void SetUpFromTilemap()
        {
            tilemapSolver.SetUpFrom(tilemap);
        }

        public void Analyse(Vector2Int location)
        {
            TilePossibilities tilePossibilities = new TilePossibilities(location.x, location.y, tilemapSolver.tileDescriptions);
            tilemapSolver.UpdateFromNeighbours(location.x, location.y, tilePossibilities);

            if (tilePossibilities.HasPossibilities)
            {
                foreach (TileDescription tileDescription in tilePossibilities.possibleTiles)
                {
                    Debug.LogFormat("Possibility: {0}", tileDescription.name);
                }
            }
            else
            {
                Debug.LogError("No possibilities remaining after analysis");
            }
        }

        public void Solve()
        {
            uint currentRetryCount = 0;

            while (currentRetryCount < maxRetryCount)
            {
                if (tilemapSolver.Solve(tilemap) && tilemapSolver.IsSolved(tilemap.cellBounds))
                {
                    break;
                }
                
                ++currentRetryCount;
                Debug.LogAssertion("No solution could be found for configuration");
            }

            CheckSolved();
        }

        public void SolveCoroutine()
        {
            if (solveCoroutine != null)
            {
                StopCoroutine(solveCoroutine);
                solveCoroutine = null;
            }

            solveCoroutine = StartCoroutine(SolveCoroutineImpl());
        }

        public void SolveStep()
        {
            if (!tilemapSolver.SolveStep(tilemap))
            {
                Debug.LogAssertion("Step failed to solve");
            }

            CheckSolved();
        }

        private IEnumerator SolveCoroutineImpl()
        {
            yield return tilemapSolver.SolveCoroutine(tilemap);

            CheckSolved();
        }

        private void CheckSolved()
        {
            if (tilemapSolver.IsSolved(tilemap.cellBounds))
            {
                Debug.Log("Tilemap solved completely");
            }
            else
            {
                Debug.LogAssertion("Tilemap solve failed");
            }
        }
    }
}
