using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GraphQL;
using Catalogue.API.Graphql;
using GraphQL.Types;

namespace Catalogue.API.Controllers
{
    [Route("graphql")]
    [ApiController]
    public class GraphqlController : ControllerBase
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;

        public GraphqlController(ISchema schema, IDocumentExecuter documentExecuter)
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GraphQLQuery query)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            var inputs = query.Variables.ToInputs();
            var executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = query.Query,
                Inputs = inputs,
                //UserContext = new GraphQLUserContext
                //{
                //    User = User
                //},
                //ValidationRules = validationRules,
#if (DEBUG)
                ExposeExceptions = true,
                EnableMetrics = true,
#endif
            };

            var result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

            if (result.Errors?.Count > 0)
            {
                return BadRequest(result);
            }

            return Ok(result);
            /* var schema = new VehicleSchema();
             var inputs = query.Variables.ToInputs();

             var result = await new DocumentExecuter().ExecuteAsync(_ =>
             {
                 _.Schema = schema.GraphQLSchema;
                 _.Query = query.Query;
                 _.OperationName = query.OperationName;
                 _.Inputs = inputs;
             });

             if (result.Errors?.Count > 0)
             {
                 return BadRequest();
             }

             return Ok(result);
             */
        }
    }
}