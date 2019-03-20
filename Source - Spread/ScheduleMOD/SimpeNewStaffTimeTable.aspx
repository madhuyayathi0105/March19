<%@ Page Title="" Language="C#" MasterPageFile="~/ScheduleMOD/ScheduleSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="SimpeNewStaffTimeTable.aspx.cs" Inherits="ScheduleMOD_SimpeNewStaffTimeTable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript" src="../Scripts/jquery-1.4.1.js"></script>
    <style type="text/css">
        .GridDock
        {
            overflow-x: auto;
            overflow-y: auto;
            width: 400px;
            height: 200px;
            padding: 0 0 0 0;
        }
    </style>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <script type="text/javascript">
        function printTTOutput() {
            var panel = document.getElementById("<%=printdiv.ClientID %>");
            var printWindow = window.open('', '', 'height=816,width=980');
            printWindow.document.write('<html><head>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
        }
    </script>
    <style tyle="text/css">
        .printclass
        {
            display: none;
        }
        .marginSet
        {
            margin: 0px;
            padding: 0px;
        }
        .headerDisp
        {
            font-size: 25px;
            font-weight: bold;
        }
        .headerDisp1
        {
            font-family: Book Antiqua;
            font-size: medium;
        }
        @media print
        {
            #printdiv
            {
                display: block;
            }
            .printclass
            {
                display: block;
                font-family: Book Antiqua;
            }
            .noprint
            {
                display: none;
            }
        }
        @media screen,print
        {
        
        }
        @page
        {
            size: A4;
        }
    </style>
    <style type="text/css">
        .modalPopup1
        {
            background-color: #ffffdd;
            border-width: 1px;
            -moz-border-radius: 5px;
            border-style: solid;
            border-color: Gray;
            min-width: 250px;
            max-width: 700px;
            min-height: 100px;
            max-height: 250px;
            overflow: scroll;
            top: 100px;
            left: 150px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green">Semester Time Table </span>
        <table class="maintablestyle" style="margin: 0px; margin-bottom: 10px; margin-top: 10px;"
            width="1000px">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    OnSelectedIndexChanged="ddlcollege_change" Font-Size="Medium" CssClass="textbox1 ddlheight5"
                                    Width="200px" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Department" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <div style="position: relative;">
                                    <asp:DropDownList ID="ddlDept" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        OnSelectedIndexChanged="ddlDept_change" Font-Size="Medium" CssClass="textbox1 ddlheight5"
                                        Width="160px" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Staff" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSearchOption" runat="server" Font-Bold="true" Font-Size="Medium"
                                    OnSelectedIndexChanged="ddlSearchOption_SelectedIndexChanged" Font-Names="Book Antiqua"
                                    CssClass="textbox1 ddlheight5" Width="200px" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Semester" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <div style="position: relative;">
                                    <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        OnSelectedIndexChanged="ddlsem_change" Font-Size="Medium" CssClass="textbox1 ddlheight5"
                                        Width="80px" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td>
                                <div style="position: relative;">
                                    <div id="div4" style="position: relative;" runat="server">
                                        <asp:UpdatePanel ID="upnlBranch" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtBranch" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                                    ReadOnly="true">-- Select --</asp:TextBox>
                                                <asp:Panel ID="pnlBranch" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                                    Width="280px">
                                                    <asp:CheckBox ID="chkBranch" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                        AutoPostBack="True" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cblBranch" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                                        OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="popExtBranch" runat="server" TargetControlID="txtBranch"
                                                    PopupControlID="pnlBranch" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblFromdate" runat="server" Text="Date" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFromDate" CssClass="txt" runat="server" Height="18px" Width="87px"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtFromDate"
                                    FilterType="Custom,Numbers" ValidChars="/" />
                                <asp:CalendarExtender ID="CalExtFromDate" TargetControlID="txtFromDate" Format="dd/MM/yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Button ID="btnGo" Text="Go" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    OnClick="btnGo_OnClick" Width="62px" Height="29px" Font-Size="Large" />
                            </td>
                            <td>
                                <asp:Button ID="btnAdd" runat="server" Text="Save" Font-Bold="true" Width="54px"
                                    Height="29px" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnAddNew_OnClick" />
                            </td>
                            <td>
                                <asp:LinkButton ID="lnksetting" runat="server" Text="Settings" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="lnksetting_click"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </center>
    <br />
    <center>
        <button id="btnPrint" runat="server" visible="true" height="29px" width="62px" onclick="return printTTOutput();"
            style="background-color: LightGreen; font-weight: bold; font-size: medium; font-family: Book Antiqua;">
            Print
        </button>
        <asp:Button ID="btndelete" Text="Remove" runat="server" Visible="false" Font-Bold="true"
            Font-Names="Book Antiqua" Width="75px" Height="29px" Font-Size="Medium" />
    </center>
    <center>
        <asp:Panel ID="pnlAlert" runat="server" CssClass="modalPopup1" Style="display: none;
            height: 200; width: 400; left: auto; top: 30px">
            <table width="100%">
                <tr class="topHandle">
                    <td colspan="2" align="left" runat="server" id="tdCaption">
                        <asp:Label ID="lblCaption" runat="server" Font-Bold="True" Text="Confirmation" Font-Names="Book Antiqua"
                            Font-Size="Large"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 90px" valign="middle" align="center">
                        <asp:Image ID="imgInfo" runat="server" ImageUrl="~/image/n1.png" />
                    </td>
                    <td valign="middle" align="left">
                        <asp:Label ID="lblErrmsg" Text="" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red"></asp:Label>
                        <br />
                        <asp:Label ID="Label5" Text="Do You want to Allow?" runat="server" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Button ID="btnUpdate" runat="server" Text="Appand" OnClick="btnUpdate_OnClick"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                        <asp:Button ID="btnReplace" runat="server" Text="Update" OnClick="btnReplace_OnClick"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_OnClick"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:ModalPopupExtender ID="alert2PopUp" runat="server" TargetControlID="HiddenField2"
            PopupControlID="pnlAlert">
        </asp:ModalPopupExtender>
        <asp:HiddenField runat="server" ID="HiddenField1" />
    </center>
    <center>
        <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup1" Style="display: none;
            height: 200; width: 400; left: auto; top: 30px">
            <table width="100%">
                <tr class="topHandle">
                    <td colspan="2" align="left" runat="server" id="td1">
                        <asp:Label ID="Label7" runat="server" Font-Bold="True" Text="Confirmation" Font-Names="Book Antiqua"
                            Font-Size="Large"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 90px" valign="middle" align="center">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/image/n1.png" />
                    </td>
                    <td valign="middle" align="left">
                        <asp:Label ID="Label8" Text="" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red"></asp:Label>
                        <br />
                        <asp:Label ID="Label9" Text="Do You want to Allow?" runat="server" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Button ID="Button2" runat="server" Text="Appand" OnClick="Button2_OnClick" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" />
                        <asp:Button ID="Button4" runat="server" Text="Cancel" OnClick="Button4_OnClick" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenField2"
            PopupControlID="Panel1">
        </asp:ModalPopupExtender>
        <asp:HiddenField runat="server" ID="HiddenField2" />
    </center>
    <br />
    <center>
        <asp:GridView ID="gridTimeTable" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#0CA6CA"
            BackColor="White" OnRowDataBound="gridTimeTable_OnRowDataBound">
            <%-- OnDataBound="gridTimeTable_OnDataBound"--%>
            <Columns>
                <asp:TemplateField HeaderText="Day">
                    <ItemTemplate>
                        <asp:Label ID="lblDateDisp" runat="server" Text='<%#Eval("DateDisp") %>'></asp:Label>
                        <asp:Label ID="lblDayVal" runat="server" Text='<%#Eval("DateVal") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="100px" HorizontalAlign="Center" BackColor="#F8B7B3" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Period 1" Visible="false">
                    <ItemTemplate>
                        <div style="position: relative;">
                            <div id="div5" style="position: relative;" runat="server">
                                <asp:UpdatePanel ID="upnlPeriod1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtPeriod1" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                            ReadOnly="true">-- Select --</asp:TextBox>
                                        <asp:Panel ID="pnlPeriod1" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                            Width="380px">
                                            <asp:CheckBox ID="chkPeriod1" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                AutoPostBack="True" OnCheckedChanged="chkPeriod_CheckedChanged" />
                                            <asp:CheckBoxList ID="cblPeriod1" CssClass="commonHeaderFont" runat="server">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popExtPreiod1" runat="server" TargetControlID="txtPeriod1"
                                            PopupControlID="pnlPeriod1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <asp:Label ID="lblPeriod_1" runat="server" Text='<%#Eval("P1Val") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblTT_1" runat="server" Text='<%#Eval("TT_1") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Period 2" Visible="false">
                    <ItemTemplate>
                        <div id="div6" style="position: relative;" runat="server">
                            <asp:UpdatePanel ID="upnlPeriod2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtPeriod2" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlPeriod2" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="380px">
                                        <asp:CheckBox ID="chkPeriod2" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkPeriod_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblPeriod2" CssClass="commonHeaderFont" runat="server">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtPreiod2" runat="server" TargetControlID="txtPeriod2"
                                        PopupControlID="pnlPeriod2" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <asp:Label ID="lblPeriod_2" runat="server" Text='<%#Eval("P2Val") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblTT_2" runat="server" Text='<%#Eval("TT_2") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Period 3" Visible="false">
                    <ItemTemplate>
                        <div id="div7" style="position: relative;" runat="server">
                            <asp:UpdatePanel ID="upnlPeriod3" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtPeriod3" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlPeriod3" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="380px">
                                        <asp:CheckBox ID="chkPeriod3" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkPeriod_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblPeriod3" CssClass="commonHeaderFont" runat="server">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtPreiod3" runat="server" TargetControlID="txtPeriod3"
                                        PopupControlID="pnlPeriod3" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <asp:Label ID="lblPeriod_3" runat="server" Text='<%#Eval("P3Val") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblTT_3" runat="server" Text='<%#Eval("TT_3") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Period 4" Visible="false">
                    <ItemTemplate>
                        <div id="div8" style="position: relative;" runat="server">
                            <asp:UpdatePanel ID="upnlPeriod4" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtPeriod4" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlPeriod4" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="380px">
                                        <asp:CheckBox ID="chkPeriod4" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkPeriod_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblPeriod4" CssClass="commonHeaderFont" runat="server">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtPreiod4" runat="server" TargetControlID="txtPeriod4"
                                        PopupControlID="pnlPeriod4" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <asp:Label ID="lblPeriod_4" runat="server" Text='<%#Eval("P4Val") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblTT_4" runat="server" Text='<%#Eval("TT_4") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Period 5" Visible="false">
                    <ItemTemplate>
                        <div id="div9" style="position: relative;" runat="server">
                            <asp:UpdatePanel ID="upnlPeriod5" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtPeriod5" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlPeriod5" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="380px">
                                        <asp:CheckBox ID="chkPeriod5" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkPeriod_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblPeriod5" CssClass="commonHeaderFont" runat="server">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtPreiod5" runat="server" TargetControlID="txtPeriod5"
                                        PopupControlID="pnlPeriod5" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <asp:Label ID="lblPeriod_5" runat="server" Text='<%#Eval("P5Val") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblTT_5" runat="server" Text='<%#Eval("TT_5") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Period 6" Visible="false">
                    <ItemTemplate>
                        <div id="div10" style="position: relative;" runat="server">
                            <asp:UpdatePanel ID="upnlPeriod6" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtPeriod6" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlPeriod6" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="380px">
                                        <asp:CheckBox ID="chkPeriod6" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkPeriod_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblPeriod6" CssClass="commonHeaderFont" runat="server">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtPreiod6" runat="server" TargetControlID="txtPeriod6"
                                        PopupControlID="pnlPeriod6" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <asp:Label ID="lblPeriod_6" runat="server" Text='<%#Eval("P6Val") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblTT_6" runat="server" Text='<%#Eval("TT_6") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Period 7" Visible="false">
                    <ItemTemplate>
                        <div id="div11" style="position: relative;" runat="server">
                            <asp:UpdatePanel ID="upnlPeriod7" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtPeriod7" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlPeriod7" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="380px">
                                        <asp:CheckBox ID="chkPeriod7" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkPeriod_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblPeriod7" CssClass="commonHeaderFont" runat="server">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtPreiod7" runat="server" TargetControlID="txtPeriod7"
                                        PopupControlID="pnlPeriod7" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <asp:Label ID="lblPeriod_7" runat="server" Text='<%#Eval("P7Val") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblTT_7" runat="server" Text='<%#Eval("TT_7") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Period 8" Visible="false">
                    <ItemTemplate>
                        <div id="div20" style="position: relative;" runat="server">
                            <asp:UpdatePanel ID="upnlPeriod8" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtPeriod8" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlPeriod8" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="380px">
                                        <asp:CheckBox ID="chkPeriod8" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkPeriod_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblPeriod8" CssClass="commonHeaderFont" runat="server">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtPreiod8" runat="server" TargetControlID="txtPeriod8"
                                        PopupControlID="pnlPeriod8" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <asp:Label ID="lblPeriod_8" runat="server" Text='<%#Eval("P8Val") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblTT_8" runat="server" Text='<%#Eval("TT_8") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Period 9" Visible="false">
                    <ItemTemplate>
                        <div id="div12" style="position: relative;" runat="server">
                            <asp:UpdatePanel ID="upnlPeriod9" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtPeriod9" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlPeriod9" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="380px">
                                        <asp:CheckBox ID="chkPeriod9" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkPeriod_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblPeriod9" CssClass="commonHeaderFont" runat="server">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtPreiod9" runat="server" TargetControlID="txtPeriod9"
                                        PopupControlID="pnlPeriod9" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <asp:Label ID="lblPeriod_9" runat="server" Text='<%#Eval("P9Val") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblTT_9" runat="server" Text='<%#Eval("TT_9") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Period 10" Visible="false">
                    <ItemTemplate>
                        <div id="div13" style="position: relative;" runat="server">
                            <asp:UpdatePanel ID="upnlPeriod10" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtPeriod10" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlPeriod10" Visible="true" runat="server" CssClass="multxtpanel"
                                        Height="300px" Width="280px">
                                        <asp:CheckBox ID="chkPeriod10" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkPeriod_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblPeriod10" CssClass="commonHeaderFont" runat="server">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtPreiod" runat="server" TargetControlID="txtPeriod10"
                                        PopupControlID="pnlPeriod10" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <asp:Label ID="lblPeriod_10" runat="server" Text='<%#Eval("P10Val") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblTT_10" runat="server" Text='<%#Eval("TT_10") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </center>
    <br />
    <center>
        <div id="printdiv" runat="server">
            <table class="printclass" style="width: 98%; height: auto; margin: 0px; padding: 0px;">
                <tr>
                    <td rowspan="5" style="width: 100px; margin: 0px; border: 0px;">
                        <asp:Image ID="imgLeftLogo2" runat="server" AlternateText="" ImageUrl="~/college/Left_Logo.jpeg"
                            Width="100px" Height="100px" />
                    </td>
                    <th class="marginSet" align="center" colspan="6">
                        <span id="spCollegeName" class="headerDisp" runat="server"></span>
                    </th>
                </tr>
                <tr>
                    <th class="marginSet" align="center" colspan="6">
                        <span id="spAddr" class="headerDisp1" runat="server"></span>
                    </th>
                </tr>
                <tr>
                    <th class="marginSet" align="center" colspan="6">
                        <span id="spReportName" class="headerDisp1" runat="server"></span>
                    </th>
                </tr>
                <tr>
                    <td class="marginSet" colspan="3" align="center">
                        <span id="spDegreeName" class="headerDisp1" runat="server"></span>
                    </td>
                    <td class="marginSet" colspan="3" align="right">
                        <span id="spSem" class="headerDisp1" runat="server"></span>
                    </td>
                </tr>
                <tr>
                    <td class="marginSet" colspan="3" align="left">
                        <span id="spProgremme" class="headerDisp1" runat="server"></span>
                    </td>
                    <td class="marginSet" colspan="3" align="right">
                        <span id="spSection" class="headerDisp1" runat="server"></span>
                    </td>
                </tr>
            </table>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" Font-Names="Book Antiqua"
                HeaderStyle-BackColor="#0CA6CA" BackColor="White">
                <%-- OnDataBound="gridTimeTable_OnDataBound"--%>
                <Columns>
                    <asp:TemplateField HeaderText="Day">
                        <ItemTemplate>
                            <asp:Label ID="lblDateDisp" runat="server" Text='<%#Eval("DateDisp") %>'></asp:Label>
                            <asp:Label ID="lblDayVal" runat="server" Text='<%#Eval("DateVal") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="100px" HorizontalAlign="Center" BackColor="#F8B7B3" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Period 1" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkPeriod_1" runat="server" Text='<%#Eval("P1Val") %>' ForeColor="Blue"
                                OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                            <asp:Label ID="lblPeriod_1" runat="server" Text='<%#Eval("P1Val") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblTT_1" runat="server" Text='<%#Eval("TT_1") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Period 2" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkPeriod_2" runat="server" Text='<%#Eval("P2Val") %>' ForeColor="Blue"
                                OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                            <asp:Label ID="lblPeriod_2" runat="server" Text='<%#Eval("P2Val") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblTT_2" runat="server" Text='<%#Eval("TT_2") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Period 3" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkPeriod_3" runat="server" Text='<%#Eval("P3Val") %>' ForeColor="Blue"
                                OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                            <asp:Label ID="lblPeriod_3" runat="server" Text='<%#Eval("P3Val") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblTT_3" runat="server" Text='<%#Eval("TT_3") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Period 4" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkPeriod_4" runat="server" Text='<%#Eval("P4Val") %>' ForeColor="Blue"
                                OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                            <asp:Label ID="lblPeriod_4" runat="server" Text='<%#Eval("P4Val") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblTT_4" runat="server" Text='<%#Eval("TT_4") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Period 5" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkPeriod_5" runat="server" Text='<%#Eval("P5Val") %>' ForeColor="Blue"
                                OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                            <asp:Label ID="lblPeriod_5" runat="server" Text='<%#Eval("P5Val") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblTT_5" runat="server" Text='<%#Eval("TT_5") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Period 6" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkPeriod_6" runat="server" Text='<%#Eval("P6Val") %>' ForeColor="Blue"
                                OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                            <asp:Label ID="lblPeriod_6" runat="server" Text='<%#Eval("P6Val") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblTT_6" runat="server" Text='<%#Eval("TT_6") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Period 7" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkPeriod_7" runat="server" Text='<%#Eval("P7Val") %>' ForeColor="Blue"
                                OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                            <asp:Label ID="lblPeriod_7" runat="server" Text='<%#Eval("P7Val") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblTT_7" runat="server" Text='<%#Eval("TT_7") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Period 8" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkPeriod_8" runat="server" Text='<%#Eval("P8Val") %>' ForeColor="Blue"
                                OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                            <asp:Label ID="lblPeriod_8" runat="server" Text='<%#Eval("P8Val") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblTT_8" runat="server" Text='<%#Eval("TT_8") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Period 9" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkPeriod_9" runat="server" Text='<%#Eval("P9Val") %>' ForeColor="Blue"
                                OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                            <asp:Label ID="lblPeriod_9" runat="server" Text='<%#Eval("P9Val") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblTT_9" runat="server" Text='<%#Eval("TT_9") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Period 10" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkPeriod_10" runat="server" Text='<%#Eval("P10Val") %>' ForeColor="Blue"
                                OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                            <asp:Label ID="lblPeriod_10" runat="server" Text='<%#Eval("P10Val") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblTT_10" runat="server" Text='<%#Eval("TT_10") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <table class="printclass" style="width: 98%; height: auto; margin-top: 100px; padding: 0px;">
                <tr>
                    <td>
                        Head Of the Department
                    </td>
                    <td style="text-align: right">
                        Signature of the Teacher
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <center>
        <div id="div1" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="div2" runat="server" class="table" style="background-color: White; height: 200px;
                    width: 33%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; left: 39%;
                    right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                    <div id="divTreeView" runat="server" align="left" style="overflow: auto; width: 450px;
                        height: 200px; border-radius: 10px; border: 1px solid Gray;">
                        <asp:CheckBoxList ID="cblTime" runat="server" Font-Bold="True" Font-Names="Book Antoqua"
                            Width="600px" ForeColor="Black">
                        </asp:CheckBoxList>
                        <center>
                            <asp:Button ID="btnColse" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnColse_Click"
                                Text="Close" runat="server" />
                            <asp:Button ID="Button1" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btndelete_OnClick"
                                Text="Remove" runat="server" />
                            <asp:Label ID="lblErrorMsg" ForeColor="Red" runat="server" Visible="false" Font-Bold="true"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </center>
                    </div>
                </div>
            </center>
        </div>
    </center>
    <center>
        <div id="div3" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="div14" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; left: 39%;
                    right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%; padding: 5px;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label4" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="Button3" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnAdd_OnClick"
                                            Text="Ok" runat="server" />
                                        <asp:Button ID="Button5" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="Button5_Click"
                                            Text="Cancel" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <center>
        <div id="divPopAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%; padding: 5px;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                            Text="Ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <%-- Pop Set Settings--%>
    <center>
        <div id="divMandFee" runat="server" visible="false" style="height: 100em; z-index: 100000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="Div15" runat="server" class="table" style="background-color: White; height: 481px;
                    width: 500px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 75px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td>
                                    <center>
                                        <b style="font-size: 20px; color: Red;">Hour Point Settings</b>
                                    </center>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Text="Edu.Level" Font-Names="Book Antiqua"
                                        Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    <asp:DropDownList ID="DropDownList1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        OnSelectedIndexChanged="DropDownList1_change" Font-Size="Medium" CssClass="textbox1 ddlheight5"
                                        Width="160px" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                  <div class="GridDock" id="dvGridWidth">
                                    <asp:GridView ID="gridPoint" runat="server" Height="200px" Width="200px" AutoGenerateColumns="false"
                                        OnDataBound="gridPoint_OnDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lblSNo" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label></center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Type" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSubjectType" runat="server" Text='<%#Eval("subject_type")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Point" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPont" Visible="true" Text='<%#Eval("points")%>' runat="server"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtPont"
                                                        FilterType="numbers,custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_SaveMandfee" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                            width: 65px;" OnClick="btn_SaveMandfee_Click" Text="Save" runat="server" />
                                        <asp:Button ID="btn_CloseMandFee" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                            width: 65px;" OnClick="btn_CloseMandFee_Click" Text="Close" runat="server" />
                                    </center>
                                </td>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblErrMsg11" runat="server" ForeColor="Red" Visible="false" Font-Names="Book Antiqua"
                                            Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
</asp:Content>
