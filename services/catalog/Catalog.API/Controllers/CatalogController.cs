using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    public class CatalogController : BaseApiController
    {
        private readonly IMediator _mediator;
        public CatalogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(IList<ProductResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductById()
        {
            var query = new GetAllProductsQuery();
            var product = await _mediator.Send(query);
            return product != null ? Ok(product) : NotFound();
        }


        [HttpGet("[Action]/{id:string}")]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductById(string id)
        {
            var query = new GetProductByIdQuery(id);
            var product = await _mediator.Send(query);
            return product != null ? Ok(product) : NotFound();
        }

        [HttpGet("[Action]/{brandName:string}")]
        [ProducesResponseType(typeof(IList<ProductResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductsByBrandName(string brandName)
        {
            var query = new GetProductsByBrandNameQuery(brandName);
            var product = await _mediator.Send(query);
            return product != null ? Ok(product) : NotFound();
        }

        [HttpGet("[Action]/{name:string}")]
        [ProducesResponseType(typeof(IList<ProductResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductsByName(string name)
        {
            var query = new GetProductsByNameQuery(name);
            var product = await _mediator.Send(query);
            return product != null ? Ok(product) : NotFound();
        }

        

        [HttpDelete("[Action]/{id:string}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var Command = new DeleteProductCommand(id);
            var isDeleted = await _mediator.Send(Command);
            return isDeleted ? Ok() : NotFound();
        }

        
    }
}