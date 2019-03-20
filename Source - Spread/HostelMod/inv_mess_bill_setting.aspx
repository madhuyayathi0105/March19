<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="inv_mess_bill_setting.aspx.cs" Inherits="inv_mess_bill_setting" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title></title>
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            .div
            {
                left: 0%;
                top: 0%;
            }
        </style>
    </head>
    <body>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <center>
                    <br />
                    <div>
                        <asp:Label ID="lbl_messbillsetting" Style="color: Green;" runat="server" class="fontstyleheader"
                            Text="Mess Bill Setting"></asp:Label>
                        <br />
                        <br />
                    </div>
                </center>
                <center>
                    <div class="maindivstyle" style="height: 520px; width: 1000px;">
                        <br />
                        <table class="maintablestyle" style="width: 1003px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_fromyear" runat="server" Text="From Year"></asp:Label>
                                    <asp:DropDownList ID="ddl_fromyr" runat="server" CssClass="textbox1  ddlheight1">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_toyear" runat="server" Text="To Year"></asp:Label>
                                    <asp:DropDownList ID="ddl_toyr" runat="server" CssClass="textbox1  ddlheight1">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lb_datesetting" runat="server" Visible="true" Font-Bold="false"
                                        Font-Size="Medium" ForeColor="Black" CausesValidation="False" OnClick="lb_datesetting_Click">Date Setting</asp:LinkButton>
                                </td>
                              <%--  magesh 12.3.18--%>
                                <td>
                                    <asp:LinkButton ID="lb_billsetting" runat="server" Visible="true" Font-Bold="false"
                                        Font-Size="Medium" ForeColor="Black" CausesValidation="False" OnClick="lb_billsetting_Click">Mess Bill amount</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_fix" Visible="false" Text="Fixed" runat="server" AutoPostBack="true"
                                        GroupName="messfix" OnCheckedChanged="rdb_fix_CheckedChange" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_div" Visible="false" Text="Dividend" runat="server" AutoPostBack="true"
                                        GroupName="fix"  />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_nondiv" Visible="false" Text="Non-Dividend" runat="server" AutoPostBack="true"
                                        GroupName="fix"  />
                                </td>
<%--magesh 12.3.18--%>
                                <td>
                                    <asp:RadioButton ID="rdb_common1" Visible="false" Text="Common" runat="server" AutoPostBack="true"
                                        GroupName="co1" OnCheckedChanged="rdb_common1_CheckedChange" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_indivual1" Text="Individual" runat="server" AutoPostBack="true"
                                        GroupName="co1" OnCheckedChanged="rdb_rdb_indivual1_CheckedChange" />
                                </td>
                                <td>
                                    <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                                </td>
                                <%-- <td>
                        <asp:Button ID="btn_addnew" runat="server" Text="Add New"
                            CssClass="textbox btn2" OnClick="btn_addnew__Click"   />
                    </td>--%>
                            </tr>
                        </table>
                        <br />
                        <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                        <div id="div1" runat="server" visible="false" style="width: 676px; height: 380px;
                            overflow: auto; border: 1px solid Gray; border-radius: 10px; background-color: White;
                            box-shadow: 0px 0px 8px #999999;">
                            <br />
                            <FarPoint:FpSpread ID="Fpspread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="1px" Width="470px" Style="height: 354px; overflow: auto; background-color: White;"
                                OnButtonCommand="btnType_Click">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                        <br />
                    </div>
                </center>
                <center>
                    <div id="poperrjs" runat="server" visible="false" style="height: 50em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 100px;
                        left: 0;">
                        <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 28px; margin-left: 217px;"
                            OnClick="imagebtnpopclose_dateset_Click" />
                        <br />
                        <br />
                        <div class="subdivstyle" style="background-color: White; overflow: auto; width: 450px;
                            height: 250px;" align="center">
                            <br />
                            <center>
                                <asp:Label ID="lbl_datesetting" runat="server" Style="font-size: large; color: Green;"
                                    Text="Date Setting"></asp:Label>
                            </center>
                            <br />
                            <div align="left" style="overflow: auto; width: 350px; height: 150px;" class="spreadborder">
                                <br />
                                <center>
                                    <table>
                                        <%--style="line-height: 33px;"--%>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_fromdate" runat="server" Text="From Date"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox  txtheight"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_fromdate" runat="server"
                                                    Format="dd/MM/yyyy">
                                                    <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_todate" runat="server" Text="To Date"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox  txtheight"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_todate" runat="server"
                                                    Format="dd/MM/yyyy">
                                                    <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:RadioButton ID="rdb_frommonth" Text="By From Month" runat="server" AutoPostBack="true"
                                                    GroupName="m" />
                                            </td>
                                            <td colspan="2">
                                                <asp:RadioButton ID="rdb_tomonth" Text="By To Month" runat="server" GroupName="m"
                                                    AutoPostBack="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                                <br />
                                <center>
                                    <asp:Button ID="btn_save_dateset" runat="server" CssClass="textbox btn2" Text="Save"
                                        OnClick="btn_save_dateset_Click" />
                                    <asp:Button ID="btn_exit_dateset" runat="server" CssClass="textbox btn2" Text="Exit"
                                        OnClick="btn_exit_dateset_Click" />
                                </center>
                            </div>
                        </div>
                    </div>
                </center>

              <%--  magesh 12.3.18--%>
                <center>
                    <div id="Divdivding" runat="server" visible="false" style="height: 90em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 100px;
                        left: 0;">
                        <asp:ImageButton ID="Imagbutn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 28px; margin-left: 411px;"
                            OnClick="Imagbutn_dateset_Click" />
                        <br />
                        <br />
                        <div class="subdivstyle" style="background-color: White; overflow: auto; width: 871px;
                            height: 750px;" align="center">
                            <br />
                            <center>
                                <asp:Label ID="lbldiv" runat="server" Style="font-size: large; color: Green;"
                                    Text="Non-Divinding Setting"></asp:Label>
                            </center>
                            <br />
                            <div align="left" style="overflow: auto; width: 839px; height: 650px;" class="spreadborder">
                                <br />
                                <center>
                                    <table>
                                        <%--style="line-height: 33px;"--%>
                                        <tr>
                                                 <td>
                                    <asp:Label ID="lbl_hostel" Text="Hostel Name" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_hostelname1" runat="server" CssClass="textbox txtheight3 textbox1"
                                                ReadOnly="true" >--Select--</asp:TextBox>
                                            <asp:Panel ID="p1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Style="width: 160px; height: 200px;">
                                                <asp:CheckBox ID="cb_hostelname1" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_hostelname1_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_hostelname1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_hostelname1_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_hostelname1"
                                                PopupControlID="p1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                            <td>
                                                <asp:Label ID="lblMonth" runat="server" Text="Month"></asp:Label>
                                            </td>
                                            <td>
                                             <asp:DropDownList ID="ddlmonth" runat="server" CssClass="textbox1  ddlheight1">
                                    </asp:DropDownList>
                                    </td>
                                
                                               
                                            <td>
                                                <asp:Label ID="lblYear" runat="server" Text="Year"></asp:Label>
                                          
                                            <asp:DropDownList ID="ddlyear" runat="server" CssClass="textbox1  ddlheight1">
                                    </asp:DropDownList>
                                            </td>
                                            <td>
                                            <td>
                                                <asp:Label ID="stutype" runat="server" Text="Mess Type"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlStudType" runat="server" CssClass="textbox  ddlheight3"
                                           >
                                        </asp:DropDownList>
                                    </td>       
                                        </tr>
                                      <tr></tr>
                                      <tr></tr>
                                       <tr></tr>
                                      <tr></tr>
                                       
                                        <tr>
                                        <td colspan='2'></td>
                                        <td>
                                                <asp:Label ID="Lblamount" runat="server" Text="Amount"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Txtamount" runat="server" CssClass="textbox  txtheight"></asp:TextBox>
                                               
                                            </td>
                                            <td> <asp:Button ID="Btnsave" runat="server" CssClass="textbox btn2" Text="Save"
                                        OnClick="Btnsave_Click" /></td>
                                        </tr>
                                    </table>
                                    <asp:Label ID="lbl_error1" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                                     <%--<div id="divFpspread" runat="server" visible="false" style="width: 676px; height: 380px;
                            overflow: auto; border: 1px solid Gray; border-radius: 10px; background-color: White;
                            box-shadow: 0px 0px 8px #999999;">--%>
                            <br />
                            <FarPoint:FpSpread ID="Fpspread2" class="subdivstyle" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="1px" Width="870px" Style="height: 457px; overflow: auto; background-color: White;"
                                >
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                       <%-- </div>--%>
                                </center>
                                <br />
                                <center>
                                   
                                    <asp:Button ID="btn_exit_dateset1" runat="server" CssClass="textbox btn2" Text="Exit"
                                        OnClick="btn_exit_dateset1_Click" />
                                </center>
                            </div>
                        </div>
                    </div>
                </center>
             <%--   magesh 12.3.18--%>
                <center>
                    <div id="Div2" runat="server" visible="false" style="height: 39em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 100px;
                        left: 0;">
                        <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 28px; margin-left: 344px;"
                            OnClick="imagebtnpopclose_addnew_Click" />
                        <br />
                        <br />
                        <div class="subdivstyle" style="background-color: White; overflow: auto; width: 700px;
                            height: 443px;" align="center">
                            <br />
                            <center>
                                <asp:Label ID="lbl_messbillset" runat="server" Style="font-size: large; color: Green;"
                                    Text="Mess Bill Setting"></asp:Label>
                            </center>
                            <br />
                            <div align="left" style="overflow: auto; width: 641px; height: 362px; border-radius: 10px;
                                border: 1px solid Gray;" class="spreadborder">
                                <br />
                                <center>
                                    <div>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_clgn" Visible="true" runat="server" Text="College Name:" ForeColor="#006666"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_clg" Visible="true" runat="server" Font-Bold="true" ForeColor="#0ca6ca"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lbl_hosn" Visible="true" runat="server" Text="Mess Name:" ForeColor="#006666"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_hosname" Visible="true" runat="server" Font-Bold="true" ForeColor="#0ca6ca"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <br />
                                    <table style="line-height: 22px;" cellspacing="4px">
                                        <tr style="display: none;">
                                            <td>
                                                <asp:Label ID="lbl_clgname" runat="server" Text="College Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_clgname" runat="server" CssClass="textbox  txtheight3" ReadOnly="true">--Select--</asp:TextBox>
                                                        <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                                            width: 200px;">
                                                            <asp:CheckBox ID="cb_clgname" runat="server" Text="Select All" AutoPostBack="True"
                                                                OnCheckedChanged="cb_clgname_CheckedChanged" />
                                                            <asp:CheckBoxList ID="cbl_clgname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_clgname_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_clgname"
                                                            PopupControlID="Panel1" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <%--<asp:DropDownList ID="ddl_college" runat="server" CssClass="ddlheight3 textbox"></asp:DropDownList>--%>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_hostelname" runat="server" Text="Mess Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_hostelname" runat="server" CssClass="textbox  txtheight3" ReadOnly="true">--Select--</asp:TextBox>
                                                        <asp:Panel ID="Panel6" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                                            width: 160px;">
                                                            <asp:CheckBox ID="cb_hostelname" runat="server" Text="Select All" AutoPostBack="True"
                                                                OnCheckedChanged="cb_hostelname_CheckedChanged" />
                                                            <asp:CheckBoxList ID="cbl_hostelname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_hostelname_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_hostelname"
                                                            PopupControlID="Panel6" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <%--                  <asp:DropDownList ID="ddl_hostelname" runat="server" CssClass="ddlheight3 textbox"></asp:DropDownList>
                                                --%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_mess" runat="server" Text="Mess Type"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdb_commonmess" Text="Common" runat="server" AutoPostBack="true"
                                                    GroupName="co1" OnCheckedChanged="rdb_commonmess_CheckedChange" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdb_indmess" Visible="false" Text="Individual" runat="server"
                                                    AutoPostBack="true" GroupName="co1" OnCheckedChanged="rdb_indmess_CheckedChange" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cb_instaff" runat="server" Text="Include Staff" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cb_dayssch" runat="server" Text="Days Scholour" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cb_hosteler" runat="server" Text="Hosteler" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cbHostlerStaff" runat="server" Text="Hosteler Staff" />
                                            </td>
                                            <td>
                                                <%--  <asp:RadioButton ID="rdb_guest" Text="Guest" runat="server" AutoPostBack="true" />--%>
                                                <asp:CheckBox ID="cb_guest" runat="server" Text="Guest" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_rebate" runat="server" Text="Rebate"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdb_common" Text="Common" runat="server" GroupName="co" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdb_indivula" Text="Individual" runat="server" GroupName="co" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="cb_stuadd" Visible="false" runat="server" AutoPostBack="true" Text="Include Student Additional" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cb_adj_exe" Visible="false" runat="server" AutoPostBack="true"
                                                    Text="Adjust In Excise" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:CheckBox ID="cb_addinco" Visible="false" runat="server" AutoPostBack="true"
                                                    OnCheckedChanged="cb_addinco_CheckedChange" Text="Additional Income" />
                                                <asp:CheckBox ID="cb_addex" Visible="false" runat="server" AutoPostBack="true" Text="Additional Expenses"
                                                    OnCheckedChanged="cb_addex_CheckedChange" />
                                                <asp:Label ID="lbl_year" runat="server" Font-Bold="true" ForeColor="#0ca6ca"></asp:Label>
                                                <asp:Label ID="lbl_mon" runat="server" Font-Bold="true" ForeColor="#0ca6ca"></asp:Label>
                                                <asp:Label ID="lbl_messfee" runat="server" Text="Mess Fee"></asp:Label>
                                                <asp:TextBox ID="txt_messfess" runat="server" placeholder="Mess Fee" CssClass="textbox textbox1"
                                                    MaxLength="10"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="filtertextbox" runat="server" TargetControlID="txt_messfess"
                                                    FilterType="numbers,custom" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:RadioButton ID="rdo_day" runat="server" Text="Day" GroupName="q" />
                                                <asp:RadioButton ID="rdo_month" runat="server" Text="Month" GroupName="q" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_groupname" runat="server" Text="Group Name-Income" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="updatepanel_groupname" runat="server" Visible="false">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_groupname" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                                        <asp:Panel ID="panel_groupname" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                                            width: 129px;">
                                                            <asp:CheckBox ID="cb_groupname" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                OnCheckedChanged="cb_groupname_CheckedChange" />
                                                            <asp:CheckBoxList ID="cbl_groupname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_groupname_SelectedIndexChange">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_groupname"
                                                            PopupControlID="panel_groupname" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:Label ID="label_groupnameex" runat="server" Text="Group Name-Expanses" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="updatepanel2" runat="server" Visible="false">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_groupnameex" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                                        <asp:Panel ID="panel2" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                                            width: 160px">
                                                            <asp:CheckBox ID="cb_groupnameex" runat="server" Width="100px" Text="Select All"
                                                                AutoPostBack="True" OnCheckedChanged="cb_groupnameex_CheckedChange" />
                                                            <asp:CheckBoxList ID="cbl_groupnameex" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_groupnameex_SelectedIndexChange">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_groupnameex"
                                                            PopupControlID="panel2" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                                <br />
                                <center>
                                    <asp:Button ID="btn_generate" runat="server" CssClass="textbox btn2" Text="Generate"
                                        OnClick="btn_generate_Click" />
                                    <asp:Button ID="btn_addnew_exit" runat="server" CssClass="textbox btn2" Text="Exit"
                                        OnClick="btn_addnew_exit_Click" />
                                </center>
                            </div>
                        </div>
                    </div>
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
                                                <center>
                                                    <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" OnClick="btnerrclose_Click"
                                                        Text="Ok" runat="server" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        </form>
    </body>
    </html>
</asp:Content>
