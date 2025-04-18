﻿using SalesSystem.Payments.Domain.Enums;
using SalesSystem.SharedKernel.DomainObjects;

namespace SalesSystem.Payments.Domain.Entities
{
    public class Payment : Entity, IAggregateRoot
    {
        public Payment(Guid orderId, Guid customerId, decimal amount)
        {
            OrderId = orderId;
            CustomerId = customerId;
            Amount = amount;
            Status = nameof(ETransactionStatus.WaitingPayment);
            Validate();
        }

        public Guid OrderId { get; private set; }
        public Guid CustomerId { get; private set; }
        public string Status { get; private set; }
        public decimal Amount { get; private set; }
        public Transaction? Transaction { get; private set; }

        public void AddTransaction(Transaction transaction)
        {
            Transaction = transaction;
        }

        public void SetAsPaid(string status, string externalReference)
        {
            Status = status;
            Transaction?.SetAsPaid(externalReference);
        }

        public void SetAsFailed() => Status = nameof(ETransactionStatus.Failed);

        public override void Validate()
        {
            AssertionConcern.EnsureGreaterThan(Amount, 0, "Amount must be greater than 0.");
            AssertionConcern.EnsureDifferent(OrderId, Guid.Empty, "Order id cannot be empty.");
        }
    }
}