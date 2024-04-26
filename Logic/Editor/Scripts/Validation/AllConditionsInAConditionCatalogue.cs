using Celeste.Logic;
using Celeste.Logic.Catalogue;
using Celeste.Tools;
using CelesteEditor.Validation;
using System.Collections.Generic;
using System.Text;

namespace CelesteEditor.Logic.Validation
{
    public class AllConditionsInAConditionCatalogue : IValidationCondition
    {
        public string DisplayName => "All Conditions In A Condition Catalogue";

        public bool Validate(StringBuilder output)
        {
            bool succeeded = true;
            List<ConditionCatalogue> conditionCatalogues = EditorOnly.FindAssets<ConditionCatalogue>();

            foreach (Condition condition in EditorOnly.FindAssets<Condition>())
            {
                if (!conditionCatalogues.Exists(x => x.ContainsItem(condition)))
                {
                    output.AppendLine($"{DisplayName}: Failed to find a {nameof(ConditionCatalogue)} that contains {nameof(Condition)} {condition.name}.");
                    succeeded = false;
                }
            }

            return succeeded;
        }
    }
}
