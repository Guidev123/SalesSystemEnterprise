﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SalesSystem.Registers.Application.Commands.Authentication.SignIn;
using SalesSystem.Registers.Application.DTOs;
using SalesSystem.Registers.Application.Services;
using SalesSystem.Registers.Infrastructure.Models;
using SalesSystem.SharedKernel.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SalesSystem.Registers.Infrastructure.Services
{
    public sealed class JwtGeneratorService(UserManager<User> userManager,
                                            IOptions<JwtExtension> appSettings)
                                          : IJwtGeneratorService
    {
        private readonly JwtExtension appSettings = appSettings.Value;

        public async Task<SignInUserResponse> JwtGenerator(string email)
        {
            var user = await userManager.FindByEmailAsync(email) ?? new();
            var claims = await userManager.GetClaimsAsync(user);

            var identityClaims = await GetUserClaimsAsync(claims, user);
            var encodedToken = EncodeToken(identityClaims);

            return GetTokenResponse(encodedToken, user, claims);
        }

        private async Task<ClaimsIdentity> GetUserClaimsAsync(ICollection<Claim> claims, User user)
        {
            var userRoles = await userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return identityClaims;
        }

        private string EncodeToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret ?? string.Empty);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = appSettings.Issuer,
                Audience = appSettings.ValidAt,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(appSettings.ExpiresAt),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }

        private SignInUserResponse GetTokenResponse(string encodedToken, User user, IEnumerable<Claim> claims)
        {
            return new SignInUserResponse(encodedToken, new UserTokenDto(user.Id, user.Email ?? string.Empty,
                claims.Select(c => new UserClaimDto(c.Type, c.Value))),
                TimeSpan.FromHours(appSettings.ExpiresAt).TotalSeconds);
        }

        private static long ToUnixEpochDate(DateTime date)
           => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}