<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="inv_Hostel_performance_report_and_chart.aspx.cs" Inherits="inv_Hostel_performance_report_and_chart" %>

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
        <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
        <%--<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>--%>
        <script type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
        <style type="text/css">
            .cont
            {
                width: 200px;
                height: 30px;
            }
            .btn
            {
                width: 40px;
                height: 30px;
            }
            .btn1
            {
                width: 80px;
                height: 30px;
            }
            .sty1
            {
                height: 550px;
                width: 900px;
                background-color: White;
                border: 5px solid #0CA6CA;
                border-top: 30px solid #0CA6CA;
                border-radius: 10px;
            }
            .backpaneldrop
            {
                position: absolute;
                background-color: White;
                border: 1px solid Gray;
            }
            .style
            {
                height: 730px;
                width: 1000px;
                border: 1px solid #999999;
                box-shadow: 0px 0px 8px #999999; /*F0F0F0*/
                -moz-box-shadow: 0px 0px 10px #999999;
                -webkit-box-shadow: 0px 0px 10px #999999;
                border: 3px solid #D9D9D9;
                border-radius: 15px;
            }
            .ddlstyle
            {
                width: 200px;
                height: 30px;
                outline: none;
                border: 1px solid #7bc1f7;
                box-shadow: 0px 0px 8px #7bc1f7;
                -moz-box-shadow: 0px 0px 8px #7bc1f7;
                -webkit-box-shadow: 0px 0px 8px #7bc1f7;
            }
            .txtdate
            {
                border: 1px solid #c4c4c4;
                height: 20px;
                width: 70px;
                font-size: 13px;
                text-transform: capitalize;
                padding: 4px 4px 4px 4px;
                border-radius: 4px;
                -moz-border-radius: 4px;
                -webkit-border-radius: 4px;
                box-shadow: 0px 0px 8px #d9d9d9;
                -moz-box-shadow: 0px 0px 8px #d9d9d9;
                -webkit-box-shadow: 0px 0px 8px #d9d9d9;
            }
            .multxtpanel
            {
                background: White;
                border-color: Black;
                border-style: Solid;
                border-width: 2px;
            }
            .spreadborder
            {
                border: 2px solid #999999;
                background-color: White;
                box-shadow: 0px 0px 8px #999999; /*F0F0F0*/
                border-radius: 10px;
                overflow: auto;
            }
            .table1
            {
                background-color: #10BADC;
                box-shadow: 0px 0px 8px #999999; /*F0F0F0*/
                border-radius: 10px;
            }
            .table
            {
                background-color: white;
                box-shadow: 0px 0px 8px #999999; /*F0F0F0*/
                border-radius: 10px;
            }
            .container
            {
                width: 100%;
            }
            .col1
            {
                float: left;
                width: 20%;
            }
            .col2
            {
                float: right;
                width: 79%;
                height: 500px;
                margin-top: 0px;
                border: 1px solid #999999;
                box-shadow: 0px 0px 8px #999999; /*F0F0F0*/
                -moz-box-shadow: 0px 0px 10px #999999;
                -webkit-box-shadow: 0px 0px 10px #999999;
                border: 3px solid #D9D9D9;
                border-radius: 15px;
                position: relative;
            }
            .tabeltd
            {
                background-color: #79BD9A;
                text-decoration: none;
                color: white;
            }
            .tdstyle:hover
            {
                outline: none; /*border: 1px solid #7bc1f7;*/
                background-color: #7F7F7F; /*box-shadow: 0px 0px 8px #7bc1f7;*/
                -moz-box-shadow: 0px 0px 8px #7bc1f7;
                -webkit-box-shadow: 0px 0px 8px #7bc1f7;
            }
            .multxtpanel
            {
                background: White;
                border-color: Gray;
                border-style: Solid;
                border-width: 2px;
                position: absolute;
                box-shadow: 0px 0px 4px #999999;
                border-radius: 5px;
                overflow: auto;
            }
            .new
            {
                border-radius: 25px;
                background-color: #10BADC;
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
            function oncl() {
                if (document.getElementById("<%=Div1.ClientID %>").style.display == "block") {
                    document.getElementById("<%=Div1.ClientID %>").style.display = "none";
                }
                else {
                    document.getElementById("<%=Div1.ClientID %>").style.display = "block";
                }
            }
            function PrintPanel() {
                var panel = document.getElementById("<%=itemusedDiv.ClientID %>");
                var printWindow = window.open('', '', 'height=auto,width=auto');
                printWindow.document.write('<html');
                printWindow.document.write('<head>');
                printWindow.document.write('</head><body >');
                printWindow.document.write('<form>');
                printWindow.document.write(panel.innerHTML);
                printWindow.document.write(' </form>');
                printWindow.document.write('</body></html>');
                printWindow.document.close();
                setTimeout(function () {
                    printWindow.print();
                }, 500);
                return false;
            }
            function sessionMenuStrengthPrint() {
                var panel = document.getElementById("<%=menuexpDiv.ClientID %>");
                var printWindow = window.open('', '', 'height=auto,width=auto');
                printWindow.document.write('<html');
                printWindow.document.write('<head>');
                printWindow.document.write('</head><body >');
                printWindow.document.write('<form>');
                printWindow.document.write(panel.innerHTML);
                printWindow.document.write(' </form>');
                printWindow.document.write('</body></html>');
                printWindow.document.close();
                setTimeout(function () {
                    printWindow.print();
                }, 500);
                return false;
            }
            
                  
        </script>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <br />
            <div style="width: 1000px;">
                <div id="Menuid" onclick="return oncl()" style="background-color: Green; background-image: url('../dashbd/Menuimage.png');
                    background-size: 46px 41px; height: 41px; width: 44px; float: left;">
                </div>
            </div>
        </center>
        <div id="Div1" runat="server" style="height: 20em; z-index: 1000; display: none;
            width: 90%; position: absolute; margin-left: 16px; margin-top: 38px;">
            <table class="table1" cellspacing="10px">
                <tr>
                    <td align="center">
                        <span style="color: #FFFFFF">Reports</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btn_itemused" runat="server" Text="Item Used Report" Width="260px"
                            Height="30px" CssClass="textbox textbox1 new" OnClick="btn_itemused_Click" />
                    </td>
                </tr>
                <tr style="display: none;">
                    <td>
                        <asp:Button ID="btnitem_purchase" runat="server" Text="Item Purchase History" Width="260px"
                            Height="30px" CssClass="textbox textbox1 new" OnClick="btnitem_purchase_Click" />
                    </td>
                </tr>
                <tr style="display: none;">
                    <td>
                        <asp:Button ID="btnmenu_prepaid" runat="server" Text="Menu Prepaid History" Width="260px"
                            Height="30px" CssClass="textbox textbox1 new" OnClick="btnmenu_prepaid_Click" />
                    </td>
                </tr>
                <tr style="display: none;">
                    <td>
                        <asp:Button ID="btncost_pur" runat="server" Text="Cost Purchase History" Width="260px"
                            Height="30px" CssClass="textbox textbox1 new" OnClick="btncost_pur_Click" />
                    </td>
                </tr>
                <tr style="display: none;">
                    <td>
                        <asp:Button ID="btnvendor_sup" runat="server" Text="Supplier Supplied History" Width="260px"
                            Height="30px" CssClass="textbox textbox1 new" OnClick="btnvendor_sup_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnmenuexp_hty" runat="server" Text="Session Menu Expenses / Strength History"
                            Width="260px" Height="30px" CssClass="textbox textbox1 new" OnClick="btnmenuexp_hty_Click" />
                    </td>
                </tr>
                <tr style="display: none">
                    <td>
                        <asp:Button ID="btnwestage" runat="server" Text="History of Item Wastage" Width="200px"
                            Height="30px" CssClass="textbox textbox1 new" OnClick="btnwestage_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <center>
            <div id="div1itemused" runat="server" visible="false">
                <center>
                    <div style="width: 1000px">
                        <span class="fontstyleheader" style="color: Green;">Hostel Performance Report / Chart</span>
                    </div>
                </center>
                <br />
                <div class="style">
                    <br />
                    <center>
                        <table style="border: 1px solid #0CA6CA; border-radius: 10px; width: 960px; background-color: #0CA6CA;
                            height: 50px; box-shadow: 0px 0px 8px #7bc1f7;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblhostelname" Text="Mess Name" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <%-- <asp:DropDownList ID="ddlhostelname" runat="server" Width="160px" Height="30px" CssClass="textbox textbox1"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlhostelname_change">
                            </asp:DropDownList>--%>
                                    <asp:UpdatePanel runat="server" ID="upp0">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txthostel" runat="server" CssClass="textbox textbox1" Width="130px"
                                                Height="20px" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="p0" runat="server" Height="200px" Width="150px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="chkhostelname" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="chkhostelchange" />
                                                <asp:CheckBoxList ID="chklsthostelname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklsthostelname_selectindex">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="pop0" runat="server" TargetControlID="txthostel" PopupControlID="p0"
                                                Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblsessionname" runat="server" Text="Session Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtsessionname" runat="server" CssClass="textbox textbox1" Width="130px"
                                                Height="20px" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="p1" runat="server" Height="200px" Width="150px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="chksessionname" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="chksession_checkedchange" />
                                                <asp:CheckBoxList ID="chklstsession" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklstsession_Change ">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtsessionname"
                                                PopupControlID="p1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblfromdate" Text="From Date" runat="server"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtfromdate" runat="server" Width="80px" AutoPostBack="true" OnTextChanged="txtfromdate_Change"
                                        CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:CalendarExtender ID="calfromdate" TargetControlID="txtfromdate" runat="server"
                                        Format="dd/MM/yyyy">
                                        <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                    </asp:CalendarExtender>
                                    <asp:Label ID="lbltodate" Text="To Date" runat="server"></asp:Label>
                                    <asp:TextBox ID="txttodate" runat="server" Width="80px" AutoPostBack="true" OnTextChanged="txttodate_Change"
                                        CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:CalendarExtender ID="caltodate" TargetControlID="txttodate" runat="server" Format="dd/MM/yyyy">
                                        <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblmenuname" runat="server" Text="Menu Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upp2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtmenuname" runat="server" CssClass="textbox textbox1" Width="130px"
                                                Height="20px" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="p2" runat="server" Height="200px" Width="150px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="chkmenuname" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="chkmenuname_Change" />
                                                <asp:CheckBoxList ID="chk_lstmenuname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chk_lstmenuname_Change">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="Pop2" runat="server" TargetControlID="txtmenuname"
                                                PopupControlID="p2" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td colspan="3">
                                    <asp:RadioButton ID="rdoday" Text="Daywise" runat="server" GroupName="day" AutoPostBack="true" />
                                    <asp:RadioButton ID="rdoweek" Text="Weekwise" runat="server" GroupName="day" AutoPostBack="true" />
                                    <asp:RadioButton ID="rdomonth" Text="Monthwise" runat="server" GroupName="day" AutoPostBack="true" />
                                    <asp:RadioButton ID="rdodaycom" runat="server" Text="Day Comparison" GroupName="day"
                                        AutoPostBack="true" />
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="uu" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtdaycompar" Visible="false" runat="server" CssClass="textbox textbox1"
                                                Width="110px" Height="20px" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="p00" runat="server" Width="150px" CssClass="multxtpanel" Visible="false">
                                                <asp:CheckBox ID="chkdaycompar" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="chdaycompar_change" />
                                                <asp:CheckBoxList ID="chklstdaycompar" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkklstdaycompar_selectIndex">
                                                    <asp:ListItem Value="0">Monday</asp:ListItem>
                                                    <asp:ListItem Value="1">Tuesday</asp:ListItem>
                                                    <asp:ListItem Value="2">Wednesday</asp:ListItem>
                                                    <asp:ListItem Value="3">Thursday</asp:ListItem>
                                                    <asp:ListItem Value="4">Friday</asp:ListItem>
                                                    <asp:ListItem Value="5">Saturday</asp:ListItem>
                                                    <asp:ListItem Value="6">Sunday</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtdaycompar"
                                                PopupControlID="p00" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:RadioButton ID="rdbquantity" Text="Quantity" runat="server" GroupName="day1" />
                                    <asp:RadioButton ID="rdbValue" Text="Value" runat="server" GroupName="day1" />
                                </td>
                                <td colspan="4">
                                    <asp:RadioButton ID="rdb_headerwise" runat="server" Text="Header Wise" GroupName="grdo" />
                                    <asp:RadioButton ID="rdb_Subheaderwise" runat="server" Text="Sub Header Wise" GroupName="grdo" />
                                    <asp:RadioButton ID="rdb_itemwise" runat="server" Text="Item Wise" GroupName="grdo" />
                                    <asp:CheckBox ID="chk_directconsum" runat="server" Text="Direct Consumption" />
                                    <asp:Button ID="btngo" runat="server" Text="Go" CssClass="textbox btn" OnClick="btngo_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div>
                            <asp:Label ID="lblerror" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                        </div>
                        <br />
                        <div id="itemusedDiv" runat="server">
                            <asp:Chart ID="Chart1" runat="server" Height="500px" Width="1030px" Visible="false"
                                Font-Names="Book Antiqua" EnableViewState="true" Style="overflow: auto;" Font-Size="Medium">
                                <Series>
                                </Series>
                                <Legends>
                                    <asp:Legend Title="Item Issued" ShadowOffset="2" Font="Book Antiqua">
                                    </asp:Legend>
                                </Legends>
                                <Titles>
                                    <asp:Title Docking="Bottom" Text="Item Name" Font="Microsoft Sans Serif, 12pt">
                                    </asp:Title>
                                    <asp:Title Docking="Left" Text="Quantity Issued" Font="Microsoft Sans Serif, 12pt">
                                    </asp:Title>
                                </Titles>
                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1" BorderWidth="0">
                                        <AxisY LineColor="White">
                                            <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                            <MajorGrid LineColor="#e6e6e6" />
                                            <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                        </AxisY>
                                        <AxisX LineColor="White" IsLabelAutoFit="true">
                                            <LabelStyle Font="Trebuchet MS, 8.25pt" Angle="-90" Interval="1" />
                                            <MajorGrid LineColor="#e6e6e6" />
                                            <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                        </AxisX>
                                        <%--   <Area3DStyle Enable3D="true" WallWidth="10"/>--%>
                                    </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                        </div>
                        <br />
                        <asp:Button ID="btnprintimag" Visible="false" Text="Chart Print To PDF" Height="30px"
                            runat="server" CssClass="btn2 textbox txtheight2" OnClientClick="return PrintPanel();" />
                        <br />
                    </center>
                </div>
            </div>
            <div id="div2Itempurchasehty" runat="server">
                <center>
                    <div style="width: 1000px">
                        <span class="fontstyleheader" style="color: Green;">Item Purchase History</span>
                    </div>
                </center>
                <br />
                <div class="style">
                    <br />
                    <center>
                        <table class="table2">
                            <tr>
                                <td>
                                    <asp:Label ID="lblpop2hosname" Text="Mess Name" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtpop2hostelname" runat="server" CssClass="textbox textbox1" Width="130px"
                                                ReadOnly="true" Height="20px">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel1" runat="server" Height="200px" Width="150px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cbpop2hostel" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cbpop2hostel_checkedchange" />
                                                <asp:CheckBoxList ID="cblpop2hostel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblpop2hostel_Change ">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtpop2hostelname"
                                                PopupControlID="Panel1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:DropDownList ID="ddlpop2hosname" runat="server" Visible="false" CssClass="textbox ddlstyle"
                                        AutoPostBack="True" Width="160px" Height="30px" OnSelectedIndexChanged="ddlpop2hostelname_change">
                                    </asp:DropDownList>
                                </td>
                                <td style="display: none;">
                                    <asp:Label ID="lblpop2sessionname" runat="server" Text="Session Name"></asp:Label>
                                </td>
                                <td style="display: none;">
                                    <asp:UpdatePanel ID="upp3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtpop2sessionname" runat="server" CssClass="textbox textbox1" Width="130px"
                                                ReadOnly="true" Height="20px">--Select--</asp:TextBox>
                                            <asp:Panel ID="p3" runat="server" Height="200px" Width="150px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="chkpop2session" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="chkpop2session_checkedchange" />
                                                <asp:CheckBoxList ID="chklst_pop2session" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklst_pop2session_Change ">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="Pop3" runat="server" TargetControlID="txtpop2sessionname"
                                                PopupControlID="p3" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblpop2from" Text="From Date" runat="server"></asp:Label>
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtpop2from" runat="server" Width="80px" CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtpop2from" runat="server"
                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:Label ID="lblpop2to" Text="To Date" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtpop2to" runat="server" Width="80px" CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtpop2to" runat="server"
                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="display: none;">
                                    <asp:Label ID="lblpop2menuname" runat="server" Text="Menu Name"></asp:Label>
                                </td>
                                <td style="display: none;">
                                    <asp:UpdatePanel ID="upp4" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtpop2menuname" runat="server" CssClass="textbox textbox1" Width="130px"
                                                ReadOnly="true" Height="20px">--Select--</asp:TextBox>
                                            <asp:Panel ID="p4" runat="server" Height="200px" Width="150px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="chkpop2menuname" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="chkpop2menuname_Change" />
                                                <asp:CheckBoxList ID="chklst_pop2menuname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chk_lstpop2menuname_Change">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="Pop4" runat="server" TargetControlID="txtpop2menuname"
                                                PopupControlID="p4" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdopop2Qty" runat="server" Text="Quantity" GroupName="pop2qty" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdopop2value" runat="server" Text="Value" GroupName="pop2qty" />
                                    <asp:Button ID="btnpop2go" runat="server" Text="Go" CssClass="textbox btn" OnClick="btnpop2go_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <center>
                            <div>
                                <asp:Label ID="lblpop2error" runat="server" ForeColor="Red"></asp:Label>
                            </div>
                            <br />
                            <br />
                            <div id="itempurchaseDiv" runat="server">
                                <asp:Chart ID="Chart2" runat="server" Width="970px" Visible="false" Font-Names="Book Antiqua"
                                    EnableViewState="true" Font-Size="Medium">
                                    <Series>
                                    </Series>
                                    <Legends>
                                        <asp:Legend Title="Item Purchased Date" ShadowOffset="2" Font="Book Antiqua">
                                        </asp:Legend>
                                    </Legends>
                                    <Titles>
                                        <asp:Title Docking="Bottom" Text="Item Name">
                                        </asp:Title>
                                        <asp:Title Docking="Left" Text="Quantity Purchased">
                                        </asp:Title>
                                    </Titles>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartArea1" BorderWidth="0">
                                            <AxisY LineColor="White">
                                                <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                                <MajorGrid LineColor="#e6e6e6" />
                                                <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                            </AxisY>
                                            <AxisX LineColor="White">
                                                <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                                <MajorGrid LineColor="#e6e6e6" />
                                                <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                            </AxisX>
                                            <%--   <Area3DStyle Enable3D="true" WallWidth="10"/>--%>
                                        </asp:ChartArea>
                                    </ChartAreas>
                                </asp:Chart>
                            </div>
                        </center>
                    </center>
                </div>
            </div>
            <div id="div3menumenuprehty" runat="server">
                <center>
                    <div style="width: 1000px">
                        <span class="fontstyleheader" style="color: Green;">Menu Preferred History</span></div>
                </center>
                <br />
                <div class="style">
                    <br />
                    <center>
                        <table class="table2">
                            <tr>
                                <td>
                                    <asp:Label ID="lblpop3hosname" Text="Mess Name" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlpop3hostel" runat="server" CssClass="textbox ddlstyle" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlpop3hostelname_change">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblpop3session" runat="server" Text="Session Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upp5" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtpop3session" runat="server" CssClass="textbox textbox1" Width="150px"
                                                Height="20px" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="p5" runat="server" Height="200px" Width="150px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="chkpop3session" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="chkpop3session_checkedchange" />
                                                <asp:CheckBoxList ID="chklstpop3session" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklst_pop3session_Change ">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="pop5" runat="server" TargetControlID="txtpop3session"
                                                PopupControlID="p5" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblpop3from" Text="From Date" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtpop3from" runat="server" Width="80px" CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txtpop3from" runat="server"
                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:Label ID="lblpop3to" Text="To Date" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtpop3to" runat="server" Width="80px" CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="txtpop3to" runat="server"
                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                                <td colspan="2">
                                    <asp:Button ID="btnpop3go" runat="server" Text="Go" CssClass="textbox btn" OnClick="btnpop3go_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <center>
                            <div>
                                <asp:Label ID="lblpop3error" ForeColor="Red" runat="server"></asp:Label>
                            </div>
                        </center>
                    </center>
                </div>
            </div>
            <div id="div4costpurhty" runat="server">
                <center>
                    <div style="width: 1000px">
                        <span class="fontstyleheader" style="color: Green;">Cost Of Purchased History</span>
                    </div>
                </center>
                <br />
                <div class="style">
                    <br />
                    <center>
                        <table class="table2">
                            <tr>
                                <td>
                                    <asp:Label ID="lblpop4hosname" Text="Mess Name" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlpop4hostel" runat="server" CssClass="textbox ddlstyle" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlpop4hostelname_change">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblpop4session" runat="server" Text="Session Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upp7" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtpop4session" runat="server" CssClass="textbox textbox1" Width="130px"
                                                Height="20px">--Select--</asp:TextBox>
                                            <asp:Panel ID="p7" runat="server" Height="200px" Width="150px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="chkpop4session" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="chkpop4session_checkedchange" />
                                                <asp:CheckBoxList ID="chklstpop4session" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chklst_pop4session_Change">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="pop7" runat="server" TargetControlID="txtpop4session"
                                                PopupControlID="p7" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblpop4menunmae" runat="server" Text="Menu Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upp8" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtpop4menuname" runat="server" CssClass="textbox textbox1" Width="130px"
                                                Height="20px">--Select--</asp:TextBox>
                                            <asp:Panel ID="p8" runat="server" Width="150px" Height="200px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="chkpop4menuname" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="chkpop4menuname_Change" />
                                                <asp:CheckBoxList ID="chklstpop4menuname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chk_lstpop4menuname_Change">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="pop8" runat="server" TargetControlID="txtpop4menuname"
                                                PopupControlID="p8" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblpop4from" Text="From Date" runat="server"></asp:Label>
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtpop4from" runat="server" Width="80px" CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txtpop4from" runat="server"
                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:Label ID="lblpop4to" Text="To Date" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtpop4to" runat="server" Width="80px" CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender6" TargetControlID="txtpop4to" runat="server"
                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:Button ID="btnpop4qty" runat="server" Text="Quantity" CssClass="textbox btn1"
                                        OnClick="btnpop4qty_Click" />
                                    <asp:Button ID="btnpop4value" runat="server" Text="Value" CssClass="textbox btn1"
                                        OnClick="btnpop4value_Click" />
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <div>
                            <asp:Label ID="lblpop4error" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                    </center>
                    <br />
                </div>
            </div>
            <div id="div5vensuphty" runat="server">
                <center>
                    <div style="width: 1000px">
                        <span class="fontstyleheader" style="color: Green;">Supplier Supplied History</span></div>
                </center>
                <br />
                <div class="style">
                    <br />
                    <center>
                        <table class="table2">
                            <tr>
                                <td>
                                    <asp:Label ID="lblpop5vendorName" runat="server" Text="Supplier Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upp9" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtpop5vendorname" Width="200px" runat="server" CssClass="textbox textbox1"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="p9" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                                <asp:CheckBox ID="chkpop5vendorname" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cbvendorname_CheckedChanged" />
                                                <asp:CheckBoxList ID="chklstpop5vendorname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblvendorname_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="pop9" runat="server" TargetControlID="txtpop5vendorname"
                                                PopupControlID="p9" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblitm" runat="server" Text="Item"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtitm" runat="server" Width="200px" CssClass="textbox textbox1"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="p10" CssClass="multxtpanel" Height="200px" Width="200px" runat="server">
                                                <asp:CheckBox ID="chkitm" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="chkitm1" />
                                                <asp:CheckBoxList ID="cblitm" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblitm1">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtitm"
                                                PopupControlID="p10" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblpop5from" Text="From Date" runat="server"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtpop5from" runat="server" Width="80px" CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender7" TargetControlID="txtpop5from" runat="server"
                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:Label ID="lblpop5to" Text="To Date" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtpop5to" runat="server" Width="80px" CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender8" TargetControlID="txtpop5to" runat="server"
                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rdQunatity" runat="server" Text="Quantity" GroupName="Newsame" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdValue" runat="server" Text="Value" GroupName="Newsame" />
                                    <asp:Button ID="btnpop5go" runat="server" Text="Go" CssClass="textbox btn" OnClick="btnpop5go_Click" />
                                </td>
                            </tr>
                        </table>
                        <center>
                            <div>
                                <asp:Label ID="lblpop5error" ForeColor="Red" runat="server"></asp:Label>
                            </div>
                            <br />
                            <div id="vensupplierhistoryDiv" runat="server">
                                <asp:Chart ID="Chart3" runat="server" Width="970px" Visible="false" Font-Names="Book Antiqua"
                                    EnableViewState="true" Font-Size="Medium">
                                    <Series>
                                    </Series>
                                    <Legends>
                                        <asp:Legend Title="Item Issued" ShadowOffset="2" Font="Book Antiqua">
                                        </asp:Legend>
                                    </Legends>
                                    <Titles>
                                        <asp:Title Docking="Bottom" Text="Vendor Name">
                                        </asp:Title>
                                        <asp:Title Docking="Left" Text="Quantity Purchased">
                                        </asp:Title>
                                    </Titles>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartArea1" BorderWidth="0">
                                            <AxisY LineColor="White">
                                                <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                                <MajorGrid LineColor="#e6e6e6" />
                                                <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                            </AxisY>
                                            <AxisX LineColor="White">
                                                <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                                <MajorGrid LineColor="#e6e6e6" />
                                                <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                            </AxisX>
                                            <%--   <Area3DStyle Enable3D="true" WallWidth="10"/>--%>
                                        </asp:ChartArea>
                                    </ChartAreas>
                                </asp:Chart>
                            </div>
                        </center>
                    </center>
                </div>
            </div>
            <div id="div6menuexp" runat="server">
                <center>
                    <div style="width: 1000px">
                        <span class="fontstyleheader" style="color: Green;">Session Menu Expenses / Strength
                            History</span></div>
                </center>
                <br />
                <div class="style">
                    <br />
                    <center>
                        <table class="table2">
                            <tr>
                                <td>
                                    <asp:Label ID="lblpop6hostel" runat="server" Text="Mess Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upp11" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtpop6hos" runat="server" CssClass="textbox textbox1" Width="150px"
                                                Height="20px">--Select--</asp:TextBox>
                                            <asp:Panel ID="p11" runat="server" CssClass="multxtpanel" Height="200px" Width="150px">
                                                <asp:CheckBox ID="chkpop6hos" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="chkpop6hostel_checkedchange" />
                                                <asp:CheckBoxList ID="chklstpop6hos" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklst_pop6hostel_Change">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="pop11" runat="server" TargetControlID="txtpop6hos"
                                                PopupControlID="p11" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblpop6session" runat="server" Text="Session Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upp12" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtpop6session" runat="server" CssClass="textbox textbox1" Width="150px"
                                                Height="20px">--Select--</asp:TextBox>
                                            <asp:Panel ID="p12" runat="server" Height="150px" Width="200px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="chkpop6session" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="chkpop6session_checkedchange" />
                                                <asp:CheckBoxList ID="chklstpop6session" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklst_pop6session_Change">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="pop6" runat="server" TargetControlID="txtpop6session"
                                                PopupControlID="p12" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblpop6formdate" Text="From Date" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_pop6fromdate" runat="server" Width="80px" CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender9" TargetControlID="txt_pop6fromdate" runat="server"
                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbltodate0" Text="To Date" runat="server"></asp:Label>
                                    <asp:TextBox ID="txt_pop6todate" runat="server" Width="80px" CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender10" TargetControlID="txt_pop6todate" runat="server"
                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rdopop6day" Text="Daywise" runat="server" GroupName="day1" AutoPostBack="true" />
                                </td>
                                <td colspan="2">
                                    <asp:RadioButton ID="rdopop6week" Text="Weekwise" runat="server" GroupName="day1"
                                        AutoPostBack="true" />
                                    <asp:RadioButton ID="rdopop6month" Text="Monthwise" runat="server" GroupName="day1"
                                        AutoPostBack="true" />
                                    <asp:RadioButton ID="rdodaycompar1" runat="server" Text="Day Comparsion" GroupName="day1"
                                        AutoPostBack="true" />
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="up16" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtcompar1" runat="server" Visible="false" CssClass="textbox textbox1"
                                                Width="110px" Height="20px" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="p16" runat="server" Width="150px" Height="200px" CssClass="multxtpanel"
                                                Visible="false">
                                                <asp:CheckBox ID="chkdaycompar1" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="chdaycompar_change1" />
                                                <asp:CheckBoxList ID="chklstdaycompar1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkklstdaycompar_selectIndex1">
                                                    <asp:ListItem Value="0">Monday</asp:ListItem>
                                                    <asp:ListItem Value="1">Tuesday</asp:ListItem>
                                                    <asp:ListItem Value="2">Wednesday</asp:ListItem>
                                                    <asp:ListItem Value="3">Thursday</asp:ListItem>
                                                    <asp:ListItem Value="4">Friday</asp:ListItem>
                                                    <asp:ListItem Value="5">Saturday</asp:ListItem>
                                                    <asp:ListItem Value="6">Sunday</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender16" runat="server" TargetControlID="txtcompar1"
                                                PopupControlID="p16" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdbExpenses" Text="Expenses" runat="server" GroupName="day5" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdbstrength" Text="Strength" runat="server" GroupName="day5" />
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btngo6" runat="server" Text="Go" OnClick="btngo6_Clcik" CssClass="textbox btn" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <center>
                            <div>
                                <asp:Label ID="lblpop6error" Visible="false" ForeColor="Red" runat="server"></asp:Label>
                            </div>
                        </center>
                        <br />
                        <div id="menuexpDiv" runat="server">
                            <asp:Chart ID="Chart4" runat="server" Width="970px" Visible="false" Font-Names="Book Antiqua"
                                EnableViewState="true" Font-Size="Medium">
                                <Series>
                                </Series>
                                <Legends>
                                    <asp:Legend Title="Item Issued" ShadowOffset="2" Font="Book Antiqua">
                                    </asp:Legend>
                                </Legends>
                                <Titles>
                                    <asp:Title Docking="Bottom" Text="Menu Name">
                                    </asp:Title>
                                    <asp:Title Docking="Left" Text="Menu Expenses">
                                    </asp:Title>
                                </Titles>
                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1" BorderWidth="0">
                                        <AxisY LineColor="White">
                                            <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                            <MajorGrid LineColor="#e6e6e6" />
                                            <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                        </AxisY>
                                        <AxisX LineColor="White">
                                            <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                            <MajorGrid LineColor="#e6e6e6" />
                                            <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                        </AxisX>
                                        <%--   <Area3DStyle Enable3D="true" WallWidth="10"/>--%>
                                    </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                        </div>
                        <br />
                        <asp:Button ID="btnprintSessionMenu" Visible="false" Text="Chart Print To PDF" Height="30px"
                            runat="server" CssClass="btn2 textbox txtheight2" OnClientClick="return sessionMenuStrengthPrint();" />
                        <br />
                    </center>
                </div>
            </div>
        </center>
        </form>
    </body>
    </html>
</asp:Content>
