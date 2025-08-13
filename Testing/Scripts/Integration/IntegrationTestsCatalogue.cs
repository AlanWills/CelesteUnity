using Celeste.Objects;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Testing
{
    [CreateAssetMenu(fileName = nameof(IntegrationTestsCatalogue), order = CelesteMenuItemConstants.TESTING_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.TESTING_MENU_ITEM + "Integration Tests Catalogue")]
    public class IntegrationTestsCatalogue : ListScriptableObject<IntegrationTest>
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
