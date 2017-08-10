using LineageIIServerEmulator.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineageIIServerEmulator.LoginServer.Models
{
    public class Account
    {
        string _UserName;
        string _PassWord;
        public Account(string UserName, string PassWord)
        {
            _UserName = UserName;
            _PassWord = PassWord;
        }

        public bool IsExist()
        {
            return false;
        }

        public bool IsPassWordCorrect()
        {
            L2Database DB = new L2Database();
            var UserModel = DB.Table("account").Where(new WhereStatement("login", _UserName).Add("password", _PassWord)).Find();
            return UserModel != null;
        }

        public void Register()
        {

        }

        public void Login()
        {

        }

    }
}
