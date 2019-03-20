<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ReceiptPaymentCumulative.aspx.cs" Inherits="ReceiptPaymentCumulative" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <body>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Receipt/Payment Cumulative Report</span></div>
            </center>
        </div>
        <div>
            <center>
                <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                    <center>
                        <div>
                            <table class="maintablestyle">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_collegename" Text="College" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtclg" runat="server" Style="height: 20px; width: 175px;" ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="pnlclg" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 350px;
                                                        height: 120px;">
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
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Header" Style="width: 50px;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_studhed" runat="server" Style="height: 20px; width: 100px;"
                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="pnl_studhed" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                    Style="width: 300px; height: auto;">
                                                    <asp:CheckBox ID="chk_studhed" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="chk_studhed_OnCheckedChanged" />
                                                    <asp:CheckBoxList ID="chkl_studhed" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_studhed_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_studhed"
                                                    PopupControlID="pnl_studhed" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Ledger"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_studled" runat="server" Style="height: 20px; width: 100px;"
                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="pnl_studled" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                    Style="width: 300px; height: auto;">
                                                    <asp:CheckBox ID="chk_studledg" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="chk_studled_OnCheckedChanged" />
                                                    <asp:CheckBoxList ID="chkl_studled" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_studled_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_studled"
                                                    PopupControlID="pnl_studled" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_fromdate" runat="server" Text="From"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_fromdate" runat="server" Style="height: 20px; width: 75px;"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_todate" runat="server" Text="To"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_todate" runat="server" Style="height: 20px; width: 75px;"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                                           <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                                <ContentTemplate>
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="btngoUpdatePanel">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                            Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
                                        <asp:Button ID="btn_search" runat="server" CssClass="textbox btn2" Text="Search"
                                            OnClick="btn_search_click" />
                                            </ContentTemplate>
                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                </div>
            </center>
        </div>
        <center>
            <%--   <div id="div1" runat="server" visible="True" style="width: 800px; height: 350px;
                overflow: auto; border: 1px solid Gray; background-color: White;">--%>
            <br />
      
             <asp:GridView ID="gridview1" runat="server" ShowFooter="false"
                        AutoGenerateColumns="true" Font-Names="Book Antiqua" ShowHeader="false" toGenerateColumns="false">
                        <%-- --%>
                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
            </asp:GridView>

            <%-- </div>--%>
        </center>
        <div>
            <center>
                <div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                        ForeColor="Red" Text="" Visible="false"></asp:Label>
                    <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                        Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display()"
                        CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                        InvalidChars="/\">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnExcel" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                        OnClick="btnExcel_Click" Text="Export To Excel" Width="127px" Height="32px" CssClass="textbox textbox1" />
                    <asp:Button ID="btnprintmasterhed" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                        Text="Print" OnClick="btnprintmaster_Click" Height="32px" Style="margin-top: 10px;"
                        CssClass="textbox textbox1" Width="60px" />
                    <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
            </center>
        </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExcel" />
            <asp:PostBackTrigger ControlID="btnprintmasterhed" />
            
        </Triggers>
    </asp:UpdatePanel>
    </body>
    </html>
</asp:Content>
