<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Alumni1.aspx.cs" Inherits="Alumni1" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .printclass
        {
            display: none;
        }
        .grid-view
        {
            padding: 0;
            margin: 0;
            border: 1px solid #333;
            font-family: "Verdana, Arial, Helvetica, sans-serif, Trebuchet MS";
            font-size: 0.9em;
        }
        
        .grid-view tr.header
        {
            color: white;
            background-color: #0CA6CA;
            height: 30px;
            vertical-align: middle;
            text-align: center;
            font-weight: bold;
            font-size: 20px;
        }
        
        .grid-view tr.normal
        {
            color: black;
            background-color: #FDC64E;
            height: 25px;
            vertical-align: middle;
            text-align: center;
        }
        
        .grid-view tr.alternate
        {
            color: black;
            background-color: #D59200;
            height: 25px;
            vertical-align: middle;
            text-align: center;
        }
        
        .grid-view tr.normal:hover, .grid-view tr.alternate:hover
        {
            background-color: white;
            color: black;
            font-weight: bold;
        }
        
        .grid_view_lnk_button
        {
            color: Black;
            text-decoration: none;
            font-size: large;
        }
        .lbl
        {
            font-family: Book Antiqua;
            font-size: 30px;
            font-weight: bold;
            color: Green;
            text-align: center;
            font-style: italic;
        }
        .hdtxt
        {
            font-family: Book Antiqua;
            font-size: large;
            font-weight: bold;
        }
        .FixedHeader
        {
            position: absolute;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">

        function checkDate() {
            var fromDate = "";
            var toDate = "";
            var date = ""
            var date1 = ""
            var month = "";
            var month1 = "";
            var year = "";
            var year1 = "";
            var empty = "";
            fromDate = document.getElementById('<%=txt_fromdate.ClientID%>').value;
            toDate = document.getElementById('<%=txt_todate.ClientID%>').value;

            date = fromDate.substring(0, 2);
            month = fromDate.substring(3, 5);
            year = fromDate.substring(6, 10);

            date1 = toDate.substring(0, 2);
            month1 = toDate.substring(3, 5);
            year1 = toDate.substring(6, 10);
            var today = new Date();
            //  var currentDate = today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1;
            var yyyy = today.getFullYear();
            if (dd < 10) { dd = '0' + dd }
            if (mm < 10) { mm = '0' + mm }
            var today = dd + '/' + mm + '/' + yyyy;

            if (year == year1) {
                if (month == month1) {
                    if (date == date1) {
                        empty = "";
                    }
                    else if (date < date1) {
                        empty = "";
                    }
                    else {
                        empty = "e";
                    }
                }
                else if (month < month1) {
                    empty = "";
                }
                else if (month > month1) {
                    empty = "e";
                }
            }
            else if (year < year1) {
                empty = "";
            }
            else if (year > year1) {
                empty = "e";
            }
            if (empty != "") {
                document.getElementById('<%=txt_fromdate.ClientID%>').value = today;
                document.getElementById('<%=txt_todate.ClientID%>').value = today;
                alert("To date should be greater than from date ");
                return false;
            }
        }
       

          
       
    </script>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Alumni Report</span>
            </div>
        </center>
    </div>
    <div>
        <center>
            <div id="maindiv" runat="server" class="maindivstyle" style="width: 1030px; height: auto">
                <div>
                    <table>
                        <tr>
                            <td>
                                <div>
                                    <table class="maintablestyle">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_collegename" Text="College" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                    OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_str1" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlstream" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlstream_OnSelectedIndexChanged"
                                                    CssClass="textbox  ddlheight" Style="width: 108px;">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                Batch
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UP_batch" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_batch" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                        <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                                            height: auto;">
                                                            <asp:CheckBox ID="cb_batch" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                OnCheckedChanged="cb_batch_OnCheckedChanged" />
                                                            <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_OnSelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="pce_batch" runat="server" TargetControlID="txt_batch"
                                                            PopupControlID="panel_batch" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UP_degree" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_degree" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                        <asp:Panel ID="panel_degree" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                            height: auto;">
                                                            <asp:CheckBox ID="cb_degree" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                OnCheckedChanged="cb_degree_OnCheckedChanged" />
                                                            <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_OnSelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="pce_degree" runat="server" TargetControlID="txt_degree"
                                                            PopupControlID="panel_degree" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="Up_dept" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_dept" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                        <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                            height: auto;">
                                                            <asp:CheckBox ID="cb_dept" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                OnCheckedChanged="cb_dept_OnCheckedChanged" />
                                                            <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_OnSelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="pce_dept" runat="server" TargetControlID="txt_dept"
                                                            PopupControlID="panel_dept" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                Section
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="Updp_sect" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_sect" runat="server" Style="height: 20px; width: 80px;" ReadOnly="true">--Select--</asp:TextBox>
                                                        <asp:Panel ID="panel_sect" runat="server" CssClass="multxtpanel" Style="width: 100px;
                                                            height: auto;">
                                                            <asp:CheckBox ID="cb_sect" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                OnCheckedChanged="cb_sect_OnCheckedChanged" />
                                                            <asp:CheckBoxList ID="cbl_sect" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sect_OnSelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_sect"
                                                            PopupControlID="panel_sect" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <div id="divdatewise" runat="server">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="cbdate" runat="server" Text="Date" onclick="return che(this)" />
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lbl_fromdate" runat="server" Text="From"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_fromdate" runat="server" Style="height: 20px; width: 75px;"
                                                                    onchange="return checkDate()"></asp:TextBox>
                                                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lbl_todate" runat="server" Text="To" Style="margin-left: 4px;"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_todate" runat="server" Style="height: 20px; width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                            <td>
                                                <asp:Button ID="btngo" runat="server" CssClass="textbox btn2" Text="Go" OnClick="btngo_Click" />
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
        <%-- <div id="divGrid" runat="server" visible="false"> 
                   
                        <asp:GridView ID="grid_Details" runat="server" AutoGenerateColumns="true" GridLines="Both" 
                              CssClass="grid-view" BackColor="WhiteSmoke" Style="width: auto;" OnRowDataBound="gdattrpt_OnRowDataBound">
                                              
                        </asp:GridView>
                    </div>--%>
    </div>
    <div id="divspread" runat="server" visible="false">
        <table>
            <tr>
                <td>
                    <FarPoint:FpSpread ID="spreadDet" runat="server" BorderStyle="Solid" BorderWidth="0px"
                        Width="980px" Style="overflow: auto; border: 0px solid #999999; border-radius: 10px;
                        background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </td>
            </tr>
            <tr>
                <td>
                    <center>
                        <div id="print" runat="server" visible="false">
                            <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                ForeColor="Red" Text="" Style="display: none;"></asp:Label>
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
                </td>
            </tr>
        </table>
    </div>
    </div>
</asp:Content>
