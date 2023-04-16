using Celeste.Notifications.Objects;
using Celeste.Notifications.Record;
using System;
using System.Collections.Generic;

namespace Celeste.Notifications.Persistence
{
    [Serializable]
    public class NotificationChannelDTO
    {
        public int guid;
        public bool enabled;

        public NotificationChannelDTO(NotificationChannel channel)
        {
            guid = channel.Guid;
            enabled = channel.Enabled;
        }
    }

    [Serializable]
    public class NotificationManagerDTO
    {
        public List<NotificationChannelDTO> notificationChannels = new List<NotificationChannelDTO>();

        public NotificationManagerDTO(NotificationRecord record)
        {
            for (int i = 0, n = record.NumNotificationChannels; i < n; ++i)
            {
                notificationChannels.Add(new NotificationChannelDTO(record.GetNotificationChannel(i)));
            }
        }
    }
}