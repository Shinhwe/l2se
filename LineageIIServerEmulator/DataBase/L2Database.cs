using LineageIIServerEmulator.LoginServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineageIIServerEmulator.DataBase
{
    public class L2Database
    {
        private string _TableName = "";
        private string _Field = "*";
        private string _WHERE = "1 = 1";
        private string _Join = "";
        private string _Limit = "";
        private string _Group = "";
        private string _Order = "";
        private List<SqlParameter> _Params = new List<SqlParameter>();
        private SQLHelper _Helper = SQLHelper.GetInstance();


        public DataObject Find()
        {
            List<DataObject> DataList = Select();
            return DataList.Count > 0 ? DataList[0] : null;
        }
        public List<DataObject> Select()
        {
            List<DataObject> List = new List<DataObject>();
            StringBuilder SB = new StringBuilder();
            SB.Append("SELECT ");
            SB.Append(_Limit);
            SB.Append(_Field);
            SB.Append(" FROM ");
            SB.Append(_TableName);
            SB.Append(_Join);
            SB.Append(" WHERE ");
            SB.Append(_WHERE);
            SB.Append(_Group);
            SB.Append(_Order);
            var SQL = SB.ToString();
            try
            {
                DataTable DT = null;
                if (_Params.Count != 0)
                {
                    DT = _Helper.ExecuteQuery(SQL, _Params.ToArray());
                }
                else
                {
                    DT = _Helper.ExecuteQuery(SQL);
                }
                if (DT != null)
                {
                    if (DT.Rows.Count > 0)
                    {
                        foreach (DataRow DR in DT.Rows)
                        {
                            DataObject Data = new DataObject();
                            foreach (DataColumn Item in DT.Columns)
                            {
                                Data.Add(Item.ColumnName, DR[Item]);
                            }
                        }
                    }
                }
                return List;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public L2Database Table(string TableName)
        {
            _TableName = TableName;
            return this;
        }

        public L2Database Where(string _Where)
        {
            _WHERE = _Where;
            return this;
        }

        public L2Database Where(WhereStatement WhereStatement)
        {
            _WHERE += WhereStatement.ToString();
            _Params.AddRange(WhereStatement.GetParas());
            return this;
        }

        public L2Database Limit(int Value)
        {
            _Limit = " TOP " + Value.ToString();
            return this;
        }

        public L2Database Order(string Value)
        {
            _Order = " ORDER BY " + Value;
            return this;
        }
        public L2Database Join(string J)
        {
            return Join("INNER", J);
        }
        public L2Database Join(string JoinType, string Join)
        {
            _Join = " " + JoinType + " JOIN " + Join;
            return this;
        }

        public L2Database Field(string _Field)
        {
            this._Field = _Field;
            return this;
        }

        public L2Database Group(string Value)
        {
            _Group = " GROUP BY " + Value;
            return this;
        }

        public bool Add(DataObject Data)
        {
            return false;
        }

        public bool Update(DataObject Data, WhereStatement Where)
        {
            return false;
        }

        public bool Delete(WhereStatement Where)
        {
            return false;
        }

        public List<DataObject> Query(string SQLText, SqlParameter[] Params)
        {
            List<DataObject> List = new List<DataObject>();
            DataTable DT = null;
            DT = _Helper.ExecuteQuery(SQLText, Params.ToArray());
            if (DT != null)
            {
                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow DR in DT.Rows)
                    {
                        DataObject Data = new DataObject();
                        foreach (DataColumn Item in DT.Columns)
                        {
                            Data.Add(Item.ColumnName, DR[Item]);
                        }
                    }
                }
            }
            return List;
        }
    }
}
