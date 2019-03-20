<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMod/TransportSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="TransportReport.aspx.cs" Inherits="TransportReport" %>

 <%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="MainContent">
    <body>
        <br />
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <link href="Styles/css/Style.css" rel="Stylesheet" type="text/css" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
        <asp:Panel ID="Panel1" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="top: 80px;
            left: 10px; position: absolute; width: 1088px; height: 21px">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="White" Text="Transport - Bus Route Details"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <%-- <asp:LinkButton ID="LinkButton2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
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
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblroute" runat="server" Text="Route" Font-Bold="True" ForeColor="Black"
                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtroute" runat="server" Height="20px" ReadOnly="true" Width="120px"
                        Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium">---Select---</asp:TextBox>
                    <asp:Panel ID="proute" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="2px" Height="250px" ScrollBars="Vertical" Width="250px">
                        <asp:CheckBox ID="chkroute" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chkroute_ChekedChange"
                            Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                        <asp:CheckBoxList ID="chklstroute" runat="server" Font-Size="Medium" AutoPostBack="True"
                            Width="100px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstroute_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:DropDownExtender ID="ddlroute" runat="server" DropDownControlID="proute" DynamicServicePath=""
                        Enabled="true" TargetControlID="txtroute">
                    </asp:DropDownExtender>
                </td>
                <td>
                    <asp:Label ID="lblvechile" runat="server" Text="Vechile" Font-Bold="True" ForeColor="Black"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtvechile" runat="server" Height="20px" ReadOnly="true" Width="120px"
                        Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium">---Select---</asp:TextBox>
                    <asp:Panel ID="pvechile" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="2px" Height="250px" ScrollBars="Vertical" Width="123px">
                        <asp:CheckBox ID="chkvechile" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkvechile_CheckedChanged" />
                        <asp:CheckBoxList ID="chklstvechile" runat="server" Font-Size="Medium" AutoPostBack="True"
                            Width="100px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstvechile_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:DropDownExtender ID="ddlvechile" runat="server" DropDownControlID="pvechile"
                        DynamicServicePath="" Enabled="true" TargetControlID="txtvechile">
                    </asp:DropDownExtender>
                </td>
                <td>
                    <asp:Label ID="lblplace" runat="server" Text="Place" Font-Bold="True" ForeColor="Black"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtplace" runat="server" Height="20px" ReadOnly="true" Width="120px"
                        Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium">---Select---</asp:TextBox>
                    <asp:Panel ID="pplace" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="2px" Height="250px" ScrollBars="Auto" Width="250px" Style="font-family: 'Book Antiqua'">
                        <asp:CheckBox ID="chkplace" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkplace_CheckedChanged" />
                        <asp:CheckBoxList ID="chklstplace" runat="server" Font-Size="Medium" AutoPostBack="True"
                            Width="350px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                            OnSelectedIndexChanged="chklstplace_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:DropDownExtender ID="ddlplace" runat="server" DropDownControlID="pplace" DynamicServicePath=""
                        Enabled="true" TargetControlID="txtplace">
                    </asp:DropDownExtender>
                </td>
                <td>
                <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                    <ContentTemplate>
                    <asp:Button ID="btngo" runat="server" Text="Go" Width="100px" OnClick="btngo_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                </td>
            </tr>
        </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
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
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
        <br />
        <asp:Label ID="errmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" ForeColor="Red" Width="676px"></asp:Label>

        <farpoint:fpspread id="Fptransport" runat="server" height="250px" width="400px" activesheetviewindex="0"
            currentpageindex="0" designstring="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
            enableclientscript="False" cssclass="cursorptr" bordercolor="Black" borderwidth="0.5">
            <CommandBar BackColor="Control" ButtonType="PushButton" Visible="false" >
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
        </farpoint:fpspread>
            </ContentTemplate>
        </asp:UpdatePanel>
       
        <asp:Label ID="lblrptname" runat="server" Visible="True" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Report Name" ></asp:Label>
        <asp:TextBox ID="txtexcelname" runat="server" Visible="True" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
        </asp:FilteredTextBoxExtender>
        <asp:Button ID="btnxl" runat="server" Visible="True" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" OnClick="btnxl_Click" />
            <asp:Label ID="lblsmserror" Text="" Font-Size="Large" Font-Names="Book Antiqua" Visible="false"
                                ForeColor="Red" runat="server" Font-Bold="true"></asp:Label>
            <asp:Button ID="btnprintmaster" runat="server" Visible="True" CssClass="textbox textbox1 btn2" Text="Print"
                                OnClick="btnprintmaster_Click" />
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
