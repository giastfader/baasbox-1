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
using BaasBoxClientLib;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BaasBoxClient.SampleApp
{
    public class App
    {
        private static BaasClient baasClient = new BaasClient("http://134.119.1.115:9000", "d54a1e82da9bb579096ec");

        private static Entry usernameEntry;
        private static Entry passwordEntry;
        private static Button signUpButton;
        private static Button loginButton;
        private static Button logoutButton;
        private static Button suspendMeButton;
        private static Button activateUserButton;
        private static Button suspendUserButton;
        private static Label statusLabel;

        public static Page GetMainPage()
        {	
            usernameEntry = new Entry {Placeholder = "Username"};
            passwordEntry = new Entry {
                Placeholder = "Password",
                IsPassword = true
            };

            signUpButton = new Button
                {
                    Text = "SignUp",
                    HorizontalOptions = LayoutOptions.Center
                };
            signUpButton.Clicked += HandleSignUpClicked;

            loginButton = new Button
                {
                    Text = "Login",
                    HorizontalOptions = LayoutOptions.Center
                };
            loginButton.Clicked += HandleLoginClicked;

            logoutButton = new Button
                {
                    Text = "Logout",
                    HorizontalOptions = LayoutOptions.Center
                };
            logoutButton.Clicked += HandleLogoutClicked;

            suspendMeButton = new Button
                {
                    Text = "Suspend Me",
                    HorizontalOptions = LayoutOptions.Center
                };
            suspendMeButton.Clicked += HandleSuspendMeClicked;

            activateUserButton = new Button
                {
                    Text = "Activate User",
                    HorizontalOptions = LayoutOptions.Center
                };
            activateUserButton.Clicked += HandleActivateUserClicked;

            suspendUserButton = new Button
                {
                    Text = "Suspend User",
                    HorizontalOptions = LayoutOptions.Center
                };
            suspendUserButton.Clicked += HandleSuspendUserClicked;

            statusLabel = new Label
                {
                    Text = "not logged in",
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };

            return new ContentPage
            { 
                Content = GetLayout()
            };
        }

        private static StackLayout GetLayout()
        {
            StackLayout stackLayout = new StackLayout
            {
                Spacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                    usernameEntry,
                    passwordEntry,
                    signUpButton,
                    loginButton,
                    logoutButton,
                    suspendMeButton,
                    activateUserButton,
                    suspendUserButton,
                    statusLabel
                }
            };

            if (Device.OS == TargetPlatform.iOS) {
                // move layout under the status bar
                stackLayout.Padding = new Thickness (0, 20, 0, 0);
            }
//            if (Device.OS == TargetPlatform.iOS) {
//                label.Font = Font.OfSize("MarkerFelt-Thin", NamedSize.Medium);
//            } else {
//                label.Font = Font.SystemFontOfSize(NamedSize.Medium);
//            }

            return stackLayout;
        }

        private static void HandleSignUpClicked (object sender, EventArgs e)
        {
            if (baasClient.SignUp(usernameEntry.Text, passwordEntry.Text).Result)
            {
                statusLabel.Text = "SignUp succeeded!";    
            }
            else
            {
                statusLabel.Text = "SignUp failed! \n";
                statusLabel.Text += baasClient.LastError.Message;
            }

        }

        private static void HandleLoginClicked (object sender, EventArgs e)
        {
            if (baasClient.Login(usernameEntry.Text, passwordEntry.Text).Result)
            {
                statusLabel.Text = "Logged in!";    
            }
            else
            {
                statusLabel.Text = "Login failed! \n";
                statusLabel.Text += baasClient.LastError.Message;
            }

        }

        private static void HandleLogoutClicked (object sender, EventArgs e)
        {
            if (baasClient.Logout().Result)
            {
                statusLabel.Text = "Logged out!";
            }
            else
            {
                statusLabel.Text = "Logout failed!";
                statusLabel.Text += baasClient.LastError.Message;
            }
        }

        private static void HandleSuspendMeClicked (object sender, EventArgs e)
        {
            if (baasClient.SuspendMe().Result)
            {
                statusLabel.Text = "Suspended myself!";
            }
            else
            {
                statusLabel.Text = "Suspend failed!";
                statusLabel.Text += baasClient.LastError.Message;
            }
        }

        private static void HandleActivateUserClicked (object sender, EventArgs e)
        {
            if (baasClient.ActivateUser(usernameEntry.Text).Result)
            {
                statusLabel.Text = String.Format("ActivateUser [0] succeeded!", usernameEntry.Text);    
            }
            else
            {
                statusLabel.Text = "SignUp failed! \n";
                statusLabel.Text += baasClient.LastError.Message;
            }
        }

        private static void HandleSuspendUserClicked (object sender, EventArgs e)
        {
            if (baasClient.SuspendUser(usernameEntry.Text).Result)
            {
                statusLabel.Text = String.Format("SuspendUser [0] succeeded!", usernameEntry.Text);    
            }
            else
            {
                statusLabel.Text = "SignUp failed! \n";
                statusLabel.Text += baasClient.LastError.Message;
            }
        }
    }
}

