<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="Staff_LoanDetailsReport.aspx.cs" Inherits="Staff_LoanDetailsReport"
    EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblsmserror.ClientID %>').innerHTML = "";
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <span class="fontstyleheader" style="color: Green;">Staff Loan Details Report</span>
            <br />
            <br />
            <table class="maintablestyle">
                <tr>
                    <td>
                        <asp:Label ID="lbl_college" runat="server" Text="College Name" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="120px"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcollege" runat="server" OnSelectedIndexChanged="ddlcollege_change"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="200px" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbl_dept" runat="server" Text="Department" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium;"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox txtheight1" ReadOnly="true"
                                    Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                <asp:Panel ID="p1" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                    border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                    box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;
                                    width: 250px;">
                                    <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" OnCheckedChanged="cb_dept_CheckedChange"
                                        AutoPostBack="true" />
                                    <asp:CheckBoxList ID="cbl_dept" runat="server" OnSelectedIndexChanged="cbl_dept_SelectedIndexChange"
                                        AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_dept"
                                    PopupControlID="p1" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lbl_desig" runat="server" Text="Designation" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium;"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_desig" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                    Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                <asp:Panel ID="P2" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                    border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                    box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;
                                    width: 250px;">
                                    <asp:CheckBox ID="cb_desig" runat="server" Text="Select All" OnCheckedChanged="cb_desig_CheckedChange"
                                        AutoPostBack="true" />
                                    <asp:CheckBoxList ID="cbl_desig" runat="server" OnSelectedIndexChanged="cbl_desig_SelectedIndexChange"
                                        AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_desig"
                                    PopupControlID="P2" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lbl_staffc" runat="server" Text="Category" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium;"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_staffc" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                    Style="font-weight: bold; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                <asp:Panel ID="P3" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                    border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                    box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                    <asp:CheckBox ID="cb_staffc" runat="server" Text="Select All" OnCheckedChanged="cb_staffc_CheckedChange"
                                        AutoPostBack="true" />
                                    <asp:CheckBoxList ID="cbl_staffc" runat="server" OnSelectedIndexChanged="cbl_staffc_SelectedIndexChange"
                                        AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_staffc"
                                    PopupControlID="P3" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_stype" runat="server" Text="Staff Type" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium;"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_stype" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                    Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                <asp:Panel ID="P4" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                    border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                    box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                    <asp:CheckBox ID="cb_stype" runat="server" Text="Select All" OnCheckedChanged="cb_stype_CheckedChange"
                                        AutoPostBack="true" />
                                    <asp:CheckBoxList ID="cbl_stype" runat="server" OnSelectedIndexChanged="cbl_stype_SelectedIndexChange"
                                        AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_stype"
                                    PopupControlID="P4" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblLoanType" runat="server" Text="Loan Type" Style="font-weight: bold;
                            margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="updLoanType" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtLoanType" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                    Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                <asp:Panel ID="pnlLoanType" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                    Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                    position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                    height: 200px;">
                                    <asp:CheckBox ID="cbLoanType" runat="server" Text="Select All" OnCheckedChanged="cbLoanType_CheckedChange"
                                        AutoPostBack="true" />
                                    <asp:CheckBoxList ID="cblLoanType" runat="server" OnSelectedIndexChanged="cblLoanType_SelectedIndexChange"
                                        AutoPostBack="true">
                                        <asp:ListItem Selected="True" Text="Loan" Value="0"></asp:ListItem>
                                        <asp:ListItem Selected="True" Text="Policy" Value="1"></asp:ListItem>
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="popLoanType" runat="server" TargetControlID="txtLoanType"
                                    PopupControlID="pnlLoanType" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lbl_scode" runat="server" Text="Staff Code" Style="font-weight: bold;
                            margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_scode" runat="server" OnTextChanged="txt_scode_Change" AutoPostBack="true"
                            MaxLength="10" Style="font-weight: bold; width: 123px; font-family: book antiqua;
                            font-size: medium;"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_scode"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="txtsearchpan">
                        </asp:AutoCompleteExtender>
                    </td>
                    <td>
                        <asp:Label ID="lbl_sname" runat="server" Text="Name" Style="font-weight: bold; margin-left: 0px;
                            font-family: book antiqua; font-size: medium;"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_sname" runat="server" OnTextChanged="txt_sname_Change" AutoPostBack="true"
                            MaxLength="50" Style="font-weight: bold; width: 123px; font-family: book antiqua;
                            margin-left: 0px; font-size: medium;"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_sname"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="txtsearchpan">
                        </asp:AutoCompleteExtender>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblMonYear" runat="server" Text="From Mon & Year" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium;"></asp:Label>
                        <%--</td>
                    <td>--%>
                        <asp:DropDownList ID="ddlMon" runat="server" Style="font-weight: bold; font-family: book antiqua;
                            font-size: medium;" CssClass="textbox1 ddlheight" OnSelectedIndexChanged="ddlMon_Change"
                            AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlYear" runat="server" Style="font-weight: bold; font-family: book antiqua;
                            font-size: medium;" CssClass="textbox1 ddlheight" OnSelectedIndexChanged="ddlYear_Change"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td colspan="4">
                        <asp:Label ID="lblToMonYr" runat="server" Text="To Mon & Year" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium;"></asp:Label>
                        <%--</td>
                    <td>--%>
                        <asp:DropDownList ID="ddlToMon" runat="server" Style="font-weight: bold; font-family: book antiqua;
                            font-size: medium;" CssClass="textbox1 ddlheight" OnSelectedIndexChanged="ddlToMon_Change"
                            AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlToYear" runat="server" Style="font-weight: bold; font-family: book antiqua;
                            font-size: medium;" CssClass="textbox1 ddlheight">
                        </asp:DropDownList>
                        <%--</td>
                    <td>--%>
                        <asp:Button ID="btnGo" runat="server" Text="GO" CssClass="textbox1 btn2" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium;" OnClick="btnGo_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="lblMainErr" runat="server" Visible="false" Text="" ForeColor="Red"
                Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"></asp:Label>
            <br />
            <FarPoint:FpSpread ID="FpSpread" runat="server" Visible="false" BorderColor="Black"
                BorderStyle="Solid" BorderWidth="1px" Width="980px" Height="800px" Style="margin-left: 2px;"
                class="spreadborder" ShowHeaderSelection="false">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="LightGreen">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            <br />
            <br />
            <div id="rprint" runat="server" visible="false">
                <asp:Label ID="lblsmserror" Text="Please Enter Your Report Name" Font-Size="Large"
                    Font-Names="Book Antiqua" Visible="false" ForeColor="Red" runat="server" Font-Bold="true"></asp:Label>
                <asp:Label ID="lblexcel" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
                <asp:TextBox ID="txtexcel" onkeypress="display()" CssClass="textbox textbox1" runat="server"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcel"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btnexcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" CssClass="textbox textbox1 btn2" Width="140px" Text="Export Excel"
                    OnClick="btnexcel_Click" />
                <asp:Button ID="btnprintmaster" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" CssClass="textbox textbox1 btn2"
                    Width="100px" />
                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
            </div>
            <br />
        </center>
    </body>
    </html>
</asp:Content>
