using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;

public partial class BarCode : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnButton_Click(object sender, EventArgs e)
    {
        try
        {
            StringBuilder html = new StringBuilder();
            html.Append("<table>");
            string Hllogo = string.Empty;
            string Query = "select itemCode from iM_itemMaster";
            ds.Clear();
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    html.Append("<tr>");
                    string ulr = generateBarcode(Convert.ToString(ds.Tables[0].Rows[row]["itemCode"]));
                    html.Append("<td>");
                    html.Append(Convert.ToString(ds.Tables[0].Rows[row]["itemCode"]));
                    html.Append("</td>");
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/BarCode/" + ulr + ".Jpeg")))
                    {
                        Hllogo = "<img src='" + "../BarCode/" + ulr + ".Jpeg?" + "' style='height:80px; width:80px;'/>";
                    }
                    html.Append("<td>");
                    html.Append(Hllogo);
                    html.Append("</td>");
                    html.Append("<td>");
                    html.Append(Hllogo);
                    html.Append("</td>");
                    html.Append("<td>");
                    html.Append(Hllogo);
                    html.Append("</td>");
                    html.Append("</tr>");
                }
                html.Append("</table>");
                IDdiv.Visible = true;
                phTimeTable.Controls.Add(new Literal { Text = html.ToString() });
            }
        }
        catch
        {

        }
    }

    public string generateBarcode(string barCode)
    {
        string path = "barcodeimg" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + barCode;
        string urlImg = Server.MapPath("~/BarCode/" + path + ".Jpeg");
        System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
        using (Bitmap bitMap = new Bitmap(barCode.Length * 10, 20))
        {
            using (Graphics graphics = Graphics.FromImage(bitMap))
            {
                Font oFont = new Font("IDAutomationHC39M", 16);
                PointF point = new PointF(2f, 2f);
                SolidBrush blackBrush = new SolidBrush(Color.Black);
                SolidBrush whiteBrush = new SolidBrush(Color.White);
                graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                graphics.DrawString("*" + barCode + "*", oFont, blackBrush, point);
            }
            using (MemoryStream ms = new MemoryStream())
            {
                //bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                //byte[] byteImage = ms.ToArray();

                //Convert.ToBase64String(byteImage);
                //imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);


                bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                bitMap.Save(urlImg, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            return path;
        }

    }
}