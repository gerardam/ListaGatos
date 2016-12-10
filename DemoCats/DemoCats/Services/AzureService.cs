using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace DemoCats.Services
{
    public class AzureService
    {
        IMobileServiceClient client;
        IMobileServiceTable<Cat> Cats;

        public AzureService()
        {
            string MyAppServiceURL = "http://demoxamaringam.azurewebsites.net/";
            client = new MobileServiceClient(MyAppServiceURL);
            Cats = client.GetTable<Cat>();
        }

        public Task<IEnumerable<Cat>> GetCats()
        {
            return Cats.ToEnumerableAsync();
        }
    }
}
