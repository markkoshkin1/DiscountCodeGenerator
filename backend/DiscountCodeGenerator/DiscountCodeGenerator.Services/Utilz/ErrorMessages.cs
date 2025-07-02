using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCodeGenerator.Services.Utilz
{
    public class ErrorMessages
    {
        public const string InvaliGenerateCodedRequest = "Count and length must be positive 7 or 8 charaters long and count should not exceed 2000.";
        public const string CodeNotFound = "Discount code not found.";
        public const string CodeAlreadyUsed = "Discount code has already been used.";
        public const string ServerError = "An unexpected error occurred. Please try again later.";
        public const string CodeGenerationFailed = "Failed to generate a unique discount code.";
        public const string DatabaseError = "Database operation failed. Please try again later.";
        public const string InvalidCodeFormat = "Invalid code format. Code must be between 7 and 8 characters long.";
    }
}
