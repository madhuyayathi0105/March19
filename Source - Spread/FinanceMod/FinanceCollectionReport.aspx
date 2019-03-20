<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="FinanceCollectionReport.aspx.cs" Inherits="FinanceCollectionReport" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title>Collection Report</title>
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <body>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
        <script type="text/javascript" language="javascript">
            function display() {
                document.getElementById('<%=lbl_validation.ClientID %>').innerHTML = "";
            }
            function checkDate() {

                var fromDate = "";
                var toDate = "";
                var date = ""
                var date1 = ""
                var month = "";
                var month1 = "";
                var year = "";
                var year1 = "";
                var empty = "";
                fromDate = document.getElementById('<%=txt_fromdate.ClientID%>').value;
                toDate = document.getElementById('<%=txt_todate.ClientID%>').value;

                date = fromDate.substring(0, 2);
                month = fromDate.substring(3, 5);
                year = fromDate.substring(6, 10);

                date1 = toDate.substring(0, 2);
                month1 = toDate.substring(3, 5);
                year1 = toDate.substring(6, 10);
                var today = new Date();
                var currentDate = today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();

                if (year == year1) {
                    if (month == month1) {
                        if (date == date1) {
                            empty = "";
                        }
                        else if (date < date1) {
                            empty = "";
                        }
                        else {
                            empty = "e";
                        }
                    }
                    else if (month < month1) {
                        empty = "";
                    }
                    else if (month > month1) {
                        empty = "e";
                    }
                }
                else if (year < year1) {
                    empty = "";
                }
                else if (year > year1) {
                    empty = "e";
                }
                if (empty != "") {
                    alert("To date should be greater than from date ");
                    return false;
                }
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Fee Collection Report</span></div>
            </center>
        </div>
        <div>
            <center>
                <div id="maindiv" runat="server" class="maindivstyle" style="width: 960px; height: auto">
                    <center>
                        <div>
                            <table class="maintablestyle">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_collegename" Text="College" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox ddlstyle ddlheight5"
                                            OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblmem" runat="server" Text="MemType"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtmem" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="pnlmem" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 126px;
                                                    height: 120px;">
                                                    <asp:CheckBox ID="cbmem" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cbmem_OnCheckedChanged" />
                                                    <asp:CheckBoxList ID="cblmem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblmem_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txtmem"
                                                    PopupControlID="pnlmem" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        PaymentMode
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="upd_paid" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_paid" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="pnl_paid" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                    Style="width: 126px; height: 160px;">
                                                    <asp:CheckBox ID="chk_paid" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="chk_paid_OnCheckedChanged" />
                                                    <asp:CheckBoxList ID="chkl_paid" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_paid_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_paid"
                                                    PopupControlID="pnl_paid" Position="Bottom">
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
                                        <asp:Label ID="lbl_todate" runat="server" Text="To" Style="margin-left: 4px;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_todate" runat="server" Style="height: 20px; width: 75px;"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Header" Style="width: 50px;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_studhed" runat="server" Style="height: 20px; width: 100px;"
                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="pnl_studhed" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                    Style="width: 300px; height: 180px;">
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
                                                    Style="width: 300px; height: 180px;">
                                                    <asp:CheckBox ID="chk_studled" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
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
                                    <td colspan="5">
                                        <asp:RadioButtonList ID="rbledgmode" runat="server" RepeatDirection="Horizontal"
                                            AutoPostBack="true" OnSelectedIndexChanged="rbledgmode_Selected">
                                            <asp:ListItem Text="LedgerWise Cumulative" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="LedgerWise Detailed" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="lblnum" runat="server" Text="Roll No"></asp:Label>
                                        <asp:DropDownList ID="ddladmit" runat="server" AutoPostBack="True" CssClass="textbox1 ddlheight1"
                                            OnSelectedIndexChanged="ddladmit_SelectedIndexChanged">
                                            <%-- <asp:ListItem>Roll No</asp:ListItem>
                                            <asp:ListItem>Reg No</asp:ListItem>
                                            <asp:ListItem>Adm No</asp:ListItem>--%>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtno" runat="server" CssClass="textbox textbox1" Width="250px"
                                            OnTextChanged="txtno_TextChanged" AutoPostBack="True"></asp:TextBox>
                                        <asp:Label ID="lblappno" runat="server" Visible="false"></asp:Label>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderroll" runat="server" TargetControlID="txtno"
                                            FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .-/$">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:AutoCompleteExtender ID="autocomplete_rollno" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtno"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <%-- <asp:Button ID="btn_roll" runat="server" CssClass="textbox btn1 textbox1" Text="?"
                                            OnClick="btn_roll_Click" />      --%>
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
                                            OnClientClick="return checkDate()" OnClick="btnsearch_Click" />
                                             </ContentTemplate>
                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <%--<div style="width: 950px; overflow: auto; height: 400px;">--%>
        
             <asp:GridView ID="gridview1" runat="server" ShowFooter="false"
                        AutoGenerateColumns="true" Font-Names="Book Antiqua" ShowHeader="false" toGenerateColumns="false">
                        <%-- --%>
                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
            </asp:GridView>
                            <%-- </div>--%>
                            <%--ledgerwise detailed report--%>
                            <center>
                                <div id="divlegdet" runat="server" visible="false">
                                    <asp:Label ID="lbldlydetail" runat="server" Text="" Visible="false" Font-Size="Medium"
                                        Font-Bold="True" Font-Names="Book Antiqua" ForeColor="Blue" />
                                    <br />
                                    <center>
                                        <div>
                                            <center>
                                                <asp:Panel ID="pnlhead" runat="server" CssClass="cpHeader" Visible="true" Height="22px"
                                                    Width="146px" BackColor="#0CA6CA" Style="margin-top: -0.1%; margin-left: -796px;">
                                                    <asp:Label ID="lblhead" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                                </asp:Panel>
                                            </center>
                                        </div>
                                        <br />
                                        <div>
                                            <asp:Panel ID="pnlcolhed" runat="server" Visible="true" CssClass="maintablestyle"
                                                Width="930px">
                                                <div id="div2" runat="server" style="height: 100px; width: 930px;">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="cblegdet" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="cbllegdet_CheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBoxList ID="cbllegdet" runat="server" Height="43px" Width="850px" Style="font-family: 'Book Antiqua';
                                                                    font-weight: 700; font-size: medium;" RepeatColumns="5" RepeatDirection="Horizontal"
                                                                    OnSelectedIndexChanged="cbllegdet_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </center>
                                    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pnlcolhed"
                                        CollapseControlID="pnlhead" ExpandControlID="pnlhead" Collapsed="true" TextLabelID="Labelfilter"
                                        CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="~/images/right.jpeg"
                                        ExpandedImage="~/images/down.jpeg">
                                    </asp:CollapsiblePanelExtender>
                                    <br />
                                    <asp:Panel ID="Panel3" runat="server">
                                
                                         <asp:GridView ID="gridview2" runat="server" ShowFooter="false"
                        AutoGenerateColumns="true" Font-Names="Book Antiqua" ShowHeader="false" toGenerateColumns="false">
                        <%-- --%>
                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
            </asp:GridView>
                                    </asp:Panel>
                                </div>
                            </center>
                            <%--print--%>
                            <div id="rptprint" runat="server" visible="false">
                                <asp:Label ID="lbl_validation" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                    Visible="false"></asp:Label><br />
                                <asp:Label ID="lbl_rptname" runat="server" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txt_excelname" runat="server" Width="180px" onkeypress="display()"
                                    CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                                <asp:Button ID="btn_excel" runat="server" OnClick="btn_excel_Click" Text="Export To Excel"
                                    Width="127px" CssClass="textbox btn2 textbox1" />
                                <asp:Button ID="btn_printmaster" runat="server" Text="Print" OnClick="btn_printmaster_Click"
                                    CssClass="textbox btn2 textbox1" Width="60px" />
                                <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                            </div>
                        </div>
                    </center>
                </div>
            </center>
        </div>
        <%-- Pop Alert--%>
        <center>
            <div id="imgAlert" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_alertclose" CssClass=" textbox textbox1 btn1" Style="height: 28px;
                                                width: 65px;" OnClick="btn_alertclose_Click" Text="ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_excel" />
            <asp:PostBackTrigger ControlID="btn_printmaster" />
            
        </Triggers>
    </asp:UpdatePanel>
    </body>
    </html>
</asp:Content>
