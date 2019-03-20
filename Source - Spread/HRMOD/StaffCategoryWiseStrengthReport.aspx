<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="StaffCategoryWiseStrengthReport.aspx.cs" Inherits="HRMOD_StaffCategoryWiseStrengthReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Month Wise Staff Strength Report</span>
            </div>
        </center>
        <br />
        <center>
            <div>
                <table class="maintablestyle" width="800px">
                    <tr>
                        <td>
                            <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_college" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                        Style="font-weight: bold; width: 135px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                    <asp:Panel ID="p1" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                        border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                        box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                        <asp:CheckBox ID="cb_college" runat="server" Text="Select All" OnCheckedChanged="cb_college_CheckedChange"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="cbl_college" runat="server" OnSelectedIndexChanged="cbl_college_SelectedIndexChange"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_college"
                                        PopupControlID="p1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lblStaffCategory" runat="server" Text="Staff Category" Font-Bold="true"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_Category" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                        Style="font-weight: bold; width: 135px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel1" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                        position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                        height: 200px;">
                                        <asp:CheckBox ID="cb_Category" runat="server" Text="Select All" OnCheckedChanged="cb_Category_CheckedChange"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="cbl_Category" runat="server" OnSelectedIndexChanged="cbl_Category_SelectedIndexChange"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_Category"
                                        PopupControlID="Panel1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lblStaffType" runat="server" Text="Staff Type" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_stafftype" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                        Style="font-weight: bold; width: 135px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel2" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                        position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                        height: 200px;">
                                        <asp:CheckBox ID="cb_stafftype" runat="server" Text="Select All" OnCheckedChanged="cb_stafftype_CheckedChange"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="cbl_stafftype" runat="server" OnSelectedIndexChanged="cbl_stafftype_SelectedIndexChange"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_stafftype"
                                        PopupControlID="Panel2" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblMonthyear" runat="server" Text="Month & Year" Font-Bold="true"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_Month" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                        Style="font-weight: bold; width: 100px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel3" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                        Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                        position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                        height: 200px; width: 120px;">
                                        <asp:CheckBox ID="cb_Month" runat="server" Text="Select All" OnCheckedChanged="cb_Month_CheckedChange"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="cbl_Month" runat="server" OnSelectedIndexChanged="cbl_Month_SelectedIndexChange"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_Month"
                                        PopupControlID="Panel3" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlyear" runat="server" CssClass="textbox  ddlheight">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btnGo" runat="server" CssClass="textbox textbox1 btn2" Text="Go"
                                OnClick="btnGo_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </center>
        <br />
        <center>
            <div>
                <FarPoint:FpSpread ID="Farpont1" runat="server" Visible="false" BorderColor="Gray"
                    BorderStyle="Solid" BorderWidth="1px">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
            </div>
            <center>
                <div id="print" runat="server" visible="false">
                    <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                        ForeColor="Red" Text="" Visible="false"></asp:Label>
                    <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                        Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display()"
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
        </center>
    </div>
</asp:Content>
