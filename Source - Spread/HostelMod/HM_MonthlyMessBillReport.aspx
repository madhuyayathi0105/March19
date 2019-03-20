<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="HM_MonthlyMessBillReport.aspx.cs" Inherits="HM_MonthlyMessBillReport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title></title>
        <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    </head>
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lbl_validation1.ClientID %>').innerHTML = "";
            }
        </script>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <div>
            <center>
                <div>
                    <asp:Label ID="header" runat="server" Text="Monthly Mess Bill Report" CssClass="fontstyleheader"
                        ForeColor="Green"></asp:Label>
                    <br />
                </div>
            </center>
        </div>
        <center>
            <div class="maindivstyle" style="height: 980px; width: 1000px;">
                <%--maincontent--%>
                <center>
                    <div>
                        <br />
                        <table class="maintablestyle" width="989px">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_college" runat="server" Text="College"></asp:Label>
                                </td>
                                <%--<asp:DropDownList ID="ddl_college" runat="server" CssClass="textbox  ddlheight3"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_college_OnSelectedIndexChanged">
                                </asp:DropDownList>--%>
                                <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                   
                                            <asp:TextBox ID="txt_colg" runat="server" CssClass="textbox txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_colg" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                height: 150px;">
                                                <asp:CheckBox ID="cb_colg" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_colg_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_colg" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_colg_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="Pop_colg" runat="server" TargetControlID="txt_colg"
                                                PopupControlID="panel_colg" Position="Bottom">


                                            </asp:PopupControlExtender>
                                              </ContentTemplate>
                                </asp:UpdatePanel>
                                        
                                </td>
                                 <td>
                                    <asp:Label ID="lbl_hostel" runat="server" Text="Hostel Name"></asp:Label>
                                </td>

                                  <td>
                                   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                            <asp:TextBox ID="txt_hostelname" runat="server" CssClass="textbox txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel1" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                height: 150px;">
                                                <asp:CheckBox ID="cb_hostelname" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_hostelname_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_hostelname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_hostelname_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_hostelname"
                                                PopupControlID="panel1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                              </ContentTemplate>
                                </asp:UpdatePanel>
                                       
                                </td>
                                <td>
                                    <asp:Label ID="lbl_hostelname" runat="server" Style="top: 10px; left: 6px;" Text="Mess Name"></asp:Label>
                                </td>
                                <td>
                              <asp:UpdatePanel ID="UPHostelName" runat="server">
                                    <ContentTemplate>
                                    <asp:DropDownList ID="ddl_messname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_messname_SelectedIndexChanged"
                                        CssClass="textbox  ddlheight2">
                                    </asp:DropDownList> 
                                      </ContentTemplate>
                                </asp:UpdatePanel>
                                </td>
                                <%--    <td>
                                <asp:UpdatePanel ID="UPHostelName" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_hostelname" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                            onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_hostelname" runat="server" CssClass="multxtpanel" Style="width: 160px;
                                            height: 200px;">
                                            <asp:CheckBox ID="cb_hostel" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_hostel_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_hostel" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_hostel_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popHostelName" runat="server" TargetControlID="txt_hostelname"
                                            PopupControlID="panel_hostelname" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
                                <td>
                                    <asp:Label ID="lbl_building" runat="server" Style="top: 10px; left: 244px;" Text="Building"> </asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UPBuilding" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_building" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_building" runat="server" CssClass="multxtpanel" Style="width: 128px;
                                                height: 200px;">
                                                <asp:CheckBox ID="cb_building" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_building_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_building" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_building_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popBuilding" runat="server" TargetControlID="txt_building"
                                                PopupControlID="panel_building" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_floor" runat="server" Style="top: 8px; left: 490px;" Text="Floor"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UPFloor" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_floor" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_floor" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                height: 200px;">
                                                <asp:CheckBox ID="cb_floor" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_floor_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_floor" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_floor_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popFloor" runat="server" TargetControlID="txt_floor"
                                                PopupControlID="panel_floor" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_batch" runat="server" Text="Batch"></asp:Label>
                                </td>
                                <td>
                                    <%--                            <asp:DropDownList ID="ddl_batch" runat="server" CssClass="textbox  ddlheight3" AutoPostBack="true"
                                OnSelectedIndexChanged="ddl_batch_OnSelectedIndexChanged">
                            </asp:DropDownList>--%>
                                    <asp:UpdatePanel ID="UP_batch" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_batch" runat="server" CssClass="textbox txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel" Style="width: 100px;
                                                height: 200px;">
                                                <asp:CheckBox ID="cb_batch" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_batch_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_batch_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="pce_batch" runat="server" TargetControlID="txt_batch"
                                                PopupControlID="panel_batch" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_degree" runat="server" Text="Degree"></asp:Label>
                                </td>
                                <td>
                                    <%--                            <asp:DropDownList ID="ddl_degree" runat="server" CssClass="textbox  ddlheight3" AutoPostBack="true"
                                OnSelectedIndexChanged="ddl_degree_OnSelectedIndexChanged">
                            </asp:DropDownList>--%>
                                    <asp:UpdatePanel ID="UP_degree" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox txtheight2" ReadOnly="true">--Select--</asp:TextBox>
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
                                    <asp:Label ID="lbl_dept" runat="server" Text="Department"></asp:Label>
                                </td>
                                <td>
                                    <%--                            <asp:DropDownList ID="ddl_dept" runat="server" CssClass="textbox  ddlheight3" AutoPostBack="true"
                                OnSelectedIndexChanged="ddl_dept_OnSelectedIndexChanged">
                            </asp:DropDownList>--%>
                                    <asp:UpdatePanel ID="Up_dept" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox txtheight2" ReadOnly="true">--Select--</asp:TextBox>
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
                                <td>
                                    <asp:Label ID="lbl_gender" runat="server" Text="Gender"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Up_gender" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_gender" runat="server" CssClass="textbox txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_gender" runat="server" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_gender" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_gender_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_gender" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_gender_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">Male</asp:ListItem>
                                                    <asp:ListItem Value="1">Female</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="Pce_gender" runat="server" TargetControlID="txt_gender"
                                                PopupControlID="panel_gender" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_month" runat="server" Text="Month&Year"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Up_month" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_month" runat="server" CssClass="textbox " ReadOnly="true" Width="57px"
                                                Height="20px">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_month" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                                width: 120px;">
                                                <asp:CheckBox ID="cb_month" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_month_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_month" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_month_SelectedIndexChanged">
                                                    <asp:ListItem Value="1">January</asp:ListItem>
                                                    <asp:ListItem Value="2">February</asp:ListItem>
                                                    <asp:ListItem Value="3">March</asp:ListItem>
                                                    <asp:ListItem Value="4">April</asp:ListItem>
                                                    <asp:ListItem Value="5">May</asp:ListItem>
                                                    <asp:ListItem Value="6">June</asp:ListItem>
                                                    <asp:ListItem Value="7">July</asp:ListItem>
                                                    <asp:ListItem Value="8">August</asp:ListItem>
                                                    <asp:ListItem Value="9">September</asp:ListItem>
                                                    <asp:ListItem Value="10">October</asp:ListItem>
                                                    <asp:ListItem Value="11">November</asp:ListItem>
                                                    <asp:ListItem Value="12">December</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="Pop_month" runat="server" TargetControlID="txt_month"
                                                PopupControlID="panel_month" Position="Bottom">
                                            </asp:PopupControlExtender>
                                            <asp:TextBox ID="txt_year" runat="server" CssClass="textbox " ReadOnly="true" Height="20px"
                                                Width="46px">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_year" runat="server" CssClass="multxtpanel" Style="width: 100px;
                                                height: 200px;">
                                                <asp:CheckBox ID="cb_year" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_year_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_year" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_year_SelectedIndexChanged">
                                                  <%--  <asp:ListItem Value="0">2017</asp:ListItem>
                                                    <asp:ListItem Value="1">2016</asp:ListItem>
                                                    <asp:ListItem Value="2">2015</asp:ListItem>
                                                    <asp:ListItem Value="3">2014</asp:ListItem>
                                                    <asp:ListItem Value="4">2013</asp:ListItem>
                                                    <asp:ListItem Value="5">2012</asp:ListItem>
                                                    <asp:ListItem Value="6">2011</asp:ListItem>
                                                    <asp:ListItem Value="7">2010</asp:ListItem>--%>
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="Pop_year" runat="server" TargetControlID="txt_year"
                                                PopupControlID="panel_year" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td colspan="2" class="maindivstyle">
                                    <asp:RadioButton ID="rdb_fromat1" Text="Format-I" runat="server" GroupName="as1"
                                        AutoPostBack="true" OnCheckedChanged="rdb_fromat1_CheckedChange" />
                                    <%-- </td>
                            <td>--%>
                                    <asp:RadioButton ID="rdb_fromat2" Text="Format-II" runat="server" GroupName="as1"
                                        AutoPostBack="true" OnCheckedChanged="rdb_fromat2_CheckedChange" />
                                </td>
                                <td colspan="2" class="maindivstyle">
                                    <asp:RadioButton ID="rdb_student" Text="Student" runat="server" GroupName="as" AutoPostBack="true"
                                        OnCheckedChanged="rdb_student_CheckedChange" />
                                    <asp:RadioButton ID="rdb_staff" Text="Staff" runat="server" GroupName="as" AutoPostBack="true"
                                        OnCheckedChanged="rdb_staff_CheckedChange" />
                                    <asp:RadioButton ID="rdb_guest" Text="Guest" runat="server" GroupName="as" AutoPostBack="true"
                                        OnCheckedChanged="rdb_guest_CheckedChange" />
                                </td>
                                <td>
                                    <asp:Label ID="lbl_rollno" runat="server" Text="Roll No" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_rollno" TextMode="SingleLine" runat="server" Height="20px" CssClass="textbox textbox1"
                                        Width="261px" AutoPostBack="true" Visible="false"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getroll1" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td colspan="2">
                                    <asp:RadioButton ID="rdb_common" Text="Common" runat="server" AutoPostBack="true"
                                        GroupName="co1" OnCheckedChanged="rdb_common_CheckedChange" Visible="false" />
                                    <%-- </td>
                            <td>--%>
                                    <asp:RadioButton ID="rdb_indivual" Text="Individual" runat="server" AutoPostBack="true"
                                        GroupName="co1" OnCheckedChanged="rdb_indivual_CheckedChange" Visible="false" />
                                </td>
                                <%--   </td>
                            <td>--%>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <%--29.12.16--%>
                                    <asp:RadioButton ID="rdb_paid" Text="Paid" runat="server" GroupName="p" AutoPostBack="true"
                                        OnCheckedChanged="rdb_paid_CheckedChange" />
                                    <asp:RadioButton ID="rdb_unpaid" Text="Not Paid" runat="server" GroupName="p" AutoPostBack="true"
                                        OnCheckedChanged="rdb_unpaid_CheckedChange" />
                                    <asp:RadioButton ID="rdb_yettobepaid" Text="Yet to be Paid" runat="server" GroupName="p"
                                        AutoPostBack="true" OnCheckedChanged="rdb_yettobepaid_CheckedChange" />
                                    <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                                    <%--29.12.16 end--%>
                                    <asp:RadioButton ID="rdb_detailedwise" Text="Detail Wise" runat="server" GroupName="as2"
                                        AutoPostBack="true" OnCheckedChanged="rdb_detailedwise_CheckedChange" Visible="false" />
                                    <asp:RadioButton ID="rdb_monthwise" Text="Month Wise" runat="server" GroupName="as2"
                                        AutoPostBack="true" OnCheckedChanged="rdb_monthwise_CheckedChange" Visible="false" />
                                </td>
                                <td>
                                    <asp:Label ID="lbl_pop1staffname" Text="Staff Name" runat="server" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_pop1staffname" runat="server" CssClass="textbox textbox1" AutoPostBack="true"
                                        Visible="false" Width="252px"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_pop1staffname"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_guestname" Text="Guest Name" runat="server" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_guest" runat="server" CssClass="textbox textbox1" AutoPostBack="true"
                                        Visible="false" Width="216px"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetguestName" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_guest"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                    <%--</td>
                              
                                <td>--%>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <center>
                            <div>
                                <asp:Label ID="lbl_error" Visible="false" runat="server" ForeColor="red"></asp:Label>
                            </div>
                        </center>
                    </div>
                    <br />
                    <div>
                        <center>
                            <asp:Panel ID="pheaderfilter" runat="server" CssClass="maintablestyle" Height="22px"
                                Width="970px" Style="margin-top: -0.1%;">
                                <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                    Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </center>
                        <center>
                            <asp:Panel ID="pheaderfilter1" runat="server" CssClass="maintablestyle" Height="22px"
                                Width="889px">
                                <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                <asp:Label ID="Label1" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                    Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                <asp:Image ID="Image2" runat="server" CssClass="cpimage" ImageUrl="~/images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </center>
                        <center>
                            <asp:Panel ID="pheaderfilterstu" runat="server" CssClass="maintablestyle" Height="22px"
                                Width="889px">
                                <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                <asp:Label ID="Label2" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                    Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                <asp:Image ID="Image3" runat="server" CssClass="cpimage" ImageUrl="~/images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </center>
                        <center>
                            <asp:Panel ID="pheaderfilterguest" runat="server" CssClass="maintablestyle" Height="22px"
                                Width="889px">
                                <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                <asp:Label ID="Label3" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                    Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                <asp:Image ID="Image4" runat="server" CssClass="cpimage" ImageUrl="~/images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </center>
                    </div>
                    <br />
                    <center>
                        <asp:Panel ID="pcolumnorder" runat="server" CssClass="maintablestyle" Width="970px">
                            <%--style="margin-left:74px;"                     --%>
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -512px;"
                                            Visible="false" Width="111px" OnClick="LinkButtonsremove_Click">Remove  All</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                        <asp:TextBox ID="tborder" Visible="false" Width="907px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" AutoPostBack="true"
                                            Width="932px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="Roll_No">Roll No</asp:ListItem>
                                             <asp:ListItem Selected="True" Value="id">Student Id</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="Stud_Name">Stud Name</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="Hostel_Name">Hostel Name</asp:ListItem>
                                            <asp:ListItem Value="Building_Name">Building Name</asp:ListItem>
                                            <asp:ListItem Value="Floor_Name">Floor Name</asp:ListItem>
                                            <asp:ListItem Value="Batch_Year">Batch Year</asp:ListItem>
                                            <asp:ListItem Value="degree">Degree</asp:ListItem>
                                            <asp:ListItem Value="sex">Gender</asp:ListItem>
                                            <asp:ListItem Value="BillMonth">Bill Month</asp:ListItem>
                                            <asp:ListItem Value="Bill_Year">Bill Year</asp:ListItem>
                                             <%--   Added By Saranyadevi13.3.2018--%>
                                            <asp:ListItem Value="Fixed_Amount">Mess Bill Amount</asp:ListItem>
                                            <asp:ListItem Value="ExpanceGroupAmtTotal">Expance</asp:ListItem>
                                            <asp:ListItem Value="Additional_Amount">Additional Amount</asp:ListItem>
                                            <asp:ListItem Value="Rebete_days">Calculate Days</asp:ListItem>
                                            <asp:ListItem Value="Rebate_Amount">Rebate Amount</asp:ListItem>
                                            <asp:ListItem Value="total">Student Mess Amount</asp:ListItem>
                                            <asp:ListItem Value="Room_Name">Room</asp:ListItem>
                                            
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>

<center>
                        <asp:Panel ID="pcolumnorder2" runat="server" CssClass="maintablestyle" Width="970px">
                            <%--style="margin-left:74px;"                     --%>
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox1_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox1_column_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnk_columnorder2" runat="server" Font-Size="X-Small" Height="16px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -512px;"
                                            Visible="false" Width="111px" OnClick="LinkButtonsremove1_Click">Remove  All</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                        <asp:TextBox ID="tborder2" Visible="false" Width="907px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder2" runat="server" Height="43px" AutoPostBack="true"
                                            Width="932px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder2_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="APP_No">APP NO</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="id">Staff Id</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="appl_name">Staff Name</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="Hostel_Name">Hostel Name</asp:ListItem>
                                            <asp:ListItem Value="Building_Name">Building Name</asp:ListItem>
                                            <asp:ListItem Value="Floor_Name">Floor Name</asp:ListItem>
                                           <%-- <asp:ListItem Value="Batch_Year">Batch Year</asp:ListItem>--%>
                                            <asp:ListItem Value="Room_Name">Room Name</asp:ListItem>
                                            <%--<asp:ListItem Value="degree">Degree</asp:ListItem>
                                            <asp:ListItem Value="sex">Gender</asp:ListItem>--%>
                                            <asp:ListItem Value="BillMonth">Bill Month</asp:ListItem>
                                            <asp:ListItem Value="Bill_Year">Bill Year</asp:ListItem>
                                            
                                          <%--   Added By Saranyadevi13.3.2018--%>
                                            <asp:ListItem Value="Fixed_Amount">Mess Bill Amount</asp:ListItem>
                                            <asp:ListItem Value="ExpanceGroupAmtTotal">Expance</asp:ListItem>
                                            <asp:ListItem Value="Additional_Amount">Additional Amount</asp:ListItem>
                                            <asp:ListItem Value="Rebete_days">Calculate Days</asp:ListItem>
                                            <asp:ListItem Value="Rebate_Amount">Rebate Amount</asp:ListItem>
                                           <asp:ListItem Value="total">Staff Mess Amount</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                    <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                        CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                        TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="right.jpeg"
                        ExpandedImage="down.jpeg">
                    </asp:CollapsiblePanelExtender>
                </center>
                <center>
                    <asp:Panel ID="pcolumnorder1" runat="server" CssClass="maintablestyle" Width="890px">
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="cb_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_column_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnk_columnorder1" runat="server" Font-Size="X-Small" Height="16px"
                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -477px;"
                                        Visible="false" Width="111px" OnClick="lb_Click">Remove  All</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="tborder1" Visible="false" Width="867px" TextMode="MultiLine" CssClass="style1"
                                        AutoPostBack="true" runat="server" Enabled="false">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="cblcolumnorder1" runat="server" Height="43px" AutoPostBack="true"
                                        Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                        RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cbl_columnorder_SelectedIndexChanged">
                                        <asp:ListItem Value="Hostel_Name">Hostel Name</asp:ListItem>
                                          <asp:ListItem Value="id">Guest Id</asp:ListItem>
                                        <asp:ListItem Value="Guest_Name">Guest Name</asp:ListItem>
                                        <asp:ListItem Value="Guest_Address">Guest Address</asp:ListItem>
                                        <asp:ListItem Value="MobileNo">Mobile No</asp:ListItem>
                                        <asp:ListItem Value="From_Company">From Company</asp:ListItem>
                                        <asp:ListItem Value="Floor_Name">Floor Name</asp:ListItem>
                                        <asp:ListItem Value="Room_Name">Room Name</asp:ListItem>
                                        <%-- <asp:ListItem Value="Admission_Date">Admission Date</asp:ListItem>--%>
                                        <asp:ListItem Value="Building_Name">Building Name</asp:ListItem>
                                        <asp:ListItem Value="Guest_Street">Guest Street</asp:ListItem>
                                        <asp:ListItem Value="Guest_City">Guest City</asp:ListItem>
                                        <asp:ListItem Value="Guest_PinCode">Guest Pincode</asp:ListItem>
                                        <%-- <asp:ListItem Value="purpose">Purpose</asp:ListItem>--%>
                                        <asp:ListItem Value="BillMonth">Bill Month</asp:ListItem>
<%--   Added by saranyadevi 13.3.2018--%>
                                        <asp:ListItem Value="Fixed_Amount">Mess Bill Amount</asp:ListItem>
                                        <asp:ListItem Value="Bill_Year">Bill Year</asp:ListItem>
                                        <asp:ListItem Value="Additional_Amount">Additional Amount</asp:ListItem>
                                        <asp:ListItem Value="Rebate_Amount">Rebate Amount</asp:ListItem>
                                        <asp:ListItem Value="Rebete_days">Calculate Days</asp:ListItem>
                                         <asp:ListItem Value="total">Guest Mess Amount</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </center>
                <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pcolumnorder1"
                    CollapseControlID="pheaderfilter1" ExpandControlID="pheaderfilter1" Collapsed="true"
                    TextLabelID="Labelfilter1" CollapsedSize="0" ImageControlID="Imagefilter1" CollapsedImage="~/images/right.jpeg"
                    ExpandedImage="~/images/down.jpeg">
                </asp:CollapsiblePanelExtender>
                <%--end column order--%>
                <center>
                    <asp:Panel ID="pcolumnorderstu" runat="server" CssClass="maintablestyle" Width="890px">
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="cb_stu" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_stu_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnk_stu" runat="server" Font-Size="X-Small" Height="16px" Style="font-family: 'Book Antiqua';
                                        font-weight: 700; font-size: small; margin-left: -477px;" Visible="false" Width="111px"
                                        OnClick="lnk_stu_Click">Remove  All</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txt_stu" Visible="false" Width="867px" TextMode="MultiLine" CssClass="style1"
                                        AutoPostBack="true" runat="server" Enabled="false">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="cbl_stu" runat="server" Height="43px" AutoPostBack="true" Width="850px"
                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                        RepeatDirection="Horizontal" OnSelectedIndexChanged="cbl_stu_SelectedIndexChanged">
                                        <asp:ListItem Value="Hostel_Name">Student Name</asp:ListItem>
                                        <asp:ListItem Value="Guest_Name">Roll No</asp:ListItem>
                                      

                                        <asp:ListItem Value="Guest_Address">Degree</asp:ListItem>
                                        <asp:ListItem Value="MobileNo">Receipt No</asp:ListItem>
                                        <asp:ListItem Value="From_Company">Date</asp:ListItem>
                                        <asp:ListItem Value="Floor_Name">Amount</asp:ListItem>
                                        <asp:ListItem Value="Room_Name">Opening Dues</asp:ListItem>
                                        <asp:ListItem Value="Admission_Date">Days</asp:ListItem>
                                        <asp:ListItem Value="Building_Name">Charges</asp:ListItem>
                                        <asp:ListItem Value="Guest_Street">Bill</asp:ListItem>
                                        <asp:ListItem Value="Guest_City">Total</asp:ListItem>
                                        <%-- <asp:ListItem Value="Guest_PinCode">Guest Pincode</asp:ListItem>
                                    <asp:ListItem Value="purpose">Purpose</asp:ListItem>
                                    <asp:ListItem Value="BillMonth">Bill Month</asp:ListItem>
                                    <asp:ListItem Value="Bill_Year">Bill Year</asp:ListItem>
                                    <asp:ListItem Value="Fixed_Amount">Fixed Amount</asp:ListItem>
                                    <asp:ListItem Value="Additional_Amount">Additional Amount</asp:ListItem>
                                    <asp:ListItem Value="Rebate_Amount">Rebate Amount</asp:ListItem>
                                    <asp:ListItem Value="Rebete_days">Calculate Days</asp:ListItem>
                                    <asp:ListItem Value="total">Total</asp:ListItem>--%>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </center>
                <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="pcolumnorderstu"
                    CollapseControlID="pheaderfilterstu" ExpandControlID="pheaderfilterstu" Collapsed="true"
                    TextLabelID="Labelfilter1" CollapsedSize="0" ImageControlID="Imagefilter1" CollapsedImage="~/images/right.jpeg"
                    ExpandedImage="~/images/down.jpeg">
                </asp:CollapsiblePanelExtender>
                <%--    dfdsfds--%>
                <center>
                    <asp:Panel ID="pcolumnorderguest" runat="server" CssClass="maintablestyle" Width="890px">
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="cb_guest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_guest_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnk_guest" runat="server" Font-Size="X-Small" Height="16px" Style="font-family: 'Book Antiqua';
                                        font-weight: 700; font-size: small; margin-left: -477px;" Visible="false" Width="111px"
                                        OnClick="lnk_guest_Click">Remove  All</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txt_guestcol" Visible="false" Width="867px" TextMode="MultiLine"
                                        CssClass="style1" AutoPostBack="true" runat="server" Enabled="false">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="cbl_guest" runat="server" Height="43px" AutoPostBack="true"
                                        Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                        RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cbl_guest_SelectedIndexChanged">
                                        <asp:ListItem Value="Hostel_Name">Guest Name</asp:ListItem>
                                        <asp:ListItem Value="Guest_Name">Company Name</asp:ListItem>
                                        <asp:ListItem Value="Guest_Address">Guest Code</asp:ListItem>
                                        <asp:ListItem Value="MobileNo">Receipt No</asp:ListItem>
                                        <asp:ListItem Value="From_Company">Date</asp:ListItem>
                                        <asp:ListItem Value="Floor_Name">Amount</asp:ListItem>
                                        <asp:ListItem Value="Room_Name">Opening Dues</asp:ListItem>
                                        <asp:ListItem Value="Admission_Date">Days</asp:ListItem>
                                        <asp:ListItem Value="Building_Name">Charges</asp:ListItem>
                                        <asp:ListItem Value="Guest_Street">Bill</asp:ListItem>
                                        <asp:ListItem Value="Guest_City">Total</asp:ListItem>
                                        <%-- <asp:ListItem Value="Guest_PinCode">Guest Pincode</asp:ListItem>
                                    <asp:ListItem Value="purpose">Purpose</asp:ListItem>
                                    <asp:ListItem Value="BillMonth">Bill Month</asp:ListItem>
                                    <asp:ListItem Value="Bill_Year">Bill Year</asp:ListItem>
                                    <asp:ListItem Value="Fixed_Amount">Fixed Amount</asp:ListItem>
                                    <asp:ListItem Value="Additional_Amount">Additional Amount</asp:ListItem>
                                    <asp:ListItem Value="Rebate_Amount">Rebate Amount</asp:ListItem>
                                    <asp:ListItem Value="Rebete_days">Calculate Days</asp:ListItem>
                                    <asp:ListItem Value="total">Total</asp:ListItem>--%>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </center>
                <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="server" TargetControlID="pcolumnorderguest"
                    CollapseControlID="pheaderfilterguest" ExpandControlID="pheaderfilterguest" Collapsed="true"
                    TextLabelID="Labelfilter1" CollapsedSize="0" ImageControlID="Imagefilter1" CollapsedImage="~/images/right.jpeg"
                    ExpandedImage="~/images/down.jpeg">
                </asp:CollapsiblePanelExtender>
                <br />
                <div id="div1" runat="server" visible="false" style="width: 950px; height: 300px;
                    background-color: white;" class="spreadborder  reportdivstyle">
                    <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" Width="930px" CssClass="spreadborder"
                        OnCellClick="FpSpread1_CellClick" OnPreRender="FpSpread1_SelectedIndexChanged">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <br />
                <br />
                <div id="div2" runat="server" visible="false" style="width: 950px; height: 430px;
                    background-color: white;" class="spreadborder  reportdivstyle">
                    <FarPoint:FpSpread ID="FpSpread2" runat="server" Visible="false" Width="930px" CssClass="spreadborder">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <br />
                <%-- <br />--%>
                <center>
                    <div id="rptprint" runat="server" visible="false">
                        <asp:Label ID="lbl_validation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                            Visible="false"></asp:Label>
                        <asp:Label ID="lbl_reportname" runat="server" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txt_excelname" runat="server" Height="20px" Width="180px" onkeypress="display()"
                            CssClass="textbox textbox1 txtheight4" Style="font-family: 'Book Antiqua'"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=", .">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btn_Excel" runat="server" CssClass="textbox btn2" OnClick="btn_Excel_Click"
                            Text="Export To Excel" Width="127px" />
                        <asp:Button ID="btn_printmaster" runat="server" Text="Print" CssClass="textbox btn2"
                            OnClick="btn_printmaster_Click" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                </center>
            </div>
        </center>
        </form>
    </body>
    </html>
</asp:Content>
