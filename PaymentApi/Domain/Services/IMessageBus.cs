using PaymentApi.CrossCuting.DTOs;

namespace PaymentApi.Domain.Services;

public interface IMessageBus
{
    void Publish(PaymentMessage message);
}