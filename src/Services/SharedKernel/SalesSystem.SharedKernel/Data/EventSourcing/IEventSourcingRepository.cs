﻿using SalesSystem.SharedKernel.Events;

namespace SalesSystem.SharedKernel.Data.EventSourcing
{
    public interface IEventSourcingRepository
    {
        Task SaveAsync<TEvent>(TEvent @event) where TEvent : Event;

        Task<IEnumerable<StoredEvent>> GetAllAsync(Guid aggregateId);
    }
}