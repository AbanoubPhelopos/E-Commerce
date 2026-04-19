using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Ordering.Api.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {

    }
}