using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReactClient.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountCodesController : ControllerBase
    {
        //private readonly DiscountCodeGenerator.DiscountCodeGeneratorClient _grpcClient;

        public DiscountCodesController()
        {
           // _grpcClient = grpcClient;
        }

        [HttpGet("generate")]
        public async Task<IActionResult> Generate(int length, int amount)
        {
            //var request = new GenerateCodesRequest { Length = length, Amount = amount };
            //var response = await _grpcClient.GenerateCodesAsync(request);
            //return Ok(response.Codes);
            throw new NotImplementedException("gRPC client is not implemented yet.");
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
