<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="BatchAllocationForPractical.aspx.cs" Inherits="BatchAllocationForPractical" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register assembly="FarPoint.Web.Spread" namespace="FarPoint.Web.Spread" tagprefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ScriptManager ID="scrptMngr" runat="server"></asp:ScriptManager>
<style type="text/css">
    .font
{
 font-family:Book Antiqua;
 font-size:medium;
 font-weight:bold;
       
    }
  

    .style1
    {
        
        left: 0px;
        position: absolute;
        width: 962px;
        height: 22px;
    }
     .style2
    {
        top: 250px;
        left: 0px;
        position: absolute;
        width: 962px;
        height: 22px;
    }

    </style>
    <br /><center>
 <asp:Label ID="lblhead" runat="server" Text="Batch Allocation For Laboratory" Font-Bold="True" 
            Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label></center>

    <br /><center>
        <table style="width:700px; height:70px; background-color:#0CA6CA;">
        <tr>
        <td>
<asp:Label ID="lblBatch" runat='server' Text="Batch" CssClass="font"></asp:Label>
        </td>
        <td>
        <asp:DropDownList ID="ddlBatch" runat="server" AutoPostBack="True" 
                onselectedindexchanged="ddlBatch_SelectedIndexChanged"  CssClass="font" Width="60px"></asp:DropDownList>
        </td>
          <td>
        <asp:Label ID="lblDegree" runat="server" Text="Degree" CssClass="font"></asp:Label>
        </td>
        <td>
        <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" 
                onselectedindexchanged="ddlDegree_SelectedIndexChanged" CssClass="font" Width="60px"></asp:DropDownList>
        </td>
          <td>
        <asp:Label ID="lblBranch" runat="server" Text="Branch" CssClass="font"></asp:Label>
        </td>
        <td>
        <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True"  
                CssClass="font" Width="150px" 
                onselectedindexchanged="ddlBranch_SelectedIndexChanged"></asp:DropDownList>
        </td>
           <td>
        <asp:Label ID="lblSem" runat="server" Text="Sem" CssClass="font"></asp:Label>
        </td>
        <td>
        <asp:DropDownList ID="ddlSem" runat="server" CssClass="font"  Width="40px" 
                AutoPostBack="True" onselectedindexchanged="ddlSem_SelectedIndexChanged"></asp:DropDownList>
        </td>
          <td>
        <asp:Label ID="lblSection" runat="server" Text="Sec" CssClass="font"></asp:Label>
        </td>
        <td>
        <asp:DropDownList ID="ddlSection" runat="server" CssClass="font"  Width="40px" 
                onselectedindexchanged="ddlSection_SelectedIndexChanged" 
                AutoPostBack="True"></asp:DropDownList>
        </td>
          
        
      
        </tr>
      
        <tr>
         <td>
        <asp:Label ID="lblSubjectType" runat="server" Text="SubType" CssClass="font"></asp:Label>
        </td>
        <td>
        <asp:DropDownList ID="ddlSubjectType" runat="server" CssClass="font" Width="90px" 
                AutoPostBack="True" 
                onselectedindexchanged="ddlSubjectType_SelectedIndexChanged" ></asp:DropDownList>
        </td>
             <td>
        <asp:Label ID="lblSubject" runat="server" Text="Subject" CssClass="font"></asp:Label>
        </td>
        <td>
        <asp:DropDownList ID="ddlSubject" runat="server" CssClass="font" Width="150px" 
                AutoPostBack="True" 
                onselectedindexchanged="ddlSubject_SelectedIndexChanged"></asp:DropDownList>
        </td>
                 <td>
        <asp:Label ID="lblBatches" runat="server" Text="No.of Batches" CssClass="font" Width="110px"></asp:Label>
        </td>
        <td>
        <asp:TextBox ID="txBatch" runat="server" CssClass="font" Width="30px" Height="22px"></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txBatch" FilterType="Numbers" runat="server"></asp:FilteredTextBoxExtender>
      
        
        </td>
        <td>
          <asp:Label ID="lblExamDate" runat="server" Text="Exam Date" CssClass="font" Width="80px"></asp:Label>
        </td>
        <td>
        <asp:TextBox ID="txtExamDate" runat="server" CssClass="font" Width="75px" 
                Height="22px" ontextchanged="txtExamDate_TextChanged"></asp:TextBox>
         <asp:CalendarExtender ID="TextBox1_CalendarExtender6" runat="server" Format="dd/MM/yyyy"
          Enabled="True" TargetControlID="txtExamDate">
      </asp:CalendarExtender>
        </td>
        <td>
          <asp:Label ID="lblSession" runat="server" Text="Session" CssClass="font"></asp:Label>
        </td>
        <td>
        <asp:DropDownList ID="ddlSession" runat="server" Height="24px" Width="53px"  
                CssClass="font" onselectedindexchanged="ddlSession_SelectedIndexChanged"></asp:DropDownList>
        </td>
        <td>
        <asp:Button ID="btnGo" runat="server" Text="Go"  CssClass="font" 
                onclick="btnGo_Click" />
        </td>
        </tr>
        </table>
        </center>
        <br />
       
         
         
    <asp:Label ID="lblerror" runat="server" Text="Label" Font-Bold="True" 
        Font-Names="Book Antiqua" Font-Size="Small" ForeColor="Red" Visible="False"></asp:Label>
        <center>
          <farpoint:fpspread ID="Fpstudents" runat="server"  HorizontalScrollBarPolicy="Never" VerticalScrollBarPolicy="Never" 
            Height="221px" Width="660px" Visible="false" OnUpdateCommand="Fpstudents_UpdateCommand">
            <commandbar backcolor="Control" buttonfacecolor="Control" 
                buttonhighlightcolor="ControlLightLight" buttonshadowcolor="ControlDark">
            </commandbar>
            <sheets>
                <FarPoint:sheetview SheetName="Sheet1">
                </FarPoint:sheetview>
            </sheets>
        </farpoint:fpspread></center>
         <center>
             <asp:Button ID="btnsave" runat="server" Text="Save" onclick="btnsave_Click" 
                 Visible="False" />
         </center>

</asp:Content>

