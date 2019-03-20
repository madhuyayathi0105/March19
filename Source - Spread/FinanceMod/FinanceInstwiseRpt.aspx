<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="FinanceInstwiseRpt.aspx.cs" Inherits="FinanceInstwiseRpt" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=btnExcel.ClientID%>').click(function () {
                var excelName = $('#<%=txtexcelname.ClientID%>').val();
                if (excelName == null || excelName == "") {
                    $('#<%=lblvalidation1.ClientID%>').show();
                    return false;
                }
                else {
                    $('#<%=lblvalidation1.ClientID%>').hide();
                }
            });

            $('#<%=txtexcelname.ClientID %>').keypress(function () {
                $('#<%=lblvalidation1.ClientID %>').hide();
            });
            $('#<%=btnMemPopup.ClientID %>').click(function () {
                var chkBoxList = document.getElementById('<%=cblmem.ClientID %>');
                var selectedCount = CheckBoxListSelectDept(chkBoxList);
                if (selectedCount != 1) {
                    alert("Please select any one Staff/Vendor/Other type!");
                    return false;
                }
            });
        });
        function CheckBoxListSelectDept(chkBoxList) {
            var totCount = 0;
            //            var chkBoxList = document.getElementById('<%=cblmem.ClientID %>');
            var chkBoxCount = chkBoxList.getElementsByTagName("input");
            for (var i = 0; i < chkBoxCount.length; i++) {
                if (chkBoxCount[i].checked)
                    totCount++;
            }
            return totCount;
        }
        function checkDate() {
            var fromDate = "";
            var toDate = "";
            var date = ""
            var date1 = ""
            var month = "";
            var month1 = "";
            var year = "";
            var year1 = "";
            var empty = "";
            fromDate = document.getElementById('<%=txt_fromdate.ClientID%>').value;
            toDate = document.getElementById('<%=txt_todate.ClientID%>').value;

            date = fromDate.substring(0, 2);
            month = fromDate.substring(3, 5);
            year = fromDate.substring(6, 10);

            date1 = toDate.substring(0, 2);
            month1 = toDate.substring(3, 5);
            year1 = toDate.substring(6, 10);
            var today = new Date();
            //  var currentDate = today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1;
            var yyyy = today.getFullYear();
            if (dd < 10) { dd = '0' + dd }
            if (mm < 10) { mm = '0' + mm }
            var today = dd + '/' + mm + '/' + yyyy;
            if (year == year1) {
                if (month == month1) {
                    if (date == date1) {
                        empty = "";
                    }
                    else if (date < date1) {
                        empty = "";
                    }
                    else {
                        empty = "e";
                    }
                }
                else if (month < month1) {
                    empty = "";
                }
                else if (month > month1) {
                    empty = "e";
                }
            }
            else if (year < year1) {
                empty = "";
            }
            else if (year > year1) {
                empty = "e";
            }
            if (empty != "") {
                document.getElementById('<%=txt_fromdate.ClientID%>').value = today;
                document.getElementById('<%=txt_todate.ClientID%>').value = today;
                alert("To date should be greater than from date ");
                return false;
            }
        }
        function SelLedgers() {
            var chkSelAll = document.getElementById("<%=chkGridSelectAll.ClientID %>");
            var tbl = document.getElementById("<%=GrdStaff.ClientID %>");
            var gridViewControls = tbl.getElementsByTagName("input");

            for (var i = 1; i < (tbl.rows.length); i++) {
                var chkSelectid = document.getElementById('MainContent_GrdStaff_selectchk_' + i.toString());

                if (chkSelAll.checked == false) {
                    chkSelectid.checked = false;
                } else {
                    chkSelectid.checked = true;
                }
            }

        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span id="sphd" runat="server" class="fontstyleheader" style="color: Green;">Institutionwise
                    Collection</span>
            </div>
        </center>
    </div>
    <div>
        <center>
            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                <ContentTemplate>
                    <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                        <table class="maintablestyle">
                            <tr>
                                <td colspan="2">
                                    <asp:RadioButtonList ID="rblMemType" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" OnSelectedIndexChanged="rblMemType_Selected">
                                        <asp:ListItem Text="Student" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Others"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td colspan="2" id="tdmemtype" runat="server" visible="false">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtmem" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="pnlmem" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 126px;
                                                height: 120px;">
                                                <asp:CheckBox ID="cbmem" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cbmem_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="cblmem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblmem_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtmem"
                                                PopupControlID="pnlmem" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td id="tdMemPopup" runat="server" visible="false">
                                    <asp:Button ID="btnMemPopup" runat="server" CssClass="textbox btn1 textbox1" Text="?"
                                        OnClick="btnMemPopup_Click" />
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lbldisp" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" ForeColor="White"></asp:Label>
                                    <asp:Label ID="lblval" runat="server" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtclg" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="pnlclg" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 350px;
                                                height: 120px;">
                                                <asp:CheckBox ID="cbclg" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cbclg_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="cblclg" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblclg_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txtclg"
                                                PopupControlID="pnlclg" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblheader" runat="server" Text="Header" Style="width: 50px;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_studhed" runat="server" Style="height: 20px; width: 100px;"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="pnl_studhed" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                Style="width: 300px; height: 180px;">
                                                <asp:CheckBox ID="chk_studhed" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="chk_studhed_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="chkl_studhed" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_studhed_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_studhed"
                                                PopupControlID="pnl_studhed" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_ledger" runat="server" Text="Ledger" Style="width: 50px;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_studled" runat="server" Style="height: 20px; width: 100px;"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="pnl_studled" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                Style="width: 300px; height: 180px;">
                                                <asp:CheckBox ID="chk_studled" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="chk_studled_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="chkl_studled" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_studled_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_studled"
                                                PopupControlID="pnl_studled" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    PaymentMode
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upd_paid" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_paid" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="pnl_paid" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                Style="width: 126px; height: 160px;">
                                                <asp:CheckBox ID="chk_paid" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="chk_paid_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="chkl_paid" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_paid_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_paid"
                                                PopupControlID="pnl_paid" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td id="tdlblfnl" runat="server" visible="false">
                                    <asp:Label runat="server" ID="lblfyear" Text="FinanceYear" Width="85px"></asp:Label>
                                </td>
                                <td id="tdfnl" runat="server" visible="false">
                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtfyear" Style="height: 20px; width: 118px;" CssClass="Dropdown_Txt_Box"
                                                runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                            <asp:Panel ID="Pfyear" runat="server" CssClass="multxtpanel" Width="178px">
                                                <asp:CheckBox ID="chkfyear" runat="server" Text="Select All" OnCheckedChanged="chkfyear_changed"
                                                    AutoPostBack="True" />
                                                <asp:CheckBoxList ID="chklsfyear" runat="server" OnSelectedIndexChanged="chklsfyear_selected"
                                                    AutoPostBack="True">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txtfyear"
                                                PopupControlID="Pfyear" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div id="divdatewise" runat="server">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_fromdate" runat="server" Text="From"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_fromdate" runat="server" Style="height: 20px; width: 75px;"
                                                        onchange="return checkDate()"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_todate" runat="server" Text="To" Style="margin-left: 4px;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_todate" runat="server" Style="height: 20px; width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td colspan="2">
                                    <asp:RadioButtonList ID="rblmode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblmode_Selected"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Header" Value="0" Selected="true"></asp:ListItem>
                                        <asp:ListItem Text="Ledger" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td colspan="2" id="tdlblStudCat" runat="server" visible="false">
                                    <asp:CheckBox ID="checkdicon" runat="server" Text="Student Catagory" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Style="width: 200px;" />
                                    <%--AutoPostBack="true" OnCheckedChanged="checkdicon_Changed"--%>
                                </td>
                                <td colspan="2" id="tdvalStudCat" runat="server" visible="false">
                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtinclude" Enabled="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                Style="height: 20px; width: 79px;" CssClass="Dropdown_Txt_Box" runat="server"
                                                ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                            <asp:Panel ID="pnlinclude" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                Width="200px" Style="height: auto;">
                                                <asp:CheckBox ID="cbinclude" runat="server" Text="Select All" Font-Size="Medium"
                                                    Font-Names="Book Antiqua" OnCheckedChanged="cbinclude_OnCheckedChanged" AutoPostBack="True" />
                                                <asp:CheckBoxList ID="cblinclude" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    OnSelectedIndexChanged="cblinclude_OnSelectedIndexChanged" AutoPostBack="True">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txtinclude"
                                                PopupControlID="pnlinclude" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td id="tdOthers" runat="server" visible="false">
                                    <asp:CheckBox ID="cbIncOthers" runat="server" Text="Include Other" Checked="true" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <table>
                                        <tr>
                                            <td colspan="4">
                                                <fieldset style="height: 23px;">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="cbAcdYear" runat="server" Text="" />
                                                                <asp:DropDownList ID="ddlAcademic" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                    Width="102px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:RadioButtonList ID="rblTypeNew" runat="server" RepeatDirection="Horizontal">
                                                                    <asp:ListItem Text="Academic Year" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="Odd"></asp:ListItem>
                                                                    <asp:ListItem Text="Even"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpGo" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btngo" runat="server" CssClass="textbox btn2" Text="Go" OnClick="btngo_Click" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <div id="divlabl" runat="server" visible="false">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblcash" runat="server" Text="Cash" Visible="false" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" BackColor="LightCoral"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblchq" runat="server" Text="Cheque" Visible="false" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" BackColor="LightGray"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldd" runat="server" Text="DD" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" BackColor="Orange"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblchal" runat="server" Text="Challan" Visible="false" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" BackColor="LightGreen"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblonline" runat="server" Text="Online" Visible="false" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" BackColor="LightGoldenrodYellow"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblcard" runat="server" Text="Card" Visible="false" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" BackColor="white"></asp:Label>
                                    </td>
                                    <%--Added by saranya on 13/02/2018--%>
                                    <td>
                                        <asp:Label ID="lblNeft" runat="server" Text="Neft" Visible="false" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" BackColor="Aqua"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <center>
                            <div id="print" runat="server" visible="false">
                                <asp:Label ID="lblvalidation1" runat="server" Text="Please Enter Your Report Name"
                                    Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Red" Style="display: none;"></asp:Label>
                                <asp:Label ID="lblrptname" runat="server" Visible="false" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txtexcelname" runat="server" Visible="false" Width="180px" onkeypress="display()"
                                    CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                    InvalidChars="/\">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="btnExcel" runat="server" Visible="false" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="btnExcel_Click" Text="Export To Excel" Width="127px"
                                    Height="32px" CssClass="textbox textbox1" />
                                <asp:Button ID="btnprintmasterhed" runat="server" Visible="false" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" Height="32px"
                                    Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                                <NEW:NEWPrintMater runat="server" ID="Printcontrolhed" Visible="false" />
                            </div>
                        </center>
                        <br />
                        <asp:GridView ID="grdInstWiseCollectionReport" Width="900px" runat="server" ShowFooter="false"
                            AutoGenerateColumns="true" Font-Names="Book Antiqua" ShowHeader="false" toGenerateColumns="false"
                            OnRowDataBound="grdInstWiseCollectionReport_RowDataBound">
                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                        </asp:GridView>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnExcel" />
                    <asp:PostBackTrigger ControlID="btnprintmasterhed" />
                </Triggers>
            </asp:UpdatePanel>
        </center>
        <%--Staff Lookup --%>
        <center>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <div id="div_staffLook" runat="server" visible="false" class="popupstyle popupheight1 ">
                        <asp:ImageButton ID="ImageButton5" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 30px; margin-left: 310px;"
                            OnClick="btn_exitstaff_Click" />
                        <br />
                        <br />
                        <div style="background-color: White; height: 550px; width: 650px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <br />
                            <center>
                                <div>
                                    <span id="spnHdName" runat="server" class="fontstyleheader" style="color: Green;">
                                    </span>
                                </div>
                            </center>
                            <br />
                            <table class="maintablestyle">
                                <tr>
                                    <td>
                                        <span class="challanLabel">
                                            <p>
                                                Search By</p>
                                        </span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsearch1" runat="server" CssClass="textbox1 ddlheight3" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlsearch1_OnSelectedIndexChanged">
                                            <asp:ListItem Text="Search By Name" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Search By Code" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtsearch1" runat="server" Visible="false" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch1"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <asp:TextBox ID="txtsearch1c" runat="server" Visible="false" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetStaffno" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch1c"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpStaffGo" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="btn_go2Staff" runat="server" CssClass="textbox btn1 textbox1" Text="Go"
                                                    OnClick="btn_go2Staff_Click" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <div>
                                <asp:Label ID="lbl_errormsgstaff" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </div>
                            <span style="padding-right: 100px; margin-left: -260px; margin-top: 3px;">
                                <asp:CheckBox ID="chkGridSelectAll" runat="server" Text="SelectAll" Visible="false"
                                    onchange="return SelLedgers();" />
                            </span>
                            <div id="divTreeView" visible="false" runat="server" align="left" style="overflow: auto;
                                width: 520px; height: 350px; border-radius: 10px; border: 1px solid Gray;">
                                <asp:GridView ID="GrdStaff" Width="500px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                    Font-Names="Book Antiqua" ShowHeader="false" toGenerateColumns="false" OnRowDataBound="GrdStaff_RowDataBound">
                                    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Label ID="lbl_sno" runat="server" Style="width: auto;" Text='<%#Eval("Sno") %>'></asp:Label>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="allchk" runat="server" Text="Select All" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="selectchk" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                </asp:GridView>
                            </div>
                            <center>
                                <div>
                                    <asp:Button ID="btn_staffOK" runat="server" CssClass="textbox btn2 textbox1" Text="Ok"
                                        OnClick="btn_staffOK_Click" />
                                    <asp:Button ID="btn_exitstaff" runat="server" CssClass="textbox btn2 textbox1" Text="Exit"
                                        OnClick="btn_exitstaff_Click" />
                                </div>
                            </center>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
    </div>
    <%--progressBar for UpGo--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpGo">
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
    <%--progressBar for UpStaffGo--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpStaffGo">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
