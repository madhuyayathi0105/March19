<%@ Page Title="" Language="C#" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="AdminKitReport.aspx.cs" Inherits="OfficeMOD_AdminKitReport" %>

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
                <span class="fontstyleheader" style="color: Green;">Admin kit Report</span>
            </div>
        </center>
    </div>
    <div>
        <center>
            <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
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
                                            <td>
                                                <asp:Label ID="lblusername" runat="server" Text="User Name" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    Font-Bold="True"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtusername" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    Font-Bold="True" Width="200px" MaxLength="50"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="Filterspace" runat="server" TargetControlID="txtusername"
                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="()}{][ ,/">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnuse" runat="server" Text="?" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    Font-Bold="True" OnClick="btnuse_Click" />
                                            </td>
                                            <td>
                                                <asp:Button ID="Button1" runat="server" CssClass="textbox btn2" Text="Go" OnClick="btngo_Click" />
                                            </td>
                                            <td>
                                                <%--Added by saranya on 11/04/2018--%>
                                                <asp:Button ID="BtnView" runat="server" CssClass="textbox btn2" Text="ViewDetails"
                                                    OnClick="btnView_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <asp:Panel ID="Puser" runat="server" Style="height: 100em; z-index: 1000; width: 100%;
                background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;">
                <center>
                    <br />
                    <br />
                    <br />
                    <div class="PopupHeaderrstud2" id="Div3" style="text-align: center; font-family: Book Antiqua;
                        font-size: medium; font-weight: bold; width: 540px; background-color: #F0F8FF;
                        border: 1px solid black;">
                        <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                            left: 200px">
                            Select User
                        </caption>
                        <br />
                        <table style="text-align: left">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkincludegroup" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                        Font-Bold="True" Text="Include Group User" AutoPostBack="true" OnCheckedChanged="chkincludegroup_Checked" />
                                </td>
                                <td>
                                    <asp:Label ID="lblusersearch" runat="server" Text="Group Id" Font-Size="Medium" Font-Names="Book Antiqua"
                                        Font-Bold="True"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtusersearch" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                        Font-Bold="True" Width="150px" AutoPostBack="true" OnTextChanged="chkincludegroup_Checked"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtusersearch"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="()}{][ ,/">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <FarPoint:FpSpread ID="Fpuser" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Height="300" Width="550" HorizontalScrollBarPolicy="Never"
                            VerticalScrollBarPolicy="AsNeeded">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <br />
                        <asp:Label ID="lblperrmsg" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True" ForeColor="Red"></asp:Label>
                        <br />
                        <asp:Button ID="btnuserok" runat="server" Text="Ok" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True" OnClick="btnuserok_Click" />
                        <asp:Button ID="btnuseexit" runat="server" Text="Exit" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True" OnClick="btnuseexit_Click" />
                    </div>
                </center>
            </asp:Panel>
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
        </center>
    </div>
</asp:Content>
