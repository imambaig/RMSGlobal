using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Identity.Domain;
using Identity.Application.DirectSales;
using Microsoft.AspNetCore.Authorization;

namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectSalesController : BaseController
    {        

        [HttpGet]
        
        public async Task<ActionResult<List<DirectSale>>> List()
        {
            return await Mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<DirectSale>> Get(Guid id)
        {
            return await Mediator.Send(new Details.Query { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command){
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(Guid id, Edit.Command command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await Mediator.Send(new Delete.Command { Id=id});
        }
    }
}