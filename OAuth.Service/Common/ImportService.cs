using OAuth.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Service.Common
{
    public class ImportService
    {
        public IEnumerable<ItemMaterial> ImportTemplet(string path, Item model, string name, out string message)
        {
            message = null;
            string suffix = Path.GetExtension(path);
            StringBuilder build = new StringBuilder(500);
            if (suffix.Equals(".xlsx"))
            {
                build.Append("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=");
                build.Append(path);
                build.Append(";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";");
            }
            else if (suffix.Equals(".xls"))
            {
                build.Append("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=");
                build.Append(path);
                build.Append(";Extended Properties='Excel 8.0;HDR=No;IMEX=1'");
            }
            else
            {
                message = "模板文件格式错误";
                return null;
            }
            DataTable dataTable = null;
            
            try
            {
                
                OleDbDataAdapter oledb = new OleDbDataAdapter(string.Format("select * from [{0}$]", name), build.ToString());
                dataTable = new DataTable();
                oledb.Fill(dataTable);
                if (dataTable==null || dataTable.Rows.Count==0)
                {
                    message = "模板内容为空！";
                    return null;
                }
                int count = dataTable.Rows.Count;
                List<ItemMaterial> list = new List<ItemMaterial>();
                if (model.TempMode == 1 || model.TempMode == 4)
                {
                    for (int i = 2; i < count; i++)
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(dataTable.Rows[i][1])))
                        {
                            continue;
                        }
                        DataRow rw = dataTable.Rows[i];
                        list.Add(new ItemMaterial()
                        {
                            ItemID = Convert.ToInt32(model.Id),
                            Mode = Convert.ToString(rw[1] == DBNull.Value ? "" : rw[1]),
                            Name = Convert.ToString(rw[2] == DBNull.Value ? "" : rw[2]),
                            Spec = Convert.ToString(rw[3] == DBNull.Value ? "" : rw[3]),
                            Texture = Convert.ToString(rw[4] == DBNull.Value ? "" : rw[4]),
                            Norm = Convert.ToString(rw[5] == DBNull.Value ? "" : rw[5]),
                            Extent = Convert.ToDouble(rw[6] == DBNull.Value ? 0 : rw[6]),
                            Sum = Convert.ToInt32(rw[7] == DBNull.Value ? 0 : rw[7]),
                            TotalWeight = Convert.ToDouble(rw[8] == DBNull.Value ? 0 : rw[8]),
                            RealPrice = Convert.ToDouble(rw[9] == DBNull.Value ? 0 : rw[9]),
                            ReachTime = Convert.ToString(rw[10] == DBNull.Value ? "" : rw[10]),
                            PriceUnit = Convert.ToString(rw[11] == DBNull.Value ? "" : rw[11]),
                            PriceMode = Convert.ToString(rw[12] == DBNull.Value ? "" : rw[12]),
                            PayMode = Convert.ToString(rw[13] == DBNull.Value ? "" : rw[13]),
                            IsTax = Convert.ToString(rw[14] == DBNull.Value ? "" : rw[14]),
                            Term = Convert.ToString(rw[15] == DBNull.Value ? "" : rw[15]),
                            Memo = Convert.ToString(rw[16] == DBNull.Value ? "" : rw[16]),
                            IsEnabled = true
                        });
                    }
                }
                if (model.TempMode == 2)
                {
                    for (int i = 2; i < count; i++)
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(dataTable.Rows[i][1])) || string.IsNullOrEmpty(Convert.ToString(dataTable.Rows[i][2])))
                        {
                            continue;
                        }
                        DataRow rw = dataTable.Rows[i];
                        list.Add(new ItemMaterial()
                        {
                            ItemID = Convert.ToInt32(model.Id),
                            Mode = Convert.ToString(rw[1] == DBNull.Value ? "" : rw[1]),
                            Name = Convert.ToString(rw[2] == DBNull.Value ? "" : rw[2]),
                            Spec = Convert.ToString(rw[3] == DBNull.Value ? "" : rw[3]),
                            Sum = Convert.ToInt32(rw[4] == DBNull.Value ? 0 : rw[4]),
                            PriceUnit = Convert.ToString(rw[5] == DBNull.Value ? "" : rw[5]),
                            RealPrice = Convert.ToDouble(rw[6]== DBNull.Value ? 0 : rw[6]),
                            ReachTime = Convert.ToString(rw[7] == DBNull.Value ? "" : rw[7]),
                            UsePlace = Convert.ToString(rw[8] == DBNull.Value ? "" : rw[8]),
                            IsTax = Convert.ToString(rw[9] == DBNull.Value ? "" : rw[9]),
                            Term = Convert.ToString(rw[10]== DBNull.Value ? "":rw[10]),
                            Memo = Convert.ToString(rw[11] == DBNull.Value ? "" : rw[11]),
                            IsEnabled = true
                        });
                    }
                }
                if (model.TempMode == 3 || model.TempMode == 5)
                {
                    for (int i = 2; i < count; i++)
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(dataTable.Rows[i][1])))
                        {
                            continue;
                        }
                        DataRow rw = dataTable.Rows[i];
                        list.Add(new ItemMaterial()
                        {
                            ItemID = Convert.ToInt32(model.Id),
                            Mode = Convert.ToString(rw[1] == DBNull.Value ? "" : rw[1]),
                            Name = Convert.ToString(rw[2] == DBNull.Value ? "" : rw[2]),
                            Spec = Convert.ToString(rw[3] == DBNull.Value ? "" : rw[3]),
                            Texture = Convert.ToString(rw[4] == DBNull.Value ? "" : rw[4]),
                            Norm = Convert.ToString(rw[5] == DBNull.Value ? "" : rw[5]),
                            Sum = Convert.ToInt32(rw[6] == DBNull.Value ? 0 : rw[6]),
                            TotalWeight = Convert.ToDouble(rw[7] == DBNull.Value ? 0 : rw[7]),
                            RealPrice = Convert.ToDouble(rw[8]== DBNull.Value ? 0:rw[8]),
                            ReachTime = Convert.ToString(rw[9] == DBNull.Value ? "" : rw[9]),
                            PriceUnit = Convert.ToString(rw[10] == DBNull.Value ? "" : rw[10]),
                            PriceMode = Convert.ToString(rw[11] == DBNull.Value ? "" : rw[11]),
                            PayMode = Convert.ToString(rw[12] == DBNull.Value ? "" : rw[12]),
                            IsTax = Convert.ToString(rw[13] == DBNull.Value ? "" : rw[13]),
                            Term = Convert.ToString(rw[14]== DBNull.Value ? "":rw[14]),
                            Memo = Convert.ToString(rw[15] == DBNull.Value ? "" : rw[15]),
                            IsEnabled = true
                        });
                    }
                }
                File.Delete(path);
                return list;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return null;
            }
        }
    }
}
