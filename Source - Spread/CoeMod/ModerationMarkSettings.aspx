<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="ModerationMarkSettings.aspx.cs" Inherits="ModerationMarkSettings" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
        <center>
        <span id="spPageHeading" runat="server" class="fontstyleheader" style="color: Green;
            margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">Moderation Mark Settings</span>
        <div style="width: 100%; margin: 0px; margin-bottom: 10px; margin-top: 10px;" visible="true">
            <table class="maintablestyle" style="height: auto; width: auto;">
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Style="height: 18px; width: 10px"></asp:Label>
                            </td>
                            <td colspan="2">
                        <asp:DropDownList ID="ddlCollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="182px" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged"
                            AutoPostBack="True" Style="">
                        </asp:DropDownList>
                    </td>
                    
                    <td>
                        <asp:Label ID="lblBatch" runat="server" Text="Batch" CssClass="commonHeaderFont"
                            AssociatedControlID="txtBatch"></asp:Label>
                            </td>
                            <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlBatch" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtBatch" Visible="true" Width="67px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlBatch" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="140px">
                                        <asp:CheckBox ID="chkBatch" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkBatch_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblBatch" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="cblBatch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtBatch" runat="server" TargetControlID="txtBatch"
                                        PopupControlID="pnlBatch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td >
                        <asp:Label ID="lblDegree" runat="server" CssClass="commonHeaderFont" Text="Degree"></asp:Label>
                            </td>
                            <td >
                        <div style="position: relative;">
                          <asp:DropDownList ID="ddlEdulevl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="70px" OnSelectedIndexChanged="ddlEdulevl_SelectedIndexChanged"
                            AutoPostBack="True" Style="">
                        </asp:DropDownList>
                          <%--  <asp:UpdatePanel ID="upnlDegree" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtDegree" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlDegree" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="140px">
                                        <asp:CheckBox ID="chkDegree" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkDegree_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblDegree" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="cblDegree_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtDegree" runat="server" TargetControlID="txtDegree"
                                        PopupControlID="pnlDegree" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </div>
                    </td>
                    <td >
                        <asp:Label ID="lblBranch" runat="server" CssClass="commonHeaderFont" Text="Branch"
                            AssociatedControlID="txtBranch"></asp:Label>
                            </td>
                            <td >
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlBranch" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtBranch" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlBranch" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="280px">
                                        <asp:CheckBox ID="chkBranch" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkBranch_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblBranch" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtBranch" runat="server" TargetControlID="txtBranch"
                                        PopupControlID="pnlBranch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                     <td>
                     <asp:Label ID="lbl_org_sem" Text="Semester" runat="server"></asp:Label>
                     </td>
                     <td>
                                    <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox textbox1 txtheight3" ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel11" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_sem" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_sem_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender20" runat="server" TargetControlID="txt_sem"
                                                PopupControlID="Panel11" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                     </td>
                    </tr>
                   <tr>
                   <td>
                     <asp:RadioButton ID="rbRegular" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Regular" AutoPostBack="true" OnCheckedChanged="Radiochanged"
                        GroupName="Hall Ticket" />
                  </td>
                  <td>
                    <asp:RadioButton ID="rbArear" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Arear" GroupName="Hall Ticket" AutoPostBack="true"
                        OnCheckedChanged="Radiochanged" />
                </td>
                <td>
                        <asp:Button ID="btnGo" CssClass="textbox textbox1 commonHeaderFont" runat="server"
                            OnClick="btnGo_Click" Text="Go" Style="width: auto; height: auto;" />
               </td>
               </tr>
            </table>
        </div>
        <br />
        <br />
        <div id="content" runat="server" visible="false">
        <table>
        <tr>
        <td>  <asp:Label ID="lblExtMin" Text="ESE MIN" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label></td>
        <td><asp:TextBox ID="txtExtMin" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:TextBox></td>
                              <asp:RegularExpressionValidator ID="revESE" ValidationGroup="Scheme" Display="Dynamic"
ValidationExpression="^(\d+)?$" ControlToValidate="txtExtMin" runat="server" ErrorMessage="Enter Number"></asp:RegularExpressionValidator>

        <td><asp:DropDownList ID="ddlExtMin" runat="server"  Width="120px" Font-Names="Book Antiqua"
                            Style="" Font-Bold="true"
                            Font-Size="Medium" Height="25px"></asp:DropDownList>
        </td>
        </tr>
        <tr>
        <td><asp:Label ID="lblCiaMin" Text="CIA MIN" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label></td>
        <td><asp:TextBox ID="txtCiaMin" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:TextBox></td>
                              <asp:RegularExpressionValidator ID="revCIA" ValidationGroup="Scheme" Display="Dynamic"
ValidationExpression="^(\d+)?$" ControlToValidate="txtCiaMin" runat="server" ErrorMessage="Enter Number"></asp:RegularExpressionValidator>

        <td><asp:DropDownList ID="ddlCiaMin" runat="server"  Width="120px" Font-Names="Book Antiqua"
                            Style="" Font-Bold="true"
                            Font-Size="Medium" Height="25px">
          </asp:DropDownList>
        </td>
        </tr>
         <tr>
        <td><asp:Label ID="lblModeration" Text="Moderation Mark" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label></td>
        <td><asp:TextBox ID="txtModeration" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:TextBox></td>
                              <asp:RegularExpressionValidator ID="revMod" ValidationGroup="Scheme" Display="Dynamic"
ValidationExpression="^(\d+)?$" ControlToValidate="txtModeration" runat="server" ErrorMessage="Enter Number"></asp:RegularExpressionValidator>
                            </tr>
        </table>
         <asp:Button ID="btnSave" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" runat="server"
                            OnClick="btnSave_Click" Text="Save" Style="width: auto; height: auto;" />
        </div>
         
    </center>
     <center>
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
                                <td>
                                    <center>
                                        <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                            Text="Ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
            </div>
    </center>
</asp:Content>

