<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Individual_StudentFeeStatus.aspx.cs" Inherits="Individual_StudentFeeStatus" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <title></title>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblsmserror.ClientID %>').innerHTML = "";
            }
            function SelLedgers() {
                var chkSelAll = document.getElementById("<%=chkGridSelectAll.ClientID %>");
                var tbl = document.getElementById("<%=GrdStudent.ClientID %>");
                var gridViewControls = tbl.getElementsByTagName("input");

                for (var i = 1; i < (tbl.rows.length); i++) {
                    var chkSelectid = document.getElementById('MainContent_GrdStudent_selectchk_' + i.toString());

                    if (chkSelAll.checked == false) {
                        chkSelectid.checked = false;
                    } else {
                        chkSelectid.checked = true;
                    }
                }
            }
            function SelStaff() {
                var chkSelAll = document.getElementById("<%=ChkSelectGridStaff.ClientID %>");
                var tbl = document.getElementById("<%=GrdStaff.ClientID %>");
                var gridViewControls = tbl.getElementsByTagName("input");

                for (var i = 1; i < (tbl.rows.length); i++) {
                    var chkSelectid = document.getElementById('MainContent_GrdStaff_selectchks_' + i.toString());

                    if (chkSelAll.checked == false) {
                        chkSelectid.checked = false;
                    } else {
                        chkSelectid.checked = true;
                    }
                }
            }
        </script>
        <style type="text/css">
            .style44
            {
                width: 68px;
            }
            .style49
            {
                width: 72px;
            }
        </style>
        <div>
            <asp:ScriptManager ID="ScriptManager2" runat="server">
            </asp:ScriptManager>
            <center>
                <div>
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: #008000">Individual Student Fee Status Report</span></div>
                    </center>
                </div>
                <div class="maindivstyle" style="width: 1170px; height: auto;">
                    <br />
                    <div>
                        <center>
                            <table class="maintablestyle" width="950px" border="0">
                                <tr>
                                    <td colspan="6">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_collegename" Text="College" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                        Width="215px" OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_hdr" runat="server" CssClass="textbox1 ddlheight4" OnSelectedIndexChanged="ddl_hdr_OnSelectedIndexChanged"
                                                        AutoPostBack="true" Width="152px">
                                                        <asp:ListItem Selected="True" Text="Group Header" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Header" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Ledger" Value="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_hdr" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upheader" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtheader" runat="server" CssClass="textbox textbox1" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="panelheader" runat="server" CssClass="multxtpanel" Style="height: auto;
                                                                width: 300px;">
                                                                <asp:CheckBox ID="cb_header" runat="server" Text="Select All" AutoPostBack="true"
                                                                    OnCheckedChanged="cb_header_checkedchanged" />
                                                                <asp:CheckBoxList ID="cbl_header" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_header_selectedindexchanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="popupheader" runat="server" TargetControlID="txtheader"
                                                                PopupControlID="panelheader" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblcate" runat="server" Text="Category"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upsem" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtfee" runat="server" CssClass="textbox textbox1" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="pnlfee" runat="server" CssClass="multxtpanel" Style="height: auto;
                                                                width: 151px;">
                                                                <asp:CheckBox ID="cb_fee" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_fee_checkedchanged" />
                                                                <asp:CheckBoxList ID="cbl_fee" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_fee_selectedindexchanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtfee"
                                                                PopupControlID="pnlfee" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <table>
                                            <tr>
                                                <td id="lbltype1" runat="server">
                                                    <asp:Label ID="Lbltype" runat="server" Text="Type"></asp:Label>
                                                    <asp:DropDownList ID="ddltype" runat="server" CssClass="textbox1 ddlheight2" AutoPostBack="True"
                                                        Width="211px" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                                                        <asp:ListItem Enabled="false">Enquired Students</asp:ListItem>
                                                        <asp:ListItem Enabled="false">Applied Students</asp:ListItem>
                                                        <asp:ListItem>Admitted Students</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td colspan="4">
                                                    <asp:Label ID="lblnum" runat="server" Text="Roll No"></asp:Label>
                                                    <asp:DropDownList ID="ddladmit" runat="server" AutoPostBack="True" CssClass="textbox1 ddlheight1"
                                                        OnSelectedIndexChanged="ddladmit_SelectedIndexChanged">
                                                        <asp:ListItem>Roll No</asp:ListItem>
                                                        <asp:ListItem>Reg No</asp:ListItem>
                                                        <asp:ListItem>Adm No</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtno" runat="server" CssClass="textbox textbox1" Width="250px"
                                                        OnTextChanged="txtno_TextChanged" AutoPostBack="True"></asp:TextBox>
                                                    <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderroll" runat="server" TargetControlID="txtno"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" -">
                                                    </asp:FilteredTextBoxExtender>--%>
                                                    <asp:AutoCompleteExtender ID="autocomplete_rollno" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtno"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                    <asp:Button ID="btn_roll" runat="server" CssClass="textbox btn1 textbox1" Text="?"
                                                        OnClick="btn_roll_Click" />
                                                    <asp:DropDownList ID="ddlViewFormat" runat="server" CssClass="textbox1 ddlheight1"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlViewFormat_Selected">
                                                        <asp:ListItem Selected="True">Format 1</asp:ListItem>
                                                        <asp:ListItem>Format 2</asp:ListItem>
                                                        <asp:ListItem>Format 3</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Type" runat="server" Text="Type"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="studstaffid" runat="server" RepeatDirection="Horizontal"
                                            AutoPostBack="true" OnSelectedIndexChanged="rbstudstaffid_Selected">
                                            <asp:ListItem Text="Student" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Staff" Value="2"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblstdtype" runat="server" Text="Report Type"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rbstudtype" runat="server" RepeatDirection="Horizontal"
                                            AutoPostBack="true" OnSelectedIndexChanged="rbstudtype_Selected">
                                            <asp:ListItem Text="Single" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Multiple" Value="2"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td colspan="3">
                                        <fieldset style="width: 410px" id="fieldset1" runat="server">
                                            <asp:CheckBox ID="inclnarr" runat="server" Text="Narration" />
                                            <asp:CheckBox ID="cbincdedut" runat="server" Text="Concession" />
                                            <asp:CheckBox ID="cbpaymode" runat="server" Text="Paymode" />
                                            <asp:CheckBox ID="cbTrans" runat="server" Text="Transfer" />
                                            <asp:CheckBox ID="cbRefund" runat="server" Text="Refund" />
                                            <asp:Label ID="lblrolldisp" runat="server" Visible="false"></asp:Label>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <asp:Button ID="btngo" runat="server" CssClass="textbox textbox1 btn1" Text="Go"
                                            OnClick="btngo_click" />
                                        <asp:Label ID="lbldisp" runat="server" Visible="false" Style="color: Black;"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="tdlblfnl" runat="server" visible="false">
                                        <asp:Label runat="server" ID="lblfyear" Text="FinanceYear" Width="85px"></asp:Label>
                                    </td>
                                    <td id="tdfnl" runat="server" visible="false">
                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtfyear" Style="height: 20px; width: 141px;" CssClass="Dropdown_Txt_Box"
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
                            </table>
                        </center>
                    </div>
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="errmsg" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="btnback" runat="server" Text="Back" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnback_Click" Visible="false" />
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Error" runat="server" Text="No Records Found." Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" ForeColor="Red" Visible="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <%--  <center>--%>
                    <%--  <div id="div1" runat="server" visible="false" style="width: 950px; overflow: auto;">--%>
                  
                    <asp:GridView ID="grdIndividualReport" Width="1000px" Style="font-weight: bold" runat="server"
                        ShowFooter="false" AutoGenerateColumns="true" Font-Names="Book Antiqua" ShowHeader="false"
                        toGenerateColumns="false" OnRowDataBound="grdIndividualReport_RowDataBound">
                        <%--OnRowDataBound="grdIndividualReport_RowDataBound"--%>
                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                    </asp:GridView>
                    <%-- </div>--%>
                    <%--</center>--%>
                    <br />
                    <br />
                    <center>
                        <div id="rprint" runat="server">
                            <asp:Label ID="lblsmserror" Text="" Font-Size="Large" Font-Names="Book Antiqua" Visible="false"
                                ForeColor="Red" runat="server" Font-Bold="true"></asp:Label>
                            <asp:Label ID="lblexcel" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            <asp:TextBox ID="txtexcel" onkeypress="display()" CssClass="textbox textbox1" runat="server"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcel"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnexcel" runat="server" CssClass="textbox textbox1 btn2" Text="Export Excel"
                                OnClick="btnexcel_Click" />
                            <asp:Button ID="btnprintmaster" runat="server" CssClass="textbox textbox1 btn2" Text="Print"
                                OnClick="btnprintmaster_Click" />
                            <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                            <%-- Added By Saranya 12Dec2017--%>
                            <asp:Button ID="btnprint" runat="server" Text="Individual Print" CssClass="textbox textbox1 btn2"
                                BackColor="LightGreen" Width="100px" OnClick="btnprint_click" />
                        </div>
                    </center>
                    <br />
                </div>
            </center>
            <br />
        </div>
        <%--Student Lookup--%>
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
                                <asp:Label ID="lbl_stream" runat="server" Text="Stream"></asp:Label>
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
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
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
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
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
                                        <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_sec2"
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
                            <td colspan="2">
                                <asp:CheckBox ID="checkdicon" runat="server" Text="Include Discontinue" AutoPostBack="true"
                                    OnCheckedChanged="checkdicon_Changed" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="width: 200px;" />
                            </td>
                            <td colspan="2">
                                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtinclude" Enabled="false" Font-Bold="true" Font-Size="Medium"
                                            Font-Names="Book Antiqua" Style="height: 20px; width: 164px;" CssClass="Dropdown_Txt_Box"
                                            runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlinclude" runat="server" CssClass="multxtpanel multxtpanleheight"
                                            Style="height: auto; width: 172px;">
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
                    <span style="padding-right: 100px; margin-left: -652px; margin-top: 3px;">
                        <asp:CheckBox ID="chkGridSelectAll" runat="server" Text="SelectAll" Visible="false"
                            onchange="return SelLedgers();" />
                    </span>
                    <div style="height: 250px; overflow: auto;">
                        <asp:GridView ID="GrdStudent" Width="900px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                            Font-Names="Book Antiqua" ShowHeader="false" toGenerateColumns="false" OnRowDataBound="GrdStudent_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lbl_sno" runat="server" Style="width: auto;" Text='<%#Eval("Sno") %>'></asp:Label>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="allchk" runat="server" Text="Select All" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="selectchk" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                        </asp:GridView>
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
        <%--Staff Lookup --%>
        <center>
            <div id="div_staffLook" runat="server" visible="false" class="popupstyle popupheight1 ">
                <asp:ImageButton ID="ImageButton5" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 30px; margin-left: 310px;"
                    OnClick="btn_exitstaff_Click" />
                <br />
                <br />
                <div style="background-color: White; height: 500px; width: 650px; border: 5px solid #0CA6CA;
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
                            <%-- <td>
                                <asp:Label ID="Label1" runat="server" Text="Total No.of Students"></asp:Label>
                                <asp:TextBox ID="txt_totnoofstudents" runat="server" CssClass="textbox txtheight"
                                    MaxLength="8" Style="text-align: right;"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="filterextendertot" runat="server" TargetControlID="txt_totnoofstudents"
                                    FilterType="Numbers">
                                </asp:FilteredTextBoxExtender>
                            </td>--%>
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
                                <%--<asp:Button ID="btn_staffLook" runat="server" CssClass="textbox btn1 textbox1" Text="?"
                            OnClick="btn_staffLook_Click" />
                       <asp:Button ID="Button4" runat="server" CssClass="textbox btn1 textbox1" Text="Clear"
                            OnClick="btnClear_Click" Style="color: Red; font-weight: bold;" />--%>
                                <br>
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
                    <span style="padding-right: 100px; margin-left: -652px; margin-top: 3px;">
                        <asp:CheckBox ID="ChkSelectGridStaff" runat="server" Text="SelectAll" Visible="false"
                            onchange="return SelStaff();" />
                    </span>
                    <div style="height: 250px; overflow: auto;">
                        <asp:GridView ID="GrdStaff" Width="600px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                            Font-Names="Book Antiqua" ShowHeader="false" toGenerateColumns="false" OnRowDataBound="GrdStaff_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lbl_sno" runat="server" Style="width: auto;" Text='<%#Eval("Sno") %>'></asp:Label>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="allchk" runat="server" Text="Select All" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="selectchks" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                        </asp:GridView>
                    </div>
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
    </body>
</asp:Content>
