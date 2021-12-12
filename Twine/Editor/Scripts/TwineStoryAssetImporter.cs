using Celeste.Twine;
using System.IO;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace CelesteEditor.Twine
{
    [ScriptedImporter(1, TwineStory.FILE_EXTENSION)]
    public class TwineStoryAssetImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            string assetText = File.ReadAllText(ctx.assetPath);
            TwineStory twineStory = ScriptableObject.CreateInstance<TwineStory>();
            JsonUtility.FromJsonOverwrite(assetText, twineStory);

            foreach (var node in twineStory.passages)
            {
                Debug.Assert(node != null, $"Null node found in twine story passages.");

                for (int i = 0, n = node.links.Count; i < n; ++i)
                {
                    TwineNodeLink link = node.links[i];
                    Debug.Assert(!link.broken, $"Link {link.name} to node {link.link} on node {node.name} is broken.");
                }
            }

            ctx.AddObjectToAsset("main", twineStory);
            ctx.SetMainObject(twineStory);
        }
    }
}