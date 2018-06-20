using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Graph;

namespace PackagesForDays.Services
{
    public class GraphIngestionService : IGraphIngestionService
    {
        /// <summary>
        /// Take a graph definition list and build the graph, checking for invalid cycles along the way
        /// </summary>
        /// <param name="graphStrings"></param>
        /// <returns></returns>
        public Graph<string> Ingest(List<string> graphStrings)
        {
            Graph<string> toReturn = new Graph<string>();

            //loop through each package in the definition
            foreach (string graphString in graphStrings)
            {
                //clean up and split the package definition by the delimiter 
                var splitDependent = graphString.Trim().Split(':');

                //grab the first package, this will either be a new dangling node or a child of another node
                var newPackage = splitDependent.First().Trim();

                //check to see if a node already exists with this package name
                var edge = toReturn.GetNodeWithValue(newPackage);

                //this is a new node, new package we haven't seen
                if (edge == null)
                {
                    //add it
                    edge = toReturn.AddNode(newPackage);
                }
              

                //if there was something after the delimiter, we need to set up a relationship between these two nodes
                if (splitDependent.Length > 1 && !string.IsNullOrWhiteSpace(splitDependent[1]))
                {
                    var parentPackage = splitDependent[1].Trim();
                    //this node has a parent dependency
                    var parent = toReturn.GetNodeWithValue(parentPackage);

                    //there's a parent dependency here but it's a new package we haven't seen yet
                    if (parent == null)
                    {
                        parent = toReturn.AddNode(parentPackage);
                    }

                    //we have a parent/child relationship, add it
                    toReturn.AddDirectedEdge(parent, edge);

                    //check if there are now any circular depencies after adding this relationship
                    if (toReturn.ContainsCircularReference())
                    {
                        throw new ArgumentException("The dependency tree definition contains a circular reference, this is invalid.");
                    }
                }

            }

            //return the graph if the configuration is valid and every node's been ingested
          
            return toReturn;
        }
    }
}
