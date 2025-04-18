﻿using SalesSystem.SharedKernel.DTOs;

namespace SalesSystem.Catalog.Domain.Interfaces.Services
{
    public interface IStockService : IDisposable
    {
        Task<bool> DebitStockAsync(Guid productId, int quantity);

        Task<bool> DebitListStockAsync(OrderProductsListDto orderProductsList);

        Task<bool> AddStockAsync(Guid productId, int quantity);

        Task<bool> AddListStockAsync(OrderProductsListDto orderProductsList);
    }
}