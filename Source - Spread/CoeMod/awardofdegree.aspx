<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="awardofdegree.aspx.cs" Inherits="awardofdegree" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <style type="text/css">
        .style1
        {
            width: 95px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 <body>
   <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
      <br/><center>
            <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Large" ForeColor="Green" Text="Details of Candidates eligible for the award of Degree"></asp:Label></center>
           
        <br /><center>
        <table style="width:700px; height:70px; background-color:#0CA6CA;">
            <tr>
                <td>
                    <asp:Label ID="lblbatch" runat="server" Text="Batch" Width="100px" Font-Bold="True"
                        ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                <div style="position:relative">
                  <asp:TextBox ID="txtbatch" runat="server" CssClass="Dropdown_Txt_Box" Height="20px"
                        ReadOnly="true" Width="100px" Style="font-family: 'Book Antiqua';  height: 15px;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                    <asp:Panel ID="pbatch" runat="server" CssClass="MultipleSelectionDDL" Height="250px"
                        Width="110px" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical">
                        <asp:CheckBox ID="chkbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnCheckedChanged="chkbatch_CheckedChanged" Text="Select All"
                            AutoPostBack="True" />
                        <asp:CheckBoxList ID="chklstbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                            OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged" Height="200px" Font-Bold="True"
                            Font-Names="Book Antiqua">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txtbatch"
                        PopupControlID="pbatch" Position="Bottom">
                    </asp:PopupControlExtender>
                    </div>
                </td>
                <td>
                    <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                        Font-Names="Book Antiqua" Font-Size="Medium" Width="100px"></asp:Label>
                </td>
                <td>
                <div style="position:relative">
                    <asp:TextBox ID="txtdegree" runat="server" CssClass="Dropdown_Txt_Box" Style="font-family: 'Book Antiqua'; height: 15px;" Height="20px" ReadOnly="true"
                        Width="100px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                    <asp:Panel ID="pdegree" runat="server" CssClass="MultipleSelectionDDL" Height="250px"
                        Width="110px" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical">
                        <asp:CheckBox ID="chkdegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnCheckedChanged="chkdegree_CheckedChanged" Text="Select All"
                            AutoPostBack="True" />
                        <asp:CheckBoxList ID="chklstdegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                            OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdegree"
                        PopupControlID="pdegree" Position="Bottom">
                    </asp:PopupControlExtender>
                   </div>
                </td>
                <td>
                    <asp:Label ID="lblbranch" runat="server" Text="Branch" Width="90px" Font-Bold="True"
                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                <div style="position:relative">
                     <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                    <asp:TextBox ID="txtbranch" runat="server" CssClass="Dropdown_Txt_Box" Height="20px"
                        ReadOnly="true" Width="180px" Style="font-family: 'Book Antiqua'; height: 15px;" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium">---Select---</asp:TextBox>
                    <asp:Panel ID="pbranch" runat="server" CssClass="MultipleSelectionDDL" Height="400px"
                        Width="400px" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical">
                        <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnCheckedChanged="chkbranch_CheckedChanged" Text="Select All"
                            AutoPostBack="True" />
                        <asp:CheckBoxList ID="chklstbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                            OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                            Font-Bold="True" Font-Names="Book Antiqua">
                        </asp:CheckBoxList>
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
                    <asp:Label ID="lblseme" runat="server" Text="Semester" Width="100px" Font-Bold="True"
                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                <div style="position:relative">
                     <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                    <asp:TextBox ID="txtseme" runat="server" CssClass="Dropdown_Txt_Box" Height="20px"
                        ReadOnly="true" Width="100px" Style="font-family: 'Book Antiqua';  height: 15px;" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium">---Select---</asp:TextBox>
                    <asp:Panel ID="pseme" runat="server" CssClass="MultipleSelectionDDL" Height="300px"
                        Width="120px" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical">
                        <asp:CheckBox ID="chkseme" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnCheckedChanged="chkseme_CheckedChanged" Text="Select All"
                            AutoPostBack="True" />
                        <asp:CheckBoxList ID="chklstseme" runat="server" Font-Size="Medium" AutoPostBack="True"
                            OnSelectedIndexChanged="chklstseme_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                            Font-Bold="True" Font-Names="Book Antiqua">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtseme"
                        PopupControlID="pseme" Position="Bottom">
                    </asp:PopupControlExtender>
                      </ContentTemplate>
                    </asp:UpdatePanel></div>
                </td>
                <td>
                    <asp:Label ID="lblsection" runat="server" Text="Section" Width="100px" Font-Bold="True"
                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                <div style="position:relative">
                      <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                    <asp:TextBox ID="txtsection" runat="server" CssClass="Dropdown_Txt_Box" Height="20px"
                        ReadOnly="true" Width="100px" Style="font-family: 'Book Antiqua';  height: 15px;" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium">---Select---</asp:TextBox>
                    <asp:Panel ID="psection" runat="server" Height="125px" CssClass="MultipleSelectionDDL"
                        Width="120px" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical">
                        <asp:CheckBox ID="chksection" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnCheckedChanged="chksection_CheckedChanged" Text="Select All"
                            AutoPostBack="True" />
                        <asp:CheckBoxList ID="chklstsection" runat="server" Font-Size="Medium" AutoPostBack="True"
                            OnSelectedIndexChanged="chklstsection_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                            Font-Bold="True" Font-Names="Book Antiqua">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtsection"
                        PopupControlID="psection" Position="Bottom">
                    </asp:PopupControlExtender>
                     </ContentTemplate>
                    </asp:UpdatePanel></div>
                </td>
                <td>
                    <asp:Label ID="lblattempt" runat="server" Text="Attempt" Width="100px" Font-Bold="True"
                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlattempt" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                        Font-Size="Medium">
                        <asp:ListItem>Both</asp:ListItem>
                        <asp:ListItem>Single</asp:ListItem>
                        <asp:ListItem>Multiple</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_Click" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" />
                </td>
                <td>
                    <asp:Button ID="btnrefsettings" runat="server" Text="Reference Number Settings" OnClick="btnrefsettings_Click"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                </td>
                </tr>
                <tr>
               <td>
                        <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" Font-Bold="True"
                            ForeColor="Black" Font-Names="Book Antiqua" OnCheckedChanged="CheckBox1_click"
                            Font-Size="Medium" />
                        <asp:Label ID="Label1" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Format1"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="True" Font-Bold="True"
                            ForeColor="Black" Font-Names="Book Antiqua" OnCheckedChanged="CheckBox2_click"
                            Font-Size="Medium" />
                        <asp:Label ID="Label2" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Format2"></asp:Label>
                    </td>
            </tr>
        </table>
      </center> 
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblnorec" runat="server" Text="There are no Records Found" ForeColor="Red"
                        Visible="False" Font-Bold="True" Style="margin-left: 0px; top: 200px; left: -4px;"
                        Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelE" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Small" Style="margin-left: 0px; top: 210px;
                        left: -4px;"></asp:Label><asp:Label ID="lblother" runat="server" Visible="False"
                            ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <br />
        <asp:Label ID="errmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" ForeColor="Red" Width="676px"></asp:Label>
        <FarPoint:FpSpread ID="FpSpread1" runat="server" Height="250px" Width="400px" ActiveSheetViewIndex="0"
            currentPageIndex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
            EnableClientScript="False" CssClass="cursorptr" BorderColor="Black" BorderWidth="0.5">
            <CommandBar BackColor="Control" ButtonType="PushButton">
                <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
            </CommandBar>
            <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                Font-Underline="False" />
            <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                Font-Underline="False" />
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" GridLineColor="Black"
                    GroupBarText="Drag a column to group by that column." SelectionBackColor="#CE5D5A"
                    SelectionForeColor="White">
                </FarPoint:SheetView>
            </Sheets>
            <TitleInfo BackColor="#E7EFF7" Font-Size="X-Large" ForeColor="" HorizontalAlign="Center"
                VerticalAlign="NotSet" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                Font-Strikeout="False" Font-Underline="False">
            </TitleInfo>
        </FarPoint:FpSpread>
        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" OnClick="btnxl_Click" />
        <asp:Label ID="lblmonth" runat="server" Text="Month" Font-Bold="True" ForeColor="Black"
            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
        <asp:DropDownList ID="ddlmonth" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
            Font-Size="Medium">
        </asp:DropDownList>
        <asp:Label ID="lblyear" runat="server" Text="Year" Font-Bold="True" ForeColor="Black"
            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
        <asp:DropDownList ID="ddlyear" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
            Font-Size="Medium">
        </asp:DropDownList>
        <asp:Button ID="btnprint" runat="server" Text="Print" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" OnClick="btnprint_Click" />
        <asp:Panel ID="PRefnosettings" runat="server" BorderColor="Black" BackColor="White"
            Visible="false" BorderWidth="2px" Style="left: 10px; top: 300px; position: absolute;">
            <br />
            <div class="PopupHeaderrstud2" id="Div3" style="text-align: center; font-family: MS Sans Serif;
                font-size: Small; font-weight: bold">
                <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                    left: 200px">
                    Reference Number Settings
                    <br />
                    <br />
                </caption>
                <table style="text-align: left">
                    <tr>
                        <td>
                            <asp:Label ID="lblregular" runat="server" Text="Regular Student Reference Number"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtregular" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" MaxLength="50" Width="300px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtregular"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=" ()./\,-_[]{}:;@#$%^&*+€<>?">
                            </asp:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbllateral" runat="server" Text="Lateral Entry Student Reference Number"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlateral" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" MaxLength="50" Width="300px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtlateral"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="  ()./\,-_[]{}:;@#$%^&*+€<>?">
                            </asp:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbltransfer" runat="server" Text="Transfer Student Reference Number"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txttransfer" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" MaxLength="50" Width="300px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txttransfer"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="  ()./\,-_[]{}:;@#$%^&*+€<>?">
                            </asp:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="btnrefsave" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Set" OnClick="btnrefsave_Click" />
                <asp:Button ID="btnexit" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Exit" OnClick="btnexit_Click" />
                <br />
                <asp:Label ID="perrmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="Red" Width="676px"></asp:Label>
            </div>
        </asp:Panel>
    </body>
    <%-- <div id="divtable" runat="server" visible="false">
                <center>
                     <table>
            <tr>
                <td>
                
           

             <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid"
                                ShowHeaderSelection="false" BorderWidth="1px" Visible="false" VerticalScrollBarPolicy="Never"
                                HorizontalScrollBarPolicy="Never" CssClass="stylefp" Width="1550px" >
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
            </div>--%>


            <center>
             <div id="divtable" runat="server" visible="false">
                    <table>
                        <tr>
                            <td>
                                <center>
                                    <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid"
                                        ShowHeaderSelection="false" BorderWidth="1px" Visible="false" VerticalScrollBarPolicy="AsNeeded"
                                        HorizontalScrollBarPolicy="AsNeeded" CssClass="stylefp" Width="1000px" >
                                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                            ButtonShadowColor="ControlDark">
                                        </CommandBar>
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                    <center>
                                        <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False">
                                        </asp:Label></center>
                                    <div id="div_report" runat="server" visible="false">
                                        <center>
                                            <asp:Label ID="lbl_reportname1" runat="server" Text="Report Name" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                            <asp:TextBox ID="txt_excelname1" runat="server" AutoPostBack="true" OnTextChanged="txtexcelname_TextChanged1"
                                                CssClass="textbox textbox1 txtheight5" onkeypress="return ClearPrint1()"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname1"
                                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:Button ID="btn_Excel1" runat="server" Text="Export To Excel"
                                                OnClick="btnExcel_Click1" />
                                            <asp:Button ID="btn_printmaster1" runat="server" Text="Print"
                                                OnClick="btn_printmaster_Click1" />
                                            <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
                                        </center>
                                    </div>
                                    </center>
                            </td>
                        </tr>
                    </table>
                </div>
                </center>
                <center>
        <div id="divPopupAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%; right: 0%;">
            <center>
                <div id="divAlertContent" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblAlertMsg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <center>
                                        <asp:Button ID="btnPopAlertClose" runat="server" Text="Ok" 
                                            OnClick="btnPopAlertClose_Click" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>



</asp:Content>

