using AutoMapper;
using MediatR;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers.Queries;

public class GetOrderListQueryHandler(IOrderRepository orderRepository, IMapper mapper) : IRequestHandler<GetOrderListQuery, List<OrderResponse>>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IMapper _mapper = mapper;
    
    public async Task<List<OrderResponse>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetOrdersByCustomerNameAsync(request.UserName, cancellationToken);
        var orderResponses = _mapper.Map<List<OrderResponse>>(orders);
        return orderResponses;
    }
}