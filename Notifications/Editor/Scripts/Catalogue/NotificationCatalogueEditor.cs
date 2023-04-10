using UnityEditor;
using CelesteEditor.DataStructures;
using Celeste.Notifications.Objects;
using Celeste.Notifications.Catalogue;

namespace CelesteEditor.Notifications.Catalogue
{
    [CustomEditor(typeof(NotificationCatalogue))]
    public class NotificationCatalogueEditor : IIndexableItemsEditor<Notification>
    {
    }
}