﻿using Microsoft.AspNetCore.Mvc;
using SalesSystem.API.Configuration;
using SalesSystem.Catalog.Application.Commands.Products.Create;
using SalesSystem.Catalog.Application.Commands.Products.Update;
using SalesSystem.Catalog.Application.Queries.Categories.GetAll;
using SalesSystem.Catalog.Application.Queries.Products.GetAll;
using SalesSystem.Catalog.Application.Queries.Products.GetByCategory;
using SalesSystem.Catalog.Application.Queries.Products.GetById;
using SalesSystem.SharedKernel.Communication.Mediator;

namespace SalesSystem.API.Controllers
{
    [Route("api/v1/catalog")]
    public class CatalogController(IMediatorHandler mediator) : MainController
    {
        [HttpGet]
        public async Task<IResult> GetAllAsync(int pageNumber = ApiConfiguration.DEFAULT_PAGE_NUMBER, int pageSize = ApiConfiguration.DEFAULT_PAGE_SIZE)
            => CustomResponse(await mediator.SendQuery(new GetAllProductsQuery(pageNumber, pageSize)).ConfigureAwait(false));

        [HttpGet("{id:guid}")]
        public async Task<IResult> GetByIdAsync(Guid id)
            => CustomResponse(await mediator.SendQuery(new GetProductByIdQuery(id)).ConfigureAwait(false));

        [HttpGet("{code}")]
        public async Task<IResult> GetAllByCategoryAsync(int code, int pageNumber = ApiConfiguration.DEFAULT_PAGE_NUMBER, int pageSize = ApiConfiguration.DEFAULT_PAGE_SIZE)
            => CustomResponse(await mediator.SendQuery(new GetProductsByCategoryQuery(pageNumber, pageSize, code)).ConfigureAwait(false));

        [HttpGet("category")]
        public async Task<IResult> GetAllCategoriesAsync()
            => CustomResponse(await mediator.SendQuery(new GetAllCategoriesQuery()).ConfigureAwait(false));

        [HttpPost]
        public async Task<IResult> CreateAsync(CreateProductCommand command)
            => CustomResponse(await mediator.SendCommand(command).ConfigureAwait(false));

        [HttpPut]
        public async Task<IResult> UpdateAsync(UpdateProductCommand command)
            => CustomResponse(await mediator.SendCommand(command).ConfigureAwait(false));
    }
}