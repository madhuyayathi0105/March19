<%@ Page Title="Security Settings" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="NewSecuritySettings.aspx.cs" Inherits="NewSecuritySettings" EnableEventValidation="false" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <link href="Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        function CheckBoxListSelectMblAppTab(cbControl) {
            var chkBoxList = document.getElementById('<%=cbl_appTab.ClientID %>');
            var chkBoxCount = chkBoxList.getElementsByTagName("input");
            if (cbControl.checked == true) {
                for (var i = 0; i < chkBoxCount.length; i++) {
                    chkBoxCount[i].checked = true;
                }
            }
            else {
                for (var i = 0; i < chkBoxCount.length; i++) {
                    chkBoxCount[i].checked = false;
                }
            }
        }

        function CheckBoxListSelectStudentMblAppTab(cbControl) {
            var chkBoxList = document.getElementById('<%=cbl_Student_AppTab.ClientID %>');
            var chkBoxCount = chkBoxList.getElementsByTagName("input");
            if (cbControl.checked == true) {
                for (var i = 0; i < chkBoxCount.length; i++) {
                    chkBoxCount[i].checked = true;
                }
            }
            else {
                for (var i = 0; i < chkBoxCount.length; i++) {
                    chkBoxCount[i].checked = false;
                }
            }
        }

        function cbl_FN_AN_selectAll(cbControl) {
            var txtBox = document.getElementById('<%=txt_FN_AN.ClientID %>');
            var chkBoxList = document.getElementById('<%=cbl_FN_AN.ClientID %>');
            var chkBoxCount = chkBoxList.getElementsByTagName("input");
            var cnt = 0;
            if (cbControl.checked == true) {
                for (var i = 0; i < chkBoxCount.length; i++) {
                    cnt++;
                    chkBoxCount[i].checked = true;
                }
                txtBox.value = "Hour(" + cnt + ")";
            }
            else {
                for (var i = 0; i < chkBoxCount.length; i++) {
                    chkBoxCount[i].checked = false;
                }
                txtBox.value = "--Select--";
            }
        }

        function permissionmint() {

            var txtBox = document.getElementById('<%=txtExtentPermission.ClientID %>');
            var i;
            if (parseInt(txtBox.value)) {
                i = txtBox.value;
                if (i >= 0 && i <= 120)
                    return;
                else
                    txtBox.value = "";
            }
        }

    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitEvents);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <center>
        <div style="width: 980px;">
            <center>
                <asp:Panel ID="Panel1" runat="server" Style="background: #0095E8; width: 980px">
                    <center>
                        <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="White" Text="Security Settings"></asp:Label>
                    </center>
                </asp:Panel>
                <asp:Panel ID="Panel2" runat="server">
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButton ID="rdb_ind" runat="server" Text="Individual" Font-Bold="True" Font-Names="Book Antiqua"
                                    Checked="true" GroupName="Report" AutoPostBack="True" OnCheckedChanged="rdb_ind_CheckedChanged" />
                            </td>
                            <td>
                                <asp:RadioButton ID="rdb_grp" runat="server" Text="Group" Font-Bold="True" Font-Names="Book Antiqua"
                                    GroupName="Report" AutoPostBack="True" OnCheckedChanged="rdb_grp_CheckedChanged" />
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text=" Select the College " Font-Bold="True"
                                    Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollege" Width="200px" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text=" Select the User " Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UPDuser" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtuser" runat="server" Height="19px" ReadOnly="true" Font-Bold="True"
                                            ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Width="113px"
                                            OnTextChanged="txtuser_TextChanged" CssClass="Dropdown_Txt_Box">- - Select - -</asp:TextBox>
                                        <asp:Panel ID="puser" runat="server" CssClass=" multxtpanel multxtpanleheight" Height="273px"
                                            ScrollBars="Vertical" Width="185px">
                                            <asp:CheckBox ID="chk_alluser" runat="server" Text="SelectAll" AutoPostBack="true"
                                                Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                                OnCheckedChanged="chk_alluser_CheckedChanged" />
                                            <asp:CheckBoxList ID="ddluser" runat="server" Font-Size="Small" AutoPostBack="True"
                                                Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Width="184px" Height="50px"
                                                OnSelectedIndexChanged="ddluser_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtuser"
                                            PopupControlID="puser" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:LinkButton ID="btnGo" runat="server" Font-Bold="True" Width="50px" Text="Go"
                                    Font-Names="Book Antiqua" Font-Size="Small" ForeColor="Black" OnClick="btnGo_Click"
                                    CausesValidation="False" Style="display: inline-block; color: Black; font-family: Book Antiqua;
                                    font-size: small; font-weight: bold; width: 50px; text-align: center; border-style: solid;
                                    border-width: 1px; border-color: gray; background-color: none; border-radius: 4px 4px 4px 4px;
                                    text-decoration: none;"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Label ID="errmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Small" ForeColor="Red"></asp:Label>
                <br />
            </center>
            <center>
                <table style="background: #0095E8; width: 980px;">
                    <tr>
                        <td>
                            <asp:Button ID="btnAttendance_1" runat="server" Text="Academic" CssClass="textbox"
                                BackColor="#A537D1" ForeColor="White" OnClick="btnAttendance_1_Click" />
                            <asp:Button ID="btnCOE_2" runat="server" Text="COE" CssClass="textbox" BackColor="#A537D1"
                                ForeColor="White" OnClick="btnCOE_2_Click" />
                            <asp:Button ID="btnFinancePrint_3" runat="server" Text="Finance" CssClass="textbox"
                                BackColor="#A537D1" ForeColor="White" OnClick="btnFinancePrint_3_Click" />
                            <asp:Button ID="btnHR_4" runat="server" Text="HR" CssClass="textbox" BackColor="#A537D1"
                                ForeColor="White" OnClick="btnHR_4_Click" />
                            <asp:Button ID="btnTransRemind_5" runat="server" Text="Transport" CssClass="textbox"
                                BackColor="#A537D1" ForeColor="White" OnClick="btnTransRemind_5_Click" />
                            <asp:Button ID="btnHostel_6" runat="server" Text="Hostel" CssClass="textbox" BackColor="#A537D1"
                                ForeColor="White" OnClick="btnHostel_6_Click" />
                            <asp:Button ID="btnadmesion_7" runat="server" Text="Application" CssClass="textbox"
                                BackColor="#A537D1" ForeColor="White" OnClick="btnadmesion_7_Click" />
                            <asp:Button ID="Inventory" runat="server" Text="Inventory" CssClass="textbox" BackColor="#A537D1"
                                ForeColor="White" OnClick="Inventory_Click" />
                            <asp:Button ID="Library" runat="server" Text="Library" CssClass="textbox" BackColor="#A537D1"
                                ForeColor="White" OnClick="Library_Click" />
                            <asp:Button ID="btnMblApp_10" runat="server" Text="Mobile App" CssClass="textbox"
                                BackColor="#A537D1" ForeColor="White" OnClick="btnMblApp_Click" />
                            <%--Deepali 16.7.18--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnAttendance" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                ForeColor="White" Font-Bold="true" OnClick="btnsave_Attendance_Click" Style="margin-left: 700px;
                                width: 80px;" />
                        </td>
                        <td>
                            <asp:Button ID="btnsave_coe" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                ForeColor="White" Font-Bold="true" OnClick="btnsave_coe_Click" Style="width: 80px;" />
                        </td>
                        <td>
                            <asp:Button ID="btnFinance" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                ForeColor="White" Font-Bold="true" OnClick="btnsave_Finance_Click" Style="width: 80px;" />
                        </td>
                        <td>
                            <asp:Button ID="btnHR" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                ForeColor="White" Font-Bold="true" OnClick="btnsave_HR_Click" Style="width: 80px;" />
                        </td>
                        <td>
                            <asp:Button ID="btnTransport" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                ForeColor="White" Font-Bold="true" OnClick="btnsave_Trans_Click" Style="width: 80px;" />
                        </td>
                        <td>
                            <asp:Button ID="btnHostel" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                ForeColor="White" Font-Bold="true" OnClick="btnsave_Hostel_Click" Style="width: 80px;" />
                        </td>
                        <td>
                            <asp:Button ID="btnapplication" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                ForeColor="White" Font-Bold="true" Style="width: 80px;" OnClick="btnapplication_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btninventory" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                ForeColor="White" Font-Bold="true" Style="width: 80px;" OnClick="btninventory_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btnLibrary" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                ForeColor="White" Font-Bold="true" Style="width: 80px;" OnClick="btnSaveLibrary_Click" />
                        </td>
                        <td>
                            <%--Deepali 16.7.18--%>
                            <asp:Button ID="btnMblAppSave" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                ForeColor="White" Font-Bold="true" Style="width: 80px;" OnClick="btnMblAppSave_Click" />
                            <%--Both For Staff and Student--%>
                        </td>
                    </tr>
                </table>
            </center>
            <asp:DropDownList ID="TabContainer1" runat="server" Style="display: none;">
                <asp:ListItem Selected="True" Value="0"></asp:ListItem>
                <asp:ListItem Value="1"></asp:ListItem>
                <asp:ListItem Value="2"></asp:ListItem>
                <asp:ListItem Value="3"></asp:ListItem>
                <asp:ListItem Value="4"></asp:ListItem>
                <asp:ListItem Value="5"></asp:ListItem>
                <asp:ListItem Value="6"></asp:ListItem>
                <asp:ListItem Value="7"></asp:ListItem>
                <asp:ListItem Value="8"></asp:ListItem>
                <asp:ListItem Value="9"></asp:ListItem>
                <asp:ListItem Value="10"></asp:ListItem>
            </asp:DropDownList>
            <%--Attendance Setting Tab--%>
            <center>
                <div id="divAttendance" runat="server" style="width: 980px; height: 400px; background: white;
                    border: 1px solid #0095E8; overflow: auto;" visible="false">
                    <table>
                        <tr>
                            <td>
                                <fieldset style="height: 55px; width: 200px;">
                                    <legend>Include Redo Attendance </legend>
                                    <asp:CheckBox ID="chkRedo" runat="server" Text="Include Redo student in Attendance" />
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="width: 170px; height: 55px;">
                                    <legend>OD Lock</legend>
                                    <asp:Label ID="LbllockDays" runat="server" Text="Lock Days" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:TextBox ID="txtOdlock" runat="server" Width="80px" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" TargetControlID="txtOdlock"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                </fieldset>
                            </td>
                            <td>
                                <fieldset style="height: 77px; width: -23px;">
                                    <legend>Post Semester Selection For CBCS Elective</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                                                    ForeColor="Black" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Width="80px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDegree" runat="server" CssClass="commonHeaderFont" Text="Degree"
                                                    AssociatedControlID="txtDegree"></asp:Label>
                                            </td>
                                            <td>
                                                <div style="position: relative;">
                                                    <asp:UpdatePanel ID="upnlDegree" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtDegree" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                                                ReadOnly="true">-- Select --</asp:TextBox>
                                                            <asp:Panel ID="pnlDegree" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                                                Width="140px">
                                                                <asp:CheckBox ID="chkDegree" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                                    AutoPostBack="True" OnCheckedChanged="chkDegree_CheckedChanged" />
                                                                <asp:CheckBoxList ID="cblDegree" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="cblDegree_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="popExtDegree" runat="server" TargetControlID="txtDegree"
                                                                PopupControlID="pnlDegree" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblBranch" runat="server" CssClass="commonHeaderFont" Text="Branch"
                                                    AssociatedControlID="txtBranch"></asp:Label>
                                            </td>
                                            <td>
                                                <div style="position: relative;">
                                                    <asp:UpdatePanel ID="upnlBranch" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtBranch" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                                                ReadOnly="true">-- Select --</asp:TextBox>
                                                            <asp:Panel ID="pnlBranch" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                                                Width="280px">
                                                                <asp:CheckBox ID="chkBranch" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                                    AutoPostBack="True" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                                <asp:CheckBoxList ID="cblBranch" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="popExtBranch" runat="server" TargetControlID="txtBranch"
                                                                PopupControlID="pnlBranch" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                                    ForeColor="White" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Width="70Px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btnSaveSem" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                                        ForeColor="White" Font-Bold="true" OnClick="btnSaveSem_Click" Style="width: 80px;" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <fieldset style="height: 250px; width: 200px; margin-left: -40px;">
                                    <legend>Attendance Leave Request Settings </legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCollegeAC" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                                    ForeColor="Black" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCollegeAcr" runat="server" Style="height: auto; width: 70px"
                                                    Font-Names="Book Antiqua" Font-Size="Smaller" AutoPostBack="true" OnSelectedIndexChanged="ddlCollegeAcr_IndexChange">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblEduLevel" runat="server" Text="Edu.Level" Font-Bold="True" Font-Names="Book Antiqua"
                                                    ForeColor="Black" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlEduLevel" runat="server" Style="height: auto; width: 70px"
                                                    Font-Names="Book Antiqua" Font-Size="Smaller" AutoPostBack="true" OnSelectedIndexChanged="ddlEduLevel_IndexChange">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblBatchYear" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                                                    ForeColor="Black" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlBatchYear" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Width="80px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlBatchYear_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <%-- <td>
                                                <asp:Label ID="lblS" runat="server" Text="Sem" Font-Bold="true" Font-Names="Book Antiqua"
                                                    ForeColor="Black" Font-Size="Medium"></asp:Label>
                                            </td>--%>
                                            <%--<td>
                                                <asp:DropDownList ID="ddlS" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Width="80px" CssClass="arrow">
                                                </asp:DropDownList>
                                            </td>--%>
                                            <td>
                                                <asp:Button ID="btnGo1" runat="server" Text="GO" CssClass="textbox" BackColor="BlueViolet"
                                                    ForeColor="White" Font-Bold="true" OnClick="btnGo1_Click" Style="width: 80px;" />
                                            </td>
                                        </tr>
                                    </table>
                                    <center>
                                        <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                            EnableClientScript="true" BorderWidth="1px" Visible="true" VerticalScrollBarPolicy="Never"
                                            HorizontalScrollBarPolicy="Never" CssClass="stylefp">
                                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                ButtonShadowColor="ControlDark">
                                            </CommandBar>
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" GridLineColor="Black"
                                                    GroupBarText="Drag a column to group by that column." SelectionBackColor="#CE5D5A"
                                                    SelectionForeColor="White">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                        <br />
                                        <asp:Button ID="btnSaveNew" runat="server" Text="Save" CssClass="textbox" BackColor="BlueViolet"
                                            ForeColor="White" Font-Bold="true" OnClick="btnSaveNew_Click" Style="width: 80px;" />
                                    </center>
                                </fieldset>
                            </td>
                            <td>
                                <fieldset style="height: 50px; width: 150px;">
                                    <legend>Student Login with OTP</legend>
                                    <asp:CheckBox ID="chkotp" runat="server" Text="OTP" />
                                </fieldset>
                            </td>
                            <td>
                                <fieldset style="height: 80px; width: 159px;">
                                    <legend>Lock Attendance With Batch Year</legend>
                                    <asp:CheckBox ID="cbatndbatyr" runat="server" Text="Batch" Style="margin-left: 20px;
                                        margin-top: 14px;" OnCheckedChanged="cbatndbatyr_OnCheckedChanged" AutoPostBack="true" />
                                    <%--<td>
                                        <div style="position: relative;">--%>
                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtbatyr" Enabled="false" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                                Style="margin-left: 86px; margin-top: -26px;" ReadOnly="true">-- Select --</asp:TextBox>
                                            <asp:Panel ID="Panel7" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                                Width="280px">
                                                <asp:CheckBox ID="cbbatyr" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                    AutoPostBack="True" OnCheckedChanged="cbbatyr_CheckedChanged" />
                                                <asp:CheckBoxList ID="cblbatyr" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                                    OnSelectedIndexChanged="cblbatyr_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txtbatyr"
                                                PopupControlID="Panel7" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <%--</div>
                                    </td>--%>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                    <table>
                    </table>
                </div>
            </center>
            <%--COE Setting Tab--%>
            <center>
                <div id="divCOE" runat="server" style="width: 980px; height: 400px; background: white;
                    border: 1px solid #0095E8; overflow: auto;" visible="false">
                    <table>
                        <tr>
                            <td>
                                <fieldset style="height: 55px; width: 200px;">
                                    <legend>Student Login for Result </legend>
                                    <asp:CheckBox ID="ChkDispMarks" runat="server" Text="Exclude Unpaid Students for hiding semester marks" />
                                </fieldset>
                            </td>
                            <td>
                                <fieldset style="height: 90px; width: 140px;">
                                    <legend>Double Valuation Settings</legend>
                                    <asp:RadioButton ID="rdbval1" runat="server" Text="1st Valuation" Font-Bold="True"
                                        Font-Names="Book Antiqua" Checked="true" GroupName="Report1" />
                                    <asp:RadioButton ID="rdbval2" runat="server" Text="2ed Valuation" Font-Bold="True"
                                        Font-Names="Book Antiqua" Checked="true" GroupName="Report1" />
                                    <asp:RadioButton ID="rbdAll" runat="server" Text="All" Font-Bold="True" Font-Names="Book Antiqua"
                                        Checked="true" GroupName="Report1" />
                                </fieldset>
                            </td>
                            <td>
                                <fieldset style="height: 30px; width: 250px;">
                                    <legend>Dispaly GPA for Fail Student</legend>
                                    <asp:CheckBox ID="chkfailGpa" runat="server" Text="include gpa for fail student" />
                                </fieldset>
                            </td>
                            <td>
                                <fieldset style="height: 30px; width: 105px;">
                                    <legend>Marksheet</legend>
                                    <asp:CheckBox ID="chkprintlock" runat="server" Text="PrintLock" />
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <fieldset style="height: 70px; width: 650px;">
                                    <legend>Student Login Result Hold </legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label9" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                                    ForeColor="Black" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DropDownList1" runat="server" Style="height: auto; width: 70px"
                                                    Font-Names="Book Antiqua" Font-Size="Smaller" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_IndexChange">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label10" runat="server" Text="Edu.Level" Font-Bold="True" Font-Names="Book Antiqua"
                                                    ForeColor="Black" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DropDownList2" runat="server" Style="height: auto; width: 70px"
                                                    Font-Names="Book Antiqua" Font-Size="Smaller">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label11" runat="server" Text="Exam Year" Font-Bold="true" Font-Names="Book Antiqua"
                                                    ForeColor="Black" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DropDownList3" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Width="80px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="DropDownList3_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label12" runat="server" Text="Exam Month" Font-Bold="true" Font-Names="Book Antiqua"
                                                    ForeColor="Black" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DropDownList4" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Width="80px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="DropDownList4_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:CheckBox ID="CheckBox1" runat="server" Text="Hold Result" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <fieldset style="height: 150px; width: 700px;">
                                    <legend>Online Exam Application Block</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label13" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                                    ForeColor="Black" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DropDownList5" runat="server" Style="height: auto; width: 70px"
                                                    Font-Names="Book Antiqua" Font-Size="Smaller" AutoPostBack="true" OnSelectedIndexChanged="DropDownList5_IndexChange">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label14" runat="server" Text="Edu.Level" Font-Bold="True" Font-Names="Book Antiqua"
                                                    ForeColor="Black" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DropDownList6" runat="server" Style="height: auto; width: 70px"
                                                    Font-Names="Book Antiqua" Font-Size="Smaller" AutoPostBack="true" OnSelectedIndexChanged="DropDownList6_IndexChange">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label15" runat="server" Text="Exam Year" Font-Bold="true" Font-Names="Book Antiqua"
                                                    ForeColor="Black" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DropDownList7" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Width="80px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="DropDownList7_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label16" runat="server" Text="Exam Month" Font-Bold="true" Font-Names="Book Antiqua"
                                                    ForeColor="Black" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DropDownList8" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Width="80px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="DropDownList8_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label17" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                                                    ForeColor="Black" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <div style="position: relative;">
                                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="TextBox1" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                                                ReadOnly="true">-- Select --</asp:TextBox>
                                                            <asp:Panel ID="Panel6" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                                                Width="140px">
                                                                <asp:CheckBox ID="CheckBox2" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                                    AutoPostBack="True" OnCheckedChanged="CheckBox2_CheckedChanged" />
                                                                <asp:CheckBoxList ID="CheckBoxList1" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="CheckBoxList1_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="TextBox1"
                                                                PopupControlID="Panel6" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <%--
                                                <asp:DropDownList ID="DropDownList9" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Width="80px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="DropDownList9_SelectedIndexChanged">
                                                </asp:DropDownList>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="7">
                                                <asp:RadioButton ID="RadioButton1" runat="server" Text="Hold" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Checked="true" GroupName="report" />
                                                <asp:RadioButton ID="RadioButton2" runat="server" Text="Release" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Checked="true" GroupName="report" />
                                                <fieldset style="height: 40px; width: 280px;">
                                                    <legend>Online Exam Application Last Date </legend>
                                                    <asp:Label ID="Label18" runat="server" Text="Date" Font-Bold="true" Font-Names="Book Antiqua"
                                                        ForeColor="Black" Font-Size="Medium"></asp:Label>
                                                    <asp:TextBox ID="txtdop" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Width="75px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtdop" Format="dd/MM/yyyy"
                                                        runat="server">
                                                    </asp:CalendarExtender>
                                                    <asp:Button ID="btnChlSave" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnClick="btnChlSave_Click" Text="Save" />
                                                </fieldset>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <fieldset style="height: 320px; width: 520px;">
                                    <legend>Student Login Result Note </legend>
                                    <asp:TextBox ID="txtResultNote" runat="server" Width="500px" Height="300px" TextMode="MultiLine"></asp:TextBox>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <fieldset style="height: 320px; width: 520px;">
                                    <legend>Fail settings </legend>
                                    <table>
                                        <asp:GridView ID="gridView2" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                            Width="100px">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <center>
                                                            <%--     <%#Container.DataItemIndex+1 %>--%>
                                                            <asp:Label ID="lblsno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Actual Grade" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="lblActual" runat="server" Text='<%#Eval("actualgrade") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Grade" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtgrade" runat="server" Text='<%#Eval("grade") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Result" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtResult" runat="server" Text='<%#Eval("Result") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:Button ID="Btngradesave" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnClick="Btngradesave_Click" Text="Save" />
                                        <asp:Button ID="btnadd" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnClick="Addnew_Click" Text="Add New Row" />
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <fieldset style="height: 139px; width: 410px; margin-left: -465px;">
                                    <legend>Invigilator Travel Allowance</legend>
                                    <br />
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Lblmin" runat="server" Text="Minimum Km" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Text_minkm" runat="server" Width="100px" Height="20px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="Text_minkm"
                                                    FilterType="Custom,Numbers" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="LblAmt" runat="server" Text="Amount" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Text_minAmt" runat="server" Width="100px" Height="20px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="Text_minAmt"
                                                    FilterType="Custom,Numbers" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label_km" runat="server" Text="Per Km" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Text_Perkm" runat="server" Width="100px" Height="20px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="Text_Perkm"
                                                    FilterType="Custom,Numbers" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label_peramt" runat="server" Text="Amount" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Text_peramt" runat="server" Width="100px" Height="20px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="Text_peramt"
                                                    FilterType="Custom,Numbers" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <center>
                                        <asp:Button ID="btnsave_InvigilatorTravel" runat="server" Text="Save" CssClass="textbox"
                                            BackColor="#1B9D17" ForeColor="White" Font-Bold="true" OnClick="btnsave_InvigilatorTravel_Click"
                                            Style="width: 80px;" />
                                    </center>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
            <%--Finance Setting Tab--%>
            <center>
                <div id="divFinance" runat="server" style="width: 980px; height: 400px; background: white;
                    border: 1px solid #0095E8; overflow: auto;" visible="false">
                    <%--finance tab--%>
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <div>
                                        <fieldset style="width: 609px; height: 73px;">
                                            <legend>Graduation Fees Settings </legend>
                                            <table>
                                                <tr>
                                                    <td>
                                                        College
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlcollegename" Width="60px" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlcollegename_SelectedIndexChanged" Font-Bold="True"
                                                            Font-Names="Book Antiqua">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td colspan="2">
                                                        Header
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlheader" runat="server" CssClass="textbox ddlheight2" OnSelectedIndexChanged="ddl_header_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        Ledger
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlledger" runat="server" CssClass="textbox ddlheight2">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        Amount
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtamt" runat="server" Style="height: 20px; width: 60px;"></asp:TextBox>
                                                    </td>
                                                    <%-- <td>
                                                <asp:Button ID="btnappsave" runat="server" Text="Save" OnClick="btnappsave_OnClick"
                                                    Style="height: 30px; width: 80px;" />
                                            </td>--%>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lbloutput" runat="server" Visible="false" Style="color: Red;"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <%-- Stream
                                            <asp:DropDownList ID="ddlstream" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                OnSelectedIndexChanged="ddlstream_SelectedIndexChanged" Width="60px" AutoPostBack="true">
                                            </asp:DropDownList>
                                            Education Level
                                            <asp:DropDownList ID="ddledlevel" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                OnSelectedIndexChanged="ddledlevel_SelectedIndexChanged" Width="60px" AutoPostBack="true">
                                            </asp:DropDownList>
                                            Ledger
                                            <asp:DropDownList ID="ddlledger" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                OnSelectedIndexChanged="ddlledger_SelectedIndexChanged" Width="80px" AutoPostBack="true">
                                            </asp:DropDownList>
                                            Amount
                                            <asp:TextBox ID="txtamt" runat="server" Style="height: 20px; width: 60px;"></asp:TextBox>
                                            <asp:Button ID="btnappsave" runat="server" Text="Save" OnClick="btnappsave_OnClick"
                                                Style="height: 30px; width: 80px;" />
                                            <asp:Label ID="lbloutput" runat="server" Visible="false" Style="color: Red;"></asp:Label>--%>
                                            <asp:RadioButton ID="rb_WithFees" Text="WithFees" GroupName="WithFees" runat="server" />
                                            <asp:RadioButton ID="rb_WithoutFees" Text="Without Fees" GroupName="WithFees" runat="server" />
                                        </fieldset>
                                        <table>
                                            <tr>
                                                <td>
                                                    Reference Starting Number
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReference" runat="server" CssClass="textbox txtheight2" Style="width: 200px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkprint" runat="server" Text="Print Allow" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <%--abarna--%>
                                <td>
                                    <fieldset style="width: 609px; height: 56px;">
                                        <legend>Finance Universal Report Multiple</legend>
                                        <table>
                                            <tr>
                                                <asp:CheckBox ID="Year" runat="server" Text="YearWise" />
                                                <asp:CheckBox ID="Semester" runat="server" Text="SemesterWise" />
                                                <asp:Button ID="setting" runat="server" Text="Save" OnClick="btnSaveSet_Click" />
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="10">
                                    <fieldset style="height: 250px;">
                                        <legend>Online Payment Finance Year,Header & Ledger Settings[Additional Fees] </legend>
                                        <table>
                                            <tr>
                                                <td>
                                                    Semester
                                                </td>
                                                <td>
                                                    <%--<asp:DropDownList ID="ddlfinOnline" runat="server" CssClass="textbox1 ddlheight2">
                                                    </asp:DropDownList>--%>
                                                    <div style="position: relative;">
                                                        <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox txtheight2" Style="width: 200px"
                                                            ReadOnly="true">--Select--</asp:TextBox>
                                                        <asp:Panel ID="panel_sem" runat="server" CssClass="multxtpanel " Style="width: 255px;
                                                            border-color: HighlightText; height: 150px;">
                                                            <asp:CheckBox ID="cb_sem" runat="server" Width="204px" Text="Select All" AutoPostBack="true"
                                                                OnCheckedChanged="cb_sem_CheckedChanged" />
                                                            <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                    </div>
                                                </td>
                                                <td>
                                                    Header
                                                </td>
                                                <td>
                                                    <div style="position: relative;">
                                                        <asp:TextBox ID="txthdOnline" runat="server" CssClass="textbox txtheight2" Style="width: 200px"
                                                            ReadOnly="true">--Select--</asp:TextBox>
                                                        <asp:Panel ID="Panel22" runat="server" CssClass="multxtpanel " Style="width: 255px;
                                                            border-color: HighlightText; height: 150px;">
                                                            <asp:CheckBox ID="cbhdOnline" runat="server" Width="204px" Text="Select All" AutoPostBack="true"
                                                                OnCheckedChanged="cbhdOnline_OnCheckedChanged" />
                                                            <asp:CheckBoxList ID="cblhdOnline" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblhdOnline_OnSelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                    </div>
                                                </td>
                                                <td>
                                                    Ledger
                                                </td>
                                                <td>
                                                    <div style="position: relative;">
                                                        <asp:TextBox ID="txtldOnline" runat="server" CssClass="textbox txtheight2" Style="width: 200px"
                                                            ReadOnly="true">--Select--</asp:TextBox>
                                                        <asp:Panel ID="Panel23" runat="server" CssClass="multxtpanel " Style="width: 255px;
                                                            border-color: HighlightText; height: 150px;">
                                                            <asp:CheckBox ID="cbedgOnline" runat="server" Width="204px" Text="Select All" AutoPostBack="true"
                                                                OnCheckedChanged="cbedgOnline_OnCheckedChanged" />
                                                            <asp:CheckBoxList ID="cbledgOnline" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbledgOnline_OnSelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                    </div>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnsaveOnline" runat="server" Text="Save" CssClass="textbox textbox1 btn2"
                                                        Width="70px" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" Style="background-color: Green;
                                                        color: White;" OnClick="btnsaveOnline_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </center>
            <%--HR Setting Tab--%>
            <center>
                <div id="divHR" runat="server" style="width: 980px; height: 400px; background: white;
                    border: 1px solid #0095E8; overflow: auto;" visible="false">
                    <table>
                        <tr>
                            <td colspan="2">
                                <fieldset style="width: 325px; height: 55px; margin-left: 20px;">
                                    <legend style="color: Black;">Online Biometric </legend>
                                    <asp:Label ID="lbContinuePermission" runat="server" Text="Continue Permission" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:TextBox ID="txtExtentPermission" AutoPostBack="true" runat="server" Width="80px"
                                        Font-Names="Book Antiqua" Font-Size="Medium" MaxLength="3" onchange="permissionmint(this)"></asp:TextBox>
                                    <asp:Label ID="Label21" runat="server" Text="(120 Mins)" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtExtentPermission"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
            <%--Transport Setting Tab--%>
            <center>
                <div id="divTransport" runat="server" style="width: 980px; height: 400px; background: white;
                    border: 1px solid #0095E8; overflow: auto;" visible="false">
                </div>
            </center>
            <%--Hostel Setting Tab--%>
            <center>
                <div id="divHostel" runat="server" style="width: 980px; height: 400px; background: white;
                    border: 1px solid #0095E8; overflow: auto;" visible="false">
                    <table>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: 285px; width: -23px;">
                                    <legend style="height: 10">Hostel</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblbreakage" runat="server" Text="Breakage" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_BreakageHeader" runat="server" CssClass="textbox ddlheight2"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_BreakageHeader_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_BreakageLedger" runat="server" CssClass="textbox ddlheight2">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdb_inmess" runat="server" Text="Include Mess Bill Calculation"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Checked="true" GroupName="Breakage"
                                                    AutoPostBack="True" OnCheckedChanged="rdb_inmess_CheckedChanged" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdb_exmess" runat="server" Text="Exclude Mess Bill Calculation"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Checked="true" GroupName="Breakage"
                                                    AutoPostBack="True" OnCheckedChanged="rdb_exmess_CheckedChanged" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="LblGym" runat="server" Text="Gym" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_GymHeader" runat="server" CssClass="textbox ddlheight2"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_GymHeader_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_GymLedger" runat="server" CssClass="textbox ddlheight2">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdb_Gymin" runat="server" Text="Include Mess Bill Calculation"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Checked="true" GroupName="Gym" AutoPostBack="True"
                                                    OnCheckedChanged="rdb_Gymin_CheckedChanged" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdb_Gymex" runat="server" Text="Exclude Mess Bill Calculation"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Checked="true" GroupName="Gym" AutoPostBack="True"
                                                    OnCheckedChanged="rdb_Gymex_CheckedChanged" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Lblhealth" runat="server" Text="Health" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_HealthHeader" runat="server" CssClass="textbox ddlheight2"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_HealthHeader_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_HealthLedger" runat="server" CssClass="textbox ddlheight2">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdb_healthin" runat="server" Text="Include Mess Bill Calculation"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Checked="true" GroupName="Health"
                                                    AutoPostBack="True" OnCheckedChanged="rdb_healthin_CheckedChanged" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdb_healthex" runat="server" Text="Exclude Mess Bill Calculation"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Checked="true" GroupName="Health"
                                                    AutoPostBack="True" OnCheckedChanged="rdb_healthex_CheckedChanged" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <fieldset style="height: 150px; width: 205px;">
                                                    <legend style="height: 10">Mess Bill</legend>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="LblRegular" runat="server" Text="Regular" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddl_RegularHeader" runat="server" CssClass="textbox ddlheight2"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_RegularHeader_OnSelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddl_RegularLedger" runat="server" CssClass="textbox ddlheight2">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="LblGust" runat="server" Text="Guest" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddl_GustHeader" runat="server" CssClass="textbox ddlheight2"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_GustHeader_OnSelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddl_GustLedger" runat="server" CssClass="textbox ddlheight2">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="LblStaff" runat="server" Text="Staff" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddl_StaffHeader" runat="server" CssClass="textbox ddlheight2"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_StaffHeader_OnSelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddl_StaffLedger" runat="server" CssClass="textbox ddlheight2">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="LblOthers" runat="server" Text="Others" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddl_OthersHeader" runat="server" CssClass="textbox ddlheight2"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_OthersHeader_OnSelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddl_OthersLedger" runat="server" CssClass="textbox ddlheight2">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <fieldset style="height: 39px; width: -23px;">
                                    <legend style="height: 10">Attendance</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdb_hostelattn" runat="server" Text="Hostel Attendance" Font-Bold="True"
                                                    Font-Names="Book Antiqua" GroupName="attendance" AutoPostBack="True" OnCheckedChanged="rdb_hostelattn_CheckedChanged" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdb_messattn" runat="server" Text="Mess Attendance" Font-Bold="True"
                                                    Font-Names="Book Antiqua" GroupName="attendance" AutoPostBack="True" OnCheckedChanged="rdb_messattn_CheckedChanged" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdb_bothattn" runat="server" Text="Both" Font-Bold="True" Font-Names="Book Antiqua"
                                                    GroupName="attendance" AutoPostBack="True" OnCheckedChanged="rdb_bothattn_CheckedChanged" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <td>
                                <fieldset style="height: 129px; width: -23px;">
                                    <legend style="height: 10">Gatepass Count</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbhostel" runat="server" Text="Hostel" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddl_Hostel" runat="server" CssClass="textbox1  ddlheight1"
                                                            Visible="true" AutoPostBack="True" OnSelectedIndexChanged="ddl_Hostel_OnSelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_count" runat="server" CssClass="textbox  txtheight" Visible="true"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="ftext_mno" runat="server" TargetControlID="txt_count"
                                                            FilterType="numbers" ValidChars="">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Lblcoll" runat="server" Text="College" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlcollegeco" runat="server" CssClass="textbox1  ddlheight1"
                                                            Visible="True" AutoPostBack="True" OnSelectedIndexChanged="ddlcollegeco_OnSelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="Txtcol_count" runat="server" CssClass="textbox  txtheight" Visible="True"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="Txtcol_count"
                                                            FilterType="numbers" ValidChars="">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:RadioButton ID="rdbboi" runat="server" Text="Biometric Based" Font-Bold="True"
                                                    Font-Names="Book Antiqua" GroupName="bio" AutoPostBack="True" />
                                            </td>
                                            <td colspan="2">
                                                <asp:RadioButton ID="rdbnonboi" runat="server" Text="Non Biometric Based" Font-Bold="True"
                                                    Font-Names="Book Antiqua" GroupName="bio" AutoPostBack="True" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <%--magesh 22.6.18--%>
                        <tr>
                            <td>
                                <fieldset style="height: 39px; width: -23px;">
                                    <legend style="height: 10">Hostel Id Generation</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdbin" runat="server" Text="Individual" Font-Bold="True" Font-Names="Book Antiqua"
                                                    GroupName="id" AutoPostBack="True" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdbhos" runat="server" Text="Hostel Wise" Font-Bold="True" Font-Names="Book Antiqua"
                                                    GroupName="id" AutoPostBack="True" />
                                            </td>
                                        </tr>
                                        <%--magesh 22.6.18--%>
                                    </table>
                                </fieldset>
                            </td>
                            <td>
                                <fieldset style="height: 39px; width: -23px;">
                                    <legend style="height: 10">Hostel Rights</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label5" runat="server" Text="Hostel Name" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="upp1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_messname" runat="server" CssClass="textbox textbox1 txtheight1">--Select--</asp:TextBox>
                                                        <asp:Panel ID="p1" runat="server" Height="150px" Width="160px" Style="background: White;
                                                            border-color: Gray; border-style: Solid; border-width: 2px; box-shadow: 0px 0px 4px #999999;
                                                            border-radius: 5px; overflow: auto;">
                                                            <asp:CheckBox ID="cb_hos" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_mess_CheckedChange" />
                                                            <asp:CheckBoxList ID="cbl_hos" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_mess_SelectedIndexChange">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_messname"
                                                            PopupControlID="p1" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <%--magesh 22.6.18--%>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <div style="position: relative;">
                                            <fieldset style="height: 39px; width: -23px;">
                                                <legend style="height: 10">Hostel</legend>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label6" runat="server" Text="Building Name" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <div style="position: relative;">
                                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txt_buildingname" runat="server" CssClass="textbox textbox1 txtheight1">--Select--</asp:TextBox>
                                                                        <asp:Panel ID="Panel3" runat="server" Height="150px" Width="160px" Style="background: White;
                                                                            border-color: Gray; border-style: Solid; border-width: 2px; box-shadow: 0px 0px 4px #999999;
                                                                            border-radius: 5px; overflow: auto;">
                                                                            <asp:CheckBox ID="cb_buildname" runat="server" Text="Select All" AutoPostBack="true"
                                                                                OnCheckedChanged="cb_buildname_checkedchange" />
                                                                            <asp:CheckBoxList ID="cbl_buildname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_buildname_SelectedIndexChanged">
                                                                            </asp:CheckBoxList>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_buildingname"
                                                                            PopupControlID="Panel3" Position="Bottom">
                                                                        </asp:PopupControlExtender>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label7" runat="server" Text="Floor Name" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <div style="position: relative;">
                                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txt_floorname" runat="server" CssClass="textbox textbox1 txtheight1">--Select--</asp:TextBox>
                                                                        <asp:Panel ID="Panel4" runat="server" Height="150px" Width="160px" Style="background: White;
                                                                            border-color: Gray; border-style: Solid; border-width: 2px; box-shadow: 0px 0px 4px #999999;
                                                                            border-radius: 5px; overflow: auto;">
                                                                            <asp:CheckBox ID="cb_floorname" runat="server" Text="Select All" AutoPostBack="true"
                                                                                OnCheckedChanged="cb_floorname_checkedchange" />
                                                                            <asp:CheckBoxList ID="cbl_floorname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_floorname_SelectedIndexChanged">
                                                                            </asp:CheckBoxList>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_floorname"
                                                                            PopupControlID="Panel4" Position="Bottom">
                                                                        </asp:PopupControlExtender>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label8" runat="server" Text="Room Name" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <div style="position: relative;">
                                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txt_roomname" runat="server" CssClass="textbox textbox1 txtheight1">--Select--</asp:TextBox>
                                                                        <asp:Panel ID="Panel5" runat="server" Height="150px" Width="160px" Style="background: White;
                                                                            border-color: Gray; border-style: Solid; border-width: 2px; box-shadow: 0px 0px 4px #999999;
                                                                            border-radius: 5px; overflow: auto;">
                                                                            <asp:CheckBox ID="cb_roomname" runat="server" Text="Select All" AutoPostBack="true"
                                                                                OnCheckedChanged="cb_room_CheckedChanged" />
                                                                            <asp:CheckBoxList ID="cbl_roomname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_room_SelectedIndexChanged">
                                                                            </asp:CheckBoxList>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_roomname"
                                                                            PopupControlID="Panel5" Position="Bottom">
                                                                        </asp:PopupControlExtender>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <%--magesh 22.6.18--%>
                                                </table>
                                            </fieldset>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                             <%--  Added By SaranyaDevi4.3.2019--%>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                    <ContentTemplate>
                                        <div style="position: relative;">
                                            <fieldset style="height: 39px; width: -23px;">
                                                <legend style="height: 10">Hostel Vacate</legend>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButtonList ID="RadioButtonHostel" runat="server" AutoPostBack="true"
                                                                RepeatDirection="Horizontal">
                                                                <asp:ListItem Text="Direct Vacate" Value="0" Selected="True">
                                                                </asp:ListItem>
                                                                <asp:ListItem Text="Request Vacate" Value="1">
                                                                </asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
                <center>
                    <div id="imagalt" runat="server" visible="false" style="height: 1000px; z-index: 10000;
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
                                                <asp:Label ID="lbl_aler" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="Button1" CssClass=" textbox textbox1 btn1" Style="height: 28px; width: 65px;"
                                                        OnClick="btn_alertclose_Click1" Text="ok" runat="server" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
            </center>
            <%--application Setting Tab krishhna kumar.r--%>
            <center>
                <div id="divapplication" runat="server" style="width: 980px; height: 400px; background: white;
                    border: 1px solid #0095E8; overflow: auto;" visible="false">
                    <table>
                        <tr>
                            <td>
                                <fieldset style="height: 55px; width: 200px;">
                                    <legend>Eligibility Mark Setting </legend>
                                    <asp:CheckBox ID="chkapplication" runat="server" Text="Eligibility Mark
            Setting" />
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
            <center>
                <div id="divinventory" runat="server" style="width: 980px; height: 400px; background: white;
                    border: 1px solid #0095E8; overflow: auto;">
                    <table>
                        <tr>
                            <td>
                                <fieldset style="height: 177px; width: 613px;">
                                    <legend style="height: 10">Inventory</legend>
                                    <fieldset style="width: 200px; height: 15px; margin-left: 160px">
                                        <asp:RadioButtonList ID="rbl_Com_ind" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbl_Com_ind_OnSelectedIndexChanged"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Common" Value="0" Selected="True">
                                            </asp:ListItem>
                                            <asp:ListItem Text="Individual" Value="1">
                                            </asp:ListItem>
                                        </asp:RadioButtonList>
                                    </fieldset>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gridView1" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                                    OnRowDataBound="gridView1_OnRowDataBound" OnDataBound="Marksgrid_pg_DataBound"
                                                    OnRowCommand="gridView1_OnRowCommand" Width="100px">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <%--     <%#Container.DataItemIndex+1 %>--%>
                                                                    <asp:Label ID="lbl_rs" runat="server" Width="30px" Text='<%#Eval("Sno") %>'></asp:Label>
                                                                </center>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="KitName" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Panel ID="pan_kit" runat="server" Visible="false" ScrollBars="Auto" Style="height: 80px;
                                                                                    width: 150px;">
                                                                                    <asp:CheckBoxList ID="kit_name" runat="server">
                                                                                    </asp:CheckBoxList>
                                                                                </asp:Panel>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddl_Kitname" runat="server" CssClass="textbox ddlheight3" AutoPostBack="true"
                                                                                    Width="110px" Visible="false">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Header Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:DropDownList ID="ddl_headername" runat="server" CssClass="textbox ddlheight3"
                                                                        AutoPostBack="true" Width="110px" Visible="true">
                                                                    </asp:DropDownList>
                                                                </center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Ledger Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:DropDownList ID="ddl_ledgername" runat="server" CssClass="textbox ddlheight3"
                                                                        AutoPostBack="true" Width="110px" Visible="true">
                                                                    </asp:DropDownList>
                                                                </center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Payment" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:TextBox ID="txtpaymt" runat="server" onblur="return addmarks(this)" CssClass="  textbox txtheight1"
                                                                        Height="17px" Width="90px"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtpaymt"
                                                                        FilterType="Numbers,Custom" ValidChars=" .">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                            <td>
                                                <div id="grid1btn" runat="server" visible="true" style="width: 50px;">
                                                    <asp:Button ID="btnaddgrid" Text="Add" CssClass="textbox textbox1" Height="32px"
                                                        Width="50px" runat="server" Style="width: 50px;" OnClick="btnaddgrid_Click" />
                                                    <asp:Button ID="btnRowOK" runat="server" Text="Save" OnClick="btnSave_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <%----%>
                                    <asp:Label ID="lbl_er" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:GridView ID="gdReport" runat="server" Visible="true" AutoGenerateColumns="false"
                    GridLines="Both" Width="730px">
                    <%--OnDataBound="gdattrpt_OnDataBound" OnRowDataBound="gdReport_OnRowDataBound"--%>
                    <Columns>
                        <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                            HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <center>
                                    <%--     <%#Container.DataItemIndex+1 %>--%>
                                    <asp:Label ID="lbl_rs" runat="server" Width="60px" Text='<%#Eval("Sno") %>'></asp:Label>
                                </center>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="KitName" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                            HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <center>
                                    <asp:DropDownList ID="ddl_Kitname" runat="server" CssClass="textbox ddlheight3" AutoPostBack="true"
                                        Width="110px" Visible="true">
                                    </asp:DropDownList>
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Header Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <center>
                                    <asp:DropDownList ID="ddl_headername" runat="server" CssClass="textbox ddlheight3"
                                        AutoPostBack="true" Width="110px" Visible="true">
                                    </asp:DropDownList>
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ledger Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <center>
                                    <asp:DropDownList ID="ddl_ledgername" runat="server" CssClass="textbox ddlheight3"
                                        AutoPostBack="true" Width="110px" Visible="true">
                                    </asp:DropDownList>
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Payment" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <center>
                                    <asp:TextBox ID="txtpaymt" runat="server" onblur="return addmarks(this)" CssClass="  textbox txtheight1"
                                        Height="17px" Width="90px"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtpaymt"
                                        FilterType="Numbers,Custom" ValidChars=" .">
                                    </asp:FilteredTextBoxExtender>
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </center>
            <%--Library--%>
            <center>
                <div id="divLibrary" runat="server" style="width: 1077px; height: auto; background: white;
                    border: 1px solid #0095E8; overflow: auto;" visible="false">
                    <table>
                        <%--   //added by kowshika--%>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: 207px; width: 613px;">
                                    <legend style="height: 10">Library Rights</legend>
                                    <table>
                                        <center>
                                            <div id="divtable" runat="server" visible="false" style="width: 620px; height: 200px;
                                                background-color: White; border-radius: 10px; margin-top: -198px">
                                                <FarPoint:FpSpread ID="Fpload1" runat="server" Width="620px" Height="200px" EnableClientScript="true"
                                                    ActiveSheetViewIndex="0" CssClass="cursorptr" BorderColor="Black" BorderWidth="0.5">
                                                    <Sheets>
                                                        <FarPoint:SheetView SheetName="Sheet1">
                                                        </FarPoint:SheetView>
                                                    </Sheets>
                                                </FarPoint:FpSpread>
                                            </div>
                                        </center>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: 90px; width: 613px;">
                                    <legend style="height: 10">Library</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFine" runat="server" Text="OverDue Fine" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlLibFineHeader" runat="server" CssClass="textbox ddlheight2"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlLibFineHeader_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlLibFineLedger" runat="server" CssClass="textbox ddlheight2">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label22" runat="server" Text="Lost Book Fine" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlLibLostFineHeader" runat="server" CssClass="textbox ddlheight2"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlLibLostFineHeader_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlLibLostFineLedger" runat="server" CssClass="textbox ddlheight2">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <fieldset style="height: 40px; width: 613px;">
                                    <legend style="height: 10">Delete Option Setting</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cb_reservation_delete" runat="server" Text="Reservation" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cb_newrequest_delete" runat="server" Text="New Request" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: 40px; width: 613px;">
                                    <legend style="height: 10">Print Option Setting</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cb_reservation_print" runat="server" Text="Reservation" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cb_newrequest_print" runat="server" Text="New Request" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: 60px; width: 613px;">
                                    <legend style="height: 10">Special Circulation</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cb_specialissue" runat="server" Text="Special Issue" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cb_specialreturn" runat="server" Text="Special Return" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cb_editfine" runat="server" Text="Edit Fine" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cb_editcancelfine" runat="server" Text="Edit Cancel Fine" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: 174px; width: 613px;">
                                    <legend style="height: 10">Special Setting</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cb_opacprint" runat="server" Text="OPAC Print" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cb_manualcallnoentry" runat="server" Text="Manual Call No Entry" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cb_transactionwithbarcode" runat="server" Text="Transaction With Barcode" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cb_sendsmstostudentsforoverdue" runat="server" Text="Send SMS to students for over due" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cb_sendsmstostaffsforoverdue" runat="server" Text="Send SMS to staffs for over due" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cb_useclientsystemdateforuserinout" runat="server" Text="Use Client System Date for User InOut" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <%--   //added by kowshika--%>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: 228px; width: 971px;">
                                    <legend style="height: 10">Library</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkeditissue" runat="server" Text="Edit Issue Date" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkeditdue" runat="server" Text="Edit Due Date" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkreturn" runat="server" Text="Edit Return Date" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkboksts" runat="server" Text="Edit Book Status In OPAC Search" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkdis" runat="server" Text="Display Book Status In OPAC Search" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkviewbok" runat="server" Text="View Book Details In OPAC Search" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkdisretn" runat="server" Text="Display Return Message" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkdisiss" runat="server" Text="Display Issue Message" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkdisfine" runat="server" Text="Display Fine Message In Issue Return" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkdistofinebok" runat="server" Text="Display Toay Fine Books In Issue Return" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkboktran" runat="server" Text="Allow Book Transaction If Geats In Entry" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ckcalculatefine" runat="server" Text="Calculate Fine In Library Holidays" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="reslink" runat="server" Text="Reservation Link In OPAC" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="disallboksta" runat="server" Text="Display All Book Status And Return" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkclientsys" runat="server" Text="Use Client System Date For User In Out" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="resvadue" runat="server" Text="Reservation Due" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkautmatic" runat="server" Text="Automatic Staff Code Generation" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkpass" runat="server" Text="Password Settings For OPAC Exit" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkautomaticcardlock" runat="server" Text="Automatic Card Lock And Card Release" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkcallno" runat="server" Text="Callno Auto Increment" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkfinecal" runat="server" Text="Fine Calculation Exclude Holidays" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkduedate" runat="server" Text="Due Date Exclude Holidays" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkmultiple" runat="server" Text="Multiple Renewal Days" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkrenewal" runat="server" Text="Renewal Permission" AutoPostBack="true"
                                                    OnCheckedChanged="chkrenewal_OnCheckedChanged" />
                                                <asp:TextBox ID="txtrenewal" Visible="false" runat="server" CssClass="  textbox txtheight1"
                                                    Height="17px" Width="50px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtrenewal" runat="server"
                                                    TargetControlID="txtrenewal" FilterType="Numbers,Custom">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
            <%--App Rights--%>
            <center>
                <%--Deepali 16.7.18--%>
                <div id="divMblApp" runat="server" style="width: 980px; height: 400px; background: white;
                    border: 1px solid #0095E8; overflow: auto;" visible="false">
                    <table style="margin-right: 400px;">
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td colspan="2">
                                            <fieldset style="height: 177px; width: 250px;">
                                                <legend style="height: 10">Staff Tab Rights</legend>
                                                <asp:Panel ID="Panel24" runat="server" ScrollBars="Auto" Style="height: 160px;">
                                                    <asp:CheckBox ID="cb_appTab" runat="server" Text="All" onclick="CheckBoxListSelectMblAppTab(this);" />
                                                    <asp:CheckBoxList ID="cbl_appTab" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_appTab_OnSelectedIndexChanged">
                                                        <asp:ListItem Value="0">Attendance</asp:ListItem>
                                                        <asp:ListItem Value="1">Cam</asp:ListItem>
                                                        <asp:ListItem Value="2">Black Box</asp:ListItem>
                                                        <asp:ListItem Value="3">Notification</asp:ListItem>
                                                        <asp:ListItem Value="4">Time Table</asp:ListItem>
                                                        <asp:ListItem Value="5">Biometric</asp:ListItem>
                                                        <asp:ListItem Value="6">Student Login</asp:ListItem>
                                                        <asp:ListItem Value="7">Schedule Change</asp:ListItem>
                                                        <asp:ListItem Value="8">Hostel Attendance</asp:ListItem>
                                                        <asp:ListItem Value="9">Mess Attendance</asp:ListItem>
                                                        <asp:ListItem Value="10">Gate Pass</asp:ListItem>
                                                        <asp:ListItem Value="11">Mess Attendance Count</asp:ListItem>
                                                        <asp:ListItem Value="12">Transport Attendance Count</asp:ListItem>
                                                        <asp:ListItem Value="13">CAM Range Analysis</asp:ListItem>
                                                        <asp:ListItem Value="14">University Range Analysis</asp:ListItem>
                                                        <asp:ListItem Value="15">Salary Report</asp:ListItem>
                                                        <asp:ListItem Value="16">Hostel Report</asp:ListItem>
                                                        <asp:ListItem Value="17">Transport Report</asp:ListItem>
                                                        <asp:ListItem Value="18">Student Strength Report</asp:ListItem>
                                                        <asp:ListItem Value="19">Staff Strength Report</asp:ListItem>
                                                        <asp:ListItem Value="20">Attendance Chart</asp:ListItem>
                                                        <asp:ListItem Value="21">Arrear Chart</asp:ListItem>
                                                        <asp:ListItem Value="22">Student Attendance Report</asp:ListItem>
                                                        <asp:ListItem Value="23">Head Biometric Report</asp:ListItem>
                                                        <asp:ListItem Value="24">Institution Wise Balance Report</asp:ListItem>
                                                        <asp:ListItem Value="25">Detailed Fee Report</asp:ListItem>
                                                        <asp:ListItem Value="26">Head Notification</asp:ListItem>
                                                        <asp:ListItem Value="27">Cumulative Receipt Report</asp:ListItem>
                                                        <asp:ListItem Value="28">Head Black Box</asp:ListItem>
                                                        <asp:ListItem Value="29">Leave Apply</asp:ListItem>
                                                        <asp:ListItem Value="30">Leave Approval</asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td colspan="2">
                                            <fieldset style="height: 177px; width: 250px;">
                                                <legend style="height: 10">Student Tab Rights</legend>
                                                <%--Deepali 28.8.18--%>
                                                <asp:Panel ID="Panel8" runat="server" ScrollBars="Auto" Style="height: 160px;">
                                                    <asp:CheckBox ID="cb_Student_AppTab" runat="server" Text="All" onclick="CheckBoxListSelectStudentMblAppTab(this);" />
                                                    <asp:CheckBoxList ID="cbl_Student_AppTab" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_Student_AppTab_OnSelectedIndexChanged">
                                                        <asp:ListItem Value="0">Profile</asp:ListItem>
                                                        <asp:ListItem Value="1">Attendance</asp:ListItem>
                                                        <asp:ListItem Value="2">CAM</asp:ListItem>
                                                        <asp:ListItem Value="3">Time Table</asp:ListItem>
                                                        <asp:ListItem Value="4">Finance</asp:ListItem>
                                                        <asp:ListItem Value="5">Library</asp:ListItem>
                                                        <asp:ListItem Value="6">Hostel/Transport</asp:ListItem>
                                                        <asp:ListItem Value="7">Notification</asp:ListItem>
                                                        <asp:ListItem Value="8">Feedback</asp:ListItem>
                                                        <asp:ListItem Value="9">Lesson</asp:ListItem>
                                                        <asp:ListItem Value="10">Question Bank</asp:ListItem>
                                                        <asp:ListItem Value="11">Gate Pass</asp:ListItem>
                                                        <asp:ListItem Value="12">Leave Apply</asp:ListItem>
                                                        <asp:ListItem Value="13">Home Work</asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <%--Deepali 13.11.18--%>
                            <td>
                                <fieldset style="height: 177px; width: 355px;">
                                    <legend style="height: 10">Student App</legend>
                                    <table>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="Label19" runat="server" CssClass="commonHeaderFont" Text="Hour in Attendance"></asp:Label>
                                                <asp:TextBox ID="txt_FN_AN" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                                    ReadOnly="true">-- Select --</asp:TextBox>
                                                <asp:Panel ID="Panel_FN_AN" runat="server" ScrollBars="Auto" Style="height: 140px;
                                                    width: 75px;" CssClass="multxtpanel">
                                                    <asp:CheckBox ID="cb_FN_AN" runat="server" Text="All" onclick="cbl_FN_AN_selectAll(this);" />
                                                    <asp:CheckBoxList ID="cbl_FN_AN" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_FN_AN_OnSelectedIndexChanged" />
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_FN_AN"
                                                    PopupControlID="Panel_FN_AN" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label20" runat="server" CssClass="commonHeaderFont" Text="Show Due Fee by"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rbList_DueFeeMode" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Header" Value="0" />
                                                    <asp:ListItem Text="Ledger" Value="1" />
                                                    <asp:ListItem Text="Both" Value="2" Selected="True" />
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="right">
                                                <asp:Button ID="btnStu_app" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                                    ForeColor="White" Font-Bold="true" Style="width: 80px;" OnClick="btnStu_app_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <%--Deepali 13.11.18--%>
                        </tr>
                    </table>
                </div>
            </center>
            <%-- Pop Alert--%>
            <center>
                <div id="imgAlert" runat="server" visible="false" style="height: 1000px; z-index: 10000;
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
                                            <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_alertclose" CssClass=" textbox textbox1 btn1" Style="height: 28px;
                                                    width: 65px;" OnClick="btn_alertclose_Click" Text="ok" runat="server" />
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
</asp:Content>
