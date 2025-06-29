using DiscountCodeGenerator.Db;
using Grpc.Core;

namespace DiscountCodeGenerator.API.Services
{
    public class DiscountService : Discount.DiscountBase
    {
        private readonly ILogger<DiscountService> _logger;
        private readonly DiscountCodeContext _context;

        public DiscountService(DiscountCodeContext context, ILogger<DiscountService> logger)
        {
            _context = context;
        }

        public override Task<GenerateResponse> GenerateCodes(GenerateRequest request, ServerCallContext context)
        {
            // Logic will be implemented here
            return Task.FromResult(new GenerateResponse { Result = false });
        }

        public override Task<UseCodeResponse> UseCode(UseCodeRequest request, ServerCallContext context)
        {
            // Logic will be implemented here
            return Task.FromResult(new UseCodeResponse { Result = 1 });
        }
    }
}
