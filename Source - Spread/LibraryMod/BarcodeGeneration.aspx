<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="BarcodeGeneration.aspx.cs" Inherits="LibraryMod_BarcodeGeneration" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <%--<link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />--%>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <script type="text/javascript">
            function display() {
                document.getElementById('MainContent_errmsg').innerHTML = "";
            }
        </script>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">BarCode Label Generation</span>
                </div>
            </center>
        </div>
        <center>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto;
                        font-family: Book Antiqua; font-weight: bold">
                        <table class="maintablestyle" style="width: 1000px; height: auto; font-family: Book Antiqua;
                            font-weight: bold">
                            <tr>
                                <td>
                                    <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        AutoPostBack="true">
                                        <%--OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged"--%>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblLib" runat="server" Text="Library"></asp:Label>
                                    <%--Style="margin-left: -50px"--%>
                                </td>
                                <td style="width: 73px">
                                    <asp:DropDownList ID="ddl_library" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Style="width: 200px;" AutoPostBack="true">
                                        <%--OnSelectedIndexChanged="ddl_library_SelectedIndexChanged"--%>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_department" Text="Department" Style="margin-left: 30px;" runat="server"></asp:Label><%--Style="margin-left: 30px"--%>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UP_department" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_department" runat="server" Style="height: 20px; width: 150px;"
                                                ReadOnly="true">--Select--</asp:TextBox><%--margin-right: -40px;--%>
                                            <asp:Panel ID="panel_department" runat="server" CssClass="multxtpanel" Style="width: 170px;
                                                height: auto;">
                                                <asp:CheckBox ID="cb_department" runat="server" Width="200px" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_department_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_department" runat="server" AutoPostBack="true">
                                                    <%--OnSelectedIndexChanged="cbl_department_OnSelectedIndexChanged"--%>
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="pce_department" runat="server" TargetControlID="txt_department"
                                                PopupControlID="panel_department" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRackno" runat="server" Text="Rack No"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_RackNo" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblShelfNo" runat="server" Text="Shelf No"></asp:Label>
                                    <%--Style="margin-left: -50px"--%>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_ShelfNo" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Style="width: 200px;" AutoPostBack="true">
                                        <%--margin-left: -130px;--%>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chk_datewise" runat="server" OnCheckedChanged="chk_datewise_OnCheckedChanged"
                                        Style="margin-left: 30px;" AutoPostBack="true" />
                                    <asp:Label ID="lbl_fromdate" runat="server" Text="From" Style="margin-top: 10px;
                                        padding-left: 10px;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_fromdate" runat="server" Style="height: 20px; width: 75px;"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                    <asp:Label ID="lbl_todate" runat="server" Text="To" Style="margin-left: 4px;"></asp:Label>
                                    <asp:TextBox ID="txt_todate" runat="server" Style="height: 20px; width: 75px; margin-left: 6px;"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpPrint" runat="server">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="BtnBarCodePrint" runat="server" ImageUrl="~/LibImages/Barcode print.jpg"
                                                OnClick="BtnBarCodePrint_click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSearchby" runat="server" Text="Search By"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_Search" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        OnSelectedIndexChanged="ddl_Search_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem>All</asp:ListItem>
                                        <asp:ListItem>Access No</asp:ListItem>
                                        <asp:ListItem>Call No</asp:ListItem>
                                        <asp:ListItem>Title</asp:ListItem>
                                        <asp:ListItem>Author</asp:ListItem>
                                        <asp:ListItem>Status</asp:ListItem>
                                        <asp:ListItem>Subject</asp:ListItem>
                                        <asp:ListItem>Bill No</asp:ListItem>
                                        <asp:ListItem>Purchased</asp:ListItem>
                                        <asp:ListItem>Category</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <span style="overflow: hidden;" id="sp1" runat="server">
                                    <td id="td_Chbtw" runat="server" visible="false">
                                        <asp:CheckBox ID="chk_between" runat="server" OnCheckedChanged="chk_between_OnCheckedChanged"
                                            AutoPostBack="true" />
                                        <asp:Label ID="Lblfrom" Text="From" runat="server"></asp:Label>
                                    </td>
                                    <td id="td_searchby" runat="server" visible="false">
                                        <asp:TextBox ID="txt_from" type="text" runat="server" placeholder="Access No" Style="height: 24px;
                                            width: 100px" Visible="true" />
                                    </td>
                                </span>
                                <td id="td_ChTo" runat="server" visible="false">
                                    <asp:Label ID="lblTo" Text="To" runat="server"></asp:Label>
                                </td>
                                <td id="td_txtTo" runat="server" visible="false">
                                    <asp:TextBox ID="txt_To" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Height="24px" Width="100px" Visible="true"></asp:TextBox>
                                </td>
                                <td id="td_status" runat="server" visible="false">
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="UpSearch" runat="server">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="btn_search" runat="server" ImageUrl="~/LibImages/search.jpg"
                                                OnClick="btnsearch_Click" />
                                            <asp:ImageButton ID="btnprint" runat="server" ImageUrl="~/LibImages/Print.jpg" OnClick="btnprint_click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnprint" />
                </Triggers>
            </asp:UpdatePanel>
        </center>
        <center>
            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                <ContentTemplate>
                    <tr id="select_range" runat="server" visible="false">
                        <td>
                            <asp:Label ID="Label13" runat="server" Text="Range "></asp:Label>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="Label14" runat="server" Text="From"></asp:Label>
                            <asp:TextBox ID="txt_frange" CssClass="textbox textbox1 txtheight" runat="server"
                                MaxLength="4"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_frange"
                                FilterType="Numbers" ValidChars="/">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label15" runat="server" Text="To"></asp:Label>
                            <asp:TextBox ID="txt_trange" CssClass="textbox textbox1 txtheight" runat="server"
                                MaxLength="4"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_trange"
                                FilterType="Numbers" ValidChars="/">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:Button ID="Btn_range" runat="server" Text="Select" OnClick="Btn_range_Click"
                                CssClass="textbox1 textbox btn2" />
                        </td>
                    </tr>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div id="divspread" runat="server" visible="false" style="width: 1000px; height: auto">
                    <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                    <asp:GridView ID="grdBarcode" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                        Font-Names="book antiqua" togeneratecolumns="true" AllowPaging="true" PageSize="500"
                        OnPageIndexChanging="grdBarcode_onpageindexchanged" Width="980px">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_sno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="<%#Container.DataItemIndex+1 %>" Visible="true">
                                    </asp:Label></center>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkenbl" runat="server" AutoPostBack="True" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <center>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
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
                                                <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                        <ContentTemplate>
                                                            <asp:ImageButton ID="btn_errorclose" runat="server" ImageUrl="~/LibImages/ok.jpg"
                                                                OnClick="btn_errorclose_Click" />
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
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpSearch">
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
        <%--progressBar for UpPrint--%>
        <center>
            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpPrint">
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
            <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2"
                PopupControlID="UpdateProgress2">
            </asp:ModalPopupExtender>
        </center>
    </body>
    </html>
</asp:Content>
