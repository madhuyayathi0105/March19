<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Refund Report.aspx.cs" Inherits="Refund_Report"
    EnableEventValidation="false" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
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
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=btnAdd.ClientID %>').click(function () {
                $('#<%=divaddtype.ClientID %>').show();
                $('#<%=txtdesc.ClientID %>').val('');
                return false;
            });
            $('#<%=btnDel.ClientID %>').click(function () {
                var rptText = $('#<%=ddlreport.ClientID %>').find('option:selected').text();
                if (rptText.trim() != null && rptText != "Select") {
                    var msg = confirm("Are you sure you want to delete this report type?");
                    if (msg)
                        return true;
                    else
                        return false;
                }
                else {
                    alert("Please select any one report type!");
                    return false;
                }
            });

            $('#<%=btnexittype.ClientID %>').click(function () {
                $('#<%=divaddtype.ClientID %>').hide();
                return false;
            });

            $('#<%=btnaddtype.ClientID %>').click(function () {
                var txtval = $('#<%=txtdesc.ClientID %>').val();
                if (txtval == null || txtval == "") {
                    alert("Please enter the report type!");
                    return false;
                }
            });

            $('#<%=btnclear.ClientID %>').click(function () {
                $('#<%=txtcolorder.ClientID %>').val('');
                $("[id*=cblcolumnorder]").removeAttr('checked');
                $('#<%=cb_column.ClientID %>').removeAttr('checked');
                return false;
            });

            $('#<%=imgcolumn.ClientID %>').click(function () {
                $('#<%=divcolorder.ClientID %>').hide();
                return false;
            });
            $('#<%=btngo.ClientID %>').click(function () {
                var rptText = $('#<%=ddlMainreport.ClientID %>').find('option:selected').text();
                if (rptText.trim() == null || rptText == "Select") {
                    alert("Please select any one report type!");
                    return false;
                }
            });

            $('#<%=btncolorderOK.ClientID %>').click(function () {
                var rptText = $('#<%=ddlreport.ClientID %>').find('option:selected').text();
                var textval = $('#<%=txtcolorder.ClientID %>').val();

                if (rptText.trim() == null || rptText == "Select") {
                    alert("Please select any one report type!");
                    return false;
                }
                $('#<%=txtcolorder.ClientID %>').removeClass("backColor");

                if ((textval == "" || textval == null)) {
                    if (textval == "" || textval == null) {
                        $('#<%=txtcolorder.ClientID %>').addClass("backColor");
                    }
                    alert("Please Select columns!");
                    return false;
                }
            });


        });
        function columnOrderCbl() {
            $('#<%=txtcolorder.ClientID %>').removeClass("backColor");
            var txtval = document.getElementById('<%=txtcolorder.ClientID%>');
            txtval.value = "";
            var getval = "";
            var cball = document.getElementById('<%=cb_column.ClientID%>');
            var cblall = document.getElementById('<%=cblcolumnorder.ClientID%>');
            var tagname = cblall.getElementsByTagName("input");
            var tagnamestr = cblall.getElementsByTagName("label");
            if (cball.checked == true) {
                for (var i = 0; i < tagname.length; i++) {
                    tagname[i].checked = true;
                    if (getval == "")
                        getval = tagnamestr[i].innerHTML; //+ "(" + (i + 1) + ")"
                    else
                        getval += ", " + tagnamestr[i].innerHTML; //+ "(" + (i + 1) + ")"
                }
            }
            else {
                for (var i = 0; i < tagname.length; i++) {
                    tagname[i].checked = false;
                }
                getval = "";
                oldval = "";
            }
            if (getval != "") {
                txtval.value = getval.toString();
            }
        }

        function columnOrderCb() {
            var txtval = document.getElementById('<%=txtcolorder.ClientID%>');
            var oldval = txtval.value.toString();
            txtval.value = "";
            var newval = "";
            var getval = "";
            var count = 0;
            var cball = document.getElementById('<%=cb_column.ClientID%>');
            var cblall = document.getElementById('<%=cblcolumnorder.ClientID%>');
            var tagname = cblall.getElementsByTagName("input");
            var tagnamestr = cblall.getElementsByTagName("label");
            for (var i = 0; i < tagname.length; i++) {
                if (tagname[i].checked == true) {
                    count += 1;
                    getval = tagnamestr[i].innerHTML; //current checked val
                    if (oldval != null && oldval != "") {
                        var result = oldval.includes(getval);
                        if (!result) {
                            oldval += "," + getval;
                        }
                    }
                    else {
                        oldval = getval;
                    }
                }
            }
            if (tagname.length == count) {
                cball.checked = true;
            }
            else {
                cball.checked = false;
            }
            if (oldval != "") {
                txtval.value = oldval.toString();
            }
        }   
         
    </script>
    <style type="text/css">
        .head
        {
            background-color: Teal;
            font-family: Book Antiqua;
            font-size: medium;
            color: black;
            top: 75px;
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
            height: auto;
            top: 100px;
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
        <asp:Label ID="lbl_head" runat="server" Font-Bold="true" Style="left: 380px; top: 0px;
            position: absolute;" Font-Names="Book Antiqua" ForeColor="white" Font-Size="Large"
            Text="Refund,Excess,Scholarship Report"></asp:Label>
    </div>
    <body>
        <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>--%>
        <div class="mainbatch">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblcollege" runat="server" Text="College"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcollege" runat="server" AutoPostBack="true" Width="150px"
                            OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbltype" runat="server" Text="Type"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txttype" Enabled="false" Width="100px" runat="server" ReadOnly="true">--Select--</asp:TextBox>
                        <asp:Panel ID="PType" runat="server" Font-Names="Book Antiqua" Font-Bold="true" CssClass="multxtpanel multxtpanleheight"
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
                        <asp:Label ID="Iblbatch" runat="server" Text="Batch"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_batch" Width="100px" runat="server" ReadOnly="true">--Select--</asp:TextBox>
                        <asp:Panel ID="pbatch" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                            CssClass="multxtpanel multxtpanleheight" Width="114px" Height="190px" Font-Size="Medium">
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
                        <asp:Label ID="Ibldegree" runat="server" Text="Degree"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_degree" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                        <asp:Panel ID="pdegree" runat="server" CssClass="multxtpanel multxtpanleheight" Height="210px"
                            Width="128px" Font-Bold="true" Font-Size="Medium">
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
                        <asp:Label ID="Iblbranch" runat="server" Text="Branch"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_branch" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                        <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel multxtpanleheight" Height="210px"
                            Width="400px">
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
                        <asp:Label runat="server" ID="lblfyear" Text="Finance Year" Style="width: 100px"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtfyear" runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                        <asp:Panel ID="Pfyear" runat="server" CssClass="multxtpanel multxtpanleheight">
                            <asp:CheckBox ID="chkfyear" runat="server" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                Text="Select All" OnCheckedChanged="chkfyear_changed" AutoPostBack="True" />
                            <asp:CheckBoxList ID="chklsfyear" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                                runat="server" OnSelectedIndexChanged="chklsfyear_selected" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtfyear"
                            PopupControlID="Pfyear" Position="Bottom">
                        </asp:PopupControlExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Text="Header"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtHeader" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                        <asp:Panel ID="Pheader" runat="server" CssClass="multxtpanel multxtpanleheight" Height="210px"
                            Width="290px">
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
                        <asp:Label ID="lblledger" runat="server" Text="Ledger"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtledger" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                        <asp:Panel ID="pledgre" runat="server" CssClass="multxtpanel multxtpanleheight" Height="210px"
                            Width="290px">
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
                        <asp:Label runat="server" ID="lblcategory" Text="Category"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtcetgory" runat="server" ReadOnly="true" Width="105px">--Select--</asp:TextBox>
                        <asp:Panel ID="Pcategory" runat="server" CssClass="multxtpanel multxtpanleheight"
                            Width="200px" Height="200px">
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
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="Label1" Text="Mode"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtmode" runat="server" Width="146px" ReadOnly="true">--Select--</asp:TextBox>
                        <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel multxtpanleheight">
                            <asp:CheckBox ID="cbmode" runat="server" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                Text="Select All" OnCheckedChanged="cbmode_changed" AutoPostBack="True" />
                            <asp:CheckBoxList ID="cblmode" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                                runat="server" OnSelectedIndexChanged="cblmode_selected" AutoPostBack="True">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txtmode"
                            PopupControlID="Panel1" Position="Bottom">
                        </asp:PopupControlExtender>
                    </td>
                    <td colspan="3">
                        <asp:CheckBox ID="chkceteg" Visible="false" runat="server" Text="Category Wise" AutoPostBack="true"
                            OnCheckedChanged="chkceteg_CheckedChanged" />
                        <asp:Label ID="lblrpt" runat="server" Text="Report"></asp:Label>
                        <asp:DropDownList ID="ddlMainreport" runat="server" CssClass="textbox textbox1 ddlheight4"
                            Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlMainreport_Selected">
                        </asp:DropDownList>
                        <asp:Button ID="btngo" runat="server" Font-Names="Book Antiqua" Text="Go" OnClick="btngo_Click"
                            Font-Size="Medium" Font-Bold="true" />
                        <asp:ImageButton ID="lnkcolorder" runat="server" Width="30px" Height="30px" Text="All"
                            ImageUrl="~/Hostel Gete Images/images (1)ppp.jpg" OnClick="lnkcolorder_Click" />
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
        <div>
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
                <TitleInfo Font-Size="X-Large" ForeColor="#000000" HorizontalAlign="Center" VerticalAlign="NotSet">
                </TitleInfo>
            </FarPoint:FpSpread>
        </div>
        <br />
        <div>
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
        </div>
        <%--</ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnmasterprint" />
                <asp:PostBackTrigger ControlID="btnxl" />
                <asp:PostBackTrigger ControlID="btngo" />
            </Triggers>
        </asp:UpdatePanel>--%>
    </body>
    <%--column order--%>
    <center>
        <div id="divcolorder" runat="server" style="height: 100%; display: none; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <asp:ImageButton ID="imgcolumn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: 83px; margin-left: 403px;" />
            <%--   <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>--%>
            <center>
                <div id="Div2" runat="server" class="table" style="background-color: White; height: 580px;
                    width: 850px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 90px;
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
                                    <center>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblrptype" Text="Report Type" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnAdd" runat="server" Text="+" CssClass="textbox textbox1 btn1" /><%--OnClick="btnAdd_OnClick"--%>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlreport" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlreport_SelectedIndexChanged"
                                                        CssClass="textbox textbox1 ddlheight4">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnDel" runat="server" Text="-" CssClass="textbox textbox1 btn1"
                                                        OnClick="btnDel_OnClick" />
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
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
                                    <asp:TextBox ID="txtcolorder" runat="server" Columns="20" Style="height: 70px; width: 800px;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" Width="800px"
                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                        RepeatDirection="Horizontal" onclick="return columnOrderCb()">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                    <center>
                                        <asp:Button ID="btncolorderOK" CssClass=" textbox btn1 comm" Style="height: 28px;
                                            width: 65px;" OnClick="btncolorderOK_Click" Text="OK" runat="server" />
                                        <asp:Button ID="btnclear" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                            Text="Clear" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
            <%-- </ContentTemplate>
            </asp:UpdatePanel>--%>
        </div>
    </center>
    <%--report type name enter text box--%>
    <center>
        <div id="divaddtype" runat="server" style="height: 100%; z-index: 10000; width: 100%;
            background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;
            display: none;">
            <center>
                <div id="panel_description11" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 467px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <table>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lbldesc" runat="server" Text="Description" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:TextBox ID="txtdesc" runat="server" Width="400px" Style="font-family: 'Book Antiqua';
                                    margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnaddtype" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="textbox btn1" OnClick="btnaddtype_Click" />
                                <asp:Button ID="btnexittype" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="textbox btn1" /><%--OnClick="btnexittype_Click"--%>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
        </div>
    </center>
    <%--progressBar for AdditionalDetails--%>
    <%--  <center>
        <asp:UpdateProgress ID="UpdateProgress12" runat="server" AssociatedUpdatePanelID="UpCallNoDet">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                            Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender12" runat="server" TargetControlID="UpdateProgress12"
            PopupControlID="UpdateProgress12">
        </asp:ModalPopupExtender>
    </center>--%>
</asp:Content>
