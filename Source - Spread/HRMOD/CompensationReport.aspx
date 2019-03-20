<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="CompensationReport.aspx.cs" Inherits="HRMOD_CompensationReport" EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <script type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById("<%=lblsmserror.ClientID %>").innerHTML = "";
            }

            function checkFloatValue(el) {
                var ex = /^[0-9]+\.?[0-9]*$/;
                if (ex.test(el.value) == false) {
                    el.value = "";
                }
            }

        
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <center>
                    <br />
                    <div>
                        <span class="fontstyleheader" style="color: Green;">Compensation Leave Report</span>
                    </div>
                </center>
                <fieldset id="maindiv" runat="server" style="width: 1000px; margin-left: 0px; height: 1100px;
                    border-color: silver; border-radius: 10px;">
                    <fieldset style="background-color: #0ca6ca; border: 1px solid #ccc; border-radius: 10px;
                        box-shadow: 0 0 8px #999999; height: 75px; margin-left: 0px; margin-top: 8px;
                        padding: 1em; margin-left: 0px; width: 950px;">
                        <table style="margin-top: -14px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_College" runat="server" Text="College" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanelclg" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_clg" runat="server" CssClass="textbox txtheight1" ReadOnly="true"
                                                Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="p1" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                                border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                                box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;
                                                width: 250px;">
                                                <asp:CheckBox ID="cb_clg" runat="server" Text="Select All" OnCheckedChanged="cb_clg_CheckedChange"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_clg" runat="server" OnSelectedIndexChanged="cbl_clg_SelectedIndexChange"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_clg"
                                                PopupControlID="p1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_dept" runat="server" Text="Department" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox txtheight1" ReadOnly="true"
                                                Style="font-weight: bold; width: 135px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel1" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                                position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                                height: 200px; width: 250px;">
                                                <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" OnCheckedChanged="cb_dept_CheckedChange"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_dept" runat="server" OnSelectedIndexChanged="cbl_dept_SelectedIndexChange"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_dept"
                                                PopupControlID="Panel1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_desig" runat="server" Text="Designation" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_desig" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                Style="font-weight: bold; width: 130px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="P2" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                                border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                                box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;
                                                width: 250px;">
                                                <asp:CheckBox ID="cb_desig" runat="server" Text="Select All" OnCheckedChanged="cb_desig_CheckedChange"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_desig" runat="server" OnSelectedIndexChanged="cbl_desig_SelectedIndexChange"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_desig"
                                                PopupControlID="P2" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lbl_staffc" runat="server" Text="Staff Category" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_staffc" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="P3" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                                border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                                box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                                <asp:CheckBox ID="cb_staffc" runat="server" Text="Select All" OnCheckedChanged="cb_staffc_CheckedChange"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_staffc" runat="server" OnSelectedIndexChanged="cbl_staffc_SelectedIndexChange"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_staffc"
                                                PopupControlID="P3" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_stype" runat="server" Text="Staff Type" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_stype" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="P4" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                                border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                                box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                                <asp:CheckBox ID="cb_stype" runat="server" Text="Select All" OnCheckedChanged="cb_stype_CheckedChange"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_stype" runat="server" OnSelectedIndexChanged="cbl_stype_SelectedIndexChange"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_stype"
                                                PopupControlID="P4" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_staffCode" runat="server" Text="Staff Name" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_staffCode" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                Style="font-weight: bold; width: 133px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="pstaffcode" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                                position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                                height: 200px;">
                                                <asp:CheckBox ID="cb_staffCode" runat="server" Text="Select All" OnCheckedChanged="cb_staffcode_CheckedChange"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_staffCode" runat="server" OnSelectedIndexChanged="cbl_staffcode_SelectedIndexChange"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_staffCode"
                                                PopupControlID="pstaffcode" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblfrom" runat="server" Text="From Date" Width="80px" Font-Bold="true"
                                        ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtfrom" runat="server" Font-Bold="true" AutoPostBack="true" Width="80px"
                                                ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua" OnTextChanged="txtfrom_TextChanged"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtfrom" runat="server"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblto" runat="server" Text="To Date" Font-Bold="true" Width="80px"
                                        ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtto" runat="server" Font-Bold="true" AutoPostBack="true" Width="80px"
                                                ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua" OnTextChanged="txtto_TextChanged"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtto" runat="server"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    </td>
                                    <td>
                                    <asp:Button ID="btn_go" runat="server" Text="Go" OnClick="btn_go_Click" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <br />
                    <div id="sp_div" runat="server">
                        <FarPoint:FpSpread ID="FpSpread" runat="server" Visible="false" BorderColor="Black"
                            BorderStyle="Solid" BorderWidth="1px" Width="925px" Height="800px" Style="margin-left: 2px;"
                            class="spreadborder" OnButtonCommand="FpSpread_Command" ShowHeaderSelection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    </br>
                    <center>
                        <div id="rprint" runat="server">
                            <asp:Label ID="lblsmserror" Text="Please Enter Your Report Name" Font-Size="Large"
                                Font-Names="Book Antiqua" Visible="false" ForeColor="Red" runat="server" Font-Bold="true"></asp:Label>
                            <asp:Label ID="lblexcel" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Visible="true"></asp:Label>
                            <asp:TextBox ID="txtexcel" onkeypress="display()" CssClass="textbox textbox1" runat="server"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcel"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnexcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                CssClass="textbox textbox1 btn2" Width="150px" Text="Export Excel" OnClick="btnexcel_Click" />
                            <asp:Button ID="btnprintmaster" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Text="Print" OnClick="btnprintmaster_Click" CssClass="textbox textbox1 btn2"
                                Width="100px" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                    </center>
                </fieldset>
            </div>
        </center>
        <center>
            <div id="divPopAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                left: 0%;">
                <center>
                    <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                        height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%; padding: 5px;">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                                Text="Ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
    </body>
    </html>
</asp:Content>
