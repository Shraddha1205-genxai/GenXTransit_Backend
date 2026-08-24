using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.DataAccess.Security;
using GenXTransitAPI.Models.DTO_s;
using GenXTransitAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepo;
        private readonly IPasswordService _passwordService;
        private readonly IEmailService _emailService;

        public AuthService(
       IAuthRepository authRepo,
       IPasswordService passwordService,
       IEmailService emailService)
        {
            _authRepo = authRepo;
            _passwordService = passwordService;
            _emailService = emailService;
        }

        public async Task<RegisterUserResponse> RegisterAsync(
       RegisterUserRequest request)
        {
            if (await _authRepo.EmailExistsAsync(request.Email))
            {
                throw new Exception("Email already exists.");
            }

            if (await _authRepo.UserNameExistsAsync(request.UserName))
            {
                throw new Exception("Username already exists.");
            }

            var temporaryPassword =
                _passwordService.GenerateTemporaryPassword();

            var passwordHash =
                _passwordService.HashPassword(
                    temporaryPassword);

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                MobileNo = request.MobileNo,

                FirstName = request.FirstName,
                LastName = request.LastName,

                RoleId = request.RoleId,

                PasswordHash = passwordHash,

                IsActive = true,
                IsEmailVerified = false,
                IsMobileVerified = false,

                IsFirstLogin = true,

                PasswordChangedDate = null,

                CreatedDate = DateTime.UtcNow,

                // Replace with current logged-in admin ID
                CreatedBy = 1
            };

            var userId =
                await _authRepo.RegisterUserAsync(user);

            await _emailService.SendUserCreatedEmail(
                request.Email,
                request.UserName,
                temporaryPassword);

            return new RegisterUserResponse
            {
                UserId = userId,
                UserName = request.UserName,
                Email = request.Email,
                Message =
                    "User registered successfully. " +
                    "Login credentials have been sent to the registered email."
            };
        }
    }
}
