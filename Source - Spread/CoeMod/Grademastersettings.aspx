<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="Grademastersettings.aspx.cs" Inherits="Grademastersettings" %>

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
 <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
     <br />
              <center>
            <asp:Label ID="Label4" runat="server" Text="Grade Master" Font-Bold="True" style=" position:relative; top:5px; text-transform:uppercase;" Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
                </center>
       
     <br /><center>
            <table style="width:700px; height:70px; background-color:#0CA6CA;">
           
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
                    
            
                        
                    <td >
                    &nbsp;
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
                         <asp:Button ID="Button1" runat="server" Text="Add Row" Visible="false" CssClass="fontblack"
                                 OnClick="Button1_Click1" />   
        <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid" ShowHeaderSelection="false"
            BorderWidth="1px" Height="350" Width="680" Visible="false" 
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
        <div id="showdata" runat="server" visible="false">
        
      
        
        <div id="rptprint" runat="server" visible="true">
         
                    <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text=""  CssClass="fontblack"
                        Visible="true"></asp:Label>
                    <br />
                     <asp:Button ID="btnsave" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" OnClick="btnsave_Click" Text="Save" />
            <asp:Button ID="btnfpspread1delete" runat="server" Text="Delete All" CssClass="fontblack" Width="90px" OnClick="btnfpspread1delete_Click1" />
                    <asp:Label ID="lblrptname" runat="server"  CssClass="fontblack" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" CssClass="fontblack"
                        onkeypress="display12()"></asp:TextBox>
                    <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click"  CssClass="fontblack" OnClientClick="return checktxt()"
                        Text="Export To Excel" Width="130px" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" Visible="false" OnClick="btnprintmaster_Click"
                         CssClass="fontblack" />
                    <Insproplus:Printcontrol runat="server" ID="Printcontrol" Visible="false" />
                </div>

                   
     <table >
                        <tr>
                            <td style="width: auto;">
                                <asp:FileUpload runat="server" ID="fpmarkexcel" Visible="true"  Font-Names="Book Antiqua"
                                    Font-Bold="True" Font-Size="Medium" />
                            </td>
                            <td>
                                <asp:Button ID="btn_import" Text="Import" runat="server" Visible="true"  Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btn_importex" />
                            </td>
                        </tr>
                    </table>
                   
             <FarPoint:FpSpread ID="fpmarkimport" runat="server" BorderColor="Black" BorderStyle="Solid"
                BorderWidth="1px" Visible="false" VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never"
                CssClass="stylefp">
                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            </div>
                 <style>
            .fontblack
            {
                font-family: Book Antiqua;
                font-size: medium;
                font-weight: bold;
                color: Black;
            }
           
        </style>
                   <script type="text/javascript" language="javascript">

                       function display12() {
                           document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
                       }
                       function checktxt() {
                           empty = "";
                           id = document.getElementById("<%=txtexcelname.ClientID %>").value;
                           if (id.trim() == "") {
                               document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "Please Enter Your Report Name";
                               empty = "E";
                           }

                           if (empty != "") {
                               return false;
                           }
                           else {

                               return true;
                           }
                       }
        </script>
<%--</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="btnprintmaster" />
<asp:PostBackTrigger ControlID="btnExcel" />
<asp:PostBackTrigger ControlID="btn_import" />
</Triggers>
   </asp:UpdatePanel>--%>
   
</asp:Content>

