using Celeste.Testing;
using Celeste.Tools;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Testing
{
    public class IntegrationTestsWizard : ScriptableWizard
    {
        #region Properties and Fields

        private Vector2 mScrollPosition = default;
        private IntegrationTestsCatalogue mIntegrationTestsCatalogue = default;
        private List<IntegrationTestGUI> mTestGUIs = new List<IntegrationTestGUI>();
        private List<IntegrationTest> mSelectedTestsCache = new List<IntegrationTest>();

        #endregion

        #region Menu Item

        [MenuItem("Celeste/Testing/Integration Tests Wizard")]
        public static void ShowIntegrationTestsWizard()
        {
            DisplayWizard<IntegrationTestsWizard>("Run Integration Tests", "Close", "Run Selected");
        }

        #endregion

        #region GUI

        private void OnEnable()
        {
            mIntegrationTestsCatalogue = EditorOnly.FindAsset<IntegrationTestsCatalogue>();

            RefreshGUI();
        }

        protected override bool DrawWizardGUI()
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Select All", GUILayout.ExpandWidth(false)))
            {
                SetSelectionStatusForAll(true);
            }

            if (GUILayout.Button("Select Failed", GUILayout.ExpandWidth(false)))
            {
                SetSelectionStatusForMatching(true, TestStatus.Failed);
            }

            if (GUILayout.Button("Deselect All", GUILayout.ExpandWidth(false)))
            {
                SetSelectionStatusForAll(false);
            }

            if (GUILayout.Button("Refresh", GUILayout.ExpandWidth(false)))
            {
                RefreshGUI();
            }

            EditorGUILayout.EndHorizontal();

            mScrollPosition = EditorGUILayout.BeginScrollView(mScrollPosition);

            foreach (IntegrationTestGUI testGUI in mTestGUIs)
            {
                testGUI.GUI();
            }

            EditorGUILayout.EndScrollView();

            return true;
        }

        private void OnWizardCreate() { }

        private void OnWizardOtherButton()
        {
            mSelectedTestsCache.Clear();

            foreach (IntegrationTestGUI testGui in mTestGUIs)
            {
                if (testGui.IsSelected)
                {
                    mSelectedTestsCache.Add(testGui.IntegrationTest);
                }
            }

            IntegrationTestInvoker.instance.RunTests(mSelectedTestsCache);
        }

        #endregion

        #region Utility Methods

        private void SetSelectionStatusForAll(bool selectionStatus)
        {
            foreach (IntegrationTestGUI testGUI in mTestGUIs)
            {
                testGUI.IsSelected = selectionStatus;
            }
        }

        private void SetSelectionStatusForMatching(bool selectionStatus, TestStatus testStatus)
        {
            foreach (IntegrationTestGUI testGUI in mTestGUIs)
            {
                testGUI.IsSelected = testGUI.TestStatus == testStatus ? selectionStatus : !selectionStatus;
            }
        }

        private void RefreshGUI()
        {
            mTestGUIs.Clear();

            for (int i = 0, n = mIntegrationTestsCatalogue.NumItems; i < n; ++i)
            {
                IntegrationTest integrationTest = mIntegrationTestsCatalogue.GetItem(i);
                mTestGUIs.Add(new IntegrationTestGUI(integrationTest));
            }
        }

        #endregion
    }
}
