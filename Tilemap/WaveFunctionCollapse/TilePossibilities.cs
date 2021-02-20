using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Tilemaps.WaveFunctionCollapse
{
    public class TilePossibilities
    {
        #region Properties and Fields

        public bool HasPossibilities
        {
            get { return possibleTiles.Count > 0; }
        }

        public bool HasCollapsed { get; private set; } = false;

        public List<TileDescription> possibleTiles = new List<TileDescription>();

        private Vector2Int coords = new Vector2Int();

        #endregion

        public TilePossibilities(int x, int y, TileDescription collapsedTile)
        {
            possibleTiles.Add(collapsedTile);
            coords.x = x;
            coords.y = y;

            HasCollapsed = true;
        }

        public TilePossibilities(int x, int y, List<TileDescription> tiles)
        {
            possibleTiles.AddRange(tiles);
            coords.x = x;
            coords.y = y;
        }

        #region Possibility Functions

        public TileDescription Collapse()
        {
            Debug.Assert(HasPossibilities);

            float totalWeight = 0;
            foreach (TileDescription tileDescription in possibleTiles)
            {
                totalWeight += tileDescription.weight;
            }

            float currentWeight = 0;
            float randomChance = UnityEngine.Random.Range(0, totalWeight);
            
            foreach (TileDescription tileDescription in possibleTiles)
            {
                if (randomChance >= currentWeight && randomChance < (currentWeight + tileDescription.weight))
                {
                    possibleTiles.Clear();
                    possibleTiles.Add(tileDescription);
                    HasCollapsed = true;

                    return tileDescription;
                }

                currentWeight += tileDescription.weight;
            }

            Debug.LogAssertionFormat("Failed to collapse tile at location {0}", coords);
            return null;
        }

        public void RemoveUnsupportedPossibilitiesBecause(Direction direction, TileDescription other)
        {
            if (HasCollapsed)
            {
                // We have already chosen so altering possibilities is not necessary
                // NB: If you want to find bugs in your rules, uncomment this - this is VERY useful for debugging as
                // collapsed tiles by definition must be valid
                return;
            }

            for (int i = possibleTiles.Count - 1; i >= 0; --i)
            {
                if (!possibleTiles[i].SupportsTile(other, direction))
                {
#if UNITY_EDITOR
                    Debug.LogFormat("Removing possible tile {0} due to not supporting {1} in direction {2}", possibleTiles[i], other != null ? other.name : "null", direction);
#endif
                    possibleTiles.RemoveAt(i);
                    Debug.AssertFormat(HasPossibilities, "No more possibilities at location {0}", coords);
                }
            }
        }

#endregion
    }
}