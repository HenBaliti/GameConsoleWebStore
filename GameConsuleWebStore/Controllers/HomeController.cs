using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GameConsuleWebStore.Models;
using Microsoft.AspNetCore.Http;
using GameConsuleWebStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.IO;

namespace GameConsuleWebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GameConsuleWebStoreContext _context;

        public HomeController(ILogger<HomeController> logger, GameConsuleWebStoreContext context)
        {
            _context = context;
            _logger = logger;
        }

        //---------------Twitter Api-------------------

        public void Tweet(string MessageData,string MessageName)
        {
            string PreMessage = "Hello Dear Customers this is another Tweet about our web store by a new customer.";
            string messageToPost =PreMessage+ "\n------------------------------\n" + "Message By: "+ MessageName+" \n"+MessageData+ "\n ------------------------------\nCome Shop Now on Our Store https://www.gamestore.com/";
            string twitterURL = "https://api.twitter.com/1.1/statuses/update.json";

            string oauth_consumer_key = "gb5XDMryCTWU8ikMKFnecAIi9";
            string oauth_consumer_secret = "Qonnzs8eR214CGTkDXV1CSZGvtKxKwgKI1QujCgx5ruK2Zd5dD";
            string oauth_token = "1325500759279071234-tlmHj9GGNvp3Fdm8nGJZ589NpFO3c7";
            string oauth_token_secret = "T7K61Bw7dQHNlIqrLtlzRGyIGreWyJCgVwGEXo74kLcVH";

            // set the oauth version and signature method
            string oauth_version = "1.0";
            string oauth_signature_method = "HMAC-SHA1";

            // create unique request details
            string oauth_nonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            System.TimeSpan timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            string oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

            // create oauth signature
            string baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" + "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}&status={6}";

            string baseString = string.Format(
                baseFormat,
                oauth_consumer_key,
                oauth_nonce,
                oauth_signature_method,
                oauth_timestamp, oauth_token,
                oauth_version,
                Uri.EscapeDataString(messageToPost)
            );

            string oauth_signature = null;
            using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(Uri.EscapeDataString(oauth_consumer_secret) + "&" + Uri.EscapeDataString(oauth_token_secret))))
            {
                oauth_signature = Convert.ToBase64String(hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes("POST&" + Uri.EscapeDataString(twitterURL) + "&" + Uri.EscapeDataString(baseString))));
            }

            // create the request header
            string authorizationFormat = "OAuth oauth_consumer_key=\"{0}\", oauth_nonce=\"{1}\", " + "oauth_signature=\"{2}\", oauth_signature_method=\"{3}\", " + "oauth_timestamp=\"{4}\", oauth_token=\"{5}\", " + "oauth_version=\"{6}\"";

            string authorizationHeader = string.Format(
                authorizationFormat,
                Uri.EscapeDataString(oauth_consumer_key),
                Uri.EscapeDataString(oauth_nonce),
                Uri.EscapeDataString(oauth_signature),
                Uri.EscapeDataString(oauth_signature_method),
                Uri.EscapeDataString(oauth_timestamp),
                Uri.EscapeDataString(oauth_token),
                Uri.EscapeDataString(oauth_version)
            );

            HttpWebRequest objHttpWebRequest = (HttpWebRequest)WebRequest.Create(twitterURL);
            objHttpWebRequest.Headers.Add("Authorization", authorizationHeader);
            objHttpWebRequest.Method = "POST";
            objHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            using (Stream objStream = objHttpWebRequest.GetRequestStream())
            {
                byte[] content = ASCIIEncoding.ASCII.GetBytes("status=" + Uri.EscapeDataString(messageToPost));
                objStream.Write(content, 0, content.Length);
            }

            var responseResult = "";

            try
            {
                //success posting
                WebResponse objWebResponse = objHttpWebRequest.GetResponse();
                StreamReader objStreamReader = new StreamReader(objWebResponse.GetResponseStream());
                responseResult = objStreamReader.ReadToEnd().ToString();
            }
            catch (Exception ex)
            {
                responseResult = "Twitter Post Error: " + ex.Message.ToString() + ", authHeader: " + authorizationHeader;
            }
        }

        //---------------^^^^^^^^^^ Twitter Api ^^^^^^^^^^-------------------



        public IActionResult SearchAutoComplete(string term)
        {
            var query = from p in _context.Product
                        where p.Name.Contains(term)
                        select new {id=p.ProductId ,label =p.Name ,value=p.ProductId };
            return Json(query.ToList());
        }

        public IActionResult Index(string messageAlert)
        {

            ViewBag.ShowUpdateEdit = messageAlert;

            //------------------YouTube Video-----------
            var random = new Random();
            List<string> YoutubeTextList = new List<string>();
            List<string> YoutubeEmbedList = new List<string>();
            YoutubeEmbedList.Add("https://www.youtube.com/embed/tuLAn9adQpI");
            YoutubeTextList.Add("FIFA 21 | Official Reveal Trailer");
            YoutubeEmbedList.Add("https://www.youtube.com/embed/bH1lHCirCGI");
            YoutubeTextList.Add("Official Call of Duty®: Modern Warfare®");
            YoutubeEmbedList.Add("https://www.youtube.com/embed/s7qB4IMJicc");
            YoutubeTextList.Add("ASSASSIN'S CREED Official Trailer");
            int index = random.Next(YoutubeEmbedList.Count);
            string EmbedYoutube = YoutubeEmbedList[index];
            string TextYoutube = YoutubeTextList[index];
            ViewBag.YouTubeUrl = EmbedYoutube;
            ViewBag.YouTubeText = TextYoutube;
            //------------------YouTube Video-----------

            return JustForYouByRecentOrders();
        }
        public IActionResult Index2()
        {
            return View("~/Views/Home/About.cshtml");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Sign-In users - can see a suita`ble products by thier recent orders.
        public IActionResult JustForYouByRecentOrders()
        {

            List<Product> result = new List<Product>();
            if (HttpContext.Session.GetString("UserId") != null)
            {
                var UserOrders = _context.Order.Where(p => p.User.Id.ToString().Equals(HttpContext.Session.GetString("UserId")));
                var UserProducts = _context.ProductOrder.Include(p=>p.Product).Join(UserOrders, up => up.OrderId, uo => uo.Id, (up, uo) => up).Select(p=>p.Product).ToList();

                if (UserOrders.ToList().Count != 0)
                {
                    var ConsuleCounter = UserProducts.GroupBy(x => x.ConsoleType)
.ToDictionary(y => y.Key, y => y.Count())
.OrderByDescending(z => z.Value);

                    var CategoryCounter = UserProducts.GroupBy(x => x.Category)
    .ToDictionary(y => y.Key, y => y.Count())
    .OrderByDescending(z => z.Value);

                    //None of them
                    if (ConsuleCounter == null && CategoryCounter == null)
                    {
                        ViewBag.ForYou = null;
                        return View("Index");
                    }
                    //only Category
                    if (ConsuleCounter == null && CategoryCounter.First().Value != 0)
                    {
                        result = _context.Product.Where(p => p.Category.Equals(CategoryCounter.First().Key)).ToList();
                        ViewBag.ForYou = result.Take(4);
                    }
                    //only Consule
                    if (ConsuleCounter.First().Value != 0 && CategoryCounter == null)
                    {
                        result = _context.Product.Where(p => p.ConsoleType.Equals(ConsuleCounter.First().Key)).ToList();
                        ViewBag.ForYou = result.Take(4);
                    }
                    //Consule + Category
                    if (ConsuleCounter.First().Value != 0 && CategoryCounter.First().Value != 0)
                    {
                        List<Product> lstProd1 = new List<Product>();
                        List<Product> lstProd2 = new List<Product>();
                        //result = ConsuleCounter.Join(CategoryCounter, con => con, cat => cat, (con, cat) => con);
                        lstProd1 = _context.Product.Where(p => p.ConsoleType.Equals(ConsuleCounter.First().Key) && p.Category.Equals(CategoryCounter.First().Key)).ToList();
                        if (lstProd1.Count < 4)
                        {
                            lstProd2 = _context.Product.Where(p => p.ConsoleType.Equals(ConsuleCounter.First().Key)).ToList();
                            //Erase the products that in lstProd1 From lstProd2
                            lstProd2 = lstProd2.Except(lstProd1).ToList();
                            //Putting the "Consule And Category" option prefer in front of the list
                            lstProd2.AddRange(lstProd1);
                            //Reversing the list for preffering the "Consule and Category" Option
                            lstProd2.Reverse();
                            //Taking the 4 Best For User Product
                            ViewBag.ForYou = lstProd2.Take(4);
                        }
                        else
                        {
                            ViewBag.ForYou = lstProd1.Take(4);
                        }
                    }
                }

                else
                {
                    ViewBag.ForYou = null;
                }


            }
            else
            {
                return View("Index");
            }
            return View("Index");
        }

    }
}
