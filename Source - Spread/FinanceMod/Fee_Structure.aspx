<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Fee_Structure.aspx.cs" Inherits="Fee_Structure" %>

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
        <script type="text/javascript">
            function PrintDiv() {
                var panel = document.getElementById("<%=contentDiv.ClientID %>");
                var printWindow = window.open('', '', 'height=816,width=1000');
                printWindow.document.write('<html><head>');
                printWindow.document.write('<style> .classRegular { font-family:Arial; font-size:9px; } .classBold10 { font-family:Arial; font-size:11px; font-weight:bold;} .classBold12 { font-family:Arial; font-size:13px; font-weight:bold;} .classBold { font-family:Arial; font-size:9px; font-weight:bold;} </style>');
                printWindow.document.write('</head><body >');
                printWindow.document.write(panel.innerHTML);
                printWindow.document.write('</body></html>');
                printWindow.document.close();
                setTimeout(function () {
                    printWindow.print();
                }, 500);
                return false;
            }

            //            function checkValidate() {
            //                var text = document.getElementById('<%=txtexcelname.ClientID %>').value;               
            //                if (text.trim() == "") {
            //                    document.getElementById('<%=lblvalidation1.ClientID %>').value = "Please Enter Your Report Name";
            //                    return false;
            //                }
            //            }
            //            function display() {
            //                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";                
            //            }
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
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <center>
                <div>
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: #008000">Fees Structure</span></div>
                    </center>
                </div>
                <div class="maindivstyle" style="width: 950px; height: auto;">
                    <div>
                        <center>
                            <table class="maintablestyle">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_col" runat="server" CssClass="textbox1 ddlheight4" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddl_col_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_stream" runat="server" Text="Type"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_stream" runat="server" Height="15px" Width="100px" CssClass="textbox  txtheight2"
                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel" Style="height: auto;
                                                    width: 100px;">
                                                    <asp:CheckBox ID="cb_stream" runat="server" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cb_stream_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_stream" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_stream_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txt_stream"
                                                    PopupControlID="pbatch" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        Batch
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_batch" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Style="height: auto;
                                                    width: 112px;">
                                                    <asp:CheckBox ID="cb_batch" runat="server" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cb_batch_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_batch_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_batch"
                                                    PopupControlID="Panel2" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_course" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Style="height: auto;
                                                    width: 112px;">
                                                    <asp:CheckBox ID="cb_course" runat="server" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cb_course_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_course" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_course_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_course"
                                                    PopupControlID="Panel3" Position="Bottom">
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
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_dept" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel" Style="height: auto;">
                                                    <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_dept_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_dept"
                                                    PopupControlID="Panel4" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_sem" runat="server" Text="Semester"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_sem" runat="server" Height="15px" Width="112px" CssClass="textbox  txtheight2"
                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel10" runat="server" CssClass="multxtpanel" Style="height: auto;
                                                    width: 120px;">
                                                    <asp:CheckBox ID="cb_sem" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_sem_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txt_sem"
                                                    PopupControlID="Panel10" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        Section
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="Updp_sect" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_sect" runat="server" CssClass="textbox textbox1 txtheight2"
                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel_sect" runat="server" CssClass="multxtpanel" Style="height: auto;">
                                                    <asp:CheckBox ID="cb_sect" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cb_sect_OnCheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_sect" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sect_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="pcb_sec" runat="server" TargetControlID="txt_sect"
                                                    PopupControlID="panel_sect" Position="Bottom">
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
                                        <asp:RadioButton ID="rb_stud" runat="server" Text="StudentWise" GroupName="studdept"
                                            Checked="true" OnCheckedChanged="rb_stud_Changed" AutoPostBack="true" />
                                        <asp:RadioButton ID="rb_dept" runat="server" Text="DepartmentWise" GroupName="studdept"
                                            OnCheckedChanged="rb_dept_Changed" AutoPostBack="true" />
                                    </td>
                                    <td colspan="3">
                                        <asp:RadioButton ID="rb_hdr" runat="server" Text="Header" OnCheckedChanged="rb_hdr_OnCheckedChanged"
                                            Checked="true" AutoPostBack="true" GroupName="hdrsel" />
                                        <asp:RadioButton ID="rb_ldr" runat="server" Text="Ledger" OnCheckedChanged="rb_ldr_OnCheckedChanged"
                                            AutoPostBack="true" GroupName="hdrsel" />
                                        <asp:RadioButton ID="rb_grphdr" runat="server" Text="Group Header" OnCheckedChanged="rb_grphdr_OnCheckedChanged"
                                            AutoPostBack="true" GroupName="hdrsel" />&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_sel" runat="server" Visible="false" Text=""></asp:Label>
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_grp" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel6" runat="server" CssClass="multxtpanel" Style="height: auto;">
                                                    <asp:CheckBox ID="cb_grp" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_grp_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_grp" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_grp_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                    <asp:TreeView ID="treeledger" runat="server" SelectedNodeStyle-ForeColor="Red" HoverNodeStyle-BackColor="Black"
                                                        Width="450px" Font-Names="Book Antiqua" ForeColor="Black" ShowCheckBoxes="All">
                                                    </asp:TreeView>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_grp"
                                                    PopupControlID="Panel6" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rbl_PayablePaid" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Selected="True">Payable</asp:ListItem>
                                            <asp:ListItem>Paid</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btngo" runat="server" Text="Go" CssClass="textbox textbox1 btn1"
                                            OnClick="btngo_click" />
                                        <asp:Button ID="btnprint" runat="server" Text="Print" CssClass="textbox textbox1 btn2"
                                            BackColor="LightGreen" OnClick="btnprint_click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Format
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlFormat" runat="server" CssClass="textbox textbox1 ddlheight1"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlFormat_Selected">
                                            <asp:ListItem>Format I</asp:ListItem>
                                            <asp:ListItem>Format II</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="4">
                                        <asp:CheckBox ID="cbIncHeader" runat="server" Text="Include Header" />
                                        <asp:CheckBox ID="cbYearWise" runat="server" Text="Yearwise" Checked="false" />
                                        <asp:CheckBox ID="cbdeptcumul" runat="server" Visible="false" Text="Include Cumulative"
                                            AutoPostBack="true" OnCheckedChanged="cbdeptcumul_changed" />
                                        <asp:TextBox ID="txtacd" runat="server" placeholder="Academic Year" Width="71px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <asp:Label ID="lblstdtype" runat="server" Text="Report Type"></asp:Label>
                            <asp:RadioButtonList ID="rbstudtype" runat="server" RepeatDirection="Horizontal"
                                AutoPostBack="true" OnSelectedIndexChanged="rbstudtype_Selected">
                                <asp:ListItem Text="Single" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Multiple" Value="2"></asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:Label ID="lblnum" runat="server" Text="Roll No"></asp:Label>
                            <asp:DropDownList ID="ddladmit" runat="server" AutoPostBack="True" CssClass="textbox1 ddlheight1"
                                OnSelectedIndexChanged="ddladmit_SelectedIndexChanged">
                                <asp:ListItem>Roll No</asp:ListItem>
                                <asp:ListItem>Reg No</asp:ListItem>
                                <asp:ListItem>Adm No</asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="txtno" runat="server" CssClass="textbox textbox1" Width="250px"
                                AutoPostBack="True"></asp:TextBox>
                            <%--OnTextChanged="txtno_TextChanged"--%>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderroll" runat="server" TargetControlID="txtno"
                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" -">
                            </asp:FilteredTextBoxExtender>
                            <asp:AutoCompleteExtender ID="autocomplete_rollno" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtno"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                            <asp:Button ID="btn_roll" runat="server" CssClass="textbox btn1 textbox1" Text="?"
                                OnClick="btn_roll_Click" />
                        </center>
                    </div>
                    <br />
                    <asp:Label ID="lbl_err" runat="server" Text="" Style="color: Red;"></asp:Label>
                    <asp:Label ID="lbldisp" runat="server" Visible="false" Style="color: Black;"></asp:Label>
                    <asp:Label ID="lblrolldisp" runat="server" Visible="false"></asp:Label>
                    <br />
                    <center>
                        <div id="div1" runat="server" visible="false" style="width: 850px;">
                            <FarPoint:FpSpread ID="Fpspread1" runat="server" CssClass="spreadborder" OnCellClick="Cell_Click"
                                OnPreRender="Fpspread1_render" OnButtonCommand="FpSpread1_ButtonCommand" ShowHeaderSelection="false">
                                <%--BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Style="background-color: White;"--%>
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                    </center>
                    <br />
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
                            <%--OnClientClick="return checkValidate()"--%>
                            <asp:Button ID="btnprintmasterhed" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Text="Print" OnClick="btnprintmaster_Click" Height="32px" Style="margin-top: 10px;"
                                CssClass="textbox textbox1" Width="60px" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                    </center>
                </div>
            </center>
        </div>
        <%-----To Print the Value-------%>
        <div style="height: 1px; width: 1px; overflow: auto;">
            <div id="contentDiv" runat="server" style="height: 710px; width: 1344px;" visible="false">
            </div>
        </div>
        <center>
            <div id="popwindow" runat="server" visible="false" class="popupstyle popupheight1 ">
                <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 30px; margin-left: 460px;"
                    OnClick="imagebtnpopclose_Click" />
                <br />
                <br />
                <div style="background-color: White; height: 500px; width: 950px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: Green;">Select The Student</span></div>
                    </center>
                    <br />
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_batch1" runat="server" Text="Batch"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_batch1" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Stream"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Updp_strm" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_strm" runat="server" CssClass="textbox txtheight" ReadOnly="true"
                                            onfocus="return myFunction1(this)"></asp:TextBox>
                                        <asp:Panel ID="panel_strm" runat="server" CssClass="multxtpanel multxtpanleheight"
                                            Style="height: auto; width: 150px;">
                                            <asp:CheckBox ID="cb_strm" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_strm_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_strm" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_strm_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="Popupce_strm" runat="server" TargetControlID="txt_strm"
                                            PopupControlID="panel_strm" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_degree2" runat="server" Text="Degree"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_degree2" runat="server" ReadOnly="true" Height="20px" CssClass="textbox txtheight">--Select--</asp:TextBox>
                                        <asp:Panel ID="pdegree" runat="server" CssClass="multxtpanel" Style="height: auto;
                                            width: 150px;">
                                            <asp:CheckBox ID="cb_degree2" runat="server" OnCheckedChanged="cb_degree2_ChekedChange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_degree2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree2_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_degree2"
                                            PopupControlID="pdegree" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_branch2" runat="server" Text="Branch"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_branch2" runat="server" ReadOnly="true" Height="20px" CssClass="textbox textbox1 txtheight">--Select--</asp:TextBox>
                                        <asp:Panel ID="pbranch" runat="server" CssClass="multxtpanel" Style="height: auto;
                                            width: 150px;">
                                            <asp:CheckBox ID="cb_branch1" runat="server" OnCheckedChanged="cb_branch1_ChekedChange"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_branch1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_branch1_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_branch2"
                                            PopupControlID="pbranch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_sec2" runat="server" Text="Section"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel8sec" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_sec2" runat="server" ReadOnly="true" Height="20px" CssClass="textbox textbox1 txtheight">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlsec2" runat="server" CssClass="multxtpanel" Style="height: auto;
                                            width: 120px;">
                                            <asp:CheckBox ID="cb_sec2" runat="server" OnCheckedChanged="cb_sec2_ChekedChange"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_sec2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sec2_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_sec2"
                                            PopupControlID="pnlsec2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_rollno3" runat="server" Text="Roll No"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_rollno3" TextMode="SingleLine" runat="server" AutoCompleteType="Search"
                                    Height="20px" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_rollno3"
                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno3"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                            </td>
                            <%--<td>
                                    <asp:Button ID="btn_go" Text="Go" OnClick="btn_go_Click" CssClass="textbox btn1 textbox1"
                                        runat="server" />
                                </td>--%>
                        </tr>
                        <tr runat="server" id="trFuParNot" visible="false">
                            <td colspan="7">
                            </td>
                            <td colspan="6" style="text-color: white; text-align: right;">
                                <asp:CheckBox ID="cbFpaid" runat="server" BackColor="#90EE90" Checked="true" Text="Fully Paid" /><asp:CheckBox
                                    ID="cbPpaid" runat="server" BackColor="#FFB6C1" Checked="true" Text="Partially Paid" /><asp:CheckBox
                                        ID="cbNpaid" runat="server" BackColor="White" Checked="true" Text="Not Paid" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btn_go" Text="Go" OnClick="btn_go_Click" CssClass="textbox btn1 textbox1"
                                    runat="server" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div>
                        <asp:Label ID="lbl_errormsg" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </div>
                    <div style="height: 250px; overflow: auto;">
                        <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" ShowHeaderSelection="false"
                            BorderWidth="0px" Width="850px" Style="overflow: auto; border: 0px solid #999999;
                            border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                            OnUpdateCommand="Fpspread2staff_Command">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#F7BE81" SelectionPolicy="Single">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <br />
                    <br />
                    <center>
                        <div>
                            <asp:Button ID="btn_studOK" runat="server" CssClass="textbox btn2 textbox1" Text="Ok"
                                OnClick="btn_studOK_Click" />
                            <asp:Button ID="btn_exitstud" runat="server" CssClass="textbox btn2 textbox1" Text="Exit"
                                OnClick="btn_exitstud_Click" />
                        </div>
                    </center>
                </div>
            </div>
        </center>
    </body>
    </html>
</asp:Content>
