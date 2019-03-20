<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="HR_Finance.aspx.cs" Inherits="HR_Finance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <style type="text/css">
        .main
        {
            font-size: medium;
            font-weight: bold;
            width: 150px;
            font-family: "Book Antiqua";
        }
        .sty
        {
            height: 25px;
            width: 130px;
            font-size: medium;
            font-weight: 700;
            font-family: "Book Antiqua";
            margin-left: -83px;
        }
        .font14
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
        .style2
        {
            width: 100px;
        }
        .style4
        {
            width: 103px;
        }
        .style5
        {
            width: 216px;
        }
        .style6
        {
            width: 103px;
        }
        .style7
        {
            width: 103px;
        }
        .style8
        {
            width: 103px;
        }
        .style9
        {
            width: 103px;
        }
        .style11
        {
            width: 103px;
        }
        .style13
        {
            width: 103px;
            top: 25px;
        }
        .style14
        {
            height: 58px;
            margin-top: 65px;
        }
    </style>
    <script type="text/javascript">

        function validation() {

            var error = "";
            var dfirst = document.getElementById("<%=txt_dept.ClientID %>");
            var cate = document.getElementById("<%=txt_category.ClientID %>");
            var report = document.getElementById("<%=report.ClientID %>");
            var month = document.getElementById("<%=ddlmonth.ClientID %>");
            var month1 = document.getElementById("<%=ddltomonth.ClientID %>");
            var desig = document.getElementById("<%=txt_designation.ClientID %>");
            var go = document.getElementById("<%=butgo.ClientID %>");

            if (month.value == "---Select---") {
                error += "Please Select FromMonth \n";

            }
            if (month1.value == "---Select---") {
                error += "Please Select ToMonth \n";

            }

            if (dfirst.value == "---Select---") {
                error += "Please Select Department \n";

            }
            if (desig.value == "---Select---") {
                error += "Please Select Designation \n";

            }
            if (cate.value == "---Select---") {
                error += "Please Select category \n";

            }

            if (report.value == "---Select---") {
                error += "Please Select Report \n";

            }

            if (error != "") {
                alert(error);
                return false;
            }
            else {
                return true;
            }
        }

        function display() {

            document.getElementById('MainContent_lblerrorxl').innerHTML = "";

        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <br />
            <div style="width: 960px; height: 30px; background-color: Control;">
                <table style="margin-left: 0px; margin-top: 0px; position: absolute;">
                    <tr>
                        <td style="text-align: center;">
                            <span style="font-size: large; font-weight: bold; width: 352px; color: Green; top: 2px;
                                font-family: Book Antiqua; left: 300px; position: absolute;">HR Finance Year Report
                            </span>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <div class="maintablestyle" style="width: 960px; height: 72px; background-color: #0CA6CA;">
                <table style="margin-left: 29px; margin-top: 0px; position: absolute; width: 900px;
                    line-height: 30px;">
                    <tr>
                        <td>
                            <span style="font-size: medium; color: Black; font-weight: 500;">From Month & Year
                            </span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlmonth" AutoPostBack="true" runat="server" Style="font-weight: 700;
                                font-family: 'Book Antiqua'; font-size: medium;" Width="100px" Height="25px"
                                OnSelectedIndexChanged="ddlmonth_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlyear" AutoPostBack="true" runat="server" Style="font-weight: 700;
                                font-size: medium; font-family: 'Book Antiqua';" Width="80px" Height="25px" OnSelectedIndexChanged="ddlyear_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <span style="font-size: medium; color: Black; font-weight: 500;">To Month & Year
                            </span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddltomonth" AutoPostBack="true" runat="server" Style="font-weight: 700;
                                font-family: 'Book Antiqua'; font-size: medium;" Width="100px" Height="25px"
                                OnSelectedIndexChanged="ddltomonth_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlyear2" runat="server" AutoPostBack="true" Style="font-weight: 700;
                                font-size: medium; font-family: 'Book Antiqua';" Width="80px" Height="25px" OnSelectedIndexChanged="ddlyear22">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <span style="font-size: medium; color: Black; font-weight: 500;">Department</span>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_dept" runat="server" Font-Bold="true" CssClass="Dropdown_Txt_Box"
                                        Font-Names="Book Antiqua" Style="top: 6px; height: 23px; left: 748px; position: absolute;"
                                        Width="143px" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel_Dept" runat="server" CssClass="multxtpanel" Style="font-family: 'Book Antiqua';
                                        position: absolute;" Font-Bold="True" Font-Names="Book Antiqua" Height="300px"
                                        Width="220px">
                                        <asp:CheckBox ID="chk_deptall" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnCheckedChanged="chk_deptall_CheckedChanged" Text="Select All"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chkls_dept" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            OnSelectedIndexChanged="chkls_dept_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_dept"
                                        PopupControlID="Panel_Dept" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span style="font-size: medium; color: Black; font-weight: 500;">Designation</span>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel_Designation" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_designation" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                        Style="top: 40px; left: 152px; position: absolute; height: 23px; width: 150px;"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="panel_Designation" runat="server" CssClass="multxtpanel" Height="300px"
                                        Width="201px">
                                        <asp:CheckBox ID="cb_Designation" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_Designation_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_Designation" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cbl_Designation_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_designation"
                                        PopupControlID="panel_Designation" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <span style="font-size: medium; color: Black; font-weight: 500;">Category</span>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_category" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                        Style="top: 40px; left: 471px; position: absolute; height: 23px; width: 150px;"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="panelcategory" runat="server" CssClass="multxtpanel" Height="300px"
                                        Width="201px">
                                        <asp:CheckBox ID="cbcategory" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnCheckedChanged="cbcategory_CheckedChanged" Text="Select All"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cblcategory" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblcategory_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_category"
                                        PopupControlID="panelcategory" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <span style="font-size: medium; color: Black; font-weight: 500;">Report</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="report" runat="server" AutoPostBack="true" OnSelectedIndexChanged="report_OnSelectedIndexChanged"
                                Style="font-weight: 700; font-family: 'Book Antiqua'; font-size: medium;" Width="100px"
                                Height="25px">
                                <asp:ListItem>---Select---</asp:ListItem>
                                <asp:ListItem>Gross Salary Report</asp:ListItem>
                                <asp:ListItem>Income Tax Report</asp:ListItem>
                                <asp:ListItem>Pf Report</asp:ListItem>
                                <asp:ListItem>Education Deduction Report</asp:ListItem>
                                <asp:ListItem>Other Deduction Report</asp:ListItem>
                                <asp:ListItem>Hostel Deduction Report</asp:ListItem>
                                <asp:ListItem>Net Salary Report</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="butgo" runat="server" Text="Go" Font-Bold="true" Width="41px" CssClass="style48"
                                Height="27px" Font-Size="Medium" ForeColor="Black" BackColor="DarkGray" Font-Names="Book Antiqua"
                                OnClientClick="return validation()" OnClick="go_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <br />
            <asp:Label ID="lblerrormsg" runat="server" Text="" Width="426px" Style="position: absolute;
                left: -62px; top: 288px;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                Visible="true" ForeColor="#FF3300"></asp:Label>
            <center>
                <FarPoint:FpSpread ID="fpspread" runat="server" BorderColor="Black" BorderStyle="Solid"
                    Visible="false" BorderWidth="1px" VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never"
                    CssClass="font14" ShowHeaderSelection="false">
                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                        ButtonShadowColor="ControlDark">
                    </CommandBar>
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
            </center>
        </center>
        <br />
        <center>
            <asp:Label ID="lblerrorxl" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Visible="false" ForeColor="#FF3300"></asp:Label>
            <asp:Label ID="lblexportxl" runat="server" Visible="false" Width="100px" Height="20px"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Text="Report Name"
                ForeColor="Black"></asp:Label>
            <asp:TextBox ID="txtexcell" runat="server" onkeypress="display()" Height="20px" Visible="false"
                Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcell"
                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|}{][':;?><,."
                InvalidChars="/\">
            </asp:FilteredTextBoxExtender>
            <asp:Button ID="butexcel" runat="server" Visible="false" Text="Export Excel" Width="105px"
                Height="31px" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="butexcel_Click" />
            <asp:Button ID="butpdf" runat="server" Text="Print" Visible="false" Font-Names="Book Antiqua"
                Font-Size="Medium" Font-Bold="true" Width="74px" Height="31px" OnClick="butpdf_Click" />
            <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
        </center>
        <br />
    </div>
</asp:Content>
