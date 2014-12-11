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
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BaasBoxClientLib
{
    /// <summary>
    /// Error response.
    /// </summary>
    [DataContract]
    public class ErrorResponse
    {
        /// <summary>
        /// Header of the Error Response.
        /// </summary>
        [DataContract]
        public class Header
        {
            /// <summary>
            /// Gets or sets the accept.
            /// </summary>
            /// <value>The accept.</value>
            [DataMember(Name="Accept")]
            public List<string> Accept { get; set; }

            /// <summary>
            /// Gets or sets the host.
            /// </summary>
            /// <value>The host.</value>
            [DataMember(Name="Host")]
            public List<string> Host { get; set; }

            /// <summary>
            /// Gets or sets the user agent.
            /// </summary>
            /// <value>The user agent.</value>
            [DataMember(Name = "User-Agent")]
            public List<string> UserAgent { get; set; }

            /// <summary>
            /// Gets or sets the SESSION.
            /// </summary>
            /// <value>The SESSIO.</value>
            [DataMember(Name="X-BB-SESSION")]
            public List<string> SESSION { get; set; }
        }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        [DataMember(Name="result")]
        public string Result { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        [DataMember(Name="message")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the resource.
        /// </summary>
        /// <value>The resource.</value>
        [DataMember(Name="resource")]
        public string Resource { get; set; }

        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        /// <value>The method.</value>
        [DataMember(Name="method")]
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets the request header.
        /// </summary>
        /// <value>The request header.</value>
        [DataMember(Name="request_header")]
        public Header RequestHeader { get; set; }

        /// <summary>
        /// Gets or sets the API version.
        /// </summary>
        /// <value>The API version.</value>
        [DataMember(Name="API_version")]
        public string APIVersion { get; set; }

        /// <summary>
        /// Gets or sets the http code.
        /// </summary>
        /// <value>The http code.</value>
        [DataMember(Name="http_code")]
        public int HttpCode { get; set; }

        /// <summary>
        /// Gets or sets the BB code.
        /// </summary>
        /// <value>The BB code.</value>
        [DataMember(Name="bb_code")]
        public string BBCode { get; set; }
    }

    /// <summary>
    /// Error response extension class.
    /// </summary>
    public static class ErrorResponseExtension
    {
        /// <summary>
        /// converts an exception to error response.
        /// </summary>
        /// <returns>The error response.</returns>
        /// <param name="ex">Ex.</param>
        public static ErrorResponse ToErrorResponse(this System.Exception ex)
        {
            ErrorResponse errorResponse = new ErrorResponse();
            errorResponse.Message = ex.Message;
            errorResponse.Result = "Exception";
            errorResponse.Method = "Exception";
            errorResponse.Resource = ex.StackTrace;

            return errorResponse;
        }
    }
}

