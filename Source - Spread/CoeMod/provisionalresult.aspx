<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="provisionalresult.aspx.cs" Inherits="provisionalresult" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register assembly="FarPoint.Web.Spread" namespace="FarPoint.Web.Spread" tagprefix="FarPoint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
        .style1
        {
            width: 101px;
        }
        .style2
        {
            width: 124px;
        }
        .style3
        {
            width: 316px;
        }
                
        .style5
        {
            width: 87px;
        }
        
        .style6
        {
            width: 770px;
        }
        
        .style7
        {
            width: 92px;
        }
        
        .style9
        {
            width: 771px;
        }
        .style10
        {
            width: 50px;
        }
        .style11
        {
            width: 766px;
        }
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<html>
<body oncontextmenu="return false">
<asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager><br /><center>
<asp:Label ID="lblhead" runat="server"  Font-Bold="True" 
            Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
            </center>
                     
    <br /><center>
    <br />
<table style="width:700px; height:70px; background-color:#0CA6CA;">
                 <tr>
                     <td>
                         <asp:Label runat="server" ID="lblbatch" Text="Batch"  Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                         </td>
                         <td>
                         <asp:DropDownList ID="ddlbatch" runat="server"  Font-Bold="True" 
         Font-Names="Book Antiqua" Font-Size="Medium" 
         onselectedindexchanged="ddlbatch_SelectedIndexChanged" Height="25px" 
         Width="56px" AutoPostBack="True" 
         >
                         </asp:DropDownList>
                     </td>
                     <td>
                         <asp:Label runat="server" ID="lbldegree" Text="Degree"  Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" ></asp:Label>
                         </td>
                         <td>
                         <asp:DropDownList runat="server" ID="ddldegree"  Height="25px" Width="66px"   
         AutoPostBack="True" 
         onselectedindexchanged="ddldegree_SelectedIndexChanged"  Font-Bold="True" 
                             Font-Names="Book Antiqua" Font-Size="Medium" >
                         </asp:DropDownList>
                     </td>
                     <td>
                         <asp:Label runat="server" ID="lblbranch" Text="Branch"  Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                         </td>
                         <td>
                         <asp:DropDownList runat="server" ID="ddlbranch"  Font-Bold="True" Height="25px" Width="180px" 
         Font-Names="Book Antiqua" Font-Size="Medium"
         onselectedindexchanged="ddlbranch_SelectedIndexChanged" 
         AutoPostBack="True">
                         </asp:DropDownList>
                     </td>
                     <td>
                         <asp:Label runat="server" ID="lblduration" Text="Sem"  Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                         </td>
                         <td>
                         <asp:DropDownList runat="server" ID="ddlduration"  Font-Bold="True" 
        Font-Names="Book Antiqua" Font-Size="Medium"
         AutoPostBack="True" Height="25px" Width="47px" 
         onselectedindexchanged="ddlduration_SelectedIndexChanged" 
        >
                         </asp:DropDownList>
                  
                     </td>
                     <td>
                         <asp:Label runat="server" ID="lblsec" Text="Sec"  Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                         </td>
                         <td>
                         <asp:DropDownList runat="server" ID="ddlsec"  Font-Bold="True" 
         Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" Width="40px" 
          AutoPostBack="True" onselectedindexchanged="ddlsec_SelectedIndexChanged" >
                         </asp:DropDownList>
                     </td>
                     <td>
                         <asp:CheckBox ID="chk_regular" runat="server" Text="Regular" Font-Bold="True" 
         Font-Names="Book Antiqua" Font-Size="Medium" Width="80px"/>
         </td>
         <td>
                         <asp:CheckBox ID="chk_arrear" runat="server" Text="Arrear" Font-Bold="True" 
         Font-Names="Book Antiqua" Font-Size="Medium" Width="70px"/>
                     </td>
                    <td>    
                        <asp:Button ID="btngo" runat="server" Text="Go" Font-Bold="True" 
         Font-Names="Book Antiqua" Font-Size="Medium" onclick="btngo_Click" /></td>
                   
                 </tr>
             </table></center>
    <table>
    <tr><td class="style6"></td></tr>
    <tr><td class="style6"><br />
               
        </asp:Panel></td></tr>
        <tr><td class="style6"></td></tr>
        <tr><td class="style6"></td></tr>
        <tr><td class="style6"></td></tr>
          <tr><td class="style6"></td></tr>
        <tr><td class="style6"></td></tr>
        <tr><td class="style6"></td></tr>
          <tr><td class="style6"></td></tr>
        
      
    <tr>
    <td class="style6">
        <asp:Label ID="noreclbl" runat="server" Font-Bold="True"  ForeColor="Red"
         Font-Names="Book Antiqua" Font-Size="Medium" ></asp:Label></td>
         </tr><tr>
         <td><asp:Panel ID="setpanel" runat="server" 
          BorderStyle="None" Width="1026px">
      
                     <asp:Label ID="Buttontotal" runat="server"  Font-Bold="True"  Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>                 
                     &nbsp;&nbsp;                 
                    <asp:Label ID="ddlpagelbl" runat="server"  Font-Bold="True" 
                        Text="     Records Per Page" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>         
           <asp:DropDownList ID="DropDownListpage" runat="server" Height="25px" Width="48px" Font-Bold="True" 
                         Font-Size="Medium"  Font-Names="Book Antiqua" 
                         onselectedindexchanged="DropDownListpage_SelectedIndexChanged" 
                         AutoPostBack="True" >
                        
           </asp:DropDownList>  
           <asp:TextBox ID="pageddltxt" runat="server" Height="19px" Width="40px" Font-Bold="True" 
                         Font-Size="Medium"  Font-Names="Book Antiqua" 
                         ontextchanged="pageddltxt_TextChanged" AutoPostBack="True" ></asp:TextBox>
           <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers" TargetControlID="pageddltxt">
           </asp:FilteredTextBoxExtender>
                        <asp:Label ID="pgsearch_lbl" runat="server" Font-Bold="True" Text="Page Search" 
                    Width="95px" Font-Names="Book Antiqua" Font-Size="Medium" Height="24px"></asp:Label>                 
                          <asp:TextBox ID="pagesearch_txt" runat="server" AutoPostBack="True" 
                      Font-Bold="True" 
                        Font-Names="Book Antiqua" Font-Size="Medium" Height="20px" Width="40px" ontextchanged="pagesearch_txt_TextChanged"
           ></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"   TargetControlID="pagesearch_txt"         
    FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>
                        <asp:Label ID="errlbl" runat="server"   ForeColor="Red" 
            Font-Bold="True" Font-Names="Book Antiqua"  Height="19px" 
                         Width="291px"></asp:Label>
                    </asp:Panel></td>
    </tr>
    <tr>
        
    
    <td class="style6">
    <asp:Panel ID="pagesetpanel" runat="server">
      
        <asp:RadioButton ID="RadioHeader" runat="server"    Font-Names="Book Antiqua" 
            Font-Size="Medium" Font-Bold="true"    Text="Header in All Pages" 
            oncheckedchanged="RadioHeader_CheckedChanged"   GroupName="header"  Checked="true"
            AutoPostBack="True" />
        <asp:RadioButton ID="Radiowithoutheader"  runat="server"  
            oncheckedchanged="Radiowithoutheader_CheckedChanged" Font-Bold="True" 
                Font-Names="Book Antiqua" Font-Size="Medium" GroupName="header" 
                Text="Header in 1st Page" AutoPostBack="True" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblpages" runat="server" Text="Page"   Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true"></asp:Label>  
                <asp:DropDownList ID="ddlpage" runat="server" 
            onselectedindexchanged="ddlpage_SelectedIndexChanged"    
            Font-Names="Book Antiqua" Font-Size="Medium"  Font-Bold="true" 
            AutoPostBack="True" Width="48px">
                </asp:DropDownList>
                  </asp:Panel>
    </td></tr>
    <tr><td class="style6">
        <farpoint:fpspread id="provisional_spread" runat="server" bordercolor="Black" borderstyle="Solid" borderwidth="1px" 
            height="200" width="400">
            <commandbar backcolor="Control" buttonfacecolor="Control" buttonhighlightcolor="ControlLightLight" ButtonType="PushButton"
                buttonshadowcolor="ControlDark"></commandbar>
            <sheets><FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black" ></FarPoint:SheetView></sheets>
        </farpoint:fpspread></td></tr>
    </table>
    </div>
</body>
</html>
</asp:Content>

