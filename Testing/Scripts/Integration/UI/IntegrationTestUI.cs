using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace Celeste.Testing
{
    [AddComponentMenu("Celeste/Testing/UI/Integration Test UI")]
    public class IntegrationTestUI : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private IntegrationTestsCatalogue mIntegrationTestsCatalogue = default;
        [SerializeField] private TextMeshProUGUI mResultsText = default;

        private StringBuilder mAllResultsInfoSB = new StringBuilder();
        private StringBuilder mTestResultsSB = new StringBuilder();

        #endregion

        private void Awake()
        {
            ShowResults();
        }

        public void RunAll()
        {
            IntegrationTestRunner.Instance.RunTests(mIntegrationTestsCatalogue.AllIntegrationTests);
        }

        public void ShowResults()
        {
            int passed = 0;
            int failed = 0;

            mAllResultsInfoSB.Clear();
            mTestResultsSB.Clear();

            for (int i = 0, n = mIntegrationTestsCatalogue.NumItems; i < n; ++i)
            {
                IntegrationTest integrationTest = mIntegrationTestsCatalogue.GetItem(i);
                Debug.Assert(integrationTest != null, $"Found null integration test at index {i}.");

                if (integrationTest != null)
                {
                    mTestResultsSB.AppendLine($"{integrationTest.DisplayName}: {integrationTest.TestStatus}");

                    if (integrationTest.TestStatus == TestStatus.Passed)
                    {
                        ++passed;
                    }
                    else if (integrationTest.TestStatus == TestStatus.Failed)
                    {
                        ++failed;
                    }
                }
            }

            mAllResultsInfoSB.AppendLine($"Total In Catalogue: {mIntegrationTestsCatalogue.NumItems}");
            mAllResultsInfoSB.AppendLine($"Total Ran: {passed + failed}");
            mAllResultsInfoSB.AppendLine($"Total Passed: {passed}");
            mAllResultsInfoSB.AppendLine($"Total Failed: {failed}");
            mAllResultsInfoSB.AppendLine();
            mAllResultsInfoSB.Append(mTestResultsSB.ToString());

            mResultsText.text = mAllResultsInfoSB.ToString();
        }
    }
}
