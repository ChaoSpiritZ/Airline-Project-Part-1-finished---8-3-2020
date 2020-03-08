using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AirlineProject
{
    public class FlyingCenterSystem
    {
        private static FlyingCenterSystem INSTANCE;
        private static object key = new object();

        private FlyingCenterSystem()
        {
            Task.Run(() =>
            {
                SystemFacade _systemFacade = new SystemFacade();
                while (true)
                {
                    Thread.Sleep(AirlineProjectConfig.SEND_TO_HISTORY_INTERVAL);
                    _systemFacade.MoveTicketsAndFlightsToHistory();
                }
            });
        }

        public static FlyingCenterSystem GetInstance()
        {
            if (INSTANCE == null)
            {
                lock (key)
                {
                    if (INSTANCE == null)
                    {
                        INSTANCE = new FlyingCenterSystem();
                    }
                }
            }
            return INSTANCE;
        }

        public FacadeBase Login(string username, string pwd, out ILoginToken loginToken)
        {
            LoginService lS = new LoginService();
            if (username == null || username == "") //added the OR when i added the WPF
            {
                loginToken = null;
                return new AnonymousUserFacade();
            }
            if (lS.TryAdminLogin(username, pwd, out LoginToken<Administrator> adminToken))
            {
                loginToken = adminToken;
                return new LoggedInAdministratorFacade();
            }
            if (lS.TryAirlineLogin(username, pwd, out LoginToken<AirlineCompany> airlineToken))
            {
                loginToken = airlineToken;
                return new LoggedInAirlineFacade();
            }
            if (lS.TryCustomerLogin(username, pwd, out LoginToken<Customer> customerToken))
            {
                loginToken = customerToken;
                return new LoggedInCustomerFacade();
            }
            loginToken = null;
            return null;
        }

    }
}
