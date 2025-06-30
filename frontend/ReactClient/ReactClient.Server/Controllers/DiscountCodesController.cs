using Microsoft.AspNetCore.Mvc;
using DiscountCodeGeneratorClient; // The generated C# namespace from your proto

namespace ReactClient.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountCodesController : ControllerBase
    {
        private readonly Discount.DiscountClient _grpcClient;

        public DiscountCodesController(Discount.DiscountClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        // GET api/discountcodes/generate?length=7&count=10
        [HttpGet("generate")]
        public async Task<IActionResult> Generate(int length, int amount)
        {
            var request = new GenerateRequest
            {
                Length = (uint)length,
                Count = (uint)amount
            };

            var response = await _grpcClient.GenerateCodesAsync(request);

            if (!response.Result)
                return BadRequest("Failed to generate codes");

            return Ok(response.Codes);
        }

        // POST api/discountcodes/use
        [HttpPost("use")]
        public async Task<IActionResult> Use([FromBody] UseCodeRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Code))
                return BadRequest("Code is required");

            var grpcResponse = await _grpcClient.UseCodeAsync(req);

            // Assuming result == 0 means success, others failure
            bool success = grpcResponse.Result == 1;

            return Ok(new
            {
                Success = success,
                ResultCode = grpcResponse.Result
            });
        }
    }
}
