using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace FlightReservationGUI
{
    public class Session
    {
        private static Session session;
        private SqlConnection connection;
        public string Ssn { get; private set; }
        public string UserType { get; private set; }
        public Session() { }
        public static Session CurrentSession
        {
            get
            {
                if (session == null)
                {
                    session = new Session();
                }
                return session;
            }
        }

        public void CreateConnection(string connectionString)
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        public SqlConnection GetConnection()
        {
            return connection;
        }

        public void SetUserSession(string ssn, string userType)
        {
            Ssn = ssn;
            UserType = userType;
        }

        public void Logout()
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
            Ssn = null;
            UserType = null;
        }
    }
}
