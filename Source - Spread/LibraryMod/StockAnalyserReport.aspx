<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="StockAnalyserReport.aspx.cs" Inherits="LibraryMod_StockAnalyserReport" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <span class="fontstyleheader" style="color: Green;">Data Scanner Report</span>
        </center>
    </div>
    <center>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="width: 900px; font-family: Book Antiqua; font-weight: bold; height: auto">
                    <table class="maintablestyle" style="height: auto; margin-top: 10px; margin-bottom: 10px;
                        padding: 6px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblclg" runat="server" Text="College">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Width="200px" Height="" AutoPostBack="True" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td colspan="5">
                                <asp:Label ID="LblType" runat="server" Text="Type :">
                                </asp:Label>
                                <asp:RadioButton ID="rbBefore" runat="server" Text="Before Confirm Scan" GroupName="Rbgrp"
                                    AutoPostBack="true" Checked="true" OnCheckedChanged="rbBefore_OnCheckedChanged" />
                                <asp:RadioButton ID="rbAfter" runat="server" Text="After Confirm Scan" GroupName="Rbgrp"
                                    AutoPostBack="true" OnCheckedChanged="rbAfter_OnCheckedChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbllibrary" runat="server" Text="Library" CssClass="commonHeaderFont">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddllibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Width="200px" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td colspan="3">
                                <asp:CheckBox ID="chkredate" runat="server" AutoPostBack="true" OnCheckedChanged="chkredate_CheckedChanged" />
                                From :
                                <asp:TextBox ID="txtfromdate" runat="server" Enabled="false" AutoPostBack="true"
                                    Width="80px" CssClass="textbox txtheight2"></asp:TextBox>
                                <asp:CalendarExtender ID="calendetextenfordatext" TargetControlID="txtfromdate" runat="server"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                                To :
                                <asp:TextBox ID="txttodate" runat="server" Enabled="false" AutoPostBack="true" Width="80px"
                                    CssClass="textbox txtheight2"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txttodate" runat="server"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblBoktype" runat="server" Text="Book Type" CssClass="commonHeaderFont">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddltype" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Width="130px" AutoPostBack="True">
                                    <asp:ListItem Text="Books"></asp:ListItem>
                                    <asp:ListItem Text="Project Books"></asp:ListItem>
                                    <asp:ListItem Text="Non Book Materials"></asp:ListItem>
                                    <asp:ListItem Text="Back Volume"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LblYr" runat="server" Text="Year" Visible="false" CssClass="commonHeaderFont">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlYear" runat="server" Visible="false" CssClass="textbox ddlstyle ddlheight3"
                                    Width="130px" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="LblStatus" runat="server" Visible="false" Text="Status" CssClass="commonHeaderFont">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" Visible="false" CssClass="textbox ddlstyle ddlheight3"
                                    Width="130px" AutoPostBack="True">
                                    <asp:ListItem Text="Available" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Lost(All)" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Lost(Library)" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Lost(Fine Collected)" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Condemn" Value="4"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td colspan="3">
                                <asp:UpdatePanel ID="UpGo" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="Btngo" runat="server" Text="Go" OnClick="btngo_Click" Style="font-family: Book Antiqua;
                                            font-size: large; font-weight: bold; background-color: lightgreen; margin-left: 164px;" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div id="divLabVal" runat="server" visible="false" style="width: 800px; font-family: Book Antiqua;
                    background-color: lightgreen; font-weight: bold;">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="GRAND TOTAL :" CssClass="commonHeaderFont">
                                </asp:Label>
                                <asp:Label ID="LblGrdTot" runat="server" Text="" CssClass="commonHeaderFont">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LblTotAvail" runat="server" Text="" Style="margin-left: 125px;" CssClass="commonHeaderFont">
                                </asp:Label>
                                <asp:Label ID="LblTotAvailBooks" runat="server" Text="" CssClass="commonHeaderFont">
                                </asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <br />
    <center>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div id="divReport" runat="server" style="width: 1000px; height: 500px; overflow: auto">
                    <asp:GridView ID="grdReport" Width="1000px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                        Font-Names="Book Antiqua" ShowHeader="false" toGenerateColumns="false" OnRowDataBound="grdReport_RowDataBound">
                        <%----%>
                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
            <ContentTemplate>
                <div id="rptprint1" runat="server" visible="false">
                    <asp:Label ID="lblvalidation2" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                        Visible="false"></asp:Label>
                    <asp:Label ID="lblrptname1" runat="server" Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname1" runat="server" Height="20px" Width="180px" onkeypress="display()"
                        Font-Size="Medium" CssClass="textbox txtheight2"></asp:TextBox>
                    <asp:ImageButton ID="btnExcel1" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                        OnClick="btnExcel1_Click" />
                    <asp:ImageButton ID="btnprintmaster1" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                        OnClick="btnprintmaster1_Click" />
                    <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExcel1" />
                <asp:PostBackTrigger ControlID="btnprintmaster1" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
    <%--progressBar for Go--%>
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
