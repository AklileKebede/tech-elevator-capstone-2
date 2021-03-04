﻿using TenmoClient.Data;

namespace TenmoClient.DAL
{
    public interface IAccountDAO
    {
        Account GetAccount(string username,int accountId);
        Account GetAccount(int accountId);
    }
}