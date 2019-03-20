<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="HT_Income.aspx.cs" Inherits="HT_Income" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
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
            .table2
            {
                border: 1px solid #0CA6CA;
                border-radius: 10px;
                background-color: #0CA6CA;
                box-shadow: 0px 0px 8px #7bc1f7;
            }
        </style>
    </head>
    <body>
        <script type="text/javascript">

            function Test() {
                var id = "";
                var value1 = "";
                var idval = "";
                var empty = "";


                id = document.getElementById("<%=ddl_group.ClientID %>");
                value1 = id.options[id.selectedIndex].text;
                if (value1.trim() == "Select") {
                    empty = "E";
                    id = document.getElementById("<%=ddl_group.ClientID %>");
                    id.style.borderColor = 'Red';
                }



                id = document.getElementById("<%=ddl_hostelname.ClientID %>");
                value1 = id.options[id.selectedIndex].text;
                if (value1.trim() == "Select") {
                    empty = "E";
                    id = document.getElementById("<%=ddl_hostelname.ClientID %>");
                    id.style.borderColor = 'Red';
                }

                id = document.getElementById("<%=txt_amount.ClientID %>");
                value1 = id.value;
                if (value1.trim() == "") {
                    empty = "E";
                    id = document.getElementById("<%=txt_amount.ClientID %>");
                    id.style.borderColor = 'Red';
                }

                id = document.getElementById("<%=txt_popdesc.ClientID %>");
                value1 = id.value;
                if (value1.trim() == "") {
                    empty = "E";
                    id = document.getElementById("<%=txt_popdesc.ClientID %>");
                    id.style.borderColor = 'Red';
                }


                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                }
            }

            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }

            function display1() {
                document.getElementById('<%=lblerror.ClientID %>').innerHTML = "";
            }
        
    
        </script>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <center>
                    <br />
                    <div>
                        <%--<asp:Label ID="lbl_hostel" runat="server" class="fontstyleheader" Text="Hostel Income/Expenses"></asp:Label>--%>
                        <span style="color: #008000;" class="fontstyleheader">Hostel Income</span>
                        <br />
                        <br />
                    </div>
                </center>
                <div class="maindivstyle" style="height: 550px; overflow: auto;">
                    <br />
                    <table class="maintablestyle" style="width: 950px;">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_hostelname" runat="server" Text="Hostel Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel_hostelname" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_hostelname" runat="server" CssClass="textbox  txtheight2" ReadOnly="true"> --Select--</asp:TextBox>
                                        <asp:Panel ID="panel_hostelname" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                            width: 160px;">
                                            <asp:CheckBox ID="cb_hostelname" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_hostelname_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_hostelname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_hostelname_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txt_hostelname"
                                            PopupControlID="panel_hostelname" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_groupname" runat="server" Text="Group Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel_groupname" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_groupname" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_groupname" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                            width: 129px;">
                                            <asp:CheckBox ID="cb_groupname" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_groupname_CheckedChange" />
                                            <asp:CheckBoxList ID="cbl_groupname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_groupname_SelectedIndexChange">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_groupname"
                                            PopupControlID="panel_groupname" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td style="display: none;">
                                <asp:Label ID="lbl_subgroupname" Visible="false" runat="server" Text="SubGroup Name"></asp:Label>
                            </td>
                            <td style="display: none;">
                                <asp:UpdatePanel ID="updatepanel_subgroupname" Visible="false" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_subgroupname" Visible="false" runat="server" CssClass="textbox  txtheight2"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_subgroupname" Visible="false" runat="server" CssClass="multxtpanel"
                                            Style="height: 200px; width: 160px;">
                                            <asp:CheckBox ID="cb_subgroupname" runat="server" Width="100px" Text="Select All"
                                                AutoPostBack="True" OnCheckedChanged="cb_subgroup_CheckedChange" />
                                            <asp:CheckBoxList ID="cbl_subgroupname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_subgroup_SelectedIndexChange">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_subgroupname"
                                            PopupControlID="panel_subgroupname" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_description" runat="server" Text="Description"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel_description" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_description" runat="server" CssClass="textbox txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_description" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                            width: 130px;">
                                            <asp:CheckBox ID="cb_description" runat="server" Width="100px" Text="Select All"
                                                AutoPostBack="True" OnCheckedChanged="cb_description_CheckedChange" />
                                            <asp:CheckBoxList ID="cbl_description" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_description_SelectedIndexChange">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_description"
                                            PopupControlID="panel_description" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_fromdate" runat="server" Text="From Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox  txtheight2" ForeColor="Black"
                                    AutoPostBack="true" OnTextChanged="txt_fromdate_Textchanged"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_todate" runat="server" Text="To Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox  txtheight2" AutoPostBack="true"
                                    OnTextChanged="txt_todate_Textchanged"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:RadioButton ID="rdb_datewise" Text="Date Wise" runat="server" GroupName="same"
                                    OnCheckedChanged="rdb_datewise_CheckedChanged" AutoPostBack="true" />
                            </td>
                            <td>
                                <asp:RadioButton ID="rdb_totalwise" Text="Total Wise" runat="server" GroupName="same"
                                    OnCheckedChanged="rdb_totalwise_CheckedChanged" AutoPostBack="true" />
                            </td>
                            <td>
                                <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btn_addnew" runat="server" CssClass="textbox btn2" Text="Add New"
                                    OnClick="btn_addnew_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label>
                    <asp:Label ID="lbl_error1" runat="server" ForeColor="Red"></asp:Label>
                    <div>
                        <br />
                        <center>
                            <asp:Panel ID="pheaderfilter" runat="server" Visible="false" CssClass="table2" Height="22px"
                                Width="850px" Style="margin-top: -0.1%;">
                                <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                    Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </center>
                        <br />
                    </div>
                    <asp:Panel ID="pcolumnorder" runat="server" CssClass="table2" Width="850px" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="CheckBox_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                        Visible="false" Width="111px" OnClick="LinkButtonsremove_Click">Remove  All</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="tborder" Visible="false" Width="837px" TextMode="MultiLine" CssClass="style1"
                                        AutoPostBack="true" runat="server" Enabled="false">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" AutoPostBack="true"
                                        Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                        RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged">
                                        <asp:ListItem Value="Roll_No">Hostel Name</asp:ListItem>
                                        <asp:ListItem Value="Roll_Admit">Group </asp:ListItem>
                                        <asp:ListItem Value="Stud_Name">Sub Group</asp:ListItem>
                                        <asp:ListItem Value="Degree">Description</asp:ListItem>
                                        <asp:ListItem Value="DOB">Amount</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                        CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                        TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="right.jpeg"
                        ExpandedImage="down.jpeg">
                    </asp:CollapsiblePanelExtender>
                    <div id="div1" runat="server" visible="false" class="reportdivstyle" style="width: 750px;">
                        <br />
                        <FarPoint:FpSpread ID="Fpspread1" runat="server" Width="730px" Style="overflow: auto;
                            height: 350px; border: 0px solid #999999; border-radius: 5px; background-color: White;
                            box-shadow: 0px 0px 8px #999999;">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <div id="div2" runat="server" visible="false" class="reportdivstyle spreadborder"
                        style="width: 775px; height: 300px;">
                        <br />
                        <FarPoint:FpSpread ID="Fpspread2" runat="server" Style="width: 750px; height: 280px;
                            overflow: auto;" OnCellClick="Cell_Click" OnPreRender="Fpspread2_render">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <br />
                    <div id="rptprint" runat="server" visible="false">
                        <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                            Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" CssClass="textbox textbox1" runat="server" Height="20px"
                            Width="180px" onkeypress="display()"></asp:TextBox>
                        <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox btn1"
                            Text="Export To Excel" Width="127px" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            CssClass="textbox btn2" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                </div>
            </div>
        </center>
        <center>
            <div id="popwindow" runat="server" visible="false" style="height: 40em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 100px;
                left: 0;">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 12px; margin-left: 390px;"
                    OnClick="imagebtnpopclose_Click" />
                <br />
                <div class="subdivstyle" style="background-color: White; overflow: auto; width: 800px;
                    height: 400px;" align="center">
                    <br />
                    <center>
                        <asp:Label ID="lbl_detailsentry" runat="server" Style="font-size: large; color: Green;"
                            Text="Hostel Income Report"></asp:Label>
                    </center>
                    <br />
                    <div align="center" style="height: 300px; width: 739px;" class="spreadborder">
                        <br />
                        <table style="width: 700px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_hostelname1" runat="server" Text="Hostel Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_hostelname" CssClass="textbox textbox1" onfocus="return myFunction(this)"
                                        Style="width: 205px" Height="30px" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: right;">
                                    <asp:Label ID="lbl_todaydate" runat="server" Text="Today Date"></asp:Label>
                                    <asp:TextBox ID="txt_todaydate" runat="server" CssClass="textbox txtheight2"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_todaydate" runat="server"
                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_group1" runat="server" Text="Group"></asp:Label>
                                </td>
                                <td>
                                    <asp:Button ID="btn_plus" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                        Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus_Click" />
                                    <asp:DropDownList ID="ddl_group" runat="server" CssClass="textbox  ddlheight3" onchange="change1(this)"
                                        onfocus="return myFunction(this)">
                                    </asp:DropDownList>
                                    <asp:Button ID="btn_minus" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                        Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btnminus_Click" />
                                    <%--  <asp:TextBox ID="txt_group1"  CssClass="textbox  txtheight2" Style="width: 150px; display: none;
                               float: right; " onfocus="return myFunction(this)" runat="server"></asp:TextBox>--%>
                                    <span style="color: Red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="display: none;">
                                    <asp:Label ID="lbl_subgroup1" runat="server" Text="Sub Group"></asp:Label>
                                </td>
                                <td style="display: none;">
                                    <asp:Button ID="btn_plus1" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                        Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus1_Click" />
                                    <asp:DropDownList ID="ddl_subgroup" runat="server" CssClass="textbox  ddlheight3"
                                        onchange="change2(this)" onfocus="return myFunction(this)">
                                    </asp:DropDownList>
                                    <asp:Button ID="btn_minus1" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                        Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btnminus1_Click" />
                                    <%--<asp:TextBox ID="txt_subgroup1" CssClass="textbox  txtheight2" Style="width: 150px; display: none;
                                float: left;" onfocus="return myFunction(this)" runat="server"></asp:TextBox>--%>
                                    <span style="color: Red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Lblpopdesc" runat="server" Text="Description"></asp:Label>
                                </td>
                                <td>
                                    <%--  <asp:Button ID="btn_plus2" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                    Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus2_Click" />
                                <asp:DropDownList ID="ddlpopdesc" CssClass="textbox   ddlheight3" runat="server"
                                    onfocus="return myFunction(this)">
                                </asp:DropDownList>
                                <asp:Button ID="btn_minus2" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btnminus2_Click" />--%>
                                    <asp:TextBox ID="txt_popdesc" runat="server" onfocus="return myFunction(this)" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_popdesc"
                                        FilterType="LowercaseLetters,uppercaseletters,custom" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: Red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_amount" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Text=" Amount"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_amount" runat="server" onfocus="return myFunction(this)" CssClass="textbox textbox1 txtheight2"
                                        OnTextChanged="txt_amount_textchanged" AutoPostBack="true"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_amount"
                                        FilterType="Numbers,Custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: Red;">*</span>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div>
                            <center>
                                <asp:Button ID="btn_update" runat="server" CssClass="textbox btn2" OnClientClick="return Test()"
                                    Text="Update" OnClick="btn_Update_Click" Visible="false" />
                                <asp:Button ID="btn_delete" runat="server" CssClass="textbox btn2" OnClientClick="return Test()"
                                    Text="Delete" OnClick="btn_delete_Click" Visible="false" />
                                <asp:Button ID="btn_save" runat="server" CssClass="textbox btn2" OnClientClick="return Test()"
                                    Text="Save" OnClick="btn_save_Click" />
                                <asp:Button ID="btn_exit" runat="server" CssClass="textbox btn2" Text="Exit" OnClick="btn_exit_Click" />
                            </center>
                        </div>
                    </div>
                </div>
            </div>
        </center>
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
                                        <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                            OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
        <center>
            <div id="plusdiv" runat="server" visible="false" class="popupstyle popupheight1">
                <center>
                    <div id="panel_addgroup" runat="server" visible="false" class="table" style="background-color: White;
                        height: 140px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        margin-top: 200px; border-radius: 10px;">
                        <table style="line-height: 30px">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_addgroup" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:TextBox ID="txt_addgroup" runat="server" Width="200px" CssClass="textbox textbox1"
                                        onkeypress="display1()"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="line-height: 35px">
                                    <asp:Button ID="btn_addgroup1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                        CssClass="textbox btn2" OnClick="btn_addgroup_Click" />
                                    <asp:Button ID="btn_exitgroup1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                        CssClass="textbox btn2" OnClick="btn_exitaddgroup_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblerror" runat="server" Visible="false" ForeColor="red" Font-Size="Smaller"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </div>
        </center>
        <%--  09.10.15--%>
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
                                        <center>
                                            <asp:Button ID="btn_yes" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btn_sureyes_Click" Text="yes" runat="server" />
                                            <asp:Button ID="btn_no" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btn_sureno_Click" Text="no" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        </form>
    </body>
    </html>
</asp:Content>
