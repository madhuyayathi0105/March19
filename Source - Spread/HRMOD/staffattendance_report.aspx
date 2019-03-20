<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="staffattendance_report.aspx.cs" Inherits="staffattendance_report" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">

        function display() {

            document.getElementById('MainContent_lblvalidation1').innerHTML = "";

        }

        function display1() {
            document.getElementById('MainContent_lblspr3validation').innerHTML = "";
        }
    </script>
    <style type="text/css">
        .font
        {
            font-size: Medium;
            font-family: Book Antiqua;
        }
        .CenterPB
        {
            position: absolute;
            left: 50%;
            top: 50%;
            margin-top: -20px;
            margin-left: -20px;
            width: auto;
            height: auto;
        }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <span class="fontstyleheader" style="color: Green;">Daily Attendance Report</span>
        </div>
    </center>
    <center>
        <table class="maintablestyle" style="height: 100px; width: 880px;">
            <tr>
                <td>
                    <asp:Label ID="lbl_college" runat="server" Text="College Name" Font-Bold="True" Font-Names="Book Antiqua"
                        Width="106px" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="updatecollege" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="160px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Label ID="lbldep" runat="server" Font-Bold="true" CssClass="font" Text="Department"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel_Department" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_Department" runat="server" ReadOnly="true" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="medium" CssClass="Dropdown_Txt_Box">---Select---</asp:TextBox>
                            <asp:Panel ID="panel_Department" runat="server" Height="300px" Width="300px" CssClass="multxtpanel">
                                <asp:CheckBox ID="cb_Department" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_Department_CheckedChanged" />
                                <asp:CheckBoxList ID="cbl_Department" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cbl_Department_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_Department"
                                PopupControlID="panel_Department" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Label ID="lbldesignation" runat="server" Font-Bold="true" CssClass="font" Text="Designation"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel_Designation" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_designation" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="panel_Designation" runat="server" Height="300px" Width="300px" CssClass="multxtpanel">
                                <asp:CheckBox ID="cb_Designation" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_Designation_CheckedChanged" />
                                <asp:CheckBoxList ID="cbl_Designation" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cbl_Designation_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_designation"
                                PopupControlID="panel_Designation" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblstaffcategory" runat="server" Font-Bold="true" CssClass="font"
                        Text="Staff Category"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel_Category" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_Category" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="panel_Category" runat="server" CssClass="multxtpanel" Width="200px"
                                Height="250px">
                                <asp:CheckBox ID="cb_Category" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    OnCheckedChanged="cb_Category_CheckedChanged" Font-Size="Medium" Text="Select All"
                                    AutoPostBack="True" />
                                <asp:CheckBoxList ID="cbl_Category" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    OnSelectedIndexChanged="cbl_Category_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_Category"
                                PopupControlID="panel_Category" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Label ID="lblstafftype" runat="server" Font-Bold="true" CssClass="font" Text="Staff Type"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_stafftype" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="panel_stafftype" runat="server" CssClass="multxtpanel" Width="200px"
                                Height="250px">
                                <asp:CheckBox ID="cbstafftype" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    OnCheckedChanged="cbstafftype_CheckedChanged" Font-Size="Medium" Text="Select All"
                                    AutoPostBack="True" />
                                <asp:CheckBoxList ID="cblstafftype" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    OnSelectedIndexChanged="cblstafftype_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_stafftype"
                                PopupControlID="panel_stafftype" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Label ID="lblleave" runat="server" Font-Bold="true" CssClass="font" Text="Leave Type"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_leavetype" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="panelleave" runat="server" CssClass="multxtpanel" Width="200px" Height="300px">
                                <asp:CheckBox ID="cbleave" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    OnCheckedChanged="cbleave_CheckedChanged" Font-Size="Medium" Text="Select All"
                                    AutoPostBack="True" />
                                <asp:CheckBoxList ID="cblleave" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    OnSelectedIndexChanged="cblleave_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_leavetype"
                                PopupControlID="panelleave" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_fromdate2" runat="server" Text="From" Font-Size="Medium" Font-Bold="True"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtfromdate" Height="16px" Width="100px" runat="server" AutoPostBack="True"
                        OnTextChanged="txtfromdate_TextChanged" CssClass="txtback" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtfromdate"
                        FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','/'" />
                    <asp:CalendarExtender ID="CalendarExtender5" Format="d/MM/yyyy" TargetControlID="txtfromdate"
                        runat="server">
                    </asp:CalendarExtender>
                    <asp:Label ID="lbl_todate2" runat="server" Text="To" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txttodate" Height="16px" Width="78px" runat="server" AutoPostBack="True"
                        OnTextChanged="txttodate_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txttodate"
                        FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','/'" />
                    <asp:CalendarExtender ID="CalendarExtender6" Format="d/MM/yyyy" TargetControlID="txttodate"
                        runat="server">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <asp:CheckBox ID="chkincreason" runat="server" Checked="false" Width="175px" Text="Include Leave Reason" /><%--Font-Bold="True" Font-Names="Book Antiqua"--%>
                </td>
                <td>
                    <asp:CheckBox ID="cbIncludepercent" runat="server" Checked="false" Text="Include %" />
                </td>
                <td>
                    <asp:Label ID="lblsection" runat="server" Text="Section" Font-Size="Medium" Font-Bold="True"
                        Font-Names="Book Antiqua"></asp:Label>
                    <asp:DropDownList ID="ddlsession" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="100px">
                        <asp:ListItem Value="M">M</asp:ListItem>
                        <asp:ListItem Value="E">E</asp:ListItem>
                        <asp:ListItem Value="All">All</asp:ListItem>
                    </asp:DropDownList>
                     <asp:Button ID="btn_go" runat="server" Text="Go" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" CausesValidation="False" OnClick="btn_go_Click" />
                </td>
                <td>
                    <asp:Label ID="lbl_showmessageappl" runat="server" ForeColor="Red" Text="Enter from Date first"
                        Visible="False" Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
                    
                </td>
            </tr>
          
        </table>
    </center>
    <br />
    <center>
        <div>
            <asp:Panel ID="pheaderfilter" runat="server" CssClass="cpHeader" BackColor="#0CA6CA"
                Width="936px">
                <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                    Font-Bold="True" Font-Names="Book Antiqua" />
                <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                    ImageAlign="Right" />
            </asp:Panel>
        </div>
        <asp:Panel ID="pcolumnorder" runat="server" Width="940px">
            <center>
                <table style="background-color: mintcream; width: 936px;">
                    <tr>
                        <td>
                            <asp:TextBox ID="txt_order" Visible="false" Width="930px" Height="20px" TextMode="MultiLine"
                                Style="resize: none;" AutoPostBack="true" runat="server" Enabled="false">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%--del--%>
                            <asp:CheckBox ID="Cbcolumn" runat="server" AutoPostBack="true" Font-Bold="True" Font-Names="Book Antiqua"
                                Style="margin-left: 2.5px;" Font-Size="Medium" OnCheckedChanged="Cbcolumn_CheckedChanged"
                                Text="Select All" />
                            &nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                            <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                OnClick="lnk_columnorder_Click" Style="font-family: 'Book Antiqua'; font-weight: 700;
                                font-size: small;" Width="111px">Remove  All</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBoxList ID="cblcolumnorder" runat="server" AutoPostBack="true" Height="43px"
                                OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged" RepeatColumns="5"
                                RepeatDirection="Horizontal" Style="font-family: 'Book Antiqua'; font-weight: 700;
                                font-size: medium;" Width="928px">
                                <asp:ListItem Selected="True" Value="m.staff_code">Staff Code</asp:ListItem>
                                <asp:ListItem Selected="True" Value="staff_name">Staff Name</asp:ListItem>
                                <asp:ListItem Value="h.dept_name">Department</asp:ListItem>
                                <asp:ListItem Selected="True" Value="h.dept_acronym">Department Acr</asp:ListItem>
                                <asp:ListItem Value="d.desig_name">Designation</asp:ListItem>
                                <asp:ListItem Value="s.category_name">Staff Category</asp:ListItem>
                                <asp:ListItem Value="stftype">Staff Type</asp:ListItem>
                                <asp:ListItem Value="''Session">Session</asp:ListItem>
                                <asp:ListItem Selected="False" Value="InoutTime">In Out Time</asp:ListItem>
                                <asp:ListItem Selected="False" Value="IncludeHoliday">Include Holiday</asp:ListItem>
                            </asp:CheckBoxList>
                            <asp:LinkButton ID="Lnkbtn_groupSettings" runat="server" OnClick="Lnkbtn_groupSettingsOnclick">Leave Group Settings</asp:LinkButton>
                            <span runat="server">Print Row Count</span>
                            <asp:TextBox ID="txtPrint" Text="35" runat="server" Width="40px" CssClass="textbox textbox1"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </center>
        </asp:Panel>
        <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
            CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
            TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="../images/right.jpeg"
            ExpandedImage="../images/down.jpeg">
        </asp:CollapsiblePanelExtender>
    </center>
    <center>
        <br />
        <asp:Label ID="lblError" runat="server" Font-Size="Medium" ForeColor="Red" Font-Bold="True"
            Font-Names="Book Antiqua" Text="" Visible="true"></asp:Label>
        <br />
        <br />
        <asp:UpdatePanel ID="UPD5" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="UPD5">
                    <ProgressTemplate>
                        <center>
                            <div class="CenterPB" style="height: 40px; width: 40px;">
                                <img src="../images/progress2.gif" height="180px" width="180px" />
                            </div>
                        </center>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress1"
                    PopupControlID="UpdateProgress1">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="Upd1" runat="server">
            <ContentTemplate>
                <FarPoint:FpSpread ID="FpSpread2" runat="server" CssClass="spreadborder" ShowHeaderSelection="false">
                    <CommandBar Visible="false">
                    </CommandBar>
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <asp:Label ID="lblvalidation" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Style="top: 296px; position: absolute;" Font-Size="Medium" ForeColor="Red" Text=""
            Visible="false"></asp:Label>
        <asp:Label ID="lblvalidation1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
        <br />
        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Report Name" Visible="false"></asp:Label>
        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Visible="false"
            onkeypress="display()"></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+\}{][':;?,.">
        </asp:FilteredTextBoxExtender>
        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Visible="false" OnClick="btnxl_Click" />
        <asp:Button ID="btnprintmaster" runat="server" Text="Print" Font-Names="Book Antiqua"
            Font-Size="Medium" Font-Bold="true" Visible="false" OnClick="btnprintmaster_Clcik" />
        <br />
        <br />
        <asp:Label ID="lblcatwise" runat="server" Text="" Font-Bold="true" Font-Names="Book Antiqua"
            Font-Size="Larger" ForeColor="Green" Visible="false"></asp:Label><%--Todays break up--%>
        <br />
        <br />
        <FarPoint:FpSpread ID="FpSpread3" runat="server" BorderWidth="1px" ShowHeaderSelection="false"
            CssClass="spreadborder">
            <CommandBar BackColor="Control" ButtonFaceColor="Control" Visible="false" ButtonHighlightColor="ControlLightLight">
            </CommandBar>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
        <br />
        <asp:Label ID="lblspr3validation" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
        <br />
        <asp:Label ID="lblspr3rptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Report Name" Visible="false"></asp:Label>
        <asp:TextBox ID="txtspr3rpt" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Visible="false"
            onkeypress="display1()"></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtspr3rpt"
            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+\}{][':;?,.">
        </asp:FilteredTextBoxExtender>
        <asp:Button ID="btnspr3expexcel" runat="server" Text="Export Excel" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Medium" Visible="false" OnClick="btnspr3expexcel_Click" />
        <asp:Button ID="btnspr3prnt" runat="server" Text="Print" Font-Names="Book Antiqua"
            Font-Size="Medium" Font-Bold="true" Visible="false" OnClick="btnspr3prnt_Clcik" />
        <asp:Button ID="btnPrintpdf" runat="server" Text="Direct Print" Font-Names="Book Antiqua"
            Font-Size="Medium" Font-Bold="true" Visible="false" OnClick="btnPrintpdfClcik" />
        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
        <Insproplus:printmaster runat="server" ID="Printmaster1" Visible="false" />
        <center>
            <div id="LeaveGroupSettings" runat="server" visible="false" style="height: 100%;
                z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                top: 0; left: 0px;">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="792px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 191px; margin-left: 224px;"
                    OnClick="imagebtnpopclose1_Click" />
                <center>
                    <div id="Div4" runat="server" class="table" style="background-color: White; height: 188px;
                        width: 467px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <br />
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_columnordertype" Text="Type" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Button ID="btn_addtype" runat="server" Text="+" Height="30px" Width="30px" CssClass="textbox textbox1 btn1"
                                        OnClick="btn_addtype_OnClick" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_coltypeadd" Height="30px" Width="150px" runat="server"
                                        CssClass=" textbox1 ddlheight4" OnSelectedIndexChanged="ddl_coltypeadd_selectedindexchange"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btn_deltype" runat="server" Text="-" Height="30px" Width="30px" CssClass="textbox textbox1 btn1"
                                        OnClick="btn_deltype_OnClick" />
                                </td>
                                <td>
                                    <asp:Label ID="Label6" Text="Priority" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_priority" Height="30px" Width="60px" runat="server" CssClass="textbox1 ddlheight4">
                                        <%--OnSelectedIndexChanged="ddl_priority_selectedindexchange" AutoPostBack="true"--%>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="rdb_leave" runat="server" Text="Leave Type"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtLeave" runat="server" CssClass="textbox textbox1 txtheight" ReadOnly="true"
                                                Enabled="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel8" runat="server" BackColor="White" BorderColor="Black" BorderStyle="ridge"
                                                BorderWidth="2px" CssClass="multxtpanel" Width="164px" Height="250px" Style="position: absolute;">
                                                <asp:CheckBox ID="cbLeave1" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cbLeave1Changed" />
                                                <asp:CheckBoxList ID="cblLeave1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblLeave1SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txtLeave"
                                                PopupControlID="Panel8" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td colspan="2">
                                    <asp:Button ID="btn_saveheader" runat="server" Text="Save" Height="30px" Width="50px"
                                        CssClass="textbox textbox1 btn1" OnClick="btnsavegroupbt_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </div>
            <center>
                <div id="imgdiv33" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="panel_description11" runat="server" visible="false" class="table" style="background-color: White;
                            height: 120px; width: 430px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
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
                                            Font-Size="Medium" CssClass="textbox btn1" Height="30px" OnClick="btndescpopadd_Click" />
                                        <asp:Button ID="btn_exitdesc1" runat="server" Text="Exit" Height="30px" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox btn1" OnClick="btndescpopexit_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                </div>
            </center>
        </center>
    </center>
</asp:Content>
