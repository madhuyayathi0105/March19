<%@ Page Title="" Language="C#" MasterPageFile="~/InventoryMod/inventorysite.master" AutoEventWireup="true"
    CodeFile="HM_Purchasestatus_Report.aspx.cs" Inherits="HM_Purchasestatus_Report" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title></title>
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
        <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            .cont
            {
                width: 200px;
                height: 30px;
            }
            .btn
            {
                width: 40px;
                height: 30px;
            }
            .btn1
            {
                width: 80px;
                height: 30px;
            }
            .sty1
            {
                height: 300px;
                width: 450px;
                margin-left: 5%;
                background-color: White;
                border: 5px solid #0CA6CA;
                border-top: 30px solid #0CA6CA;
                border-radius: 10px;
            }
            .backpaneldrop
            {
                position: absolute;
                background-color: White;
                border: 1px solid Gray;
            }
            .style
            {
                height: 500px;
                width: 1000px;
                border: 1px solid #999999;
                background-color: #F0F0F0;
                box-shadow: 0px 0px 8px #999999; /*F0F0F0*/
                -moz-box-shadow: 0px 0px 10px #999999;
                -webkit-box-shadow: 0px 0px 10px #999999;
                border: 3px solid #D9D9D9;
                border-radius: 15px;
            }
            .spreadborder
            {
                border: 2px solid #999999;
                background-color: White;
                box-shadow: 0px 0px 8px #999999; /*F0F0F0*/
                border-radius: 10px;
                overflow: auto;
            }
            
            .ddlstyle
            {
                width: 200px;
                height: 30px;
                outline: none;
                border: 1px solid #7bc1f7;
                box-shadow: 0px 0px 8px #7bc1f7;
                -moz-box-shadow: 0px 0px 8px #7bc1f7;
                -webkit-box-shadow: 0px 0px 8px #7bc1f7;
            }
            .txtdate
            {
                border: 1px solid #c4c4c4;
                height: 20px;
                width: 70px;
                font-size: 13px;
                text-transform: capitalize;
                padding: 4px 4px 4px 4px;
                border-radius: 4px;
                -moz-border-radius: 4px;
                -webkit-border-radius: 4px;
                box-shadow: 0px 0px 8px #d9d9d9;
                -moz-box-shadow: 0px 0px 8px #d9d9d9;
                -webkit-box-shadow: 0px 0px 8px #d9d9d9;
            }
            .multxtpanel
            {
                background: White;
                border-color: Gray;
                border-style: Solid;
                border-width: 2px;
                position: absolute;
                box-shadow: 0px 0px 4px #999999;
                border-radius: 5px;
                overflow: auto;
            }
        </style>
    </head>
    <body>
        <form id="form1">
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }    
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <br />
            <center>
                <div>
                    <center>
                        <asp:Label ID="Label1" runat="server" Style="color: Green;" Text="Purchase Status Report"
                            CssClass="fontstyleheader"></asp:Label>
                        <br />
                        <br />
                    </center>
                </div>
            </center>
            <center>
                <div class="style">
                    <br />
                    <center>
                        <table style="margin-left: 70px; border: 1px solid #0CA6CA; border-radius: 10px;
                            background-color: #0CA6CA; position: absolute; width: 880px; height: 50px; box-shadow: 0px 0px 8px #999999;">
                            <tr>
                                <td style="display: none;">
                                    <asp:Label ID="lblhos" runat="server" Text="Hostel Name"></asp:Label>
                                </td>
                                <td style="display: none;">
                                    <asp:DropDownList ID="ddlhos" runat="server" AutoPostBack="true" Width="170px" CssClass="textbox ddlstyle">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="From Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtfrom" runat="server" AutoPostBack="true" Width="75px" CssClass="textbox textbox1"
                                        ForeColor="Black" OnTextChanged="txtfrom_TextChanged"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtfrom" runat="server"
                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:Label ID="lblto" runat="server" Text="To Date"></asp:Label>
                                    <asp:TextBox ID="txtto" runat="server" AutoPostBack="true" Width="75px" ForeColor="Black"
                                        CssClass="textbox textbox1" OnTextChanged="txtto_TextChanged"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtto" runat="server"
                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lblvendor1" runat="server" Text="Vendor"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtvenname" runat="server" Width="160px" CssClass="textbox textbox1"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Width="250px" Height="250px">
                                                <asp:CheckBox ID="Chkven" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="Chksechosname" />
                                                <asp:CheckBoxList ID="Cblven" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cblsechosname">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtvenname"
                                                PopupControlID="Panel1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblitm" runat="server" Text="Item Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upp1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtitmname" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                                Width="160px" Height="20px">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel" Width="200px" Height="250px">
                                                <asp:CheckBox ID="Chkitm" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="Chkitmname" />
                                                <asp:CheckBoxList ID="Cblitm" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cblitmname">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtitmname"
                                                PopupControlID="Panel4" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngoclick" CssClass="textbox btn" />
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <br />
                    <br />
                    <div>
                        <center>
                            <asp:Label ID="lblerror" runat="server" ForeColor="Red"></asp:Label>
                        </center>
                    </div>
                    <center>
                        <div id="spreaddiv" runat="server" visible="false" style="width: 770px; height: 350px;"
                            class="spreadborder">
                            <br />
                            <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="1px" Width="750px" Height="350px">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                    </center>
                    <br />
                    <center>
                        <div id="rptprint" runat="server" visible="false">
                            <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                Visible="false"></asp:Label>
                            <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" onkeypress="display()"
                                Font-Size="Medium" CssClass="textbox textbox1 "></asp:TextBox>
                            <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox btn1"
                                Text="Export To Excel" Width="127px" />
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                CssClass="textbox btn1" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                    </center>
                </div>
            </center>
        </div>
        </form>
    </body>
    </html>
</asp:Content>
