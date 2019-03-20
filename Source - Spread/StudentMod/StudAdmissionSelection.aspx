<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudAdmissionSelection.aspx.cs" Inherits="StudAdmissionSelection" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .rdbstyle input[type=radio]
        {
            display: none;
        }
        .rdbstyle input[type=radio] + label
        {
            display: inline-block;
            margin: -2px;
            padding: 4px 12px;
            margin-bottom: 0;
            font-size: 14px;
            line-height: 20px;
            color: #993399;
            text-align: center;
            text-shadow: 0 1px 1px rgba(255,255,255,0.75);
            vertical-align: middle;
            cursor: pointer;
            background-color: #f5f5f5;
            background-image: -moz-linear-gradient(top,#fff,#e6e6e6);
            background-image: -webkit-gradient(linear,0 0,0 100%,from(#fff),to(#e6e6e6));
            background-image: -webkit-linear-gradient(top,#fff,#e6e6e6);
            background-image: -o-linear-gradient(top,#fff,#e6e6e6);
            background-image: linear-gradient(to bottom,#fff,#e6e6e6);
            background-repeat: repeat-x;
            border: 1px solid #ccc;
            border-color: #e6e6e6 #e6e6e6 #bfbfbf;
            border-color: rgba(0,0,0,0.1) rgba(0,0,0,0.1) rgba(0,0,0,0.25);
            border-bottom-color: #b3b3b3;
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffffffff',endColorstr='#ffe6e6e6',GradientType=0);
            filter: progid:DXImageTransform.Microsoft.gradient(enabled=false);
            -webkit-box-shadow: inset 0 1px 0 rgba(255,255,255,0.2),0 1px 2px rgba(0,0,0,0.05);
            -moz-box-shadow: inset 0 1px 0 rgba(255,255,255,0.2),0 1px 2px rgba(0,0,0,0.05);
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.2),0 1px 2px rgba(0,0,0,0.05);
        }
        .rdbstyle input[type=radio]:checked + label
        {
            background-image: none;
            outline: 0;
            -webkit-box-shadow: inset 0 2px 4px rgba(0,0,0,0.15),0 1px 2px rgba(0,0,0,0.05);
            -moz-box-shadow: inset 0 2px 4px rgba(0,0,0,0.15),0 1px 2px rgba(0,0,0,0.05);
            box-shadow: inset 0 2px 4px rgba(0,0,0,0.15),0 1px 2px rgba(0,0,0,0.05);
            border-bottom-color: #b3b3b3;
            border-bottom-style: solid;
            border-bottom-color: #89D17C;
            border-bottom-width: medium;
        }
        .btnapprove1
        {
            background: transparent;
        }
        .btnapprove1:hover
        {
            background-color: Orange;
            color: White;
        }
        .lnk:hover
        {
            text-shadow: 0 1px 1px rgba(255,255,255,0.75);
            color: Green;
            font-weight: bold;
        }
    </style>
    <style type="text/css">
        .printclass
        {
            display: none;
        }
        .grid-view
        {
            padding: 0;
            margin: 0;
            border: 1px solid #333;
            font-family: "Verdana, Arial, Helvetica, sans-serif, Trebuchet MS";
            font-size: 0.9em;
        }
        
        .grid-view tr.header
        {
            color: white;
            background-color: #0CA6CA;
            height: 30px;
            vertical-align: middle;
            text-align: center;
            font-weight: bold;
            font-size: 20px;
        }
        
        .grid-view tr.normal
        {
            color: black;
            background-color: #FDC64E;
            height: 25px;
            vertical-align: middle;
            text-align: center;
        }
        
        .grid-view tr.alternate
        {
            color: black;
            background-color: #D59200;
            height: 25px;
            vertical-align: middle;
            text-align: center;
        }
        
        .grid-view tr.normal:hover, .grid-view tr.alternate:hover
        {
            background-color: white;
            color: black;
            font-weight: bold;
        }
        
        .grid_view_lnk_button
        {
            color: Black;
            text-decoration: none;
            font-size: large;
        }
        .lbl
        {
            font-family: Book Antiqua;
            font-size: 30px;
            font-weight: bold;
            color: Green;
            text-align: center;
            font-style: italic;
        }
        .hdtxt
        {
            font-family: Book Antiqua;
            font-size: large;
            font-weight: bold;
        }
        .FixedHeader
        {
            position: absolute;
            font-weight: bold;
        }
    </style>
    <style type="text/css">
        .printclass
        {
            display: none;
        }
        .grid-view
        {
            padding: 0;
            margin: 0;
            border: 1px solid #333;
            font-family: "Verdana, Arial, Helvetica, sans-serif, Trebuchet MS";
            font-size: 0.9em;
        }
        
        .grid-view tr.header
        {
            color: white;
            background-color: #0CA6CA;
            height: 30px;
            vertical-align: middle;
            text-align: center;
            font-weight: bold;
            font-size: 20px;
        }
        
        .grid-view tr.normal
        {
            color: black;
            background-color: #FDC64E;
            height: 25px;
            vertical-align: middle;
            text-align: center;
        }
        
        .grid-view tr.alternate
        {
            color: black;
            background-color: #D59200;
            height: 25px;
            vertical-align: middle;
            text-align: center;
        }
        
        .grid-view tr.normal:hover, .grid-view tr.alternate:hover
        {
            background-color: white;
            color: black;
            font-weight: bold;
        }
        
        .grid_view_lnk_button
        {
            color: Black;
            text-decoration: none;
            font-size: large;
        }
        .lbl
        {
            font-family: Book Antiqua;
            font-size: 30px;
            font-weight: bold;
            color: Green;
            text-align: center;
            font-style: italic;
        }
        .hdtxt
        {
            font-family: Book Antiqua;
            font-size: large;
            font-weight: bold;
        }
        .FixedHeader
        {
            position: absolute;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">

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

        
    </script>
    <script type="text/javascript">

        function radioChange() {
            var cbdegree = document.getElementById('<%=cbdegree.ClientID %>');
            var deg = document.getElementById('<%=txt_degree.ClientID %>');
            var dept = document.getElementById('<%=txt_dept.ClientID %>');
            if (cbdegree.checked) {
                deg.disabled = false;
                dept.disabled = false;
            }
            else {
                deg.disabled = true;
                dept.disabled = true;
            }
        }          
             
    </script>
    <script type="text/javascript">
        function OnGridHeaderSelected() {
            var id = document.getElementById("<%=gridstud.ClientID %>");
            var len = id.rows.length;
            var i = 0;
            var checkedId = id.rows[0].getElementsByTagName("input")[0].checked;
            for (var ak = 1; ak < len; ak++) {
                if (id.rows[ak].getElementsByTagName("input")[i].type == "checkbox") {
                    if (checkedId == true) {
                        id.rows[ak].getElementsByTagName("input")[i].checked = true;
                    } else {
                        id.rows[ak].getElementsByTagName("input")[i].checked = false;
                    }
                }
            }
        }
        $(document).ready(function () {
            $('#<%=Button5.ClientID %>').click(function () {
                $('#<%=panel4.ClientID %>').hide();
                return false;
            });
            $('#<%=btnSendSmsPop.ClientID %>').click(function () {
                var txtval = $('#<%=txt_SmsMsgPop.ClientID %>').val();
                if (txtval.trim() == null || txtval.trim() == "") {
                    alert("Please Enter Message");
                    return false;
                }
            });
            $('#<%=btnClosePop.ClientID %>').click(function () {
                $('#<%=popSendSms.ClientID %>').hide();
                return false;
            });
            $('#<%=Button12.ClientID %>').click(function () {
                $('#<%=panel2.ClientID %>').hide();
                return false;
            });
        });
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlContents.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=1191');
            printWindow.document.write('<html');
            printWindow.document.write('<head><title>SchoolCompartmentWise</title>');
            printWindow.document.write('</head><body >');
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
        function licetPrintdiv() {
            var panel = document.getElementById("<%=licet_print_div.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=1191');
            printWindow.document.write('<html');
            printWindow.document.write('<head><title>Application Print</title>');
            printWindow.document.write('</head><body >');
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

        function flg1() {

            document.getElementById('<%=Button13.ClientID%>').style.display = 'block';
            document.getElementById('<%=Button14.ClientID%>').style.display = 'block';
        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span id="sphd" runat="server" class="fontstyleheader" style="color: Green;">Admission
                    Selection Process </span>
            </div>
        </center>
    </div>
    <div>
        <center>
            <div id="maindiv" runat="server" style="width: 1000px; height: auto">
                <table>
                    <tr>
                        <td colspan="5">
                            <asp:RadioButtonList ID="rdbtype" runat="server" OnSelectedIndexChanged="rdbtype_SelectedIndexChanged"
                                AutoPostBack="true" RepeatDirection="Horizontal" CellSpacing="4" BorderColor="#999999"
                                Font-Bold="True" CssClass="rdbstyle">
                                <asp:ListItem Value="0" Selected="True">Applied</asp:ListItem>
                                <asp:ListItem Value="1">Shortlist</asp:ListItem>
                                <asp:ListItem Value="2">Admitted</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10">
                            <%-- <asp:Panel ID="Panel1" BorderColor="#993333" runat="server" BorderStyle="Solid" Style="height: 80px;
                                width: auto;">--%>
                            <table class="maindivstyle">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtclg" runat="server" Style="height: 20px; width: 175px;" ReadOnly="true">--Select--</asp:TextBox>
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
                                    <td colspan="2">
                                        Batch
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_batch" Height="27px" runat="server" CssClass=" textbox1 txtheight"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_batch_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbledu" runat="server" Text="Edu Level"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddledu" runat="server" Width="80px" AutoPostBack="True" OnSelectedIndexChanged="ddledu_SelectedIndexchange"
                                            CssClass="ddlheight textbox1">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbdegree" runat="server" onchange="return radioChange();" /><%--onclick="return radioChange();"--%>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UP_degree" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_degree" runat="server" Enabled="false" Style="height: 20px;
                                                    width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel_degree" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                    height: 200px;">
                                                    <asp:CheckBox ID="cb_degree" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cb_degree_OnCheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="pce_degree" runat="server" TargetControlID="txt_degree"
                                                    PopupControlID="panel_degree" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="Up_dept" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_dept" runat="server" Enabled="false" Style="height: 20px; width: 100px;"
                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                    height: 300px;">
                                                    <asp:CheckBox ID="cb_dept" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cb_dept_OnCheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="pce_dept" runat="server" TargetControlID="txt_dept"
                                                    PopupControlID="panel_dept" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="11">
                                        <%--  <div id="divdatewise" runat="server">--%>
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
                                                <td>
                                                    <asp:Label ID="lbl_searchstudname" runat="server" Text="Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_studName" runat="server" Placeholder="Student Name" CssClass="textbox textbox1 txtheight2"
                                                        Width="135px" AutoPostBack="true" OnTextChanged="txt_studName_TextChanged"></asp:TextBox><%--Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"--%>
                                                    <asp:AutoCompleteExtender ID="acext_name" runat="server" DelimiterCharacters="" Enabled="True"
                                                        ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                        CompletionSetCount="10" ServicePath="" TargetControlID="txt_studName" CompletionListCssClass="autocomplete_completionListElement"
                                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_studName"
                                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_searchappno" runat="server" Text="Application No"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_studApplNo" runat="server" CssClass="textbox textbox1 txtheight"
                                                        Width="100px" AutoPostBack="true" OnTextChanged="txt_studApplNo_TextChanged"
                                                        Placeholder="Application No"></asp:TextBox><%--Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"--%>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender02" runat="server" TargetControlID="txt_studApplNo"
                                                        FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="getappfrom" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_studApplNo"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_studMblno" Placeholder="Mobile Number" runat="server" CssClass="textbox textbox1 txtheight2"
                                                        Width="125px" MaxLength="13" AutoPostBack="true" OnTextChanged="txt_studMblno_TextChanged"></asp:TextBox><%--Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"--%>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_studMblno"
                                                        FilterType="numbers,custom" ValidChars=" +">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="getmob" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_studMblno"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblrpt" runat="server" Text="Report"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlMainreport" runat="server" CssClass="textbox textbox1 ddlheight4"
                                            Width="100px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btngo" runat="server" CssClass="textbox btn2" Text="Go" OnClick="btngo_Click" />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="lnkcolorder" runat="server" Width="30px" Height="30px" Text="All"
                                            ImageUrl="~/Hostel Gete Images/images (1)ppp.jpg" OnClick="lnkcolorder_Click" />
                                        <%-- <asp:LinkButton ID="lnkcolorder" runat="server" Text="Column Order" OnClick="lnkcolorder_Click"></asp:LinkButton>--%>
                                        <%-- </td>
                        <td>--%>
                                    </td>
                                </tr>
                            </table>
                            <%--</asp:Panel>--%>
                        </td>
                    </tr>
                    <tr>
                    </tr>
                </table>
            </div>
            <div>
                <fieldset id="buttonview" runat="server" visible="false" style="width: 100px; height: 25px;
                    border-color: Blue;">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnpdfstud" runat="server" CssClass="textbox btn2" Text="PDF" OnClientClick=" return PrintPanel()" />
                                <%--OnClick="btnpdfstud_Click"--%>
                            </td>
                            <td>
                                <asp:Button ID="btnsmsstud" runat="server" CssClass="textbox btn2" Text="SMS" OnClick="btnsmsstud_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnshortstud" runat="server" CssClass="textbox btn2" Text="Shortlist"
                                    OnClick="btnshortstud_Click" Visible="false" />
                            </td>
                            <td>
                                <asp:Button ID="btnadmitstud" runat="server" CssClass="textbox btn2" Text="Admit"
                                    OnClick="btnadmitstud_Click" Visible="false" />
                            </td>
                            <td>
                                <asp:Button ID="btncallltrstud" runat="server" CssClass="textbox btn2" Text="Call Letter"
                                    OnClick="btncallltrstud_Click" Visible="false" />
                            </td>
                            <td>
                                <asp:Button ID="btnrejstud" runat="server" CssClass="textbox btn2" Text="Reject"
                                    OnClick="btnrejstud_Click" Visible="false" />
                            </td>
                            <td>
                                <asp:Button ID="btnleftstud" runat="server" CssClass="textbox btn2" Text="Cancel"
                                    OnClick="btnleftstud_Click" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <asp:Panel ID="pnlContents" runat="server" Visible="false">
                <style type="text/css" media="print">
                    @page
                    {
                        size: A3 portrait;
                        margin: 0.5cm;
                    }
                    @media print
                    {
                        .printclass
                        {
                            display: table;
                        }
                        thead
                        {
                            display: table-header-group;
                        }
                        tfoot
                        {
                            display: table-footer-group;
                        }
                        #header
                        {
                            position: fixed;
                            top: 0px;
                            left: 0px;
                        }
                        #footer
                        {
                            position: fixed;
                            bottom: 0px;
                            left: 0px;
                        }
                        #printable
                        {
                            position: relative;
                            bottom: 30px;
                            height: 300;
                        }
                    
                    }
                    @media screen
                    {
                        thead
                        {
                            display: block;
                        }
                        tfoot
                        {
                            display: block;
                        }
                    }
                </style>
                <div id="printable">
                    <table>
                        <thead>
                            <tr>
                                <th>
                                    <div style="margin: 0px; border: 0px;">
                                        <table class="printclass" style="width: 100%; font-weight: bold; font-family: Book Antiqua;
                                            font-size: medium; margin: 0px; margin-top: 20px;">
                                            <tr>
                                                <td rowspan="6" style="width: 80px; margin: 0px; border: 0px;">
                                                    <asp:Image ID="imgLeftLogo" runat="server" AlternateText="" ImageUrl="~/college/Left_Logo.jpeg"
                                                        Width="80px" Height="100px" Style="margin: 0px; border: 0px;" />
                                                </td>
                                                <td align="center">
                                                    <span id="spCollege" runat="server" style="font-size: 18px;"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="spAffBy" runat="server" style="font-size: 15px;"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="spController" runat="server" style="font-size: 15px;"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="spSeating" runat="server" style="font-size: 15px;"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <span id="spDateSession" runat="server" style="font-size: 14px; display: none;">
                                                    </span><span id="sprptnamedt" runat="server" style="font-size: 14px;"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" colspan="2">
                                                    Date: <span id="spdate" runat="server" style="font-size: 14px;"></span>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </th>
                            </tr>
                            <tr>
                                <td colspan="2" style="display: none;">
                                    <center>
                                        <div>
                                            <asp:Table ID="tblFormat2" runat="server" Style="width: 1417px; border-color: Black;
                                                text-align: center; border-bottom: 0px solid black; font-weight: bold; font-size: medium;
                                                border-style: solid; border-width: 1px;">
                                                <asp:TableRow ID="tblRow1" runat="server">
                                                    <asp:TableCell ID="tblcellsno" runat="server" Text="S.No" Width="30px"></asp:TableCell>
                                                    <asp:TableCell ID="tblcellInvName" runat="server" Text="Invigilator Name" Width="69px"></asp:TableCell>
                                                    <asp:TableCell ID="tblcellHallNo" runat="server" Text="Hall No" Width="65px"></asp:TableCell>
                                                    <asp:TableCell ID="tcInvSign" runat="server" Text="Initials of the Invigilator" Width="65px"></asp:TableCell>
                                                    <asp:TableCell ID="TableCell4" runat="server" Text="Degree/Branch" Width="105px"></asp:TableCell>
                                                    <asp:TableCell ID="TableCell6" runat="server" Text="Subject Code" Width="80px"></asp:TableCell>
                                                    <asp:TableCell ID="TableCell7" runat="server" Text="Reg. No of the Candidate" Width="380px"></asp:TableCell>
                                                    <asp:TableCell ID="TableCell8" runat="server" Text="Total No of Student" Width="70px"></asp:TableCell>
                                                    <asp:TableCell ID="tcBooletNo" runat="server" Text="Answer Booklet Numbers" Width="40px"></asp:TableCell>
                                                    <asp:TableCell ID="tcHallSuperend" runat="server" Text="Signature <br/>of the<br/> Hall <br/>Superintendents"
                                                        Width="40px"></asp:TableCell>
                                                    <asp:TableCell ID="TableCell11" runat="server" Text="Present" Width="55px"></asp:TableCell>
                                                    <asp:TableCell ID="TableCell12" runat="server" Text="Absent" Width="55px"></asp:TableCell>
                                                    <asp:TableCell ID="TableCell13" runat="server" Text="Initials<br/> of the<br/> Invigilator"
                                                        Width="65px"></asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                        </div>
                                    </center>
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:GridView ID="gridstud" Visible="false" runat="server" AutoGenerateColumns="true"
                                        GridLines="Both" CssClass="grid-view" HeaderStyle-BackColor="#0CA6CA" ForeColor="Black"
                                        Style="width: auto; overflow: auto;" OnRowDataBound="gridstud_OnRowDataBound">
                                        <%--OnDataBound="gridstud_OnDataBound"--%>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </asp:Panel>
        </center>
    </div>
    <%--column order--%>
    <%--Commented by saranya--%>
    <%-- <center>
        <div id="divcolorder" runat="server" style="height: 100%; display: none; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <asp:ImageButton ID="imgcolumn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: 83px; margin-left: 403px;" />
            <%--   <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
            <center>
                <div id="Div2" runat="server" class="table" style="background-color: White; height: 348px;
                    width: 850px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 90px;
                    border-radius: 10px;">
                    <center>
                        <table>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblcolr" runat="server" Text="Column Order" Style="font-family: Book Antiqua;
                                        font-size: 20px; font-weight: bold; color: Green;"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblrptype" Text="Report Type" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnAdd" runat="server" Text="+" CssClass="textbox textbox1 btn1" /><%--OnClick="btnAdd_OnClick"
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlreport" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlreport_SelectedIndexChanged"
                                                        CssClass="textbox textbox1 ddlheight4">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnDel" runat="server" Text="-" CssClass="textbox textbox1 btn1"
                                                        OnClick="btnDel_OnClick" />
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="cb_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" onchange="return columnOrderCbl()" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtcolorder" runat="server" Columns="20" Style="height: 70px; width: 800px;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" Width="800px"
                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                        RepeatDirection="Horizontal" onclick="return columnOrderCb()">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                    <center>
                                        <asp:Button ID="btncolorderOK" CssClass=" textbox btn1 comm" Style="height: 28px;
                                            width: 65px;" OnClick="btncolorderOK_Click" Text="OK" runat="server" />
                                        <%--   </center>
                                </td>
                                <td>
                                    <center>
                                        <asp:Button ID="btnclear" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                            Text="Clear" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
            <%-- </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </center>--%>
    <%--   Added by saranya on 10/07/2018--%>
    <center>
        <div id="poppernew" runat="server" visible="false" style="height: 355em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
            left: 0;">
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                width: 30px; position: absolute; margin-top: -4px; margin-left: 474px;" OnClick="imagebtnpopclose1_Click" />
            <br />
            <center>
                <div class="popsty" style="background-color: White; height: 690px; width: 974px;
                    border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;
                    margin-top: -8px">
                    <br />
                    <br />
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_columnordertype" Text="Report Type" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Button ID="btn_addtype" runat="server" Text="+" CssClass="textbox textbox1 btn1"
                                        OnClick="btn_addtype_OnClick" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_coltypeadd" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_coltypeadd_SelectedIndexChanged"
                                        CssClass="textbox textbox1 ddlheight4">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btn_deltype" runat="server" Text="-" CssClass="textbox textbox1 btn1"
                                        OnClick="btn_deltype_OnClick" />
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <fieldset style="border-radius: 10px; width: 900px; height: 500px">
                        <legend style="font-size: larger; font-weight: bold">ColumnOrder Header Settings</legend>
                        <table class="table">
                            <tr>
                                <td>
                                    <asp:ListBox ID="lb_selectcolumn" runat="server" SelectionMode="Multiple" Height="490px"
                                        Width="300px"></asp:ListBox>
                                </td>
                                <td>
                                    <table class="table1">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnMvOneRt" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                    Text=">" CssClass="textbox textbox1 btn1" OnClick="btnMvOneRt_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnMvTwoRt" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                    Text=">>" CssClass="textbox textbox1 btn1" OnClick="btnMvTwoRt_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnMvOneLt" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                    Text="<" CssClass="textbox textbox1 btn1" OnClick="btnMvOneLt_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnMvTwoLt" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                    Text="<<" CssClass="textbox textbox1 btn1" OnClick="btnMvTwoLt_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:ListBox ID="lb_column1" runat="server" SelectionMode="Multiple" Height="490px"
                                        Width="300px"></asp:ListBox>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label ID="lblalerterr" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                        <br />
                        <center>
                            <asp:Button ID="btnok" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                Text="OK" CssClass="textbox textbox1 btn2" OnClick="btnok_click" />
                            <asp:Button ID="btnclose" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                Text="Close" CssClass="textbox textbox1 btn2" OnClick="btnclose_click" />
                        </center>
                    </fieldset>
                </div>
            </center>
        </div>
    </center>
    <center>
        <div id="imgdiv33" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="panel_description11" runat="server" visible="false" class="table" style="background-color: White;
                    height: 120px; width: 467px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <table>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lbl_description111" runat="server" Text="Description" Font-Bold="true"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:TextBox ID="txt_description11" runat="server" Width="400px" Style="font-family: 'Book Antiqua';
                                    margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btn_adddesc1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="textbox btn1" OnClick="btndescpopadd_Click" />
                                <asp:Button ID="btn_exitdesc1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="textbox btn1" OnClick="btndescpopexit_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
        </div>
    </center>
    <div id="Div2" runat="server" visible="false" style="height: 300em; z-index: 1000;
        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
        left: 0px;">
        <center>
            <div id="Div6" runat="server" class="table" style="background-color: White; height: 120px;
                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 400px;
                border-radius: 10px;">
                <center>
                    <table style="height: 100px; width: 100%">
                        <tr>
                            <td align="center">
                                <asp:Label ID="LblAlertMsg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center>
                                    <asp:Button ID="Button2" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                        OnClick="btn_errorcloseAlert_Click" Text="ok" runat="server" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </center>
    </div>
    <%--report type name enter text box--%>
    <%-- <center>
        <div id="divaddtype" runat="server" style="height: 100%; z-index: 10000; width: 100%;
            background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;
            display: none;">
            <center>
                <div id="panel_description11" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 467px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <table>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lbldesc" runat="server" Text="Description" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:TextBox ID="txtdesc" runat="server" Width="400px" Style="font-family: 'Book Antiqua';
                                    margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnaddtype" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="textbox btn1" OnClick="btnaddtype_Click" />
                                <asp:Button ID="btnexittype" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="textbox btn1" /><%--OnClick="btnexittype_Click"
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
        </div>
    </center>--%>
    <div>
        <center>
            <asp:Panel ID="panel4" runat="server" Style="background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83);
                border-color: inherit; border-style: none; border-width: 1px; height: 100em;
                left: -8px; position: absolute; top: -9px; width: 101%; display: none;" BorderColor="Blue">
                <asp:Panel ID="panel6" runat="server" Visible="true" BackColor="mediumaquamarine"
                    Style="border-style: none; border-color: inherit; border-width: 1px; height: 590px;
                    width: 985px; left: 7px; top: 87px; position: absolute;" BorderColor="Blue">
                    <br />
                    <div class="panel6" id="Div1" style="text-align: center; font-family: Book Antiqua;
                        font-size: medium; font-weight: bold">
                        <caption style="top: 20px; border-style: solid; border-color: Black; position: absolute;
                            left: 200px">
                            <asp:Label ID="Label17" runat="server" Text="Student Details" Font-Bold="true" Font-Size="Large"
                                Font-Names="Book Antiqua"></asp:Label>
                        </caption>
                        <asp:Button ID="Button5" runat="server" Text="X" ForeColor="Black" OnClientClick="return hideclick()"
                            Style="top: 0px; left: 942px; position: absolute; height: 26px; border-width: 0;
                            background-color: mediumaquamarine; width: 25px;" Font-Bold="True" Font-Names="Microsoft Sans Serif"
                            Font-Size="Medium" />
                        <br />
                        <br />
                        <asp:Panel ID="panel3" runat="server" Visible="true" BackColor="Lavender" Height="500px"
                            ScrollBars="Vertical" Style="border-style: none; border-color: inherit; border-width: 1px;
                            width: 949px; left: 21px; position: absolute;" BorderColor="Blue">
                            <table align="right">
                                <tr align="right">
                                    <td align="right">
                                        <asp:Button ID="Button6" Visible="false" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Print" Width="50px" OnClick="Button6_Clcik" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <center>
                                <%-- <asp:UpdatePanel ID="UpdatePaneltd" runat="server">
                                    <ContentTemplate>--%>
                                <asp:CheckBox ID="cbpersonal" runat="server" Text="Personal Details" AutoPostBack="true"
                                    OnCheckedChanged="cbpersonal_Changed" Visible="true" Style="margin-left: -464px;" />
                                <br />
                                <asp:CheckBox ID="cbDegreeUpdate" runat="server" Text="Update Degree Details" AutoPostBack="true"
                                    OnCheckedChanged="cbDegreeUpdate_Changed" Visible="true" Style="margin-left: -419px;" />
                                <%-- </ContentTemplate>
                                </asp:UpdatePanel>--%>
                                <div id="coursedetails" runat="server" visible="false">
                                    <div>
                                        <asp:Image ID="stud_img" runat="server" Width="100px" Height="120px" Style="float: right;
                                            margin-right: 50px;" />
                                    </div>
                                    <table>
                                        <tr>
                                            <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                <span style="font-weight: bold; font-size: large;">Course Information</span>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>
                                                <span>Stream</span>
                                            </td>
                                            <td>
                                                <span id="college_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Graduation</span>
                                            </td>
                                            <td>
                                                <span id="degree_Span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Degree</span>
                                            </td>
                                            <td>
                                                <span id="graduation_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Choise I</span>
                                            </td>
                                            <td>
                                                <span id="course_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Choise II</span>
                                            </td>
                                            <td>
                                                <span id="course_span2" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: right;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                <span style="font-weight: bold; font-size: large;">Personal Information</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Applicant Name</span>
                                            </td>
                                            <td>
                                                <span id="applicantname_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Application Number</span>
                                            </td>
                                            <td>
                                                <span id="applicantno_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>
                                                <span>Last Name</span>
                                            </td>
                                            <td>
                                                <span id="lastname_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Date of birth</span>
                                            </td>
                                            <td>
                                                <span id="dob_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Gender</span>
                                            </td>
                                            <td>
                                                <span id="gender_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Parent Name</span>
                                            </td>
                                            <td>
                                                <span id="parent_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>
                                                <span>RelationShip</span>
                                            </td>
                                            <td>
                                                <span id="relationship_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Occupation</span>
                                            </td>
                                            <td>
                                                <span id="occupation_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Mother Tongue</span>
                                            </td>
                                            <td>
                                                <span id="mothertongue_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Religion</span>
                                            </td>
                                            <td>
                                                <span id="religion_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Nationality</span>
                                            </td>
                                            <td>
                                                <span id="nationality_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Community</span>
                                            </td>
                                            <td>
                                                <span id="commuity_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Caste</span>
                                            </td>
                                            <td>
                                                <span id="Caste_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Are You of Tamil Origin From Andaman and Nicobar Islands ?</span>
                                            </td>
                                            <td>
                                                <span id="tamilorigin_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Are You a Child of an ex-serviceman of Tamil Nadu origin ?</span>
                                            </td>
                                            <td>
                                                <span id="Ex_service_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Are You Differently abled</span>
                                            </td>
                                            <td>
                                                <span id="Differentlyable_Span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Are you first genaration learner ?</span>
                                            </td>
                                            <td>
                                                <span id="first_generation_Span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Is Residence on Campus Required ? </span>
                                            </td>
                                            <td>
                                                <span id="residancerequired_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Distinction in Sports </span>
                                            </td>
                                            <td>
                                                <span id="sport_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Extra Curricular Activites/Co-Curricular Activites </span>
                                            </td>
                                            <td>
                                                <span id="Co_Curricular_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Are you NCC cadet?</span>
                                            </td>
                                            <td>
                                                <span id="ncccadetspan" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                <span style="font-weight: bold; font-size: large;">Communication Address</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Address Line1</span>
                                            </td>
                                            <td>
                                                <span id="caddressline1_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Address Line2</span>
                                            </td>
                                            <td>
                                                <span id="Addressline2_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Address Line3</span>
                                            </td>
                                            <td>
                                                <span id="Addressline3_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>City</span>
                                            </td>
                                            <td>
                                                <span id="city_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>State</span>
                                            </td>
                                            <td>
                                                <span id="state_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Country</span>
                                            </td>
                                            <td>
                                                <span id="Country_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>PIN Code</span>
                                            </td>
                                            <td>
                                                <span id="Postelcode_Span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Mobile Number</span>
                                            </td>
                                            <td>
                                                <span id="Mobilenumber_Span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Alternate Mobile No</span>
                                            </td>
                                            <td>
                                                <span id="Alternatephone_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Email ID</span>
                                            </td>
                                            <td>
                                                <span id="emailid_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Phone Number with (Landline) STD/ISD code</span>
                                            </td>
                                            <td>
                                                <span id="std_ist_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                <span style="font-weight: bold; font-size: large;">Permanent Address</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Address Line1</span>
                                            </td>
                                            <td>
                                                <span id="paddressline1_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Address Line2</span>
                                            </td>
                                            <td>
                                                <span id="paddressline2_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Address Line3</span>
                                            </td>
                                            <td>
                                                <span id="paddressline3_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>City</span>
                                            </td>
                                            <td>
                                                <span id="pcity_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>State</span>
                                            </td>
                                            <td>
                                                <span id="pstate_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Country</span>
                                            </td>
                                            <td>
                                                <span id="pcountry_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>PIN Code</span>
                                            </td>
                                            <td>
                                                <span id="ppostelcode_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>
                                                <span>Mobile Number</span>
                                            </td>
                                            <td>
                                                <span id="pmobilenumber_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>
                                                <span>Alternate Mobile No</span>
                                            </td>
                                            <td>
                                                <span id="palternatephone_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>
                                                <span>Email ID</span>
                                            </td>
                                            <td>
                                                <span id="peamilid_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Phone Number with (Landline) STD/ISD code</span>
                                            </td>
                                            <td>
                                                <span id="pstdisd_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: right;">
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="Academicinfo" runat="server" visible="false">
                                    <div id="ugdiv_verification" runat="server" visible="false">
                                        <table>
                                            <tr>
                                                <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                    <span style="font-weight: bold; font-size: large;">Academic Information</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Qualifying Examination Pass</span>
                                                </td>
                                                <td>
                                                    <span id="qualifyingexam_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Name of School</span>
                                                </td>
                                                <td>
                                                    <span id="Nameofschool_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Location of School</span>
                                                </td>
                                                <td>
                                                    <span id="locationofschool_Span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Medium of Study of Qualifying Examination</span>
                                                </td>
                                                <td>
                                                    <span id="mediumofstudy_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Qualifying Board & State</span>
                                                </td>
                                                <td>
                                                    <span id="qualifyingboard_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Are you Vocational stream</span>
                                                </td>
                                                <td>
                                                    <span id="Vocationalspan" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Marks/Grade</span>
                                                </td>
                                                <td>
                                                    <span id="marksgrade_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>PCM Mark</span>
                                                </td>
                                                <td>
                                                    <span id="pcm_mark" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                            </tr>
                                        </table>
                                        <br />
                                        <asp:GridView ID="VerificationGridug" runat="server">
                                        </asp:GridView>
                                        <br />
                                    </div>
                                    <div id="pgdiv_verification" runat="server" visible="false">
                                        <table style="width: 600px;">
                                            <tr>
                                                <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                    <span style="font-weight: bold; font-size: large;">Academic Information</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Qualifying Examination Passed</span>
                                                </td>
                                                <td>
                                                    <span id="ugqualifyingexam_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Name of the College</span>
                                                </td>
                                                <td>
                                                    <span id="nameofcollege_Sapn" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Location of the College</span>
                                                </td>
                                                <td>
                                                    <span id="locationofcollege_sapn" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Mention Major</span>
                                                </td>
                                                <td>
                                                    <span id="major_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Type of Major</span>
                                                </td>
                                                <td>
                                                    <span id="typeofmajor_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Type of Semester</span>
                                                </td>
                                                <td>
                                                    <span id="typeofsemester_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Medium of Study at UG level</span>
                                                </td>
                                                <td>
                                                    <span id="mediumofstudy_spanug" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Marks/Grade</span>
                                                </td>
                                                <td>
                                                    <span id="marksorgradeug_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Registration No as Mentioned on your Mark Sheet </span>
                                                </td>
                                                <td>
                                                    <span id="reg_no_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="tnspan" runat="server" visible="false">TANCET Mark / Year of Pass </span>
                                                </td>
                                                <td>
                                                    <span id="Tancetspan" runat="server" visible="false"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Cut Off Mark</span>
                                                </td>
                                                <td>
                                                    <span id="cutoffmarkpg" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Pcm Mark</span>
                                                </td>
                                                <td>
                                                    <span id="pcmmarkval" runat="server"></span>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <asp:GridView ID="Verificationgridpg" runat="server">
                                        </asp:GridView>
                                        <br />
                                    </div>
                                    <div id="ugtotaldiv" runat="server" visible="false">
                                        <table style="width: 700px;">
                                            <tr>
                                                <td>
                                                    <span>Total Marks Obtained</span>
                                                </td>
                                                <td>
                                                    <span id="total_marks_secured" runat="server"></span>
                                                </td>
                                                <td style="display: none;">
                                                    <span>Maximum Marks</span>
                                                </td>
                                                <td style="display: none;">
                                                    <span id="maximum_marks" runat="server"></span>
                                                </td>
                                                <td style="display: none;">
                                                    <span>Percentage</span>
                                                </td>
                                                <td style="display: none;">
                                                    <span id="percentage_span" runat="server"></span>
                                                </td>
                                                <td>
                                                    <span>Cut Off Mark</span>
                                                </td>
                                                <td>
                                                    <span id="cutoffmark_span" runat="server"></span>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="pgtotaldiv" runat="server" visible="false">
                                        <table style="width: 890px;">
                                            <tr>
                                                <td>
                                                    <span>Total percentage of marks in all subjects (Language/major/Allied/Ancillary/Elective
                                                        inclusive ofTheory and Practical</span>
                                                </td>
                                                <td>
                                                    <span id="percentagemajorspan" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Total % of Marks in Major subjects alone (Including theory & Practicals)</span>
                                                </td>
                                                <td>
                                                    <span id="majorsubjectspan" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Total percentage marks in major/Allied/Ancillary subjects alone inclusive of Theory
                                                        and Practicals</span>
                                                </td>
                                                <td>
                                                    <span id="alliedmajorspan" runat="server"></span>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <br />
                                </div>
                                <div id="licet_print_div" style="width: 700px;" runat="server" visible="false">
                                    <div>
                                        <center>
                                            <table id="clgHeader_tbl" visible="false" runat="server" style="margin-top: 5px;">
                                                <tr>
                                                    <td align="left" rowspan="5" width="100">
                                                        <asp:Image ID="clglogoleft" runat="server" Width="90px" Height="100px" Style="float: right;" />
                                                    </td>
                                                    <td align="center">
                                                        <span runat="server" style="font-size: 16px; font-family: Times New Roman; font-weight: bold;
                                                            text-align: center;" id="collegename_span1"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <span runat="server" id="collegename_span2"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <span runat="server" id="collegename_span3"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <span runat="server" id="clgphfax_span4"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <span runat="server" id="clgemail_span5"></span><span runat="server" id="clsgwebsite_span6">
                                                        </span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </div>
                                    <div>
                                        <table style="width: 300px; float: left;">
                                            <tr>
                                                <td>
                                                    <span style="margin-top: 50px; font-weight: bold; height: 120px;">Application Number</span>
                                                </td>
                                                <td>
                                                    <span style="margin-top: 50px; font-weight: bold;" id="applicationno_span1" runat="server">
                                                    </span>
                                                </td>
                                            </tr>
                                        </table>
                                        <table style="width: 110px; float: right;">
                                            <tr>
                                                <td>
                                                    <asp:Image ID="stud_printimg" runat="server" Width="100px" Height="120px" Style="float: right;" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <table style="width: 690px;">
                                        <tr>
                                            <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                <span style="font-weight: bold; font-size: large;">Course Information</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Institution Name</span>
                                            </td>
                                            <td>
                                                <span id="Institution_Name_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Graduation</span>
                                            </td>
                                            <td>
                                                <span id="Graduation_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Degree</span>
                                            </td>
                                            <td>
                                                <span id="Degree_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Course I </span>
                                            </td>
                                            <td>
                                                <span id="CourseI" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="choiseIIT" runat="server">Course II</span>
                                            </td>
                                            <td>
                                                <span id="choiseII" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="Seattypet" runat="server">Seat Type</span>
                                            </td>
                                            <td>
                                                <span id="Seattypev" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="DateT" runat="server">Date of Enquiry</span>
                                            </td>
                                            <td>
                                                <span id="Datev" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="AdmDateT" runat="server">Date Of Admission</span>
                                            </td>
                                            <td>
                                                <span id="AdmDateV" runat="server"></span>
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="Div7">
                                        <div id="regular_div" runat="server" visible="false">
                                            <table style="width: 680px;">
                                                <tr>
                                                    <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                        <span style="font-weight: bold; font-size: large;">Academic Information</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Qualifying Examination Passed</span>
                                                    </td>
                                                    <td>
                                                        <span id="qualifyingexam_span1" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr style="display: none;">
                                                    <td>
                                                        <span>Name of the Board & State</span>
                                                    </td>
                                                    <td>
                                                        <span id="Nameofboard_span1" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Institution last Attended</span>
                                                    </td>
                                                    <td>
                                                        <span id="Nameofschool_span1" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Institution address/Contact No</span>
                                                    </td>
                                                    <td>
                                                        <span id="locationofschool_Span1" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr style="display: none;">
                                                    <td>
                                                        <span>Language Studied in X-std</span>
                                                    </td>
                                                    <td>
                                                        <span id="Span9" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Medium of Instruction</span><%--Language Studied in XII-std--%>
                                                    </td>
                                                    <td>
                                                        <span id="languagestudidespan" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr style="display: none;">
                                                    <td>
                                                        <span>Marks/Grade</span>
                                                    </td>
                                                    <td>
                                                        <span id="Span10" runat="server"></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="lateral_div" runat="server" visible="false">
                                            <table style="width: 680px;">
                                                <tr>
                                                    <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                        <span style="font-weight: bold; font-size: large;">Academic Information</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Qualifying Examination Passed</span>
                                                    </td>
                                                    <td>
                                                        <span id="qualifyingexam_span2" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Name of the college last studied</span>
                                                    </td>
                                                    <td>
                                                        <span id="Nameofschool_span2" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Location of the College</span>
                                                    </td>
                                                    <td>
                                                        <span id="collegelocation_span2" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Mention Major</span>
                                                    </td>
                                                    <td>
                                                        <span id="majorspan2" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr style="display: none;">
                                                    <td>
                                                        <span>Type of Major</span>
                                                    </td>
                                                    <td>
                                                        <span id="Span15" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Type of Semester</span>
                                                    </td>
                                                    <td>
                                                        <span id="typeofsemester" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Medium of Study at UG level</span>
                                                    </td>
                                                    <td>
                                                        <span id="mediumofstudyug" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Marks/Grade</span>
                                                    </td>
                                                    <td>
                                                        <span id="markorgrade_span" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Registration No. as Mentioned on your Mark Sheet </span>
                                                    </td>
                                                    <td>
                                                        <span id="reg_no_span2" runat="server"></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <br />
                                        <asp:GridView ID="VerificationGridug1" OnRowDataBound="verification_Databoud" runat="server">
                                        </asp:GridView>
                                        <table id="ugtotaltable" runat="server" visible="false" style="width: 680px;">
                                            <tr>
                                                <td>
                                                    <span>Total Marks Obtained</span> <span id="ug_totalmark_span1" runat="server"></span>
                                                </td>
                                                <td>
                                                </td>
                                                <td style="display: none;">
                                                    <span>Maximum Marks</span>
                                                </td>
                                                <td style="display: none;">
                                                    <span id="Span21" runat="server"></span>
                                                </td>
                                                <td style="display: none;">
                                                    <span>Percentage</span>
                                                </td>
                                                <td style="display: none;">
                                                    <span id="percentage" runat="server"></span>
                                                </td>
                                                <td>
                                                    <span>Cut Off Mark</span>
                                                </td>
                                                <td>
                                                    <span id="cutoffmark_span1" runat="server"></span>
                                                </td>
                                                 <td>
                                                    <span>Pcm Mark</span>
                                                </td>
                                                <td>
                                                    <span id="Pcmmarkvalue_span" runat="server"></span>
                                                </td>
                                            </tr>
                                        </table>
                                        <table id="pgtotaltable" runat="server" visible="false" style="width: 680px;">
                                            <tr>
                                                <td>
                                                    <span>Total % of Marks in All Subjects(Major/Allied/Ancillary/Elective/Practicals)</span>
                                                </td>
                                                <td>
                                                    <span id="All_totalspan" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Total % of Marks in Major subjects alone (Including theory & Practicals)</span>
                                                </td>
                                                <td>
                                                    <span id="Majortotalspan" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Total % of Marks in major & Allied/Ancillary subjects alone (Including theory
                                                        & Practicals)</span>
                                                </td>
                                                <td>
                                                    <span id="Majorpracticalspan" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="display: none;">
                                                    <span>Total Percentage Of Internal Mark</span>
                                                </td>
                                                <td>
                                                    <span id="Internalmarkspan" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="display: none;">
                                                    <span>Total Percentage Of External Mark</span>
                                                </td>
                                                <td>
                                                    <span id="externalmarkspan" runat="server"></span>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <table style="width: 680px;">
                                        <tr style="display: none;">
                                            <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                <span style="font-weight: bold; font-size: large;">Part-I Language</span>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>
                                                <span>Part-I Language</span>
                                            </td>
                                            <td>
                                                <span id="partIlanguagespan" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                <span style="font-weight: bold; font-size: large;">Personal Information</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Applicant's name</span><%--(In English)--%>
                                            </td>
                                            <td>
                                                <span id="Applicantname_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>
                                                <span>Last name(In English)</span>
                                            </td>
                                            <td>
                                                <span id="Span23" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>
                                                <span>Applicant's name(In Tamil)</span>
                                            </td>
                                            <td>
                                                <span id="applicatnfirstnametamilspan" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>
                                                <span>Last name(In Tamil)</span>
                                            </td>
                                            <td>
                                                <span id="applicantlastnametamilspan" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Date Of birth</span>
                                            </td>
                                            <td>
                                                <span id="dob_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Place Of Birth</span>
                                            </td>
                                            <td>
                                                <span id="placeofbirthspan" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Gender</span>
                                            </td>
                                            <td>
                                                <span id="sex_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>
                                                <span>Relationship</span>
                                            </td>
                                            <td>
                                                <span id="Span26" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Mother Tongue</span>
                                            </td>
                                            <td>
                                                <span id="mothertongue_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Religion</span>
                                            </td>
                                            <td>
                                                <span id="Religion_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Nationality</span>
                                            </td>
                                            <td>
                                                <span id="Nationality_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Community</span>
                                            </td>
                                            <td>
                                                <span id="Community_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Caste</span>
                                            </td>
                                            <td>
                                                <span id="Caste_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Blood Group</span>
                                            </td>
                                            <td>
                                                <span id="bloodgroupspan" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Aadhar Card No</span>
                                            </td>
                                            <td>
                                                <span id="Aadharspan" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>
                                                <span id="missionaryid" runat="server" visible="false">Are You a missionary child ?</span>
                                            </td>
                                            <td>
                                                <span id="spanmissionarychild" runat="server" visible="false"></span>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>
                                                <span>Are you of Tamil Origin From Andaman and Nicobar Islands ?</span>
                                            </td>
                                            <td>
                                                <span id="Span32" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>
                                                <span>Are you a Child of an Ex-serviceman ?</span>
                                            </td>
                                            <td>
                                                <span id="Span33" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>
                                                <span>Are you a Differently abled</span>
                                            </td>
                                            <td>
                                                <span id="Span34" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>
                                                <span>Are you a first genaration learner ?</span>
                                            </td>
                                            <td>
                                                <span id="Span35" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Is Hostel accommodation required? </span>
                                            </td>
                                            <td>
                                                <span id="ishostelreq_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>
                                                <span>Distinction in Sports </span>
                                            </td>
                                            <td>
                                                <span id="Span37" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Extra Curricular Activites/Co-Curricular Activites </span>
                                            </td>
                                            <td>
                                                <span id="extracurricular_span" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                <span style="font-weight: bold; font-size: large;">Particulars of The Parents/Guardian</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Father's name </span>
                                                <%--(In English)--%>
                                            </td>
                                            <td>
                                                <span id="fathername_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>
                                                <span>Father's name (In Tamil)</span>
                                            </td>
                                            <td>
                                                <span id="parentnametamilspan" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Occupation</span>
                                            </td>
                                            <td>
                                                <span id="foccup" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Annual Income</span>
                                            </td>
                                            <td>
                                                <span id="fannualincomespan" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Contact No</span>
                                            </td>
                                            <td>
                                                <span id="fathercontactnospan" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>E-mail ID</span>
                                            </td>
                                            <td>
                                                <span id="fatheremailidspan" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Name of the mother</span>
                                            </td>
                                            <td>
                                                <span id="mothernamespan" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Occupation</span>
                                            </td>
                                            <td>
                                                <span id="motheroccupationspan" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Annual Income</span>
                                            </td>
                                            <td>
                                                <span id="motherannualincomespan" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Contact No</span>
                                            </td>
                                            <td>
                                                <span id="mothercontactnospan" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>E-mail ID</span>
                                            </td>
                                            <td>
                                                <span id="motheremailspan" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Name of the guardian</span><%-- (if living with guardian)--%>
                                            </td>
                                            <td>
                                                <span id="guardiannamepspan" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Contact No</span>
                                            </td>
                                            <td>
                                                <span id="guardiancontactnospan" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>E-mail ID</span>
                                            </td>
                                            <td>
                                                <span id="guardinaemailspan" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                <span style="font-weight: bold; font-size: large;">Communication Address</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Address Line1</span>
                                            </td>
                                            <td>
                                                <span id="caddress1_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Address Line2</span>
                                            </td>
                                            <td>
                                                <span id="caddress2_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>
                                                <span>Address Line3</span>
                                            </td>
                                            <td>
                                                <span id="caddress3_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>City</span>
                                            </td>
                                            <td>
                                                <span id="ccity_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>State</span>
                                            </td>
                                            <td>
                                                <span id="cState_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Country</span>
                                            </td>
                                            <td>
                                                <span id="Country_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>PIN Code</span>
                                            </td>
                                            <td>
                                                <span id="Postelcode_Span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Mobile Number</span>
                                            </td>
                                            <td>
                                                <span id="Mobilenumber_Span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Alternate Mobile No</span>
                                            </td>
                                            <td>
                                                <span id="Alternatephone_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Email ID</span>
                                            </td>
                                            <td>
                                                <span id="emailid_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Phone Number with (Landline) STD/ISD code</span>
                                            </td>
                                            <td>
                                                <span id="std_ist_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Signature Of the Student</span>
                                            </td>
                                            <td>
                                                <span id="studsignature" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                <span style="font-weight: bold; font-size: large;">Permanent Address</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Address Line1</span>
                                            </td>
                                            <td>
                                                <span id="paddressline1_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Address Line2</span>
                                            </td>
                                            <td>
                                                <span id="paddressline2_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>
                                                <span>Address Line3</span>
                                            </td>
                                            <td>
                                                <span id="paddressline3_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>City</span>
                                            </td>
                                            <td>
                                                <span id="pcity_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>State</span>
                                            </td>
                                            <td>
                                                <span id="pstate_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Country</span>
                                            </td>
                                            <td>
                                                <span id="pcountry_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>PIN Code</span>
                                            </td>
                                            <td>
                                                <span id="ppostelcode_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Phone Number with (Landline) STD/ISD code</span>
                                            </td>
                                            <td>
                                                <span id="pstdisd_span1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                <span style="font-weight: bold; font-size: large;">Refered Details</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Refered By</span>
                                            </td>
                                            <td>
                                                <span id="Refer" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="Refernamet" runat="server"></span>
                                            </td>
                                            <td>
                                                <span id="Refernamev" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="Refercodet" runat="server"></span>
                                            </td>
                                            <td>
                                                <span id="Refercodev" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="departt" runat="server"></span>
                                            </td>
                                            <td>
                                                <span id="departv" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="colleget" runat="server"></span>
                                            </td>
                                            <td>
                                                <span id="collegev" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="contactt" runat="server"></span>
                                            </td>
                                            <td>
                                                <span id="Contactv" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="Signaturet" runat="server"></span>
                                            </td>
                                            <td>
                                                <span id="Signaturev" runat="server"></span>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </div>
                                <br />
                                <div id="certificate_detdownload" runat="server" style="height: auto; width: 650px;">
                                    <center>
                                        <asp:GridView ID="certificate_grid" runat="server" CaptionAlign="Top" HorizontalAlign="Justify"
                                            OnSelectedIndexChanged="certificate_grid_SelectedIndexChanged" CellPadding="4"
                                            ForeColor="#333333" GridLines="Vertical" Width="600px">
                                            <RowStyle BackColor="#E3EAEB" />
                                            <Columns>
                                                <asp:CommandField ShowSelectButton="True" SelectText="Download" ControlStyle-ForeColor="Blue" />
                                            </Columns>
                                            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="Gray" Font-Bold="True" ForeColor="White" />
                                            <EditRowStyle BackColor="#7C6F57" />
                                            <AlternatingRowStyle BackColor="White" />
                                        </asp:GridView>
                                        <br />
                                        <asp:GridView ID="communitity_grid" runat="server" CaptionAlign="Top" HorizontalAlign="Justify"
                                            OnSelectedIndexChanged="communitity_grid_SelectedIndexChanged" CellPadding="4"
                                            ForeColor="#333333" GridLines="Vertical" Width="600px">
                                            <RowStyle BackColor="#E3EAEB" />
                                            <Columns>
                                                <asp:CommandField ShowSelectButton="True" SelectText="Download" ControlStyle-ForeColor="Blue" />
                                            </Columns>
                                            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="Gray" Font-Bold="True" ForeColor="White" />
                                            <EditRowStyle BackColor="#7C6F57" />
                                            <AlternatingRowStyle BackColor="White" />
                                        </asp:GridView>
                                    </center>
                                    <br />
                                </div>
                                <br />
                                <div id="AddtionalInformationDiv" runat="server" visible="false">
                                    <center>
                                        <table>
                                            <tr>
                                                <td colspan="6" style="text-align: center; color: White; background-color: brown;">
                                                    <span style="font-weight: bold; font-size: large;">Admission Information</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Admission No
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_AdmissionNo" runat="server" CssClass="  textbox txtheight" Width="120px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    Admission Date
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_AdmissionDate" runat="server" CssClass="  textbox txtheight"
                                                        Width="120px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender5" runat="server" Format="dd/MM/yyyy" TargetControlID="txt_AdmissionDate" />
                                                </td>
                                                <td rowspan="2">
                                                    <asp:Image ID="StudentImage" runat="server" Height="100px" Width="100px" />
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="cbCounselling" runat="server" AutoPostBack="true" OnCheckedChanged="cbCounselling_CheckedChange"
                                                        Checked="false" />
                                                    Counselling No
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCounsellingNo" runat="server" CssClass="  textbox txtheight"
                                                        Width="120px" MaxLength="20" Enabled="false"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="feCouns" runat="server" TargetControlID="txtCounsellingNo"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                                <td>
                                                    Counselling Date
                                                </td>
                                                <td>
                                                    <div style="position: relative;">
                                                        <asp:TextBox ID="txtCounsellingDt" runat="server" CssClass="  textbox txtheight"
                                                            Width="120px" Enabled="false"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender6" runat="server" Format="dd/MM/yyyy" TargetControlID="txtCounsellingDt"
                                                            PopupPosition="BottomRight" />
                                                    </div>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Roll No &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:CheckBox ID="cbSame" runat="server" AutoPostBack="true" OnCheckedChanged="cbSame_CheckedChange" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_rollno" runat="server" CssClass="  textbox txtheight" Width="120px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    Student Type
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlAdmissionStudType" runat="server" CssClass="ddlheight ddlheight1"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlAdmissionStudType_IndexChange"
                                                        Width="120px">
                                                        <asp:ListItem>Day Scholar</asp:ListItem>
                                                        <asp:ListItem>Hostler</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="linkbtn" runat="server" Text="Add Student Photo" OnClick="Link_Photo"></asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnUpdateInformation" runat="server" OnClick="btnUpdateInformation_Click"
                                                        Text="Update" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <div>
                                                        <table id="transport_div" runat="server">
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="rbldayScTrans" runat="server" RepeatDirection="Horizontal"
                                                                        CssClass="ddlheight ddlheight4" AutoPostBack="true" OnSelectedIndexChanged="rbldayScTrans_IndexChange">
                                                                        <asp:ListItem Value="0">Own Transport</asp:ListItem>
                                                                        <asp:ListItem Value="1">Institution Transport</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblBoardPnt" runat="server" Text="Boarding"></asp:Label>
                                                                    <asp:DropDownList ID="ddl_boarding" runat="server" CssClass="textbox1 ddlheight5">
                                                                    </asp:DropDownList>
                                                                    <%-- <asp:TextBox ID="txtBoardPnt" runat="server" Style="float: left;" CssClass="textbox txtheight5"
                                                        MaxLength="50" Visible="false"></asp:TextBox>
                                                                   <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtBoardPnt"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                                     <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="getboard" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtBoardPnt"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>--%>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox runat="server" ID="cb_IncDayscMess" Text="Include Day Scholar Mess Fees"
                                                                        Checked="false" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="Hostel_div" runat="server">
                                                            <tr>
                                                                <td>
                                                                    Hostel Name
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlHosHostel" runat="server" CssClass="ddlheight ddlheight2"
                                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlHosHostel_IndexChange" Width="120px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    Room Type
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddl_roomtype" runat="server" CssClass="textbox1 ddlheight4"
                                                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_roomtype_selectedindexchanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    Room Name
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlHosRoom" runat="server" CssClass="ddlheight ddlheight2"
                                                                        Width="120px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Mode
                                                </td>
                                                <td colspan="2">
                                                    <asp:DropDownList ID="rblModeDet" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Value="1" Selected="True">Regular</asp:ListItem>
                                                        <asp:ListItem Value="2">Transfer</asp:ListItem>
                                                        <asp:ListItem Value="3">Lateral</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    Batch :
                                                    <asp:Label ID="lblBatchDet" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    Current Semester :
                                                    <asp:Label ID="lblCurSemDet" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lbRcpt" runat="server" PostBackUrl="~/ChallanReceipt.aspx" Text="Go To Receipt"
                                                        Width="200px"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <div id="photo_div" runat="server" visible="false" class="popupstyle popupheight1"
                                            style="position: fixed; width: 100%; z-index: 1000; height: 100%;">
                                            <center>
                                                <div id="Div18" runat="server" class="table" style="background-color: White; height: 300px;
                                                    width: 500px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 110px;
                                                    border-radius: 10px;">
                                                    <br />
                                                    <center>
                                                        <asp:Label ID="Label14" runat="server" Text="Add Student Photo" Style="color: Red;"
                                                            Font-Bold="true" Font-Size="large"></asp:Label>
                                                    </center>
                                                    <center>
                                                        <br />
                                                        <br />
                                                        <table style="margin-left: 75px;">
                                                            <tr>
                                                                <td>
                                                                    <asp:FileUpload ID="fileuploadbrowse" Style="font-weight: bold; font-family: book antiqua;
                                                                        font-size: medium; background-color: #6699ee; border-radius: 6px;" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btn_photoupload" Text="UpLoad" runat="server" OnClick="btn_photoupload_OnClick"
                                                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee;
                                                                        border-radius: 6px;" />
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:Button ID="btn_uploadclose" Text="Close" runat="server" OnClick="btn_uploadclose_OnClick"
                                                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee;
                                                                        border-radius: 6px; margin-left: 0px;" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </center>
                                                </div>
                                            </center>
                                        </div>
                                    </center>
                                </div>
                                <div id="divDegreeDetails" runat="server" style="margin-top: 20px; height: auto;
                                    margin-bottom: 10px; position: relative;" visible="false">
                                    <center>
                                        <div id="lblhdrDeg" style="font-weight: bold; font-size: large; color: White; width: 900px;
                                            background-color: brown;">
                                            Update Degree Details</div>
                                    </center>
                                    <br />
                                    <table>
                                        <tr>
                                            <td colspan="4" style="text-align: center; color: Green; font-weight: bold; font-family: Book Antiqua;">
                                                <span>Degree Information</span>
                                            </td>
                                            <td colspan="4" style="text-align: center; color: Green; font-weight: bold; font-family: Book Antiqua;">
                                                <span>Update Degree Information</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Batch Year
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_OldBatch" runat="server" Enabled="false" CssClass="  textbox txtheight"
                                                    Width="120px"></asp:TextBox>
                                            </td>
                                            <td>
                                                Degree
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_OldDegree" runat="server" Enabled="false" CssClass="  textbox txtheight"
                                                    Width="120px"></asp:TextBox>
                                            </td>
                                            <td>
                                                New Batch Year
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="upd_ddlBatch" runat="server" CssClass="textbox ddlheight3"
                                                    Width="120px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                New Degree
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="upd_ddlDegree" runat="server" CssClass="  textbox ddlheight3"
                                                    Width="120px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Seat Type
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_OldSeattype" runat="server" Enabled="false" CssClass="  textbox txtheight"
                                                    Width="120px"></asp:TextBox>
                                            </td>
                                            <td>
                                                Application No
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_OldApplNo" runat="server" Enabled="false" CssClass="  textbox txtheight"
                                                    Width="120px"></asp:TextBox>
                                            </td>
                                            <td>
                                                New Seat Type
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="upd_ddlSeatType" runat="server" CssClass="  textbox ddlheight3"
                                                    Width="120px">
                                                </asp:DropDownList>
                                            </td>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chkFeesUpd" runat="server" Checked="true" Text="Fees Update" />
                                                <asp:Button ID="btnUpdDegDet" runat="server" Text="Update" OnClick="btnUpdDegDet_Click"
                                                    Style="height: 25px; width: 70px; border: 1px solid green; border-radius: 5px;
                                                    text-decoration: none; font-size: 14px; font-weight: bold;" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="margin-top: 20px; margin-bottom: 10px; position: relative;">
                                    <center>
                                        <%-- <span style="font-size: 18px; font-weight: bold; color: Green;"><u>Student Fee Update</u><span
                                        style="padding-left: 200px;">--%>
                                        <asp:Label ID="lblstudmsg" runat="server" Visible="false" Text=" Student Fee Update"
                                            Style="font-size: 18px; font-weight: bold; color: Green; text-decoration: underline;"></asp:Label>
                                        <asp:Button ID="btnFeeUpdate" runat="server" Text="Update" Visible="false" OnClick="btnFeeUpdate_Click"
                                            Style="height: 25px; width: 70px; border: 1px solid green; border-radius: 5px;
                                            text-decoration: none; font-size: 14px; font-weight: bold;" />
                                        <asp:CheckBox ID="ShowAllCb" runat="server" Visible="false" AutoPostBack="true" OnCheckedChanged="ShowAllCb_CheckedChange"
                                            Text="Show All Ledger" />
                                        <%--</span></span>--%>
                                    </center>
                                </div>
                                <div id="divstuddt" runat="server" visible="false" style="width: 490px; height: 450px;
                                    overflow: auto;">
                                    <asp:Label ID="lblAppnoFee" runat="server" Text="" Visible="false"></asp:Label>
                                    <asp:GridView ID="gridFeeDet" runat="server" Width="460px" AutoGenerateColumns="false"
                                        GridLines="Both" OnRowDataBound="gridFeeDet_OnRowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAdmFeeSno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ledger" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAdmLedger" runat="server" Text='<%#Eval("AdmLedger") %>'></asp:Label>
                                                    <asp:Label ID="lblAdmLedgerId" runat="server" Text='<%#Eval("AdmLedgerId") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblAdmHeaderId" runat="server" Text='<%#Eval("AdmHeaderId") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblAdmPaymode" runat="server" Text='<%#Eval("AdmPaymode") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblAdmDedRes" runat="server" Text='<%#Eval("AdmDedRes") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblAdmFine" runat="server" Text='<%#Eval("AdmFine") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblAdmRefund" runat="server" Text='<%#Eval("AdmRefund") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Alloted" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtAdmFeeAllot" runat="server" CssClass="  textbox txtheight" Width="80px"
                                                        Text='<%#Eval("FeeAlloted") %>' Height="15px" Style="text-align: right;" onchange="return checktotalamount1();"></asp:TextBox><asp:FilteredTextBoxExtender
                                                            ID="FilteredtxtAdmledge1Amt" runat="server" TargetControlID="txtAdmFeeAllot"
                                                            FilterType="Numbers,custom" ValidChars=".">
                                                        </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Concession" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtAdmDeduc" runat="server" CssClass="  textbox txtheight" Width="80px"
                                                        Text='<%#Eval("Deduction") %>' Height="15px" Style="text-align: right;" onchange="return checktotalamount1();"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredtxtAdmledge2Amt" runat="server" TargetControlID="txtAdmDeduc"
                                                        FilterType="Numbers,custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAdmFeeTotal" runat="server" Text='<%#Eval("TotalAmt") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <table id="trtotal" runat="server" visible="false">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblallot" runat="server" Text="Total Allot"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblaltamt" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblconse" runat="server" Text="Total Consession"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblconsamt" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbltot" runat="server" Text="Total Amount"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbltotamt" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </center>
                            <br />
                            <br />
                            <br />
                        </asp:Panel>
                    </div>
                </asp:Panel>
            </asp:Panel>
        </center>
    </div>
    <center>
        <div id="imgdiv2" runat="server" visible="false" style="height: 50em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: -140px;
            left: 0px;">
            <div id="pnl2" runat="server" class="panel6" style="background-color: white; height: 650px;
                width: 800px; margin-top: 200px; border: 1px solid #008B8B; border-top: 25px solid #008B8B;
                border-radius: 10px;">
                <center>
                    <table style="height: 150px; width: 800px;">
                        <tr>
                            <td colspan="2">
                                <fieldset>
                                    <span id="spstudDet" runat="server" visible="false" style="color: Green; font-size: larger;">
                                    </span>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkIsColDegChange" runat="server" Text="" ForeColor="Red" AutoPostBack="true"
                                                    OnCheckedChanged="chkIsColDegChange_CheckChange" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblColChangeDeg" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Width="55px"></asp:Label>
                                                <asp:DropDownList ID="ddlColChangeDeg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Width="330px" AutoPostBack="True" OnSelectedIndexChanged="ddlColChangeDeg_SelectedIndexchange">
                                                </asp:DropDownList>
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="lblBatChangeDeg" runat="server" Text="Batch :" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Width="55px"></asp:Label>
                                                <%--</td>
                                            <td>--%>
                                                <asp:DropDownList ID="ddlBatChangeDeg" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Height="27px" runat="server" CssClass=" textbox1 txtheight">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblEdulevChangeDeg" runat="server" Text="Edu Level" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                <asp:DropDownList ID="ddlEdulevChangeDeg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Width="70px" AutoPostBack="True" OnSelectedIndexChanged="ddlEdulevChangeDeg_SelectedIndexchange">
                                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDegChangeDeg" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Width="100px"></asp:Label>
                                                <asp:DropDownList ID="ddlDegChangeDeg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Width="78px" AutoPostBack="True" OnSelectedIndexChanged="ddlDegChangeDeg_SelectedIndexchange">
                                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDeptChangeDeg" runat="server" Text="Dept" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                                <asp:DropDownList ID="ddlDeptChangeDeg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Width="160px">
                                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                </asp:DropDownList>
                                                <%--AutoPostBack="true" OnSelectedIndexChanged="ddldept_Change"--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_comm" Text="Community" runat="server" Font-Names="Book Antiqua"></asp:Label>
                                            </td>
                                            <%--<td>
                                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_comm" runat="server" CssClass="textbox textbox1 txtheight3"
                                                            Width="90px" ReadOnly="true">-- Select--</asp:TextBox>
                                                        <asp:Panel ID="Panel8" runat="server" BackColor="White" BorderColor="Black" Width="90px"
                                                            BorderStyle="Solid" BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                                            <asp:CheckBox ID="cb_comm" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_comm_checkedchange" />
                                                            <asp:CheckBoxList ID="cbl_comm" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_comm_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_comm"
                                                            PopupControlID="Panel8" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>--%>
                                            <td>
                                                <asp:DropDownList ID="ddl_community" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Width="78px" AutoPostBack="True">
                                                    <%--OnSelectedIndexChanged="ddl_community_SelectedIndexchange"--%>
                                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_alert" runat="server" Text="Seat Type" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_seattype" runat="server" CssClass="ddlheight3 textbox textbox1"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddl_seattype_IndexChange">
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="2" id="tdconces" runat="server" visible="false">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblcons" runat="server" Text="Concession" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlconces" runat="server" CssClass="ddlheight3 textbox textbox1">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButtonList ID="rbtype" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="Amount" Value="0" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Percentage" Value="1"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" Font-Bold="true" />
                                            <asp:Button ID="btn_popclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btn_popclose_Click" Text="Cancel" runat="server" Font-Bold="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <center>
                                    <asp:Label ID="txt_gridAdmLedgeTot" runat="server" Visible="false" ForeColor="Red"
                                        Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </center>
                                <div style="width: 490px; height: 450px; overflow: auto;">
                                    <asp:GridView ID="gridAdmLedge" runat="server" Width="460px" AutoGenerateColumns="false"
                                        GridLines="Both" OnRowDataBound="gridAdmLedge_OnRowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Ledger" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAdmLedger" runat="server" Text='<%#Eval("AdmLedger") %>'></asp:Label>
                                                    <asp:Label ID="lblAdmLedgerId" runat="server" Text='<%#Eval("AdmLedgerId") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblAdmHeaderId" runat="server" Text='<%#Eval("AdmHeaderId") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblAdmPaymode" runat="server" Text='<%#Eval("AdmPaymode") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblAdmDedRes" runat="server" Text='<%#Eval("AdmDedRes") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblAdmFine" runat="server" Text='<%#Eval("AdmFine") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblAdmRefund" runat="server" Text='<%#Eval("AdmRefund") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Alloted" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtAdmFeeAllot" runat="server" CssClass="  textbox txtheight" Width="80px"
                                                        Text='<%#Eval("FeeAlloted") %>' Height="15px" Style="text-align: right;" onchange="return checktotalamount();"></asp:TextBox><asp:FilteredTextBoxExtender
                                                            ID="FilteredtxtAdmledge1Amt" runat="server" TargetControlID="txtAdmFeeAllot"
                                                            FilterType="Numbers,custom" ValidChars=".">
                                                        </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Concession" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtAdmDeduc" runat="server" CssClass="  textbox txtheight" Width="80px"
                                                        Text='<%#Eval("Deduction") %>' Height="15px" Style="text-align: right;" onchange="return checktotalamount();"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredtxtAdmledge2Amt" runat="server" TargetControlID="txtAdmDeduc"
                                                        FilterType="Numbers,custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAdmFeeTotal" runat="server" Text='<%#Eval("TotalAmt") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <div runat="server" id="remark_div" visible="false">
                                        <span style="margin-top: -2px; float: left;">Remark</span>
                                        <asp:TextBox ID="txt_remarks" runat="server" TextMode="MultiLine" Height="40px" Width="400px"></asp:TextBox>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td>
                                <asp:CheckBox ID="cbAdmLedgeFee" runat="server" Text="Use Ledger Amount" AutoPostBack="true"
                                    OnCheckedChanged="cbAdmLedgeFee_Change" Checked="false" />
                            </td>
                            <td>
                                <asp:Button ID="btnfeesave" runat="server" Text="Save" Width="44px" Style="border: 1px solid indigo;"
                                    OnClick="btnfeesave_Click" />
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td colspan="2">
                                Ledger1
                                <asp:DropDownList ID="ddlAdmLedge1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAdmLedge1_IndexChanged"
                                    CssClass="ddlheight3 textbox textbox1" Width="120px">
                                </asp:DropDownList>
                                Amount
                                <asp:TextBox ID="txtAdmledge1Amt" runat="server" CssClass="txtheight textbox textbox1"
                                    Width="70px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredtxtAdmledge1Amt" runat="server" TargetControlID="txtAdmledge1Amt"
                                    FilterType="Numbers,custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td colspan="2">
                                Ledger2
                                <asp:DropDownList ID="ddlAdmLedge2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAdmLedge2_IndexChanged"
                                    CssClass="ddlheight3 textbox textbox1" Width="120px">
                                </asp:DropDownList>
                                Amount
                                <asp:TextBox ID="txtAdmledge2Amt" runat="server" CssClass="txtheight textbox textbox1"
                                    Width="70px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredtxtAdmledge2Amt" runat="server" TargetControlID="txtAdmledge2Amt"
                                    FilterType="Numbers,custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </div>
        <%--sms div--%>
        <div id="popSendSms" runat="server" style="height: 100em; z-index: 1000; width: 100%;
            position: absolute; top: 0; left: 0; display: none;">
            <center>
                <div style="background-color: #FFFFFF; height: 300px; margin-top: 180px; width: 500px;">
                    <br />
                    <table>
                        <tr>
                            <td style="color: Green; text-align: center; font-size: 20px; font-weight: bold;">
                                Send SMS
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Type your message here :
                                <br />
                                <asp:TextBox ID="txt_SmsMsgPop" runat="server" Width="400px" TextMode="MultiLine"
                                    Rows="10" Placeholder="New Message"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnSendSmsPop" runat="server" Style="border: 1px solid orange; border-radius: 5px;
                                    width: 150px; height: 30px;" Text="Send SMS" OnClick="btnSendSmsPop_click" />
                                <asp:Button ID="btnClosePop" runat="server" Style="border: 1px solid orange; border-radius: 5px;
                                    width: 60px; height: 30px;" Text="Exit" OnClick="btnClosePop_click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
        </div>
    </center>
    <%--call letter--%>
    <div id="Div3" runat="server" visible="false" style="height: 100em; z-index: 1000;
        width: 100%; position: absolute; top: 0; left: 0;">
        <center>
            <div style="background-color: #FFFFFF; height: 260px; margin-top: 180px; width: 350px;">
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label10" runat="server" Text="Date of Preparation" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPrepDate" runat="server" Width="90px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd/MM/yyyy" TargetControlID="txtPrepDate" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Interview Date" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_callDate" runat="server" Width="90px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy" TargetControlID="Txt_callDate" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label13" runat="server" Text="Interview Time" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlIntHr" runat="server" Width="40px">
                                <asp:ListItem Value="1" Selected="True">01</asp:ListItem>
                                <asp:ListItem Value="2">02</asp:ListItem>
                                <asp:ListItem Value="3">03</asp:ListItem>
                                <asp:ListItem Value="4">04</asp:ListItem>
                                <asp:ListItem Value="5">05</asp:ListItem>
                                <asp:ListItem Value="6">06</asp:ListItem>
                                <asp:ListItem Value="7">07</asp:ListItem>
                                <asp:ListItem Value="8">08</asp:ListItem>
                                <asp:ListItem Value="9">09</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlIntMin" runat="server" Width="40px">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlIntMed" runat="server" Width="40px">
                                <asp:ListItem Value="AM" Selected="true">AM</asp:ListItem>
                                <asp:ListItem Value="PM">PM</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label11" runat="server" Text="Venue" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVenue" runat="server" Width="120px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label12" runat="server" Text="DD Amount" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtddAmount" runat="server" Width="120px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" MaxLength="10"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="ftTxt" runat="server" FilterType="Numbers,Custom"
                                ValidChars="." TargetControlID="txtddAmount">
                            </asp:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Button ID="Button7" runat="server" Style="background-color: rgb(0, 128, 128);
                    border: 0px; color: White;" Text="Okay" OnClick="Okay_clcik" OnClientClick="return InterviewOk(this);" />
                <asp:Button ID="Button8" runat="server" Style="background-color: rgb(0, 128, 128);
                    border: 0px; color: White;" Text="Cancel" OnClick="Cancel_clcik" />
                <%-- <button style="background-color: rgb(0, 128, 128); border: 0px; color: White;">
                    Okay</button>--%>
            </div>
        </center>
    </div>
    <%--student reject--%>
    <asp:Panel ID="panel2" runat="server" Style="background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83);
        border-color: inherit; border-style: none; border-width: 1px; height: 100em;
        left: -8px; position: absolute; top: 33px; width: 101%; display: none;" BorderColor="Blue">
        <center>
            <asp:Panel ID="panel9" runat="server" Visible="false" BackColor="mediumaquamarine"
                Style="border-style: none; border-color: inherit; border-width: 1px; height: 376px;
                width: 928px; left: 47px; top: 74px; position: absolute;" BorderColor="Blue">
                <div class="panel6" id="Div4" style="text-align: center; font-family: Book Antiqua;
                    font-size: medium; font-weight: bold">
                    <asp:Button ID="Button10" runat="server" Text="X" ForeColor="Black" OnClick="btnreason_click"
                        Style="top: 0px; left: 897px; position: absolute; height: 26px; border-width: 0;
                        background-color: mediumaquamarine; width: 25px;" Font-Bold="True" Font-Names="Microsoft Sans Serif"
                        Font-Size="Medium" />
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="Reason For Admission" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Style="top: 80px; position: absolute;
                                    left: 62px;"></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="Button1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="+" Width="29px" Style="top: 77px; position: absolute;
                                    left: 225px; height: 27px; display: none;" OnClick="btnadd_Click" />
                                <asp:DropDownList ID="ddlreason" runat="server" Style="top: 78px; position: absolute;
                                    left: 254px;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="616px">
                                </asp:DropDownList>
                                <asp:Button ID="btnminus" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="-" Width="29px" Style="top: 77px; position: absolute;
                                    left: 870px; height: 27px; display: none;" OnClick="btnminus_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="Button11" runat="server" Text="Admit" OnClick="btnadmitstud_Click"
                                    Style="top: 198px; left: 397px; position: absolute; height: 31px; width: 88px"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" margin-left="6px" />
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Panel ID="Panel10" runat="server" Visible="false" Style="top: 104px; border-color: Black;
                    background-color: lightyellow; border-style: solid; border-width: 0.5px; left: 9px;
                    position: absolute; width: 912px; height: 75px;">
                    <asp:TextBox ID="txtadd" runat="server" Width="886px" Style="font-family: 'Book Antiqua';
                        top: 14px; position: absolute; margin-left: 9px" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtadd"
                        FilterType="LowercaseLetters, UppercaseLetters,Custom" ValidChars="  " />
                    <asp:Button ID="btnadd1" runat="server" Text="Add" OnClick="btnadd1_Click" Style="top: 46px;
                        left: 330px; position: absolute; height: 26px; width: 88px" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" margin-left="6px" />
                    <asp:Button ID="btnexit1" runat="server" Text="Exit" OnClick="btnexit1_Click" Style="top: 46px;
                        left: 419px; position: absolute; height: 26px; width: 88px" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="panel11" runat="server" Visible="false" BackColor="mediumaquamarine"
                Style="border-style: none; border-color: inherit; border-width: 1px; height: 376px;
                width: 928px; margin-top: 74px;" BorderColor="Blue">
                <div class="panel6" id="Div5" style="text-align: center; font-family: Book Antiqua;
                    font-size: medium; font-weight: bold">
                    <asp:Button ID="Button12" runat="server" Text="X" ForeColor="Black" OnClick="btnrjt_click"
                        Style="margin-left: 897px; height: 26px; border-width: 0; background-color: mediumaquamarine;
                        width: 25px;" Font-Bold="True" Font-Names="Microsoft Sans Serif" Font-Size="Medium" />
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label62" runat="server" Text="Reason For Reject" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Style=""></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="Button13" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="+" Width="29px" Style="height: 27px; display: none;"
                                    OnClick="btnaddrejt_Click" />
                            </td>
                            <td>
                                <asp:DropDownList ID="btnrejectreason" runat="server" Style="" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="616px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="Button14" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="-" Width="29px" Style="height: 27px; display: none;"
                                    OnClick="btnminusrejt_Click" />
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="Button15" runat="server" Text="Reject" OnClick="btnrct_Click" Style="margin-left: 397px;
                                    height: 31px; width: 88px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Panel ID="Panel12" runat="server" Visible="false" Style="border-color: Black;
                    background-color: lightyellow; border-style: solid; border-width: 0.5px; width: 912px;
                    height: 75px;">
                    <asp:TextBox ID="TextBox1" runat="server" Width="886px" Style="font-family: 'Book Antiqua';
                        margin-top: 14px; margin-left: 10px;" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtadd"
                        FilterType="LowercaseLetters, UppercaseLetters,Custom" ValidChars="  " />
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="Button16" runat="server" Text="Add" OnClick="btnadd1ret_Click" Style="height: 26px;
                                    width: 88px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" margin-left="6px" />
                                <asp:Button ID="Button17" runat="server" Text="Exit" OnClick="btnexit1rejt_Click"
                                    Style="height: 26px; width: 88px" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
        </center>
    </asp:Panel>
    <%-- New Print div--%>
    <div style="height: 1px; width: 1px; overflow: auto;">
        <div id="contentDiv" runat="server" style="height: 710px; width: 1344px;" visible="false">
        </div>
    </div>
</asp:Content>
