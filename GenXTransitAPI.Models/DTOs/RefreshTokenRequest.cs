using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.Models.DTOs
{
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenResponse
    {
        public int UserId { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
