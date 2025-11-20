using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.Application.Commands;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IMediator _mediator;
        public BasketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]/{username}")]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBasket(string username)
        {
            var query = new GetBasketByUserNameQuery(username);
            var basket = await _mediator.Send(query);
            return Ok(basket);
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<ActionResult<ShoppingCartResponse>> CreateBasket([FromBody] CreateShoppingCartCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        
        [HttpDelete("[action]/{username}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            var command = new DeleteShoppinngCartCommand(username);
            await _mediator.Send(command);
            return Ok();
        }

    }
}