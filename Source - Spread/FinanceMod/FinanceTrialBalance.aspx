<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="FinanceTrialBalance.aspx.cs" Inherits="FinanceTrialBalance" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=btnExcel.ClientID%>').click(function () {
                var excelName = $('#<%=txtexcelname.ClientID%>').val();
                if (excelName == null || excelName == "") {
                    $('#<%=lblvalidation1.ClientID%>').show();
                    $('#<%=print.ClientID%>').show();                    
                    return false;
                }
                else {
                    $('#<%=lblvalidation1.ClientID%>').hide();
                    $('#<%=print.ClientID%>').hide();
                }
            });
            $('#<%=txtexcelname.ClientID %>').keypress(function () {
                $('#<%=lblvalidation1.ClientID %>').hide();
            });
        });

    </script>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Trail Balance Sheet</span></div>
        </center>
    </div>
    <div>
        <center>
            <div id="maindiv" runat="server"  style="width: 1000px; height: auto">
                <center>
                    <div>
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_collegename" Text="College" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_collegename_OnSelectedIndexChanged">
                                    </asp:DropDownList>
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
                                <td id="tdfnl" runat="server" visible="false">
                                    <asp:Label runat="server" ID="lblfyear" Text="FinanceYear" Width="85px"></asp:Label>
                                </td>
                                <td colspan="2" id="tdfnls" runat="server" visible="false">
                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtfyear" Style="height: 20px; width: 174px;" CssClass="Dropdown_Txt_Box"
                                                runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                            <asp:Panel ID="Pfyear" runat="server" CssClass="multxtpanel" Width="178px">
                                                <asp:CheckBox ID="chkfyear" runat="server" Text="Select All" OnCheckedChanged="chkfyear_changed"
                                                    AutoPostBack="True" />
                                                <asp:CheckBoxList ID="chklsfyear" runat="server" OnSelectedIndexChanged="chklsfyear_selected"
                                                    AutoPostBack="True">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txtfyear"
                                                PopupControlID="Pfyear" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                        OnSelectedIndexChanged="rblType_Selected">
                                        <asp:ListItem Text="Header" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Ledger"></asp:ListItem>
                                    </asp:RadioButtonList>
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
                                    <asp:Button ID="btn_search" runat="server" CssClass="textbox btn2" Text="Search"
                                        OnClick="btn_search_click" />
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
        <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" BorderStyle="Solid"
            BorderWidth="0px" Style="overflow: auto; border: 0px solid #999999; border-radius: 10px;
            background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder">
            <%--OnCellClick="Cellcont_Click"OnPreRender="Fpspread1_render"--%>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
        <%-- </div>--%>
    </center>
    <div>
        <center>
            <div id="print" runat="server" Style="display: none;">
                <asp:Label ID="lblvalidation1" runat="server" Text="Please Enter Your Report Name"
                    Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Red" Style="display: none;"></asp:Label>
                <asp:Label ID="lblrptname" runat="server" Visible="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txtexcelname" runat="server" Visible="true" Width="180px" onkeypress="display()"
                    CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                    InvalidChars="/\">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btnExcel" runat="server" Visible="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btnExcel_Click" Text="Export To Excel" Width="127px"
                    Height="32px" CssClass="textbox textbox1" />
                <asp:Button ID="btnprintmasterhed" runat="server" Visible="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" Height="32px"
                    Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
            </div>
        </center>
    </div>
</asp:Content>
