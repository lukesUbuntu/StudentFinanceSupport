using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Web.Routing;
using System.Security.Principal;

using StudentFinanceSupport.Controllers;
using StudentFinanceSupport.Models;

namespace StudentFinanceSupport.UnitTesting.Controllers
{
    [TestClass]
    class HomeControllerTest
    {
         [TestMethod]
        public void TestAboutView()
        {
            var controller = new HomeController();
            var result = controller.About() as ViewResult;
            Assert.AreEqual("About Application.", result.ViewBag.Message);
        }


    }
}
