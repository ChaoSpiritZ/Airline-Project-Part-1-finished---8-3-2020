using System;
using System.Collections.Generic;
using AirlineProject;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AirlineProjectTest
{
    [TestClass]
    public class AnonymousFacadeTest
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
        public void AnonymousFacadeGetAirlineCompanyByIdMethod()
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

            //pretty much a AdminFacadeCreateNewAirlineMethod clone
        }

        [TestMethod]
        public void AnonymousFacadeGetAllAirlineCompaniesMethod()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline2);

            IAnonymousUserFacade anonymousFacade = (IAnonymousUserFacade)TestConfig.fcs.Login("", "", out ILoginToken anonymousToken);
            IList<AirlineCompany> airlineCompanies = anonymousFacade.GetAllAirlineCompanies();

            Assert.AreEqual(TestData.airline1, airlineCompanies[0]);
            Assert.AreEqual(TestData.airline2, airlineCompanies[1]);
        }

        [TestMethod]
        public void AnonymousFacadeGetAllFlightsMethod()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);

            ILoggedInAirlineFacade airlineFacade = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline1.UserName, TestData.airline1.Password, out ILoginToken airlineToken);
            Flight flight1 = new Flight(0, 0, TestData.argentinaID, TestData.barbadosID, TestData.futureDate1, TestData.futureDate2, 200);
            Flight flight2 = new Flight(0, 0, TestData.chadID, TestData.denmarkID, TestData.futureDate1, TestData.futureDate2, 190);
            airlineFacade.CreateFlight((LoginToken<AirlineCompany>)airlineToken, flight1);
            airlineFacade.CreateFlight((LoginToken<AirlineCompany>)airlineToken, flight2);

            IAnonymousUserFacade anonymousFacade = (IAnonymousUserFacade)TestConfig.fcs.Login("", "", out ILoginToken anonymousToken);
            IList<Flight> flights = anonymousFacade.GetAllFlights();

            Assert.AreEqual(flight1, flights[0]);
            Assert.AreEqual(flight2, flights[1]);
        }

        [TestMethod]
        public void AnonymousFacadeGetAllFlightsVacancyMethod()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);

            ILoggedInAirlineFacade airlineFacade = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline1.UserName, TestData.airline1.Password, out ILoginToken airlineToken);
            Flight flight1 = new Flight(0, 0, TestData.argentinaID, TestData.barbadosID, TestData.futureDate1, TestData.futureDate2, 200);
            Flight flight2 = new Flight(0, 0, TestData.chadID, TestData.denmarkID, TestData.futureDate1, TestData.futureDate2, 190);
            airlineFacade.CreateFlight((LoginToken<AirlineCompany>)airlineToken, flight1);
            airlineFacade.CreateFlight((LoginToken<AirlineCompany>)airlineToken, flight2);

            IAnonymousUserFacade anonymousFacade = (IAnonymousUserFacade)TestConfig.fcs.Login("", "", out ILoginToken anonymousToken);
            Dictionary<Flight, int> flightsVacancy = anonymousFacade.GetAllFlightsVacancy();

            Assert.AreEqual(flight1.RemainingTickets, flightsVacancy[flight1]);
            Assert.AreEqual(flight2.RemainingTickets, flightsVacancy[flight2]);
        }

        [TestMethod]
        public void AnonymousFacadeGetFlightMethod()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);

            ILoggedInAirlineFacade airlineFacade = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline1.UserName, TestData.airline1.Password, out ILoginToken airlineToken);
            Flight flight1 = new Flight(0, 0, TestData.argentinaID, TestData.barbadosID, TestData.futureDate1, TestData.futureDate2, 200);
            airlineFacade.CreateFlight((LoginToken<AirlineCompany>)airlineToken, flight1);

            IAnonymousUserFacade anonymousFacade = (IAnonymousUserFacade)TestConfig.fcs.Login("", "", out ILoginToken anonymousToken);
            Flight actualFlight = anonymousFacade.GetFlight(flight1.ID);

            Assert.AreEqual(flight1.ID, actualFlight.ID);
            Assert.AreEqual(flight1.AirlineCompanyId, actualFlight.AirlineCompanyId);
            Assert.AreEqual(flight1.OriginCountryCode, actualFlight.OriginCountryCode);
            Assert.AreEqual(flight1.DestinationCountryCode, actualFlight.DestinationCountryCode);
            Assert.AreEqual(flight1.DepartureTime, actualFlight.DepartureTime);
            Assert.AreEqual(flight1.LandingTime, actualFlight.LandingTime);
            Assert.AreEqual(flight1.RemainingTickets, actualFlight.RemainingTickets);
        }

        [TestMethod]
        public void AnonymousFacadeGetFlightsByDepartureDateMethod()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);

            ILoggedInAirlineFacade airlineFacade = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline1.UserName, TestData.airline1.Password, out ILoginToken airlineToken);
            Flight flight1 = new Flight(0, 0, TestData.argentinaID, TestData.barbadosID, TestData.futureDate1, TestData.futureDate2, 200);
            Flight flight2 = new Flight(0, 0, TestData.chadID, TestData.denmarkID, TestData.futureDate1, TestData.futureDate3, 190);
            Flight flight3 = new Flight(0, 0, TestData.egyptID, TestData.franceID, TestData.futureDate2, TestData.futureDate3, 180);
            airlineFacade.CreateFlight((LoginToken<AirlineCompany>)airlineToken, flight1);
            airlineFacade.CreateFlight((LoginToken<AirlineCompany>)airlineToken, flight2);
            airlineFacade.CreateFlight((LoginToken<AirlineCompany>)airlineToken, flight3);

            IAnonymousUserFacade anonymousFacade = (IAnonymousUserFacade)TestConfig.fcs.Login("", "", out ILoginToken anonymousToken);
            IList<Flight> flights = anonymousFacade.GetFlightsByDepartureDate(TestData.futureDate1);

            Assert.AreEqual(flight1, flights[0]);
            Assert.AreEqual(flight2, flights[1]);
            Assert.AreEqual(2, flights.Count);
        }

        [TestMethod]
        public void AnonymousFacadeGetFlightsByDestinationCountryMethod()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);

            ILoggedInAirlineFacade airlineFacade = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline1.UserName, TestData.airline1.Password, out ILoginToken airlineToken);
            Flight flight1 = new Flight(0, 0, TestData.argentinaID, TestData.barbadosID, TestData.futureDate1, TestData.futureDate2, 200);
            Flight flight2 = new Flight(0, 0, TestData.argentinaID, TestData.chadID, TestData.futureDate1, TestData.futureDate3, 190);
            Flight flight3 = new Flight(0, 0, TestData.barbadosID, TestData.chadID, TestData.futureDate2, TestData.futureDate3, 180);
            airlineFacade.CreateFlight((LoginToken<AirlineCompany>)airlineToken, flight1);
            airlineFacade.CreateFlight((LoginToken<AirlineCompany>)airlineToken, flight2);
            airlineFacade.CreateFlight((LoginToken<AirlineCompany>)airlineToken, flight3);

            IAnonymousUserFacade anonymousFacade = (IAnonymousUserFacade)TestConfig.fcs.Login("", "", out ILoginToken anonymousToken);
            IList<Flight> flights = anonymousFacade.GetFlightsByDestinationCountry(TestData.chadID);

            Assert.AreEqual(flight2, flights[0]);
            Assert.AreEqual(flight3, flights[1]);
            Assert.AreEqual(2, flights.Count);
        }

        [TestMethod]
        public void AnonymousFacadeGetFlightsByLandingDateMethod()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);

            ILoggedInAirlineFacade airlineFacade = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline1.UserName, TestData.airline1.Password, out ILoginToken airlineToken);
            Flight flight1 = new Flight(0, 0, TestData.argentinaID, TestData.barbadosID, TestData.futureDate1, TestData.futureDate2, 200);
            Flight flight2 = new Flight(0, 0, TestData.chadID, TestData.denmarkID, TestData.futureDate1, TestData.futureDate3, 190);
            Flight flight3 = new Flight(0, 0, TestData.egyptID, TestData.franceID, TestData.futureDate2, TestData.futureDate3, 180);
            airlineFacade.CreateFlight((LoginToken<AirlineCompany>)airlineToken, flight1);
            airlineFacade.CreateFlight((LoginToken<AirlineCompany>)airlineToken, flight2);
            airlineFacade.CreateFlight((LoginToken<AirlineCompany>)airlineToken, flight3);

            IAnonymousUserFacade anonymousFacade = (IAnonymousUserFacade)TestConfig.fcs.Login("", "", out ILoginToken anonymousToken);
            IList<Flight> flights = anonymousFacade.GetFlightsByLandingDate(TestData.futureDate3);

            Assert.AreEqual(flight2, flights[0]);
            Assert.AreEqual(flight3, flights[1]);
            Assert.AreEqual(2, flights.Count);
        }

        [TestMethod]
        public void AnonymousFacadeGetFlightsByOriginCountryMethod()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.ADMIN_USERNAME, AirlineProjectConfig.ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);

            ILoggedInAirlineFacade airlineFacade = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline1.UserName, TestData.airline1.Password, out ILoginToken airlineToken);
            Flight flight1 = new Flight(0, 0, TestData.argentinaID, TestData.barbadosID, TestData.futureDate1, TestData.futureDate2, 200);
            Flight flight2 = new Flight(0, 0, TestData.argentinaID, TestData.chadID, TestData.futureDate1, TestData.futureDate3, 190);
            Flight flight3 = new Flight(0, 0, TestData.barbadosID, TestData.chadID, TestData.futureDate2, TestData.futureDate3, 180);
            airlineFacade.CreateFlight((LoginToken<AirlineCompany>)airlineToken, flight1);
            airlineFacade.CreateFlight((LoginToken<AirlineCompany>)airlineToken, flight2);
            airlineFacade.CreateFlight((LoginToken<AirlineCompany>)airlineToken, flight3);

            IAnonymousUserFacade anonymousFacade = (IAnonymousUserFacade)TestConfig.fcs.Login("", "", out ILoginToken anonymousToken);
            IList<Flight> flights = anonymousFacade.GetFlightsByOriginCountry(TestData.argentinaID);

            Assert.AreEqual(flight1, flights[0]);
            Assert.AreEqual(flight2, flights[1]);
            Assert.AreEqual(2, flights.Count);
        }

        //----------------------expected-exceptions-----------------------------


    }
}
