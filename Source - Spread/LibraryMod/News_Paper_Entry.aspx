<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="News_Paper_Entry.aspx.cs" Inherits="LibraryMod_News_Paper_Entry" %>

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
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">News Paper Multiple Entry</span></div>
        </center>
    </div>
    <div>
        <center>
            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                <ContentTemplate>
                    <div id="maindiv" runat="server" class="maindivstyle" style="width: 990px; height: auto">
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <center>
                                            <div style="width: 1000px; height: auto">
                                                <table class="maintablestyle" style="height: auto; margin-left: -15px; margin-top: 10px;
                                                    margin-bottom: 10px; padding: 6px; font-family: Book Antiqua; font-weight: bold">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbllibrary" runat="server" Text="Library:">
                                                            </asp:Label>
                                                            <asp:DropDownList ID="ddllibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddllibrary_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_year" runat="server" Text="Sub.Year:">
                                                            </asp:Label>
                                                            <asp:DropDownList ID="ddl_year" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Width="130px" AutoPostBack="True" OnSelectedIndexChanged="ddl_year_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_fromdate" runat="server" Text="From Date:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox textbox1 txtheight2"
                                                                AutoPostBack="true" OnTextChanged="txt_fromdate_TextChanged"></asp:TextBox>
                                                            <asp:CalendarExtender ID="cext_fromdate" TargetControlID="txt_fromdate" runat="server"
                                                                Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_todate" runat="server" Text="To Date:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox textbox1  txtheight2"
                                                                AutoPostBack="true" OnTextChanged="txt_todate_TextChanged"></asp:TextBox>
                                                            <asp:CalendarExtender ID="cext_todate" TargetControlID="txt_todate" runat="server"
                                                                Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpGo" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:ImageButton ID="Btngo" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="btngo_Click" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div style="width: 1000px; height: auto">
                                                <table class="maintablestyle" style="height: auto; margin-left: -15px; margin-top: 10px;
                                                    margin-bottom: 10px; padding: 6px; font-family: Book Antiqua; font-weight: bold">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_suptype" runat="server" Text="Supplier Type"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtsuptype" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                                ReadOnly="true" Width="100px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                                                                Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                            <asp:Panel ID="psuptype" runat="server" CssClass="multxtpanel" Width="125px">
                                                                <asp:CheckBox ID="chksuptype" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chksuptype_CheckedChanged"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                                <asp:CheckBoxList ID="chklsuptype" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                    Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsuptype_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtsuptype"
                                                                PopupControlID="psuptype" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblsupname" runat="server" Text="Supplier Name"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtsupname" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                                ReadOnly="true" Width="100px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                                                                Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                            <asp:Panel ID="psupname" runat="server" CssClass="multxtpanel" Width="125px">
                                                                <asp:CheckBox ID="chksupname" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chksupname_CheckedChanged"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                                <asp:CheckBoxList ID="chklsupname" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                    Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsupname_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtsupname"
                                                                PopupControlID="psupname" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblang" runat="server" Text="Language"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtlang" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                                ReadOnly="true" Width="100px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                                                                Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                            <asp:Panel ID="plang" runat="server" CssClass="multxtpanel" Width="125px">
                                                                <asp:CheckBox ID="chklang" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chklang_CheckedChanged"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                                <asp:CheckBoxList ID="chkllang" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                    Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chkllang_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtlang"
                                                                PopupControlID="plang" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_jname" runat="server" Text="Journal Name"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Txt_jname" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                                ReadOnly="true" Width="100px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                                                                Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                            <asp:Panel ID="Pjname" runat="server" CssClass="multxtpanel" Width="125px">
                                                                <asp:CheckBox ID="chkjname" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chkjname_CheckedChanged"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                                <asp:CheckBoxList ID="chkljname" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                    Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chkljname_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="Txt_jname"
                                                                PopupControlID="pjname" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
        <br />
        <br />
        <center>
            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                <ContentTemplate>
                    <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Style="overflow: auto; border: 0px solid #999999; border-radius: 10px;
                        background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder"
                        DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                        EnableClientScript="true" Pager-Align="Right" Pager-ButtonType="ImageButton"
                        CommandBar-ButtonType="ImageButton" CommandBar-Visible="False" Pager-Mode="Both"
                        Pager-Position="Bottom" Pager-PageCount="10" Visible="false" OnUpdateCommand="FpSpread1_UpdateCommand">
                        <%--    OnUpdateCommand="FpSpread1_UpdateCommand"--%>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <br />
            <center>
                <div>
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lbl_total_journal" runat="server" Visible="false" Style="text-align: left;
                                margin-left: -453px; margin-top: 2px; background-color: Aqua;"></asp:Label>
                            <asp:ImageButton ID="btn_Save" runat="server" ImageUrl="~/LibImages/save.jpg" Visible="false"
                                Style="margin-left: 326px; margin-top: 2px;" OnClick="btn_Save_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </center>
            <br />
            <br />
            <%--<div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                        Visible="false"></asp:Label>
                    <asp:Label ID="lblrptname" runat="server" Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" onkeypress="display()"
                        Font-Size="Medium" CssClass="textbox textbox1"></asp:TextBox>
                    <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox textbox1 btn2"
                        Text="Export To Excel" Width="127px" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        Width="60px" CssClass="textbox textbox1 btn2" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </div>--%>
        </center>
    </div>
    <center>
        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
            <ContentTemplate>
                <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <br />
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:UpdatePanel ID="UpdatePanelbtn2" runat="server">
                                                    <ContentTemplate>
                                                        <asp:ImageButton ID="btnerrclose" runat="server" ImageUrl="~/LibImages/ok.jpg" OnClick="btnerrclose_Click" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
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
    <%--progressBar for GO--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpGo">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                            Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
