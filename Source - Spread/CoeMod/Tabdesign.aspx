<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="Tabdesign.aspx.cs" Inherits="Tabdesign" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Printcontrol" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   <style type="text/css">
        .sty
        {
            font-size: medium;
            font-family: Book Antiqua;
            font-weight:bold;
        }
        .multicheckbox
{
    z-index: 1; left: 258px; top: -1222px; position: absolute; overflow:auto; background-color: white;
    border: 1px solid gray;
    color: Black;
}
.photo
{
    height:150px;
    width:150px;
}


    </style>
        <script type = "text/javascript">
            function PrintPanel() {

                var panel = document.getElementById("<%=pnlContents.ClientID %>");
                var printWindow = window.open('', '', 'height=842,width=1191');
                printWindow.document.write('<html');
                printWindow.document.write('<head><title>DIV Contents</title>');
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   <br />
    <center>
        <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Large" ForeColor="Green" Text="University Result Analysis"></asp:Label>
            </center>
   <center>
   <br />
    <table style="width:900px; height:70px; background-color:#0CA6CA;">
        <tr>
            <td >
                <asp:Label ID="Label1" runat="server" Text="College" Visible="false" Font-Bold="True" Style="font-family: 'Book Antiqua';" ForeColor="Black"
                    Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
            </td>
            <td >
                <asp:DropDownList ID="ddlcollege" runat="server" Visible="false" CssClass="dropdown" Style="font-family: 'Book Antiqua';
                    " Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Width="120px" AutoPostBack="True" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
              <td>
                        <asp:Label ID="lblexamyear" runat="server" Text="Exam Year" Font-Bold="true" Font-Names="Book Antiqua"
                            ForeColor="Black" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlYear" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Width="90px" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblexammonth" runat="server" Text="Exam Month" Font-Bold="true" Font-Names="Book Antiqua"
                            ForeColor="Black" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                      
                        <asp:DropDownList ID="ddlMonth" runat="server" Font-Bold="true" Font-Names="Book Antiqua" Enabled="true"
                            Font-Size="Medium" Width="90px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
            <td >
                <asp:Label ID="lblbach" runat="server" Text="Batch" Style=" font-family: 'Book Antiqua';" Font-Bold="True" ForeColor="Black"
                    Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1"  runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txtbatch" runat="server" Height="20px" CssClass="dropdown" ReadOnly="true"
                            Width="85px" Style=" font-family: 'Book Antiqua'"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                        <asp:Panel ID="pbatch" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid" CssClass="multicheckbox"
                            BorderWidth="2px" Style="font-family: 'Book Antiqua'">
                            <asp:CheckBox ID="chkbatch" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chkbatch_ChekedChange"
                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                            <asp:CheckBoxList ID="chklsbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txtbatch"
                            PopupControlID="pbatch" Position="Bottom">
                        </asp:PopupControlExtender>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:Label ID="lbldegree" runat="server" Text="Degree" Style="font-family: 'Book Antiqua';" Font-Bold="True" ForeColor="Black"
                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
            </td>
            <td >
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txtdegree" runat="server" Height="20px" ReadOnly="true" CssClass="dropdown"
                            Width="85px" Style="font-family: 'Book Antiqua';"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                        <asp:Panel ID="pdegree" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid" CssClass="multicheckbox"
                            BorderWidth="2px" Height="300px" Style="font-family: 'Book Antiqua'; overflow-y: scroll;">
                            <asp:CheckBox ID="chkdegree" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkdegree_CheckedChanged" />
                            <asp:CheckBoxList ID="chklstdegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdegree"
                            PopupControlID="pdegree" Position="Bottom">
                        </asp:PopupControlExtender>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td >
                <asp:Label ID="lblbranch" runat="server" Style="
                    font-family: 'Book Antiqua';" Text="Branch" Font-Bold="True" ForeColor="Black"
                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
            </td>
            <td >
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txtbranch" runat="server" Height="20px" CssClass="dropdown" ReadOnly="true"
                            Width="85px" Style=" font-family: 'Book Antiqua';"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                        <asp:Panel ID="pbranch" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid" CssClass="multicheckbox"
                            BorderWidth="2px" Height="300px" Style="font-family: 'Book Antiqua'; overflow-y: scroll;">
                            <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkbranch_CheckedChanged" />
                            <asp:CheckBoxList ID="chklstbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                Width="350px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                Height="58px" OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtbranch"
                            PopupControlID="pbranch" Position="Bottom">
                        </asp:PopupControlExtender>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
             <td >
                <asp:Label ID="lblcri" runat="server" Visible="false" Style="
                    font-family: 'Book Antiqua';" Text="Test" Font-Bold="True" ForeColor="Black"
                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
            </td>
            <td >
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txttest" runat="server" Visible="false" Height="20px" CssClass="dropdown" ReadOnly="true"
                            Width="120px" Style=" font-family: 'Book Antiqua';"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                        <asp:Panel ID="Panel3" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid" CssClass="multicheckbox"
                            BorderWidth="2px" Height="300px" Style="font-family: 'Book Antiqua'; overflow-y: scroll;">
                            <asp:CheckBox ID="chktest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chktest_CheckedChanged" />
                            <asp:CheckBoxList ID="chkltest" runat="server" Font-Size="Medium" AutoPostBack="True"
                                Width="350px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                Height="58px" OnSelectedIndexChanged="chkltest_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txttest"
                            PopupControlID="Panel3" Position="Bottom">
                        </asp:PopupControlExtender>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td >
                <asp:Button ID="btngo" runat="server" Height="30px" CssClass="dropdown" Text="Go"
                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btngo_Click" />
            </td>
        </tr>
    </table>
   
    </center>
    <br />
   
   
    <asp:Label ID="errmsg" runat="server" ForeColor="Red" CssClass="sty" Visible="false"></asp:Label>
    <br />
   
    <br />
    <div id="showdata" runat="server">

      <asp:Panel id="pnlContents" runat = "server">
        <style type="text/css" media="print">
             
			@page {
      size: A3 portrait;
      margin: 0.5cm;
    }
          
    @media print 
    {
        
         thead {display: table-header-group;}
         tfoot {display: table-footer-group;   }
        #header
        {
            position:fixed;
            top:0px;
            left:0px;
        }
        #footer
        {
            position:fixed;
            bottom:0px;
            left:0px;
        }
                   #printable {
			position:relative;
			bottom:30px;
			height:300;
			}
        
    }
    @media screen {
thead { display: block; }
tfoot { display: block; }
}
    </style>
                <br />
                <div id="printable" >
          <table style=" width:100%" >
           <thead>
      <tr>
         <th>   <div id="Div1" runat="server" >
        <center>
      
    <table style=" width:100%" >
    <tr align="left">
    <td></td>
    <td>
      <center>
    <asp:Label ID="Label2" style="  font-family: book antiqua;
    font-size: 17px;
    font-weight: bold;" runat="server">MADRAS CHRISTIAN COLLEGE (AUTONOMOUS)</asp:Label>
    <br />
      <asp:Label ID="Label3" style="   font-family: book antiqua;
    font-size: 15px;
    font-weight: normal;" runat="server">(Affiliated to the University of Madras)   </asp:Label>
    <br />
     <asp:Label ID="lbltabl_head" style="   font-family: book antiqua; 
    font-size: 15px;
    font-weight: bold;" runat="server"> </asp:Label>

  

    </center>
    </td>
    
    <td></td>
    </tr>
 <tr>
 <td>

     <asp:Label ID="lbldegreeheader" style="  font-family: book antiqua;
    font-size: 14px;
   " runat="server">  </asp:Label></td>
 <td>
 <center>
    

     <asp:Label ID="Label4" style="  font-family: book antiqua;
    font-size: 14px;
   " runat="server">List of Students who have successfully completed the course, passed all the examinations and are qualified for the award of the  </asp:Label>
    </center>
 </td>
 <td></td>
 </tr>
 <tr>
 <td>


     <asp:Label ID="Label6" style="  font-family: book antiqua;
    font-size: 14px; 
   " runat="server"> Center Code : 0171 </asp:Label></td>
 <td>  
 <center>
     <asp:Label ID="lbldgreewithexmm_y" style="  font-family: book antiqua;
    font-size: 14px;
   " runat="server">  </asp:Label>
   </center></td>
 <td style=" text-align:right">   <asp:Label ID="lblstudcount" style="  font-family: book antiqua;
    font-size: 14px;
   " runat="server">  </asp:Label></td></tr>
  
    </table>
      </center>
  
    <style>
    .movetop
    {
        margin-top:15px;
         position: relative;
    top: 10px;
    }
    </style>
  
    
     <center>
    <span id="Span26" runat="server"  style="  font-family: book antiqua; position:relative; bottom: 15px;
    font-size: 18px;
    font-weight: bold;"></span>
    </center>
    </div></th>
        
      </tr>
      <tr>
      <td colspan="3">
      <center>
      <div>
      <table style="width: 1417px; border-color: Black; text-align:center; border-bottom:0px solid black; font-weight:bold; font-size:medium;
    border-style: solid;
    border-width: 1px;">
      <tr>
      <td style=" width:43px;">
      S.No.
      </td>
       <td style="width:107px;">
     Reg.No.
      </td>
       <td style="width:308px; text-align:left;">
      Name
      </td>
      <td style="width:69px;">
     Photo
      </td>
      <td style="width:63px;">
     Sex
      </td>
       <td style="width:117px;">
     Date of Birth
      </td>
       <td style="width:114px;">
    Subject
      </td>
       <td style="width:114px;">
    
      </td>
       <td style="width:110px;">
    CGPA
      </td>
       <td style="width:75px;">
     LG
      </td>
       <td style="width:127px;">
     Class

      </td>

      <td style="width:85px;">
  Applied for Prov.

      </td>

      <td style="width:85px;">
    Applied for Degree

      </td>
      </tr>
      </table>
      </div>
      </center>
      </td>
      </tr>
   </thead>
    <tbody>
          <tr><td>
           <center>
               <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Visible="true" VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never" >
                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                        ButtonShadowColor="ControlDark">
                    </CommandBar>
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
        </center>
          </td></tr>
          </tbody>
       <tfoot style=" width:900px;">
       <BR />  <BR />  <BR />
       <tr>
       <td><center>
       <table style=" width:900px;">
         <tr style=" font-family:Book Antiqua; font-weight:bold; font-size:medium; "><td style=" text-align:left; "><BR /><BR /><BR /><BR />CONTROLLER OF EXAMINATIONS</td> <td style=" text-align:right;"><BR /><BR /><BR /><BR />PRINCIPAL</td> </tr>
       </table>
     
       </center></td>
       </tr>
       </tfoot>
          </table>
           
           </div>
         
            </asp:Panel>

     

            <div id="rptprint" runat="server" visible="true">
         
                    <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text=""  CssClass="fontblack"
                        Visible="true"></asp:Label>
                    <br />
                  
                    <asp:Label ID="lblrptname" runat="server"  CssClass="fontblack" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" CssClass="fontblack"
                        onkeypress="display12()"></asp:TextBox>
                    <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click"  CssClass="fontblack" OnClientClick="return checktxt()"
                        Text="Export To Excel" Width="130px" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" Visible="true" OnClick="btnprintmaster_Click"
                         CssClass="fontblack" />
                           <asp:Button ID="btnPrint" runat="server" CssClass="fontblack" Text="Direct Print"  OnClientClick = "return PrintPanel();" />
                    <Insproplus:Printcontrol runat="server" ID="Printcontrol" Visible="false" />
                </div>
    </div>
    <br />
    <br />
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
</asp:Content>

