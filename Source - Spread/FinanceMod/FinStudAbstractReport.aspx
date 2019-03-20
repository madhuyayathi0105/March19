<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="FinStudAbstractReport.aspx.cs" Inherits="FinStudAbstractReport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=btnExcel.ClientID %>').click(function () {
                var txtval = $('#<%=txtexcelname.ClientID %>').val();
                if (txtval == null || txtval == "") {
                    $('#<%=lblvalidation1.ClientID %>').show();
                    $('#<%=lblvalidation1.ClientID %>').text("Please Enter Your  Report Name");
                    return false;
                }
            });
        });
        function display() {
            document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
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
                document.getElementById('<%=txt_fromdate.ClientID%>').value = currentDate;
                document.getElementById('<%=txt_todate.ClientID%>').value = currentDate;
                alert("To date should be greater than from date ");
                return false;
            }
        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span id="sphd" runat="server" class="fontstyleheader" style="color: Green;"></span>
            </div>
        </center>
    </div>
    <div>
        <center>
            <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lblclg" Text="College" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcollegename" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                OnSelectedIndexChanged="ddlcollegename_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblbatch" runat="server" Text="Batch"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlbatch" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged" AutoPostBack="true" Width="109px">
                            </asp:DropDownList>
                            <%--<asp:UpdatePanel ID="UP_batch" runat="server">
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
                            </asp:UpdatePanel>--%>
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
                        <td>
                            <asp:Label ID="lblsem" runat="server" Text="Semester"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="Updp_sem" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_sem" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_sem" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                        height: 190px;">
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
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblheader" runat="server" Text="Header" Style="width: 50px;"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_studhed" runat="server" Style="height: 20px; width: 100px;"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnl_studhed" runat="server" CssClass="multxtpanel multxtpanleheight"
                                        Style="width: 300px; height: 180px;">
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
                            <asp:Label ID="lblledger" runat="server" Text="Ledger"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_studled" runat="server" Style="height: 20px; width: 100px;"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnl_studled" runat="server" CssClass="multxtpanel multxtpanleheight"
                                        Style="width: 300px; height: 180px;">
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
                        <td id="lblpay" runat="server" visible="false">
                            PayMode
                        </td>
                        <td id="lblpayflt" runat="server" visible="false">
                            <asp:UpdatePanel ID="upd_paid" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_paid" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnl_paid" runat="server" CssClass="multxtpanel multxtpanleheight"
                                        Style="width: 126px; height: 160px;">
                                        <asp:CheckBox ID="chk_paid" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="chk_paid_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="chkl_paid" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_paid_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_paid"
                                        PopupControlID="pnl_paid" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblfyear" Text="FinanceYear" Width="85px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlFinYear" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                OnSelectedIndexChanged="ddlFinYear_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                            <%-- <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtfyear" Style="height: 20px; width: 101px;" CssClass="Dropdown_Txt_Box"
                                        runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                    <asp:Panel ID="Pfyear" runat="server" CssClass="multxtpanel" Width="178px">
                                        <asp:CheckBox ID="chkfyear" runat="server" Text="Select All" OnCheckedChanged="chkfyear_changed"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chklsfyear" runat="server" OnSelectedIndexChanged="chklsfyear_selected"
                                            AutoPostBack="True">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txtfyear"
                                        PopupControlID="Pfyear" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </td>
                        <td>
                            <asp:Label ID="lbl_seat" runat="server" Text="SeatType"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_seat" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel5" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                        height: auto;">
                                        <asp:CheckBox ID="cb_seat" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_seat_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_seat" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_seat_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_seat"
                                        PopupControlID="Panel5" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="checkdicon" runat="server" Text="Student Catagory" Font-Names="Book Antiqua"
                                Font-Size="Medium" Style="width: 200px;" />
                            <%--AutoPostBack="true" OnCheckedChanged="checkdicon_Changed"--%>
                        </td>
                        <td colspan="2">
                            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtinclude" Enabled="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                        Style="height: 20px; width: 164px;" CssClass="Dropdown_Txt_Box" runat="server"
                                        ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlinclude" runat="server" CssClass="multxtpanel multxtpanleheight"
                                        Width="200px" Style="height: auto;">
                                        <asp:CheckBox ID="cbinclude" runat="server" Text="Select All" Font-Size="Medium"
                                            Font-Names="Book Antiqua" OnCheckedChanged="cbinclude_OnCheckedChanged" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cblinclude" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                            OnSelectedIndexChanged="cblinclude_OnSelectedIndexChanged" AutoPostBack="True">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txtinclude"
                                        PopupControlID="pnlinclude" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td colspan="8">
                            <table>
                                <tr>
                                    <td colspan="2">
                                        <div id="divdatewise" runat="server">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_fromdate" runat="server" Text="From"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_fromdate" runat="server" Style="height: 20px; width: 75px;"
                                                            onchange="return checkDate()"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_todate" runat="server" Text="To" Style="margin-left: 4px;"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_todate" runat="server" Style="height: 20px; width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                    <%-- <td colspan="2">
                                        <asp:RadioButtonList ID="rbtype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbtype_Selected"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Header" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Ledger" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>   --%>
                                    <td>
                                        <asp:Button ID="btngo" runat="server" CssClass="textbox btn2" Text="Go" OnClick="btngo_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                    </tr>
                </table>
                <br />
                <FarPoint:FpSpread ID="spreadDet" runat="server" Visible="false" BorderStyle="Solid"
                    BorderWidth="0px" Width="980px" Style="overflow: auto; border: 0px solid #999999;
                    border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                    class="spreadborder">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <center>
                    <div id="print" runat="server" visible="false">
                        <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            ForeColor="Red" Text="" Style="display: none;"></asp:Label>
                        <asp:Label ID="lblrptname" runat="server" Visible="false" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" Visible="false" Width="180px" onkeypress="display()"
                            CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                            InvalidChars="/\">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnExcel" runat="server" Visible="false" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnExcel_Click" Text="Export To Excel" Width="127px"
                            Height="32px" CssClass="textbox textbox1" />
                        <asp:Button ID="btnprintmasterhed" runat="server" Visible="false" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" Height="32px"
                            Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                        <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                    </div>
                </center>
            </div>
        </center>
    </div>
</asp:Content>
