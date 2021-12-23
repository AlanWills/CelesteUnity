using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Testing
{
    public abstract class IntegrationTestType : ScriptableObject
    {
        #region Properties and Fields

        protected abstract Type ArgsType { get; }

        [NonSerialized] protected Action<string> Log = default;
        [NonSerialized] private Action Finish = default;

        #endregion

        public IntegrationTestTypeArgs CreateArgs()
        {
            Type argsType = ArgsType;
            IntegrationTestTypeArgs args = CreateInstance(argsType) as IntegrationTestTypeArgs;
            args.name = argsType.Name;

            return args;
        }

        public void TestSetup(Action finish, Action<string> log)
        {
            Finish = finish;
            Log = log;

            DoTestSetup();
        }

        public IEnumerator TestUpdate(IntegrationTestTypeArgs args)
        {
            yield return DoTestUpdate(args);

            Finish();
        }

        public void TestTearDown()
        {
            DoTestTearDown();

            Finish = null;
            Log = null;
        }

        protected virtual void DoTestSetup() { }
        protected abstract IEnumerator DoTestUpdate(IntegrationTestTypeArgs args);
        protected virtual void DoTestTearDown() { }
    }
}
