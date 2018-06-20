using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PackagesForDays.Services;

namespace PackagesForDays.Controllers
{
    [Route("api/[controller]")]
    public class DependencyCheckerController : Controller
    {
        private readonly IGraphIngestionService _graphIngestionService;

        public DependencyCheckerController(IGraphIngestionService graphIngestionService)
        {
            _graphIngestionService = graphIngestionService;
        }

        [HttpPost("[action]")]
        public GraphResponse CheckGraph([FromBody] GraphRequest request)
        {
            var response = new GraphResponse();
            response.Request = string.Join(", ", request.Payload);

            try
            {
                var graph = _graphIngestionService.Ingest(request.Payload);
                response.Result = graph.TopologicalSort();
            }
            catch (ArgumentException ex)
            {
                response.Result = ex.Message;
            }

            return response;

        }

        public class GraphResponse
        {
            public string Request { get; set; }
            public string Result { get; set; }
        }

        public class GraphRequest
        {
            public List<string> Payload { get; set; }
        }
    }
}
