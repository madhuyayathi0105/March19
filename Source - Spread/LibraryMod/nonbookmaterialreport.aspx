<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="nonbookmaterialreport.aspx.cs" Inherits="LibraryMod_nonbookmaterialreport" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
                <span class="fontstyleheader" style="color: Green;">Non Book Material Report</span></div>
        </center>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <center>
                    <div>
                        <table class="maintablestyle" style="height: auto; margin-left: 0px; margin-top: 10px;
                            margin-bottom: 10px; padding: 6px; font-family: Book Antiqua; font-weight: bold">
                            <tr>
                                <td>
                                    <asp:Label ID="lblCollege" runat="server" Text="College" CssClass="commonHeaderFont">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbllibrary" runat="server" Text="Library" CssClass="commonHeaderFont">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlLibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="163px" AutoPostBack="True" OnSelectedIndexChanged="ddllibrary_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbldept" runat="server" Text="Department" CssClass="commonHeaderFont">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddldept" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="160px" AutoPostBack="True" OnSelectedIndexChanged="ddldept_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblstatus" runat="server" Text="Status" CssClass="commonHeaderFont">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlstatus" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddlstatus_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2px">
                                    <asp:Label ID="lblsearch" runat="server" Text="By Search" CssClass="commonHeaderFont">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlsearchby" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="129px" AutoPostBack="True" OnSelectedIndexChanged="ddlsearchby_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtsearch" runat="server" Visible="false" CssClass="textbox txtheight2"
                                        Style="width: 100px;"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpGo" runat="server">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="btngo" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="btngoClick" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
        <ContentTemplate>
            <center>
                <asp:GridView ID="grdManualExit" runat="server" ShowFooter="false" AutoGenerateColumns="false"
                    Font-Names="Book Antiqua" toGenerateColumns="false" AllowPaging="true" PageSize="500"
                    OnSelectedIndexChanged="grdManualExit_OnSelectedIndexChanged" OnPageIndexChanging="grdManualExit_OnPageIndexChanged"
                    Width="980px">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-Width="120px" DataField="nonbookmat_no" HeaderText="Access No" />
                        <asp:BoundField ItemStyle-Width="120px" DataField="attachment" HeaderText="MaterialName" />
                        <asp:BoundField ItemStyle-Width="120px" DataField="title" HeaderText="Title" />
                        <asp:BoundField ItemStyle-Width="190px" DataField="author" HeaderText="Author" />
                        <asp:BoundField ItemStyle-Width="110px" DataField="publisher" HeaderText="Publisher" />
                        <asp:BoundField ItemStyle-Width="120px" DataField="department" HeaderText="Department" />
                        <asp:BoundField ItemStyle-Width="190px" DataField="contents" HeaderText="Contents" />
                        <asp:BoundField ItemStyle-Width="110px" DataField="issue_flag" HeaderText="Status" />
                        <asp:BoundField ItemStyle-Width="120px" DataField="invoiceno" HeaderText="Invoice No" />
                        <asp:BoundField ItemStyle-Width="190px" DataField="supplier" HeaderText="Supplier" />
                        <asp:BoundField ItemStyle-Width="110px" DataField="billdate" HeaderText="Bill Date" />
                    </Columns>
                    <HeaderStyle BackColor="#0CA6CA" ForeColor="black" />
                </asp:GridView>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
        <ContentTemplate>
            <div id="div2" runat="server" visible="false">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblnoofbooks" runat="server" Text="Total Books"></asp:Label>
                            <asp:TextBox ID="txtnoofbooks" runat="server" Enabled="false" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="20px" Width="75px" OnTextChanged="txtnoofstud_TextChanged"
                                AutoPostBack="True"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <center>
        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
            <ContentTemplate>
                <div id="print2" runat="server" visible="false">
                    <asp:Label ID="lblvalidation3" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                        ForeColor="Red" Text="" Style="display: none;"></asp:Label>
                    <asp:Label ID="lblrptname2" runat="server" Visible="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname2" runat="server" Visible="true" Width="180px" onkeypress="display()"
                        CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                    <asp:ImageButton ID="btnExcel2" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                        OnClick="btnExcel_Click2" />
                    <asp:ImageButton ID="btnprintmasterhed2" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                        OnClick="btnprintmaster_Click2" />
                    <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExcel2" />
                <asp:PostBackTrigger ControlID="btnprintmasterhed2" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
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
                                            <asp:UpdatePanel ID="UpdatePanelbtn2" runat="server">
                                                <ContentTemplate>
                                                    <center>
                                                        <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                            OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                                    </center>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
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
    <%--progressBar for UpGo--%>
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
