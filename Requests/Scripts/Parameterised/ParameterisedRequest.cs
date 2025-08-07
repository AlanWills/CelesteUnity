using System;
using UnityEngine;

namespace Celeste.Requests
{
    public class ParameterisedRequestWithArgs<TRequestArgs> : ScriptableObject, IRequestWithArgs<TRequestArgs>
    {
        #region Properties and Fields

        [NonSerialized] private IRequestHandlerWithArgs<TRequestArgs> requestHandler;
        
        #endregion
        
        public void SetHandler(IRequestHandlerWithArgs<TRequestArgs> handler)
        {
            requestHandler = handler;
        }

        public void ClearHandler()
        {
            requestHandler = null;
        }

        public void Raise(TRequestArgs requestArgs, SuccessCallback successCallback, FailureCallback failureCallback)
        {
            Debug.Log($"Executing {nameof(SimpleRequest)} {name}.");
            RaiseSilently(requestArgs, successCallback, failureCallback);
        }

        public void RaiseSilently(TRequestArgs requestArgs, SuccessCallback successCallback, FailureCallback failureCallback)
        {
            if (requestHandler != null)
            {
                requestHandler.OnRequestMade(requestArgs, successCallback, failureCallback);
            }
            else
            {
                failureCallback?.Invoke(IRequest.REQUEST_NOT_HANDLED_ERROR_CODE, IRequest.REQUEST_NOT_HANDLED_ERROR_MESSAGE);
            }
        }
    }
    
    public class ParameterisedRequestWithResponse<TResponseArgs> : ScriptableObject, IRequestWithResponse<TResponseArgs>
    {
        #region Properties and Fields

        [NonSerialized] private IRequestHandlerWithResponse<TResponseArgs> requestHandler;
        
        #endregion
        
        public void SetHandler(IRequestHandlerWithResponse<TResponseArgs> handler)
        {
            requestHandler = handler;
        }

        public void ClearHandler()
        {
            requestHandler = null;
        }

        public void Raise(SuccessCallback<TResponseArgs> successCallback, FailureCallback failureCallback)
        {
            Debug.Log($"Executing {nameof(SimpleRequest)} {name}.");
            RaiseSilently(successCallback, failureCallback);
        }

        public void RaiseSilently(SuccessCallback<TResponseArgs> successCallback, FailureCallback failureCallback)
        {
            if (requestHandler != null)
            {
                requestHandler.OnRequestMade(successCallback, failureCallback);
            }
            else
            {
                failureCallback?.Invoke(IRequest.REQUEST_NOT_HANDLED_ERROR_CODE, IRequest.REQUEST_NOT_HANDLED_ERROR_MESSAGE);
            }
        }
    }
    
    public class ParameterisedRequestWithArgsAndResponse<TRequestArgs, TResponseArgs> : ScriptableObject, IRequestWithArgsAndResponse<TRequestArgs, TResponseArgs>
    {
        #region Properties and Fields

        [NonSerialized] private IRequestHandlerWithArgsAndResponse<TRequestArgs, TResponseArgs> requestHandler;
        
        #endregion
        
        public void SetHandler(IRequestHandlerWithArgsAndResponse<TRequestArgs, TResponseArgs> handler)
        {
            requestHandler = handler;
        }

        public void ClearHandler()
        {
            requestHandler = null;
        }

        public void Raise(TRequestArgs requestArgs, SuccessCallback<TResponseArgs> successCallback, FailureCallback failureCallback)
        {
            Debug.Log($"Executing {nameof(SimpleRequest)} {name}.");
            RaiseSilently(requestArgs, successCallback, failureCallback);
        }

        public void RaiseSilently(TRequestArgs requestArgs, SuccessCallback<TResponseArgs> successCallback, FailureCallback failureCallback)
        {
            if (requestHandler != null)
            {
                requestHandler.OnRequestMade(requestArgs, successCallback, failureCallback);
            }
            else
            {
                failureCallback?.Invoke(IRequest.REQUEST_NOT_HANDLED_ERROR_CODE, IRequest.REQUEST_NOT_HANDLED_ERROR_MESSAGE);
            }
        }
    }
}