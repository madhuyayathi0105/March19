<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="Inv_Dayscholar_stud_staff.aspx.cs" Inherits="Inv_Dayscholar_stud_staff" %>

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
            .sty
            {
                height: 500px;
                width: 1000px;
                border: 1px solid Gray;
                background-color: #F0F0F0;
                border-radius: 10px;
            }
        </style>
    </head>
    <body>
        <script type="text/javascript">
            //    theivamani 29.10.15
            function Test() {
                var id = "";
                var value1 = "";
                var idval = "";
                var empty = "";

                id = document.getElementById("<%=txt_rollno.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_rollno.ClientID %>");
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


            function display() {
                document.getElementById('<%=lbl_validation1.ClientID %>').innerHTML = "";
            }
            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }
            function Test1() {
                var id = "";
                var empty = "";
                id = document.getElementById("<%=txt_staffname.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_staffname.ClientID %>");
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
        </script>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <br />
            <div>
                <span class="fontstyleheader" style="color: #008000;">DayScholar Student / Staff Registration</span>
            </div>
            <br />
        </center>
        <center>
            <div class="maindivstyle" style="height: 550px; width: 1000px;">
                <center>
                    <br />
                    <table class="maintablestyle" width="940px">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_collegename" Text="College" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_college" runat="server" CssClass="textbox textbox1 ddlheight2"
                                    Width="240px" OnSelectedIndexChanged="ddl_college_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_hostelname" Text="Mess Name" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Upp1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_hostelname" runat="server" Width="120px" CssClass="textbox textbox1"
                                            Height="20px" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="p5" runat="server" Width="150px" Height="220px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_hostelname" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_hostelname_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_hostelname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_hostelname_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_hostelname"
                                            PopupControlID="p5" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_sessionname" Text="Session Name" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upp" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_sessionname" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                            Width="120px" Height="18px">--Select--</asp:TextBox>
                                        <asp:Panel ID="p1" runat="server" Width="180px" Height="200px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_sessionname" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_sessionname_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_sessionname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sessionname_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_sessionname"
                                            PopupControlID="p1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cb_both" runat="server" Visible="false" OnCheckedChanged="cb_both_CheckedChanged"
                                    AutoPostBack="true" />
                                <asp:Label ID="lbl_batch" Width="100px" Text="Batch" Visible="false" runat="server"></asp:Label>
                                <asp:Label ID="lbl_department" Width="100px" Text="Department" Visible="false" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Upp3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_batch" Visible="false" runat="server" CssClass="textbox textbox1"
                                            Width="120px" Height="20px" ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="p2" runat="server" Visible="false" Width="120px" Height="200px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_batch" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_batch_CheckedChange" />
                                            <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_batch"
                                            PopupControlID="p2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="uup1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_department" Visible="false" runat="server" CssClass="textbox textbox1"
                                            Width="120px" Height="20px" ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="pp0" runat="server" Visible="false" Width="200px" Height="250px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_department" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_department_CheckedChange" />
                                            <asp:CheckBoxList ID="cbl_department" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_department_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pup1" runat="server" TargetControlID="txt_department"
                                            PopupControlID="pp0" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_degree" Visible="false" Text="Degree" runat="server"></asp:Label>
                                <asp:Label ID="lbl_design" Visible="false" Text="Designation" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Upp4" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_degree" Visible="false" runat="server" CssClass="textbox textbox1"
                                            Width="120px" Height="20px" ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="p3" Visible="false" runat="server" CssClass="multxtpanel" Width="180px"
                                            Height="200px">
                                            <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_degree_ChekedChange" />
                                            <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_degree"
                                            PopupControlID="p3" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="uup2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_design" Visible="false" runat="server" CssClass="textbox textbox1"
                                            Width="120px" Height="20px" ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="pp2" Visible="false" Height="200px" Width="180px" runat="server" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_design" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_desig_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_design" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_desig_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_design"
                                            PopupControlID="pp2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_branch" Visible="false" Text="Branch" runat="server"></asp:Label>
                                <asp:Label ID="lblstaff" Visible="false" Text="Staff Type" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Upp6" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_branch" Visible="false" runat="server" CssClass="textbox textbox1"
                                            Width="120px" Height="20px" ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="p6" Visible="false" runat="server" Height="200px" Width="200px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_branch_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_branch"
                                            PopupControlID="p6" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="uup3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_stafftype" Visible="false" runat="server" CssClass="textbox textbox1"
                                            Width="120px" Height="20px" ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="pp3" Visible="false" runat="server" Height="150px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_stafftype" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_stafftype_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_stafftype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_stafftype_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txt_stafftype"
                                            PopupControlID="pp3" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_section" Text="Section" Visible="false" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Upp5" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_section" Visible="false" runat="server" CssClass="textbox textbox1"
                                            Width="120px" Height="20px" ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="p4" Visible="false" runat="server" Width="120px" Height="100px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_section" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_section_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_section" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_section_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_section"
                                            PopupControlID="p4" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_sex" Text="Gender" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upp7" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_sex" runat="server" ReadOnly="true" Width="120px" Height="20px"
                                            CssClass="textbox textbox1">--Select--</asp:TextBox>
                                        <asp:Panel ID="p11" runat="server" Height="100px" Width="150px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_sex" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_sex_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_sex" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sex_SelectedIndexChanged">
                                                <asp:ListItem Value="0">Male</asp:ListItem>
                                                <asp:ListItem Value="1">Female</asp:ListItem>
                                                <asp:ListItem Value="2">Transgender</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupExt4" runat="server" TargetControlID="txt_sex"
                                            PopupControlID="p11" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td colspan="2">
                                <asp:RadioButton ID="rdb_staff" runat="server" Text="Staff" GroupName="day" OnCheckedChanged="rdb_staff_Select"
                                    AutoPostBack="true" />
                                <asp:RadioButton ID="rdb_student" runat="server" Text="Student" GroupName="day" OnCheckedChanged="rdb_student_select"
                                    AutoPostBack="true" />
                                <asp:Button ID="btn_go" Text="Go" CssClass="textbox btn1" runat="server" OnClick="btn_go_Click"
                                    OnClientClick="return valid2()" />
                                <asp:Button ID="btn_addnew" Text="Add New" CssClass="textbox btn2" runat="server"
                                    OnClick="btn_addnew_Click" />
                            </td>
                        </tr>
                    </table>
                </center>
                <br />
                <center>
                    <asp:Label Style="color: Red;" ID="lbl_error1" runat="server"></asp:Label>
                </center>
                <p style="width: 691px;" align="right">
                    <asp:Label ID="lbl_stucnt" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                </p>
                <p style="width: 691px;" align="right">
                    <asp:Label ID="lbl_staffcnt" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                </p>
                <%--<div id="div1" runat="server" style="width: 777px; height: 300px; overflow: auto;
                background-color: white;" class="spreadborder" visible="false">--%>
                <center>
                    <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" BorderColor="Black"
                        BorderStyle="Solid" BorderWidth="1px" Style="height: 280px; overflow: auto; background-color: White;
                        border-radius: 10px; box-shadow: 0px 0px 8px #999999" OnUpdateCommand="Fpspread_Command">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </center>
                <%-- </div>--%>
                <br />
                <center>
                    <div id="rptprint" runat="server" visible="false">
                        <asp:Label ID="lbl_validation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                            Visible="false"></asp:Label>
                        <asp:Label ID="lbl_reportname" runat="server" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txt_excelname" runat="server" Height="20px" Width="180px" onkeypress="display()"
                            CssClass="textbox btn2"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_excelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btn_Excel" runat="server" CssClass="textbox btn2" OnClick="btn_Excel_Click"
                            Text="Export To Excel" Width="127px" />
                        <asp:Button ID="btn_printmaster" runat="server" Text="Print" CssClass="textbox btn2"
                            OnClick="btn_printmaster_Click" />
                        <asp:Button ID="btn_delete" runat="server" Text="Delete" CssClass="textbox btn2"
                            OnClick="btn_delete_click" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                </center>
            </div>
        </center>
        <%--<center>
        <div id="popupstudaddinl" runat="server" visible="false" class="popupstyle popupheight">
            <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: 65px; margin-left: 284px;"
                OnClick="imagebtnpopclose1_Click" />
            <br />
            <br />
            <br />
            <br />
            <div style="background-color: White; height: 380px; width: 600px; border: 5px solid #0CA6CA;
                border-top: 30px solid #0CA6CA; border-radius: 10px;">
                <br />
                <center>
                    <asp:Label ID="lbl_studentadd" runat="server" Font-Bold="true" Text="DayScholar Student / Staff Registration"
                        Style="font-size: large; color: #008000;"></asp:Label>
                </center>
                <br />
                <br />
                <table>
                   
                </table>
                <br />
                <br />
               
            </div>
        </div>
    </center>--%>
        <center>
            <div id="poperrjs" runat="server" visible="false" style="height: 50em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="ImageButton4" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 28px; margin-left: 385px;"
                    OnClick="imagebtnpopclose_Click" />
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; overflow: auto; width: 784px;
                    height: 452px;" align="center">
                    <br />
                    <center>
                        <asp:Label ID="lbl_itemcostmaster" runat="server" class="fontstyleheader" Style="color: #008000;"
                            Text="DayScholar Student / Staff Registration"></asp:Label>
                    </center>
                    <br />
                    <div align="left" style="overflow: auto; width: 739px; height: 357px; border-radius: 10px;
                        border: 1px solid Gray;">
                        <br />
                        <center>
                            <table class="maintablestyle" width="150px">
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rdb_stu" Text="Student" runat="server" GroupName="same" AutoPostBack="true"
                                            OnCheckedChanged="rdb_stu_CheckedChanged" />
                                        <asp:RadioButton ID="rdb_sta" Text="Staff" runat="server" GroupName="same" AutoPostBack="true"
                                            OnCheckedChanged="rdb_sta_CheckedChanged" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table>
                                <tr>
                                 <td>
                                            <asp:Label ID="lbl_pop1hostelname" Text="Hostel Name" runat="server" Visible="True"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_pop1hostelname" runat="server" CssClass="textbox ddlheight4 textbox1"
                                                Width="190px" 
                                                AutoPostBack="true" onfocus="return myFunction(this)" Visible="true" OnSelectedIndexChanged="ddl_pop1hostelname_SelectedIndexChanged"><%----%>
                                            </asp:DropDownList>
                                            <span id="Span1" runat="server" visible="true" style="color: Red;">*</span>
                                        </td></tr>
                                        <tr>
                                    <td>
                                        <asp:Label ID="lbl_stf_date" Text="Date" runat="server" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="Updp_todate" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_stf_date" runat="server" CssClass="textbox textbox1" AutoPostBack="true"
                                                    OnTextChanged="txt_stf_date_changed" Visible="false"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_stf_date" runat="server"
                                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_stf_time" Text="Time" runat="server" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_stfhr" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1"
                                            Visible="false">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_stfm" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1"
                                            Visible="false">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_stfam" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1"
                                            Visible="false">
                                            <asp:ListItem>AM</asp:ListItem>
                                            <asp:ListItem>PM</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_hostelname2" runat="server" Text="Mess Name" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_hostelname2" runat="server" Width="160px" Height="30px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_hostelname2_Change" CssClass="textbox textbox1"
                                            Visible="false">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_session2" runat="server" Text="Session Name" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="uup6" runat="server" Visible="false">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_session2" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                                    Width="148px" Height="18px">--Select--</asp:TextBox>
                                                <asp:Panel ID="pp6" runat="server" Height="200px" Width="150px" CssClass="multxtpanel">
                                                    <asp:CheckBox ID="cb_session2" runat="server" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cb_session2_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_session2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_session2_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txt_session2"
                                                    PopupControlID="pp6" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_Staffname" runat="server" Text="Staff Name" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_staffname" runat="server" Height="20px" CssClass="textbox textbox1"
                                            Width="238px" onfocus="return myFunction(this)" AutoPostBack="true" OnTextChanged="txt_staffname_Text_Changed"
                                            Visible="false"></asp:TextBox>
                                        <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_staffname"
                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=", ">
                            </asp:FilteredTextBoxExtender>--%>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetStaffNameadd" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffname"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <asp:Button ID="btn_staff" runat="server" Text="?" CssClass="textbox btn" OnClick="btn_staff_Click"
                                            Visible="false" />
                                        <span id="staff" runat="server" visible="false" style="color: Red;">*</span>
                                    </td>

                                     <td>
                                        <asp:Label ID="Llid" Text="Staff Id" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtid1" runat="server" CssClass="textbox textbox1" BackColor="#DCF9D1"
                                            onfocus="return myFunction(this)" Width="190px" Height="20px"  Enabled="false"
                                             MaxLength="30"></asp:TextBox>
                                      
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_department1" runat="server" Text="Department" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_department1" ReadOnly="true" TextMode="SingleLine" runat="server"
                                            Height="20px" CssClass="textbox textbox1" Width="300px" Visible="false"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_department1"
                                            FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ,">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr style="display: none;">
                                    <td>
                                        <asp:Label ID="lbl_stafftype1" runat="server" Text="Staff Type" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_stafftype1" ReadOnly="true" runat="server" Height="20px" CssClass="textbox textbox1"
                                            Width="160px" Visible="false"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txt_stafftype1"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <%-- <tr>
                                <td>
                                    <asp:Label ID="Label11" runat="server" Text="Degree"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox5" TextMode="SingleLine" runat="server" Height="20px" CssClass="textbox textbox1"
                                        Width="300px"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txt_degree1"
                                        FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=".@- ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>--%>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_date" runat="server" Text="Date" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_date" runat="server" Visible="false" CssClass="textbox textbox1"
                                                    OnTextChanged="txt_datestud_changed" AutoPostBack="true">
                                                </asp:TextBox>
                                                <asp:CalendarExtender ID="Cal_date" TargetControlID="txt_date" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                                    Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_time" runat="server" Text="Time" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_hour" Width="50px" Height="25px" runat="server" Visible="false"
                                            CssClass="textbox textbox1">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_minits" Width="50px" Height="25px" runat="server" Visible="false"
                                            CssClass="textbox textbox1">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_seconds" Width="50px" Height="25px" Visible="false" runat="server"
                                            CssClass="textbox textbox1">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_timeformate" Width="50px" Height="25px" runat="server"
                                            Visible="false" CssClass="textbox textbox1">
                                            <asp:ListItem>AM</asp:ListItem>
                                            <asp:ListItem>PM</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_hostelname1" runat="server" Text="Mess Name" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_hostelname1" runat="server" Width="160px" Height="30px"
                                            Visible="false" CssClass="textbox textbox1" AutoPostBack="true" OnSelectedIndexChanged="ddl_hostelname1_Change">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_session1" runat="server" Text="Session Name" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" Visible="false">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_sessionname1" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                                    Width="149px" Height="18px">--Select--</asp:TextBox>
                                                <asp:Panel ID="Psession" runat="server" Height="200px" Width="150px" CssClass="multxtpanel">
                                                    <asp:CheckBox ID="cb_sessionname1" runat="server" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cb_sessionname1_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_sessionname1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sessionname1_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txt_sessionname1"
                                                    PopupControlID="Psession" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                  <%--  magesh 12.3.18--%>
                                            <td>
                                        <asp:Label ID="lbl_pop1messtype" Text="Student Type" runat="server"></asp:Label>
                                    </td>
                                    <td colspan="2px">
                                        <%--<asp:RadioButton ID="rdbveg" runat="server" Text="Veg" GroupName="same2" />
                                <asp:RadioButton ID="rdbnonveg" runat="server" Text="Non Veg" GroupName="same2" />
                                        <asp:RadioButtonList ID="Radiobtnstype" runat="server" Font-Names="Book Antiqua"
                                            Style="margin-left: 0px;" RepeatDirection="Horizontal" Visible="false">
                                            <asp:ListItem Value="0">Veg</asp:ListItem>
                                            <asp:ListItem Value="1">Non Veg</asp:ListItem>
                                        </asp:RadioButtonList>--%>
                                        <asp:DropDownList ID="ddlStudType" runat="server" CssClass="textbox  ddlheight3"
                                            onfocus="return myFunction(this)">
                                        </asp:DropDownList>
                                    </td>  <%--magesh 12.3.18--%>
                                </tr>
                                <%-- <tr>
                        <td>
                            <asp:Label ID="lbl_rolladmit" runat="server" Text="Roll Admit"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_rolladmit" TextMode="SingleLine" ReadOnly="true" runat="server" Height="20px"
                                CssClass="textbox textbox1" Width="300px" onfocus="return myFunction(this)"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_rolladmit"
                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ,">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btn_rolladmit" runat="server" Text="?" CssClass="textbox btn" OnClick="btnroladmit_click" />
                            <span style="color: Red;">*</span>
                        </td>
                    </tr>--%>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_rollno" runat="server" Text="Roll No" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_rollno" TextMode="SingleLine" runat="server" Height="20px" CssClass="textbox textbox1"
                                            Width="243px" onfocus="return myFunction(this)" AutoPostBack="true" OnTextChanged="txt_rollno_txtchange"
                                            Visible="false"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getroll1" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <asp:Button ID="btn_rolladmit" runat="server" Text="?" CssClass="textbox btn" OnClick="btnroladmit_click"
                                            Visible="false" />
                                        <span id="stu" runat="server" visible="false" style="color: Red;">*</span>
                                    </td>

                                        <td>
                                        <asp:Label ID="lblid" Text="Student Id" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtid" runat="server" CssClass="textbox textbox1" BackColor="#DCF9D1"
                                            onfocus="return myFunction(this)" Width="190px" Height="20px"  Enabled="false"
                                             MaxLength="30"></asp:TextBox>
                                      
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_name" runat="server" Text="Name" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_name" TextMode="SingleLine" runat="server" ReadOnly="true" Height="20px"
                                            CssClass="textbox textbox1" Width="300px" Visible="false"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_name"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ,.">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_degree1" runat="server" Text="Degree" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_degree1" TextMode="SingleLine" ReadOnly="true" runat="server"
                                            Height="20px" CssClass="textbox textbox1" Width="300px" Visible="false"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txt_degree1"
                                            FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=".@- ,">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <center>
                                <div>
                                    <asp:Button ID="btn_save2staff" runat="server" CssClass="textbox btn2" Text="Save"
                                        OnClick="btn_save2staff_Click" OnClientClick="return Test1()" Visible="false" />
                                    <asp:Button ID="btn_exit3staff" runat="server" CssClass="textbox btn2" Text="Exit"
                                        OnClick="btn_exit3staff_Click" Visible="false" />
                                </div>
                            </center>
                            <center>
                                <div>
                                    <asp:Button ID="btn_save" runat="server" CssClass="textbox btn2" Text="Save" OnClick="btn_save_Click"
                                        OnClientClick="return Test()" Visible="false" />
                                    <asp:Button ID="btn_exit1" runat="server" CssClass="textbox btn2" Text="Exit" OnClick="btn_exit1_Click"
                                        Visible="false" />
                                </div>
                            </center>
                        </center>
                    </div>
                </div>
            </div>
        </center>
        <center>
            <div id="popupselectstd" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 68px; margin-left: 415px;"
                    OnClick="imagebtnpopclose2_Click" />
                <br />
                <br />
                <br />
                <br />
                <div style="background-color: White; height: 498px; width: 858px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <asp:Label ID="Label1" runat="server" Text="Select the Student" class="fontstyleheader"
                            Style="color: #008000;"></asp:Label>
                    </center>
                    <br />
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_batch1" runat="server" Text="Batch"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_batch1" Width="100px" Height="30px" runat="server" CssClass="textbox textbox1"
                                    onfocus="return myFunction(this)">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_degree2" runat="server" Text="Degree"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_degree2" runat="server" ReadOnly="true" Height="20px" CssClass="textbox textbox1"
                                            Width="120px">--Select--</asp:TextBox>
                                        <asp:Panel ID="pdegree" runat="server" Width="150px" Height="170px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_degree2" runat="server" OnCheckedChanged="cb_degree2_ChekedChange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_degree2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree2_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_degree2"
                                            PopupControlID="pdegree" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_branch2" runat="server" Text="Branch"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_branch2" runat="server" ReadOnly="true" Height="20px" CssClass="textbox textbox1"
                                            Width="120px">--Select--</asp:TextBox>
                                        <asp:Panel ID="pbranch" runat="server" Width="250px" Height="200px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_branch1" runat="server" OnCheckedChanged="cb_branch1_ChekedChange"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_branch1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_branch1_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_branch2"
                                            PopupControlID="pbranch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_rollno1" runat="server" Text="Roll No"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_rollno1" TextMode="SingleLine" runat="server" AutoCompleteType="Search"
                                    Height="20px" CssClass="textbox textbox1" Width="120px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_rollno1"
                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getroll" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno1"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Button ID="btn_go1" Text="Go" OnClick="btn_go1_Click" CssClass="textbox btn1"
                                    runat="server" />
                            </td>
                        </tr>
                    </table>
                    <p style="width: 691px;" align="right">
                        <asp:Label ID="lbl_cnt" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </p>
                    <div>
                        <asp:Label ID="lbl_errormsg" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </div>
                    <div>
                        <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" BorderWidth="0px"
                            Width="500px" Style="overflow: auto; height: 250px; border: 0px solid #999999;
                            border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                            OnUpdateCommand="Fpspread2_Command">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <br />
                    <br />
                    <center>
                        <div>
                            <asp:Button ID="btn_ok" runat="server" CssClass="textbox btn2" Text="Ok" OnClick="buttonok_Click" />
                            <asp:Button ID="btn_exit2" runat="server" CssClass="textbox btn2" Text="Exit" OnClick="btn_exit2_Click" />
                        </div>
                    </center>
                </div>
            </div>
        </center>
        <%--<center>
        <div id="popstaff" runat="server" visible="false" class="popupstyle popupheight">
            <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: 70px; margin-left: 254px;"
                OnClick="imagebtnpopclose3_Click" />
            <br />
            <br />
            <br />
            <br />
            <div style="background-color: White; height: 390px; width: 531px; border: 5px solid #0CA6CA;
                border-top: 30px solid #0CA6CA; border-radius: 10px;">
                <br />
                <center>
                    <asp:Label ID="lblpopstaffadd" runat="server" Font-Bold="true" Text="Staff Additional"
                        Style="font-size: large; color: #008000;"></asp:Label>
                </center>
                <br />
                <br />
                <table>
                    <tr>
                        
            </div>
        </div>
    </center>--%>
        <center>
            <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 143px;
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
                                                OnClick="btnerrclose_Click" Text="ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <%-- theivamnai 29.10.15--%>
        <center>
            <div id="surediv" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div3" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_sure" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_yes" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btn_sureyes_Click" Text="yes" runat="server" />
                                            <asp:Button ID="btn_no" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btn_sureno_Click" Text="no" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="popupsscode1" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 31px; margin-left: 387px;"
                    OnClick="imagebtnpopclose4_Click" />
                <br />
                <br />
                <div style="background-color: White; height: 650px; width: 800px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <asp:Label ID="lbl_selctstaf" runat="server" Font-Bold="true" Style="font-size: large;
                            color: #008000;" Text="Select the Staff Name"></asp:Label>
                    </center>
                    <br />
                    <div>
                        <center>
                            <table class="maintablestyle">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_collegename1" runat="server" Text="College"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_collegename1" Width="250px" Height="30px" runat="server"
                                            AutoPostBack="true" CssClass="textbox textbox1">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_department2" runat="server" Text="Department"></asp:Label>
                                        <asp:DropDownList ID="ddl_department2" Width="160px" Height="30px" runat="server"
                                            AutoPostBack="true" CssClass="textbox textbox1" OnSelectedIndexChanged="ddl_department2_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_Search" runat="server" Text="Search By"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_Search" Width="250px" Height="30px" runat="server" AutoPostBack="true"
                                            CssClass="textbox textbox1" OnSelectedIndexChanged="ddl_search_SelectedIndexChanged">
                                            <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                            <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_Search" TextMode="SingleLine" Visible="false" runat="server"
                                            Height="20px" CssClass="textbox textbox1" Width="180px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txt_Search"
                                            FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_Search"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <asp:TextBox ID="txt_wardencode" Visible="false" TextMode="SingleLine" runat="server"
                                            Height="20px" CssClass="textbox textbox1" Width="180px"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getstaffcode" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_wardencode"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <asp:Button ID="btn_Search" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_searchbygo_Click" />
                                    </td>
                                </tr>
                            </table>
                            <div>
                                <center>
                                    <p style="width: 216px;">
                                        <asp:Label ID="lbl_errorsearch" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                    </p>
                                    <p>
                                        <asp:Label ID="lbl_errorstaff" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                    </p>
                                    <p>
                                        <asp:Label ID="error" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                    </p>
                                    <FarPoint:FpSpread ID="Fpstaff" runat="server" Visible="false" Width="700px" OnUpdateCommand="Fpspread1_Command"
                                        Style="overflow: auto; height: 500px; border: 0px solid #999999; border-radius: 5px;
                                        background-color: White; box-shadow: 0px 0px 8px #999999;">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </center>
                            </div>
                            <br />
                            <center>
                                <div>
                                    <asp:Button ID="btn_save4" runat="server" CssClass="textbox btn2" Text="Save" OnClick="btn_save4_Click" />
                                    <asp:Button ID="btn_exit4" runat="server" CssClass="textbox btn2" Text="Exit" OnClick="btn_exit4_Click" />
                                </div>
                            </center>
                        </center>
                    </div>
                </div>
            </div>
        </center>
        </form>
    </body>
    </html>
</asp:Content>
