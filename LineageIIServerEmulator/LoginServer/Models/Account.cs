using LineageIIServerEmulator.DAO;
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
            this._UserName = UserName;
            this._PassWord = PassWord;
        }

        public bool IsExist()
        {
            return false;
        }

        public bool IsPassWordCorrect()
        {
            return AccountDAO.GetInstance().CheckPassword(_UserName, _PassWord);
        }

        public void Register()
        {

        }

        public void Login()
        {

        }

    }
}
