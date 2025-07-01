using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCodeGenerator.Services.Models
{
    public class Result<T, TStatusCode>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public string? ErrorMessage { get; }

        public TStatusCode StatusCode { get; }

        public Result(T? value, bool isSuccess, TStatusCode statusCode, string? errorMessage = null)
        {
            Value = value;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }
    }
}
