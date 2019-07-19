using AzureSerchService.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AzureSerchService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(FormCollection frm1)
        {

            string name = Request.Form["tej"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://tejdemo.search.windows.net/indexes/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("api-key", "E75208ACFD828F3A8509CE33F70C6747");
                var responseTask = client.GetAsync("azureblob-index/docs?api-version=2019-05-06&search=" + name + "*");
                responseTask.Wait();

                JObject obj = new JObject();
                List<blobSearch> modelobj = new List<blobSearch>();
                JArray jarr = new JArray();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    var students = readTask.Result;
                    var ODataJSON = JsonConvert.DeserializeObject<JObject>(students);
                    ODataJSON.Property("@odata.context").Remove();
                    ODataJSON.Add("Terminal", ODataJSON["value"]);
                    obj = ODataJSON;
                    jarr = (JArray)obj["value"];
                    foreach (var item in jarr)
                    {
                        modelobj.Add(new blobSearch()
                        {
                            content = (item["content"].ToString()),
                            metadata_storage_path = createToken(Base64Decode((item["metadata_storage_path"].ToString())), "storageazuresearch11719", "ZjiD6VtZtC6FA1y0FCtfNKQRWSnBDiGyfbngxVKnCzUVOmd+Obm633eG0lgy2jqFKEJGX0aewjmCGyZGDKYd2w==")//(item["metadata_storage_path"].ToString())//GetDocUrl(Encoding.ASCII.GetBytes(item["metadata_storage_path"].ToString())),
                           // metadata_content_type = (item["metadata_content_type"].ToString())                           
                        });

                    }
                }

                ViewBag.Message = modelobj;

                return View();
            }
        }

        private static string createToken(string resourceUri, string keyName, string key)
        {

            TimeSpan sinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var week = 60 * 60 * 24 * 7;
            var expiry = Convert.ToString((int)sinceEpoch.TotalSeconds + week);
            string stringToSign = HttpUtility.UrlEncode(resourceUri) + "\n" + expiry;
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
            var sasToken = String.Format(CultureInfo.InvariantCulture, "SharedAccessSignature sr={0}&sig={1}&se={2}&skn={3}", HttpUtility.UrlEncode(resourceUri), HttpUtility.UrlEncode(signature), expiry, keyName);
            return sasToken;
        }

        public ActionResult About(FormCollection frm)
        {
            string name = Request.Form["tej"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://tejdemo.search.windows.net/indexes/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("api-key", "E75208ACFD828F3A8509CE33F70C6747");
                var responseTask = client.GetAsync("realestate-us-index/docs?api-version=2019-05-06&search="+name+"*");
                responseTask.Wait();

                JObject obj = new JObject();
                List<propertymodel> modelobj = new List<propertymodel>();
                JArray jarr= new JArray();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    var students = readTask.Result;
                    var ODataJSON = JsonConvert.DeserializeObject<JObject>(students);
                    ODataJSON.Property("@odata.context").Remove();
                    ODataJSON.Add("Terminal", ODataJSON["value"]);                   
                    obj = ODataJSON;
                    jarr = (JArray)obj["value"];
                    foreach (var item in jarr)
                    {
                        modelobj.Add(new propertymodel()
                        {
                            city = (item["city"].ToString()),
                            description = (item["description"].ToString()),
                            status = (item["status"].ToString()),
                            unit = (item["unit"].ToString()),
                            type = (item["type"].ToString()),
                            region = (item["region"].ToString()),
                            countryCode = (item["countryCode"].ToString()),
                            postCode = (item["postCode"].ToString()),
                            street = (item["street"].ToString()),
                            beds = Convert.ToInt16(item["beds"]),
                            baths = Convert.ToInt16(item["baths"]),
                        });
                        
                    }
                }

                ViewBag.Message = modelobj ;
                return View();
               
            }
        }


        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}