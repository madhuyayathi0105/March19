<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true" CodeFile="CO_StudentTutor.aspx.cs" Inherits="CO_StudentTutor" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" >
    <title>Tutor</title>
    <%--<link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />--%>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
      <script src="Styles/~/Scripts/jquery-latest.min.js" type="text/javascript"></script>
    <style type="text/css">
        .div
        {
            left: 0%;
            top: 0%;
            position: fixed;
            width: 100%;
            z-index: 1000;
            height: 100px;
            background-color: lightblue;
            border-style: 1px;
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
    <form id="form1" >
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
   
    <br />
    <div>
        <center>
            <span class="fontstyleheader" style="color: Green;">Student Mentor</span>
             <br />  <br />
        </center>
        <center>
            <div>
                <%--maincontent--%>
                <div class="maindivstyle" style="width: 1000px; height: 850px;">
                    <br />
                    <table class="maintablestyle" width="940px">
                        <tr>
                           <%-- <td>
                                <asp:Label ID="lbl_hostelname" runat="server" Text="Hostel Name"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_hostelname" runat="server" CssClass="textbox textbox1 ddlheight4"
                                    OnSelectedIndexChanged="ddlHostelName_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>--%>
                              <td>
                            <asp:Label ID="lbl_hostelname" Text="Hostel Name" runat="server" Width="95px"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="upp_hostelname" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_hostelname" runat="server" CssClass="textbox textbox1 txtheight4"
                                        ReadOnly="true" onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_hostelname" runat="server" BorderStyle="Solid" BorderWidth="2px"
                                        CssClass="multxtpanel" Style="position: absolute;" Height="200px" Width="150px">
                                        <asp:CheckBox ID="cb_hostelname" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_hostelname_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_hostelname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_hostelname_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popupext_hostelname" runat="server" TargetControlID="txt_hostelname"
                                        PopupControlID="panel_hostelname" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                            <td>
                                <asp:Label ID="lbl_building" runat="server" Text="Building"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upp_building" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_buildingname" runat="server" CssClass="textbox textbox1 txtheight2"
                                            Height="20px" ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="panel_building" runat="server" Height="200px" Width="150px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_buildingname" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="chkbuildname_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_buildingname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklstbuildname_Change">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popupext_buildingname" runat="server" TargetControlID="txt_buildingname"
                                            PopupControlID="panel_building" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_floorname" runat="server" Text="Floor"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upp_floorname" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_floorname" runat="server" CssClass="textbox textbox1 txtheight2"
                                            Height="20px" ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="panel_floorname" runat="server" Height="200px" Width="150px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_floorname" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="chkflrname_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_floorname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklstflrname_Change">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popupext_floorname" runat="server" TargetControlID="txt_floorname"
                                            PopupControlID="panel_floorname" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_roomname" runat="server" Text="Room"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upp_roomname" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_roomname" runat="server" CssClass="textbox textbox1 txtheight2"
                                            ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="panel_roomname" runat="server" CssClass="multxtpanel" Height="200px"
                                            Width="120px">
                                            <asp:CheckBox ID="cb_roomname" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="chkroomname_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_roomname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklstroomname_Change">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popupext_roomname" runat="server" TargetControlID="txt_roomname"
                                            PopupControlID="panel_roomname" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%--<asp:Label ID="lbl_staffname" runat="server" Text="Staff Name"></asp:Label>--%>

                                 <asp:DropDownList ID="ddl_stud_staff" runat="server" CssClass="textbox1" Style="width:105px;height: 30px;"
                                    OnSelectedIndexChanged="ddl_stud_staff_SelectedIndexChanged" AutoPostBack="true" 
> <asp:ListItem Value="0">Student Name</asp:ListItem>
                            <asp:ListItem Value="1">Staff Name</asp:ListItem>                          

                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_staffname" Visible="false" runat="server" placeholder="Search Staff Name" CssClass=" textbox textbox1 txtheight4"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="acext_staffname" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffname"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                </asp:AutoCompleteExtender>

                                 <asp:TextBox ID="txt_studname"  Visible="false" runat="server" placeholder="Search Student Name" CssClass=" textbox textbox1 txtheight4"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getstud_Name" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_studname"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                </asp:AutoCompleteExtender>


                            </td>
                            <td colspan="6">
                                <asp:Button ID="btn_question" runat="server" CssClass="textbox btn" Text="?" OnClick="btnQ_Click" />
                                <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                                <asp:Button ID="btn_addnew" runat="server" Text="Add New" CssClass="textbox btn2"
                                    OnClick="btnaddnew_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                    <center>
                        <%--div id="divSheet" runat="server" style="width: 850px; height: 350px; overflow: auto;
                        border: 1px solid Gray; background-color: White; border-radius: 10px;">
                        <br />
                       <center>--%>
                        <br />
                    </center>
                    <div id="divdcorder" runat="server">
                        <div>
                            <center>
                                <asp:Panel ID="panel_filter" runat="server" CssClass="maintablestyle" Height="22px"
                                    Width="940px" Style="margin-top: -0.1%;">
                                    <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                    <asp:Label ID="lbl_filter" Text="Column Order" runat="server" Font-Size="Medium"
                                        Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                    <asp:Image ID="img_filter" runat="server" CssClass="cpimage" ImageUrl="right.jpeg"
                                        ImageAlign="Right" />
                                </asp:Panel>
                            </center>
                        </div>
                        <br />
                        <center>
                            <asp:Panel ID="panel_columnorder" runat="server" CssClass="maintablestyle" Width="940px">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cb_columnorder" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_columnorder_CheckedChanged" />
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                                Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -520px;"
                                                Visible="false" Width="111px" OnClick="lnk_columnorder_Click">Remove  All</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                            <asp:TextBox ID="txt_columnorder" Visible="false" Width="930px" TextMode="MultiLine"
                                                CssClass="style1" AutoPostBack="true" runat="server" Enabled="false">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBoxList ID="cbl_columnorder" runat="server" Height="43px" AutoPostBack="true"
                                                Width="928px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                                RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cbl_columnorder_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Value="Roll_No">Roll No</asp:ListItem>
                                                <asp:ListItem Value="Reg_No">Reg No </asp:ListItem>
                                                <asp:ListItem Selected="True" Value="Stud_Name">Student Name</asp:ListItem>
                                                <asp:ListItem Selected="True" Value="Staff_Name">Staff Name</asp:ListItem>
                                                <asp:ListItem Value="HostelName">Hostel Name</asp:ListItem>
                                                <asp:ListItem Value="BuildingFK">Building Name</asp:ListItem>
                                                <asp:ListItem Value="FloorFK">Floor Name</asp:ListItem>
                                                <asp:ListItem Value="RoomFK">Room Name</asp:ListItem>
                                               <%-- <asp:ListItem Value="Room_Type">Room Type</asp:ListItem>--%>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <br />
                                </table>
                            </asp:Panel>
                        </center>
                        <asp:CollapsiblePanelExtender ID="cpext_columnorder" runat="server" TargetControlID="panel_columnorder"
                            CollapseControlID="panel_filter" ExpandControlID="panel_filter" Collapsed="true"
                            TextLabelID="lbl_filter" CollapsedSize="0" ImageControlID="img_filter" CollapsedImage="right.jpeg"
                            ExpandedImage="down.jpeg">
                        </asp:CollapsiblePanelExtender>
                    </div>
                    <br />
                    <center>
                        <div id="maindiv" runat="server" visible="false" style="width: 820px; height: 360px;
                            overflow: auto; background-color: White; border-radius: 10px;">
                            <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" Width="800px" OnUpdateCommand="Fpspread1_Command" Style="height: 350px;
                                border-radius: 10px; overflow: auto; border: 0px solid #999999; background-color: White;
                                box-shadow: 0px 0px 8px #999999;">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                        <br />
                        <center>
                            <asp:Button ID="btn_delete" Visible="false" runat="server" Text="Delete" CssClass="textbox btn2"
                                OnClick="btn_delete_Click" />
                        </center>
                        <br />
                        <div id="rptprint" runat="server" visible="false">
                         <asp:Label ID="lbl_validation" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                    Visible="false"></asp:Label>
                          
                            <asp:Label ID="lbl_rptname" runat="server" 
                                Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txt_excelname" runat="server" CssClass="textbox textbox1 txtheight4"
                                 onkeypress="display()"></asp:TextBox>
                            <%--   theivamani 15.10.15--%>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txt_excelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btn_excel" runat="server" 
                                OnClick="btn_excel_Click" CssClass="textbox" Text="Export To Excel" Width="127px"
                                Height="30px" />
                            <asp:Button ID="btn_printmaster" runat="server" Text="Print" OnClick="btn_printmaster_Click"
                                 Width="60px" Height="30px" CssClass="textbox" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                    </center>
                </div>
                <%--pop up add new--%>
                <center>
                    <div id="popAddNew" runat="server" visible="false" class="popupstyle popupheight1">
                        <br />
                        <div class="subdivstyle" style="background-color: White; height: 628px; width: 900px;">
                            <asp:ImageButton ID="imgbtn_popupclose" runat="server" Width="40px" Height="40px"
                                ImageUrl="~/images/close.png" Style="height: 30px; width: 30px; position: absolute;
                                margin-top: -37px; margin-left: 430px;" OnClick="imagebtnpopclose_Click" />
                            <br />
                            <center>
                                <asp:Label ID="lbl_header2" runat="server" class="fontstyleheader" style="color: Green;" Text="Student Mentor"></asp:Label>
                            </center>
                            <br />
                            <div align="center" style="overflow: auto; width: 850px; height: 490px; border-radius: 10px;
                                border: 1px solid Gray;">
                                <br />
                                <center>
                                    <div  position: absolute;">
                                        <table class="maintablestyle">
                                            <tr>
                                                <%--<td>
                                                    <asp:Label ID="lbl_hostelname1" runat="server" Text="Hostel Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_hostelname1" runat="server" CssClass="textbox textbox1 ddlheight2"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddl2HostelName_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>--%>
                                                  <td>
                                <asp:Label ID="lbl_hostel" Text="Hostel Name" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Upp2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_hostelnameadd" runat="server" CssClass="textbox txtheight3 textbox1"
                                            ReadOnly="true" onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                        <asp:Panel ID="p1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Style="width: 160px; height: 200px;">
                                            <asp:CheckBox ID="cb_hostelnameadd" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_hostelnameadd_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_hostelnameadd" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_hostelnameadd_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_hostelnameadd"
                                            PopupControlID="p1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                                                <td>
                                                    <asp:Label ID="lbl_building1" runat="server" Text="Building"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upp_building1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtbuildingpop1" runat="server" ReadOnly="true" Width="120px" CssClass="textbox textbox1 txtheight2">-- Select--</asp:TextBox>
                                                            <asp:Panel ID="panel_building1" runat="server" Height="200px" Width="150px" CssClass="multxtpanel">
                                                                <asp:CheckBox ID="cb_building1" runat="server" Text="Select All" AutoPostBack="true"
                                                                    OnCheckedChanged="chkbuildpop1_CheckedChanged" />
                                                                <asp:CheckBoxList ID="cbl_building1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklbuildpop1_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="popupext_building1" runat="server" TargetControlID="txtbuildingpop1"
                                                                PopupControlID="panel_building1" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_floorname1" runat="server" Text="Floor"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upp_floorname1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_floorname1" runat="server" CssClass="textbox textbox1 txtheight2"
                                                                ReadOnly="true">-- Select--</asp:TextBox>
                                                            <asp:Panel ID="panel_floorname1" runat="server" Height="200px" Width="150px" CssClass="multxtpanel">
                                                                <asp:CheckBox ID="cb_floorname1" runat="server" Text="Select All" AutoPostBack="true"
                                                                    OnCheckedChanged="chkfloorpop1_CheckedChanged" />
                                                                <asp:CheckBoxList ID="cbl_floorname1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklfloorpop1_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="popupext_floorname1" runat="server" TargetControlID="txt_floorname1"
                                                                PopupControlID="panel_floorname1" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_roomname1" runat="server" Text="Room"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upp_roomname1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_roomname1" runat="server" CssClass="textbox textbox1 txtheight2"
                                                                ReadOnly="true">-- Select--</asp:TextBox>
                                                            <asp:Panel ID="panel_roomname1" runat="server" CssClass="multxtpanel" Height="200px"
                                                                Width="120px">
                                                                <asp:CheckBox ID="cb_roomname1" runat="server" Text="Select All" AutoPostBack="true"
                                                                    OnCheckedChanged="chkroompop1_CheckedChanged" />
                                                                <asp:CheckBoxList ID="cbl_roomname1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklroompop1_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="popupext_roomname1" runat="server" TargetControlID="txt_roomname1"
                                                                PopupControlID="panel_roomname1" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_staffname1" runat="server" Text="Staff Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_staffname1" placeholder="Search Staff Name" runat="server"  CssClass="textbox textbox1 txtheight4"
                                                        Width="120px"></asp:TextBox>
                                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetStaffNameadd" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffname1"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                                                </td>
                                                <td colspan="6">
                                                    <asp:Button ID="btn_addstaffquestion" runat="server" CssClass="textbox btn" Text="?"
                                                        OnClick="btnAddStaff_Click" />
                                                    <asp:Button ID="btn_addstaffgo" runat="server" Text="Go" CssClass="textbox btn1"
                                                        OnClick="btn_addstaffgo_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </center>
                                <br />                              
                                <asp:Label ID="lbl_error1" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                               
                                
                               <%-- <div id="div1" runat="server" style="width: 800px; height: 350px; overflow: auto;
                                    border: 1px solid Gray; background-color: White; border-radius: 10px;">--%>
                                    <br />
                                    <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" BorderColor="Black"
                                        Style="border-radius: 10px;" BorderStyle="Solid" BorderWidth="1px" Width="750px"
                                        Height="325px" OnUpdateCommand="Fpspread2_Command">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                               <%-- </div>--%>
                                <br />
                            </div>
                            <br />
                            <div>
                                <center>
                                    <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="textbox btn2" OnClick="btn_save_Click" />
                                    <asp:Button ID="btn_exit" runat="server" Text="Exit" CssClass="textbox btn2" OnClick="btnexit_Click" />
                                </center>
                            </div>
                            <br />
                        </div>
                    </div>
                </center>
                <%--pop up add new Itemscheck--%>
                <center>
                    <div id="popAddStaff" runat="server" visible="false" class="popupstyle popupheight">
                        <br />
                        <div class="subdivstyle" style="background-color: White; height: 635px; width: 800px;">
                            <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                Style="height: 30px; width: 30px; position: absolute; margin-top: -37px; margin-left: 385px;"
                                OnClick="imagebtnpopclose1_Click" />
                            <br />
                            <center>
                                <asp:Label ID="lbl_selctstaffcode" runat="server" Font-Bold="true" Style="font-size: large;
                                    color: Green;" Text="Select the Staff Name"></asp:Label>
                            </center>
                            <br />
                            <div>
                                <center>
                                    <table class="maintablestyle">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_collegename2" runat="server" Text="College"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_collegename2" runat="server" AutoPostBack="true" CssClass=" textbox1 ddlheight5">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_deptname2" runat="server" Text="Department"></asp:Label>
                                                <asp:DropDownList ID="ddl_deptname2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_deptname2_SelectedIndexChanged"
                                                    CssClass=" textbox1 ddlheight4">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_search2" runat="server" Text="Search By"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_search2" runat="server" AutoPostBack="true" CssClass=" textbox1 ddlheight5"
                                                    OnSelectedIndexChanged="ddl_search2_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                                    <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_searchbyname" TextMode="SingleLine" runat="server" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_searchbyname" runat="server" TargetControlID="txt_searchbyname"
                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:AutoCompleteExtender ID="acext_searchbyname" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchbyname"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                                </asp:AutoCompleteExtender>
                                                <asp:TextBox ID="txt_searchbycode" Visible="false" TextMode="SingleLine" runat="server"
                                                    CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_searchbycode" runat="server" TargetControlID="txt_searchbycode"
                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:AutoCompleteExtender ID="acext_searchbycode" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchbycode"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                                </asp:AutoCompleteExtender>
                                                <asp:Button ID="btn_searchgo" runat="server" CssClass="textbox btn1" Text="Go" OnClick="butnsearchbygo_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div>
                                         <p style="width: 691px;" align="right">
                                            <asp:Label ID="lbl_search3" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                        </p>
                                        <p>
                                            <asp:Label ID="lbl_error3" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                        </p>
                                        <FarPoint:FpSpread ID="Fpstaff" runat="server" Visible="false" Width="600px" Style="overflow: auto;
                                            height: 300px; border: 0px solid #999999; border-radius: 10px; background-color: White;
                                            box-shadow: 0px 0px 8px #999999;" OnCellClick="Cell_Click">
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </div>
                                    <br />
                                    <center>
                                        <div>
                                            <asp:Button ID="btn_save2" runat="server" CssClass="textbox btn2" Text="Save" OnClick="btnsav_Click" />
                                            <asp:Button ID="btn_exit2" runat="server" CssClass="textbox btn2" Text="Exit" OnClick="btnex_Click" />
                                        </div>
                                    </center>
                                </center>
                            </div>
                        </div>
                    </div>
                </center>
            </div>
            <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="panel_erroralert" runat="server" class="table" style="background-color: White;
                        height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        margin-top: 200px; border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_erroralert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_erroralert" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btnerrclose_Click" Text="ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
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
                                                    OnClick="btn_sureyes_Click" Text="Yes" runat="server" />
                                                <asp:Button ID="btn_no" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                    OnClick="btn_sureno_Click" Text="No" runat="server" />
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
                <div id="savediv" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_saveconfirm" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_saveconfirm" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                    OnClick="btn_saveconfirm_Click" Text="yes" runat="server" />
                                                <asp:Button ID="btn_savenotconfirm" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                    OnClick="btn_savenotconfirm_Click" Text="No" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
        </center>
        <br />
    </div>
    </form>
</body>
</html>

</asp:Content>

