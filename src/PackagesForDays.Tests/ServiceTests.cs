
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

            Assert.ThrowsException<ArgumentException>(() => graphIngestionService.Ingest(invalidGraphDefinitionList));




        }

        [TestMethod]
        public void CanIngestGraphWithValidConfiguration100RandomShuffles()
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

        

            for (var attempt = 0; attempt < 100; attempt++)
            {
                try
                {
                    validGraphDefinitionList.Shuffle();
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
        }

        [TestMethod]
        public void CanIngestGraphAndDetectCircularReferences100RandomShuffles()
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

            for (var attempt = 0; attempt < 100; attempt++)
            {
                invalidGraphDefinitionList.Shuffle();
                Assert.ThrowsException<ArgumentException>(() => graphIngestionService.Ingest(invalidGraphDefinitionList));

            }
        }





    }
}
