using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GenXTransitAPI.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TotalCount { get; set; }

        public static ApiResponse<T> Ok(T data, string message = "Successs", int? total = null)
            => new() { Success = true, Message = message, Data = data, TotalCount = total };

        public static ApiResponse<T> Fail(string message)
            => new() { Success = false, Message = message };
    }
    public class PagedResponse<T>
    {
        public List<T> Items { get; set; } = new();

        public int TotalRecords { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }
    }
}
