using Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackagesForDays.Services
{
    public interface IGraphIngestionService
    {
        Graph<string> Ingest(List<string> graphString);
    }
}
