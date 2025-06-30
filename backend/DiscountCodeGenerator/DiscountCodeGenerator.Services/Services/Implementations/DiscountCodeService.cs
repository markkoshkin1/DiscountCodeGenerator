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
            if (count <= 0 || length <= 0 || count > 2000)
                throw new ArgumentException("Invalid count or length");
            try
            {
                var finalCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var allGenerated = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                while (finalCodes.Count < count)
                {
                    var remaining = count - finalCodes.Count;
                    var generatedSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    while (generatedSet.Count < remaining)
                    {
                        var code = _codeGenerator.Generate(length);
                        if (allGenerated.Add(code))
                        {
                            generatedSet.Add(code);
                        }
                    }

                    var generatedList = generatedSet.ToList();

                    var existingCodes = await _db.DiscountCodes
                        .AsNoTracking()
                        .Where(c => generatedList.Contains(c.Code))
                        .Select(c => c.Code)
                        .ToListAsync();

                    var uniqueCodes = generatedList.Except(existingCodes, StringComparer.OrdinalIgnoreCase);

                    foreach (var code in uniqueCodes)
                    {
                        finalCodes.Add(code);
                    }
                }

                var entities = finalCodes.Select(c => new DiscountCode
                {
                    Code = c,
                    CreatedAt = DateTime.UtcNow
                }).ToList();

                _db.DiscountCodes.AddRange(entities);
                await _db.SaveChangesAsync();

                return finalCodes.ToList();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error generating discount codes");
                throw;
            }
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
