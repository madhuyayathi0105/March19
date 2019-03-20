<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMod/TransportSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Student Transport Report.aspx.cs" Inherits="Default5" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <link href="Styles/css/Style.css" rel="Stylesheet" type="text/css" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
        <asp:Panel ID="Panel1" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="top: 70px;
            left: 10px; position: absolute; width: 1088px; height: 21px">
           &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="White" Text="Student's Transport/Hostel Details"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <%--  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="LinkButton2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Small" ForeColor="White" PostBackUrl="~/Default_login.aspx" CausesValidation="False">Back</asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="LinkButton3" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Small" ForeColor="White" PostBackUrl="~/Default_login.aspx" CausesValidation="False">Home</asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="lb2" runat="server" OnClick="lb2_Click" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Small" ForeColor="White" CausesValidation="False">Logout</asp:LinkButton>--%>
        </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <br />
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblbach" runat="server" Text="Batch" Font-Bold="True" ForeColor="Black"
                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtbatch" runat="server" Height="20px" ReadOnly="true" Width="120px"
                        Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium">---Select---</asp:TextBox>
                    <asp:Panel ID="pbatch" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="2px" Height="250px" ScrollBars="Vertical" Width="250px">
                        <asp:CheckBox ID="chkbatch" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chkbatch_ChekedChange"
                            Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                        <asp:CheckBoxList ID="chklsbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                            Width="100px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:DropDownExtender ID="ddlbatch" runat="server" DropDownControlID="pbatch" DynamicServicePath=""
                        Enabled="true" TargetControlID="txtbatch">
                    </asp:DropDownExtender>
                </td>
                <td>
                    <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtdegree" runat="server" Height="20px" ReadOnly="true" Width="120px"
                        Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium">---Select---</asp:TextBox>
                    <asp:Panel ID="pdegree" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="2px" Height="250px" ScrollBars="Vertical" Width="123px">
                        <asp:CheckBox ID="chkdegree" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkdegree_CheckedChanged" />
                        <asp:CheckBoxList ID="chklstdegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                            Width="100px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:DropDownExtender ID="ddldegree" runat="server" DropDownControlID="pdegree" DynamicServicePath=""
                        Enabled="true" TargetControlID="txtdegree">
                    </asp:DropDownExtender>
                </td>
                <td>
                    <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" ForeColor="Black"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtbranch" runat="server" Height="20px" ReadOnly="true" Width="120px"
                        Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium">---Select---</asp:TextBox>
                    <asp:Panel ID="pbranch" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="2px" Height="250px" ScrollBars="Auto" Width="250px" Style="font-family: 'Book Antiqua'">
                        <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkbranch_CheckedChanged" />
                        <asp:CheckBoxList ID="chklstbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                            Width="350px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                            OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:DropDownExtender ID="ddlbranch" runat="server" DropDownControlID="pbranch" DynamicServicePath=""
                        Enabled="true" TargetControlID="txtbranch">
                    </asp:DropDownExtender>
                </td>
                <td>
                    <asp:Label ID="lblsec" runat="server" Text="Section" Font-Bold="True" ForeColor="Black"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtsec" runat="server" Height="20px" ReadOnly="true" Width="100px"
                        Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium">---Select---</asp:TextBox>
                    <asp:Panel ID="psec" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="2px" Height="250px" ScrollBars="Auto" Width="120px" Style="font-family: 'Book Antiqua'">
                        <asp:CheckBox ID="chksec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chksec_CheckedChanged" />
                        <asp:CheckBoxList ID="chklssec" runat="server" Font-Size="Medium" AutoPostBack="True"
                            Width="350px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                            OnSelectedIndexChanged="chklstsec_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:DropDownExtender ID="ddlsem" runat="server" DropDownControlID="psec" DynamicServicePath=""
                        Enabled="true" TargetControlID="txtsec">
                    </asp:DropDownExtender>
                </td>

                <td>
                                    

                                     <asp:Label ID="lblstutype" runat="server" Text="Student Type" Font-Bold="True" ForeColor="Black"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlstutype" runat="server" Font-Bold="true" CssClass="font" Width="100px">
                                    <asp:ListItem  Text="All"></asp:ListItem>
                                    <asp:ListItem  Text="DAY SCHOLAR"></asp:ListItem>
                                    <asp:ListItem  Text="Hostler"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                <td>
                <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                    <ContentTemplate>
                    <asp:Button ID="btngo" runat="server" Text="Go" Width="60px" OnClick="btngo_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                </td>
            </tr>
        </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
        <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="16px"
            Style="margin-left: 0px; top: 150px; left: 10px; position: absolute; width: 1088px;">
        </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
        <table>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelE" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Small" Style="margin-left: 0px; top: 210px;
                        left: -4px;"></asp:Label>
                    <asp:Label ID="lblother" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
                </td>
            </tr>
        </table>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
        <br />
        <br />
        <br />
        <FarPoint:FpSpread ID="Fpstudenttransport" runat="server" Height="250px" Width="400px"
            ActiveSheetViewIndex="0" currentPageIndex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
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

            
        <asp:Label ID="lblnorec" runat="server" Text="No Records Found" ForeColor="Red" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Label ID="errmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" ForeColor="Red" Width="676px"></asp:Label>

        
            <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" OnClick="btnxl_Click" />
            

        
            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
            
        <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
            
           

         <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="btngoUpdatePanel">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                            Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
    </center>



    </body>
</asp:Content>
