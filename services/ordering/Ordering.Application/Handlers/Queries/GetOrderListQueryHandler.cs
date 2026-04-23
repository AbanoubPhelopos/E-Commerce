using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers.Queries;

public class GetOrderListQueryHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<GetOrderListQueryHandler> logger) : IRequestHandler<GetOrderListQuery, List<OrderResponse>>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GetOrderListQueryHandler> _logger = logger;
    
    public async Task<List<OrderResponse>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting orders for user {UserName}", request.UserName);
        var orders = await _orderRepository.GetOrdersByCustomerNameAsync(request.UserName, cancellationToken);
        var orderResponses = _mapper.Map<List<OrderResponse>>(orders);
        _logger.LogInformation("Found {OrderCount} orders for user {UserName}", orderResponses.Count, request.UserName);
        return orderResponses;
    }
}