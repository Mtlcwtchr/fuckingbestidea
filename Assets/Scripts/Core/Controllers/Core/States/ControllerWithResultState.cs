namespace Core.Controllers.Core.States
{
    internal enum ControllerWithResultState
    {
        None,
        WaitFlowAsync,
        WaitForResultAsync,
        Completed,
        Failed
    }
}
