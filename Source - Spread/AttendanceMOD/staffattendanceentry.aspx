﻿<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="staffattendanceentry.aspx.cs" Inherits="staffattendanceentry"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <body>
    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <style type="text/css" media="screen">
            .floats
            {
                height: 26px;
            }
            .CenterPB
            {
                position: absolute;
                left: 50%;
                top: 50%;
                margin-top: -20px;
                margin-left: -20px;
                width: auto;
                height: auto;
            }
        </style>
        <br />
        <center>
            <span class="fontstyleheader" style="color: Green;">Individual Staff Attendance Entry
                Without Time Table</span>
        </center>
        <br />
        <center>
            <div class="maintablestyle" style="width: 950px;">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblbatch" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged" Height="25px"
                                Width="60px" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lbldegree" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddldegree" Height="24px" Width="88px" AutoPostBack="True"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblbranch" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlbranch" Font-Bold="True" Height="25px" Width="328px"
                                Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblduration" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlduration" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" AutoPostBack="True" Height="25px" Width="47px" OnSelectedIndexChanged="ddlduration_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblsec" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlsec" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="25px" Width="49px" AutoPostBack="True" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblFromdate" runat="server" Text="Date" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromDate" CssClass="txt" runat="server" Height="20px" Width="75px"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtFromDate_TextChanged"
                                AutoPostBack="True"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtFromDate_FilteredTextBoxExtender" FilterType="Custom,Numbers"
                                ValidChars="/" runat="server" TargetControlID="txtFromDate">
                            </asp:FilteredTextBoxExtender>
                            <asp:CalendarExtender ID="calfromdate" TargetControlID="txtFromDate" Format="dd/MM/yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlsubject" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="264px" OnSelectedIndexChanged="ddlsubject_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblhours" runat="server" Text="Hours" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txthour" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                        Width="110px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="phour" runat="server" Width="112px" CssClass="multxtpanel">
                                        <asp:CheckBox ID="chkhours" runat="server" OnCheckedChanged="chkhours_ChekedChange"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chklshour" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklshour_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txthour"
                                        PopupControlID="phour" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lblstaff" runat="server" Text="Staff" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtstaff" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                        Width="110px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="pstaff" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="chkstaff" runat="server" OnCheckedChanged="chkstaff_ChekedChange"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chklsstaff" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsstaff_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtstaff"
                                        PopupControlID="pstaff" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                        <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                                <ContentTemplate>
                                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="btngoUpdatePanel">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
                            <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" Height="26px" />
                                 </ContentTemplate>
                        </asp:UpdatePanel>
                        </td>
                        <td>

                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel6">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress3"
            PopupControlID="UpdateProgress3">
        </asp:ModalPopupExtender>
                            <asp:Button ID="btndetails" runat="server" Text="Hour Details" OnClick="btndetails_Click"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Height="26px" />

                                </ContentTemplate>
                        </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <asp:Label ID="errmsg" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Height="16px"></asp:Label>
            <br />
            <asp:Label ID="lblholireason" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Height="16px"></asp:Label>
            <br />
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel3">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
                   
                    <br />
                     <div id="divGridII" runat="server" style="width: 900px; height: auto; overflow: auto;">
         <asp:GridView ID="Showgrid" runat="server" Width="100px" BorderStyle="Double"
                            Font-Names="Book Antiqua" Font-Size="Medium" GridLines="Both" CellPadding="4"  
                             ShowFooter="false" ShowHeader="true" OnRowDataBound="OnRowDataBound">
                           
                        </asp:GridView>
         </div>
                    <br />
                    <asp:Button ID="btnselect" runat="server" Text="Select All" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnselect_Click" />
                    <asp:Button ID="btndeselect" runat="server" Text="De-Select All" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btndeselect_Click" />
                    <asp:Button ID="btnsave" runat="server" Text="Save" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnsave_Click" />
                </ContentTemplate>
                
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                <ContentTemplate>
            <asp:Panel ID="panhrdetails" runat="server" BackColor="AliceBlue" ScrollBars="Vertical"
                Style="top: 280px; left: 137px; position: absolute; width: 750px" BorderColor="Black"
                BorderStyle="Double">
                <br />
                <asp:Label ID="headlbl_sl" runat="server" Text="Hour Details" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Style="top: 10px; left: 330px; position: absolute;"></asp:Label>
                <br />
                <br />
                <asp:Label ID="lblholihrdetails" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Attendance Completed" ForeColor="Red"></asp:Label>
                <table>
                    <tr>
                        <td>
                            <center style="width: 665px">
                             <asp:GridView ID="grid_sliplist" runat="server" Width="500px" BorderStyle="Double"
                            Font-Names="Book Antiqua" Font-Size="Medium" GridLines="Both" CellPadding="4"  
                             ShowFooter="false" ShowHeader="true" >
                              </asp:GridView>
                                </center>

                           
                               
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblerrhr" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Attendance Completed" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style114">

                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <ContentTemplate>
                                <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpdatePanel7">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="UpdateProgress4"
            PopupControlID="UpdateProgress4">
        </asp:ModalPopupExtender>
                            <asp:Button ID="exit_sliplist" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnClick="exit_sliplist_Click" Style="margin-left: 608px" />

                                </ContentTemplate>
                           </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            </ContentTemplate>
            </asp:UpdatePanel>

        </center>

         </ContentTemplate>
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="Btngo" />--%>
           <%-- <asp:PostBackTrigger ControlID="btndetails" />--%>
           <%-- <asp:PostBackTrigger ControlID="exit_sliplist" />--%>
            
            
        </Triggers>
    </asp:UpdatePanel>
    </body>

    </html>
    
</asp:Content>
