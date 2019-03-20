<%@ Page Title="" Language="C#" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master" AutoEventWireup="true" CodeFile="Subject_Room_Allocation.aspx.cs" Inherits="Subject_Room_Allocation" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

 <title></title>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .maindivstylesize
        {
            height: 820px;
            width: 1000px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

 <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
       <center><br />
         <asp:Label ID="Label2" runat="server" Text="Individual Staff Report" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Large" ForeColor="Green"></asp:Label>
           </center>
            <br />
                <center>
                    <table style="width:1000px; height:70px; background-color:#0CA6CA;">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_staffcode" Text="Staff Code" runat="server" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_staffcode" runat="server" CssClass="textbox textbox1 ddlheight1"
                                    OnSelectedIndexChanged="ddl_staffcode_SelectedIndexChanged" AutoPostBack="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_staffname" Text="Staff Name" runat="server" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_staffname" runat="server" CssClass="textbox textbox1 ddlheight4"
                                    OnSelectedIndexChanged="ddl_staffname_SelectedIndexChanged" AutoPostBack="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_staffcodesearch" Text="Staff Code" runat="server" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_staffcodesearch" runat="server" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="True"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_staffcodesearch"
                                    FilterType="UppercaseLetters,LowercaseLetters,Numbers,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getcode" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffcodesearch"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="multxt1panel"
                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_staffnamesearch" Text="Staff Name" runat="server" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_staffnamesearch" runat="server" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="True"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_staffnamesearch"
                                    FilterType="UppercaseLetters,LowercaseLetters,Numbers,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffnamesearch"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="multxt1panel"
                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                </asp:AutoCompleteExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_fromdate" Text="From" runat="server" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_fromdate" runat="server" Width="80px" AutoPostBack="true" OnTextChanged="txt_fromdate_TextChanged"
                                    Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="True"></asp:TextBox>
                                <asp:CalendarExtender ID="caladmin" TargetControlID="txt_fromdate" runat="server"
                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_todate" Text="To" runat="server" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_todate" runat="server" Width="80px" AutoPostBack="true" OnTextChanged="txt_todate_TextChanged"
                                    Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="True"> Font-Bold="True"</asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_todate" runat="server"
                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Button ID="btn_go" runat="server" Text="Go" CssClass="textbox textbox1 btn1"
                                    OnClick="btn_go_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_staffname1" Visible="false" Text="Staff Name:" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbl_stafnme2" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </center>
                <br />
                <center>
                    <asp:Label ID="lbl_staf" runat="server" ForeColor="Red" Text="There is no class for the staff between the given date"
                        Visible="False" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label></center>
                <br />
                <center>
                    <div id="div1" runat="server" visible="false" style="width: 900px; height: 550px;
                        overflow: auto; border: 1px solid Gray; background-color: White;">
                        <br />
                        <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" BorderStyle="NotSet"
                            BorderWidth="0px">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <br />
                </center>
                <br />
                <br />
                <center>
                    <asp:Label ID="lbl_norec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False">
                    </asp:Label></center>
                <div id="div_report" runat="server" visible="false">
                    <center>
                        <asp:Label ID="lbl_reportname" runat="server" Text="Report Name" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="true" OnTextChanged="txtexcelname_TextChanged"
                            CssClass="textbox textbox1 txtheight5" onkeypress="display()"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btn_Excel" runat="server" Text="Export To Excel" Width="150px" CssClass="textbox btn2"
                            AutoPostBack="true" OnClick="btnExcel_Click" />
                        <asp:Button ID="btn_printmaster" runat="server" Text="Print" CssClass="textbox btn2"
                            AutoPostBack="true" OnClick="btn_printmaster_Click" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </center>
                </div>
            </div>
        </center>
    </div>
</asp:Content>

