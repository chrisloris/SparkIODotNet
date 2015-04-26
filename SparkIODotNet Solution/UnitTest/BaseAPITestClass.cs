using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkIO.WebServices;

namespace SparkIO.UnitTest
{
    [TestClass]
    public class BaseAPITestClass
    {
        [TestClass]
        public class UserIDAndPassWordContruction
        {
            [TestMethod]
            public void CreateWithIDAndPassword()
            {
                BaseAPI baseObj = new BaseAPI(Helper.GetUserName(), Helper.GetPassword(), Helper.GetCoreID());

                // test we can access the core by asking its name
                Assert.AreEqual(baseObj.GetCoreInfo().Name, Helper.GetCoreName());
            }

            [TestMethod]
            [ExpectedException(typeof(SparkIO.WebServices.Exceptions.UsernameOrPasswordIncorrectException))]
            public void BadPassword()
            {
                BaseAPI baseObj = new BaseAPI(Helper.GetUserName(), Helper.GetPassword() + "xx", Helper.GetCoreID());

                // test we can access the core by asking its name
                Assert.AreEqual(baseObj.GetCoreInfo().Name, Helper.GetCoreName());
            }

            [TestMethod]
            [ExpectedException(typeof(SparkIO.WebServices.Exceptions.NotAuthorizedForThisCoreException))]
            public void UnauthorizedCore()
            {
                BaseAPI baseObj = new BaseAPI(Helper.GetUserName(), Helper.GetPassword(), Helper.GetCoreID() + "00");

                // test we can access the core by asking its name
                Assert.AreEqual(baseObj.GetCoreInfo().Name, Helper.GetCoreName());
            }


        }
    }
}
