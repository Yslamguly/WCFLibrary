using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace BookHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(WCFLibrary.Service1)))
            {
                try
                {
                    host.Open();
                    Console.WriteLine("Host started @ " + DateTime.Now.ToString());
                    Console.ReadLine();
                }
                catch (AddressAccessDeniedException ex)
                {
                    Console.WriteLine(ex.Message + " \nPlease, restart Visual studio as an administrator");
                    Console.ReadLine();
                }

            }
        }
    }
}
