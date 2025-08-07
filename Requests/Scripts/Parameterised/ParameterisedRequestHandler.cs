using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Requests
{
    public class ParameterisedRequestHandlerWithArgs<TRequest, TRequestArgs> : MonoBehaviour, IRequestHandlerWithArgs<TRequestArgs>
        where TRequest : ParameterisedRequestWithArgs<TRequestArgs>
    {
        #region Properties and Fields

        [SerializeField] private TRequest request;
        [SerializeField] private bool logSetAndClearHandler;
        [SerializeField] private UnityEvent<TRequestArgs, SuccessCallback, FailureCallback> response;

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
        
        public void OnRequestMade(TRequestArgs requestArgs, SuccessCallback successCallback, FailureCallback failureCallback)
        {
            response?.Invoke(requestArgs, successCallback, failureCallback);
        }

        #endregion
    }
    
    public class ParameterisedRequestHandlerWithResponse<TRequest, TResponseArgs> : MonoBehaviour, IRequestHandlerWithResponse<TResponseArgs>
        where TRequest : ParameterisedRequestWithResponse<TResponseArgs>
    {
        #region Properties and Fields

        [SerializeField] private TRequest request;
        [SerializeField] private bool logSetAndClearHandler;
        [SerializeField] private UnityEvent<SuccessCallback<TResponseArgs>, FailureCallback> response;

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
        
        public void OnRequestMade(SuccessCallback<TResponseArgs> successCallback, FailureCallback failureCallback)
        {
            response?.Invoke(successCallback, failureCallback);
        }

        #endregion
    }
    
    public class ParameterisedRequestHandlerWithArgsAndResponse<TRequest, TRequestArgs, TResponseArgs> : MonoBehaviour, IRequestHandlerWithArgsAndResponse<TRequestArgs, TResponseArgs>
        where TRequest : ParameterisedRequestWithArgsAndResponse<TRequestArgs, TResponseArgs>
    {
        #region Properties and Fields

        [SerializeField] private TRequest request;
        [SerializeField] private bool logSetAndClearHandler;
        [SerializeField] private UnityEvent<TRequestArgs, SuccessCallback<TResponseArgs>, FailureCallback> response;

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
        
        public void OnRequestMade(TRequestArgs requestArgs, SuccessCallback<TResponseArgs> successCallback, FailureCallback failureCallback)
        {
            response?.Invoke(requestArgs, successCallback, failureCallback);
        }

        #endregion
    }
}