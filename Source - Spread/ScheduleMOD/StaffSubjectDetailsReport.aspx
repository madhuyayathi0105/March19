<%@ Page Title="" Language="C#" MasterPageFile="~/ScheduleMOD/ScheduleSubSiteMaster.master" AutoEventWireup="true" CodeFile="StaffSubjectDetailsReport.aspx.cs" Inherits="staffsubjecthoursreport" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="Ajax" %>
 <%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
      
            <br />
           <center> <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="Purple" Text="Staff Subject Details Report"></asp:Label></center>
     
         <asp:UpdatePanel ID="udpsubject" runat="server">
         <ContentTemplate>
          <asp:UpdateProgress ID="UpdateProgress1" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="udpsubject">
                        <ProgressTemplate>
                            <div class="CenterPB" style="height: 40px; width: 40px;">
                                <img src="images/progress2.gif" height="180px" width="180px" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress1"
                        PopupControlID="UpdateProgress1">
                    </asp:ModalPopupExtender>
                    <br />
                   <table style="background-color:#0CA6CA;width: 900px;height:65px;">
        <tr>
        <td>
         <asp:Label ID="lblcollege" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label></td>
       <td><asp:DropDownList ID="ddlcollege" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged" Font-Bold="true" Font-Names="Book Antiqua" Width="150px"
                        Font-Size="Medium"></asp:DropDownList></td>
      <td> <asp:Label ID="lblyear" runat="server" Text="Year" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" ></asp:Label></td>
                       <td> 
                       <div style="position:relative;">
                       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtyear" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                Width="100px" Style=" font-family: 'Book Antiqua';"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="pyear" runat="server" CssClass="MultipleSelectionDDL" BackColor="White" ScrollBars="Auto" 
                                    Style="font-family: 'Book Antiqua'">
                                <asp:CheckBox ID="chkyear" runat="server" Font-Bold="True" 
                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="chkyear_CheckedChanged" />
                                <asp:CheckBoxList ID="chklsyear" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsyear_SelectedIndexChanged">
                                    <asp:ListItem Text="I Year" Value="1,2"></asp:ListItem>
                                    <asp:ListItem Text="II Year" Value="3,4"></asp:ListItem>
                                    <asp:ListItem Text="III Year" Value="5,6"></asp:ListItem>
                                    <asp:ListItem Text="IV Year" Value="7,8"></asp:ListItem>
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtyear"
                                PopupControlID="pyear" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    </div></td>
                 <td>   <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" ></asp:Label></td>
                       <td> 
                        <div style="position:relative;">
                       <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtdegree" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                Width="100px" Style=" font-family: 'Book Antiqua';"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="pdegree" runat="server" CssClass="MultipleSelectionDDL" BackColor="White" ScrollBars="Auto" 
                                    Style="font-family: 'Book Antiqua'">
                                <asp:CheckBox ID="chkdegree" runat="server" Font-Bold="True" 
                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="chkdegree_CheckedChanged"/>
                                <asp:CheckBoxList ID="chklsdegree" runat="server" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="chklsdegree_CheckedChanged"
                                    Font-Bold="True" Font-Names="Book Antiqua" >
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtdegree"
                                PopupControlID="pdegree" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    </div></td>
                    <td><asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" ></asp:Label></td>
                        <td>
                         <div style="position:relative;">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtbranch" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                Width="100px" Style=" font-family: 'Book Antiqua';"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="pbranch" runat="server" CssClass="MultipleSelectionDDL" BackColor="White" ScrollBars="Auto" 
                                    Style="font-family: 'Book Antiqua'">
                                <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" 
                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkbranch_CheckedChanged"/>
                                <asp:CheckBoxList ID="chklsbranch" runat="server" Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="chklsbranch_CheckedChanged"
                                    Font-Bold="True" Font-Names="Book Antiqua" >
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtbranch"
                                PopupControlID="pbranch" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    </div></td>
                    <td><asp:Label ID="lblsec" runat="server" Text="Sections" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label></td>
                       <td> 
                        <div style="position:relative;">
                       <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtsec" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                Width="100px" Style=" font-family: 'Book Antiqua';"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="psec" runat="server" CssClass="MultipleSelectionDDL" BackColor="White" ScrollBars="Auto" 
                                    Style="font-family: 'Book Antiqua'">
                                <asp:CheckBox ID="chksec" runat="server" Font-Bold="True" 
                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chksec_CheckedChanged"/>
                                <asp:CheckBoxList ID="chklssec" runat="server" Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="chklssec_CheckedChanged"
                                    Font-Bold="True" Font-Names="Book Antiqua" >
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtsec"
                                PopupControlID="psec" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel></div></td>
                   <td> <asp:Button ID="btngo" Text="Go" runat="server" Font-Bold="true" Font-Names="Book Antiqua" /></td>
                   </tr></table>
      
        <br />
        <asp:Label ID="errmsg" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Red" Font-Names="Book Antiqua" Visible="false"></asp:Label>
      <br />
        <FarPoint:FpSpread ID="fpsubjectdetails" runat="server" 
          Height="250px" Width="900px" 
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
      <br />
      <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Text="Report Name"></asp:Label>
  <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" ></asp:TextBox>
    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname" FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                         </asp:FilteredTextBoxExtender> 
                           <asp:Button ID="btnxl" runat="server" Text="Export Excel"   Font-Bold="True" 
          Font-Names="Book Antiqua" Font-Size="Medium" onclick="btnxl_Click"/>  
          <asp:Button ID="btnprintmaster" runat="server" Text="Print" 
            onclick="btnprintmaster_Click"   Font-Names="Book Antiqua" Font-Size="Medium"  Font-Bold="true" />
            <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />

      </ContentTemplate>
      </asp:UpdatePanel>
    </body>
</asp:Content>

