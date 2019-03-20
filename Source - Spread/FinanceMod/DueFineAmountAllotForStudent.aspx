<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="DueFineAmountAllotForStudent.aspx.cs" Inherits="DueFineAmountAllotForStudent" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js" type="text/javascript"></script>
    <!--include jQuery Validation Plugin-->
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.12.0/jquery.validate.min.js"
        type="text/javascript"></script>
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID%>').innerHTML = "";
            }
            function checkDateh() {
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

                var spfromdate = fromDate.split("/");

                date = parseInt(spfromdate[0].toString());
                month = parseInt(spfromdate[1].toString());
                year = parseInt(spfromdate[2].toString());

                var totodate = toDate.split("/");
                date1 = parseInt(totodate[0].toString());
                month1 = parseInt(totodate[1].toString());
                year1 = parseInt(totodate[2].toString());

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
                    document.getElementById('<%=txt_fromdate.ClientID%>').value = currentDate;

                    alert("To date should be greater than from date ");
                    return false;
                }
            }


            $(document).ready(function () {
                $('#btnfeesave').click(function () {
                    var header = $('#ddl_trheader');
                    var ledger = $('#ddl_trledger');
                    var amount = $('#txtamount');
                    if (header.length == 0 || $(header).val() == "") {
                        alert("Please Select Any One Header");
                        return false;
                    }
                    if (ledger.length == 0 || $(ledger).val() == "") {
                        alert("Please Select Any One Ledger");
                        return false;
                    }
                    if ($(amount).val() == "" || $(amount).val() == "0") {
                        alert("Please Enter the Amount");
                        return false;
                    }
                })
            });

        </script>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Due Fine Amount Allot For Student</span></div>
            </center>
        </div>
        <div>
            <center>
                <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <center>
                                        <table class="maintablestyle">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_collegename" Text="College" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                        OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_str1" runat="server" Text="Type"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlstream" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlstream_OnSelectedIndexChanged"
                                                        CssClass="textbox  ddlheight" Style="width: 108px;">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    Batch
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UP_batch" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_batch" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                                                height: 200px;">
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
                                                <td>
                                                    <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UP_degree" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_degree" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
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
                                                    <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="Up_dept" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_dept" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
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
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblsem" runat="server" Text="Semester"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="Updp_sem" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_sem" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="panel_sem" runat="server" CssClass="multxtpanel" Style="width: 124px;
                                                                height: 172px;">
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
                                                <td>
                                                    <span>Header</span>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtheader" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="pnlheader" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                                Style="width: 230px; height: 250px;">
                                                                <asp:CheckBox ID="cbheader" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                    OnCheckedChanged="cbheader_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="cblheader" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblheader_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txtheader"
                                                                PopupControlID="pnlheader" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <span>Ledger</span>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtledger" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="pnlledger" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                                Style="width: 280px; height: 250px;">
                                                                <asp:CheckBox ID="cbledger" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                    OnCheckedChanged="cbledger_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="cblledger" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblledger_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txtledger"
                                                                PopupControlID="pnlledger" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <%--   <td>
                                                <asp:DropDownList ID="ddlsemester" runat="server" CssClass="textbox  ddlheight" Style="width: 108px;">
                                                </asp:DropDownList>
                                            </td>--%>
                                                <td>
                                                    <span>Date </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox textbox1 txtheight"
                                                        onblur="return checkDate()"></asp:TextBox>
                                                    <asp:CalendarExtender ID="calfrmdate" runat="server" TargetControlID="txt_fromdate"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <span>Duration</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtduration" runat="server" Width="40px" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderdur" runat="server" TargetControlID="txtduration"
                                                        FilterType="Numbers,Custom" ValidChars="" InvalidChars="/\">
                                                    </asp:FilteredTextBoxExtender>
                                                    <%--   </td>
                                            <td >--%>
                                                    <asp:Button ID="btnGo" runat="server" Text="GO" Font-Bold="true" Font-Names="Book Antiqua"
                                                        CssClass="textbox textbox1 btn1" Width="70px" OnClick="btnGo_Click" />
                                                </td>
                                                <%-- <td>
                                                <span>To </span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox textbox1 txtheight"
                                                    onblur="return checkDate()"></asp:TextBox>
                                                <asp:CalendarExtender ID="caltodate" runat="server" TargetControlID="txt_todate"
                                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>--%>
                                            </tr>
                                        </table>
                                    </center>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <div id="divspread" runat="server" visible="false" style="width: 961px; overflow: auto;
                                            background-color: White; border-radius: 10px;">
                                            <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" BorderStyle="Solid"
                                                BorderWidth="0px" Width="858px" Style="overflow: auto; height: auto; border: 0px solid #999999;
                                                border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                                class="spreadborder" OnButtonCommand="FpSpread1_OnButtonCommand">
                                                <Sheets>
                                                    <FarPoint:SheetView SheetName="Sheet1">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                            </FarPoint:FpSpread>
                                        </div>
                                    </center>
                                    <br />
                                </td>
                            </tr>
                            <%--save students--%>
                            <tr>
                                <td id="tdfees" runat="server" visible="false" colspan="5">
                                    <table>
                                        <tr>
                                            <td>
                                                Header
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_trheader" runat="server" CssClass="textbox ddlheight4"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_trheader_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                Ledger
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_trledger" runat="server" CssClass="textbox ddlheight4">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                Amount
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtamount" runat="server" CssClass="textbox textbox1 txtheight"
                                                    Width="100px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtamount"
                                                    FilterType="Numbers,Custom" ValidChars=". " InvalidChars="/\">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnfeesave" runat="server" Text="Save" Font-Bold="true" Font-Names="Book Antiqua"
                                                    CssClass="textbox textbox1 btn1" Width="70px" OnClick="btnfeesave_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <%--sms send template add--%>
                            <tr>
                                <td colspan="2">
                                    <div id="divpur" runat="server" visible="false">
                                        <asp:Label ID="lblpurpose1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Purpose"></asp:Label>
                                        <asp:DropDownList ID="ddlpurpose" runat="server" AutoPostBack="True" Font-Bold="True"
                                            CssClass="textbox ddlstyle ddlheight3" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Width="300px" OnSelectedIndexChanged="ddlpurpose_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <FarPoint:FpSpread ID="FpSpread2" runat="server" Visible="false" BorderColor="Black"
                                        BorderStyle="Solid" BorderWidth="1px" Height="250px" Width="850px" OnCellClick="FpSpread2_CellClick"
                                        OnPreRender="FpSpread2_SelectedIndexChanged">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" SelectionBackColor="#CE5D5A"
                                                SelectionForeColor="White">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div id="divbtn" runat="server" visible="false">
                                        <asp:Button ID="btnaddtemplate" runat="server" Text="Add Template" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnaddtemplate_Click" />
                                        <asp:Button ID="btndeletetemplate" runat="server" Text="Delete Template" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btndeletetemplate_Click" />
                                    </div>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtmessage" runat="server" Visible="false" TextMode="MultiLine"
                                        Height="200px" Width="500px" Style="font-family: 'Book Antiqua'" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                </td>
                                <br />
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnsendsms" runat="server" Visible="false" Width="90px" Text="SendSms"
                                        Font-Size="Medium" Font-Bold="true" Font-Names="Book Antiqua" CssClass="textbox textbox1 btn1"
                                        OnClick="btnsendsms_Click" />
                                    <br />
                                </td>
                                <%--<td>
                                <asp:Button ID="btnsms" runat="server" Text="SEND" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="btnsms_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="btnxl_Click" />
                            </td>--%>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <div id="print" runat="server" visible="false">
                                            <asp:Label ID="lblvalidation1" runat="server" Style="margin-top: -18px; margin-left: 10px;
                                                position: absolute;" Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Red"
                                                Text="" Visible="false"></asp:Label>
                                            <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Text="Report Name"></asp:Label>
                                            <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display(this)"
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
                                            <%--<asp:Button ID="btnsendsms" runat="server" Width="90px" Text="SendSms" Font-Size="Medium"
                                            Font-Bold="true" Font-Names="Book Antiqua" CssClass="textbox textbox1 btn1" OnClick="btnsendsms_Click" />--%>
                                        </div>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
            <center>
                <div id="divempedit" runat="server" visible="false" style="height: 200%; z-index: 30;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div4" runat="server" class="table" style="background-color: White; height: 410px;
                            width: 710px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 216px;
                            border-radius: 10px;">
                            <center>
                                <asp:Panel ID="templatepanel" runat="server" BorderColor="Black" BackColor="AliceBlue"
                                    Visible="false" BorderWidth="2px" Height="400px" Width="690px">
                                    <div class="PopupHeaderrstud2" id="Div3" style="text-align: center; font-family: MS Sans Serif;
                                        font-size: Small; font-weight: bold">
                                        <table>
                                            <tr>
                                                <td colspan="4">
                                                    <br />
                                                    <span>Message Template</span>
                                                    <br />
                                                    <br />
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Label ID="lblpurpose" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" ForeColor="Black" Text="Purpose" Width="100px"></asp:Label>
                                                    <asp:Button ID="btnplus" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnClick="btnplus_Click" Text=" + " />
                                                    <asp:DropDownList ID="ddlpurposemsg" runat="server" AutoPostBack="True" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" Width="200px">
                                                    </asp:DropDownList>
                                                    <asp:Button ID="btnminus" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnClick="btnminus_Click" Text=" - " />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Label ID="lblerror" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" ForeColor="Red" Style="height: 21px" Width="676px"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtpurposemsg" runat="server" TextMode="MultiLine" Height="200px"
                                                        Width="680px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnTextChanged="txtpurposemsg_TextChanged"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <table>
                                            <tr>
                                                <td colspan="2">
                                                    <center>
                                                        <asp:Button ID="btnsave" runat="server" Text="Save" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" OnClick="btnsave_Click" Height=" 26px" Width=" 88px" />
                                                        <asp:Button ID="btnexit" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Height=" 26px" Width=" 88px" OnClick="btnexit_Click" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </asp:Panel>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
            <center>
                <div id="divtempsecond" runat="server" visible="false" style="height: 200%; z-index: 30;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div5" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 320px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 216px;
                            border-radius: 10px;">
                            <center>
                                <asp:Panel ID="purposepanel" runat="server" BorderColor="Black" BackColor="AliceBlue"
                                    Visible="false" BorderWidth="2px" Height="100px" Width="300px">
                                    <div class="panelinfraction" id="Div2" style="text-align: center; font-family: MS Sans Serif;
                                        font-size: Small; font-weight: bold">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label1" runat="server" Text="Purpose Type" Style="text-align: center;"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblpurposecaption" runat="server" Text="Purpose" Style="font-size: medium;
                                                        font-weight: bold; height: 22px; font-family: 'Book Antiqua'; position: absolute;
                                                        top: 21px; left: 10px;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtpurposecaption" runat="server" Style="font-size: medium; font-weight: bold;
                                                        height: 22px; font-family: 'Book Antiqua';"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnpurposeadd" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                                        height: 26px;" OnClick="btnpurposeadd_Click" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnpurposeexit" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                                        height: 26px; width: 88px;" OnClick="btnpurposeexit_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                </asp:Panel>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
            <center>
                <div id="imgdiv2" runat="server" visible="false" style="height: 200%; z-index: 30;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 216px;
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
            <center>
                <div id="contentDiv" runat="server" style="height: 710px; width: 1344px;" visible="false">
                </div>
            </center>
        </div>
    </body>
    </html>
</asp:Content>
