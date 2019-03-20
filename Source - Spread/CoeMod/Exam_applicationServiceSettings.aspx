<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Exam_applicationServiceSettings.aspx.cs"
    Inherits="CoeMod_Exam_applicationServiceSettings" MasterPageFile="~/CoeMod/COESubSiteMaster.master" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
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
        <span class="fontstyleheader" style="color: Green;">Service Settings</span></div>
    </center>
   </div>
     <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
   <div>
  
        <center>
         <div >
           <table class="maintablestyle" style="height: auto; font-family: Book Antiqua; font-weight: bold;
                        margin-left: 0px; margin-top: 10px; margin-bottom: 10px; padding: 6px; width:auto">
                    <tr>
                        <td>
                            <asp:Label ID="lblclg" Font-Bold="true" 
                                 Font-Size="Medium" ForeColor="Black" Font-Names="Book Antiqua"
                                runat="server" Text="College"></asp:Label>
                            <asp:DropDownList ID="ddlclg" runat="server" 
                                 AutoPostBack="true" OnSelectedIndexChanged="ddlclg_SelectedIndexChanged"
                                Width="100px" Height="28px" CssClass="textbox ddlstyle ddlheight3">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblstream" Font-Bold="true" Font-Size="Medium" ForeColor="Black" Font-Names="Book Antiqua"
                                runat="server" Text="Stream"></asp:Label>
                            <asp:DropDownList ID="ddlstream" runat="server" 
                                 AutoPostBack="true" OnSelectedIndexChanged="ddlstream_SelectedIndexChanged"
                                Width="100px" Height="28px" CssClass="textbox ddlstyle ddlheight3">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbledulevel" Font-Bold="true"  Font-Size="Medium" ForeColor="Black" Font-Names="Book Antiqua"
                                runat="server" Text="Edu-Level"></asp:Label></td><td>
                            <asp:UpdatePanel ID="Upp3" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_edulevel" runat="server"  ReadOnly="true" Font-Bold="True"
                                        Font-Names="Book Antiqua" Width="100px" Height="18px" CssClass="textbox ddlstyle ddlheight3">--Select--</asp:TextBox>
                                    <asp:Panel ID="p2" runat="server" CssClass="multxtpanel" Width="150px" Height="180px">
                                        <asp:CheckBox ID="cb_edulevel" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_edulevel_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_edulev" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_edulevel_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_edulevel"
                                        PopupControlID="p2" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                         <td>
                            <asp:Label ID="lbldegdet" Font-Bold="true"  Font-Size="Medium" ForeColor="Black" Font-Names="Book Antiqua"
                                runat="server" Text="BatchInfo"></asp:Label></td><td>
                        
                            
                                    <asp:TextBox ID="txtdegdetails" runat="server"  ReadOnly="true" Font-Bold="True"
                                        Font-Names="Book Antiqua" Height="18px" CssClass="textbox ddlstyle ddlheight3">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Width="300px" Height="267px">
                                        <asp:CheckBox ID="cb_degdetails" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_degdetails_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_degdetails" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degdetails_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdegdetails"
                                        PopupControlID="Panel1" Position="Bottom">
                                    </asp:PopupControlExtender>
                               
                        </td>
                        <td>
                         <asp:Button ID="btnsave" runat="server" Font-Names="Book Antiqua" Text="Save" OnClick="btnsave_Click"
                            Font-Size="Medium" Font-Bold="true" />
                             <asp:Button ID="btndelete" runat="server" Font-Names="Book Antiqua" Text="Delete" OnClick="btndelete_Click"
                            Font-Size="Medium" Font-Bold="true" />
                        </td>
                    </tr>
                </table>
            </div>
        </center>
        
  </div>
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
                           
                                <td align="center">
                                        <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                            Text="Ok" runat="server" />
                                </td>
                              
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
