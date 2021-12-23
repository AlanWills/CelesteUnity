using Celeste.Testing;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CelesteEditor.Testing
{
    [InitializeOnLoad]
    public class IntegrationTestInvoker : ScriptableSingleton<IntegrationTestInvoker>
    {
        private List<IntegrationTest> tests = new List<IntegrationTest>();

        static IntegrationTestInvoker()
        {
            EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
        }

        public void RunTest(IntegrationTest integrationTest)
        {
            RunTests(new List<IntegrationTest>() { integrationTest });
        }

        public void RunTests(List<IntegrationTest> integrationTests)
        {
            tests.Clear();
            tests.AddRange(integrationTests);

            if (!Application.isPlaying)
            {
                if (SceneManager.sceneCount != 1 || string.CompareOrdinal(SceneManager.GetSceneAt(0).name, IntegrationTestConstants.INTEGRATION_TEST_SCENE_NAME) != 0)
                {
                    // Load the integration test scene if it's not the only scene loaded right now
                    EditorSceneManager.OpenScene(IntegrationTestConstants.INTEGRATION_TEST_SCENE_PATH, OpenSceneMode.Single);
                }

                EditorApplication.isPlaying = true;
            }
            else
            {
                RunTests();
            }
        }

        private void RunTests()
        {
            IntegrationTestRunner runner = FindObjectOfType<IntegrationTestRunner>();
            if (runner != null)
            {
                runner.RunTests(instance.tests);
            }
        }

        private static void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.EnteredPlayMode && instance.tests.Count > 0)
            {
                instance.RunTests();
            }
            else if (obj == PlayModeStateChange.EnteredEditMode)
            {
                instance.tests.Clear();
            }
        }
    }
}
