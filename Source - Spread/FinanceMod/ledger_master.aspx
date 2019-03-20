<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ledger_master.aspx.cs" Inherits="ledger_master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title>Ledger Master</title>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <style type="text/css">
        .stNew
        {
            text-transform: uppercase;
        }
    </style>
    <body>
        <script type="text/javascript">
            function Test() {
                var id = "";
                var value1 = "";
                var idval = "";
                var empty = "";
                id = document.getElementById("<%=txt_ledgername1.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_ledgername1.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_ledgeracr.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_ledgeracr.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=ddl_college.ClientID %>");
                value1 = id.options[id.selectedIndex].text;
                if (value1.trim().toUpperCase() == "SELECT") {
                    empty = "E";
                    id = document.getElementById("<%=ddl_college.ClientID %>");
                    id.style.borderColor = 'Red';
                }
                id = document.getElementById("<%=ddl_actype.ClientID %>");
                value1 = id.options[id.selectedIndex].text;
                if (value1.trim().toUpperCase() == "SELECT") {
                    empty = "E";
                    id = document.getElementById("<%=ddl_actype.ClientID %>");
                    id.style.borderColor = 'Red';
                }
                id = document.getElementById("<%=txtheader.ClientID %>");
                // value1 = id.options[id.selectedIndex].text;
                if (id.value.trim() == "--Select--") {
                    empty = "E";
                    id = document.getElementById("<%=txtheader.ClientID %>");
                    id.style.borderColor = 'Red';
                }


                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                }
            }

            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }

            function getled(txt) {
                $.ajax({
                    type: "POST",
                    url: "ledger_master.aspx/checkLedgeName",
                    data: '{ledgername: "' + txt + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: Success,
                    failure: function (response) {
                        alert(response);
                    }
                });
            }
            function Success(response) {
                var mesg1 = $("#screrr")[0];
                switch (response.d) {
                    case "0":
                        mesg1.style.color = "green";
                        mesg1.innerHTML = "Not Exist";
                        break;
                    case "1":
                        mesg1.style.color = "green";
                        document.getElementById('<%=txt_ledgername1.ClientID %>').value = "";
                        mesg1.innerHTML = "Exist";
                        break;
                    case "2":
                        mesg1.style.color = "red";
                        mesg1.innerHTML = "Enter LedgerName";
                        break;
                    case "error":
                        mesg1.style.color = "red";
                        mesg1.innerHTML = "Error Occurred";
                        break;
                }
            }

            function getledacr(txt) {
                $.ajax({
                    type: "POST",
                    url: "ledger_master.aspx/checkLedgeacr",
                    data: '{ledgeracr: "' + txt + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: onSuccess,
                    failure: function (response) {
                        alert(response);
                    }
                });
            }
            function onSuccess(response) {
                var mesg2 = $("#scracrerr")[0];
                switch (response.d) {
                    case "0":
                        mesg2.style.color = "green";
                        mesg2.innerHTML = "Not Exist";
                        break;
                    case "1":
                        mesg2.style.color = "green";
                        document.getElementById('<%=txt_ledgeracr.ClientID %>').value = "";
                        mesg2.innerHTML = "Exist";
                        break;
                    case "2":
                        mesg2.style.color = "red";
                        mesg2.innerHTML = "Enter LedgerAcronym";
                        break;
                    case "error":
                        mesg2.style.color = "red";
                        mesg2.innerHTML = "Error Occurred";
                        break;
                }
            }
        
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <center>
                    <br />
                    <div>
                        <asp:Label ID="Label1" runat="server" class="fontstyleheader" Style="color: Green;"
                            Text="Ledger Master"></asp:Label>
                        <br />
                    </div>
                </center>
                <div class="maindivstyle" style="width: 1000px; height: auto;">
                    <table class="maintablestyle" style="width: 800px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblcol" runat="server" Text="College"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlcol" runat="server" CssClass="textbox textbox1 ddlheight5"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlcol_OnSelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_ledgername" runat="server" Text="Ledger"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_ledgername" runat="server" CssClass="textbox  txtheight1" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pbatch" runat="server" Width="200px" CssClass="multxtpanel multxtpanleheight">
                                            <asp:CheckBox ID="cb_ledgername" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_ledgername_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_ledgername" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_ledgername_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txt_ledgername"
                                            PopupControlID="pbatch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblacctyp" runat="server" Text="Account Type"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtacctyp" runat="server" CssClass="textbox  txtheight1" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pacctyp" runat="server" Width="110px" Height="100px" CssClass="multxtpanel multxtpanleheight">
                                            <asp:CheckBox ID="cbacctyp" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cbacctyp_CheckedChanged" />
                                            <asp:CheckBoxList ID="cblacctyp" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblacctyp_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtacctyp"
                                            PopupControlID="pacctyp" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_searchby" runat="server" Text="Search By"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddl_type" runat="server" OnSelectedIndexChanged="ddl_type_OnSelectedIndexChanged"
                                    CssClass="textbox  ddlheight" AutoPostBack="True">
                                    <asp:ListItem Value="0">Ledger</asp:ListItem>
                                    <asp:ListItem Value="1">Header</asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txt_searchby" Visible="false" runat="server" CssClass="textbox  txtheight4"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchby"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                                <asp:TextBox ID="txt_header" Visible="false" runat="server" CssClass="textbox  txtheight4"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getheader" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_header"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkPriority" runat="server" AutoPostBack="true" OnCheckedChanged="chkPriority_OnCheckedChanged" />Priority
                            </td>
                            <td>
                                <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1 textbox1" Text="Go"
                                    OnClick="btn_go_Click" />
                                <asp:Button ID="btn_addnew" runat="server" Text="Add New" CssClass="textbox btn2 textbox1"
                                    OnClick="btn_addnew_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Style="float: left; position: absolute;
                        left: 260px;" Visible="false"></asp:Label>
                    <center>
                        <div id="div1" runat="server" visible="false" style="width: 900px;">
                            <FarPoint:FpSpread ID="Fpspread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="1px" Width="750px" Style="overflow: auto; background-color: White;"
                                OnCellClick="Cell_Click" OnPreRender="Fpspread1_render" OnButtonCommand="FpSpread1_ButtonCommand">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                    </center>
                    <div id="divPriorityBtns" runat="server" visible="false" style="padding-top: 5px;">
                        <asp:Button ID="btnSetPriority" runat="server" Text="Set Priority" CssClass=" textbox textbox1 btn2"
                            Width="100px" OnClick="btnSetPriority_Click" />
                        <asp:Button ID="btnResetPriority" runat="server" Text="Reset" CssClass=" textbox textbox1 btn2"
                            OnClick="btnResetPriority_Click" />
                    </div>
                    <div id="rptprint" runat="server" visible="false">
                        <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                            Visible="false"></asp:Label>
                        <br />
                        <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" CssClass="textbox textbox1 txtheight3"
                            onkeypress="display()"></asp:TextBox>
                        <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox textbox1 btn2"
                            Text="Export To Excel" Width="130px" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            CssClass="textbox textbox1 btn2" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                    <br />
                </div>
            </div>
        </center>
        <center>
            <div id="poperrjs" runat="server" visible="false" style="height: 50em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 28px; margin-left: 330px;"
                    OnClick="imagebtnpopclose_Click" />
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; overflow: auto; width: 700px;
                    height: 500px;" align="center">
                    <br />
                    <center>
                        <asp:Label ID="lbl_ledgermaster" runat="server" class="fontstyleheader" Style="color: Green;"
                            Text="Ledger Master"></asp:Label>
                    </center>
                    <div align="left" style="overflow: auto; width: 600px; height: 410px; border-radius: 10px;">
                        <br />
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_collegename" Text="College Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_college" runat="server" CssClass="textbox textbox1 ddlheight5"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_college_OnSelectedIndexChanged"
                                            onfocus="return myFunction(this)">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_ledgername1" runat="server" Text="Ledger Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_ledgername1" TextMode="SingleLine" runat="server" CssClass="textbox textbox1 txtheight4"
                                            onfocus="return myFunction(this)" onblur="return getled(this.value)"></asp:TextBox>
                                        <span style="color: Red;">*</span><span style="font-weight: bold; font-size: larger;"
                                            id="screrr"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_ledgeracr" runat="server" Text="Ledger Acronym"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_ledgeracr" TextMode="SingleLine" runat="server" CssClass="textbox textbox1 txtheight4 stNew"
                                            MaxLength="6" onfocus="return myFunction(this)" onblur="return getledacr(this.value)"></asp:TextBox>
                                        <span style="color: Red;">*</span><span style="font-weight: bold; font-size: larger;"
                                            id="scracrerr"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_actype" runat="server" Text="Account Type"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_actype" onfocus="return myFunction(this)" runat="server"
                                            CssClass="textbox ddlheight" Width="120px" AutoPostBack="true">
                                        </asp:DropDownList>
                                       
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="indcr" runat="server" Text="(Cr - Credit, Dr - Debit,Both)"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblaccheader" runat="server" Text="Account Header"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtheader" runat="server" CssClass="textbox textbox1 txtheight4"
                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                <span style="color: Red;">*</span>
                                                <asp:Panel ID="pnlheader" runat="server" CssClass="multxtpanel multxtpanleheight1">
                                                    <asp:CheckBox ID="cbheader" runat="server" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cbheader_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cblheader" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblheader_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtheader"
                                                    PopupControlID="pnlheader" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_openbal" runat="server" Visible="true" Text="Opening Balance"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_openbal" CssClass="textbox textbox1 txtheight2" Visible="true"
                                            runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_openbal"
                                            FilterType="Numbers,Custom" ValidChars=". ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_group" runat="server" Text="Group"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_group" TextMode="SingleLine" ReadOnly="true" runat="server"
                                            CssClass="textbox textbox1 txtheight4" onfocus="return myFunction(this)"></asp:TextBox>
                                        <asp:Button ID="btn_group" runat="server" Text="?" CssClass="textbox textbox1 btn1"
                                            OnClick="btn_group_click" />
                                        <span style="color: Red;">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_desc" runat="server" Text="Description"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_desc" TextMode="MultiLine" CssClass="textbox textbox1 txtheight5"
                                            runat="server" Width="300px" Height="75px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_desc"
                                            FilterType="Custom,UppercaseLetters,LowercaseLetters" ValidChars=",-. ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <%-- <tr>
                                    <td>
                                        <asp:Label ID="lbl_OpeningBalance" runat="server" Text="Description"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtopenbal"  CssClass="textbox textbox1 txtheight5"
                                            runat="server" Width="300px" Height="75px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_desc"
                                            FilterType="numbers" ValidChars=",. ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_feetype" runat="server" Text="Fee Type"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:RadioButton ID="rdb_tufee" Text="Term Fees" runat="server" Checked="true" GroupName="same" />
                                        <asp:RadioButton ID="rdb_otfee" Text="Other Fees" runat="server" GroupName="same" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <br />
                                        <center>
                                            <asp:Button ID="btn_update" runat="server" Text="Update" CssClass="textbox textbox1 btn2"
                                                Visible="false" OnClientClick="return Test()" OnClick="btn_update_Click" />
                                            <asp:Button ID="btn_delete" runat="server" Text="Delete" CssClass="textbox textbox1 btn2"
                                                Visible="false" OnClientClick="return Test()" OnClick="btn_delete_Click" />
                                            <asp:Button ID="btn_save" runat="server" CssClass="textbox textbox1 btn2" Text="Save"
                                                OnClientClick="return Test()" OnClick="btn_save_Click" />
                                            <asp:Button ID="btn_exit" runat="server" CssClass="textbox textbox1 btn2" Text="Exit"
                                                OnClick="btn_exit_Click" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </div>
            </div>
        </center>
        <center>
            <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                            <asp:Button ID="btn_errorclose" CssClass="textbox textbox1 btn1" OnClick="btn_errorclose_Click"
                                                Text="OK" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="imgdiv1" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div4" runat="server" class="table" style="background-color: White; height: 150px;
                        width: 300px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 150px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblalert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btnyes" CssClass="textbox btn1 textbox1" OnClick="btnyes_Click" Text="Yes"
                                                runat="server" />
                                            <asp:Button ID="btnno" CssClass="textbox btn1 textbox1" OnClick="btnno_Click" Text="No"
                                                runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="poppergroup" runat="server" visible="false" class="popupstyle popupheight1 ">
                <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                    width: 30px; position: absolute; margin-top: 9px; margin-left: 190px;" OnClick="imagebtnpopclose5_Click" />
                <br />
                <center>
                    <div style="background-color: White; height: 400px; width: 400px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <div>
                            <center>
                                <span style="color: Green;" class="fontstyleheader">Group Name List</span>
                            </center>
                        </div>
                        <center>
                            <div style="width: 370px; height: 330px;">
                                <center>
                                    <div>
                                        <br />
                                        <asp:Label ID="lblerr4" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                        <asp:Panel ID="panel3" runat="server" BorderColor="Black" BackColor="AliceBlue" Visible="false"
                                            BorderWidth="2px" Height="270px" Style="overflow: auto;">
                                            <br />
                                            <div class="PopupHeaderrstud2" id="Div2" style="text-align: center; font-family: MS Sans Serif;
                                                font-size: Small; font-weight: bold">
                                                <asp:TreeView ID="TreeView1" runat="server" HoverNodeStyle-ForeColor="Red" ExpandDepth="0"
                                                    ShowLines="true" ShowExpandCollapse="true" ForeColor="Green" LeafNodeStyle-ForeColor="Black"
                                                    OnTreeNodeDataBound="TreeView1_DataBound" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged"
                                                    AutoPostBack="true">
                                                </asp:TreeView>
                                            </div>
                                        </asp:Panel>
                                        <br />
                                    </div>
                                </center>
                                <asp:Button ID="btnexitgrp" runat="server" Text="Exit" CssClass="textbox btn2 textbox1"
                                    OnClick="btnexitgrp_click" />
                            </div>
                        </center>
                    </div>
                </center>
            </div>
        </center>
    </body>
    </html>
</asp:Content>
