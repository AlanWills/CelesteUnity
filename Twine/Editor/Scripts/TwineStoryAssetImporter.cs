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

                for (int i = 0, n = node.Links.Count; i < n; ++i)
                {
                    TwineNodeLink link = node.Links[i];
                    Debug.Assert(!link.broken, $"Link {link.name} to node {link.link} on node {node.Name} is broken.");
                }

                var tags = node.Tags;
                for (int i = 0, n = tags != null ? tags.Count : 0; i < n; ++i)
                {
                    tags[i] = tags[i].Trim();
                }
            }

            ctx.AddObjectToAsset("main", twineStory);
            ctx.SetMainObject(twineStory);
        }
    }
}