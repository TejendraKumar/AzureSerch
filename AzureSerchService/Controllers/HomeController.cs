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
                        var contect = item["content"] ?? string.Empty;
                        var encodedString = item["metadata_storage_path"].ToString();
                        var decodedString = Base64Decode( encodedString);
                        var isAlreadyDecoded = decodedString == string.Empty ? false : true;
                        modelobj.Add(new blobSearch()
                        {
                            Content = contect.ToString(),
                            ContentType = item["metadata_storage_content_type"].ToString(),
                            metadata_storage_path = isAlreadyDecoded? decodedString: encodedString,
                            IsAlreadyDecoded = isAlreadyDecoded,
                            SasToken = "P3N2PTIwMTgtMDMtMjgmc3M9YmZxdCZzcnQ9c2NvJnNwPXJ3ZGxhY3VwJnNlPTIwMTktMDgtMzFUMTg6MjQ6NDhaJnN0PTIwMTktMDctMTlUMTA6MjQ6NDhaJnNwcj1odHRwcyxodHRwJnNpZz1rWHN3Y1prJTJCeFE0aDQzazJESE5FUldtZyUyRll0VEg0Z3RucTlLaWpIcjR2USUzRA==",//"?sv=2018-03-28&ss=bfqt&srt=sco&sp=rwdlacup&se=2019-08-31T18:24:48Z&st=2019-07-19T10:24:48Z&spr=https,http&sig=kXswcZk%2BxQ4h43k2DHNERWmg%2FYtTH4gtnq9KijHr4vQ%3D"//(item["metadata_storage_path"].ToString())//GetDocUrl(Encoding.ASCII.GetBytes(item["metadata_storage_path"].ToString())),
                                                                                                                                                                                                                                                                 // metadata_content_type = (item["metadata_content_type"].ToString())                           
                        });

                    }
                }

               // ViewBag.Message = modelobj;

                return View(modelobj);
            }
        }

        private static string CreateContentUrl(string resourceUri, string sasToken)
        {

            //TimeSpan sinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1);
            //var week = 60 * 60 * 24 * 7;
            //var expiry = Convert.ToString((int)sinceEpoch.TotalSeconds + week);
            //string stringToSign = HttpUtility.UrlEncode(resourceUri) + "\n" + expiry;
            //HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            //var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
            //var sasToken = String.Format(CultureInfo.InvariantCulture, "SharedAccessSignature sr={0}&sig={1}&se={2}&skn={3}", HttpUtility.UrlEncode(resourceUri), HttpUtility.UrlEncode(signature), expiry, keyName);
            return resourceUri+sasToken;
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


        public static string Base64Decode( string encryptedtext)
        {
            var decodedStr = string.Empty;  
            //try
            //{

            //    try
            //    {
            //        encryptedtext = encryptedtext.Replace(" ", "+").Replace("-", "");
            //        var base64EncodedBytes = System.Convert.FromBase64String(encryptedtext);
            //        decodedStr = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                 

            //    }
            //    catch (Exception ex)
            //    {
            //        encryptedtext = encryptedtext.Replace(" ", "+").Replace("-", "");
            //        int mod4 = encryptedtext.Length % 4;
            //        if (mod4 > 0)
            //        {
            //            encryptedtext += new string('=', 4 - mod4);
            //        }
            //        byte[] encryptedBytes = Convert.FromBase64String(encryptedtext);
            //        decodedStr = System.Text.Encoding.UTF8.GetString(encryptedBytes);
            //    }
            //}
            //catch
            //{
                
            //}
            return decodedStr;

        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}