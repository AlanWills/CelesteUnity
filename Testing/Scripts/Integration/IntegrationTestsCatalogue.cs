using Celeste.Objects;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Testing
{
    [CreateAssetMenu(fileName = nameof(IntegrationTestsCatalogue), menuName = "Celeste/Testing/Integration Tests Catalogue")]
    public class IntegrationTestsCatalogue : ArrayScriptableObject<IntegrationTest>
    {
        public List<IntegrationTest> AllIntegrationTests
        {
            get
            {
                List<IntegrationTest> tests = new List<IntegrationTest>(NumItems);
                for (int i = 0; i < NumItems; ++i)
                {
                    tests.Add(GetItem(i));
                }

                return tests;
            }
        }
    }
}
