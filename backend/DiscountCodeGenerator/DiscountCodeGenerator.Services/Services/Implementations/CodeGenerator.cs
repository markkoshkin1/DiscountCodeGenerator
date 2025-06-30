using DiscountCodeGenerator.Services.Services.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCodeGenerator.Services.Services.Implementations
{

    //TODO: Replace memory cache with distrubuted cache if you want to use more than one instance of the service
    public class CodeGenerator : ICodeGenerator
    {
        private static readonly char[] _chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToCharArray();
        private readonly int _charSetLength = _chars.Length;
        private readonly IMemoryCache _memoryCache;

        public CodeGenerator(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public string GenerateUniqueCode(uint length)
        {
            string code;
            do
            {
                code = Generate(length);
            } while (CodeExists(code));

            CacheCode(code);
            return code;
        }

        private string Generate(uint length)
        {
            if (length <= 0)
                throw new ArgumentException("Length must be positive", nameof(length));

            var data = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(data);

            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = _chars[data[i] % _charSetLength];
            }

            return new string(result);
        }

        private bool CodeExists(string code)
        {
            var isExist = _memoryCache.TryGetValue(code, out _);
            if (isExist)
            {
                Log.Warning("Code {Code} already exists in cache", code);
            }   

            return isExist;
        }

        private void CacheCode(string code)
        {
            _memoryCache.Set(code, true, TimeSpan.FromMinutes(5));
        }
    }
}
