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
    /// Login response.
    /// </summary>
    [DataContract]
    public class LoginResponse
    {
        /// <summary>
        /// Role.
        /// </summary>
        [DataContract]
        public class Role
        {
            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>The name.</value>
            [DataMember]
            public string Name { get; set; }
        }

        /// <summary>
        /// User.
        /// </summary>
        [DataContract]
        public class User
        {
            /// <summary>
            /// Gets or sets the roles.
            /// </summary>
            /// <value>The roles.</value>
            [DataMember]
            public List<Role> Roles { get; set; }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>The name.</value>
            [DataMember]
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the status.
            /// </summary>
            /// <value>The status.</value>
            [DataMember]
            public string Status { get; set; }
        }

        /// <summary>
        /// Response data.
        /// </summary>
        [DataContract]
        public class ResponseData
        {
            /// <summary>
            /// Gets or sets the user.
            /// </summary>
            /// <value>The user.</value>
            [DataMember(Name = "user")]
            public User User { get; set; }

            /// <summary>
            /// Gets or sets the sign up date.
            /// </summary>
            /// <value>The sign up date.</value>
            [DataMember(Name = "signUpDate")]
            public string SignUpDate { get; set; }

            /// <summary>
            /// Gets or sets the session I.
            /// </summary>
            /// <value>The session I.</value>
            [DataMember(Name = "X-BB-SESSION")]
            public string SessionID { get; set; }
        }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        [DataMember]
        public string Result { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        [DataMember]
        public ResponseData Data { get; set; }

        /// <summary>
        /// Gets or sets the http code.
        /// </summary>
        /// <value>The http code.</value>
        [DataMember]
        public int HttpCode { get; set; }
    }
}

