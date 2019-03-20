<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="Studentwise_Arrear_List.aspx.cs" Inherits="Studentwise_Arrear_List" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html>
      <script type="text/javascript">
          function PrintPanel() {
              var panel = document.getElementById("<%=pnlContents.ClientID %>");
              var printWindow = window.open('', '', 'height=842,width=1191');
              printWindow.document.write('<html');
              printWindow.document.write('<head><title>Student Arrear List</title>');
              printWindow.document.write('</head><body >');
              printWindow.document.write('<form>');
              printWindow.document.write(panel.innerHTML);
              printWindow.document.write(' </form>');
              printWindow.document.write('</body></html>');
              printWindow.document.close();
              setTimeout(function () {
                  printWindow.print();
              }, 500);
              return false;
          }
    </script>
    <style type="text/css">
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
     .style37
      {
          position: absolute;
          left: 711px;
          top: 144px;
      }
      .style38
      {
          left: 12px;
          top: 337px;
          width: 65px;
          height: 20px;
      }
      .style39
      {
          height: 73px;
          width: 1017px;
      }
      .style42
      {
          top: 449px;
          left: 638px;
          height: 33px;
          width: 145px;
      }
      .style43
      {
          top: 204px;
          left: 188px;
          position: absolute;
          height: 21px;
          width: 126px;
          bottom: 272px;
      }
      .style44
      {
          top: 200px;
          left: 315px;
          position: absolute;
      }
      .style45
      {
          top: 206px;
          left: 376px;
          position: absolute;
      }
      .style46
      {
          top: 204px;
          left: 406px;
          position: absolute;
          height: 21px;
      }
      .style47
      {
          top: 206px;
          left: 498px;
          position: absolute;
          width: 34px;
      }
      .style48
      {
          top: 205px;
          left: 534px;
          position: absolute;
          height: 21px;
          width: 303px;
      }
      .style49
      {
          top: 103px;
          left: 690px;
          position: absolute;
          width: 48px;
      }
      .style50
      {
          top: 106px;
          left: 17px;
          position: absolute;
          height: 21px;
          width: 46px;
      }
      .style51
      {
          top: 104px;
          left: 67px;
          position: absolute;
          height: 26px;
          width: 56px;
      }
      .style52
      {
          top: 107px;
          left: 130px;
          position: absolute;
          height: 21px;
          width: 56px;
      }
      .style53
      {
          top: 105px;
          left: 191px;
          position: absolute;
      }
      .style54
      {
          top: 133px;
          left: 114px;
          position: absolute;
          width: 59px;
          height: 21px;
      }
      .style57
      {
          top: 0px;
          left: 50px;
          width: 42px;
          height: 21px;
          position: absolute;
      }
}
     </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
         <br />   <center>
     <asp:Label ID="Label1" runat="server" Text="Studentwise Arrears List" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label></center>
  <br />         <center>
                <table style="width:700px; height:50px; background-color:#0CA6CA;">
                    <tr>
                        <td>
                            <asp:Label ID="lblYear" runat="server" Text="Batch" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                           <asp:DropDownList ID="ddlbatch" runat="server" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"
                                AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                CausesValidation="True" Width="64px" Height="23px">
                            </asp:DropDownList>
                        </td>
                        <td>
                         <asp:Label ID="lblDegree" runat="server" Text="Degree " Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua">
                            </asp:Label>
                        </td>
                        <td>    
                            <asp:DropDownList ID="ddldegree" runat="server" AutoPostBack="True" Height="23px"
                                OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CausesValidation="True" Width="70px">
                            </asp:DropDownList>
                        </td>
                        <td>
                         
                            <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>  
                            <asp:DropDownList ID="ddlbranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True"
                                Width="180px" Height="23px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            
                            <asp:Label ID="lblfrmsem" runat="server" Text="From Sem" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua" Width="80px">
                            </asp:Label>
                        </td>
                        <td>
                           
                            <asp:DropDownList ID="ddlfrmsem" runat="server" AutoPostBack="True" Height="23px"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True"
                                OnSelectedIndexChanged="ddlfrmsem_SelectedIndexChanged" Width="50px">
                            </asp:DropDownList>
                        </td>
                        <td>
                          <asp:Label ID="lbltoSem" runat="server" Text="To sem" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua" Width="60px">
                            </asp:Label>
                        </td>
                        <td>
                           <asp:DropDownList ID="ddltosem" runat="server" AutoPostBack="true" Height="23px"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddltosem_SelectedIndexChanged"
                                Width="50px">
                            </asp:DropDownList>
                        </td>
                        <td>
                           <asp:Button ID="Button1" runat="server" Text="Go" OnClick="btnGo_Click" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" Width="44px" Height="27px" />
                        </td>
                    </tr>
                </table>
            </center>    
            <br />
               
            <asp:Panel ID="Panel1" runat="server">
                <table>
                    <tr>
                        <td
                            <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" ForeColor="#FF3300" Text="No Record(s) Found" CssClass="style2"
                                Width="500"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
           <br />
           <center>
                <asp:Panel ID="pnlContents" runat="server" Visible="true" Style="margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative;">
               <style type="text/css" media="print">
                @page
                {
                    size: A3 portrait;
                    margin: 0.5cm;
                }
                @media print
                {
                    .printclass
                    {
                        display: table;
                    }
                      #printable
                    {
                        position: relative;
                        bottom: 30px;
                        height: 300;
                    }
                }
               </style>
               <center>
                <div id="printable">
                <center>
                   <table  class="printclass" style="width: 100%; font-weight: bold; font-family: Book Antiqua;
                                        font-size: medium; margin-top: 20px;">
                                        <tr>
                                        <td rowspan="4" style="margin: 0px; padding: 0px; margin-left:20px ; width: 70px;">
                                        <asp:Image ID="imgLeftLogo2" runat="server" AlternateText="" ImageUrl="~/college/Left_Logo.jpeg" style="width: 60px; height: 60px; margin: 0px; padding: 0px;" Visible="false"  />
                                    </td>
                                     <td colspan="5" align="center">
                                        <span id="Span1" runat="server" style="font-weight: bold; font-size: large;" Visible="false">
                                        </span>
                                    </td>
                                     <td rowspan="4" style="margin: 0px; padding: 0px; width: 70px;">
                                        <asp:Image ID="Image1" runat="server" AlternateText="" ImageUrl="~/college/Right_Logo.jpeg" style="width: 60px; height: 60px; margin: 0px; padding: 0px;" Visible="false"    />
                                    </td>
                                    </tr><tr>
                                     <td colspan="5" align="center">
                                        <span id="spnCollegeHeader" runat="server" style="font-weight: bold; font-size: large;" Visible="false">
                                        </span>
                                    </td>
                                        
                                        </tr>
                                        <tr>
                                         <td colspan="5" align="center">
                                        <span id="Span2" runat="server" style="font-weight: bold; font-size: large;" Visible="false">
                                        </span>
                                        </td>
                                        </tr>
                                         <tr>
                                         <td colspan="5" align="center">
                                        <span id="Span3" runat="server" style="font-weight: bold; font-size: large;" Visible="false">
                                        </span>
                                        </td>
                                        </tr>
                                        
                                        <tr><td></td><td style="margin-left:50px">
                                       
            <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                BorderWidth="1px" Height="200" Width="1200px">
                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark" ButtonType="PushButton">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="True">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread></td>
            </tr>
           
            </table>
             </center>
                </div>
                </center>
            </asp:Panel>
        </center>
        <br />
        <center>
          <asp:Button ID="btnprintmaster" runat="server" Visible="false" Text="Print" 
                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" OnClientClick="return PrintPanel();" />
                  
                    </center>
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
    </body>
    </html>
</asp:Content>

