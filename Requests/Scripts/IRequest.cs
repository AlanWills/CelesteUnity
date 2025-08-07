namespace Celeste.Requests
{
    public delegate void SuccessCallback();
    public delegate void SuccessCallback<in TResponseArgs>(TResponseArgs responseArgs);
    public delegate void FailureCallback(int errorCode, string errorMessage);

    public interface IRequest
    {
        public const int REQUEST_NOT_HANDLED_ERROR_CODE = -1;
        public const string REQUEST_NOT_HANDLED_ERROR_MESSAGE = "Failure: No handler exists for the attempted request.";
        
        void SetHandler(IRequestHandler requestHandler);
        void ClearHandler();
        
        void Raise(SuccessCallback successCallback, FailureCallback failureCallback);
        void RaiseSilently(SuccessCallback successCallback, FailureCallback failureCallback);
    }
    
    public interface IRequestWithArgs<TRequestArgs>
    {
        void SetHandler(IRequestHandlerWithArgs<TRequestArgs> requestHandler);
        void ClearHandler();
        
        void Raise(TRequestArgs requestArgs, SuccessCallback successCallback, FailureCallback failureCallback);
        void RaiseSilently(TRequestArgs requestArgs, SuccessCallback successCallback, FailureCallback failureCallback);
    }
    
    public interface IRequestWithResponse<TResponseArgs>
    {
        void SetHandler(IRequestHandlerWithResponse<TResponseArgs> requestHandler);
        void ClearHandler();
        
        void Raise(SuccessCallback<TResponseArgs> successCallback, FailureCallback failureCallback);
        void RaiseSilently(SuccessCallback<TResponseArgs> successCallback, FailureCallback failureCallback);
    }
    
    public interface IRequestWithArgsAndResponse<TRequestArgs, TResponseArgs>
    {
        void SetHandler(IRequestHandlerWithArgsAndResponse<TRequestArgs, TResponseArgs> requestHandler);
        void ClearHandler();
        
        void Raise(TRequestArgs requestArgs, SuccessCallback<TResponseArgs> successCallback, FailureCallback failureCallback);
        void RaiseSilently(TRequestArgs requestArgs, SuccessCallback<TResponseArgs> successCallback, FailureCallback failureCallback);
    }
    
    public struct EmptyArgs { }
}