<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Enrollmentselection.aspx.cs" Inherits="Enrollmentselection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/Style.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .textbox
        {
            border: 1px solid #c4c4c4;
            height: 20px;
            width: 160px;
            font-size: 13px;
            text-transform: capitalize;
            padding: 4px 4px 4px 4px;
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            box-shadow: 0px 0px 8px #d9d9d9;
            -moz-box-shadow: 0px 0px 8px #d9d9d9;
            -webkit-box-shadow: 0px 0px 8px #d9d9d9;
        }
        .textbox1:hover
        {
            outline: none;
            border: 1px solid #7bc1f7;
            box-shadow: 0px 0px 8px #7bc1f7;
            -moz-box-shadow: 0px 0px 8px #7bc1f7;
            -webkit-box-shadow: 0px 0px 8px #7bc1f7;
        }
    </style>
    <style type="text/css">
        .modal_popup_background_color
        {
            background: rgba(0, 0, 0, 0.6);
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
        .search
        {
            background-image: url('images/sat.gif');
            background-repeat: no-repeat;
            width: 40px;
            height: 40px;
            border: 0;
        }
        .sakthi
        {
            width: 140px;
            height: 130px;
            background-color: Black;
        }
        
        .font11
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
        .modalPopup
        {
            background-color: #ffffdd;
            border-width: 1px;
            -moz-border-radius: 5px;
            border-style: solid;
            border-color: Gray;
            min-width: 300px;
            max-width: 1600px;
            min-height: 10px;
            max-height: 200px;
            top: 100px;
            left: 150px;
            width: 1200px;
        }
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }
        ody, input
        {
            font-family: Tahoma;
            font-size: 11px;
        }
        
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=80);
        }
        
        .accordion
        {
            width: 300px;
        }
        
        .accordionHeader
        {
            border: 1px solid #2F4F4F;
            color: white;
            background-color: #2E4d7B;
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
            height: 15px;
        }
        
        .accordionHeaderSelected
        {
            border: 1px solid #2F4F4F;
            color: white;
            background-color: #5078B3;
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
            height: 15px;
        }
        
        .accordionContent
        {
            background-color: White;
            border: 1px dashed #2F4F4F;
            border-top: none;
            padding: 5px;
            padding-top: 10px;
        }
        
        .autocomplete_highlightedListItem
        {
            background-color: #ffff99;
            color: black;
            padding: 1px;
            width: 241px;
        }
        .autocomplete_completionListElement
        {
            margin: 0px !important;
            background-color: White;
            color: windowtext;
            border: buttonshadow;
            border-width: 0px;
            border-style: solid;
            cursor: 'default';
            height: 100px;
            font-family: Book Antiqua;
            font-size: small;
            text-align: left;
            list-style-type: none;
            padding-left: 1px;
            width: 430px;
            overflow: auto;
            overflow-x: hidden;
        }
    </style>
    <style type="text/css">
        .ajax__myTab
        {
            text-align: center;
        }
        .ajax__myTab .ajax__tab_header
        {
            font-family: Book Antiqua;
            text-align: initial;
            font-size: 16px;
            font-weight: bold;
            color: White;
            border-left: solid 1px #666666;
            border-bottom: thin 1px #666666;
        }
        .ajax__myTab .ajax__tab_outer
        {
            border: 1px solid black;
            width: 220px;
            height: 35px;
            border-top: 3px solid transparent;
        }
        .ajax__myTab .ajax__tab_inner
        {
            padding-left: 4px;
            background-color: indigo;
            width: 275px;
            height: 35px;
        }
        
        .ajax__myTab .ajax__tab_tab
        {
            height: 22px;
            padding: 4px;
            margin: 0;
            text-align: center;
        }
        .ajax__myTab .ajax__tab_hover .ajax__tab_outer
        {
            border-top: 3px solid #00527D;
        }
        .ajax__myTab .ajax__tab_hover .ajax__tab_inner
        {
            background-color: #A1C344;
            color: White;
        }
        .ajax__myTab .ajax__tab_hover .ajax__tab_tab
        {
            background-color: #A1C344;
            cursor: pointer;
            color: White;
        }
        .ajax__myTab .ajax__tab_active .ajax__tab_outer
        {
            border-top: 2px solid white;
            border-bottom: transparent;
            color: #B0E0E6;
        }
        .ajax__myTab .ajax__tab_active .ajax__tab_inner
        {
            background-color: #F36200;
            border-bottom: transparent;
        }
        .ajax__myTab .ajax__tab_active .ajax__tab_tab
        {
            background-color: #F36200;
            cursor: inherit;
            width: 160px;
        }
        .ajax__myTab .ajax__tab_body
        {
            border: 1.5px solid #F36200;
            padding: 6px;
            background-color: #EFEBEF;
        }
        .ajax__myTab .ajax__tab_disabled
        {
            color: #F1F1F1;
        }
        .btnapprove1
        {
            background: transparent;
        }
        .btnapprove1:hover
        {
            background-color: Orange;
            color: White;
        }
        .fixedheader
        {
            position: absolute;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">

        function PrintDiv() {
            var panel = document.getElementById("<%=contentDiv.ClientID %>");
            var printWindow = window.open('', '', 'height=900,width=1000');
            printWindow.document.write('<html><head>');
            printWindow.document.write('<style> .classRegular { font-family:Arial; font-size:9px; } .classBold10 { font-family:Arial; font-size:11px; font-weight:bold;} .classBold12 { font-family:Arial; font-size:13px; font-weight:bold;} .classBold { font-family:Arial; font-size:9px; font-weight:bold;} </style>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                //<div id="footer" style="background-color:White;"></div>
                // <div id="header" style="background-color:White;"></div>
                //                document.getElementById('header').style.display = 'none';
                //                document.getElementById('footer').style.display = 'none';
                printWindow.print();
            }, 500);
            return false;
        }

        function display() {
            document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
        }
        function disp() {
            document.getElementById('<%=lblalert.ClientID %>').innerHTML = "";
        }

        function che(id) {
            var value = id.checked;
            var id_Value = document.getElementById("<%=txt_degree.ClientID %>");
            var second_id = document.getElementById("<%=txt_department.ClientID %>");
            if (value == true) {
                id_Value.disabled = false;
                second_id.disabled = false;
                return true;
            }
            else {
                id_Value.disabled = true;
                second_id.disabled = true;
                return true;
            }
        }
        function justclick(id) {
            var counter = 0;
            var value = id.checked;
            if (value == true) {
                var chk = document.getElementById("<%=cbldegree.ClientID %>");
                var checkbox = chk.getElementsByTagName("input");
                for (var i = 0; i < checkbox.length; i++) {
                    if (checkbox[i].checked) {
                        counter++;
                    }
                }
                alert(counter);
            }
            else {

            }
        }

        function checkDate() {
            var fromDate = "";
            var toDate = "";
            var date = ""
            var date1 = ""
            var month = "";
            var month1 = "";
            var year = "";
            var year1 = "";
            var empty = "";
            fromDate = document.getElementById('<%=txt_fromdate.ClientID%>').value;
            toDate = document.getElementById('<%=txt_todate.ClientID%>').value;
            var fdate = document.getElementById('<%=txt_fromdate.ClientID%>');
            var tdate = document.getElementById('<%=txt_todate.ClientID%>');

            date = fromDate.substring(0, 2);
            month = fromDate.substring(3, 5);
            year = fromDate.substring(6, 10);

            date1 = toDate.substring(0, 2);
            month1 = toDate.substring(3, 5);
            year1 = toDate.substring(6, 10);
            var today = new Date();
            var currentDate = today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();

            if (year == year1) {
                if (month == month1) {
                    if (date == date1) {
                        empty = "";
                    }
                    else if (date < date1) {
                        empty = "";
                    }
                    else {
                        empty = "e";
                    }
                }
                else if (month < month1) {
                    empty = "";
                }
                else if (month > month1) {
                    empty = "e";
                }
            }
            else if (year < year1) {
                empty = "";
            }
            else if (year > year1) {
                empty = "e";
            }
            if (empty != "") {
                fdate.value = currentDate;
                tdate.value = currentDate;
                alert("To date should be greater than from date ");
                return false;
            }
        }

        function checkDateADD() {
            var fromDate = "";
            var toDate = "";
            var date = ""
            var date1 = ""
            var month = "";
            var month1 = "";
            var year = "";
            var year1 = "";
            var empty = "";
            fromDate = document.getElementById('<%=txt_startdate.ClientID%>').value;
            toDate = document.getElementById('<%=txt_enddate.ClientID%>').value;
            var fdate = document.getElementById('<%=txt_startdate.ClientID%>');
            var tdate = document.getElementById('<%=txt_enddate.ClientID%>');

            date = fromDate.substring(0, 2);
            month = fromDate.substring(3, 5);
            year = fromDate.substring(6, 10);

            date1 = toDate.substring(0, 2);
            month1 = toDate.substring(3, 5);
            year1 = toDate.substring(6, 10);
            var today = new Date();
            var currentDate = today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();

            if (year == year1) {
                if (month == month1) {
                    if (date == date1) {
                        empty = "";
                    }
                    else if (date < date1) {
                        empty = "";
                    }
                    else {
                        empty = "e";
                    }
                }
                else if (month < month1) {
                    empty = "";
                }
                else if (month > month1) {
                    empty = "e";
                }
            }
            else if (year < year1) {
                empty = "";
            }
            else if (year > year1) {
                empty = "e";
            }
            if (empty != "") {
                fdate.value = currentDate;
                tdate.value = currentDate;
                alert("To date should be greater than from date ");
                return false;
            }
        }
    </script>
    <div style="height: auto; width: 1000px; margin-left: auto; margin-right: auto;">
        <center>
            <div style="width: 95%; height: 20px; text-align: right;">
                <center>
                    <span style="font-weight: bold; font-size: large; color: Green;">Enrollment Selection</span>
                </center>
            </div>
            <br />
            <center>
                <div style="width: 1000px; margin-left: 0px; margin-top: 0 px;">
                    <center>
                        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                        </asp:ToolkitScriptManager>
                        <asp:Panel ID="tabpnl" runat="server" Style="height: 500px; width: 100%; border-radius: 6px;"
                            CssClass="cpBody">
                            <asp:TabContainer ID="TabContainer1" runat="server" Style="height: 600px; width: 1000px;"
                                BackColor="Lavender" CssClass="ajax__myTab" Visible="true" AutoPostBack="true"
                                OnActiveTabChanged="TabContainer1_Changed">
                                <asp:TabPanel ID="tabpnlsel" runat="server" Visible="false" CssClass="ajax__myTab1"
                                    HeaderText="Selection" TabIndex="0" Style="font-family: Book Antiqua; font-size: medium;">
                                    <ContentTemplate>
                                    </ContentTemplate>
                                </asp:TabPanel>
                                <asp:TabPanel ID="tabpnlconfm" runat="server" CssClass="ajax__myTab1" Visible="false"
                                    HeaderText="Confirmation" TabIndex="1" Style="font-family: Book Antiqua; font-size: medium;">
                                    <ContentTemplate>                                       
                                        <table style="top: 300px; left: 15px; position: absolute; width: 950px;">
                                            <tr>
                                            </tr>
                                            <tr>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <td>
                                                        <div>
                                                            <center>
                                                                <FarPoint:FpSpread ID="fpconfrm" runat="server" BorderColor="Black" BorderStyle="Solid"
                                                                    OnButtonCommand="fpconfrm_Command" BorderWidth="1px" Visible="false" VerticalScrollBarPolicy="Always"
                                                                    HorizontalScrollBarPolicy="Always">
                                                                    <Sheets>
                                                                        <FarPoint:SheetView SheetName="Sheet1">
                                                                        </FarPoint:SheetView>
                                                                    </Sheets>
                                                                </FarPoint:FpSpread>
                                                            </center>
                                                        </div>
                                                    </td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtconfirmdt" runat="server" Visible="false" CssClass="textbox textbox1 txtheight2"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="100px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" TargetControlID="txtconfirmdt" />
                                                    <asp:Button ID="btnConfrmsave" runat="server" Visible="false" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Text="Confirmation" OnClick="btnConfrmsave_Click" Height="32px"
                                                        Style="margin-top: 10px;" CssClass="textbox textbox1" Width="119px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <center>
                                                        <div id="printconffm" runat="server" visible="false">
                                                            <asp:Label ID="lblalert" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                ForeColor="Red" Text="" Visible="false"></asp:Label>
                                                            <asp:Label ID="lblreportname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                Text="Report Name"></asp:Label>
                                                            <asp:TextBox ID="txtreport" runat="server" Width="180px" onkeypress=" return disp()"
                                                                CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                                                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                                                InvalidChars="/\">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:Button ID="btnexlconfrm" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                OnClick="btnexlconfrm_Click" Text="Export To Excel" Width="127px" Height="32px"
                                                                CssClass="textbox textbox1" />
                                                            <asp:Button ID="btnprintconfrm" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                Text="Print" OnClick="btnprintconfrm_Click" Height="32px" Style="margin-top: 10px;"
                                                                CssClass="textbox textbox1" Width="60px" />
                                                            <Insproplus:printmaster runat="server" ID="printconfm" Visible="false" />
                                                        </div>
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:TabPanel>
                                <asp:TabPanel ID="tabpnlsett" runat="server" CssClass="ajax__myTab1" Visible="false"
                                    HeaderText=" Settings" TabIndex="2" Style="font-family: Book Antiqua; font-size: medium;">
                                    <ContentTemplate>
                                    </ContentTemplate>
                                </asp:TabPanel>
                            </asp:TabContainer>
                            <center>
                                <%--selection and confirmation--%>
                                <center>
                                    <div id="divsel" runat="server" visible="false" style="margin-top: -554px; position: absolute;">
                                        <center>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <div style="height: 100px; width: 100%; border: 1px solid lightblue; -webkit-border-radius: 10px;
                                                            -moz-border-radius: 10px; border-radius: 10px; padding: 10px; margin: 0 auto;">
                                                            <table id="Maintable" runat="server" width="950px">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lbl_collegename" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddl_collegename" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium" OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged"
                                                                            Height="29px" Width="202px" AutoPostBack="true">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:Label ID="lblStr" Text="Stream" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                            runat="server"></asp:Label>
                                                                        <asp:DropDownList ID="ddltype" runat="server" Width="132px" Height="30px" Enabled="false"
                                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="type_Change"
                                                                            AutoPostBack="true">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold;">Batch:</span>
                                                                        <asp:Label ID="lblbatch" runat="server" Text="" Style="font-family: Book Antiqua;
                                                                            font-size: medium; font-weight: bold;"></asp:Label>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold;">Education
                                                                            Level</span>
                                                                        <asp:DropDownList ID="ddledulevel" runat="server" Width="70px" Height="30px" Font-Bold="True"
                                                                            Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="edulevel_SelectedIndexChange">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblDeg" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                            runat="server"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:UpdatePanel ID="UpdatePanel_Department" runat="server">
                                                                            <ContentTemplate>
                                                                                <asp:TextBox ID="txt_degree" runat="server" ReadOnly="true" Width="190px" Font-Bold="True"
                                                                                    Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox textbox1 txtheight">---Select---</asp:TextBox>
                                                                                <asp:Panel ID="paneldegree" runat="server" Height="300px" Width="112px" CssClass="multxtpanel">
                                                                                    <asp:CheckBox ID="cbdegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                                        Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="cbdegree_Changed" />
                                                                                    <asp:CheckBoxList ID="cbldegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="cbldegree_SelectedIndexChanged">
                                                                                    </asp:CheckBoxList>
                                                                                </asp:Panel>
                                                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_degree"
                                                                                    PopupControlID="paneldegree" Position="Bottom">
                                                                                </asp:PopupControlExtender>
                                                                            </ContentTemplate>
                                                                            <%-- <Triggers>
                                                                                <asp:AsyncPostBackTrigger ControlID="cbldegree" />
                                                                            </Triggers>--%>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblBran" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:UpdatePanel ID="updept" runat="server">
                                                                            <ContentTemplate>
                                                                                <asp:TextBox ID="txt_department" runat="server" ReadOnly="true" Width="136px" Font-Bold="True"
                                                                                    Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox textbox1 txtheight">---Select---</asp:TextBox>
                                                                                <asp:Panel ID="paneldepartment" runat="server" Height="300px" Width="225px" CssClass="multxtpanel">
                                                                                    <asp:CheckBox ID="cbdepartment1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                                        Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="cbdepartment_Changed" />
                                                                                    <asp:CheckBoxList ID="cbldepartment" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="cbldepartment_SelectedIndexChanged">
                                                                                    </asp:CheckBoxList>
                                                                                </asp:Panel>
                                                                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_department"
                                                                                    PopupControlID="paneldepartment" Position="Bottom">
                                                                                </asp:PopupControlExtender>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:RadioButton ID="cbapply" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium" Width="133px" Text="Non Resident " GroupName="same" />
                                                                        <asp:RadioButton ID="cbnotapply" runat="server" Text="Resident " Font-Bold="True"
                                                                            Font-Names="Book Antiqua" Font-Size="Medium" Width="97px" GroupName="same" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btn_go" runat="server" Text="Go" Visible="false" OnClick="btn_go_Click"
                                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Height="30px" Width="50px"
                                                                            CssClass="textbox textbox1" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="7">
                                                                        <table id="tblconfrm" runat="server" visible="false">
                                                                            <tr>
                                                                                <td>
                                                                                    <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold;">From</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtfrconfrm" runat="server" onchange="return checkDateADD()" CssClass="textbox textbox1 txtheight2"
                                                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="100px"></asp:TextBox>
                                                                                    <asp:CalendarExtender ID="CalendarExtender11" runat="server" Format="dd/MM/yyyy"
                                                                                        TargetControlID="txtfrconfrm" />
                                                                                </td>
                                                                                <td>
                                                                                    <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold;">To</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txttoconfrm" runat="server" onchange="return checkDateADD()" CssClass="textbox textbox1 txtheight2"
                                                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="100px"></asp:TextBox>
                                                                                    <asp:CalendarExtender ID="CalendarExtender22" runat="server" Format="dd/MM/yyyy"
                                                                                        TargetControlID="txttoconfrm" />
                                                                                </td>
                                                                                <td>
                                                                                    <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold;">Enrollmnet</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="ddlenrollconfm" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                                        Font-Size="Medium" Height="29px" Width="190px" AutoPostBack="true" OnSelectedIndexChanged="ddlenrollconfm_OnSelectedIndexChanged">
                                                                                        <asp:ListItem Value="0" Text="Enrolled" Selected="True">Enrolled</asp:ListItem>
                                                                                        <asp:ListItem Value="1" Text="Enrolled Confirm">Enrolled Confirm</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="btnenrollconfrm" runat="server" Text="Go" OnClick="btnenrollconfrm_Click"
                                                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Height="30px" Width="50px"
                                                                                        CssClass="textbox textbox1" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <asp:Label ID="errorlable" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div style="height: auto; width: 95%;">
                                                            <center>
                                                                <FarPoint:FpSpread ID="fpspread" runat="server" BorderColor="Black" BorderStyle="Solid"
                                                                    OnButtonCommand="fpspread_Command" BorderWidth="1px" Visible="true" VerticalScrollBarPolicy="Always"
                                                                    HorizontalScrollBarPolicy="Always">
                                                                    <Sheets>
                                                                        <FarPoint:SheetView SheetName="Sheet1">
                                                                        </FarPoint:SheetView>
                                                                    </Sheets>
                                                                </FarPoint:FpSpread>
                                                            </center>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div>
                                                            <asp:CheckBox ID="cbx" runat="server" Text="Select All" AutoPostBack="true" Visible="false"
                                                                OnCheckedChanged="cbx_Changed" />
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:Button ID="btnexcel" runat="server" Text="Export Excel" Visible="false" OnClick="btn_excelClcik"
                                                                Height="30px" Width="90px" CssClass="textbox textbox1" />
                                                            <asp:Button ID="btn_pdf" runat="server" Text="Print " Visible="false" OnClick="pdf_Click"
                                                                Height="30px" Width="90px" CssClass="textbox textbox1" />
                                                            <asp:Button ID="btnenrolment" runat="server" Text="Generate Enrollment Card" Visible="false"
                                                                OnClick="btnenrolment_Click" Height="30px" Width="165px" CssClass="textbox textbox1" />
                                                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </div>
                                </center>
                                <%--Enrollment Setting--%>
                                <div id="subdivbase" runat="server" visible="false" style="margin-top: -554px; position: absolute;">
                                    <center>
                                        <table>
                                            <tr>
                                                <td>
                                                    <div id="divenrollsett" runat="server" style="height: auto; width: 100%; border: 1px solid lightblue;
                                                        -webkit-border-radius: 10px; -moz-border-radius: 10px; border-radius: 10px; padding: 10px;
                                                        margin: 0 auto;">
                                                        <table>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <div id="divdatewise" runat="server">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lbl_fromdate" runat="server" Text="From" Style="font-family: Book Antiqua;
                                                                                        font-size: medium; font-weight: bold;"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txt_fromdate" runat="server" onchange="return checkDate()" CssClass="textbox textbox1 txtheight2"
                                                                                        Style="height: 20px; width: 75px; font-family: Book Antiqua; font-size: medium;
                                                                                        font-weight: bold;"></asp:TextBox>
                                                                                    <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="txt_fromdate" runat="server"
                                                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                                    </asp:CalendarExtender>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lbl_todate" runat="server" Text="To" Style="font-family: Book Antiqua;
                                                                                        font-size: medium; font-weight: bold;"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txt_todate" runat="server" onchange="return checkDate()" CssClass="textbox textbox1 txtheight2"
                                                                                        Style="height: 20px; width: 75px; font-family: Book Antiqua; font-size: medium;
                                                                                        font-weight: bold;"></asp:TextBox>
                                                                                    <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txt_todate" runat="server"
                                                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                                    </asp:CalendarExtender>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:RadioButton ID="rbbaseresid" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                                        Font-Size="Medium" Width="133px" Text="Non Resident " GroupName="red" AutoPostBack="true"
                                                                                        OnCheckedChanged="rbbaseresid_Changed" />
                                                                                    <asp:RadioButton ID="rbbasenotresid" runat="server" Text="Resident " Font-Bold="True"
                                                                                        Font-Names="Book Antiqua" Font-Size="Medium" Width="97px" GroupName="red" AutoPostBack="true"
                                                                                        OnCheckedChanged="rbbasenotresid_Changed" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnEnrollset" runat="server" Text="Go" Style="font-family: Book Antiqua;
                                                                        font-size: medium; font-weight: bold; width: 70px;" OnClick="btnEnrollset_Click" />
                                                                    <asp:Button ID="btnEnrollsetadd" runat="server" Text="ADD" Style="font-family: Book Antiqua;
                                                                        font-size: medium; font-weight: bold; width: 80px;" OnClick="btnEnrollsetadd_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <br />
                                                    <br />
                                                    <FarPoint:FpSpread ID="FpEnrollGo" runat="server" Width="900px" BorderColor="Black"
                                                        BorderStyle="Solid" BorderWidth="1px" AutoPostBack="true" Style="top: 260px;"
                                                        Visible="false" HierBar-ShowParentRow="False" OnCellClick="FpEnrollGo_OnCellClick"
                                                        OnPreRender="FpEnrollGo_Selectedindexchanged">
                                                        <Sheets>
                                                            <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                                                            </FarPoint:SheetView>
                                                        </Sheets>
                                                    </FarPoint:FpSpread>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <center>
                                                        <div id="print" runat="server" visible="false">
                                                            <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                ForeColor="Red" Text="" Visible="false"></asp:Label>
                                                            <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                Text="Report Name"></asp:Label>
                                                            <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress=" return display()"
                                                                CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                                                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                                                InvalidChars="/\">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:Button ID="Button1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                OnClick="btnExcel_Click" Text="Export To Excel" Width="127px" Height="32px" CssClass="textbox textbox1" />
                                                            <asp:Button ID="btnprintmasterhed" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                Text="Print" OnClick="btnprintmaster_Click" Height="32px" Style="margin-top: 10px;"
                                                                CssClass="textbox textbox1" Width="60px" />
                                                            <asp:Button ID="btnExport" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                Text="Export Print" OnClick="btnExport_Click" Height="32px" Style="margin-top: 10px;"
                                                                CssClass="textbox textbox1" Width="120px" />
                                                            <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                                                        </div>
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </div>
                        </asp:Panel>
                    </center>
                </div>
            </center>
            <center>
                <div id="subdiv" runat="server" visible="false" style="margin-left: 20px; height: 300px;
                    width: 900px; z-index: 50em;" class="popupstyle popupheight1 ">
                    <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: -4px; margin-left: 480px;"
                        OnClick="imagebtnpopclose_Click" />
                    <div style="background-color: White; height: 550px; width: 950px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <div>
                                <span class="fontstyleheader" style="color: Green;">Add Details</span></div>
                        </center>
                        <br />
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <table width="880px">
                                            <tr>
                                                <td>
                                                    <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold;">Start
                                                        Date</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_startdate" runat="server" onchange="return checkDateADD()" CssClass="textbox textbox1 txtheight2"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="100px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" TargetControlID="txt_startdate" />
                                                </td>
                                                <td>
                                                    <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold;">End Date</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_enddate" runat="server" onchange="return checkDateADD()" CssClass="textbox textbox1 txtheight2"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="100px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender7" runat="server" Format="dd/MM/yyyy" TargetControlID="txt_enddate" />
                                                </td>
                                                <td>
                                                    <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold;">Number
                                                        of Students per Session</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_Noofseat" runat="server" MaxLength="3" Width="80px" CssClass="textbox textbox1 txtheight2"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" TargetControlID="txt_Noofseat"
                                                        FilterType="Numbers" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold;">Number
                                                        of Session</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_noofsession" runat="server" Width="80px" AutoPostBack="true"
                                                        MaxLength="2" OnTextChanged="txt_sessionchange" CssClass="textbox textbox1 txtheight2"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_noofsession"
                                                        FilterType="Numbers" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                                <td colspan="2">
                                                    <asp:RadioButton ID="rbresid" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Width="133px" Text="Non Resident " GroupName="add" AutoPostBack="true"
                                                        OnCheckedChanged="rbresid_Changed" />
                                                    <asp:RadioButton ID="rbnotresid" runat="server" Text="Resident " Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" Width="97px" GroupName="add" AutoPostBack="true"
                                                        OnCheckedChanged="rbnotresid_Changed" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnsave" runat="server" Text="Save" Style="font-family: Book Antiqua;
                                                        font-size: medium; font-weight: bold;" OnClick="btnsave_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <div style="height: 350px; overflow: auto;">
                                                <asp:GridView ID="Sessiongrid" runat="server" Visible="false" AutoGenerateColumns="false"
                                                    HeaderStyle-BackColor="YellowGreen">
                                                    <%--HeaderStyle-CssClass="fixedheader"--%>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-Width="50px" HeaderStyle-Height="10px"
                                                            ItemStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="snolbl" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Start Time" HeaderStyle-Width="124px" HeaderStyle-Height="10px"
                                                            ItemStyle-Width="124px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txt_starttime" Text='<%# Eval("statr") %>' runat="server"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="End Time" HeaderStyle-Width="124px" HeaderStyle-Height="10px"
                                                            ItemStyle-Width="124px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txt_endtime" Text='<%# Eval("end") %>' runat="server"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </div>
            </center>
        </center>
    </div>
    <center>
        <div id="poperrjs" runat="server" visible="false" style="height: 100em; z-index: 10000;
            width: 100%; position: absolute; top: 0; left: 0;">
            <center>
                <div style="background-color: #FFFFFF; height: 200px; margin-top: 180px; width: 300px;
                    border: 1px solid rgb(0, 128, 128); border-radius: 5px; border-top-width: 5px;">
                    <br />
                    <br />
                    <span id="errorspan" runat="server"></span>
                    <br />
                    <br />
                    <br />
                    <br />
                    <asp:Button ID="popupbtn" runat="server" Style="background-color: rgb(0, 128, 128);
                        border: 0px; color: White;" Text="Okay" OnClick="btnpopup_clcik" />
                    <%-- <button style="background-color: rgb(0, 128, 128); border: 0px; color: White;">
                    Okay</button>--%>
                </div>
            </center>
        </div>
    </center>
    <center>
        <div style="height: 1px; width: 1px; overflow: auto;">
            <div id="contentDiv" runat="server" style="height: 710px; width: 1344px;" visible="false">
            </div>
    </center>
</asp:Content>
