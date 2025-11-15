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
        public async Task<IActionResult> GetProducts()
        {
            var query = new GetAllProductsQuery();
            var product = await _mediator.Send(query);
            return Ok(product);
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(IList<BrandResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBrands()
        {
            var query = new GetAllBrandsQuery();
            var brands = await _mediator.Send(query);
            return Ok(brands);
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(IList<TypesResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTypes()
        {
            var query = new GetAllTypesQuery();
            var types = await _mediator.Send(query);
            return Ok(types);
        }

        [HttpGet("[Action]/{id}")]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductById(string id)
        {
            var query = new GetProductByIdQuery(id);
            var product = await _mediator.Send(query);
            return Ok(product);
        }

        [HttpGet("[Action]/{brandName}")]
        [ProducesResponseType(typeof(IList<ProductResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductsByBrandName(string brandName)
        {
            var query = new GetProductsByBrandNameQuery(brandName);
            var product = await _mediator.Send(query);
            return Ok(product);
        }

        [HttpGet("[Action]/{name}")]
        [ProducesResponseType(typeof(IList<ProductResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductsByName(string name)
        {
            var query = new GetProductsByNameQuery(name);
            var product = await _mediator.Send(query);
            return Ok(product);
        }

        [HttpPost("[Action]")]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            var product = await _mediator.Send(command);
            return Ok(product);
        }

        [HttpPut("[Action]")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand command)
        {
            var isUpdated =  await _mediator.Send(command);
            return Ok(isUpdated);
        }

        [HttpDelete("[Action]/{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var command = new DeleteProductCommand(id);
            var isDeleted = await _mediator.Send(command);
            return Ok(isDeleted);
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(IList<BrandResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBrands()
        {
            var query = new GetAllBrandsQuery();
            var brands = await _mediator.Send(query);
            return Ok(brands);
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(IList<TypesResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTypes()
        {
            var query = new GetAllTypesQuery();
            var types = await _mediator.Send(query);
            return Ok(types);
        }
    }
}