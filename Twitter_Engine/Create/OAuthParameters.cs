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
using System;
using System.Security.Cryptography;
using System.Text;

namespace BH.Engine.Twitter
{
    public static partial class Create
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/
        public static OAuthParameters OAuthParameters(Credentials credentials)
        {
            return new OAuthParameters()
            {
                OAuthConsumerKey = credentials.APIKey,
                OAuthToken = credentials.AccessToken,
                OAuthConsumerSecret = credentials.APISecretKey,
                OAuthTokenSecret = credentials.AcessTokenSecret,
                OAuthNonce = OAuthnonce(),
                OAuthTimestamp = OAthtimestamp()
            };
            
        }
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/
        private static string OAuthnonce()
        {
            return System.Convert.ToBase64String(new System.Text.ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));

        }
        private static string OAthtimestamp()
        {
            TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            string OAuthtimestamp = System.Convert.ToInt64(timeSpan.TotalSeconds).ToString();

            return OAuthtimestamp;
        }
        
    }
}
