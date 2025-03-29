﻿using SalesSystem.Register.Application.Commands.Authentication.Delete;
using SalesSystem.Register.Application.Commands.Authentication.Register;
using SalesSystem.Register.Application.Commands.Authentication.ResetPassword;
using SalesSystem.Register.Application.Commands.Authentication.SignIn;
using SalesSystem.Register.Application.DTOs;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Register.Application.Services
{
    public interface IAuthenticationService
    {
        Task<Response<RegisterUserResponse>> RegisterAsync(RegisterUserCommand command);

        Task<Response<SignInUserResponse>> SignInAsync(SignInUserCommand command);

        Task<Response<ResetPasswordUserResponse>> ResetPasswordAsync(ResetPasswordUserCommand command);

        Task<Response<UserDTO>> FindByUserEmailAsync(string email);

        Task<Response<UserDTO>> CheckPasswordAsync(UserDTO userDTO, string password);

        Task<Response<DeleteUserResponse>> DeleteAsync(DeleteUserCommand command);

        Task<Response<string>> GeneratePasswordResetTokenAsync(UserDTO userDTO);
    }
}