using System;
using AirlineProject;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AirlineProjectTest
{
    [TestClass]
    public class AdminFacadeTest
    {
        [TestInitialize] //goes to this before every test
        public void Initialize()
        {
            AirlineProjectConfig.CONNECTION_STRING = @"Data Source=DESKTOP-JJ6DFK2;Initial Catalog=TESTINGAirlineProject;Integrated Security=True";
            TestConfig.testFacade.ClearAllTables();
        }

        [TestCleanup]
        public void Cleanup() //goes to this after every test
        {
            AirlineProjectConfig.CONNECTION_STRING = @"Data Source=DESKTOP-JJ6DFK2;Initial Catalog=AirlineProject;Integrated Security=True";
        }

        [TestMethod]
        public void AdminFacadeCreateNewAirlineMethod()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);

            IAnonymousUserFacade anonymousFacade = (IAnonymousUserFacade)TestConfig.fcs.Login("", "", out ILoginToken anonymousToken);
            AirlineCompany actualAirline = anonymousFacade.GetAirlineCompanyById(TestData.airline1.ID);

            Assert.AreEqual(TestData.airline1.ID, actualAirline.ID);
            Assert.AreEqual(TestData.airline1.AirlineName, actualAirline.AirlineName);
            Assert.AreEqual(TestData.airline1.UserName, actualAirline.UserName);
            Assert.AreEqual(TestData.airline1.Password, actualAirline.Password);
            Assert.AreEqual(TestData.airline1.CountryCode, actualAirline.CountryCode);
        }

        [TestMethod]
        public void AdminFacadeCreateNewCustomerMethod()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewCustomer((LoginToken<Administrator>)adminToken, TestData.customer1);

            Customer actualCustomer = TestConfig.testFacade.GetCustomerById(TestData.customer1.ID);

            Assert.AreEqual(TestData.customer1.ID, actualCustomer.ID);
            Assert.AreEqual(TestData.customer1.FirstName, actualCustomer.FirstName);
            Assert.AreEqual(TestData.customer1.LastName, actualCustomer.LastName);
            Assert.AreEqual(TestData.customer1.UserName, actualCustomer.UserName);
            Assert.AreEqual(TestData.customer1.Password, actualCustomer.Password);
            Assert.AreEqual(TestData.customer1.Address, actualCustomer.Address);
            Assert.AreEqual(TestData.customer1.PhoneNo, actualCustomer.PhoneNo);
            Assert.AreEqual(TestData.customer1.CreditCardNumber, actualCustomer.CreditCardNumber);
        }

        [TestMethod]
        public void AdminFacadeRemoveAirlineMethod()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);
            adminFacade.RemoveAirline((LoginToken<Administrator>)adminToken, TestData.airline1);

            IAnonymousUserFacade anonymousFacade = (IAnonymousUserFacade)TestConfig.fcs.Login("", "", out ILoginToken anonymousToken);

            Assert.AreEqual(null, anonymousFacade.GetAirlineCompanyById(TestData.airline1.ID));
        }

        [TestMethod]
        public void AdminFacadeRemoveCustomerMethod()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewCustomer((LoginToken<Administrator>)adminToken, TestData.customer1);
            adminFacade.RemoveCustomer((LoginToken<Administrator>)adminToken, TestData.customer1);

            Assert.AreEqual(null, TestConfig.testFacade.GetCustomerById(TestData.customer1.ID));
        }

        [TestMethod]
        public void AdminFacadeUpdateAirlineDetailsMethod()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);
            AirlineCompany updatedAirline = new AirlineCompany(TestData.airline1.ID, "Alpho", "AlphoUser", "AlphoPass", TestData.argentinaID);
            adminFacade.UpdateAirlineDetails((LoginToken<Administrator>)adminToken, updatedAirline);

            IAnonymousUserFacade anonymousFacade = (IAnonymousUserFacade)TestConfig.fcs.Login("", "", out ILoginToken anonymousToken);
            AirlineCompany actualAirline = anonymousFacade.GetAirlineCompanyById(TestData.airline1.ID);

            Assert.AreEqual(updatedAirline.ID, actualAirline.ID);
            Assert.AreEqual(updatedAirline.AirlineName, actualAirline.AirlineName);
            Assert.AreEqual(updatedAirline.UserName, actualAirline.UserName);
            Assert.AreEqual(updatedAirline.Password, actualAirline.Password);
            Assert.AreEqual(updatedAirline.CountryCode, actualAirline.CountryCode);
        }

        [TestMethod]
        public void AdminFacadeUpdateCustomerDetailsMethod()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewCustomer((LoginToken<Administrator>)adminToken, TestData.customer1);
            Customer updatedCustomer = new Customer(TestData.customer1.ID, "Alphonse", "Alric", "AlphonseUser", "AlphonsePass", "AlphonseAddress", "050-aaaaaaa", "creditaaa");
            adminFacade.UpdateCustomerDetails((LoginToken<Administrator>)adminToken, updatedCustomer);

            Customer actualCustomer = TestConfig.testFacade.GetCustomerById(TestData.customer1.ID);

            Assert.AreEqual(updatedCustomer.ID, actualCustomer.ID);
            Assert.AreEqual(updatedCustomer.FirstName, actualCustomer.FirstName);
            Assert.AreEqual(updatedCustomer.LastName, actualCustomer.LastName);
            Assert.AreEqual(updatedCustomer.UserName, actualCustomer.UserName);
            Assert.AreEqual(updatedCustomer.Password, actualCustomer.Password);
            Assert.AreEqual(updatedCustomer.Address, actualCustomer.Address);
            Assert.AreEqual(updatedCustomer.PhoneNo, actualCustomer.PhoneNo);
            Assert.AreEqual(updatedCustomer.CreditCardNumber, actualCustomer.CreditCardNumber);
        }

        //----------------------expected-exceptions-----------------------------

        [TestMethod]
        [ExpectedException(typeof(AirlineNameAlreadyExistsException))]

        public void AdminFacadeCreateNewAirlineAirlineNameAlreadyExistsException()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);
        }

        [TestMethod]
        [ExpectedException(typeof(CountryNotFoundException))]
        public void AdminFacadeCreateNewAirlineMethodCountryNotFoundException()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, new AirlineCompany(0, "omega", "omegauser", "omegapass", 15));
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void AdminFacadeRemoveAirlineMethodUserNotFoundException()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.RemoveAirline((LoginToken<Administrator>)adminToken, TestData.airline1);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void AdminFacadeRemoveCustomerMethodUserNotFoundException()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.RemoveCustomer((LoginToken<Administrator>)adminToken, TestData.customer1);
        }

        [TestMethod]
        [ExpectedException(typeof(WrongPasswordException))]
        public void AdminLoginMethodWrongPasswordException()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, "wrongpass", out ILoginToken adminToken);
        }
    }
}
