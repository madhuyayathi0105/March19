<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Finance_MIS_Report.aspx.cs" Inherits="Finance_MIS_Report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <%--MasterPageFile="~/Site.master"--%>
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <body>
        <%--<asp:content id="Content1" contentplaceholderid="HeadContent" runat="Server">--%>
        <script type="text/javascript">
            function display() {
                document.getElementById('MainContent_errmsg').innerHTML = "";
            }

            function OnCheckBoxCheckChanged(evt) {

                var src = window.event != window.undefined ? window.event.srcElement : evt.target;
                var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
                if (isChkBoxClick) {
                    var parentTable = GetParentByTagName("table", src);
                    var nxtSibling = parentTable.nextSibling;
                    if (nxtSibling && nxtSibling.nodeType == 1) {
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
        <%-- </asp:content>--%>
        <%--<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">--%>
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <center>
                <div>
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: #008000">Finance MIS Report</span></div>
                    </center>
                </div>
                <div class="maindivstyle" style="width: 960px; height: auto;">
                    <br />
                    <div>
                        <center>
                            <table id="Table1" class="maintablestyle" runat="server">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblstr" runat="server" Text="Stream"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddltype" runat="server" AutoPostBack="true" CssClass="textbox1 ddlheight1"
                                            OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Batch
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtbatch" runat="server" CssClass="textbox textbox1" ReadOnly="true">---Select---</asp:TextBox>
                                                <asp:Panel ID="pbatch" runat="server" Height="250px" Width="150px" CssClass="multxtpanel">
                                                    <asp:CheckBox ID="chkbatch" runat="server" OnCheckedChanged="chkbatch_CheckedChanged"
                                                        Text="Select All" AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="chklstbatch" runat="server" OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtbatch"
                                                    PopupControlID="pbatch" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtdegree" runat="server" CssClass="textbox textbox1" ReadOnly="true">---Select---</asp:TextBox>
                                                <asp:Panel ID="pdegree1" runat="server" Height="250px" Width="150px" CssClass="multxtpanel">
                                                    <asp:CheckBox ID="chkdegree" runat="server" OnCheckedChanged="chkdegree_CheckedChanged"
                                                        Text="Select All" AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="chklstdegree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtdegree"
                                                    PopupControlID="pdegree1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldept" runat="server" Text="Branch"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtbranch" runat="server" CssClass="textbox textbox1" ReadOnly="true">---Select---</asp:TextBox>
                                                <asp:Panel ID="pbranch" runat="server" Height="200px" Width="200px" CssClass="multxtpanel">
                                                    <asp:CheckBox ID="chkbranch" runat="server" OnCheckedChanged="chkbranch_CheckedChanged"
                                                        Text="Select All" AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="chklstbranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtbranch"
                                                    PopupControlID="pbranch" Position="Bottom">
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
                                                <asp:Panel ID="panel_sem" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                    height: 202px;">
                                                    <asp:CheckBox ID="cb_sem" runat="server" Width="124px" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cb_sem_OnCheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtendergt" runat="server" TargetControlID="txt_sem"
                                                    PopupControlID="panel_sem" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        Type
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlacctype" runat="server" CssClass="textbox1 ddlheight2" OnSelectedIndexChanged="ddlacctype_change"
                                            AutoPostBack="true">
                                            <asp:ListItem>Group Header</asp:ListItem>
                                            <asp:ListItem>Header</asp:ListItem>
                                            <asp:ListItem>Ledger</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="Label1" Text="Header"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtaccheader" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                                    Enabled="false">---Select---</asp:TextBox>
                                                <asp:Panel ID="paccheader" runat="server" Height="250px" Width="250px" CssClass="multxtpanel">
                                                    <asp:CheckBox ID="chkaccheader" runat="server" OnCheckedChanged="chkaccheader_CheckedChanged"
                                                        Text="Select All" AutoPostBack="true" />
                                                    <asp:CheckBoxList ID="chklstaccheader" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklstaccheader_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                    <asp:TreeView ID="treeview_spreadfields" runat="server" SelectedNodeStyle-ForeColor="Red"
                                                        Height="258px" Width="343px" ForeColor="Black" ShowCheckBoxes="All">
                                                    </asp:TreeView>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtaccheader"
                                                    PopupControlID="paccheader" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_type" runat="server" Text="Student Type"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_type" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                                    ReadOnly="true" Width="100px" Enabled="false">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel6" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                    height: auto;">
                                                    <asp:CheckBox ID="cb_type" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_type_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_type" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_type_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_type"
                                                    PopupControlID="Panel6" Position="Bottom">
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
                                                    Finance Year
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upfinyear" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtfyear" runat="server" CssClass="textbox textbox1" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="Pfyear" runat="server" CssClass="multxtpanel" Height="200px">
                                                                <asp:CheckBox ID="chkfyear" runat="server" Text="Select All" OnCheckedChanged="chkfyear_changed"
                                                                    AutoPostBack="True" />
                                                                <asp:CheckBoxList ID="chklsfyear" Font-Names="Book Antiqua" runat="server" OnSelectedIndexChanged="chklsfyear_selected"
                                                                    AutoPostBack="True">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtfyear"
                                                                PopupControlID="Pfyear" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td colspan="2">
                                                    <asp:CheckBox ID="checkdicon" runat="server" Text="Student Catagory" AutoPostBack="true"
                                                        OnCheckedChanged="checkdicon_Changed" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        Style="width: 200px;" />
                                                </td>
                                                <td colspan="2">
                                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtinclude" Enabled="false" Font-Size="Medium" Font-Names="Book Antiqua"
                                                                Style="height: 20px; width: 164px;" CssClass="Dropdown_Txt_Box" runat="server"
                                                                ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                                            <asp:Panel ID="pnlinclude" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                                Width="172px">
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
                                                    <asp:CheckBox ID="cbdue" runat="server" Text=" With Due" Checked="false" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="cbCurSem" runat="server" Text="Current Semester" Checked="false" />
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lnkSetting" runat="server" Text="Setting" OnClick="lnkSetting_Click"></asp:LinkButton>
                                                </td>
                                                <td colspan="2">
                                                    <asp:CheckBox ID="chkinclude" runat="server" Text=" Include Discontinue" Checked="true"
                                                        Visible="false" Enabled="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <div id="divdatewise" runat="server">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="cbdate" runat="server" /><%--onclick="return cbdateChange();"--%>
                                                    </td>
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
                                    <td>
                                        <asp:Button ID="btnGo" runat="server" OnClick="btnGo_Click" Text="Go" CssClass="textbox textbox1 btn1" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                    <br />
                    <asp:Label ID="errmsg" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                    <br />
                    <center>
                        <FarPoint:FpSpread ID="FpSpread1" runat="server" HorizontalScrollBarPolicy="Never"
                            VerticalScrollBarPolicy="Never" OnCellClick="Cell_Click" OnPreRender="Fpspread1_render" >
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1"  Visible="true" SelectionBackColor="#F7BE81" SelectionPolicy="Single">
                                </FarPoint:SheetView>
                            </Sheets>
                            <TitleInfo Font-Size="X-Large" ForeColor="" HorizontalAlign="Center" VerticalAlign="NotSet">
                            </TitleInfo>
                        </FarPoint:FpSpread>
                    </center>
                    <br />
                    <center>
                        <div>
                            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" CssClass="textbox textbox1" Height="20px"
                                Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" onkeypress="display()"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnxl" runat="server" Width="127px" Height="32px" CssClass="textbox textbox1"
                                Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                OnClick="btnxl_Click" />
                            <asp:Button ID="btnprintmaster" runat="server" CssClass="textbox textbox1" Height="32px"
                                Width="60px" Text="Print" OnClick="btnprintmaster_Click" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="true" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                    </center>
                    <br />
                    <center>
                        <div id="poppergroup" runat="server" visible="false" class="popupstyle popupheight1 ">
                            <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                                width: 30px; position: absolute; margin-top: 9px; margin-left: 460px;" OnClick="imagebtnpopclose5_Click" />
                            <br />
                            <center>
                                <div style="background-color: White; height: 550px; width: 950px; border: 5px solid #0CA6CA;
                                    border-top: 30px solid #0CA6CA; border-radius: 10px; overflow: auto;">
                                    <br />
                                    <table style="width: 750px;">
                                        <tr>
                                            <td>
                                                <span>Batch :</span> &nbsp;&nbsp;&nbsp; <span id="batchSpan" runat="server"></span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbldepts" runat="Server" Text="Department"></asp:Label>
                                                &nbsp;&nbsp;&nbsp; <span id="DepartmentSpan" runat="server"></span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblsems" runat="Server" Text="Semester"></asp:Label>
                                                &nbsp;&nbsp;&nbsp; <span id="SemesterSpan" runat="server"></span>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <center>
                                        <FarPoint:FpSpread ID="FpSpread2" runat="server" HorizontalScrollBarPolicy="Never"
                                            VerticalScrollBarPolicy="Never">
                                            <Sheets>
                                                <FarPoint:SheetView AutoPostBack="true" SheetName="Sheet1" BackColor="White" Visible="true">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                            <TitleInfo Font-Size="X-Large" ForeColor="" HorizontalAlign="Center" VerticalAlign="NotSet">
                                            </TitleInfo>
                                        </FarPoint:FpSpread>
                                    </center>
                                    <br />
                                    <br />
                                    <center>
                                        <div>
                                            <asp:Label ID="Label2" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                            <asp:Label ID="lblrpt" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Report Name"></asp:Label>
                                            <asp:TextBox ID="txtsndrpt" runat="server" CssClass="textbox textbox1" Height="20px"
                                                Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" onkeypress="display()"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtsndrpt"
                                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:Button ID="btnsndexcel" runat="server" Width="127px" Height="32px" CssClass="textbox textbox1"
                                                Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                OnClick="btnsndexcel_Click" />
                                            <asp:Button ID="btnsncprint" runat="server" CssClass="textbox textbox1" Height="32px"
                                                Width="60px" Text="Print" OnClick="btnsncprint_Click" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Font-Bold="true" />
                                            <Insproplus:printmaster runat="server" ID="Printmaster1" Visible="false" />
                                        </div>
                                    </center>
                                </div>
                            </center>
                        </div>
                    </center>
                    <center>
                        <div id="divSetting" runat="server" visible="false" class="popupstyle popupheight1 ">
                            <asp:ImageButton ID="imgSetting" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                                width: 30px; position: absolute; margin-top: 9px; margin-left: 112px;" OnClick="imgSetting_Click" />
                            <br />
                            <center>
                                <div style="background-color: White; height: 363px; width: 257px; border: 5px solid #0CA6CA;
                                    border-top: 30px solid #0CA6CA; border-radius: 10px; overflow: auto;">
                                    <br />
                                    <span style="color: Green; font-size: large; font-weight: bold;">Feecategory Settings</span>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnAddRow" runat="server" Text="Add New" OnClick="btnAddRow_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gdSetting" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                                    OnDataBound="gdSetting_OnDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sno" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:Label ID="lblsno" runat="server" Text='<%#Eval("Sno") %>'></asp:Label>
                                                                </center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Batch" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:DropDownList ID="ddlBatch" runat="server" CssClass="textbox1 ddlheight1">
                                                                    </asp:DropDownList>
                                                                </center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Feecategory" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:DropDownList ID="ddlFeecat" runat="server" CssClass="textbox1 ddlheight1">
                                                                    </asp:DropDownList>
                                                                </center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnRowOK" runat="server" Text="OK" OnClick="btnRowOK_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
        </div>
        <%--</asp:Content> --%>
</asp:Content>
