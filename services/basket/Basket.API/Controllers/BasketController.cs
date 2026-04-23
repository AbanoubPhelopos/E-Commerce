using AutoMapper;
using Basket.Application.Commands;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    public class BasketController(IMediator mediator, IPublishEndpoint publishEndpoint, IMapper mapper, ILogger<BasketController> logger) : BaseApiController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<BasketController> _logger = logger;

        [HttpGet("{username}")]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBasket(string username)
        {
            _logger.LogInformation("Getting basket for user {UserName}", username);
            var query = new GetBasketByUserNameQuery(username);
            var basket = await _mediator.Send(query);
            return Ok(basket);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<ActionResult<ShoppingCartResponse>> CreateBasket([FromBody] CreateShoppingCartCommand command)
        {
            _logger.LogInformation("Creating basket for user {UserName}", command.UserName);
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpDelete("{username}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            _logger.LogInformation("Deleting basket for user {UserName}", username);
            var command = new DeleteShoppingCartCommand(username);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("checkout")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            _logger.LogInformation("Checkout basket for user {UserName}", basketCheckout.UserName);
            var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
            var basket = await _mediator.Send(query);
            if (basket == null)
            {
                _logger.LogWarning("Basket not found for user {UserName} during checkout", basketCheckout.UserName);
                return BadRequest();
            }
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;
            await _publishEndpoint.Publish(eventMessage);
            _logger.LogInformation("BasketCheckoutEvent published for user {UserName} with total price {TotalPrice}", basketCheckout.UserName, eventMessage.TotalPrice);

            await _mediator.Send(new DeleteShoppingCartCommand(basketCheckout.UserName));
            return Accepted();
        }

    }
}