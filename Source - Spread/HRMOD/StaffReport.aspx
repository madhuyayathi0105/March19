<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="StaffReport.aspx.cs" Inherits="StaffReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div style="left: -5px;">
            <br />
            <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="top: 85px;
                left: -15px; position: absolute; width: 1045px; height: 21px; margin-bottom: 0px;">
                &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" Text="Staff Report" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="White"></asp:Label>&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <br />
                <br />
                <br />
            </asp:Panel>
        </div>
        <br />
        <br />
        <div>
            <center>
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lbldepartment" runat="server" Text="Department" Width="100px" ForeColor="Black"
                                Font-Size="Medium" Font-Names="Book Antiqua" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="tbseattype" runat="server" Height="20px" ReadOnly="true" Width="150px"
                                        Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="pseattype" runat="server" Height="400px" CssClass="multxtpanel" Style="margin-left: 10px;">
                                        <asp:CheckBox ID="chkselect" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnCheckedChanged="chkselect_CheckedChanged" Text="Select All"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cbldepttype" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Width="343px" OnSelectedIndexChanged="cbldepttype_SelectedIndexChanged" Height="401px"
                                            Font-Bold="True" Font-Names="Book Antiqua">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="tbseattype"
                                        PopupControlID="pseattype" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbldesignation" runat="server" Text="Designation" Font-Bold="True"
                                ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Width="100px"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtdesi" runat="server" Height="20px" ReadOnly="true" Width="150px"
                                        Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="Paneldesi" runat="server" Height="400px" CssClass="multxtpanel" Style="margin-left: 10px;">
                                        <asp:CheckBox ID="chkdesi" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnCheckedChanged="chkdesi_CheckedChanged" Text="Select All"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cbldesi" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Width="343px" OnSelectedIndexChanged="cbldesi_SelectedIndexChanged" Height="401px"
                                            Font-Bold="True" Font-Names="Book Antiqua">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtdesi"
                                        PopupControlID="Paneldesi" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lblcategory" runat="server" Text="Category" Width="90px" Font-Bold="True"
                                ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtcategory" runat="server" Height="20px" ReadOnly="true" Width="151px"
                                        Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="panelcategory" runat="server" Height="175px" Width="200px" CssClass="multxtpanel">
                                        <asp:CheckBox ID="chkcategory" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnCheckedChanged="chkcategory_CheckedChanged" Text="Select All"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cblcategory" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Width="158px" OnSelectedIndexChanged="cblcategory_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                            Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtcategory"
                                        PopupControlID="panelcategory" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblstafftype" runat="server" Text="Staff Type" Width="100px" Font-Bold="True"
                                ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtstafftype" runat="server" Height="20px" ReadOnly="true" Width="151px"
                                        Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="panelstafftype" runat="server" Height="175px" Width="200px" CssClass="multxtpanel">
                                        <asp:CheckBox ID="chkstafftype" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnCheckedChanged="chkstafftype_CheckedChanged" Text="Select All"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cblstafftype" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Width="158px" OnSelectedIndexChanged="cblstafftype_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                            Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtstafftype"
                                        PopupControlID="panelstafftype" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Button ID="btgo" runat="server" Text="GO" OnClick="btgo_Click" ForeColor="Black"
                                Font-Size="Medium" Font-Names="Book Antiqua" Font-Bold="True" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblcomp" runat="server" Width="500px" Font-Bold="True" ForeColor="#FF3300"
                                Font-Names="Book Antiqua" Font-Size="Small" Style="left: 431px; top: 157px; position: absolute"></asp:Label>
                        </td>
                    </tr>
                </table>
            </center>
        </div>
        <asp:Panel ID="Panel3" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="16px"
            Style="margin-left: 0px; top: 210px; left: -4px; position: absolute; width: 1169px;">
        </asp:Panel>
        <br />
        <br />
        <center>
        <asp:Label ID="lblother" runat="server" Visible="False" ForeColor="Red" Font-Size="Medium"
            Font-Names="Book Antiqua" Font-Bold="True"></asp:Label>
        </center>
        <br />
        <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
            BorderWidth="1px" Height="200" Width="400">
            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                ButtonShadowColor="ControlDark">
            </CommandBar>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
        <div>
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnexportexcel" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True" Text="Export To Excel" OnClick="btnexportexcel_Click" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" Style="position: absolute;
                            left: 443px;" OnClick="btnprintmaster_Click" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </td>
                </tr>
            </table>
        </div>
    </body>
    </html>
</asp:Content>
