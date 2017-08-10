using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineageIIServerEmulator.DataBase
{
    public class WhereStatement
    {
        private List<SqlParameter> Paras;
        private Dictionary<string, object> Statements = new Dictionary<string, object>();
        public WhereStatement(string Key, object Value)
        {
            Statements.Add(Key, Value);
        }
        public WhereStatement Add(string Key, object Value)
        {
            Statements.Add(Key, Value);
            return this;
        }

        public new string ToString()
        {
            StringBuilder SB = new StringBuilder();
            foreach (var Item in Statements.Keys)
            {
                if (Statements[Item] is Array)
                {
                    var S = (string[])Statements[Item];
                    SB.Append(Item);
                    SB.Append("( ");
                    SB.Append(S[0]);
                    SB.Append(" @");
                    SB.Append(Item.Replace(".", ""));
                }
                else
                {
                    SB.Append(Item);
                    SB.Append(" = ");
                    SB.Append("@");
                    SB.Append(Item.Replace(".", ""));
                }
                SB.Append(" AND ");
            }
            var SS = SB.ToString();
            return SS.Substring(0, SS.Length - 4) + " )";
        }

        public SqlParameter[] GetParas()
        {
            Paras = new List<SqlParameter>();
            foreach (var Item in Statements.Keys)
            {
                if (Statements[Item] is Array)
                {
                    var S = (string[])Statements[Item];
                    Paras.Add(new SqlParameter("@" + Item.Replace(".", ""), S[1]));
                }
                else
                {
                    Paras.Add(new SqlParameter("@" + Item.Replace(".", ""), Statements[Item]));
                }
            }
            return Paras.ToArray();
        }
    }
}
