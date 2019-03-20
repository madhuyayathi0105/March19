<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="AllowanceAndDeductionReport.aspx.cs" Inherits="HRMOD_AllowanceAndDeductionReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        function display() {
            document.getElementById("<%=lblvalidation1.ClientID %>").innerHTML = "";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span id="sphd" runat="server" class="fontstyleheader" style="color: Green;">Salary
                        Abstract Report</span>
                </div>
            </center>
        </div>
        <div>
            <center>
                <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtclg" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlclg" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 350px;
                                            height: 200px;">
                                            <asp:CheckBox ID="cbclg" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cbclg_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cblclg" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblclg_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txtclg"
                                            PopupControlID="pnlclg" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                Month
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlmonth" runat="server" CssClass="ddlheight textbox1" Width="70px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Year
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlyear" runat="server" CssClass="ddlheight textbox1" Width="90px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Allownace
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtallow" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 210px;
                                            height: 120px;">
                                            <asp:CheckBox ID="cballow" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cballow_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cblallow" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblallow_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtallow"
                                            PopupControlID="Panel1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                Deduction
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtdeduct" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 210px;
                                            height: 200px;">
                                            <asp:CheckBox ID="cbdeduct" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cbdeduct_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbldeduct" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbldeduct_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtdeduct"
                                            PopupControlID="Panel2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_staffc" runat="server" Text="Category"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_staffc" runat="server" ReadOnly="true" CssClass="textbox textbox1 txtheight1"
                                            Style="width: 120px;">--Select--</asp:TextBox>
                                        <asp:Panel ID="P3" runat="server" CssClass="multxtpanel" Height="200px" Width="196px">
                                            <asp:CheckBox ID="cb_staffc" runat="server" Text="Select All" OnCheckedChanged="cb_staffc_CheckedChange"
                                                AutoPostBack="true" />
                                            <asp:CheckBoxList ID="cbl_staffc" runat="server" OnSelectedIndexChanged="cbl_staffc_SelectedIndexChange"
                                                AutoPostBack="true">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_staffc"
                                            PopupControlID="P3" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_stype" runat="server" Text="Staff Type"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_stype" runat="server" ReadOnly="true" CssClass="textbox textbox1 txtheight1"
                                            Style="width: 120px;">--Select--</asp:TextBox>
                                        <asp:Panel ID="P4" runat="server" CssClass="multxtpanel" Height="200px">
                                            <asp:CheckBox ID="cb_stype" runat="server" Text="Select All" OnCheckedChanged="cb_stype_CheckedChange"
                                                AutoPostBack="true" />
                                            <asp:CheckBoxList ID="cbl_stype" runat="server" OnSelectedIndexChanged="cbl_stype_SelectedIndexChange"
                                                AutoPostBack="true">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_stype"
                                            PopupControlID="P4" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                            <asp:RadioButton ID="Radio_cumulative" runat="server" AutoPostBack="true" Text="Cummulative" Checked="true" OnCheckedChanged="radioCumCheckedChange"/> <%--delsi--%>

                            </td>
                            <td>
                             <asp:RadioButton ID="Radio_detail" runat="server" AutoPostBack="true" Text="Detail" OnCheckedChanged="radioDetCheckedChange"/>
                            </td>
                            <td>
                                <asp:Button ID="btngo" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btngo_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div id="Deduction" runat="server" visible="false">
                        <FarPoint:FpSpread ID="DeductionDetSp" runat="server" BorderStyle="Solid" BorderWidth="0px"
                            Width="980px" Style="overflow: auto; border: 0px solid #999999; border-radius: 10px;
                            background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <center>
                            <br />
                            <div id="print">
                                <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                                <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display()"
                                    CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_ "
                                    InvalidChars="/\">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Export To Excel"
                                    Width="127px" Height="32px" CssClass="textbox textbox1" />
                                <asp:Button ID="btnprintmasterhed" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                    Height="32px" Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                                <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                            </div>
                        </center>
                    </div>
                    <br />
                </div>
            </center>
        </div>
    </body>
</asp:Content>
