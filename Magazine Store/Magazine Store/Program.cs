using Magazine_Store.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Magazine_Store
{
    class Program
    {
        public static string token = null;

        static void Main(string[] args)
        {
            token = getToken();
            Console.WriteLine("Token: " + token);

            //get categories
            Console.Write("Get categories... ");
            Categories categories = getCategories();
            Console.WriteLine("done.");


            //get subscribers
            Console.Write("Get subscribers... ");
            Subscribers subscribers = getSubscribers();
            Console.WriteLine("done.");

            //get all magazines from each category
            Console.Write("Get magazines... ");
            List<Magazine> magazines = getAllMagazines(categories.data);
            Console.WriteLine("done.");

            //for subscribers who match all categories
            SubscribersList matchedSubscribers = getMatchedSubscribers(subscribers, categories, magazines);
            
            string jsonAnswer = JsonConvert.SerializeObject(matchedSubscribers);
            string response = postAnswer(jsonAnswer);

            parseAnswer(response);

            Console.WriteLine("Press any key to quit...");
            Console.ReadKey();
        }

        private static void parseAnswer(string response)
        {
            Answer answerData = JsonConvert.DeserializeObject<Answer>(response);
            Console.WriteLine("Answer Correct: " + answerData.data.answerCorrect);
            Console.WriteLine("Total Time: " + answerData.data.totalTime);
        }

        private static SubscribersList getMatchedSubscribers(Subscribers subscribers, Categories categories, List<Magazine> magazines)
        {
            SubscribersList matchedSubscribers = new SubscribersList();
            matchedSubscribers.subscribers = new List<string>();
            foreach (Subscriber subscriber in subscribers.data)
            {
                //build a list of categories this user subscribes to
                List<string> subscriberCategories = new List<string>();
                foreach (int id in subscriber.magazineIds)
                {
                    //get the category of this magazine
                    string category = magazines.Where(x => x.id == id).First().category;

                    //if it's not already on our list, add it
                    if (!subscriberCategories.Contains(category))
                    {
                        subscriberCategories.Add(category);
                    }
                }

                //if we have the same amount of categories, subscriber has one for each
                if (subscriberCategories.Count() == categories.data.Count())
                {
                    matchedSubscribers.subscribers.Add(subscriber.id);
                }
            }
            return matchedSubscribers;
        }
        
        //gets a raw JSON string from the API
        public static string getRawData(string url)
        {
            string hostUrl = "http://magazinestore.azurewebsites.net";
            WebClient client = new WebClient();
            string data = client.DownloadString(hostUrl + url);
            return data;
        }

        public static string postAnswer(string data)
        {
            string url = "http://magazinestore.azurewebsites.net/api/answer/" + token;
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            string response = client.UploadString(url, data);
            return response;
        }

        public static string getToken()
        {
            string data = getRawData("/api/token");
            Token tokenData = JsonConvert.DeserializeObject<Token>(data);
            if (tokenData.success && !String.IsNullOrEmpty(tokenData.token))
            {
                return tokenData.token;
            }
            else
            {
                throw new Exception("failed to get token");
            }
        }

        public static Categories getCategories()
        {
            string data = getRawData("/api/categories/" + token);
            Categories categoryData = JsonConvert.DeserializeObject<Categories>(data);
            if (categoryData.success && categoryData.data.Any())
            {
                return categoryData;
            }
            else
            {
                throw new Exception("failed to get categories");
            }
        }

        public static Magazines getMagazinesByCategory(string category)
        {
            string data = getRawData("/api/magazines/" + token + "/" + category);
            Magazines magazineData = JsonConvert.DeserializeObject<Magazines>(data);
            if (magazineData.success && magazineData.data.Any())
            {
                return magazineData;
            }
            else
            {
                throw new Exception("failed to get magazines");
            }
        }

        private static List<Magazine> getAllMagazines(List<string> data)
        {
            List<Magazine> magazines = new List<Magazine>();
            //using categories, loop through and get list of magazines
            List<string> magazineUrls = new List<string>();
            foreach (string category in data)
            {
                Magazines mags = getMagazinesByCategory(category);
                
                //build one uniform list of magazines for later consumption
                foreach (Magazine mag in mags.data)
                {
                    magazines.Add(mag);
                }
            }

            return magazines;
        }
        
        public static Subscribers getSubscribers()
        {
            string data = getRawData("/api/subscribers/" + token);
            Subscribers subscriberData = JsonConvert.DeserializeObject<Subscribers>(data);
            if (subscriberData.success && subscriberData.data.Any())
            {
                return subscriberData;
            }
            else
            {
                throw new Exception("failed to get subscribers");
            }
        }
    }
}
