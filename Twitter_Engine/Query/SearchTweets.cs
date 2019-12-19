/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2019, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */
using BH.oM.Twitter;
using BH.oM.Base;
using RestSharp;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using serialiser = BH.Engine.Serialiser;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Net;

namespace BH.Engine.Twitter
{
    public static partial class Query
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/
        public static TweetResults SearchTweets(Application application,string endpoint,SearchParameters searchParameters)
        {
            return new TweetResults()
            {
                Results = serialiser.Convert.FromJson(SearchPremium(application, endpoint, searchParameters)) as CustomObject
            };
        }
        /***************************************************/
        public static TweetResults SearchTweets(Credentials credentials, string query)
        {
            return new TweetResults()
            {
                Results = serialiser.Convert.FromJson(SearchStandard(credentials, query)) as CustomObject
            };
        }
        /***************************************************/
        private static string SearchPremium(Application application, string endpoint, SearchParameters searchParameters)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            RestClient client = new RestClient("https://api.twitter.com/1.1/tweets/search/fullarchive/production.json");
            RestRequest request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json", searchParameters.ToRequestBody(),ParameterType.RequestBody);
            request.AddParameter("Authorization", "Bearer " + application.BearerToken, ParameterType.HttpHeader);
            IRestResponse response = client.Execute(request);
            return response.ToResults("results");
        }
        /***************************************************/
        private static string SearchStandard(Credentials credentials,string query)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            string url = "https://api.twitter.com/1.1/search/tweets.json";
            OAuthParameters OAuthParameters = Create.OAuthParameters(credentials);

            string basestring = buildBaseString(query, OAuthParameters, url);
            OAuthParameters.AddOAuthSignature(basestring);

            //Tell Twitter we don't do the 100 continue thing
            ServicePointManager.Expect100Continue = false;
            string authheader = authorizationHeaderParams(OAuthParameters);

            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", authheader);
            request.AddQueryParameter("q", query);
            IRestResponse response = client.Execute(request);
            return response.ToResults("statuses");
        }
        /***************************************************/
        private static string buildBaseString(string query, OAuthParameters OAuthParameters, string url)
        {
            SortedDictionary<string, string> basestringParameters = new SortedDictionary<string, string>();
            basestringParameters.Add("q", query);
            basestringParameters.Add("oauth_version", OAuthParameters.OAuthVersion);
            basestringParameters.Add("oauth_consumer_key", OAuthParameters.OAuthConsumerKey);
            basestringParameters.Add("oauth_nonce", OAuthParameters.OAuthNonce);
            basestringParameters.Add("oauth_signature_method", OAuthParameters.OAuthSignatureMethod);
            basestringParameters.Add("oauth_timestamp", OAuthParameters.OAuthTimestamp);
            basestringParameters.Add("oauth_token",OAuthParameters.OAuthToken );
            //Build the signature string
            StringBuilder baseString = new StringBuilder();
            baseString.Append("GET" + "&");
            baseString.Append(EncodeCharacters(Uri.EscapeDataString(url.Split('?')[0]) + "&"));
            foreach (KeyValuePair<string, string> entry in basestringParameters)
            {
                baseString.Append(EncodeCharacters(Uri.EscapeDataString(entry.Key + "=" + entry.Value + "&")));
            }
            //Remove the trailing ambersand char last 3 chars - %26
            string finalBaseString = baseString.ToString().Substring(0, baseString.Length - 3);
            return finalBaseString;
        }
        private static string authorizationHeaderParams(OAuthParameters OAuthParameters)
        {
            StringBuilder authorizationHeaderParams = new StringBuilder();
            authorizationHeaderParams.Append("OAuth ");
            authorizationHeaderParams.Append("oauth_nonce=" + "\"" + Uri.EscapeDataString(OAuthParameters.OAuthNonce) + "\",");
            authorizationHeaderParams.Append("oauth_signature_method=" + "\"" + Uri.EscapeDataString(OAuthParameters.OAuthSignatureMethod) + "\",");
            authorizationHeaderParams.Append("oauth_timestamp=" + "\"" + Uri.EscapeDataString(OAuthParameters.OAuthTimestamp) + "\",");
            authorizationHeaderParams.Append("oauth_consumer_key=" + "\"" + Uri.EscapeDataString(OAuthParameters.OAuthConsumerKey) + "\",");
            if (!string.IsNullOrEmpty(OAuthParameters.OAuthToken))
            {
                authorizationHeaderParams.Append("oauth_token=" + "\"" + Uri.EscapeDataString(OAuthParameters.OAuthToken) + "\",");
            }    
            authorizationHeaderParams.Append("oauth_signature=" + "\"" + Uri.EscapeDataString(OAuthParameters.OAuthSignature) + "\",");
            authorizationHeaderParams.Append("oauth_version=" + "\"" + Uri.EscapeDataString(OAuthParameters.OAuthVersion) + "\"");
            return authorizationHeaderParams.ToString();
        }
        
    }
}

