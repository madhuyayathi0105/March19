<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="MonthlyCummulativeSalary.aspx.cs" Inherits="MonthlyCummulativeSalary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html>
    <script type="text/javascript">

        function validation() {
            var err = "";

            var Txt_dep = document.getElementById("<%=txtallowance.ClientID %>");
            var Txt_catgry = document.getElementById("<%=txtdeduction.ClientID %>");



            if (Txt_dep.value == "--Select--") {
                err += "Please Select the Allowance \n";

            }

            if (Txt_catgry.value == "--Select--") {

                //  alert("Please Select Month");
                err += "Please Select the Deduction \n";

            }

            if (err != "") {
                alert(err);
                return false;
            }
            else {
                return true;
            }


        }
  
  
    </script>
    <style type="text/css">
        .cpHeader
        {
            color: white;
            background-color: #719DDB;
            font-size: 12px;
            cursor: pointer;
            padding: 4px;
            font-style: normal;
            font-variant: normal;
            font-weight: bold;
            line-height: normal;
            font-family: "auto Trebuchet MS" , Verdana;
        }
        .cpBody
        {
            background-color: #DCE4F9;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
        }
        
        .cpimage
        {
            float: right;
            vertical-align: middle;
            background-color: transparent;
        }
        
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        
        .HellowWorldPopup
        {
            min-width: 600px;
            min-height: 400px;
            background: white;
        }
    </style>
    <body oncontextmenu="return false">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <br />
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green">Monthly Salary Statement</span>
                </div>
            </center>
            <br />
            <center>
                <div>
                    <asp:Panel ID="pnldemond" runat="server" BorderColor="Black" BorderWidth="2px" Width="1089px"
                        BackColor="#0CA6CA">
                        <table style="height: 51px">
                            <tr>
                                <td>
                                    <asp:Label ID="Label13" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="From Year" Width="76px"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="cblbatchyear" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="cblbatchyear_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                 <td>
                                    <asp:Label ID="lbl_toyear" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="To Year" Width="58px"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_toyear" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlToYear_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td class="style405">
                                    <asp:Label ID="lblmonth" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Month"></asp:Label>
                                </td>
                                <td class="style315">
                                    <asp:DropDownList ID="cblmonthfrom" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnSelectedIndexChanged="cblmonthfrom_SelectedIndexChanged"
                                        AutoPostBack="True">
                                        <asp:ListItem Value="0">All</asp:ListItem>
                                        <asp:ListItem Value="1">January</asp:ListItem>
                                        <asp:ListItem Value="2">February</asp:ListItem>
                                        <asp:ListItem Value="3">March</asp:ListItem>
                                        <asp:ListItem Value="4">April</asp:ListItem>
                                        <asp:ListItem Value="5">May</asp:ListItem>
                                        <asp:ListItem Value="6">June</asp:ListItem>
                                        <asp:ListItem Value="7">July</asp:ListItem>
                                        <asp:ListItem Value="8">August</asp:ListItem>
                                        <asp:ListItem Value="9">September</asp:ListItem>
                                        <asp:ListItem Value="10">October</asp:ListItem>
                                        <asp:ListItem Value="11">November</asp:ListItem>
                                        <asp:ListItem Value="12">December</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblto" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Text="To" Visible="False"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="cbotomonth" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Visible="False">
                                        <asp:ListItem Value="0">All</asp:ListItem>
                                        <asp:ListItem Value="1">January</asp:ListItem>
                                        <asp:ListItem Value="2">February</asp:ListItem>
                                        <asp:ListItem Value="3">March</asp:ListItem>
                                        <asp:ListItem Value="4">April</asp:ListItem>
                                        <asp:ListItem Value="5">May</asp:ListItem>
                                        <asp:ListItem Value="6">June</asp:ListItem>
                                        <asp:ListItem Value="7">July</asp:ListItem>
                                        <asp:ListItem Value="8">August</asp:ListItem>
                                        <asp:ListItem Value="9">September</asp:ListItem>
                                        <asp:ListItem Value="10">October</asp:ListItem>
                                        <asp:ListItem Value="11">November</asp:ListItem>
                                        <asp:ListItem Value="12">December</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbldept" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Department"></asp:Label>
                                </td>
                                <td>
                                    <div id="castediv" runat="server">
                                        <asp:TextBox ID="tbseattype" runat="server" Height="20px" ReadOnly="true" Width="135px"
                                            OnTextChanged="tbseattype_TextChanged" Style="font-family: 'Book Antiqua'" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                        <br />
                                    </div>
                                    <asp:Panel ID="pseattype" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="2px" Height="300px" ScrollBars="Vertical" Width="350px">
                                        <asp:CheckBox ID="chkselect" runat="server" AutoPostBack="True" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="chkselect_CheckedChanged"
                                            Text="Select All" />
                                        <asp:CheckBoxList ID="cbldepttype" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Width="258px" OnSelectedIndexChanged="cbldepttype_SelectedIndexChanged" Height="102px"
                                            Font-Bold="True" Font-Names="Book Antiqua">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="ddeseattype" runat="server" TargetControlID="tbseattype"
                                        PopupControlID="pseattype" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </td>
                                <td>
                                    <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Category"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbblood" runat="server" Height="20px" OnTextChanged="tbblood_TextChanged"
                                        ReadOnly="true" Width="109px" Style="font-family: 'Book Antiqua'; margin-top: 0px;"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                    <br />
                                    <asp:Panel ID="pblood" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="2px" Height="200px" ScrollBars="Auto" Width="200px" Style="font-family: 'Book Antiqua'">
                                        <asp:CheckBox ID="chkcategory" runat="server" AutoPostBack="True" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="chkcategory_CheckedChanged"
                                            Text="Select All" Checked="True" />
                                        <asp:CheckBoxList ID="cblcategory" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Width="158px" OnSelectedIndexChanged="cblcategory_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                            Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="ddeblood" runat="server" PopupControlID="pblood" Enabled="true"
                                        TargetControlID="tbblood" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    <%--DynamicServicePath=""--%>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btngo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClientClick="return validation()" Text="GO" OnClick="btndemond_go_Click" />
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Allowance"></asp:Label>
                                </td>
                                <td>
                                    <div id="Div1" runat="server" class="linkbtn">
                                        <asp:TextBox ID="txtallowance" runat="server" Height="20px" ReadOnly="true" Width="135px"
                                            OnTextChanged="tbseattype_TextChanged" Style="font-family: 'Book Antiqua'" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                        <br />
                                    </div>
                                    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                    <asp:Panel ID="Pallowance" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="2px" ScrollBars="Vertical">
                                        <asp:CheckBox ID="chkallowance" runat="server" AutoPostBack="True" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="cblallowance_CheckedChanged"
                                            Text="Select All" Checked="True" />
                                        <asp:CheckBoxList ID="cblallowance" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Width="167px" OnSelectedIndexChanged="cblallowance_SelectedIndexChanged" Font-Bold="True"
                                            Font-Names="Book Antiqua">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="DropDownExtender1" runat="server" PopupControlID="Pallowance"
                                        Enabled="true" TargetControlID="txtallowance" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    <%--DynamicServicePath=""--%>
                                </td>
                                <td>
                                    <asp:Label ID="lblde" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Text="Deduction"></asp:Label>
                                </td>
                                <td>
                                    <div id="Div2" runat="server" class="linkbtn">
                                        <asp:TextBox ID="txtdeduction" runat="server" Height="20px" ReadOnly="true" Width="135px"
                                            Style="font-family: 'Book Antiqua'; margin-bottom: 0px;" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium">---Select---</asp:TextBox>
                                        <br />
                                    </div>
                                    <asp:PlaceHolder ID="PlaceHolderded" runat="server"></asp:PlaceHolder>
                                    <asp:Panel ID="Pdeduction" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="2px" ScrollBars="Vertical" Height="200px" Width="200px">
                                        <asp:CheckBox ID="Chkdeduction" runat="server" AutoPostBack="True" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="Chkdeduction_CheckedChanged"
                                            Text="Select All" Checked="True" />
                                        <asp:CheckBoxList ID="cbldeduction" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Width="167px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cbldeduction_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="DropDownExtender2" runat="server" PopupControlID="Pdeduction"
                                        Enabled="true" TargetControlID="txtdeduction" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    <%--DynamicServicePath=""--%>
                                </td>
                                <td>
                                    <asp:CheckBox ID="cbinculdeRelive" CssClass="gridCB" Text="Relive Staff" Font-Names="Book Antiqua"
                                        Font-Size="Medium" runat="server" Visible="true"></asp:CheckBox>
                                </td>
                                <td>
                                    <asp:CheckBox ID="cbSelect" CssClass="gridCB" AutoPostBack="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" runat="server" Visible="true" OnCheckedChanged="cbSelect_CheckedChanged">
                                    </asp:CheckBox>
                                </td>
                                <td>
                                    <asp:Label ID="ldlstaff" runat="server" Font-Bold="True" Visible="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Staff Wise"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblstafnam" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Visible="false" Text="Staff Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlstfnam" runat="server" Style="display: none;" Width="260px"
                                        Font-Bold="true" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlstfnam_SelectedIndexChanged"
                                        Font-Names="Book Antiqua" Font-Size="Medium">
                                    </asp:DropDownList>
                                    <%--<asp:UpdatePanel ID="updstfname" runat="server" Visible="false">
                                        <ContentTemplate>--%>
                                    <asp:TextBox ID="txtstfname" runat="server" Visible="false" Height="20px" ReadOnly="true"
                                        Width="135px" Style="font-family: 'Book Antiqua'; margin-bottom: 0px;" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="pnlstfname" runat="server" Visible="false" BackColor="White" BorderColor="Black"
                                        BorderStyle="Solid" BorderWidth="2px" ScrollBars="Vertical" Height="250px" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cbstfname" runat="server" AutoPostBack="True" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="cbstfname_CheckedChanged"
                                            Text="Select All" Checked="True" />
                                        <asp:CheckBoxList ID="cblstfname" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Width="167px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblstfname_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popstfname" runat="server" PopupControlID="pnlstfname"
                                        Enabled="true" TargetControlID="txtstfname" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    <%--</ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </center>
            <br />
            <center>
                <asp:Label ID="lblnorec" runat="server" Text="There are no Records Found" ForeColor="Red"
                    Visible="False" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
            </center>
            <br />
            <center>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <FarPoint:FpSpread ID="fpsalarydemond" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Height="295px" Width="1000px" ShowHeaderSelection="false">
                        <CommandBar BackColor="Control" ButtonType="PushButton" ButtonFaceColor="Control"
                            ButtonHighlightColor="ControlLightLight" ButtonShadowColor="ControlDark">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Label ID="lblexcel" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium"></asp:Label>
            <asp:TextBox ID="txtxl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium"></asp:TextBox>
            <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" OnClick="btnxl_Click" />
            <asp:FilteredTextBoxExtender ID="Filterspace" runat="server" TargetControlID="txtxl"
                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(){}[] .">
            </asp:FilteredTextBoxExtender>
            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
            <asp:Button ID="btnsal" runat="server" Text="Salary Certificate" OnClick="btnsal_Click"
                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
            </center>
            
                <br />
        </div>
    </body>
    </html>
</asp:Content>
