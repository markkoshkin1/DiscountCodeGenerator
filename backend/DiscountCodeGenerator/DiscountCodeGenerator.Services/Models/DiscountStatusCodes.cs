using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCodeGenerator.Services.Models
{
    public enum DiscountStatusCodes
    {
        Ok = 1,
        InvalidRequest = 2,
        CodeNotFound = 3,
        CodeAlreadyUsed = 4,
        ServerError = 5,
    }
}
