<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="DailyPayment_ReportChart.aspx.cs" Inherits="DailyPayment_ReportChart" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
            function accType() {
                var acc = document.getElementById('<%=ddlacctype.ClientID%>').value;
                if (acc == 0) {
                    alert('Please Select Any one Account Type');
                    return false;
                }

                var cbheader = document.getElementById('<%=chk_studhed.ClientID %>');
                var cblheader = document.getElementById('<%=chkl_studhed.ClientID%>');
                var headertag = cblheader.getElementsByTagName("input");
                var empty = "";
                if (cbheader.checked == false) {
                    for (var i = 0; i < headertag.length; i++) {
                        if (headertag[i].checked == true)
                            empty = "#";
                    }
                    if (empty == "") {
                        alert("Please Select Any One Header");
                        return false;
                    }
                }

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
                    alert("To date should be greater than from date ");
                    document.getElementById('<%=txt_todate.ClientID %>').value = currentDate;
                    return false;
                }
            }
       
        </script>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Daily Payment Report Chart</span></div>
            </center>
        </div>
        <div>
            <center>
                <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <center>
                                        <div>
                                            <table id="maintable" runat="server" class="maintablestyle">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_collegename" Text="College" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                            OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td id="tdstr" runat="server" visible="false">
                                                        <asp:Label ID="lbl_str1" runat="server" Text="Stream"></asp:Label>
                                                    </td>
                                                    <td id="tdddlstr" runat="server" visible="false">
                                                        <asp:DropDownList ID="ddlstream" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlstream_OnSelectedIndexChanged"
                                                            CssClass="textbox  ddlheight" Style="width: 108px;">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td id="tdbatch" runat="server" visible="false">
                                                        Batch
                                                    </td>
                                                    <td id="tdcblbatch" runat="server" visible="false">
                                                        <asp:UpdatePanel ID="UP_batch" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_batch" runat="server" CssClass="textbox textbox1 txtheight1"
                                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel" Style="height: 200px;">
                                                                    <asp:CheckBox ID="cb_batch" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                        OnCheckedChanged="cb_batch_OnCheckedChanged" />
                                                                    <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_OnSelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="pce_batch" runat="server" TargetControlID="txt_batch"
                                                                    PopupControlID="panel_batch" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td id="tddegree" runat="server" visible="false">
                                                        <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                                                    </td>
                                                    <td id="tdcbldegree" runat="server" visible="false">
                                                        <asp:UpdatePanel ID="UP_degree" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox textbox1 txtheight1"
                                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="panel_degree" runat="server" CssClass="multxtpanel" Style="height: 200px;">
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
                                                    <%--staff menu--%>
                                                    <td id="tdstaffdept" runat="server" visible="false">
                                                        Department
                                                    </td>
                                                    <td id="tdcblstaffdept" runat="server" visible="false">
                                                        <asp:UpdatePanel ID="up_staffdept" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_staffdept" runat="server" CssClass="textbox textbox1 txtheight1"
                                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="pnl_staffdept" runat="server" CssClass="multxtpanel" Style="height: 200px;">
                                                                    <asp:CheckBox ID="cb_staffdept" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                        OnCheckedChanged="cb_staffdept_OnCheckedChanged" />
                                                                    <asp:CheckBoxList ID="cbl_staffdept" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_staffdept_OnSelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="pup_staffdept" runat="server" TargetControlID="txt_staffdept"
                                                                    PopupControlID="pnl_staffdept" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td id="tdstaffdesg" runat="server" visible="false">
                                                        Designation
                                                    </td>
                                                    <td id="tdcblstaffdesg" runat="server" visible="false">
                                                        <asp:UpdatePanel ID="up_staffdesg" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_staffdesg" runat="server" CssClass="textbox textbox1 txtheight1"
                                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="pnl_staffdesg" runat="server" CssClass="multxtpanel" Style="height: 200px;">
                                                                    <asp:CheckBox ID="cb_staffdesg" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                        OnCheckedChanged="cb_staffdesg_OnCheckedChanged" />
                                                                    <asp:CheckBoxList ID="cbl_staffdesg" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_staffdesg_OnSelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_staffdesg"
                                                                    PopupControlID="pnl_staffdesg" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td id="tdstafftype" runat="server" visible="false">
                                                        Staff Type
                                                    </td>
                                                    <td id="tdcblstafftype" runat="server" visible="false">
                                                        <asp:UpdatePanel ID="up_stafftype" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_stafftype" runat="server" CssClass="textbox textbox1 txtheight1"
                                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="pnl_stafftype" runat="server" CssClass="multxtpanel" Style="height: 200px;">
                                                                    <asp:CheckBox ID="cb_stafftype" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                        OnCheckedChanged="cb_stafftype_OnCheckedChanged" />
                                                                    <asp:CheckBoxList ID="cbl_stafftype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_stafftype_OnSelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_stafftype"
                                                                    PopupControlID="pnl_stafftype" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <%--vendor menu--%>
                                                    <td id="tdvendorcode" runat="server" visible="false">
                                                        Vendor Code
                                                    </td>
                                                    <td id="tdcblvendorcode" runat="server" visible="false">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_vendorcode" runat="server" CssClass="textbox textbox1 txtheight1"
                                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="pnl_vendorcode" runat="server" CssClass="multxtpanel" Style="height: 200px;">
                                                                    <asp:CheckBox ID="cb_vendorcode" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                        OnCheckedChanged="cb_vendorcode_OnCheckedChanged" />
                                                                    <asp:CheckBoxList ID="cbl_vendorcode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_vendorcode_OnSelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_vendorcode"
                                                                    PopupControlID="pnl_vendorcode" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td id="tdvendorname" runat="server" visible="false">
                                                        Company Name
                                                    </td>
                                                    <td id="tdcblvendorname" runat="server" visible="false">
                                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_vendorname" runat="server" CssClass="textbox textbox1 txtheight1"
                                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="pnl_vendorname" runat="server" CssClass="multxtpanel" Style="height: 200px;">
                                                                    <asp:CheckBox ID="cb_vendorname" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                        OnCheckedChanged="cb_vendorname_OnCheckedChanged" />
                                                                    <asp:CheckBoxList ID="cbl_vendorname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_vendorname_OnSelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_vendorname"
                                                                    PopupControlID="pnl_vendorname" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td id="tdvendorcont" runat="server" visible="false">
                                                        Vendor Contact Name
                                                    </td>
                                                    <td id="tdcblvendorcont" runat="server" visible="false" colspan="2">
                                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_vendorcont" runat="server" CssClass="textbox textbox1 txtheight1"
                                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="pnlvendorcont" runat="server" CssClass="multxtpanel" Style="height: 200px;">
                                                                    <asp:CheckBox ID="cb_vendorcont" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                        OnCheckedChanged="cb_vendorcont_OnCheckedChanged" />
                                                                    <asp:CheckBoxList ID="cbl_vendorcont" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_vendorcont_OnSelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txt_vendorcont"
                                                                    PopupControlID="pnlvendorcont" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td id="tddept" runat="server" visible="false">
                                                        <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                                                    </td>
                                                    <td id="tdcbldept" runat="server" visible="false">
                                                        <asp:UpdatePanel ID="Up_dept" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox textbox1 txtheight1"
                                                                    Width="125px" ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel" Style="height: 150px;">
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
                                                    <td id="tdsem" runat="server" visible="false">
                                                        <asp:Label ID="lblsem" runat="server" Text="Semester"></asp:Label>
                                                    </td>
                                                    <td id="tdcblsem" runat="server" visible="false">
                                                        <asp:UpdatePanel ID="Updp_sem" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox textbox1 txtheight1" ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="panel_sem" runat="server" CssClass="multxtpanel" Style="height: 150px;">
                                                                    <asp:CheckBox ID="cb_sem" runat="server" Width="124px" Text="Select All" AutoPostBack="True"
                                                                        OnCheckedChanged="cb_sem_OnCheckedChanged" />
                                                                    <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_OnSelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_sem"
                                                                    PopupControlID="panel_sem" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td id="tdsec" runat="server" visible="false">
                                                        Section
                                                    </td>
                                                    <td id="tdcblsec" runat="server" visible="false">
                                                        <asp:UpdatePanel ID="Updp_sect" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_sect" runat="server" CssClass="textbox textbox1 txtheight1"
                                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="panel_sect" runat="server" CssClass="multxtpanel" Style="height: 150px;">
                                                                    <asp:CheckBox ID="cb_sect" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                        OnCheckedChanged="cb_sect_OnCheckedChanged" />
                                                                    <asp:CheckBoxList ID="cbl_sect" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sect_OnSelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_sect"
                                                                    PopupControlID="panel_sect" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblheadorled" runat="server" Text="Account Type" Style="width: 50px;"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlacctype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlacctype_OnSelectedIndexChanged"
                                                            CssClass="textbox ddlstyle ddlheight3" Style="width: 111px;">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblhead" runat="server" Text="Header"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_studhed" runat="server" CssClass="textbox textbox1 txtheight1"
                                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="pnl_studhed" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                                    Style="height: 150px;">
                                                                    <asp:CheckBox ID="chk_studhed" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                        OnCheckedChanged="chk_studhed_OnCheckedChanged" />
                                                                    <asp:CheckBoxList ID="chkl_studhed" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_studhed_OnSelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_studhed"
                                                                    PopupControlID="pnl_studhed" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblledg" runat="server" Text=" Ledger"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_studled" runat="server" Style="height: 20px; width: 100px;"
                                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="pnl_studled" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                                    Style="width: 126px; height: 120px;">
                                                                    <asp:CheckBox ID="chk_studled" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                        OnCheckedChanged="chk_studled_OnCheckedChanged" />
                                                                    <asp:CheckBoxList ID="chkl_studled" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_studled_OnSelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_studled"
                                                                    PopupControlID="pnl_studled" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%-- <asp:Label runat="server" ID="Label3" Text="Finance Year"></asp:Label>--%>
                                                        Finance Year
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtfyear" Style="height: 20px; width: 100px;" CssClass="textbox textbox1 txtheight1"
                                                                    runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                                                <asp:Panel ID="Pfyear" runat="server" CssClass="multxtpanel" Height="100px">
                                                                    <asp:CheckBox ID="chkfyear" runat="server" Text="Select All" OnCheckedChanged="chkfyear_changed"
                                                                        AutoPostBack="True" />
                                                                    <asp:CheckBoxList ID="chklsfyear" runat="server" OnSelectedIndexChanged="chklsfyear_selected"
                                                                        AutoPostBack="True">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtfyear"
                                                                    PopupControlID="Pfyear" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_fromdate" runat="server" Text="From"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_todate" runat="server" Text="To"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5">
                                                        <asp:RadioButton ID="rbstud" runat="server" Text="Student" AutoPostBack="true" GroupName="s1"
                                                            OnCheckedChanged="rbstud_OnCheckedChanged" />
                                                        <asp:RadioButton ID="rbstaff" runat="server" Text="Staff" AutoPostBack="true" GroupName="s1"
                                                            OnCheckedChanged="rbstaff_OnCheckedChanged" />
                                                        <asp:RadioButton ID="rbvendor" runat="server" Text="Vendor" AutoPostBack="true" GroupName="s1"
                                                            OnCheckedChanged="rbvendor_OnCheckedChanged" />
                                                        <asp:RadioButton ID="rbother" runat="server" Text="Others" AutoPostBack="true" GroupName="s1"
                                                            OnCheckedChanged="rbother_OnCheckedChanged" />
                                                        <asp:CheckBox ID="chkcumul" runat="server" Text="Cumulative" Visible="false" AutoPostBack="true"
                                                            OnCheckedChanged="chkcumul_OnCheckedChanged" />
                                                        <asp:CheckBox ID="cbldetail" runat="server" Text="Detail" OnCheckedChanged="cbdetail_OnCheckedChanged"
                                                            AutoPostBack="true" />
                                                        <asp:RadioButton ID="rbheader" runat="server" Visible="false" OnCheckedChanged="rbheader_OnCheckedChanged"
                                                            Text="Header" AutoPostBack="true" GroupName="h1" />
                                                        <asp:RadioButton ID="rbledger" runat="server" Visible="false" OnCheckedChanged="rbledger_OnCheckedChanged"
                                                            Text="Ledger" AutoPostBack="true" GroupName="h1" />
                                                    </td>
                                                    <td colspan="3">
                                                        <fieldset id="divsearch" runat="server" style="width: 240px; height: 20px;">
                                                            <asp:DropDownList ID="rbl_rollno" runat="server" Visible="false" CssClass="textbox  ddlheight"
                                                                AutoPostBack="true" OnSelectedIndexChanged="rbl_rollno_OnSelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:Label ID="lbltext" runat="server" Visible="false"></asp:Label>
                                                            <asp:TextBox ID="txtsearch" runat="server" Style="height: 20px; width: 149px;" placeholder="Search"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetName" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="panelbackground">
                                                            </asp:AutoCompleteExtender>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" TargetControlID="txtsearch"
                                                                FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers" ValidChars=" -.">
                                                            </asp:FilteredTextBoxExtender>
                                                        </fieldset>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btn_search" runat="server" CssClass="textbox btn2" Text="Search"
                                                            OnClientClick="return accType()" OnClick="btnsearch_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <asp:Label ID="lbl_error1" runat="server" Visible="false"></asp:Label>
                                    </center>
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <asp:Chart ID="chart" runat="server" Visible="false">
                            <Series>
                            </Series>
                            <Legends>
                                <asp:Legend Title="Performance Graph" Font="Book Antiqua">
                                </asp:Legend>
                            </Legends>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1">
                                    <AxisY LineColor="White">
                                        <LabelStyle Font="Trebuchet MS, 15pt" />
                                        <MajorGrid LineColor="#e6e6e6" />
                                        <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                    </AxisY>
                                    <AxisX LineColor="White">
                                        <LabelStyle Font="Trebuchet MS,15pt" />
                                        <MajorGrid LineColor="#e6e6e6" />
                                        <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                    </AxisX>
                                </asp:ChartArea>
                            </ChartAreas>
                            <Legends>
                            </Legends>
                        </asp:Chart>
                    </div>
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Label ID="output" runat="server" Visible="false" Style="color: Blue; font-size: large;"></asp:Label>
                                        <br />
                                        <div id="divspread" runat="server" visible="false" style="width: 961px; overflow: auto;
                                            background-color: White; border-radius: 10px;">
                                            <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderStyle="Solid" BorderWidth="0px"
                                                Style="overflow: auto; border: 0px solid #999999; border-radius: 10px; background-color: White;
                                                box-shadow: 0px 0px 8px #999999;" class="spreadborder">
                                                <Sheets>
                                                    <FarPoint:SheetView SheetName="Sheets1">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                            </FarPoint:FpSpread>
                                        </div>
                                    </center>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <center>
                                            <div id="print" runat="server" visible="false">
                                                <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    ForeColor="Red" Text="" Visible="false"></asp:Label>
                                                <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    Text="Report Name"></asp:Label>
                                                <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display()"
                                                    CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                                    InvalidChars="/\">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:Button ID="btnExcel" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    OnClick="btnExcel_Click" Text="Export To Excel" Width="127px" Height="32px" CssClass="textbox textbox1" />
                                                <asp:Button ID="btnprintmasterhed" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    Text="Print" OnClick="btnprintmaster_Click" Height="32px" Style="margin-top: 10px;"
                                                    CssClass="textbox textbox1" Width="60px" />
                                                <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                                            </div>
                                        </center>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
            <center>
                <div id="pupdiv" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="pupdiv1" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
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
            </center>
        </div>
    </body>
    </html>
</asp:Content>
