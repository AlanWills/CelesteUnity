namespace Celeste.Requests
{
    public interface IRequestHandler
    {
        void OnRequestMade(SuccessCallback successCallback, FailureCallback failureCallback);
    }
    
    public interface IRequestHandlerWithArgs<in TRequestArgs>
    {
        void OnRequestMade(TRequestArgs requestArgs, SuccessCallback successCallback, FailureCallback failureCallback);
    }
    
    public interface IRequestHandlerWithResponse<out TResponseArgs>
    {
        void OnRequestMade(SuccessCallback<TResponseArgs> successCallback, FailureCallback failureCallback);
    }
    
    public interface IRequestHandlerWithArgsAndResponse<in TRequestArgs, out TResponseArgs>
    {
        void OnRequestMade(TRequestArgs requestArgs, SuccessCallback<TResponseArgs> successCallback, FailureCallback failureCallback);
    }
}