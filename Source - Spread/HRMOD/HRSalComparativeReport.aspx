<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="HRSalComparativeReport.aspx.cs" Inherits="HRSalComparativeReport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span id="sphd" runat="server" class="fontstyleheader" style="color: Green;">Salary
                        Comparative Statement</span>
                </div>
            </center>
        </div>
        <div>
            <center>
                <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtclg" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlclg" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 350px;
                                            height: 200px;">
                                            <asp:CheckBox ID="cbclg" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cbclg_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cblclg" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblclg_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txtclg"
                                            PopupControlID="pnlclg" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblmon" runat="server" Text="Month"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlmonth" runat="server" CssClass="ddlheight textbox1">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblyr" runat="server" Text="Year"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlyear" runat="server" CssClass="ddlheight textbox1">
                                </asp:DropDownList>
                            </td>
                            <td style="display: none;">
                                <asp:Label ID="Label1" runat="server" Text="Allownace"></asp:Label>
                            </td>
                            <td style="display: none;">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtallow" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 350px;
                                            height: 120px;">
                                            <asp:CheckBox ID="cballow" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cballow_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cblallow" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblallow_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtallow"
                                            PopupControlID="Panel1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Deduction"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtdeduct" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 350px;
                                            height: 200px;">
                                            <asp:CheckBox ID="cbdeduct" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cbdeduct_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbldeduct" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbldeduct_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtdeduct"
                                            PopupControlID="Panel2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cbDifference" runat="server" Text="Difference" />
                            </td>
                            <td>
                                <asp:Button ID="btngo" runat="server" CssClass="textbox btn2" Text="Go" OnClick="btngo_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <FarPoint:FpSpread ID="spreadDet" runat="server" Visible="false" BorderStyle="Solid"
                        BorderWidth="0px" Width="980px" Style="overflow: auto; border: 0px solid #999999;
                        border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                        class="spreadborder">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <center>
                        <br />
                        <div id="print" runat="server" visible="false">
                            <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="" Style="display: none;"></asp:Label>
                            <asp:Label ID="lblrptname" runat="server" Visible="false" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" Visible="false" Width="180px" onkeypress="display()"
                                CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                InvalidChars="/\">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnExcel" runat="server" Visible="false" OnClick="btnExcel_Click"
                                Text="Export To Excel" Width="127px" Height="32px" CssClass="textbox textbox1" />
                            <asp:Button ID="btnprintmasterhed" runat="server" Visible="false" Text="Print" OnClick="btnprintmaster_Click"
                                Height="32px" Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                            <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                        </div>
                    </center>
                    <br />
                    <div id="Deduction" runat="server" visible="false">
                        <FarPoint:FpSpread ID="DeductionDetSp" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Width="980px" Style="margin-left: 2px;" class="spreadborder"
                            ShowHeaderSelection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <br />
                        <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                            Visible="false"></asp:Label>
                        <asp:Label ID="Label4" runat="server" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="TextBox1" CssClass="textbox textbox1" runat="server" Height="20px"
                            Width="180px" onkeypress="display()"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,. ">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="Button1" runat="server" CssClass="textbox btn1" Text="Export To Excel"
                            Width="127px" OnClick="btnExcel_Click1" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" CssClass="textbox btn1"
                            OnClick="btnprintmaster_Click1" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                    <br />
                </div>
            </center>
        </div>
    </body>
</asp:Content>
