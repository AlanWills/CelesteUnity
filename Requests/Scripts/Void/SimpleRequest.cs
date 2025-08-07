using System;
using UnityEngine;

namespace Celeste.Requests
{
    [CreateAssetMenu(fileName = nameof(SimpleRequest), menuName = CelesteMenuItemConstants.REQUESTS_MENU_ITEM + "Simple Request", order = CelesteMenuItemConstants.REQUESTS_MENU_ITEM_PRIORITY)]
    public class SimpleRequest : ScriptableObject, IRequest
    {
        #region Properties and Fields

        [NonSerialized] private IRequestHandler requestHandler;
        
        #endregion
        
        public void SetHandler(IRequestHandler handler)
        {
            requestHandler = handler;
        }

        public void ClearHandler()
        {
            requestHandler = null;
        }

        public void Raise(SuccessCallback successCallback, FailureCallback failureCallback)
        {
            Debug.Log($"Executing {nameof(SimpleRequest)} {name}.");
            RaiseSilently(successCallback, failureCallback);
        }

        public void RaiseSilently(SuccessCallback successCallback, FailureCallback failureCallback)
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
}