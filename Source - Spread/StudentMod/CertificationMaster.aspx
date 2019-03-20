<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master" AutoEventWireup="true" CodeFile="CertificationMaster.aspx.cs" Inherits="CertificationMaster" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <script type="text/javascript">
        function display(x) {
            x.style.borderColor = "#c4c4c4";

        }
        function Check() {
            var empty = true;
            var txtval = document.getElementById('<%=txtcertf.ClientID %>').value;
            if (txtval == "") {
                txtval = document.getElementById('<%=txtcertf.ClientID %>');
                txtval.style.borderColor = 'Red';
                empty = false;
            }
            if (empty == true) {
                return true;
            }
            else {
                return false;
            }
        }
        function ClearPrint1() {
            var id = document.getElementById('<%=lbl_norec.ClientID%>');
            id.innerHTML = "";
            id.visible = false;
        }

      
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Certificate Master</span></div>
           
        </center>
    </div>
    <div>
        <center>
            <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                <center>
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lblclg" Text="College" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlclg" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    OnSelectedIndexChanged="ddlclg_SelectedIndexChanged" Width="217px" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Course Type
                            </td>
                            <td>
                                <asp:DropDownList ID="ddledu" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddledu_OnSelectedIndexChanged"
                                    Style="width: 67px; height: 30px;" CssClass="textbox3 textbox1">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblDeg" runat="server" Text="Degree"></asp:Label>
                            </td>
                            <td>
                                <%--<asp:DropDownList ID="ddldegree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddldegree_OnSelectedIndexChanged"
                                    Style="width: 110px; height: 30px;" CssClass="textbox3 textbox1">
                                </asp:DropDownList>--%>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtdegree" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnldegree" runat="server" CssClass="multxtpanel multxtpanleheight"
                                            Style="width: 126px; height: 120px;">
                                            <asp:CheckBox ID="cbdegree" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cbdegree_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbldegree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbldegree_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtdegree"
                                            PopupControlID="pnldegree" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td id="tdept" runat="server" visible="false">
                                Department
                            </td>
                            <td id="tfffdept" runat="server" visible="false">
                                <%-- <asp:DropDownList ID="ddldept" runat="server" AutoPostBack="true" Style="width: 229px;
                                    height: 30px;" CssClass="textbox3 textbox1">
                                </asp:DropDownList>--%>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtdept" runat="server" Style="height: 20px; width: 225px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnldept" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 228px;
                                            height: 120px;">
                                            <asp:CheckBox ID="cbdept" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cbdept_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbldept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbldept_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtdept"
                                            PopupControlID="pnldept" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Certificate Name
                            </td>
                            <td>
                                <%-- <asp:DropDownList ID="ddlcerticate" runat="server" Height="35px" CssClass="textbox textbox1 ddlstyle ddlheight4">
                                </asp:DropDownList>--%>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtctfname" runat="server" Style="height: 20px; width: 225px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 228px;
                                            height: 120px;">
                                            <asp:CheckBox ID="cbctf" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cbctf_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cblctf" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblctf_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtctfname"
                                            PopupControlID="Panel1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                Category Type
                            </td>
                            <td>
                                <%-- <asp:DropDownList ID="ddlcerticate" runat="server" Height="35px" CssClass="textbox textbox1 ddlstyle ddlheight4">
                                </asp:DropDownList>--%>
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_catbase" runat="server" Style="height: 20px; width: 225px;"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 228px;
                                            height: 120px;">
                                            <asp:CheckBox ID="cb_catbase" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_catbaseCheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_catbase" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_catbase_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_catbase"
                                            PopupControlID="Panel4" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Button ID="btngo" runat="server" Text="Go" Font-Size="Medium" Height="33px"
                                    Width="71px" Font-Names="Book Antiqua" CssClass="textbox textbox1" OnClick="btngo_Click" />
                            </td>
                            <td colspan="2">
                                <asp:Button ID="btnaddnew" runat="server" Text="AddNew" Font-Size="Medium" Height="33px"
                                    Width="88px" Font-Names="Book Antiqua" CssClass="textbox textbox1" OnClick="btnaddnew_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <table>
                        <tr>
                            <td>
                                <div id="divspread" runat="server" visible="false" style="width: 961px; overflow: auto;
                                    background-color: White; border-radius: 10px;">
                                    <FarPoint:FpSpread ID="FpSpreadbase" runat="server" Visible="true" BorderWidth="5px"
                                        BorderStyle="Groove" BorderColor="#0CA6CA" Width="930px" Style="overflow: auto;
                                        border: 0px solid #999999; border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                        class="spreadborder" OnCellClick="FpSpreadbase_CellClick" OnPreRender="FpSpreadbase_SelectedIndexChanged">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <asp:Label ID="lbl_norec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False">
                    </asp:Label>
                    <div id="div_report" runat="server" visible="false">
                        <center>
                            <asp:Label ID="lbl_reportname" runat="server" Text="Report Name" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="false" OnTextChanged="txtexcelname_TextChanged"
                                CssClass="textbox textbox1 txtheight5" onkeypress="return ClearPrint1()"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btn_Excel" runat="server" Text="Export To Excel" Width="150px" CssClass="textbox textbox1 btn2"
                                AutoPostBack="true" Font-Names="Book Antiqua" OnClick="btnExcel_Click" Font-Bold="true" />
                            <asp:Button ID="btn_printmaster" Font-Names="Book Antiqua" runat="server" Text="Print"
                                CssClass="textbox textbox1 btn2" AutoPostBack="true" OnClick="btn_printmaster_Click"
                                Font-Bold="true" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="divaddnew" runat="server" visible="false" class="popupstyle popupheight1 ">
                <asp:ImageButton ID="imgaddmew" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 30px; margin-left: 460px;"
                    OnClick="imgaddmew_Click" />
                <br />
                <br />
                <div style="background-color: White; height: 500px; width: 950px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <center>
                        <span class="fontstyleheader" style="color: Green;">Add Certificate Details</span>
                    </center>
                    <br />
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="Label1" Text="College" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="addnewddlclg" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    OnSelectedIndexChanged="addnewddlclg_SelectedIndexChanged" Width="180px" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Course Type
                            </td>
                            <td>
                                <asp:DropDownList ID="addnewddledu" runat="server" AutoPostBack="true" OnSelectedIndexChanged="addnewddledu_OnSelectedIndexChanged"
                                    Style="width: 68px; height: 30px;" CssClass="textbox3 textbox1">
                                </asp:DropDownList>
                            </td>
                            <td>
                               <asp:Label ID="lblAddDeg" runat="server" Text="Degree"></asp:Label> 
                            </td>
                            <td>
                                <%--<asp:DropDownList ID="addnewddldegree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="addnewddldegree_OnSelectedIndexChanged"
                                    Style="width: 110px; height: 30px;" CssClass="textbox3 textbox1">
                                </asp:DropDownList>--%>
                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="addnewtxtdegree" runat="server" Style="height: 20px; width: 124px;"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="addnewpnldegree" runat="server" CssClass="multxtpanel multxtpanleheight"
                                            Style="width: 126px; height: 120px;">
                                            <asp:CheckBox ID="addnewcbdegree" runat="server" Width="100px" Text="Select All"
                                                AutoPostBack="True" OnCheckedChanged="addnewcbdegree_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="addnewcbldegree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="addnewcbldegree_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="addnewtxtdegree"
                                            PopupControlID="addnewpnldegree" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td id="tddept" runat="server" visible="false">
                                Department
                            </td>
                            <td id="tddeptcb" runat="server" visible="false">
                                <%--  <asp:DropDownList ID="addnewddldept" runat="server" AutoPostBack="true" Style="width: 200px;
                                    height: 30px;" CssClass="textbox3 textbox1">
                                </asp:DropDownList>--%>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="addnewtxtdept" runat="server" Style="height: 20px; width: 181px;"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="addnewpnldept" runat="server" CssClass="multxtpanel multxtpanleheight"
                                            Style="width: 185px; height: 120px;">
                                            <asp:CheckBox ID="addnewcbdept" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="addnewcbdept_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="addnewcbldept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="addnewcbldept_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="addnewtxtdept"
                                            PopupControlID="addnewpnldept" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Certificate Name
                            </td>
                            <td colspan="2">
                                <%--<asp:Button ID="btn_plus" runat="server" Text="+" CssClass="textbox textbox1" Font-Bold="true"
                                    Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus_Click" Style="height: 30px;
                                    width: 35px; margin-left: 10px; margin-top: -37px;" />--%>
                                <%-- <asp:Button ID="btn_plus" runat="server" Text="+" CssClass="textbox textbox1" Font-Bold="true"
                                    Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus_Click" Style="height: 30px;
                                    width: 35px;" />--%>
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btn_plus" runat="server" Text="+" CssClass="textbox textbox1" Font-Bold="true"
                                            Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus_Click" Style="height: 30px;
                                            width: 35px;" />
                                        <asp:TextBox ID="addnewtxtctf" runat="server" Style="height: 20px; width: 150px;"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 228px;
                                            height: 120px;">
                                            <asp:CheckBox ID="addnewcbctf" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="addnewcbctf_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="addnewcblctf" runat="server" AutoPostBack="True" OnSelectedIndexChanged="addnewcblctf_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="addnewtxtctf"
                                            PopupControlID="Panel2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                        <asp:Button ID="btn_minus" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                            Style="height: 30px; width: 35px;" Font-Names="Book Antiqua" CssClass="textbox textbox1"
                                            OnClick="btnminus_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                    <ContentTemplate>
                                        <div id="div3" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                            left: 0px;">
                                            <center>
                                                <div id="Div4" runat="server" class="table" style="background-color: White; height: 108px;
                                                    width: 225px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 148px;
                                                    border-radius: 10px;">
                                                    <asp:Label ID="lbl_cnfmshow" runat="server" Text="Are You Want To Delete This Record?"
                                                        ForeColor="Red"></asp:Label>
                                                    <br />
                                                    <br />
                                                    <asp:Button ID="btn_cnfmok" runat="server" Text="Ok" CssClass="textbox btn2 textbox1"
                                                        Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btn_cnfmok_Click" />
                                                    <asp:Button ID="btn_cnfromcancel" runat="server" Text="Cancel" Font-Bold="true" Font-Size="Medium"
                                                        Font-Names="Book Antiqua" CssClass="textbox btn2 textbox1" OnClick="btn_cnfromcancel_click" />
                                                </div>
                                            </center>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <%-- <asp:Button ID="btn_minus" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                    Style="height: 30px; width: 35px;" Font-Names="Book Antiqua" CssClass="textbox textbox1"
                                    OnClick="btnminus_Click" />--%>
                                <%-- <asp:Button ID="btn_minus" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                    Style="height: 30px; width: 35px; margin-left: 272px; margin-top: -37px;" Font-Names="Book Antiqua"
                                    CssClass="textbox textbox1" OnClick="btnminus_Click" />--%>
                            </td>
                            <td>
                                Category Type
                            </td>
                            <td colspan="3">
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btn_plusnew" Visible="false" runat="server" Text="+" CssClass="textbox textbox1"
                                            Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btn_plusnew_Click"
                                            Style="height: 30px; width: 35px;" />
                                        <asp:TextBox ID="txt_certtype" runat="server" Style="height: 20px; width: 150px;"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 228px;
                                            height: 120px;">
                                            <asp:CheckBox ID="cb_certtype" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_certtype_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_certtype" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cb_certtype_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_certtype"
                                            PopupControlID="Panel3" Position="Bottom">
                                        </asp:PopupControlExtender>
                                        <asp:Button ID="btn_minusnew" Visible="false" runat="server" Text="-" Font-Bold="true"
                                            Font-Size="Medium" Style="height: 30px; width: 35px;" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" OnClick="btn_minusnew_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Button ID="addnewbtngo" runat="server" Text="Go" Font-Size="Medium" Height="33px"
                                    Width="71px" Font-Names="Book Antiqua" CssClass="textbox textbox1" OnClick="addnewbtngo_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <div id="divaddspread" runat="server" visible="false" style="width: 850px; overflow: auto;
                                        background-color: White; border-radius: 10px;">
                                        <FarPoint:FpSpread ID="FpSpreadadd" runat="server" Visible="true" BorderWidth="5px"
                                            BorderStyle="Groove" BorderColor="#0CA6CA" Style="overflow: auto; border: 0px solid #999999;
                                            border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                            class="spreadborder" OnButtonCommand="FpSpreadadd_OnButtonCommand">
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <center>
                                            <asp:Button ID="btnsave" runat="server" Text="Save" Font-Size="Medium" Visible="false"
                                                Height="33px" Width="71px" Font-Names="Book Antiqua" CssClass="textbox textbox1"
                                                OnClick="btnsave_Click" />
                                        </center>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </div>
        </center>
        <center>
            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                <ContentTemplate>
                    <div id="divcertf" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="panel_description" runat="server" class="table" style="background-color: White;
                                height: auto; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                margin-top: 148px; border-radius: 10px;">
                                <table>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblctfname" runat="server" Text="Certificate Name"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:TextBox ID="txtcertf" runat="server" Height="25px" onkeypress="return display(this)"
                                                CssClass="textbox textbox1" Width="200px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <br />
                                            <asp:Button ID="btnsavecertf" runat="server" Visible="true" CssClass="textbox textbox1"
                                                Text="Add" Style="width: 60px; height: 28px;" OnClick="btnsavecertf_Click" OnClientClick="return Check()" />
                                            <asp:Button ID="btnexistcertf" runat="server" Visible="true" CssClass="textbox textbox1"
                                                Text="Exit" Style="width: 60px; height: 28px;" OnClick="btnexistcertf_Click" />
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </center>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
        <center>
            <div id="popview" runat="server" class="popupstyle popupheight1" visible="false">
                <asp:ImageButton ID="imagebtnpop1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 8px; margin-left: 223px;"
                    OnClick="btn_popclose_Click" />
                <br />
                <div style="background-color: White; height: 314px; width: 467px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <span class="fontstyleheader" style="color: Green;">Update And Delete</span></center>
                    <br />
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_UDcertificatename" runat="server" Text="Certificate Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_UDcertificatename" runat="server" CssClass="textbox textbox1 txtheight5"
                                        Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_UDcategorytype" runat="server" Text="Category Type"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_UDcategorytype" runat="server" CssClass="ddlheight5 textbox textbox1">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_UDcertificatecopy" runat="server" Text="Certificate Copy"></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:CheckBox ID="cb_UDorginal" runat="server" Text="Original" />
                                    <asp:CheckBox ID="cb_UDduplicate" runat="server" Text="Duplicate" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_UDdate" runat="server" Text="Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_UDdate" runat="server" CssClass="textbox textbox1 txtheight"
                                                Enabled="false"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_UDdate" runat="server"
                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <asp:Button ID="btn_UDupdate" runat="server" Text="Update" CssClass="textbox1 textbox btn2" OnClick="btn_UDupdate_Click" />
                        <asp:Button ID="btn_UDdelete" runat="server" Text="Delete" CssClass="textbox1 textbox btn2" OnClick="btn_UDdelete_Click" />
                        <asp:Button ID="btn_UDexit" runat="server" Text="Exit" CssClass="textbox1 textbox btn2" OnClick="btn_UDexit_Click" />
                    </center>
                </div>
            </div>
        </center>
        <center>
            <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_alert" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        <asp:Label ID="Label2" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_errorclose" CssClass="textbox textbox1" Style="height: 28px;
                                                width: 65px;" OnClick="btn_errorclose_Click" Text="Ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                <ContentTemplate>
                    <div id="Div1" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblerr" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                <asp:Label ID="Label4" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btnclose" CssClass="textbox textbox1" Style="height: 28px; width: 65px;"
                                                        OnClick="btnclose_Click" Text="Ok" runat="server" />
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
    </div>
</asp:Content>

