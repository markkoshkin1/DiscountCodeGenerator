using DiscountCodeGenerator.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCodeGenerator.Services.Services.Abstractions
{
    public interface IDiscountCodeService
    {
        /// <summary>
        /// Generates a specified number of unique discount codes of a given length.
        /// </summary>
        /// <param name="count">Amount of codes to generate</param>
        /// <param name="length">Lenght of code</param>
        /// <returns>List of code</returns>
        Task<Result<List<string>, DiscountStatusCodes>> GenerateCodesAsync(uint count, uint length);

        /// <summary>
        /// Uses a discount code. If the code exists and is valid, it will be marked as used.
        /// </summary>
        /// <param name="code"></param>
        /// <returns>Status code</returns>
        Task<Result<bool, DiscountStatusCodes>> UseCodeAsync(string code);
    }
}
