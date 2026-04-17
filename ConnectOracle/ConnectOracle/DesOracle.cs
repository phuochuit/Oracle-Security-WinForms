using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectOracle
{
    internal class DesOracle
    {
        OracleConnection conn;

        public DesOracle(OracleConnection conn)
        {
            this.conn = conn;
        }

        public byte[] EncryptDES(string PlainText, string priKey)
        {
            try
            {
                string Function = "DES.encrypt";

                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = Function;
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter resultParam = new OracleParameter();
                resultParam.ParameterName = "@Result";
                resultParam.OracleDbType = OracleDbType.Raw;
                resultParam.Size = 500;
                resultParam.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(resultParam);

                OracleParameter str = new OracleParameter();
                str.ParameterName = "@p_plainText";
                str.OracleDbType = OracleDbType.Varchar2;
                str.Value = PlainText;
                str.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(str);

                OracleParameter k = new OracleParameter();
                k.ParameterName = "@priKey";
                k.OracleDbType = OracleDbType.Varchar2;
                k.Value = priKey;
                k.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(k);

                cmd.ExecuteNonQuery();

                if (resultParam.Value != DBNull.Value)
                {
                    OracleBinary ret = (OracleBinary)resultParam.Value;
                    return (byte[])ret.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().ToString());
            }

            return null;
        }

        public string DecryptDES(byte[] Encrypted, string priKey)
        {
            try
            {
                string Function = "DES.decrypt";

                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = Function;
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter resultParam = new OracleParameter();
                resultParam.ParameterName = "@Result";
                resultParam.OracleDbType = OracleDbType.Varchar2;
                resultParam.Size = 100;
                resultParam.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(resultParam);

                OracleParameter str = new OracleParameter();
                str.ParameterName = "@p_encryptedText";
                str.OracleDbType = OracleDbType.Raw;
                str.Value = Encrypted;
                str.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(str);

                OracleParameter k = new OracleParameter();
                k.ParameterName = "@priKey";
                k.OracleDbType = OracleDbType.Varchar2;
                k.Value = priKey;
                k.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(k);

                cmd.ExecuteNonQuery();

                if (resultParam.Value != DBNull.Value)
                {
                    OracleString ret = (OracleString)resultParam.Value;
                    return ret.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().ToString());
            }

            return null;
        }
    }
}
