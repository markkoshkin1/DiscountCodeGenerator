using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCodeGenerator.Services.Services.Abstractions
{
    public interface IDiscountCodeService
    {
        Task<List<string>> GenerateCodesAsync(int count, int length);
        Task<bool> UseCodeAsync(string code);
    }
}
