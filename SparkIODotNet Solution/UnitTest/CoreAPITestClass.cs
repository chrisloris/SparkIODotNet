using System;
using System.Configuration;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkIO.WebServices;

namespace SparkIO.UnitTest
{

    [TestClass]
    public class CoreAPITestClass
    {
        [TestClass]
        public class GetVariableString
        {
            [TestMethod]
            // Spark Web API Not Returning expected error codes
            //[ExpectedException(typeof(SparkIO.WebServices.Exceptions.InvalidVariableOrFunctionException))]
            //Hoping they will fix to return exception like when calling non-existent variable
            //[ExpectedException(typeof(SparkIO.WebServices.Exceptions.CoreNotConnectedToCloudException))]
            [ExpectedException(typeof(System.ApplicationException))]
            public void InvalidFunction()
            {
                CoreAPI core = Helper.GetCoreAPI();

                // test we can access the core by asking its name
                Assert.AreEqual(core.GetCoreInfo().Name, Helper.GetCoreName());

                // call non-existant function
                core.CallFunctionInt("xxxxxx", "none");
            }

            [TestMethod]
            // Spark Web API Not Returning expected error codes
            //[ExpectedException(typeof(SparkIO.WebServices.Exceptions.InvalidVariableOrFunctionException))]
            [ExpectedException(typeof(SparkIO.WebServices.Exceptions.CoreNotConnectedToCloudException))]
            public void InvalidVariable()
            {
                CoreAPI core = Helper.GetCoreAPI();

                // call non-existant function
                core.GetVariableInt("xxxxxx");
            }
            /*
            [TestMethod]
            public void SetAndGetValue()
            {
            }

            [TestMethod]
            public void GetIntVariable()
            {
            }

            [TestMethod]
            public void GetDoubleVariable()
            {
            }

            [TestMethod]
            public void GetBooleanVariable()
            {
            }
            */
        }
    }
}
