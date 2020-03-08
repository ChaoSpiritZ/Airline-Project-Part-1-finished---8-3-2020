using System;
using System.Collections.Generic;
using AirlineProject;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AirlineProjectTest
{
    [TestClass]
    public class AirlineFacadeTest
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

        //--------------------------ANONYMOUS-FACADE-----------------------------------------

        

        //----------------------ADMIN-FACADE------------------------------------------

        

        //-----------------------------------AIRLINE-FACADE----------------------------------

        [TestMethod]
        public void AirlineFacadeCancelFlightMethod()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);

            ILoggedInAirlineFacade airlineFacade = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline1.UserName, TestData.airline1.Password, out ILoginToken airlineToken);
            Flight newFlight = new Flight(0, 0, TestData.argentinaID, TestData.barbadosID, TestData.futureDate1, TestData.futureDate2, 200);
            airlineFacade.CreateFlight((LoginToken<AirlineCompany>)airlineToken, newFlight);
            airlineFacade.CancelFlight((LoginToken<AirlineCompany>)airlineToken, newFlight);

            IAnonymousUserFacade anonymousFacade = (IAnonymousUserFacade)TestConfig.fcs.Login("", "", out ILoginToken anonymousToken);

            Assert.AreEqual(null, anonymousFacade.GetFlight(newFlight.ID));
        }

        [TestMethod]
        public void AirlineFacadeChangeMyPasswordMethod()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);

            ILoggedInAirlineFacade airlineFacade = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline1.UserName, TestData.airline1.Password, out ILoginToken airlineToken);
            airlineFacade.ChangeMyPassword((LoginToken<AirlineCompany>)airlineToken, TestData.airline1.Password, "54321");

            IAnonymousUserFacade anonymousFacade = (IAnonymousUserFacade)TestConfig.fcs.Login("", "", out ILoginToken anonymousToken);

            Assert.AreEqual("54321", anonymousFacade.GetAirlineCompanyById(TestData.airline1.ID).Password);
        }

        [TestMethod]
        public void AirlineFacadeCreateFlightMethod()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);

            ILoggedInAirlineFacade airlineFacade = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline1.UserName, TestData.airline1.Password, out ILoginToken airlineToken);
            Flight newFlight = new Flight(0, 0, TestData.argentinaID, TestData.barbadosID, TestData.futureDate1, TestData.futureDate2, 200);
            airlineFacade.CreateFlight((LoginToken<AirlineCompany>)airlineToken, newFlight);

            IAnonymousUserFacade anonymousFacade = (IAnonymousUserFacade)TestConfig.fcs.Login("", "", out ILoginToken anonymousToken);
            Flight actualFlight = anonymousFacade.GetFlight(newFlight.ID);

            Assert.AreEqual(newFlight.ID, actualFlight.ID);
            Assert.AreEqual(newFlight.AirlineCompanyId, actualFlight.AirlineCompanyId);
            Assert.AreEqual(newFlight.OriginCountryCode, actualFlight.OriginCountryCode);
            Assert.AreEqual(newFlight.DestinationCountryCode, actualFlight.DestinationCountryCode);
            Assert.AreEqual(newFlight.DepartureTime, actualFlight.DepartureTime);
            Assert.AreEqual(newFlight.LandingTime, actualFlight.LandingTime);
            Assert.AreEqual(newFlight.RemainingTickets, actualFlight.RemainingTickets);
        }

        [TestMethod]
        public void AirlineFacadeGetAllFlightsMethod()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline2);

            Flight flight1 = new Flight(0, 0, TestData.argentinaID, TestData.barbadosID, TestData.futureDate1, TestData.futureDate2, 200);
            Flight flight2 = new Flight(0, 0, TestData.chadID, TestData.denmarkID, TestData.futureDate1, TestData.futureDate3, 200);
            Flight flight3 = new Flight(0, 0, TestData.egyptID, TestData.franceID, TestData.futureDate2, TestData.futureDate3, 200);
            ILoggedInAirlineFacade airlineFacade1 = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline1.UserName, TestData.airline1.Password, out ILoginToken airlineToken1);
            ILoggedInAirlineFacade airlineFacade2 = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline2.UserName, TestData.airline2.Password, out ILoginToken airlineToken2);
            airlineFacade1.CreateFlight((LoginToken<AirlineCompany>)airlineToken1, flight1);
            airlineFacade1.CreateFlight((LoginToken<AirlineCompany>)airlineToken1, flight2);
            airlineFacade2.CreateFlight((LoginToken<AirlineCompany>)airlineToken2, flight3);

            IList<Flight> flights1 = airlineFacade1.GetAllFlights((LoginToken<AirlineCompany>)airlineToken1);

            Assert.AreEqual(flight1, flights1[0]);
            Assert.AreEqual(flight2, flights1[1]);
            Assert.AreEqual(2, flights1.Count);
        }

        [TestMethod]
        public void AirlineFacadeGetAllTicketsMethod()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline2);
            adminFacade.CreateNewCustomer((LoginToken<Administrator>)adminToken, TestData.customer1);
            adminFacade.CreateNewCustomer((LoginToken<Administrator>)adminToken, TestData.customer2);
            adminFacade.CreateNewCustomer((LoginToken<Administrator>)adminToken, TestData.customer3);

            Flight flight1 = new Flight(0, 0, TestData.argentinaID, TestData.barbadosID, TestData.futureDate1, TestData.futureDate2, 200);
            Flight flight2 = new Flight(0, 0, TestData.chadID, TestData.denmarkID, TestData.futureDate1, TestData.futureDate3, 200);
            Flight flight3 = new Flight(0, 0, TestData.egyptID, TestData.franceID, TestData.futureDate2, TestData.futureDate3, 200);
            ILoggedInAirlineFacade airlineFacade1 = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline1.UserName, TestData.airline1.Password, out ILoginToken airlineToken1);
            ILoggedInAirlineFacade airlineFacade2 = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline2.UserName, TestData.airline2.Password, out ILoginToken airlineToken2);
            airlineFacade1.CreateFlight((LoginToken<AirlineCompany>)airlineToken1, flight1);
            airlineFacade1.CreateFlight((LoginToken<AirlineCompany>)airlineToken1, flight2);
            airlineFacade2.CreateFlight((LoginToken<AirlineCompany>)airlineToken2, flight3);

            ILoggedInCustomerFacade customerFacade1 = (ILoggedInCustomerFacade)TestConfig.fcs.Login(TestData.customer1.UserName, TestData.customer1.Password, out ILoginToken customerToken1);
            ILoggedInCustomerFacade customerFacade2 = (ILoggedInCustomerFacade)TestConfig.fcs.Login(TestData.customer2.UserName, TestData.customer2.Password, out ILoginToken customerToken2);
            ILoggedInCustomerFacade customerFacade3 = (ILoggedInCustomerFacade)TestConfig.fcs.Login(TestData.customer3.UserName, TestData.customer3.Password, out ILoginToken customerToken3);
            Ticket ticket1 = customerFacade1.PurchaseTicket((LoginToken<Customer>)customerToken1, flight1);
            Ticket ticket2 = customerFacade2.PurchaseTicket((LoginToken<Customer>)customerToken2, flight2);
            Ticket ticket3 = customerFacade3.PurchaseTicket((LoginToken<Customer>)customerToken3, flight3);

            IList<Ticket> tickets1 = airlineFacade1.GetAllTickets((LoginToken<AirlineCompany>)airlineToken1);

            Assert.AreEqual(ticket1, tickets1[0]);
            Assert.AreEqual(ticket2, tickets1[1]);
            Assert.AreEqual(2, tickets1.Count);
        }

        [TestMethod]
        public void AirlineFacadeModifyAirlineDetailsMethod()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);

            ILoggedInAirlineFacade airlineFacade1 = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline1.UserName, TestData.airline1.Password, out ILoginToken airlineToken1);
            AirlineCompany updatedAirline = new AirlineCompany(TestData.airline1.ID, "Alpho", "AlphoUser", "AlphoPass", TestData.argentinaID);
            airlineFacade1.ModifyAirlineDetails((LoginToken<AirlineCompany>)airlineToken1, updatedAirline);

            IAnonymousUserFacade anonymousFacade = (IAnonymousUserFacade)TestConfig.fcs.Login("", "", out ILoginToken anonymousToken);
            AirlineCompany actualAirline = anonymousFacade.GetAirlineCompanyById(TestData.airline1.ID);

            Assert.AreEqual(updatedAirline.ID, actualAirline.ID);
            Assert.AreEqual(updatedAirline.AirlineName, actualAirline.AirlineName);
            Assert.AreEqual(updatedAirline.UserName, actualAirline.UserName);
            Assert.AreEqual(updatedAirline.Password, actualAirline.Password);
            Assert.AreEqual(updatedAirline.CountryCode, actualAirline.CountryCode);
        }

        [TestMethod]
        public void AirlineFacadeUpdateFlightMethod()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);

            Flight flight1 = new Flight(0, 0, TestData.argentinaID, TestData.barbadosID, TestData.futureDate1, TestData.futureDate2, 200);
            
            ILoggedInAirlineFacade airlineFacade1 = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline1.UserName, TestData.airline1.Password, out ILoginToken airlineToken1);
            airlineFacade1.CreateFlight((LoginToken<AirlineCompany>)airlineToken1, flight1);
            Flight updatedFlight = new Flight(flight1.ID, flight1.AirlineCompanyId, TestData.chadID, TestData.denmarkID, TestData.futureDate1, TestData.futureDate2, 199);
            airlineFacade1.UpdateFlight((LoginToken<AirlineCompany>)airlineToken1, updatedFlight);

            IAnonymousUserFacade anonymousFacade = (IAnonymousUserFacade)TestConfig.fcs.Login("", "", out ILoginToken anonymousToken);
            Flight actualFlight = anonymousFacade.GetFlight(flight1.ID);

            Assert.AreEqual(updatedFlight.ID, actualFlight.ID);
            Assert.AreEqual(updatedFlight.AirlineCompanyId, actualFlight.AirlineCompanyId);
            Assert.AreEqual(updatedFlight.OriginCountryCode, actualFlight.OriginCountryCode);
            Assert.AreEqual(updatedFlight.DestinationCountryCode, actualFlight.DestinationCountryCode);
            Assert.AreEqual(updatedFlight.DepartureTime, actualFlight.DepartureTime);
            Assert.AreEqual(updatedFlight.LandingTime, actualFlight.LandingTime);
            Assert.AreEqual(updatedFlight.RemainingTickets, actualFlight.RemainingTickets);
        }

        //----------------------expected-exceptions-----------------------------

        [TestMethod]
        [ExpectedException(typeof(AirlineNameAlreadyExistsException))]
        public void AirlineFacadeModifyAirlineDetailsMethodAirlineNameAlreadyExistsException()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline2);

            ILoggedInAirlineFacade airlineFacade1 = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline1.UserName, TestData.airline1.Password, out ILoginToken airlineToken1);
            airlineFacade1.ModifyAirlineDetails((LoginToken<AirlineCompany>)airlineToken1, TestData.airline1);

            ILoggedInAirlineFacade airlineFacade2 = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline2.UserName, TestData.airline2.Password, out ILoginToken airlineToken2);
            AirlineCompany modifiedAirline = new AirlineCompany(TestData.airline2.ID, TestData.airline1.AirlineName, TestData.airline2.UserName, TestData.airline2.Password, TestData.airline2.CountryCode);
            modifiedAirline.AirlineName = TestData.airline1.AirlineName;
            airlineFacade2.ModifyAirlineDetails((LoginToken<AirlineCompany>)airlineToken2, modifiedAirline);
        }

        [TestMethod]
        [ExpectedException(typeof(UsernameAlreadyExistsException))]
        public void AirlineFacadeModifyAirlineDetailsMethodUsernameAlreadyExistsException()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline2);

            ILoggedInAirlineFacade airlineFacade1 = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline1.UserName, TestData.airline1.Password, out ILoginToken airlineToken1);
            airlineFacade1.ModifyAirlineDetails((LoginToken<AirlineCompany>)airlineToken1, TestData.airline1);

            ILoggedInAirlineFacade airlineFacade2 = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline2.UserName, TestData.airline2.Password, out ILoginToken airlineToken2);
            AirlineCompany modifiedAirline = new AirlineCompany(TestData.airline2.ID, TestData.airline2.AirlineName, TestData.airline1.UserName, TestData.airline2.Password, TestData.airline2.CountryCode);
            modifiedAirline.UserName = TestData.airline1.UserName;
            airlineFacade2.ModifyAirlineDetails((LoginToken<AirlineCompany>)airlineToken2, modifiedAirline);
        }

        [TestMethod]
        [ExpectedException(typeof(EmptyPasswordException))]

        public void AirlineFacadeModifyAirlineDetailsMethodEmptyPasswordException()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);

            ILoggedInAirlineFacade airlineFacade1 = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline1.UserName, TestData.airline1.Password, out ILoginToken airlineToken1);
            airlineFacade1.ChangeMyPassword((LoginToken<AirlineCompany>)airlineToken1, TestData.airline1.Password, "");

        }

        [TestMethod]
        [ExpectedException(typeof(WrongPasswordException))]
        public void AirlineLoginMethodWrongPasswordException()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);

            ILoggedInAirlineFacade airlineFacade1 = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline1.UserName, "wrongpass", out ILoginToken airlineToken1);
        }
    }
}
