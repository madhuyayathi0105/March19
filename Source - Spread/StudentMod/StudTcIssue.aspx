<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudTcIssue.aspx.cs" Inherits="StudentMod_StudTcIssue" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>TC Issue</title>
    <link rel="Shortcut Icon" href="~/college/Left_Logo.jpeg" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript" language="javascript">
        function display() {
            document.getElementById('<%=lbl_validation.ClientID %>').innerHTML = "";
        }
    </script>
    <asp:ScriptManager ID="scrptMgr" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green">Student TC Issue Report</span>
            </div>
        </center>
    </div>
    <center>
        <div class="maindivstyle" style="width: 970px; height: 500px;">
            <center>
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lblCollege" runat="server" Text="College"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_college" Width="120px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_college_OnIndexChange">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_stream" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_strm" Width="80px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_strm_OnIndexChange">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_batch" runat="server" Text="Batch"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_batch" Width="80px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_batch_OnIndexChange">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_degree" runat="server" Text="Degree"></asp:Label>
                        </td>
                        <td>
                            <%--  <asp:DropDownList ID="ddl_degree" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_degree_OnIndexChange">
                            </asp:DropDownList>--%>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_degree" runat="server" ReadOnly="true" Height="20px" CssClass="textbox txtheight">Degree</asp:TextBox>
                                    <asp:Panel ID="pdegree" runat="server" Width="150px" Height="170px" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_degree" runat="server" OnCheckedChanged="cb_degree_ChekedChange"
                                            Text="Select All" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_degree"
                                        PopupControlID="pdegree" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_branch" runat="server" Text="Branch"></asp:Label>
                        </td>
                        <td>
                            <%--<asp:DropDownList ID="ddl_branch" Width="120px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_branch_OnIndexChange">
                            </asp:DropDownList>--%>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_branch" runat="server" ReadOnly="true" Height="20px" CssClass="textbox textbox1 txtheight">Branch</asp:TextBox>
                                    <asp:Panel ID="pbranch" runat="server" Width="250px" Height="200px" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_branch" runat="server" OnCheckedChanged="cb_branch_ChekedChange"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_branch"
                                        PopupControlID="pbranch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Button ID="btn_go" Text="Go" OnClick="btn_go_Click" CssClass="textbox btn1 textbox1"
                                runat="server" />
                        </td>
                        <td>
                            <asp:Button ID="btn_Add" Text="Issue" OnClick="btn_Issue_Click" CssClass="textbox btn2 textbox1"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:CheckBox ID="cbDateWise" runat="server" Text="" Checked="false" />
                            <asp:Label ID="lblFrom" runat="server" Text="From"></asp:Label>
                            <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox  txtheight" Width="70px"
                                OnTextChanged="checkDate" AutoPostBack="true"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_fromdate" runat="server"
                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                            </asp:CalendarExtender>
                            To
                            <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox  txtheight" OnTextChanged="checkDate"
                                Width="70px" AutoPostBack="true"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_todate" runat="server"
                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                </table>
                <br />
                <div>
                    <asp:Label ID="lbl_errormsg" Visible="false" runat="server" Font-Bold="true" Text=""
                        ForeColor="Red"></asp:Label>
                </div>
                <div>
                    <FarPoint:FpSpread ID="spreadStudList" runat="server" Visible="false" ShowHeaderSelection="false"
                        BorderWidth="0px" Width="900px" Style="overflow: auto; height: 300px; border: 0px solid #999999;
                        border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" SelectionPolicy="Single">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lbl_validation" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                        Visible="false"></asp:Label><br />
                    <asp:Label ID="lbl_rptname" runat="server" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txt_excelname" runat="server" Width="180px" onkeypress="display()"
                        CssClass="textbox textbox1 txtheight2" MaxLength="70"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="fteExcel" runat="server" TargetControlID="txt_excelname"
                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" _-">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btn_excel" runat="server" OnClick="btn_excel_Click" Text="Export To Excel"
                        Width="127px" CssClass="textbox btn2 textbox1" />
                    <asp:Button ID="btn_printmaster" runat="server" Text="Print" OnClick="btn_printmaster_Click"
                        CssClass="textbox btn2 textbox1" Width="60px" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </div>
                <br />
            </center>
        </div>
    </center>
    <%--  ******popup window******--%>
    <center>
        <div id="popwindow" runat="server" visible="false" class="popupstyle popupheight1 ">
            <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: 15px; margin-left: 450px;"
                OnClick="imagebtnpopclose_Click" />
            <br />
            <div style="background-color: White; height: 530px; width: 950px; border: 5px solid #0CA6CA;
                border-top: 30px solid #0CA6CA; border-radius: 10px;">
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: Green;">Student TC Issue</span></div>
                </center>
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lblCollege1" runat="server" Text="College"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_college1" Width="200px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_college1_OnIndexChange">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_stream1" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_strm1" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_strm1_OnIndexChange">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_batch1" runat="server" Text="Batch"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_batch1" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_batch1_OnIndexChange">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_degree1" runat="server" Text="Degree"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_degree1" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_degree1_OnIndexChange">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_branch1" runat="server" Text="Branch"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_branch1" Width="200px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_branch1_OnIndexChange">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_SearchBy" runat="server" Text="Search By"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddl_searchBy" Width="120px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_searchBy_OnIndexChange">
                                <asp:ListItem Selected="True">Adm No</asp:ListItem>
                                <asp:ListItem>Student Name</asp:ListItem>
                                <asp:ListItem>Roll No</asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="txt_SearchBy" runat="server" CssClass="textbox  txtheight1" Width="190px"
                                MaxLength="45" Placeholder="Adm No">
                            </asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="fceSear" runat="server" FilterType="UppercaseLetters,LowercaseLetters, Numbers,Custom"
                                ValidChars=" ." TargetControlID="txt_SearchBy">
                            </asp:FilteredTextBoxExtender>
                            <asp:AutoCompleteExtender ID="autocomplete_rollno" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetSearch" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_SearchBy"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td colspan="3">
                            <asp:RadioButton ID="rdb_general" Text="General" runat="server" GroupName="tc" />
                            <asp:RadioButton ID="rdb_Request" Text="Request" runat="server" GroupName="tc" />
                        </td>
                        <td>
                            <asp:Button ID="btn_go1" Text="Go" OnClick="btn_go1_Click" CssClass="textbox btn1 textbox1"
                                runat="server" />
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                <br />
                <div>
                    <asp:Label ID="lbl_errormsg1" Visible="false" Font-Bold="true" runat="server" Text=""
                        ForeColor="Red"></asp:Label>
                </div>
                <div>
                    <FarPoint:FpSpread ID="spreadStudAdd" runat="server" Visible="false" ShowHeaderSelection="false"
                        OnUpdateCommand="spreadStudAdd_Command" BorderWidth="0px" Width="850px" Style="overflow: auto;
                        height: 300px; border: 0px solid #999999; border-radius: 10px; background-color: White;
                        box-shadow: 0px 0px 8px #999999;">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#F7BE81" SelectionPolicy="Single">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <div>
                    <br />
                    <center>
                        <asp:Button ID="btnIsssueTC" runat="server" CssClass=" textbox btn2" Width="120px"
                            Text="Issue TC" OnClick="btnIsssueTC_Click" />
                    </center>
                </div>
            </div>
        </div>
    </center>
</asp:Content>
