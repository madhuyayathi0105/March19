<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="pareport.aspx.cs" Inherits="pareport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script type="text/javascript">

     function display() {
         document.getElementById('MainContent_lblerror').innerHTML = "";
     }

     function validation() {
         var bat = document.getElementById('<%=txtbatch.ClientID %>').value;
         var deg = document.getElementById('<%=txtdegree.ClientID %>').value;
         var bran = document.getElementById('<%=txtdept.ClientID %>').value;

         if (bat == "--Select--") {
             alert("Please Select Batch");
             return false;
         }
         else if (deg == "--Select--") {
             alert("Please Select Degree");
             return false;
         }
         else if (bran == "--Select--") {
             alert("Please Select Branch");
             return false;
         }
         else {
             return true;
         }
     }

     function displayraj() {
         document.getElementById('MainContent_lblerrmsgxl').innerHTML = "";
     }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ScriptManager ID="Script1" runat="server">
    </asp:ScriptManager>
  <br />
    <center>
        <asp:Label ID="lbl" runat="server" Text="CAM and COE Performance Analysis Report" Font-Bold="true" Font-Names="Bood Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
       </center>
       <br />
       <center>
            <asp:UpdatePanel ID="upd" runat="server">
                <ContentTemplate>
                    <table style="width:700px; height:70px; background-color:#0CA6CA;">
                        <tr>
                            <td>
                                <asp:Label ID="lblbatch" runat="server" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Batch" ForeColor="Black"></asp:Label>
                                    </td>
                                    <td>
                                    <div style="position:relative">
                                    <asp:UpdatePanel ID="up1" runat="server">
                                    <ContentTemplate>
                                <asp:TextBox ID="txtbatch" runat="server" AutoPostBack="true" CssClass="Dropdown_Txt_Box"
                                    ReadOnly="true" Width="106px" Height="20px" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">--Select--</asp:TextBox>
                                <asp:Panel ID="Panel1" runat="server" CssClass="MultipleSelectionDDL" Width="108px"
                                    Height="150px" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical">
                                    <asp:CheckBox ID="chckbatch" runat="server" OnCheckedChanged="checkBatch_CheckedChanged"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All"
                                        AutoPostBack="true" />
                                    <asp:CheckBoxList ID="chcklistbatch" runat="server" OnSelectedIndexChanged="cheklistBatch_SelectedIndexChanged"
                                        Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua" AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtbatch"
                                    PopupControlID="Panel1" Position="Bottom">
                                </asp:PopupControlExtender></ContentTemplate></asp:UpdatePanel></div>
                            </td>
                            <td>
                                <asp:Label ID="lbldegree" runat="server" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Degree" ForeColor="Black"></asp:Label>
                                    </td>
                                    <td>
                                    <div style="position:relative">
                                    
                                <asp:TextBox ID="txtdegree" runat="server" AutoPostBack="true" CssClass="Dropdown_Txt_Box"
                                    ReadOnly="true" Width="109px" Height="20px" Font-Bold="True" Font-Names="Book Antiqua"
                                     Font-Size="Medium">--Select--</asp:TextBox>
                                <asp:Panel ID="Panel3" runat="server" CssClass="MultipleSelectionDDL" Width="184px"
                                    Height="150px" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical">
                                    <asp:CheckBox ID="chckdegree" runat="server" OnCheckedChanged="checkDegree_CheckedChanged"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All"
                                        AutoPostBack="true" />
                                    <asp:CheckBoxList ID="chcklistdegree" runat="server" OnSelectedIndexChanged="cheklist_Degree_SelectedIndexChanged"
                                        Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua" AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtdegree"
                                    PopupControlID="Panel3" Position="Bottom">
                                </asp:PopupControlExtender>
                               </div>
                            </td>
                            <td>
                                <asp:Label ID="lbldept" runat="server" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Branch" ForeColor="Black"></asp:Label>
                                    </td>
                                    <td>
                                    <div style="position:relative">
                                    
                                <asp:TextBox ID="txtdept" runat="server" AutoPostBack="true" CssClass="Dropdown_Txt_Box"
                                    ReadOnly="true" Width="106px" Height="20px" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">--Select--</asp:TextBox>
                                <asp:Panel ID="paneldept" runat="server" CssClass="MultipleSelectionDDL" Height="150px"
                                    Width="268px" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical">
                                    <asp:CheckBox ID="chckdept" runat="server" OnCheckedChanged="checkdept_CheckedChanged"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All"
                                        AutoPostBack="true" />
                                    <asp:CheckBoxList ID="cbldept" runat="server" OnSelectedIndexChanged="cbldept_SelectedIndexChanged"
                                        Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua" AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtdept"
                                    PopupControlID="paneldept" Position="Bottom">
                                </asp:PopupControlExtender>
                               </div>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="Rbtn" runat="server" CellSpacing="0" AutoPostBack="true"
                                    RepeatDirection="Horizontal" Font-Bold="true" Font-Size="Large" ForeColor="Black"
                                    Style=" width: 190px;"
                                    font-name="Book Antiqua" OnSelectedIndexChanged="Rbtn_OnSelectedIndexChanged">
                                    <asp:ListItem>Internal</asp:ListItem>
                                    <asp:ListItem>External</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:Label ID="lbltest" runat="server" Text="Test" font-name="Book Antiqua" Font-Size="Medium" Font-Bold="true" ForeColor="Black"></asp:Label>
                                    </td>
                                    <td>
                                <asp:DropDownList ID="ddltest" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                                    Style=" width: 173px;"
                                    Font-Bold="true" Font-Size="Medium" Height="25px" OnSelectedIndexChanged="ddltest_OnSelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblmonth" runat="server" Text="Month & Year" font-name="Book Antiqua"
                                    Font-Size="Medium" Width="125px" Font-Bold="true" ForeColor="Black"></asp:Label>
                                    </td>
                                    <td>
                                <asp:DropDownList ID="ddlmonth" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                                    Style=" width: 100px;"
                                    Font-Bold="true" Font-Size="Medium" Height="25px" OnSelectedIndexChanged="ddlmonth_OnSelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                             <td>
                       
                        <asp:Button ID="btngo" runat="server" Text="Go" Font-Bold="true" Font-Size="Medium"
                            Style="width: 45px;"
                            OnClientClick="return validation()" ForeColor="Black"  Font-Names="Book Antiqua" OnClick="btngo_OnClick" />
                    </td>
                        </tr>
                    </table>
                    </center>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="ddltest" />
                </Triggers>
                <Triggers>
                    <asp:PostBackTrigger ControlID="ddlmonth" />
                </Triggers>
            </asp:UpdatePanel> </center>
            

    <asp:Label ID="lblerrormsg" runat="server" Text="" Width="302px" Style="position: absolute;
        left: 15px; top: 277px;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
        Visible="true" ForeColor="#FF3300"></asp:Label>
    <br />
    <br />
    <center>
        <table>
            <tr>
                <td>
                    <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
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
                </td>
            </tr>
        </table>
    </center>
    <table>
        <tr>
            <td>
                <asp:Label ID="lblerror" runat="server" Text="" Width="250px" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Visible="true" ForeColor="#FF3300"></asp:Label>
                <br />
                <asp:Label ID="lblexportxl" runat="server" Visible="false" Width="100px" Height="20px"
                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Text="Report Name"
                    ForeColor="Black"></asp:Label>
                <asp:TextBox ID="txtexcell" runat="server" Visible="false" Height="20px" Width="180px"
                    Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" onkeypress="display()"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcell"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                    InvalidChars="/\">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btnexcel" runat="server" OnClick="btnexcel_OnClick" Visible="false"
                    Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                    Style="margin-left: 6px;" />
                <asp:Button ID="btnprint" runat="server" OnClick="btnprint_OnClick" Visible="false"
                    Text="Print" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
            </td>
        </tr>
    </table>
</asp:Content>

