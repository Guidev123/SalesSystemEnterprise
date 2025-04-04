﻿using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Catalog.Application.Commands.Products.Create
{
    public record CreateProductCommand(string Name, string Description, string Image,
        decimal Price, int QuantityInStock, decimal Height,
        decimal Width, decimal Depth, Guid CategoryId) : Command<CreateProductResponse>
    {
        public override bool IsValid()
        {
            SetValidationResult(new CreateProductValidation().Validate(this));
            return ValidationResult!.IsValid;
        }
    }
}