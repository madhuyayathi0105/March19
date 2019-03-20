<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="SalaryHoldSet.aspx.cs" Inherits="SalaryHoldSet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <%--<script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>--%>
    <script type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
    <style>
        body
        {
            font-family: Book Antiqua;
            font-size: 14px;
        }
        .myLink:hover
        {
            color: Red;
        }
    </style>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <script type="text/javascript">
            function SelectAll(id) {
                //get reference of GridView control
                var grid = document.getElementById("<%= grdstaffhold.ClientID %>");
                //variable to contain the cell of the grid
                var cell;

                if (grid.rows.length > 0) {
                    //loop starts from 1. rows[0] points to the header.
                    for (i = 1; i < grid.rows.length; i++) {
                        //get the reference of first column
                        cell = grid.rows[i].cells[1];

                        //loop according to the number of childNodes in the cell
                        for (j = 0; j < cell.childNodes.length; j++) {
                            //if childNode type is CheckBox                 
                            if (cell.childNodes[j].type == "checkbox") {
                                //assign the status of the Select All checkbox to the cell 
                                //checkbox within the grid
                                cell.childNodes[j].checked = document.getElementById(id).checked;
                            }
                        }
                    }
                }
            }

        </script>
        <center>
            <div>
                <center>
                    <br />
                    <div>
                        <span class="fontstyleheader" style="color: Green">Salary Hold </span>
                    </div>
                </center>
                <center>
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <ContentTemplate>
                            <div class="maindivstyle" style="height: 142px; width: 1050px;">
                                <br />
                                <table class="maintablestyle">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblcoll" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                CssClass="textbox1 ddlheight5" OnSelectedIndexChanged="ddlcollege_change" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Department
                                        </td>
                                        <%-- <td>
                                            <asp:CheckBox ID="chkdept" runat="server" Text="Department" AutoPostBack="true" OnCheckedChanged="chkdept_change" />
                                        </td>--%>
                                        <td>
                                            <asp:UpdatePanel ID="upddeptcom" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtdeptcom" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                                        Style="width: 135px;">--Select--</asp:TextBox>
                                                    <asp:Panel ID="pnldeptcom" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                                        position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                                        height: 200px;">
                                                        <asp:CheckBox ID="cbdeptcom" runat="server" Text="Select All" OnCheckedChanged="cbdeptcom_CheckedChange"
                                                            AutoPostBack="true" />
                                                        <asp:CheckBoxList ID="cbldeptcom" runat="server" OnSelectedIndexChanged="cbldeptcom_SelectedIndexChange"
                                                            AutoPostBack="true">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtdeptcom"
                                                        PopupControlID="pnldeptcom" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            Designation
                                        </td>
                                        <%--<td>
                                            <asp:CheckBox ID="cbDesig" runat="server" Text="Designation" AutoPostBack="true"
                                                OnCheckedChanged="cbDesigChange" Checked="false" />
                                        </td>--%>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_desig" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                        Style="width: 206px;">--Select--</asp:TextBox>
                                                    <asp:Panel ID="P2" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                                        border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                                        box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                                        <asp:CheckBox ID="cb_desig" runat="server" Text="Select All" OnCheckedChanged="cb_desig_CheckedChange"
                                                            AutoPostBack="true" />
                                                        <asp:CheckBoxList ID="cbl_desig" runat="server" OnSelectedIndexChanged="cbl_desig_SelectedIndexChange"
                                                            AutoPostBack="true">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_desig"
                                                        PopupControlID="P2" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Staff Type
                                        </td>
                                        <%-- <td>
                                            <asp:CheckBox ID="chkstftype" runat="server" Text="Staff Type" AutoPostBack="true"
                                                OnCheckedChanged="chkstftype_change" />
                                        </td>--%>
                                        <td>
                                            <asp:UpdatePanel ID="updstfcom" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtstftypecom" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                                        Style="width: 234px; margin-left: 0px;">--Select--</asp:TextBox>
                                                    <asp:Panel ID="pnlstftypecom" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                                        position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                                        height: 200px;">
                                                        <asp:CheckBox ID="cbstftypecom" runat="server" Text="Select All" OnCheckedChanged="cbstftypecom_CheckedChange"
                                                            AutoPostBack="true" />
                                                        <asp:CheckBoxList ID="cblstftypecom" runat="server" OnSelectedIndexChanged="cblstftypecom_SelectedIndexChange"
                                                            AutoPostBack="true">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtstftypecom"
                                                        PopupControlID="pnlstftypecom" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            Staff Category
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="updstfcatcom" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtscatcom" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                                        Style="width: 135px; margin-left: 0px;">--Select--</asp:TextBox>
                                                    <asp:Panel ID="pnlscatcom" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                                        position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                                        height: 200px;">
                                                        <asp:CheckBox ID="cbscatcom" runat="server" Text="Select All" OnCheckedChanged="cbscatcom_CheckedChange"
                                                            AutoPostBack="true" />
                                                        <asp:CheckBoxList ID="cblscatcom" runat="server" OnSelectedIndexChanged="cblscatcom_SelectedIndexChange"
                                                            AutoPostBack="true">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txtscatcom"
                                                        PopupControlID="pnlscatcom" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <asp:UpdatePanel ID="upl" runat="server">
                                            <ContentTemplate>
                                                <td colspan="12">
                                                    Month & Year
                                                    <asp:DropDownList ID="ddl_mon" runat="server" OnSelectedIndexChanged="ddl_mon_Change"
                                                        AutoPostBack="true" CssClass="textbox1 ddlheight1">
                                                        <asp:ListItem Value="1">January</asp:ListItem>
                                                        <asp:ListItem Value="2">February</asp:ListItem>
                                                        <asp:ListItem Value="3">March</asp:ListItem>
                                                        <asp:ListItem Value="4">April</asp:ListItem>
                                                        <asp:ListItem Value="5">May</asp:ListItem>
                                                        <asp:ListItem Value="6">June</asp:ListItem>
                                                        <asp:ListItem Value="7">July</asp:ListItem>
                                                        <asp:ListItem Value="8">August</asp:ListItem>
                                                        <asp:ListItem Value="9">September</asp:ListItem>
                                                        <asp:ListItem Value="10">October</asp:ListItem>
                                                        <asp:ListItem Value="11">November</asp:ListItem>
                                                        <asp:ListItem Value="12">December</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="ddl_year" runat="server" CssClass="textbox1 ddlheight1">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                </td>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbhold" runat="server" Text="Hold" Checked="true" AutoPostBack="true"
                                                OnCheckedChanged="rdbhold_check" />
                                            <asp:RadioButton ID="rdbUnhold" runat="server" Text="Unhold" AutoPostBack="true"
                                                OnCheckedChanged="rdbunhold_check" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnshow" runat="server" Text="GO" CssClass="textbox1 btn2" OnClick="btnshow_click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </center>
                <center>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                            <asp:GridView ID="grdstaffhold" Width="570px" runat="server" ShowFooter="false"
                                AutoGenerateColumns="true" Font-Names="Book Antiqua" toGenerateColumns="true"
                                OnRowDataBound="grdstaffhold_RowDataBound" AllowPaging="true" PageSize="100"
                                OnPageIndexChanging="grdstaffhold_OnPageIndexChanged">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="allchk" runat="server" Text="Select All" onchange="return SelLedgers();" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="selectchk" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </center>
                <br />
                <br />
                <br />
                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btn_save_click" Visible="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </center>
    </body>
</asp:Content>
