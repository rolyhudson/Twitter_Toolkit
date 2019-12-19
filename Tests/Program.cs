
using BH.oM.Twitter;
using BH.Engine.Twitter;


namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            //standard();
            testStandard();
        }
    
            
        private static void testStandard()
        {
            Credentials credentials = new Credentials()
            {
                APIKey = "",
                APISecretKey = "",
                AccessToken = "",
                AcessTokenSecret = ""
            };
            TweetResults tweetResults = Query.SearchTweets(credentials, "twitterdev");
        }
        private static void testPreimum()
        {
            Credentials credentials = new Credentials()
            {
                APIKey = "",
                APISecretKey = ""

            };
            Application app = Query.AuthenticateApplication(credentials);
            SearchParameters searchParameters = new SearchParameters()
            {
                Query = "-is:retweet Liverpool lang:en",
                MaxResults = 10
            };
            string endpoint = "https://api.twitter.com/1.1/tweets/search/fullarchive/full.json";
            TweetResults tweetResults = Query.SearchTweets(app, endpoint, searchParameters);
        }
    }
}
