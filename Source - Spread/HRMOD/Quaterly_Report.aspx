<%@ Page Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="Quaterly_Report.aspx.cs" Inherits="HRMOD_Quaterly_Report" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
        </script>
        <div>
            <br />
            <center>
                <div>
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: Green;">Quarterly Report</span></div>
                    </center>
                    <div class="maindivstyle" style="width: 1000px; height: auto;">
                        <br />
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_college" runat="server" Text="College Name" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="120px"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="updatecollege" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlcollege" runat="server" OnSelectedIndexChanged="ddlcollege_OnSelectedIndexChanged"
                                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="285px" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
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
                                                box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
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
                                <td colspan="2">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_desig" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="P2" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                                border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                                box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
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
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_staffc" runat="server" Text="Staff Category" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_staffc" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                Style="font-weight: bold; width: 90px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
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
                                <td>
                                    <asp:Label ID="lblfyear" runat="server" Text="From Year" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;"></asp:Label>
                                    <asp:DropDownList ID="ddl_fromyear" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Width="65px" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblfmonth" runat="server" Text="From Month" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_frommonth" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Width="80px" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbltyear" runat="server" Text="To Year" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;"></asp:Label>
                                    <asp:DropDownList ID="ddl_toyear" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Width="65px" AutoPostBack="True" OnSelectedIndexChanged="Year_click">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbltmonth" runat="server" Text="To Month" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_tomonth" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Width="80px" AutoPostBack="True" OnSelectedIndexChanged="Month_click">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <%-- <td>
                                    <asp:Label ID="lbl_sname" runat="server" Text="Staff Name" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;"></asp:Label>
                                        
                                </td>--%>
                                <td colspan="2">
                                    <%--
                                 <asp:TextBox ID="txt_sname" runat="server" MaxLength="50" Style="font-weight: bold;
                                        font-family: book antiqua; margin-left: 0px; font-size: medium;"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_sname"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="txtsearchpan">
                                    </asp:AutoCompleteExtender>--%>
                                    <%-- <asp:CheckBox ID="cb_deduction" runat="server" Text="Deduction Date" AutoPostBack="true" OnCheckedChanged="cb_deduction_click" />--%>
                                    <%--<asp:TextBox ID="txt_deduction" runat="server" MaxLength="30" Style="width: 70px;
                                        font-family: book antiqua; margin-left: 0px;"></asp:TextBox>
                                    <asp:CalendarExtender ID="caldeductiondate" TargetControlID="txt_deduction" runat="server"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>--%>
                                    <asp:LinkButton ID="linkbtn_Date" runat="server" OnClick="linkbtn_Click">Deduction Date Setting</asp:LinkButton><br />
                                </td>
                                <td colspan="2">
                                    <asp:LinkButton ID="lnkDeposit" runat="server" OnClick="lnkDepositClick">Deposit Date Setting</asp:LinkButton><br />
                                    <%-- <asp:CheckBox ID="cb_deposit" runat="server" Text="Deposit Date" AutoPostBack="False" />
                                    <asp:TextBox ID="txt_deposit" runat="server" MaxLength="30" Style="width: 70px; font-family: book antiqua;
                                        margin-left: 0px;"></asp:TextBox>
                                    <asp:CalendarExtender ID="caldepositedate" TargetControlID="txt_deposit" runat="server"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:CheckBox ID="cb_challanno" runat="server" Text="Challan No" AutoPostBack="False" />--%>
                                </td>
                             <%--   <td>
                                    <asp:TextBox ID="txt_challan" runat="server" MaxLength="30" Style="width: 70px; font-family: book antiqua;
                                        margin-left: 0px;"></asp:TextBox>
                                </td>--%>
                                <td>
                                    <asp:Button ID="btn_go" runat="server" Text="Go" OnClick="btn_go_Click" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;" CssClass="textbox btn1" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <center>
                            <%--delsi--%>
                            <div id="popupwindow" runat="server" visible="false" style="height: 48em; z-index: 1000;
                                width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                                left: 0;">
                                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                    Style="height: 30px; width: 30px; position: absolute; margin-top: 13px; margin-left: 180px;"
                                    OnClick="imagebtnpopclose4_Click" />
                                <br />
                                <div style="background-color: White; height: 500px; width: 418px; border: 5px solid #0CA6CA;
                                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                                    <br />
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="Fromyear" runat="server" Width="50px" CssClass="textbox1 ddlheight1">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="frommonth" runat="server" Width="100px" CssClass="textbox1 ddlheight1">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="toyear" runat="server" Width="50px" CssClass="textbox1 ddlheight1">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="tomonth" runat="server" Width="100px" CssClass="textbox1 ddlheight1">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btnsavedate" runat="server" Text="Go" OnClick="btnsavedateclick_Click"
                                                        CssClass="textbox btn1" />
                                                </center>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5" align="center">
                                                <div style="overflow: auto; height: 400px;">
                                                    <center>
                                                        <asp:GridView ID="griddate" runat="server" AutoGenerateColumns="false" Width="250px"
                                                            CssClass="spreadborder" Style="overflow: auto; font-size: small; "
                                                            HeaderStyle-BackColor="#0CA6CA" HeaderStyle-ForeColor="White" Visible="true">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-Height="30">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Month Year">
                                                                    <ItemTemplate>
                                                                        <asp:Textbox ID="lbl_monthyear" runat="server" Text='<%# Eval("MonthYear") %>'></asp:Textbox>
                                                                        <asp:Label ID="lbl_monthyear1" runat="server" Visible="false" Text='<%# Eval("MonthYearDb") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Date">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="dddate" runat="server">
                                                                            <asp:ListItem Value="1">1</asp:ListItem>
                                                                            <asp:ListItem Value="2">2</asp:ListItem>
                                                                            <asp:ListItem Value="3">3</asp:ListItem>
                                                                            <asp:ListItem Value="4">4</asp:ListItem>
                                                                            <asp:ListItem Value="5">5</asp:ListItem>
                                                                            <asp:ListItem Value="6">6</asp:ListItem>
                                                                            <asp:ListItem Value="7">7</asp:ListItem>
                                                                            <asp:ListItem Value="8">8</asp:ListItem>
                                                                            <asp:ListItem Value="9">9</asp:ListItem>
                                                                            <asp:ListItem Value="10">10</asp:ListItem>
                                                                            <asp:ListItem Value="11">11</asp:ListItem>
                                                                            <asp:ListItem Value="12">12</asp:ListItem>
                                                                            <asp:ListItem Value="13">13</asp:ListItem>
                                                                            <asp:ListItem Value="14">14</asp:ListItem>
                                                                            <asp:ListItem Value="15">15</asp:ListItem>
                                                                            <asp:ListItem Value="16">16</asp:ListItem>
                                                                            <asp:ListItem Value="17">17</asp:ListItem>
                                                                            <asp:ListItem Value="18">18</asp:ListItem>
                                                                            <asp:ListItem Value="19">19</asp:ListItem>
                                                                            <asp:ListItem Value="20">20</asp:ListItem>
                                                                            <asp:ListItem Value="21">21</asp:ListItem>
                                                                            <asp:ListItem Value="22">22</asp:ListItem>
                                                                            <asp:ListItem Value="23">23</asp:ListItem>
                                                                            <asp:ListItem Value="24">24</asp:ListItem>
                                                                            <asp:ListItem Value="25">25</asp:ListItem>
                                                                            <asp:ListItem Value="26">26</asp:ListItem>
                                                                            <asp:ListItem Value="27">27</asp:ListItem>
                                                                            <asp:ListItem Value="28">28</asp:ListItem>
                                                                            <asp:ListItem Value="29">29</asp:ListItem>
                                                                            <asp:ListItem Value="30">30</asp:ListItem>
                                                                            <asp:ListItem Value="31">31</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Challan No">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtChallanNo" runat="server" placeHolder="Challan No" Text='<%#Eval("ChallanNo") %>'></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                        </br>
                                                        <asp:Button ID="btnsave" runat="server" Text="Save" Visible="true" OnClick="btnsavedate_Click"
                                                            CssClass="textbox btn1" />
                                                    </center>
                                                </div>
                                            </td>
                                        </tr>
                                        <%-- <tr>
                                        <td colspan="4" align="center">
                                       
                                       
                                        </td>
                                        
                                        </tr>--%>
                                    </table>
                                </div>
                            </div>
                        </center>
                        <asp:Label ID="ermsg" Font-Bold="true" runat="server" ForeColor="Red"></asp:Label>
                        <center>
                            <FarPoint:FpSpread ID="Fpspread1" runat="server" ShowHeaderSelection="false" Visible="false"
                                BorderWidth="5px" BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0"
                                Style="margin-left: -5px" OnCellClick="FpSpread1_CellClick" OnPreRender="FpSpread1_SelectedIndexChanged"
                                OnButtonCommand="Fpspread1_ButtonCommand">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </center>
                        <center>
                            <div id="rptprint" runat="server" visible="false">
                                <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txtexcelname" CssClass="textbox textbox1" runat="server" Height="20px"
                                    Width="180px" onkeypress="display()"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,. ">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox btn1"
                                    Text="Export To Excel" Width="127px" />
                                <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                    CssClass="textbox btn1" />
                                <insproplus:printmaster runat="server" id="Printcontrol" visible="false" />
                            </div>
                        </center>
                        <br />
                    </div>
                </div>
            </center>
            <center>
                <div id="alertmessage" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                            <asp:Label ID="lbl_alerterror" Visible="false" runat="server" Text="" Style="color: Red;"
                                                Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                    width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
    </body>
    </html>
</asp:Content>
