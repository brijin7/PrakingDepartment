using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;

/// <summary>
/// this class contains common methods 
/// </summary>
public class Helper
{
    public void APIGet<T>(string requestUri, out T JArrayResult, out int StatusCode, out string Response) where T : class
    {
        try
        {
            HttpClient Client = new HttpClient();
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = Client.GetAsync(requestUri).Result;

            string Content = response.Content.ReadAsStringAsync().Result;
            string JSONResponse = JObject.Parse(Content)["response"].ToString();
            StatusCode = Convert.ToInt32(JObject.Parse(Content)["statusCode"]);

            if (StatusCode == 1)
            {
                JArrayResult = JsonConvert.DeserializeObject<T>(JSONResponse);
                Response = "";
            }
            else
            {
                JArrayResult = null;
                Response = JSONResponse;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void APIpost<T>(string requestUri, T InputObject, out int StatusCode, out string Response)
    {
        try
        {
            HttpClient Client = new HttpClient();
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = Client.PostAsJsonAsync<T>(requestUri, InputObject).Result;

            string Content = response.Content.ReadAsStringAsync().Result;

            Response = JObject.Parse(Content)["response"].ToString();
            StatusCode = Convert.ToInt32(JObject.Parse(Content)["statusCode"]);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void APIpost<T>(string requestUri, T InputObject, out int StatusCode, out string Response,out string BookingId)
    {
        try
        {
            HttpClient Client = new HttpClient();
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = Client.PostAsJsonAsync<T>(requestUri, InputObject).Result;

            string Content = response.Content.ReadAsStringAsync().Result;

            Response = JObject.Parse(Content)["response"].ToString();
            StatusCode = Convert.ToInt32(JObject.Parse(Content)["statusCode"]);
            BookingId = JObject.Parse(Content)["bookingId"].ToString();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void APIput<T>(string requestUri, T InputObject, out int StatusCode, out string Response)
    {
        try
        {
            HttpClient Client = new HttpClient();
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = Client.PutAsJsonAsync<T>(requestUri, InputObject).Result;

            string Content = response.Content.ReadAsStringAsync().Result;

            Response = JObject.Parse(Content)["response"].ToString();
            StatusCode = Convert.ToInt32(JObject.Parse(Content)["statusCode"]);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void APIDelete(string requestUri, out int StatusCode, out string Response)
    {
        try
        {
            HttpClient Client = new HttpClient();
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = Client.DeleteAsync(requestUri).Result;

            string Content = response.Content.ReadAsStringAsync().Result;

            Response = JObject.Parse(Content)["response"].ToString();
            StatusCode = Convert.ToInt32(JObject.Parse(Content)["statusCode"]);
        }
        catch (Exception)
        {
            throw;
        }
    }
}