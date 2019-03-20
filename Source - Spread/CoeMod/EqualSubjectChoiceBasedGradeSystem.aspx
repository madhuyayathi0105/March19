<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="EqualSubjectChoiceBasedGradeSystem.aspx.cs" Inherits="EqualSubjectChoiceBasedGradeSystem" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Printcontrol" TagPrefix="Insproplus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

 
        
   
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
       
       <br />
            <center>
            <asp:Label ID="Label4" runat="server" Text="Equal Subject For Choice Based Grade System" Font-Bold="True" 
                ForeColor="Green" Font-Names="Book Antiqua" Font-Size="Large"></asp:Label>
                </center>
           <br />
                    
       
       <center>
         <table style="width:700px; height:70px; background-color:#0CA6CA;">
                <tr>
                    <td>
                        <asp:Label ID="lblcollege" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"
                             Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="230px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlcollge_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbleduleve" runat="server" Text="Edu Level" Font-Bold="True" Font-Names="Book Antiqua"
                             Font-Size="Medium" Width="100px"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddledu" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="65px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddledu_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblbatch" runat="server" Text="Batch From" Font-Bold="True" Font-Names="Book Antiqua"
                             Font-Size="Medium" Width="100px"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="60px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblsubtype" runat="server" Text="Subject Type" Font-Bold="True" Font-Names="Book Antiqua"
                             Font-Size="Medium" Width="100px"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsubtype" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="160px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlsubtype_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
          
                <tr>
                    <td>
                        <asp:Label ID="lblexamyear" runat="server" Text="Exam Year" Font-Bold="True" Font-Names="Book Antiqua"
                             Font-Size="Medium" Width="100px"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlexamyear" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="60px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlexamyear_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblexammonth" runat="server" Text="Exam Month" Font-Bold="True" Font-Names="Book Antiqua"
                             Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlmonth" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="90px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlmonth_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="Buttongo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="Buttongo_Click" Text="Go" />
                    </td>
                </tr>
            </table>
       </center>
        
        
        <asp:UpdatePanel ID="upd" runat="server">
            <ContentTemplate>
                <asp:Label ID="errmsg" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                    Width="800px" Font-Bold="true" ForeColor="Red"></asp:Label>
                
                <asp:Button ID="btnsave" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btnsave_Click" Text="Save" />
                <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderWidth="2px" Height="50px">
                    <CommandBar BackColor="Control" ButtonFaceColor="Control" Visible="false" ButtonHighlightColor="ControlLightLight"
                        ButtonShadowColor="ControlDark">
                    </CommandBar>
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                
                <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderWidth="2px" Height="50px">
                    <CommandBar BackColor="Control" ButtonFaceColor="Control" Visible="false" ButtonHighlightColor="ControlLightLight"
                        ButtonShadowColor="ControlDark">
                    </CommandBar>
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                
                <asp:Button ID="btndelete" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btndelete_Click" Text="Delete" />
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>

