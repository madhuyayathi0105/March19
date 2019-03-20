<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="FinanceYearWiseHeaderReport.aspx.cs" Inherits="FinanceYearWiseHeaderReport"
    EnableEventValidation="false" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../Styles/css/Style.css" rel="Stylesheet" type="text/css" />
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
    <style type="text/css">
        .head
        {
            background-color: Teal;
            font-family: Book Antiqua;
            font-size: medium;
            color: black;
            top: 80px;
            position: absolute;
            font-weight: bold;
            width: 1019px;
            height: 25px;
            left: 5px;
        }
        .mainbatch
        {
            background-color: #3AAB97;
            width: 1019px;
            position: absolute;
            height: 90px;
            top: 105px;
            left: 5px;
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
            color: black;
        }
        .cpBody
        {
            background-color: #DCE4F9;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="head">
        <asp:Label ID="lbl_head" runat="server" Font-Bold="true" Style="left: 340px; top: 0px;
            position: absolute;" Font-Names="Book Antiqua" ForeColor="white" Font-Size="Large"
            Text="Finance Year Wise Header Report"></asp:Label>
    </div>
    <body>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class="mainbatch">
                    <asp:Label ID="lblcollege" Font-Bold="true" Style="position: absolute; left: 15px;
                        top: 11px; height: 60px;" Font-Size="Medium" ForeColor="white" Font-Names="Book Antiqua"
                        runat="server" Text="College"></asp:Label>
                    <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="true" Font-Size="Medium"
                        Font-Names="Book Antiqua" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                        Style="position: absolute; left: 75px; top: 11px; height: 25px;">
                    </asp:DropDownList>
                    <asp:Label ID="lbltype" Font-Bold="true" Style="position: absolute; left: 285px;
                        top: 11px; height: 60px;" Font-Size="Medium" ForeColor="white" Font-Names="Book Antiqua"
                        runat="server" Text="Type"></asp:Label>
                    <asp:TextBox ID="txttype" CssClass="Dropdown_Txt_Box" Enabled="false" Font-Size="Medium"
                        Font-Names="Book Antiqua" Font-Bold="true" Style="position: absolute; left: 340px;
                        top: 11px; right: 250px;" Width="100px" runat="server" ReadOnly="true">--Select--</asp:TextBox>
                    <asp:Panel ID="PType" runat="server" Font-Names="Book Antiqua" Font-Bold="true" CssClass="multxtpanel"
                        Width="114px" style="height:atuo;" Font-Size="Medium">
                        <asp:CheckBox ID="chktype" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                            AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="chktype_batchchanged" />
                        <asp:CheckBoxList ID="chklstype" Font-Bold="true" Font-Size="Medium" runat="server"
                            AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstype_batchselected">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txttype"
                        PopupControlID="PType" Position="Bottom">
                    </asp:PopupControlExtender>
                    <asp:Label ID="Iblbatch" Font-Bold="true" Style="position: absolute; left: 445px;
                        top: 11px; height: 60px;" Font-Size="Medium" ForeColor="white" Font-Names="Book Antiqua"
                        runat="server" Text="Batch"></asp:Label>
                    <asp:TextBox ID="txt_batch" CssClass="Dropdown_Txt_Box" Font-Size="Medium" Font-Names="Book Antiqua"
                        Font-Bold="true" Style="position: absolute; left: 500px; top: 11px; right: 250px;"
                        Width="100px" runat="server" ReadOnly="true">--Select--</asp:TextBox>
                    <asp:Panel ID="pbatch" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                        CssClass="multxtpanel" Width="114px" style="height:atuo;" Font-Size="Medium">
                        <asp:CheckBox ID="Chk_batch" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                            AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="Chlk_batchchanged" />
                        <asp:CheckBoxList ID="Chklst_batch" Font-Bold="true" Font-Size="Medium" runat="server"
                            AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="Chlk_batchselected">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="popupbatch" runat="server" TargetControlID="txt_batch"
                        PopupControlID="pbatch" Position="Bottom">
                    </asp:PopupControlExtender>
                    <asp:Label ID="Ibldegree" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                        ForeColor="white" Font-Size="Medium" Style="position: absolute; left: 606px;
                        top: 11px;" Text="Degree"></asp:Label>
                    <asp:TextBox ID="txt_degree" CssClass="Dropdown_Txt_Box" Font-Names="Book Antiqua"
                        Font-Bold="true" Style="left: 691px; position: absolute; top: 11px;" runat="server"
                        ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                    <asp:Panel ID="pdegree" runat="server" CssClass="multxtpanel" Width="128px" style="height:atuo;" Font-Bold="true"
                        Font-Size="Medium">
                        <asp:CheckBox ID="chk_degree" Font-Bold="true" runat="server" Font-Size="Medium"
                            Text="Select All" AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="checkDegree_CheckedChanged" />
                        <asp:CheckBoxList ID="Chklst_degree" Font-Bold="true" Font-Size="Medium" runat="server"
                            AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cheklist_Degree_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="popupdegree" runat="server" TargetControlID="txt_degree"
                        PopupControlID="pdegree" Position="Bottom">
                    </asp:PopupControlExtender>
                    <asp:Label ID="Iblbranch" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                        ForeColor="white" Font-Size="Medium" Style="position: absolute; left: 797px;
                        top: 11px;" Text="Branch"></asp:Label>
                    <asp:TextBox ID="txt_branch" CssClass="Dropdown_Txt_Box" Font-Bold="true" Font-Names="Book Antiqua"
                        Style="position: absolute; left: 865px; top: 11px;" runat="server" ReadOnly="true"
                        Width="100px">--Select--</asp:TextBox>
                    <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Width="400px" style="height:atuo;">
                        <asp:CheckBox ID="chk_branch" runat="server" Font-Bold="true" Font-Size="Medium"
                            Font-Names="Book Antiqua" Text="Select All" OnCheckedChanged="chk_branchchanged"
                            AutoPostBack="True" />
                        <asp:CheckBoxList ID="chklst_branch" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                            runat="server" OnSelectedIndexChanged="chklst_branchselected" AutoPostBack="True">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="popupbranch" runat="server" TargetControlID="txt_branch"
                        PopupControlID="Panel3" Position="Bottom">
                    </asp:PopupControlExtender>
                    <asp:Label runat="server" ID="lblfyear" Text="Finance Year" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="White" Style="position: absolute; top: 50px; left: 11px;
                        width: 100px"></asp:Label>
                    <asp:TextBox ID="txtfyear" CssClass="Dropdown_Txt_Box" Font-Bold="true" Font-Names="Book Antiqua"
                        Style="position: absolute; left: 107px; top: 49px;" runat="server" ReadOnly="true"
                        Width="133px">--Select--</asp:TextBox>
                    <asp:Panel ID="Pfyear" runat="server" CssClass="multxtpanel" style="height:atuo;">
                        <asp:CheckBox ID="chkfyear" runat="server" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                            Text="Select All" OnCheckedChanged="chkfyear_changed" AutoPostBack="True" />
                        <asp:CheckBoxList ID="chklsfyear" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                            runat="server" OnSelectedIndexChanged="chklsfyear_selected" AutoPostBack="True">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtfyear"
                        PopupControlID="Pfyear" Position="Bottom">
                    </asp:PopupControlExtender>
                    <asp:Label ID="lblheader" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                        ForeColor="white" Font-Size="Medium" Style="position: absolute; left: 255px;
                        top: 50px;" Text="Header"></asp:Label>
                    <asp:TextBox ID="txtHeader" CssClass="Dropdown_Txt_Box" Font-Bold="true" Font-Names="Book Antiqua"
                        Style="position: absolute; left: 312px; top: 49px;" runat="server" ReadOnly="true"
                        Width="100px">--Select--</asp:TextBox>
                    <asp:Panel ID="Pheader" runat="server" CssClass="multxtpanel" Width="300px" style="height:atuo;">
                        <asp:CheckBox ID="chkheader" runat="server" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                            Text="Select All" OnCheckedChanged="chkheader_changed" AutoPostBack="True" />
                        <asp:CheckBoxList ID="chklsheader" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                            runat="server" OnSelectedIndexChanged="chklsheader_selected" AutoPostBack="True">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtHeader"
                        PopupControlID="Pheader" Position="Bottom">
                    </asp:PopupControlExtender>
                    <asp:Label ID="lblledger" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                        ForeColor="white" Font-Size="Medium" Style="position: absolute; left: 423px;
                        top: 49px;" Text="Ledger"></asp:Label>
                    <asp:TextBox ID="txtledger" CssClass="Dropdown_Txt_Box" Font-Bold="true" Font-Names="Book Antiqua"
                        Style="position: absolute; left: 480px; top: 50px;" runat="server" ReadOnly="true"
                        Width="100px">--Select--</asp:TextBox>
                    <asp:Panel ID="pledgre" runat="server" CssClass="multxtpanel" style="height:atuo;">
                        <asp:TreeView ID="treeview_spreadfields" runat="server" SelectedNodeStyle-ForeColor="Red"
                            HoverNodeStyle-BackColor="Black" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                            ForeColor="Black" ShowCheckBoxes="All"  Width="450px">
                        </asp:TreeView>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtledger"
                        PopupControlID="pledgre" Position="Bottom">
                    </asp:PopupControlExtender>
                    <asp:Label runat="server" ID="lblcategory" Text="Category" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="White" Style="position: absolute; top: 50px; left: 589px;
                        width: 105px"></asp:Label>
                    <asp:TextBox ID="txtcetgory" CssClass="Dropdown_Txt_Box" Font-Bold="true" Font-Names="Book Antiqua"
                        Style="position: absolute; left: 662px; top: 49px;" runat="server" ReadOnly="true"
                        Width="105px">--Select--</asp:TextBox>
                    <asp:Panel ID="Pcategory" runat="server" CssClass="multxtpanel" Width="150px" style="height:atuo;">
                        <asp:CheckBox ID="chkcatefory" runat="server" Font-Bold="true" Font-Size="Medium"
                            Font-Names="Book Antiqua" Text="Select All" OnCheckedChanged="chkcatefory_changed"
                            AutoPostBack="True" />
                        <asp:CheckBoxList ID="chklscategory" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                            runat="server" OnSelectedIndexChanged="chklscategory_selected" AutoPostBack="True">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtcetgory"
                        PopupControlID="Pcategory" Position="Bottom">
                    </asp:PopupControlExtender>
                    <asp:Label ID="lbl_seat" runat="server" Text="Seat Type" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="White" Style="position: absolute; top: 50px; left: 773px;
                        width: 105px"></asp:Label>
                    <asp:TextBox ID="txt_seat" runat="server" CssClass="Dropdown_Txt_Box" Font-Bold="true"
                        Font-Names="Book Antiqua" Style="position: absolute; left: 857px; top: 49px;"
                        runat="server" ReadOnly="true" Width="105px">--Select--</asp:TextBox>
                    <asp:Panel ID="Panel5" runat="server" CssClass="multxtpanel" Style="width: 191px;
                        height: auto;">
                        <asp:CheckBox ID="cb_seat" runat="server" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                            Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_seat_CheckedChanged" />
                        <asp:CheckBoxList ID="cbl_seat" runat="server" Font-Bold="true" Font-Size="Medium"
                            Font-Names="Book Antiqua" AutoPostBack="True" OnSelectedIndexChanged="cbl_seat_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_seat"
                        PopupControlID="Panel5" Position="Bottom">
                    </asp:PopupControlExtender>
                    <asp:Button ID="btngo" runat="server" Font-Names="Book Antiqua" Text="Go" OnClick="btngo_Click"
                        Font-Size="Medium" Font-Bold="true" Style="top: 48px; left: 970px; position: absolute;" />
                </div>
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <asp:Label ID="errmsg" runat="server" Font-Names="Book Antiqua" Text="Go" Font-Size="Medium"
                    Font-Bold="true" ForeColor="Red"></asp:Label>
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
                <asp:FilteredTextBoxExtender ID="Filterspace" runat="server" TargetControlID="txtexcelname"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(){}[]. ">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btnxl_Click" />
                <asp:Button ID="btnmasterprint" runat="server" Text="Print" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btnmasterprint_Click" />
                <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnmasterprint" />
                <asp:PostBackTrigger ControlID="btnxl" />
                <asp:PostBackTrigger ControlID="btngo" />
            </Triggers>
        </asp:UpdatePanel>
    </body>
</asp:Content>
