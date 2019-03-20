<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="Cumulative_Books_Journal.aspx.cs" Inherits="LibraryMod_Cumulative_Books_Journal" %>

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
                <span class="fontstyleheader" style="color: Green;">Library Books And Journal Details</span></div>
        </center>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
            <ContentTemplate>
                <center>
                    <div id="maindiv" runat="server" class="maindivstyle" style="width: 900px; height: auto">
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <center>
                                            <div style="width: 1000px; height: auto">
                                                <table class="maintablestyle" style="height: auto; margin-left: -105px; margin-top: 10px;
                                                    margin-bottom: 10px; padding: 6px; font-family: Book Antiqua; font-weight: bold">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblclg" runat="server" Text="College">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Width="230px" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbllibrary" runat="server" Text="Library:">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddllibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Width="177px" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddllibrary_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_dept" runat="server" Text="Department:">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddl_dept" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Width="177px" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddl_dept_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkredate" runat="server" AutoPostBack="true" OnCheckedChanged="chkredate_CheckedChanged"
                                                                Text="Cumulative" />
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
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <center>
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
           
                 <asp:gridview id="gridview1" runat="server" showfooter="false" autogeneratecolumns="true" ShowHeader="true"
                    font-names="book antiqua" togeneratecolumns="true" allowpaging="true" pagesize="500" 
                    onselectedindexchanged="gridview1_onselectedindexchanged" onpageindexchanging="gridview1_onpageindexchanged"
                    width="980px">
                    
                    <headerstyle backcolor="#0ca6ca" forecolor="white" />
                </asp:gridview>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <br />
        <br />
        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
            <ContentTemplate>
                <div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                        Visible="false"></asp:Label>
                    <asp:Label ID="lblrptname" runat="server" Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" onkeypress="display()"
                        Font-Size="Medium" CssClass="textbox textbox1"></asp:TextBox>
                    <asp:ImageButton ID="btnExcel" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                        OnClick="btnExcel_Click" />
                    <asp:ImageButton ID="btnprintmaster" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                        OnClick="btnprintmaster_Click" />
                     <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
            <ContentTemplate>
                <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25,
25, .2); position: absolute; top: 0; left: 0px;">
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
