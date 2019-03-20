<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="ExamFeeMaster.aspx.cs" Inherits="ExamFeeMaster" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
<style type="text/css">
    .font
 {
 font-family:Book Antiqua;
 font-size:medium;
 font-weight:bold;
        }
 
</style><br />
<center>
    <asp:Label ID="Label7" runat="server" Text ="Exam Fee Master Settings" 
            Font-Bold="True" 
            Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label></center>
      <br />
   <center>
       <table style="width:700px; height:70px; background-color:#0CA6CA;" >
       <tr>
       <td>
           <asp:Label ID="lblheader" runat="server" Text="Header" CssClass="font"></asp:Label>
       </td>
       <td>
           
            <asp:DropDownList ID="ddlheader" runat="server" Font-Bold="True" 
                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True" 
                        onselectedindexchanged="ddlheader_SelectedIndexChanged" 
               Width="120px" ></asp:DropDownList> 
       </td>
       <td>
       <asp:Label ID="lblledger" runat="server" Text="Ledger" CssClass="font"></asp:Label>
       </td>
       <td>
       <asp:DropDownList ID="ddlledger" runat="server"  CssClass="font" Width="130px" 
               AutoPostBack="True" 
               onselectedindexchanged="ddlledger_SelectedIndexChanged">
           </asp:DropDownList>
       </td>
       <td>
        <asp:Label ID="lblbatch" runat="server" Text="Batch" CssClass="font"></asp:Label>
       </td>
       <td>
        <asp:DropDownList ID="ddlbatch" runat="server"  CssClass="font" Width="70px" 
               AutoPostBack="True" onselectedindexchanged="ddlbatch_SelectedIndexChanged">
           </asp:DropDownList>
       </td>
       <td>
        <asp:Label ID="lbldegree" runat="server" Text="Degree" CssClass="font"></asp:Label>
       </td>
       <td>
         <asp:DropDownList ID="ddldegree" runat="server"  CssClass="font" Width="100px" 
               AutoPostBack="True" onselectedindexchanged="ddldegree_SelectedIndexChanged">
           </asp:DropDownList>
       
       </td>
       <td>
        <asp:Label ID="lblbranch" runat="server" Text="Branch" CssClass="font"></asp:Label>
       </td>
       <td>
            <asp:DropDownList ID="ddlbranch" runat="server" CssClass="font" Width="200px" 
                AutoPostBack="True" 
                onselectedindexchanged="ddlbranch_SelectedIndexChanged">
            </asp:DropDownList>
       </td>
        </tr>
 <tr>
       <td>
       <asp:Label ID="lblexamtype" runat="server" Text="Exam Type" CssClass="font" Width="100px"></asp:Label>
       </td>
       <td>
       <asp:DropDownList ID="ddlexamtype" runat="server" CssClass="font" Width="100px" 
               AutoPostBack="True" onselectedindexchanged="ddlexamtype_SelectedIndexChanged">
           <asp:ListItem></asp:ListItem>
           <asp:ListItem>Theory</asp:ListItem>
           <asp:ListItem>Practical</asp:ListItem>
            </asp:DropDownList>
       </td>
       <td>
       <asp:Label ID="lblsubject" runat="server" Text="Subject" CssClass="font"></asp:Label>
       </td>
       <td>
       <asp:DropDownList ID="ddlsubject" runat="server" CssClass="font" Width="200px" 
               AutoPostBack="True" 
               onselectedindexchanged="ddlsubject_SelectedIndexChanged">
            </asp:DropDownList>
       </td>
       </tr></table>
       
   </center>
       <br />
       <center><table>
      
       <tr>
       
       <td>
           <asp:Label ID="lblapplfee" runat="server" Text="Application Fees"  CssClass="font"></asp:Label>
       </td>
       <td>
           <asp:TextBox ID="txtapplfee" runat="server" CssClass="font" Width="50px"></asp:TextBox>
           <asp:FilteredTextBoxExtender ID="txtapplfeefil" runat="server" TargetControlID="txtapplfee" FilterType="Numbers"></asp:FilteredTextBoxExtender>
       </td>
       </tr>
       <tr>
      <td>
           <asp:Label ID="lblregexamfee" runat="server" Text="Regular Exam Fee"  CssClass="font"></asp:Label>
       </td>
       <td>
           <asp:TextBox ID="txtregexmfee" runat="server" CssClass="font" Width="50px"></asp:TextBox>
           <asp:FilteredTextBoxExtender ID="txtregexmfeefil"  runat="server" TargetControlID="txtregexmfee" FilterType="Numbers"></asp:FilteredTextBoxExtender>
       </td>
       </tr>
       <tr>
      <td>
           <asp:Label ID="lblarrearexmfee" runat="server" Text="Arrear Exam Fee"  CssClass="font"></asp:Label>
       </td>
       <td>
           <asp:TextBox ID="txtArrexmfee" runat="server" CssClass="font" Width="50px"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="txtArrexmfeefil"   runat="server" TargetControlID="txtArrexmfee" FilterType="Numbers"></asp:FilteredTextBoxExtender>
       </td>
       </tr>
         <tr>
      <td>
           <asp:Label ID="lblretotfee" runat="server" Text="Re-Total Fee"  CssClass="font"></asp:Label>
       </td>
       <td>
           <asp:TextBox ID="txtretotfee" runat="server" CssClass="font" Width="50px"></asp:TextBox>
           <asp:FilteredTextBoxExtender ID="txtretotfeefil"   runat="server" TargetControlID="txtretotfee" FilterType="Numbers"></asp:FilteredTextBoxExtender>
       </td>
       </tr>
        <tr>
      <td>
           <asp:Label ID="lblrevaluation" runat="server" Text="Re-Valuation Fee"  CssClass="font"></asp:Label>
       </td>
       <td>
           <asp:TextBox ID="txtrevalfee" runat="server" CssClass="font" Width="50px"></asp:TextBox>
           <asp:FilteredTextBoxExtender ID="txtrevalfeefil"   runat="server" TargetControlID="txtrevalfee" FilterType="Numbers"></asp:FilteredTextBoxExtender>
       </td>
       </tr>
       <tr>
      <td>
           <asp:Label ID="lblrechalfee" runat="server" Text="Re-Challenge Fee"  CssClass="font"></asp:Label>
       </td>
       <td>
           <asp:TextBox ID="txtrechalfee" runat="server" CssClass="font" Width="50px"></asp:TextBox>
           <asp:FilteredTextBoxExtender ID="txtrechalfeefil"   runat="server" TargetControlID="txtrechalfee" FilterType="Numbers"></asp:FilteredTextBoxExtender>
       </td>
       </tr>
       </table></center>
       <br />
       <br />
    <center><asp:Button ID="BtnApply" runat="server" Text="Apply" CssClass="font" 
            onclick="BtnSave_Click"/></center> 
            <br />
    <FarPoint:FpSpread ID="sprdexamfee" runat="server" BorderColor="Black" BorderStyle="Solid"
        BorderWidth="1px" Height="200" Width="1070" HorizontalScrollBarPolicy="Never" VerticalScrollBarPolicy="Never" >
        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
            ButtonShadowColor="ControlDark">
        </CommandBar>
        <Sheets>
            <FarPoint:SheetView SheetName="Sheet1">
            </FarPoint:SheetView>
        </Sheets>
    </FarPoint:FpSpread>
    <br />
    <br />
    <center><asp:Button ID="btnset" runat="server" Text="Set"  CssClass="font" Visible="false" onclick="btnset_Click"/>
    <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="font" 
            Visible="false" onclick="btnsave_Click" /></center>
            <%--<asp:Panel ID="Panel4" runat="server">
    <asp:Panel ID="pledger" runat="server" BackColor="White" BorderColor="Black" 
                            BorderStyle="Solid" BorderWidth="2px" Height="274px" ScrollBars="Vertical" 
                            style="margin-top: 9px" Width="282px">
                            <asp:CheckBoxList ID="cblledger" runat="server" AutoPostBack="True" 
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" 
                                onselectedindexchanged="cblledger_SelectedIndexChanged" Width="223px" >
                            </asp:CheckBoxList>
                        </asp:Panel>
    </asp:Panel>--%>
</asp:Content>

