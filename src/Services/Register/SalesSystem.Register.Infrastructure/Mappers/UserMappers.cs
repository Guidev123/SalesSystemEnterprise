﻿using SalesSystem.Register.Application.Commands.Authentication.Register;
using SalesSystem.Register.Application.Commands.Customers.AddAddress;
using SalesSystem.Register.Domain.Entities;
using SalesSystem.Register.Infrastructure.Models;

namespace SalesSystem.Register.Infrastructure.Mappers
{
    public static class UserMappers
    {
        public static User MapToUser(this RegisterUserCommand command) => new()
        {
            UserName = command.Email,
            Email = command.Email,
            EmailConfirmed = true
        };
    }
}