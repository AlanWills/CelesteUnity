using UnityEngine.Events;

namespace Celeste.Logic.Interfaces
{
    public struct ConditionProgress
    {
        public int currentProgress;
        public int requiredProgress;
    }

    public interface IProgressCondition
    {
        ConditionProgress GetProgress();

        void AddOnProgressChangedCallback(UnityAction<IProgressCondition> callback);
        void RemoveOnProgressChangedCallback(UnityAction<IProgressCondition> callback);
    }
}