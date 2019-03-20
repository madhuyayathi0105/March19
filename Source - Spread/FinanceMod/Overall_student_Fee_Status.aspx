<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Overall_student_Fee_Status.aspx.cs" Inherits="Overall_student_Fee_Status" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
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
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="20px"
            Width="966px">
            <%-- style="top: 71px; left: 0px; position: absolute; width: 960px"--%>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label4" runat="server" Text="Course Wise Student's Fee List" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="White"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </asp:Panel>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <%--<asp:Panel ID="pnl" runat="server" Width="1007px" Height="147px">--%>
        <table class="maintablestyle">
            <tr align="left">
                <td colspan="10">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_collegename" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_collegename" runat="server" OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged"
                                    AutoPostBack="true" Style="height: 24px; width: 144px;">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbltype" Text="Type" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddltype" runat="server" Width="145px" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblbatch" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtbatch" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                            Width="128px" Style="font-size: medium; font-weight: bold; height: 20px; font-family: 'Book Antiqua';">---Select---</asp:TextBox>
                                        <asp:Panel ID="pbatch" runat="server" style="height:auto;" Width="130px" CssClass="multxtpanel multxtpanleheight">
                                            <asp:CheckBox ID="chkbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnCheckedChanged="chkbatch_CheckedChanged" Text="Select All"
                                                AutoPostBack="True" />
                                            <asp:CheckBoxList ID="chklstbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtbatch"
                                            PopupControlID="pbatch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbldegree" Text="Degree" Width="94px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Style="font-family: Book Antiqua;
                                    font-size: medium; font-weight: bold;"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtdegree" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                            Width="128px" Style="font-size: medium; font-weight: bold; font-family: 'Book Antiqua';
                                            height: 20px;">---Select---</asp:TextBox>
                                        <asp:Panel ID="pdegree1" runat="server" style="height:auto;"  Width="130px" CssClass="multxtpanel multxtpanleheight">
                                            <asp:CheckBox ID="chkdegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnCheckedChanged="chkdegree_CheckedChanged" Text="Select All"
                                                AutoPostBack="True" />
                                            <asp:CheckBoxList ID="chklstdegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtdegree"
                                            PopupControlID="pdegree1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="10">
                    <table>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblbranch" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtbranch" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                            Width="139px" Style="font-size: medium; font-weight: bold; font-family: 'Book Antiqua';
                                            height: 20px;">---Select---</asp:TextBox>
                                        <asp:Panel ID="pbranch" runat="server" style="height:auto;"  Width="250px" CssClass="multxtpanel multxtpanleheight">
                                            <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnCheckedChanged="chkbranch_CheckedChanged" Text="Select All"
                                                AutoPostBack="True" />
                                            <asp:CheckBoxList ID="chklstbranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged"
                                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtbranch"
                                            PopupControlID="pbranch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblstudtypr" Text="Student Type" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                    width: 103px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlstudtype" runat="server" Style="font-family: Book Antiqua;
                                    font-size: medium; font-weight: bold; height: 24px;" OnSelectedIndexChanged="ddlstudtype_change"
                                    AutoPostBack="true" Width="145px">
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
                                <asp:Label runat="server" ID="lblacctype" Text="A/c Type" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                    width: 103px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlacctype" runat="server" Style="font-family: Book Antiqua;
                                    font-size: medium; font-weight: bold; height: 24px;" OnSelectedIndexChanged="ddlacctype_change"
                                    AutoPostBack="true">
                                    <asp:ListItem>---Select---</asp:ListItem>
                                    <asp:ListItem>Group Header</asp:ListItem>
                                    <asp:ListItem>Header</asp:ListItem>
                                    <asp:ListItem>Ledger</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label1" Text="A/c Header" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                    width: 118px;"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtaccheader" runat="server" Height="30px" CssClass="Dropdown_Txt_Box"
                                            ReadOnly="true" Width="127px" Style="font-size: medium; font-weight: bold; height: 20px;
                                            font-family: 'Book Antiqua';" Enabled="false">---Select---</asp:TextBox>
                                        <asp:Panel ID="paccheader" runat="server" style="height:auto;"  Width="250px" CssClass="multxtpanel multxtpanleheight">
                                            <asp:CheckBox ID="chkaccheader" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnCheckedChanged="chkaccheader_CheckedChanged" Text="Select All"
                                                AutoPostBack="True" />
                                            <asp:CheckBoxList ID="chklstaccheader" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                OnSelectedIndexChanged="chklstaccheader_SelectedIndexChanged" Font-Bold="True"
                                                Font-Names="Book Antiqua">
                                            </asp:CheckBoxList>
                                            <asp:TreeView ID="treeview_spreadfields" runat="server" SelectedNodeStyle-ForeColor="Red"
                                                HoverNodeStyle-BackColor="Black"  Width="343px" Font-Names="Book Antiqua"
                                                ForeColor="Black" ShowCheckBoxes="All">
                                            </asp:TreeView>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtaccheader"
                                            PopupControlID="paccheader" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="10">
                    <table>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblfeetype" Text="Fee Type" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="110px" Style="font-family: Book Antiqua; font-size: medium;
                                    font-weight: bold;"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlfeetype" runat="server" Style="font-family: Book Antiqua;
                                    font-size: medium; font-weight: bold; height: 24px;" Enabled="false" OnSelectedIndexChanged="ddlfeetype_change"
                                    AutoPostBack="true" Width="145px">
                                    <asp:ListItem>---Select---</asp:ListItem>
                                    <asp:ListItem>Paid</asp:ListItem>
                                    <asp:ListItem>Yet To Be Paid</asp:ListItem>
                                    <asp:ListItem>Both</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblfeesem" Text="Semester" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                    width: 103px"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtfeesem" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                            Width="128px" Style="font-size: medium; font-weight: bold; font-family: 'Book Antiqua';">---Select---</asp:TextBox>
                                        <asp:Panel ID="pfeesem" runat="server" style="height:auto;" Width="150px" CssClass="multxtpanel multxtpanleheight">
                                            <asp:CheckBox ID="chkfeesem" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnCheckedChanged="chkfeesem_CheckedChanged" Text="Select All"
                                                AutoPostBack="True" />
                                            <asp:CheckBoxList ID="chklsfeesem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chklsfeesem_SelectedIndexChanged"
                                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txtfeesem"
                                            PopupControlID="pfeesem" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td colspan="2">
                                <asp:CheckBox ID="chkdate" runat="server" Text="Date Wise" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="chkdate_CheckedChanged"
                                    Style="width: 103px" />
                            </td>
                            <td colspan="3">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lblfrom" Text="From " Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Style="width: 103px;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtdate" runat="server" Enabled="false" Font-Bold="true" Width="80px"
                                                ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtdate" runat="server"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtdate"
                                    FilterType="Numbers,Custom" ValidChars="/">
                                </asp:FilteredTextBoxExtender>--%>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblto" Text="To" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                                width: 103px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtto" runat="server" Enabled="false" Font-Bold="true" Width="80px"
                                                ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtto" runat="server"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtto"
                                    FilterType="Numbers,Custom" ValidChars="/">
                                </asp:FilteredTextBoxExtender>--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="10">
                    <table>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblpaymode" Text="Pay Mode" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                    width: 103px"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtpaymode" runat="server" Height="30px" CssClass="Dropdown_Txt_Box"
                                            ReadOnly="true" Width="140px" Style="font-size: medium; font-weight: bold; height: 20px;
                                            width: 130px; font-family: 'Book Antiqua';" Enabled="false">---Select---</asp:TextBox>
                                        <asp:Panel ID="Panel4" runat="server"  Width="133px" CssClass="multxtpanel multxtpanleheight">
                                            <asp:CheckBox ID="chkpaymode" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnCheckedChanged="chkpaymode_CheckedChanged" Text="Select All"
                                                AutoPostBack="True" />
                                            <asp:CheckBoxList ID="chklstpaymode" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                OnSelectedIndexChanged="chklstpaymode_SelectedIndexChanged" Font-Bold="True"
                                                Font-Names="Book Antiqua">
                                                <%-- <asp:ListItem>Cash</asp:ListItem>
                                                <asp:ListItem>Cheque</asp:ListItem>
                                                <asp:ListItem>DD</asp:ListItem>
                                                <asp:ListItem>Challan</asp:ListItem>
                                                <asp:ListItem>Online Pay</asp:ListItem>--%>
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txtpaymode"
                                            PopupControlID="Panel4" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblfyear" Text="Finance Year" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="width: 100px"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtfyear" CssClass="Dropdown_Txt_Box" Font-Bold="true" Font-Names="Book Antiqua"
                                            runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                        <asp:Panel ID="Pfyear" runat="server" CssClass="multxtpanel multxtpanleheight" style="height:auto;" Width="200px">
                                            <asp:CheckBox ID="chkfyear" runat="server" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                Text="Select All" OnCheckedChanged="chkfyear_changed" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="chklsfyear" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                                                runat="server" OnSelectedIndexChanged="chklsfyear_selected" AutoPostBack="True">
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
                                    OnCheckedChanged="checkdicon_Changed" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="width: 200px;" />
                                <%-- <asp:CheckBox ID="chkfeeroll" runat="server" Text="Include Fee of the Role" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" Style="width: 200px; margin-left: 10px;
                            margin-top: -3px; position: absolute;" />--%>
                            </td>
                            <td colspan="2">
                                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtinclude" Enabled="false" Font-Bold="true" Font-Size="Medium"
                                            Font-Names="Book Antiqua" Style="height: 20px; width: 164px;" CssClass="Dropdown_Txt_Box"
                                            runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlinclude" runat="server" CssClass="multxtpanel multxtpanleheight"
                                            Width="172px" style="height:auto;">
                                            <asp:CheckBox ID="cbinclude" runat="server" Text="Select All" Font-Bold="true" Font-Size="Medium"
                                                Font-Names="Book Antiqua" OnCheckedChanged="cbinclude_OnCheckedChanged" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cblinclude" runat="server" Font-Bold="true" Font-Size="Medium"
                                                Font-Names="Book Antiqua" OnSelectedIndexChanged="cblinclude_OnSelectedIndexChanged"
                                                AutoPostBack="True">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txtinclude"
                                            PopupControlID="pnlinclude" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td id="tddegree" runat="server" visible="false">
                                <asp:CheckBox ID="checkcoursetot" runat="server" Text="Degree Wise Total" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Visible="false" />
                            </td>
                            <%-- <td colspan="2">
                        <asp:CheckBox ID="chkyettobepaid" runat="server" Text="Date Wise Yet Be Paid" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" />
                    </td>--%>
                            <%--<td>
                        <asp:Button ID="btnGo" runat="server" OnClick="btnGo_Click" Text="Go" Style="font-family: Book Antiqua;
                            font-size: medium; font-weight: 700;" />
                    </td>--%>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="10">
                    <table>
                        <tr>
                            <td colspan="5">
                                <asp:CheckBox ID="chkfeeroll" runat="server" Text="Include Fee of the Role" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Style="width: 200px;" />
                                <asp:CheckBox ID="chkyettobepaid" runat="server" Text="Date Wise Yet Be Paid" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" />
                                <asp:CheckBox ID="cbbfadm" runat="server" Text="Include Before Admission" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbbankcheck" runat="server" Text="Include Bank" AutoPostBack="true"
                                    OnCheckedChanged="cbbankcheck_Changed" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="width: 200px;" />
                            </td>
                            <td colspan="2">
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtbankname" Enabled="false" Font-Bold="true" Font-Size="Medium"
                                            Font-Names="Book Antiqua" Style="height: 20px; width: 164px;" CssClass="Dropdown_Txt_Box"
                                            runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlbank" runat="server" CssClass="multxtpanel multxtpanleheight" Width="172px" style="height:auto;">
                                            <asp:CheckBox ID="cbbank" runat="server" Text="Select All" Font-Bold="true" Font-Size="Medium"
                                                Font-Names="Book Antiqua" OnCheckedChanged="cbbank_OnCheckedChanged" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cblbank" runat="server" Font-Bold="true" Font-Size="Medium"
                                                Font-Names="Book Antiqua" OnSelectedIndexChanged="cblbank_OnSelectedIndexChanged"
                                                AutoPostBack="True">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txtbankname"
                                            PopupControlID="pnlbank" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Button ID="btnGo" runat="server" OnClick="btnGo_Click" Text="Go" Style="font-family: Book Antiqua;
                                    font-size: medium; font-weight: 700;" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:CheckBox ID="cbledgacr" runat="server" Text="Include Ledger Acronym" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" />
                </td>
            </tr>
        </table>
        <%--</asp:Panel>     --%>
        <%--  <asp:Panel ID="Panel1" runat="server" BackImageUrl="~/image/Top Band-2.jpg" Height="16px"
            Style="margin-left: 0px; width: 966px; height: 16px; background-image: url('image/Top%20Band-2.jpg');"
            Width="1088px">
        </asp:Panel>--%>
        <br />
        <table>
            <tr>
                <td>
                    <asp:Label ID="errmsg" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblc2" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                        Text="Cash" BackColor="LightCoral"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblc3" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                        Text="Cheque" BackColor="LightGray"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblc5" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                        Text="DD" BackColor="Orange"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblc1" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                        Text="Challan" BackColor="LightGreen"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblc4" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                        Text="Online Pay" BackColor="LightGoldenrodYellow"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblcard" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Card" BackColor="white"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <FarPoint:FpSpread ID="FpSpread1" runat="server" HorizontalScrollBarPolicy="Never"
            VerticalScrollBarPolicy="Never">
            <CommandBar BackColor="Control" ButtonFaceColor="Control" Visible="true" ButtonHighlightColor="ControlLightLight"
                ButtonShadowColor="ControlDark" ButtonType="PushButton" ShowPDFButton="false">
                <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
            </CommandBar>
            <Sheets>
                <FarPoint:SheetView AutoPostBack="true" SheetName="Sheet1" Visible="true">
                </FarPoint:SheetView>
            </Sheets>
            <TitleInfo Font-Size="X-Large" ForeColor="" HorizontalAlign="Center" VerticalAlign="NotSet">
            </TitleInfo>
        </FarPoint:FpSpread>
        <br />
        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Report Name"></asp:Label>
        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
            Font-Bold="True" onkeypress="display()" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
        </asp:FilteredTextBoxExtender>
        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" OnClick="btnxl_Click" />
        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
    </body>
</asp:Content>
<%--<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
</asp:Content>
--%>
