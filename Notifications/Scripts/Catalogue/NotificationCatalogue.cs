using UnityEngine;
using Celeste.Objects;
using Celeste.Notifications.Objects;

namespace Celeste.Notifications.Catalogue
{
    [CreateAssetMenu(fileName = nameof(NotificationCatalogue), menuName = CelesteMenuItemConstants.NOTIFICATIONS_MENU_ITEM + "Notification Catalogue", order = CelesteMenuItemConstants.NOTIFICATIONS_MENU_ITEM_PRIORITY)]
    public class NotificationCatalogue : ListScriptableObject<Notification>
    {
    }
}