using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DiscountCodeGeneratorClient;

namespace ReactClient.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountCodesController : ControllerBase
    {
        //private readonly Discount.DiscountClient client;

        //private readonly DiscountCodeGenerator.DiscountCodeGeneratorClient _grpcClient;
        private readonly Discount.DiscountClient _grpcClient;

        public DiscountCodesController(Discount.DiscountClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        [HttpGet("generate")]
        public async Task<IActionResult> Generate(int length, int amount)
        {
            //using var channel = GrpcChannel.ForAddress("https://localhost:7034");
            //var client = new Discount.DiscountClient(channel);
            //var result = await client.UseCodeAsync(new UseCodeRequest { Code = "1234567" });

            var request = new UseCodeRequest { Code = "1234567" };

            var response = await _grpcClient.UseCodeAsync(request);

            //var request = new GenerateCodesRequest { Length = length, Amount = amount };
            //var response = await _grpcClient.GenerateCodesAsync(request);
            return Ok();
            //throw new NotImplementedException("gRPC client is not implemented yet.");
        }

        //[HttpPost("use")]
        //public async Task<IActionResult> Use([FromBody] UseCodeRequest req)
        //{
        //    throw new NotImplementedException("gRPC client is not implemented yet.");
        //    //var grpcReq = new UseCodeGrpcRequest { Code = req.Code };
        //    //var grpcResp = await _grpcClient.UseCodeAsync(grpcReq);
        //    //return Ok(new { grpcResp.Status, grpcResp.Message });
        //}
    }
}
