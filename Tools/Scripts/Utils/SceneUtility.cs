using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Celeste.Tools
{
    public static class SceneUtility
    {
        public static void FindMissingComponentsInLoadedScenes()
        {
            for (int i = 0, n = SceneManager.sceneCount; i < n; i++)
            {
                FindMissingComponents(SceneManager.GetSceneAt(i));
            }
        }

        public static void FindMissingComponents(this Scene scene)
        {
            if (scene.IsValid())
            {
                Stack<GameObject> gameObjectsToSearch = new Stack<GameObject>();

                foreach (GameObject gameObject in scene.GetRootGameObjects())
                {
                    gameObjectsToSearch.Push(gameObject);
                }

                while (gameObjectsToSearch.Count > 0)
                {
                    FindMissingComponentsInNext(gameObjectsToSearch);
                }
            }
            else
            {
                Debug.LogWarning($"{nameof(FindMissingComponents)}: Skipping invalid scene '{scene.name}'.");
            }
        }

        private static void FindMissingComponentsInNext(Stack<GameObject> toSearch)
        {
            Debug.Assert(toSearch.Count > 0, $"No more game objects left to search!");
            GameObject gameObject = toSearch.Pop();
            var components = gameObject.GetComponents<Component>();

            for (int i = 0, n = components != null ? components.Length : 0; i < n; ++i)
            {
                if (components[i] == null)
                {
                    Debug.LogError($"Found missing component at index {i} on game object {gameObject.name}.", gameObject);
                }
            }

            for (int i = 0, n = gameObject.transform.childCount; i < n; ++i)
            {
                toSearch.Push(gameObject.transform.GetChild(i).gameObject);
            }
        }
    }
}
