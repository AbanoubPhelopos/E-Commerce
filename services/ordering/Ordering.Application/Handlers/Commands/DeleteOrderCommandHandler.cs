using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptions;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers.Commands;

public class DeleteOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<DeleteOrderCommandHandler> logger): IRequestHandler<DeleteOrderCommand,Unit>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<DeleteOrderCommandHandler> _logger = logger;
    
    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting order {OrderId}", request.Id);
        var orderToDelete = await _orderRepository.GetByIdAsync(request.Id,cancellationToken);

        if (orderToDelete == null)
        {
            _logger.LogWarning("Order {OrderId} not found for deletion", request.Id);
            throw new OrderNotFoundException(nameof(Order),request.Id);
        }
        
        await _orderRepository.DeleteAsync(orderToDelete, cancellationToken);
        _logger.LogInformation("Order {OrderId} deleted successfully", request.Id);
        return Unit.Value;
    }
}