<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudDuePaidList.aspx.cs" Inherits="StudDuePaidList" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .rdbstyle input[type=radio]
        {
            display: none;
        }
        .rdbstyle input[type=radio] + label
        {
            display: inline-block;
            margin: -2px;
            padding: 4px 12px;
            margin-bottom: 0;
            font-size: 14px;
            line-height: 20px;
            color: #993399;
            text-align: center;
            text-shadow: 0 1px 1px rgba(255,255,255,0.75);
            vertical-align: middle;
            cursor: pointer;
            background-color: #f5f5f5;
            background-image: -moz-linear-gradient(top,#fff,#e6e6e6);
            background-image: -webkit-gradient(linear,0 0,0 100%,from(#fff),to(#e6e6e6));
            background-image: -webkit-linear-gradient(top,#fff,#e6e6e6);
            background-image: -o-linear-gradient(top,#fff,#e6e6e6);
            background-image: linear-gradient(to bottom,#fff,#e6e6e6);
            background-repeat: repeat-x;
            border: 1px solid #ccc;
            border-color: #e6e6e6 #e6e6e6 #bfbfbf;
            border-color: rgba(0,0,0,0.1) rgba(0,0,0,0.1) rgba(0,0,0,0.25);
            border-bottom-color: #b3b3b3;
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffffffff',endColorstr='#ffe6e6e6',GradientType=0);
            filter: progid:DXImageTransform.Microsoft.gradient(enabled=false);
            -webkit-box-shadow: inset 0 1px 0 rgba(255,255,255,0.2),0 1px 2px rgba(0,0,0,0.05);
            -moz-box-shadow: inset 0 1px 0 rgba(255,255,255,0.2),0 1px 2px rgba(0,0,0,0.05);
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.2),0 1px 2px rgba(0,0,0,0.05);
        }
        .rdbstyle input[type=radio]:checked + label
        {
            background-image: none;
            outline: 0;
            -webkit-box-shadow: inset 0 2px 4px rgba(0,0,0,0.15),0 1px 2px rgba(0,0,0,0.05);
            -moz-box-shadow: inset 0 2px 4px rgba(0,0,0,0.15),0 1px 2px rgba(0,0,0,0.05);
            box-shadow: inset 0 2px 4px rgba(0,0,0,0.15),0 1px 2px rgba(0,0,0,0.05);
            border-bottom-color: #b3b3b3;
            border-bottom-style: solid;
            border-bottom-color: #89D17C;
            border-bottom-width: medium;
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
        .lnk:hover
        {
            text-shadow: 0 1px 1px rgba(255,255,255,0.75);
            color: Green;
            font-weight: bold;
        }
    </style>
    <style type="text/css">
        .printclass
        {
            display: none;
        }
        .grid-view
        {
            padding: 0;
            margin: 0;
            border: 1px solid #333;
            font-family: "Book antiqua";
            font-size: small;
        }
        
        .grid-view tr.header
        {
            color: white;
            background-color: #0CA6CA;
            height: 30px;
            vertical-align: middle;
            text-align: center;
            font-weight: bold;
            font-size: 20px;
        }
        
        .grid-view tr.normal
        {
            color: black;
            background-color: #FDC64E;
            height: 25px;
            vertical-align: middle;
            text-align: center;
        }
        
        .grid-view tr.alternate
        {
            color: black;
            background-color: #D59200;
            height: 25px;
            vertical-align: middle;
            text-align: center;
        }
        
        .grid-view tr.normal:hover, .grid-view tr.alternate:hover
        {
            background-color: white;
            color: black;
            font-weight: bold;
        }
        
        .grid_view_lnk_button
        {
            color: Black;
            text-decoration: none;
            font-size: large;
        }
        .lbl
        {
            font-family: Book Antiqua;
            font-size: 30px;
            font-weight: bold;
            color: Green;
            text-align: center;
            font-style: italic;
        }
        .hdtxt
        {
            font-family: Book Antiqua;
            font-size: large;
            font-weight: bold;
        }
        .FixedHeader
        {
            position: absolute;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=btnExcel.ClientID%>').click(function () {
                var excelName = $('#<%=txtexcelname.ClientID%>').val();
                if (excelName == null || excelName == "") {
                    $('#<%=lblvalidation1.ClientID%>').show();
                    return false;
                }
                else {
                    $('#<%=lblvalidation1.ClientID%>').hide();
                }
            });

            $('#<%=txtexcelname.ClientID %>').keypress(function () {
                $('#<%=lblvalidation1.ClientID %>').hide();
            });
            $('#<%=imgcolumn.ClientID %>').click(function () {
                $('#<%=popstud.ClientID %>').hide();
                return false;
            });
        });
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
            //  var currentDate = today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1;
            var yyyy = today.getFullYear();
            if (dd < 10) { dd = '0' + dd }
            if (mm < 10) { mm = '0' + mm }
            var today = dd + '/' + mm + '/' + yyyy;

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
                document.getElementById('<%=txt_fromdate.ClientID%>').value = today;
                document.getElementById('<%=txt_todate.ClientID%>').value = today;
                alert("To date should be greater than from date ");
                return false;
            }
        }
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlContents.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=1191');
            printWindow.document.write('<html');
            printWindow.document.write('<head><title>SchoolCompartmentWise</title>');
            printWindow.document.write('</head><body >');
            printWindow.document.write('<form>');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write(' </form>');
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span id="sphd" runat="server" class="fontstyleheader" style="color: Green;">Student
                    Paid Balance Details</span>
            </div>
        </center>
    </div>
    <div>
        <center>
            <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtclg" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlclg" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 350px;
                                        height: 120px;">
                                        <asp:CheckBox ID="cbclg" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cbclg_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="cblclg" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblclg_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txtclg"
                                        PopupControlID="pnlclg" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_graduation" runat="server" Text="Graduation"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updgrad" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_grad" runat="server" CssClass="textbox  textbox1 " Width="100px"
                                        ReadOnly="true">-- Select--</asp:TextBox>
                                    <asp:Panel ID="pnlgrad" runat="server" BackColor="White" CssClass="multxtpanel" Height="250px"
                                        Width="120px" Style="position: absolute;">
                                        <asp:CheckBox ID="cb_grad" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_grad_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_grad" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_grad_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender18" runat="server" TargetControlID="txt_grad"
                                        PopupControlID="pnlgrad" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lblbatch" Text="Batch" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtbatch" runat="server" CssClass="textbox  textbox1" Width="80px"
                                        ReadOnly="true">-- Select--</asp:TextBox>
                                    <asp:Panel ID="pnlbatch" runat="server" BackColor="White" CssClass="multxtpanel"
                                        Height="250px" Width="120px" Style="position: absolute;">
                                        <asp:CheckBox ID="cbbatch" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cbbatch_checkedchange" />
                                        <asp:CheckBoxList ID="cblbatch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblbatch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtbatch"
                                        PopupControlID="pnlbatch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_degree" Text="Degree" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  textbox1" Width="80px"
                                        ReadOnly="true">-- Select--</asp:TextBox>
                                    <asp:Panel ID="p3" runat="server" BackColor="White" CssClass="multxtpanel" Height="250px"
                                        Width="120px" Style="position: absolute;">
                                        <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_degree_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtenderdeg" runat="server" TargetControlID="txt_degree"
                                        PopupControlID="p3" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_branch" Text="Branch" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_branch" runat="server" CssClass="textbox textbox1 txtheight3"
                                        ReadOnly="true">-- Select--</asp:TextBox>
                                    <asp:Panel ID="p4" runat="server" CssClass="multxtpanel" Height="250px"  Width="250px">
                                        <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_branch_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender19" runat="server" TargetControlID="txt_branch"
                                        PopupControlID="p4" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_org_sem" Text="Semester" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox textbox1 " Width="100px"
                                        ReadOnly="true">-- Select--</asp:TextBox>
                                    <asp:Panel ID="Panel11" runat="server" CssClass="multxtpanel" Height="250px" Width="100px"
                                        Style="position: absolute;">
                                        <asp:CheckBox ID="cb_sem" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_sem_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender20" runat="server" TargetControlID="txt_sem"
                                        PopupControlID="Panel11" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_sec" Text="Section" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_sec" runat="server" Width="70px" CssClass="textbox textbox1 txtheight"
                                        ReadOnly="true">-- Select--</asp:TextBox>
                                    <asp:Panel ID="Panel8" runat="server" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                        <asp:CheckBox ID="cb_sec" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_sec_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_sec" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sec_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_sec"
                                        PopupControlID="Panel8" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
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
                            <asp:Label ID="lbl_ledger" runat="server" Text="Ledger" Style="width: 50px;"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
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
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_studled"
                                        PopupControlID="pnl_studled" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            PayMode
                        </td>
                        <td>
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
                        <td id="tdlblfin" runat="server" visible="false">
                            <asp:Label runat="server" ID="lblfyear" Text="FinanceYear" Width="85px"></asp:Label>
                        </td>
                        <td id="tdfin" runat="server" visible="false">
                            <asp:DropDownList ID="ddlfinyear" runat="server" CssClass="textbox ddlstyle ddlheight3">
                            </asp:DropDownList>
                        </td>
                        <td id="tdfnl" runat="server" visible="false">
                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtfyear" Style="height: 20px; width: 118px;" CssClass="Dropdown_Txt_Box"
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
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_type" runat="server" Text="Type"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_type" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel6" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                        height: auto;">
                                        <asp:CheckBox ID="cb_type" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_type_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_type" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_type_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_type"
                                        PopupControlID="Panel6" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_seat" runat="server" Text="Seat Type"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_seat" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel5" runat="server" CssClass="multxtpanel" Style="width: 200px;
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
                        <td>
                            <asp:Label ID="lbl_Community" runat="server" Text="Community"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_community" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="panelcomm" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                        height: auto;">
                                        <asp:CheckBox ID="cb_community" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_community_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_community" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_community_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_community"
                                        PopupControlID="panelcomm" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td colspan="3">
                            <div id="divdatewise" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_fromdate" runat="server" Text="From"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_fromdate" runat="server" Style="height: 20px; width: 67px;"
                                                onchange="return checkDate()"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_todate" runat="server" Text="To" Style="margin-left: 4px;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_todate" runat="server" Style="height: 20px; width: 67px;" onchange="return checkDate()"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:CheckBox ID="checkdicon" runat="server" Text="Include Student Catagory" Font-Names="Book Antiqua"
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
                        <td>
                            <asp:Button ID="btngo" runat="server" CssClass="textbox btn2" Text="Go" OnClick="btngo_Click" />
                        </td>
                    </tr>
                </table>
                <div id="divlabl" runat="server" visible="false">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblcash" runat="server" Text="Cash" Visible="false" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" BackColor="LightCoral"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblchq" runat="server" Text="Cheque" Visible="false" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" BackColor="LightGray"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbldd" runat="server" Text="DD" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" BackColor="Orange"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblchal" runat="server" Text="Challan" Visible="false" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" BackColor="LightGreen"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblonline" runat="server" Text="Online" Visible="false" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" BackColor="LightGoldenrodYellow"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblcard" runat="server" Text="Card" Visible="false" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" BackColor="white"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <center>
                    <div id="print" runat="server" visible="false">
                        <asp:Label ID="lblvalidation1" runat="server" Text="Please Enter Your Report Name"
                            Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Red" Style="display: none;"></asp:Label>
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
                <br />
                <FarPoint:FpSpread ID="spreadDet" runat="server" Visible="false" BorderStyle="Solid"
                    BorderWidth="0px" Style="overflow: auto; border: 0px solid #999999; border-radius: 10px;
                    background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder"
                    OnCellClick="spreadDet_OnCellClick" OnPreRender="spreadDet_Selectedindexchanged">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
            </div>
        </center>
        <center>
            <div id="popstud" runat="server" style="height: 100%; z-index: 1000; width: 100%;
                background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;
                display: none;">
                <asp:ImageButton ID="imgcolumn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 90px; margin-left: 456px;" />
                <%--   <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>--%>
                <center>
                    <div id="Div2" runat="server" class="table" style="background-color: White; height: 520px;
                        width: 950px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 100px;
                        border-radius: 10px;">
                        <br />
                        <span style="font-size: larger; color: Green; font-weight: bold;">Student Details</span>
                        <asp:Panel ID="pnlContents" runat="server" Visible="false">
                            <style type="text/css" media="print">
                                @page
                                {
                                    size: A3 portrait;
                                    margin: 0.5cm;
                                }
                                @media print
                                {
                                    .printclass
                                    {
                                        display: table;
                                    }
                                    thead
                                    {
                                        display: table-header-group;
                                    }
                                    tfoot
                                    {
                                        display: table-footer-group;
                                    }
                                    #header
                                    {
                                        position: fixed;
                                        top: 0px;
                                        left: 0px;
                                    }
                                    #footer
                                    {
                                        position: fixed;
                                        bottom: 0px;
                                        left: 0px;
                                    }
                                    #printable
                                    {
                                        position: relative;
                                        bottom: 30px;
                                        height: 300;
                                    }
                                
                                }
                                @media screen
                                {
                                    thead
                                    {
                                        display: block;
                                    }
                                    tfoot
                                    {
                                        display: block;
                                    }
                                }
                            </style>
                            <div id="printable">
                                <table>
                                    <thead>
                                        <tr>
                                            <th>
                                                <div style="margin: 0px; border: 0px;">
                                                    <table class="printclass" style="width: 100%; font-weight: bold; font-family: Book Antiqua;
                                                        font-size: medium; margin: 0px; margin-top: 20px;">
                                                        <tr>
                                                            <td rowspan="6" style="width: 80px; margin: 0px; border: 0px;">
                                                                <asp:Image ID="imgLeftLogo" runat="server" AlternateText="" ImageUrl="~/college/Left_Logo.jpeg"
                                                                    Width="80px" Height="100px" Style="margin: 0px; border: 0px;" />
                                                            </td>
                                                            <td align="center">
                                                                <span id="spCollege" runat="server" style="font-size: 18px;"></span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                <span id="spAffBy" runat="server" style="font-size: 15px;"></span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                <span id="spController" runat="server" style="font-size: 15px;"></span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                <span id="spSeating" runat="server" style="font-size: 15px;"></span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" colspan="2">
                                                                <span id="spDateSession" runat="server" style="font-size: 14px; display: none;">
                                                                </span><span id="sprptnamedt" runat="server" style="font-size: 14px;"></span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" colspan="2">
                                                                Date: <span id="spdate" runat="server" style="font-size: 14px;"></span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="display: none;">
                                                <center>
                                                    <div>
                                                        <asp:Table ID="tblFormat2" runat="server" Style="width: 1417px; border-color: Black;
                                                            text-align: center; border-bottom: 0px solid black; font-weight: bold; font-size: medium;
                                                            border-style: solid; border-width: 1px;">
                                                            <asp:TableRow ID="tblRow1" runat="server">
                                                                <asp:TableCell ID="tblcellsno" runat="server" Text="S.No" Width="30px"></asp:TableCell>
                                                                <asp:TableCell ID="tblcellInvName" runat="server" Text="Invigilator Name" Width="69px"></asp:TableCell>
                                                                <asp:TableCell ID="tblcellHallNo" runat="server" Text="Hall No" Width="65px"></asp:TableCell>
                                                                <asp:TableCell ID="tcInvSign" runat="server" Text="Initials of the Invigilator" Width="65px"></asp:TableCell>
                                                                <asp:TableCell ID="TableCell4" runat="server" Text="Degree/Branch" Width="105px"></asp:TableCell>
                                                                <asp:TableCell ID="TableCell6" runat="server" Text="Subject Code" Width="80px"></asp:TableCell>
                                                                <asp:TableCell ID="TableCell7" runat="server" Text="Reg. No of the Candidate" Width="380px"></asp:TableCell>
                                                                <asp:TableCell ID="TableCell8" runat="server" Text="Total No of Student" Width="70px"></asp:TableCell>
                                                                <asp:TableCell ID="tcBooletNo" runat="server" Text="Answer Booklet Numbers" Width="40px"></asp:TableCell>
                                                                <asp:TableCell ID="tcHallSuperend" runat="server" Text="Signature <br/>of the<br/> Hall <br/>Superintendents"
                                                                    Width="40px"></asp:TableCell>
                                                                <asp:TableCell ID="TableCell11" runat="server" Text="Present" Width="55px"></asp:TableCell>
                                                                <asp:TableCell ID="TableCell12" runat="server" Text="Absent" Width="55px"></asp:TableCell>
                                                                <asp:TableCell ID="TableCell13" runat="server" Text="Initials<br/> of the<br/> Invigilator"
                                                                    Width="65px"></asp:TableCell>
                                                            </asp:TableRow>
                                                        </asp:Table>
                                                    </div>
                                                </center>
                                            </td>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <span id="Span1" runat="server" style="font-size: medium; color: Green; font-weight: bold;">
                                                    Credit:</span> <span id="spcredit" runat="server" style="font-size: medium; color: Green;
                                                        font-weight: bold;"></span><span id="Span2" runat="server" style="font-size: medium;
                                                            color: Green; font-weight: bold;">Debit:</span> <span id="spdebit" runat="server"
                                                                style="font-size: medium; color: Green; font-weight: bold;"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="height: 390px; width: 945px; overflow: auto;">
                                                    <asp:GridView ID="gridstud" runat="server" GridLines="Both" AutoGenerateColumns="true"
                                                        CssClass="grid-view" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-Font-Size="Medium"
                                                        ForeColor="Black" Style="width: auto; overflow: auto;" OnRowDataBound="gridstud_OnRowDataBound">
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </asp:Panel>
                        <br />
                        <div>
                            <asp:Button ID="btnpdfstud" runat="server" CssClass="textbox btn2" Text="PDF" OnClientClick=" return PrintPanel()" />
                            <%--OnClick="btnpdfstud_Click"--%>
                        </div>
                    </div>
                </center>
            </div>
        </center>
    </div>
</asp:Content>
