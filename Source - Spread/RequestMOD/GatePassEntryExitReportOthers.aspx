<%@ Page Title="" Language="C#" MasterPageFile="~/RequestMOD/RequestSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="GatePassEntryExitReportOthers.aspx.cs" Inherits="GatePassEntryExitReportOthers" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>--%>
    <script type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function PrintDiv() {
            var panel = document.getElementById("<%=contentDiv.ClientID %>");
            var printWindow = window.open('', '', 'height=auto,width=685');
            printWindow.document.write('<html');
            printWindow.document.write('<head> <style type="text/css"> p{ font-size: x-small;margin: 0px; padding: 0px; border: 0px;  } body{ margin:0px;}</style>');
            printWindow.document.write('</head><body>');
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
        function display() {
            document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
        }
        function columnOrderCbl() {
            var cball = document.getElementById('<%=cb_column.ClientID%>');
            var cblall = document.getElementById('<%=cblcolumnorder.ClientID%>');
            var tagname = cblall.getElementsByTagName("input");
            if (cball.checked == true) {
                for (var i = 0; i < tagname.length; i++) {
                    tagname[i].checked = true;
                }
            }
            else {
                for (var i = 0; i < tagname.length; i++) {
                    tagname[i].checked = false;
                }
            }
        }
        function columnOrderCb() {
            var count = 0;
            var cball = document.getElementById('<%=cb_column.ClientID%>');
            var cblall = document.getElementById('<%=cblcolumnorder.ClientID%>');
            var tagname = cblall.getElementsByTagName("input");
            var totalCount = (tagname.length - 1);
            for (var i = 0; i < tagname.length; i++) {
                if (tagname[i].checked == true) {
                    count += 1;
                }
            }
            if (tagname.length == count) {
                cball.checked = true;
            }
            else {
                cball.checked = false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body style="font-family: Book Antiqua;">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: #008000">Gate Entry/Exit Report Others</span></div>
                    <br />
                </center>
            </div>
            <div class="maindivstyle">
                <div>
                    <center>
                        <table class="maintablestyle" >
                            <tr style="padding-left: 20px;">
                                <td>
                                    College
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_col" runat="server" CssClass="textbox  txtheight1" Style="height: 15px;
                                                width: 160px;" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel" Style="height: 100px;">
                                                <asp:CheckBox ID="cb_col" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_col_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_col" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_col_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txt_col"
                                                PopupControlID="pbatch" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <%-- <td>
                                    <asp:Label ID="lblmem" runat="server" Text="MemType"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtmem" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="pnlmem" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 126px;
                                                height: auto;">
                                                <asp:CheckBox ID="cbmem" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cbmem_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="cblmem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblmem_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txtmem"
                                                PopupControlID="pnlmem" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>--%>
                                <td>
                                    <asp:Label ID="lblmem" runat="server" Text="MemType"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlmemtype" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Style="width: 100px;" OnSelectedIndexChanged="ddlmemtype_Selected" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Status
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_status" runat="server" CssClass="textbox txtheight1" Style="width: 60px;
                                                height: 15px;" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel" Style="height: 100px;
                                                width: 100px;">
                                                <asp:CheckBox ID="cb_status" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_status_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_status" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_status_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_status"
                                                PopupControlID="Panel4" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                               
                                  <td>
                                    Request
                                </td>
                                <%--Status--%>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_app_status" runat="server" CssClass="textbox txtheight1" Style="width: 60px;
                                                height: 15px;" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Style="height: 100px;
                                                width: 130px;">
                                                <asp:CheckBox ID="cb_app_status" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_app_status_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_app_status" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cb_app_status_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">Un Approved</asp:ListItem>
                                                    <asp:ListItem Value="1">Approved</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_app_status"
                                                PopupControlID="Panel1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td></tr>
                                <tr>
                                <td colspan="4">
                                    <table>
                                        <tr>
                                            <td colspan="2">
                                                <fieldset id="cbfiled" runat="server" style="border-color: Black; height: 10px; width: 113px;">
                                                    <asp:CheckBox ID="cbentry" runat="server" Text="Entry" />
                                                    <asp:CheckBox ID="cbexit" runat="server" Text="Exit" />
                                                    <%-- AutoPostBack="true" OnCheckedChanged="cbentry_OnCheckedChanged"
                                      AutoPostBack="true" OnCheckedChanged="cbexit_OnCheckedChanged"--%>
                                                </fieldset>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cbdtfrom" runat="server" AutoPostBack="true" OnCheckedChanged="cbdtfrom_OnCheckedChanged" />
                                                From
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtfrmdt" runat="server" CssClass="textbox textbox1 txtheight1"
                                                    Style="height: 15px; width: 69px;" Enabled="false"></asp:TextBox>
                                                <asp:CalendarExtender ID="Cal_date" TargetControlID="txtfrmdt" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                                    Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td colspan="2">
                                                To
                                                <asp:TextBox ID="txttodt" runat="server" CssClass="textbox textbox1 txtheight1" Style="height: 15px;
                                                    width: 69px;" Enabled="false"></asp:TextBox>
                                                <asp:CalendarExtender ID="caltodate" runat="server" TargetControlID="txttodt" CssClass="cal_Theme1 ajax__calendar_active"
                                                    Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                           
                         
                                <td colspan="10">
                                    <table>
                                        <tr>
                                            <td colspan="14">
                                                <asp:CheckBox ID="cbtime" runat="server" Enabled="true" AutoPostBack="true" OnCheckedChanged="cbtime_Changed" />
                                                From
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlhourfr" Width="50px" runat="server" CssClass="ddlheight textbox1"
                                                    Enabled="false">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlminsfr" Width="50px" runat="server" CssClass="ddlheight
        textbox1" Enabled="false">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlsecsfr" runat="server" Width="50px" CssClass="ddlheight textbox1"
                                                    Enabled="false">
                                                    <asp:ListItem>AM</asp:ListItem>
                                                    <asp:ListItem>PM</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                To
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlhourto" runat="server" Width="50px" CssClass="ddlheight2 textbox1"
                                                    Enabled="false">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlminsto" runat="server" Width="50px" CssClass="ddlheight2 textbox1"
                                                    Enabled="false">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlsecsto" runat="server" Width="50px" CssClass="ddlheight2 textbox1"
                                                    Enabled="false">
                                                    <asp:ListItem>AM</asp:ListItem>
                                                    <asp:ListItem>PM</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td> </tr>
                                 <tr id="visi" runat="server" visible="false">
                      <td>
                            Destination
                            </td>
                            <td>
                              <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" CssClass="textbox txtheight1" Style="width: 158px;
                                                height: 15px;" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                                width: 230px;">
                                                <asp:CheckBox ID="chkdes" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkdes_CheckedChanged"
                                                     />
                                                <asp:CheckBoxList ID="cbldes" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbldes_SelectedIndexChanged" >
                                                   
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="TextBox1"
                                                PopupControlID="Panel2" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel></td>
                     
                            <td>
                            Department
                            </td>
                            <td>
                              <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="TextBox4" runat="server" CssClass="textbox txtheight1" Style="width: 95px;
                                                height: 15px;" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel6" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                                width: 230px;">
                                                <asp:CheckBox ID="Chkdept" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkdes1_CheckedChanged"
                                                     />
                                                <asp:CheckBoxList ID="Cbldept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbldes1_SelectedIndexChanged"  >
                                                   <asp:ListItem Value="0">Others</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="TextBox4"
                                                PopupControlID="Panel6" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate></asp:UpdatePanel></td>
                                             
                            <td>
                            Staff Type
                            </td>
                            <td>
                              <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="TextBox3" runat="server" CssClass="textbox txtheight1" Style="width: 131px;
                                                height: 15px;" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel5" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                                width: 230px;">
                                                <asp:CheckBox ID="Chkstafftype" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkdes2_CheckedChanged"
                                                     />
                                                <asp:CheckBoxList ID="cblstafftype" runat="server" AutoPostBack="True"  OnSelectedIndexChanged="cbldes2_SelectedIndexChanged" >
                                                    
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="TextBox3"
                                                PopupControlID="Panel5" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel></td>
  <td>
                            Staff Name
                            </td>
                            <td>
  <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="TextBox2" runat="server" CssClass="textbox txtheight1" Style="width: 94px;
                                                height: 15px;" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                                width: 230px;">
                                                <asp:CheckBox ID="chkstaffname" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkdes3_CheckedChanged"
                                                    />
                                                <asp:CheckBoxList ID="cblstaffname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbldes3_SelectedIndexChanged"  >
                                                   
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="TextBox2"
                                                PopupControlID="Panel3" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                            </td>
                                    
                            </tr>
                             <tr>
                                <td colspan="4">
                                    <table>
                                        <tr>
                                            <td>
                                                Search
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlsearch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlsearch_OnSelected"
                                                    CssClass="textbox1 ddlheight2">
                                                    <%--<asp:ListItem Selected="True" Text="Roll No" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Reg No" Value="1"></asp:ListItem>--%>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtsearch" runat="server" CssClass="textbox textbox1" Style="height: 20px;
                                                    width: 170px;" AutoPostBack="true" Placeholder="Enter The Input"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="acext_roll" runat="server" DelimiterCharacters="" Enabled="True"
                                                    ServiceMethod="GetRoll" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                    CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch" CompletionListCssClass="autocomplete_completionListElement"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                                </asp:AutoCompleteExtender>
                                                <asp:FilteredTextBoxExtender ID="flttxt" runat="server" TargetControlID="txtsearch"
                                                    ValidChars=" ./-$" FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td colspan="2">
                                    <asp:Button ID="btngo" runat="server" CssClass="textbox1 textbox btn1" Text="Go"
                                        OnClick="btngo_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <div id="divcolorder" runat="server" visible="true">
                                        <center>
                                            <div>
                                                <center>
                                                    <asp:Panel ID="pheaderfilter" runat="server" CssClass="cpHeader" Height="22px" Width="146px"
                                                        BackColor="#0CA6CA" Style="margin-top: -0.1%; margin-left: -853px;">
                                                        <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                                    </asp:Panel>
                                                </center>
                                            </div>
                                            <br />
                                            <div>
                                                <asp:Panel ID="pcolumnorder" runat="server" CssClass="maintablestyle" Width="930px">
                                                    <div id="divcolumn" runat="server" height="auto"  width="auto">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="cb_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                        Font-Size="Medium" Text="Select All" onchange="return columnOrderCbl()" />
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -477px;"
                                                                        Visible="false" Width="111px">Remove  All</asp:LinkButton><%--OnClick="lb_Click"--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="tborder" Visible="false" Width="867px" TextMode="MultiLine" CssClass="style1"
                                                                        AutoPostBack="true" runat="server" Enabled="false">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="60px" Width="950px"
                                                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="4"
                                                                        RepeatDirection="Horizontal" onclick="return columnOrderCb()">
                                                                    </asp:CheckBoxList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </center>
                                        <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                                            CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                                            TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="~/images/right.jpeg"
                                            ExpandedImage="~/images/down.jpeg">
                                        </asp:CollapsiblePanelExtender>
                                        <br />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divspread" runat="server" visible="false" style="width: 961px; overflow: auto;
                                        background-color: White; border-radius: 10px;">
                                        <FarPoint:FpSpread ID="spreadDet" runat="server" Visible="true" BorderStyle="Solid"
                                            BorderWidth="0px"  Style="overflow: auto;Width:auto; border: 0px solid #999999;
                                            border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                            class="spreadborder" OnPreRender="spreadDet_SelectedIndexChanged" 
            OnCellClick="spreadDet_CellClick">
                                            <%--OnCellClick="FpSpread1_OnCellClick" OnPreRender="FpSpread1_Selectedindexchanged"--%>
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <center>
                                            <div id="print" runat="server" visible="false">
                                                <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="" Visible="false"></asp:Label>
                                                <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                                                <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display()"
                                                    CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                                    InvalidChars="/\">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Export To Excel"
                                                    Width="127px" Height="32px" CssClass="textbox textbox1" />
                                                <asp:Button ID="btnprintmasterhed" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                                    Height="32px" Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                                                     <asp:Button ID="Button1" runat="server" Text="Reprint" OnClick="btnprintmaster1_Click"
                                                    Height="32px" Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                                                <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                                            </div>
                                        </center>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </div>
        </center>
        <center>
            <div id="alertDiv" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>

        <div style="height: 1px; width: 1px; overflow: auto;">
        <div id="contentDiv" runat="server" style="height: auto; width: 900px;" visible="false">
        </div>
    </div>
     <input type="hidden" runat="server" id="Hidden1"  />
    </div> 
    </body>
</asp:Content>
