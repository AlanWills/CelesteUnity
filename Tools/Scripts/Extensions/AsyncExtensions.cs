using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Tools
{
    public static class AsyncExtensions
    {
        public static async void FireAndForget(this Task task, string context = null)
        {
            try
            {
                await task;
            }
            catch (Exception e)
            {
                string message = string.IsNullOrEmpty(context) 
                    ? $"[Async Error]: {e}" 
                    : $"[Async Error - {context}]: {e}";
            
                UnityEngine.Debug.LogError(message);
            }
        }
        
        public static async void FireAndForget(this Awaitable task, string context = null)
        {
            try
            {
                await task;
            }
            catch (Exception e)
            {
                string message = string.IsNullOrEmpty(context) 
                    ? $"[Async Error]: {e}" 
                    : $"[Async Error - {context}]: {e}";
            
                UnityEngine.Debug.LogError(message);
            }
        }
        
        public static async void FireAndForget<T>(this Awaitable<T> task, string context = null)
        {
            try
            {
                await task;
            }
            catch (Exception e)
            {
                string message = string.IsNullOrEmpty(context) 
                    ? $"[Async Error]: {e}" 
                    : $"[Async Error - {context}]: {e}";
            
                UnityEngine.Debug.LogError(message);
            }
        }
    }
}