using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCodeGenerator.Services.Services.Abstractions
{
    public interface ICodeGenerator
    {
        string GenerateUniqueCode(int length);
    }
}
