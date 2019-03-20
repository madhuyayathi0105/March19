<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="Book_Reservation.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="LibraryMod_Book_Reservation" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .txtcaps
        {
            border: 1px solid #c4c4c4;
            padding: 4px 4px 4px 4px;
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            box-shadow: 0px 0px 8px #d9d9d9;
            -moz-box-shadow: 0px 0px 8px #d9d9d9;
            -webkit-box-shadow: 0px 0px 8px #d9d9d9;
        }
        .maindivstylesize
        {
            height: 1300px;
            width: 1000px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">
        function valid1() {
            var idval = "";
            var empty = "";
            var id = "";
            var value1 = "";
            id = document.getElementById("<%=txt_roll.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_roll.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txt_title.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_title.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            if (empty.trim() != "") {
                return false;
            }
            else {
                return true;
            }
        }
    function myFunction(x) {
            x.style.borderColor = "#c4c4c4";
        }
        <%--Student tab time--%>
        setInterval(function () {
            document.getElementById("<%=txt_time.ClientID %>").value = (new Date()).toLocaleTimeString();
            return false;
        }, 1000);

          function SelLedgers() {
            var chkSelAll = document.getElementById("<%=chkGridSelectAll.ClientID %>");
            var tbl = document.getElementById("<%=grdBkReserve.ClientID %>");
            var gridViewControls = tbl.getElementsByTagName("input");

            for (var i = 1; i < (tbl.rows.length); i++) {
                var chkSelectid = document.getElementById('MainContent_grdBkReserve_selectchk_' + i.toString());

                if (chkSelAll.checked == false) {
                    chkSelectid.checked = false;
                } else {
                    chkSelectid.checked = true;
                }
            }

        }
    </script>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Reservation</span></div>
        </center>
    </div>
    <center>
        <div style="width: 1000px; height: auto">
            <table class="maintablestyle" style="height: auto; font-family: Book Antiqua; font-weight: bold;
                margin-left: -105px; margin-top: 10px; margin-bottom: 10px; padding: 6px;">
                <tr>
                    <td>
                        <asp:Label ID="lblclg" runat="server" Text="College">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="updatepanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Width="130px" AutoPostBack="True" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="updatepanel5" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lbllibrary" runat="server" Text="Library">
                                </asp:Label>
                                <asp:DropDownList ID="ddllibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Width="130px" AutoPostBack="True" OnSelectedIndexChanged="ddllibrary_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblsearch" runat="server" Text="SearchBy">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsearch" runat="server" CssClass="textbox ddlstyle ddlheight3"
                            Width="130px" AutoPostBack="True" OnSelectedIndexChanged="ddlSearchby_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:TextBox ID="text_tile" runat="server" Style="width: 115px;" CssClass="textbox txtheight2"
                            Visible="false"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="Getsearch" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="text_tile"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="panelbackground">
                        </asp:AutoCompleteExtender>
                        <asp:TextBox ID="text_Date" runat="server" Visible="false" Width="80px" CssClass="textbox txtheight2"
                            onfocus="return myFunction(this)"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_date" runat="server"
                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <asp:DropDownList ID="ddl_serach_Wise" runat="server" CssClass="textbox ddlstyle ddlheight3"
                            Width="130px" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddl_serach_Wise_SelectedIndexChanged"
                            Visible="false">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="upGo" runat="server">
                            <ContentTemplate>
                                <asp:ImageButton ID="Btngo" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="btngo_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnadd" runat="server" ImageUrl="~/LibImages/Add.jpg" OnClick="btnadd_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <br />
    <br />
    <center>
        <div>
            <asp:UpdatePanel ID="updatepanel7" runat="server">
                <ContentTemplate>
                    <span style="padding-right: 100px; margin-left: -260px; margin-top: 3px;">
                        <asp:CheckBox ID="chkGridSelectAll" runat="server" Text="SelectAll" Visible="false"
                            onchange="return SelLedgers();" Style="margin-left: -280px;" />
                    </span>
                    <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                    <asp:GridView ID="grdBkReserve" runat="server" ShowFooter="false" ShowHeader="false"
                        AutoGenerateColumns="true" Font-Names="book antiqua" togeneratecolumns="true"
                        OnSelectedIndexChanged="grdBkReserve_onselectedindexchanged" OnRowCreated="grdBkReserve_OnRowCreated"
                        OnRowDataBound="grdBkReserve_RowDataBound" Width="980px">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="allchk" runat="server" Text="Select All" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="selectchk" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </center>
    <br />
    <br />
    <center>
        <asp:UpdatePanel ID="UpDel" runat="server">
            <ContentTemplate>
                <asp:Button ID="btn_delete" Width="85px" runat="server" CssClass="textbox btn2" Text="Delete"
                    Visible="false" OnClick="btn_delete_Click" />
                <asp:Button ID="btn_cancel_res" Width="120px" runat="server" CssClass="textbox btn2"
                    Visible="false" Text="Cancel Reservation" OnClick="btn_cancel_res_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <br />
    <br />
    <center>
        <asp:UpdatePanel ID="updatepanel3" runat="server">
            <ContentTemplate>
                <div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                        Visible="false"></asp:Label>
                    <asp:Label ID="lblrptname" runat="server" Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" onkeypress="display()"
                        Font-Size="Medium" CssClass="textbox txtheight2"></asp:TextBox>
                    <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox textbox1 btn2"
                        Text="Export To Excel" Width="127px" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        Width="60px" CssClass="textbox textbox1 btn2" />
                    <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExcel" />
                <asp:PostBackTrigger ControlID="btnprintmaster" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
    <center>
        <div id="AddpopupReserve" runat="server" visible="false" class="popupstyle popupheight">
            <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: 20px; margin-left: 200px;"
                OnClick="imagebtnpopclose_Click" />
            <br />
            <div style="background-color: White; height: 473px; font-family: Book Antiqua; font-weight: bold;
                width: 458px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                <br />
                <center>
                    <asp:Label ID="lbl_studentadd1" runat="server" Text="Advance Reservation" class="fontstyleheader"
                        Style="color: Green;"></asp:Label>
                </center>
                <table>
                    <tr>
                        <td colspan="2">
                            <asp:UpdatePanel ID="updatepanel9" runat="server">
                                <ContentTemplate>
                                    <fieldset style="width: 215px; height: 15px;">
                                        <asp:RadioButtonList ID="rblstustaff" runat="server" RepeatDirection="Horizontal"
                                            AutoPostBack="true" OnSelectedIndexChanged="rblstustaff_Selected">
                                            <asp:ListItem Text="Student" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Staff" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </fieldset>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_lib" runat="server" Text="Library:"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updatepanel6" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddl_txt_lib" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="221px" Height="30px" AutoPostBack="True">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <%--<asp:UpdatePanel ID="updatepanel9" runat="server">
                            <ContentTemplate>--%>
                        <td>
                            <asp:Label ID="lbl_date" Text="Date:" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_date" runat="server" Width="80px" CssClass="textbox txtheight2"
                                onfocus="return myFunction(this)"></asp:TextBox>
                            <asp:CalendarExtender ID="caldate" TargetControlID="txt_date" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                        </td>
                        <%-- </ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_time" runat="server" Style="top: 10px; left: 6px;" Text="Time:"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updatepanel10" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_time" runat="server" CssClass="txtcaps txtheight" AutoPostBack="true">
                                    </asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="updatepanel22" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="lbl_userentry" runat="server" Text="User Entry:"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td colspan='1'>
                            <asp:UpdatePanel ID="updatepanel11" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddl_userentry" Width="228px" Height="30px" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddl_userentry_SelectedIndexChanged" CssClass="textbox txtheight2"
                                        Style="float: left;">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="updatepanel23" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="lbl_roll" runat="server" Text="Roll No:"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updatepanel12" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_roll" runat="server" Height="20px" CssClass="textbox txtheight2"
                                        Width="185px" AutoPostBack="true"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_roll"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Button ID="btn_libid" runat="server" Text="?" CssClass="textbox btn" OnClick="btn_libid_Click"
                                        Enabled="true" />
                                    <span style="color: Red;">*</span>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_name" runat="server" Text="Name:"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updatepanel19" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_name" runat="server" Style="width: 221px;" CssClass="textbox txtheight2"
                                        Enabled="false"></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_accno" runat="server" Text="Access No:"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updatepanel20" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_accno" runat="server" Height="20px" CssClass="textbox txtheight2"
                                        Width="185px" AutoPostBack="true" OnTextChanged="txt_accno_OnTextChanged"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_accno"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Button ID="btn_accno" runat="server" Text="?" CssClass="textbox btn" OnClick="btn_accno_Click" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_title" runat="server" Text="Title:"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updatepanel21" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_title" runat="server" Style="width: 221px;" CssClass="textbox txtheight2"
                                        Enabled="false"></asp:TextBox>
                                    <span style="color: Red;">*</span>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <asp:UpdatePanel ID="UpSave" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btn_save" runat="server" CssClass="textbox btn2" Text="Save" OnClick="btn_save_Click"
                            OnClientClick="return valid1()" />
                        <asp:Button ID="btn_exit" runat="server" CssClass="textbox btn2" Text="Exit" OnClick="btn_exit_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </center>
    <center>
        <asp:UpdatePanel ID="updatepanel13" runat="server">
            <ContentTemplate>
                <div id="popupselectlibid" runat="server" visible="false" class="popupstyle popupheight1">
                    <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 27px; margin-left: 434px;"
                        OnClick="imagebtnpopclose1_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; height: 600px; width: 900px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <asp:Label ID="lbl_popupselectlibid" runat="server" Text="Select Library ID" class="fontstyleheader"
                                Style="color: Green;"></asp:Label>
                        </center>
                        <br />
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="Lblbatch" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" Font-Size="Medium"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" Font-Size="Medium"
                                        AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="LblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="160px" AutoPostBack="true" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="LblSem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblSec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Sec">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" Font-Bold="True"
                                        Visible="true" Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Black">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <asp:Label ID="lbl_lib_id" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Library Id">
                                    </asp:Label>
                                    <asp:TextBox ID="tx_libid" runat="server" Style="width: 137px; margin-left: 23px"
                                        CssClass="textbox txtheight2"></asp:TextBox>
                                    <asp:Label ID="lb_name" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Name">
                                    </asp:Label>
                                    <asp:TextBox ID="tx_libname" runat="server" Style="width: 154px; margin-left: 3px"
                                        CssClass="textbox txtheight2"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upgo_libid" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btn_go_libid" Text="Go" OnClick="btn_go_libid_Click" CssClass="textbox btn1"
                                                runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="divRollNo" runat="server" runat="server" style="height: 400px; overflow: auto;
                            width: 800px;">
                            <asp:HiddenField ID="SelectedGridCell" runat="server" Value="-1" />
                            <asp:GridView ID="grdStudent" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                Font-Names="book antiqua" togeneratecolumns="true" OnSelectedIndexChanged="grdStudent_onselectedindexchanged"
                                OnRowCreated="grdStudent_OnRowCreated" Width="840px">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0ca6ca" ForeColor="Black" />
                            </asp:GridView>
                        </div>
                        <br />
                        <center>
                            <asp:UpdatePanel ID="UpStdOk" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btn_std_exit1" runat="server" CssClass="textbox btn2" Text="Exit"
                                        OnClick="btn_std_exit1_Click" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </center>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="updatepanel14" runat="server">
            <ContentTemplate>
                <div id="popupselectBook" runat="server" visible="false" class="popupstyle popupheight1">
                    <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 27px; margin-left: 434px;"
                        OnClick="imagebtnpopclose2_Click" />
                    <br />
                    <div style="background-color: White; height: 580px; width: 900px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <asp:Label ID="Label2" runat="server" Text="Select Book" class="fontstyleheader"
                                Style="color: Green;"></asp:Label>
                        </center>
                        <br />
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lb_search" runat="server" Text="Search" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="dd_search" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" Font-Size="Medium"
                                        AutoPostBack="true" OnSelectedIndexChanged="dd_search_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_search_book" runat="server" Style="width: 154px; margin-left: 3px"
                                        CssClass="textbox txtheight2" Visible="false"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getsearchvalue" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_search_book"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpGoBook" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btn_go_book" Text="Go" OnClick="btn_go_book_Click" CssClass="textbox btn1"
                                                runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="grdBook_book" runat="server" style="width: 800px; height: 400px; overflow: auto;">
                            <asp:HiddenField ID="HiddenFieldgrdBook" runat="server" Value="-1" />
                            <asp:GridView ID="grdBook" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                Font-Names="book antiqua" togeneratecolumns="true" OnSelectedIndexChanged="grdBook_onselectedindexchanged"
                                Width="1067px" OnRowCreated="grdBook_OnRowCreated">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_sno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="<%#Container.DataItemIndex+1 %>" Visible="true">
                                            </asp:Label></center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                            </asp:GridView>
                        </div>
                        <br />
                        <center>
                            <div>
                                <asp:UpdatePanel ID="UpAcc_ok" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btn_Acc_exit1" runat="server" CssClass="textbox btn2" Text="Exit"
                                            Visible="false" OnClick="btn_Acc_exit1_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </center>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <div id="DivpopupStaff" runat="server" visible="false" class="popupstyle popupheight1">
                    <asp:ImageButton ID="ImageButton4" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 27px; margin-left: 374px;"
                        OnClick="imagebtnpopclose4_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; height: 600px; width: 800px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <asp:Label ID="lbl_staff" runat="server" Text="Select Staff" class="fontstyleheader"
                                Style="color: Green;"></asp:Label>
                        </center>
                        <br />
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_staffdept" runat="server" Text="Department" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_staffdept" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="160px" AutoPostBack="true" OnSelectedIndexChanged="ddl_staffdept_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_staffname" runat="server" Text="Staff Name" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_staffname" runat="server" Style="width: 154px; margin-left: 3px"
                                        CssClass="textbox txtheight2"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upstaff_Go" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btn_staff_Go" Text="Go" OnClick="btn_staff_Go_Click" CssClass="textbox btn1"
                                                runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="divGrdstaff" runat="server" style="height: 400px; width: 700px; overflow: auto;">
                            <asp:HiddenField ID="HiddenFieldgrdStaff" runat="server" Value="-1" />
                            <asp:GridView ID="grdStaff" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                Font-Names="book antiqua" togeneratecolumns="true" Width="650px" OnRowCreated="grdStaff_OnRowCreated"
                                OnSelectedIndexChanged="grdStaff_onselectedindexchanged">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_sno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="<%#Container.DataItemIndex+1 %>" Visible="true">
                                            </asp:Label></center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                            </asp:GridView>
                        </div>
                        <br />
                        <center>
                            <div>
                                <asp:UpdatePanel ID="Upstaff_ok" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btn_staff_exit1" runat="server" CssClass="textbox btn2" Text="Exit"
                                            OnClick="btn_staff_exit1_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </center>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="updatepanel15" runat="server">
            <ContentTemplate>
                <div id="DivTrackMaster" runat="server" visible="false" class="popupstyle popupheight1">
                    <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 27px; margin-left: 434px;"
                        OnClick="imagebtnpopclose3_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; height: 580px; width: 900px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <asp:Label ID="Label3" runat="server" Text="Track Master" class="fontstyleheader"
                                Style="color: Green;"></asp:Label>
                        </center>
                        <br />
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_tracklib" runat="server" Text="Library" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_tracklib" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" Font-Size="Medium"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_tracklib_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rbltrack" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                        OnSelectedIndexChanged="rbltrack_Selected">
                                        <asp:ListItem Text="Reserved" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Issued"></asp:ListItem>
                                        <asp:ListItem Text="Not Issued Yet"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <%--  <div>
                    <asp:Label ID="lbl_errormsg1" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                </div>
                <p style="width: 691px;" align="right">
                    <asp:Label ID="lbl_count" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                </p>
                <div>
                    <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" AutoPostBack="true"
                        BorderWidth="0px" Style="overflow: auto; height: 300px; border: 0px solid #999999;
                        border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <br />
                <center>
                    <div>
                        <asp:Button ID="btn_ok" runat="server" CssClass="textbox btn2" Text="Ok" OnClick="btn_ok_Click" />
                        <asp:Button ID="btn_exit1" runat="server" CssClass="textbox btn2" Text="Exit" OnClick="btn_exit1_Click" />
                    </div>
                </center>--%>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="updatepanel16" runat="server">
            <ContentTemplate>
                <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <br />
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                    OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <div>
        <center>
            <asp:UpdatePanel ID="updatepanel17" runat="server">
                <ContentTemplate>
                    <div id="Diveleterecord" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="Div5" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lbl_Diveleterecord" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:UpdatePanel ID="Updetele_yes" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Button ID="btn_detele_yes__record" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                                width: 65px;" OnClick="btn_detele_yes__record_Click" Text="yes" runat="server" />
                                                            <asp:Button ID="btn_detele_no__record" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                                width: 65px;" OnClick="btn_detele_no__recordClick" Text="no" runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
    </div>
    <center>
        <asp:UpdatePanel ID="updatepanel18" runat="server">
            <ContentTemplate>
                <div id="DivAlertcancel" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <br />
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="Labelalerterror" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnerrclosecancel" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                    width: 65px;" OnClick="btnerrclosecancel_Click" Text="Ok" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--Progress bar for  go--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upGo">
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
    <%--Progress bar for  UpDel--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpDel">
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
    <%--Progress bar for  UpSave--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpSave">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="UpdateProgress3"
            PopupControlID="UpdateProgress3">
        </asp:ModalPopupExtender>
    </center>
    <%--Progress bar for  UpSave--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Upgo_libid">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="UpdateProgress4"
            PopupControlID="UpdateProgress4">
        </asp:ModalPopupExtender>
    </center>
    <%--Progress bar for  UpStdOk--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="UpStdOk">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender5" runat="server" TargetControlID="UpdateProgress5"
            PopupControlID="UpdateProgress5">
        </asp:ModalPopupExtender>
    </center>
    <%--Progress bar for  UpGoBook--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="UpGoBook">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender6" runat="server" TargetControlID="UpdateProgress6"
            PopupControlID="UpdateProgress6">
        </asp:ModalPopupExtender>
    </center>
    <%--Progress bar for  UpAcc_ok--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="UpAcc_ok">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender7" runat="server" TargetControlID="UpdateProgress7"
            PopupControlID="UpdateProgress7">
        </asp:ModalPopupExtender>
    </center>
    <%--Progress bar for  Upstaff_ok--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="Upstaff_ok">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender8" runat="server" TargetControlID="UpdateProgress8"
            PopupControlID="UpdateProgress8">
        </asp:ModalPopupExtender>
    </center>
    <%--Progress bar for  Updetele_yes--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress9" runat="server" AssociatedUpdatePanelID="Updetele_yes">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender9" runat="server" TargetControlID="UpdateProgress9"
            PopupControlID="UpdateProgress9">
        </asp:ModalPopupExtender>
    </center>
    <%--Progress bar for  Upstaff_Go--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress10" runat="server" AssociatedUpdatePanelID="Upstaff_Go">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender10" runat="server" TargetControlID="UpdateProgress10"
            PopupControlID="UpdateProgress10">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
