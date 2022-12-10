using System;
using UnityEngine.Events;

namespace Celeste.Events
{
    public interface ICallbackHandle : IEquatable<ICallbackHandle>
    {
        bool IsValid { get; }
        int Id { get; }

        void MakeInvalid();
    }

    public struct CallbackHandle : ICallbackHandle
    {
        #region Properties and Fields

        public bool IsValid => Id != default;
        public int Id { get; private set; }

        private static int s_guid = 1;

        #endregion

        public CallbackHandle(int id)
        {
            Id = id;
        }

        public void MakeInvalid()
        {
            Id = default;
        }

        public static CallbackHandle New()
        {
            return new CallbackHandle(++s_guid);
        }

        #region Overrides

        public override bool Equals(object obj)
        {
            return obj is CallbackHandle handle &&
                   Id == handle.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }

        public bool Equals(ICallbackHandle other)
        {
            return Id == other.Id;
        }

        public static bool operator==(CallbackHandle lhs, CallbackHandle rhs)
        {
            return lhs.Id == rhs.Id;
        }

        public static bool operator !=(CallbackHandle lhs, CallbackHandle rhs)
        {
            return !(lhs == rhs);
        }

        #endregion
    }

    public struct ActionCallback
    {
        public ICallbackHandle Handle { get; }
        public Action Action { get; }

        public ActionCallback(ICallbackHandle handle, Action action)
        {
            Handle = handle;
            Action = action;
        }
    }

    public struct ActionCallback<T>
    {
        public ICallbackHandle Handle { get; }
        public Action<T> Action { get; }

        public ActionCallback(ICallbackHandle handle, Action<T> action)
        {
            Handle = handle;
            Action = action;
        }
    }

    public struct UnityActionCallback
    {
        public ICallbackHandle Handle { get; }
        public UnityAction Action { get; }

        public UnityActionCallback(ICallbackHandle handle, UnityAction action)
        {
            Handle = handle;
            Action = action;
        }
    }

    public struct UnityActionCallback<T>
    {
        public ICallbackHandle Handle { get; }
        public UnityAction<T> Action { get; }

        public UnityActionCallback(ICallbackHandle handle, UnityAction<T> action)
        {
            Handle = handle;
            Action = action;
        }
    }
}
