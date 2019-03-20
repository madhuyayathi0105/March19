<%@ Page Title="" Language="C#" MasterPageFile="~/RequestMOD/RequestSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Request_Report.aspx.cs" Inherits="Request_Report"
    EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .maindivstylesize
        {
            height: 1100px;
            width: 1290px;
        }
        .newtextbox
        {
            border: 1px solid #c4c4c4;
            padding: 4px 4px 4px 4px;
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            box-shadow: 0px 0px 8px #d9d9d9;
            -moz-box-shadow: 0px 0px 8px #d9d9d9;
            -webkit-box-shadow: 0px 0px 8px #d9d9d9;
            width:1200px;
        }
        .fontstyleheaderrr
        {
            font-family: Book Antiqua;
            font-size: larger;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <script type="text/javascript">
            function DisplayLoadingDiv() {
                document.getElementById("<%=divImageLoading.ClientID %>").style.display = "block";
            }
            function HideLoadingDiv() {
                document.getElementById("<%=divImageLoading.ClientID %>").style.display = "none";
            }
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
//            function QuantityChange() {

//                var itemcostTotal = document.getElementById("<%=lblitemcostTotal.ClientID %>");
//                var tbl = document.getElementById("<%=SelectdptGrid.ClientID %>");
//                var gridViewControls = tbl.getElementsByTagName("input");
//                var totalcost = 0.0;
//                for (var i = 0; i < (gridViewControls.length); i++) {

//                    var txtQty = document.getElementById('MainContent_SelectdptGrid_txt_quantity_' + i.toString());
//                    var txtRPU = document.getElementById('MainContent_SelectdptGrid_txt_SuggestedCost_' + i.toString());
//                    var txtcost = document.getElementById('MainContent_SelectdptGrid_txt_Cost_' + i.toString());

//                    var QtyVal = 0.0;
//                    var RPUVal = 0.0;

//                    if (txtQty.value.trim() != "") {
//                        QtyVal = parseFloat(txtQty.value);

//                    }
//                    if (txtRPU.value.trim() != "") {
//                        RPUVal = parseFloat(txtRPU.value);

//                    }

//                    txtcost.value = (QtyVal * RPUVal).toString();
//                    totalcost = parseFloat(totalcost) + parseFloat(txtcost.value);
//                    itemcostTotal.innerHTML = totalcost;
//                }

            //            }

            function QuantityChange(objRef, colIndex) {

                var row = objRef.parentNode.parentNode;

                var rowIndex = row.rowIndex - 1;

                if (rowIndex.toString() != "") {
                    var txtQty = document.getElementById('MainContent_SelectdptGrid_txt_quantity_' + rowIndex.toString());
                    var txtRPU = document.getElementById('MainContent_SelectdptGrid_txt_SuggestedCost_' + rowIndex.toString());
                    var txtcost = document.getElementById('MainContent_SelectdptGrid_txt_Cost_' + rowIndex.toString());
                    var QtyVal = 0.0;
                    var RPUVal = 0.0;

                    if (txtQty.value.trim() != "") {
                        QtyVal = parseFloat(txtQty.value);

                    }
                    if (txtRPU.value.trim() != "") {
                        RPUVal = parseFloat(txtRPU.value);

                    }

                    txtcost.value = (QtyVal * RPUVal).toString();
                    totalcost = parseFloat(totalcost) + parseFloat(txtcost.value);
                    itemcostTotal.innerHTML = totalcost;
                }


            }

            function QuantityChange1(objRef, colIndex) {

                var row = objRef.parentNode.parentNode;
                var rowIndex = row.rowIndex - 1;

                if (rowIndex.toString() != "") {
                    var txtQty = document.getElementById('MainContent_SelectlibGrid_txt_copy_' + rowIndex.toString());
                    var txtRPU = document.getElementById('MainContent_SelectlibGrid_txtSuggestedCost_' + rowIndex.toString());
                    var txtcost = document.getElementById('MainContent_SelectlibGrid_txtCost_' + rowIndex.toString());
                    var QtyVal = 0.0;
                    var RPUVal = 0.0;

                    if (txtQty.value.trim() != "") {
                        QtyVal = parseFloat(txtQty.value);

                    }
                    if (txtRPU.value.trim() != "") {
                        RPUVal = parseFloat(txtRPU.value);

                    }

                    txtcost.value = (QtyVal * RPUVal).toString();
                    totalcost = parseFloat(totalcost) + parseFloat(txtcost.value);
                    bookcostTotal.innerHTML = totalcost;

                }


            }
        </script>
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <div>
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: #008000;">Request Report</span></div>
                </center>
                <br />
            </div>
            <center>
                <div class="maindivstyle maindivstylesize">
                    <br />
                    <div>
                        <center>
                            <table class="table" width="1200px">
                                <tr>
                                    <td colspan="2">
                                        <div class="maindivstyle" align="center" style="border-radius: 7px; width: 320px;
                                            height: 30px;">
                                            <asp:RadioButton ID="rdo_request" Text="Request" runat="server" GroupName="rr" AutoPostBack="true"
                                                OnCheckedChanged="rdo_request_CheckedChanged" />
                                            <asp:RadioButton ID="rdo_approval" Text="Waiting" runat="server" GroupName="rr" AutoPostBack="true"
                                                OnCheckedChanged="rdo_approval_CheckedChanged" />
                                            <asp:RadioButton ID="rdo_reject" Text="Reject" runat="server" GroupName="rr" AutoPostBack="true"
                                                OnCheckedChanged="rdo_reject_CheckedChanged" />
                                            <asp:RadioButton ID="rdo_waiting" Text="Approval" runat="server" GroupName="rr" AutoPostBack="true"
                                                OnCheckedChanged="rdo_waiting_CheckedChanged" />
                                        </div>
                                    </td>

                                     <td>
                                     <asp:DropDownList ID="ddl_search" runat="server" CssClass="ddlheight5 textbox1 textbox" AutoPostBack="true">
                                     <asp:ListItem Value="0" Text="Search Based On Request Date" />
                                     <asp:ListItem Value="1" Text="Search Based On Applied Date" />
                                     </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_fromdate" Text="From Date" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="Updp_fromdate" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox txtheight2" OnTextChanged="txt_fromdate_TextChanged"
                                                    AutoPostBack="true"></asp:TextBox>
                                                <asp:CalendarExtender ID="Cal_date" TargetControlID="txt_fromdate" runat="server"
                                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_starttime1" Text="From Time" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_hour" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_minits" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_seconds" Width="50px" Height="25px" Visible="false" runat="server"
                                            CssClass="textbox textbox1">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_timeformate" Width="48px" Height="25px" runat="server"
                                            CssClass="textbox textbox1">
                                            <asp:ListItem>AM</asp:ListItem>
                                            <asp:ListItem>PM</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <%-- <td colspan="2"></td>--%>
                                    <td>
                                        <asp:Label ID="lbl_rpt_college" runat="server" Text="College"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_rpt_collge" OnSelectedIndexChanged="ddl_rpt_collge_changed" AutoPostBack="true" runat="server" CssClass="ddlheight5 textbox1 textbox">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                      <asp:DropDownList ID="ddlsearchappstf" runat="server" CssClass="textbox1 ddlheight2"
                                                    OnSelectedIndexChanged="ddlsearchappstf_change" AutoPostBack="true" 
                                                    Font-Names="Book Antiqua" Font-Size="Medium">
                                                </asp:DropDownList>
                                                  <asp:TextBox ID="txt_staffname" Visible="false" runat="server" MaxLength="100" AutoPostBack="true"
                                                     CssClass="textbox txtheight2 txtcapitalize"
                                                    Style="width: 137px; font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender13" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffname"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="txtsearchpan">
                                                </asp:AutoCompleteExtender>
                                                <asp:TextBox ID="txt_StaffCode" Visible="false" runat="server" MaxLength="100" AutoPostBack="true"
                                                     CssClass="textbox txtheight2 txtcapitalize"
                                                    Style="width: 137px; font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_StaffCode"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="txtsearchpan">
                                                </asp:AutoCompleteExtender>

                                    </td>
                                   
                                    <td>
                                        <asp:Label ID="lbl_todate" Text="To Date" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="Updp_todate" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox txtheight2" OnTextChanged="txt_todate_TextChanged"
                                                    AutoPostBack="true"></asp:TextBox>
                                                <asp:CalendarExtender ID="Cal_date1" TargetControlID="txt_todate" runat="server"
                                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <%-- <td>
                                    <asp:Label ID="lbl_totime" Text="To Time" runat="server"></asp:Label>
                                </td>--%>
                                    <td>
                                        <asp:Label ID="lbl_endtime1" Text="To Time" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_endhour" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_endminit" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_endsecnonds" Width="50px" Height="25px" Visible="false"
                                            runat="server" CssClass="textbox textbox1">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_endformate" Width="48px" Height="25px" runat="server" CssClass="textbox textbox1">
                                            <asp:ListItem>AM</asp:ListItem>
                                            <asp:ListItem>PM</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <%-- <td>
                                    <asp:TextBox ID="txt_totime" runat="server" CssClass="textbox txtheight2"></asp:TextBox>
                                </td>--%>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <center>
                                            <table width="920px">
                                                <tr>
                                                    <td id="td_all" runat="server" align="center">
                                                        <asp:ImageButton ID="imgbtn_all" runat="server" Width="50px" Height="50px" Text="All"
                                                            ImageUrl="~/Hostel Gete Images/images (1)ppp.jpg" OnClick="imgbtn_all_Click" />
                                                        <br />
                                                        <asp:Label ID="Label2" runat="server" Style="top: 10px; left: 6px;" Text="All"></asp:Label>
                                                    </td>
                                                    <td id="td_stud" runat="server" align="center">
                                                        <asp:ImageButton ID="imgbtn_item" runat="server" Width="50px" Height="50px" Text="Item Request"
                                                            ImageUrl="~/request_img/index.jpg" OnClick="imgbtn_item_Click" />
                                                        <br />
                                                        <asp:Label ID="lbl_item" runat="server" Style="top: 10px; left: 6px;" Text="Item"></asp:Label>
                                                    </td>
                                                    <td id="td_staff" runat="server" align="center">
                                                        <asp:ImageButton ID="imgbtn_service" runat="server" Width="50px" Height="50px" ImageUrl="~/request_img/images.jpg"
                                                            OnClick="imgbtn_service_Click" />
                                                        <br />
                                                        <asp:Label ID="lbl_service" runat="server" Style="top: 10px; left: 6px;" Text="Service"></asp:Label>
                                                    </td>
                                                    <td id="td_par" runat="server" align="center">
                                                        <asp:ImageButton ID="imgbtn_visitor" runat="server" Width="50px" Height="50px" ImageUrl="~/request_img/visit.jpg"
                                                            OnClick="imgbtn_visitor_Click" /><br />
                                                        <asp:Label ID="lbl_visitor" runat="server" Style="top: 10px; left: 6px;" Text="Visitor Appointment"></asp:Label>
                                                    </td>
                                                    <td id="td_comp" runat="server" align="center">
                                                        <asp:ImageButton ID="imgbtn_complaints" runat="server" Width="50px" Height="50px"
                                                            ImageUrl="~/request_img/compl.jpg" OnClick="imgbtn_complaints_Click" /><br />
                                                        <asp:Label ID="lbl_complaints" runat="server" Style="top: 10px; left: 6px;" Text="Complaints"></asp:Label>
                                                    </td>
                                                    <td id="td_indi" runat="server" align="center">
                                                        <asp:ImageButton ID="imgbtn_leave" runat="server" Width="50px" Height="50px" ImageUrl="~/request_img/leave.png"
                                                            OnClick="imgbtn_leave_Click" /><br />
                                                        <asp:Label ID="lbl_leave" runat="server" Style="top: 10px; left: 6px;" Text="Leave"></asp:Label>
                                                    </td>
                                                    <td id="td_mag" runat="server" align="center">
                                                        <asp:ImageButton ID="imgbtn_gatepass" runat="server" Width="50px" Height="50px" ImageUrl="~/request_img/gate.jpg"
                                                            OnClick="imgbtn_gatepass_Click" /><br />
                                                        <asp:Label ID="lbl_gatepass" runat="server" Style="top: 10px; left: 6px;" Text="GatePass"></asp:Label>
                                                    </td>
                                                    <td id="td3" runat="server" align="center">
                                                        <asp:ImageButton ID="img_event" runat="server" Width="50px" Height="50px" ImageUrl="~/request_img/mic-podium-02.jpg"
                                                            OnClick="img_event_Click" /><br />
                                                        <asp:Label ID="Label27" runat="server" Style="top: 10px; left: 6px;" Text="Event"></asp:Label>
                                                    </td>
                                                    <td id="td_certificateRequest" runat="server" visible="false" align="center">
                                                        <asp:ImageButton ID="imgCertificateReq" runat="server" Width="50px" Height="50px"
                                                            ImageUrl="~/request_img/Certificate.png" OnClick="imgCertificateReq_Click" /><br />
                                                        <asp:Label ID="Label11" runat="server" Style="top: 10px; left: 6px;" Text="Certificate"></asp:Label>
                                                    </td>
                                                    <td id="td1" runat="server" align="center">
                                                        <asp:ImageButton ID="img_search" runat="server" Width="50px" Height="50px" ImageUrl="~/Hostel Gete Images/file-manager.png"
                                                            OnClick="imgbtn_search_Click" /><br />
                                                        <asp:Label ID="Label3" runat="server" Style="top: 10px; left: 6px;" Text="Search"></asp:Label>
                                                    </td>
                                                     <td id="td_library" runat="server" align="center">
                                            <asp:ImageButton ID="imgbtn_library" runat="server" Width="50px" Height="50px"  OnClick="imgbtn_library_Click" ImageUrl="~/request_img/Library.jpg"  />
                                            <br />
                                           <asp:Label ID="Label12" runat="server" Style="top: 10px; left: 6px;" Text="Book"></asp:Label>
                                        </td>
                                         <%-- sd22/2019--%>
                                                    <td id="td_Hostel" runat="server" align="center">
                                                        <asp:ImageButton ID="imgbtn_Hostel" runat="server" Width="50px" Height="50px" OnClick="imgbtn_Hostel_Click"
                                                            ImageUrl="~/request_img/Hostel.png" />
                                                        <br />
                                                        <asp:Label ID="Label16" runat="server" Style="top: 10px; left: 6px;" Text="Hostel Vacate"></asp:Label>
                                                    </td>
                                                    <td id="td2" runat="server" align="center">
                                                        <asp:ImageButton ID="link" runat="server" Width="50px" Height="50px" ImageUrl="~/request_img/arrow.jpg"
                                                            OnClick="imgbtn_Link_Click" /><br />
                                                        <asp:Label ID="Label4" runat="server" Style="top: 10px; left: 6px;" Text="Go to Request"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="imgbtn_tab" runat="server" Width="33px" Visible="false" OnClick="tag_Click"
                                            Height="34px" ImageUrl="~/image/Menuimage.png" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rdo_gatestudent" runat="server" Text="Student" GroupName="sss"
                                            Checked="true" />
                                    </td>
                                    <td colspan="2">
                                        <asp:RadioButton ID="rdo_gatestaff" runat="server" Text="Staff" GroupName="sss" />
                                        <asp:Label ID="lbl_studrptroll" Visible="false" runat="server" Text="Roll No"></asp:Label>
                                        <asp:TextBox ID="txt_studrptroll" runat="server" Visible="false" CssClass="txtheight3 textbox textbox1"
                                            AutoPostBack="true" OnTextChanged="txt_studrptroll_Changed"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender10" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getroll" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_studrptroll"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <asp:Label ID="lbl_studrptname" Visible="false" runat="server" Text="Student Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_studrptname" Visible="false" runat="server" CssClass="txtheight5 textbox textbox1"
                                            AutoPostBack="true" OnTextChanged="txt_studrptname_Changed"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender11" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getnamegatpass" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_studrptname"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_rptfind" Visible="false" runat="server" Text="Search" CssClass="btn2 textbox textbox1"
                                            OnClick="btn_rptfind_Click" />
                                    </td>
                                </tr>
                                <%--departmentwise option--%>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cbdept" Text="DepartmentWise" runat="server" Visible="false" AutoPostBack="true"
                                            OnCheckedChanged="cbdept_Changed" />
                                    </td>
                                    <td colspan="5" id="tddept" runat="server" visible="false">
                                        <table>
                                            <tr>
                                                <td>
                                                    Department
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="updept" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_depts" runat="server" CssClass="textbox textbox1" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="pnldept" runat="server" CssClass="multxtpanel" Height="200px" Width="250px">
                                                                <asp:CheckBox ID="cb_depts" runat="server" AutoPostBack="true" OnCheckedChanged="cbl_depts_CheckedChanged"
                                                                    Text="Select All" />
                                                                <asp:CheckBoxList ID="cbl_depts" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_depts_selectedchanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="pnlextnder" runat="server" PopupControlID="pnldept"
                                                                TargetControlID="txt_depts" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    Designation
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="updesi" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_desig" runat="server" CssClass="textbox textbox1" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="pnldes" runat="server" CssClass="multxtpanel" Height="200px" Width="250px">
                                                                <asp:CheckBox ID="cb_desig" runat="server" AutoPostBack="true" OnCheckedChanged="cb_desig_CheckedChanged"
                                                                    Text="Select All" />
                                                                <asp:CheckBoxList ID="cbl_desig" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_desig_selectedchanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" PopupControlID="pnldes"
                                                                TargetControlID="txt_desig" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    Staff Category
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upstaffcat" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_staffcat" runat="server" CssClass="textbox textbox1" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="pnl_staffcat" runat="server" CssClass="multxtpanel" Height="200px"
                                                                Width="200px">
                                                                <asp:CheckBox ID="cb_staffcat" runat="server" AutoPostBack="true" OnCheckedChanged="cb_staffcat_CheckedChanged"
                                                                    Text="Select All" />
                                                                <asp:CheckBoxList ID="cbl_staffcat" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_staffcat_selectedchanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" PopupControlID="pnl_staffcat"
                                                                TargetControlID="txt_staffcat" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trdept" runat="server" visible="false">
                                    <td>
                                        Staff Type
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="updstafftype" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_stafftyp" runat="server" CssClass="textbox textbox1" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="pnlstafftyp" runat="server" CssClass="multxtpanel" Height="200px"
                                                    Width="200px">
                                                    <asp:CheckBox ID="cb_stafftyp" runat="server" AutoPostBack="true" OnCheckedChanged="cb_stafftyp_CheckedChanged"
                                                        Text="Select All" />
                                                    <asp:CheckBoxList ID="cbl_stafftyp" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_stafftyp_selectedchanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" PopupControlID="pnlstafftyp"
                                                    TargetControlID="txt_stafftyp" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Label ID="lbl_rpt_errr" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                            <div id="popwindow1" runat="server" style="display: none; width: 555px; height: 280px;
                                z-index: 5000; margin-left: 104px; margin-top: -112px; position: absolute;" class="table">
                                <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                    Style="height: 30px; width: 30px; position: absolute; margin-top: -10px; margin-left: 259px;"
                                    OnClientClick="return selectpop();" />
                                <div>
                                    <center>
                                        <table style="line-height: 40px;">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_itm" Text="Item Request" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_itm" runat="server" CssClass="textbox txtheight5" Width="390px"
                                                        onchange="return changed();">
                                                    </asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="ftext_rollno" runat="server" TargetControlID="txt_itm"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:AutoCompleteExtender ID="acext_rollno" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="Getstudname" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_itm"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_serv" Text="Service" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_serv" runat="server" CssClass="textbox txtheight5" onfocus="return myFunction(this)"
                                                        onchange="return changestf();" Width="390px">
                                                    </asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_serv"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_serv"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_vist" Text="Visitor" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_vist" runat="server" CssClass="textbox txtheight5" onchange="return changecom();"
                                                        Width="390px">
                                                    </asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_vist"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:AutoCompleteExtender ID="auto_comp" runat="server" DelimiterCharacters="" Enabled="True"
                                                        ServiceMethod="getcompname" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_vist"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_com" Text="Compliant" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_com" runat="server" CssClass="textbox txtheight5" onchange="return changepar();"
                                                        Width="390px">
                                                    </asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="auto_parent" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="Getparentname" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_com"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_lev" Text="Leave Request" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_lev" runat="server" CssClass="textbox txtheight5" Width="390px"
                                                        onchange="return changeindi();">
                                                    </asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_lev"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="getindivame" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_lev"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_gate" Text="Gate Pass" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_gate" runat="server" CssClass="textbox txtheight5" Width="390px"
                                                        onchange="return changemag();">
                                                    </asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_gate"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="getmagname" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_gate"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </div>
                            </div>
                        </center>
                    </div>
                    <br />
                    <%--       ***************************Item request************************--%>
                    <div id="item_div" runat="server" visible="false">
                        <div>
                            <center>
                                <asp:Panel ID="pheaderfilter0" runat="server" CssClass="maintablestyle" Height="22px"
                                    Width="850px" Style="margin-top: -0.1%;">
                                    <asp:Label ID="lbl_st" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                        Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                    <asp:Image ID="Image7" runat="server" CssClass="cpimage" ImageUrl="right.jpeg" ImageAlign="Right" />
                                </asp:Panel>
                            </center>
                            <br />
                        </div>
                        <asp:Panel ID="pcolumnorder0" runat="server" CssClass="maintablestyle" Width="850px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox_column0" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column0_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton8" runat="server" Font-Size="X-Small" Height="16px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                            Visible="false" Width="111px" OnClick="LinkButtonsremove0_Click">Remove  All</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                        <asp:TextBox ID="tborder0" Visible="false" Width="840px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder0" runat="server" Height="43px" AutoPostBack="true"
                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder0_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="RequestCode">Requisition No</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="RequestDate">Req Date</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="ReqAppNo">Requested Staff</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="RequestBy">Requested Department</asp:ListItem>
                                            <asp:ListItem Value="Remarks">Remarks</asp:ListItem>
                                            <asp:ListItem Value="MemType">MemType</asp:ListItem>
                                            <asp:ListItem Value="ReqExpectedDate">Expected Date</asp:ListItem>
                                            <asp:ListItem Value="RequisitionPK">Count Of Item</asp:ListItem>
                                            <asp:ListItem Value="ReqAppStatus">Approval Status</asp:ListItem>
                                            <asp:ListItem Value="ReqApproveStage"> Approval Stage</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="cpecolumnorder0" runat="server" TargetControlID="pcolumnorder0"
                            CollapseControlID="pheaderfilter0" ExpandControlID="pheaderfilter0" Collapsed="true"
                            TextLabelID="lbl_st" CollapsedSize="0" ImageControlID="Image7" CollapsedImage="right.jpeg"
                            ExpandedImage="down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                        <asp:Label ID="lbl_err_item" runat="server" ForeColor="Red"></asp:Label>
                        <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" BorderWidth="5px"
                            BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" OnPreRender="FpSpread1_SelectedIndexChanged"
                            OnButtonCommand="fpspread1_ButtonCommand">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <%--       ***************************Service************************--%>
                    <div id="service_div" runat="server" visible="false">
                        <div>
                            <center>
                                <asp:Panel ID="pheaderfilter" runat="server" CssClass="maintablestyle" Height="22px"
                                    Width="850px" Style="margin-top: -0.1%;">
                                    <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                    <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                        Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                    <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="right.jpeg"
                                        ImageAlign="Right" />
                                </asp:Panel>
                            </center>
                            <br />
                        </div>
                        <asp:Panel ID="pcolumnorder" runat="server" CssClass="maintablestyle" Width="850px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                            Visible="false" Width="111px" OnClick="LinkButtonsremove_Click">Remove  All</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                        <asp:TextBox ID="tborder" Visible="false" Width="840px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" AutoPostBack="true"
                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="RequestCode">Requisition No</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="RequestDate">Req Date</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="ReqAppNo">Requested Staff</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="RequestBy">Requested Department</asp:ListItem>
                                            <asp:ListItem Value="Remarks">Remarks</asp:ListItem>
                                            <asp:ListItem Value="MemType">MemType</asp:ListItem>
                                            <asp:ListItem Value="ReqExpectedDate">Expected Date</asp:ListItem>
                                            <asp:ListItem Value="RequisitionPK">Count Of Item</asp:ListItem>
                                            <asp:ListItem Value="ReqAppStatus">Approval Status</asp:ListItem>
                                            <asp:ListItem Value="ReqApproveStage"> Approval Stage</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                            CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                            TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="right.jpeg"
                            ExpandedImage="down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                        <asp:Label ID="lbl_err_staff" runat="server" ForeColor="Red"></asp:Label>
                        <center>
                            <%-- <div id="div2" runat="server" visible="false" style="width: 890px; height: 350px;
                                    overflow: auto; border: 1px solid Gray; background-color: White;">--%>
                            <br />
                            <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" BorderWidth="5px"
                                BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" OnButtonCommand="fpspread2_ButtonCommand">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <%-- </div>--%>
                            <br />
                        </center>
                    </div>
                    <%--       ***************************Visitor************************--%>
                    <div id="vist_div" runat="server" visible="false">
                        <div>
                            <center>
                                <asp:Panel ID="pheaderfilter1" runat="server" CssClass="maintablestyle" Height="22px"
                                    Width="850px" Style="margin-top: -0.1%;">
                                    <asp:Label ID="lbl_par" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                        Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                    <asp:Image ID="Image2" runat="server" CssClass="cpimage" ImageUrl="right.jpeg" ImageAlign="Right" />
                                </asp:Panel>
                            </center>
                            <br />
                        </div>
                        <asp:Panel ID="pcolumnorder1" runat="server" CssClass="maintablestyle" Width="850px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox_column11" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column1_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton1" runat="server" Font-Size="X-Small" Height="16px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                            Visible="false" Width="111px" OnClick="LinkButtonsremove1_Click">Remove  All</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                        <asp:TextBox ID="tborder1" Visible="false" Width="840px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder1" runat="server" Height="43px" AutoPostBack="true"
                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder1_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="RequestCode">Requisition No</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="RequestDate">Requisition Date</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="ReqAppNo">Requested Staff</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="VendorCompName">Company Name </asp:ListItem>
                                            <asp:ListItem Selected="True" Value="VenContactName">Person Name</asp:ListItem>
                                            <asp:ListItem Value="Remarks">Remarks</asp:ListItem>
                                            <asp:ListItem Value="ReqExpectedDate">Expected Date</asp:ListItem>
                                            <asp:ListItem Value="ReqAppStatus">Approval Status</asp:ListItem>
                                            <asp:ListItem Value="ReqApproveStage"> Approval Stage</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="cpecolumnorder1" runat="server" TargetControlID="pcolumnorder1"
                            CollapseControlID="pheaderfilter1" ExpandControlID="pheaderfilter1" Collapsed="true"
                            TextLabelID="lbl_par" CollapsedSize="0" ImageControlID="Image2" CollapsedImage="right.jpeg"
                            ExpandedImage="down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                        <asp:Label ID="lbl_err_parent" runat="server" ForeColor="Red"></asp:Label>
                        <center>
                            <FarPoint:FpSpread ID="Fpspread3" runat="server" Visible="false" BorderWidth="5px"
                                BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" OnButtonCommand="fpspread3_ButtonCommand">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <%-- </div>--%>
                            <br />
                        </center>
                    </div>
                    <%--       ***************************Compliant************************--%>
                    <div id="comp_div" runat="server" visible="false">
                        <div>
                            <center>
                                <asp:Panel ID="pheaderfilter2" runat="server" CssClass="maintablestyle" Height="22px"
                                    Width="850px" Style="margin-top: -0.1%;">
                                    <asp:Label ID="Label1" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                        Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                    <asp:Image ID="Image3" runat="server" CssClass="cpimage" ImageUrl="right.jpeg" ImageAlign="Right" />
                                </asp:Panel>
                            </center>
                            <br />
                        </div>
                        <asp:Panel ID="pcolumnorder2" runat="server" CssClass="maintablestyle" Width="850px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox_column2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column2_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" Font-Size="X-Small" Height="16px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                            Visible="false" Width="111px" OnClick="LinkButtonsremove2_Click">Remove  All</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                        <asp:TextBox ID="tborder2" Visible="false" Width="840px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder2" runat="server" Height="43px" AutoPostBack="true"
                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder2_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="RequestCode">Requisition No</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="RequestDate">Requisition Date</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="ReqAppNo">Requested Staff</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="ReqComplaints">Complaints </asp:ListItem>
                                            <asp:ListItem Value="ReqComplaintSub">Complaint Subject </asp:ListItem>
                                            <asp:ListItem Value="ReqSuggestions">Suggestions </asp:ListItem>
                                            <asp:ListItem Value="ReqAppStatus">Approval Status</asp:ListItem>
                                            <asp:ListItem Value="ReqApproveStage"> Approval Stage</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="cpecolumnorder2" runat="server" TargetControlID="pcolumnorder2"
                            CollapseControlID="pheaderfilter2" ExpandControlID="pheaderfilter2" Collapsed="true"
                            TextLabelID="lbl_com" CollapsedSize="0" ImageControlID="Image3" CollapsedImage="right.jpeg"
                            ExpandedImage="down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                        <asp:Label ID="lbl_err_comp" runat="server" ForeColor="Red"></asp:Label>
                        <center>
                            <%-- <div id="div4" runat="server" visible="false" style="width: 890px; height: 350px;
                            overflow: auto; border: 1px solid Gray; background-color: White;">--%>
                            <br />
                            <FarPoint:FpSpread ID="Fpspread4" runat="server" Visible="false" BorderWidth="5px"
                                BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" OnButtonCommand="fpspread4_ButtonCommand">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <%-- </div>--%>
                            <br />
                        </center>
                    </div>
                    <%--       ***************************leave 850 width************************--%>
                    <div id="leave_div" runat="server" visible="false">
                        <div>
                            <center>
                                <asp:Panel ID="pheaderfilter3" runat="server" CssClass="maintablestyle" Height="22px"
                                    Width="1168px" Style="margin-top: -0.1%;">
                                    <asp:Label ID="lbl_indi" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                        Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                    <asp:Image ID="Image4" runat="server" CssClass="cpimage" ImageUrl="right.jpeg" ImageAlign="Right" />
                                </asp:Panel>
                            </center>
                            <br />
                        </div>
                        <asp:Panel ID="pcolumnorder3" runat="server" CssClass="maintablestyle" Width="1168px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox_column3" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" Checked="true" OnCheckedChanged="CheckBox_column3_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton5" runat="server" Font-Size="X-Small" Height="16px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                            Visible="false" Width="111px" OnClick="LinkButtonsremove3_Click">Remove  All</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                        <asp:TextBox ID="tborder3" Visible="false" Width="840px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder3" runat="server" Height="30px" AutoPostBack="true"
                                            Width="1168px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder3_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="RequestCode">Request Code</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="RequestDate">Requisition Date</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="Staff_Code">Staff Code</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="Staff_Name">Staff Name</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="LeaveFrom">From Date</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="LeaveTo">To Date</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="Leave">Leave</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="LeaveSession">LeaveSession</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="HalfDate">HalfDay Date</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="LeaveMasterFK">Leave Type</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="GateReqReason">Leave Reason</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="ReqAppStatus">Approval Status</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="ReqApproveStage"> Approved/Rejected Stage</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="ReqAppStaffAppNo"> Approved/Rejected Reason</asp:ListItem>
                                            <asp:ListItem Selected="False" Value="Leave Count">Leave Count</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="cpecolumnorder3" runat="server" TargetControlID="pcolumnorder3"
                            CollapseControlID="pheaderfilter3" ExpandControlID="pheaderfilter3" Collapsed="true"
                            TextLabelID="lbl_indi" CollapsedSize="0" ImageControlID="Image4" CollapsedImage="right.jpeg"
                            ExpandedImage="down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                        <asp:Label ID="lbl_err_indi" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
                        <center>
                            <br />
                            <FarPoint:FpSpread ID="Fpspread5" runat="server" Visible="false" BorderWidth="5px"
                                BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" OnButtonCommand="fpspread5_ButtonCommand">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <br />
                        </center>
                    </div>
                    <%--       ***************************gatepass************************--%>
                    <div id="gate_div" runat="server" visible="false">
                        <div>
                            <center>
                                <asp:Panel ID="pheaderfilter4" runat="server" CssClass="maintablestyle" Height="22px"
                                    Width="850px" Style="margin-top: -0.1%;">
                                    <asp:Label ID="lbl_mag" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                        Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                    <asp:Image ID="Image5" runat="server" CssClass="cpimage" ImageUrl="right.jpeg" ImageAlign="Right" />
                                </asp:Panel>
                            </center>
                            <br />
                        </div>
                        
                        <asp:Panel ID="pcolumnorder4" runat="server" CssClass="maintablestyle" Width="850px">

                        <asp:UpdatePanel ID="Upp3" runat="server">
                            <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox_column4" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column4_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton6" runat="server" Font-Size="X-Small" Height="16px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                            Visible="false" Width="111px" OnClick="LinkButtonsremove4_Click">Remove  All</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                        <asp:TextBox ID="tborder4" Visible="false" Width="840px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder4" runat="server" Height="43px" AutoPostBack="true"
                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder4_SelectedIndexChanged">
                                              <asp:ListItem  Value="Roll_No">Roll No</asp:ListItem>
                                              <asp:ListItem  Value="Stud_Name">Student Name</asp:ListItem>
                                              <asp:ListItem  Value="Degree">Degree</asp:ListItem>
<asp:ListItem  Value="Sections">Section</asp:ListItem>
<asp:ListItem  Value="Coll_acronymn">Institution</asp:ListItem>
<asp:ListItem  Value="GateReqReason">Reason</asp:ListItem>
                                            <asp:ListItem  Value="RequestCode">Requisition No</asp:ListItem>
                                            <asp:ListItem  Value="RequestDate">Requisition Date</asp:ListItem>
                                            <%-- <asp:ListItem Selected="True" Value="ReqStaffAppNo">Requested Staff</asp:ListItem>--%>
                                            <%-- <asp:ListItem Selected="True" Value="Stud_Name">Student Name</asp:ListItem>  --%>
                                            <asp:ListItem Value="MemType">Mem Type</asp:ListItem>
                                            <asp:ListItem Value="RequestBy">Request By</asp:ListItem>
                                            <asp:ListItem Value="RequestMode">Request Mode</asp:ListItem>
                                            <asp:ListItem Value="GateReqExitDate" >Exit Date</asp:ListItem>
                                            <asp:ListItem Value="GateReqExitTime" >Exit Time</asp:ListItem>
                                            <asp:ListItem Value="GateReqEntryDate" >Entry Date</asp:ListItem>
                                            <asp:ListItem Value="GateReqEntryTime" >Entry Time</asp:ListItem>
                                            <asp:ListItem Value="ReqAppStatus">Approval Status</asp:ListItem>
                                            <asp:ListItem Value="ReqApproveStage"> Approval Stage</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
 </ContentTemplate>
                        </asp:UpdatePanel>

                        </asp:Panel>

                        
                        <asp:CollapsiblePanelExtender ID="cpecolumnorder4" runat="server" TargetControlID="pcolumnorder4"
                            CollapseControlID="pheaderfilter4" ExpandControlID="pheaderfilter4" Collapsed="true"
                            TextLabelID="lbl_mag" CollapsedSize="0" ImageControlID="Image5" CollapsedImage="right.jpeg"
                            ExpandedImage="down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                        <asp:Label ID="lbl_err_mag" Font-Bold="true" runat="server" ForeColor="Red"></asp:Label>
                        <center>
                            <br />
                            <FarPoint:FpSpread ID="Fpspread6" runat="server" Visible="false" BorderWidth="5px"
                                BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" OnButtonCommand="fpspread6_ButtonCommand">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <div id="GatePassMultipleRequestDiv" runat="server" visible="false">
                                <asp:Button ID="btnGatePassMultipleRequest" runat="server" Text="Approval" CssClass="btn2 textbox textbox1"
                                    OnClick="btnGatePassMultipleRequestClick" />
                            </div>
                            <%--</div>--%>
                            <br />
                        </center>
                    </div>
                    <%--       ***************************all search************************--%>
                    <div id="div_all" runat="server" visible="false">
                        <br />
                        <div>
                            <center>
                                <asp:Panel ID="pheaderfilterall" runat="server" CssClass="maintablestyle" Height="22px"
                                    Width="850px" Style="margin-top: -0.1%;">
                                    <asp:Label ID="Labelfilterall" Text="Column Order" runat="server" Font-Size="Medium"
                                        Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                    <asp:Image ID="Imagefilterall" runat="server" CssClass="cpimage" ImageUrl="right.jpeg"
                                        ImageAlign="Right" />
                                </asp:Panel>
                            </center>
                            <br />
                        </div>
                        <asp:Panel ID="pcolumnorderall" runat="server" CssClass="maintablestyle" Width="850px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox_columnall" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_columnall_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnk_columnorderall" runat="server" Font-Size="X-Small" Height="16px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                            Visible="false" Width="111px" OnClick="LinkButtonsremoveall_Click">Remove  All</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                        <asp:TextBox ID="tborderall" Visible="false" Width="840px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorderall" runat="server" Height="43px" AutoPostBack="true"
                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorderall_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="RequestType">Request Type</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="RequestCode">Requisition No</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="RequestDate">Requisition Date</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="ReqAppNo">Requested Staff/Student</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="MemType">Mem Type</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="cpecolumnorderall" runat="server" TargetControlID="pcolumnorderall"
                            CollapseControlID="pheaderfilterall" ExpandControlID="pheaderfilterall" Collapsed="true"
                            TextLabelID="Labelfilterall" CollapsedSize="0" ImageControlID="Imagefilterall"
                            CollapsedImage="right.jpeg" ExpandedImage="down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                        <asp:Label ID="lbl_all_err" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
                        <br />
                        <FarPoint:FpSpread ID="Fpspread9" runat="server" Visible="false" BorderWidth="5px"
                            BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" OnButtonCommand="fpspread9_ButtonCommand">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <br />
                    </div>
                    <%--  **********************************Event************************--%>
                    <div id="div_event" runat="server" visible="false">
                        <div>
                            <center>
                                <asp:Panel ID="pheaderfilter5" runat="server" CssClass="maintablestyle" Height="22px"
                                    Width="850px" Style="margin-top: -0.1%;">
                                    <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                    <asp:Label ID="lbl_oth" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                        Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                    <asp:Image ID="Image6" runat="server" CssClass="cpimage" ImageUrl="right.jpeg" ImageAlign="Right" />
                                </asp:Panel>
                            </center>
                            <br />
                        </div>
                        <asp:Panel ID="pcolumnorder5" runat="server" CssClass="maintablestyle" Width="850px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox_column5" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column5_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton7" runat="server" Font-Size="X-Small" Height="16px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                            Visible="false" Width="111px" OnClick="LinkButtonsremove5_Click">Remove  All</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                        <asp:TextBox ID="tborder5" Visible="false" Width="840px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder5" runat="server" Height="43px" AutoPostBack="true"
                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder5_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="filename">Attachment</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="RequestCode">Requisition No</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="RequestDate">Requisition Date</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="ReqAppNo">Requested Staff</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="ReqEventName">Event Name</asp:ListItem>
                                            <asp:ListItem Value="ReqFromDate">Request FromDate</asp:ListItem>
                                            <asp:ListItem Value="ReqToDate">Request ToDate</asp:ListItem>
                                            <asp:ListItem Value="ReqLocType">Location Type</asp:ListItem>
                                            <asp:ListItem Value="ReqAppStatus">Request Status</asp:ListItem>
                                            <asp:ListItem Value="ReqApproveStage"> Approval Stage</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="cpecolumnorder5" runat="server" TargetControlID="pcolumnorder5"
                            CollapseControlID="pheaderfilter5" ExpandControlID="pheaderfilter5" Collapsed="true"
                            TextLabelID="lbl_oth" CollapsedSize="0" ImageControlID="Image6" CollapsedImage="right.jpeg"
                            ExpandedImage="down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                        <asp:Label ID="lbl_err_event" Font-Bold="true" runat="server" ForeColor="Red"></asp:Label>
                        <center>
                            <%-- <div id="div7" runat="server" visible="false" style="width: 890px; height: 350px;
                            overflow: auto; border: 1px solid Gray; background-color: White;">--%>
                            <br />
                            <FarPoint:FpSpread ID="Fpspread7" runat="server" Visible="false" BorderWidth="5px"
                                BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" OnButtonCommand="fpspread7_ButtonCommand">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                             <div id="Div7" runat="server" visible="false">
                                <asp:Button ID="Btnevent" runat="server" Text="Approval" CssClass="btn2 textbox textbox1"
                                    OnClick="btnBtneventRequestClick" Visible="false" />
                            </div>
                            <%-- </div>--%>
                            <br />
                        </center>
                    </div>
                    <%--       ***************************certificate request************************--%>
                    <asp:Label ID="lbl_certErr" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                    <div id="div_certificate" runat="server" visible="false">
                        <%--<div>
                            <center>
                                <asp:Panel ID="Panel1" runat="server" CssClass="maintablestyle" Height="22px"
                                    Width="850px" Style="margin-top: -0.1%;">
                                    <asp:Label ID="Label12" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                        Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                    <asp:Image ID="Image1" runat="server" CssClass="cpimage" ImageUrl="right.jpeg" ImageAlign="Right" />
                                </asp:Panel>
                            </center>
                            <br />
                        </div>
                        <asp:Panel ID="Panel2" runat="server" CssClass="maintablestyle" Width="850px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox3" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column1_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" Font-Size="X-Small" Height="16px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                            Visible="false" Width="111px" OnClick="LinkButtonsremove1_Click">Remove  All</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                        <asp:TextBox ID="TextBox9" Visible="false" Width="840px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="CheckBoxList1" runat="server" Height="43px" AutoPostBack="true"
                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder1_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="RequestCode">Requisition No</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="RequestDate">Requisition Date</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="ReqAppNo">Requested Staff</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="VendorCompName">Company Name </asp:ListItem>
                                            <asp:ListItem Selected="True" Value="VenContactName">Person Name</asp:ListItem>
                                            <asp:ListItem Value="Remarks">Remarks</asp:ListItem>
                                            <asp:ListItem Value="ReqExpectedDate">Expected Date</asp:ListItem>
                                            <asp:ListItem Value="ReqAppStatus">Approval Status</asp:ListItem>
                                            <asp:ListItem Value="ReqApproveStage"> Approval Stage</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pcolumnorder1"
                            CollapseControlID="pheaderfilter1" ExpandControlID="pheaderfilter1" Collapsed="true"
                            TextLabelID="lbl_par" CollapsedSize="0" ImageControlID="Image2" CollapsedImage="right.jpeg"
                            ExpandedImage="down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />--%>
                        <center>
                            <FarPoint:FpSpread ID="fp_certificate" runat="server" BorderWidth="5px" BorderStyle="Groove"
                                BorderColor="#0CA6CA" ActiveSheetViewIndex="0" OnButtonCommand="fp_certificate_ButtonCommand"
                                ShowHeaderSelection="false">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <br />
                        </center>
                    </div>


                    
                        <%--    Added By Saranyadevi10.1.2019--%>
             <%--   ************************************ Hostel Request***********************************--%>

              <div id="Div_lib_Req" runat="server" visible="false">
                        <div>
                            <center>
                                <asp:Panel ID="pheaderfilter10" runat="server" CssClass="maintablestyle" Height="22px"
                                    Width="850px" Style="margin-top: -0.1%;">
                                    <asp:Label ID="lbl_st10" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                        Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                    <asp:Image ID="Image1" runat="server" CssClass="cpimage" ImageUrl="right.jpeg" ImageAlign="Right" />
                                </asp:Panel>
                            </center>
                            <br />
                        </div>
                        <asp:Panel ID="pcolumnorder10" runat="server" CssClass="maintablestyle" Width="850px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox_column10" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column10_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton10" runat="server" Font-Size="X-Small" Height="16px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                            Visible="false" Width="111px" OnClick="LinkButtonsremove10_Click">Remove  All</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                        <asp:TextBox ID="tborder10" Visible="false" Width="840px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder10" runat="server" Height="43px" AutoPostBack="true"
                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder10_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="RequestCode">Requisition No</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="RequestDate">Req Date</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="ReqAppNo">Requested Staff</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="RequestBy">Requested Library</asp:ListItem>
                                            <asp:ListItem Value="Remarks">Remarks</asp:ListItem>
                                            <asp:ListItem Value="MemType">MemType</asp:ListItem>
                                            <asp:ListItem Value="ReqExpectedDate">Expected Date</asp:ListItem>
                                            <asp:ListItem Value="RequisitionPK">No Of Book</asp:ListItem>
                                            <asp:ListItem Value="ReqAppStatus">Approval Status</asp:ListItem>
                                            <asp:ListItem Value="ReqApproveStage"> Approval Stage</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="cpecolumnorder10" runat="server" TargetControlID="pcolumnorder10"
                            CollapseControlID="pheaderfilter10" ExpandControlID="pheaderfilter10" Collapsed="true"
                            TextLabelID="lbl_st10" CollapsedSize="0" ImageControlID="Image7" CollapsedImage="right.jpeg"
                            ExpandedImage="down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                        <asp:Label ID="lbl_err_lib" runat="server" ForeColor="Red"></asp:Label>
                        <center>
                          <div id="div_Grid_Library" runat="server" style="display: table; margin: 0px; height: auto; margin-bottom: 20px;
                                        margin-top: 10px; position: relative; width: auto; text-align: left;">
                                        <asp:GridView ID="Showgrid" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                                            HeaderStyle-BackColor="#0CA6CA" AutoGenerateColumns="false" Font-Names="Book Antiqua"
                                            ShowHeaderWhenEmpty="true">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SNo" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="30px">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_snum" runat="server" Visible="true" Text='<%#Eval("SNo") %>'>
                                                            </asp:Label>
                                                               <asp:Label ID="lbl_reqpk" runat="server" Visible="false" Text='<%#Eval("Reqpk") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="30px">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="selectchk" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="30px">
                                                    <ItemTemplate>
                                                        <asp:Button ID="viewbtn" runat="server" Text="View" OnClick="btnedit_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Requisition No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="200px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_RequisitionNo" runat="server" Style="width: auto; text-align: right;"
                                                            Text='<%#Eval("RequisitionNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Req Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_ReqDate" runat="server" Style="width: auto; text-align: right;" Text='<%#Eval("ReqDate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Requested Staff" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="250px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_RequestedStaff" runat="server" Style="width: auto; text-align: right;" Text='<%#Eval("RequestedStaff") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Requested Library" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_RequestedLibrary" runat="server" Style="width: auto; text-align: right;"
                                                            Text='<%#Eval("RequestedBook") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remarks" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Remarks" runat="server" Style="width: auto; text-align: right;" Text='<%#Eval("Remarks") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="MemType" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_MemType" runat="server" Style="width: auto; text-align: right;" Text='<%#Eval("MemType") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Expected Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_ExpectedDate" runat="server" Style="width: auto; text-align: right;"
                                                            Text='<%#Eval("ExpectedDate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Count Of Book" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Count" runat="server" Style="width: auto; text-align: right;"
                                                            Text='<%#Eval("Count") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Approval Status" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="200px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_ApprovalStatus" runat="server" Style="width: auto; text-align: right;"
                                                            Text='<%#Eval("ApprovalStatus") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Approval Stage" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="250px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_ApprovalStage" runat="server" Style="width: auto; text-align: right;"
                                                            Text='<%#Eval("ApprovalStage") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                             
                                            </Columns>
                                            <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium"
                                                Font-Names="Book Antiqua" />
                                            <FooterStyle BackColor="White" ForeColor="#333333" />
                                            <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                            <RowStyle ForeColor="#333333" />
                                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                                        </asp:GridView>
                                    </div>
                        
                        </center>
                    </div>
                    <br />
                     <%--    Added By Saranyadevi10.1.2019--%>
                    <%--   ************************************ Library Request***********************************--%>
                     <asp:Label ID="lbl_err_Host" runat="server" ForeColor="Red"></asp:Label>
                     <br />
                    <div id="Div_Hostel_Vacate" runat="server" visible="false">
                        <div>
                            <center>
                                <asp:Panel ID="pheaderfilter12" runat="server" CssClass="maintablestyle" Height="22px"
                                    Width="850px" Style="margin-top: -0.1%;">
                                    <asp:Label ID="lbl_st12" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                        Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                    <asp:Image ID="Image10" runat="server" CssClass="cpimage" ImageUrl="right.jpeg" ImageAlign="Right" />
                                </asp:Panel>
                            </center>
                            <br />
                        </div>
                        <asp:Panel ID="pcolumnorder12" runat="server" CssClass="maintablestyle" Width="850px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox_column12" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column12_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton12" runat="server" Font-Size="X-Small" Height="16px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                            Visible="false" Width="111px" OnClick="LinkButtonsremove12_Click">Remove  All</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                        <asp:TextBox ID="tborder12" Visible="false" Width="840px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder12" runat="server" Height="43px" AutoPostBack="true"
                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder12_SelectedIndexChanged">
                                            
                                           
                                                    <asp:ListItem Selected="True" Value="Hostel">Hostel Name</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="Roll_No">Roll No</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="Stud_Name">Student Name</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="Degree">Degree</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="Sections">Section</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="Coll_acronymn">Institution</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="HostelReqReason">Reason</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="RequestCode">Requisition No</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="RequestDate">Requisition Date</asp:ListItem>
                                                    <asp:ListItem Value="MemType">Mem Type</asp:ListItem>
                                                    <asp:ListItem Value="ReqAppStatus">Approval Status</asp:ListItem>
                                                    <asp:ListItem Value="ReqApproveStage"> Approval Stage</asp:ListItem>

                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pcolumnorder12"
                            CollapseControlID="pheaderfilter12" ExpandControlID="pheaderfilter12" Collapsed="true"
                            TextLabelID="lbl_st10" CollapsedSize="0" ImageControlID="Image7" CollapsedImage="right.jpeg"
                            ExpandedImage="down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <br />
                       
                        <center>
                            <div id="div_Hostal_Vacate" runat="server" style="display: table; margin: 0px; height: auto;
                                margin-bottom: 20px; margin-top: 10px; position: relative; width: auto; text-align: left;">
                                <asp:GridView ID="ShowGridView" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                                    HeaderStyle-BackColor="#0CA6CA" AutoGenerateColumns="false" Font-Names="Book Antiqua"
                                    ShowHeaderWhenEmpty="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="SNo" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="30px">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Label ID="lbl_snum" runat="server" Visible="true" Text='<%#Eval("SNo") %>'>
                                                    </asp:Label>
                                                    <asp:Label ID="lbl_reqpk" runat="server" Visible="false" Text='<%#Eval("reqpk") %>'>
                                                    </asp:Label>
                                                </center>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="selectchk" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:Button ID="viewbtn" runat="server" Text="View" OnClick="btnedit_Click1" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Hostel Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="200px">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_hosname" runat="server" Style="width: auto; text-align: right;"
                                                    Text='<%#Eval("Hostel") %>'></asp:Label>
                                                    <asp:Label ID="lbl_hospk" runat="server" Visible="false" Style="width: auto; text-align: right;"
                                                    Text='<%#Eval("hospk") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Roll No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="200px">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_RollNo" runat="server" Style="width: auto; text-align: right;"
                                                    Text='<%#Eval("Rollno") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Student Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_StudentName" runat="server" Style="width: auto; text-align: right;"
                                                    Text='<%#Eval("StudentName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Degree" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="250px">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Degree" runat="server" Style="width: auto; text-align: right;"
                                                    Text='<%#Eval("Degree") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Section" HeaderStyle-BackColor="#0CA6CA"
                                            HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Section" runat="server" Style="width: auto; text-align: right;"
                                                    Text='<%#Eval("Section") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Institution" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Institution" runat="server" Style="width: auto; text-align: right;"
                                                    Text='<%#Eval("Institution") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reason" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Reason" runat="server" Style="width: auto; text-align: right;"
                                                    Text='<%#Eval("Reason") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Requisition No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_RequisitionNo" runat="server" Style="width: auto; text-align: right;"
                                                    Text='<%#Eval("RequisitionNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Requisition Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_RequisitionDate" runat="server" Style="width: auto; text-align: right;"
                                                    Text='<%#Eval("RequisitionDate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mem Type" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_MemType" runat="server" Style="width: auto; text-align: right;"
                                                    Text='<%#Eval("MemType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        
                                       
                                        
                                        <asp:TemplateField HeaderText="Approval Status" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="200px">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_ApprovalStatus" runat="server" Style="width: auto; text-align: right;"
                                                    Text='<%#Eval("ApprovalStatus") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Approval Stage" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="250px">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_ApprovalStage" runat="server" Style="width: auto; text-align: right;"
                                                    Text='<%#Eval("ApprovalStage") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium"
                                        Font-Names="Book Antiqua" />
                                    <FooterStyle BackColor="White" ForeColor="#333333" />
                                    <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                    <RowStyle ForeColor="#333333" />
                                    <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                                </asp:GridView>
                            </div>
                        </center>
                    </div>
                    <br />
                    <asp:Button ID="btn_del_stud" Text="Request Cancel" Visible="false" Width="140px"
                        CssClass="btn2 textbox textbox1" runat="server" OnClick="btn_del_stud_Click" />
                    <asp:Button ID="btn_del_stud1" Text="Request Cancel After The Approval" Visible="false"
                        Width="240px" CssClass="btn2 textbox textbox1" runat="server" OnClick="btn1_del_stud_Click" />
                    <asp:Button ID="btn_approval" runat="server" Visible="false" OnClick="Btn_Approval_Click"
                        CssClass="btn2 textbox textbox1" Text="Approval" Font-Bold="true" />
                    <asp:Button ID="btn_reject" runat="server" Visible="false" Text="Reject" OnClick="Btn_Reject_Click"
                        CssClass="btn2 textbox textbox1" Font-Bold="true" />
                    <br />
                    <br />
                    <center>
                        <div id="div_color" runat="server" visible="false">
                            <table class="maindivstyle maintablestyle">
                                <tr>
                                    <td>
                                        <asp:Label ID="gy" runat="server" Width="20px" Height="20px" BackColor="#F0A3CC"></asp:Label>
                                        <asp:Label ID="fill" runat="server" Text="Waiting" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="109px"></asp:Label>
                                        <asp:Label ID="cor" runat="server" Width="20px" Height="20px" BackColor="#A4F9C9"></asp:Label>
                                        <asp:Label ID="partialfill" runat="server" Text="Approved" Font-Bold="True" Font-Names="Book Antiqua"
                                            Width="152px" Font-Size="Medium"></asp:Label>
                                        <asp:Label ID="mis" runat="server" Width="20px" Height="20px" BackColor="#EDF7A3"></asp:Label>
                                        <asp:Label ID="unfill" runat="server" Text="Reject" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="145px"></asp:Label>
                                        <asp:Label ID="lblcan" runat="server" Width="20px" Height="20px" BackColor="#CC66FF"></asp:Label>
                                        <asp:Label ID="lblcancel" runat="server" Text="Cancel" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="145px"></asp:Label>
                                              <asp:Label ID="lblreqestedfordel" runat="server" Width="20px" Height="20px" BackColor="#FFA07A"></asp:Label>
                                                <asp:Label ID="lblreqcanclr" runat="server" Text="Requested For Cancel" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="169px"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                    <br />
                    <center>
                        <asp:Label ID="lbl_norec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False">
                        </asp:Label></center>
                    <div id="div_report" runat="server" visible="false">
                        <center>
                            <asp:Label ID="lbl_reportname" runat="server" Text="Report Name" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="true" OnTextChanged="txtexcelname_TextChanged"
                                CssClass="textbox textbox1 txtheight5" onkeypress="display()"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btn_Excel" runat="server" Text="Export To Excel" Width="150px" CssClass="textbox textbox1 btn2"
                                AutoPostBack="true" OnClick="btnExcel_Click" />
                            <asp:Button ID="btn_printmaster" runat="server" Text="Print" CssClass="textbox textbox1 btn2"
                                AutoPostBack="true" OnClick="btn_printmaster_Click" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </center>
                    </div>
                    <%--      *********************************popup********************--%>
                    <center>
                        <div id="popview" runat="server" class="popupstyle popupheight1" visible="false">
                            <asp:ImageButton ID="imagebtnpop1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                Style="height: 30px; width: 30px; position: absolute; margin-top: 8px; margin-left: 504px;"
                                OnClick="btn_popclose_Click" />
                            <br />
                            <div style="background-color: White; height: 1354px; width: 1046px; border: 5px solid #0CA6CA;
                                border-top: 30px solid #0CA6CA; border-radius: 10px;">
                                <br />
                                <span class="fontstyleheader" style="color: #008000;">Request Approval Details</span>
                                <br />
                                <%--   **************************Item Request****************--%>
                                  <%-- *********************saranyadevi30.11.2018**********************--%>
                                <div id="div_itmreqst" runat="server" visible="false">
                                    <br />
                                    <div class="maindivstyle" align="center" style="border-radius: 7px; width: 400px;
                                        height: 40px; margin-left: 500px;">
                                        <table>
                                            <tr>
                                                <td width="105px">
                                                    Requisition No
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_reqno" runat="server" Width="100px" ReadOnly="true" CssClass="newtextbox  textbox1 txtheight">
                                                    </asp:TextBox>
                                                    <asp:TextBox ID="TextBox2" Visible="false" runat="server" ReadOnly="true" CssClass="newtextbox  textbox1 txtheight">
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_date" Text="Req Date" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="Updp_date" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_date" ReadOnly="false" runat="server" CssClass="newtextbox txtheight textbox2" Width="100px"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_date" runat="server"
                                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <br />
                                    <table>
                                        <tr>
                                            <td width="105px">
                                            </td>
                                            <td colspan="3">
                                                <asp:CheckBox ID="cb_own" runat="server" Text="Own Department" />
                                                <asp:CheckBox ID="cb_other" runat="server" Text="Other Department" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="105px">
                                                Department
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txt_dept" TextMode="SingleLine" ReadOnly="true" onfocus="return myFunction(this)"
                                                    runat="server" Height="20px" CssClass="newtextbox textbox1" Width="300px"></asp:TextBox>
                                                <asp:Button ID="btn_dept" runat="server" Text="?" Width="100px" CssClass="newtextbox btn" OnClick="btn_dept_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="105px">
                                                Remarks
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txt_reqremarks" runat="server" TextMode="MultiLine" CssClass="newtextbox textbox2"
                                                    Height="20px" Width="300px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_reqremarks" runat="server" TargetControlID="txt_reqremarks"
                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=".&,@-_()">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="105px">
                                                Expected Date
                                            </td>
                                            <td width="100px">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_exdate" runat="server" CssClass="newtextbox txtheight textbox2" Width="100px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_exdate" runat="server"
                                                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_additem" runat="server" CssClass="newtextbox btn2" Width="100px" Text="Add Item"
                                                    OnClick="btn_additem_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="griddiv" runat="server" style="width: 850px; height: 300px; margin-left: 21px;"
                                        class="spreadborder">
                                        <asp:GridView ID="SelectdptGrid" runat="server" AutoGenerateColumns="false" Width="800px"
                                            HeaderStyle-BackColor="#0CA6CA" HeaderStyle-ForeColor="White" OnRowDataBound="typegrid_OnRowDataBound"
                                            OnRowCommand="SelectdptGrid_RowCommand">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="cb_select" runat="server"/>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_itemcode" runat="server" Text='<%# Eval("ItemCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_itemname" runat="server" Text='<%# Eval("ItemName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="300px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Measure">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_itemmeasure" runat="server" Text='<%# Eval("Measure") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Quantity">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_quantity" runat="server" Style="text-align: center;" Text='<%# Eval("Quantity") %>'
                                                            Width="80px" onkeyup="QuantityChange(this)" CssClass="newtextbox"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_quantity"
                                                            FilterType="Custom,Numbers" ValidChars=".">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="SuggestedCost/Qty">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_SuggestedCost" runat="server" Style="text-align: center;" Text='<%# Eval("SuggestedCost") %>'
                                                            Width="80px" CssClass="newtextbox"  onkeyup="QuantityChange(this)"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtende" runat="server" TargetControlID="txt_SuggestedCost"
                                                            FilterType="Custom,Numbers" ValidChars=".">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Cost">
                                                    <ItemTemplate>
                                                    <asp:TextBox ID="txt_Cost" runat="server" Enabled="false" Style="text-align: center; background-color:#F0A3CC;" Text='<%# Eval("Cost") %>'
                                                            Width="80px" CssClass="newtextbox" ></asp:TextBox>
                                                  
                                                    
                                                     
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Approved Qty" Visible="false">
                                                    <ItemTemplate>
                                                    <asp:Label ID="lbl_Approvedqty" runat="server" Text='<%# Eval("Approvedqty") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                                 
                                            </Columns>
                                        </asp:GridView>
                                        <br />
                                             <asp:Label ID="lblitemTotal" runat="server" Visible="true" Text="Total Cost:" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"  ForeColor="Green"  style=" margin-left:556px;"></asp:Label>
                                              <asp:Label ID="lblitemcostTotal" runat="server"  Visible="true"  Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"  ForeColor="Green"></asp:Label>

                                    </div>
                                    <br />


                                  <%-- ****************** Added By SaranyaDevi14.2.2019**************************--%>
                        <%--############## Inventory Statge Status######################--%>
                        <%--display: table; margin: 0px; height: 10px; overflow:auto; margin-bottom: 20px; margin-top: 10px; position: relative; width: auto; text-align: left;--%>
                        <center>
                          <div id="divGridInventoryStatus" runat="server" style="height:400px; width:850px; overflow:auto;" >
                                        <asp:GridView ID="GridInventoryStatus" Height="200" Width="800" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                                            HeaderStyle-BackColor="#0CA6CA" AutoGenerateColumns="true" Font-Names="Book Antiqua"
                                            ShowHeaderWhenEmpty="true">
                                            </asp:GridView>
                                           </div>
                                           </center>
                                  <%--***********************  End*********************************--%>

                                    <%--
                    *****************Added By Saranyadevi 5.12.2018**************************--%>
                          <center>
                                            <span id="sp_appstaff_Item"  class="fontstyleheaderrr" runat="server" visible="false" style="color: #008000;">
                                                Approval Permission Staff</span></center>
                                        <asp:GridView ID="grid_Item_approvalstaff" runat="server" Visible="false" AutoGenerateColumns="false"
                                            GridLines="Both" OnRowDataBound="OnRowDataBound_grid_Item_approvalstaff">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl1sno1" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Staff Code" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_code1" runat="server" Text='<%#Eval("StaffCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Staff Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblname1" runat="server" Text='<%#Eval("StaffName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Department" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldept1" runat="server" Text='<%#Eval("Department") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Designation" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldegn1" runat="server" Text='<%#Eval("Designation") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Stage" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblstage1" runat="server" Text='<%#Eval("Stage") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>

                   <%--     ****************End*************************--%>
                   <br />
                                    <asp:Button ID="btn_itm_save" Visible="false" Text="Approval" runat="server" CssClass="textbox textbox1 btn2"
                                        OnClick="btn_itm_save_Click" />
                                    <asp:Button ID="btn_item_reject" Visible="false" Text="Reject" runat="server" CssClass="textbox textbox1 btn2"
                                        OnClick="btn_item_reject_Click" />
                                </div>
                                <%--  ****************************Event***********--%>
                                <div id="div_event_app" runat="server" visible="false">
                                    <br />
                                    <div class="maindivstyle" align="center" style="border-radius: 7px; width: 400px;
                                        height: 40px; margin-left: 500px;">
                                        <table>
                                            <tr>
                                                <td>
                                                    Requisition No
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="rqustn_no_event" runat="server" ReadOnly="true" CssClass="newtextbox  textbox1 txtheight">
                                                    </asp:TextBox>
                                                    <asp:TextBox ID="TextBox8" Visible="false" runat="server" ReadOnly="true" CssClass="newtextbox  textbox1 txtheight">
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label29" Text="Req Date" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="TextBox6" runat="server" Enabled="false" ReadOnly="true" CssClass="newtextbox txtheight textbox2"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="TextBox6" runat="server"
                                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <br />
                                    <div class="maindivstyle">
                                        <br />
                                        <table class="maindivstyle">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblname" runat="server" Style="top: 15px; font-family: 'Book Antiqua'"
                                                        Text="Event Name"> </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtothers" ReadOnly="true" CssClass="textbox textbox1" onfocus="return myFunction(this)"
                                                        runat="server">
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rdb1" runat="server" Enabled="false" Text="Indoor" AutoPostBack="true"
                                                        GroupName="place" OnCheckedChanged="rdb1_CheckedChanged" />
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rdb2" runat="server" Enabled="false" Text="Outdoor" AutoPostBack="true"
                                                        GroupName="place" OnCheckedChanged="rdb2_CheckedChanged" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div id="DIV_indoor" runat="server" visible="false">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_org_batch" Text="Batch" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddl_org_batch" Enabled="false" runat="server" CssClass="ddlheight textbox textbox1"
                                                            OnSelectedIndexChanged="ddl_org_batch_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_degree" Text="Degree" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  textbox1 txtheight6"
                                                            ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_branch" Text="Branch" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_branch" ReadOnly="true" runat="server" CssClass="textbox textbox1 txtheight6"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_org_sem" Text="Semester" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_org_sem" ReadOnly="true" runat="server" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lb_org_staffname" runat="server" Text="Staff Name"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_staffnamemul" ReadOnly="true" runat="server" CssClass="textbox  txtheight6"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="DIV_outdoor" runat="server" visible="false">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_outinstitution" runat="server" Text="Institution"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_outinstitution" runat="server" ReadOnly="true" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_outorganizer" runat="server" Text="Organizer"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_outorganizer" runat="server" ReadOnly="true" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <br />
                                    <br />
                                    <div style="width: 900px; height: 180px; overflow: auto;">
                                        <asp:GridView ID="gridadd" runat="server" Visible="true" AutoGenerateColumns="false"
                                            GridLines="Both">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl1sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txteventdate" ReadOnly="true" runat="server" Text='<%#Eval("Dummy") %>'
                                                            CssClass="textbox txtheight4"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtactname" ReadOnly="true" runat="server" Text='<%#Eval("Dummy1") %>'
                                                            CssClass="textbox txtheight4"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description" HeaderStyle-BackColor="#0CA6CA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_descri" ReadOnly="true" runat="server" Text='<%#Eval("Dummy2") %>'
                                                            CssClass="textbox txtheight5"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Start Time" HeaderStyle-BackColor="#0CA6CA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_start" ReadOnly="true" runat="server" Text='<%#Eval("Dummy3") %>'
                                                            CssClass="textbox txtheight" placeholder="Ex: 12:00:AM"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="End Time" HeaderStyle-BackColor="#0CA6CA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_end" ReadOnly="true" runat="server" Text='<%#Eval("Dummy4") %>'
                                                            CssClass="textbox txtheight" placeholder="Ex: 12:00:PM"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Location" HeaderStyle-BackColor="#0CA6CA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_loc" ReadOnly="true" Text='<%#Eval("Dummay5") %>' runat="server"
                                                            CssClass="textbox txtheight3"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="No Of Person" HeaderStyle-BackColor="#0CA6CA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_noact" ReadOnly="true" Text='<%#Eval("Dummay6") %>' runat="server"
                                                            CssClass="textbox txtheight"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="No Of Conducted Person" HeaderStyle-BackColor="#0CA6CA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_noconper" ReadOnly="true" Text='<%#Eval("Dummay7") %>' runat="server"
                                                            CssClass="textbox txtheight"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <%--
                    ########################pre request########################--%>
                                    <br />
                                    <center>
                                        <asp:Label ID="lbl_prereqheader" runat="server" Text="PreRequest" Visible="false"
                                            ForeColor="#008000" Font-Bold="true" Font-Size="Larger"></asp:Label></center>
                                    <div id="GridView4_div" runat="server" visible="false" style="width: 900px; height: 60px;
                                        overflow: auto;">
                                        <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsno3" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Activity" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtact" ReadOnly="true" Text='<%#Eval("Dummy") %>' runat="server"
                                                            CssClass="textbox txtheight4"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Start Date" HeaderStyle-BackColor="#0CA6CA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_startdate" ReadOnly="true" Text='<%#Eval("Dummy1") %>' runat="server"
                                                            CssClass="textbox txtheight3"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="End Date" HeaderStyle-BackColor="#0CA6CA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_enddate" ReadOnly="true" Text='<%#Eval("Dummy2") %>' runat="server"
                                                            CssClass="textbox txtheight"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description" HeaderStyle-BackColor="#0CA6CA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_actname" ReadOnly="true" Text='<%#Eval("Dummy3") %>' runat="server"
                                                            CssClass="textbox txtheight4"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Represented By" HeaderStyle-BackColor="#0CA6CA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_repsen" ReadOnly="true" Text='<%#Eval("Dummy4") %>' runat="server"
                                                            CssClass="textbox txtheight5"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <br />
                                    <div id="GridView5_div" runat="server" visible="false" style="width: 600px; height: 80px;
                                        overflow: auto; margin-left: -470px">
                                        <center>
                                            <asp:Label ID="lbl_materialreq" runat="server" Text="MaterialsRequest" Visible="false"
                                                ForeColor="#008000" Font-Bold="true" Font-Size="Larger"></asp:Label></center>
                                        <asp:GridView ID="GridView5" runat="server" AutoGenerateColumns="false" GridLines="Both">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsno3" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_name" ReadOnly="true" Text='<%#Eval("Dummy") %>' runat="server"
                                                            CssClass="textbox txtheight"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Quantity" HeaderStyle-BackColor="#0CA6CA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_qty" ReadOnly="true" Text='<%#Eval("Dummy1") %>' runat="server"
                                                            Width="50px" CssClass="textbox txtheight"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Expected" HeaderStyle-BackColor="#0CA6CA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_exp" ReadOnly="true" Text='<%#Eval("Dummy2") %>' runat="server"
                                                            CssClass="textbox txtheight"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Purchase Status" HeaderStyle-BackColor="#0CA6CA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="tx_inmax" ReadOnly="true" Text='<%#Eval("Dummy3") %>' runat="server"
                                                            CssClass="textbox txtheight2"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div style="width: 600px; height: 320px; overflow: auto; margin-left: 399px; margin-top: -2px;">
                                        <center>
                                            <asp:Label ID="lbl_expncreq" runat="server" Text="ExpenseRequest" Visible="false"
                                                ForeColor="#008000" Font-Bold="true" Font-Size="Larger"></asp:Label></center>
                                        <asp:GridView ID="GridView7" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                            Visible="false">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsno4" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Expence Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtcname" ReadOnly="true" Text='<%#Eval("Dummy") %>' runat="server"
                                                            CssClass="textbox txtheight4"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description" HeaderStyle-BackColor="#0CA6CA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtcnt" ReadOnly="true" Text='<%#Eval("Dummy1") %>' runat="server"
                                                            CssClass="textbox txtheight3"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount" HeaderStyle-BackColor="#0CA6CA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtamt1" ReadOnly="true" Text='<%#Eval("Dummy2") %>' runat="server"
                                                            Width="80px" CssClass="textbox txtheight"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div style="width: 900px; height: 120px; overflow: auto; margin-left: -1px; margin-top: -232px;">
                                        <center>
                                            <asp:Label ID="lbl_spncrdetail" runat="server" Text="Sponser Details" Visible="false"
                                                ForeColor="#008000" Font-Bold="true" Font-Size="Larger"></asp:Label></center>
                                        <asp:GridView ID="GridView3" Width="400px" runat="server" AutoGenerateColumns="false"
                                            GridLines="Both">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsno3" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sponser Type" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtdname" ReadOnly="true" Text='<%#Eval("Dummy") %>' runat="server"
                                                            CssClass="textbox txtheight3"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Name" HeaderStyle-BackColor="#0CA6CA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtresource" ReadOnly="true" Text='<%#Eval("Dummy1") %>' Width="290PX"
                                                            runat="server" CssClass="textbox txtheight5"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Resources" HeaderStyle-BackColor="#0CA6CA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtres" ReadOnly="true" Text='<%#Eval("Dummy2") %>' runat="server"
                                                            CssClass="textbox txtheight5"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount" HeaderStyle-BackColor="#0CA6CA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtamt" ReadOnly="true" Text='<%#Eval("Dummy3") %>' runat="server"
                                                            CssClass="textbox txtheight2"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <br />
                                    <center>
                                        <asp:Button ID="btn_app_eventsave" runat="server" Text="Approve" CssClass="textbox textbox1 btn2"
                                            OnClick="btn_app_eventsave_Click" />
                                        <asp:Button ID="btn_event_reject" runat="server" Text="Reject" CssClass="textbox textbox1 btn2"
                                            OnClick="btn_event_reject_Click" />
                                    </center>
                                </div>
                                <%--  *********************************service**********************--%>
                                <center>
                                    <div id="div_service" runat="server" visible="false">
                                        <%-- <span style="color:#008000; font-size:x-large">Service</span>
                  <br />
                  <br />--%>
                                        <br />
                                        <div class="maindivstyle" align="center" style="border-radius: 7px; width: 400px;
                                            height: 40px; margin-left: 500px;">
                                            <table>
                                                <tr>
                                                    <td>
                                                        Requisition No
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_serreqno" runat="server" ReadOnly="true" CssClass="newtextbox  textbox1 txtheight">
                                                        </asp:TextBox>
                                                        <asp:TextBox ID="TextBox3" runat="server" Visible="false" ReadOnly="true" CssClass="newtextbox  textbox1 txtheight">
                                                        </asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_serreqdate" Text="Req Date" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upp_serreqdate" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_serreqdate" runat="server" ReadOnly="true" CssClass="newtextbox txtheight textbox2"></asp:TextBox>
                                                                <asp:CalendarExtender ID="cext_serreqdate" TargetControlID="txt_serreqdate" runat="server"
                                                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                                </asp:CalendarExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <br />
                                        <table>
                                            <tr>
                                                <td>
                                                </td>
                                                <td colspan="3">
                                                    <asp:CheckBox ID="cb_serown" runat="server" Text="Own Department" />
                                                    <asp:CheckBox ID="cb_serother" runat="server" Text="Other Department" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Department
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txt_serdept" TextMode="SingleLine" ReadOnly="true" onfocus="return myFunction(this)"
                                                        runat="server" Height="20px" CssClass="newtextbox textbox1" Width="300px"></asp:TextBox>
                                                    <asp:Button ID="btn_dept1" runat="server" Text="?" CssClass="newtextbox btn" OnClick="btn_dept1_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Remarks
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txt_serremarks" runat="server" TextMode="MultiLine" CssClass="newtextbox textbox2"
                                                        Height="20px" Width="300px"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="ftext_serremarks" runat="server" TargetControlID="txt_serremarks"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=".&,@-_()">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%--Suggested vendor--%>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txt_sersugvendor" runat="server" Visible="false" CssClass="newtextbox  textbox1 txtheight5">
                                                    </asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="ftext_sersugvendor" runat="server" TargetControlID="txt_sersugvendor"
                                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-@&">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:AutoCompleteExtender ID="acext_sersugvendor" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetVendorDet" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_sersugvendor"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%--Suggested Service Location--%>
                                                </td>
                                                <td colspan="3">
                                                    <asp:RadioButton ID="rb_indoor" Visible="false" runat="server" Text="InDoor" GroupName="s2"
                                                        Checked="true" AutoPostBack="true" />
                                                    <asp:RadioButton ID="rb_outdoor" Visible="false" runat="server" Text="OutDoor" GroupName="s2"
                                                        AutoPostBack="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Expected Date
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upp_serexpdate" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_serexpdate" runat="server" CssClass="newtextbox txtheight textbox2"></asp:TextBox>
                                                            <asp:CalendarExtender ID="cext_serexpdate" TargetControlID="txt_serexpdate" runat="server"
                                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_seradditem" runat="server" CssClass="newtextbox btn2" Text="Add Item"
                                                        OnClick="btn_seradditem_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <div id="sergriddiv" runat="server" style="width: 850px; height: 300px; margin-left: 21px;"
                                            class="spreadborder">
                                            <asp:GridView ID="Sergrid" runat="server" AutoGenerateColumns="false" Width="800px"
                                                HeaderStyle-BackColor="#0CA6CA" HeaderStyle-ForeColor="White" OnRowDataBound="sertypegrid_OnRowDataBound"
                                                OnRowCommand="serSelectdptGrid_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cb_select" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Location">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="gridddl_loc" runat="server" AutoPostBack="false">
                                                                <asp:ListItem Text="Indoor" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Outdoor" Value="1"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SuggestVendor">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="gridddl_sugvendor" runat="server" AutoPostBack="false">
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Item Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_seritemcode" runat="server" Text='<%# Eval("ItemCode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Item Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_seritemname" runat="server" Text='<%# Eval("ItemName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="300px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Item Measure">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_seritemmeasure" runat="server" Text='<%# Eval("Measure") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quantity">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_serquantity" runat="server" Style="text-align: center;" Text='<%# Eval("Quantity") %>'
                                                                Width="80px" CssClass="newtextbox"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="Ftext_serqty" runat="server" TargetControlID="txt_serquantity"
                                                                FilterType="Custom,Numbers" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <br />
                                        <asp:Button ID="btn_ser_save" Visible="false" runat="server" Text="Approval" CssClass="textbox textbox1 btn2"
                                            OnClick="btn_ser_save_Click" />
                                        <asp:Button ID="btn_reject_ser_save" Visible="false" runat="server" Text="Reject"
                                            CssClass="textbox textbox1 btn2" OnClick="btn_reject_ser_save_Click" />
                                    </div>
                                </center>
                                <%-- *******end of service*****--%>
                                <center>
                                    <div id="Newdiv" runat="server" visible="false" style="height: 50em; z-index: 100000;
                                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                        left: 0;">
                                        <asp:ImageButton ID="ImageButton1new11" runat="server" Width="40px" Height="40px"
                                            ImageUrl="~/images/close.png" Style="height: 30px; width: 30px; position: absolute;
                                            margin-top: 55px; margin-left: 338px;" OnClick="imagebtnpopclose1new_Click" />
                                        <br />
                                        <br />
                                        <br />
                                        <center>
                                            <div style="background-color: White; height: 500px; width: 700px; border: 5px solid #0CA6CA;
                                                border-top: 30px solid #0CA6CA; border-radius: 10px;">
                                                <br />
                                                <br />
                                                <center>
                                                    <span style="font-size: large; color: Green;">Department Name</span>
                                                </center>
                                                <br />
                                                <div style="overflow: auto; width: 620px; height: 312px; border: 1px solid Gray;">
                                                    <asp:GridView ID="dptgrid" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-ForeColor="White">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblsno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Select">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="cbcheck" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="DeptCode">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbldeptcode" runat="server" Text='<%# Eval("DeptCode") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="DeptName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbldeptname" runat="server" Text='<%# Eval("DeptName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="300px" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                                <br />
                                                <asp:CheckBox ID="cbselectall" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cbselectAll_Change" Style="margin-left: -156px; position: absolute;" />
                                                <asp:Button ID="btndeptsave" runat="server" Text="Save" CssClass="textbox btn2" OnClick="btndept_save" />
                                                <asp:Button ID="btndeptexit" runat="server" Text="Exit" CssClass="textbox btn2" OnClick="btndept_exit" />
                                            </div>
                                        </center>
                                    </div>
                                </center>
                                <%--*********popup for dept*********--%>
                                <center>
                                    <div id="div_visitor" runat="server" visible="false">
                                        <br />
                                        <div class="maindivstyle" align="center" style="border-radius: 7px; width: 400px;
                                            height: 40px; margin-left: 500px;">
                                            <table>
                                                <tr>
                                                    <td>
                                                        Requisition No
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_visitorreqno" runat="server" ReadOnly="true" CssClass="newtextbox  textbox7 txtheight" Width="99px">
                                                        </asp:TextBox>
                                                        <asp:TextBox ID="TextBox4" Visible="false" runat="server" ReadOnly="true" CssClass="newtextbox  textbox1 txtheight">
                                                        </asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_visitorreqdate" Text="Req Date" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upp_visitorreqdate" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_visitorreqdate" runat="server" ReadOnly="true" CssClass="newtextbox textbox7 textbox2" Width="99px"></asp:TextBox>
                                                                <asp:CalendarExtender ID="cext_visitorreqdate" TargetControlID="txt_visitorreqdate"
                                                                    runat="server" CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                                </asp:CalendarExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <br />
                                        <table>
                                            <tr>
                                                <td>
                                                    Company Name
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txt_cname" TextMode="SingleLine" runat="server" Height="20px" onfocus="return myFunction(this)"
                                                        CssClass="newtextbox textbox1" AutoPostBack="true" OnTextChanged="txt_cname_TextChanged"
                                                        Width="300px"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="ftext_cname" runat="server" TargetControlID="txt_cname"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=".&,@-_() ">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:AutoCompleteExtender ID="acext_cname" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetVendComp" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_cname"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="txtsearchpan">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Name
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txt_name" TextMode="SingleLine" runat="server" onfocus="return myFunction(this)"
                                                        Height="20px" CssClass="newtextbox textbox1" Width="300px" AutoPostBack="true"
                                                        OnTextChanged="txt_name_TextChanged" onblur="return getdet(this)"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="ftext_name" runat="server" TargetControlID="txt_name"
                                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=". ">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:AutoCompleteExtender ID="acext_name" runat="server" DelimiterCharacters="" Enabled="True"
                                                        ServiceMethod="GetVendCompDet" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_name"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="txtsearchpan">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Designation
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txt_visitorDesg" Visible="false" TextMode="SingleLine" runat="server"
                                                        Height="20px" CssClass="newtextbox textbox1" Width="300px"></asp:TextBox>
                                                    <asp:Button ID="btn_desgplus" Visible="true" runat="server" Text="+" CssClass="textbox btn"
                                                        Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btn_desgplus_Click" />
                                                    <asp:DropDownList ID="ddl_designation" Visible="true" runat="server" CssClass="textbox textbox1 ddlstyle ddlheight5">
                                                    </asp:DropDownList>
                                                    <asp:Button ID="btn_desgminus" runat="server" Visible="true" Text="-" Font-Size="Medium"
                                                        Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btn_desgminus_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Department
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txt_visitorDept" Visible="false" TextMode="SingleLine" runat="server"
                                                        Height="20px" CssClass="newtextbox textbox1" Width="300px"></asp:TextBox>
                                                    <asp:Button ID="btn_deptplus" runat="server" Visible="true" Text="+" CssClass="textbox btn"
                                                        Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btn_deptplus_Click" />
                                                    <asp:DropDownList ID="ddl_department" runat="server" Visible="true" CssClass="textbox textbox1 ddlstyle ddlheight5">
                                                    </asp:DropDownList>
                                                    <asp:Button ID="btn_deptminus" runat="server" Visible="true" Text="-" Font-Size="Medium"
                                                        Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btn_deptminus_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Phone.No
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txt_visitorph" Visible="true" TextMode="SingleLine" runat="server"
                                                        Height="20px" CssClass="newtextbox textbox8" MaxLength="15" Width="99px"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="ftext_visitorph" runat="server" TargetControlID="txt_visitorph"
                                                        FilterType="numbers,custom" ValidChars="- ">
                                                    </asp:FilteredTextBoxExtender>
                                                    Mobile.No
                                                    <asp:TextBox ID="txt_visitormob" Visible="true" TextMode="SingleLine" runat="server"
                                                        Height="20px" CssClass="newtextbox textbox8" MaxLength="10" Width="99px"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="ftext_visitormob" runat="server" TargetControlID="txt_visitormob"
                                                        FilterType="numbers,custom" ValidChars="- ">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Address
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_address" Visible="true" TextMode="MultiLine" runat="server"
                                                        Height="60px" Width="300px" CssClass="newtextbox textbox1" MaxLength="500">
                                                    </asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" TargetControlID="txt_address"
                                                        FilterType="lowercaseletters,uppercaseletters, numbers,custom" ValidChars="-/@$,. :">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    E-Mail
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_visitoremail" Visible="true" TextMode="SingleLine" runat="server"
                                                        Height="20px" CssClass="newtextbox textbox1" Width="300px"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="ftext_visitoremail" runat="server" TargetControlID="txt_visitoremail"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars="!#$%&'*+-/=?^_`{|}~ @.">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Meet To
                                                </td>
                                                <td colspan="3">
                                                    <asp:CheckBox ID="cb_dept" runat="server" Checked="true" Text="Department" AutoPostBack="true"
                                                        OnCheckedChanged="cb_dept_CheckedChanged" onchange="return checkchange1(this.value)"
                                                        onfocus="return myFunction(this)" />
                                                    <asp:CheckBox ID="cb_individual" runat="server" Text="Individual" AutoPostBack="true"
                                                        onchange="checkchange2(this)" onfocus="return myFunction(this)" OnCheckedChanged="cb_individual_CheckedChanged" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td colspan="3">
                                                    <div id="div_dept" runat="server" visible="false" onfocus="return myFunction(this)">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lbl_dept_to" Text="Department To" runat="server"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_dept_to" runat="server" Width="210px" Height="20px" onfocus="return myFunction(this)"
                                                                        CssClass="textbox1 textbox"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_dept_to"
                                                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                                                    </asp:FilteredTextBoxExtender>
                                                                    <asp:AutoCompleteExtender ID="auto_dept" runat="server" DelimiterCharacters="" Enabled="True"
                                                                        ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                                        CompletionSetCount="10" ServicePath="" TargetControlID="txt_dept_to" CompletionListCssClass="autocomplete_completionListElement"
                                                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                                                    </asp:AutoCompleteExtender>
                                                                    <asp:Button ID="btn_stud_deptto_add" runat="server" Text="Add" CssClass="textbox btn1 textbox1"
                                                                        OnClientClick="return change3();" OnClick="btn_stud_deptto_add_Click" />
                                                                    <asp:Button ID="btn_stud_deptto_rmv" runat="server" Width="55px" Text="Remove" CssClass="textbox btn1 textbox1"
                                                                        OnClientClick="return change31();" OnClick="btn_stud_deptto_rmv_Click" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_to1" runat="server" Visible="false" onfocus="return myFunction(this)"
                                                                        Width="210px" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_to1"
                                                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                                                    </asp:FilteredTextBoxExtender>
                                                                    <asp:AutoCompleteExtender ID="auto_dept1" runat="server" DelimiterCharacters="" Enabled="True"
                                                                        ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                                        CompletionSetCount="10" ServicePath="" TargetControlID="txt_to1" CompletionListCssClass="autocomplete_completionListElement"
                                                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                                                    </asp:AutoCompleteExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lbl_dept_cc" Text="" runat="server"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_dept_cc" runat="server" Width="210px" onfocus="return myFunction(this)"
                                                                        Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txt_dept_cc"
                                                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                                                    </asp:FilteredTextBoxExtender>
                                                                    <asp:AutoCompleteExtender ID="auto_dept2" runat="server" DelimiterCharacters="" Enabled="True"
                                                                        ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                                        CompletionSetCount="10" ServicePath="" TargetControlID="txt_dept_cc" CompletionListCssClass="autocomplete_completionListElement"
                                                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                                                    </asp:AutoCompleteExtender>
                                                                    <asp:Button ID="btn_stud_deptcc_add" runat="server" CssClass="textbox btn1 textbox1"
                                                                        Text="Add" OnClientClick="return change4();" OnClick="btn_stud_deptcc_add_Click" />
                                                                    <asp:Button ID="btn_stud_deptcc_remove" runat="server" Width="55px" CssClass="textbox btn1 textbox1"
                                                                        Text="Remove" OnClientClick="return change41();" OnClick="btn_stud_deptcc_remove_Click" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_cc1" runat="server" Visible="false" onfocus="return myFunction(this)"
                                                                        Width="210px" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txt_cc1"
                                                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                                                    </asp:FilteredTextBoxExtender>
                                                                    <asp:AutoCompleteExtender ID="auto_dept3" runat="server" DelimiterCharacters="" Enabled="True"
                                                                        ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                                        CompletionSetCount="10" ServicePath="" TargetControlID="txt_cc1" CompletionListCssClass="autocomplete_completionListElement"
                                                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                                                    </asp:AutoCompleteExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div id="div_indiv" runat="server" style="margin-left: 11px" visible="false" onfocus="return myFunction(this)">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lbl_indiv" Text="Individual To" runat="server"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_indiv" runat="server" Width="210px" onfocus="return myFunction(this)"
                                                                        Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender55" runat="server" TargetControlID="txt_indiv"
                                                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                                                    </asp:FilteredTextBoxExtender>
                                                                    <asp:AutoCompleteExtender ID="autostudindi1" runat="server" DelimiterCharacters=""
                                                                        Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_indiv"
                                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                        CompletionListItemCssClass="panelbackground">
                                                                    </asp:AutoCompleteExtender>
                                                                    <asp:Button ID="btn_stud_indito_add" runat="server" CssClass="textbox btn1 textbox1"
                                                                        Text="Add" OnClientClick="return change5();" OnClick="btn_stud_indito_add_Click" />
                                                                    <asp:Button ID="btn_stud_indito_rmv" Width="55px" runat="server" CssClass="textbox btn1 textbox1"
                                                                        Text="Remove" OnClientClick="return change51();" OnClick="btn_stud_indito_rmv_Click" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_indiv1" runat="server" Visible="false" onfocus="return myFunction(this)"
                                                                        Width="210px" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txt_indiv1"
                                                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                                                    </asp:FilteredTextBoxExtender>
                                                                    <asp:AutoCompleteExtender ID="autostudindi2" runat="server" DelimiterCharacters=""
                                                                        Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_indiv1"
                                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                        CompletionListItemCssClass="panelbackground">
                                                                    </asp:AutoCompleteExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lbl_indiv_cc" Text="" runat="server"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_indiv_cc" runat="server" Width="210px" onfocus="return myFunction(this)"
                                                                        Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender57" runat="server" TargetControlID="txt_indiv_cc"
                                                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                                                    </asp:FilteredTextBoxExtender>
                                                                    <asp:AutoCompleteExtender ID="autostudindi3" runat="server" DelimiterCharacters=""
                                                                        Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_indiv_cc"
                                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                        CompletionListItemCssClass="panelbackground">
                                                                    </asp:AutoCompleteExtender>
                                                                    <asp:Button ID="btn_stud_indicc_add" runat="server" CssClass="textbox btn1 textbox1"
                                                                        Text="Add" OnClientClick="return change6();" OnClick="btn_stud_indicc_add_Click" />
                                                                    <asp:Button ID="btn_stud_indicc_rmv" runat="server" Width="55px" CssClass="textbox btn1 textbox1"
                                                                        Text="Remove" OnClientClick="return change61();" OnClick="btn_stud_indicc_rmv_Click" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_cc2" runat="server" Visible="false" onfocus="return myFunction(this)"
                                                                        Width="210px" Height="20px" CssClass="textbox1 textbox"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender58" runat="server" TargetControlID="txt_cc2"
                                                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                                                    </asp:FilteredTextBoxExtender>
                                                                    <asp:AutoCompleteExtender ID="autostudindi4" runat="server" DelimiterCharacters=""
                                                                        Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_cc2"
                                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                        CompletionListItemCssClass="panelbackground">
                                                                    </asp:AutoCompleteExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Purpose
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txt_visitorpurpose" runat="server" onfocus="return myFunction(this)"
                                                        Height="20px" Width="300px" TextMode="MultiLine" CssClass="newtextbox textbox2"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="ftext_visitorpurpose" runat="server" TargetControlID="txt_visitorpurpose"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars="&' @.()">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr id="col" runat="server" visible="false">
                                                <td>
                                                    Expected Date
                                                </td>
                                                <td colspan="4">
                                                    <div>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:UpdatePanel ID="upp_visitdate" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox ID="txt_visitdate" runat="server" CssClass="txtcaps newtextbox textbox7 txtheight" 
                                                                                AutoPostBack="true" Width="99px"></asp:TextBox>
                                                                            <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txt_visitdate" runat="server"
                                                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                                            </asp:CalendarExtender>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                                <td>
                                                                    Time
                                                                    <asp:DropDownList ID="ddl_hrs" runat="server" CssClass="txtcaps" Height="25px" Width="50px">
                                                                    </asp:DropDownList>
                                                                    <asp:DropDownList ID="ddl_mins" runat="server" CssClass="txtcaps" Height="25px" Width="50px">
                                                                    </asp:DropDownList>
                                                                    <asp:DropDownList ID="ddl_ampm" runat="server" CssClass="txtcaps" Height="25px" Width="50px">
                                                                        <asp:ListItem>AM</asp:ListItem>
                                                                        <asp:ListItem>PM</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <div id="imgdiv5" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                            left: 0px;">
                                            <center>
                                                <div id="panel_description2" runat="server" visible="false" class="table" style="background-color: White;
                                                    height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                                    margin-top: 200px; border-radius: 10px;">
                                                    <table>
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Label ID="lbl_description3" runat="server" Text="Description" Font-Bold="true"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                <asp:TextBox ID="txt_department" runat="server" Width="200px" Style="font-family: 'Book Antiqua';
                                                                    margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <br />
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Button ID="btn_deptadddesc1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" CssClass="textbox btn1" OnClick="btn_deptadddesc1_Click" />
                                                                <asp:Button ID="btn_deptexitdesc1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" CssClass="textbox btn1" OnClick="btn_deptexitdesc1_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </center>
                                        </div>
                                        <div id="imgdiv6" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                            left: 0px;">
                                            <center>
                                                <div id="panel_description3" runat="server" visible="false" class="table" style="background-color: White;
                                                    height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                                    margin-top: 200px; border-radius: 10px;">
                                                    <table>
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Label ID="lbl_description4" runat="server" Text="Description" Font-Bold="true"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                <asp:TextBox ID="txt_designation" runat="server" Width="200px" Style="font-family: 'Book Antiqua';
                                                                    margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <br />
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Button ID="btn_desgadddesc1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" CssClass="textbox btn1" OnClick="btn_desgadddesc1_Click" />
                                                                <asp:Button ID="btn_desgexitdesc1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" CssClass="textbox btn1" OnClick="btn_desgexitdesc1_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </center>
                                        </div>
                                        <br />
                                        <asp:Button ID="btn_visit_save" Text="Approval" runat="server" CssClass="textbox textbox1 btn2"
                                            OnClick="btn_visit_save_Click" OnClientClick="return DisplayLoadingDiv();" />
                                        <asp:Button ID="btn_visit_reject" Text="Reject" runat="server" CssClass="textbox textbox1 btn2"
                                            OnClick="btn_visit_reject_Click" />
                                        <asp:Button ID="btn_print" Text="Print" runat="server" Visible="false" CssClass="textbox textbox1 btn2"
                                            OnClick="btn_print_Click" />
                                    </div>
                                    <center>
                                        <div id="divImageLoading" runat="server" style="height: 300em; z-index: 100000; width: 100%;
                                            background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;
                                            display: none;">
                                            <center>
                                                <img src="../images/loader.gif" style="margin-top: 320px; height: 50px; border-radius: 10px;" />
                                                <br />
                                                <span style="font-family: Book Antiqua; font-size: Medium; font-weight: bold; color: Black;">
                                                    Processing Please Wait...</span>
                                            </center>
                                        </div>
                                    </center>
                                </center>
                                <%--  ********end of visitor******--%>
                                <center>
                                    <div id="div_complaints" runat="server" visible="false">
                                        <br />
                                        <br />
                                        <div class="maindivstyle" align="center" style="border-radius: 7px; width: 400px;
                                            height: 40px; margin-left: 500px;">
                                            <table>
                                                <tr>
                                                    <td>
                                                        Requisition No
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_compreqno" runat="server" ReadOnly="true" CssClass="newtextbox  textbox1 txtheight">
                                                        </asp:TextBox>
                                                        <asp:TextBox ID="TextBox5" runat="server" Visible="false" ReadOnly="true" CssClass="newtextbox  textbox1 txtheight">
                                                        </asp:TextBox>
                                                    </td>
                                                    <td width="60px">
                                                        <asp:Label ID="lbl_compreqdate" Text="Req Date" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upp_compreqdate" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_compreqdate" runat="server" ReadOnly="true" CssClass="newtextbox txtheight textbox2"></asp:TextBox>
                                                                <asp:CalendarExtender ID="cext_compreqdate" TargetControlID="txt_compreqdate" runat="server"
                                                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                                </asp:CalendarExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <br />
                                        <br />
                                        <table>
                                            <tr>
                                                <td>
                                                    Complaints
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtComplaintsDetails" runat="server" onfocus="return myFunction(this)"
                                                        Rows="15" TextMode="MultiLine" Style="resize: none; height: 80px;" CssClass="newtextbox
        textbox1" Width="300px"></asp:TextBox>
                                                </td>
                                                <td colspan="3">
                                                    <asp:Button ID="btn_compplus" runat="server" Text="+" CssClass="textbox btn" Font-Size="Medium"
                                                        Font-Names="Book Antiqua" Visible="false" OnClick="btn_compplus_Click" />
                                                    <asp:DropDownList ID="ddl_complaints" Visible="false" runat="server" onfocus="return myFunction(this)"
                                                        CssClass="textbox textbox1 ddlstyle ddlheight5">
                                                    </asp:DropDownList>
                                                    <asp:Button ID="btn_compminus" runat="server" Visible="false" Text="-" Font-Size="Medium"
                                                        Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btn_compminus_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Complaints Regarding
                                                </td>
                                                <td colspan="5">
                                                    <asp:TextBox ID="txt_regards" runat="server" onfocus="return myFunction(this)" Rows="15"
                                                        TextMode="MultiLine" Style="resize: none; height: 80px;" CssClass="newtextbox
        textbox1" Width="300px"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="ftext_regards" runat="server" TargetControlID="txt_regards"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars="&' @.()-">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Suggestions
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtComplaintSuggession" runat="server" onfocus="return myFunction(this)"
                                                        Rows="15" TextMode="MultiLine" Style="resize: none; height: 80px;" CssClass="newtextbox
        textbox1" Width="300px"></asp:TextBox>
                                                </td>
                                                <td colspan="3">
                                                    <asp:Button ID="btn_sugplus" Visible="false" runat="server" Text="+" CssClass="textbox btn"
                                                        Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btn_sugplus_Click" />
                                                    <asp:DropDownList ID="ddl_suggestions" Visible="false" runat="server" onfocus="return myFunction(this)"
                                                        CssClass="textbox textbox1 ddlstyle ddlheight5">
                                                    </asp:DropDownList>
                                                    <asp:Button ID="btn_sugminus" runat="server" Visible="false" Text="-" Font-Size="Medium"
                                                        Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btn_sugminus_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <div id="imgdiv3" runat="server" visible="false" style="height: 100em; z-index: 1000;
                                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                            left: 0px;">
                                            <center>
                                                <div id="panel_description" runat="server" visible="false" class="table" style="background-color: White;
                                                    height: 160px; width: 580px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                                    margin-top: 180px; border-radius: 10px;">
                                                    <table>
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Label ID="lbl_description11" runat="server" Text="Description" Font-Bold="true"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="padding: 8px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                <asp:TextBox ID="txt_complaints" Rows="4" TextMode="MultiLine" runat="server" Width="450px"
                                                                    Style="font-family: 'Book Antiqua'; margin-left: 13px; resize: none; height: 50px;"
                                                                    Font-Bold="True" Font-Names="Book
        Antiqua" Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="ftext_complaints" runat="server" TargetControlID="txt_complaints"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars="&' @.()-">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="padding: 8px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Button ID="btn_compadddesc1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" CssClass="textbox btn1" OnClick="btn_compadddesc1_Click" />
                                                                <asp:Button ID="btn_compexitdesc1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" CssClass="textbox btn1" OnClick="btn_compexitdesc1_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </center>
                                        </div>
                                        <div id="imgdiv4" runat="server" visible="false" style="height: 100em; z-index: 1000;
                                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                            left: 0px;">
                                            <center>
                                                <div id="panel_description1" runat="server" visible="false" class="table" style="background-color: White;
                                                    height: 160px; width: 580px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                                    margin-top: 180px; border-radius: 10px;">
                                                    <table>
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Label ID="lbl_description2" runat="server" Text="Description" Font-Bold="true"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="padding: 8px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                <asp:TextBox ID="txt_sggestions" Rows="4" TextMode="MultiLine" runat="server" Width="450px"
                                                                    Style="font-family: 'Book Antiqua'; margin-left: 13px; resize: none; height: 50px;"
                                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="ftext_sggestions" runat="server" TargetControlID="txt_sggestions"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars="&' @.()-">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="padding: 8px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Button ID="btn_sugadddesc1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book
        Antiqua" Font-Size="Medium" CssClass="textbox btn1" OnClick="btn_sugadddesc1_Click" />
                                                                <asp:Button ID="btn_sugexitdesc1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" CssClass="textbox btn1" OnClick="btn_sugexitdesc1_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </center>
                                        </div>
                                        <br />
                                        <br />
                                        <asp:Button ID="btn_comp_save" runat="server" Visible="false" Text="Approval" CssClass="textbox textbox1 btn2"
                                            OnClick="btn_comp_save_Click" />
                                        <asp:Button ID="btn_comp_reject" runat="server" Visible="false" Text="Reject" CssClass="textbox textbox1 btn2"
                                            OnClick="btn_comp_reject_Click" />
                                    </div>
                                </center>
                                <%--  **************end of complaints****************--%>
                                <center>
                                    <div id="div_leavereq" runat="server" visible="false">
                                        <br />
                                        <div class="maindivstyle" align="center" style="border-radius: 7px; width: 400px;
                                            height: 40px; margin-left: 500px;">
                                            <table>
                                                <tr>
                                                    <td>
                                                        Requisition No
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_rqstn_leave" runat="server" ReadOnly="true"  width="100px" CssClass="newtextbox  textbox1 txtheight">
                                                        </asp:TextBox>
                                                        <asp:TextBox ID="TextBox7" Visible="false" runat="server" ReadOnly="true" CssClass="newtextbox  textbox1 txtheight">
                                                        </asp:TextBox>
                                                    </td>
                                                    <td width="50px">
                                                        <asp:Label ID="lbl_rqstn_leave" Width="80px" Text="Req Date" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_time_rqstn_leave" runat="server" ReadOnly="true" width="100px" CssClass="newtextbox txtheight textbox2"></asp:TextBox>
                                                                <asp:CalendarExtender ID="CalendarExtender9" TargetControlID="txt_time_rqstn_leave"
                                                                    runat="server" CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                                </asp:CalendarExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <br />
                                        <br />
                                        <table>
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label13" runat="server" Text="Staff Code"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_staff_code" CssClass="textbox textbox1 txtheight" ReadOnly="true"
                                                                    runat="server" AutoPostBack="true" OnTextChanged="txt_staff_code_TextChanged"></asp:TextBox>
                                                                <asp:AutoCompleteExtender ID="autoindi_indi4" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="Getstaffcode" MinimumPrefixLength="0" CompletionInterval="100"
                                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staff_code"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="panelbackground">
                                                                </asp:AutoCompleteExtender>
                                                                <asp:Button ID="Btn_Staff_Code" Visible="false" runat="server" CssClass="btn textbox textbox1"
                                                                    Text="?" OnClick="Btn_Staff_Code_Click" />
                                                            </td>
                                                            <td style="width: 80px;">
                                                                <asp:Label ID="app_dateee" runat="server" Text="Apply Date"></asp:Label>
                                                            </td>
                                                            <td style="width: 60px;">
                                                                <asp:TextBox ID="txt_applydate" Enabled="false" CssClass="textbox textbox1 txtheight"
                                                                    runat="server"></asp:TextBox>
                                                                <asp:CalendarExtender ID="CalendarExtender6" TargetControlID="txt_applydate" Format="d/MM/yyyy"
                                                                    runat="server">
                                                                </asp:CalendarExtender>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txt_applydate"
                                                                    FilterType="Custom,Numbers" ValidChars="/">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height: 6px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label15" runat="server" Text="Staff Name"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_staff_name" CssClass="textbox textbox1 txtheight4" ReadOnly="true"
                                                                    runat="server" OnTextChanged="txt_staff_name_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="Getstaffname1" MinimumPrefixLength="0" CompletionInterval="100"
                                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staff_name"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="panelbackground">
                                                                </asp:AutoCompleteExtender>
                                                            </td>
                                                            <td rowspan="4" colspan="2">
                                                                <div style="overflow: auto; margin-left: 20px;">
                                                                    <asp:Image ID="imagestaff" runat="server" ImageUrl="" ToolTip="Staff Photo" Style="height: 110px;
                                                                        width: 130px;" />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height: 6px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label22" runat="server" Text="Department"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_dep" ReadOnly="true" runat="server" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100"
                                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_dep"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="panelbackground">
                                                                </asp:AutoCompleteExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height: 6px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label23" runat="server" Text="Designation"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_des" ReadOnly="true" runat="server" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="Getdes" MinimumPrefixLength="0" CompletionInterval="100"
                                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_des"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="panelbackground">
                                                                </asp:AutoCompleteExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height: 6px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label24" runat="server" Text="Leave Type"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddl_leave_type" Enabled="false" CssClass="textbox textbox1 ddlheight3"
                                                                    runat="server">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height: 6px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label25" Visible="false" runat="server" Text="Leave Mode"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="rdlist" runat="server" Text="Full Day" Visible="false" onfocus="return myFunction(this)"
                                                                    onchange="return rbtime(this.value);" GroupName="ii" />
                                                                <asp:RadioButton ID="rdlist1" runat="server" Visible="false" Text="Half Day" GroupName="ii"
                                                                    onfocus="return myFunction(this)" onchange="return rbchange_leave(this.value);" />
                                                                <%--<%--<asp:RadioButtonList ID="rdlist" runat="server" OnSelectedIndexChanged="rdlist_SelectedIndexChanged"
                                                AutoPostBack="false" RepeatDirection="Horizontal" onchange= "rbchange_leave(this)" onfocus="return myFunction(this)">
                                                <asp:ListItem Value="0">Full Day</asp:ListItem>
                                                <asp:ListItem Value="1">Half Day</asp:ListItem>
                                            </asp:RadioButtonList>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl_sess" runat="server" Text="Session" Style="display: none;" Visible="false"
                                                                    onfocus="return myFunction(this)"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddl_sess" Visible="false" CssClass="ddlheight textbox textbox1"
                                                                    runat="server" Style="display: none;" onfocus="return myFunction(this)">
                                                                    <asp:ListItem>Morning</asp:ListItem>
                                                                    <asp:ListItem>Evening</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height: 6px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl_from" runat="server" Text="From Date"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_frm" Enabled="false" runat="server" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                                <asp:CalendarExtender ID="CalendarExtender7" TargetControlID="txt_frm" Format="d/MM/yyyy"
                                                                    runat="server">
                                                                </asp:CalendarExtender>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txt_frm"
                                                                    FilterType="Custom,Numbers" ValidChars="/">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                            <td style="width: 60px;">
                                                                <asp:Label ID="lbl_to" runat="server" Text="To Date" onfocus="return myFunction(this)"></asp:Label>
                                                            </td>
                                                            <td style="width: 60px;">
                                                                <asp:TextBox ID="txt_to" runat="server" Enabled="false" onfocus="return myFunction(this)"
                                                                    CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                                <asp:CalendarExtender ID="CalendarExtender8" TargetControlID="txt_to" Format="d/MM/yyyy"
                                                                    runat="server">
                                                                </asp:CalendarExtender>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txt_to"
                                                                    FilterType="Custom,Numbers" ValidChars="/">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height: 6px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label26" runat="server" Text="Reason"></asp:Label>
                                                            </td>
                                                            <%-- <td colspan="2">
                                                        <asp:DropDownList ID="ddl_leavereason" Enabled="false" CssClass="ddlheight2 textbox textbox1"
                                                            runat="server" onchange="leavereason(this)" onfocus="return myFunction(this)">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txt_reason" runat="server" CssClass="textbox textbox1" Style="display: none;
                                                            float: right;" onfocus="return myFunction(this)"></asp:TextBox>
                                                    </td>--%>
                                                            <td>
                                                                <asp:TextBox ID="txtleavereason" runat="server" Visible="true" Enabled="false" Width="270px"></asp:TextBox>
                                                                <%--  <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txtleavereason"
                                                                    FilterType="LowercaseLetters,UppercaseLetters,Custom,Numbers" ValidChars="/ & ,.">
                                                                </asp:FilteredTextBoxExtender>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height: 6px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <div id="div_GV1" runat="server" visible="false" style="width: 290px; height: 120px;
                                                                    overflow: auto;">
                                                                    <asp:GridView ID="GV1" runat="server" Visible="true" AutoGenerateColumns="false"
                                                                        GridLines="Both" OnRowDataBound="OnRowDataBound_gv1">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbl1sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtdate" ReadOnly="true" runat="server" Text='<%#Eval("Dummy1") %>'
                                                                                        CssClass="textbox txtheight"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Morning" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chk_mrng" runat="server" Checked="true" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Evening" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chk_evng" runat="server" Checked="true" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <HeaderStyle BackColor="#0CA6CA" Font-Bold="True" ForeColor="Black" />
                                                                    </asp:GridView>
                                                                </div>
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:Label ID="lbl_holidayalert" runat="server" ForeColor="Green" Visible="false"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <div style="width: 552px; height: 350px; overflow: auto; margin-left: -4px; margin-top: -73px;">
                                                        <asp:GridView ID="gridViewstaffleave" runat="server" AutoGenerateColumns="true" GridLines="Both"
                                                            RowStyle-HorizontalAlign="Right" OnRowDataBound="gridViewstaffleave_RowDataBound"
                                                            OnDataBound="gridViewstaffleave_databound" OnRowCommand="gridViewstaffleave_RowCommand">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                                    HeaderStyle-Width="50px">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:Label ID="lbl_sno" runat="server" Width="60px" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle BackColor="#0CA6CA" Font-Bold="True" ForeColor="Black" />
                                                        </asp:GridView>
                                                    </div>
                                                    <br />
                                                </td>
                                            </tr>
                                        </table>
                                        <div style="margin-top: -83px; margin-left: 318px; overflow: auto; width: 200;">
                                            <table>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="Label7" runat="server" Width="10px" Height="10px" BackColor="Tomato"></asp:Label>
                                                        <asp:Label ID="Label8" runat="server" Text="Leave Not Available" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium" Width="159px"></asp:Label>
                                                        <asp:Label ID="Label9" runat="server" Width="10px" Height="10px" BackColor="#A4F9C9"></asp:Label>
                                                        <asp:Label ID="Label10" runat="server" Text="Partially Available" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Width="152px" Font-Size="Medium"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_chngreason" runat="server" Text="Remarks"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_reasonchng" runat="server" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" TargetControlID="txt_reasonchng"
                                                            FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" *$%@!.-">
                                                        </asp:FilteredTextBoxExtender>
                                                        <br />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="divaltstf" runat="server" visible="false" style="float: left;">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblaltstf" runat="server" Text="Incharge Staff Name:" Style="font-size: large;
                                                            font-weight: bold; font-family: Book Antiqua;">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblaltstfcode" runat="server" Text="" Style="font-size: large; font-weight: bold;
                                                            font-family: Book Antiqua;">
                                                        </asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <br />
                                        <div id="div5" runat="server" style="width: 960px; overflow: auto;">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label6" runat="server" Text="Last Remark: " ForeColor="green" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label5" Width="800px" runat="server" ForeColor="#B20779" Font-Bold="true"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <br />
                                        <div id="div_altersub" runat="server" style="width: 960px; overflow: auto;">
                                            <center>
                                                <span id="spn1" class="fontstyleheaderrr" visible="false" runat="server" style="color: #008000;">
                                                    Alternate Subject Details</span></center>
                                            <asp:GridView ID="grid_altersub" runat="server" Visible="true" AutoGenerateColumns="false"
                                                GridLines="Both" Width="600px" OnDataBound="grid_altersub_databound" OnRowDataBound="grid_altersub_onrowdatabound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtday0" ReadOnly="true" runat="server" Text='<%#Eval("Dummy0") %>'
                                                                Width="85px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Day-Hour" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtday1" ReadOnly="true" runat="server" Text='<%#Eval("Dummy") %>'
                                                                Width="135px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Staff Code" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtday22" ReadOnly="true" runat="server" Text='<%#Eval("Dummy5") %>'
                                                                Width="150px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Staff" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtday2" ReadOnly="true" runat="server" Text='<%#Eval("Dummy3") %>'
                                                                Width="150px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Subject" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtday3" ReadOnly="true" runat="server" Text='<%#Eval("Dummy4") %>'
                                                                Width="215PX"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Alter Staff Code" HeaderStyle-BackColor="#0CA6CA"
                                                        HeaderStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtday44" ReadOnly="true" runat="server" Text='<%#Eval("Dummy6") %>'
                                                                Width="150px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Alter Staff" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtday4" ReadOnly="true" runat="server" Text='<%#Eval("Dummy2") %>'
                                                                Width="150px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Alter Subject" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtday5" ReadOnly="true" runat="server" Text='<%#Eval("Dummy1") %>'
                                                                Width="225PX"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle BackColor="#0CA6CA" Font-Bold="True" ForeColor="White" />
                                            </asp:GridView>
                                        </div>
                                        <br />
                                        <div style="width: 870px; height: 120px; overflow: auto;">
                                            <center>
                                                <span id="sp_appstaf" class="fontstyleheaderrr" runat="server" visible="false" style="color: #008000;">
                                                    Approval Permission Staff</span></center>
                                            <asp:GridView ID="grid_approvalstaff" runat="server" Visible="true" AutoGenerateColumns="false"
                                                GridLines="Both" OnRowDataBound="OnRowDataBound_grid_approvalstaff">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl1sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Staff Code" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblcode" runat="server" Text='<%#Eval("Dummy0") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Staff Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblname" runat="server" Text='<%#Eval("Dummy") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Department" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbldept" runat="server" Text='<%#Eval("Dummy1") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Designation" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbldegn" runat="server" Text='<%#Eval("Dummy2") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Stage" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblstage" runat="server" Text='<%#Eval("Dummy3") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle BackColor="#0CA6CA" Font-Bold="True" ForeColor="Black" />
                                            </asp:GridView>
                                        </div>
                                        <br />
                                        <div>
                                            <asp:Label ID="lbl_errormsg" Font-Bold="true" ForeColor="Red" runat="server"></asp:Label>
                                        </div>
                                        <br />
                                        <asp:Button ID="batchbtn" Visible="false" runat="server" Font-Bold="True" BorderStyle="None"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Text="Alternate Schedule" CssClass="cursorptr"
                                            ForeColor="Blue" Font-Underline="true" OnClick="altbatch_click" />
                                        <asp:Button ID="Btn_Apply_Leave" runat="server" OnClick="Btn_Apply_Leave_Click" CssClass="btn2 textbox textbox1"
                                            Text="Approval" Font-Bold="true" />
                                        <asp:Button ID="Btn_Cancel" Visible="false" Text="Reject" runat="server" OnClick="Btn_Cancel_Click"
                                            CssClass="btn2 textbox textbox1" Font-Bold="true" />
                                        <asp:Button ID="btnApprCancel" Text="Approve Cancel" runat="server" Visible="false"
                                            OnClick="btnApprCancel_Click" CssClass="btn2 textbox textbox1" Width="150px"
                                            Font-Bold="true" />
                                             <asp:Button ID="btnpartcancel" Text="Selected Date Request Cancel" runat="server" Visible="false"
                                            OnClick="btnselecteddatecancel_Click" CssClass="btn2 textbox textbox1" Width="224px"
                                            Font-Bold="true" />
                                            <asp:Button ID="Request_cancelHierbased" Text="Request For Cancel" runat="server" OnClick="Request_cancelHierbased_Click" CssClass="btn2 textbox textbox1" Width="140px" Font-Bold="true" />
                                        <asp:Button ID="btn_exitpop" Text="Exit" runat="server" OnClick="btn_exitpop_Click"
                                            CssClass="btn2 textbox textbox1" Font-Bold="true" />
                                    </div>
                                </center>
                                <%--  ********end of leave******--%>
                                <div id="div_gate_reqstn" runat="server" visible="false">
                                    <br />
                                    <div class="maindivstyle" align="center" style="border-radius: 7px; width: 400px;
                                        height: 40px; margin-left: 500px;">
                                        <table>
                                            <tr>
                                                <td>
                                                    Requisition No
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_reqtn_gate" runat="server" ReadOnly="true" width="100px" CssClass="newtextbox  textbox1 txtheight">
                                                    </asp:TextBox>
                                                    <asp:TextBox ID="TextBox1" runat="server" Visible="false" ReadOnly="true" CssClass="newtextbox  textbox1 txtheight">
                                                    </asp:TextBox>
                                                </td>
                                                <td width="50px">
                                                    <asp:Label ID="lbl_reqtn_gate" Width="70px" Text="Req Date" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_reqtn_gate_date" runat="server" ReadOnly="true" width="100px" CssClass="newtextbox txtheight textbox2"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender10" TargetControlID="txt_reqtn_gate_date"
                                                                runat="server" CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>

                                                 <%--    *****************************Start Div_Library****************************--%>
                         <center>
                                <div id="div_lib" runat="server" visible="false">
                                    <br />
                                    <div class="maindivstyle" align="center" style="border-radius: 7px; width: 400px;
                                        height: 40px; margin-left: 500px;">
                                        <center>
                                        <table>
                                            <tr>
                                                <td width="105px">
                                                    Requisition No
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Text_lib_ReqNo" runat="server" Width="100px" Enabled="false" ReadOnly="true" CssClass="newtextbox  textbox1 txtheight">
                                                    </asp:TextBox>
                                                      <asp:TextBox ID="TextBoxReqNo" Visible="false" runat="server" ReadOnly="true" CssClass="newtextbox  textbox1 txtheight">
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label32" Text="Req Date" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="Text_lib_date" ReadOnly="false"  Enabled="false" runat="server" CssClass="newtextbox txtheight textbox2" Width="100px"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender15" TargetControlID="Text_lib_date" runat="server"
                                                                Format="dd/MM/yyyy">
                                                                <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                                            </asp:CalendarExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                        </center>
                                    </div>
                                    <br />
                                    <center>
                                        <table>
                                       
                                        <tr>
                                            <td width="105px">
                                                Library Name
                                            </td>
                                            <td colspan="3">
                                               <asp:DropDownList ID="ddl_libname" Visible="true" runat="server"  Enabled="false" CssClass="textbox textbox1 ddlstyle ddlheight5">
                                                </asp:DropDownList>
                                             
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="105px">
                                                Remarks
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txt_reqlibremarks" runat="server"  Enabled="false" TextMode="MultiLine" CssClass="newtextbox textbox2"
                                                    Height="20px" Width="300px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txt_reqlibremarks"
                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=".&,@-_() ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="105px">
                                                Expected Date
                                            </td>
                                            <td width="100px">
                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_libexdate" runat="server"  Enabled="false" CssClass="newtextbox txtheight textbox2"
                                                            AutoPostBack="true" Width="100px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender16" TargetControlID="txt_libexdate" runat="server"
                                                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_Addlib" runat="server" Width="100px"  Enabled="false" CssClass="newtextbox btn2" Text="Add Book" />
                                                   
                                            </td>
                                        </tr>
                                    </table>
                                    </center>
                                    <br />
                                        <div id="Divlibgrid" runat="server" style="width: 950px; height: 300px; margin-left: 21px;"
                                        class="spreadborder">
                                        <asp:GridView ID="SelectlibGrid" runat="server" AutoGenerateColumns="false" Width="900px"
                                            HeaderStyle-BackColor="#0CA6CA" HeaderStyle-ForeColor="White">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_sno1" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="cbl_select" runat="server" Checked="true"  />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Title">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Author">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAuthor" runat="server" Text='<%# Eval("Author") %>'></asp:Label>
                                                        <asp:Label ID="lblbookid" runat="server" Visible="false" Text='<%# Eval("bookid") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="300px" />
                                                </asp:TemplateField>
                                              
                                                 
                                                <asp:TemplateField HeaderText="No.Of Copy">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_copy" runat="server" Style="text-align: center;" Text='<%# Eval("copy") %>'
                                                            Width="80px" CssClass="newtextbox" onkeyup="QuantityChange1(this)"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_copy"
                                                            FilterType="Custom,Numbers" ValidChars=".">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="SuggestedCostPerCopy">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtSuggestedCost" runat="server" Style="text-align: center;" Text='<%# Eval("SuggestedCost") %>'
                                                            Width="80px" CssClass="newtextbox" onkeyup="QuantityChange1(this)"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtende" runat="server" TargetControlID="txtSuggestedCost"
                                                            FilterType="Custom,Numbers" ValidChars=".">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Cost">
                                                    <ItemTemplate>
                                                    <asp:TextBox ID="txtCost" runat="server" Enabled="false" Style="text-align: center; background-color:#F0A3CC;" Text='<%# Eval("Cost") %>'
                                                            Width="80px" CssClass="newtextbox" ></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Approved Copy" Visible="false">
                                                    <ItemTemplate>
                                                    <asp:Label ID="lbl_Approvedqty" runat="server" Text='<%# Eval("Approvedcopy") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                         <br />
                                             <asp:Label ID="lblbookTotal" runat="server" Visible="true" Text="Total Cost:" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"  ForeColor="Green"  style=" margin-left:556px;"></asp:Label>
                                              <asp:Label ID="lblbookcostTotal" runat="server"  Visible="true"  Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"  ForeColor="Green"></asp:Label>
                                  
                                    </div>
                                     
                                  

                                          <center>
                                            <span id="sp_appstaff_Book"  class="fontstyleheaderrr" runat="server" visible="false" style="color: #008000;">
                                                Approval Permission Staff</span></center>
                                        <asp:GridView ID="GridView_BookApprovalstaff" runat="server" Visible="false" AutoGenerateColumns="false"
                                            GridLines="Both" OnRowDataBound="OnRowDataBound_grid_Book_approvalstaff">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl1sno2" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Staff Code" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_staffcode1" runat="server" Text='<%#Eval("StaffCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Staff Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblstaffname1" runat="server" Text='<%#Eval("StaffName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Department" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblstaffdept1" runat="server" Text='<%#Eval("Department") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Designation" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblstaffdegn1" runat="server" Text='<%#Eval("Designation") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Stage" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblstaffstage1" runat="server" Text='<%#Eval("Stage") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                         <br />
                                    <asp:Button ID="btn_book_save" Visible="false" Text="Approval" runat="server" CssClass="textbox textbox1 btn2"
                                        OnClick="btn_book_save_Click" />
                                    <asp:Button ID="btn_book_Reject" Visible="false" Text="Reject" runat="server" CssClass="textbox textbox1 btn2"
                                        OnClick="btn_book_Reject_Click" />
                                          </center>
  </div>
                      <%--  ********************************End Of the Div_Library******************************--%>
                         <%--*****************************Start Div_HostelVacate*************************--%>
                                 <center>
                                 <div id="Div_HostelVacate" runat="server" visible="false">
                                    <br />
                                    <div>
                                     <table>
                                    
                                     <tr>
                                    <td>
                                    <asp:Label ID="Label18" runat="server" Text="Student Name :" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="large" ForeColor="BlueViolet" ></asp:Label>
                                     <asp:Label ID="Labelstdname" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="large"></asp:Label>
                                    </td>
                                    <td></td>
                                     <td>
                                    <asp:Label ID="Label28" runat="server" Text="Roll No:" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="large" ForeColor="BlueViolet"></asp:Label>
                                     <asp:Label ID="LabelRoll" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="large"></asp:Label>
                                    </td>
                                      <td></td>
                                     <td>
                                    <asp:Label ID="Label30" runat="server" Text="Branch Name:" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="large" ForeColor="BlueViolet"></asp:Label>
                                     <asp:Label ID="LabelBranch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="large"></asp:Label>
                                    </td>
                                     
                                     
                                    </tr>
                                    </table>
                                    <br />

                                    <table>
                                   
                                    <tr>
                                    <td>
                                     <asp:Label ID="Hostelname" runat="server" Text="Hostel Name :"></asp:Label>
                                    </td>
                                    <td>
                                     <asp:Label ID="Hostelname1" runat="server"></asp:Label>
                                    </td>
                                    </tr>
                                    <tr>
                                    <td>
                                     <asp:Label ID="Bulid" runat="server" Text="Building Name :"></asp:Label>
                                    </td>
                                    <td>
                                     <asp:Label ID="Bulid1" runat="server"></asp:Label>
                                    </td>
                                    </tr>
                                     <tr>
                                    <td>
                                     <asp:Label ID="LabelFloor" runat="server" Text="Floor Name :"></asp:Label>
                                    </td>
                                    <td>
                                     <asp:Label ID="Floor1" runat="server"></asp:Label>
                                    </td>
                                    </tr>
                                     <tr>
                                    <td>
                                     <asp:Label ID="Labelroom" runat="server" Text="Room Name :"></asp:Label>
                                    </td>
                                    <td>
                                     <asp:Label ID="Labelroom1" runat="server"></asp:Label>
                                    </td>
                                    </tr>
                                    </table>
                                   
                                    </div>
                                  <br />
                                     <div id="Div_HostelVacate_grid" runat="server" style="width: 900px; height: 300px; margin-left: 21px;"
                                            class="spreadborder">
                                            <asp:GridView ID="GridView_HostelVacate" runat="server" AutoGenerateColumns="false" Width="800px"
                                                HeaderStyle-BackColor="#0CA6CA" HeaderStyle-ForeColor="White">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_sno1" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Item Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("Itemcode") %>'></asp:Label>
                                                      <asp:Label ID="lblItempk" runat="server" Visible="false" Text='<%# Eval("Itempk") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Item Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblItem" runat="server" Text='<%# Eval("Item") %>'></asp:Label>

                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Alloted Qty">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAllotedQty" runat="server" Text='<%# Eval("AllotedQty") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Damaged Qty">
                                                        <ItemTemplate>
                                                        <asp:TextBox ID="txt_DamagedQty" runat="server" onchange="DamagedQuantity(this)" Width="100px" Text='<%# Eval("DamagedQty") %>'></asp:TextBox>
                                                       
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    </Columns>
                                                    </asp:GridView>
                                                    </div>

                                                   

                                              <center>
                                              
                                              <div>
                                              <table>
                                              <tr>
                                              <td>
                                              <asp:Label ID="lblhost" runat="server" Visible="false"></asp:Label>
                                               <asp:Label ID="Label31" runat="server" Text="Refund Setting:"></asp:Label>
                                              <asp:RadioButton ID="samehead" runat="server" AutoPostBack="true" Checked="true" Text="Same Header/Ledger"  GroupName="sh" OnCheckedChanged="samehead_OnCheckedChanged" />
                                               <asp:Label ID="Labelcollcode" runat="server" Visible="false"></asp:Label>
                                               <asp:RadioButton ID="otherhead" runat="server" AutoPostBack="true" Text="Other Header/Ledger" GroupName="sh" OnCheckedChanged="otherhead_OnCheckedChanged"  />
                                              
                                              </td>
                                              </tr>
                                              </table>
                                              </div>
                                                <div id="Div8GridViewhead" runat="server" style="width: 900px; height: 300px; margin-left: 21px;"
                                            class="spreadborder">
                                              <asp:GridView ID="GridView6" runat="server" AutoGenerateColumns="false" Width="800px"
                                                HeaderStyle-BackColor="#0CA6CA" HeaderStyle-ForeColor="White">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_sno1" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="selectchk" runat="server" AutoPostBack="true" OnCheckedChanged="GridView6_selectchk"/>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Sem">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSem" runat="server" Text='<%# Eval("Sem") %>'></asp:Label>
                                                    <asp:Label ID="LabelSem1" runat="server" Visible="false" Text='<%# Eval("Sem1") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Header">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHeader" runat="server" Text='<%# Eval("Header") %>'></asp:Label>
                                                    <asp:Label ID="LabelHeader" runat="server" Visible="false" Text='<%# Eval("Header1") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Ledger">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLedger" runat="server" Text='<%# Eval("Ledger") %>'></asp:Label>
                                                             <asp:Label ID="LabelLedger" Visible="false" runat="server" Text='<%# Eval("Ledger1") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Alloted Amt">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAllotedAmt" runat="server" Text='<%# Eval("AllotedAmt") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Paid Amt">
                                                        <ItemTemplate>
                                                          <asp:Label ID="lblPaidAmt" runat="server" Text='<%# Eval("PaidAmt") %>'></asp:Label>
                                                      
                                                       
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="balance Amt">
                                                        <ItemTemplate>
                                                          <asp:Label ID="lblbalanceAmt" runat="server" Text='<%# Eval("balanceAmt") %>'></asp:Label>
                                                      
                                                       
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Breakage Amt">
                                                        <ItemTemplate>
                                                          
                                                        <asp:TextBox ID="txt_BreakageAmt" runat="server" Width="100px" Text='<%# Eval("BreakageAmt") %>'></asp:TextBox>
                                                       
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    
                                                    </Columns>
                                                    </asp:GridView>


        
                                                    </div>
                                                    <div id="newHeader" runat="server" visible="false">
                                                    <table>
                                                    <tr>
                                                    <td>
                                                       <asp:Label ID="lbl_Header" runat="server"  Text="Header :"></asp:Label>
                                                    </td>
                                                    <td>
                                                       <asp:DropDownList ID="ddl_Header" runat="server"  CssClass="textbox textbox1"
                                    Height="30px" Width="230px" OnSelectedIndexChanged="ddl_Header_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                                                    </td>
                                                      <td>
                                                       <asp:Label ID="lbl_Ledger" runat="server"  Text="Ledger :"></asp:Label>
                                                    </td>
                                                    <td>
                                                       <asp:DropDownList ID="ddl_Ledger" runat="server"  CssClass="textbox textbox1"
                                    Height="30px" Width="230px"
                                    >
                                </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                      <asp:Label ID="Label_Allotamt" runat="server"  Text="Alloted Amount :"></asp:Label>
                                                    </td>
                                                    <td>
                                                      <asp:TextBox ID="TextAllotamt" runat="server" Font-Bold="True" Placeholder="0.00"
                                                                CssClass="  textbox txtheight1" Height="17px" Width="110px" MaxLength="6"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" TargetControlID="TextAllotamt"
                                                                FilterType="Numbers,Custom" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>

                                                    </td>
                                                    </tr>
                                                    </table>
                                                  
                                                    </div>
                                             
                                              </center>
                                              <center>
                                            <span id="sp_appstaff_Hostel" class="fontstyleheaderrr" runat="server" visible="false"
                                                style="color: #008000;">Approval Permission Staff</span></center>
                                        <asp:GridView ID="HostelGridView_approvalstaff" runat="server" Visible="false" AutoGenerateColumns="false"
                                            GridLines="Both" OnRowDataBound="OnRowDataBound_HostelGridView_approvalstaff">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl1sno2" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Staff Code" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_staffcode1" runat="server" Text='<%#Eval("StaffCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Staff Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblstaffname1" runat="server" Text='<%#Eval("StaffName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Department" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblstaffdept1" runat="server" Text='<%#Eval("Department") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Designation" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblstaffdegn1" runat="server" Text='<%#Eval("Designation") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Stage" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblstaffstage1" runat="server" Text='<%#Eval("Stage") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <br />
                                        <asp:Button ID="btn_hostelvacate_save" Visible="false" Text="Approval" runat="server" CssClass="textbox textbox1 btn2"
                                            OnClick="btn_hostelvacate_save_Click" />
                                        <asp:Button ID="btn_hostelvacate_Reject" Visible="false" Text="Reject" runat="server" CssClass="textbox textbox1 btn2"
                                            OnClick="btn_hostelvacate_Reject_Click" />
                                            </div>
</center>
                                 <%--************************************End Of The  Div_HostelVacate*********************--%>

                                <br />
                                <center>
                                    <div id="panelrollnopop" runat="server" visible="false">
                                        <div id="div_gatestudview" runat="server" visible="false">
                                            <div id="divreq" style="margin-left: -505px" runat="server" visible="true">
                                                <br />
                                                <center>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                Roll No
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_rollreq" runat="server" Enabled="false" CssClass="textbox textbox1"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Student Name
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_namereq" runat="server" Enabled="false" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Batch Year
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_gatebatch" runat="server" Enabled="false" CssClass="textbox textbox1"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Degree
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_degreq" runat="server" Enabled="false" CssClass="textbox textbox1"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Department
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_deptreq" runat="server" Enabled="false" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Semester
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_semreq" runat="server" Enabled="false" CssClass="textbox textbox1"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Section
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_secreq" runat="server" Enabled="false" CssClass="textbox textbox1"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Apply Date
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_appldatereq" runat="server" CssClass="textbox textbox1" Width="100px"
                                                                    Enabled="false"></asp:TextBox>
                                                                <asp:CalendarExtender ID="calapplreqdt" runat="server" TargetControlID="txt_appldatereq"
                                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                        <tr id="hos1" runat="server">
                                                            <td>
                                                                Hostel Name
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_gatehostel" runat="server" CssClass="textbox textbox1 txtheight5"
                                                                    Width="100px" Enabled="false"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr id="hos2" runat="server">
                                                            <td>
                                                                Building Name
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_gatebuli" runat="server" CssClass="textbox textbox1" Width="100px"
                                                                    Enabled="false"></asp:TextBox>
                                                            </td>
                                                        </tr >
                                                        <tr id="hos3" runat="server">
                                                            <td>
                                                                Floor Name
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_gateflr" runat="server" CssClass="textbox textbox1" Width="100px"
                                                                    Enabled="false"></asp:TextBox>
                                                            </td>
                                                            </tr>
                                                            <tr id="hos4" runat="server">
                                                                <td>
                                                                    Room Name
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_gatermname" runat="server" CssClass="textbox textbox1" Width="100px"
                                                                        Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="hos5" runat="server">
                                                                <td>
                                                                    Room Type
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_gateroom" runat="server" CssClass="textbox textbox1" Width="100px"
                                                                        Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                    </table>
                                                </center>
                                            </div>
                                            <div id="divright" runat="server" visible="false" style="margin-left: 512px; margin-top: -288px">
                                                <div style="background-color: White; height: 400px; width: 306px;">
                                                    <div style="height: 400px; overflow: auto;">
                                                        <asp:GridView ID="grdshow" runat="server" Visible="false" AutoGenerateColumns="false"
                                                            GridLines="Both">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                                    HeaderStyle-Width="">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:Label ID="lbl_sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Month" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                                    HeaderStyle-Width="">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:Label ID="lbl_month" runat="server" Text='<%#Eval("month") %>'></asp:Label>
                                                                            <%-- <asp:Label ID="lbl_monno" runat="server" Text='<%#Eval("monthno") %>'></asp:Label>--%>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Allowed Permission" HeaderStyle-BackColor="#0CA6CA"
                                                                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:Label ID="lblleave" runat="server" Text='<%#Eval("allleave") %>'></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Granted Permission" HeaderStyle-BackColor="#0CA6CA"
                                                                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:Label ID="grantleave" runat="server" Text='<%#Eval("grantleave") %>'></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Remaining Permission" HeaderStyle-BackColor="#0CA6CA"
                                                                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:Label ID="balleave" runat="server" Text='<%#Eval("balleave") %>'></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                            <div style="margin-left: -272px; margin-top: -136px">
                                                <asp:Image ID="image8" runat="server" ToolTip="Student's Photo" ImageUrl="" Style="height: 110px;
                                                    width: 130px;" />
                                            </div>
                                        </div>
                                        <div id="div_gatestaffview" runat="server" visible="false">
                                            <div id="div3" style="margin-left: -505px" runat="server" visible="true">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            Staff Code
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_gatestsffcode" runat="server" CssClass="textbox txtheight textbox1"
                                                                ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Staff Name
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_gatestaffname" runat="server" CssClass="textbox txtheight5 textbox1"
                                                                ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Department
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_gatedeptname" runat="server" CssClass="textbox txtheight5 textbox1"
                                                                ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Designation
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_gatedeign" runat="server" CssClass="textbox txtheight5 textbox1"
                                                                ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div style="margin-left: -452px; margin-top: 14px">
                                                <asp:Image ID="image9" runat="server" ToolTip="Staff's Photo" ImageUrl="" Style="height: 110px;
                                                    width: 130px;" />
                                            </div>
                                            <div id="div4" runat="server" visible="false" style="margin-left: 512px; margin-top: -268px">
                                                <div style="background-color: White; height: 340px; width: 306px;">
                                                    <div style="height: 340px; overflow: auto;">
                                                        <asp:GridView ID="GridView1" runat="server" Visible="false" AutoGenerateColumns="false"
                                                            GridLines="Both">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                                    HeaderStyle-Width="">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:Label ID="lbl_sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Month" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                                    HeaderStyle-Width="">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:Label ID="lbl_month" runat="server" Text='<%#Eval("month") %>'></asp:Label>
                                                                            <%-- <asp:Label ID="lbl_monno" runat="server" Text='<%#Eval("monthno") %>'></asp:Label>--%>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Allowed Permission" HeaderStyle-BackColor="#0CA6CA"
                                                                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:Label ID="lblleave" runat="server" Text='<%#Eval("allleave") %>'></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Granted Permission" HeaderStyle-BackColor="#0CA6CA"
                                                                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:Label ID="grantleave" runat="server" Text='<%#Eval("grantleave") %>'></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Remaining Permission" HeaderStyle-BackColor="#0CA6CA"
                                                                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:Label ID="balleave" runat="server" Text='<%#Eval("balleave") %>'></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="margin-left: 6px; margin-top: 79px">
                                            <asp:Panel ID="paneladd" runat="server" Visible="false">
                                                <center>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblreasongate" runat="server" Text="Reason for Gate Pass"></asp:Label>
                                                                <asp:DropDownList ID="ddlgatepass" runat="server" CssClass="ddlheight5 textbox textbox1"
                                                                    onchange="reason(this)" onfocus="return myFunction(this)">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_ddlgatepassreson" runat="server" Style="display: none; float: right"
                                                                    onfocus="return myFunction(this)" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </center>
                                                <br />
                                                <div style="float: left; width: 550px; margin-left: 01px">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label19" runat="server" Text="Apply Date"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtapply" runat="server" CssClass="textbox textbox1 txtheight1"
                                                                    AutoPostBack="true"> </asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" FilterType="Custom,Numbers"
                                                                    ValidChars="/" runat="server" TargetControlID="txtapply">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:CalendarExtender ID="CalendarExtender11" TargetControlID="txtapply" Format="dd/MM/yyyy"
                                                                    runat="server" Enabled="True">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblrequest" runat="server" Text="Request By"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlrequest" runat="server" CssClass="ddlheight4 textbox textbox1"
                                                                    onchange="req_by(this)" onfocus="return myFunction(this)">
                                                                </asp:DropDownList>
                                                                <asp:TextBox ID="txt_gatepassreq" runat="server" CssClass="textbox textbox1 txtheight5"
                                                                    Style="display: none; float: right;" onfocus="return myFunction(this)"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblrequestmode" runat="server" Text="Request Mode"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlrequestmode" runat="server" CssClass="ddlheight4 textbox textbox1"
                                                                    onchange="reqmode(this)" onfocus="return myFunction(this)">
                                                                </asp:DropDownList>
                                                                <asp:TextBox ID="txt_gatepassreqmode" runat="server" CssClass="textbox textbox1 txtheight5"
                                                                    Style="display: none; float: right;" onfocus="return myFunction(this)"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblissueperson" runat="server" Text="Request Staff"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtissueper" runat="server" CssClass="textbox1 textbox txtheight6">
                                                                </asp:TextBox>
                                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender9" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="Getstaffname" MinimumPrefixLength="0" CompletionInterval="100"
                                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtissueper"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="panelbackground">
                                                                </asp:AutoCompleteExtender>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers"
                                                                    ValidChars=", -." runat="server" TargetControlID="txtissueper">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:Button ID="btnstaff" CssClass="btn textbox textbox1" Text="?" runat="server"
                                                                    OnClick="btnstaff_click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div style="float: left; margin-left: 100px;">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label14" runat="server" Text="Expected Date From"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtfromdate" runat="server" CssClass="textbox textbox1 txtheight1"
                                                                    AutoPostBack="true"> </asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="TextBox1_FilteredTextBoxExtender" FilterType="Custom,Numbers"
                                                                    ValidChars="/" runat="server" TargetControlID="txtfromdate">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="txtfromdate" Format="dd/MM/yyyy"
                                                                    runat="server" Enabled="True">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="labExittime" runat="server" Text="Expected Time Exit"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlhour" Width="50px" runat="server" CssClass="ddlheight textbox textbox1">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="ddlmin" Width="50px" runat="server" CssClass="ddlheight textbox textbox1">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="ddlsession" runat="server" Width="50px" CssClass="ddlheight textbox textbox1">
                                                                    <asp:ListItem>AM</asp:ListItem>
                                                                    <asp:ListItem>PM</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="exp_date_to" runat="server" Text="Expected Date To"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txttodate" runat="server" AutoPostBack="true" CssClass="textbox textbox1 txtheight1"> </asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender51" FilterType="Custom,Numbers"
                                                                    ValidChars="/" runat="server" TargetControlID="txttodate">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:CalendarExtender ID="CalendarExtender41" TargetControlID="txttodate" Format="dd/MM/yyyy"
                                                                    runat="server" Enabled="True">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtstaff_co" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Text="" Style="opacity: 0; height: 0; width: 0;"></asp:TextBox>
                                                                <asp:Label ID="labentrytime" runat="server" Text="Expected Time Entry"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlendhour" runat="server" Width="50px" CssClass="ddlheight2 textbox textbox1">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="ddlendmin" runat="server" Width="50px" CssClass="ddlheight2 textbox textbox1">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="ddlenssession" runat="server" Width="50px" CssClass="ddlheight2 textbox textbox1">
                                                                    <asp:ListItem>AM</asp:ListItem>
                                                                    <asp:ListItem>PM</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div style="float: left; margin-left: 440px;">
                                                    <center>
                                                        <asp:Label ID="lblerror1" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                    </center>
                                                    <br />
                                                    <center>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnnew" Visible="false" runat="server" Text="Clear" OnClick="btnnew_click"
                                                                        CssClass="btn1 textbox textbox1" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnsave" runat="server" Text="Approval" CssClass="btn2 textbox textbox1"
                                                                        OnClick="btnsave_click" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="btn2 textbox textbox1"
                                                                        OnClick="btnReject_click" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="Btnedit" runat="server" Text="Exit" OnClick="Btnedit_click" CssClass="btn1 textbox textbox1" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </center>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </center>
                                <table>
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <%--popwindow1--%>
                                <center>
                                    <div id="Div1" runat="server" visible="false" class="popupstyle popupheight">
                                        <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                            Style="height: 30px; width: 30px; position: absolute; margin-top: 8px; margin-left: 433px;"
                                            OnClick="imagebtnpopclose11_Click" />
                                        <br />
                                        <div class="subdivstyle" style="background-color: White; height: 578px; width: 900px;">
                                            <br />
                                            <div>
                                                <asp:Label ID="lbl_selectitem3" runat="server" Style="font-size: large; color: Green;"
                                                    Text="Select the Item" Font-Bold="true"></asp:Label>
                                            </div>
                                            <br />
                                            <asp:UpdatePanel ID="upp4" runat="server">
                                                <ContentTemplate>
                                                    <table class="maintablestyle">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl_itemheader3" runat="server" Text="Item Header Name"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_itemheader3" runat="server" CssClass="textbox" ReadOnly="true"
                                                                    Width="106px" Height="20px">--Select--</asp:TextBox>
                                                                <asp:Panel ID="p5" runat="server" CssClass="multxtpanel" Style="height: 200px; width: 160px;">
                                                                    <asp:CheckBox ID="cb_itemheader3" runat="server" Text="Select All" AutoPostBack="true"
                                                                        OnCheckedChanged="cb_itemheader3_CheckedChange" />
                                                                    <asp:CheckBoxList ID="cbl_itemheader3" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_itemheader_SelectedIndexChange">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupExt5" runat="server" TargetControlID="txt_itemheader3"
                                                                    PopupControlID="p5" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lbl_subheadername" runat="server" Text="Sub Header Name"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txt_subheadername" runat="server" Height="20px" CssClass="textbox textbox1"
                                                                            ReadOnly="true" Width="120px">--Select--</asp:TextBox>
                                                                        <asp:Panel ID="Panel5" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                                            height: 190px;">
                                                                            <asp:CheckBox ID="cb_subheadername" runat="server" Width="100px" OnCheckedChanged="cb_subheadername_CheckedChange"
                                                                                Text="Select All" AutoPostBack="True" />
                                                                            <asp:CheckBoxList ID="cbl_subheadername" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_subheadername_SelectedIndexChanged">
                                                                            </asp:CheckBoxList>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_subheadername"
                                                                            PopupControlID="Panel5" Position="Bottom">
                                                                        </asp:PopupControlExtender>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lbl_itemtype3" runat="server" Text="Item Name"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="Upp5" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txt_itemname3" runat="server" CssClass="textbox" ReadOnly="true"
                                                                            Width="106px" Height="20px">--Select--</asp:TextBox>
                                                                        <asp:Panel ID="p51" runat="server" CssClass="multxtpanel" Style="height: 300px; width: 200px;">
                                                                            <asp:CheckBox ID="chk_pop2itemtyp" runat="server" Text="Select All" AutoPostBack="true"
                                                                                OnCheckedChanged="chkitemtyp" />
                                                                            <asp:CheckBoxList ID="chklst_pop2itemtyp" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklstitemtyp">
                                                                            </asp:CheckBoxList>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="PopupExt51" runat="server" TargetControlID="txt_itemname3"
                                                                            PopupControlID="p51" Position="Bottom">
                                                                        </asp:PopupControlExtender>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span>Search By</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddl_type" runat="server" CssClass="textbox ddlstyle" Height="30px"
                                                                    OnSelectedIndexChanged="ddl_type_SelectedIndexChanged" AutoPostBack="True">
                                                                    <asp:ListItem Value="0">Item Name</asp:ListItem>
                                                                    <asp:ListItem Value="1">Item Code</asp:ListItem>
                                                                    <asp:ListItem Value="2">Item Header</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_searchby" Visible="false" placeholder="Search Item Name" runat="server"
                                                                    CssClass="textbox textbox1" Height="20px"></asp:TextBox>
                                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="Getnamemm" MinimumPrefixLength="0" CompletionInterval="100"
                                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchby"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="txtsearchpan">
                                                                </asp:AutoCompleteExtender>
                                                                <asp:TextBox ID="txt_searchitemcode" Visible="false" placeholder="Search Item Code"
                                                                    runat="server" CssClass="textbox textbox1"></asp:TextBox>
                                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender99" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="Getitemcode1" MinimumPrefixLength="0" CompletionInterval="100"
                                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchitemcode"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="txtsearchpan">
                                                                </asp:AutoCompleteExtender>
                                                                <asp:TextBox ID="txt_searchheadername" Visible="false" placeholder="Search Item Header"
                                                                    runat="server" CssClass="textbox textbox1"></asp:TextBox>
                                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender77" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="Getitemheader1" MinimumPrefixLength="0" CompletionInterval="100"
                                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchheadername"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="txtsearchpan">
                                                                </asp:AutoCompleteExtender>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btn_go3" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go3_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btn_go3" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <br />
                                            <asp:Label ID="lbl_error3" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                            <center>
                                                <span>Menu Name: </span>
                                                <asp:Label ID="menulbl" runat="server" ForeColor="#0099CC
"></asp:Label></center>
                                            <br />
                                            <div id="div2" runat="server" visible="false" style="width: 850px; height: 318px;
                                                background-color: White;" class="spreadborder">
                                                <div style="width: 550px; float: left;">
                                                    <br />
                                                    <asp:DataList ID="gvdatass" runat="server" Font-Size="Medium" RepeatColumns="4" Width="500px"
                                                        ForeColor="#333333">
                                                        <AlternatingItemStyle BackColor="White" />
                                                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                                        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                                        <ItemStyle BackColor="#E3EAEB" BorderWidth="1px" Height="0px" />
                                                        <ItemTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="CheckBox2" AutoPostBack="true" OnCheckedChanged="selectedmenuchk"
                                                                            runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lbl_itemname" ForeColor="Green" runat="server" Text='<%# Eval("ItemName") %>'></asp:Label>
                                                                        <asp:Label ID="lbl_itemcode" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("ItemCode") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="CheckBox1" Visible="false" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblitemheadername" ForeColor="Green" Visible="false" runat="server"
                                                                            Text='<%# Eval("ItemHeaderName") %>'></asp:Label>
                                                                        <asp:Label ID="lbl_itemheadercode" ForeColor="Red" Visible="false" runat="server"
                                                                            Text='<%# Eval("ItemHeaderCode") %>'></asp:Label>
                                                                        <asp:Label ID="lbl_measureitem" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("ItemUnit") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                        <SelectedItemStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                                    </asp:DataList>
                                                </div>
                                                <div style="width: 200px; float: right;">
                                                    <%--20.10.15--%>
                                                    <br />
                                                    <%--  <br />
                        <br />--%>
                                                    <asp:GridView ID="selectitemgrid" runat="server" HeaderStyle-BackColor="#0CA6CA"
                                                        AutoGenerateColumns="false" HeaderStyle-ForeColor="White">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="snogv" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Item Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="itemnamegv" runat="server" Text='<%# Eval("Item Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle BorderWidth="1px" Width="200px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Item Code" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="itemcodegv" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("ItemCode") %>'> </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%-- <asp:CheckBox ID="CheckBox1" Visible="false" runat="server" />--%>
                                                            <asp:TemplateField HeaderText="Item Headername" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_headername" ForeColor="Green" Visible="false" runat="server" Text='<%# Eval("Header Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Item Headercode" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_itemheadercode" ForeColor="Red" Visible="false" runat="server"
                                                                        Text='<%# Eval("Header code") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Item Unit" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_measureitem" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("Item unit") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%-- <asp:Label ID="itemcodegv" runat="server" Text='<%# Eval("item_code") %>'></asp:Label>--%>
                                                            <%-- </asp:TemplateField>--%>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                            <br />
                                            <center>
                                                <asp:Button ID="btn_itemsave4" runat="server" Text="Save" CssClass="textbox btn2"
                                                    OnClientClick="return valid5()" OnClick="btn_itemsave4_Click" />
                                                <asp:Button ID="btn_conexist4" runat="server" Text="Exit" CssClass="textbox btn2"
                                                    OnClick="btn_conexit4_Click" />
                                            </center>
                                        </div>
                                    </div>
                                </center>
                                <%--  *************************************************************************8--%>
                                <center>
                                    <div id="Div22" runat="server" visible="false" class="popupstyle popupheight">
                                        <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                            Style="height: 30px; width: 30px; position: absolute; margin-top: 26px; margin-left: 340px;"
                                            OnClick="imagebtnpop1close22_Click" />
                                        <br />
                                        <br />
                                        <div style="background-color: White; height: 550px; width: 700px; border: 5px solid #0CA6CA;
                                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                                            <br />
                                            <span style="font-size: large; color: Green;">Select the Staff Name</span>
                                            <br />
                                            <br />
                                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                <ContentTemplate>
                                                    <table class="maintablestyle">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label17" runat="server" Text="College"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlcollege" CssClass="ddlheight4 textbox textbox1" runat="server">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblDepartment" runat="server" Text="Department"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddldepratstaff" CssClass="ddlheight4 textbox textbox1" runat="server"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddldepratstaff_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label20" runat="server" Text="Staff Type">
                                                                </asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddl_stftype" CssClass="ddlheight4 textbox textbox1" runat="server"
                                                                    OnSelectedIndexChanged="ddl_stftype_SelectedIndexChanged" AutoPostBack="true">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label21" runat="server" Text="Designation"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddl_design" CssClass="ddlheight4 textbox textbox1" runat="server"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_design_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblsearchby" runat="server" Text="Staff By" Visible="false"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlstaff" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                                    runat="server" OnSelectedIndexChanged="ddlstaff_SelectedIndexChanged" AutoPostBack="true"
                                                                    Visible="false">
                                                                    <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                                                    <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_search" runat="server" OnTextChanged="txt_search_TextChanged"
                                                                    AutoPostBack="True" Visible="false"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <center>
                                                        <asp:Label ID="ermsg" runat="server" Text="" Visible="false" ForeColor="Red" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                        <FarPoint:FpSpread ID="fsstaff" runat="server" ActiveSheetViewIndex="0" Height="300"
                                                            Width="510" VerticalScrollBarPolicy="AsNeeded" BorderWidth="0.5" Visible="False"
                                                            BorderStyle="Double" OnCellClick="fsstaff_CellClick">
                                                            <CommandBar BackColor="Control" ButtonType="PushButton" Visible="false">
                                                                <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                                                            </CommandBar>
                                                            <Sheets>
                                                                <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="CadetBlue">
                                                                </FarPoint:SheetView>
                                                            </Sheets>
                                                        </FarPoint:FpSpread>
                                                    </center>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <br />
                                            <center>
                                                <asp:Button runat="server" ID="btnstaffadd" CssClass="btn1 textbox textbox1" Text="Ok"
                                                    OnClick="btnstaffadd_Click" />
                                                <asp:Button runat="server" ID="btnexitpop" Text="Exit" CssClass="btn1 textbox textbox1"
                                                    OnClick="exitpop_Click" />
                                            </center>
                                        </div>
                                    </div>
                                </center>
                                <div id="div_save" runat="server" visible="false">
                                    <asp:Button ID="btn_reqsave" runat="server" Visible="true" CssClass="textbox btn2"
                                        Text="Save" OnClick="btn_reqsave_Click" />
                                    <asp:Button ID="btn_reqclear" runat="server" Visible="false" CssClass="textbox btn2"
                                        Text="Clear" OnClick="btn_reqclear_Click" />
                                    <asp:Panel ID="dynamictxt" runat="server" Height="400px" Width="300px">
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </center>
                    <center>
                        <div id="div_CertificatePop" runat="server" class="popupstyle popupheight1" visible="false">
                            <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                Style="height: 30px; width: 30px; position: absolute; margin-top: 8px; margin-left: 471px;"
                                OnClick="btn_popcertificateclose_Click" />
                            <br />
                            <div style="background-color: White; height: 1354px; width: 960px; border: 5px solid #0CA6CA;
                                border-top: 30px solid #0CA6CA; border-radius: 10px;">
                                <br />
                                <span class="fontstyleheader" style="color: #008000;">Certificate Request Approval Details</span>
                                <br />
                                <br />
                                <br />
                                <div class="maindivstyle" align="center" style="border-radius: 7px; width: 400px;
                                    height: 40px; margin-left: 500px;">
                                    <table>
                                        <tr>
                                            <td>
                                                Request No
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_certreqno" runat="server" ReadOnly="true" CssClass="newtextbox  textbox1 txtheight">
                                                </asp:TextBox>
                                                <asp:Label ID="lbl_requestPK" Visible="false" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                Date
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_certreqdate" runat="server" ReadOnly="true" CssClass="newtextbox  textbox1 txtheight">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <table>
                                    <tr>
                                        <td>
                                            Name
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_certname" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="tr_certRollno" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="lbl_certRollnoT" runat="server" Text="Roll No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_certRollno" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="tr_certRegno" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="lbl_certRegnoT" runat="server" Text="Reg No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_certRegno" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="tr_certAdmissionno" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="lbl_certadmissoinnoT" runat="server" Text="Admission No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_certadmissoinno" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Degree
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_certdeg" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <div>
                                    <asp:GridView ID="certificateRequest_grid" runat="server" AutoGenerateColumns="false"
                                        Width="380px" Style="margin-top: 10px;" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-ForeColor="White">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Certificate Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_certificatename" runat="server" Text='<%# Eval("CertificateName") %>'></asp:Label>
                                                    <asp:Label ID="lbl_certificateId" Visible="false" runat="server" Text='<%# Eval("Certificate_ID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="250px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cb_certificaterequest" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EditRowStyle BackColor="#7C6F57" />
                                        <AlternatingRowStyle BackColor="Bisque" />
                                    </asp:GridView>
                                    <br />
                                    <asp:Label ID="lbl_certerror" Visible="false" runat="server" ForeColor="Red"></asp:Label>
                                    <div id="btn_certificateBtn" runat="server" visible="false">
                                        <asp:Button ID="btn_certApp" runat="server" CssClass="textbox btn2" Text="Approval"
                                            OnClick="btn_certApp_Click" />
                                        <%--  <asp:Button ID="btn_certClear" runat="server" CssClass="textbox btn2" Text="Clear"
                                            OnClick="btn_certClear_Click" />--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </center>
                    <%--
                        ****************************  alert ************************--%>
                    <div id="div_del_confm" runat="server" visible="false" style="height: 80em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="div_confm" runat="server" class="table" style="background-color: White;
                                height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                margin-top: 550px; border-radius: 10px;">
                                <table>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Label ID="lbl_del_cnfm" runat="server" Text="Are You Want To Delete This Record?"
                                                    ForeColor="Red" Font-Bold="true">
                                                </asp:Label>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <center>
                                    <asp:Button ID="bt1_delete_ok" CssClass=" textbox btn1 comm" Style="height: 28px;
                                        width: 65px;" OnClick="bt1_delete_ok_Click" Text="Yes" runat="server" />
                                    <asp:Button ID="btn_delete_exit" CssClass=" textbox btn1 comm" Style="height: 28px;
                                        width: 65px;" OnClick="btn_delete_exit_Click" Text="Cancel" runat="server" />
                                </center>
                            </div>
                        </center>
                    </div>
                    <center>
                        <div id="imgdiv2" runat="server" visible="false" style="height: 150em; z-index: 1000;
                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                            left: 0px;">
                            <center>
                                <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 500px;
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
                                                            width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" Font-Bold="true" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </div>
                            </center>
                        </div>
                        <div id="divleavedis" runat="server" visible="false" style="height: 200%; z-index: 1000;
                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                            left: 0px;">
                            <asp:ImageButton ID="img_divleavedis" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                Style="height: 30px; width: 30px; position: absolute; margin-top: 126px; margin-left: 436px;"
                                OnClick="img_divleavedis_Click" />
                            <br />
                            <br />
                            <center>
                                <div id="Div6" runat="server" class="table" style="background-color: White; height: 450px;
                                    width: 900px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 103px;
                                    border-radius: 10px;">
                                    <center>
                                        <table style="height: 100px; width: 100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lbl_leavedis" runat="server" Text="" Style="color: Green;" Font-Bold="true"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lbl_leavedetail" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <div style="width: 875px; height: 350px; overflow: auto;">
                                                        <asp:GridView ID="gridView2" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                                            RowStyle-HorizontalAlign="Right">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:Label ID="lbl_sno" runat="server" Width="50px" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="From Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:Label ID="lbl_fromdate" runat="server" Width="80px" Text='<%#Eval("Dummy1") %>'></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="To Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:Label ID="lbl_todate" runat="server" Width="80px" Text='<%#Eval("Dummy2") %>'></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Full/Half Day" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:Label ID="lbl_fullhalf" runat="server" Width="80px" Text='<%#Eval("Dummy6") %>'></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Morning/Evening" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:Label ID="lbl_mrngeve" runat="server" Width="80px" Text='<%#Eval("Dummy7") %>'></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Reason" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:Label ID="lbl_reason" runat="server" Width="200px" Text='<%#Eval("Dummy3") %>'></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Last Approval Staff" HeaderStyle-BackColor="#0CA6CA"
                                                                    HeaderStyle-HorizontalAlign="center">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:Label ID="lbl_lstappstaff" runat="server" Width="200px" Text='<%#Eval("Dummy4") %>'></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Approval Remarks" HeaderStyle-BackColor="#0CA6CA"
                                                                    HeaderStyle-HorizontalAlign="center">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:Label ID="lbl_remark" runat="server" Width="200px" Text='<%#Eval("Dummy5") %>'></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle BackColor="#0CA6CA" Font-Bold="True" ForeColor="Black" />
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <center>
                                                        <asp:Button ID="btn_leavedisclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                            width: 65px;" OnClick="btn_leavedisclose_Click" Text="ok" runat="server" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </div>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
            <script type="text/javascript">

                function selectpopview() {
                    var stud = document.getElementById("<%=popwindow1.ClientID %>");
                    stud.style.display = "block";

                    return false;
                    /*  function leavereason(id) {
                    var value1 = id.value;

                    if (value1.trim().toUpperCase() == "OTHERS") {
                    var idval = document.getElementById("<%=txtleavereason.ClientID %>");
                    idval.style.display = "block";

                    }
                    else {
                    var idval = document.getElementById("<%=txtleavereason.ClientID %>");
                    idval.style.display = "none";
                    }
                    }*/

                    function reason(id) {
                        var value1 = id.value;

                        if (value1.trim().toUpperCase() == "OTHERS") {
                            var idval = document.getElementById("<%=txt_ddlgatepassreson.ClientID %>");
                            idval.style.display = "block";

                        }
                        else {
                            var idval = document.getElementById("<%=txt_ddlgatepassreson.ClientID %>");
                            idval.style.display = "none";
                        }
                    }

                    function req_by(id) {
                        var value1 = id.value;
                        if (value1.trim().toUpperCase() == "OTHERS") {
                            var idval = document.getElementById("<%=txt_gatepassreq.ClientID %>");
                            idval.style.display = "block";
                        }
                        else {
                            var idval = document.getElementById("<%=txt_gatepassreq.ClientID %>");
                            idval.style.display = "none";
                        }
                    }


                    function reqmode(id) {
                        var value1 = id.value;
                        if (value1.trim().toUpperCase() == "OTHERS") {
                            var idval = document.getElementById("<%=txt_gatepassreqmode.ClientID %>");
                            idval.style.display = "block";
                        }
                        else {
                            var idval = document.getElementById("<%=txt_gatepassreqmode.ClientID %>");
                            idval.style.display = "none";
                        }
                    }
                    function getdet(txt1) {
                        alert('hi')
                        $.ajax({
                            type: "POST",
                            url: "Request.aspx/getData1",
                            data: '{VenContactName: "' + txt1 + '"}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                bindss(response.d);
                            },
                            failure: function (response) {
                                alert(response);
                            }
                        });
                    }
                    function bindss(Employees) {
                        var VenContactDesig = Employees[0].VenContactDesig;
                        document.getElementById('<%=ddl_designation.ClientID %>').value = VenContactDesig;
                        var VenContactDept = Employees[0].VenContactDept;
                        document.getElementById('<%=ddl_department.ClientID %>').value = VenContactDept;
                        var VendorPhoneNo = Employees[0].VendorPhoneNo;
                        document.getElementById('<%=txt_visitorph.ClientID %>').value = VendorPhoneNo;
                        var VendorMobileNo = Employees[0].VendorMobileNo;
                        document.getElementById('<%=txt_visitormob.ClientID %>').value = VendorMobileNo;
                        var VendorEmail = Employees[0].VendorEmail;
                        document.getElementById('<%=txt_visitoremail.ClientID %>').value = VendorEmail;
                    }
                    function valid5() {
                        idval4 = document.getElementById("<%=txt_visitorpurpose.ClientID %>").value;
                        if (idval4.trim() == "") {
                            idval4 = document.getElementById("<%=txt_visitorpurpose.ClientID %>");
                            idval4.style.borderColor = 'Red';
                            empty = "E";
                            if (empty.trim() != "") {
                                return false;
                            }
                            else {
                                return true;
                            }
                        }
                    }


                    function change3() {
                        var idval = document.getElementById("<%=txt_to1.ClientID %>");
                        idval.style.display = "block";
                        return false;
                    }
                    function change31() {
                        var idval = document.getElementById("<%=txt_to1.ClientID %>");
                        idval.style.display = "none";
                        document.getElementById('<%=txt_to1.ClientID %>').value = "";

                        return false;
                    }
                    function change4() {
                        var idval = document.getElementById("<%=txt_cc1.ClientID %>");
                        idval.style.display = "block";

                        return false;
                    }
                    function change41() {
                        var idval = document.getElementById("<%=txt_cc1.ClientID %>");
                        idval.style.display = "none";
                        document.getElementById('<%=txt_cc1.ClientID %>').value = "";
                        return false;
                    }
                    function change5() {
                        var idval = document.getElementById("<%=txt_indiv1.ClientID %>");
                        idval.style.display = "block";
                        return false;
                    }
                    function change51() {
                        var idval = document.getElementById("<%=txt_indiv1.ClientID %>");
                        idval.style.display = "none";
                        document.getElementById('<%=txt_indiv1.ClientID %>').value = "";
                        return false;
                    }
                    function change6() {
                        var idval = document.getElementById("<%=txt_cc2.ClientID %>");
                        idval.style.display = "block";
                        return false;
                    }
                    function change61() {
                        var idval = document.getElementById("<%=txt_cc2.ClientID %>");
                        idval.style.display = "none";
                        document.getElementById('<%=txt_cc2.ClientID %>').value = "";
                        return false;
                    }
                    function checkchange1(id) {
                        if (cb_dept.checked == true) {
                            var idval = document.getElementById("<%=div_dept.ClientID %>");
                            idval.style.display = "block";
                            return false;
                        }
                        if (cb_dept.checked == false) {
                            var idval = document.getElementById("<%=div_dept.ClientID %>");
                            idval.style.display = "none";
                            return false;
                        }
                    }
                    function checkchange2() {
                        if (cb_individual.checked == true) {
                            var idval = document.getElementById("<%=div_indiv.ClientID %>");
                            idval.style.display = "block";
                            return false;
                        }
                        else {
                            var idval = document.getElementById("<%=div_indiv.ClientID %>");
                            idval.style.display = "none";
                            return false;
                        }
                    }
                    function myFunction(x) {

                        x.style.borderColor = "#c4c4c4";
                    }


                    function rbchange_leave1(id) {

                        var value1 = id.value;
                        alter(value1);
                        if (value1.trim().toUpperCase() == "Half Day") {
                            var idval = document.getElementById("<%=ddl_sess.ClientID %>");
                            idval.style.display = "block";

                            var idval1 = document.getElementById("<%=lbl_sess.ClientID %>");
                            idval.style.display = "block";
                        }

                        else {
                            var idval = document.getElementById("<%=ddl_sess.ClientID %>");
                            idval.style.display = "none";

                            var idval1 = document.getElementById("<%=lbl_sess.ClientID %>");
                            idval.style.display = "none";
                        }
                    }

                    function rbchange_leave(id) {
                        if (rdlist1.checked == true) {
                            var idval = document.getElementById("<%=ddl_sess.ClientID %>");
                            idval.style.display = "block";
                            var idval1 = document.getElementById("<%=lbl_sess.ClientID %>");
                            idval1.style.display = "block";
                            var idval2 = document.getElementById("<%=txt_to.ClientID %>");
                            idval2.style.display = "none";
                            var idval3 = document.getElementById("<%=lbl_to.ClientID %>");
                            idval3.style.display = "none";
                            return false;
                        }
                        if (rdlist1.checked == false) {
                            var idval = document.getElementById("<%=ddl_sess.ClientID %>");
                            idval.style.display = "none";
                            var idval1 = document.getElementById("<%=lbl_sess.ClientID %>");
                            idval1.style.display = "none";
                            var idval2 = document.getElementById("<%=txt_to.ClientID %>");
                            idval2.style.display = "block";
                            var idval3 = document.getElementById("<%=lbl_to.ClientID %>");
                            idval3.style.display = "block";
                            return false;
                        }

                    }
                    function rbtime(id) {
                        if (rdlist.checked == true) {
                            var idval2 = document.getElementById("<%=txt_to.ClientID %>");
                            idval2.style.display = "block";
                            var idval3 = document.getElementById("<%=lbl_to.ClientID %>");
                            idval3.style.display = "block";
                            rbchange_leave();
                            return false;
                        }
                    }
                }
            </script>
        </div>
        <div style="height: 1px; width: 1px; overflow: auto;">
            <div id="contentDiv" runat="server" style="height: auto; width: 1344px;">
            </div>
            <</div>
    </body>
</asp:Content>
