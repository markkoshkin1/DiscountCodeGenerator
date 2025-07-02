using DiscountCodeGenerator.Services.Services.Abstractions;
using Grpc.Core;
using Serilog;

namespace DiscountCodeGenerator.API.Services
{
    public class DiscountService : Discount.DiscountBase
    {
        private readonly IDiscountCodeService _discountCodeService;

        public DiscountService(IDiscountCodeService discountCodeService)
        {
            _discountCodeService = discountCodeService;
        }

        public override async Task<GenerateResponse> GenerateCodes(GenerateRequest request, ServerCallContext context)
        {
            Log.Information("Generating {Count} codes of length {Length}", request.Count, request.Length);
            var result = await _discountCodeService.GenerateCodesAsync(request.Count, request.Length);
            var response = new GenerateResponse();
            if (result.IsSuccess)
            {
                response.Codes.AddRange(result.Value);
                response.Result = true;

                return response;
            }

            response.Result = false;
            return response;

        }

        public override async Task<UseCodeResponse> UseCode(UseCodeRequest request, ServerCallContext context)
        {
            Log.Information("Using discount code: {Code}", request.Code);
            var result = await _discountCodeService.UseCodeAsync(request.Code);
            var response = new UseCodeResponse();

            if (result.IsSuccess)
            {
                response.Result = (uint)StatusCode.OK;
                return response;
            }

            switch (result.StatusCode)
            {
                case DiscountCodeGenerator.Services.Models.DiscountStatusCodes.CodeNotFound:
                    response.Result = (uint)StatusCode.NotFound;
                    break;
                case DiscountCodeGenerator.Services.Models.DiscountStatusCodes.CodeAlreadyUsed:
                case DiscountCodeGenerator.Services.Models.DiscountStatusCodes.InvalidCodeFormat:
                    response.Result = (uint)StatusCode.InvalidArgument;
                    break;
                default:
                    response.Result = (uint)StatusCode.Internal;
                    break;
            }

            return response;
        }
    }
}
