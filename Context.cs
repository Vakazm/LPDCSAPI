using LPDCSAPI.Classes;

namespace LPDCSAPI
{
    // Class serves as database, storing information about all dealers
    public static class Context
    {
        private static List <Dealer> Dealers = new List <Dealer> ();

        // Add dealer to database and return their token
        public static string AddDealer (Dealer.Reginfo reginfo)
        {
            Dealer dealer = new Dealer { reginfo = reginfo };
            dealer.GenerateToken ();
            Dealers.Add (dealer);
            return dealer.Token;
        }

        // Find delaer by their token
        public static Dealer? FindDealer (string token)
        {
            return Dealers.Find (d => d.Token == token);
        }

        // Check if dealer in database
        public static bool IsDealer (string? username, string? email)
        {
            foreach (var d in Dealers)
            {
                if (d.reginfo.Username == username || d.reginfo.Email == email) { return true; }
            }
            return false;
        }

        // 
        public static string? LoginDealer (string? username, string? email, string password)
        {
            Dealer? dealer = null;
            // Login by both username and email
            if (!string.IsNullOrEmpty (username) && !string.IsNullOrEmpty (email))
            {
                foreach (var d in Dealers)
                {
                    if (d.reginfo.Username == username && d.reginfo.Email == email && d.reginfo.Password == password)
                    {
                        dealer = d;
                        break;
                    }
                }
            }
            // Login by email only
            else if (string.IsNullOrEmpty (email))
            {
                foreach (var d in Dealers)
                {
                    if (d.reginfo.Username == username && d.reginfo.Password == password)
                    {
                        dealer = d;
                        break;
                    }
                }
            }
            // Login by username only
            else if (string.IsNullOrEmpty (username))
            {
                foreach (var d in Dealers)
                {
                    if (d.reginfo.Email == email && d.reginfo.Password == password)
                    {
                        dealer = d;
                        break;
                    }
                }
            }

            if (dealer == null) { return null; }  // Login failed
            else  // Login successful
            {
                if (string.IsNullOrEmpty (dealer.Token)) { dealer.GenerateToken (); }
                return dealer.Token;
            }
        }
    }
}
