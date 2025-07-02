using DiscountCodeGenerator.Db;
using DiscountCodeGenerator.Services.Services.Abstractions;
using Serilog;
using DiscountCodeGenerator.Models;
using Microsoft.EntityFrameworkCore;
using DiscountCodeGenerator.Services.Models;
using DiscountCodeGenerator.Services.Utilz;

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

        public async Task<Result<List<string>, DiscountStatusCodes>> GenerateCodesAsync(uint count, uint length)
        {
            if (count <= 0 || length <= 0 || count > 2000)
            {
                Log.Warning("Invalid request to generate codes: count={Count}, length={Length}", count, length);
                return new Result<List<string>, DiscountStatusCodes>(
                        null,
                        false,
                        DiscountStatusCodes.InvalidRequest,
                        ErrorMessages.InvaliGenerateCodedRequest);
            }

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

                    if (existingCodes.Any() == true)
                    {
                        Log.Warning($"Collision with db detected for {existingCodes.Count()} codes");
                    }

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
                Log.Information($"Generated {finalCodes.Count} unique discount codes of length {length}");

                var result = new Result<List<string>, DiscountStatusCodes>(
                    finalCodes.ToList(),
                    true,
                    DiscountStatusCodes.Ok);
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error generating discount codes");
                return new Result<List<string>, DiscountStatusCodes>(
                    null,
                    false,
                    DiscountStatusCodes.ServerError,
                    ErrorMessages.ServerError);
            }
        }

        public async Task<Result<bool, DiscountStatusCodes>> UseCodeAsync(string code)
        {
            try
            {
                var discountCode = await _db.DiscountCodes.FirstOrDefaultAsync(c => c.Code == code);
                if (discountCode == null)
                {
                    Log.Warning($"Code {code} doesn't exist");
                    return new Result<bool, DiscountStatusCodes>
                    (
                        false,
                        false,
                        DiscountStatusCodes.CodeNotFound,
                        ErrorMessages.CodeNotFound
                    );
                }

                if (discountCode.IsUsed)
                {
                    Log.Warning($"Code {code} was already used");
                    return new Result<bool, DiscountStatusCodes>
                     (
                         false,
                         false,
                         DiscountStatusCodes.CodeAlreadyUsed,
                         ErrorMessages.CodeAlreadyUsed
                     );
                }

                discountCode.UpdatedAt = DateTime.UtcNow;
                discountCode.IsUsed = true;
                await _db.SaveChangesAsync();
                Log.Information($"Code {code} was successfully used");

                return new Result<bool, DiscountStatusCodes>
                      (
                          true,
                          true,
                          DiscountStatusCodes.Ok
                      );
            }
            catch (DbUpdateConcurrencyException)
            {
                Log.Error($"While using code concurrency execption occured for {code}");

                return new Result<bool, DiscountStatusCodes>
                     (
                         false,
                         false,
                         DiscountStatusCodes.CodeAlreadyUsed,
                         ErrorMessages.CodeAlreadyUsed
                     );
            }
        }
    }

}
