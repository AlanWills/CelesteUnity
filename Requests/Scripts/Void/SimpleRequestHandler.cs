using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Requests
{
    public class SimpleRequestHandler : MonoBehaviour, IRequestHandler
    {
        #region Properties and Fields

        [SerializeField] private SimpleRequest request;
        [SerializeField] private bool logSetAndClearHandler;
        [SerializeField] private UnityEvent<SuccessCallback, FailureCallback> response;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if (logSetAndClearHandler)
            {
                Debug.Log($"{name} is setting a handler for response {(request != null ? request.name : "null")}.");
            }

            Debug.Assert(request != null, $"{name} has a null response on handler {GetType().Name}");
            request?.SetHandler(this);
        }

        private void OnDisable()
        {
            if (logSetAndClearHandler)
            {
                Debug.Log($"{name} is clearing a handler for request {(request != null ? request.name : "null")}.");
            }
            
            request?.ClearHandler();
        }

        #endregion

        #region Request Methods
        
        public void OnRequestMade(SuccessCallback successCallback, FailureCallback failureCallback)
        {
            response?.Invoke(successCallback, failureCallback);
        }

        #endregion
    }
}