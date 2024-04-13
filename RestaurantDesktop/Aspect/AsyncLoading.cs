using PostSharp.Aspects;
using PostSharp.Serialization;
using RestaurantDesktop.Service;

namespace RestaurantDesktop.Aspect
{
    [PSerializable]
    public class AsyncLoading : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            MessageService.SendLoadingBegin();
            base.OnEntry(args);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            MessageService.SendLoadingEnd();
            base.OnExit(args);
        }
    }
}
