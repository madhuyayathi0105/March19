<%@ Page Title="" Language="C#" MasterPageFile="~/ScheduleMOD/ScheduleSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="workload.aspx.cs" Inherits="NewAttendance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
    <script language="javascript" type="text/javascript">
        function postBackByObject() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                __doPostBack("", "");
            }
        }

    </script>
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblnorec').innerHTML = "";

        }
    </script>
    <html>
    <style type="text/css">
        .style3
        {
            width: 310px;
        }
        .style4
        {
        }
        .style6
        {
            width: 160px;
        }
        .style7
        {
            width: 141px;
        }
        .style11
        {
            width: 346px;
            height: 28px;
        }
        .style12
        {
            height: 28px;
        }
        .style16
        {
            width: 344px;
        }
        .cursorptr
        {
            cursor: pointer;
        }
        .style18
        {
            width: 794px;
        }
        .style19
        {
            width: 370px;
        }
        .style20
        {
            width: 312px;
        }
        .newstyle
        {
            position: absolute;
            top: 297px;
            left: 367px;
            background-color: Silver;
            border: 1px;
        }
    </style>
    <body oncontextmenu="return false">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div style="height: 55px; margin-left: 0px;">
            <br />
            <center>
                <asp:Label ID="lblhead" runat="server" Text="Staff Workload" CssClass="fontstyleheader"
                    ForeColor="Green"></asp:Label></center>
            <br />
            <asp:Panel ID="pnl_filter" runat="server">
                <table>
                    <tr>
                        <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                            background-color: lightblue; border-width: 1px;">
                            <asp:Label ID="lblcollege" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                            border-width: 1px; border-right-style: solid;">
                            <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="21px" Width="187px" AutoPostBack="True" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                            background-color: lightblue; border-width: 1px;">
                            <asp:Label ID="lblreporttype" runat="server" Text="Workload" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                            border-width: 1px; border-right-style: solid;">
                            <asp:DropDownList ID="ddlreporttype" runat="server" Font-Bold="True" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlreporttype_SelectedIndexChanged" Font-Names="Book Antiqua"
                                Font-Size="Medium">
                                <asp:ListItem>Detailed</asp:ListItem>
                                <asp:ListItem>Scheduled</asp:ListItem>
                                <asp:ListItem>Individual Workload</asp:ListItem>
                                <asp:ListItem>Class Wise Workload</asp:ListItem>
                                <asp:ListItem>Class Wise Staff Workload</asp:ListItem>
                                <asp:ListItem>Staff Work Load With Experience</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                            background-color: lightblue; border-width: 1px;">
                            <asp:Label ID="lblFromdate" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                            border-width: 1px; border-right-style: solid;">
                            <asp:TextBox ID="txtFromDate" CssClass="txt" runat="server" Height="25px" Width="75px"
                                OnTextChanged="txtFromDate_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" AutoPostBack="True"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtFromDate_FilteredTextBoxExtender" FilterType="Custom,Numbers"
                                ValidChars="/" runat="server" TargetControlID="txtFromDate">
                            </asp:FilteredTextBoxExtender>
                            <asp:CalendarExtender ID="calfromdate" TargetControlID="txtFromDate" Format="d/MM/yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                        </td>
                        <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                            background-color: lightblue; border-width: 1px;">
                            <asp:Label ID="lbltodate" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                            border-width: 1px; border-right-style: solid;">
                            <asp:TextBox ID="txtToDate" CssClass="txt" runat="server" Height="25px" Width="75px"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtToDate_TextChanged"
                                AutoPostBack="True"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtToDate_FilteredTextBoxExtender" runat="server"
                                TargetControlID="txtToDate" FilterType="Custom,Numbers" ValidChars="/">
                            </asp:FilteredTextBoxExtender>
                            <asp:CalendarExtender ID="caltodate" TargetControlID="txtToDate" Format="d/MM/yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                        </td>
                        <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                            background-color: lightblue; border-width: 1px;">
                            <asp:Label ID="deptlbl" runat="server" Text="Department" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                            border-width: 1px; border-right-style: solid;">
                            <asp:DropDownList ID="deptddl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnSelectedIndexChanged="deptddl_SelectedIndexChanged" Height="21px"
                                Width="187px" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btngo_session" runat="server" Text="Go" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnClick="btngo_session_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div id="invisiblediv" runat="server">
                <table style="width: 860px;">
                    <tr>
                        <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                            background-color: lightblue; border-width: 1px;">
                            <asp:Label ID="desiglbl" runat="server" Text="Designation" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                            border-width: 1px; border-right-style: solid;">
                            <asp:DropDownList ID="desigddl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnSelectedIndexChanged="desigddl_SelectedIndexChanged" Height="23px"
                                Width="246px" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                            background-color: lightblue; border-width: 1px;">
                            <asp:Label ID="stafflbl" runat="server" Text="Staff Name" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                            border-width: 1px; border-right-style: solid;">
                            <asp:DropDownList ID="stafftxt" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="28px" Width="102px" AutoPostBack="True" OnSelectedIndexChanged="stafftxt_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                            background-color: lightblue; border-width: 1px;">
                            <asp:Label ID="sublbl" runat="server" Text="Subject" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                            border-width: 1px; border-right-style: solid;">
                            <asp:DropDownList ID="subjddl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="22px" Width="246px" AutoPostBack="True" OnSelectedIndexChanged="subjddl_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                            background-color: lightblue; border-width: 1px;">
                            <asp:Label ID="frmperlbl" runat="server" Text="Period Wise Selection" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                            border-width: 1px; border-right-style: solid;">
                            <asp:DropDownList ID="frmperddl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="27px" Width="50px" AutoPostBack="True" OnSelectedIndexChanged="frmperddl_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                            background-color: lightblue; border-width: 1px;">
                            <asp:Label ID="toperlbl" runat="server" Text="To" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                            border-width: 1px; border-right-style: solid;">
                            <asp:DropDownList ID="toperddl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="27px" Width="50px" AutoPostBack="True" OnSelectedIndexChanged="toperddl_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="diffperlbl" runat="server" Text="To period should be greater than from period"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Small" ForeColor="Red"></asp:Label>
                        </td>
                        <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                            background-color: lightblue; border-width: 1px;">
                            <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                            border-width: 1px; border-right-style: solid;">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <%--<asp:PlaceHolder ID="ph" runat="server"></asp:PlaceHolder>--%>
                                    <asp:TextBox ID="txt_sem" runat="server" ReadOnly="true" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="psem" runat="server" CssClass="MultipleSelectionDDL" Width="150px"
                                        Style="overflow-x: hidden; overflow-y: hidden;">
                                        <asp:CheckBox ID="chk_sem" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            OnCheckedChanged="chk_sem_ChekedChanged" Font-Size="Medium" Text="Select All"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="ddlsem" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlsem_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                            <asp:ListItem>1</asp:ListItem>
                                            <asp:ListItem>2</asp:ListItem>
                                            <asp:ListItem>3</asp:ListItem>
                                            <asp:ListItem>4</asp:ListItem>
                                            <asp:ListItem>5</asp:ListItem>
                                            <asp:ListItem>6</asp:ListItem>
                                            <asp:ListItem>7</asp:ListItem>
                                            <asp:ListItem>8</asp:ListItem>
                                            <asp:ListItem>9</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:DropDownExtender ID="DropDownExtender2" runat="server" DropDownControlID="psem"
                                        DynamicServicePath="" Enabled="true" TargetControlID="txt_sem">
                                    </asp:DropDownExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="colorlbl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                            border-width: 2px; border-left-style: solid; border-right-style: solid; width: 321px;">
                            <asp:DropDownList ID="optradio" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="optradio_SelectedIndexChanged">
                                <asp:ListItem Value="color">Color</asp:ListItem>
                                <asp:ListItem Value="subj">Subject</asp:ListItem>
                                <asp:ListItem Value="subclass">Subject and Class</asp:ListItem>
                            </asp:DropDownList>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_attndddl" runat="server" ReadOnly="true" Height="21px" Width="156px"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="float: left;
                                        margin-top: -25px; margin-left: 163px;">---Select---</asp:TextBox>
                                    <asp:Panel ID="pbatch" runat="server" CssClass="MultipleSelectionDDL" Style="overflow-x: hidden;
                                        overflow-y: hidden;">
                                        <asp:CheckBox ID="chk_attndddl" runat="server" Width="158px" Font-Bold="True" Font-Names="Book Antiqua"
                                            OnCheckedChanged="chk_attndddl_ChekedChanged" Font-Size="Medium" Text="Select All"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="attndddl" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            OnSelectedIndexChanged="attndddl_SelectedIndexChanged" Width="143px" Font-Bold="True"
                                            Font-Names="Book Antiqua">
                                            <asp:ListItem Value="NO" Text="No Attendance" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="P" Text="Present" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="A" Text="Absent" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="Per" Text="Permission" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="LA" Text="Late" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="OD" Text="OD" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="NA" Text="Not Joined" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="Free" Text="Free" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="Busy" Text="Busy" Selected="True"></asp:ListItem>
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pextendbatch" runat="server" TargetControlID="txt_attndddl"
                                        PopupControlID="pbatch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Button ID="btnGo" runat="server" Text="Go" Style="height: 26px; font-weight: 700"
                                OnClick="btnGo_Click" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="fmlbl" runat="server" Text="Select from date" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Small" ForeColor="Red"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="tolbl" runat="server" Text="Select to date" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Small" ForeColor="Red"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="difflbl" runat="server" Text="From date should be less than to date"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Small" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table style="top: 300px;">
                    <tr>
                        <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                            background-color: lightblue; border-width: 1px;">
                            <asp:Label ID="lblbatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                            background-color: lightblue; border-width: 1px;">
                            <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" AutoPostBack="true" Width="80px" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                            background-color: lightblue; border-width: 1px;">
                            <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                            background-color: lightblue; border-width: 1px;">
                            <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" AutoPostBack="true" Width="100px" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                            background-color: lightblue; border-width: 1px;">
                            <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                            background-color: lightblue; border-width: 1px;">
                            <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" AutoPostBack="true" Width="200px" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbcombine" runat="server" Text="Combine Hour Work Load" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" />
                        </td>
                        <td>
                            <asp:CheckBox ID="chkfreestaff" runat="server" Text="Free Staff Only" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" />
                        </td>
                    </tr>
                </table>
            </div>
            <table>
                <tr>
                    <td>
                        <%--<asp:Panel ID="Panel5" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style=" width: 1170px; height: 18px; margin-bottom: 0px;background-image: url('Menu/Top%20Band-2.jpg');">
                            <br />
                        </asp:Panel>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="errlbl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td class="style18">
                                    <asp:Panel ID="Panel3" runat="server" BorderStyle="None">
                                        <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                        &nbsp;&nbsp;
                                        <asp:Label ID="ddlpagelbl" runat="server" Font-Bold="True" Text="     Records Per Page"
                                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        <asp:DropDownList ID="DropDownListpage" runat="server" Height="25px" Width="65px"
                                            Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="pageddltxt" runat="server" Height="22px" Width="40px" Font-Bold="True"
                                            Font-Size="Medium" Font-Names="Book Antiqua" OnTextChanged="pageddltxt_TextChanged"
                                            AutoPostBack="True"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers"
                                            TargetControlID="pageddltxt">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Label ID="pgsearch_lbl" runat="server" Font-Bold="True" Text="Page Search:"
                                            Width="95px" Font-Names="Book Antiqua" Font-Size="Medium" Height="24px"></asp:Label>
                                        <asp:TextBox ID="pagesearch_txt" runat="server" AutoPostBack="True" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Height="22px" Width="40px" OnTextChanged="pagesearch_txt_TextChanged"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="pagesearch_txt"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Label ID="errmsg" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Height="22px" Width="307px"></asp:Label>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="colorpnl" runat="server">
                            <table id="colortbl">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="noattnd" runat="server" BackColor="LightSeaGreen" ForeColor="Maroon"
                                            Height="21px" Width="35px" Text=" NO" Font-Bold="True" Enabled="false"></asp:TextBox>
                                        <asp:Label ID="Label1" runat="server" Text="No Attendance"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="freehr" runat="server" BackColor="LightGray" Height="21px" Width="35px"
                                            Font-Bold="True" Enabled="false"></asp:TextBox>
                                        <asp:Label ID="Label2" runat="server" Text="Free"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Bc" runat="server" BackColor="DarkMagenta" ForeColor="Orange" Height="21px"
                                            Width="35px" Enabled="false" Font-Bold="True"></asp:TextBox>
                                        <asp:Label ID="Label10" runat="server" Text="Busy"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="p" runat="server" Text="  P" BackColor="Olive" ForeColor="Black"
                                            Height="21px" Width="35px" Font-Bold="True" Enabled="false"></asp:TextBox>
                                        <asp:Label ID="Label9" runat="server" Text="Present" Enabled="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="a" runat="server" Text="  A" BackColor="Red" ForeColor="White" Height="21px"
                                            Width="35px" Enabled="false" Font-Bold="True"></asp:TextBox>
                                        <asp:Label ID="Label3" runat="server" Text="Absent" Enabled="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="per" runat="server" Text="PER" BackColor="Wheat" ForeColor="Blue"
                                            Height="21px" Width="35px" Font-Bold="True" Enabled="false"></asp:TextBox>
                                        <asp:Label ID="Label4" runat="server" Text="Permission"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="la" runat="server" Text=" LA" Enabled="false" BackColor="LightBlue"
                                            ForeColor="Blue" Height="21px" Width="35px" Font-Bold="True"></asp:TextBox>
                                        <asp:Label ID="Label5" runat="server" Text="Late"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="od" runat="server" Text=" OD" Enabled="false" BackColor="DarkGoldenrod"
                                            ForeColor="White" Height="21px" Width="35px" Font-Bold="True"></asp:TextBox>
                                        <asp:Label ID="Label6" runat="server" Text="OD"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="na" runat="server" Text=" NA" Enabled="false" BackColor="MediumOrchid"
                                            ForeColor="Orange" Height="21px" Width="35px" Font-Bold="True"></asp:TextBox>
                                        <asp:Label ID="Label8" runat="server" Text="Not Joined"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <table>
                            <tr>
                                <td class="style20">
                                    <asp:TreeView ID="load_tree" onclick="javascript:postBackByObject()" runat="server"
                                        Font-Size="Medium" ForeColor="#996633" ShowCheckBoxes="Leaf" Font-Bold="True"
                                        ViewStateMode="Enabled" ShowExpandCollapse="true" OnSelectedNodeChanged="load_tree_SelectedNodeChanged"
                                        OnTreeNodeCheckChanged="load_tree_TreeNodeCheckChanged" OnTreeNodeExpanded="load_tree_TreeNodeExpanded">
                                        <HoverNodeStyle BackColor="#CCCC00" ForeColor="Red" BorderColor="#CCCC00" />
                                        <LeafNodeStyle ForeColor="#009933" />
                                        <NodeStyle ForeColor="#333300" />
                                        <ParentNodeStyle ForeColor="#CC3300" />
                                        <RootNodeStyle ForeColor="#993300" />
                                        <SelectedNodeStyle ForeColor="#CC3300" />
                                    </asp:TreeView>
                                </td>
                                <td>
                                    <asp:Button ID="okbtn" runat="server" Text="OK" OnClick="okbtn_Click" Height="28px"
                                        Font-Bold="true" Font-Size="Medium" Width="57px" />
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td class="style18">
                                    <asp:UpdatePanel ID="spread_update" runat="server">
                                        <ContentTemplate>
                                            <FarPoint:FpSpread ID="load_spread" runat="server" BorderWidth="1px" Height="50px"
                                                OnCellClick="load_spread_CellClick" CssClass="cursorptr" overflow="true" OnPreRender="load_spread_SelectedIndexChanged"
                                                OnUpdateCommand="load_spread_UpdateCommand">
                                                <CommandBar BackColor="Control" ButtonFaceColor="Control" Visible="true" ButtonHighlightColor="ControlLightLight"
                                                    ButtonShadowColor="ControlDark" ButtonType="PushButton">
                                                </CommandBar>
                                                <Sheets>
                                                    <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                            </FarPoint:FpSpread>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Report Name"></asp:Label>
                                    <asp:TextBox ID="txtexcelname" runat="server" onkeypress="display()" Height="20px"
                                        Width="180px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btnxl_Click" />
                                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblnorec" runat="server" ForeColor="Red" Visible="False" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chk_sms" runat="server" Text="SMS" Font-Bold="true" Font-Names="BookAntiqua" />
                                    <asp:CheckBox ID="chk_mail" runat="server" Text="Mail" Font-Bold="true" Font-Names="BookAntiqua" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txt_message" runat="server" TextMode="MultiLine" Height="200px"
                                        Width="500px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnsms" runat="server" Font-Bold="true" Font-Names="BookAntiqua"
                                        Text="Send" OnClick="btnsms_Click" />
                                </td>
                            </tr>
                        </table>
            </table>
    </body>
    </html>
</asp:Content>
