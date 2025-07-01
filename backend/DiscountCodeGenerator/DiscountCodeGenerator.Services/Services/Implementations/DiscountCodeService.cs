using DiscountCodeGenerator.Db;
using DiscountCodeGenerator.Services.Services.Abstractions;
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

        public async Task<List<string>> GenerateCodesAsync(uint count, uint length)
        {
            if (count <= 0 || length <= 0 || count > 2000)
                throw new ArgumentException("Invalid count or length");
            try
            {
                var finalCodes = new HashSet<string>(StringComparer.Ordinal);

                while (finalCodes.Count < count)
                {
                    var remaining = count - finalCodes.Count;
                    var generatedSet = new HashSet<string>(StringComparer.Ordinal);

                    while (generatedSet.Count < remaining)
                    {
                        var code = _codeGenerator.GenerateUniqueCode(length);
                        generatedSet.Add(code);
                    }

                    var existingCodes = await _db.DiscountCodes
                        .AsNoTracking()
                        .Where(c => generatedSet.Contains(c.Code))
                        .Select(c => c.Code)
                        .ToListAsync();

                    var uniqueCodes = generatedSet.Except(existingCodes, StringComparer.Ordinal);

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

                await _db.DiscountCodes.AddRangeAsync(entities);
                await _db.SaveChangesAsync();

                return finalCodes.ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error generating discount codes");
                throw;
            }
        }

        public async Task<bool> UseCodeAsync(string code)
        {
            try
            {
                var discountCode = await _db.DiscountCodes.FirstOrDefaultAsync(c => c.Code == code);
                if (discountCode == null)
                {
                    Log.Warning($"Code {code} doesn't exist");
                    return false;
                }

                if (discountCode.IsUsed)
                {
                    Log.Warning($"Code {code} was already used");
                    return false;
                }

                discountCode.UpdatedAt = DateTime.UtcNow;
                discountCode.IsUsed = true;
                await _db.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                Log.Error($"While using code concurrency execption occured for {code}");

                return false;
            }
        }
    }

}
