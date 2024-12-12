using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication_test_jwt
{
    public partial class Contact : Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                string token =new  TokenGenerator().GenerateToken("012344", "egiAjf9daAgbpr733fvkjNr3ikJhdkJ0753LAXpioe349LKHWFsjiei3", "rick.chiang", "company");
                string response = await CallProtectedApi(token);
                Label_api.Text = response;
            }
        }

        private async Task<string> CallProtectedApi(string token)
        {
            using (var client = new HttpClient())
            {
                var url = "http://localhost:31376/WeatherForecast"; 
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token); 
                var response = await client.GetAsync(url); 
                if (response.IsSuccessStatusCode)
                { 
                    var responseString = await response.Content.ReadAsStringAsync(); 
                    return responseString; 
                }
                return null;
            }
        }

    }
}