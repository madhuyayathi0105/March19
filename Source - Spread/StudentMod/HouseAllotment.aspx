<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="HouseAllotment.aspx.cs" Inherits="HouseAllotment" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div>
        <body>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <script type="text/javascript">
                function display() {
                    document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
                }
            </script>
            <div>
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: Green;">House Allotment</span>
                    </div>
                </center>
            </div>
            <br />
            <center>
                <div style="width: 1000px; height: auto;">
                    <center>
                        <table class="maintablestyle" style="width: 740px;">
                            <tr>
                                <td>
                                    <asp:Label ID="Label13" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcollege" runat="server" AutoPostBack="true" CssClass="textbox ddlstyle ddlheight3"
                                        Width="193px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_House1" runat="server" Text="House Name"></asp:Label>
                                </td>
                                <%--   <td>
                                    <asp:DropDownList ID="ddlhousename" runat="server" AutoPostBack="false" CssClass="textbox ddlstyle ddlheight3"
                                        Width="193px">
                                    </asp:DropDownList>
                                </td>--%>
                                <td>
                                    <asp:UpdatePanel ID="UpHouse" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txthouse" runat="server" CssClass="textbox txtheight2" Width="150px"
                                                ReadOnly="true" placeholder="House" onfocus="return myFunction1(this)"></asp:TextBox>
                                            <asp:Panel ID="Panelhouse" runat="server" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_house" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_house_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_house" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_house_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txthouse"
                                                PopupControlID="Panelhouse" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btn_show" Text="GO" runat="server" CssClass="textbox btn1 textbox1"
                                        OnClick="btn_go_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btn_addnew" runat="server" Text="Add New" CssClass="textbox btn2"
                                        OnClick="btn_addnew_Click" />
                                </td>
                                <%-- <td colspan="2">
                            <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                OnSelectedIndexChanged="rblType_Selected">
                                <asp:ListItem Text="Entry" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Report"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>--%>
                            </tr>
                        </table>
                    </center>
                </div>
                <%--<div id="main_filter" runat="server" visible="false">
            <fieldset style="width: 500px;">
                <table id="rcptsngle" runat="server">
                    <tr>
                        <td>
                            <asp:DropDownList ID="rbl_rollno" runat="server" CssClass="textbox  ddlheight" AutoPostBack="true"
                                OnSelectedIndexChanged="rbl_rollno_OnSelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:TextBox ID="txt_rollno" runat="server" placeholder="Roll No" CssClass="textbox  txtheight2"
                                AutoPostBack="true" OnTextChanged="txt_rollno_Changed"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderroll" runat="server" TargetControlID="txt_rollno"
                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                            </asp:FilteredTextBoxExtender>
                            <asp:AutoCompleteExtender ID="autocomplete_rollno" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                            <asp:Label ID="lblStudStatus" runat="server" Text="" Visible="false" CssClass="textbox btn1 textbox1"
                                Style="color: Green; font-weight: bold;"></asp:Label>
                            <asp:TextBox ID="txt_name" runat="server" placeholder="Name" CssClass="textbox txtheight2"
                                AutoPostBack="true"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetName" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_name"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                            <asp:Button ID="btn_roll" runat="server" CssClass="textbox btn1 textbox1" Text="?"
                                OnClick="btn_roll_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btnSearch" Text="Save" runat="server" CssClass="textbox btn1 textbox1"
                                OnClick="btn_save_Click" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>--%>
                <br />
                <div>
                    <asp:Label ID="lbl_error" Visible="false" runat="server" Text="" ForeColor="Red"
                        AutoPostBack="true"></asp:Label>
                </div>
                <div id="fps_print" runat="server">
                    <div>
                        <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" ShowHeaderSelection="false"
                            BorderWidth="0px" Width="830px" Style="overflow: auto; border: 0px solid #999999;
                            border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                            OnCellClick="Cell_Click" OnPreRender="fpspread2_rowselect">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#F7BE81" SelectionPolicy="Single">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <div id="print" runat="server" visible="false">
                        <asp:Label ID="lblvalidation1" runat="server" Text="Please Enter Report Name" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red" Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname" runat="server" Visible="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" Visible="true" Width="180px" onkeypress="display()"
                            CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                            InvalidChars="/\">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnExcel" runat="server" Visible="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnExcel_Click" Text="Export To Excel" Width="127px"
                            Height="32px" CssClass="textbox textbox1" />
                        <asp:Button ID="btnprintmasterhed" runat="server" Visible="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" Height="32px"
                            Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                        <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                    </div>
                </div>
            </center>
            <%--main filter--%>
            <center>
                <div id="addnew_popup" runat="server" visible="false" style="height: 50em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0;">
                    <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 23px; margin-left: 341px;"
                        OnClick="imagebtnaddnewpopclose_Click" />
                    <br />
                    <br />
                    <div class="subdivstyle" style="background-color: White; overflow: auto; width: 700px;
                        height: 400px;" align="center">
                        <center>
                            <br />
                            <asp:Label ID="lbl_house" runat="server" class="fontstyleheader" Style="color: Green;"
                                Text="House Entry"></asp:Label>
                        </center>
                        <br />
                        <div align="left" style="overflow: auto; width: 500px; height: 250px; border-radius: 10px;
                            border: 1px solid Gray;">
                            <br />
                            <center>
                                <div id="main_filter" runat="server">
                                    <table id="rcptsngle" runat="server">
                                        <tr style="height: 40px;">
                                            <td>
                                                <asp:Label ID="lbl_select" runat="server" Text="Select"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="rbl_rollno" runat="server" CssClass="textbox  ddlheight" AutoPostBack="true"
                                                    OnSelectedIndexChanged="rbl_rollno_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txt_rollno" runat="server" placeholder="Roll No" CssClass="textbox  txtheight2"
                                                    AutoPostBack="true" OnTextChanged="txt_rollno_Changed"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender55" runat="server" TargetControlID="txt_rollno"
                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:AutoCompleteExtender ID="autocomplete_rollno" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="panelbackground">
                                                </asp:AutoCompleteExtender>
                                                <asp:Label ID="lblStudStatus" runat="server" Text="" Visible="false" CssClass="textbox btn1 textbox1"
                                                    Style="color: Green; font-weight: bold;"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="height: 40px;">
                                            <td>
                                                <asp:Label ID="Label3" runat="server" Text="Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_name" runat="server" placeholder="Name" CssClass="textbox txtheight2"
                                                    AutoPostBack="true"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetName" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_name"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="panelbackground">
                                                </asp:AutoCompleteExtender>
                                                <asp:Button ID="btn_roll" runat="server" CssClass="textbox btn1 textbox1" Text="?"
                                                    OnClick="btn_roll_Click" />
                                            </td>
                                        </tr>
                                        <tr style="height: 40px;">
                                            <td>
                                                <asp:Label ID="Label2" runat="server" Text="House Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlhousename1" runat="server" AutoPostBack="false" CssClass="textbox ddlstyle ddlheight3"
                                                    Width="193px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <br />
                                                <br />
                                                <center>
                                                    <asp:Button ID="btnsave" Text="Save" runat="server" CssClass="textbox btn2" OnClick="btn_save_Click" />&nbsp;&nbsp;
                                                    <asp:Button ID="btn_delete" runat="server" Text="Delete" CssClass="textbox btn2"
                                                        OnClick="btn_delete_Click" Visible="false" />&nbsp;&nbsp;
                                                    <asp:Button ID="btn_exit" Text="Exit" runat="server" Visible="false" CssClass="textbox btn2"
                                                        OnClick="btn_exit_Click" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </center>
                        </div>
                    </div>
                </div>
            </center>
            <%--  ******popup window******--%>
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
                                    <asp:Label ID="lbl_stream" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_strm" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_degree2" runat="server" Text="Degree"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_degree2" runat="server" ReadOnly="true" Height="20px" CssClass="textbox txtheight">--Select--</asp:TextBox>
                                            <asp:Panel ID="pdegree" runat="server" Width="150px" Height="170px" CssClass="multxtpanel">
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
                                            <asp:Panel ID="pbranch" runat="server" Width="250px" Height="200px" CssClass="multxtpanel">
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
                                    <span class="challanLabel">
                                        <p>
                                            Section</p>
                                    </span>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel8sec" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_sec2" runat="server" ReadOnly="true" Height="20px" CssClass="textbox textbox1 txtheight">--Select--</asp:TextBox>
                                            <asp:Panel ID="pnlsec2" runat="server" Width="120px" Height="80px" CssClass="multxtpanel">
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
                                    <asp:Button ID="btn_go" Text="Go" CssClass="textbox btn1 textbox1" OnClick="btn_popup_go_Click"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr runat="server" id="trFuParNot" visible="false">
                                <td colspan="5">
                                </td>
                                <td colspan="8" style="text-color: white; text-align: right;">
                                    <asp:CheckBox ID="cbFirstGrad" runat="server" BackColor="#EE9090" Checked="true"
                                        Text="First Graduate" />
                                    <asp:CheckBox ID="cbFpaid" runat="server" BackColor="#90EE90" Checked="true" Text="Fully Paid" /><asp:CheckBox
                                        ID="cbPpaid" runat="server" BackColor="#FFB6C1" Checked="true" Text="Partially Paid" />
                                    <asp:CheckBox ID="cbNpaid" runat="server" BackColor="White" Checked="true" Text="Not Paid" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div>
                            <asp:Label ID="lbl_errormsg" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </div>
                        <div>
                            <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" ShowHeaderSelection="false"
                                BorderWidth="0px" Width="830px" Style="overflow: auto; height: 250px; border: 0px solid #999999;
                                border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                OnUpdateCommand="Fpspread1_Command">
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
                <div style="height: 1px; width: 1px; overflow: auto;">
                    <div id="Div3" runat="server" style="height: 710px; width: 1344px;" visible="false">
                    </div>
                </div>
            </center>
        </body>
    </div>
</asp:Content>
