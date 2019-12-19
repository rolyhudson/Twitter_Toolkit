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
using BH.oM.Base;

namespace BH.oM.Twitter
{
    public class OAuthParameters : BHoMObject
    {
        /***************************************************/
        /****            Public Properties              ****/
        /***************************************************/
        public string OAuthConsumerKey { get; set; } = "";
        public string OAuthToken { get; set; } = "";
        public string OAuthConsumerSecret { get; set; } = "";
        public string OAuthTokenSecret { get; set; } = "";
        public string OAuthSignatureMethod { get; set; } = "HMAC-SHA1";
        public string OAuthVersion { get; set; } = "1.0";
        public string OAuthNonce { get; set; } ="";
        public string OAuthTimestamp { get; set; } = "";
        public string OAuthSignature { get; set; } = "";
        
    }
}
