<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="TransactionReport.aspx.cs" Inherits="LibraryMod_TransactionReport" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">


        function SelLedgers() {
            var chkSelAll = document.getElementById("<%=chkGridSelectAll.ClientID %>");
            var tbl = document.getElementById("<%=gridview2.ClientID %>");
            var gridViewControls = tbl.getElementsByTagName("input");

            for (var i = 1; i < (tbl.rows.length - 1); i++) {
                var chkSelectid = document.getElementById('MainContent_gridview2_selectchk_' + i.toString());

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
                <span class="fontstyleheader" style="color: Green;">Transaction Report</span></div>
        </center>
    </div>
    <div>
        <asp:UpdatePanel ID="updatepanel5" runat="server">
            <ContentTemplate>
                <center>
                    <div id="maindiv" runat="server" class="maindivstyle" style="font-family: Book Antiqua;
                        font-weight: bold; width: 1000px; height: auto">
                        <div>
                            <table class="maintablestyle" style="height: auto; margin-left: 0px; margin-top: 10px;
                                margin-bottom: 10px; padding: 6px; font-family: Book Antiqua; font-weight: bold">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCollege" runat="server" Text="College">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="1">
                                        <asp:Label ID="lblreporttype" runat="server" Text="Report Type">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlreporttype" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddlreporttype_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbllibrary" runat="server" Text="Library">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddllibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddllibrary_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldept" runat="server" Text="Department">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddldept" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Enabled="false" Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddldept_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtdept" runat="server" Visible="false" CssClass="textbox txtheight2"
                                                    ReadOnly="true" Width="100px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                <asp:Panel ID="Paneldept" runat="server" Visible="false" Width="280px" CssClass="multxtpanel multxtpanleheight">
                                                    <asp:CheckBox ID="chkdept" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnCheckedChanged="chkdept_CheckedChanged" Text="Select All"
                                                        AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="chklstdept" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                        OnSelectedIndexChanged="chklstdept_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtdept"
                                                    PopupControlID="Paneldept" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblsubject" runat="server" Text="Subject" Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblrackno" runat="server" Text="Rack No" Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblselectfor" runat="server" Text="Select for ">
                                        </asp:Label>
                                        <asp:Label ID="lbltype" runat="server" Text="Type" Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lbldepttype" runat="server" Text="Type" Visible="false">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsubject" runat="server" Visible="false" CssClass="textbox ddlstyle ddlheight3"
                                            Width="200px" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlrackno" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="100px" Visible="false" AutoPostBack="True" OnSelectedIndexChanged="ddlrackno_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlselectfor" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="70px" AutoPostBack="True" OnSelectedIndexChanged="ddlselectfor_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:CheckBox ID="cbresignedstaff" runat="server" Visible="false" AutoPostBack="true"
                                            Text="Resigned Staff" />
                                        <asp:DropDownList ID="ddltype" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="100px" Visible="false" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddldepttype" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="100px" Visible="false" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblrollno" runat="server" Text="Roll No:"></asp:Label>
                                        <asp:Label ID="lblaccno" runat="server" Text="Acc No" Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblshelfno" runat="server" Text="Shelf No" Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lbldays" runat="server" Text="Days" Visible="false">
                                        </asp:Label>
                                        <asp:CheckBox ID="cbduplicateaccno" runat="server" Visible="false" AutoPostBack="true"
                                            Text="Duplicate AccNo" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlshelfno" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="100px" Visible="false" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txt_rolllno" runat="server" CssClass="textbox txtheight2" Style="width: 80px;">
                                        </asp:TextBox>
                                        <asp:Button ID="enqbtn" runat="server" Text="?" Visible="false" Height="20px" OnClick="enqbtn_Click"
                                            Width="20px" />
                                        <asp:DropDownList ID="ddlaccno" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="70px" Visible="false" AutoPostBack="True" OnSelectedIndexChanged="ddlaccno_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txt_accno" runat="server" Visible="false" CssClass="textbox txtheight2"
                                            Style="width: 30px;">
                                        </asp:TextBox>
                                        <asp:TextBox ID="txt_accno2" runat="server" Visible="false" CssClass="textbox txtheight2"
                                            Style="width: 30px;">
                                        </asp:TextBox>
                                        <asp:CheckBox ID="cbmissingaccno" runat="server" Visible="false" AutoPostBack="true"
                                            Text="Missing AccNo" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblstatus1" runat="server" Text="Status" Visible="false"></asp:Label>
                                        <asp:Label ID="lblname" runat="server" Text="Name" Visible="false"></asp:Label>
                                        <asp:CheckBox ID="cbaccessno" runat="server" Text="AccessNo" OnCheckedChanged="cbaccessno_OnCheckedChanged"
                                            Visible="false" AutoPostBack="true" />
                                        <br />
                                        <asp:Label ID="lblaccnofrom" runat="server" Text="From:" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlstatus1" runat="server" Visible="false" CssClass="textbox ddlstyle ddlheight3"
                                            Width="100px" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtname" runat="server" Visible="false" CssClass="textbox txtheight2"
                                            Style="width: 80px;">
                                        </asp:TextBox>
                                        <asp:CheckBox ID="cbnotreturn" runat="server" Text="Not Return" />
                                        <br />
                                        <asp:TextBox ID="tex_accnofrom" runat="server" Visible="false" Enabled="false" CssClass="textbox txtheight2"
                                            Style="width: 75px;"></asp:TextBox>
                                    </td>
                                    <td colspan="1">
                                        <asp:CheckBox ID="cbbatch" runat="server" Visible="false" AutoPostBack="true" OnCheckedChanged="cbbatch_OnCheckedChanged" />
                                        <asp:Label ID="lblbatch" runat="server" Visible="false" Text="Batch Year">
                                        </asp:Label>
                                        <br />
                                        <asp:Label ID="lblaccnoto" runat="server" Text="To:" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbatch" runat="server" Enabled="false" Visible="false" CssClass="textbox ddlstyle ddlheight3"
                                            Width="100px">
                                        </asp:DropDownList>
                                        <asp:Label ID="lbl_acr" runat="server" Visible="false" Text="Acr"></asp:Label>
                                        <asp:TextBox ID="txt_acr" runat="server" Visible="false" Style="height: 15px; width: 40px;"></asp:TextBox>
                                        <br />
                                        <asp:TextBox ID="txt_accnoto" runat="server" Visible="false" Enabled="false" CssClass="textbox txtheight2"
                                            Style="width: 75px;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblinwardtype" runat="server" Text="Inward Type" Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblsearchby" runat="server" Text="Search by" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlinwardtype" runat="server" Visible="false" CssClass="textbox ddlstyle ddlheight3"
                                            Width="100px" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlsearchby" runat="server" Visible="false" CssClass="textbox ddlstyle ddlheight3"
                                            Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddlsearchby_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:CheckBox ID="chk_ovrngtiss" runat="server" AutoPostBack="true" Text="Overnight Issue"
                                            Visible="false" />
                                    </td>
                                    <td colspan="4">
                                        <asp:RadioButtonList ID="rbllostbooks" runat="server" Visible="false" RepeatDirection="Horizontal"
                                            AutoPostBack="true" OnSelectedIndexChanged="rbllostbooks_SelectedIndexChanged">
                                            <asp:ListItem>ReplacebyNewBook</asp:ListItem>
                                            <asp:ListItem>WithFine</asp:ListItem>
                                            <asp:ListItem Selected="True">All</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:RadioButtonList ID="rbbookdetails" runat="server" Visible="false" RepeatDirection="Horizontal"
                                            AutoPostBack="true">
                                            <asp:ListItem>Reference Books Only</asp:ListItem>
                                            <asp:ListItem>Text Books Only</asp:ListItem>
                                            <asp:ListItem Selected="True">All</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:RadioButtonList ID="rbtype" runat="server" Visible="false" RepeatDirection="Horizontal"
                                            AutoPostBack="true">
                                            <asp:ListItem Selected="True">Hit by Student</asp:ListItem>
                                            <asp:ListItem>Hit by Staff</asp:ListItem>
                                            <asp:ListItem>Visitor</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:RadioButtonList ID="rbhitstatus" runat="server" Visible="false" RepeatDirection="Horizontal"
                                            AutoPostBack="true">
                                            <asp:ListItem Selected="True">Hit by Student</asp:ListItem>
                                            <asp:ListItem>Hit by Staff</asp:ListItem>
                                            <asp:ListItem>All</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:RadioButtonList ID="rbdeptwise" runat="server" Visible="false" RepeatDirection="Horizontal"
                                            AutoPostBack="true">
                                            <asp:ListItem Selected="True">Subjectwise Report</asp:ListItem>
                                            <asp:ListItem>Total No.of Books</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:RadioButtonList ID="rblist" runat="server" Visible="false" RepeatDirection="Horizontal"
                                            AutoPostBack="true">
                                            <asp:ListItem>Issue List</asp:ListItem>
                                            <asp:ListItem>Return List</asp:ListItem>
                                            <asp:ListItem>Due List</asp:ListItem>
                                            <asp:ListItem Selected="True">All</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblstatus" runat="server" Text="Status" Visible="false">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlstatus" runat="server" Visible="false" CssClass="textbox ddlstyle ddlheight3"
                                            Width="100px" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblremarks" runat="server" Text="Remarks" Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblsupplier" runat="server" Visible="false" Text="Supplier">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlremarks" runat="server" Visible="false" CssClass="textbox ddlstyle ddlheight3"
                                            Width="200px" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlsupplier" runat="server" Visible="false" CssClass="textbox ddlstyle ddlheight3"
                                            Width="200px" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtremarks" runat="server" Visible="false" CssClass="textbox txtheight2"
                                            Style="width: 160px;">
                                        </asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblpubyear" runat="server" Text="Pub. Year" Visible="false">
                                        </asp:Label>
                                        <asp:CheckBox ID="cbbillno" runat="server" Text="Bill No." OnCheckedChanged="cbbillno_OnCheckedChanged"
                                            Visible="false" AutoPostBack="true" />
                                        <br />
                                        <asp:Label ID="lblbillnofrom" runat="server" Text="From:" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtpubyear" runat="server" Visible="false" CssClass="textbox txtheight2"
                                            Style="width: 80px;">
                                        </asp:TextBox>
                                        <br />
                                        <asp:TextBox ID="txtbillnofrom" runat="server" Visible="false" Enabled="false" CssClass="textbox txtheight2"
                                            Style="width: 75px;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <br />
                                        <asp:CheckBox ID="cbcumlative" runat="server" Text="Cumlative" />
                                        <asp:Label ID="lblbillnoto" runat="server" Text="To:" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <br />
                                        <asp:TextBox ID="txtbillnoto" runat="server" Visible="false" Enabled="false" CssClass="textbox txtheight2"
                                            Style="width: 75px;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <%--<fieldset style="width: 300px; height: 20px;">--%>
                                        <asp:CheckBox ID="cbfrom" runat="server" AutoPostBack="true" OnCheckedChanged="cbfrom_OnCheckedChanged" />
                                        <asp:Label ID="lbl_fromdate1" runat="server" Text="From:"></asp:Label>
                                        <asp:TextBox ID="txt_fromdate1" runat="server" Enabled="false" Style="height: 20px;
                                            width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_fromdate1" runat="server"
                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                        <asp:Label ID="lbl_todate1" runat="server" Text="To:" Style="margin-left: 4px;"></asp:Label>
                                        <asp:TextBox ID="txt_todate1" runat="server" Enabled="false" Style="height: 20px;
                                            width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_todate1" runat="server"
                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                        <%--</fieldset>--%>
                                    </td>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpGo" runat="server">
                                            <ContentTemplate>
                                                <asp:ImageButton ID="btn_go" runat="server" ImageUrl="~/LibImages/Go.jpg" Style="margin-top: 10px;"
                                                    OnClick="btn_go_Click" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btn_go" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </center>
                <asp:Panel runat="server" ID="Panellookup1" Visible="false" BackColor="AliceBlue"
                    Style="border: thin solid Black; left: 263px; top: 185px; width: 978px; height: 562px;
                    position: absolute;">
                    <asp:Button ID="btncloselook1" OnClick="btncloselook1_Click" runat="server" Text="X"
                        Height="21px" BackColor="Transparent" BorderColor="Transparent" CssClass="floatr" />
                    <center>
                        <asp:Label ID="Label25" runat="server" Text="Student LookUp" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </center>
                    <table style="width: 385px; height: 85px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblcollege1" runat="server" Text="College_Name" Font-Bold="true" Font-Names="MS Sans Serif"
                                    Font-Size="Small"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollegenew" runat="server" OnSelectedIndexChanged="ddlcollegenew_SelectedIndexChanged"
                                    Font-Names="MS Sans Serif" Font-Size="Small" Height="20px" Width="251px" Font-Bold="true"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Batch" Font-Bold="true" Font-Names="MS Sans Serif"
                                    Font-Size="Small"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbatch11" runat="server" OnSelectedIndexChanged="ddlbatch1_SelectedIndexChanged"
                                    Font-Names="MS Sans Serif" Font-Size="Small" Height="20px" Width="70px" Font-Bold="true"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="true" Font-Names="MS Sans Serif"
                                    Font-Size="Small"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDegree" runat="server" Font-Bold="true" Font-Names="MS Sans Serif"
                                    Font-Size="Small" Height="20px" Width="70px" AutoPostBack="True" OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="true" Font-Names="MS Sans Serif"
                                    Font-Size="Small"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlBranch1" runat="server" Font-Bold="true" Font-Names="MS Sans Serif"
                                    Font-Size="Small" Height="20px" Width="185px" OnSelectedIndexChanged="ddlBranch1_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnlookupgo1" runat="server" Text="Go" Height="21px" Style="top: 53px;
                                    position: absolute; left: 870px;" CssClass="font" OnClick="btnlookupgo1_Click" />
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                    <table style="width: 462px; height: 25px; top: 100px; position: absolute;">
                        <tr>
                            <td>
                                <asp:Label ID="Label87" runat="server" Text="Search By" Font-Names="MS Sans Serif"
                                    Font-Size="Small" Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlheader" runat="server" AutoPostBack="true" Width="100px"
                                    OnSelectedIndexChanged="ddlheader_SelectedIndexChanged" Font-Names="MS Sans Serif"
                                    Font-Size="Small">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddloperator" runat="server" AutoPostBack="true" Width="100px"
                                    OnSelectedIndexChanged="ddloperator_SelectedIndexChanged" Font-Names="MS Sans Serif"
                                    Font-Size="Small" Enabled="False">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="tbvalue" runat="server" AutoPostBack="true" OnTextChanged="tbvalue_TextChanged"
                                    Font-Names="MS Sans Serif" Font-Size="Small" Height="15px" Width="153px" Enabled="False"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="lblerrefp1" runat="server" Text="" Visible="false" ForeColor="Red"
                        CssClass="font" Style="top: 26px; position: absolute;"></asp:Label>
                    <div id="StudGridLookup" runat="server" visible="false" style="width: 600px; height: 400px;
                        overflow: auto">
                        <center>
                            <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                            <asp:GridView ID="gridview1" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                Style="height: auto; width: auto;" Font-Names="book antiqua" togeneratecolumns="true"
                                OnSelectedIndexChanged="gridview1_OnSelectedIndexChanged" OnRowCreated="gridview1_OnRowCreated"
                                OnRowDataBound="gridview1_OnRowDataBound">
                                <HeaderStyle BackColor="#0ca6ca" ForeColor="Black" />
                            </asp:GridView>
                        </center>
                    </div>
                </asp:Panel>
                <center>
                    <div id="showreport2" runat="server" visible="false">
                        <table>
                            <tr>
                                <td>
                                    <FarPoint:FpSpread ID="spreadDet1" runat="server" BorderStyle="Solid" BorderWidth="0px"
                                        Width="980px" Style="overflow: auto; border: 0px solid #999999; border-radius: 10px;
                                        background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </td>
                            </tr>
                            <td>
                                <center>
                                    <div id="print" runat="server" visible="false">
                                    </div>
                                </center>
                            </td>
                        </table>
                    </div>
                </center>
                <br />
                <br />
                <center>
                    <span style="padding-right: 100px; margin-left: 442px; margin-top: 3px;">
                        <asp:CheckBox ID="chkGridSelectAll" runat="server" Text="SelectAll" Visible="false"
                            onchange="return SelLedgers();" />
                    </span>
                </center>
                <center>
                    <div>
                        <asp:GridView ID="gridview2" runat="server" Style="height: auto; width: auto;" Font-Names="book antiqua"
                            OnSelectedIndexChanged="gridview2_OnSelectedIndexChanged" OnPageIndexChanging="gridview2_OnPageIndexChanged"
                            OnRowDataBound="gridview2_RowDataBound">
                            <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                        </asp:GridView>
                    </div>
                </center>
                <br />
                <center>
                    <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                        <ContentTemplate>
                            <label id="lbl_pagecnt" runat="server" visible="false" style="background-color: Green">
                            </label>
                            <label id="lbl_totrecord" runat="server" visible="false" style="background-color: Green">
                            </label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </center>
                <br />
                <br />
                <center>
                    <div id="rptprint1" runat="server" visible="false">
                        <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            ForeColor="Red" Text="" Style="display: none;"></asp:Label>
                        <asp:Label ID="lblrptname" runat="server" Visible="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" Visible="true" Width="180px" onkeypress="display()"
                            CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                        <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                            InvalidChars="/\">
                        </asp:FilteredTextBoxExtender>--%>
                        <asp:Button ID="btnExcel" runat="server" Visible="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnExcel_Click" Text="Export To Excel" Width="127px"
                            Height="32px" CssClass="textbox textbox1" />
                        <asp:Button ID="btnprintmasterhed" runat="server" Visible="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" Height="32px"
                            Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                        <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                    <br />
                </center>
                <center>
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
                                                <asp:UpdatePanel ID="updatepanelbtn2" runat="server">
                                                    <ContentTemplate>
                                                        <center>
                                                            <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                                OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                                        </center>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExcel" />
                <asp:PostBackTrigger ControlID="btnprintmasterhed" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <%--progressBar for Go--%>
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
</asp:Content>
