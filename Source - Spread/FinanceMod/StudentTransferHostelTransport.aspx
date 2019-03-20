<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="StudentTransferHostelTransport.aspx.cs" Inherits="StudentTransferHostelTransport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .printclass
        {
            display: none;
        }
        /* body
        {
            background-image: url('images/money.jpg');
            background-repeat: no-repeat;
            background-attachment: fixed;
        }
        img
        {
            opacity: 0.5;
            filter: alpha(opacity=50); 
        }*/
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
            font-size: 16px;
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
    <body>
        <script type="text/javascript">
            function PrintPanel() {
                var panel = document.getElementById("<%=pnlContents.ClientID %>");
                var printWindow = window.open('', '', 'height=842,width=1191');
                printWindow.document.write('<html');
                printWindow.document.write('<head><title>Attendance Report</title>');
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
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Student Transfer Hostel and Transport</span></div>
            </center>
        </div>
        <div>
            <center>
                <div id="maindiv" runat="server" class="maindivstyle" style="width: auto; height: auto">
                    <div>
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:RadioButtonList ID="rbmode" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                        OnSelectedIndexChanged="rbmode_Selected">
                                        <asp:ListItem Text="Entry" Value="0" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Report" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td id="tdentry" runat="server" visible="false">
                                    <fieldset style="height: 15px; width: auto;">
                                        <asp:RadioButtonList ID="rbremove" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                            OnSelectedIndexChanged="rbremove_Selected">
                                            <asp:ListItem Text="Transport" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Hostel" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="diventry" runat="server" visible="false">
                            <table>
                                <tr>
                                    <td colspan="2">
                                        <div id="div1" style="width: 500px; float: left;">
                                            <center>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList ID="rbl_rollno" runat="server" CssClass="textbox  ddlheight" AutoPostBack="true"
                                                                OnSelectedIndexChanged="rbl_rollno_OnSelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_roll" runat="server" CssClass="textbox txtheight4 textbox1"
                                                                OnTextChanged="txt_roll_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="ftext_rollno" runat="server" TargetControlID="txt_roll"
                                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:AutoCompleteExtender ID="acext_rollno" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_roll"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="panelbackground">
                                                            </asp:AutoCompleteExtender>
                                                            <asp:TextBox ID="txt_reg" runat="server" Style="display: none" CssClass="textbox txtheight4 textbox1"
                                                                onchange="return checkrno(this.value)" onkeyup="return checkrno(this.value)"
                                                                onblur="return get(this.value)" onfocus="return myFunction(this)"></asp:TextBox>
                                                            <span style="color: Red;">*</span> <span id="rnomsg"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Name
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_name" runat="server" CssClass="textbox txtheight6 textbox1"
                                                                onblur="getname(this.value)"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="ftext_name" runat="server" TargetControlID="txt_name"
                                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:AutoCompleteExtender ID="acext_name" runat="server" DelimiterCharacters="" Enabled="True"
                                                                ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                                CompletionSetCount="10" ServicePath="" TargetControlID="txt_name" CompletionListCssClass="autocomplete_completionListElement"
                                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                                            </asp:AutoCompleteExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Date
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="Updp_date" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txt_date" runat="server" CssClass="textbox txtheight2 textbox2"
                                                                        Width="100px"></asp:TextBox>
                                                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_date" runat="server"
                                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                                    </asp:CalendarExtender>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </center>
                                        </div>
                                    </td>
                                    <td>
                                        <div id="div2" style="width: auto; display: none;">
                                            <asp:Image ID="image2" runat="server" ToolTip="Student's Photo" ImageUrl="" Style="height: 110px;
                                                width: 100px;" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%--student detail from  div--%>
                                        <div style="width: 450px;">
                                            <fieldset style="height: 232px; width: 370px; border: 1px solid #999999;">
                                                <legend>Student Details</legend>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_colg" runat="server" CssClass="txtheight5 txtcaps">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_str1" runat="server" Text="Stream"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_strm" runat="server" CssClass="txtheight3 txtcaps">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Batch
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_batch" runat="server" CssClass="txtheight txtcaps">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_degree" runat="server" CssClass="txtheight txtcaps">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_dept" runat="server" CssClass="txtheight5 txtcaps">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblsem" runat="server" Text="Semester"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_sem" runat="server" CssClass="txtheight txtcaps">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Section
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_sec" runat="server" CssClass="txtheight txtcaps">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <%--Seat Type--%>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_seattype" Visible="false" runat="server" CssClass="txtheight txtcaps">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </div>
                                    </td>
                                    <td>
                                        <%--change to another section--%>
                                        <div>
                                            <fieldset id="Fieldset1" runat="server" style="height: 232px; width: 370px; border: 1px solid #999999;">
                                                <%--<legend>To</legend>--%>
                                                <table>
                                                    <tr>
                                                        <%-- <td>
                                                        <asp:RadioButtonList ID="rbremove" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                                            OnSelectedIndexChanged="rbremove_Selected">
                                                            <asp:ListItem Text="Transport" Value="0" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Hostel" Value="1"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>--%>
                                                        <td>
                                                            <asp:Label ID="lblfeecat" runat="server" Text="Feecategory"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlfeecat" Width="150px" runat="server" CssClass="textbox  ddlheight"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlfeecat_OnSelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label1" runat="server" Text="Header"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlhed" Width="150px" runat="server" CssClass="textbox  ddlheight">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label2" runat="server" Text="Ledger"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlled" Width="150px" runat="server" CssClass="textbox  ddlheight">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Paid Amount
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtpaidamt" Enabled="false" runat="server" CssClass="txtheight txtcaps"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtpaidamt"
                                                                FilterType="numbers,custom" ValidChars=" .">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Enter The Excess Amount
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtexamt" runat="server" CssClass="txtheight txtcaps"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexamt"
                                                                FilterType="numbers,custom" ValidChars=" .">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <br />
                                        <asp:Button ID="btntransfer" runat="server" CssClass="textbox btn btn2" Text="Save"
                                            OnClick="btntransfer_Click" Style="font-family: Book Antiqua; font-size: medium;" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divreport" runat="server">
                            <table class="maintablestyle">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblrptclg" runat="server" Text="College"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlrptclg" Width="300px" runat="server" CssClass="textbox  ddlheight"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlrptclg_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="Type"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddltype" Width="100px" runat="server" CssClass="textbox  ddlheight">
                                        </asp:DropDownList>
                                        <%--AutoPostBack="true" OnSelectedIndexChanged="ddlrptclg_OnSelectedIndexChanged"--%>
                                    </td>
                                    <td colspan="2">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_fromdate" runat="server" Text="From"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_fromdate" runat="server" Style="height: 20px; width: 75px;"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_fromdate" runat="server"
                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_todate" runat="server" Text="To"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_todate" runat="server" Style="height: 20px; width: 75px;"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_todate" runat="server"
                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnrptgo" runat="server" CssClass="textbox btn btn2" Text="Go" OnClick="btnrptgo_Click"
                                            Style="font-family: Book Antiqua; font-size: medium; width: 60px;" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <%-- <div id="divgrid" runat="server">
                            <asp:GridView ID="gdrpt" runat="server" AutoGenerateColumns="true" GridLines="Both"
                                CssClass="grid-view" BackColor="WhiteSmoke" OnRowDataBound="gdrpt_OnRowDataBound" >
                            </asp:GridView>
                        </div>--%>
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
                                                            font-size: medium; margin-top: 20px;">
                                                            <tr>
                                                                <td rowspan="5" style="width: 80px; margin: 0px; border: 0px;">
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
                                                                <td align="center">
                                                                    <span id="spDateSession" runat="server" style="font-size: 14px;"></span>
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
                                                            <%--<table id="tblNew" runat="server" style="width: 1417px; border-color: Black; text-align: center; border-bottom: 0px solid black;
                                        font-weight: bold; font-size: medium; border-style: solid; border-width: 1px;">
                                        <tr>
                                            <td style="width: 30px;">
                                                S.No
                                            </td>
                                            <td style="width: 69px;">
                                                Invigilator Name
                                            </td>
                                            <td style="width: 65px;">
                                                Hall No
                                            </td>
                                            <td style="width: 65px;">
                                                Initials of the Invigilator
                                            </td>
                                            <td style="width: 105px;">
                                                Degree/Branch
                                            </td>
                                            <td style="width: 80px;">
                                                Subject Code
                                            </td>
                                            <td style="width: 380px;">
                                                Reg. No of the Candidate
                                            </td>
                                            <td style="width: 70px;">
                                                Total No of Student
                                            </td>
                                            <td id="tcBooletNo1" runat="server" style="width: 70px;">
                                                   Answer Booklet Numbers
                                                </td>
                                                <td id="tcHallSuperend1" runat="server" style="width: 70px;">
                                                    Signature of the Hall Superintendents
                                                </td>
                                            <td  style="width: 55px;">
                                                Present
                                            </td>
                                            <td style="width: 55px;">
                                                Absent
                                            </td>
                                            <td style="width: 65px;">
                                                Initials of the Invigilator
                                            </td>
                                        </tr>
                                    </table>--%>
                                                        </div>
                                                    </center>
                                                </td>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <%-- <FarPoint:FpSpread ID="Fspread3" Visible="false" runat="server">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" AllowPage="false" GridLineColor="Black">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>--%>
                                                    <%-- <div id="divgrid" runat="server" visible="true" style="width: auto; height: auto;
                                            overflow: auto; border-radius: 10px;">--%>
                                                    <asp:GridView ID="gdrpt" runat="server" AutoGenerateColumns="true" GridLines="Both"
                                                        CssClass="grid-view" BackColor="WhiteSmoke" OnRowDataBound="gdrpt_OnRowDataBound">
                                                    </asp:GridView>
                                                    <%--</div>--%>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </asp:Panel>
                            <br />
                            <br />
                            <asp:Button ID="btnExport" runat="server" Text="Export To PDF" Visible="false" OnClientClick=" return PrintPanel()"
                                CssClass="textbox btn btn2" Style="font-family: Book Antiqua; font-size: medium;
                                width: 132px;" />
                        </div>
                    </div>
                </div>
            </center>
        </div>
        <center>
            <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
    </body>
    </html>
</asp:Content>
