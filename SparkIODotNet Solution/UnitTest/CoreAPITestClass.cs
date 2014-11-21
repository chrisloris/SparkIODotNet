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
            public void GetDefaultValue()
            {
                CoreAPI core = Helper.GetCoreAPI();
                
                string strDefault = core.GetVariableString("varstring");

                Assert.AreEqual(strDefault, "teststrvalue");
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
