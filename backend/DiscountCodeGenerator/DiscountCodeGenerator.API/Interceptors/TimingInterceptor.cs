using Grpc.Core.Interceptors;
using Grpc.Core;
using System.Diagnostics;
using Serilog;

namespace DiscountCodeGenerator.API.Interceptors
{
    public class TimingInterceptor : Interceptor
    {
        public TimingInterceptor()
        {
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                return await continuation(request, context);
            }
            finally
            {
                stopwatch.Stop();
                Log.Information("gRPC call {Method} took {ElapsedMilliseconds}ms",
                    context.Method, stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
