//
//  Copyright 2014  picture-vision
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using Newtonsoft.Json;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BaasBoxClientLib
{
    public class BaasClient
    {
        #region properties

        /// <summary>
        /// Gets the baas box app code.
        /// </summary>
        /// <value>The baas box app code.</value>
        public string BaasBoxAppCode
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the BaasBoxUrl.
        /// </summary>
        /// <value>The baas box URL.</value>
        public string BaasBoxUrl
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the last error.
        /// </summary>
        /// <value>The last error.</value>
        public ErrorResponse LastError
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the logged on user.
        /// </summary>
        /// <value>The logged on user.</value>
        public LoginResponse LoggedOnUser
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the SessionId.
        /// </summary>
        /// <value>The sessionID.</value>
        public string SessionId
        {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="BaasBoxClientLib.BaasBoxClient"/> class.
        /// Default values for BaasBox standard install are beeing used.
        /// </summary>
        public BaasClient()
            :this("http://localhost:9000", "1234567890")
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="BaasBoxClientLib.BaasClient"/> class.
        /// </summary>
        /// <param name="baasBoxUrl">Baas box URL.</param>
        /// <param name="appCode">App code.</param>
        public BaasClient(string baasBoxUrl, string appCode)
        {
            this.SessionId = String.Empty;
            this.BaasBoxUrl = baasBoxUrl;
            this.BaasBoxAppCode = appCode;
        }

        #region public methods

        /// <summary>
        /// Signs up.
        /// </summary>
        /// <returns>The up.</returns>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        public async Task<bool> SignUp(string username, string password)
        {
            SignUpData data = new SignUpData();
            data.UserName = username;
            data.Password = password;

            //TODO better validation of given data
            if (String.IsNullOrEmpty(username))
            {
                ErrorResponse errorResponse = new ErrorResponse();
                errorResponse.Message = "Empty Username not allowed";
                errorResponse.Result = "Empty Username";
                errorResponse.Method = "SignUp";

                this.SessionId = String.Empty;   
                this.LoggedOnUser = null;
                this.LastError = errorResponse;
                return false;
            }

            try
            {
                var client = new HttpClient {BaseAddress = new Uri(this.BaasBoxUrl)};
                client.DefaultRequestHeaders.Add("X-BAASBOX-APPCODE", this.BaasBoxAppCode);

                var content = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");
                var response = client.PostAsync(this.BaasBoxUrl + "/user", content).Result; 

                string response_str = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    LoginResponse loginResponse = JsonConvert.DeserializeObject<LoginResponse> (response_str);

                    this.SessionId = loginResponse.Data.SessionID; 
                    this.LoggedOnUser = loginResponse;
                    this.LastError = null;
                    return true;
                }
                else
                {
                    ErrorResponse errorResponse = JsonConvert.DeserializeObject<ErrorResponse> (response_str);
                    this.SessionId = String.Empty; 
                    this.LoggedOnUser = null;
                    this.LastError = errorResponse;
                    return false;
                }   
            }
            catch (Exception ex)
            {
                this.SessionId = String.Empty;   
                this.LoggedOnUser = null;
                this.LastError = ex.ToErrorResponse();
                return false;
            }
        }

        /// <summary>
        /// Login with the specified username and password.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        public async Task<bool> Login(string username, string password)
        {
            var pairs = new Dictionary<string, string> ();
            pairs.Add ("username", username);
            pairs.Add ("password", password);
            pairs.Add("appcode", this.BaasBoxAppCode);

            var content = new FormUrlEncodedContent(pairs);

            try
            {
                var client = new HttpClient {BaseAddress = new Uri(this.BaasBoxUrl)};
                var response = client.PostAsync(this.BaasBoxUrl + "/login", content).Result; 

                string response_str = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    LoginResponse loginResponse = JsonConvert.DeserializeObject<LoginResponse> (response_str);

                    this.SessionId = loginResponse.Data.SessionID; 
                    this.LoggedOnUser = loginResponse;
                    this.LastError = null;
                    return true;
                }
                else
                {
                    ErrorResponse errorResponse = JsonConvert.DeserializeObject<ErrorResponse> (response_str);
                    this.SessionId = String.Empty; 
                    this.LoggedOnUser = null;
                    this.LastError = errorResponse;
                    return false;
                }   
            }
            catch (Exception ex)
            {
                this.SessionId = String.Empty;   
                this.LoggedOnUser = null;
                this.LastError = ex.ToErrorResponse();
                return false;
            }
        }

        /// <summary>
        /// Logout this instance.
        /// </summary>
        public async Task<bool> Logout()
        {
            try
            {
                var client = new HttpClient {BaseAddress = new Uri(this.BaasBoxUrl)};
                client.DefaultRequestHeaders.Add("X-BB-SESSION", this.SessionId);
                client.DefaultRequestHeaders.Add("X-BAASBOX-APPCODE", this.BaasBoxAppCode);

                var response = client.PostAsync(this.BaasBoxUrl + "/logout", null).Result; 

                string response_str = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    this.LoggedOnUser = null;
                    this.LastError = null;
                    return true;
                }
                else
                {
                    ErrorResponse errorResponse = JsonConvert.DeserializeObject<ErrorResponse> (response_str);
                    this.SessionId = String.Empty; 
                    this.LoggedOnUser = null;
                    this.LastError = errorResponse;
                    return false;
                }   
            }
            catch (Exception ex)
            {
                this.SessionId = String.Empty;   
                this.LoggedOnUser = null;
                this.LastError = ex.ToErrorResponse();
                return false;
            }
        }

        /// <summary>
        /// Suspends the current logged on user.
        /// </summary>
        /// <returns>The me.</returns>
        public async Task<bool> SuspendMe()
        {
            try
            {
                var client = new HttpClient {BaseAddress = new Uri(this.BaasBoxUrl)};
                client.DefaultRequestHeaders.Add("X-BB-SESSION", this.SessionId);
                client.DefaultRequestHeaders.Add("X-BAASBOX-APPCODE", this.BaasBoxAppCode);

                var response = client.PutAsync(this.BaasBoxUrl + "/me/suspend", null).Result; 

                string response_str = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    this.LoggedOnUser = null;
                    this.LastError = null;
                    return true;
                }
                else
                {
                    ErrorResponse errorResponse = JsonConvert.DeserializeObject<ErrorResponse> (response_str);
                    this.SessionId = String.Empty; 
                    this.LoggedOnUser = null;
                    this.LastError = errorResponse;
                    return false;
                }   
            }
            catch (Exception ex)
            {
                this.SessionId = String.Empty;   
                this.LoggedOnUser = null;
                this.LastError = ex.ToErrorResponse();
                return false;
            }
        }


        public async Task<bool> ActivateUser(string username)
        {
            try
            {
                var client = new HttpClient {BaseAddress = new Uri(this.BaasBoxUrl)};
                client.DefaultRequestHeaders.Add("X-BB-SESSION", this.SessionId);
                client.DefaultRequestHeaders.Add("X-BAASBOX-APPCODE", this.BaasBoxAppCode);

                var response = client.PutAsync(this.BaasBoxUrl + "/admin/user/activate/" + username, null).Result; 

                string response_str = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    this.LoggedOnUser = null;
                    this.LastError = null;
                    return true;
                }
                else
                {
                    ErrorResponse errorResponse = JsonConvert.DeserializeObject<ErrorResponse> (response_str);
                    this.SessionId = String.Empty; 
                    this.LoggedOnUser = null;
                    this.LastError = errorResponse;
                    return false;
                }   
            }
            catch (Exception ex)
            {
                this.SessionId = String.Empty;   
                this.LoggedOnUser = null;
                this.LastError = ex.ToErrorResponse();
                return false;
            }
        }

        public async Task<bool> SuspendUser(string username)
        {
            try
            {
                var client = new HttpClient {BaseAddress = new Uri(this.BaasBoxUrl)};
                client.DefaultRequestHeaders.Add("X-BB-SESSION", this.SessionId);
                client.DefaultRequestHeaders.Add("X-BAASBOX-APPCODE", this.BaasBoxAppCode);

                var response = client.PutAsync(this.BaasBoxUrl + "/admin/user/suspend/" + username, null).Result; 

                string response_str = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    this.LoggedOnUser = null;
                    this.LastError = null;
                    return true;
                }
                else
                {
                    ErrorResponse errorResponse = JsonConvert.DeserializeObject<ErrorResponse> (response_str);
                    this.SessionId = String.Empty; 
                    this.LoggedOnUser = null;
                    this.LastError = errorResponse;
                    return false;
                }   
            }
            catch (Exception ex)
            {
                this.SessionId = String.Empty;   
                this.LoggedOnUser = null;
                this.LastError = ex.ToErrorResponse();
                return false;
            }
        }



        #endregion
    }
}

