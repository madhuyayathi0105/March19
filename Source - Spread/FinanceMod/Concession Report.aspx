<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Concession Report.aspx.cs" Inherits="Concession_Report"
    EnableEventValidation="false" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
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
            width: 980px;
            height: 25px;
            left: 15px;
        }
        .mainbatch
        {
            background-color: #3AAB97;
            width: 980px;
            position: absolute;
            height: 140px;
            top: 105px;
            left: 15px;
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
        <center>
            <asp:Label ID="lbl_head" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                ForeColor="white" Font-Size="Large" Text="Concession Fee Report"></asp:Label>
        </center>
    </div>
    <body>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class="mainbatch">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblcollege" Font-Bold="true" Style="height: 60px;" Font-Size="Medium"
                                    ForeColor="white" Font-Names="Book Antiqua" runat="server" Text="College"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua" Width="154px" AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                                    Style="height: 25px;">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbltype" Font-Bold="true" Style="height: 60px;" Font-Size="Medium"
                                    ForeColor="white" Font-Names="Book Antiqua" runat="server" Text="Type"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txttype" CssClass="Dropdown_Txt_Box" Enabled="false" Font-Size="Medium"
                                    Font-Names="Book Antiqua" Font-Bold="true" Style="right: 250px;" Width="100px"
                                    runat="server" ReadOnly="true">--Select--</asp:TextBox>
                                <asp:Panel ID="PType" runat="server" Font-Names="Book Antiqua" Font-Bold="true" CssClass="multxtpanel"
                                    Width="114px" Font-Size="Medium">
                                    <asp:CheckBox ID="chktype" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                        AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="chktype_batchchanged" />
                                    <asp:CheckBoxList ID="chklstype" Font-Bold="true" Font-Size="Medium" runat="server"
                                        AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstype_batchselected">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txttype"
                                    PopupControlID="PType" Position="Bottom">
                                </asp:PopupControlExtender>
                            </td>
                            <td>
                                <asp:Label ID="Iblbatch" Font-Bold="true" Style="height: 60px;" Font-Size="Medium"
                                    ForeColor="white" Font-Names="Book Antiqua" runat="server" Text="Batch"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_batch" CssClass="Dropdown_Txt_Box" Font-Size="Medium" Font-Names="Book Antiqua"
                                    Font-Bold="true" Width="100px" runat="server" ReadOnly="true">--Select--</asp:TextBox>
                                <asp:Panel ID="pbatch" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                    CssClass="multxtpanel" Width="114px" Font-Size="Medium">
                                    <asp:CheckBox ID="Chk_batch" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                        AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="Chlk_batchchanged" />
                                    <asp:CheckBoxList ID="Chklst_batch" Font-Bold="true" Font-Size="Medium" runat="server"
                                        AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="Chlk_batchselected">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="popupbatch" runat="server" TargetControlID="txt_batch"
                                    PopupControlID="pbatch" Position="Bottom">
                                </asp:PopupControlExtender>
                            </td>
                            <td>
                                <asp:Label ID="Ibldegree" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                    ForeColor="white" Font-Size="Medium" Text="Degree"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_degree" CssClass="Dropdown_Txt_Box" Font-Names="Book Antiqua"
                                    Font-Bold="true" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                <asp:Panel ID="pdegree" runat="server" CssClass="multxtpanel" Width="128px" Font-Bold="true"
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
                            </td>
                            <td>
                                <asp:Label ID="Iblbranch" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                    ForeColor="white" Font-Size="Medium" Text="Branch"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_branch" CssClass="Dropdown_Txt_Box" Font-Bold="true" Font-Names="Book Antiqua"
                                    runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Width="400px">
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
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblfyear" Text="Finance Year" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="White" Style="width: 100px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtfyear" CssClass="Dropdown_Txt_Box" Font-Bold="true" Font-Names="Book Antiqua"
                                    runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                <asp:Panel ID="Pfyear" runat="server" CssClass="multxtpanel">
                                    <asp:CheckBox ID="chkfyear" runat="server" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                        Text="Select All" OnCheckedChanged="chkfyear_changed" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklsfyear" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                                        runat="server" OnSelectedIndexChanged="chklsfyear_selected" AutoPostBack="True">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txtfyear"
                                    PopupControlID="Pfyear" Position="Bottom">
                                </asp:PopupControlExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblheader" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                    ForeColor="white" Font-Size="Medium" Text="Header"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtHeader" CssClass="Dropdown_Txt_Box" Font-Bold="true" Font-Names="Book Antiqua"
                                    runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                <asp:Panel ID="Pheader" runat="server" CssClass="multxtpanel">
                                    <asp:CheckBox ID="chkheader" runat="server" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                        Text="Select All" OnCheckedChanged="chkheader_changed" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklsheader" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                                        runat="server" OnSelectedIndexChanged="chklsheader_selected" AutoPostBack="True">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtHeader"
                                    PopupControlID="Pheader" Position="Bottom">
                                </asp:PopupControlExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblledger" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                    ForeColor="white" Font-Size="Medium" Text="Ledger"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtledger" CssClass="Dropdown_Txt_Box" Font-Bold="true" Font-Names="Book Antiqua"
                                    runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                <asp:Panel ID="pledgre" runat="server" CssClass="multxtpanel">
                                    <asp:TreeView ID="treeview_spreadfields" runat="server" SelectedNodeStyle-ForeColor="Red"
                                        HoverNodeStyle-BackColor="Black" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                                        ForeColor="Black" ShowCheckBoxes="All">
                                    </asp:TreeView>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtledger"
                                    PopupControlID="pledgre" Position="Bottom">
                                </asp:PopupControlExtender>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblcategory" Text="Fee Category" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="White" Style="width: 105px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtcetgory" CssClass="Dropdown_Txt_Box" Font-Bold="true" Font-Names="Book Antiqua"
                                    runat="server" ReadOnly="true" Width="105px">--Select--</asp:TextBox>
                                <asp:Panel ID="Pcategory" runat="server" CssClass="multxtpanel">
                                    <asp:CheckBox ID="chkcatefory" runat="server" Font-Bold="true" Font-Size="Medium"
                                        Font-Names="Book Antiqua" Text="Select All" OnCheckedChanged="chkcatefory_changed"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklscategory" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                                        runat="server" OnSelectedIndexChanged="chklscategory_selected" AutoPostBack="True">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtcetgory"
                                    PopupControlID="Pcategory" Position="Bottom">
                                </asp:PopupControlExtender>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblconscession" Text="Concession" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="White" Style="width: 90px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtcons" CssClass="Dropdown_Txt_Box" Font-Bold="true" Font-Names="Book Antiqua"
                                    runat="server" ReadOnly="true" Width="122px">--Select--</asp:TextBox>
                                <asp:Panel ID="Pconcession" runat="server" CssClass="multxtpanel">
                                    <asp:CheckBox ID="chkconcession" runat="server" Font-Bold="true" Font-Size="Medium"
                                        Font-Names="Book Antiqua" Text="Select All" OnCheckedChanged="chkconcession_changed"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklsconcession" Font-Names="Book Antiqua" Font-Bold="true"
                                        Font-Size="Medium" runat="server" OnSelectedIndexChanged="chklsconcession_selected"
                                        AutoPostBack="True">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtcons"
                                    PopupControlID="Pconcession" Position="Bottom">
                                </asp:PopupControlExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:RadioButton ID="rbconsolidate" runat="server" Text="Consolidate" GroupName="Report"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" ForeColor="White"
                                    AutoPostBack="true" OnCheckedChanged="rbconsolidate_checkdchange" />
                                <asp:RadioButton ID="rbdetailed" runat="server" Text="Detailed" GroupName="Report"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" ForeColor="White"
                                    AutoPostBack="true" OnCheckedChanged="rbdetailed_checkdchange" />
                                <asp:RadioButton ID="rbledger" runat="server" Visible="false" Text="LedgerWise" GroupName="Report"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" ForeColor="White"
                                    AutoPostBack="true" OnCheckedChanged="rbledger_checkdchange" />
                            </td>
                            <td colspan="3" id="fldPayType" runat="server" visible="false">
                                <fieldset style="width: 261px; height: 21px;">
                                    <asp:RadioButtonList ID="rblPayType" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" OnSelectedIndexChanged="rblPayType_Selected" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="White">
                                        <asp:ListItem Text="Paid" Value="0" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Yet To Be Paid" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Both" Value="2"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </fieldset>
                            </td>
                            <td colspan="3">
                                <asp:CheckBox ID="cbStaff" runat="server" Text="Include Staff" Enabled="false" AutoPostBack="true"
                                    OnCheckedChanged="cbStaff_changed" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="White" />
                                <asp:Button ID="btn_staffLook" runat="server" CssClass="textbox btn1 textbox1" Enabled="false"
                                    Text="?" OnClick="btn_staffLook_Click" />
                                <asp:Button ID="btngo" runat="server" Font-Names="Book Antiqua" Text="Go" OnClick="btngo_Click"
                                    Font-Size="Medium" Font-Bold="true" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Label ID="lbldisp" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="White"></asp:Label>
                                <asp:Label ID="lblval" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <br />
                <br />
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
                <%--Staff Lookup --%>
                <center>
                    <div id="div_staffLook" runat="server" visible="false" class="popupstyle popupheight1 ">
                        <asp:ImageButton ID="ImageButton5" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 30px; margin-left: 310px;"
                            OnClick="btn_exitstaff_Click" />
                        <br />
                        <br />
                        <div style="background-color: White; height: 400px; width: 650px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <br />
                            <center>
                                <div>
                                    <span class="fontstyleheader" style="color: Green;">Select The Staff</span></div>
                            </center>
                            <br />
                            <table class="maintablestyle">
                                <tr>
                                    <td>
                                        <span class="challanLabel">
                                            <p>
                                                Search By</p>
                                        </span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsearch1" runat="server" CssClass="textbox1 ddlheight3" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlsearch1_OnSelectedIndexChanged">
                                            <asp:ListItem Text="Search By Name" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Search By Code" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtsearch1" runat="server" Visible="false" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch1"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <asp:TextBox ID="txtsearch1c" runat="server" Visible="false" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetStaffno" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch1c"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_go2Staff" runat="server" CssClass="textbox btn1 textbox1" Text="Go"
                                            OnClick="btn_go2Staff_Click" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <div>
                                <asp:Label ID="lbl_errormsgstaff" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </div>
                            <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" ShowHeaderSelection="false"
                                BorderWidth="0px" Style="width: 620px; height: 230px; auto; border: 0px solid #999999;
                                border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                OnButtonCommand="Fpspread2staff_Command">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#F7BE81" SelectionPolicy="Single">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <br />
                            <center>
                                <div>
                                    <asp:Button ID="btn_staffOK" runat="server" CssClass="textbox btn2 textbox1" Text="Ok"
                                        OnClick="btn_staffOK_Click" />
                                    <asp:Button ID="btn_exitstaff" runat="server" CssClass="textbox btn2 textbox1" Text="Exit"
                                        OnClick="btn_exitstaff_Click" />
                                </div>
                            </center>
                        </div>
                    </div>
                </center>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnmasterprint" />
                <asp:PostBackTrigger ControlID="btnxl" />
                <asp:PostBackTrigger ControlID="btngo" />
            </Triggers>
        </asp:UpdatePanel>
    </body>
</asp:Content>
