using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptions;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers.Commands;

public class UpdateOrderCommandHandler (IOrderRepository orderRepository, IMapper mapper, ILogger<CheckoutOrderCommandHandler> logger):IRequestHandler<UpdateOrderCommand,Unit>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CheckoutOrderCommandHandler> _logger = logger;
    
    public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _orderRepository.GetByIdAsync(request.Id,cancellationToken);
        if (orderToUpdate == null)
        {
            throw new OrderNotFoundException(nameof(Order),$"Order with id {request.Id} not found");
        }
        await _orderRepository.UpdateAsync(orderToUpdate, cancellationToken);
        _logger.LogInformation($"Order {request.Id} updated");
        return Unit.Value;
    }
}