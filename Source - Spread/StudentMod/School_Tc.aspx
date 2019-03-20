<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="School_Tc.aspx.cs" Inherits="StudentMod_School_Tc" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Tc</title>
    <style type="text/css">
        p
        {
            font-size: medium;
            margin: 0px;
            padding: 0px;
            border: 0px;
        }
    </style>
    <script type="text/javascript">
        function autoComplete1_OnClientPopulating(sender, args) {
            var collegecode = document.getElementById("<%=ddlcollege.ClientID%>").value;
            sender.set_contextKey(collegecode);
        }
        function autoComplete2_OnClientPopulating(sender, args) {
            var collegecode = document.getElementById("<%=ddlcollege.ClientID%>").value;
            sender.set_contextKey(collegecode);
        }
        function autoComplete3_OnClientPopulating(sender, args) {
            var SEARCHTYPE = document.getElementById("<%=ddl_searchtype.ClientID %>").value;
            //var SEARCHTYPE = skillsSelect.options[skillsSelect.selectedIndex].value;
            sender.set_contextKey(SEARCHTYPE);
        }
        function funation1(id) {
            var nation = id.value;
            if (nation.trim().toUpperCase() == "OTHERS") {
                var idvalue = document.getElementById("<%=txt_othernationality1.ClientID %>");
                idvalue.style.display = "block";
            }
            else {
                var idvalue = document.getElementById("<%=txt_othernationality1.ClientID %>");
                idvalue.style.display = "none";
            }
        }
        function oncomm1(id) {
            var value1 = id.options[id.selectedIndex].text;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idvalue = document.getElementById("<%=txtCommunity1.ClientID %>");
                idvalue.style.display = "block";
            }
            else {
                var idvalue = document.getElementById("<%=txtCommunity1.ClientID %>");
                idvalue.style.display = "none";
            }
        }
        function onreli1(id) {
            var value1 = id.options[id.selectedIndex].text;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idvalue = document.getElementById("<%=txt_otherreligion1.ClientID %>");
                idvalue.style.display = "block";
                idvalue.style.color = "Gray";
            }
            else if (value1.trim().toUpperCase() == "CHRISTIAN") {
                var idvalue2 = document.getElementById("<%=txt_otherreligion1.ClientID %>");
                idvalue2.style.display = "none";
            }
            else {
                var idvalue = document.getElementById("<%=txt_otherreligion1.ClientID %>");
                idvalue.style.display = "none";
            }
        }
        function funation(id) {
            var nation = id.value;
            if (nation.trim().toUpperCase() == "OTHERS") {
                var idvalue = document.getElementById("<%=txt_othernationality.ClientID %>");
                idvalue.style.display = "block";
            }
            else {
                var idvalue = document.getElementById("<%=txt_othernationality.ClientID %>");
                idvalue.style.display = "none";
            }
        }
        function oncomm(id) {
            var value1 = id.options[id.selectedIndex].text;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idvalue = document.getElementById("<%=txtCommunity.ClientID %>");
                idvalue.style.display = "block";
            }
            else {
                var idvalue = document.getElementById("<%=txtCommunity.ClientID %>");
                idvalue.style.display = "none";
            }
        }
        function onreli(id) {
            var value1 = id.options[id.selectedIndex].text;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idvalue = document.getElementById("<%=txt_otherreligion.ClientID %>");
                idvalue.style.display = "block";
                idvalue.style.color = "Gray";
            }
            else if (value1.trim().toUpperCase() == "CHRISTIAN") {
                var idvalue2 = document.getElementById("<%=txt_otherreligion.ClientID %>");
                idvalue2.style.display = "none";
            }
            else {
                var idvalue = document.getElementById("<%=txt_otherreligion.ClientID %>");
                idvalue.style.display = "none";
            }
        }
        function FunctionCaste(id) {
            var value1 = id.options[id.selectedIndex].text;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idvalue = document.getElementById("<%=txtCasteOther.ClientID %>");
                idvalue.style.display = "block";
                //idvalue.value = "Other Caste";
                idvalue.style.color = "Gray";
            }
            else {
                var idvalue = document.getElementById("<%=txtCasteOther.ClientID %>");
                idvalue.style.display = "none";
            }
        }
        function FunctionCaste1(id) {
            var value1 = id.options[id.selectedIndex].text;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idvalue = document.getElementById("<%=txt_caste1.ClientID %>");
                idvalue.style.display = "block";
                //idvalue.value = "Other Caste";
                idvalue.style.color = "Gray";
            }
            else {
                var idvalue = document.getElementById("<%=txt_caste1.ClientID %>");
                idvalue.style.display = "none";
            }
        }
        function Functionattendance(id) {
            var value1 = id.options[id.selectedIndex].text;
            if (value1.trim().toUpperCase() == "OTHERS") {
                var idvalue = document.getElementById("<%=txt_attendance.ClientID %>");
                idvalue.style.display = "block";
                //idvalue.value = "Other Caste";
                idvalue.style.color = "Gray";
            }
            else {
                var idvalue = document.getElementById("<%=txt_attendance.ClientID %>");
                idvalue.style.display = "none";
            }
        }
        function blurFunction(x) {
            x.style.borderColor = "#c4c4c4";
        }
        function PrintDiv() {
            var panel = document.getElementById("<%=contentDiv.ClientID %>");
            var printWindow = window.open('', '', 'height=auto,width=1191');
            printWindow.document.write('<html');
            printWindow.document.write('<head> <style type="text/css"> p{ font-size: x-small;margin: 0px; padding: 0px; border: 0px;  } body{ margin:0px;}</style>');
            printWindow.document.write('</head><body>');
            printWindow.document.write('<form>');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write(' </form>');
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <span class="fontstyleheader" style="color: Green; font-size: x-large;">TRANSFER CERTIFICATE</span>
        </div>
        <br />
    </center>
    <div>
        <asp:Label ID="lbl_clgT" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="lbl_degreeT" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="lbl_branchT" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="lbl_semT" runat="server" Visible="false"></asp:Label>
        <center>
            <table class="maintablestyle">
                <tr>
                    <td>
                        <asp:Label ID="lbl_clgname" runat="server" Text="College"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlcollege" CssClass="ddlheight4 textbox1" runat="server" AutoPostBack="true"
                            Width="215px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbl_batch" runat="server" Text="Batch"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_batch" runat="server" CssClass="ddlheight textbox1" AutoPostBack="true"
                            OnSelectedIndexChanged="ddl_batch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbl_degree" Text="Degree" Width="89px" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  textbox1 txtheight3"
                                    Width="165px" ReadOnly="true">-- Select--</asp:TextBox>
                                <asp:Panel ID="p3" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="120px" Style="position: absolute;">
                                    <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="true"
                                        OnCheckedChanged="cb_degree_checkedchange" />
                                    <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender18" runat="server" TargetControlID="txt_degree"
                                    PopupControlID="p3" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lbl_branch" Text="Branch" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_branch" runat="server" CssClass="textbox textbox1 txtheight3"
                                    ReadOnly="true">-- Select--</asp:TextBox>
                                <asp:Panel ID="p4" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="180px" Style="position: absolute;">
                                    <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="true"
                                        OnCheckedChanged="cb_branch_checkedchange" />
                                    <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender19" runat="server" TargetControlID="txt_branch"
                                    PopupControlID="p4" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_org_sem" Text="Semester" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox textbox1 txtheight3" ReadOnly="true">-- Select--</asp:TextBox>
                                <asp:Panel ID="Panel11" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                    <asp:CheckBox ID="cb_sem" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_sem_checkedchange" />
                                    <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender20" runat="server" TargetControlID="txt_sem"
                                    PopupControlID="Panel11" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblFormat" runat="server" Text="Format"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlAppFormat" CssClass="ddlheight textbox1" runat="server"
                            Width="123px">
                            <%--  <asp:ListItem Value="0">CBSE</asp:ListItem>
                            <asp:ListItem Value="1">SSLC</asp:ListItem>
                            <asp:ListItem Value="2">HSC</asp:ListItem>
                            <asp:ListItem Value="3">TRANSFER CERTIFICATE</asp:ListItem>
                            <asp:ListItem Value="4">MIGRATION CERTIFICATE</asp:ListItem>--%>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbl_searchstudname" runat="server" Text="Student Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_searchstudname" placeholder="Student Name" runat="server" CssClass="textbox textbox1 txtheight2"
                            Width="165px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_searchstudname"
                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .">
                        </asp:FilteredTextBoxExtender>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="getstudentadmit" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchstudname"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="panelbackground">
                        </asp:AutoCompleteExtender>
                    </td>
                    <td>
                        <%-- <asp:Label ID="lbl_searchappno" runat="server" Text="Admission No"></asp:Label>--%>
                        <asp:DropDownList ID="ddl_searchtype" runat="server" CssClass="textbox  ddlheight"
                            AutoPostBack="true" OnSelectedIndexChanged="ddl_searchtype_OnSelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_searchappno" runat="server" CssClass="textbox textbox1 txtheight1"
                            Width="135px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender02" runat="server" TargetControlID="txt_searchappno"
                            FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=".-/ ">
                        </asp:FilteredTextBoxExtender>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" Enabled="True"
                            ServiceMethod="getappfrom" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                            CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchappno" CompletionListCssClass="autocomplete_completionListElement"
                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground"
                            UseContextKey="true" OnClientPopulating="autoComplete3_OnClientPopulating" DelimiterCharacters="">
                        </asp:AutoCompleteExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btn_go" runat="server" Text="Go" CssClass="textbox textbox1 btn1"
                            OnClick="btn_go_OnClick" />
                    </td>
                    <td>
                        <asp:Button ID="btn_addnew" runat="server" Text="Add New" CssClass="textbox textbox1 btn2"
                            OnClick="btn_addnew_OnClick" />
                    </td>
                    <td colspan="3">
                        <asp:RadioButton ID="rdb_general" Text="General" runat="server" GroupName="tc" />
                        <asp:RadioButton ID="rdb_Request" Text="Request" runat="server" GroupName="tc" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label>
            <br />
            <center>
                <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Style="height: 370px; overflow: auto; background-color: White;
                    border-radius: 10px; box-shadow: 0px 0px 8px #999999" OnButtonCommand="fp_btn_Click"
                    ShowHeaderSelection="false" Visible="false" OnUpdateCommand="FpSpread1_Command">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
            </center>
            <br />
            <center>
                <asp:Button ID="btn_print" Visible="false" runat="server" Text="Print" CssClass="textbox btn2"
                    OnClick="btn_print_Click" />
                <%--<asp:Button ID="btnprintback" Visible="false" runat="server" Text="Print Backside" CssClass="textbox btn2"
                    OnClick="btn_print_Click" />--%>
            </center>
            <div id="pop_studdetails" runat="server" visible="false" style="height: 48em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 13px; margin-left: 471px;"
                    OnClick="imagebtnpopclose3_Click" />
                <br />
                <div style="background-color: White; height: auto; width: 960px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <div style="position: absolute; margin-top: -6px; margin-left: 29px;">
                        <center>
                            <asp:Label ID="lbl_time" runat="server" Font-Names="Viner Hand ITC" ForeColor="#DF95FB"></asp:Label>
                        </center>
                    </div>
                    <center>
                        <span style="color: Green;" class="fontstyleheader ">TRANSFER CERTIFICATE</span>
                    </center>
                    <table>
                        <tr>
                            <td>
                                Admission No
                            </td>
                            <td>
                                <asp:TextBox ID="txt_admissionno" placeholder="Admission No" CssClass="textbox textbox1 txtheight4"
                                    runat="server" OnTextChanged="txt_admissionno_Onchange" AutoPostBack="true"></asp:TextBox>
                                <asp:Label ID="lbl_app_no" Visible="false" runat="server"></asp:Label>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="getadmissionnoname" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_admissionno"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground" UseContextKey="true" OnClientPopulating="autoComplete1_OnClientPopulating">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td colspan="3">
                                Affiliation No
                                <asp:TextBox ID="txt_affilicationno" CssClass="textbox textbox1 txtheight1" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Name of the Student
                            </td>
                            <td>
                                <asp:TextBox ID="txt_studname" CssClass="textbox textbox1 txtheight4" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Qualified for promotion
                            </td>
                            <td>
                                <asp:TextBox ID="txt_qualified" CssClass="textbox textbox1 txtheight4" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Mother's Name
                            </td>
                            <td>
                                <asp:TextBox ID="txt_mothername" CssClass="textbox textbox1 txtheight4" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Paid school dues
                            </td>
                            <td>
                                <asp:TextBox ID="txt_paidschool" CssClass="textbox textbox1 txtheight4" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Father's Name
                            </td>
                            <td>
                                <asp:TextBox ID="txt_fathername" CssClass="textbox textbox1 txtheight4" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Fee concession
                            </td>
                            <td>
                                <asp:TextBox ID="txt_feecon" CssClass="textbox textbox1 txtheight4" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Guardian's Name
                            </td>
                            <td>
                                <asp:TextBox ID="txt_GuardianName" CssClass="textbox textbox1 txtheight4" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Total No of working days
                            </td>
                            <td>
                                <asp:TextBox ID="txt_totalnoofworkingdays" CssClass="textbox textbox1 txtheight4"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                DOB
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldobdate" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                    onfocus="myFunction(this)" Style="width: 47px;">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddldobMonth" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                    Style="width: 60px;">
                                    <asp:ListItem Value="00">MMM</asp:ListItem>
                                    <asp:ListItem Value="01">JAN</asp:ListItem>
                                    <asp:ListItem Value="02">FEB</asp:ListItem>
                                    <asp:ListItem Value="03">MAR</asp:ListItem>
                                    <asp:ListItem Value="04">APR</asp:ListItem>
                                    <asp:ListItem Value="05">MAY</asp:ListItem>
                                    <asp:ListItem Value="06">JUN</asp:ListItem>
                                    <asp:ListItem Value="07">JUL</asp:ListItem>
                                    <asp:ListItem Value="08">AUG</asp:ListItem>
                                    <asp:ListItem Value="09">SEP</asp:ListItem>
                                    <asp:ListItem Value="10">OCT</asp:ListItem>
                                    <asp:ListItem Value="11">NOV</asp:ListItem>
                                    <asp:ListItem Value="12">DEC</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddldobYear" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                    Style="width: 59px;">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Total No of working days present
                            </td>
                            <td>
                                <asp:TextBox ID="txt_totalnoofworkingdayspresent" CssClass="textbox textbox1 txtheight4"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Nationality
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcountry" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                    onfocus="myFunction(this)" onchange="return funation(this)" Style="width: 160px;
                                    float: left;">
                                </asp:DropDownList>
                                <br />
                                <asp:TextBox ID="txt_othernationality" CssClass="textbox textbox1" Visible="true"
                                    MaxLength="50" runat="server" onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);"
                                    onfocus="myFunction(this)" placeholder="Other Nationality" Style="width: 150px;
                                    float: left; display: none;"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" TargetControlID="txt_othernationality"
                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                General conduct
                            </td>
                            <td>
                                <asp:TextBox ID="txt_generalconduct" CssClass="textbox textbox1 txtheight4" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Caste
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCaste" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                    onfocus="myFunction(this)" onchange="return FunctionCaste(this)" Style="width: 160px;
                                    float: left;">
                                </asp:DropDownList>
                                <br />
                                <asp:TextBox ID="txtCasteOther" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                    onkeydown="return (event.keyCode!=13);" onfocus="myFunction(this)" placeholder="Other Caste"
                                    Style="width: 150px; float: left; display: none;"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" TargetControlID="txtCasteOther"
                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                Date of application for certificate
                            </td>
                            <td>
                                <asp:TextBox ID="txt_applicationcerticate" CssClass="textbox textbox1 txtheight4"
                                    runat="server"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                    Format="dd/MM/yyyy" TargetControlID="txt_applicationcerticate">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>Religion</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlreligion" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                    onfocus="myFunction(this)" onchange="return onreli(this)" Style="width: 160px;
                                    float: left;">
                                </asp:DropDownList>
                                <br />
                                <asp:TextBox ID="txt_otherreligion" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                    onfocus="myFunction1(this)" onkeydown="return (event.keyCode!=13);" Style="width: 150px;
                                    display: none; float: left;"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" TargetControlID="txt_otherreligion"
                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <span>Community</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcoummunity" CssClass="textbox3 textbox1" runat="server"
                                    onblur="blurFunction(this)" onchange="return oncomm(this)" onfocus="myFunction(this)"
                                    Style="width: 160px; float: left;">
                                </asp:DropDownList>
                                <br />
                                <asp:TextBox ID="txtCommunity" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                    onfocus="myFunction1(this)" onkeydown="return (event.keyCode!=13);" Style="width: 150px;
                                    display: none; float: left; text-transform: uppercase;"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender66" runat="server" TargetControlID="txtCommunity"
                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Category
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_catagory" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                    onfocus="myFunction(this)" Style="width: 160px; float: left;">
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                    <asp:ListItem Value="1">Adi Dravidar (SC) or (S.T.)</asp:ListItem>
                                    <asp:ListItem Value="2">Backward Class</asp:ListItem>
                                    <asp:ListItem Value="3">Most Backward Class</asp:ListItem>
                                    <asp:ListItem Value="4">Convert Christianity from Scheduled Caste</asp:ListItem>
                                    <asp:ListItem Value="5">Denotified Communities</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                Medical Inspection
                            </td>
                            <td>
                                <asp:RadioButton ID="rdb_medical" runat="server" Text="Yes" GroupName="med" Style="float: left;" />
                                <asp:RadioButton ID="rdb_medical1" runat="server" Checked="true" Text="No" GroupName="med"
                                    Style="float: left;" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Date of first admission </br> in the school with class
                            </td>
                            <td>
                                <asp:TextBox ID="txt_admdate" CssClass="textbox textbox1 txtheight" runat="server"></asp:TextBox>
                                <asp:CalendarExtender ID="caldrivdt" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                    Format="dd/MM/yyyy" TargetControlID="txt_admdate">
                                </asp:CalendarExtender>
                                <%--  <asp:TextBox ID="txt_joinclass" placeholder="Joinning class" CssClass="textbox textbox1 txtheight"
                                    runat="server"></asp:TextBox>--%>
                                <asp:DropDownList ID="ddl_joinclass" CssClass="textbox3 textbox1" runat="server"
                                    Style="width: 120px;">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Date of issue of certificate
                            </td>
                            <td>
                                <asp:TextBox ID="txt_dateofissueofcertificate" CssClass="textbox textbox1 txtheight4"
                                    runat="server"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                    Format="dd/MM/yyyy" TargetControlID="txt_dateofissueofcertificate">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                last studied Date
                            </td>
                            <td>
                                <asp:TextBox ID="txt_laststudieddate" CssClass="textbox textbox1 txtheight4" runat="server"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender4" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                    Format="dd/MM/yyyy" TargetControlID="txt_laststudieddate">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                last studied class
                            </td>
                            <td>
                                <asp:TextBox ID="txt_laststudiedclass" placeholder="Last Studied Class" CssClass="textbox textbox1 txtheight4"
                                    runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Reasons for leaving the school
                            </td>
                            <td>
                                <asp:TextBox ID="txt_leaving" Width="225px" CssClass="textbox textbox1 txtheight4"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%--<asp:Label ID="lbl_school" runat="server" Text="School / Board"></asp:Label>--%>
                                School / Board Annual examination<br />
                                last taken with result
                            </td>
                            <td>
                                <asp:TextBox ID="txt_schoolorboard" CssClass="textbox textbox1 txtheight4" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Any other remarks
                            </td>
                            <td>
                                <asp:TextBox ID="txt_remarks" Width="225px" CssClass="textbox textbox1 txtheight4"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                No of Attempts
                            </td>
                            <td>
                                <asp:TextBox ID="txt_failsameclass" CssClass="textbox textbox1 txtheight2" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Educational District
                            </td>
                            <td>
                                <asp:TextBox ID="txt_educationdistrict" CssClass="textbox textbox1 txtheight4" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Subject Studied
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txt_subjectstudied" CssClass="textbox textbox1 txtheight5" Width="302px"
                                    runat="server"></asp:TextBox>
                                Disposal No
                            </td>
                            <td>
                                <asp:TextBox ID="txt_disposelno" CssClass="textbox textbox1 txtheight4" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1">
                                Extra curricular activities
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txt_extraactivites" CssClass="textbox textbox1 txtheight6" runat="server"
                                    onblur="blurFunction(this)" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                    Style="float: left;"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender78" runat="server" TargetControlID="txt_extraactivites"
                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=",.-& ">
                                </asp:FilteredTextBoxExtender>
                                <%-- <asp:RadioButton ID="rdbextraactivitesYes" runat="server" Text="Yes" GroupName="Same7"
                                    Style="float: left;" onclick="return extra_fun(this)" />
                                <asp:RadioButton ID="rdbextraactivitesNo" runat="server" Checked="true" Text="No"
                                    GroupName="Same7" Style="float: left;" onclick="return extra_fun1(this)" />
                                <asp:DropDownList ID="DropDownList4" CssClass="textbox3 textbox1" runat="server"
                                    onchange="return otherextra(this)" Style="width: 160px; display: none; float: left;">
                                </asp:DropDownList>
                                <asp:TextBox ID="txt_extraactivites" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                    MaxLength="50" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                    Style="width: 150px; display: none; float: left;"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender78" runat="server" TargetControlID="txt_extraactivites"
                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                                <asp:TextBox ID="txt_specifyNcc" CssClass="textbox textbox1" runat="server" placeholder="Specify Activites"
                                    MaxLength="50" onblur="blurFunction(this)" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                    Style="width: 150px; display: none; float: left;"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender79" runat="server" TargetControlID="txt_specifyNcc"
                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>--%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                Whether NCC Cadet / Boy Scout / Girl Guide
                                <asp:TextBox ID="txt_ncc" CssClass="textbox textbox1 txtheight6" runat="server"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_ncc"
                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=",. ">
                                </asp:FilteredTextBoxExtender>
                                <%--  <asp:RadioButton ID="rdb_ncc" runat="server" Text="Yes" GroupName="ncc" Style="float: right;" />
                                <asp:RadioButton ID="rdb_ncc1" runat="server" Checked="true" Text="No" GroupName="ncc"
                                    Style="float: right;" />--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Part 1 language
                            </td>
                            <td>
                                <asp:TextBox ID="txt_part1language" CssClass="textbox textbox1 txtheight4" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Identification Marks
                            </td>
                            <td>
                                <asp:TextBox ID="txt_identification" CssClass="textbox textbox1 txtheight4" Width="225px"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Course offered
                            </td>
                            <td colspan="2">
                                <asp:RadioButton ID="rdo_voc" runat="server" Checked="true" Text="General Education"
                                    GroupName="voc" Style="float: left;" />
                                <asp:RadioButton ID="rdo_voc1" runat="server" Text="Vocational Education" GroupName="voc"
                                    Style="float: left;" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Medium
                            </td>
                            <td>
                                <asp:TextBox ID="txt_mudiumofstudy" CssClass="textbox textbox1 txtheight4" Width="225px"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                T.M.R No
                                <asp:TextBox ID="txt_tmrno" CssClass="textbox textbox1 txtheight2" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Certificate No
                                <asp:TextBox ID="txt_CertificatenoH" CssClass="textbox textbox1 txtheight" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Certificate Date
                            </td>
                            <td>
                                <asp:TextBox ID="txt_CertificatedateH" CssClass="textbox textbox1 txtheight2" runat="server"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                    Format="dd/MM/yyyy" TargetControlID="txt_CertificatedateH">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Serial No
                                <asp:TextBox ID="txt_serialno" CssClass="textbox textbox1 txtheight2" runat="server"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                Teachar Name
                                <asp:TextBox ID="txt_classteacher" CssClass="textbox textbox1 txtheight2" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Checked By
                                <asp:TextBox ID="txt_checkedby" CssClass="textbox textbox1 txtheight2" runat="server"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                Checked design
                                <asp:TextBox ID="txt_design" CssClass="textbox textbox1 txtheight2" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:Button ID="btn_Save" runat="server" Text="Save" CssClass="textbox btn2" OnClick="btn_save_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                </div>
            </div>
        </center>
        <center>
            <div id="pop_clg_tc" runat="server" visible="false" style="height: 48em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 13px; margin-left: 471px;"
                    OnClick="imagebtnpopclose4_Click" />
                <br />
                <div style="background-color: White; height: auto; width: 960px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <div style="position: absolute; margin-top: -6px; margin-left: 29px;">
                        <center>
                            <asp:Label ID="Label1" runat="server" Font-Names="Viner Hand ITC" ForeColor="#DF95FB"></asp:Label>
                        </center>
                    </div>
                    <center>
                        <span style="color: Green;" class="fontstyleheader ">TRANSFER CERTIFICATE</span>
                    </center>
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="delFlagValue" runat="server" Text="" Visible="false"></asp:Label>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:CheckBox ID="cb_serialnoSettings" Text="Automatic Generation" runat="server"
                                    AutoPostBack="true" OnCheckedChanged="cb_serialnoSettings_onchange"></asp:CheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Register No
                            </td>
                            <td>
                                <asp:TextBox ID="txt_regno" AutoPostBack="true" OnTextChanged="txt_regno_Onchange"
                                    CssClass="textbox textbox1 txtheight4" runat="server"></asp:TextBox>
                                <asp:Label ID="lbl_app_no1" Visible="false" runat="server"></asp:Label>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="getregno" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_regno"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground" UseContextKey="true" OnClientPopulating="autoComplete2_OnClientPopulating">
                                </asp:AutoCompleteExtender>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txt_regno"
                                    FilterType="UppercaseLetters,LowercaseLetters,numbers, Custom" ValidChars=" -/">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                Serial No
                            </td>
                            <td>
                                <asp:TextBox ID="txt_serial_no" placeholder="Serial No" CssClass="textbox textbox1 txtheight4"
                                    runat="server"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" TargetControlID="txt_serial_no"
                                    FilterType="uppercaseletters,lowercaseletters,Numbers,custom" ValidChars=" /.-&">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Name of the Student
                            </td>
                            <td>
                                <asp:TextBox ID="txt_studname1" CssClass="textbox textbox1 txtheight4" runat="server"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txt_studname1"
                                    FilterType="UppercaseLetters,LowercaseLetters, Custom" ValidChars=" .">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <%--  <td>
                                Leaving the Institution
                            </td>
                            <td>
                                <asp:TextBox ID="txt_leavinginstition" placeholder="Leaving the Institution" CssClass="textbox textbox1 txtheight5"
                                    runat="server"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txt_leavinginstition"
                                    FilterType="UppercaseLetters,LowercaseLetters,numbers, Custom" ValidChars=" ./&-">
                                </asp:FilteredTextBoxExtender>
                            </td>--%>
                            <td>
                                Date of Admission
                            </td>
                            <td>
                                <asp:TextBox ID="txt_doAdmission" runat="server" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender7" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                    Format="dd/MM/yyyy" TargetControlID="txt_doAdmission">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Mother's Name
                            </td>
                            <td>
                                <asp:TextBox ID="txt_mothername1" CssClass="textbox textbox1 txtheight4" runat="server"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txt_mothername1"
                                    FilterType="UppercaseLetters,LowercaseLetters, Custom" ValidChars=" .">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <%--<td>
                                Date of commencement of classes
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_commencementofclass" CssClass="ddlheight1 textbox1" runat="server">
                                </asp:DropDownList>
                                <asp:TextBox ID="txt_commencementofclass" CssClass="textbox textbox1 txtheight2"
                                    runat="server"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txt_commencementofclass"
                                    FilterType="UppercaseLetters,LowercaseLetters, Custom" ValidChars=" .()/-&">
                                </asp:FilteredTextBoxExtender>
                            </td>--%>
                            <td>
                                Date of leaving
                            </td>
                            <td>
                                <asp:TextBox ID="txt_doLeaving" runat="server" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender8" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                    Format="dd/MM/yyyy" TargetControlID="txt_doLeaving">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Father's Name
                            </td>
                            <td>
                                <asp:TextBox ID="txt_fathername1" CssClass="textbox textbox1 txtheight4" runat="server"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txt_fathername1"
                                    FilterType="UppercaseLetters,LowercaseLetters, Custom" ValidChars=" .">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                DOB
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldobdate1" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                    onfocus="myFunction(this)" Style="width: 47px;">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddldobMonth1" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                    Style="width: 60px;">
                                    <asp:ListItem Value="00">MMM</asp:ListItem>
                                    <asp:ListItem Value="01">JAN</asp:ListItem>
                                    <asp:ListItem Value="02">FEB</asp:ListItem>
                                    <asp:ListItem Value="03">MAR</asp:ListItem>
                                    <asp:ListItem Value="04">APR</asp:ListItem>
                                    <asp:ListItem Value="05">MAY</asp:ListItem>
                                    <asp:ListItem Value="06">JUN</asp:ListItem>
                                    <asp:ListItem Value="07">JUL</asp:ListItem>
                                    <asp:ListItem Value="08">AUG</asp:ListItem>
                                    <asp:ListItem Value="09">SEP</asp:ListItem>
                                    <asp:ListItem Value="10">OCT</asp:ListItem>
                                    <asp:ListItem Value="11">NOV</asp:ListItem>
                                    <asp:ListItem Value="12">DEC</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddldobYear1" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                    Style="width: 59px;">
                                </asp:DropDownList>
                            </td>
                            <%-- <td>
                                Month of Leaving<%--Last attended the class and Date
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_lastattendedclass" CssClass="ddlheight1 textbox1" runat="server">
                                </asp:DropDownList>
                            </td>--%>
                        </tr>
                        <%--  <tr>--%>
                        <%-- <td>
                                <%--Date on which application for
                                <br />
                                Transfer Certificate was made
                            </td>
                            <td>
                                <asp:Button ID="btn_plus1" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                    Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus1_Click" />
                                <asp:DropDownList ID="ddl_tccertificateissuedate" CssClass="ddlheight1 textbox1"
                                    runat="server">
                                </asp:DropDownList>
                                <asp:Button ID="btn_minus1" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btnminus1_Click" />
                            </td>--%>
                        <%-- </tr>--%>
                        <tr>
                            <td>
                                Nationality
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcountry1" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                    onfocus="myFunction(this)" onchange="return funation1(this)" Style="width: 160px;
                                    float: left;">
                                </asp:DropDownList>
                                <br />
                                <asp:TextBox ID="txt_othernationality1" CssClass="textbox textbox1" Visible="true"
                                    MaxLength="50" runat="server" onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);"
                                    onfocus="myFunction(this)" placeholder="Other Nationality" Style="width: 150px;
                                    float: left; display: none;"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_othernationality1"
                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                Conduct & Character
                            </td>
                            <td>
                                <asp:Button ID="btn_plus" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                    Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus_Click" />
                                <asp:DropDownList ID="ddl_generalconduct" CssClass="ddlheight1 textbox1" runat="server">
                                </asp:DropDownList>
                                <asp:Button ID="btn_minus" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btnminus_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>Religion</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlreligion1" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                    onfocus="myFunction(this)" onchange="return onreli1(this)" Style="width: 160px;
                                    float: left;">
                                </asp:DropDownList>
                                <br />
                                <asp:TextBox ID="txt_otherreligion1" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                    onfocus="myFunction1(this)" onkeydown="return (event.keyCode!=13);" Style="width: 150px;
                                    display: none; float: left;"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_otherreligion1"
                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                Medium of Instruction
                            </td>
                            <td>
                                <asp:TextBox ID="txt_mudiumofstudy1" CssClass="textbox textbox1 txtheight4" Width="225px"
                                    runat="server"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txt_mudiumofstudy1"
                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>Community</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcoummunity1" CssClass="textbox3 textbox1" runat="server"
                                    onblur="blurFunction(this)" onchange="return oncomm1(this)" onfocus="myFunction(this)"
                                    Style="width: 160px; float: left;">
                                </asp:DropDownList>
                                <br />
                                <asp:TextBox ID="txtCommunity1" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                    onfocus="myFunction1(this)" onkeydown="return (event.keyCode!=13);" Style="width: 150px;
                                    display: none; float: left; text-transform: uppercase;"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtCommunity1"
                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                First Language
                            </td>
                            <td>
                                <asp:TextBox ID="txt_part1language1" CssClass="textbox textbox1 txtheight4" Width="225px"
                                    runat="server"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" TargetControlID="txt_part1language1"
                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Caste
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_caste1" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                    onfocus="myFunction(this)" onchange="return FunctionCaste1(this)" Style="width: 160px;
                                    float: left;">
                                </asp:DropDownList>
                                <br />
                                <asp:TextBox ID="txt_caste1" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                    onkeydown="return (event.keyCode!=13);" onfocus="myFunction(this)" placeholder="Other Caste"
                                    Style="width: 150px; float: left; display: none;"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="txt_caste1"
                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                Date of issue of certificate
                            </td>
                            <td>
                                <%--  <asp:DropDownList ID="ddl_dateofissuecertificate" CssClass="ddlheight1 textbox1"
                                    runat="server">
                                </asp:DropDownList>--%>
                                <asp:TextBox ID="txt_dateofissuecertificate" runat="server" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender6" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                    Format="dd/MM/yyyy" TargetControlID="txt_dateofissuecertificate">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Attendance
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_attendance" CssClass="textbox3 textbox1" runat="server"
                                    onblur="blurFunction(this)" onfocus="myFunction(this)" onchange="return Functionattendance(this)"
                                    Style="width: 160px; float: left;">
                                </asp:DropDownList>
                                <br />
                                <asp:TextBox ID="txt_attendance" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                    onkeydown="return (event.keyCode!=13);" onfocus="myFunction(this)" placeholder="Attendance Type"
                                    Style="width: 150px; float: left; display: none;"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" TargetControlID="txt_attendance"
                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                Remarks
                            </td>
                            <td>
                                <asp:TextBox ID="txt_remarks1" Width="225px" CssClass="textbox textbox1 txtheight4"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Period of Studied
                            </td>
                            <td>
                                <asp:TextBox ID="txt_periodofstudied" placeholder="Period of Studied" CssClass="textbox textbox1 txtheight4"
                                    runat="server"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" TargetControlID="txt_periodofstudied"
                                    FilterType="Numbers,custom" ValidChars=" /.-&">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <%-- MIGRATION CERTIFICATE--%>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <table id="format5" runat="server">
                                    <tr>
                                        <td>
                                            Migration Sl. No:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_migrationserielno" placeholder="Migration Serial No" CssClass="textbox textbox1 txtheight4"
                                                runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txt_migrationserielno"
                                                FilterType="uppercaseletters,lowercaseletters,Numbers,custom" ValidChars=" /.-&">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            Purpose
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPurpose" placeholder="Bonafide Purpose" CssClass="textbox textbox1 txtheight4"
                                                runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtPurpose"
                                                FilterType="uppercaseletters,lowercaseletters,Numbers,custom" ValidChars=" /.-&">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Date of issue
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_dateoofissuemigration" CssClass="ddlheight1 textbox1" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Programme Completed
                                        </td>
                                        <td>
                                            <asp:TextBox ID="programeCompleted" placeholder="Programme Completed" CssClass="textbox textbox1 txtheight5"
                                                runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" TargetControlID="programeCompleted"
                                                FilterType="uppercaseletters,lowercaseletters,Numbers,custom" ValidChars=" /.-&">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Aadharcard No
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_Aadharcardno" runat="server" Width="40px" CssClass="textbox textbox1"
                                                onblur="if(this.value.length!='4') this.value='';blurFunction(this)" MaxLength="4"
                                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                            <asp:TextBox ID="txt_Aadharcardno2" runat="server" Width="40px" CssClass="textbox textbox1"
                                                onblur="if(this.value.length!='4') this.value='';blurFunction(this)" MaxLength="4"
                                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                            <asp:TextBox ID="txt_Aadharcardno3" runat="server" Width="40px" CssClass="textbox textbox1"
                                                onblur="if(this.value.length!='4') this.value='';blurFunction(this)" MaxLength="4"
                                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender32" runat="server" TargetControlID="txt_Aadharcardno"
                                                FilterType="Numbers">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender76" runat="server" TargetControlID="txt_Aadharcardno2"
                                                FilterType="Numbers">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender77" runat="server" TargetControlID="txt_Aadharcardno3"
                                                FilterType="Numbers">
                                            </asp:FilteredTextBoxExtender>
                                            <%-- <asp:TextBox ID="txt_aadharcardno" placeholder="Aadharcard No" MaxLength="12" CssClass="textbox textbox1 txtheight4"
                                    runat="server"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="txt_aadharcardno"
                                    FilterType="Numbers" ValidChars="">
                                </asp:FilteredTextBoxExtender>--%>
                                        </td>
                                        <td>
                                            Last examination appeared
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_exammonthandyear" placeholder="Exam month & year" CssClass="textbox textbox1 txtheight4"
                                                runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="txt_exammonthandyear"
                                                FilterType="uppercaseletters,lowercaseletters,Numbers,custom" ValidChars=" /.-&">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Admission Date
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_admissiondate" CssClass="textbox textbox1 txtheight" runat="server"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender5" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                                Format="dd/MM/yyyy" TargetControlID="txt_admissiondate">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:Button ID="btn_saveclg" runat="server" Text="Save" CssClass="textbox btn2" OnClick="btn_saveclg_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
            </div>
        </center>
        <center>
            <div id="plusdiv" runat="server" visible="false" class="popupstyle popupheight1">
                <center>
                    <div id="panel_addgroup" runat="server" visible="false" class="table" style="background-color: White;
                        height: 140px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        margin-top: 200px; border-radius: 10px;">
                        <table style="line-height: 30px">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_addgroup" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:TextBox ID="txt_addgroup" runat="server" Width="200px" CssClass="textbox textbox1"
                                        onkeypress="display1()"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="line-height: 35px">
                                    <asp:Button ID="btn_addgroup1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                        CssClass="textbox btn2" OnClick="btn_addgroup_Click" />
                                    <asp:Button ID="btn_exitgroup1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                        CssClass="textbox btn2" OnClick="btn_exitaddgroup_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblerror" runat="server" Visible="false" ForeColor="red" Font-Size="Smaller"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </div>
        </center>
    </div>
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
                                        <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                            OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <div style="height: 1px; width: 1px; overflow: auto;">
        <div id="contentDiv" runat="server" style="height: auto; width: 1344px;" visible="false">
        </div>
    </div>
</asp:Content>
