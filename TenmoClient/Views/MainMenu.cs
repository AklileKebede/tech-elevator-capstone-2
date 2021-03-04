﻿using MenuFramework;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.DAL;
using TenmoClient.Data;

namespace TenmoClient.Views
{
    public class MainMenu : ConsoleMenu
    {

        // TODO: INITILIZAE DAO'S HERE, AND SET THEM IN THE CONSTRUCTOR
        private IAccountDAO accountDao;
        private ITransferDAO transferDao;

        public MainMenu(string api_url)
        {
            this.accountDao = new AccountApiDAO(api_url);
            this.transferDao = new TransferApiDAO(api_url);

            // TODO: NEED TO UPDATE THE CONSTRUCTOR TO HAVE THE DAO'S PASSED IN, AND SET THEM IN THE CONSTRUCTOR
            AddOption("View your current balance", ViewBalance)
                .AddOption("View your past transfers", ViewTransfers) // case 5 and 6
                .AddOption("View your pending requests", ViewRequests) // case 8 and 9
                .AddOption("Send TE bucks", SendTEBucks) // case 4
                .AddOption("Request TE bucks", RequestTEBucks) // case 7
                .AddOption("Log in as different user", Logout)
                .AddOption("Exit", Exit);
        }

        protected override void OnBeforeShow()
        {
            Console.WriteLine($"TE Account Menu for User: {UserService.GetUserName()}");
        }

        private MenuOptionResult ViewBalance()
        {
            try
            {
                // create a rest request to the /users/username/account# url, get back a balance
                int accountId = MainMenu.GetInteger("Please enter your account Id: ");

                // TODO: THIS WILL CALL THE ACCOUNTDAO AND IT WILL RETURN AN ACCOUNT (GETACCOUNT METHOD).  WE WILL USE THAT ACCOUNT TO REFERENCE THE BALANCE BY ACCOUNT.BALANCE
                // UserService.GetUserName
                Account account = accountDao.GetAccount(accountId);
                //Account account1 = accountDao.GetAccount(UserService.GetUserName(), accountId);
                Console.WriteLine($"Your account {accountId} has the balance of: {account.Balance}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult ViewTransfers()
        {
            Console.WriteLine("Not yet implemented!");

            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult ViewRequests()
        {
            Console.WriteLine("Not yet implemented!");
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult SendTEBucks()
        {
            try
            {
                List<API_User> users = transferDao.GetUsers();
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("Users");
                Console.WriteLine("ID                           Name");
                Console.WriteLine("-------------------------------------------");
                foreach (API_User user in users)
                {
                    Console.WriteLine($"{user.UserId}                           {user.Username}");
                }
                Console.WriteLine("---------");
                Console.WriteLine("");
                bool badInput = true;
                int userId;

                //loop until we find a user id that is actually in the list
                while (badInput)
                {
                    userId = GetInteger("Enter ID of user you are sending to (0 to cancel): ");
                    if (userId == 0)
                    {
                        return MenuOptionResult.WaitAfterMenuSelection;
                    }
                    foreach (API_User user in users)
                    {
                        if (user.UserId == userId)
                        {
                            badInput = false;
                        }
                    }
                    if (badInput)
                    {
                        Console.WriteLine("Please enter a valid User Id");
                    }
                }

                // THIS NEEDS TO BE ADDED IN AND SLIGHTLY CHANGED IF WE NEED TO GO THROUGH AND VALIDATE ACCOUNT NUMBERS OF BOTH SENDER AND RECEIVER
                //badInput = true;
                //int accountId;
                //while (badInput)
                //{
                //    userId = GetInteger("Enter your account ID of money to be taken out: ");
                //    if (userId == 0)
                //    {
                //        return MenuOptionResult.WaitAfterMenuSelection;
                //    }
                //    foreach (API_User user in users)
                //    {
                //        if (user.UserId == userId)
                //        {
                //            badInput = false;
                //        }
                //    }
                //    if (badInput)
                //    {
                //        Console.WriteLine("Please enter a valid User Id");
                //    }
                //}

                // TODO: PUT IN LOGIC HERE TO MAKE SURE AMOUNT IS GREATER THAN 0
                decimal amount = GetInteger("Enter amount: ");
                Account account = accountDao.GetAccount(UserService.GetUserId());
                if (amount > account.Balance)
                {
                    Console.WriteLine("Insufficient balance");
                    return MenuOptionResult.WaitAfterMenuSelection;
                }
                else
                {

                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult RequestTEBucks()
        {
            try
            {
                List<API_User> users = transferDao.GetUsers();
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("Users");
                Console.WriteLine("ID                           Name");
                Console.WriteLine("-------------------------------------------");
                foreach (API_User user in users)
                {
                    Console.WriteLine($"{user.UserId}                           {user.Username}");
                }
                Console.WriteLine("---------");
                Console.WriteLine("");
                bool badInput = true;
                int userId = -1;

                //loop until we find a user id that is actually in the list
                while (badInput)
                {
                    userId = GetInteger("Enter ID of user you are requesting from (0 to cancel): ");
                    if (userId == 0)
                    {
                        return MenuOptionResult.WaitAfterMenuSelection;
                    }
                    foreach (API_User user in users)
                    {
                        if (user.UserId == userId)
                        {
                            badInput = false;
                        }
                    }
                    if (badInput)
                    {
                        Console.WriteLine("Please enter a valid User Id");
                    }
                }

                // THIS NEEDS TO BE ADDED IN AND SLIGHTLY CHANGED IF WE NEED TO GO THROUGH AND VALIDATE ACCOUNT NUMBERS OF BOTH SENDER AND RECEIVER
                //badInput = true;
                //int accountId;
                //while (badInput)
                //{
                //    userId = GetInteger("Enter your account ID of money to be taken out: ");
                //    if (userId == 0)
                //    {
                //        return MenuOptionResult.WaitAfterMenuSelection;
                //    }
                //    foreach (API_User user in users)
                //    {
                //        if (user.UserId == userId)
                //        {
                //            badInput = false;
                //        }
                //    }
                //    if (badInput)
                //    {
                //        Console.WriteLine("Please enter a valid User Id");
                //    }
                //}

                // TODO: PUT IN LOGIC HERE TO MAKE SURE AMOUNT IS GREATER THAN 0
                decimal amount = GetInteger("Enter amount: ");
                Account account = accountDao.GetAccount(userId);
                if (amount > account.Balance)
                {
                    Console.WriteLine("Insufficient balance");
                    return MenuOptionResult.WaitAfterMenuSelection;
                }
                else
                {

                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return MenuOptionResult.WaitAfterMenuSelection;

        }

        private MenuOptionResult Logout()
        {
            UserService.SetLogin(new API_User()); //wipe out previous login info
            return MenuOptionResult.CloseMenuAfterSelection;
        }

    }
}
