using DiscountCodeGenerator.Db;
using DiscountCodeGenerator.Services.Services.Abstractions;
using DiscountCodeGenerator.Services.Services.Implementations;
using Grpc.Core;

namespace DiscountCodeGenerator.API.Services
{
    public class DiscountService : Discount.DiscountBase
    {
        private readonly ILogger<DiscountService> _logger;
        private readonly DiscountCodeContext _context;
        private readonly IDiscountCodeService _discountCodeService;

        public DiscountService(DiscountCodeContext context, IDiscountCodeService discountCodeService)
        {
            _context = context;
            _discountCodeService = discountCodeService;
        }

        public override async Task<GenerateResponse> GenerateCodes(GenerateRequest request, ServerCallContext context)
        {
            // Logic will be implemented here
            var result = await _discountCodeService.GenerateCodesAsync(request.Count, request.Length);

            var response = new GenerateResponse();
            response.Codes.AddRange(result);
            response.Result = true;

            return response;
        }

        public override async Task<UseCodeResponse> UseCode(UseCodeRequest request, ServerCallContext context)
        {
            var result = await _discountCodeService.UseCodeAsync(request.Code);
            var response = new UseCodeResponse
            {
                Result = result ? 1u : 0u
            };
            return response;
        }
    }
}
