using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication_test_jwt
{
    public partial class About : Page
    {
        private const string SharedKey = "userIndexFake";
        protected async void Page_Load(object sender, EventArgs e)
        {

            // 發送 API 請求
            string apiUrl = @"http://localhost:31376/api/Token2";
            string timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            string signature = GenerateHmacSignature(timestamp, SharedKey);

            // 使用 HttpClient 發送請求
            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-Custom-Signature", signature);
                client.DefaultRequestHeaders.Add("X-Timestamp", timestamp);
                client.DefaultRequestHeaders.Add("X-UserId", "12345");

                var response =  client.GetAsync(apiUrl).Result;
                var token2 = response.Content.ReadAsStringAsync().Result;


                // 根據狀態碼處理不同情況
                if (response.IsSuccessStatusCode)
                {
                    var jsonObject = JsonNode.Parse(token2);
                    StringBuilder html = new StringBuilder();
                    html.Append("<table border='1' style='width: 50%; margin: 10px auto; text-align: left;'>");
                    html.Append($"<tr><th colspan='2' style='text-align: center; background-color: {response.IsSuccessStatusCode};'>{(response.IsSuccessStatusCode ? "成功" : "失敗")}</th></tr>");

                    foreach (var property in jsonObject.AsObject())
                    {
                        html.Append($"<tr><td style='width: 20%;'>{property.Key}</td><td>{property.Value}</td></tr>");
                        string responseAfterToken2 = await new ProtectedApi().CallProtectedApi(property.Value == null ? "" : property.Value.ToString());
                        Label_api.Text += responseAfterToken2;
                    }
                    html.Append("</table>");
                   
                    // 顯示到頁面
                    Response.Write(html.ToString());

                    

                }
                else
                {

                    Response.Write($"Token2: {token2}");

                }

               
                
            }

        }
        private string GenerateHmacSignature(string data, string key)//將金鑰轉成亂碼值。
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(hash);
            }
        }
        


    }
}