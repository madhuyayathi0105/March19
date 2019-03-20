<%@ Page Title="Student Placement Details" Language="C#" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Placement Details.aspx.cs" Inherits="Placement_Details" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Student Strength Status Report</title>
    <link href="../Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript" src="../Scripts/jquery-1.4.1.js"></script>
    <%--<script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>--%>
    <script type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
    <style type="text/css">
        .maindivstylesize
        {
            height: 12000px;
            width: 1000px;
        }
        .lbl
        {
            text-align: center;
        }
        .container
        {
            width: 100%;
        }
        .col1
        {
            float: left;
            width: 50%;
        }
        .col2
        {
            float: right;
            width: 50%;
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
        }
        
        .textboxshadow:hover
        {
            outline: none;
            border: 1px solid #BAFAB8;
            box-shadow: 0px 0px 8px #BAFAB8;
            -moz-box-shadow: 0px 0px 8px #BAFAB8;
            -webkit-box-shadow: 0px 0px 8px #BAFAB8;
        }
        .textboxchng
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
        .maindivstyle maindivstylesizes
        {
            width: 1129px;
            height: 12000px;
        }
    </style>
    <script type="text/javascript">
        function QuantityChange() {


        }
        function QuantityChange1(objRef) {
            var grdvw = document.getElementById("<%=gview.ClientID %>");
            var grid = document.getElementById('<%=gview.ClientID%>');
            var ddl = document.getElementById('MainContent_gview_selectall_0');

            if (ddl.checked == true) {
                for (var i = 1; i < grid.rows.length; i++) {
                    var ddl_select = document.getElementById('MainContent_gview_select_' + i.toString());
                    ddl_select.checked = true;

                }

            }
            else {
                for (var i = 1; i < grid.rows.length; i++) {
                    var ddl_select = document.getElementById('MainContent_gview_select_' + i.toString());
                    ddl_select.checked = false;
                }
            }




        }
        function valid2() {
            var grid = document.getElementById('<%=gview.ClientID%>');
            var ints = 0;
            var ddl = document.getElementById('MainContent_gview_selectall_0');
            if (grid.rows.length > 0) {
                if (ddl.checked == true) {
                    var btn1 = document.getElementById('<%=myLink.ClientID %>');
                    btn1.style.display = "none";
                    ints = 1;
                }
                else if (grid.rows.length > 0) {

                    for (var i = 1; i < grid.rows.length; i++) {
                        var ddl_select = document.getElementById('MainContent_gview_select_' + i.toString());
                        if (ddl_select.checked == true) {
                            var btn1 = document.getElementById('<%=myLink.ClientID %>');
                            btn1.style.display = "none";
                            ints = 1;
                        }

                    }
                }
                if (ints != 1) {
                    alert("Please Select At least  One Student");
                    return false;
                }
                else {
                    return true;
                }
            }
        }

        function checkmain() {
          
            var grid = document.getElementById('<%=gview.ClientID%>');
          
            for (var i = 0; i < grid.rows.length; i++) {
                document.getElementById('<%=gview.ClientID%>').rows[i].cells[1].style.display = "none";
                document.getElementById('<%=gview.ClientID%>').rows[i].cells[2].style.display = "none";
              
            }
        }
        function valid1() {

            var idval = "";
            var empty = "";
            var comval = "";
            idval = document.getElementById("<%=drpcompany.ClientID %>");
            comval = idval.options[idval.selectedIndex].innerHTML;
            if (comval.trim() == "" || comval.trim() == "Select") {
                idval = document.getElementById("<%=drpcompany.ClientID %>");
                idval.style.borderColor = 'Red';
                empty = "E";
            }
            idval = document.getElementById("<%=drbposition.ClientID %>");
            comval = idval.options[idval.selectedIndex].innerHTML;
            if (comval.trim() == "" || comval.trim() == "Select") {
                idval = document.getElementById("<%=drbposition.ClientID %>");
                idval.style.borderColor = 'Red';
                empty = "E";
            }
            idval = document.getElementById("<%=txtround.ClientID %>").value;
            if (idval.trim() == "") {
                idval = document.getElementById("<%=txtround.ClientID %>");
                idval.style.borderColor = 'Red';
                empty = "E";
            }
            idval = document.getElementById("<%=drpdate.ClientID %>").value;

            if (idval.trim() == "") {
                idval = document.getElementById("<%=drpdate.ClientID %>");
                idval.style.borderColor = 'Red';
                empty = "E";
            }
            if (empty.trim() != "") {
                return false;
            }
            else {
                return true;
            }
        }
        function btnroundscript() {
            var idval = document.getElementById("<%=txtround.ClientID %>").value;
        if (idval.trim() == "") {
            alert("Please Enter The Interview Round");
            return false;

          }
        }
    </script>
    <style type="text/css">
        .GridDock
        {
            overflow-x: auto;
            width: 1200px;
            padding: 0 0 0 0;
            position: static;
        }
    </style>
    <script type="text/javascript">
        function getValue(obj) {
            var fromsem = 0;
            var tosem = 0;
            var selectoption = document.getElementById("<%=ddrsem.ClientID %>");
            fromsem = selectoption.options[selectoption.selectedIndex].text;
            var selectoption1 = document.getElementById("<%=ddrtosem.ClientID %>");
            tosem = selectoption1.options[selectoption1.selectedIndex].text;

            if (fromsem <= tosem) {

            }
            else {
                alert("From Semester less than To Semester");

            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: #008000; font-size: xx-large">Student Placement
                            Report</span>
                    </div>
                </center>
                <br />
                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                    <ContentTemplate>
                        <div class="maindivstyle maindivstylesizes" id="sdiv">
                            <br />
                            <table class="maintablestyle" width="1200px">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_clgname" Width="100px" runat="server" Text="College"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlcollege" CssClass="ddlheight4 textbox textbox1" runat="server"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Stream" Width="125px" runat="server" Text="Stream"></asp:Label>
                                    </td>
                                    <td>
                                        <div style="position: relative;">
                                          
                                                    <asp:TextBox ID="txt_stream" runat="server" CssClass="textbox txtheight3 textbox1"
                                                        ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="Paneln" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                        BorderWidth="2px" CssClass="multxtpanel" Style="width: 100px; height: 130px;">
                                                        <asp:CheckBox ID="cb_stream" runat="server" Text="Select All" AutoPostBack="True"
                                                            OnCheckedChanged="cb_stream_checkedchange" />
                                                        <asp:CheckBoxList ID="cbl_stream" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_stream_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_stream"
                                                        PopupControlID="Paneln" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                              
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_graduation" Width="100px" runat="server" Text="Graduation"></asp:Label>
                                    </td>
                                    <td>
                                        <div style="position: relative;">
                                         
                                                    <asp:TextBox ID="txt_graduation" runat="server" CssClass="textbox txtheight3 textbox1"
                                                        ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="p1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                        BorderWidth="2px" CssClass="multxtpanel" Style="width: 130px; height: 130px;">
                                                        <asp:CheckBox ID="cb_graduation" runat="server" Text="Select All" AutoPostBack="True"
                                                            OnCheckedChanged="cb_graduation_checkedchange" />
                                                        <asp:CheckBoxList ID="cbl_graduation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_graduation_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_graduation"
                                                        PopupControlID="p1" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                              
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_batch" Width="102px" runat="server" Text="Batch"></asp:Label>
                                    </td>
                                    <td>
                                        <div style="position: relative;">
                                           
                                                    <asp:TextBox ID="txt_batch" runat="server" Width="129px" CssClass="textbox txtheight1 textbox1"
                                                        ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="Panel2" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                        BorderWidth="2px" CssClass="multxtpanel" Style="width: 120px; height: 150px;">
                                                        <asp:CheckBox ID="cb_batch" runat="server" Text="Select All" AutoPostBack="True"
                                                            OnCheckedChanged="cb_batch_checkedchange" />
                                                        <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_batch"
                                                        PopupControlID="Panel2" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_degree" Text="Degree" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <div style="position: relative;">
                                           
                                                    <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                        Width="149px" ReadOnly="true">-- Select--</asp:TextBox>
                                                    <asp:Panel ID="p3" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                        BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="120px" Style="position: absolute;">
                                                        <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="true"
                                                            OnCheckedChanged="cb_degree_checkedchange" />
                                                        <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender18" runat="server" TargetControlID="txt_degree"
                                                        PopupControlID="p3" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                              
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_branch" Text="Branch" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <div style="position: relative;">
                                            
                                                    <asp:TextBox ID="txt_branch" runat="server" CssClass="textbox textbox1 txtheight3"
                                                        ReadOnly="true">-- Select--</asp:TextBox>
                                                    <asp:Panel ID="p4" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                        BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                                        <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="true"
                                                            OnCheckedChanged="cb_branch_checkedchange" />
                                                        <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender19" runat="server" TargetControlID="txt_branch"
                                                        PopupControlID="p4" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                              
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_org_sem" Text="Semester" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        
                                                <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox textbox1 txtheight3" ReadOnly="true">-- Select--</asp:TextBox>
                                                <asp:Panel ID="Panel11" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                    BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                                    <asp:CheckBox ID="cb_sem" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_sem_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender20" runat="server" TargetControlID="txt_sem"
                                                    PopupControlID="Panel11" Position="Bottom">
                                                </asp:PopupControlExtender>
                                           
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_sec" Text="Section" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        
                                                <asp:TextBox ID="txt_sec" runat="server" Width="132px" CssClass="textbox textbox1 txtheight"
                                                    ReadOnly="true">-- Select--</asp:TextBox>
                                                <asp:Panel ID="Panel8" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                    BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                                    <asp:CheckBox ID="cb_sec" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_sec_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_sec" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sec_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_sec"
                                                    PopupControlID="Panel8" Position="Bottom">
                                                </asp:PopupControlExtender>
                                           
                                    </td>
                                </tr>
                                <tr>
                                
                                    <td>
                                        <asp:CheckBox ID="cb_seatchk" runat="server" Style="float: left" AutoPostBack="true"
                                            OnCheckedChanged="cb_seatchk_CheckedChanged" />
                                        <asp:Label ID="lbl_seat" Text="Seat Type" runat="server" Style="float: left"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_seat" runat="server" CssClass="textbox textbox1 txtheight11"
                                            Width="151px" ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                        <asp:Panel ID="Panel4" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="140px" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_seat" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_seat_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_seat" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_seat_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_seat"
                                            PopupControlID="Panel4" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_typechk" runat="server" Style="float: left" AutoPostBack="true"
                                            OnCheckedChanged="cb_typechk_CheckedChanged" />
                                        <asp:Label ID="lbl_type" Text="Mode" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_type" runat="server" CssClass="textbox textbox1 txtheight1"
                                            ReadOnly="true" Enabled="false" Width="136">-- Select--</asp:TextBox>
                                        <asp:Panel ID="Panel5" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="140px" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_type" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_type_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_type" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_type_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_type"
                                            PopupControlID="Panel5" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </td>
                                    <td >
                                        <asp:CheckBox ID="cb_relichk" runat="server" Style="float: left" AutoPostBack="true"
                                            OnCheckedChanged="cb_relichk_CheckedChanged" />
                                        <asp:Label ID="lbl_religion" Text="Religion" runat="server"></asp:Label></td>
                                        <td>
                                        <asp:TextBox ID="txt_religion" runat="server" Width="134px" CssClass="textbox textbox1 txtheight"
                                            ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                        <asp:Panel ID="Panel6" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="150px" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_religion" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_religion_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_religion" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_religion_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_religion"
                                            PopupControlID="Panel6" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </td>

                                      <td>
                                        <asp:CheckBox ID="cb_commchk" runat="server" Style="float: left" AutoPostBack="true"
                                            OnCheckedChanged="cb_commchk_CheckedChanged" />
                                        <asp:Label ID="lbl_comm" Text="Community" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_comm" runat="server" CssClass="textbox textbox1 txtheight3"
                                            ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                        <asp:Panel ID="Panel7" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="140px" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_comm" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_comm_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_comm" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_comm_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_comm"
                                            PopupControlID="Panel7" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </td>
                                    <%--  <td>
                                    <asp:RadioButton ID="rdb_cumm" runat="server" Text="Cumulative" GroupName="a"
                                        AutoPostBack="true" Visible="false"
                                         OnCheckedChanged="rdb_cumm_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_detail" Visible="true" runat="server" Text="Details" GroupName="a"
                                        AutoPostBack="true" OnCheckedChanged="rdb_detail_CheckedChanged" Checked="true" />
                                </td>--%>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cb_studtypechk" runat="server" Style="float: left" AutoPostBack="true"
                                            OnCheckedChanged="cb_studtypechk_CheckedChanged" />
                                        <asp:Label ID="lbl_studtype" Text="Student Type" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        
                                                <asp:TextBox ID="txt_studtype" runat="server" Width="150px" CssClass="textbox textbox1 txtheight3"
                                                    ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                                <asp:Panel ID="Panel3" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                    BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="140px" Style="position: absolute;">
                                                    <asp:CheckBox ID="cb_studtype" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_studtype_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_studtype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_studtype_SelectedIndexChanged">
                                                        <%--  <asp:ListItem Value="Hostler','Day Scholar">Both</asp:ListItem>
                                                <asp:ListItem Value="Hostler">Hostler</asp:ListItem>
                                                <asp:ListItem Value="Day Scholar">Day Scholar</asp:ListItem>--%>
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_studtype"
                                                    PopupControlID="Panel3" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            
                                    </td>
                                  
                                    <td runat="server" id="tdsports">
                                        <asp:CheckBox ID="cb_sports" runat="server" Style="float: left" AutoPostBack="true"
                                            OnCheckedChanged="cb_sports_CheckedChanged" />
                                        <asp:Label ID="lbl_sports" Text="Sports" runat="server"></asp:Label>
                                    </td>
                                    <td runat="server" id="tdsports1">
                                        <asp:TextBox ID="txt_sports" runat="server" Width="133px" CssClass="textbox textbox1 txtheight3"
                                            ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                        <asp:Panel ID="Panel12" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Height="160px" Width="140px" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_sport" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_sport_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_sport" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sport_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txt_sports"
                                            PopupControlID="Panel12" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </td>
                                
                                    
                                    <td colspan="2" runat="server" id="tdmothertng">
                                        <asp:CheckBox ID="cb_mothertng" runat="server" Style="float: left" AutoPostBack="true"
                                            OnCheckedChanged="cb_mothertng_CheckedChanged" />
                                        <asp:Label ID="lbl_mothertng" Text="MotherTongue" Style="float: left" runat="server"></asp:Label>
                                        <%--</td>
                                <td>--%>
                                        <asp:TextBox ID="txt_mothertng" runat="server" CssClass="textbox textbox1 txtheight2"
                                            ReadOnly="true" Width="121px" Enabled="false">-- Select--</asp:TextBox>
                                        <asp:Panel ID="Panel14" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="140px" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_mothertongue" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_mothertongue_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_mothertongue" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_mothertongue_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender14" runat="server" TargetControlID="txt_mothertng"
                                            PopupControlID="Panel14" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </td>
                                    <td colspan="3" runat="server" id="tdphychallange" visible="true">
                                        <asp:CheckBox ID="cb_phychallange" runat="server" Style="float: left" AutoPostBack="true"
                                            OnCheckedChanged="cb_phychallange_CheckedChanged" />
                                        <asp:Label ID="lbl_phychallange" Text="PhysicalChallange" Style="float: left" runat="server"></asp:Label>
                                        
                                                <asp:TextBox ID="txt_phychallage" Style="float: left" runat="server" CssClass="textbox textbox1 txtheight3"
                                                    Width="108px" ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                                <asp:Panel ID="Panel15" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                    BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="140px" Style="position: absolute;">
                                                    <asp:CheckBox ID="cb_phychlg" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_phychlg_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_phychlg" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_phychlg_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender15" runat="server" TargetControlID="txt_phychallage"
                                                    PopupControlID="Panel15" Position="Bottom">
                                                </asp:PopupControlExtender>
                                          
                                    </td>
                                    <td colspan="2" runat="server" id="tdtransport" visible="false">
                                        <asp:CheckBox ID="cb_trans" Enabled="false" runat="server" Style="float: left" AutoPostBack="true"
                                            OnCheckedChanged="cb_trans_CheckedChanged" />
                                        <asp:Label ID="lbl_transport" Text="Transport Type" Style="float: left" runat="server"></asp:Label>
                                      
                                                <asp:TextBox ID="txt_transport" Style="float: left" runat="server" CssClass="textbox textbox1 "
                                                    Width="108px" ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                                <asp:Panel ID="Panel16" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                    BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="140px" Style="position: absolute;">
                                                    <asp:CheckBox ID="cb_transport" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_transport_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_transport" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_transport_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender16" runat="server" TargetControlID="txt_transport"
                                                    PopupControlID="Panel16" Position="Bottom">
                                                </asp:PopupControlExtender>
                                           
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cb_Gender" runat="server" Style="float: left" AutoPostBack="true"
                                            OnCheckedChanged="cb_EnGender_CheckedChanged" />
                                        <asp:Label ID="lbl_gen" Text="Gender" Style="float: left" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_gen" Enabled="false" runat="server" CssClass="textbox  textbox1 txtheight3"
                                            ReadOnly="true" Width="152">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel17" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Width="150px" Height="180px" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_gen" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_Gender_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_gen" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_gen_SelectedIndexChanged">
                                                <asp:ListItem Value="0">Male</asp:ListItem>
                                                <asp:ListItem Value="1">Female</asp:ListItem>
                                                <%-- <asp:ListItem Value="2">Both</asp:ListItem>--%>
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender17" runat="server" TargetControlID="txt_gen"
                                            PopupControlID="Panel17" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </td>
                                    <td runat="server" id="tdcbboard">
                                        <asp:CheckBox ID="cb_board" Checked="false" runat="server" Style="float: left" AutoPostBack="true"
                                            OnCheckedChanged="cb_board_CheckedChanged" />
                                        <asp:Label ID="lbl_board" Text="Board" Style="float: left" runat="server"></asp:Label>
                                    </td>
                                    <td runat="server" id="tdboard">
                                        <asp:TextBox ID="txtBoardUniv" runat="server" CssClass="textbox textbox1 txtheight"
                                            ReadOnly="true" Enabled="false" Width="132px">Board</asp:TextBox>
                                        <asp:Panel ID="pnlBoardUniv" runat="server" BackColor="White" BorderColor="Black"
                                            BorderStyle="Solid" BorderWidth="2px" CssClass="multxtpanel" Width="140px" Height="250px"
                                            Style="position: absolute;">
                                            <asp:CheckBox ID="cb_BoardUniv" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_BoardUniv_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_BoardUniv" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_BoardUniv_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pceBoardUniv" runat="server" TargetControlID="txtBoardUniv"
                                            PopupControlID="pnlBoardUniv" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </td>
                                    <td runat="server" id="tdcbstate">
                                        <asp:CheckBox ID="cb_state" Checked="false" runat="server" Style="float: left" AutoPostBack="true"
                                            OnCheckedChanged="cb_unistate_CheckedChanged" />
                                        <asp:Label ID="lbl_state" Text="State" Style="float: left" runat="server"></asp:Label>
                                    </td>
                                    <td runat="server" id="tdstate">
                                        <asp:TextBox ID="txtstate" runat="server" CssClass="textbox textbox1 txtheight" Width="135"
                                            ReadOnly="true" Enabled="false">State</asp:TextBox>
                                        <asp:Panel ID="pnlstate" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Width="140px" Height="250px" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_states" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_state_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_state" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_state_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pcestate" runat="server" TargetControlID="txtstate"
                                            PopupControlID="pnlstate" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </td>
                                    <%--         Added By Saranyadevi 24.2.2018--%>
                                    <td runat="server" id="tdcbdisreason" visible="false">
                                        <asp:CheckBox ID="cb_Disreaason" Checked="false" runat="server" Style="float: left"
                                            AutoPostBack="true" OnCheckedChanged="cb_Disreaason_CheckedChanged" />
                                        <asp:Label ID="lblReason" Text="DisContinue Reason" Style="float: left" runat="server"></asp:Label>
                                    </td>
                                    <td runat="server" id="tddisreason" visible="false">
                                       
                                                <asp:TextBox ID="txt_reason" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true"
                                                    Enabled="false" CssClass="textbox textbox1 txtheight">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel_reason" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                                    height: auto;">
                                                    <asp:CheckBox ID="cb_reason" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_reason_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_reason" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_reason_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="pce_reason" runat="server" TargetControlID="txt_reason"
                                                    PopupControlID="panel_reason" Position="Bottom">
                                                </asp:PopupControlExtender>
                                           
                                    </td>
                                    <td runat="server" id="tdquota" visible="false">
                                        <asp:CheckBox ID="cbquotacheck" Checked="false" runat="server" Style="float: left"
                                            AutoPostBack="true" OnCheckedChanged="cb_quota_CheckedChanged" />
                                        <asp:Label ID="lblquota" Text="Quota/Category" Style="float: left" runat="server"></asp:Label>
                                    </td>
                                    <td runat="server" id="tdquotapanel" visible="false">
                                      
                                                <asp:TextBox ID="txtQuota" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true"
                                                    Enabled="false" CssClass="textbox textbox1 txtheight">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel18" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                                    height: auto;">
                                                    <asp:CheckBox ID="cbQuota" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cbQuota_checkedchange" />
                                                    <asp:CheckBoxList ID="cblQuota" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblQuota_SelectedIndexChange">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender21" runat="server" TargetControlID="txtQuota"
                                                    PopupControlID="panel18" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_colord" runat="server" Text="Report Type"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_colord" runat="server" CssClass="ddlheight3 textbox textbox1" Width="140px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="shtype" runat="server" visible="false">
                                    <%--abarna--%>
                                    <td runat="server" id="tdallotcommunity">
                                        <asp:CheckBox ID="allotcommchk" runat="server" Style="float: left" AutoPostBack="true"
                                            OnCheckedChanged="allotcommchk_CheckedChanged" />
                                        <asp:Label ID="lblallotcom" Text="Alloted community" runat="server"></asp:Label>
                                    </td>
                                    <td runat="server" id="tdallotcommunity1">
                                       
                                                <asp:TextBox ID="txt_Allotcomm" runat="server" CssClass="textbox textbox1 txtheight3"
                                                    ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                                <asp:Panel ID="pnl_allotcomm" runat="server" BackColor="White" BorderColor="Black"
                                                    BorderStyle="Solid" BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="140px"
                                                    Style="position: absolute;">
                                                    <asp:CheckBox ID="cb_allotcomm" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_allotcomm_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_allotcomm" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_allotcomm_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender22" runat="server" TargetControlID="txt_Allotcomm"
                                                    PopupControlID="pnl_allotcomm" Position="Bottom">
                                                </asp:PopupControlExtender>
                                           
                                    </td>
                                    <td runat="server" id="typenametd">
                                        <asp:CheckBox ID="chk_typename" runat="server" Style="float: left" AutoPostBack="true"
                                            OnCheckedChanged="chk_typename_CheckedChanged" />
                                        <asp:Label ID="lbl_name" Text="Type Name" runat="server"></asp:Label>
                                    </td>
                                    <td runat="server" id="typenametd2">
                                       
                                                <asp:TextBox ID="txt_Typename" runat="server" CssClass="textbox textbox1 txtheight3"
                                                    ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                                <asp:Panel ID="pnl_name" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                    BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="140px" Style="position: absolute;">
                                                    <asp:CheckBox ID="cb_typename" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_typename_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_typename" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_typename_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender23" runat="server" TargetControlID="txt_Typename"
                                                    PopupControlID="pnl_name" Position="Bottom">
                                                </asp:PopupControlExtender>
                                          
                                    </td>
                                    <td runat="server" id="typesizetd1">
                                        <asp:CheckBox ID="chk_typesizename" runat="server" Style="float: left" AutoPostBack="true"
                                            OnCheckedChanged="chk_typesizename_CheckedChanged" />
                                        <asp:Label ID="Label2" Text="Type Size" runat="server"></asp:Label>
                                    </td>
                                    <td runat="server" id="typesizetd2">
                                       
                                                <asp:TextBox ID="txt_typesize" runat="server" CssClass="textbox textbox1 txtheight3"
                                                    ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                                <asp:Panel ID="pnl_size" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                    BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="140px" Style="position: absolute;">
                                                    <asp:CheckBox ID="cb_typesize" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_typesize_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_typesize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_typesize_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender24" runat="server" TargetControlID="txt_typesize"
                                                    PopupControlID="pnl_size" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            
                                    </td>
                                </tr>
                                <tr>
                                    <td id="ar" runat="server">
                                        <asp:RadioButton ID="rdb_cumm" Visible="true" runat="server" Text="Without Arrear"
                                            GroupName="a" Checked="true" AutoPostBack="true" OnCheckedChanged="without_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rdb_detail" Visible="true" runat="server" Text="With Arrear"
                                            GroupName="a" AutoPostBack="true" OnCheckedChanged="without_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="Rdb_nil" Visible="true" runat="server" Text="Nil Arrear"
                                            GroupName="a" AutoPostBack="true" OnCheckedChanged="without_CheckedChanged" />
                                    </td>
                                    <td id="Td3" runat="server" visible="false">
                                        <fieldset style="width: 134px; height: 35px;">
                                             <asp:Label ID="Lblnil" Text="Arrear" runat="server" ></asp:Label>
                                            <asp:TextBox ID="Txtnil" runat="server" CssClass="textbox textbox1 txtheight4"
                                                Width="59px"  ></asp:TextBox>
                                                 <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="Txtnil"
                                                FilterType="numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </fieldset>
                                    </td>
                                    <td id="Td2" runat="server" visible="false">
                                        <fieldset style="width: 102px; height: 35px;">
                                            <asp:RadioButton ID="rdbatt" Visible="true" runat="server" Text="Attempts" GroupName="b"
                                                AutoPostBack="true" OnCheckedChanged="rdbatt_CheckedChanged" />
                                            <asp:RadioButton ID="rdbarrear" Visible="true" runat="server" Text="Arrear" GroupName="b"
                                                Checked="true" AutoPostBack="true" OnCheckedChanged="rdbatt_CheckedChanged" />
                                        </fieldset>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkabsent" runat="server" Visible="true" Text="Include Absent"
                                            Style="float: left" />
                                    </td>
                                    <td id="arrear" runat="server" visible="false" colspan="2">
                                        <fieldset style="width: 248px; height: 48px;">
                                          <asp:Label ID="lblatt" Text="No Attempts" runat="server" Visible="false"></asp:Label>
                                            <asp:TextBox ID="txtattempts" runat="server" CssClass="textbox textbox1 txtheight4"
                                                Width="59px"  Visible="false"></asp:TextBox>
                                                 <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtattempts"
                                                FilterType="numbers">
                                            </asp:FilteredTextBoxExtender>
                                            <%-- <asp:CheckBox ID="chkstill" runat="server" Visible="false" Text="Still Attempts" Style="float: left" />--%>
                                            <asp:CheckBox ID="chksarrear" runat="server" Visible="true" Text="Single Arrear"
                                                Style="float: left" />
                                            <asp:CheckBox ID="chsmulti" runat="server" Visible="true" Text="Multiple Arrear"
                                                Style="float: left" />
                                            <asp:CheckBox ID="chsstill" runat="server" Visible="true" Text="Still Arrear" Style="float: left" />
                                        </fieldset>
                                    </td>
                                    <td id="noarrear" runat="server" visible="true" colspan="6">
                                        <fieldset style="width: 213px; height: 35px;">

                                            <asp:Label ID="Label4" Text="From Sem" runat="server"></asp:Label>
                                            <asp:DropDownList ID="ddrsem" runat="server" CssClass="ddlheight3 textbox textbox4"
                                                Width="54px" onchange="getValue(this)">
                                                <%--OnSelectedIndexChanged="ddrtosem_selected"--%>
                                            </asp:DropDownList>
                                            <asp:Label ID="Label5" Text="To " runat="server"></asp:Label>
                                            <asp:DropDownList ID="ddrtosem" runat="server" CssClass="ddlheight3 textbox textbox4"
                                                Width="54px" onchange="getValue(this)">
                                            </asp:DropDownList>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="Td1" runat="server" visible="true" colspan="3">
                                        <fieldset style="width: 418px; height: 35px;">
                                            <asp:Label ID="Label1" Text="CGPA From Range" runat="server"></asp:Label>
                                            <asp:TextBox ID="Txtrange" runat="server" CssClass="textbox textbox1 txtheight4"
                                                Width="59px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="ftext_rollno" runat="server" TargetControlID="Txtrange"
                                                FilterType="numbers,custom" ValidChars=" .">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:DropDownList ID="drbless" runat="server" CssClass="ddlheight3 textbox textbox4"
                                                Width="54px">
                                            </asp:DropDownList>
                                            <asp:Label ID="Label3" Text="To Range" runat="server"></asp:Label>
                                            <asp:TextBox ID="Txtto" runat="server" CssClass="textbox textbox1 txtheight4" Width="59px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="Txtto"
                                                FilterType="numbers,custom" ValidChars=" .">
                                            </asp:FilteredTextBoxExtender>
                                        </fieldset>
                                    </td>
                                    <td >
                                      <asp:CheckBox ID="chkincluplacement" runat="server" Visible="true" OnCheckedChanged="chkincluplacement_CheckedChanged" Text="Include Placement" AutoPostBack="true"
                                            Style="float: left;"/></td>
                                                <td id="colspaan" runat="server" visible="false">
                                                            <asp:Label ID="lblcom" Text="Company Name" runat="server" Visible="false"></asp:Label>
                                                           
                                                        </td>
                                                        <td id="colspaan1" runat="server" visible="false">
                                                        <asp:TextBox ID="txtcompany" Visible="false" runat="server" CssClass="textbox textbox1 txtheight1"
                                        ReadOnly="true" Width="127px" Height="18px">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel1" runat="server" Visible="false" CssClass="multxtpanel" Style="width: 200px;
                                        height: 200px;">
                                        <asp:CheckBox ID="chkcom" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="chkcom_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblcom" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblcom_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txtcompany"
                                        PopupControlID="Panel1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                                        </td>
                                                         <td id="colspaan2" runat="server" visible="false">
                                                            <asp:Label ID="lblstufilter" Text="Student Filter" runat="server" Visible="false"></asp:Label>
                                                    
                                                        </td>
                                                        <td id="colspaan3" runat="server" visible="false">
                                                            <asp:DropDownList ID="ddlfilter" runat="server"  Width="173px" Visible="false"
                                                                CssClass="textbox textbox1 ddlheight4">
                                                            </asp:DropDownList>
                                                        </td>
                                                        </tr>
                                                        <tr>
                                                        <td>
                                                        <asp:CheckBox ID="chkprevious" runat="server" Visible="true" OnCheckedChanged="chkprevious_CheckedChanged" Text="previous Mark Details" AutoPostBack="true"
                                            Style="float: left;"/>
                                                        </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel25" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="btndetailgo" Visible="true" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Text="Go" CssClass="textbox btn1 textbox1" OnClick="btndetailgo_Click" /></ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <%-- End By Saranyadevi 24.2.2018--%>
                            </table>
                            <br />
                            <br />
                            <div style="border-radius: 7px; width: 400px; margin-left: 722px;">
                                <asp:ImageButton ID="imgbtn_columsetting" runat="server" Width="30px" Height="30px"
                                    Text="All" ImageUrl="~/Hostel Gete Images/images (1)ppp.jpg" OnClick="imgbtn_all_Click" />
                                <asp:LinkButton ID="myLink" Text="Schedule" OnClick="LinkButton_Click" OnClientClick="return valid2()"
                                    runat="server" Visible="false" />
                            </div>
                            <div>
                                <br />
                                <center>
                                    <asp:Label ID="lbl_headernamespd2" runat="server" ForeColor="Green" Visible="false"
                                        Font-Size="X-Large"></asp:Label>
                                    <asp:Label ID="lblerror" runat="server" ForeColor="Red" Visible="false" Font-Size="X-Large"></asp:Label>
                                </center>
                                <asp:Label ID="lbl_err_stud" Font-Bold="true" runat="server" ForeColor="Red"></asp:Label>
                                <center>
                                    <div class="GridDock" id="divgrid" >
                                        <asp:GridView ID="gview" runat="server" ShowHeader="false" Visible="false" Width="1000"  ><%--AllowPaging="true" PageSize="100"  OnPageIndexChanging="grdManualExit_OnPageIndexChanged"--%>
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <center>
                                                            <%--  <input type="CheckBox" runat="server" id="selectall" Visible="false" 
                            onclick="return QuantityChange1(this)" style="width: 100px" />
                                       <input type="CheckBox" runat="server" id="select" 
                            onclick="return QuantityChange()" style="width: 100px" />--%>
                                                            <asp:CheckBox ID="selectall" runat="server" Visible="false" onclick="return QuantityChange1(this)" />
                                                            <asp:CheckBox ID="select" runat="server" onchange="return QuantityChange()" />
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                                            <FooterStyle BackColor="White" ForeColor="#333333" />
                                            <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                            <RowStyle ForeColor="#333333" />
                                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                                        </asp:GridView>
                                    </div>
                                </center>
                                <br />
                                <br />
                            </div>
                            <center>
                             <div id="Div4" runat="server" visible="false" style="height: 355em; z-index: 1000;
                                    width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                                    left: 0;">
                                    <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                                        width: 30px; position: absolute; margin-top: -4px; margin-left: 474px;" OnClick="ImageButton3_Click" />
                                    <br />
                                    <center>
                                        <div class="popsty" style="background-color: White; height: 690px; width: 974px;
                                            border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;
                                            margin-top: -8px">
                                            <br />
                                               <center>
                                                <span style="color: Green; font-size: large;">Previous Percentage</span>
                                            </center>
                                            <br />
                                            <br />
                                            <center>
                                                <asp:GridView ID="gpremark" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#0CA6CA"
                                        HeaderStyle-ForeColor="White"  >
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"  />
                                            </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <center>
                                                            <%--  <input type="CheckBox" runat="server" id="selectall" Visible="false" 
                            onclick="return QuantityChange1(this)" style="width: 100px" />
                                       <input type="CheckBox" runat="server" id="select" 
                            onclick="return QuantityChange()" style="width: 100px" />--%>
                                                            <asp:CheckBox ID="select" runat="server" />
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Course">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Course" runat="server" Text='<%# Eval("course") %>'></asp:Label>
                                                     <asp:Label ID="Label7" Visible="false" runat="server" Text='<%# Eval("Textcode") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="From Range">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_gviewfrom" runat="server" Text=""></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"  />
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="To Range">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_gviewto" runat="server" Text=""></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"  />
                                            </asp:TemplateField>
                                          </Columns>
                                          </asp:GridView>
                                            </center>
                                          
                                            </div>
                                            </center></div>
                            </center>
                            <center>
                                <div id="poppernew" runat="server" visible="false" style="height: 355em; z-index: 1000;
                                    width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                                    left: 0;">
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                                        width: 30px; position: absolute; margin-top: -4px; margin-left: 474px;" OnClick="imagebtnpopclose1_Click" />
                                    <br />
                                    <center>
                                        <div class="popsty" style="background-color: White; height: 690px; width: 974px;
                                            border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;
                                            margin-top: -8px">
                                            <br />
                                            <br />
                                            <center>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_columnordertype" Text="Report Type" runat="server"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btn_addtype" runat="server" Text="+" CssClass="textbox textbox1 btn1"
                                                                OnClick="btn_addtype_OnClick" />
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddl_coltypeadd" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_coltypeadd_SelectedIndexChanged"
                                                                CssClass="textbox textbox1 ddlheight4">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btn_deltype" runat="server" Text="-" CssClass="textbox textbox1 btn1"
                                                                OnClick="btn_deltype_OnClick" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </center>
                                            <br />
                                            <fieldset style="border-radius: 10px; width: 900px; height: 500px">
                                                <legend style="font-size: larger; font-weight: bold">ColumnOrder Header Settings</legend>
                                                <table class="table">
                                                    <tr>
                                                        <td>
                                                            <asp:ListBox ID="lb_selectcolumn" runat="server" SelectionMode="Multiple" Height="490px"
                                                                Width="300px"></asp:ListBox>
                                                        </td>
                                                        <td>
                                                            <table class="table1">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnMvOneRt" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                                            Text=">" CssClass="textbox textbox1 btn1" OnClick="btnMvOneRt_Click" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnMvTwoRt" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                                            Text=">>" CssClass="textbox textbox1 btn1" OnClick="btnMvTwoRt_Click" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnMvOneLt" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                                            Text="<" CssClass="textbox textbox1 btn1" OnClick="btnMvOneLt_Click" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnMvTwoLt" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                                            Text="<<" CssClass="textbox textbox1 btn1" OnClick="btnMvTwoLt_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            <asp:ListBox ID="lb_column1" runat="server" SelectionMode="Multiple" Height="490px"
                                                                Width="300px"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <asp:Label ID="lblalerterr" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                                                <br />
                                                <center>
                                                    <asp:Button ID="btnok" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                        Text="OK" CssClass="textbox textbox1 btn2" OnClick="btnok_click" />
                                                    <asp:Button ID="btnclose" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                        Text="Close" CssClass="textbox textbox1 btn2" OnClick="btnclose_click" />
                                                </center>
                                            </fieldset>
                                        </div>
                                    </center>
                                </div>
                            </center>
                            <center>
                                <div id="Div1" runat="server" visible="false" style="height: 355em; z-index: 1000;
                                    width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                                    left: 0;">
                                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/close.png" Style="height: auto;
                                        width: 30px; position: absolute; margin-top: 79px; margin-left: 300px;" OnClick="imagebtnpopclose2_Click" />
                                    <br />
                                    <center>
                                        <div class="popsty" style="background-color: White; height: auto; width: 659px;
                                            border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;
                                            margin-top: 60px">
                                            <br />
                                            <br />
                                            <center>
                                                <span style="color: Green; font-size: large;">Interview Selection</span>
                                            </center>
                                            <br />
                                            <br />
                                            <center>
                                                <table border="0px">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Lblcompany" Text="Company Name" runat="server"></asp:Label>
                                                            <span style="color: Red;">*</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drpcompany" runat="server" AutoPostBack="true" Width="198px"
                                                                OnSelectedIndexChanged="drpcompany_SelectedIndexChanged" CssClass="textbox textbox1 ddlheight4">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Lblposition" Text="Position" runat="server"></asp:Label>
                                                            <span style="color: Red;">*</span>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="Button1" runat="server" Text="+" CssClass="textbox textbox1 btn1"
                                                                OnClick="btn_positionadd_OnClick" />
                                                            <asp:DropDownList ID="drbposition" runat="server" CssClass="textbox textbox1 ddlheight4">
                                                                <asp:ListItem>Select</asp:ListItem>
                                                            </asp:DropDownList>
                                                      
                                                            <asp:Button ID="Button9" runat="server" Text="-" CssClass="textbox textbox1 btn1"
                                                                OnClick="btn_position_OnClick" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label8" Text="Salary" runat="server"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="Button6" runat="server" Text="+" CssClass="textbox textbox1 btn1"
                                                                OnClick="btn_salary_OnClick" />
                                                            <%--  <asp:TextBox ID="drbsalary" runat="server" Height="18px" Width="80px" 
                                                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"> </asp:TextBox>
                                                                   <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="drbsalary"
                        FilterType="Numbers" ValidChars=",">
                    </asp:FilteredTextBoxExtender>--%>
                                                            <asp:DropDownList ID="drbsalary" runat="server" CssClass="textbox textbox1 ddlheight4">
                                                            </asp:DropDownList>
                                                        
                                                            <asp:Button ID="Button7" runat="server" Text="-" CssClass="textbox textbox1 btn1"
                                                                OnClick="btnex_OnClick" />
                                                        </td>
                                                    </tr>
                                                    <tr id="Tr1" runat="server">
                                                        <td>
                                                            <asp:Label ID="lblindate" Text="Interview Date" runat="server"></asp:Label>
                                                            <span style="color: Red;">*</span>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="Updp_todate" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="drpdate" runat="server" Height="18px" Width="80px" AutoPostBack="True"
                                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"> </asp:TextBox>
                                                                    <asp:CalendarExtender ID="CalendarExtender2" Format="d-MM-yyyy" TargetControlID="drpdate"
                                                                        runat="server">
                                                                    </asp:CalendarExtender>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_starttime1" Text="Time" runat="server"></asp:Label>
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
                                                        <td>
                                                            <asp:Label ID="lblround" Text="No Of Round" runat="server"></asp:Label>
                                                             <span style="color: Red;">*</span>
                                                        </td>
                                                        <td>
                                                         <asp:TextBox ID="txtround" runat="server" Height="18px" Width="80px" AutoPostBack="True"
                                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                                        <asp:Button ID="btnroundgo" runat="server" Text="Go" BackColor="#d78af3" CssClass="textbox textbox1 btn1"
                                                                OnClick="btnroundgo_OnClick" Width="50px" OnClientClick="return btnroundscript()" />
                                                           
                                                           <%-- <asp:Button ID="btnroundadd" runat="server" Text="+" CssClass="textbox textbox1 btn1"
                                                                OnClick="btnround_OnClick" />
                                                            <asp:DropDownList ID="drpround" runat="server" Width="68px" CssClass="textbox textbox1 ddlheight8"
                                                                Height="31px">
                                                            </asp:DropDownList>
                                                            <asp:Button ID="Button13" runat="server" Text="-" CssClass="textbox textbox1 btn1"
                                                                OnClick="btnroundex_OnClick" />--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                    <td colspan="3">
                                                       <asp:RadioButton ID="rdbdetails" Visible="true" runat="server" Text="Confirm To Shortlist" GroupName="a"
                                                Checked="true" AutoPostBack="true"  />
                                                   <asp:RadioButton ID="rdbcun" Visible="true" runat="server" Text="Confirm To Shortlist After Student Desire " GroupName="a"  AutoPostBack="true"
                                                /></td> </tr>
                                                <tr> <td>
                                                 <asp:checkbox ID="rdbmail" Visible="true" runat="server" Text="Mail" GroupName="b" OnCheckedChanged="mail_onchecked"
                                                AutoPostBack="true"  />
                                                   <asp:checkbox ID="rdbsms" Visible="true" runat="server" Text="Sms " GroupName="b"  AutoPostBack="true" OnCheckedChanged="mail_onchecked"
                                                />
                                    </td>

                                    
                                                    </tr>
                                                    <tr id="mailid" runat="server" visible="false">
                                                    <td colspan="3">
                                                     <asp:TextBox ID="txtbody"  runat="server" CssClass="textbox textbox1 txtheight1"
                                        Width="563px" Height="113px" TextMode="MultiLine" Visible="false"></asp:TextBox>
                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <asp:GridView ID="gridround" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#0CA6CA"
                                        HeaderStyle-ForeColor="White" Width="459px"  >
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Mode Of Interview">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_gviewround" Width="410px" runat="server" Text='<%# Eval("No Of Rounds") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"  />
                                            </asp:TemplateField>
                                          </Columns>
                                          </asp:GridView>
                                          <br />
                                           <br />
                                        
                                                <center>
                                                    <asp:UpdatePanel ID="UpdatePanel26" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Button ID="btnallot" runat="server" BackColor="Aqua" Width="117px" Text="Allot"
                                                                CssClass="textbox textbox1 btn1" OnClick="btnallot_OnClick" OnClientClick="return valid1()" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </center>
                                            </center>
                                            <br />
                                        </div>
                                    </center>
                                </div>
                            </center>
                            <div id="imgdiv2" runat="server" visible="false" style="height: 300em; z-index: 1000;
                                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                left: 0px;">
                                <center>
                                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 400px;
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
                                                                width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" />
                                                        </center>
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </div>
                                </center>
                            </div>
                            <div id="rptprint" runat="server" visible="false">
                                <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="" Visible="false"></asp:Label>
                                <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txtexcelname" CssClass="textbox textbox1" runat="server" Height="20px"
                                    Width="180px" onkeypress="return ClearPrint()"></asp:TextBox>
                                <asp:Button ID="btnExcel" runat="server" OnClick="btnExcelNew_Click" CssClass="textbox btn1"
                                    Text="Export To Excel" Width="127px" />
                                <asp:Button ID="btnprintmaster" runat="server" Text="Print" Visible="false" OnClick="btnprintmasterNew_Click"
                                    CssClass="textbox btn2" />
                                <Insproplus:printmaster runat="server" ID="Printmaster1" Visible="false" />
                            </div>
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
                                    <asp:Button ID="btn_Excel" runat="server" Text="Export To Excel" Width="150px" CssClass="textbox textbox1 btn2"
                                        AutoPostBack="true" Font-Names="Book Antiqua" OnClick="btnExcel_Click" Font-Bold="true" />
                                    <asp:Button ID="btn_printmaster" Font-Names="Book Antiqua" runat="server" Text="Print"
                                        CssClass="textbox textbox1 btn2" AutoPostBack="true" OnClick="btn_printmaster_Click"
                                        Font-Bold="true" />
                                    <NEW:NEWPrintMater runat="server" ID="NEWPrintMater1" Visible="false" />
                                </center>
                            </div>
                            <div id="imgdiv33" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                left: 0px;">
                                <center>
                                    <div id="panel_description11" runat="server" visible="false" class="table" style="background-color: White;
                                        height: 120px; width: 467px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
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
                                                        Font-Size="Medium" CssClass="textbox btn1" OnClick="btndescpopadd_Click" />
                                                    <asp:Button ID="btn_exitdesc1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" CssClass="textbox btn1" OnClick="btndescpopexit_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </center>
                            </div>
                            <div id="Div2" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                left: 0px;">
                                <center>
                                    <div id="Div3" runat="server" class="table" style="background-color: White; height: 120px;
                                        width: 467px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                        border-radius: 10px;">
                                        <table>
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="Label6" runat="server" Text="Position" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:TextBox ID="Txtposition" runat="server" Width="400px" Style="font-family: 'Book Antiqua';
                                                        margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Button ID="Btnposition" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" CssClass="textbox btn1" OnClick="Btnposition_Click" />
                                                    <asp:Button ID="Btnpositionex" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" CssClass="textbox btn1" OnClick="Btnpositionex_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </center>
                            </div>
                         
                            <div id="Div6" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                left: 0px;">
                                <center>
                                    <div id="Div7" runat="server" class="table" style="background-color: White; height: 120px;
                                        width: 467px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                        border-radius: 10px;">
                                        <table>
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="Label9" runat="server" Text="Salary" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:TextBox ID="txtsal" runat="server" Width="400px" Style="font-family: 'Book Antiqua';
                                                        margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtsal"
                                                        FilterType="numbers" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Button ID="Button2" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" CssClass="textbox btn1" OnClick="btnsaladd" />
                                                    <asp:Button ID="Button3" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" CssClass="textbox btn1" OnClick="btnsaldex_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </center>
                            </div>
                           
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btn_Excel" />
                          <asp:PostBackTrigger ControlID="btndetailgo" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </center>
        <center>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel25">
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
            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel26">
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
</asp:Content>
