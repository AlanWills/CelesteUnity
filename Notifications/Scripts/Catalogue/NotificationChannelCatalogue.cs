using UnityEngine;
using Celeste.Objects;
using Celeste.Notifications.Objects;

namespace Celeste.Notifications.Catalogue
{
    [CreateAssetMenu(fileName = nameof(NotificationChannelCatalogue), menuName = CelesteMenuItemConstants.NOTIFICATIONS_MENU_ITEM + "Notification Channel Catalogue", order = CelesteMenuItemConstants.NOTIFICATIONS_MENU_ITEM_PRIORITY)]
    public class NotificationChannelCatalogue : ListScriptableObject<NotificationChannel>
    {
    }
}