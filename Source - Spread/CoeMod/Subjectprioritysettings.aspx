<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="Subjectprioritysettings.aspx.cs" Inherits="Subjectprioritysettings" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Printcontrol" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
        .cpBody
        {
            background-color: #DCE4F9;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
        }
    </style>
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblerror').innerHTML = "";
        }   
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 <asp:UpdatePanel ID="up1" runat="server">
   <ContentTemplate>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
     <br />
              <center>
            <asp:Label ID="Label4" runat="server" Text="Subject Wise Priority Settings" Font-Bold="True" 
                ForeColor="Green" Font-Names="Book Antiqua" Font-Size="Large"></asp:Label>
                </center><br />
     <center>
        <table style="width:700px; height:50px; background-color:#0CA6CA;">
           <tr>
                  <td>
                        <asp:Label ID="Lblbatch" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                             Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="120px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                             Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="120px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="LblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                             Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="120Px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                      <td>
                        <asp:Label ID="lblsem" runat="server" Text="Semester" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium"  AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                 <td>
                        <asp:Label ID="Label1" runat="server" Text="Subject Type" Font-Bold="True" Font-Names="Book Antiqua" Visible="false"
                             Font-Size="Medium"></asp:Label>
                    </td>
                   <td>
                        <asp:DropDownList ID="ddlsubtype" runat="server" Font-Bold="true" Font-Names="Book Antiqua" Visible="false"
                            Font-Size="Medium" Width="120px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlsubtype_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                        
                    <td >
                 
                        <asp:Button ID="Buttongo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="Buttongo_Click" Text="Go" />
                    </td>
                </tr>
               
            </table>
       </center>   
        <br />
        <asp:Label ID="lblerror" runat="server" Font-Names="Book Antiqua" Font-Size="Medium" Width="800px"
            Font-Bold="true" ForeColor="Red"></asp:Label>
        <br />
        <asp:Button ID="btnreset" runat="server" OnClick="btnresetclick"  Text="Reset" Visible="false"  style=" font-family:Book Antiqua;
            font-size:medium;
            font-weight:bold;
            color:Black;"/>
            <br />
        <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid" ShowHeaderSelection="false"
            BorderWidth="1px" Height="350" Width="680" Visible="false" OnUpdateCommand="FpSpread1_UpdateCommand"
            HorizontalScrollBarPolicy="Never">
            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                ButtonShadowColor="ControlDark" ShowPDFButton="false">
            </CommandBar>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
        <br />
       
        <br />
        <asp:Button ID="btnsave" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" OnClick="btnsave_Click" Text="Save" />
        <br />
</ContentTemplate>
   </asp:UpdatePanel>
   
</asp:Content>

