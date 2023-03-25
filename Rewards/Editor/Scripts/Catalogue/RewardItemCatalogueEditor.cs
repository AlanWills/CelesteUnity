using Celeste.Rewards.Catalogue;
using Celeste.Rewards.Objects;
using CelesteEditor.DataStructures;
using UnityEditor;

namespace CelesteEditor.Rewards.Catalogue
{
    [CustomEditor(typeof(RewardItemCatalogue))]
    public class RewardItemCatalogueEditor : IIndexableItemsEditor<RewardItem>
    {
    }
}
