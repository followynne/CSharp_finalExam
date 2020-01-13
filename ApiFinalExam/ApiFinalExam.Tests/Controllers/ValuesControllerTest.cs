using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ApiFinalExam;
using ApiFinalExam.Controllers;

namespace ApiFinalExam.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Disposizione
            ValuesController controller = new ValuesController();

            // Azione
            IEnumerable<string> result = controller.Get();

            // Asserzione
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("value1", result.ElementAt(0));
            Assert.AreEqual("value2", result.ElementAt(1));
        }

        [TestMethod]
        public void GetById()
        {
            // Disposizione
            ValuesController controller = new ValuesController();

            // Azione
            string result = controller.Get(5);

            // Asserzione
            Assert.AreEqual("value", result);
        }

        [TestMethod]
        public void Post()
        {
            // Disposizione
            ValuesController controller = new ValuesController();

            // Azione
            controller.Post("value");

            // Asserzione
        }

        [TestMethod]
        public void Put()
        {
            // Disposizione
            ValuesController controller = new ValuesController();

            // Azione
            controller.Put(5, "value");

            // Asserzione
        }

        [TestMethod]
        public void Delete()
        {
            // Disposizione
            ValuesController controller = new ValuesController();

            // Azione
            controller.Delete(5);

            // Asserzione
        }
    }
}
