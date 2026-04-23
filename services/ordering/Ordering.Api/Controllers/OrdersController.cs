using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Queries;
using Ordering.Application.Responses;

namespace Ordering.Api.Controllers
{
    public class OrdersController(IMediator mediator, ILogger<OrdersController> logger) : BaseApiController
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<OrdersController> _logger = logger;

        [HttpGet("{userName}")]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByUserName(string userName)
        {
            _logger.LogInformation("Getting orders for user {UserName}", userName);
            var query = new GetOrderListQuery(userName);
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> CheckoutOrder(CheckoutOrderCommand command)
        {
            _logger.LogInformation("Checking out order for user {UserName}", command.UserName);
            var orderId = await _mediator.Send(command);
            return Ok(orderId);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateOrder(UpdateOrderCommand command)
        {
            _logger.LogInformation("Updating order {OrderId}", command.Id);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            _logger.LogInformation("Deleting order {OrderId}", id);
            var command = new DeleteOrderCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}