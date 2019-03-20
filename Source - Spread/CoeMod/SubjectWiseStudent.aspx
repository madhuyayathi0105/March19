<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="SubjectWiseStudent.aspx.cs" Inherits="Timetablenew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register assembly="FarPoint.Web.Spread" namespace="FarPoint.Web.Spread" tagprefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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

    <div>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
   
    </asp:ScriptManager>
     <asp:UpdatePanel ID="pnlPageload" runat="server">
     <ContentTemplate>
    <br /><center>
            
            <asp:Label ID="Label1" runat="server" Text ="Subject Wise Student Report" 
            Font-Bold="True" 
            Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label></center>

           <br />
           <br /> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
   <center> <FarPoint:FpSpread ID="HAllSpread" runat="server" BorderColor="Black" BorderStyle="Solid" oncellclick="HAllSpread_CellClick" OnPreRender="HAllSpread_SelectedIndexChanged" CssClass="stylefp" OnUpdateCommand="HAllSpread_UpdateCommand"
        BorderWidth="1px" Height="800" Width="940" HorizontalScrollBarPolicy="Never"  >
        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
            ButtonShadowColor="ControlDark">
        </CommandBar>
        <Sheets>
            <FarPoint:SheetView SheetName="Sheet1">
            </FarPoint:SheetView>
        </Sheets>
    </FarPoint:FpSpread></center>
    <br />
        <center> <asp:Button ID="Button1" runat="server" Text="Ok" Font-Bold="True" 
                Font-Names="Book Antiqua" Font-Size="Medium" onclick="Button1_Click" /></center>
    <br />
    <center>
    <FarPoint:FpSpread ID="individualsubstud" runat="server" BorderColor="Black" BorderStyle="Solid"
        BorderWidth="1px" Height="200" Width="680" Visible="false" HorizontalScrollBarPolicy="Never" >
        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
            ButtonShadowColor="ControlDark" ButtonType="PushButton" ShowPDFButton="True">
        </CommandBar>
        <Sheets>
            <FarPoint:SheetView SheetName="Sheet1">
            </FarPoint:SheetView>
        </Sheets>
    </FarPoint:FpSpread></center>
     </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

