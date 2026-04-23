using System;
using System.Collections.Generic;
using System.Linq; 
using System.Threading.Tasks;
using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Controllers
{
    public class CatalogController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CatalogController> _logger;
        public CatalogController(IMediator mediator, ILogger<CatalogController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(IList<ProductResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProducts([FromQuery] CatalogSpecParams specParams)
        {
            _logger.LogInformation("Getting products with page {PageIndex}, size {PageSize}", specParams.PageIndex, specParams.PageSize);
            var query = new GetAllProductsQuery(specParams);
            var product = await _mediator.Send(query);
            return Ok(product);
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(IList<BrandResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBrands()
        {
            _logger.LogInformation("Getting all brands");
            var query = new GetAllBrandsQuery();
            var brands = await _mediator.Send(query);
            return Ok(brands);
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(IList<TypesResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTypes()
        {
            _logger.LogInformation("Getting all types");
            var query = new GetAllTypesQuery();
            var types = await _mediator.Send(query);
            return Ok(types);
        }

        [HttpGet("[Action]/{id}")]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductById(string id)
        {
            _logger.LogInformation("Getting product by id {ProductId}", id);
            var query = new GetProductByIdQuery(id);
            var product = await _mediator.Send(query);
            return Ok(product);
        }

        [HttpGet("[Action]/{brandName}")]
        [ProducesResponseType(typeof(IList<ProductResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductsByBrandName(string brandName)
        {
            _logger.LogInformation("Getting products by brand name {BrandName}", brandName);
            var query = new GetProductsByBrandNameQuery(brandName);
            var product = await _mediator.Send(query);
            return Ok(product);
        }

        [HttpGet("[Action]/{name}")]
        [ProducesResponseType(typeof(IList<ProductResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductsByName(string name)
        {
            _logger.LogInformation("Getting products by name {ProductName}", name);
            var query = new GetProductsByNameQuery(name);
            var product = await _mediator.Send(query);
            return Ok(product);
        }

        [HttpPost("[Action]")]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            _logger.LogInformation("Creating product {ProductName}", command.Name);
            var product = await _mediator.Send(command);
            return Ok(product);
        }

        [HttpPut("[Action]")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand command)
        {
            _logger.LogInformation("Updating product {ProductId}", command.Id);
            var isUpdated =  await _mediator.Send(command);
            return Ok(isUpdated);
        }

        [HttpDelete("[Action]/{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            _logger.LogInformation("Deleting product {ProductId}", id);
            var command = new DeleteProductCommand(id);
            var isDeleted = await _mediator.Send(command);
            return Ok(isDeleted);
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(IList<BrandResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBrands()
        {
            _logger.LogInformation("Getting all brands");
            var query = new GetAllBrandsQuery();
            var brands = await _mediator.Send(query);
            return Ok(brands);
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(IList<TypesResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTypes()
        {
            _logger.LogInformation("Getting all types");
            var query = new GetAllTypesQuery();
            var types = await _mediator.Send(query);
            return Ok(types);
        }
    }
}