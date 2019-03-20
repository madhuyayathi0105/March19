<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="indivual_student_item_request_approval.aspx.cs" Inherits="indivual_student_item_request_approval" %>

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
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            .container
            {
                width: 50%;
            }
            .col1
            {
                float: left;
                width: 50%;
            }
            .col2
            {
                float: right;
            }
            .div
            {
                left: 0%;
                top: 0%;
            }
            .table2
            {
                border: 1px solid #0CA6CA;
                border-radius: 10px;
                background-color: #0CA6CA;
                box-shadow: 0px 0px 8px #7bc1f7;
            }
        </style>
    </head>
    <body>
        <script type="text/javascript">

            function display() {
                document.getElementById('<%=lbl_validation.ClientID %>').innerHTML = "";
            }

            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }
        </script>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <div>
            <center>
                <asp:Label ID="Label1" runat="server"  class="fontstyleheader" Style=" color: Green;" Text="Individual Student Item Request Approval"></asp:Label>
                <br />
                <br />
            </center>
            <center>
                <div class="maindivstyle" style="width: 1000px; height: 889px;">
                    <br />
                    <table class="maintablestyle" style="width: 856px;">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_batch" runat="server" Text="Batch"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_batch" runat="server" CssClass="textbox  txtheight3" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                            width: 140px;">
                                            <asp:CheckBox ID="cb_batch" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_batch_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_batch_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_batch"
                                            PopupControlID="Panel1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_degree" runat="server" Text="Degree"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  txtheight3" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                            width: 140px;">
                                            <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_degree_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_degree"
                                            PopupControlID="Panel2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_branch" runat="server" Text="Branch"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_branch" runat="server" CssClass="textbox  txtheight3" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                            width: 200px;">
                                            <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_branch_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_branch"
                                            PopupControlID="Panel3" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_sem" runat="server" Text="Semester"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox  txtheight" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_sem" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_sem_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_sem"
                                            PopupControlID="Panel4" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_sec" runat="server" Text="Section"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_sec" runat="server" CssClass="textbox  txtheight3" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel5" runat="server" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_sec" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_sec_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_sec" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sec_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_sec"
                                            PopupControlID="Panel5" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_fromdate" runat="server" Text="From Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox  txtheight3" ForeColor="Black"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_todate" runat="server" Text="To Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox  txtheight3"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_staff" runat="server" Text="Staff" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_staff" runat="server" CssClass="textbox  txtheight3" ReadOnly="true"
                                            Visible="false">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel6" runat="server" CssClass="multxtpanel" Visible="false">
                                            <asp:CheckBox ID="cb_staff" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_staff_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_staff" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_staff_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_staff"
                                            PopupControlID="Panel6" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td colspan="4">
                                <asp:RadioButton ID="rdb_wfa" Text="Waiting For Approval" runat="server" GroupName="same"
                                    AutoPostBack="true" OnCheckedChanged="rdb_wfa_CheckedChanged" />
                                <asp:RadioButton ID="rdb_app" Text="Approved" runat="server" GroupName="same" AutoPostBack="true"
                                    OnCheckedChanged="rdb_app_CheckedChanged" />
                                <asp:RadioButton ID="rdb_reject" Text="Rejected" runat="server" GroupName="same"
                                    AutoPostBack="true" OnCheckedChanged="rdb_reject_CheckedChanged" />
                            </td>
                            <td>
                                <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="lbl_error" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                    <div>
                        <br />
                        <center>
                            <asp:Panel ID="pheaderfilter" runat="server" CssClass="table2" Height="22px" Width="850px"
                                Style="margin-top: -0.1%;">
                                <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                    Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageAlign="Right" />
                            </asp:Panel>
                        </center>
                        <br />
                    </div>
                    <center>
                        <asp:Panel ID="pcolumnorder" runat="server" CssClass="table2" Width="850px">
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
                                        <asp:TextBox ID="tborder" Visible="false" Width="837px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" AutoPostBack="true"
                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="7" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged">
                                            <asp:ListItem Value="Roll_No">Roll No </asp:ListItem>
                                            <asp:ListItem Value="Stud_Name">Name</asp:ListItem>
                                            <asp:ListItem Value="Course_Name">Degree</asp:ListItem>
                                            <asp:ListItem Value="Dept_Name">Branch</asp:ListItem>
                                            <asp:ListItem Value="Current_Semester">Semester</asp:ListItem>
                                            <asp:ListItem Value="Sections">Section</asp:ListItem>
                                            <asp:ListItem Value="TotItemQty">Total No Of Item</asp:ListItem>
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
                    <br />
                    <br />
                    <div id="div1" runat="server" visible="false" class="reportdivstyle spreadborder"
                        style="width: 750px;">
                        <br />
                        <br />
                        <FarPoint:FpSpread ID="Fpspread1" runat="server" Width="750px" Style="overflow: auto;
                            height: 350px; border: 0px solid #999999; border-radius: 5px; background-color: White;"
                            OnCellClick="Cell_Click" OnPreRender="Fpspread1_render">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <br />
                    <center>
                        <div id="rptprint" runat="server" visible="false">
                            <asp:Label ID="lbl_validation" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                Visible="false"></asp:Label>
                            <asp:Label ID="lbl_rptname" runat="server" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txt_excelname" runat="server" CssClass="textbox textbox1 txtheight4"
                                Font-Bold="True" onkeypress="display()"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btn_excel" runat="server" OnClick="btn_excel_Click" CssClass="textbox"
                                Text="Export To Excel" Width="127px" Height="30px" />
                            <asp:Button ID="btn_printmaster" runat="server" Text="Print" OnClick="btn_printmaster_Click"
                                Width="60px" Height="30px" CssClass="textbox" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                    </center>
                </div>
            </center>
        </div>
        <center>
            <div id="poperrjs" runat="server" visible="false" style="height: 80em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 28px; margin-left: 419px;"
                    OnClick="imagebtnpopclose1_Click" />
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; overflow: auto; width: 850px;"
                    align="center">
                    <br />
                    <center>
                        <asp:Label ID="lbl_stuitemreq" runat="server" Style="font-size: large; color: Green;"
                            Text="Student Item Request Approval"></asp:Label>
                    </center>
                    <br />
                    <div align="center" style="overflow: auto; width: 750px; height: 460px; border-radius: 10px;
                        border: 1px solid Gray;">
                        <br />
                        <div style="width: 357px; float: left;">
                            <table align="left" style="width: 300;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_rollno" runat="server" Text="Roll No"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_rollno" TextMode="SingleLine" runat="server" CssClass="textbox  txtheight3"
                                            onfocus="return myFunction(this)"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_rollno"
                                            FilterType="UppercaseLetters,LowercaseLetters,numbers" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_totnoofitem" runat="server" Text="Total No Of Item"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_totnoofitem" TextMode="SingleLine" runat="server" CssClass="textbox  txtheight3"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btn_go1" Text="Go" OnClick="btn_go1_Click" CssClass="textbox btn1"
                                            runat="server" Visible="false" />
                                    </td>
                                </tr>
                            </table>
                            <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" AutoPostBack="true"
                                BorderWidth="0px" Width="320px" Style="overflow: auto; height: 200px; border: 0px solid #999999;
                                border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                OnUpdateCommand="Fpspread2_Command" ActiveSheetViewIndex="0">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <br />
                            <br />
                            <div id="divv" runat="server">
                                <table>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="lbl_appstuname" runat="server" Text="Approval Staff Name"></asp:Label>
                                            <asp:TextBox ID="txt_appstuname" TextMode="SingleLine" runat="server" CssClass="textbox  txtheight3"
                                                ReadOnly="true" onfocus="return myFunction(this)"></asp:TextBox>
                                            <asp:Button ID="btn_appstuname" runat="server" Text="?" CssClass="textbox btn" OnClick="btn_appstuname_Click" />
                                            <span style="color: Red;">*</span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                        </div>
                        <div style="width: 335px; margin-left: 10px; float: left;">
                            <table align="center" style="width: 400;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_name" runat="server" Text="Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_name" TextMode="SingleLine" runat="server" CssClass="textbox  txtheight3"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_degree1" runat="server" Text="Degree"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_degree1" TextMode="SingleLine" runat="server" CssClass="textbox  txtheight3"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_branch1" runat="server" Text="Branch"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_branch1" TextMode="SingleLine" runat="server" CssClass="textbox  txtheight3"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_sem1" runat="server" Text="Semester"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_sem1" TextMode="SingleLine" runat="server" CssClass="textbox  txtheight3"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_sec1" runat="server" Text="Section"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_sec1" TextMode="SingleLine" runat="server" CssClass="textbox  txtheight3"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_photo" runat="server" Text="Photo"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Image ID="Image2" runat="server" CssClass="cpimage" Style="width: 100px; margin-left: 10px;
                                            float: left;" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="margin-right: 170px;" class="container">
                            <div id="div5" runat="server" class="col1">
                            </div>
                            <div id="div6" runat="server" class="col2">
                                <asp:Button ID="btn_appr" Text="Approval" CssClass="textbox btn2" runat="server"
                                    OnClick="btn_appr_Click" />
                                <asp:Button ID="btn_rej" Text="Reject" CssClass="textbox btn2" runat="server" OnClick="btn_rej_Click" />
                                <asp:Button ID="btn_exit_app" runat="server" Text="Exit" CssClass="textbox btn2"
                                    OnClick="btn_exit__app_Click" Visible="false" />
                            </div>
                        </div>
                    </div>
                    <br />
                    <div>
                        <center>
                            &nbsp; &nbsp; &nbsp;
                            <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="textbox btn2" Visible="true" />
                            <asp:Button ID="btn_exit" runat="server" Text="Exit" CssClass="textbox btn2" OnClick="btn_exit_Click" />
                        </center>
                    </div>
                    <br />
                </div>
            </div>
        </center>
        <center>
            <div id="popupsscode1" runat="server" visible="false" class="popupstyle popupheight2">
                <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 26px; margin-left: 436px;"
                    OnClick="imagebtnpopclose4_Click" />
                <br />
                <br />
                <div style="background-color: White; height: 650px; width: 900px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <asp:Label ID="lbl_selectstaffcode" runat="server" Style="font-size: large; color: #0AA7B3;"
                            Text="Select Staff Code"></asp:Label>
                    </center>
                    <br />
                    <div>
                        <center>
                            <table class="maintablestyle">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_college" runat="server" Text="College"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_college" Width="250px" Height="30px" runat="server" AutoPostBack="true"
                                            CssClass="textbox textbox1">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_department" runat="server" Text="Department"></asp:Label>
                                        <asp:DropDownList ID="ddl_department" Width="180px" Height="30px" runat="server"
                                            AutoPostBack="true" CssClass="textbox textbox1">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_searchby" runat="server" Text="Search By"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_searchby" Width="250px" Height="30px" runat="server" CssClass="textbox textbox1">
                                            <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_searchby" Visible="true" TextMode="SingleLine" runat="server"
                                            Height="20px" CssClass="textbox textbox1" Width="180px"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchby"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <asp:Button ID="btn_go2" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go2_Click" />
                                    </td>
                                </tr>
                            </table>
                            <div>
                                <p>
                                    <asp:Label ID="lbl_errorsearch" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                </p>
                                <p>
                                    <asp:Label ID="lbl_errorsearch1" runat="server" Visible="false" Font-Bold="true"
                                        ForeColor="Red"></asp:Label>
                                </p>
                                <FarPoint:FpSpread ID="Fpstaff" runat="server" Visible="false" Width="700px" Style="overflow: auto;
                                    height: 500px; border: 0px solid #999999; border-radius: 5px; background-color: White;
                                    box-shadow: 0px 0px 8px #999999;">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </div>
                            <br />
                            <center>
                                <div runat="server" id="btndiv1" visible="false">
                                    <asp:Button ID="btn_save1" runat="server" CssClass="textbox btn2" Text="Save" OnClick="btn_save1_Click" />
                                    <asp:Button ID="btn_exit2" runat="server" CssClass="textbox btn2" Text="Exit" OnClick="btn_exit2_Click" />
                                </div>
                            </center>
                        </center>
                    </div>
                </div>
            </div>
        </center>
        <div id="imgdiv2" runat="server" visible="false" class="popupstyle" style="height: 50em;">
            <center>
                <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_alerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_errclose" CssClass=" textbox btn2 comm" OnClick="btn_errclose_Click"
                                            Text="OK" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
        </form>
    </body>
    </html>
</asp:Content>
