using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Proxy
{
    internal class RequeteJCDecaux
    {
        public async Task<Station> CreatingObtectJCDecaux(int number, string contratName)
        {
            HttpClient client = new HttpClient();

            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                string link = "https://api.jcdecaux.com/vls/v3/stations/" + number.ToString() + "?contract=" + contratName + "&apiKey=847fe2621dfc49202c3db3e62c32bc308d0af982";
                HttpResponseMessage response = await client.GetAsync(link);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(responseBody);

                Station root = JsonConvert.DeserializeObject<Station>(responseBody);

                return root;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                using (StreamWriter writer = new StreamWriter("log7.log", true))
                {
                    writer.WriteLine(ex.ToString());
                }
                return null;
            }
        }


        public async Task<List<Station>> GetListStations()
        {
            HttpClient client = new HttpClient();

            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                string link = "https://api.jcdecaux.com/vls/v3/stations?apiKey=847fe2621dfc49202c3db3e62c32bc308d0af982";
                HttpResponseMessage response = await client.GetAsync(link);

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                List<Station> list = JsonConvert.DeserializeObject<List<Station>>(responseBody);

                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                using (StreamWriter writer = new StreamWriter("log7.log", true))
                {
                    writer.WriteLine(ex.ToString());
                }
                return null;
            }
        }
    }
}

