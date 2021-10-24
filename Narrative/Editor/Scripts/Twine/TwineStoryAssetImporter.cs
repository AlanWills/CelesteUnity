using System.IO;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace CelesteEditor.Narrative.Twine
{
    [ScriptedImporter(1, "twine")]
    public class TwineStoryAssetImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            string assetText = File.ReadAllText(ctx.assetPath);
            TwineStory twineStory = ScriptableObject.CreateInstance<TwineStory>();
            JsonUtility.FromJsonOverwrite(assetText, twineStory);

            foreach (var node in twineStory.passages)
            {
                foreach (var link in node.links)
                {
                    Debug.Assert(!link.broken, $"Link {link.name} to node {link.link} on node {node.name} is broken.");
                }
            }

            ctx.AddObjectToAsset("main", twineStory);
            ctx.SetMainObject(twineStory);
        }
    }
}