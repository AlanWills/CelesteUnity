using Celeste.Twine;
using System.IO;
using UnityEditor;
using UnityEngine;
using static CelesteEditor.Scene.MenuItemUtility;

namespace DnDEditor.Twine
{
    public static class MenuItems
    {
        [MenuItem("Celeste/Save/Open Twine Save", priority = 0)]
        public static void OpenTwineSaveMenuItem()
        {
            OpenExplorerAtPersistentData();
        }

        [MenuItem("Celeste/Save/Delete Twine Save", priority = 100)]
        public static void DeleteTwineSaveMenuItem()
        {
            DeletePersistentDataFile(TwineManager.FILE_NAME);
        }

        [MenuItem("Celeste/Narrative/Create Links For Twine Story")]
        public static void CreateLinksFromText()
        {
            foreach (var obj in Selection.objects)
            {
                if (obj is TwineStory)
                {
                    UnityEngine.Debug.Log($"Creating links for story {obj.name}.");
                    TwineStory twineStory = obj as TwineStory;

                    foreach (TwineNode twineNode in twineStory.passages)
                    {
                        twineNode.Links.Clear();
                        twineNode.Links.AddRange(TwineNodeLink.CreateFromText(twineNode.Text));

                        foreach (TwineNodeLink twineNodeLink in twineNode.Links)
                        {
                            TwineNode linkedNode = twineStory.passages.Find(x => string.CompareOrdinal(x.Name, twineNodeLink.link) == 0);
                            twineNodeLink.pid = linkedNode != null ? linkedNode.pid : 0;
                            twineNodeLink.broken = linkedNode == null;
                        }
                    }

                    string storyPath = AssetDatabase.GetAssetPath(twineStory);
                    twineStory.Save(storyPath);
                    
                    AssetDatabase.Refresh();
                }
                else
                {
                    UnityEngine.Debug.LogWarning($"Skipping creating links for {obj.name}.  It is not a {nameof(TwineStory)}.");
                }
            }
        }
    }
}