<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="LeaveApplySettings.aspx.cs"   MasterPageFile="~/StudentMod/StudentSubSiteMaster.master" 
    Inherits="LeaveApplySettings" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>Leave Apply Settings</title>
    <link href="../Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ScriptManager ID="scriptMrgr" runat="server"></asp:ScriptManager>
    <script>
        $(document).ready(function () {

            $('#<%=lblvalidation1.ClientID%>').text("");

            $('#<%=txtexcelname.ClientID%>').keypress(function () {
                $('#<%=lblvalidation1.ClientID%>').text("");
            });

            $('#<%=btnExcel.ClientID %>').click(function () {
                var val = $('#<%=txtexcelname.ClientID%>').val();
                if (val != "") {
                    $('#<%=lblvalidation1.ClientID%>').text("");
                    return true;
                }
                else {
                    $('#<%=lblvalidation1.ClientID%>').text("Please Enter Your Leave Setting Report Name");
                    $('#<%=lblvalidation1.ClientID%>').show();
                    return false;
                }

            });

            $('#<%=btnDeleteLeaveSet.ClientID %>').click(function () {
                if (confirm("Do You Want To Delete This Record?")) {
                    return true;
                }
                else
                    return false;
            });

        });
        function normalBorder(id) {
            id.style.borderColor = "#c4c4c4";
        }
    </script>
    <div>
        <center>
            <span style="font-family: Book Antiqua; font-size: 25px; font-weight: bold; color: Green;">
                Leave Apply Settings </span>
        </center>
    </div>
    <div>
        <center>
            <div class="maindivstyle" style="width: 950px; height: 560px;">
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtclg" runat="server" Style="height: 20px; width: 250px;" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlclg" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 270px;
                                        height: 120px;">
                                        <asp:CheckBox ID="cbclg" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cbclg_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="cblclg" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblclg_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtclg"
                                        PopupControlID="pnlclg" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lblDept" runat="server" Text="Branch"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="Up_dept" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel multxtpanleheight">
                                        <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_dept_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pce_dept" runat="server" TargetControlID="txt_dept"
                                        PopupControlID="panel_dept" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td colspan="2">
                            <asp:Button ID="btngo" runat="server" Height="30px" Width="80px" CssClass="textbox textbox1 btn3"
                                Text="Go" OnClick="btngo_Click" />
                            <asp:Button ID="btnadd" runat="server" Height="30px" Width="80px" CssClass="textbox textbox1 btn3"
                                Text="Add" OnClick="btnadd_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <div id="divspread" runat="server" visible="false" style="height: 400px; overflow: auto;">
                    <FarPoint:FpSpread ID="fpreport" runat="server" Visible="true" BorderStyle="Solid"
                        BorderWidth="0px" Width="930px" Style="overflow: auto; border: 0px solid #999999;
                        border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                        class="spreadborder" OnCellClick="fpreport_OnCellClick" OnPreRender="fpreport_Selectedindexchanged">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="Green">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <br />
                <div>
                    <center>
                        <div id="print" runat="server" visible="false">
                            <asp:Label ID="lblvalidation1" runat="server" Style="margin-top: -18px; display: none;
                                margin-left: 10px; position: absolute;" Font-Names="Book Antiqua" Font-Size="Medium"
                                ForeColor="Red" Text=""></asp:Label>
                            <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display(this)"
                                CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                InvalidChars="/\">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnExcel" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                OnClick="btnExcel_Click" Text="Export To Excel" Width="127px" Height="32px" CssClass="textbox textbox1" />
                            <asp:Button ID="btnprintmasterhed" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Text="Print" OnClick="btnprintmaster_Click" Height="32px" Style="margin-top: 10px;"
                                CssClass="textbox textbox1" Width="60px" />
                            <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                        </div>
                    </center>
                </div>
            </div>
        </center>
        <%--add screen--%>
        <div id="divadd" runat="server" visible="false" style="height: 44em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
            left: 0;">
            <br />
            <center>
                <div style="height: 460px; width: 850px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA;
                    border-radius: 10px; background-color: White;">
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                        width: 30px; margin-left: 97%; position: relative; top: -40px;" OnClick="imagepopclose_click" />
                    <span style="font-family: Book Antiqua; font-size: 25px; font-weight: bold; color: Green;">
                        Add Settings </span>
                    <br />
                    <table class="maintablestyle" width="800px">
                        <tr>
                            <td>
                                <asp:Label ID="addlblclg" runat="server" Text="College"></asp:Label>
                            </td>
                            <td id="tdddl" runat="server" visible="false">
                                <asp:DropDownList ID="addddlclg" CssClass="textbox textbox1 ddlheight4" Height="25px"
                                    Width="250px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="addddlclg_IndexChange">
                                </asp:DropDownList>
                            </td>
                            <td id="tdcbl" runat="server" visible="false">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="addtxtclg" runat="server" Style="height: 20px; width: 250px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="addpnlhedg" runat="server" CssClass="multxtpanel multxtpanleheight"
                                            Style="width: 270px; height: 120px;">
                                            <asp:CheckBox ID="addcbhedg" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="addcbhedg_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="addcblhedg" runat="server" AutoPostBack="True" OnSelectedIndexChanged="addcblhedg_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="addtxtclg"
                                            PopupControlID="addpnlhedg" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblDept2" runat="server" Text="Branch"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updept2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtDept2" runat="server" CssClass="textbox txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlDept2" runat="server" CssClass="multxtpanel multxtpanleheight">
                                            <asp:CheckBox ID="cb_dept2" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_dept2_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_dept2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept2_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtDept2"
                                            PopupControlID="pnlDept2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rbmode" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                    OnSelectedIndexChanged="rbmode_OnSelected">
                                    <asp:ListItem Text="Single" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Multiple" Value="1"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table>
                        <tr>
                            <td colspan="2">
                                Maximum Number Of Leave Settings For Student
                            </td>
                            <td>
                                <asp:TextBox ID="txtMaxLeaveSet" runat="server" CssClass="textbox" Style="height: 20px;
                                    width: 50px;" MaxLength="2" onfocus="normalBorder(this)"></asp:TextBox>
                                <span style="color: Red;">*</span>
                                <asp:FilteredTextBoxExtender ID="fceMaxLeaveSet" runat="server" TargetControlID="txtMaxLeaveSet"
                                    FilterType="Numbers">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIncFinance" runat="server" Text="Include Finance" AutoPostBack="true"
                                    OnCheckedChanged="chkIncFinance_CheckedChange" Checked="false" Enabled="false" />
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblFinHeader" runat="server" Text="Header" Visible="false"></asp:Label>
                                <asp:DropDownList ID="ddlFinHeader" runat="server" CssClass="textbox " Width="120px"
                                    Height="30px" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlFinHeader_OnSelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:Label ID="lblFinLedger" runat="server" Text="Ledger" Visible="false"></asp:Label>
                                <asp:DropDownList ID="ddlFinLedger" runat="server" CssClass="textbox " Width="120px"
                                    Height="30px" Visible="false">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Fine Settings
                            </td>
                            <td>
                                <script type="text/javascript">
                                    function FinSetAdd() {
                                        var val = document.getElementById("<%=txtFineSet.ClientID %>").value.trim();
                                        if (val == "" || val == "0" || val == "00") {
                                            document.getElementById("<%=txtFineSet.ClientID %>").style.borderColor = "Red";
                                            return false;
                                        }
                                        return true;
                                    }
                            
                                </script>
                                <asp:TextBox ID="txtFineSet" runat="server" CssClass="textbox" Style="height: 20px;
                                    width: 50px;" MaxLength="2" onfocus="normalBorder(this)"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="fceFineSet" runat="server" TargetControlID="txtFineSet"
                                    FilterType="Numbers">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="btnAddFineRow" runat="server" Height="30px" Width="80px" CssClass="textbox textbox1 btn3"
                                    Text="Add" OnClick="btnAddFineRow_Click" OnClientClick="return FinSetAdd();" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div style="width: 420px; height: 200px; overflow: auto;">
                                    <asp:GridView ID="fineSetGrid" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                        GridLines="None" Width="400px">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="LightBlue">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblsno" runat="server" ForeColor="Brown" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle BackColor="LightBlue"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="From (Days)" HeaderStyle-BackColor="LightBlue">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtFineDaysFrom" runat="server" CssClass="textbox" Style="height: 20px;
                                                        width: 50px;" MaxLength="2" onfocus="normalBorder(this)" Text='<%#Eval("DaysFrom") %>'></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="fceFineDaysFrom" runat="server" TargetControlID="txtFineDaysFrom"
                                                        FilterType="Numbers">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <HeaderStyle BackColor="LightBlue"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" Width="70px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="To (Days)" HeaderStyle-BackColor="LightBlue">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtFineDaysTo" runat="server" CssClass="textbox" Style="height: 20px;
                                                        width: 50px;" MaxLength="2" onfocus="normalBorder(this)" Text='<%#Eval("DaysTo") %>'></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="fceFineDaysTo" runat="server" TargetControlID="txtFineDaysTo"
                                                        FilterType="Numbers">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <HeaderStyle BackColor="LightBlue"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Amount" HeaderStyle-BackColor="LightBlue">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtFineAmount" runat="server" CssClass="textbox" Style="height: 20px;
                                                        width: 80px;" MaxLength="14" onfocus="normalBorder(this)" Text='<%#Eval("Amount") %>'></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="fceFineAmt" runat="server" TargetControlID="txtFineAmount"
                                                        FilterType="Custom,Numbers" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <HeaderStyle BackColor="LightBlue"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <center>
                                    <asp:Button ID="btnSaveLeaveSet" runat="server" Height="30px" Width="80px" CssClass="textbox textbox1 btn3"
                                        Text="Save" OnClick="btnSaveLeaveSet_Click" OnClientClick="return SaveLeaveSet();" />
                                    <asp:Button ID="btnUpdateLeaveSet" runat="server" Height="30px" Width="80px" CssClass="textbox textbox1 btn3"
                                        Text="Update" OnClick="btnUpdateLeaveSet_Click" OnClientClick="return SaveLeaveSet();"
                                        Visible="false" />
                                    <asp:Button ID="btnDeleteLeaveSet" runat="server" Height="30px" Width="80px" CssClass="textbox textbox1 btn3"
                                        Text="Delete" OnClick="btnDeleteLeaveSet_Click" OnClientClick="return SaveLeaveSet();"
                                        Visible="false" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
        </div>
        <%--alertmsg--%>
        <center>
            <div id="imgalert" runat="server" visible="false" style="height: 200%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 216px;
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
                                            <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" />
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
</asp:Content>