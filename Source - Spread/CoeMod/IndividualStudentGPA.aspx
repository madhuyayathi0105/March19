<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="IndividualStudentGPA.aspx.cs" Inherits="IndividualStudentGPA" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <style type="text/css">
        .style1
        {
            width: 135px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<body>

  <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
     <br /><center>
               <asp:Label ID="Label5" runat="server" Font-Bold="True" 
                   Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green" 
                   Text="Consolidated student GPA and CGPA"></asp:Label></center>
            
        <br />
        <center>
     <table style="width:700px; height:70px; background-color:#0CA6CA;">
  <tr>
            <td>
            <asp:Label ID="lblbatch" runat="server" Text="Batch"  Width="100px" Font-Bold="True" ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua" ></asp:Label>
            </td>

            <td>
            <div style="position:relative">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                <asp:TextBox ID="txtbatch" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true" Width="100px" style=" font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                <asp:Panel ID="pbatch" runat="server" Width="110px" CssClass="MultipleSelectionDDL" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical" Height="150px" >
                <asp:CheckBox ID="chkbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"  oncheckedchanged="chkbatch_CheckedChanged" Text="Select All"  AutoPostBack="True" />
                <asp:CheckBoxList ID="chklstbatch" runat="server" Font-Size="Medium" AutoPostBack="True" onselectedindexchanged="chklstbatch_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"></asp:CheckBoxList>
                </asp:Panel>
                <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txtbatch"
                                PopupControlID="pbatch" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
               </div>
             </td>

             <td>
             <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Width="100px"></asp:Label>
             </td>

             <td>
             <div style="position:relative">
             <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                <asp:TextBox ID="txtdegree" runat="server" Height="20px" CssClass="Dropdown_Txt_Box" ReadOnly="true" Width="100px" style=" font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox> 
                <asp:Panel ID="pdegree" runat="server" CssClass="MultipleSelectionDDL" Width="110px" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical" Height="150px">
                <asp:CheckBox ID="chkdegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"  oncheckedchanged="chkdegree_CheckedChanged" Text="Select All"  AutoPostBack="True" />
                <asp:CheckBoxList ID="chklstdegree" runat="server" Font-Size="Medium" AutoPostBack="True" onselectedindexchanged="chklstdegree_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"></asp:CheckBoxList>
                </asp:Panel>
                 <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdegree"
                                PopupControlID="pdegree" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel></div>
                 </td>

             <td >
             <asp:Label ID="lblbranch" runat="server" Text="Branch"  Width="90px" Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
             </td>

             <td>
             <div style="position:relative">
             <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                <asp:TextBox ID="txtbranch" runat="server" Height="20px" CssClass="Dropdown_Txt_Box" ReadOnly="true" Width="180px" style=" font-family: 'Book Antiqua'; " Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                <asp:Panel ID="pbranch" runat="server"  Width="350px" CssClass="MultipleSelectionDDL" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical" Height="150px">
                <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" oncheckedchanged="chkbranch_CheckedChanged" Text="Select All"  AutoPostBack="True" />
                <asp:CheckBoxList ID="chklstbranch" runat="server" Font-Size="Medium" AutoPostBack="True" onselectedindexchanged="chklstbranch_SelectedIndexChanged"  style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"></asp:CheckBoxList>
                </asp:Panel>
               <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtbranch"
                                PopupControlID="pbranch" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel></div>
             </td>
 </tr>
 <tr>
          <td>
          <asp:Label ID="lblseme" runat="server" Text="Semester"  Width="100px" Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
          </td>
 <td>
<div style="position:relative">
              <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                <asp:TextBox ID="txtseme" runat="server" Height="20px" CssClass="Dropdown_Txt_Box" ReadOnly="true" Width="150px" style=" font-family: 'Book Antiqua'; " Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                <asp:Panel ID="pseme" runat="server" Width="200px" CssClass="MultipleSelectionDDL" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical" Height="150px">
                <asp:CheckBox ID="chkseme" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" oncheckedchanged="chkseme_CheckedChanged" Text="Select All"  AutoPostBack="True" />
                <asp:CheckBoxList ID="chklstseme" runat="server" Font-Size="Medium" AutoPostBack="True" onselectedindexchanged="chklstseme_SelectedIndexChanged"  style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"></asp:CheckBoxList>
                </asp:Panel>
                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtseme"
                                PopupControlID="pseme" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel></div>
             </td>

             <td>
             <asp:Label ID="lblsection" runat="server" Text="Section"  Width="100px" Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
             </td>

             <td>
             <div style="position:relative">
              <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                <asp:TextBox ID="txtsection" runat="server" Height="20px" CssClass="Dropdown_Txt_Box" ReadOnly="true" Width="100px" style=" font-family: 'Book Antiqua'; " Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                <asp:panel id="psection" runat="server" width="110px" CssClass="MultipleSelectionDDL" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical" Height="150px">
                <asp:CheckBox ID="chksection" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" oncheckedchanged="chksection_CheckedChanged" Text="Select All"  AutoPostBack="True" />
                <asp:CheckBoxList ID="chklstsection" runat="server" Font-Size="Medium" AutoPostBack="True" onselectedindexchanged="chklstsection_SelectedIndexChanged"  style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"></asp:CheckBoxList>
                </asp:Panel>
                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtsection"
                                PopupControlID="psection" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel></div>
             </td>
<td>
                <asp:Button ID="btngo" runat="server" Text="Go" onclick="btngo_Click" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
             </td>

        </tr>
     </table>
</center>
   <table >
    <tr><td > <asp:Label ID="lblnorec" runat="server" Text="There are no Records Found" ForeColor="Red" Visible="False" Font-Bold="True" style="margin-left: 0px; top: 200px; left: -4px;" Font-Names="Book Antiqua" Font-Size="Small"></asp:Label></td></tr>
    <tr><td>  <asp:Label ID="LabelE" runat="server" Visible="False" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Small" style="margin-left: 0px; top: 210px; left: -4px;"></asp:Label><asp:Label ID="lblother" runat="server" Visible="False" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>  </td></tr>
    </table>
     <br />
    
     <asp:Label ID="errmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Red" Width="676px"></asp:Label>

     <FarPoint:FpSpread ID="FpSpread1" runat="server" 
          Height="250px" Width="400px" 
          ActiveSheetViewIndex="0" currentPageIndex="0" 
          DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;" 
          EnableClientScript="False" CssClass="cursorptr"   BorderColor="Black"       
          BorderWidth="0.5" >
          <commandbar backcolor="Control" ButtonType="PushButton" >
              <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
          </commandbar>
          <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" 
              Font-Strikeout="False" Font-Underline="False" />
          <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" 
              Font-Strikeout="False" Font-Underline="False" />
          <sheets>
              <FarPoint:SheetView SheetName="Sheet1" 
                  
                  EditTemplateColumnCount="2" GridLineColor="Black"
                  GroupBarText="Drag a column to group by that column." 
                  SelectionBackColor="#CE5D5A" 
                  SelectionForeColor="White">
              </FarPoint:SheetView>
          </sheets>
          <TitleInfo BackColor="#E7EFF7" Font-Size="X-Large" ForeColor="" 
              HorizontalAlign="Center" VerticalAlign="NotSet" Font-Bold="False" 
              Font-Italic="False" Font-Overline="False" Font-Strikeout="False" 
              Font-Underline="False">
          </TitleInfo>
      </FarPoint:FpSpread>

      <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" 
          Font-Names="Book Antiqua" Font-Size="Medium" onclick="btnxl_Click"/>
           <asp:Button ID="Button1" runat="server" Visible="false" Text="Print" Font-Bold="True" 
          Font-Names="Book Antiqua" Font-Size="Medium" onclick="Button1_Click"/>
         <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
  
    
</body>
</asp:Content>

