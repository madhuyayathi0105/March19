<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="BankMaster.aspx.cs" Inherits="BankMaster" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <body>

        <script type="text/javascript">
            function valid2() {
                var idval = "";
                var empty = "";
                var id = "";
                var value1 = "";
                idval = document.getElementById("<%=txt_bkname.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_bkname.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                idval = document.getElementById("<%=txt_bankcode.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_bankcode.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                idval = document.getElementById("<%=txt_str.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_str.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                idval = document.getElementById("<%=ddl_dis.ClientID %>").value;
                if (idval.trim().toupper() == "SELECT") {
                    idval = document.getElementById("<%=ddl_dis.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                idval = document.getElementById("<%=txt_acholdrname.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_acholdrname.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                idval = document.getElementById("<%=txt_acnumbr.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_acnumbr.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                }

            }

            function valid1() {
                var idval = "";
                var empty = "";
                var id = "";
                var value1 = "";
                idval = document.getElementById("<%=txtchqno.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txtchqno.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                idval = document.getElementById("<%=txtleaf.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txtleaf.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                idval = document.getElementById("<%=txtstartno.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txtstartno.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                idval = document.getElementById("<%=txtendno.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txtendno.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                }
            }

            function getchqno(txt) {
                $.ajax({
                    type: "POST",
                    url: "BankMaster.aspx/Checkchequeno",
                    data: '{chqno:"' + txt + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnSuccess,
                    failure: function (response) {
                        alert(response);
                    }
                });
            }

            function OnSuccess(response) {
                var errmsg = $("#screrr")[0];
                switch (response.d) {
                    case "0":
                        errmsg.style.color = "green";
                        errmsg.innerHTML = "Not Exist";
                        break;
                    case "1":
                        errmsg.style.color = "green";
                        errmsg.innerHTML = "Exist";
                        document.getElementById('<%=txtchqno.ClientID %>').value = "";
                        break;
                    case "2":
                        errmsg.style.color = "red";
                        errmsg.innerHTML = "Please enter Cheque No";
                        break;
                    case "error":
                        errmsg.style.color = "red";
                        errmsg.innerHTML = "Error occurred";
                        break;
                }
            }


            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }


            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
        </script>    
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Bank Master</span></div>
            </center>
        </div>
        <center>
            <div class="maindivstyle">
                <%--maincontent--%>
                <center>
                    <div>
                        <br />
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lblcol" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcolload" runat="server" CssClass="textbox textbox1 ddlheight3"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlcolload_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_bankname" runat="server" Text="Bank Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UPbankName" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_bankname" runat="server" CssClass="textbox txtheight1" ReadOnly="true"
                                                onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_bankname" runat="server" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_bank" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_bank_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_bank" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_bank_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popBankName" runat="server" TargetControlID="txt_bankname"
                                                PopupControlID="panel_bankname" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_acname" runat="server" Text="Account Name"> </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_acname" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getaccname" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_acname"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_acno" runat="server" Text="Account Number"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_acno" runat="server" CssClass="textbox txtheight1"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getaccnumber" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_acno"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td colspan="2">
                                <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                                <ContentTemplate>
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="btngoUpdatePanel">
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
                                    <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1 textbox1" Text="Go"
                                        OnClick="btn_go_Click" />
                                    <asp:Button ID="btn_addnew" runat="server" CssClass="textbox btn2 textbox1" Text="Add New"
                                        OnClick="btn_addnew_Click" />
                                        </ContentTemplate>
                        </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <div>
                        <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                    </div>
                    <div id="div2" runat="server" visible="false" style="width: 900px; height: 300px;
                        overflow: auto; border: 1px solid Gray; border-radius: 10px; background-color: White;
                        box-shadow: 0px 0px 8px #999999;">
                        <br />
                        

                         <br />
                         <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                         <asp:GridView ID="gridview1" runat="server" ShowFooter="false" Width="750px"
                        AutoGenerateColumns="true" Font-Names="Book Antiqua" ShowHeader="false" toGenerateColumns="false" OnRowCreated="gridview1OnRowCreated" OnSelectedIndexChanged="gridview1_OnSelectedIndexChanged" >
                        <%-- --%>
                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
            </asp:GridView>

                    </div>
                    <br />
                    <div id="rptprint" runat="server" visible="false">
                        <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                            Visible="false"></asp:Label>
                        <br />
                        <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" CssClass="textbox textbox1 txtheight2"
                            onkeypress="display()"></asp:TextBox>
                        <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox textbox1 btn2"
                            Text="Export To Excel" Width="130px" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            CssClass="textbox textbox1 btn2" />
                        <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                </center>
                <br />
            </div>
            <%------mainpopup window1------%>
            <center>
                <div id="popwindow" runat="server" visible="false" class="popupstyle popupheight1 ">
                    <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                        width: 30px; position: absolute; margin-top: 74px; margin-left: 436px;" OnClick="imagebtnpopclose_Click" />
                    <br />
                    <br />
                    <br />
                    <br />
                    <div style="background-color: White; height: 500px; width: 900px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <div>
                                <span class="fontstyleheader" style="color: Green;">Bank Master</span></div>
                        </center>
                        <br />
                        <div id="div01" runat="server" class="container" visible="false" style="width: 808px; height:auto;">
                            <div class="col1">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblcollege" runat="server" Text="College Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlpopclg" runat="server" CssClass="textbox textbox1 ddlheight4"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlpopclg_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_accrdate" runat="server" Text="Account Creation Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_accrdate" runat="server" Height="20px" CssClass="textbox textbox1"
                                                OnTextChanged="txt_accrdate_OnTextChanged" AutoPostBack="true" Width="150px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_accrdate" runat="server"
                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_bankcode" runat="server" Text="Bank Code"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_bankcode" TextMode="SingleLine" runat="server" Height="20px"
                                                CssClass="textbox textbox1" Width="150px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_bankcode"
                                                FilterType="Custom,UppercaseLetters,LowercaseLetters,Numbers" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                            <span style="color: Red;">*</span>
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButton ID="rdo_cac" Text="Current Account" runat="server" Checked="true"
                                                GroupName="rdosame" />
                                            <asp:RadioButton ID="rdo_sac" Text="Savings Account" runat="server" GroupName="rdosame" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_bkname" runat="server" Text="Bank Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_bkname" runat="server" Height="20px" CssClass="textbox textbox1"
                                                Width="150px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="filterextender" runat="server" TargetControlID="txt_bkname"
                                                FilterType="Custom,UppercaseLetters,LowercaseLetters,Numbers" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                            <span style="color: Red;">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblbranch" runat="server" Text="Branch Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_branch" TextMode="SingleLine" runat="server" Height="20px" CssClass="textbox textbox1"
                                                Width="150px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_branch"
                                                FilterType="Custom,UppercaseLetters,LowercaseLetters,Numbers" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_str" runat="server" Text="Street"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_str" runat="server" Height="20px" CssClass="textbox textbox1"
                                                Width="150px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtextstr" runat="server" TargetControlID="txt_str"
                                                FilterType="Custom,UppercaseLetters,LowercaseLetters,Numbers" ValidChars=" ,,.,/">
                                            </asp:FilteredTextBoxExtender>
                                            <span style="color: Red;">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_acholdrname" runat="server" Text="Account Holder Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_acholdrname" runat="server" Height="20px" CssClass="textbox textbox1"
                                                Width="150px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtexthol" runat="server" TargetControlID="txt_acholdrname"
                                                FilterType="Custom,UppercaseLetters,LowercaseLetters" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                            <span style="color: Red;">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_area" runat="server" Text="District"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_dis" runat="server" CssClass="textbox textbox1 ddlheight4"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddl_dis_OnSelectedIndexChanged">
                                            </asp:DropDownList>
                                            <span style="color: Red;">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_acnumbr" runat="server" Text="Account Number"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_acnumbr" runat="server" Height="20px" CssClass="textbox textbox1"
                                                MaxLength="32" Width="150px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtextacnum" runat="server" TargetControlID="txt_acnumbr"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                            <span style="color: Red;">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_city" runat="server" Text="City"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_city" runat="server" Height="20px" CssClass="textbox textbox1"
                                                Width="150px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtextcity" runat="server" TargetControlID="txt_city"
                                                FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_mblno" runat="server" Text="Mobile Phone No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_mblno" runat="server" Height="20px" MaxLength="14" CssClass="textbox textbox1"
                                                Width="150px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtextmble" runat="server" TargetControlID="txt_mblno"
                                                FilterType="Custom,Numbers" ValidChars="+ ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_state" runat="server" Text="State"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_state" runat="server" CssClass="textbox textbox1 ddlheight4"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_ofcno" runat="server" Text="Office Phone No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_ofcno" runat="server" Height="20px" MaxLength="14" CssClass="textbox textbox1"
                                                Width="150px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtextoff" runat="server" TargetControlID="txt_ofcno"
                                                FilterType="Custom,Numbers" ValidChars="-">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_pincode" runat="server" Text="Pin Code"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_pincode" runat="server" Height="20px" MaxLength="6" CssClass="textbox textbox1"
                                                Width="150px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtextpin" runat="server" TargetControlID="txt_pincode"
                                                FilterType="Numbers" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_rtgs" runat="server" Text="RTGS Code"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_rtgs" runat="server" Height="20px" CssClass="textbox textbox1"
                                                Width="150px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtextrtgs" runat="server" TargetControlID="txt_rtgs"
                                                FilterType="Numbers,UppercaseLetters" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_ifsc" runat="server" Text="IFSC Code"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_ifsc" runat="server" Height="20px" CssClass="textbox textbox1"
                                                Width="150px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtextifsc" runat="server" TargetControlID="txt_ifsc"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_micr" runat="server" Text="MICR Code"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_micr" runat="server" Height="20px" CssClass="textbox textbox1"
                                                Width="150px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtextmicr" runat="server" TargetControlID="txt_micr"
                                                FilterType="Custom,Numbers,UppercaseLetters" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_saname1" runat="server" Text="Signing Authority Name1"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_saname1" runat="server" ReadOnly="true" Height="20px" CssClass="textbox textbox1"
                                                Width="150px"></asp:TextBox>
                                            <asp:Button ID="btn_saname1" runat="server" Text="?" CssClass="textbox textbox1 btn1"
                                                OnClick="btn_saname1_Click" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_saname2" runat="server" Text="Signing Authority Name2"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_saname2" runat="server" ReadOnly="true" Height="20px" CssClass="textbox textbox1"
                                                Width="150px"></asp:TextBox>
                                            <asp:Button ID="btn_saname2" runat="server" Text="?" CssClass="textbox btn1 textbox1"
                                                OnClick="btn_saname2_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="lnkadd" runat="server" Text="Add Cheque Book" OnClick="lbadd_click"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <br />
                        <center>
                            <div>
                                <asp:Button ID="btn_save" runat="server" CssClass="textbox textbox1 btn2" Visible="false"
                                    Text="Save" OnClientClick="return valid2()" OnClick="btn_save_Click" />
                                <asp:Button ID="btn_update" runat="server" CssClass="textbox textbox1 btn2" Visible="false"
                                    Text="Update" OnClientClick="return valid2()" OnClick="btn_update_Click" />
                                <asp:Button ID="btn_delete" runat="server" CssClass="textbox textbox1 btn2" Visible="false"
                                    Text="Delete" OnClientClick="return valid2()" OnClick="btn_delete_Click" />
                                <asp:Button ID="btn_exit" runat="server" CssClass="textbox textbox1 btn2" Text="Exit"
                                    OnClick="btn_exit_Click" />
                            </div>
                        </center>
                    </div>
                </div>
            </center>
            <%--------end of mainpopup-----%>
            <%---------popup sign1---------%>
            <center>
                <div id="popupsscode1" runat="server" visible="false" class="popupstyle popupheight1 ">
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                        width: 30px; position: absolute; margin-top: 9px; margin-left: 330px;" OnClick="imagebtnpopclose2_Click" />
                    <br />
                    <center>
                        <div style="background-color: White; height: 480px; width: 700px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <br />
                            <div>
                                <center>
                                    <span class="fontstyleheader" style="color: Green;">Select Signing Authority</span>
                                </center>
                            </div>
                            <br />
                            <center>
                                <div>
                                    <center>
                                        <div>
                                            <table class="maintablestyle">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblsearch1" runat="server" Text="Search By"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlsearch1" runat="server" CssClass="textbox1 ddlheight3" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddlsearch1_OnSelectedIndexChanged">
                                                            <asp:ListItem Text="Search By Name" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Search By Code" Value="1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtsearch1" runat="server" Visible="false" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch1"
                                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                            CompletionListItemCssClass="panelbackground">
                                                        </asp:AutoCompleteExtender>
                                                        <asp:TextBox ID="txtsearch1c" runat="server" Visible="false" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetdesCode" MinimumPrefixLength="0" CompletionInterval="100"
                                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch1c"
                                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                            CompletionListItemCssClass="panelbackground">
                                                        </asp:AutoCompleteExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btn_go2" runat="server" CssClass="textbox btn1 textbox1" Text="Go"
                                                            OnClick="btn_go2_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </center>
                                    <br />
                                    <div>
                                        <asp:Label ID="lbl_errorsearch" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                        <asp:Label ID="lbl_errorsearch1" runat="server" Visible="false" Font-Bold="true"
                                            ForeColor="Red"></asp:Label>
                                    </div>
                                    <div id="div3" runat="server" visible="true" style="width: 600px; height: 280px;
                                        overflow: auto; border: 1px solid Gray; background-color: White;">
                                        

                                        <asp:HiddenField ID="SelectedGridstaffCellIndex" runat="server" Value="-1" />
                         <asp:GridView ID="staff_gridview" runat="server" ShowFooter="false" Width="570px"
                        AutoGenerateColumns="true" Font-Names="Book Antiqua" ShowHeader="false" toGenerateColumns="false" OnRowCreated="staff_gridviewOnRowCreated" OnSelectedIndexChanged="staff_gridview_OnSelectedIndexChanged" >
                        <%-- --%>
                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
            </asp:GridView>
                                    </div>
                                    <br />
                                    <div id="Div4" runat="server" visible="true">
                                        <asp:Button ID="btn_save1" runat="server" Font-Bold="true" Visible="false" Font-Names="Book Antiqua"
                                            OnClick="btn_save1_Click" Font-Size="Medium" CssClass="textbox btn2" Text="Save"
                                            Width="60px" Height="35px" />
                                        <asp:Button ID="btn_exit2" runat="server" Text="Exit" OnClick="btn_exit2_Click" CssClass="textbox btn2 textbox1" />
                                    </div>
                                </div>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
            <%-----end of signpopup1-------%>
            <%---------popup sign2---------%>
            <center>
                <div id="popupsscode2" runat="server" visible="false" class="popupstyle popupheight1 ">
                    <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                        width: 30px; position: absolute; margin-top: 9px; margin-left: 330px;" OnClick="imagebtnpopclose3_Click" />
                    <br />
                    <center>
                        <div style="background-color: White; height: 480px; width: 700px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <br />
                            <div>
                                <center>
                                    <span class="fontstyleheader" style="color: Green;">Select Signing Authority</span>
                                </center>
                            </div>
                            <br />
                            <center>
                                <div>
                                    <center>
                                        <div>
                                            <table class="maintablestyle">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblsearch2" runat="server" Text="Search By"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlsearch2" runat="server" CssClass="textbox1 ddlheight3" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddlsearch2_OnSelectedIndexChanged">
                                                            <asp:ListItem Text="Search By Name" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Search By Code" Value="1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtsearch2" Visible="false" runat="server" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch2"
                                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                            CompletionListItemCssClass="panelbackground">
                                                        </asp:AutoCompleteExtender>
                                                        <asp:TextBox ID="txtsearch2c" runat="server" Visible="false" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetdesCode" MinimumPrefixLength="0" CompletionInterval="100"
                                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch2c"
                                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                            CompletionListItemCssClass="panelbackground">
                                                        </asp:AutoCompleteExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btngo3" runat="server" CssClass="textbox btn1 textbox1" Text="Go"
                                                            OnClick="btn_go3_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </center>
                                    <br />
                                    <div>
                                        <asp:Label ID="lblerr1" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                        <asp:Label ID="lblerr2" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label></div>
                                    <div id="div5" runat="server" visible="true" style="width: 600px; height: 280px;
                                        overflow: auto; border: 1px solid Gray; background-color: White;">
                                        
                                        <br />
                                         <asp:HiddenField ID="SelectedGridView2CellIndex" runat="server" Value="-1" />
                         <asp:GridView ID="GridView2" runat="server" ShowFooter="false" Width="570px"
                        AutoGenerateColumns="true" Font-Names="Book Antiqua" ShowHeader="false" toGenerateColumns="false" OnRowCreated="GridView2_OnRowCreated" OnSelectedIndexChanged="GridView2_OnSelectedIndexChanged" >
                        <%-- --%>
                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
            </asp:GridView>
                                    </div>
                                    <br />
                                    <div id="Div6" runat="server" visible="true">
                                        <asp:Button ID="btnsave" runat="server" Visible="false" OnClick="btnsave_click" CssClass="textbox btn2 textbox1"
                                            Text="Save" />
                                        <asp:Button ID="btnexit" runat="server" Text="Exit" OnClick="btnexit_click" CssClass="textbox btn2 textbox1" />
                                    </div>
                                </div>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
            <%---------end of signpop2-----%>
            <%----Popcheque----------------%>
            <center>
                <div id="popchq" runat="server" visible="false" class="popupstyle popupheight1 ">
                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                        width: 30px; position: absolute; margin-top: 73px; margin-left: 390px;" OnClick="imagebtnpopclosechq_Click" />
                    <br />
                    <br />
                    <br />
                    <br />
                    <div style="background-color: White; height: 520px; width: 820px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <span class="fontstyleheader" style="color: Green;">Cheque Book</span>
                        </center>
                        <br />
                        <center>
                            <div id="div9" runat="server" visible="false">
                                <table class="maintablestyle">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblchqbank" runat="server" Text="Bank Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtbankname" TextMode="SingleLine" runat="server" ReadOnly="true"
                                                CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblchqcode" runat="server" Visible="false" Text="Bank Code"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtbankcode" TextMode="SingleLine" runat="server" Visible="false"
                                                ReadOnly="true" Height="20px" CssClass="textbox textbox1" Width="120px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblchqacc" runat="server" Text="Account Type"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtchqacc" runat="server" ReadOnly="true" CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_savechq" runat="server" CssClass="textbox btn2 textbox1" Visible="false"
                                                Text="Add New" OnClientClick="return valid1()" OnClick="btn_savechq_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <asp:Label ID="lblchqerr" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                                <div id="div8" runat="server" visible="false" style="width: 800px; height: 300px;
                                    overflow: auto; border: 1px solid Gray; border-radius: 10px; background-color: White;
                                    box-shadow: 0px 0px 8px #999999;">
                                    <br />
                           

                                    <asp:HiddenField ID="SelectedGridView3CellIndex" runat="server" Value="-1" />
                         <asp:GridView ID="GridView3" runat="server" ShowFooter="false" Width="670px"
                        AutoGenerateColumns="true" Font-Names="Book Antiqua" ShowHeader="false" toGenerateColumns="false" OnRowCreated="GridView3_OnRowCreated" OnSelectedIndexChanged="GridView3_OnSelectedIndexChanged" >
                        <%-- --%>
                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
            </asp:GridView>
                                </div>
                                <br />
                                <center>
                                    <div>
                                        <asp:Button ID="btn_updatechq" runat="server" CssClass="textbox textbox1 btn2" Visible="false"
                                            Text="Update" OnClientClick="return valid1()" OnClick="btn_updatechq_Click" />
                                        <asp:Button ID="btn_deletechq" runat="server" CssClass="textbox textbox1 btn2" Visible="false"
                                            Text="Delete" OnClientClick="return valid1()" OnClick="btn_deletechq_Click" />
                                        <asp:Button ID="btn_exitchq" runat="server" CssClass="textbox textbox1 btn2" Text="Exit"
                                            OnClick="btn_exitchq_Click" />
                                    </div>
                                </center>
                            </div>
                        </center>
                    </div>
                </div>
            </center>
            <%------end of Popcheque-------%>
            <%----Popcheque Det------------%>
            <center>
                <div id="popchqdet" runat="server" visible="false" class="popupstyle popupheight1 ">
                    <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                        width: 30px; position: absolute; margin-top: 74px; margin-left: 290px;" OnClick="imagebtnpopclosechqdet_Click" />
                    <br />
                    <br />
                    <br />
                    <br />
                    <div style="background-color: White; height: 400px; width: 600px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <span class="fontstyleheader" style="color: Green;">Cheque Book Details</span>
                        </center>
                        <br />
                        <div id="div11" runat="server" visible="true">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblbkname" runat="server" Text="Bank Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtbkname" TextMode="SingleLine" runat="server" ReadOnly="true"
                                            Height="20px" CssClass="textbox textbox1" Width="150px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbnkchq" runat="server" Visible="false" Text="Bank Code"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtbnkchqcode" TextMode="SingleLine" runat="server" Visible="false"
                                            Height="20px" CssClass="textbox textbox1" Width="120px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblchqno" runat="server" Text="Cheque Book No"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtchqno" runat="server" MaxLength="20" onfocus="return myFunction(this)"
                                            onblur="return getchqno(this.value)" CssClass="textbox textbox1 txtheight1" Width="150px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtchqno"
                                            FilterType="Numbers" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                        <span style="color: Red;">*</span>
                                    </td>
                                    <td>
                                        <span style="font-weight: bold; font-size: medium;" id="screrr"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblnoofleaf" runat="server" Text="No Of Leaf"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtleaf" runat="server" Height="20px" MaxLength="6" OnTextChanged="txtleaf_OnTextChanged"
                                            AutoPostBack="true" CssClass="textbox textbox1" Width="150px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtleaf"
                                            FilterType="Numbers" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                        <span style="color: Red;">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblstartno" runat="server" Text="Start Number"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtstartno" runat="server" Height="20px" MaxLength="6" OnTextChanged="txtstartno_OnTextChanged"
                                            AutoPostBack="true" CssClass="textbox textbox1" Width="150px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtstartno"
                                            FilterType="Numbers" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                        <span style="color: Red;">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblendno" runat="server" Text="End Number"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtendno" runat="server" MaxLength="6" CssClass="textbox textbox1 txtheight1"
                                            Width="150px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtendno"
                                            FilterType="Numbers" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                        <span style="color: Red;">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblrecveddate" runat="server" Text="Received Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtrevddate" runat="server" Height="20px" CssClass="textbox textbox1"
                                            Width="150px"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtrevddate" runat="server"
                                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblstaff" runat="server" Text="Received Staff"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtstaff" runat="server" ReadOnly="false" Height="20px" CssClass="textbox textbox1"
                                            Width="225px"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getstaffname" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtstaff"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <asp:Button ID="btn_staff" runat="server" Text="?" CssClass="textbox textbox1 btn1"
                                            OnClick="btn_staff_Click" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <center>
                                <div>
                                    <asp:Button ID="btn_savechqdet" runat="server" CssClass="textbox textbox1 btn2" Visible="false"
                                        Text="Save" OnClientClick="return valid1()" OnClick="btn_savechqdet_Click" />
                                    <asp:Button ID="btn_updatechqdet" runat="server" CssClass="textbox textbox1 btn2"
                                        Visible="false" Text="Update" OnClientClick="return valid1()" OnClick="btn_updatechqdet_Click" />
                                    <asp:Button ID="btn_deletechqdet" runat="server" CssClass="textbox textbox1 btn2"
                                        Visible="false" Text="Delete" OnClientClick="return valid1()" OnClick="btn_deletechqdet_Click" />
                                    <asp:Button ID="btn_exitchqdet" runat="server" CssClass="textbox textbox1 btn2" Text="Exit"
                                        OnClick="btn_exitchqdet_Click" />
                                </div>
                            </center>
                        </div>
                    </div>
                </div>
            </center>
            <%----end Popcheque------------%>
            <%----Popcheque spread---------%>
            <center>
                <div id="popchqstaff" runat="server" visible="false" class="popupstyle popupheight1 ">
                    <asp:ImageButton ID="imgpopchq" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                        width: 30px; position: absolute; margin-top: 9px; margin-left: 300px;" OnClick="imagebtnpopclosestaff_Click" />
                    <br />
                    <center>
                        <div style="background-color: White; height: 480px; width: 620px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <br />
                            <div>
                                <center>
                                    <span class="fontstyleheader" style="color: Green;">Select Signing Authority</span>
                                </center>
                            </div>
                            <br />
                            <center>
                                <div>
                                    <center>
                                        <table class="maintablestyle" width="400px">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblsearchchq" runat="server" Text="Search By"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtsearch3" runat="server" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch3"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btngostaff" runat="server" CssClass="textbox btn1 textbox1" Text="Go"
                                                        OnClick="btngostaff_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                    <br />
                                    <div>
                                        <asp:Label ID="lblerrchq1" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                        <asp:Label ID="lblerrchq2" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                    </div>
                                    <div id="div13" runat="server" visible="true" style="width: 600px; height: 280px;
                                        overflow: auto; border: 1px solid Gray; background-color: White;">
                                        

                                        <asp:HiddenField ID="SelectedGridView4CellIndex" runat="server" Value="-1" />
                         <asp:GridView ID="GridView4" runat="server" ShowFooter="false" Width="570px"
                        AutoGenerateColumns="true" Font-Names="Book Antiqua" ShowHeader="false" toGenerateColumns="false" OnRowCreated="GridView4_OnRowCreated" OnSelectedIndexChanged="GridView4_OnSelectedIndexChanged" >
                        <%-- --%>
                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
            </asp:GridView>
                                    </div>
                                    <br />
                                    <div id="Div14" runat="server" visible="true">
                                        <asp:Button ID="Button2" runat="server" Visible="false" OnClick="btnsave_click" CssClass="textbox btn2 textbox1"
                                            Text="Save" />
                                        <asp:Button ID="btnexitchq" runat="server" Text="Exit" OnClick="btnexitchq_click"
                                            Font-Names="Book Antiqua" CssClass="textbox btn2 textbox1" />
                                    </div>
                                </div>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
            <%----end spread Popcheque-----%>
            <%----alert div2---------------%>
            <center>
                <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 300px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
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
                                                <asp:Button ID="btn_errorclose" CssClass="textbox btn1" Style="height: 28px; width: 65px;"
                                                    OnClick="btn_errorclose_Click" Text="ok" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
            <%----end of alert div2--------%>
            <%----alert div1---------------%>
            <center>
                <div id="imgdiv1" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div7" runat="server" class="table" style="background-color: White; height: 150px;
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
                                                <asp:Button ID="btnyes" CssClass="textbox btn1" Style="height: 28px; width: 65px;"
                                                    OnClick="btnyes_Click" Text="Yes" runat="server" />
                                                <asp:Button ID="btnno" CssClass="textbox btn1" Style="height: 28px; width: 65px;"
                                                    OnClick="btnno_Click" Text="No" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
            <%----end of alert div1--------%>
            <%----alert div0---------------%>
            <center>
                <div id="imgDiv0" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div12" runat="server" class="table" style="background-color: White; height: 150px;
                            width: 300px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 150px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblchqerrdet" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnchqyes" CssClass="textbox btn1" Style="height: 28px; width: 65px;"
                                                    OnClick="btnyeschq_Click" Text="Yes" runat="server" />
                                                <asp:Button ID="btnchqno" CssClass="textbox btn1" Style="height: 28px; width: 65px;"
                                                    OnClick="btnnochq_Click" Text="No" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
            <%----end of alert div0--------%>
        </center>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExcel" />
            <asp:PostBackTrigger ControlID="btnprintmaster" />
            
        </Triggers>
    </asp:UpdatePanel>
    </body>
    </html>
</asp:Content>
