using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.Application.Queries;
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

        [HttpGet("{action}/{username}")]
        public async Task<IActionResult> GetBasket(string username)
        {
            var query = new GetBasketByUserNameQuery(username);
            var basket = await _mediator.Send(query);
            return Ok(basket);
        }
    }
}