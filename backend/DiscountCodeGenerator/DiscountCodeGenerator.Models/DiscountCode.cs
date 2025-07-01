using System.ComponentModel.DataAnnotations;

namespace DiscountCodeGenerator.Models
{
    public class DiscountCode
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();

        /// <summary>
        /// The generated discount code (7-8 characters).
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if the code has been used.
        /// </summary>
        public bool IsUsed { get; set; } = false;

        /// <summary>
        /// Date and time when the code was generated.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date and time when the code was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Row version for optimistic concurrency control.
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
