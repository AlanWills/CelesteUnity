using UnityEngine;
using Celeste.Objects;
using Celeste.Notifications.Objects;

namespace Celeste.Notifications.Catalogue
{
    [CreateAssetMenu(fileName = nameof(NotificationCatalogue), menuName = "Celeste/Notifications/Notification Catalogue")]
    public class NotificationCatalogue : ListScriptableObject<Notification>
    {
    }
}