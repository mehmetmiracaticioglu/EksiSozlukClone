using EksiSozlukClone.Common.Models.RequestModels;
using EksiSozlukClone.Common.Models.RequestModels.Core.Application.Features.Commands.User.Create;
using EksiSozlukClone.Core.Application.Features.Commands.User.Create;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EksiSozlukClone.Api.WebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
                this.mediator = mediator;   
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var res = await mediator.Send(command);
            return Ok(res);
        }


        [HttpPost]
        [Route("update") ]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        {
            var res = await mediator.Send(command);
            return Ok(res);
        }


    }
}
