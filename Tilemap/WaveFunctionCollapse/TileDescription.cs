using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Celeste.Tilemaps.WaveFunctionCollapse
{
    [CreateAssetMenu(fileName = "TileDescription", menuName = "Celeste/Tilemaps/Wave Function Collapse/Tile Description")]
    public class TileDescription : ScriptableObject
    {
        #region Serialized Fields

        public IEnumerable<Rule> Rules
        {
            get { return rules; }
        }

        public int NumRules
        {
            get { return rules.Count; }
        }

        public float weight = 1;
        public TileBase tile;
        [SerializeField, HideInInspector] private List<Rule> rules = new List<Rule>();

        #endregion

        #region Rules Functions

        public Rule AddRule()
        {
            Rule rule = ScriptableObject.CreateInstance<Rule>();
            rule.hideFlags = HideFlags.HideInHierarchy;
            rules.Add(rule);

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.AddObjectToAsset(rule, this);
            UnityEditor.EditorUtility.SetDirty(this);
#endif

            return rule;
        }

        public void RemoveRule(int ruleIndex)
        {
            if (0 <= ruleIndex && ruleIndex < rules.Count)
            {
#if UNITY_EDITOR
                UnityEditor.AssetDatabase.RemoveObjectFromAsset(rules[ruleIndex]);
                UnityEditor.EditorUtility.SetDirty(this);
#endif
                rules.RemoveAt(ruleIndex);
            }
        }

        public void RemoveRule(Rule rule)
        {

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.RemoveObjectFromAsset(rule);
            UnityEditor.EditorUtility.SetDirty(this);
#endif
            rules.Remove(rule);
        }

        public void ClearRules()
        {
#if UNITY_EDITOR
            foreach (Rule rule in rules)
            {
                UnityEditor.AssetDatabase.RemoveObjectFromAsset(rule);
            }
#endif

            rules.Clear();
        }

        public Rule FindRule(Predicate<Rule> predicate)
        {
            return rules.Find(predicate);
        }

        /// <summary>
        /// Attempts to find a rule within this tile description which has the inputted otherTile set and direction Opposite to the inputted direction.
        /// </summary>
        /// <param name="otherTile">The rule must have this tile set</param>
        /// <param name="direction">The rule must have the opposite of this direction set</param>
        /// <returns></returns>
        public Rule FindOppositeRule(TileDescription otherTile, Direction direction)
        {
            return FindRule(x => x.otherTile == otherTile && x.direction == direction.Opposite());
        }

        public Rule GetRule(int ruleIndex)
        {
            return 0 <= ruleIndex && ruleIndex < NumRules ? rules[ruleIndex] : null;
        }

        public bool SupportsTile(TileDescription tile, Direction direction)
        {
            foreach (Rule rule in rules)
            {
                if (rule.otherTile == tile && rule.direction == direction)
                {
                    return true;
                }
            }

            return false;
        }

#endregion
    }
}
