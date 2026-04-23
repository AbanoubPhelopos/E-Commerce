using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptions;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers.Commands;

public class UpdateOrderCommandHandler (IOrderRepository orderRepository, IMapper mapper, ILogger<UpdateOrderCommandHandler> logger):IRequestHandler<UpdateOrderCommand,Unit>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<UpdateOrderCommandHandler> _logger = logger;
    
    public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating order {OrderId}", request.Id);
        var orderToUpdate = await _orderRepository.GetByIdAsync(request.Id,cancellationToken);
        if (orderToUpdate == null)
        {
            _logger.LogWarning("Order {OrderId} not found for update", request.Id);
            throw new OrderNotFoundException(nameof(Order),$"Order with id {request.Id} not found");
        }
        _mapper.Map(request, orderToUpdate);
        await _orderRepository.UpdateAsync(orderToUpdate, cancellationToken);
        _logger.LogInformation("Order {OrderId} updated successfully", request.Id);
        return Unit.Value;
    }
}