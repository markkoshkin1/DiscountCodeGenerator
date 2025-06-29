using DiscountCodeGenerator.Db;
using DiscountCodeGenerator.Services.Services.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using DiscountCodeGenerator.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscountCodeGenerator.Services.Services.Implementations
{
    public class DiscountCodeService : IDiscountCodeService
    {
        private readonly DiscountCodeContext _db;
        private readonly ICodeGenerator _codeGenerator;

        public DiscountCodeService(DiscountCodeContext db, ICodeGenerator codeGenerator)
        {
            _db = db;
            _codeGenerator = codeGenerator;
        }

        public async Task<List<string>> GenerateCodesAsync(int count, int length)
        {
            var codes = new List<string>();

            for (int i = 0; i < count; i++)
            {
                var code = _codeGenerator.Generate(length);
                codes.Add(code);
                _db.DiscountCodes.Add(new DiscountCode { Code = code, CreatedAt = DateTime.UtcNow });
            }

            await _db.SaveChangesAsync();
            return codes;
        }

        public async Task<bool> UseCodeAsync(string code)
        {
            var discountCode = await _db.DiscountCodes.FirstOrDefaultAsync(c => c.Code == code);
            if (discountCode == null)
                return false;

            discountCode.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }
    }

}
