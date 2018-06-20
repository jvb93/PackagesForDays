
using System;
using System.Collections.Generic;
using System.Linq;
using Graph;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PackagesForDays.Services;

namespace PackagesForDays.Tests
{
    [TestClass]
    public class ServiceTests
    {
        [TestMethod]
        public void CanIngestGraphWithValidConfiguration()
        {
            IGraphIngestionService graphIngestionService = new GraphIngestionService();

            var validGraphDefinitionList = new List<string>()
            {
                "KittenService: ",
                "Leetmeme: Cyberportal",
                "Cyberportal: Ice",
                "CamelCaser: KittenService",
                "Fraudstream: Leetmeme",
                "Ice: "

            };

            try
            {
                var graph = graphIngestionService.Ingest(validGraphDefinitionList);
                Assert.AreEqual(graph.Nodes.Count, 6);

                var cyberPortalNode = graph.GetNodeWithValue("Cyberportal");
                var leetMemeNode = graph.GetNodeWithValue("Leetmeme");

                Assert.IsTrue(cyberPortalNode.Children.Contains(leetMemeNode));
            }
            catch (ArgumentException ex)
            {
                Assert.Fail(ex.Message);
            }

         

        }

        [TestMethod]
        public void CanIngestGraphAndDetectCircularReferences()
        {
            IGraphIngestionService graphIngestionService = new GraphIngestionService();

            var invalidGraphDefinitionList = new List<string>()
            {
                "KittenService: ",
                "Leetmeme: Cyberportal",
                "Cyberportal: Ice",
                "CamelCaser: KittenService",
                "Fraudstream: ",
                "Ice: Leetmeme"

            };
              
            try
            {
                var graph = graphIngestionService.Ingest(invalidGraphDefinitionList);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex != null);
            }



        }





    }
}
