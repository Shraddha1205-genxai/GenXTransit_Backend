using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models.DTO_s;
using GenXTransitAPI.Models.DTOs;
using GenXTransitAPI.Models.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GenXTransitAPI.DataAccess.Security
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _settings;

        private readonly SymmetricSecurityKey _signingKey;

        private readonly TokenValidationParameters
            _refreshValidation;

        public JwtService(
            IOptions<JwtSettings> options)
        {
            _settings = options.Value;

            if (string.IsNullOrWhiteSpace(_settings.Key))
            {
                throw new InvalidOperationException(
                    "JWT Key is missing from configuration.");
            }

            _signingKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _settings.Key));

            // Validation settings used specifically
            // for Refresh Token validation.
            _refreshValidation =
                new TokenValidationParameters
                {
                    ValidateIssuer = true,

                    ValidateAudience = true,

                    ValidateIssuerSigningKey = true,

                    ValidateLifetime = true,

                    ValidIssuer =
                        _settings.Issuer,

                    ValidAudience =
                        _settings.Audience,

                    IssuerSigningKey =
                        _signingKey,

                    ClockSkew =
                        TimeSpan.FromSeconds(30)
                };
        }


        // =====================================================
        // ACCESS TOKEN
        // =====================================================

        public string GenerateAccessToken(User user)
        {
            var now = DateTime.UtcNow;

            var expires =
                now.AddMinutes(
                    _settings.AccessTokenExpiryMinutes);

            var claims = new List<Claim>
            {
                new Claim(
                    JwtRegisteredClaimNames.Sub,
                    user.UserId.ToString()),

                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.UserId.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    user.UserName ?? string.Empty),

                new Claim(
                    ClaimTypes.Email,
                    user.Email ?? string.Empty),

                new Claim(
                    ClaimTypes.Role,
                    user.RoleId.ToString()),

                new Claim(
                    "IsFirstLogin",
                    user.IsFirstLogin.ToString()),

                new Claim(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
            };

            var credentials =
                new SigningCredentials(
                    _signingKey,
                    SecurityAlgorithms.HmacSha512);

            var token =
                new JwtSecurityToken(
                    issuer: _settings.Issuer,
                    audience: _settings.Audience,
                    claims: claims,
                    notBefore: now,
                    expires: expires,
                    signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }


        // =====================================================
        // REFRESH TOKEN
        // =====================================================

        public string GenerateRefreshToken(User user)
        {
            var now = DateTime.UtcNow;

            var expires =
                now.AddDays(
                    _settings.RefreshTokenExpiryDays);

            var claims = new List<Claim>
            {
                // User ID
                new Claim(
                    JwtRegisteredClaimNames.Sub,
                    user.UserId.ToString()),

                // User ID for ASP.NET Core
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.UserId.ToString()),

                // Unique token ID
                new Claim(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString()),

                // Identify this JWT as a refresh token
                new Claim(
                    "typ",
                    "refresh")
            };

            var credentials =
                new SigningCredentials(
                    _signingKey,
                    SecurityAlgorithms.HmacSha512);

            var token =
                new JwtSecurityToken(
                    issuer: _settings.Issuer,
                    audience: _settings.Audience,
                    claims: claims,
                    notBefore: now,
                    expires: expires,
                    signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }


        // =====================================================
        // VALIDATE REFRESH TOKEN
        // =====================================================

        public ClaimsPrincipal? ValidateRefreshToken(
            string refreshToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(
                    refreshToken))
                {
                    return null;
                }

                var handler =
                    new JwtSecurityTokenHandler();

                var principal =
                    handler.ValidateToken(
                        refreshToken,
                        _refreshValidation,
                        out var validatedToken);

                // Make sure token is actually a JWT
                if (validatedToken is not JwtSecurityToken jwtToken)
                {
                    return null;
                }

                // Make sure signing algorithm is what we expect
                if (!string.Equals(
                    jwtToken.Header.Alg,
                    SecurityAlgorithms.HmacSha512,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }

                // Make sure this is a refresh token
                var tokenType =
                    principal.FindFirst("typ")?.Value;

                if (tokenType != "refresh")
                {
                    return null;
                }

                // Everything is valid
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}