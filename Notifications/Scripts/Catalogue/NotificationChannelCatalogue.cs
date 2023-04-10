using UnityEngine;
using Celeste.Objects;
using Celeste.Notifications.Objects;

namespace Celeste.Notifications.Catalogue
{
    [CreateAssetMenu(fileName = nameof(NotificationChannelCatalogue), menuName = "Celeste/Notifications/Notification Channel Catalogue")]
    public class NotificationChannelCatalogue : ListScriptableObject<NotificationChannel>
    {
    }
}