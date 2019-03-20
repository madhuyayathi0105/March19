<%@ Page Title="" Language="C#" MasterPageFile="~/InventoryMod/inventorysite.master"
    AutoEventWireup="true" CodeFile="Inv_CodeMaster.aspx.cs" Inherits="Inv_CodeMaster" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title></title>
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            .container
            {
                width: 100%;
            }
            .col1
            {
                float: left;
                width: 50%;
            }
            .col2
            {
                float: right;
                width: 50%;
            }
        </style>
    </head>
    <body>
        <form id="form1">
        <script type="text/javascript">
            function checkvalue() {
                var fl = 0; var f2 = 0;
                var id = document.getElementById("<%=ug_grid.ClientID %>");
                var gridViewControls = id.getElementsByTagName("input");
                var len = id.rows.length;
                var row = gridViewControls.length - 1;
                for (var i = 0; i < gridViewControls.length; i++) {
                    if (i != row) {
                        if (gridViewControls[i].name.indexOf("txt_acronym") > 1) {
                            if (gridViewControls[i].value == "") {
                                fl = 1;
                            }
                        }
                        if (gridViewControls[i].name.indexOf("txt_startno") > 1) {
                            if (gridViewControls[i].value == "") {
                                fl = 1;
                            }
                        }
                        if (gridViewControls[i].name.indexOf("txt_size") > 1) {
                            if (gridViewControls[i].value == "") {
                                fl = 1;
                            }
                        }
                        if (gridViewControls[i].name.indexOf("txt_size") > 6) {
                            if (gridViewControls[i].value > 6) {
                                f2 = 1;
                            }
                        }
                    }
                    else {
                        if (gridViewControls[i].name.indexOf("txt_size") > 6) {
                            if (gridViewControls[i].value > 6) {
                                f2 = 1;
                            }

                        }
                    }
                }
                if (f2 == 1) {
                    alert('Please Enter Size Less than equal to 6');

                    return false;
                } else if (fl == 1) {
                    alert('Please Fill All Values');

                    return false;
                }
                else {

                    return true;
                }
            }
            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Code Master</span></div>
        </center>
        <br />
        <div>
            <div class="maindivstyle" style="height: 608px;">
                <center>
                    <br />
                    <table class="maintablestyle" style="width: 870px">
                        <tr>
                            <td>
                                <span>College Name</span>
                                <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox1 ddlstyle ddlheight6"
                                    OnSelectedIndexChanged="ddl_collegeSelectedindexchange" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_frmdate" Text="From Date" runat="server"></asp:Label>
                            </td>
                            <td rowspan="2">
                                <asp:TextBox ID="txt_frmdate" runat="server" CssClass="textbox textbox1" Width="80px"></asp:TextBox>
                                <asp:CalendarExtender ID="calfrmdate" TargetControlID="txt_frmdate" runat="server"
                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_prvdate" Text="Previous Date" runat="server"></asp:Label>
                            </td>
                            <td rowspan="2">
                                <asp:TextBox ID="txt_prvdate" runat="server" CssClass="textbox textbox1" Width="80px"
                                    Enabled="false" Visible="false"></asp:TextBox>
                                <asp:CalendarExtender ID="calvacatedate" TargetControlID="txt_prvdate" runat="server"
                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                                <asp:DropDownList ID="ddl_PrevDate" runat="server" CssClass=" textbox1  ddlheight2"
                                    OnSelectedIndexChanged="ddl_PrevDate_OnSelectedIndexChange" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </center>
                <br />
                <div>
                    <center>
                        <table class="table">
                            <tr>
                                <td>
                                    <fieldset style="border-radius: 10px;">
                                        <legend>New Setting</legend>
                                        <div>
                                            <asp:GridView ID="ug_grid" runat="server" AutoGenerateColumns="false" GridLines="None"
                                                OnRowDataBound="grid_prev_Bound0" OnDataBound="OnDataBound0">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Item Code" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_itemdetails" runat="server" Text='<%#Eval("Dummy1") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText=" " HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cb_x" runat="server" Text="Dept" />
                                                            <br />
                                                            <asp:CheckBox ID="cb_x1" runat="server" Text="Item" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Acronym" HeaderStyle-BackColor="#0CA6CA">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_acronym" runat="server" CssClass="textbox txtheight" onfocus="return myFunction2(this)"
                                                                MaxLength="35" Style="text-transform: uppercase;"></asp:TextBox>
                                                            <%--   theivamani 5.11.15--%>
                                                            <asp:FilteredTextBoxExtender ID="filterextenderacr" runat="server" TargetControlID="txt_acronym"
                                                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=" ">
                                                            </asp:FilteredTextBoxExtender>
                                                            <%--                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender147" runat="server" TargetControlID="txt_acronym"
                                                        FilterType="Numbers" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Start No" HeaderStyle-BackColor="#0CA6CA">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_startno" runat="server" CssClass="textbox txtheight" onfocus="return myFunction2(this)"
                                                                MaxLength="6"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="filterextender" runat="server" TargetControlID="txt_startno"
                                                                FilterType="Numbers" ValidChars=" ">
                                                            </asp:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Size" HeaderStyle-BackColor="#0CA6CA">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_size" runat="server" CssClass="textbox txtheight" Width="30px"
                                                                MaxLength="1" onfocus="return myFunction2(this)" onchange="myFunction(this)"></asp:TextBox>
                                                            <asp:RangeValidator ID="Range1" ControlToValidate="txt_size" MinimumValue="1" MaximumValue="6"
                                                                Type="Integer" runat="server" ToolTip="Enter Only 1 - 6" SetFocusOnError="False" />
                                                            <asp:FilteredTextBoxExtender ID="filterextender1" runat="server" TargetControlID="txt_size"
                                                                FilterType="Numbers" ValidChars=" ">
                                                            </asp:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="border-radius: 10px;">
                                        <legend>Old Setting</legend>
                                        <div>
                                            <asp:GridView ID="grid_prev" runat="server" AutoGenerateColumns="false" OnRowDataBound="grid_prev_Bound"
                                                GridLines="None" OnDataBound="OnDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_sno1" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Item Code" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_itemdetails1" runat="server" Text='<%#Eval("Dummy1") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText=" " HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cb_y" runat="server" Text="Dept" Enabled="false" />
                                                            <br />
                                                            <asp:CheckBox ID="cb_y1" runat="server" Text="Item" Enabled="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Acronym" HeaderStyle-BackColor="#0CA6CA">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_acronym1" runat="server" CssClass="textbox txtheight" Enabled="false"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="filterextenderacr1" runat="server" TargetControlID="txt_acronym1"
                                                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=" ">
                                                            </asp:FilteredTextBoxExtender>
                                                            <%--                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1472" runat="server" TargetControlID="txt_acronym1"
                                                        FilterType="Numbers" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Start No" HeaderStyle-BackColor="#0CA6CA">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_startno1" runat="server" CssClass="textbox txtheight" Enabled="false"
                                                                MaxLength="6"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="filterextender2" runat="server" TargetControlID="txt_startno1"
                                                                FilterType="Numbers" ValidChars=" ">
                                                            </asp:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Size" HeaderStyle-BackColor="#0CA6CA">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_size1" runat="server" CssClass="textbox txtheight" Width="30px"
                                                                MaxLength="3" Enabled="false"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="filterextender12" runat="server" TargetControlID="txt_size1"
                                                                FilterType="Numbers" ValidChars=" ">
                                                            </asp:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
                <br />
                <center>
                    <div class="container">
                        <div class="col1">
                            <%--style="position: absolute;margin-left: 170px; "--%>
                            <asp:Button ID="btn_save" Text="Save" runat="server" CssClass="textbox btn2" OnClick="btn_save_Click"
                                OnClientClick="return checkvalue()" />
                            <asp:Button ID="btn_reset" Text="Reset" runat="server" CssClass="textbox btn2" OnClick="btn_reset_Click" />
                            <%--<asp:Button ID="btn_exit" Text="Exit" runat="server" CssClass="textbox btn2" OnClick="btn_exit_Click" />--%>
                        </div>
                        <div class="col2">
                            <%--style="position: absolute;margin-left: 701px;"--%>
                            <asp:Button ID="btn_delete" Text="Delete" runat="server" CssClass="textbox btn2"
                                OnClick="delete_btn_Click" />
                        </div>
                    </div>
                </center>
            </div>
            <br />
            <div id="imgdiv2" runat="server" visible="false" class="popupstyle" style="height: 50em;">
                <center>
                    <div id="panel_erralert" runat="server" class="table" style="background-color: White;
                        height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        margin-top: 200px; border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_alerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_errclose" CssClass=" textbox btn2 comm" OnClick="btn_errclose_Click"
                                                Text="OK" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
            <%--theivamani 5.11.15--%>
            <center>
                <div id="surediv" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div3" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_sure" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_yes" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                    OnClick="btn_sureyes_Click" Text="yes" runat="server" />
                                                <asp:Button ID="btn_no" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                    OnClick="btn_sureno_Click" Text="no" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
        </div>
        </form>
    </body>
    </html>
</asp:Content>
