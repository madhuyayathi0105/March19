<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true"
    CodeFile="Exam fee status.aspx.cs" Inherits="Exam_fee_status" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Printcontrol" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .fontbold
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
        
        .fontnormal
        {
            font-family: Book Antiqua;
            font-size: medium;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <asp:Label ID="Label4" runat="server" Text="Exam Fee Status" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Large" ForeColor="Green"></asp:Label></center>
        <br />
        <center>
            <table style="width: 700px; height: 70px; background-color: #0CA6CA;">
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
                            Font-Size="Medium" Width="80px"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlYear" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Width="90px" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblexammonth" runat="server" Text="Exam Month" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="100px"></asp:Label>
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
                <tr>
                    <td>
                        <asp:Label ID="Lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="120px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="LblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="120Px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RadioButton ID="rbentry" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Entry" GroupName="Feespaid" AutoPostBack="true" OnCheckedChanged="Radiochange"
                            Width="90px" />
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
            <br />
            <asp:Label ID="lblerror" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                Font-Bold="true" ForeColor="Red"></asp:Label>
            <br />
            <FarPoint:FpSpread ID="FpSpread1" runat="server" Height="250px" Width="400px" ActiveSheetViewIndex="0"
                currentPageIndex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                EnableClientScript="False" CssClass="cursorptr" BorderColor="Black" BorderWidth="0.5"
                OnUpdateCommand="FpSpread1_UpdateCommand" ShowHeaderSelection="false">
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
                    <asp:CheckBox ID="chkExamApp" runat="server" Text="With Exam Application"  Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" />
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
        </center>
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
</asp:Content>
