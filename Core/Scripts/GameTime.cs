using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Core
{
    public static class GameTime
    {
        public static long Now
        {
            get { return DateTimeOffset.UtcNow.ToUnixTimeSeconds(); }
        }
    }
}