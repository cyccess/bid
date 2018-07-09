using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.tool.xml;
using OAuth.Domain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Service.Common
{
    public class PdfService
    {
        public void DownloadPdf(string htmlText)
        {
            //WebClient wc = new WebClient();
            //从网址下载Html字串
            //string htmlText = wc.DownloadString(url);
            //File.WriteAllBytes($"/UploadFile/pdf/合同{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.pdf", pdfFile);
        }

        public string ContractToPDF1(Supplier supplier, Item it, IEnumerable<Contract> list, string norm, string weight, string mode, string freight, Purchaser client,string place,string date,string data,List<string[]> lt)
        {
            string contractNo = $"{it.ItemNo}-{supplier.Id.ToString().PadLeft(5, '0')}";
            int rwCount = list.Count();
            //载入字体
            //"UniGB-UCS2-H" "UniGB-UCS2-V"是简体中文，分别表示横向字 和 // 纵向字 //" STSong-Light"是字体名称 
            BaseFont baseFont = BaseFont.CreateFont("C:\\WINDOWS\\FONTS\\STSONG.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 10);
            iTextSharp.text.Font fontTitle = new iTextSharp.text.Font(baseFont, 15);
             //设置边界
            Document document = new Document(PageSize.A4,0,0,0,0);
            string name = $"合同{ DateTime.Now.ToString("yyyyMMddHHmmssfff")}.pdf";
            PdfWriter.GetInstance(document, new FileStream(AppDomain.CurrentDomain.BaseDirectory + $"/UploadFile/pdf/{name}", FileMode.Create));
            // 添加文档信息
            document.AddTitle("买卖合同");
            //document.AddSubject("Contract of PDFInfo");
            //document.AddKeywords("Info, PDF, Contract");
            //document.AddCreator("Contract");
            document.AddAuthor("");
            document.Open();
            // 添加文档内容
            PdfPTable table = new PdfPTable(12);
            //table.SetWidths(new float[] { 200f, 100f, 100f, 100f, 150f, 100f, 100f, 110f, 150f, 100f, 150f, 140f });
            table.SetWidths(new float[] { 82f, 45f, 45f, 35f, 50f, 35f, 35f, 47f, 58f, 50f, 58f, 50f });

            PdfPCell cell = new PdfPCell(new Phrase("买卖合同", fontTitle));
            cell.Border = 0;
            cell.Colspan = 12;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.AddCell(cell);

            PdfPCell h1 = new PdfPCell(new Phrase("卖方：", font));
            h1.Border = 0;
            h1.Colspan = 1;
            table.AddCell(h1);

            PdfPCell h2 = new PdfPCell(new Phrase(supplier.SupplierName, font));
            h2.Border = 0;
            h2.Colspan = 5;
            table.AddCell(h2);

            PdfPCell h3 = new PdfPCell(new Phrase("合同编号：", font));
            h3.Border = 0;
            h3.Colspan = 2;
            table.AddCell(h3);

            PdfPCell h4 = new PdfPCell(new Phrase(contractNo, font));
            h4.Border = 0;
            h4.Colspan = 4;
            table.AddCell(h4);

            PdfPCell h5 = new PdfPCell(new Phrase("买方：", font));
            h5.Border = 0;
            h5.Colspan = 1;
            table.AddCell(h5);

            PdfPCell h6 = new PdfPCell(new Phrase(client.PurchaserName,font));
            h6.Border = 0;
            h6.Colspan = 5;
            table.AddCell(h6);

            PdfPCell h7 = new PdfPCell(new Phrase("合同签订地：", font));
            h7.Border = 0;
            h7.Colspan = 2;
            table.AddCell(h7);

            PdfPCell h8 = new PdfPCell(new Phrase(it.SignPlace, font));
            h8.Border = 0;
            h8.Colspan = 4;
            table.AddCell(h8);

            PdfPCell h9 = new PdfPCell(new Phrase(""));
            h9.Border = 0;
            h9.Colspan = 6;
            table.AddCell(h9);

            PdfPCell h10 = new PdfPCell(new Phrase("签订时间：", font));
            h10.Border = 0;
            h10.Colspan = 2;
            table.AddCell(h10);

            PdfPCell h11 = new PdfPCell(new Phrase(it.SignTime.ToString("yyyy年MM月dd日"), font));
            h11.Border = 0;
            h11.Colspan = 4;
            table.AddCell(h11);
            double mt = 0, tl = 0;
            if (rwCount <= 8)
            {
                table.AddCell(new Phrase("品名", font));
                table.AddCell(new Phrase("材质", font));
                table.AddCell(new Phrase("规格", font));
                table.AddCell(new Phrase("长度", font));
                table.AddCell(new Phrase("标准", font));
                table.AddCell(new Phrase("数量", font));
                table.AddCell(new Phrase("单位", font));
                table.AddCell(new Phrase("吨位", font));
                table.AddCell(new Phrase("单价（元）", font));
                table.AddCell(new Phrase("合计金额（元）", font));
                table.AddCell(new Phrase("计重方式", font));
                table.AddCell(new Phrase("备注", font));

                foreach (var item in list)
                {
                    var array = lt.AsEnumerable().Where(p => Convert.ToInt32(p[0]) == item.ID).ToArray();
                    double price = item.TempMode == 2 ? item.Amount * Convert.ToDouble(array[0][1]) : item.TotalWeight * Convert.ToDouble(array[0][1]);
                    table.AddCell(new Phrase(item.Name, font));
                    table.AddCell(new Phrase(item.Texture, font));
                    table.AddCell(new Phrase(item.Spec, font));
                    table.AddCell(new Phrase(Convert.ToString(item.Extent), font));
                    table.AddCell(new Phrase(item.Norm, font));
                    table.AddCell(new Phrase(Convert.ToString(item.Amount), font));
                    table.AddCell(new Phrase(Convert.ToString(array[0][3]), font));
                    table.AddCell(new Phrase(Convert.ToString(item.TotalWeight), font));
                    table.AddCell(new Phrase(Convert.ToString(array[0][1]), font));
                    table.AddCell(new Phrase(Convert.ToString(price), font));
                    table.AddCell(new Phrase(item.PriceUnit, font));
                    table.AddCell(new Phrase(Convert.ToString(array[0][2]), font));
                    mt += item.TotalWeight;
                    tl += price;
                }
                table.AddCell(new Phrase("合计", font));
                table.AddCell(new Phrase("", font));
                table.AddCell(new Phrase("", font));
                table.AddCell(new Phrase("", font));
                table.AddCell(new Phrase("", font));
                table.AddCell(new Phrase("", font));
                table.AddCell(new Phrase("", font));
                table.AddCell(new Phrase(Convert.ToString(mt), font));
                table.AddCell(new Phrase("", font));
                table.AddCell(new Phrase(Convert.ToString(tl), font));
                table.AddCell(new Phrase("", font));
                table.AddCell(new Phrase("", font));
            }
            else
            {
                PdfPCell ht = new PdfPCell(new Phrase("详见附件", font));
                ht.Border = 0;
                ht.Colspan = 12;
                ht.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(ht);
            }
            string st1 = $"一、执行标准：{norm}";
            string st2 = $"二、随货的单证及资料：{data}";
            string st3 = "三、包装方式：按国家标准。";
            string st4 = $"四、交货地点：{place}";
            string st5 = $"五、交货期：{date}";
            string st6 = $"六、计重方式：{weight}";
            string st7 = "七、检验期间：自收货之日起一月内，买方应按合同第一、第二、第三条的约定对货物(包括数量、质量及随货凭证)进行检验，如货物不符合同约定，买方应向卖方提出书面异议；若出现质量问题，卖方负责包退包换，产生的一切费用由卖方负担。";
            string st8 = $"八、结算方式：{mode}";
            string st9 = $"九、运费负担：{freight}";
            string st10 = "十、违约责任：1、若卖方不按期交货，每延期一天卖方应支付合同总额1%的违约金。2、卖方交付的货物不符合合同的约定(包括产品名称、规格、数量、质量等),买方有权解除合同并要求卖方支付合同总额30%的违约金，若买方实际损失大于该金额则卖方应赔偿买方的实际损失。";
            string st11 = "十一、纠纷的解决方式：若买卖双方履行本合同发生争议，双方协商解决。若协商不成，则向胶州市人民法院起诉。";
            string st12 = "十二、本合同未尽事宜，按《中华人民共和国合同法》执行。";
            string st13 = "十三、本合同一式两份，双方各执一份，合同自双方签字盖章之日起生效，传真件有效，手写无效";

            PdfPCell t1 = new PdfPCell(new Phrase(st1,font));
            t1.Border = 0;
            t1.Colspan = 12;
            table.AddCell(t1);

            PdfPCell t2 = new PdfPCell(new Phrase(st2, font));
            t2.Border = 0;
            t2.Colspan = 12;
            table.AddCell(t2);

            PdfPCell t3 = new PdfPCell(new Phrase(st3, font));
            t3.Border = 0;
            t3.Colspan = 12;
            table.AddCell(t3);

            PdfPCell t4 = new PdfPCell(new Phrase(st4, font));
            t4.Border = 0;
            t4.Colspan = 12;
            table.AddCell(t4);

            PdfPCell t5 = new PdfPCell(new Phrase(st5, font));
            t5.Border = 0;
            t5.Colspan = 12;
            table.AddCell(t5);

            PdfPCell t6 = new PdfPCell(new Phrase(st6, font));
            t6.Border = 0;
            t6.Colspan = 12;
            table.AddCell(t6);

            PdfPCell t7 = new PdfPCell(new Phrase(st7, font));
            t7.Border = 0;
            t7.Colspan = 12;
            table.AddCell(t7);

            PdfPCell t8 = new PdfPCell(new Phrase(st8, font));
            t8.Border = 0;
            t8.Colspan = 12;
            table.AddCell(t8);

            PdfPCell t9 = new PdfPCell(new Phrase(st9, font));
            t9.Border = 0;
            t9.Colspan = 12;
            table.AddCell(t9);

            PdfPCell t10 = new PdfPCell(new Phrase(st10, font));
            t10.Border = 0;
            t10.Colspan = 12;
            table.AddCell(t10);

            PdfPCell t11 = new PdfPCell(new Phrase(st11, font));
            t11.Border = 0;
            t11.Colspan = 12;
            table.AddCell(t11);

            PdfPCell t12 = new PdfPCell(new Phrase(st12, font));
            t12.Border = 0;
            t12.Colspan = 12;
            table.AddCell(t12);

            PdfPCell t13 = new PdfPCell(new Phrase(st13, font));
            t13.Border = 0;
            t13.Colspan = 12;
            table.AddCell(t13);

            PdfPCell h12 = new PdfPCell(new Phrase("卖方", font));
            h12.Border = 0;
            h12.Colspan = 1;
            table.AddCell(h12);

            PdfPCell h13 = new PdfPCell(new Phrase(supplier.SupplierName, font));
            h13.Border = 0;
            h13.Colspan = 5;
            table.AddCell(h13);

            PdfPCell h14 = new PdfPCell(new Phrase("买方", font));
            h14.Border = 0;
            h14.Colspan = 2;
            table.AddCell(h14);

            PdfPCell h15 = new PdfPCell(new Phrase(client.PurchaserName, font));
            h15.Border = 0;
            h15.Colspan = 4;
            table.AddCell(h15);

            PdfPCell h16 = new PdfPCell(new Phrase("单位名称(章) ", font));
            h16.Border = 0;
            h16.Colspan = 1;
            table.AddCell(h16);

            PdfPCell h17 = new PdfPCell(new Phrase("", font));
            h17.Border = 0;
            h17.Colspan = 5;
            table.AddCell(h17);

            PdfPCell h18 = new PdfPCell(new Phrase("单位名称(章) ", font));
            h18.Border = 0;
            h18.Colspan = 2;
            table.AddCell(h18);

            PdfPCell h19 = new PdfPCell(new Phrase("", font));
            h19.Border = 0;
            h19.Colspan = 4;
            table.AddCell(h19);

            PdfPCell h20 = new PdfPCell(new Phrase("单位地址： ", font));
            h20.Border = 0;
            h20.Colspan = 1;
            table.AddCell(h20);

            PdfPCell h21 = new PdfPCell(new Phrase(supplier.Address, font));
            h21.Border = 0;
            h21.Colspan = 5;
            table.AddCell(h21);

            PdfPCell h22 = new PdfPCell(new Phrase("单位地址：", font));
            h22.Border = 0;
            h22.Colspan = 2;
            table.AddCell(h22);

            PdfPCell h23 = new PdfPCell(new Phrase(client.Address, font));
            h23.Border = 0;
            h23.Colspan = 4;
            table.AddCell(h23);

            PdfPCell h24 = new PdfPCell(new Phrase("法定代表人：", font));
            h24.Border = 0;
            h24.Colspan = 1;
            table.AddCell(h24);

            PdfPCell h25 = new PdfPCell(new Phrase(supplier.LegalPerson, font));
            h25.Border = 0;
            h25.Colspan = 5;
            table.AddCell(h25);

            PdfPCell h26 = new PdfPCell(new Phrase("法定代表人：", font));
            h26.Border = 0;
            h26.Colspan = 2;
            table.AddCell(h26);

            PdfPCell h27 = new PdfPCell(new Phrase(client.LegalPerson, font));
            h27.Border = 0;
            h27.Colspan = 4;
            table.AddCell(h27);

            PdfPCell h28 = new PdfPCell(new Phrase("委托代理人：", font));
            h28.Border = 0;
            h28.Colspan = 1;
            table.AddCell(h28);

            PdfPCell h29 = new PdfPCell(new Phrase(supplier.EntrustPerson, font));
            h29.Border = 0;
            h29.Colspan = 5;
            table.AddCell(h29);

            PdfPCell h30 = new PdfPCell(new Phrase("委托代理人：", font));
            h30.Border = 0;
            h30.Colspan = 2;
            table.AddCell(h30);

            PdfPCell h31 = new PdfPCell(new Phrase("", font));
            h31.Border = 0;
            h31.Colspan = 4;
            table.AddCell(h31);

            PdfPCell h32 = new PdfPCell(new Phrase("传真电话：", font));
            h32.Border = 0;
            h32.Colspan = 1;
            table.AddCell(h32);

            PdfPCell h33 = new PdfPCell(new Phrase(supplier.Fax, font));
            h33.Border = 0;
            h33.Colspan = 5;
            table.AddCell(h33);

            PdfPCell h34 = new PdfPCell(new Phrase("传真电话：", font));
            h34.Border = 0;
            h34.Colspan = 2;
            table.AddCell(h34);

            PdfPCell h35 = new PdfPCell(new Phrase(client.Fax, font));
            h35.Border = 0;
            h35.Colspan = 4;
            table.AddCell(h35);

            PdfPCell h36 = new PdfPCell(new Phrase("开户银行：", font));
            h36.Border = 0;
            h36.Colspan = 1;
            table.AddCell(h36);

            PdfPCell h37 = new PdfPCell(new Phrase(supplier.BankName, font));
            h37.Border = 0;
            h37.Colspan = 5;
            table.AddCell(h37);

            PdfPCell h38 = new PdfPCell(new Phrase("开户银行：", font));
            h38.Border = 0;
            h38.Colspan = 2;
            table.AddCell(h38);

            PdfPCell h39 = new PdfPCell(new Phrase(client.BankName, font));
            h39.Border = 0;
            h39.Colspan = 4;
            table.AddCell(h39);

            PdfPCell h40 = new PdfPCell(new Phrase("账号：", font));
            h40.Border = 0;
            h40.Colspan = 1;
            table.AddCell(h40);

            PdfPCell h41 = new PdfPCell(new Phrase(supplier.BankNo, font));
            h41.Border = 0;
            h41.Colspan = 5;
            table.AddCell(h41);

            PdfPCell h42 = new PdfPCell(new Phrase("账号：", font));
            h42.Border = 0;
            h42.Colspan = 2;
            table.AddCell(h42);

            PdfPCell h43 = new PdfPCell(new Phrase(client.BankNo, font));
            h43.Border = 0;
            h43.Colspan = 4;
            table.AddCell(h43);

            //PdfPCell h44 = new PdfPCell(new Phrase("买方信息", font));
            //h44.Colspan = 2;
            //table.AddCell(h44);

            //PdfPCell h45 = new PdfPCell(new Phrase("单位地址", font));
            //h45.Colspan = 2;
            //table.AddCell(h45);

            //table.AddCell(new Phrase("法定代表人", font));
            //table.AddCell(new Phrase("委托代理人", font));
            //table.AddCell(new Phrase("传真电话", font));

            //PdfPCell h46 = new PdfPCell(new Phrase("开户银行", font));
            //h46.Colspan = 2;
            //table.AddCell(h46);

            //PdfPCell h47 = new PdfPCell(new Phrase("账号", font));
            //h47.Colspan = 2;
            //table.AddCell(h47);

            //foreach (var item in lt)
            //{
            //    PdfPCell h48 = new PdfPCell(new Phrase(item.PurchaserName, font));
            //    h48.Colspan = 2;
            //    table.AddCell(h48);

            //    PdfPCell h49 = new PdfPCell(new Phrase(item.Address, font));
            //    h49.Colspan = 2;
            //    table.AddCell(h49);

            //    table.AddCell(new Phrase(item.LegalPerson, font));
            //    table.AddCell(new Phrase(item.EntrustPerson, font));
            //    table.AddCell(new Phrase(item.Fax, font));

            //    PdfPCell h50 = new PdfPCell(new Phrase(item.BankName, font));
            //    h50.Colspan = 2;
            //    table.AddCell(h50);

            //    PdfPCell h51 = new PdfPCell(new Phrase(item.BankNo, font));
            //    h51.Colspan = 2;
            //    table.AddCell(h51);
            //}

            document.Add(table);
            document.Close();
            if (rwCount > 8)
            {
                Document doc = new Document(PageSize.A4);
                string nm = $"合同附表{ DateTime.Now.ToString("yyyyMMddHHmmssfff")}.pdf";
                PdfWriter.GetInstance(doc, new FileStream(AppDomain.CurrentDomain.BaseDirectory + $"/UploadFile/pdf/{nm}", FileMode.Create));
                // 添加文档信息
                doc.AddTitle($"合同附表（合同号：{contractNo}）");
                doc.AddAuthor("");
                doc.Open();
                // 添加文档内容
                PdfPTable pt = new PdfPTable(12);
                pt.SetWidths(new float[] { 200f, 100f, 100f, 100f, 150f, 150f, 100f, 100f, 150f, 100f, 150f, 100f });

                PdfPCell cl = new PdfPCell(new Phrase($"合同附表（合同号：{contractNo}）", fontTitle));
                cl.Colspan = 12;
                cl.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                pt.AddCell(cl);

                pt.AddCell(new Phrase("品名", font));
                pt.AddCell(new Phrase("材质", font));
                pt.AddCell(new Phrase("规格", font));
                pt.AddCell(new Phrase("长度", font));
                pt.AddCell(new Phrase("标准", font));
                pt.AddCell(new Phrase("数量", font));
                pt.AddCell(new Phrase("单位", font));
                pt.AddCell(new Phrase("吨位", font));
                pt.AddCell(new Phrase("单价（元）", font));
                pt.AddCell(new Phrase("合计金额（元）", font));
                pt.AddCell(new Phrase("计重方式", font));
                pt.AddCell(new Phrase("备注", font));

                foreach (var item in list)
                {
                    var array = lt.AsEnumerable().Where(p => Convert.ToInt32(p[0]) == item.ID).ToArray();
                    double price = item.TempMode == 2 ? item.Amount * Convert.ToDouble(array[0][1]) : item.TotalWeight * Convert.ToDouble(array[0][1]);
                    pt.AddCell(new Phrase(item.Name, font));
                    pt.AddCell(new Phrase(item.Texture, font));
                    pt.AddCell(new Phrase(item.Spec, font));
                    pt.AddCell(new Phrase(Convert.ToString(item.Extent), font));
                    pt.AddCell(new Phrase(item.Norm, font));
                    pt.AddCell(new Phrase(Convert.ToString(item.Amount), font));
                    pt.AddCell(new Phrase(Convert.ToString(array[0][3]), font));
                    pt.AddCell(new Phrase(Convert.ToString(item.TotalWeight), font));
                    pt.AddCell(new Phrase(Convert.ToString(array[0][1]), font));
                    pt.AddCell(new Phrase(Convert.ToString(price), font));
                    pt.AddCell(new Phrase(item.PriceUnit, font));
                    pt.AddCell(new Phrase(Convert.ToString(array[0][2]), font));
                    mt += item.TotalWeight;
                    tl += price;
                }

                pt.AddCell(new Phrase("合计", font));
                pt.AddCell(new Phrase("", font));
                pt.AddCell(new Phrase("", font));
                pt.AddCell(new Phrase("", font));
                pt.AddCell(new Phrase("", font));
                pt.AddCell(new Phrase("", font));
                pt.AddCell(new Phrase("", font));
                pt.AddCell(new Phrase(Convert.ToString(mt), font));
                pt.AddCell(new Phrase("", font));
                pt.AddCell(new Phrase(Convert.ToString(tl), font));
                pt.AddCell(new Phrase("", font));
                pt.AddCell(new Phrase("", font));
                doc.Add(pt);
                doc.Close();
                return $"{name},{nm}";
            }
            else
            {
                return name;
            }
        }

        public string ContractToPDF2(Supplier supplier, Item it, IEnumerable<Contract> list, string norm, string weight, string mode, string freight, Purchaser client, string place, string date, string data, List<string[]> lt)
        {
            string contractNo = $"{it.ItemNo}-{supplier.Id.ToString().PadLeft(5, '0')}";
            int rwCount = list.Count();
            //载入字体
            //"UniGB-UCS2-H" "UniGB-UCS2-V"是简体中文，分别表示横向字 和 // 纵向字 //" STSong-Light"是字体名称 
            BaseFont baseFont = BaseFont.CreateFont("C:\\WINDOWS\\FONTS\\STSONG.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 10);
            iTextSharp.text.Font fontTitle = new iTextSharp.text.Font(baseFont, 15);
            //设置边界
            Document document = new Document(PageSize.A4, 0, 0, 0, 0);
            string name = $"合同{ DateTime.Now.ToString("yyyyMMddHHmmssfff")}.pdf";
            PdfWriter.GetInstance(document, new FileStream(AppDomain.CurrentDomain.BaseDirectory + $"/UploadFile/pdf/{name}", FileMode.Create));
            // 添加文档信息
            document.AddTitle("买卖合同");
            //document.AddSubject("Contract of PDFInfo");
            //document.AddKeywords("Info, PDF, Contract");
            //document.AddCreator("Contract");
            document.AddAuthor("");
            document.Open();
            // 添加文档内容
            PdfPTable table = new PdfPTable(10);
            //table.SetWidths(new float[] { 200f, 100f, 100f, 100f, 150f, 100f, 100f, 110f, 150f, 100f, 150f, 140f });
            table.SetWidths(new float[] { 82f, 45f, 45f, 50f, 35f, 35f, 58f, 50f, 58f, 50f }); //35f,47f

            PdfPCell cell = new PdfPCell(new Phrase("买卖合同", fontTitle));
            cell.Border = 0;
            cell.Colspan = 10;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.AddCell(cell);

            PdfPCell h1 = new PdfPCell(new Phrase("卖方：", font));
            h1.Border = 0;
            h1.Colspan = 1;
            table.AddCell(h1);

            PdfPCell h2 = new PdfPCell(new Phrase(supplier.SupplierName, font));
            h2.Border = 0;
            h2.Colspan = 4;
            table.AddCell(h2);

            PdfPCell h3 = new PdfPCell(new Phrase("合同编号：", font));
            h3.Border = 0;
            h3.Colspan = 2;
            table.AddCell(h3);

            PdfPCell h4 = new PdfPCell(new Phrase(contractNo, font));
            h4.Border = 0;
            h4.Colspan = 3;
            table.AddCell(h4);

            PdfPCell h5 = new PdfPCell(new Phrase("买方：", font));
            h5.Border = 0;
            h5.Colspan = 1;
            table.AddCell(h5);

            PdfPCell h6 = new PdfPCell(new Phrase(client.PurchaserName, font));
            h6.Border = 0;
            h6.Colspan = 4;
            table.AddCell(h6);

            PdfPCell h7 = new PdfPCell(new Phrase("合同签订地：", font));
            h7.Border = 0;
            h7.Colspan = 2;
            table.AddCell(h7);

            PdfPCell h8 = new PdfPCell(new Phrase(it.SignPlace, font));
            h8.Border = 0;
            h8.Colspan = 3;
            table.AddCell(h8);

            PdfPCell h9 = new PdfPCell(new Phrase(""));
            h9.Border = 0;
            h9.Colspan = 5;
            table.AddCell(h9);

            PdfPCell h10 = new PdfPCell(new Phrase("签订时间：", font));
            h10.Border = 0;
            h10.Colspan = 2;
            table.AddCell(h10);

            PdfPCell h11 = new PdfPCell(new Phrase(it.SignTime.ToString("yyyy年MM月dd日"), font));
            h11.Border = 0;
            h11.Colspan = 3;
            table.AddCell(h11);
            double mt = 0, tl = 0;
            if (rwCount <= 8)
            {
                table.AddCell(new Phrase("品名", font));
                table.AddCell(new Phrase("材质", font));
                table.AddCell(new Phrase("规格", font));
                //table.AddCell(new Phrase("长度", font));
                table.AddCell(new Phrase("标准", font));
                table.AddCell(new Phrase("数量", font));
                table.AddCell(new Phrase("单位", font));
                //table.AddCell(new Phrase("吨位", font));
                table.AddCell(new Phrase("单价（元）", font));
                table.AddCell(new Phrase("合计金额（元）", font));
                table.AddCell(new Phrase("计重方式", font));
                table.AddCell(new Phrase("备注", font));

                foreach (var item in list)
                {
                    var array = lt.AsEnumerable().Where(p => Convert.ToInt32(p[0]) == item.ID).ToArray();
                    double price = item.TempMode == 2 ? item.Amount * Convert.ToDouble(array[0][1]) : item.TotalWeight * Convert.ToDouble(array[0][1]);
                    table.AddCell(new Phrase(item.Name, font));
                    table.AddCell(new Phrase(item.Texture, font));
                    table.AddCell(new Phrase(item.Spec, font));
                    //table.AddCell(new Phrase(Convert.ToString(item.Extent), font));
                    table.AddCell(new Phrase(item.Norm, font));
                    table.AddCell(new Phrase(Convert.ToString(item.Amount), font));
                    table.AddCell(new Phrase(Convert.ToString(item.Unit), font));//
                    //table.AddCell(new Phrase(Convert.ToString(item.TotalWeight), font));
                    table.AddCell(new Phrase(Convert.ToString(array[0][1]), font));
                    table.AddCell(new Phrase(Convert.ToString(price), font));
                    table.AddCell(new Phrase(item.PriceUnit, font));
                    table.AddCell(new Phrase(Convert.ToString(array[0][2]), font));
                    mt += item.TotalWeight;
                    tl += price;
                }
                table.AddCell(new Phrase("合计", font));
                table.AddCell(new Phrase("", font));
                table.AddCell(new Phrase("", font));
                table.AddCell(new Phrase("", font));
                //table.AddCell(new Phrase("", font));
                table.AddCell(new Phrase("", font));
                table.AddCell(new Phrase("", font));
                table.AddCell(new Phrase(Convert.ToString(mt), font));
                //table.AddCell(new Phrase("", font));
                table.AddCell(new Phrase(Convert.ToString(tl), font));
                table.AddCell(new Phrase("", font));
                table.AddCell(new Phrase("", font));
            }
            else
            {
                PdfPCell ht = new PdfPCell(new Phrase("详见附件", font));
                ht.Border = 0;
                ht.Colspan = 10;
                ht.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(ht);
            }
            string st1 = $"一、执行标准：{norm}";
            string st2 = $"二、随货的单证及资料：{data}";
            string st3 = "三、包装方式：按国家标准。";
            string st4 = $"四、交货地点：{place}";
            string st5 = $"五、交货期：{date}";
            string st6 = $"六、计重方式：{weight}";
            string st7 = "七、检验期间：自收货之日起一月内，买方应按合同第一、第二、第三条的约定对货物(包括数量、质量及随货凭证)进行检验，如货物不符合同约定，买方应向卖方提出书面异议；若出现质量问题，卖方负责包退包换，产生的一切费用由卖方负担。";
            string st8 = $"八、结算方式：{mode}";
            string st9 = $"九、运费负担：{freight}";
            string st10 = "十、违约责任：1、若卖方不按期交货，每延期一天卖方应支付合同总额1%的违约金。2、卖方交付的货物不符合合同的约定(包括产品名称、规格、数量、质量等),买方有权解除合同并要求卖方支付合同总额30%的违约金，若买方实际损失大于该金额则卖方应赔偿买方的实际损失。";
            string st11 = "十一、纠纷的解决方式：若买卖双方履行本合同发生争议，双方协商解决。若协商不成，则向胶州市人民法院起诉。";
            string st12 = "十二、本合同未尽事宜，按《中华人民共和国合同法》执行。";
            string st13 = "十三、本合同一式两份，双方各执一份，合同自双方签字盖章之日起生效，传真件有效，手写无效";

            PdfPCell t1 = new PdfPCell(new Phrase(st1, font));
            t1.Border = 0;
            t1.Colspan = 10;
            table.AddCell(t1);

            PdfPCell t2 = new PdfPCell(new Phrase(st2, font));
            t2.Border = 0;
            t2.Colspan = 10;
            table.AddCell(t2);

            PdfPCell t3 = new PdfPCell(new Phrase(st3, font));
            t3.Border = 0;
            t3.Colspan = 10;
            table.AddCell(t3);

            PdfPCell t4 = new PdfPCell(new Phrase(st4, font));
            t4.Border = 0;
            t4.Colspan = 10;
            table.AddCell(t4);

            PdfPCell t5 = new PdfPCell(new Phrase(st5, font));
            t5.Border = 0;
            t5.Colspan = 10;
            table.AddCell(t5);

            PdfPCell t6 = new PdfPCell(new Phrase(st6, font));
            t6.Border = 0;
            t6.Colspan = 10;
            table.AddCell(t6);

            PdfPCell t7 = new PdfPCell(new Phrase(st7, font));
            t7.Border = 0;
            t7.Colspan = 10;
            table.AddCell(t7);

            PdfPCell t8 = new PdfPCell(new Phrase(st8, font));
            t8.Border = 0;
            t8.Colspan = 10;
            table.AddCell(t8);

            PdfPCell t9 = new PdfPCell(new Phrase(st9, font));
            t9.Border = 0;
            t9.Colspan = 10;
            table.AddCell(t9);

            PdfPCell t10 = new PdfPCell(new Phrase(st10, font));
            t10.Border = 0;
            t10.Colspan = 10;
            table.AddCell(t10);

            PdfPCell t11 = new PdfPCell(new Phrase(st11, font));
            t11.Border = 0;
            t11.Colspan = 10;
            table.AddCell(t11);

            PdfPCell t12 = new PdfPCell(new Phrase(st12, font));
            t12.Border = 0;
            t12.Colspan = 10;
            table.AddCell(t12);

            PdfPCell t13 = new PdfPCell(new Phrase(st13, font));
            t13.Border = 0;
            t13.Colspan = 10;
            table.AddCell(t13);

            PdfPCell h12 = new PdfPCell(new Phrase("卖方", font));
            h12.Border = 0;
            h12.Colspan = 1;
            table.AddCell(h12);

            PdfPCell h13 = new PdfPCell(new Phrase(supplier.SupplierName, font));
            h13.Border = 0;
            h13.Colspan = 4;
            table.AddCell(h13);

            PdfPCell h14 = new PdfPCell(new Phrase("买方", font));
            h14.Border = 0;
            h14.Colspan = 2;
            table.AddCell(h14);

            PdfPCell h15 = new PdfPCell(new Phrase(client.PurchaserName, font));
            h15.Border = 0;
            h15.Colspan = 3;
            table.AddCell(h15);

            PdfPCell h16 = new PdfPCell(new Phrase("单位名称(章) ", font));
            h16.Border = 0;
            h16.Colspan = 1;
            table.AddCell(h16);

            PdfPCell h17 = new PdfPCell(new Phrase("", font));
            h17.Border = 0;
            h17.Colspan = 4;
            table.AddCell(h17);

            PdfPCell h18 = new PdfPCell(new Phrase("单位名称(章) ", font));
            h18.Border = 0;
            h18.Colspan = 2;
            table.AddCell(h18);

            PdfPCell h19 = new PdfPCell(new Phrase("", font));
            h19.Border = 0;
            h19.Colspan = 3;
            table.AddCell(h19);

            PdfPCell h20 = new PdfPCell(new Phrase("单位地址： ", font));
            h20.Border = 0;
            h20.Colspan = 1;
            table.AddCell(h20);

            PdfPCell h21 = new PdfPCell(new Phrase(supplier.Address, font));
            h21.Border = 0;
            h21.Colspan = 4;
            table.AddCell(h21);

            PdfPCell h22 = new PdfPCell(new Phrase("单位地址：", font));
            h22.Border = 0;
            h22.Colspan = 2;
            table.AddCell(h22);

            PdfPCell h23 = new PdfPCell(new Phrase(client.Address, font));
            h23.Border = 0;
            h23.Colspan = 3;
            table.AddCell(h23);

            PdfPCell h24 = new PdfPCell(new Phrase("法定代表人：", font));
            h24.Border = 0;
            h24.Colspan = 1;
            table.AddCell(h24);

            PdfPCell h25 = new PdfPCell(new Phrase(supplier.LegalPerson, font));
            h25.Border = 0;
            h25.Colspan = 4;
            table.AddCell(h25);

            PdfPCell h26 = new PdfPCell(new Phrase("法定代表人：", font));
            h26.Border = 0;
            h26.Colspan = 2;
            table.AddCell(h26);

            PdfPCell h27 = new PdfPCell(new Phrase(client.LegalPerson, font));
            h27.Border = 0;
            h27.Colspan = 3;
            table.AddCell(h27);

            PdfPCell h28 = new PdfPCell(new Phrase("委托代理人：", font));
            h28.Border = 0;
            h28.Colspan = 1;
            table.AddCell(h28);

            PdfPCell h29 = new PdfPCell(new Phrase(supplier.EntrustPerson, font));
            h29.Border = 0;
            h29.Colspan = 4;
            table.AddCell(h29);

            PdfPCell h30 = new PdfPCell(new Phrase("委托代理人：", font));
            h30.Border = 0;
            h30.Colspan = 2;
            table.AddCell(h30);

            PdfPCell h31 = new PdfPCell(new Phrase("", font));
            h31.Border = 0;
            h31.Colspan = 3;
            table.AddCell(h31);

            PdfPCell h32 = new PdfPCell(new Phrase("传真电话：", font));
            h32.Border = 0;
            h32.Colspan = 1;
            table.AddCell(h32);

            PdfPCell h33 = new PdfPCell(new Phrase(supplier.Fax, font));
            h33.Border = 0;
            h33.Colspan = 4;
            table.AddCell(h33);

            PdfPCell h34 = new PdfPCell(new Phrase("传真电话：", font));
            h34.Border = 0;
            h34.Colspan = 2;
            table.AddCell(h34);

            PdfPCell h35 = new PdfPCell(new Phrase(client.Fax, font));
            h35.Border = 0;
            h35.Colspan = 3;
            table.AddCell(h35);

            PdfPCell h36 = new PdfPCell(new Phrase("开户银行：", font));
            h36.Border = 0;
            h36.Colspan = 1;
            table.AddCell(h36);

            PdfPCell h37 = new PdfPCell(new Phrase(supplier.BankName, font));
            h37.Border = 0;
            h37.Colspan = 4;
            table.AddCell(h37);

            PdfPCell h38 = new PdfPCell(new Phrase("开户银行：", font));
            h38.Border = 0;
            h38.Colspan = 2;
            table.AddCell(h38);

            PdfPCell h39 = new PdfPCell(new Phrase(client.BankName, font));
            h39.Border = 0;
            h39.Colspan = 3;
            table.AddCell(h39);

            PdfPCell h40 = new PdfPCell(new Phrase("账号：", font));
            h40.Border = 0;
            h40.Colspan = 1;
            table.AddCell(h40);

            PdfPCell h41 = new PdfPCell(new Phrase(supplier.BankNo, font));
            h41.Border = 0;
            h41.Colspan = 4;
            table.AddCell(h41);

            PdfPCell h42 = new PdfPCell(new Phrase("账号：", font));
            h42.Border = 0;
            h42.Colspan = 2;
            table.AddCell(h42);

            PdfPCell h43 = new PdfPCell(new Phrase(client.BankNo, font));
            h43.Border = 0;
            h43.Colspan = 3;
            table.AddCell(h43);

            //PdfPCell h44 = new PdfPCell(new Phrase("买方信息", font));
            //h44.Colspan = 2;
            //table.AddCell(h44);

            //PdfPCell h45 = new PdfPCell(new Phrase("单位地址", font));
            //h45.Colspan = 2;
            //table.AddCell(h45);

            //table.AddCell(new Phrase("法定代表人", font));
            //table.AddCell(new Phrase("委托代理人", font));
            //table.AddCell(new Phrase("传真电话", font));

            //PdfPCell h46 = new PdfPCell(new Phrase("开户银行", font));
            //h46.Colspan = 2;
            //table.AddCell(h46);

            //PdfPCell h47 = new PdfPCell(new Phrase("账号", font));
            //h47.Colspan = 2;
            //table.AddCell(h47);

            //foreach (var item in lt)
            //{
            //    PdfPCell h48 = new PdfPCell(new Phrase(item.PurchaserName, font));
            //    h48.Colspan = 2;
            //    table.AddCell(h48);

            //    PdfPCell h49 = new PdfPCell(new Phrase(item.Address, font));
            //    h49.Colspan = 2;
            //    table.AddCell(h49);

            //    table.AddCell(new Phrase(item.LegalPerson, font));
            //    table.AddCell(new Phrase(item.EntrustPerson, font));
            //    table.AddCell(new Phrase(item.Fax, font));

            //    PdfPCell h50 = new PdfPCell(new Phrase(item.BankName, font));
            //    h50.Colspan = 2;
            //    table.AddCell(h50);

            //    PdfPCell h51 = new PdfPCell(new Phrase(item.BankNo, font));
            //    h51.Colspan = 2;
            //    table.AddCell(h51);
            //}

            document.Add(table);
            document.Close();
            if (rwCount > 8)
            {
                Document doc = new Document(PageSize.A4);
                string nm = $"合同附表{ DateTime.Now.ToString("yyyyMMddHHmmssfff")}.pdf";
                PdfWriter.GetInstance(doc, new FileStream(AppDomain.CurrentDomain.BaseDirectory + $"/UploadFile/pdf/{nm}", FileMode.Create));
                // 添加文档信息
                doc.AddTitle($"合同附表（合同号：{contractNo}）");
                doc.AddAuthor("");
                doc.Open();
                // 添加文档内容
                PdfPTable pt = new PdfPTable(10);
                pt.SetWidths(new float[] { 200f, 100f, 100f, 150f, 150f, 100f, 150f, 100f, 150f, 100f });

                PdfPCell cl = new PdfPCell(new Phrase($"合同附表（合同号：{contractNo}）", fontTitle));
                cl.Colspan = 10;
                cl.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                pt.AddCell(cl);

                pt.AddCell(new Phrase("品名", font));
                pt.AddCell(new Phrase("材质", font));
                pt.AddCell(new Phrase("规格", font));
                //pt.AddCell(new Phrase("长度", font));
                pt.AddCell(new Phrase("标准", font));
                pt.AddCell(new Phrase("数量", font));
                pt.AddCell(new Phrase("单位", font));
                //pt.AddCell(new Phrase("吨位", font));
                pt.AddCell(new Phrase("单价（元）", font));
                pt.AddCell(new Phrase("合计金额（元）", font));
                pt.AddCell(new Phrase("计重方式", font));
                pt.AddCell(new Phrase("备注", font));

                foreach (var item in list)
                {
                    var array = lt.AsEnumerable().Where(p => Convert.ToInt32(p[0]) == item.ID).ToArray();
                    double price = item.TempMode == 2 ? item.Amount * Convert.ToDouble(array[0][1]) : item.TotalWeight * Convert.ToDouble(array[0][1]);
                    pt.AddCell(new Phrase(item.Name, font));
                    pt.AddCell(new Phrase(item.Texture, font));
                    pt.AddCell(new Phrase(item.Spec, font));
                    //pt.AddCell(new Phrase(Convert.ToString(item.Extent), font));
                    pt.AddCell(new Phrase(item.Norm, font));
                    pt.AddCell(new Phrase(Convert.ToString(item.Amount), font));
                    pt.AddCell(new Phrase(Convert.ToString(item.Unit), font));//
                    //pt.AddCell(new Phrase(Convert.ToString(item.TotalWeight), font));
                    pt.AddCell(new Phrase(Convert.ToString(array[0][1]), font));
                    pt.AddCell(new Phrase(Convert.ToString(price), font));
                    pt.AddCell(new Phrase(item.PriceUnit, font));
                    pt.AddCell(new Phrase(Convert.ToString(array[0][2]), font));
                    //mt += item.TotalWeight;
                    tl += price;
                }

                pt.AddCell(new Phrase("合计", font));
                pt.AddCell(new Phrase("", font));
                pt.AddCell(new Phrase("", font));
                //pt.AddCell(new Phrase("", font));
                pt.AddCell(new Phrase("", font));
                pt.AddCell(new Phrase("", font));
                pt.AddCell(new Phrase("", font));
                //pt.AddCell(new Phrase(Convert.ToString(mt), font));
                pt.AddCell(new Phrase("", font));
                pt.AddCell(new Phrase(Convert.ToString(tl), font));
                pt.AddCell(new Phrase("", font));
                pt.AddCell(new Phrase("", font));
                doc.Add(pt);
                doc.Close();
                return $"{name},{nm}";
            }
            else
            {
                return name;
            }
        }

        public string ReportToPDF1(Item it, IEnumerable<ItemContrast> list)
        {
            BaseFont baseFont = BaseFont.CreateFont("C:\\WINDOWS\\FONTS\\STSONG.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 9);
            iTextSharp.text.Font fontTitle = new iTextSharp.text.Font(baseFont, 15);
            //设置边界
            Document document = new Document(PageSize.A2.Rotate());
            string name = $"型材询价单{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.pdf";
            PdfWriter.GetInstance(document, new FileStream(AppDomain.CurrentDomain.BaseDirectory + $"/UploadFile/pdf/{name}", FileMode.Create));
            // 添加文档信息
            document.AddTitle("型材询价单");
            document.AddSubject("Contract of PDFInfo");
            document.AddKeywords("Info, PDF, Contract");
            document.AddCreator("");
            document.AddAuthor("");
            document.Open();
            // 添加文档内容
            PdfPTable table = new PdfPTable(24);
            table.SetWidths(new float[] { 600f, 600f, 600f, 600f, 600f, 600f, 600f, 600f, 1200f, 950f, 1750f, 600f, 600f, 600f, 600f, 600f, 600f, 600f, 600f, 600f, 600f, 1000f, 1000f, 1000f });//850f

            PdfPCell h1 = new PdfPCell(new Phrase("采购单位：青岛武晓集团有限公司", fontTitle));
            h1.Colspan = 13;
            h1.HorizontalAlignment = 1;
            table.AddCell(h1);

            PdfPCell h2 = new PdfPCell(new Phrase("询价类别", fontTitle));
            h2.Colspan = 2;
            h2.HorizontalAlignment = 1;
            table.AddCell(h2);

            PdfPCell h3 = new PdfPCell(new Phrase("型材询价单", fontTitle));
            h3.Colspan = 6;
            h3.HorizontalAlignment = 1;
            table.AddCell(h3);

            PdfPTable h41 = new PdfPTable(1);
            h41.AddCell(new Phrase("询价期间",font));
            h41.AddCell(new Phrase($"{it.StartDate.ToString("yyyy-MM-dd HH:mm")} 至 {it.EndDate.ToString("yyyy-MM-dd HH:mm")}",font));

            PdfPCell h4 = new PdfPCell(h41);
            h4.Colspan = 2;
            h4.HorizontalAlignment = 1;
            table.AddCell(h4);

            PdfPTable h51 = new PdfPTable(1);
            h51.AddCell(new Phrase(it.ItemNo, font));
            h51.AddCell(new Phrase(it.Memo, font));

            PdfPCell h5 = new PdfPCell(h51);
            h5.Colspan = 5;
            h5.HorizontalAlignment = 1;
            table.AddCell(h5);

            table.AddCell(new Phrase("名称", font));
            //table.AddCell(new Phrase("类别", font));
            table.AddCell(new Phrase("规格", font));
            table.AddCell(new Phrase("材质", font));
            table.AddCell(new Phrase("标准", font));
            table.AddCell(new Phrase("长度(米)", font));
            table.AddCell(new Phrase("数量(支)", font));
            table.AddCell(new Phrase("总重(T)", font));
            //table.AddCell(new Phrase("实际到厂单价", font));
            table.AddCell(new Phrase("到货时间", font));
            table.AddCell(new Phrase("价格为理计 / 磅计", font));
            table.AddCell(new Phrase("是否承兑 / 现款", font));
            table.AddCell(new Phrase("货到付款 / 款到发货 / 预付款", font));
            table.AddCell(new Phrase("是否含税", font));
            table.AddCell(new Phrase("备注", font));

            PdfPCell h6 = new PdfPCell(new Phrase("报价单位", font));
            h6.Colspan = 8;
            h6.HorizontalAlignment = 1;
            table.AddCell(h6);

            table.AddCell(new Phrase("最低报价单位", font));
            table.AddCell(new Phrase("选购采购厂商", font));
            table.AddCell(new Phrase("备注", font));

            foreach (var item in list)
            {
                table.AddCell(new Phrase(item.Name, font));
                table.AddCell(new Phrase(item.Spec, font));
                table.AddCell(new Phrase(item.Texture, font));
                table.AddCell(new Phrase(item.Norm, font));
                table.AddCell(new Phrase(item.Extent, font));
                table.AddCell(new Phrase(item.Amount, font));
                table.AddCell(new Phrase(item.TotalWeight, font));
                //table.AddCell(new Phrase(item.RealPrice, font));
                table.AddCell(new Phrase(item.ReachTime, font));
                table.AddCell(new Phrase(item.PriceUnit, font));
                table.AddCell(new Phrase(item.PriceMode, font));
                table.AddCell(new Phrase(item.PayMode, font));
                table.AddCell(new Phrase(item.IsTax, font));
                table.AddCell(new Phrase(item.Memo, font));
                table.AddCell(new Phrase(item.SupplierName1, font));
                table.AddCell(new Phrase(item.SupplierName2, font));
                table.AddCell(new Phrase(item.SupplierName3, font));
                table.AddCell(new Phrase(item.SupplierName4, font));
                table.AddCell(new Phrase(item.SupplierName5, font));
                table.AddCell(new Phrase(item.SupplierName6, font));
                table.AddCell(new Phrase(item.SupplierName7, font));
                table.AddCell(new Phrase(item.SupplierName8, font));
                table.AddCell(new Phrase(item.MinName, font));
                table.AddCell(new Phrase(item.InName, font));
                table.AddCell(new Phrase(item.Explain, font));
            }

            document.Add(table);
            document.Close();
            return name;
        }

        public string ReportToPDF2(Item it, IEnumerable<ItemContrast> list)
        {
            BaseFont baseFont = BaseFont.CreateFont("C:\\WINDOWS\\FONTS\\STSONG.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 9);
            iTextSharp.text.Font fontTitle = new iTextSharp.text.Font(baseFont, 15);
            //设置边界
            Document document = new Document(PageSize.A2.Rotate());
            string name = $"五金询价单{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.pdf";
            PdfWriter.GetInstance(document, new FileStream(AppDomain.CurrentDomain.BaseDirectory + $"/UploadFile/pdf/{name}", FileMode.Create));
            // 添加文档信息
            document.AddTitle("五金询价单");
            document.AddSubject("Contract of PDFInfo");
            document.AddKeywords("Info, PDF, Contract");
            //document.AddCreator("Contract");
            document.AddAuthor("");
            document.Open();
            // 添加文档内容
            PdfPTable table = new PdfPTable(19);
            table.SetWidths(new float[] { 600f, 600f, 600f, 850f, 600f, 600f, 600f, 600f, 600f, 600f, 600f, 600f, 600f, 600f, 600f, 600f, 1000f, 1000f, 1000f });

            PdfPCell h1 = new PdfPCell(new Phrase("采购单位：青岛武晓集团有限公司", fontTitle));
            h1.Colspan = 8;
            h1.HorizontalAlignment = 1;
            table.AddCell(h1);

            PdfPCell h2 = new PdfPCell(new Phrase("询价类别", fontTitle));
            h2.Colspan = 2;
            h2.HorizontalAlignment = 1;
            table.AddCell(h2);

            PdfPCell h3 = new PdfPCell(new Phrase("五金询价单", fontTitle));
            h3.Colspan = 6;
            h3.HorizontalAlignment = 1;
            table.AddCell(h3);

            PdfPTable h41 = new PdfPTable(1);
            h41.AddCell(new Phrase("询价期间", font));
            h41.AddCell(new Phrase($"{it.StartDate.ToString("yyyy-MM-dd HH:mm")} 至 {it.EndDate.ToString("yyyy-MM-dd HH:mm")}", font));

            PdfPCell h4 = new PdfPCell(h41);
            h4.Colspan = 2;
            h4.HorizontalAlignment = 1;
            table.AddCell(h4);

            PdfPTable h51 = new PdfPTable(1);
            h51.AddCell(new Phrase(it.ItemNo, font));
            h51.AddCell(new Phrase(it.Memo, font));

            PdfPCell h5 = new PdfPCell(h51);
            h5.Colspan = 5;
            h5.HorizontalAlignment = 1;
            table.AddCell(h5);

            table.AddCell(new Phrase("名称", font));
            //*table.AddCell(new Phrase("类别", font));
            table.AddCell(new Phrase("规格", font));
            //table.AddCell(new Phrase("材质", font));
            //table.AddCell(new Phrase("标准", font));
            //table.AddCell(new Phrase("长度(米)", font));
            table.AddCell(new Phrase("数量", font));
            table.AddCell(new Phrase("单位", font));
            //table.AddCell(new Phrase("实际到厂单价", font));
            table.AddCell(new Phrase("到货时间", font));
            //table.AddCell(new Phrase("价格为理计 / 磅计", font));
            //table.AddCell(new Phrase("是否承兑 / 现款", font));
            //table.AddCell(new Phrase("货到付款 / 款到发货 / 预付款", font));
            table.AddCell(new Phrase("使用地点", font));
            table.AddCell(new Phrase("是否含税", font));
            table.AddCell(new Phrase("备注", font));

            PdfPCell h6 = new PdfPCell(new Phrase("报价单位", font));
            h6.Colspan = 8;
            h6.HorizontalAlignment = 1;
            table.AddCell(h6);

            table.AddCell(new Phrase("最低报价单位", font));
            table.AddCell(new Phrase("选购采购厂商", font));
            table.AddCell(new Phrase("备注", font));

            foreach (var item in list)
            {
                table.AddCell(new Phrase(item.Name, font));
                //table.AddCell(new Phrase(item.Mode, font));
                table.AddCell(new Phrase(item.Spec, font));
                //table.AddCell(new Phrase(item.Texture, font));
                //table.AddCell(new Phrase(item.Norm, font));
                //table.AddCell(new Phrase(item.Extent, font));
                table.AddCell(new Phrase(item.Amount, font));
                table.AddCell(new Phrase(item.PriceUnit, font));
                //table.AddCell(new Phrase(item.RealPrice, font));
                table.AddCell(new Phrase(item.ReachTime, font));
                //table.AddCell(new Phrase(item.PriceUnit, font));
                //table.AddCell(new Phrase(item.PriceMode, font));
                //table.AddCell(new Phrase(item.PayMode, font));
                table.AddCell(new Phrase(item.UsePlace, font));
                table.AddCell(new Phrase(item.IsTax, font));
                table.AddCell(new Phrase(item.Memo, font));
                table.AddCell(new Phrase(item.SupplierName1, font));
                table.AddCell(new Phrase(item.SupplierName2, font));
                table.AddCell(new Phrase(item.SupplierName3, font));
                table.AddCell(new Phrase(item.SupplierName4, font));
                table.AddCell(new Phrase(item.SupplierName5, font));
                table.AddCell(new Phrase(item.SupplierName6, font));
                table.AddCell(new Phrase(item.SupplierName7, font));
                table.AddCell(new Phrase(item.SupplierName8, font));
                table.AddCell(new Phrase(item.MinName, font));
                table.AddCell(new Phrase(item.InName, font));
                table.AddCell(new Phrase(item.Explain, font));
            }

            document.Add(table);
            document.Close();
            return name;
        }

        public string ReportToPDF3(Item it, IEnumerable<ItemContrast> list)
        {
            BaseFont baseFont = BaseFont.CreateFont("C:\\WINDOWS\\FONTS\\STSONG.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 9);
            iTextSharp.text.Font fontTitle = new iTextSharp.text.Font(baseFont, 15);
            //设置边界
            Document document = new Document(PageSize.A2.Rotate());
            string name = $"板材询价单{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.pdf";
            PdfWriter.GetInstance(document, new FileStream(AppDomain.CurrentDomain.BaseDirectory + $"/UploadFile/pdf/{name}", FileMode.Create));
            // 添加文档信息
            document.AddTitle("板材询价单");
            document.AddSubject("Contract of PDFInfo");
            document.AddKeywords("Info, PDF, Contract");
            //document.AddCreator("Contract");
            document.AddAuthor("");
            document.Open();
            // 添加文档内容
            PdfPTable table = new PdfPTable(25);
            table.SetWidths(new float[] { 600f, 600f, 600f, 600f, 600f, 600f, 600f, 850f, 600f, 1000f, 950f, 1750f, 600f, 600f, 600f, 600f, 600f, 600f, 600f, 600f, 600f, 600f, 1000f, 1000f, 1000f });

            PdfPCell h1 = new PdfPCell(new Phrase("采购单位：青岛武晓集团有限公司", fontTitle));
            h1.Colspan = 14;
            h1.HorizontalAlignment = 1;
            table.AddCell(h1);

            PdfPCell h2 = new PdfPCell(new Phrase("询价类别", fontTitle));
            h2.Colspan = 2;
            h2.HorizontalAlignment = 1;
            table.AddCell(h2);

            PdfPCell h3 = new PdfPCell(new Phrase("板材询价单", fontTitle));
            h3.Colspan = 6;
            h3.HorizontalAlignment = 1;
            table.AddCell(h3);

            PdfPTable h41 = new PdfPTable(1);
            h41.AddCell(new Phrase("询价期间", font));
            h41.AddCell(new Phrase($"{it.StartDate.ToString("yyyy-MM-dd HH:mm")} 至 {it.EndDate.ToString("yyyy-MM-dd HH:mm")}", font));

            PdfPCell h4 = new PdfPCell(h41);
            h4.Colspan = 2;
            h4.HorizontalAlignment = 1;
            table.AddCell(h4);

            PdfPTable h51 = new PdfPTable(1);
            h51.AddCell(new Phrase(it.ItemNo, font));
            h51.AddCell(new Phrase(it.Memo, font));

            PdfPCell h5 = new PdfPCell(h51);
            h5.Colspan = 5;
            h5.HorizontalAlignment = 1;
            table.AddCell(h5);

            table.AddCell(new Phrase("名称", font));
            table.AddCell(new Phrase("类别", font));
            table.AddCell(new Phrase("规格", font));
            table.AddCell(new Phrase("材质", font));
            table.AddCell(new Phrase("标准", font));
            //table.AddCell(new Phrase("长度(米)", font));
            table.AddCell(new Phrase("数量(支)", font));
            table.AddCell(new Phrase("总重(T)", font));
            table.AddCell(new Phrase("实际到厂单价", font));
            table.AddCell(new Phrase("到货时间", font));
            table.AddCell(new Phrase("价格为理计 / 磅计", font));
            table.AddCell(new Phrase("是否承兑 / 现款", font));
            table.AddCell(new Phrase("货到付款 / 款到发货 / 预付款", font));
            table.AddCell(new Phrase("是否含税", font));
            table.AddCell(new Phrase("备注", font));

            PdfPCell h6 = new PdfPCell(new Phrase("报价单位", font));
            h6.Colspan = 8;
            h6.HorizontalAlignment = 1;
            table.AddCell(h6);

            table.AddCell(new Phrase("最低报价单位", font));
            table.AddCell(new Phrase("选购采购厂商", font));
            table.AddCell(new Phrase("备注", font));

            foreach (var item in list)
            {
                table.AddCell(new Phrase(item.Name, font));
                table.AddCell(new Phrase(item.Mode, font));
                table.AddCell(new Phrase(item.Spec, font));
                table.AddCell(new Phrase(item.Texture, font));
                table.AddCell(new Phrase(item.Norm, font));
                //table.AddCell(new Phrase(item.Extent, font));
                table.AddCell(new Phrase(item.Amount, font));
                table.AddCell(new Phrase(item.TotalWeight, font));
                table.AddCell(new Phrase(item.RealPrice, font));
                table.AddCell(new Phrase(item.ReachTime, font));
                table.AddCell(new Phrase(item.PriceUnit, font));
                table.AddCell(new Phrase(item.PriceMode, font));
                table.AddCell(new Phrase(item.PayMode, font));
                table.AddCell(new Phrase(item.IsTax, font));
                table.AddCell(new Phrase(item.Memo, font));
                table.AddCell(new Phrase(item.SupplierName1, font));
                table.AddCell(new Phrase(item.SupplierName2, font));
                table.AddCell(new Phrase(item.SupplierName3, font));
                table.AddCell(new Phrase(item.SupplierName4, font));
                table.AddCell(new Phrase(item.SupplierName5, font));
                table.AddCell(new Phrase(item.SupplierName6, font));
                table.AddCell(new Phrase(item.SupplierName7, font));
                table.AddCell(new Phrase(item.SupplierName8, font));
                table.AddCell(new Phrase(item.MinName, font));
                table.AddCell(new Phrase(item.InName, font));
                table.AddCell(new Phrase(item.Explain, font));
            }

            document.Add(table);
            document.Close();
            return name;
        }

        public string ReportToPDF4(Item it, IEnumerable<ItemContrast> list)
        {
            BaseFont baseFont = BaseFont.CreateFont("C:\\WINDOWS\\FONTS\\STSONG.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 9);
            iTextSharp.text.Font fontTitle = new iTextSharp.text.Font(baseFont, 15);
            //设置边界
            Document document = new Document(PageSize.A2.Rotate());
            string name = $"管材询价单{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.pdf";
            PdfWriter.GetInstance(document, new FileStream(AppDomain.CurrentDomain.BaseDirectory + $"/UploadFile/pdf/{name}", FileMode.Create));
            // 添加文档信息
            document.AddTitle("管材询价单");
            document.AddSubject("Contract of PDFInfo");
            document.AddKeywords("Info, PDF, Contract");
            //document.AddCreator("Contract");
            document.AddAuthor("");
            document.Open();
            // 添加文档内容
            PdfPTable table = new PdfPTable(26);
            table.SetWidths(new float[] { 600f, 600f, 600f, 600f, 600f, 600f, 600f, 600f, 850f, 600f, 1000f, 950f, 1750f, 600f, 600f, 600f, 600f, 600f, 600f, 600f, 600f, 600f, 600f, 1000f, 1000f, 1000f });

            PdfPCell h1 = new PdfPCell(new Phrase("采购单位：青岛武晓集团有限公司", fontTitle));
            h1.Colspan = 15;
            h1.HorizontalAlignment = 1;
            table.AddCell(h1);

            PdfPCell h2 = new PdfPCell(new Phrase("询价类别", fontTitle));
            h2.Colspan = 2;
            h2.HorizontalAlignment = 1;
            table.AddCell(h2);

            PdfPCell h3 = new PdfPCell(new Phrase("管材询价单", fontTitle));
            h3.Colspan = 6;
            h3.HorizontalAlignment = 1;
            table.AddCell(h3);

            PdfPTable h41 = new PdfPTable(1);
            h41.AddCell(new Phrase("询价期间", font));
            h41.AddCell(new Phrase($"{it.StartDate.ToString("yyyy-MM-dd HH:mm")} 至 {it.EndDate.ToString("yyyy-MM-dd HH:mm")}", font));

            PdfPCell h4 = new PdfPCell(h41);
            h4.Colspan = 2;
            h4.HorizontalAlignment = 1;
            table.AddCell(h4);

            PdfPTable h51 = new PdfPTable(1);
            h51.AddCell(new Phrase(it.ItemNo, font));
            h51.AddCell(new Phrase(it.Memo, font));

            PdfPCell h5 = new PdfPCell(h51);
            h5.Colspan = 5;
            h5.HorizontalAlignment = 1;
            table.AddCell(h5);

            table.AddCell(new Phrase("名称", font));
            table.AddCell(new Phrase("类别", font));
            table.AddCell(new Phrase("规格", font));
            table.AddCell(new Phrase("材质", font));
            table.AddCell(new Phrase("标准", font));
            table.AddCell(new Phrase("长度(米)", font));
            table.AddCell(new Phrase("数量(支)", font));
            table.AddCell(new Phrase("总重(T)", font));
            table.AddCell(new Phrase("实际到厂单价", font));
            table.AddCell(new Phrase("到货时间", font));
            table.AddCell(new Phrase("价格为理计 / 磅计", font));
            table.AddCell(new Phrase("是否承兑 / 现款", font));
            table.AddCell(new Phrase("货到付款 / 款到发货 / 预付款", font));
            table.AddCell(new Phrase("是否含税", font));
            table.AddCell(new Phrase("备注", font));

            PdfPCell h6 = new PdfPCell(new Phrase("报价单位", font));
            h6.Colspan = 8;
            h6.HorizontalAlignment = 1;
            table.AddCell(h6);

            table.AddCell(new Phrase("最低报价单位", font));
            table.AddCell(new Phrase("选购采购厂商", font));
            table.AddCell(new Phrase("备注", font));

            foreach (var item in list)
            {
                table.AddCell(new Phrase(item.Name, font));
                table.AddCell(new Phrase(item.Mode, font));
                table.AddCell(new Phrase(item.Spec, font));
                table.AddCell(new Phrase(item.Texture, font));
                table.AddCell(new Phrase(item.Norm, font));
                table.AddCell(new Phrase(item.Extent, font));
                table.AddCell(new Phrase(item.Amount, font));
                table.AddCell(new Phrase(item.TotalWeight, font));
                table.AddCell(new Phrase(item.RealPrice, font));
                table.AddCell(new Phrase(item.ReachTime, font));
                table.AddCell(new Phrase(item.PriceUnit, font));
                table.AddCell(new Phrase(item.PriceMode, font));
                table.AddCell(new Phrase(item.PayMode, font));
                table.AddCell(new Phrase(item.IsTax, font));
                table.AddCell(new Phrase(item.Memo, font));
                table.AddCell(new Phrase(item.SupplierName1, font));
                table.AddCell(new Phrase(item.SupplierName2, font));
                table.AddCell(new Phrase(item.SupplierName3, font));
                table.AddCell(new Phrase(item.SupplierName4, font));
                table.AddCell(new Phrase(item.SupplierName5, font));
                table.AddCell(new Phrase(item.SupplierName6, font));
                table.AddCell(new Phrase(item.SupplierName7, font));
                table.AddCell(new Phrase(item.SupplierName8, font));
                table.AddCell(new Phrase(item.MinName, font));
                table.AddCell(new Phrase(item.InName, font));
                table.AddCell(new Phrase(item.Explain, font));
            }

            document.Add(table);
            document.Close();
            return name;
        }

        public string ReportToPDF5(Item it, IEnumerable<ItemContrast> list)
        {
            BaseFont baseFont = BaseFont.CreateFont("C:\\WINDOWS\\FONTS\\STSONG.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 9);
            iTextSharp.text.Font fontTitle = new iTextSharp.text.Font(baseFont, 15);
            //设置边界
            Document document = new Document(PageSize.A2.Rotate());
            string name = $"法兰询价单{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.pdf";
            PdfWriter.GetInstance(document, new FileStream(AppDomain.CurrentDomain.BaseDirectory + $"/UploadFile/UploadFile/pdf/{name}", FileMode.Create));
            // 添加文档信息
            document.AddTitle("法兰询价单");
            document.AddSubject("Contract of PDFInfo");
            document.AddKeywords("Info, PDF, Contract");
            //document.AddCreator("Contract");
            document.AddAuthor("");
            document.Open();
            // 添加文档内容
            PdfPTable table = new PdfPTable(25);
            table.SetWidths(new float[] { 600f, 600f, 600f, 600f, 600f, 600f, 600f, 850f, 600f, 1000f, 950f, 1750f, 600f, 600f, 600f, 600f, 600f, 600f, 600f, 600f, 600f, 600f, 1000f, 1000f, 1000f });

            PdfPCell h1 = new PdfPCell(new Phrase("采购单位：青岛武晓集团有限公司", fontTitle));
            h1.Colspan = 14;
            h1.HorizontalAlignment = 1;
            table.AddCell(h1);

            PdfPCell h2 = new PdfPCell(new Phrase("询价类别", fontTitle));
            h2.Colspan = 2;
            h2.HorizontalAlignment = 1;
            table.AddCell(h2);

            PdfPCell h3 = new PdfPCell(new Phrase("法兰询价单", fontTitle));
            h3.Colspan = 6;
            h3.HorizontalAlignment = 1;
            table.AddCell(h3);

            PdfPTable h41 = new PdfPTable(1);
            h41.AddCell(new Phrase("询价期间", font));
            h41.AddCell(new Phrase($"{it.StartDate.ToString("yyyy-MM-dd HH:mm")} 至 {it.EndDate.ToString("yyyy-MM-dd HH:mm")}", font));

            PdfPCell h4 = new PdfPCell(h41);
            h4.Colspan = 2;
            h4.HorizontalAlignment = 1;
            table.AddCell(h4);

            PdfPTable h51 = new PdfPTable(1);
            h51.AddCell(new Phrase(it.ItemNo, font));
            h51.AddCell(new Phrase(it.Memo, font));

            PdfPCell h5 = new PdfPCell(h51);
            h5.Colspan = 5;
            h5.HorizontalAlignment = 1;
            table.AddCell(h5);

            table.AddCell(new Phrase("名称", font));
            table.AddCell(new Phrase("类别", font));
            table.AddCell(new Phrase("规格", font));
            table.AddCell(new Phrase("材质", font));
            table.AddCell(new Phrase("标准", font));
            //table.AddCell(new Phrase("长度(米)", font));
            table.AddCell(new Phrase("数量(支)", font));
            table.AddCell(new Phrase("总重(T)", font));
            table.AddCell(new Phrase("实际到厂单价", font));
            table.AddCell(new Phrase("到货时间", font));
            table.AddCell(new Phrase("价格为理计 / 磅计", font));
            table.AddCell(new Phrase("是否承兑 / 现款", font));
            table.AddCell(new Phrase("货到付款 / 款到发货 / 预付款", font));
            table.AddCell(new Phrase("是否含税", font));
            table.AddCell(new Phrase("备注", font));

            PdfPCell h6 = new PdfPCell(new Phrase("报价单位", font));
            h6.Colspan = 8;
            h6.HorizontalAlignment = 1;
            table.AddCell(h6);

            table.AddCell(new Phrase("最低报价单位", font));
            table.AddCell(new Phrase("选购采购厂商", font));
            table.AddCell(new Phrase("备注", font));

            foreach (var item in list)
            {
                table.AddCell(new Phrase(item.Name, font));
                table.AddCell(new Phrase(item.Mode, font));
                table.AddCell(new Phrase(item.Spec, font));
                table.AddCell(new Phrase(item.Texture, font));
                table.AddCell(new Phrase(item.Norm, font));
                //table.AddCell(new Phrase(item.Extent, font));
                table.AddCell(new Phrase(item.Amount, font));
                table.AddCell(new Phrase(item.TotalWeight, font));
                table.AddCell(new Phrase(item.RealPrice, font));
                table.AddCell(new Phrase(item.ReachTime, font));
                table.AddCell(new Phrase(item.PriceUnit, font));
                table.AddCell(new Phrase(item.PriceMode, font));
                table.AddCell(new Phrase(item.PayMode, font));
                table.AddCell(new Phrase(item.IsTax, font));
                table.AddCell(new Phrase(item.Memo, font));
                table.AddCell(new Phrase(item.SupplierName1, font));
                table.AddCell(new Phrase(item.SupplierName2, font));
                table.AddCell(new Phrase(item.SupplierName3, font));
                table.AddCell(new Phrase(item.SupplierName4, font));
                table.AddCell(new Phrase(item.SupplierName5, font));
                table.AddCell(new Phrase(item.SupplierName6, font));
                table.AddCell(new Phrase(item.SupplierName7, font));
                table.AddCell(new Phrase(item.SupplierName8, font));
                table.AddCell(new Phrase(item.MinName, font));
                table.AddCell(new Phrase(item.InName, font));
                table.AddCell(new Phrase(item.Explain, font));
            }

            document.Add(table);
            document.Close();
            return name;
        }
    }
}
