using DiscountCodeGenerator.Services.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCodeGenerator.Services.Services.Implementations
{
    public class CodeGenerator : ICodeGenerator
    {
        private static readonly char[] _chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToCharArray();
        private readonly int _charSetLength = _chars.Length;

        public string Generate(int length)
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
    }
}
