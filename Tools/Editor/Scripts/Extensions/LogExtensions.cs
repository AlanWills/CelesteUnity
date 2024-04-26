using System.Reflection;

namespace CelesteEditor.Tools
{
    public static class LogExtensions
    {
        public static void Clear()
        {
            var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }
    }
}
