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
        var orderToDelete = await _orderRepository.GetByIdAsync(request.Id,cancellationToken);

        if (orderToDelete == null)
        {
            throw new OrderNotFoundException(nameof(Order),request.Id);
        }
        
        await _orderRepository.DeleteAsync(orderToDelete, cancellationToken);
        _logger.LogInformation($"Order {request.Id} deleted");
        return Unit.Value;
    }
}