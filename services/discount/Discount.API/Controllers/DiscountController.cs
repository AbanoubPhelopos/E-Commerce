using MediatR;
using Microsoft.AspNetCore.Mvc;
using Discount.Application.Queries;
using Discount.Application.Commands;
using Discount.Grpc.Protos;

namespace Discount.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DiscountController> _logger;

        public DiscountController(IMediator mediator, ILogger<DiscountController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{productName}")]
        [ProducesResponseType(typeof(CouponModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDiscount(string productName)
        {
            _logger.LogInformation("Getting discount for product {ProductName}", productName);
            var query = new GetDiscountQuery(productName);
            var coupon = await _mediator.Send(query);
            return Ok(coupon);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CouponModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateDiscount([FromBody] CouponModel coupon)
        {
            _logger.LogInformation("Creating discount for product {ProductName} with amount {Amount}", coupon.ProductName, coupon.Amount);
            var command = new CreateDiscountCommand(coupon.ProductName, coupon.Description, coupon.Amount);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(CouponModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateDiscount([FromBody] CouponModel coupon)
        {
            _logger.LogInformation("Updating discount for product {ProductName} with id {Id}", coupon.ProductName, coupon.Id);
            var command = new UpdateDiscountCommand(coupon.Id, coupon.ProductName, coupon.Description, coupon.Amount);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{productName}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteDiscount(string productName)
        {
            _logger.LogInformation("Deleting discount for product {ProductName}", productName);
            var command = new DeleteDiscountCommand(productName);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
