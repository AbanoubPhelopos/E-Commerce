using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers.Commands;

public class CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<CheckoutOrderCommandHandler> logger) : IRequestHandler<CheckoutOrderCommand, int>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CheckoutOrderCommandHandler> _logger = logger;
    public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Checking out order for user {UserName}", request.UserName);
        var order = _mapper.Map<Order>(request);
        var generatedOrder = await _orderRepository.AddAsync(order, cancellationToken);
        _logger.LogInformation("Order {OrderId} created for user {UserName}", generatedOrder.Id, request.UserName);
        return generatedOrder.Id;
    }
}