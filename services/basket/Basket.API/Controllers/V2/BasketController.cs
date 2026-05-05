using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version}/[controller]")]
    public class BasketController : BaseApiController
    {

        [HttpPost("checkout-v2")]
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