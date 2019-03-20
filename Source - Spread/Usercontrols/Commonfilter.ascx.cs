using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FarPoint.Web.Spread;

public partial class Usercontrols_Commonfilter : System.Web.UI.UserControl
{    
    Button btn;

    public static FpSpread fpspreadsample;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public void loadspread(FpSpread FpSpread1)
    {
        try
        {
            ddlheader.Items.Clear();
            fpspreadsample = (FpSpread)FpSpread1;
            for (int j = 0; j < fpspreadsample.Sheets[0].ColumnHeader.RowCount; j++)
            {
                for (int i = 0; i < fpspreadsample.Sheets[0].ColumnCount; i++)
                {
                    string header = FpSpread1.Sheets[0].ColumnHeader.Cells[j, i].Text;
                    if (header.Trim() != "" && header != null)
                    {
                        System.Web.UI.WebControls.ListItem lt = new System.Web.UI.WebControls.ListItem { Text = header, Value = i.ToString() };
                        if (!ddlheader.Items.Contains(lt))
                        {
                            ddlheader.Items.Add(new System.Web.UI.WebControls.ListItem { Text = header, Value = i.ToString() });
                        }
                    }
                }
            }
            ddlvalue.Items.Clear();
            int column = Convert.ToInt32(ddlheader.SelectedValue.ToString());
            for (int i = 0; i < fpspreadsample.Sheets[0].RowCount; i++)
            {
                string value = fpspreadsample.Sheets[0].Cells[i, column].Text.ToString();
                if (value.Trim() != "" && value != null)
                {
                    System.Web.UI.WebControls.ListItem lt = new System.Web.UI.WebControls.ListItem { Text = value };
                    if (!ddlvalue.Items.Contains(lt))
                    {
                        ddlvalue.Items.Add(value);
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddlheader_SelectIndexChange(object sender, EventArgs e)
    {
        try
        {
            ddlvalue.Items.Clear();
            int column = Convert.ToInt32(ddlheader.SelectedValue.ToString());
            for (int i = 0; i < fpspreadsample.Sheets[0].RowCount; i++)
            {
                string value = fpspreadsample.Sheets[0].Cells[i, column].Text.ToString();
                if (value.Trim() != "" && value != null)
                {
                    System.Web.UI.WebControls.ListItem lt = new System.Web.UI.WebControls.ListItem { Text = value };
                    if (!ddlvalue.Items.Contains(lt))
                    {
                        ddlvalue.Items.Add(value);
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
}