<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="NonMemberEntry.aspx.cs"
    MasterPageFile="~/LibraryMod/LibraryMaster.master" Inherits="LibraryMod_NonMemberEntry" %>

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
        function valid1() {
            var idval = "";
            var empty = "";
            var id = "";
            var value1 = "";
            id = document.getElementById("<%=txtuserid.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txtuserid.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=ddldept1.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddldept1.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txtname.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txtname.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txtnoc.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txtnoc.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txtdue.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txtdue.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=cbstatus.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=cbstatus.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txtfine.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txtfine.ClientID %>");
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

        function SelLedgers() {
            var chkSelAll = document.getElementById("<%=chkGridSelectAll.ClientID %>");
            var tbl = document.getElementById("<%=grdNonMem.ClientID %>");
            var gridViewControls = tbl.getElementsByTagName("input");

            for (var i = 1; i < (tbl.rows.length); i++) {
                var chkSelectid = document.getElementById('MainContent_grdNonMem_selectchk_' + i.toString());

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
                <span class="fontstyleheader" style="color: Green;">Library External User Entry
                </span>
            </div>
        </center>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <center>
                    <table class="maintablestyle" style="height: auto; font-family: Book Antiqua; font-weight: bold;
                        margin-left: 0px; margin-top: 10px; margin-bottom: 10px; padding: 6px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblclg" runat="server" Text="College">
                                </asp:Label>
                                <asp:DropDownList ID="ddlclg" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddlcollege_selectedindexchange">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblSearchby" runat="server" Text="SerachBy"></asp:Label>
                                <asp:DropDownList ID="ddlserach" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlsearch_selectedindexchange">
                                    <asp:ListItem>All</asp:ListItem>
                                    <asp:ListItem>User Name</asp:ListItem>
                                    <asp:ListItem>User ID</asp:ListItem>
                                    <asp:ListItem>Reg Date</asp:ListItem>
                                    <asp:ListItem>MemberType</asp:ListItem>
                                    <asp:ListItem>Department</asp:ListItem>
                                    <asp:ListItem>Designation</asp:ListItem>
                                    <asp:ListItem>Gender</asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtusernam" runat="server" Visible="false" Style="width: 115px;"
                                    CssClass="textbox txtheight2"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getsearch" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtusernam"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                    <%--    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground"--%>
                                </asp:AutoCompleteExtender>
                                <asp:Label ID="lblfrom" runat="server" Text="From:" Visible="false" Style="margin-left: 4px;"></asp:Label>
                                <asp:TextBox ID="txt_fromdate1" runat="server" Visible="false" Style="width: 75px;"
                                    onchange="return checkDate()" CssClass="textbox txtheight2"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txt_fromdate1" runat="server"
                                    Format="dd/MMM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                                <asp:Label ID="lbl_todate" Visible="false" runat="server" Text="To:" Style="margin-left: 4px;"></asp:Label>
                                <asp:TextBox ID="txt_todate1" runat="server" Visible="false" Style="width: 75px;"
                                    onchange="return checkDate()" CssClass="textbox txtheight2"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender6" TargetControlID="txt_todate1" runat="server"
                                    Format="dd/MMM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                                <asp:DropDownList ID="ddlmemtype" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Width="150px" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlmemtype_selectedindexchange">
                                    <asp:ListItem>--Select--</asp:ListItem>
                                    <asp:ListItem>Student</asp:ListItem>
                                    <asp:ListItem>Staff</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlgender" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Width="150px" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlgender_selectedindexchange">
                                    <asp:ListItem>--Select--</asp:ListItem>
                                    <asp:ListItem>Female</asp:ListItem>
                                    <asp:ListItem>Male</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpGo" runat="server">
                                    <ContentTemplate>
                                        <asp:ImageButton ID="btnGo" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="btn_go_click" />
                                        <asp:ImageButton ID="btnAddnew" runat="server" ImageUrl="~/LibImages/Add new.jpg"
                                            OnClick="btn_add_click" />
                                        <asp:ImageButton ID="btndelete" runat="server" ImageUrl="~/LibImages/Delete1.jpg"
                                            OnClick="btn_delete_click" Visible="false" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                    <span style="padding-right: 100px; margin-left: -260px; margin-top: 3px;">
                        <asp:CheckBox ID="chkGridSelectAll" runat="server" Font-Names="Book Antiqua" Text="SelectAll"
                            Visible="false" onchange="return SelLedgers();" Style="margin-left: -550px;" />
                    </span>
                    <div id="divtable" runat="server" visible="false" style="font-family: Book Antiqua;">
                        <table>
                            <tr>
                                <td>
                                    <center>
                                        <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                                        <asp:GridView ID="grdNonMem" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                            ShowHeader="true" Font-Names="book antiqua" togeneratecolumns="true" AllowPaging="true"
                                            PageSize="100" OnSelectedIndexChanged="grdNonMem_onselectedindexchanged" OnPageIndexChanging="grdNonMem_onpageindexchanged"
                                            Width="980px" OnRowCreated="grdNonMem_OnRowCreated" OnRowDataBound="grdNonMem_RowDataBound">
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
                                        <center>
                                            <asp:Label ID="lbl_norec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False">
                                            </asp:Label></center>
                                        <div id="div_report" runat="server" visible="false">
                                            <center>
                                                <asp:Label ID="lbl_reportname" runat="server" Text="Report Name" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="true" OnTextChanged="txtexcelname_TextChanged"
                                                    CssClass="textbox textbox1 txtheight5" onkeypress="return ClearPrint1()"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:ImageButton ID="btn_Excel" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                                                    OnClick="btnExcel_Click" />
                                                <asp:ImageButton ID="btn_printmaster" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                                                    OnClick="btn_printmaster_Click" />
                                                <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                                            </center>
                                        </div>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btn_Excel" />
                <asp:PostBackTrigger ControlID="btn_printmaster" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <center>
        <asp:UpdatePanel ID="UpdatePanel23" runat="server">
            <ContentTemplate>
                <div id="divPopupAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%; right: 0%;">
                    <center>
                        <div id="divAlertContent" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblAlertMsg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <center>
                                                <asp:ImageButton ID="btnPopAlertClose" runat="server" ImageUrl="~/LibImages/ok.jpg"
                                                    OnClick="btnPopAlertClose_Click" />
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
    <center>
        <asp:UpdatePanel ID="UpdatePanel24" runat="server">
            <ContentTemplate>
                <div id="divaddnew" runat="server" visible="false" style="height: 70px; z-index: 100;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0px;">
                    <br />
                    <center>
                        <div id="divaddnew1" runat="server" class="table" style="background-color: White;
                            border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-left: auto;
                            margin-right: auto; width: 1000px; height: auto; z-index: 1000; border-radius: 5px;">
                            <center>
                                <span style="top: 10px; bottom: 20px; text-align: center; color: Green; font-size: large;
                                    position: relative; font-weight: bold;">Library User Entry</span>
                            </center>
                            <br />
                            <table style="margin: 10px; margin-bottom: 10px; font-family: Book Antiqua; font-weight: bold;
                                margin-top: 22px; margin-left: -521px; position: relative; width: 382px; height: 382px;">
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lbluserid" runat="server" Text="User ID:">
                                        </asp:Label>
                                        <asp:TextBox ID="txtuserid" runat="server" Style="width: 115px; margin-left: 77px"
                                            CssClass="textbox txtheight2" MaxLength="20"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblname" runat="server" Text="Name:">
                                        </asp:Label>
                                        <asp:TextBox ID="txtname" runat="server" Style="width: 115px; margin-left: 87px"
                                            CssClass="textbox txtheight2" MaxLength="52"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblperadd" runat="server" Text="Permanent Address:">
                                        </asp:Label>
                                        <asp:TextBox ID="txtperadd" runat="server" Style="width: 115px; height: 50px" CssClass="textbox txtheight2"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblpin" runat="server" Text="Pincode:">
                                        </asp:Label>
                                        <asp:TextBox ID="txtpin" runat="server" Style="width: 115px; margin-left: 82px" CssClass="textbox txtheight2"
                                            MaxLength="6"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtpin"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblphone" runat="server" Text="Phone No:">
                                                </asp:Label>
                                                <asp:TextBox ID="txtphone" runat="server" Style="width: 115px; margin-left: 70px"
                                                    CssClass="textbox txtheight2" MaxLength="20"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtphone"
                                                    FilterType="Numbers">
                                                </asp:FilteredTextBoxExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:UpdatePanel ID="upMove" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="btnmove" Text=">>>" CssClass=" textbox btn1" Style="width: 70px;
                                                    margin-left: 283px; margin-top: -26px;" runat="server" OnClick="btn_move_Click" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblemail" runat="server" Text="Email:">
                                        </asp:Label>
                                        <asp:TextBox ID="txtemail" runat="server" Style="width: 115px; margin-left: 98px"
                                            CssClass="textbox txtheight2"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lbldept1" runat="server" Text="Department:">
                                        </asp:Label>
                                        <asp:DropDownList ID="ddldept1" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="150px" OnSelectedIndexChanged="ddldept1_selectedindexchange" Style="margin-left: 40px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbldesig" runat="server" Text="Designation:">
                                        </asp:Label>
                                        <asp:TextBox ID="txtdesig" runat="server" Style="width: 115px; margin-left: 52px"
                                            CssClass="textbox txtheight2" MaxLength="50"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtdesig"
                                            FilterType="LowercaseLetters">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblnoc" runat="server" Text="No Of Cards:">
                                        </asp:Label>
                                        <asp:TextBox ID="txtnoc" runat="server" Style="width: 70px" CssClass="textbox txtheight2"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtnoc"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lbldueday" runat="server" Text="Due Days:">
                                        </asp:Label>
                                        <asp:TextBox ID="txtdue" runat="server" Style="width: 70px" CssClass="textbox txtheight2"
                                            MaxLength="3"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtdue"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                            </table>
                            <table style="margin: 10px; margin-bottom: 10px; margin-top: -402px; margin-left: 727px;
                                position: relative; width: 178px; height: 311px;">
                                <tr>
                                    <td>
                                        <asp:Image ID="imgstudp" runat="server" Style="width: 105px; height: 105px; position: absolute;
                                            left: 35px; top: 20px;" />
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="conditional">
                                            <ContentTemplate>
                                                <asp:FileUpload ID="fulstudp" runat="server" Style="position: absolute; left: 5px;
                                                    top: 135px;" Width="183px" />
                                                <asp:Button ID="BtnsaveStud" runat="server" Text="Upload" Width="80px" OnClick="BtnsaveStud_Click"
                                                    Style="position: absolute; left: 5px; top: 160px;" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="BtnsaveStud" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                            <table style="margin: 10px; margin-bottom: 10px; font-family: Book Antiqua; font-weight: bold;
                                margin-top: -312px; margin-left: 230px; position: relative; width: 395px; height: 394px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbldob" runat="server" Text="Date Of Birth:" Visible="true" Style="margin-left: 4px;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtdob" runat="server" Visible="true" Style="width: 75px;" onchange="return checkDate()"
                                            CssClass="textbox txtheight2"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtdob" runat="server"
                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblgender" runat="server" Text="Gender:">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblStatus" runat="server" Visible="true" RepeatDirection="Horizontal"
                                            AutoPostBack="true" ForeColor="Black">
                                            <asp:ListItem>Female</asp:ListItem>
                                            <asp:ListItem Selected="True">Male</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="tmpadd" runat="server" Text="Temporary Address:">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txttempadd" runat="server" Style="width: 115px; height: 50px" CssClass="textbox txtheight2"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblpincode" runat="server" Text="Pincode:">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtpincode" runat="server" Style="width: 115px" CssClass="textbox txtheight2"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblphno" runat="server" Text="Phone No:">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtphno" runat="server" Style="width: 115px;" CssClass="textbox txtheight2"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblemailid" runat="server" Text="Email:">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtemailid" runat="server" Style="width: 115px" CssClass="textbox txtheight2"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblmemty" runat="server" Text="MemberType:">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblmemty" runat="server" Visible="true" RepeatDirection="Horizontal"
                                            AutoPostBack="true" ForeColor="Black">
                                            <asp:ListItem>Student</asp:ListItem>
                                            <asp:ListItem>Staff</asp:ListItem>
                                            <asp:ListItem Selected="True">Visitor</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbldoreg" runat="server" Text="Date Of Registration:" Visible="true"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtdoreg" runat="server" Visible="true" Style="width: 75px;" onchange="return checkDat()"
                                            CssClass="textbox txtheight2"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtdoreg" runat="server"
                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblfine" runat="server" Text="Fine Amount:">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfine" runat="server" Style="width: 85px" CssClass="textbox txtheight2"
                                            MaxLength="8"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtfine"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblstatus" runat="server" Text="Status:">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbstatus" runat="server" Enabled="true" AutoPostBack="true" OnCheckedChanged="cbstatus_OnCheckedChanged"
                                            Text="Active/InActive" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblclsdate" runat="server" Text="Close Date:" CssClass="commonHeaderFont">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtclsdate" runat="server" Visible="true" Style="width: 75px;" onchange="return checkDat()"
                                            CssClass="textbox txtheight2"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txtclsdate" runat="server"
                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td align="center">
                                        <asp:UpdatePanel ID="UpSave" runat="server">
                                            <ContentTemplate>
                                                <center>
                                                    <asp:ImageButton ID="btnsave" runat="server" ImageUrl="~/LibImages/save.jpg" OnClick="btn_Save_Click" />
                                                    <asp:ImageButton ID="btnupdate" runat="server" ImageUrl="~/LibImages/update (2).jpg"
                                                        OnClick="btnupdate_Click" />
                                                    <asp:ImageButton ID="btnexit" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                                        OnClick="btn_exit_Click" />
                                                </center>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel25" runat="server">
            <ContentTemplate>
                <div id="div1" runat="server" visible="false" style="height: 100em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%; right: 0%;">
                    <center>
                        <div id="div2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbldeletealter" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:UpdatePanel ID="UpdatePanelbtn6" runat="server">
                                                <ContentTemplate>
                                                    <center>
                                                        <asp:ImageButton ID="btnyes" runat="server" ImageUrl="~/LibImages/yes.jpg" OnClick="btnPopAlertyes_Click" />
                                                        <asp:ImageButton ID="btnNo" runat="server" ImageUrl="~/LibImages/no (2).jpg" OnClick="btnPopAlertNo_Click" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--progressBar for GO--%>
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
    <%--progressBar for save--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpSave">
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
    <%--progressBar for Yes Or No--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanelbtn6">
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
    <%--progressBar for Move--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="upMove">
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
</asp:Content>
