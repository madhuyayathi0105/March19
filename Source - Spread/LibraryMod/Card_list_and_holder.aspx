<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="Card_list_and_holder.aspx.cs" Inherits="LibraryMod_Card_list_and_holder" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script>        function QuantityChange1(objRef) {
            var grdvw = document.getElementById("<%=grdManualExit.ClientID %>");
            var grid = document.getElementById('<%=grdManualExit.ClientID%>');
            var ddl = document.getElementById('MainContent_grdManualExit_selectall_0');

            if (ddl.checked == true) {
                for (var i = 1; i < grid.rows.length; i++) {
                    var ddl_select = document.getElementById('MainContent_grdManualExit_select_' + i.toString());
                    ddl_select.checked = true;

                }

            }
            else {
                for (var i = 1; i < grid.rows.length; i++) {
                    var ddl_select = document.getElementById('MainContent_grdManualExit_select_' + i.toString());
                    ddl_select.checked = false;
                }
            }




        }
    </script>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Details of User and Token Number Report</span></div>
        </center>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
            <ContentTemplate>
                <center>
                    <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                        <div>
                            <table class="maintablestyle" style="height: auto; font-family: Book Antiqua; font-weight: bold;
                                margin-left: 0px; margin-top: 10px; margin-bottom: 10px; padding: 6px;">
                                <tr>
                                    <td colspan="2">
                                        <fieldset style="width: 230px; height: 10px;">
                                            <asp:RadioButtonList ID="rblreporttype" runat="server" RepeatDirection="Horizontal"
                                                AutoPostBack="true" Enabled="True" Font-Names=" Book antiqua" OnCheckedChanged="rblreporttype_CheckedChange">
                                                <asp:ListItem Text="Card List" Value="0" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Card Holder" Value="1"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </fieldset>
                                    </td>
                                    <td colspan="2">
                                        <fieldset style="width: 230px; height: 10px;">
                                            <asp:RadioButtonList ID="Rblreturn" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                                Enabled="True" Font-Names=" Book antiqua" OnCheckedChanged="Rblreturn_CheckedChange">
                                                <asp:ListItem Text="Student" Value="0" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Staff" Value="1"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label16" runat="server" Text="College">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtclg" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                            Width="160px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                        <asp:Panel ID="Panel4" runat="server" Width="280px" CssClass="multxtpanel multxtpanleheight">
                                            <asp:CheckBox ID="cbclg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnCheckedChanged="cbclg_CheckedChanged" Text="Select All"
                                                AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cblclg" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                Font-Bold="True" OnSelectedIndexChanged="cblclg_SelectedIndexChanged" Font-Names="Book Antiqua">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtclg"
                                            PopupControlID="Panel4" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" Text="Batch">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtbatch" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                            Width="100px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                        <asp:Panel ID="pbatch" runat="server" Width="110px" CssClass="multxtpanel multxtpanleheight">
                                            <asp:CheckBox ID="chkbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnCheckedChanged="chkbatch_CheckedChanged" Text="Select All"
                                                AutoPostBack="True" />
                                            <asp:CheckBoxList ID="chklstbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                Font-Bold="True" Font-Names="Book Antiqua">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtbatch"
                                            PopupControlID="pbatch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Text="Degree">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtdegree" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                            Width="100px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                        <asp:Panel ID="pdegree1" runat="server" Width="300px" Style="text-align: left;" CssClass="multxtpanel multxtpanleheight">
                                            <asp:CheckBox ID="chkdegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnCheckedChanged="chkdegree_CheckedChanged" Text="Select All"
                                                AutoPostBack="True" />
                                            <asp:CheckBoxList ID="chklstdegree" runat="server" Style="font-family: Book Antiqua;
                                                font-size: medium; font-weight: bold; text-align: left;" Font-Size="Medium" AutoPostBack="True"
                                                OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged" Height="58px" Font-Bold="True"
                                                Font-Names="Book Antiqua">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdegree"
                                            PopupControlID="pdegree1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" Text="Branch">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtbranch" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                            Width="110px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                        <asp:Panel ID="pbranch" runat="server" Width="350px" Style="text-align: left;" CssClass="multxtpanel multxtpanleheight">
                                            <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnCheckedChanged="chkbranch_CheckedChanged" Text="Select All"
                                                AutoPostBack="True" />
                                            <asp:CheckBoxList ID="chklstbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged" Style="font-family: 'Book Antiqua';
                                                text-align: left;" Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtbranch"
                                            PopupControlID="pbranch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblstaffDept" runat="server" Text="Department">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtstaffDept" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                            Width="110px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                        <asp:Panel ID="pstaffDept" runat="server" Height="400px" Width="335px" Style="text-align: left;"
                                            CssClass="multxtpanel multxtpanleheight">
                                            <asp:CheckBox ID="chksatffDept" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnCheckedChanged="chksatffDept_CheckedChanged" Text="Select All"
                                                AutoPostBack="True" />
                                            <asp:CheckBoxList ID="chklststaffDept" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                OnSelectedIndexChanged="chklststaffDept_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                                Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtstaffDept"
                                            PopupControlID="pstaffDept" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblStaffCategory" runat="server" Text="Staff Category">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_StaffCatogery" runat="server" CssClass="textbox txtheight2"
                                            ReadOnly="true" Width="160px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel2" runat="server" Width="280px" CssClass="multxtpanel multxtpanleheight">
                                            <asp:CheckBox ID="cb_StaffCatogery" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnCheckedChanged="cb_StaffCatogery_CheckedChanged" Text="Select All"
                                                AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_StaffCatogery" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                OnSelectedIndexChanged="cbl_StaffCatogery_SelectedIndexChanged" Font-Bold="True"
                                                Font-Names="Book Antiqua">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_StaffCatogery"
                                            PopupControlID="Panel2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_staffcode" Visible="false" runat="server" AutoPostBack="true"
                                            CssClass="textbox txtheight2" OnTextChanged="txt_staffcode_TextChanged" Style="width: 80px;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_rollno"
                                            FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getstfcode" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffcode"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td style="padding-left: 25px;">
                                        <asp:DropDownList ID="ddlrollno" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlrollno_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_rollno" runat="server" AutoPostBack="true" CssClass="textbox txtheight2"
                                            OnTextChanged="txt_rollno_TextChanged" Style="width: 80px;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_rollno"
                                            FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpGo" runat="server">
                                            <ContentTemplate>
                                                <asp:ImageButton ID="btnMainGo" runat="server" ImageUrl="~/LibImages/Go.jpg" Style="margin-top: 10px;"
                                                    OnClick="btnMainGo_Click" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </center>
                <br />
                <br />
                <center>
                    <asp:GridView ID="grdManualExit" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                        ShowHeader="true" Font-Names="Book Antiqua" toGenerateColumns="false" AllowPaging="true"
                        PageSize="500" OnSelectedIndexChanged="grdManualExit_OnSelectedIndexChanged" OnPageIndexChanging="grdManualExit_OnPageIndexChanged">
                     
                        <HeaderStyle BackColor="#0CA6CA" ForeColor="black" />
                    </asp:GridView>
                </center>
                <center>
                    <div id="rptprint1" runat="server" visible="false">
                        <br />
                        <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox txtheight2" Width="180px"
                            onkeypress="display1()" Style="font-family: 'Book Antiqua'" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtexcelname1"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                            InvalidChars="/\">
                        </asp:FilteredTextBoxExtender>
                        <asp:ImageButton ID="btnExcel1" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                            OnClick="btnExcel1_Click" />
                        <asp:ImageButton ID="btnprintmaster1" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                            OnClick="btnprintmaster1_Click" />
                        <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                    <br />
                </center>
                <center>
                    <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <br />
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:UpdatePanel ID="UpdatePanelbtn2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                                OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <%--progressBar for UpGo--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpGo">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
