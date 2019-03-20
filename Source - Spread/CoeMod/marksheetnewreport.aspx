<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="marksheetnewreport.aspx.cs" Inherits="marksheetnewreport" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
      .stylefp
         {
             cursor:pointer;
             
         }
         .ModalPopupBG
{
    background-color: #666699;   
    filter: alpha(opacity=50);
    opacity: 0.7;
}

.HellowWorldPopup
{
    min-width:600px;
    min-height:400px;
    background:white;
   
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager><br />
    <center>
    <asp:Label ID="Label1" runat="server" Text ="Consolidated Mark Sheet" 
            Font-Bold="True" 
            Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label></center>
<br /><center>
<table style="width:700px; height:70px; background-color:#0CA6CA;">
<tr>
<td>
    <asp:Label ID="lblbatchyear" runat="server" Text="Batch" Font-Bold="True" 
        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
</td>
<td>
    <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" 
        Font-Names="Book Antiqua" Font-Size="Medium" Width="70px" 
        onselectedindexchanged="ddlbatch_SelectedIndexChanged" AutoPostBack="True">
    </asp:DropDownList>
</td>
<td>
    <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" 
        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
</td>
<td>
    <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="True" 
        Font-Names="Book Antiqua" Font-Size="Medium" Width="100px" 
        onselectedindexchanged="ddldegree_SelectedIndexChanged" 
        AutoPostBack="True">
    </asp:DropDownList>
</td>
<td>
    <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" 
        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
</td>
<td>
    <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="True" 
        Font-Names="Book Antiqua" Font-Size="Medium" Width="150px" 
        onselectedindexchanged="ddlbranch_SelectedIndexChanged" 
        AutoPostBack="True">
    </asp:DropDownList>
</td>
<td>
    <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" 
        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
</td>
<td>
    <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="True" 
        Font-Names="Book Antiqua" Font-Size="Medium" Width="40px" 
        onselectedindexchanged="ddlsem_SelectedIndexChanged" AutoPostBack="True">
    </asp:DropDownList>
   </td>
    <td>
        <asp:Label ID="lblsection" runat="server" Text="Section" Font-Bold="True" 
            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
    </td>
    <td>
        <asp:DropDownList ID="ddlSec" runat="server" Font-Bold="True" 
            Font-Names="Book Antiqua" Font-Size="Medium" Width="40px">
        </asp:DropDownList>
    </td>
    <td>
        <asp:Button ID="btngo" runat="server" Text="Go" Font-Bold="True" 
            Font-Names="Book Antiqua" Font-Size="Medium" onclick="btngo_Click" />
    </td>
 </tr>
<tr>
<td>
    <asp:Label ID="lbldate" runat="server" Text="Date" Font-Bold="True" 
        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
</td>
<td>
    <asp:TextBox ID="TextBox1" runat="server" Font-Bold="True" 
        Font-Names="Book Antiqua" Font-Size="Medium" 
        ontextchanged="TextBox1_TextChanged" Width="85px"></asp:TextBox>
      <asp:CalendarExtender ID="TextBox1_CalendarExtender6" runat="server" Format="dd/MM/yyyy"
          Enabled="True" TargetControlID="TextBox1">
      </asp:CalendarExtender>

</td>
</tr>
</table>
</center>

<br />
    <asp:Label ID="lblnorec" runat="server" Text="No record Found" Font-Bold="True" 
        Font-Names="Book Antiqua" Font-Size="Small" ForeColor="#FF3300" Visible="False"></asp:Label>
           <br />
           <br /><center>
    <FarPoint:FpSpread ID="sprdmarksheet" runat="server" BorderColor="Black" BorderStyle="Solid"
        BorderWidth="1px" Height="200" Width="1250" Visible="false">
        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
            ButtonShadowColor="ControlDark" ButtonType="PushButton" ShowPDFButton="True">
        </CommandBar>
        <Sheets>
            <FarPoint:SheetView SheetName="Sheet1">
            </FarPoint:SheetView>
        </Sheets>
    </FarPoint:FpSpread></center>
    <center>
    <asp:Button ID="btnexcel" runat="server" Text="Excel" Font-Bold="True" 
            Font-Names="Book Antiqua" Font-Size="Medium" onclick="btnexcel_Click" 
            Visible="False" />
    </center>
</asp:Content>

