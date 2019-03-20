<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="Subscription_Subscribe.aspx.cs" Inherits="LibraryMod_Subscription_Subscribe" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">
        function valid1() {
            var idval = "";
            var empty = "";
            var id = "";
            var value1 = "";
            id = document.getElementById("<%=txt_joucode.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_joucode.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=ddl_lib.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddl_lib.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }

            id = document.getElementById("<%=txt_subscode.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_subscode.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txt_title.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_title.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txt_supp_name.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_supp_name.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }


            id = document.getElementById("<%=fromdate.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=fromdate.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }

            id = document.getElementById("<%=todate.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=todate.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }

            if (empty.trim() != "") {
                return false;
            }
            else {

                return true;
            }
        }
    </script>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Subscribe</span></div>
        </center>
    </div>
    <div>
        <center>
            <div>
                <table>
                    <tr>
                        <td>
                            <center>
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <div style="width: 1000px; height: auto">
                                            <table class="maintablestyle" style="height: auto; width: auto; margin-left: -16px;
                                                margin-top: 10px; margin-bottom: 10px; padding: 6px; font-family: Book Antiqua;
                                                font-weight: bold">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblclg" runat="server" Text="College:">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UP_issue" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                    Width="155px" AutoPostBack="True" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbllibrary" runat="server" Text="Library:">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddllibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                    Width="155px" AutoPostBack="True" OnSelectedIndexChanged="ddllibrary_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_type" runat="server" Text="Type:">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddl_Type" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                    Width="155px" AutoPostBack="True" OnSelectedIndexChanged="ddl_Type_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_year" runat="server" Text="Sub.Year:">
                                                        </asp:Label>
                                                        <asp:CheckBox ID="Chk_year" runat="server" AutoPostBack="true" OnCheckedChanged="Chk_year_CheckedChanged" />
                                                        <asp:DropDownList ID="ddl_year" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                            Width="64px" AutoPostBack="True" OnSelectedIndexChanged="ddl_year_SelectedIndexChanged"
                                                            Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_reprt" runat="server" Text="Report Type:">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddl_reportype" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                    Width="155px" AutoPostBack="True" OnSelectedIndexChanged="ddl_reportype_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_delivery" runat="server" Text="Delivery Type:">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddl_delivery" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                    Width="155px" AutoPostBack="True" OnSelectedIndexChanged="ddl_delivery_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblsearch" runat="server" Text="SearchBy:">                                            
                                                 
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlsearch" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                    Width="155px" AutoPostBack="True" OnSelectedIndexChanged="ddlSearchby_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_bysearch" runat="server" CssClass="textbox txtheight2" Visible="false"
                                                                    Width="75px"></asp:TextBox>
                                                                <asp:DropDownList ID="ddldept" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                    Width="153px" AutoPostBack="True" OnSelectedIndexChanged="ddldep_SelectedIndexChanged"
                                                                    Visible="false">
                                                                </asp:DropDownList>
                                                                <asp:Label ID="Label_lang" runat="server" Text="lang:" Visible="false">  
                                                       
                                                                </asp:Label>
                                                                <asp:DropDownList ID="Cbo_TitleLanguage" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                    Width="75px" AutoPostBack="True" OnSelectedIndexChanged="ddl_lang_SelectedIndexChanged"
                                                                    Visible="false">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Sub Code:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_bysubscode" runat="server" Style="width: 148px;" CssClass="textbox txtheight2">
                                                        </asp:TextBox>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:RadioButtonList ID="rblActive" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                                            OnSelectedIndexChanged="rblActive_Selected">
                                                        </asp:RadioButtonList>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpGo" runat="server">
                                                            <ContentTemplate>
                                                                <asp:ImageButton ID="Btngo" runat="server" ImageUrl="~/LibImages/Go.jpg" Style="margin-left: -3px;"
                                                                    OnClick="btngo_Click" />
                                                                <asp:ImageButton ID="btnadd" runat="server" ImageUrl="~/LibImages/Add.jpg" OnClick="btnadd_Click" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </center>
                        </td>
                    </tr>
                </table>
            </div>
        </center>
        <br />
        <br />
        <center>
            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                <ContentTemplate>
                    <div>
                        <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                        <asp:GridView ID="grdSubscription" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                            ShowHeader="true" Font-Names="book antiqua" togeneratecolumns="true" OnSelectedIndexChanged="grdSubscription_onselectedindexchanged"
                            OnRowDataBound="grdSubscription_RowDataBound" OnRowCreated="grdSubscription_OnRowCreated"
                            Width="980px">
                            <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                        </asp:GridView>
                        <br />
                        <br />
                        <br />
                        <br />
                        <div id="rptprint" runat="server" visible="false">
                            <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                Visible="false"></asp:Label>
                            <asp:Label ID="lblrptname" runat="server" Font-Size="Medium" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" onkeypress="display()"
                                Font-Size="Medium" CssClass="textbox txtheight2"></asp:TextBox>
                            <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox textbox1 btn2"
                                Text="Export To Excel" Width="127px" />
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                Width="60px" CssClass="textbox textbox1 btn2" />
                            <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnExcel" />
                    <asp:PostBackTrigger ControlID="btnprintmaster" />
                </Triggers>
            </asp:UpdatePanel>
        </center>
    </div>
    <%-- add Popup--%>
    <asp:UpdatePanel ID="UpdatePanel17" runat="server">
        <ContentTemplate>
            <div id="popview" runat="server" class="popupstyle popupheight1" visible="false"
                style="height: 300em; font-family: Book Antiqua;">
                <asp:ImageButton ID="imagebtnpop1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 8px; margin-left: 1155px;"
                    OnClick="btn_popclose_Click" />
                <br />
                <center>
                    <div style="background-color: White; height: 745px; width: 960px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <span class="fontstyleheader" style="color: #008000;">Subscription Details</span>
                        </center>
                        <div>
                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                <ContentTemplate>
                                    <center>
                                        <fieldset id="studdetail" runat="server" style="height: 650px; width: 850px;">
                                            <table width="840px">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_lib" runat="server" Text="Library:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddl_lib" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                            Width="285px" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddl_lib_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <span style="color: Red;">*</span>
                                                    </td>
                                                    <td>
                                                        Sub Term:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Txt_STerm" runat="server" Style="width: 175px;" CssClass="textbox txtheight2"
                                                            Enabled="false" BackColor="#DCF9D1"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        SubscriptionCode:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_subscode" runat="server" Style="width: 79px;" CssClass="textbox txtheight2"
                                                            Enabled="false" BackColor="#DCF9D1"></asp:TextBox>
                                                        <span style="color: Red;">*</span> S.No
                                                        <asp:TextBox ID="Txtsno" runat="server" Style="width: 79px; margin-left: 21px" CssClass="textbox txtheight2"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        Budget Head:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="Cbo_Head" runat="server" Style="width: 185px; height: 30px;"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_Budget_SelectedIndexChanged"
                                                            CssClass="textbox3 textbox1">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        SubDate:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="dtp_subsdate" runat="server" AutoPostBack="true" Width="80px" CssClass="textbox txtheight2"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="dtp_subsdate" runat="server"
                                                            Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                    <td>
                                                        Journal Code:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_joucode" runat="server" Style="width: 135px;" onkeypress="display(this)"
                                                            CssClass="textbox txtheight2" Enabled="false" BackColor="#DCF9D1"></asp:TextBox>
                                                        <span style="color: Red;">*</span>
                                                        <asp:Button ID="btn_joucode" runat="server" Text="?" Style="width: 25px; height: 30px;"
                                                            OnClick="btn_joucode_OnClick" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Title:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_title" runat="server" Width="171px" Height="20px" CssClass="textbox txtheight2"
                                                            Enabled="false" BackColor="#DCF9D1"></asp:TextBox>
                                                        <span style="color: Red;">*</span>
                                                    </td>
                                                    <td>
                                                        Supplier Name:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_supp_name" runat="server" Style="width: 135px;" onkeypress="display(this)"
                                                            CssClass="textbox txtheight2" Enabled="false" BackColor="#DCF9D1"></asp:TextBox>
                                                        <span style="color: Red;">*</span>
                                                        <asp:Button ID="btn_supp_name" runat="server" Text="?" Style="width: 25px; height: 30px;"
                                                            OnClick="btn_supp_name_OnClick" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Title In Tamil:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="text_ttamil" runat="server" Width="171px" Height="20px" CssClass="textbox txtheight2"
                                                            Enabled="false" BackColor="#DCF9D1"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        Address:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Textadd" runat="server" Width="171px" Height="20px" CssClass="textbox txtheight2"
                                                            Enabled="false" BackColor="#DCF9D1"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Email:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Textemail" runat="server" Width="171px" Height="20px" CssClass="textbox txtheight2"
                                                            Enabled="false" BackColor="#DCF9D1"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        Website:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TextWebsite" runat="server" Width="171px" Height="20px" CssClass="textbox txtheight2"
                                                            Enabled="false" BackColor="#DCF9D1"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Period
                                                    </td>
                                                    <td>
                                                        From:
                                                        <asp:TextBox ID="fromdate" runat="server" Width="80px" CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                                                        <asp:CalendarExtender ID="cext_fromdate" TargetControlID="fromdate" runat="server"
                                                            Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                        <span style="color: Red;">*</span> To:
                                                        <asp:TextBox ID="todate" runat="server" Width="80px" AutoPostBack="true" OnTextChanged="todate_OnTextChanged" CssClass="textbox textbox1  txtheight2"></asp:TextBox>
                                                        <asp:CalendarExtender ID="cext_todate" TargetControlID="todate" runat="server" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                        <span style="color: Red;">*</span>
                                                    </td>
                                                    <td>
                                                        NoOfIssues:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Txt_NoofIssues" runat="server" Width="171px" Height="20px" CssClass="textbox txtheight2"
                                                            Enabled="false" BackColor="#DCF9D1"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Sub.Year:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="dd_Subyr" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                            Width="128px" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="dd_Subyr_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        Start Date:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="DTP_StartDate" runat="server" Width="80px" CssClass="textbox textbox1  txtheight2"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="DTP_StartDate" runat="server"
                                                            Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Renewal Date:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="dtp_renewaldate" runat="server" Width="80px" CssClass="textbox textbox1  txtheight2"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="dtp_renewaldate" runat="server"
                                                            Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                    <td>
                                                        Periodicity:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Textperiodicity" runat="server" Width="171px" Height="20px" CssClass="textbox txtheight2"
                                                            Enabled="false" BackColor="#DCF9D1"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Price:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_journalprice" runat="server" Width="171px" Height="20px" CssClass="textbox txtheight2"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        Cost:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Txt_Cost" runat="server" Text="0.00" Width="171px" Height="20px"
                                                            AutoPostBack="true" OnTextChanged="Txt_Cost_OnTextChanged" CssClass="textbox txtheight2"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Discount%:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Txt_Discount" runat="server" Text="0.00" Width="171px" Height="20px"  AutoPostBack="true" OnTextChanged="Txt_Discount_OnTextChanged"
                                                            CssClass="textbox txtheight2"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        Subs Amt:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Txt_SubsAmt" runat="server" Width="171px" Height="20px" CssClass="textbox txtheight2"
                                                            Enabled="false" BackColor="#DCF9D1"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Remarks:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_remarks" runat="server" Width="114px" Height="20px" CssClass="textbox txtheight2"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        Journal Issues:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_journalissues" runat="server" Width="114px" Height="20px" CssClass="textbox txtheight2"
                                                            Enabled="false" BackColor="#DCF9D1"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <fieldset id="Fieldset1" runat="server" style="height: 160px; width: 720px;">
                                                <fieldset style="width: 235px; height: 15px; margin-left: 2px">
                                                    <asp:RadioButtonList ID="rbl_amttype" runat="server" RepeatDirection="Horizontal"
                                                        AutoPostBack="true" OnSelectedIndexChanged="rbl_amttype_Selected">
                                                        <asp:ListItem Text="DD" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Cheque"></asp:ListItem>
                                                        <asp:ListItem Text="Transfer"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </fieldset>
                                                <table width="700px">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label_dd" runat="server" Text="DD No:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_ddno" runat="server" Width="80px" Height="20px" CssClass="textbox txtheight2"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label_da" runat="server" Text="DD Date:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="dtp_dddate" runat="server" Width="80px" CssClass="textbox textbox1  txtheight2"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="dtp_dddate" runat="server"
                                                                Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label_dm" runat="server" Text="DD Amt:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_ddamount" runat="server" Width="80px" Height="20px" CssClass="textbox txtheight2"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            Bank Name:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Cbo_BankName" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Width="190px" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddl_Bank_Name_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            Branch:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Cbo_Branch" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Width="190px" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddl_Branch_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            Place:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Cbo_Place" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Width="190px" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddl_place_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            In Favour:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_favourof" runat="server" Width="185px" Height="20px" CssClass="textbox txtheight2"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Lst_IssueList" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Visible="false">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                            <br />
                                            <center>
                                                <asp:UpdatePanel ID="UpSave" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="Txt_IssueBy" runat="server" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="Txt_PerIssue" runat="server" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="Txt_TotIssue" runat="server" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="Txt_IssueType" runat="server" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="Txt_IssueTypeVal" runat="server" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txt_suppliercode" runat="server" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txt_subsquocode" runat="server" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="Txt_Department" runat="server" Visible="false"></asp:TextBox>
                                                        <asp:LinkButton ID="LnkChangeissuedate" Text="Change Issue Date" Font-Name="Book Antiqua"
                                                            Font-Size="11pt" OnClick="LnkChangeissuedate_Click" runat="server" Width="215px" />
                                                        <asp:ImageButton ID="btn_save" runat="server" ImageUrl="~/LibImages/save.jpg" OnClick="btn_save_Click" />
                                                        <asp:ImageButton ID="btn_update" runat="server" ImageUrl="~/LibImages/update (2).jpg"
                                                            OnClick="btn_update_Click" Visible="false" />
                                                        <asp:ImageButton ID="btn_Delete" runat="server" ImageUrl="~/LibImages/delete.jpg"
                                                            OnClick="btn_Delete_Click" Visible="false" />
                                                        <asp:ImageButton ID="btn_exit" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                                            OnClick="btn_exit_Click" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </center>
                                        </fieldset>
                                    </center>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <br />
                    </div>
                </center>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--Change issue popup--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel18" runat="server">
            <ContentTemplate>
                <div id="popwindowChangeissuedate" runat="server" visible="false" style="height: 100%;
                    z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                    top: 0; left: 0px;">
                    <center>
                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <br />
                                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                    <ContentTemplate>
                                        <table style="height: 100px; width: 100%">
                                            <tr>
                                                <td>
                                                    Start Date:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="DTP_UpdStartDate" runat="server" Width="80px" CssClass="textbox textbox1  txtheight2"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="DTP_UpdStartDate" runat="server"
                                                        Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Change Date:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="DTP_ChangeDate" runat="server" Width="80px" CssClass="textbox textbox1  txtheight2"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender6" TargetControlID="DTP_ChangeDate" runat="server"
                                                        Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="UpChangeDate" runat="server">
                                                        <ContentTemplate>
                                                            <center>
                                                                <asp:ImageButton ID="btnSaveIssuedate" runat="server" ImageUrl="~/LibImages/save.jpg"
                                                                    OnClick="btnSaveIssuedate_Click" />
                                                                <asp:ImageButton ID="Buttonclose" runat="server" ImageUrl="~/LibImages/close.jpg"
                                                                    OnClick="Buttonclose_Click" />
                                                            </center>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%-- journal Code popup--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
            <ContentTemplate>
                <div id="popupselectjournal_Code" runat="server" visible="false" class="popupstyle popupheight1">
                    <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 27px; margin-left: 434px;"
                        OnClick="imagebtnpopclose2_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; font-family: Book Antiqua; height: 700px; width: 900px;
                        border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <asp:Label ID="Label2" runat="server" Text="Select Periodical Code" class="fontstyleheader"
                                Style="color: Green;"></asp:Label>
                        </center>
                        <br />
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    Library:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_sub_lib" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="155px" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddl_sub_libOnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Code:
                                    <asp:TextBox ID="txt_code" runat="server" Style="width: 154px; margin-left: 3px"
                                        CssClass="textbox txtheight2"></asp:TextBox>
                                </td>
                                <td>
                                    Title:
                                    <asp:TextBox ID="txtTitle" runat="server" Style="width: 154px; margin-left: 3px"
                                        CssClass="textbox txtheight2"></asp:TextBox>
                                    <asp:Button ID="btn_perio_go" Text="Go" OnClick="btn_perio_go_Click" CssClass="textbox btn1"
                                        runat="server" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="per" runat="server" style="height: 500px; width: 700px; overflow: auto;">
                            <asp:HiddenField ID="HiddenField1" runat="server" Value="-1" />
                            <asp:GridView ID="grdPeriodical" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                Font-Names="book antiqua" togeneratecolumns="true" OnSelectedIndexChanged="grdPeriodical_onselectedindexchanged"
                                OnRowCreated="grdPeriodical_OnRowCreated" Width="600px">
                                <%--AllowPaging="true" PageSize="50" OnPageIndexChanging="grdPeriodical_onpageindexchanged"--%>
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_sno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="<%#Container.DataItemIndex+1 %>" Visible="true">
                                            </asp:Label></center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                            </asp:GridView>
                        </div>
                        <br />
                        <center>
                            <asp:UpdatePanel ID="UpdatePanelbtn3" runat="server">
                                <ContentTemplate>
                                    <div>
                                        <asp:ImageButton ID="btn_jour_exit1" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                            Visible="false" OnClick="btn_jour_exit1_Click" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </center>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--  supplier Name--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
            <ContentTemplate>
                <div id="DivSuppliername" runat="server" visible="false" class="popupstyle popupheight1">
                    <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 27px; margin-left: 434px;"
                        OnClick="imagebtnpopclose3_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; font-family: Book Antiqua; height: 700px; width: 900px;
                        border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <asp:Label ID="Label1" runat="server" Text="Select Supplier Code" class="fontstyleheader"
                                Style="color: Green;"></asp:Label>
                        </center>
                        <br />
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    Library:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_supp_lib" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="155px" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddl_supp_lib_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Supplier Code:
                                    <asp:TextBox ID="TextSuppliercoe" runat="server" Style="width: 154px; margin-left: 3px"
                                        CssClass="textbox txtheight2"></asp:TextBox>
                                </td>
                                <td>
                                    Supplier Name:
                                    <asp:TextBox ID="TextSuppliername" runat="server" Style="width: 154px; margin-left: 3px"
                                        CssClass="textbox txtheight2"></asp:TextBox>
                                    <asp:Button ID="btn_supp_go" Text="Go" OnClick="btn_supp_go_Click" CssClass="textbox btn1"
                                        runat="server" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="suplier" runat="server" style="height: 500px; width: 700px; overflow: auto;">
                            <asp:HiddenField ID="HiddenField2" runat="server" Value="-1" />
                            <asp:GridView ID="grdSupplier" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                Font-Names="book antiqua" togeneratecolumns="true" OnSelectedIndexChanged="grdSupplier_onselectedindexchanged"
                                OnRowCreated="grdSupplier_OnRowCreated" Width="600px">
                                <%--AllowPaging="true" PageSize="50" OnPageIndexChanging="grdSupplier_onpageindexchanged"--%>
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_sno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="<%#Container.DataItemIndex+1 %>" Visible="true">
                                            </asp:Label></center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                            </asp:GridView>
                        </div>
                        <br />
                        <center>
                            <asp:UpdatePanel ID="UpdatePanelbtn4" runat="server">
                                <ContentTemplate>
                                    <div>
                                        <asp:ImageButton ID="btn_supp_exit1" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                            Visible="false" OnClick="btn_supp_exit1_Click" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </center>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
            <ContentTemplate>
                <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div1" runat="server" class="table" style="background-color: White; height: 120px;
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
                                                <asp:UpdatePanel ID="UpdatePanelbtn5" runat="server">
                                                    <ContentTemplate>
                                                        <asp:ImageButton ID="btnerrclose" runat="server" ImageUrl="~/LibImages/ok.jpg" OnClick="btnerrclose_Click" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <div>
        <asp:UpdatePanel ID="UpdatePanel14" runat="server">
            <ContentTemplate>
                <center>
                    <div id="Divissuerecord" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="Div5" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lbl_Divissuerecord" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:UpdatePanel ID="UpYes" runat="server">
                                                        <ContentTemplate>
                                                            <asp:ImageButton ID="btn_yes_issue" runat="server" ImageUrl="~/LibImages/yes.jpg"
                                                                OnClick="btn_yes_issue_Click" />
                                                            <asp:ImageButton ID="btn_no_issue" runat="server" ImageUrl="~/LibImages/no (2).jpg"
                                                                OnClick="bbtn_no_issue_Click" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
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
        </asp:UpdatePanel>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel15" runat="server">
            <ContentTemplate>
                <center>
                    <div id="surediv" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                                <asp:Label ID="lbl_sure" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpSure" runat="server">
                                                    <ContentTemplate>
                                                        <center>
                                                            <asp:ImageButton ID="btn_yes" runat="server" ImageUrl="~/LibImages/yes.jpg" OnClick="btn_sureyes_Click" />
                                                            <asp:ImageButton ID="btn_no" runat="server" ImageUrl="~/LibImages/no (2).jpg" OnClick="btn_sureno_Click" />
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
    <%--progressBar for GO--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpGo">
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
    </center>
    <%--progressBar for UpYes--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpYes">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for UpSure--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpSure">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="UpdateProgress3"
            PopupControlID="UpdateProgress3">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for UpSave--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpSave">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="UpdateProgress4"
            PopupControlID="UpdateProgress4">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for UpChangeDate--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="UpChangeDate">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender5" runat="server" TargetControlID="UpdateProgress5"
            PopupControlID="UpdateProgress5">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
