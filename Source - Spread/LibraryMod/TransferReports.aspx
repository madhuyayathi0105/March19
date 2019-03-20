<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="TransferReports.aspx.cs" Inherits="LibraryMod_TransferReports" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Transfer Reports</span></div>
        </center>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
            <ContentTemplate>
                <center>
                    <div id="maindiv" runat="server" class="maindivstyle" style="font-family: Book Antiqua;
                        font-weight: bold; width: 1000px; height: auto">
                        <div>
                            <table class="maintablestyle" style="height: auto; margin-left: 0px; margin-top: 10px;
                                margin-bottom: 10px; padding: 6px; font-family: Book Antiqua; font-weight: bold">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCollege" runat="server" Text="College">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="155px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbltransferfrom" runat="server" Text="Transfer From">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddltransferfrom" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="155px" AutoPostBack="True" OnSelectedIndexChanged="ddltransferfrom_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbltransferto" runat="server" Text="Transfer To">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddltransferto" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="155px" AutoPostBack="True" OnSelectedIndexChanged="ddltransferto_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbooktype" runat="server" Text="Book Type">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbooktype" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="155px" AutoPostBack="True" OnSelectedIndexChanged="ddlbooktype_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td colspan="3">
                                        <asp:RadioButtonList ID="rbltransfertype" runat="server" RepeatDirection="Horizontal"
                                            AutoPostBack="true" OnSelectedIndexChanged="rbltransfertype_SelectedIndexChanged">
                                            <asp:ListItem Selected="true">Transfer Library</asp:ListItem>
                                            <asp:ListItem>Transfer Dept</asp:ListItem>
                                            <asp:ListItem>Dept Return</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <fieldset style="width: 400px; height: 20px;">
                                            <asp:CheckBox ID="cbfrom" runat="server" Text="Date Wise" AutoPostBack="true" OnCheckedChanged="cbfrom_OnCheckedChanged" />
                                            <asp:Label ID="lbl_fromdate1" runat="server" Text="From:"></asp:Label>
                                            <asp:TextBox ID="txt_fromdate1" runat="server" Enabled="false" Style="height: 20px;
                                                width: 85px;" onchange="return checkDate()"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_fromdate1" runat="server"
                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                            </asp:CalendarExtender>
                                            <asp:Label ID="lbl_todate1" runat="server" Text="To:" Style="margin-left: 4px;"></asp:Label>
                                            <asp:TextBox ID="txt_todate1" runat="server" Enabled="false" Style="height: 20px;
                                                width: 85px;" onchange="return checkDate()"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_todate1" runat="server"
                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                            </asp:CalendarExtender>
                                        </fieldset>
                                    </td>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanelbtn1" runat="server">
                                            <ContentTemplate>
                                                <asp:ImageButton ID="btn_go" runat="server" ImageUrl="~/LibImages/Go.jpg" Style="margin-top: 10px;"
                                                    OnClick="btn_go_Click" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </center>
                <br />
                <br />
                <center>
                    <center>
                        <asp:GridView ID="grdManualExit" runat="server" ShowFooter="false" AutoGenerateColumns="true" ShowHeader="true"
                            Font-Names="Book Antiqua" toGenerateColumns="false" AllowPaging="true" PageSize="500"
                            OnSelectedIndexChanged="grdManualExit_OnSelectedIndexChanged" OnPageIndexChanging="grdManualExit_OnPageIndexChanged">
                          
                            <HeaderStyle BackColor="#0CA6CA" ForeColor="White" />
                        </asp:GridView>
                    </center>
                </center>
                 <center>
                    <div id="rptprint1" runat="server" visible="false">
                        <br />
                        <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                   
                        <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox textbox1" Height="20px"
                            Width="180px" onkeypress="display1()" Style="font-family: 'Book Antiqua'" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtexcelname1"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                            InvalidChars="/\">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnExcel1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            OnClick="btnExcel1_Click" Font-Size="Medium" Text="Export To Excel" Width="127px"
                            Height="31px" CssClass="textbox textbox1" />
                        <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="31px"
                            CssClass="textbox textbox1" />
                         <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                    </div>
                    <br />
                </center>
                <center>
                    <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <br />
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanelbtn2" runat="server">
                                                    <ContentTemplate>
                                                        <center>
                                                            <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                                OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                                        </center>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
