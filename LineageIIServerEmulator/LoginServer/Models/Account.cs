using LineageIIServerEmulator.DataBase;
using LineageIIServerEmulator.Utils;
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
            L2Database DB = new L2Database();
            var UserModel = DB.Table("account").Where(new WhereStatement("login", _UserName)).Find();
            return UserModel != null;
        }

        public bool IsPassWordCorrect()
        {
            L2Database DB = new L2Database();
            var UserModel = DB.Table("account").Where(new WhereStatement("login", _UserName).Add("password", _PassWord)).Find();
            return UserModel != null;
        }

        public void Register()
        {
            L2Database DB = new L2Database();
            DataObject Object = new DataObject();
            Object.Add("login", _UserName);
            Object.Add("password", SHA512.Encrypt(_PassWord));
            DB.Table("account").Add(Object);
        }

        public void Login()
        {

        }

    }
}
