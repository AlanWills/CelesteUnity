using Celeste.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Celeste.Testing
{
    [AddComponentMenu("Celeste/Testing/Integration Test Runner")]
    public class IntegrationTestRunner : SceneSingleton<IntegrationTestRunner>
    {
        #region Properties and Fields

        [Header("General")]
        [SerializeField] private string mStartingSceneName = "Startup";
        [SerializeField] private float mRequiredInactivityTime = 3.0f;

        private Coroutine mRunningTestsCoroutine = default;
        private Coroutine mRunningTestCoroutine = default;
        private IntegrationTest mCurrentTest = default;
        private List<Coroutine> mActiveCoroutines = new List<Coroutine>();
        private float mCurrentInactivityTime = 0.0f;

        #endregion

        public static AsyncOperation LoadScene()
        {
            return SceneManager.LoadSceneAsync(IntegrationTestConstants.INTEGRATION_TEST_SCENE_NAME, LoadSceneMode.Single);
        }

        private static AsyncOperation UnloadScene()
        {
            return SceneManager.UnloadSceneAsync(IntegrationTestConstants.INTEGRATION_TEST_SCENE_NAME);
        }

        #region Unity Methods

        private void Start()
        {
            Application.logMessageReceived += HandleLog;
        }

        private void OnDestroy()
        {
            TryStopCurrentTests();

            Application.logMessageReceived -= HandleLog;
        }

        #endregion

        #region Run Test Steps

        public void RunTests(List<IntegrationTest> integrationTests)
        {
            TryStopCurrentTests();
            mRunningTestsCoroutine = StartCoroutine(RunTestsImpl(integrationTests));
        }

        private IEnumerator RunTestsImpl(List<IntegrationTest> integrationTests)
        {
            foreach (IntegrationTest integrationTest in integrationTests)
            {
                yield return Setup(integrationTest);
                
                while (mCurrentInactivityTime < mRequiredInactivityTime)
                {
                    mCurrentInactivityTime += Time.deltaTime;
                    yield return null;
                }

                float currentTestTime = 0;
                mRunningTestCoroutine = StartCoroutine(RunTestImpl(integrationTest));

                while (mRunningTestCoroutine != null)
                {
                    yield return null;

                    currentTestTime += Time.unscaledDeltaTime;

                    if (currentTestTime > mCurrentTest.TimeoutSeconds)
                    {
                        // Test has timed out
                        mCurrentTest.FailBecauseOfTimeout(currentTestTime);
                        StopCurrentTestExecution();
                    }
                }

                yield return TearDown();
            }
        }

        private IEnumerator RunTestImpl(IntegrationTest integrationTest)
        {
            while (integrationTest.Running)
            {
                yield return integrationTest.Run();
            }

            StopCurrentTestExecution();
        }

        public void TryStopCurrentTests()
        {
            StopCurrentTestExecution();

            if (mRunningTestsCoroutine != null)
            {
                StopCoroutine(mRunningTestsCoroutine);
                mRunningTestsCoroutine = null;
            }

            mCurrentTest = null;
        }

        private void StopCurrentTestExecution()
        {
            if (mRunningTestCoroutine != null)
            {
                StopCoroutine(mRunningTestCoroutine);
                mRunningTestCoroutine = null;
            }
        }

        private IEnumerator Setup(IntegrationTest integrationTest)
        {
            mCurrentTest = integrationTest;

            // Boot game and wait for it to finish loading
            {
                var task = SceneManager.LoadSceneAsync(mStartingSceneName, LoadSceneMode.Additive);
                yield return new WaitUntil(() => task.isDone);

                task = UnloadScene();
                yield return new WaitUntil(() => task.isDone);
            }

            // Setup test
            {
                mCurrentInactivityTime = 0.0f;
                mCurrentTest.Setup();
            }
        }

        private IEnumerator TearDown()
        {
            mCurrentTest.TearDown();
            mCurrentTest = null;
            
            foreach (var entry in mActiveCoroutines)
            {
                StopCoroutine(entry);
            }
            mActiveCoroutines.Clear();

            // Unload the entire scene stack, by just going back to the initial scene
            var task = LoadScene();
            yield return new WaitUntil(() => task.isDone);

            mCurrentInactivityTime = 0.0f;
        }

        #endregion

        #region Callbacks

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (mCurrentTest != null && mCurrentTest.Running)
            {
                TestLogType testLogType = GetTestLogType(type);

                if ((mCurrentTest.FailOnLogType & testLogType) == testLogType)
                {
                    mCurrentTest.FailBecauseOfLogType(logString, testLogType);
                    StopCurrentTestExecution();
                }
            }
        }

        private TestLogType GetTestLogType(LogType type)
        {
            switch (type)
            {
                case LogType.Log:
                    return TestLogType.Log;

                case LogType.Warning:
                    return TestLogType.Warning;

                case LogType.Error:
                    return TestLogType.Error;

                case LogType.Exception:
                    return TestLogType.Exception;

                case LogType.Assert:
                    return TestLogType.Assertion;

                default:
                    return TestLogType.Exception;
            }
        }

        #endregion
    }
}
