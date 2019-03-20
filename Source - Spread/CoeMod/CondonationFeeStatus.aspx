<%@ Page Title="Condonation Fee Status" Language="C#" AutoEventWireup="true" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    CodeFile="CondonationFeeStatus.aspx.cs" Inherits="CondonationFeeStatus" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Printcontrol" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="position: relative; margin: 0px; margin-bottom: 25px;
        width: 100%; height: auto;">
        <center>
            <div style="margin-top: 10px; margin-bottom: 10px;">
                <span class="fontstyleheader" style="color: Green">Condonation Fee Status</span>
            </div>
        </center>
        <center>
            <div id="divSearch" runat="server" visible="true" style="color: black; font-family: Book Antiqua;
                height: auto; width: 100%; margin-bottom: 10px; padding-bottom: 10px;">
                <table class="maintablestyle" id="tblsearch" runat="server">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td colspan="8">
                                        <asp:CheckBox ID="chkSearchBy" runat="server" Checked="false" AutoPostBack="true"
                                            OnCheckedChanged="chkSearchBy_CheckedChange" Text="Search" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divNormal" runat="server" visible="true">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblcollege" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlcollege" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Font-Bold="true" Width="250px" AutoPostBack="True" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblexamyear" runat="server" Text="Exam Year" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlYear" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Font-Bold="true" Width="90px" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblexammonth" runat="server" Text="Exam Month" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlMonth" runat="server" Font-Bold="true" Width="90px" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Width="80px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddldegree" runat="server" Visible="false" Font-Bold="true"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Width="120px" AutoPostBack="true"
                                                CssClass="arrow" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:UpdatePanel ID="UpnlDegree" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtDegree" Width=" 100px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                                                    <asp:Panel ID="pnlDegree" runat="server" CssClass="multxtpanel" Height="200px">
                                                        <asp:CheckBox ID="chkDegree" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                            runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkDegree_CheckedChanged" />
                                                        <asp:CheckBoxList ID="cblDegree" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblDegree_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popupExtDegree" runat="server" TargetControlID="txtDegree"
                                                        PopupControlID="pnlDegree" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="LblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlbranch" Visible="false" runat="server" Font-Bold="true"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Width="120Px" AutoPostBack="true"
                                                CssClass="arrow" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:UpdatePanel ID="upnlBranch" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtBranch" Width=" 100px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                                                    <asp:Panel ID="pnlBranch" runat="server" CssClass="multxtpanel" Height="200px">
                                                        <asp:CheckBox ID="chkBranch" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                            runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                        <asp:CheckBoxList ID="cblBranch" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popupExtBranch" runat="server" TargetControlID="txtBranch"
                                                        PopupControlID="pnlBranch" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rbentry" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Entry" GroupName="Feespaid" AutoPostBack="true" OnCheckedChanged="Radiochange" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rbreport" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Report" GroupName="Feespaid" AutoPostBack="true" OnCheckedChanged="Radiochange"
                                                Width="100px" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblreport" runat="server" Text="Report Type" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Width="120px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlreporttype" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Width="120px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlreporttype_SelectedIndexChanged">
                                                <asp:ListItem Text="Both"></asp:ListItem>
                                                <asp:ListItem Text="Paid"></asp:ListItem>
                                                <asp:ListItem Text="UnPaid"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="Buttongo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnClick="Buttongo_Click" Text="Go" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divSearchBy" runat="server" visible="false">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSearchExamYear" runat="server" Text="Exam Year" Font-Bold="true"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSearchExamYear" runat="server" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Font-Bold="true" Width="90px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSearchExamMonth" runat="server" Text="Exam Month" Font-Bold="true"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSearchExamMonth" runat="server" Font-Bold="true" Width="90px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSearchBy" runat="server" Text="Search By" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSearchBy" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Font-Bold="true" Width="100">
                                                <asp:ListItem Selected="True" Value="0" Text="Roll No"></asp:ListItem>
                                                <asp:ListItem Selected="False" Value="1" Text="Reg No"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSearch" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Font-Bold="true" Text=""></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="rblSearchEntryOrReport" runat="server" Font-Bold="true"
                                                Font-Names="Book Antiqua" Font-Size="Medium" RepeatDirection="Horizontal" AutoPostBack="true"
                                                OnSelectedIndexChanged="rblSearchEntryOrReport_SelectedIndexChanged" Style="margin-right: 20px;">
                                                <asp:ListItem Selected="True" Text="Entry" Value="0"></asp:ListItem>
                                                <asp:ListItem Selected="False" Text="Report" Value="1"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblReportType" runat="server" Text="Report Type" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Width="120px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSearchReportType" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Width="120px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlSearchReportType_SelectedIndexChanged">
                                                <asp:ListItem Text="Both"></asp:ListItem>
                                                <asp:ListItem Text="Paid"></asp:ListItem>
                                                <asp:ListItem Text="UnPaid"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSearch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnClick="btnSearch_Click" Text="Search" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </center>
        <br />
        <br />
        <asp:Label ID="lblerror" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
            Font-Bold="true" ForeColor="Red"></asp:Label>
        <br />
        <FarPoint:FpSpread ID="FpSpread1" runat="server" Height="250px" Width="400px" ActiveSheetViewIndex="0"
            currentpageindex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
            EnableClientScript="False" CssClass="cursorptr" BorderColor="Black" BorderWidth="0.5"
            OnUpdateCommand="FpSpread1_UpdateCommand">
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
        <div id="NotFinanceDiv" runat="server" visible="false">
            <asp:Button ID="btnsave" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" OnClick="btnsave_Click" Text="Save" />
        </div>
        <div id="FinanceDiv" runat="server" visible="false">
            <asp:Button ID="btnChallan" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" OnClick="btnChallan_Click" Text="Generate Challan" />
            <asp:Button ID="btnChallanConf" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" OnClick="btnChallanConf_Click" Text="Challan Confirm" />
            <asp:Button ID="btnChallanDel" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" OnClick="btnChallanDel_Click" Text="Delete Challan" />
        </div>
        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Report Name"></asp:Label>
        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" onkeypress="display()"></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|}{][':;?><,."
            InvalidChars="/\">
        </asp:FilteredTextBoxExtender>
        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" OnClick="btnxl_Click" />
        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
        <Insproplus:Printcontrol runat="server" ID="Printcontrol" Visible="false" />
        <%--Delete Confirmation Popup --%>
        <center>
            <div id="surediv" runat="server" visible="false" style="height: 100em; z-index: 1000;
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
                                        <asp:Label ID="lbl_sure" runat="server" Text="Do You Want To Delete Selected Students' Challans?"
                                            Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_yes" CssClass=" textbox textbox1 btn1 " Style="height: 28px;
                                                width: 65px;" OnClick="btn_sureyes_Click" Text="Yes" runat="server" />
                                            <asp:Button ID="btn_no" CssClass=" textbox textbox1 btn1 " Style="height: 28px; width: 65px;"
                                                OnClick="btn_sureno_Click" Text="No" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="imgAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
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
                                                width: 65px;" OnClick="btn_alertclose_Click" Text="Ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="divCondonation" runat="server" visible="false">
                <FarPoint:FpSpread ID="FpCondonation" runat="server" AutoPostBack="false" Width="900px"
                    runat="server" Visible="true" BorderStyle="Solid" BorderWidth="0px" CssClass="spreadborder"
                    OnUpdateCommand="FpCondonation_UpdateCommand" ShowHeaderSelection="false" Style="width: 100%;
                    height: auto;">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <center>
                    <div id="rptprint1" runat="server" visible="false" style="margin: 20px;">
                        <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox textbox1" Height="20px"
                            Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                            onkeypress="display1()" Font-Size="Medium"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtexcelname1"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                            InvalidChars="/\">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnExcel1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            OnClick="btnExcel1_Click" Font-Size="Medium" Text="Export To Excel" Width="127px"
                            Height="35px" CssClass="textbox textbox1" />
                        <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="35px"
                            CssClass="textbox textbox1" />
                        <Insproplus:Printcontrol runat="server" ID="Printcontrol1" Visible="false" />
                        <asp:Button ID="btnSaveCondonationStatus" runat="server" Text="Save Condonation"
                            OnClick="btnSaveCondonationStatus_Click" Visible="false" Font-Names="Book Antiqua"
                            Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                        <%-- <asp:Button ID="btnPrintPhasing" runat="server" Text="Phasing Sheet"  OnClick="btnPrintPhasing_Click"
                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />--%>
                        <%--<asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />--%>
                    </div>
                </center>
            </div>
        </center>
    </div>
</asp:Content>
