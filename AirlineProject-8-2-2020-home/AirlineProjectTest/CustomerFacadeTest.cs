using System;
using System.Collections.Generic;
using AirlineProject;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AirlineProjectTest
{
    [TestClass]
    public class CustomerFacadeTest
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
        public void CustomerFacadeCancelTicketMethod()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);
            adminFacade.CreateNewCustomer((LoginToken<Administrator>)adminToken, TestData.customer1);

            Flight flight1 = new Flight(0, 0, TestData.argentinaID, TestData.barbadosID, TestData.futureDate1, TestData.futureDate2, 200);
            ILoggedInAirlineFacade airlineFacade1 = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline1.UserName, TestData.airline1.Password, out ILoginToken airlineToken1);
            airlineFacade1.CreateFlight((LoginToken<AirlineCompany>)airlineToken1, flight1);

            IAnonymousUserFacade anonymousFacade = (IAnonymousUserFacade)TestConfig.fcs.Login("", "", out ILoginToken anonymousToken);

            //Assert.AreEqual(200, anonymousFacade.GetFlight(flight1.ID).RemainingTickets);

            ILoggedInCustomerFacade customerFacade1 = (ILoggedInCustomerFacade)TestConfig.fcs.Login(TestData.customer1.UserName, TestData.customer1.Password, out ILoginToken customerToken1);
            Ticket ticket1 = customerFacade1.PurchaseTicket((LoginToken<Customer>)customerToken1, flight1);

            Assert.AreEqual(199, anonymousFacade.GetFlight(flight1.ID).RemainingTickets);

            customerFacade1.CancelTicket((LoginToken<Customer>)customerToken1, ticket1);

            Assert.AreEqual(200, anonymousFacade.GetFlight(flight1.ID).RemainingTickets);
            Assert.AreEqual(0, customerFacade1.GetAllMyFlights((LoginToken<Customer>)customerToken1).Count);
        }

        [TestMethod]
        public void CustomerFacadeGetAllMyFlightsMethod()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);
            adminFacade.CreateNewCustomer((LoginToken<Administrator>)adminToken, TestData.customer1);

            Flight flight1 = new Flight(0, 0, TestData.argentinaID, TestData.barbadosID, TestData.futureDate1, TestData.futureDate2, 200);
            Flight flight2 = new Flight(0, 0, TestData.chadID, TestData.denmarkID, TestData.futureDate1, TestData.futureDate3, 190);
            ILoggedInAirlineFacade airlineFacade1 = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline1.UserName, TestData.airline1.Password, out ILoginToken airlineToken1);
            airlineFacade1.CreateFlight((LoginToken<AirlineCompany>)airlineToken1, flight1);
            airlineFacade1.CreateFlight((LoginToken<AirlineCompany>)airlineToken1, flight2);

            IAnonymousUserFacade anonymousFacade = (IAnonymousUserFacade)TestConfig.fcs.Login("", "", out ILoginToken anonymousToken);

            ILoggedInCustomerFacade customerFacade1 = (ILoggedInCustomerFacade)TestConfig.fcs.Login(TestData.customer1.UserName, TestData.customer1.Password, out ILoginToken customerToken1);

            Assert.AreEqual(0, customerFacade1.GetAllMyFlights((LoginToken<Customer>)customerToken1).Count);

            Ticket ticket1 = customerFacade1.PurchaseTicket((LoginToken<Customer>)customerToken1, flight1);
            Ticket ticket2 = customerFacade1.PurchaseTicket((LoginToken<Customer>)customerToken1, flight2);

            IList<Flight> actualflights = customerFacade1.GetAllMyFlights((LoginToken<Customer>)customerToken1);

            Assert.AreEqual(2, customerFacade1.GetAllMyFlights((LoginToken<Customer>)customerToken1).Count);
            Assert.AreEqual(flight1, actualflights[0]);
            Assert.AreEqual(flight2, actualflights[1]);
        }

        [TestMethod]
        public void CustomerFacadePurchaseTicketMethod()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);
            adminFacade.CreateNewCustomer((LoginToken<Administrator>)adminToken, TestData.customer1);

            Flight flight1 = new Flight(0, 0, TestData.argentinaID, TestData.barbadosID, TestData.futureDate1, TestData.futureDate2, 200);
            ILoggedInAirlineFacade airlineFacade1 = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline1.UserName, TestData.airline1.Password, out ILoginToken airlineToken1);
            airlineFacade1.CreateFlight((LoginToken<AirlineCompany>)airlineToken1, flight1);

            IAnonymousUserFacade anonymousFacade = (IAnonymousUserFacade)TestConfig.fcs.Login("", "", out ILoginToken anonymousToken);

            Assert.AreEqual(200, anonymousFacade.GetFlight(flight1.ID).RemainingTickets);

            ILoggedInCustomerFacade customerFacade1 = (ILoggedInCustomerFacade)TestConfig.fcs.Login(TestData.customer1.UserName, TestData.customer1.Password, out ILoginToken customerToken1);
            Ticket ticket1 = customerFacade1.PurchaseTicket((LoginToken<Customer>)customerToken1, flight1);

            Assert.AreEqual(199, anonymousFacade.GetFlight(flight1.ID).RemainingTickets);
            Assert.AreEqual(1, customerFacade1.GetAllMyFlights((LoginToken<Customer>)customerToken1).Count);
            Assert.AreEqual(ticket1.FlightId, anonymousFacade.GetFlight(flight1.ID).ID);
            Assert.AreEqual(ticket1.CustomerId, TestData.customer1.ID);
        }

        //----------------------expected-exceptions-----------------------------

        [TestMethod]
        [ExpectedException(typeof(NoMoreTicketsException))]
        public void CustomerFacadePurchaseTicketMethodNoMoreTicketsException()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);
            adminFacade.CreateNewCustomer((LoginToken<Administrator>)adminToken, TestData.customer1);

            ILoggedInAirlineFacade airlineFacade1 = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline1.UserName, TestData.airline1.Password, out ILoginToken airlineToken1);
            Flight flight1 = new Flight(0, 0, TestData.argentinaID, TestData.barbadosID, TestData.futureDate1, TestData.futureDate2, 0);
            airlineFacade1.CreateFlight((LoginToken<AirlineCompany>)airlineToken1, flight1);

            ILoggedInCustomerFacade customerFacade1 = (ILoggedInCustomerFacade)TestConfig.fcs.Login(TestData.customer1.UserName, TestData.customer1.Password, out ILoginToken customerToken1);
            customerFacade1.PurchaseTicket((LoginToken<Customer>)customerToken1, flight1);
        }

        [TestMethod]
        [ExpectedException(typeof(TicketAlreadyExistsException))]
        public void CustomerFacadePurchaseTicketMethodTicketAlreadyExistsException()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);
            adminFacade.CreateNewCustomer((LoginToken<Administrator>)adminToken, TestData.customer1);

            ILoggedInAirlineFacade airlineFacade1 = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline1.UserName, TestData.airline1.Password, out ILoginToken airlineToken1);
            Flight flight1 = new Flight(0, 0, TestData.argentinaID, TestData.barbadosID, TestData.futureDate1, TestData.futureDate2, 200);
            airlineFacade1.CreateFlight((LoginToken<AirlineCompany>)airlineToken1, flight1);

            ILoggedInCustomerFacade customerFacade1 = (ILoggedInCustomerFacade)TestConfig.fcs.Login(TestData.customer1.UserName, TestData.customer1.Password, out ILoginToken customerToken1);
            customerFacade1.PurchaseTicket((LoginToken<Customer>)customerToken1, flight1);
            customerFacade1.PurchaseTicket((LoginToken<Customer>)customerToken1, flight1);
        }

        [TestMethod]
        [ExpectedException(typeof(WrongPasswordException))]
        public void CustomerLoginMethodWrongPasswordException()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewCustomer((LoginToken<Administrator>)adminToken, TestData.customer1);

            ILoggedInCustomerFacade customerFacade1 = (ILoggedInCustomerFacade)TestConfig.fcs.Login(TestData.customer1.UserName, "wrongpass", out ILoginToken customerToken1);

        }
    }
}
