<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="DeclarationForm.aspx.cs" Inherits="DeclarationForm" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        function PrintDiv() {
            var panel = document.getElementById("<%=contentDiv.ClientID %>");
            var printWindow = window.open('', '', 'height=auto,width=1191');
            printWindow.document.write('<html');
            printWindow.document.write('<head>');
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
    </script>
}
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <center>              
                <div>
                    <span class="fontstyleheader" style="color: Green;">Declaration Form</span></div>  <br />
            </center>
            <table id="Table1" class="maintablestyle" runat="server">
                <tr>
                    <td>
                        <asp:Label ID="lbl_collegename" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_collegename" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged"
                            Height="29px" Width="202px" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblStr" Text="Stream" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                            runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddltype" runat="server" Width="92px" Height="30px" Enabled="false"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="type_Change"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold;">Batch</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" Width="70px" Height="30px" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="batch_SelectedIndexChange">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold;">Education
                            Level</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddledulevel" runat="server" Width="70px" Height="30px" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="edulevel_SelectedIndexChange">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblDeg" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                            runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel_Department" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_degree" runat="server" ReadOnly="true" Width="88px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox textbox1 txtheight">---Select---</asp:TextBox>
                                <asp:Panel ID="paneldegree" runat="server" Height="300px" CssClass="multxtpanel">
                                    <asp:CheckBox ID="cbdegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="cbdegree_Changed" />
                                    <asp:CheckBoxList ID="cbldegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnSelectedIndexChanged="cbldegree_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_degree"
                                    PopupControlID="paneldegree" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="cbldegree" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblBran" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_department" runat="server" ReadOnly="true" Width="177px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox textbox1 txtheight">---Select---</asp:TextBox>
                                <asp:Panel ID="paneldepartment" runat="server" Height="300px" CssClass="multxtpanel">
                                    <asp:CheckBox ID="cbdepartment1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="cbdepartment_Changed" />
                                    <asp:CheckBoxList ID="cbldepartment" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="cbldepartment_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_department"
                                    PopupControlID="paneldepartment" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold;">From
                        </span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtfrmdate" runat="server" Style="font-family: Book Antiqua; font-size: medium;
                            font-weight: bold;" CssClass="textbox textbox1 txtheight" AutoPostBack="true"
                            OnTextChanged="txtfrmdate_TextChanged"></asp:TextBox>
                        <asp:CalendarExtender ID="calfrmdate" runat="server" TargetControlID="txtfrmdate"
                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold;">To
                        </span>
                    </td>
                    <td>
                        <asp:TextBox ID="txttodate" runat="server" Style="font-family: Book Antiqua; font-size: medium;
                            font-weight: bold;" CssClass="textbox textbox1 txtheight" AutoPostBack="true"
                            OnTextChanged="txttodate_TextChanged"></asp:TextBox>
                        <asp:CalendarExtender ID="caltodate" runat="server" TargetControlID="txttodate" CssClass="cal_Theme1 ajax__calendar_active"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold;">Status</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlstatus" runat="server" Width="93px" Height="30px" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlstatus_SelectedIndexChange">
                            <asp:ListItem Text="Applied"></asp:ListItem>
                            <asp:ListItem Text="Shortlist"></asp:ListItem>
                            <asp:ListItem Text="Admitted"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="11">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;"
                                        ID="lbl_searchstudname" runat="server" Text="Student Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_searchstudname" runat="server" CssClass="textbox textbox1 txtheight2"
                                        Width="165px" AutoPostBack="true" OnTextChanged="txt_searchstudname_TextChanged"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="acext_name" runat="server" DelimiterCharacters="" Enabled="True"
                                        ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                        CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchstudname" CompletionListCssClass="autocomplete_completionListElement"
                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_searchstudname"
                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Label Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;"
                                        ID="lbl_searchappno" runat="server" Text="App No"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_searchappno" runat="server" CssClass="textbox textbox1 txtheight"
                                        Width="135px" AutoPostBack="true" OnTextChanged="txt_searchappno_TextChanged"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender02" runat="server" TargetControlID="txt_searchappno"
                                        FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="getappfrom" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchappno"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Button ID="btngo" runat="server" Text="GO" Font-Bold="true" Font-Names="Book Antiqua"
                                        CssClass="textbox textbox1 btn1" OnClick="btngo_click" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnk_setting" runat="server" Text="Setting" OnClick="lnk_setting_Click"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
          
            <asp:Label ID="mainpgeerr" runat="server" Text="" Visible="false" Font-Bold="true"
                Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Red"></asp:Label>
            <br />
            <br />
            <div id="sp_div" runat="server">
                <FarPoint:FpSpread ID="FpSpread" runat="server" Visible="false" BorderColor="Black"
                    BorderStyle="Solid" BorderWidth="1px" Width="778px" Height="600px" Style="margin-left: 2px;"
                    class="spreadborder" OnButtonCommand="Fpspread_command" ShowHeaderSelection="false">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
            </div>
            <br />
            <br />
            <asp:Button ID="btn_pdf" runat="server" Text="Pdf" Visible="false" Font-Bold="true"
                Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox textbox1 btn2"
                OnClick="btn_pdf_click" />
            <br />
            <br />
            <div id="rprint" runat="server" visible="false">
                <asp:Label ID="lblsmserror" Text="Please Enter Your Report Name" Font-Size="Large"
                    Font-Names="Book Antiqua" Visible="false" ForeColor="Red" runat="server" Font-Bold="true"></asp:Label>
                <asp:Label ID="lblexcel" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
                <asp:TextBox ID="txtexcel" onkeypress="display(this)" CssClass="textbox textbox1"
                    runat="server"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcel"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btnexcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" CssClass="textbox textbox1 btn3" Height="30px" Text="Export Excel"
                    OnClick="btnexcel_Click" />
                <asp:Button ID="btnprintmaster" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Print" Width="59px" Height="30px" OnClick="btnprintmaster_Click"
                    CssClass="textbox textbox1 btn3" />
                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
            </div>
            <br />
            <div id="addsetting" runat="server" visible="false" style="height: 50em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="ImageButton_close" runat="server" Width="40px" Height="40px"
                    ImageUrl="~/images/close.png" Style="height: 30px; width: 30px; position: absolute;
                    margin-top: 95px; margin-left: 278px;" OnClick="ImageButton_close_Click" />
                <br />
                <br />
                <br />
                <br />
                <br />
                <div style="background-color: White; height: 330px; width: 584px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <center>
                        <br />
                        <span style="color: Green;" class="fontstyleheader">Setting</span>
                        <br />
                        <br />
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="cbpartlang" runat="server" Style="font-family: Book Antiqua; font-size: medium;
                                        font-weight: bold;" Text="Part-I Language" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="cbclgtme" runat="server" Style="font-family: Book Antiqua; font-size: medium;
                                        font-weight: bold;" Text="College Timing" AutoPostBack="true" OnCheckedChanged="cbclgtme_OnCheckedChanged" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtclg" runat="server" Enabled="false" Width="88px" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:CheckBox ID="cbatbtnme" Style="font-family: Book Antiqua; font-size: medium;
                                        font-weight: bold;" runat="server" Text="AT/BT/NME" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <asp:Button ID="btn_settingsave" runat="server" Text="Save" CssClass="textbox textbox1 btn2"
                            OnClick="btn_settingsave_Click" />
                    </center>
                </div>
            </div>
            <br />
            <div id="imgdiv2" runat="server" visible="false" style="height: 200%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 292px;
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
        </div>
    </center>
    <%--new print div--%>
    <div style="height: 1px; width: 1px; overflow: auto;">
        <div id="contentDiv" runat="server" style="height:auto; width: 1344px;" visible="false">
        </div>
    </div>
</asp:Content>
