using System;

namespace Celeste.Core
{
    public struct CallbackHandle
    {
        public static readonly CallbackHandle INVALID_HANDLE = new CallbackHandle(-1);

        public bool IsValid => Id != INVALID_HANDLE.Id;
        public int Id { get; }

        public CallbackHandle(int id)
        {
            Id = id;
        }

        public override bool Equals(object obj)
        {
            return obj is CallbackHandle handle &&
                   Id == handle.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }

        public static bool operator==(CallbackHandle lhs, CallbackHandle rhs)
        {
            return lhs.Id == rhs.Id;
        }

        public static bool operator !=(CallbackHandle lhs, CallbackHandle rhs)
        {
            return !(lhs == rhs);
        }
    }

    public struct Callback
    {
        public CallbackHandle Handle { get; }
        public Action Action { get; }

        public Callback(CallbackHandle handle, Action action)
        {
            Handle = handle;
            Action = action;
        }
    }

    public struct Callback<T>
    {
        public CallbackHandle Handle { get; }
        public Action<T> Action { get; }

        public Callback(CallbackHandle handle, Action<T> action)
        {
            Handle = handle;
            Action = action;
        }
    }
}
