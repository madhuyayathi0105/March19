<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="Title_Author_publisherwisereport.aspx.cs" Inherits="LibraryMod_Title_Author_publisherwisereport" %>

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
                <span class="fontstyleheader" style="color: Green;">Title,Author and PublisherWise Report</span></div>
        </center>
    </div>
    <div>
        <center>
            <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; font-family: Book Antiqua;
                font-weight: bold; height: auto">
                <div>
                    <table>
                        <tr>
                            <td>
                                <center>
                                    <div>
                                        <table class="maintablestyle" style="height: auto; margin-left: 0px; margin-top: 10px;
                                            margin-bottom: 10px; padding: 6px;">
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblCollege" runat="server" Text="College">
                                                            </asp:Label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lbllibrary" runat="server" Text="Library">
                                                            </asp:Label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddllibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddllibrary_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lbl_department" Text="Department" runat="server"></asp:Label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UP_department" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_department" runat="server" Style="height: 20px; width: 100px;"
                                                                ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="panel_department" runat="server" CssClass="multxtpanel" Style="width: 170px;
                                                                height: auto;">
                                                                <asp:CheckBox ID="cb_department" runat="server" Width="200px" Text="Select All" AutoPostBack="true"
                                                                    OnCheckedChanged="cb_department_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="cbl_department" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_department_OnSelectedIndexChanged">
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
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblreporttype" runat="server" Text="Report Type">
                                                            </asp:Label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlreporttype" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlreporttype_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lbltitletype" runat="server" Text="Type" Visible="true">
                                                            </asp:Label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddltitletype" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddltitletype_SelectedIndexChanged"
                                                                Visible="true">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lbl_Title" Text="Title:" runat="server" Visible="true"></asp:Label>
                                                            <asp:Label ID="lbl_Author" Text="Author:" runat="server" Visible="false"></asp:Label>
                                                            <asp:Label ID="lbl_Publisher" Text="Publisher:" runat="server" Visible="false"></asp:Label>
                                                            <asp:Label ID="lbl_Isbn" Text="ISBN:" runat="server" Visible="false"></asp:Label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txttitle" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" Width="200px" Visible="true" CssClass="textbox txtheight2"></asp:TextBox>
                                                            <asp:TextBox ID="txtauthor" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" Width="200px" Visible="false" CssClass="textbox txtheight2"></asp:TextBox>
                                                            <asp:TextBox ID="txtpublisher" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" Width="200px" Visible="false" CssClass="textbox txtheight2"></asp:TextBox>
                                                            <asp:TextBox ID="txtisbn" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" Width="200px" Visible="false" CssClass="textbox txtheight2"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <fieldset style="width: 350px; height: 15px;">
                                                        <asp:RadioButtonList ID="rbltransType" runat="server" RepeatDirection="Horizontal"
                                                            AutoPostBack="true" OnSelectedIndexChanged="rbltransType_Selected">
                                                        </asp:RadioButtonList>
                                                    </fieldset>
                                                </td>
                                                <td colspan="2">
                                                    <fieldset style="width: 350px; height: 15px;">
                                                        <asp:RadioButtonList ID="rblbooks" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                                            OnSelectedIndexChanged="rblbooks_Selected">
                                                        </asp:RadioButtonList>
                                                    </fieldset>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbltype1" Text="Type:" runat="server" Visible="true"></asp:Label>
                                                    <asp:TextBox ID="txttype" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Height="24px" Width="50px" Visible="true"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txttype"
                                                        FilterType="Numbers" ValidChars="">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:Button ID="btn_go" runat="server" CssClass="textbox btn2" Text="Go" OnClick="btngo_Click"
                                                        BackColor="LightGreen" />
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
        </center>
        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
            <ContentTemplate>
                <div id="div2" runat="server" visible="false">
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblnoofbooks" runat="server" Text="No Of Author:"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblnoofbooksCount" runat="server" Text=""></asp:Label>
                                </td>
                                <%-- <td>
                                    <asp:Label ID="Label1" runat="server" Text="No Of Books:"></asp:Label>
                                </td>--%>
                            </tr>
                        </table>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <center>
            <div id="showreport1" runat="server" visible="false">
                <table>
                    <tr>
                        <td>
                            <asp:GridView ID="grdManualExit" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                Font-Names="Book Antiqua" toGenerateColumns="false" ShowHeader="true" OnSelectedIndexChanged="grdManualExit_OnSelectedIndexChanged"
                                Width="980px">
                                <HeaderStyle BackColor="#0CA6CA" ForeColor="black" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <center>
                                <div id="print" runat="server" visible="false">
                                    <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        ForeColor="Red" Text="" Style="display: none;"></asp:Label>
                                    <asp:Label ID="lblrptname" runat="server" Visible="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Report Name"></asp:Label>
                                    <asp:TextBox ID="txtexcelname" runat="server" Visible="true" Width="180px" onkeypress="display()"
                                        CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                    <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                        InvalidChars="/\">
                                    </asp:FilteredTextBoxExtender>--%>
                                    <asp:Button ID="btnExcel" runat="server" Visible="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btnExcel_Click" Text="Export To Excel" Width="127px"
                                        Height="32px" CssClass="textbox textbox1" />
                                    <asp:Button ID="btnprintmasterhed" runat="server" Visible="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" Height="32px"
                                        Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                                    <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                                </div>
                            </center>
                        </td>
                    </tr>
                </table>
            </div>
        </center>
        <center>
            <div id="showreport2" runat="server" visible="false">
                <table>
                    <tr>
                        <td>
                            <asp:GridView ID="GridView1" runat="server" Font-Names="Book Antiqua" toGenerateColumns="true"
                                AllowPaging="true" PageSize="1000" OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged"
                                ShowHeader="true" OnPageIndexChanging="GridView1_OnPageIndexChanged" OnRowDataBound="GridView1_RowDataBound"
                                Width="980px">
                                <HeaderStyle BackColor="#0CA6CA" ForeColor="black" />
                            </asp:GridView>
                        </td>
                      
                    </tr>
                    <tr>
                        <td>
                            <center>
                                <div id="print2" runat="server" visible="false">
                                    <asp:Label ID="lblvalidation3" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        ForeColor="Red" Text="" Style="display: none;"></asp:Label>
                                    <asp:Label ID="lblrptname2" runat="server" Visible="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Report Name"></asp:Label>
                                    <asp:TextBox ID="txtexcelname2" runat="server" Visible="true" Width="180px" onkeypress="display()"
                                        CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                    <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                        InvalidChars="/\">
                                    </asp:FilteredTextBoxExtender>--%>
                                    <asp:Button ID="btnExcel2" runat="server" Visible="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btnExcel_Click2" Text="Export To Excel" Width="127px"
                                        Height="32px" CssClass="textbox textbox1" />
                                    <asp:Button ID="btnprintmasterhed2" runat="server" Visible="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click2" Height="32px"
                                        Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                                    <NEW:NEWPrintMater runat="server" ID="NEWPrintMater1" Visible="false" />
                                </div>
                            </center>
                        </td>
                    </tr>
                </table>
            </div>
        </center>
        <center>
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
                                            <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btnerrclose_Click" Text="Ok" runat="server" />
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
