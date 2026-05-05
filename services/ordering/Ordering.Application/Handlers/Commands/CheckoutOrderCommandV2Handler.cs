namespace Ordering.Application.Handlers.Commands;

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

public class CheckoutOrderCommandV2Handler(IOrderRepository orderRepository, IMapper mapper, ILogger<CheckoutOrderCommandV2Handler> logger)
        : IRequestHandler<CheckoutOrderCommandV2, int>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CheckoutOrderCommandV2Handler> _logger = logger;
    public async Task<int> Handle(CheckoutOrderCommandV2 request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Checking out order for {OrderName}", request.OrderName);
        var order = _mapper.Map<Order>(request);
        var generatedOrder = await _orderRepository.AddAsync(order, cancellationToken);
        _logger.LogInformation("Order {OrderId} created for {OrderName}", generatedOrder.Id, request.OrderName);
        return generatedOrder.Id;
    }
}