using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalogue.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SearchManager;
using SearchManager.Model;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        ISearchManager _iSearchManager;
        public SearchController(ISearchManager iSearchManager)
        {
            _iSearchManager = iSearchManager;
        }

        [HttpPost]
        public async Task<ActionResult> Create(Vehicle vehicle)
        {
            await _iSearchManager.InsertDocuments(new List<Vehicle>() { vehicle });
            return Ok();
        }

        [HttpPost("query")]
        public async Task<ActionResult<SearchResult>> Query(SearchQuery searchQuery)
        {
            return await _iSearchManager.GetSearchResults<SearchResult>(searchQuery);
        }

        [HttpGet]
        public async Task<ActionResult<SearchResult>> Get(SearchQuery query)
        {
            return await _iSearchManager.GetSearchResults<Vehicle>(query);
        }


    }
}
