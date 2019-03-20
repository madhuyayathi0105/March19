<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="DFCR_ReportSchool.aspx.cs" Inherits="DFCR_ReportSchool" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
            function validates() {
                var txt = document.getElementById('<%=txtexcelname.ClientID %>');
                var alerttxt = document.getElementById('<%=lblvalidation1.ClientID %>');
                if (txt.value.trim() == "") {
                    alerttxt.innerHTML = "Please Enter Your Report Name!";
                    return false;
                }
            }
            function divhide() {
                var txt = document.getElementById('<%=divcolorder.ClientID %>');
                txt.style.display = "none";
                return false;
            }
            function columnOrderCbl() {
                var cball = document.getElementById('<%=cb_column.ClientID%>');
                var cblall = document.getElementById('<%=cblcolumnorder.ClientID%>');
                var tagname = cblall.getElementsByTagName("input");
                if (cball.checked == true) {
                    for (var i = 0; i < tagname.length; i++) {
                        tagname[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < tagname.length; i++) {
                        tagname[i].checked = false;
                    }
                }
            }

            function columnOrderCb() {
                var count = 0;
                var cball = document.getElementById('<%=cb_column.ClientID%>');
                var cblall = document.getElementById('<%=cblcolumnorder.ClientID%>');
                var tagname = cblall.getElementsByTagName("input");
                for (var i = 0; i < tagname.length; i++) {
                    if (tagname[i].checked == true) {
                        count += 1;
                    }
                }
                if (tagname.length == count) {
                    cball.checked = true;
                }
                else {
                    cball.checked = false;
                }

            }
            function OnCheckBoxCheckChanged(evt) {

                var src = window.event != window.undefined ? window.event.srcElement : evt.target;
                var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
                if (isChkBoxClick) {
                    var parentTable = GetParentByTagName("table", src);
                    var nxtSibling = parentTable.nextSibling;
                    if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node
                    {
                        if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
                        {
                            //check or uncheck children at all levels
                            CheckUncheckChildren(parentTable.nextSibling, src.checked);
                        }
                    }
                    //check or uncheck parents at all levels
                    CheckUncheckParents(src, src.checked);
                }
            }
            function CheckUncheckChildren(childContainer, check) {
                var childChkBoxes = childContainer.getElementsByTagName("input");
                var childChkBoxCount = childChkBoxes.length;
                for (var i = 0; i < childChkBoxCount; i++) {
                    childChkBoxes[i].checked = check;
                }
            }
            function CheckUncheckParents(srcChild, check) {
                var parentDiv = GetParentByTagName("div", srcChild);
                var parentNodeTable = parentDiv.previousSibling;

                if (parentNodeTable) {
                    var checkUncheckSwitch;

                    if (check) //checkbox checked
                    {
                        var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                        if (isAllSiblingsChecked)
                            checkUncheckSwitch = true;
                        else
                            return; //do not need to check parent if any(one or more) child not checked
                    }
                    else //checkbox unchecked
                    {
                        checkUncheckSwitch = false;
                    }

                    var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
                    if (inpElemsInParentTable.length > 0) {
                        var parentNodeChkBox = inpElemsInParentTable[0];
                        parentNodeChkBox.checked = checkUncheckSwitch;
                        //do the same recursively
                        CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
                    }
                }
            }
            function AreAllSiblingsChecked(chkBox) {
                var parentDiv = GetParentByTagName("div", chkBox);
                var childCount = parentDiv.childNodes.length;
                for (var i = 0; i < childCount; i++) {
                    if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
                    {
                        if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                            var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                            //if any of sibling nodes are not checked, return false
                            if (!prevChkBox.checked) {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            //utility function to get the container of an element by tagname
            function GetParentByTagName(parentTagName, childElementObj) {
                var parent = childElementObj.parentNode;
                while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
                    parent = parent.parentNode;
                }
                return parent;
            }
        </script>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Daily Fees Collection Student Report</span></div>
            </center>
        </div>
        <div>
            <center>
                <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lblclg" Text="College" Font-Names="Book Antiqua" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollege" runat="server" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                                    AutoPostBack="true" Style="height: 24px; width: 144px;">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbltype" Text="Type" runat="server" Font-Names="Book Antiqua" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddltype" runat="server" Style="height: 24px; width: 95px;"
                                    Font-Names="Book Antiqua" AutoPostBack="true" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblbatch" runat="server" Text="Batch"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UP_batch" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_batch" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                            height: auto;">
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
                                        <asp:TextBox ID="txt_degree" runat="server" Style="height: 20px; width: 125px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_degree" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                            height: auto;">
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
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Up_dept" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_dept" runat="server" Style="height: 20px; width: 140px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                            height: auto;">
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
                            <td id="tdstdtype" runat="server" visible="false">
                                <asp:Label runat="server" ID="lblstudtypr" Text="Student Type" Font-Names="Book Antiqua"
                                    Style="font-family: Book Antiqua; width: 103px"></asp:Label>
                            </td>
                            <td id="tdstdflttype" runat="server" visible="false">
                                <asp:DropDownList ID="ddlstudtype" runat="server" Style="font-family: Book Antiqua;
                                    height: auto;" OnSelectedIndexChanged="ddlstudtype_change" AutoPostBack="true"
                                    Width="145px">
                                    <asp:ListItem>All</asp:ListItem>
                                    <asp:ListItem Value="1">Regular</asp:ListItem>
                                    <asp:ListItem Value="3">Lateral</asp:ListItem>
                                    <asp:ListItem Value="2">Transfer</asp:ListItem>
                                    <asp:ListItem Value="4">Re-admit</asp:ListItem>
                                    <asp:ListItem Value="5">Re-join</asp:ListItem>
                                    <asp:ListItem Value="6">EnRoll</asp:ListItem>
                                    <asp:ListItem Value="7">Before Admission</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblacctype" Text="A/c Type" Font-Names="Book Antiqua"
                                    Style="font-family: Book Antiqua; width: 103px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlacctype" runat="server" Style="font-family: Book Antiqua;
                                    height: auto; width: 95px;" OnSelectedIndexChanged="ddlacctype_change" AutoPostBack="true">
                                    <%--  <asp:ListItem>---Select---</asp:ListItem>--%>
                                    <%--  <asp:ListItem>Group Header</asp:ListItem>--%>
                                    <asp:ListItem>Header</asp:ListItem>
                                    <%--  <asp:ListItem>Ledger</asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label1" Text="A/c Header" Font-Names="Book Antiqua"
                                    Style="font-family: Book Antiqua; width: 118px;"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtheader" runat="server" Height="30px" CssClass="Dropdown_Txt_Box"
                                            ReadOnly="true" Width="101px" Style="height: 20px; font-family: 'Book Antiqua';"
                                            Enabled="false">---Select---</asp:TextBox>
                                        <asp:Panel ID="paccheader" runat="server" Style="height: auto; width: 250px;" CssClass="multxtpanel multxtpanleheight">
                                            <asp:CheckBox ID="cbheader" runat="server" Font-Names="Book Antiqua" OnCheckedChanged="cbheader_OnCheckedChanged"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cblheader" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblheader_SelectedIndexChanged"
                                                Font-Names="Book Antiqua">
                                            </asp:CheckBoxList>
                                            <asp:TreeView ID="treeledger" runat="server" SelectedNodeStyle-ForeColor="Red" HoverNodeStyle-BackColor="Black"
                                                Width="450px" Font-Names="Book Antiqua" ForeColor="Black" ShowCheckBoxes="All">
                                            </asp:TreeView>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtheader"
                                            PopupControlID="paccheader" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblledger" runat="server" Text="Ledger"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
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
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblfeetype" Text="Fee Type" Font-Names="Book Antiqua"
                                    Width="110px" Style="font-family: Book Antiqua;"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlfeetype" runat="server" Style="font-family: Book Antiqua;
                                    height: auto;" Enabled="false" OnSelectedIndexChanged="ddlfeetype_change" AutoPostBack="true"
                                    Width="145px">
                                    <asp:ListItem>Paid</asp:ListItem>
                                    <asp:ListItem>Yet To Be Paid</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblsem" runat="server" Text="Semester"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Updp_sem" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_sem" runat="server" Style="height: 20px; width: 95px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_sem" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                            height: auto;">
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
                                PayMode
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upd_paid" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_paid" runat="server" Style="height: 20px; width: 101px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnl_paid" runat="server" CssClass="multxtpanel multxtpanleheight"
                                            Style="width: 126px; height: auto;">
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
                                <asp:DropDownList ID="ddlfinlyear" runat="server" Style="font-family: Book Antiqua;
                                    height: auto;" Width="145px">
                                </asp:DropDownList>
                            </td>
                            <td id="tdfinlyr" runat="server" visible="false">
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtfyear" Style="height: 20px; width: 125px;" CssClass="Dropdown_Txt_Box"
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
                            <td colspan="8">
                                <table>
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
                                                            <%--  <asp:ListItem Value="1">Regular</asp:ListItem>
                                        <asp:ListItem Value="3">Lateral</asp:ListItem>
                                        <asp:ListItem Value="2">Transfer</asp:ListItem>--%>
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_type"
                                                        PopupControlID="Panel6" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td colspan="2">
                                            <asp:CheckBox ID="checkdicon" runat="server" Text="Student Catagory" Font-Names="Book Antiqua"
                                                Style="width: 200px;" />
                                            <%-- AutoPostBack="true" OnCheckedChanged="checkdicon_Changed"--%>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtinclude" Font-Names="Book Antiqua" Style="height: 20px;" CssClass="Dropdown_Txt_Box"
                                                        runat="server" ReadOnly="true" Width="110px">--Select--</asp:TextBox>
                                                    <asp:Panel ID="pnlinclude" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                        Width="172px">
                                                        <asp:CheckBox ID="cbinclude" runat="server" Text="Select All" Font-Names="Book Antiqua"
                                                            OnCheckedChanged="cbinclude_OnCheckedChanged" AutoPostBack="True" />
                                                        <asp:CheckBoxList ID="cblinclude" runat="server" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblinclude_OnSelectedIndexChanged"
                                                            AutoPostBack="True">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txtinclude"
                                                        PopupControlID="pnlinclude" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cbdate" runat="server" Text="Date" AutoPostBack="true" OnCheckedChanged="cbdate_Changed" />
                                        </td>
                                        <td colspan="2">
                                            <div id="divdatewise" runat="server">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_fromdate" runat="server" Text="From"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_fromdate" runat="server" Style="height: 20px; width: 68px;"
                                                                onchange="return checkDate()"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                            </asp:CalendarExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_todate" runat="server" Text="To" Style="margin-left: 4px;"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_todate" runat="server" Style="height: 20px; width: 68px;" onchange="return checkDate()"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                            </asp:CalendarExtender>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cbRcpt" runat="server" Text="Receipt No" />
                                        </td>
                                        <td colspan="2">
                                            <asp:LinkButton ID="lnkcolorder" runat="server" Text="Column Order" OnClick="lnkcolorder_Click"></asp:LinkButton>
                                            <asp:CheckBox ID="cbBeforAdm" runat="server" Text="Before Admission" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnGo" runat="server" OnClick="btnGo_Click" Text="Go" Style="font-family: Book Antiqua;
                                                font-weight: 700;" />
                                        </td>
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkfeeroll" runat="server" Visible="false" Text="Fee off the Role"
                                                Font-Names="Book Antiqua" Style="width: 200px;" />
                                            <asp:CheckBox ID="cbbfadm" runat="server" Visible="false" Text="Before Admission"
                                                Font-Names="Book Antiqua" />
                                            <asp:CheckBox ID="cbledgacr" runat="server" Text="Ledger Acr" Visible="false" Enabled="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <table>
                                    <tr>
                                        <td id="tdbank" runat="server" visible="false">
                                            <asp:CheckBox ID="cbbankcheck" runat="server" Text="Include Bank" AutoPostBack="true"
                                                OnCheckedChanged="cbbankcheck_Changed" Font-Names="Book Antiqua" Style="width: 200px;" />
                                        </td>
                                        <td colspan="2" id="tdbanks" runat="server" visible="false">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtbankname" Enabled="false" Font-Names="Book Antiqua" Style="height: 20px;
                                                        width: 164px;" CssClass="Dropdown_Txt_Box" runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                                    <asp:Panel ID="pnlbank" runat="server" CssClass="multxtpanel multxtpanleheight" Width="172px">
                                                        <asp:CheckBox ID="cbbank" runat="server" Text="Select All" Font-Names="Book Antiqua"
                                                            OnCheckedChanged="cbbank_OnCheckedChanged" AutoPostBack="True" />
                                                        <asp:CheckBoxList ID="cblbank" runat="server" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblbank_OnSelectedIndexChanged"
                                                            AutoPostBack="True">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txtbankname"
                                                        PopupControlID="pnlbank" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td colspan="5">
                                            <%-- <asp:LinkButton ID="lnkcolorder" runat="server" Text="Column Order" OnClick="lnkcolorder_Click"></asp:LinkButton>--%>
                                            <asp:CheckBox ID="cbabstract" runat="server" Text="Abstract" Visible="false" />
                                            <asp:CheckBox ID="cbdegwisetotal" runat="server" Text="Datewise Total" Visible="false" />
                                            <asp:CheckBox ID="cbdgreename" runat="server" Text="Without Date Total" Visible="false" />
                                            <asp:CheckBox ID="cbdeptName" runat="server" Text="Degree Name" Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div id="divlabl" runat="server" visible="false">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblcash" runat="server" Text="Cash" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" BackColor="LightCoral"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblchq" runat="server" Text="Cheque" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" BackColor="LightGray"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lbldd" runat="server" Text="DD" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" BackColor="Orange"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblchal" runat="server" Text="Challan" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" BackColor="LightGreen"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblonline" runat="server" Text="Online" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" BackColor="LightGoldenrodYellow"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblcard" runat="server" Text="Card" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" BackColor="white"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <FarPoint:FpSpread ID="spreadDet" runat="server" Visible="false" BorderStyle="Solid"
                        BorderWidth="0px" Style="overflow: auto; border: 0px solid #999999; border-radius: 10px;
                        background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="Green">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <br />
                    <br />
                    <center>
                        <div id="print" runat="server" visible="false">
                            <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                ForeColor="Red" Text="" Visible="true"></asp:Label>
                            <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display()"
                                CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                InvalidChars="/\">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnExcel" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                OnClientClick="return validates();" OnClick="btnExcel_Click" Text="Export To Excel"
                                Width="127px" Height="32px" CssClass="textbox textbox1" />
                            <%--OnClientClick="return validate();"--%>
                            <asp:Button ID="btnprintmasterhed" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Text="Print" OnClick="btnprintmaster_Click" Height="32px" Style="margin-top: 10px;"
                                CssClass="textbox textbox1" Width="60px" />
                            <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                        </div>
                    </center>
                </div>
            </center>
            <center>
                <div id="divcolorder" runat="server" style="height: 100%; display: none; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div2" runat="server" class="table" style="background-color: White; height: 200px;
                            width: 650px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 100px;
                            border-radius: 10px;">
                            <center>
                                <table>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblcolr" runat="server" Text="Column Order" Style="font-family: Book Antiqua;
                                                font-size: 20px; font-weight: bold; color: Green;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cb_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Select All" onchange="return columnOrderCbl()" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" Width="600px"
                                                Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                                RepeatDirection="Horizontal" onclick="return columnOrderCb()">
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <br />
                                            <center>
                                                <asp:Button ID="btncolorderOK" OnClientClick="return divhide();" CssClass=" textbox btn1 comm"
                                                    Style="height: 28px; width: 65px;" OnClick="btncolorderOK_Click" Text="OK" runat="server" />
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
        <center>
            <div id="alertDiv" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
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
    </body>
</asp:Content>
