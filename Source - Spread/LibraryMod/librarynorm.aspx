<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="librarynorm.aspx.cs" Inherits="LibraryMod_librarynorm" %>

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
                <span class="fontstyleheader" style="color: Green;">Library Norms</span></div>
        </center>
    </div>
    <div>
        <center>
            <div>
                <table class="maintablestyle" style="height: auto; font-family: Book Antiqua; font-weight: bold;
                    margin-left: 0px; margin-top: 10px; margin-bottom: 10px; padding: 6px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblCollege" runat="server" Text="College" CssClass="commonHeaderFont">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel26" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lblinwardentry" runat="server" Text="Inward Entry Type:" CssClass="commonHeaderFont">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlinwardentry" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddlinwardentry_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbllibrary" runat="server" Text="Library" CssClass="commonHeaderFont">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlLibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="163px" AutoPostBack="True" OnSelectedIndexChanged="ddllibrary_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbldept" runat="server" Text="Department" CssClass="commonHeaderFont">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddldept" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddldept_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="upgo" runat="server">
                                <ContentTemplate>
                                    <asp:ImageButton ID="btngo" runat="server" ImageUrl="~/LibImages/Go.jpg" Style="margin-top: 10px;"
                                        OnClick="btngoClick" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>
        </center>
        <center>
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <div id="showreport2" runat="server" visible="false">
                        <table>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                                    <asp:GridView ID="grdLibNorms" runat="server" ShowFooter="false" ShowHeaderWhenEmpty="true"
                                        AutoGenerateColumns="false" Font-Names="book antiqua" togeneratecolumns="false"
                                        AllowPaging="true" PageSize="100" Width="1000px" OnPageIndexChanging="grdLibNorms_onpageindexchanged"
                                        OnRowDataBound="grdLibNorms_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_sno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Text="<%#Container.DataItemIndex+1 %>" Visible="true">
                                                    </asp:Label></center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Department" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="200px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_dep" runat="server" Text='<%#Eval("Department") %>' Width="200px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="As Per Norms Volume" HeaderStyle-BackColor="#0CA6CA"
                                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lbl_aspervol" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("asperVolumes") %>'
                                                        Height="24px" Width="70px" Style="text-align: right;"></asp:TextBox></center>
                                                    <asp:FilteredTextBoxExtender ID="filterextenderAddOrDel" runat="server" TargetControlID="lbl_aspervol"
                                                        FilterType="Numbers,Custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="As Per Norms Title" HeaderStyle-BackColor="#0CA6CA"
                                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lbl_aspertit" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("asperTitle") %>'
                                                        Height="24px" Width="70px" Style="text-align: right;"></asp:TextBox></center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Available Volume" HeaderStyle-BackColor="#0CA6CA"
                                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Avavol" runat="server" Text='<%#Eval("AvaVolume") %>' Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Available Title" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Avatit" runat="server" Text='<%#Eval("AvaTitle") %>' Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Needed Volume" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_needvol" runat="server" Text='<%#Eval("needVolume") %>' Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Needed Title" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_needtit" runat="server" Text='<%#Eval("needtit") %>' Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Extra Volume" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_extvol" runat="server" Text='<%#Eval("extVolume") %>' Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Extra Title" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_exttit" runat="server" Text='<%#Eval("exttit") %>' Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:HiddenField ID="HiddenField1" runat="server" Value="-1" />
                                    <asp:GridView ID="grdLibNormsPer" runat="server" ShowFooter="false" ShowHeaderWhenEmpty="true"
                                        AutoGenerateColumns="false" Font-Names="book antiqua" togeneratecolumns="false"
                                        AllowPaging="true" PageSize="100" Width="1000px" OnRowDataBound="grdLibNormsPer_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_sno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Text="<%#Container.DataItemIndex+1 %>" Visible="true">
                                                    </asp:Label></center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Department" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="200px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_dep" runat="server" Text='<%#Eval("Department") %>' Width="200px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="As Per Norms National" HeaderStyle-BackColor="#0CA6CA"
                                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lbl_aspernationl" runat="server" Text='<%#Eval("aspernation") %>'
                                                        Width="80px"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="As Per Norms International" HeaderStyle-BackColor="#0CA6CA"
                                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lbl_asperinter" runat="server" Text='<%#Eval("asperinter") %>' Width="80px"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Available National" HeaderStyle-BackColor="#0CA6CA"
                                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Avanational" runat="server" Text='<%#Eval("Avanational") %>' Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Available International" HeaderStyle-BackColor="#0CA6CA"
                                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Avainter" runat="server" Text='<%#Eval("Avainter") %>' Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Needed National" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_neednationl" runat="server" Text='<%#Eval("neednationl") %>' Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Needed International" HeaderStyle-BackColor="#0CA6CA"
                                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_needinter" runat="server" Text='<%#Eval("needinter") %>' Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Extra National" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_extnational" runat="server" Text='<%#Eval("extnationale") %>'
                                                        Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Extra International" HeaderStyle-BackColor="#0CA6CA"
                                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_extineter" runat="server" Text='<%#Eval("extinter") %>' Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <br />
                            <br />
                        </table>
                        <br />
                        <div>
                            <center>
                                <asp:UpdatePanel ID="UpSave" runat="server">
                                    <ContentTemplate>
                                        <asp:ImageButton ID="btn_save" runat="server" Enabled="false" ImageUrl="~/LibImages/save.jpg"
                                            OnClick="btnsave_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </center>
                        </div>
                        <center>
                            <div id="print2" runat="server" visible="false">
                                <asp:Label ID="lblvalidation3" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    ForeColor="Red" Text="" Style="display: none;"></asp:Label>
                                <asp:Label ID="lblrptname2" runat="server" Visible="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txtexcelname2" runat="server" Visible="true" Width="180px" onkeypress="display()"
                                    CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                <asp:ImageButton ID="btnExcel2" runat="server" Visible="true" ImageUrl="~/LibImages/export to excel.jpg"
                                    OnClick="btnExcel_Click2" />
                                <asp:ImageButton ID="btnprintmasterhed2" runat="server" Visible="true" ImageUrl="~/LibImages/Print White.jpg"
                                    Style="margin-top: 10px;" OnClick="btnprintmaster_Click2" />
                                <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                            </div>
                        </center>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
        <center>
            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
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
                                                    <asp:ImageButton ID="btnerrclose" runat="server" ImageUrl="~/LibImages/ok.jpg" OnClick="btnerrclose_Click" />
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
        <tr>
            <td>
                <center>
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <div id="Div1" runat="server" visible="false">
                                <%-- <asp:Label ID="Label1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    ForeColor="Red" Text="" Style="display: none;"></asp:Label>
                                <asp:Label ID="Label2" runat="server" Visible="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Text="Report Name"></asp:Label>
                                <asp:TextBox ID="TextBox1" runat="server" Visible="true" Width="180px" onkeypress="display()"
                                    CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                               
                                <asp:ImageButton ID="Button1" runat="server" Visible="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="btnExcel_Click2" ImageUrl="~/LibImages/export to excel.jpg"
                                    CssClass="textbox textbox1" />
                                <asp:ImageButton ID="Button2" runat="server" Visible="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="btnprintmaster_Click2" ImageUrl="~/LibImages/Print White.jpg"
                                    CssClass="textbox textbox1" />
                                <Insproplus:printmaster runat="server" ID="Printmaster1" Visible="false" />--%>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </center>
            </td>
        </tr>
    </div>
    <%--Progress bar for  go--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upgo">
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
    <%--Progress bar for  UpSave--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpSave">
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
</asp:Content>
