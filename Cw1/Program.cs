using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cw1
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            //komentarz ctrl+k+c
            //odkomentowanie ctrl+k+u

            int number = 2;
            double numberDouble = 3.2;
            float numberFloat = 3.0f;

            string name = "Karolina";
            bool boolean = true;
            string lastName = " Guzewska";
            string patch = @"C:\User\s17428\Desktop\Cw1";

            Person newPerson = new Person { FirstName = "Karolina"};

            var url = "http://www.pja.edu.pl";

            var httpClient = new HttpClient();
            var response =await httpClient.GetAsync(url);

            if(response.IsSuccessStatusCode)
            {
                var htmlContent = await response.Content.ReadAsStringAsync();
                var regex = new Regex("[a-z]+[a-z0-9]*@[a-z0-9]+\\.[a-z]", RegexOptions.IgnoreCase);

                var matches = regex.Matches(htmlContent);
                foreach (var match in matches)
                {
                    Console.WriteLine(match.ToString());

                }
            }


        }
    }
}
