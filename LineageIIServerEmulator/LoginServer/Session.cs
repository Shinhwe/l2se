using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineageIIServerEmulator.LoginServer
{
    public class LoginSession
    {
        private int _PlayOkId1;
        private int _PlayOkId2;
        private int _LoginOkId1;
        private int _LoginOkId2;

        private int _HashCode;
        public LoginSession()
        {
            Random Rnd = new Random();
            _PlayOkId1 = Rnd.Next(100);
            _PlayOkId2 = Rnd.Next(100);
            _LoginOkId1 = Rnd.Next(100);
            _LoginOkId2 = Rnd.Next(100);

            int HashCode = _PlayOkId1;
            HashCode *= 17;
            HashCode += _PlayOkId2;
            HashCode *= 37;
            HashCode += _LoginOkId1;
            HashCode *= 51;
            HashCode += _LoginOkId2;

            _HashCode = HashCode;

        }

        public int GetPlayId1()
        {
            return _PlayOkId1;
        }
        public int GetPlayId2()
        {
            return _PlayOkId2;
        }
        public int GetLoginId1()
        {
            return _LoginOkId1;
        }
        public int GetLoginId2()
        {
            return _LoginOkId2;
        }

        public override int GetHashCode()
        {
            return _HashCode;
        }
    }
}
