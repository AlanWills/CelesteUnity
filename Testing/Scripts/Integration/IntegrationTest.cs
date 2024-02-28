using Celeste.Scene;
using Celeste.Tools.Attributes.GUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Testing
{
    public enum TestStatus
    {
        NotRun,
        Running,
        FinishedRunning,
        Passed,
        Failed,
    }

    [Flags]
    public enum TestLogType
    {
        Log = 1 << 0,
        Warning = 1 << 1,
        Error = 1 << 2,
        Exception = 1 << 3,
        Assertion = 1 << 4
    }

    [CreateAssetMenu(fileName = "IntegrationTest", order = CelesteMenuItemConstants.TESTING_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.TESTING_MENU_ITEM + "Integration Test")]
    public class IntegrationTest : ScriptableObject
    {
        #region Properties and Fields

        public bool Running { get { return TestStatus == TestStatus.Running; } }
        public TestStatus TestStatus { get; private set; } = TestStatus.NotRun;

        public string DisplayName
        {
            get { return mDisplayName; }
        }

        public string SnapshotName
        {
            get { return mSnapshotName; }
        }

        public float TimeoutSeconds
        {
            get { return mTimeoutSeconds; }
        }

        public TestLogType FailOnLogType
        {
            get { return mFailOnLogType; }
        }

        public int NumTestLogEntries
        {
            get { return mTestLog.Count; }
        }

        [SerializeField] private string mDisplayName = default;
        [SerializeField] private string mSnapshotName = default;
        [SerializeField] private float mTimeoutSeconds = 60;
        [SerializeField] private TestLogType mFailOnLogType = default;
        [SerializeField] private IntegrationTestType mTestType = default;
        [SerializeField, ReadOnly] private IntegrationTestTypeArgs mTestTypeArgs = default;

        [NonSerialized] private List<string> mTestLog = new List<string>(50);

        #endregion

        public void CreateArgs()
        {
            if (mTestTypeArgs != null)
            {
#if UNITY_EDITOR
                ScriptableObject.DestroyImmediate(mTestTypeArgs, true);
#endif
                mTestTypeArgs = null;
            }

            mTestTypeArgs = mTestType != null ? mTestType.CreateArgs() : null;

            if (mTestTypeArgs != null)
            {
#if UNITY_EDITOR
                UnityEditor.AssetDatabase.AddObjectToAsset(mTestTypeArgs, this);
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh();
#endif
            }
        }

        public void Setup()
        {
            TestStatus = TestStatus.Running;
            mTestLog.Clear();
            mTestType.TestSetup(Cancel, (string s) => { mTestLog.Add(s); });
        }

        public IEnumerator Run()
        {
            yield return mTestType.TestUpdate(mTestTypeArgs);
        }

        public void TearDown()
        {
            mTestType.TestTearDown();

            // If this status has been set to anything else, it means the test has been interrupted and we shouldn't evaluate the result
            if (TestStatus == TestStatus.FinishedRunning)
            {
                if (HaveConditionsPassed())
                {
                    TestStatus = TestStatus.Passed;
                    mTestLog.Add($"Test Passed!");
                }
                else
                {
                    FailBecauseOfConditions();
                }
            }
        }

        public void Cancel()
        {
            TestStatus = TestStatus.FinishedRunning;
        }

        public void FailBecauseOfLogType(string logString, TestLogType testLogType)
        {
            if (Running)
            {
                FailBecauseOfReason($"{IntegrationTestConstants.FAIL_REASON_LOG_MESSAGE_TYPE}: {testLogType}");
                mTestLog.Add(logString);
            }
        }

        public void FailBecauseOfTimeout(float actualRunTime)
        {
            if (Running)
            {
                FailBecauseOfReason($"{IntegrationTestConstants.FAIL_REASON_TIMEOUT}.  Actual run time: {actualRunTime}");
            }
        }

        private void FailBecauseOfConditions()
        {
            FailBecauseOfReason(IntegrationTestConstants.FAIL_REASON_CONDITION);
        }

        public void FailBecauseOfReason(string reason)
        {
            TestStatus = TestStatus.Failed;
            mTestLog.Add($"Test Failed due to {reason}.");
        }

        public string GetTestLogEntry(int index)
        {
            return 0 <= index && index < NumTestLogEntries ? mTestLog[index] : "";
        }

        public void SaveLastResultsAsExpected()
        {
            TestStatus = TestStatus.NotRun;

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }

        private bool HaveConditionsPassed()
        {
            return true;
        }
    }
}
