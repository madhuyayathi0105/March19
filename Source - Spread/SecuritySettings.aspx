<%@ Page Title="Security Settings" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="SecuritySettings.aspx.cs" Inherits="SecuritySettings" EnableEventValidation="false" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <link href="Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function frelig() {

            document.getElementById('<%=btnnewcriteria.ClientID%>').style.display = 'block';
            document.getElementById('<%=btnreovecritreia.ClientID%>').style.display = 'block';
        }
    </script>
    <script type="text/javascript">
        function RetAgeChange(age) {
            var ddlval = document.getElementById("<%=ddlStfType.ClientID %>").value;
            if (ddlval.trim() != "" && ddlval.trim() != "Select") {
                if (parseFloat(age.value) > 60) {
                    age.value = "";
                    alert("Retirement Age should be less than or Equal to '60'!");
                }
            }
            else {
                age.value = "";
                alert("Please Select Staff Type!");
            }
        }
        function Add_Templete_Click() {
            var smsvar = document.getElementById("<%=panelsms.ClientID%>");
            smsvar.style.visibility = "visible";
        }
        function OnCheckBoxCheckChanged(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            if (isChkBoxClick) {
                //                __doPostBack("", "");
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
        function smsfocusin() {
            $('.errorsms').html("");
        }
        function emailfocusin() {
            $('.errormail').html("");
        }
        function smsfocusout() {
            var str = $('#<%=txt_CopySmsMobNo.ClientID %>').val();
            var validMobNum = [];
            var invalidmobnum = [];
            var strarray = str.split(',');
            var validcount = 0;
            var invalidcount = 0;
            if ($('#<%=txt_CopySmsMobNo.ClientID %>').val() != "") {
                for (var i = 0; i < strarray.length; i++) {
                    if (validatePhone(strarray[i])) {
                        validMobNum[validcount] = strarray[i];
                        validcount++;
                    }
                    else {
                        invalidmobnum[invalidcount] = strarray[i];
                        invalidcount++;
                    }
                }
                $('#<%=txt_CopySmsMobNo.ClientID %>').val(validMobNum);
                if (invalidmobnum.length > 0 && invalidcount != 0) {
                    $('.errorsms').css('display', 'block');
                    $('.errorsms').html("Please Enter Valid 10 - Digits Mobile Numbers Seperated By Comma!!! Invalid Mobile Numbers Are " + invalidmobnum.join(','));
                }
                else {
                    $('.errorsms').html("");
                }
            }
            else {
                $('#<%=txt_CopySmsMobNo.ClientID %>').val("");
                $('.errorsms').html("");
            }
        }
        function emailfocusout() {
            var stremailid = $('#<%=txt_CopyEmailid.ClientID %>').val();
            var validemailid = [];
            var invalidemailid = [];
            var stremailarray = stremailid.split(',');
            var validemailcount = 0;
            var invalidemailcount = 0;
            if ($('#<%=txt_CopyEmailid.ClientID %>').val() != "") {
                for (var i = 0; i < stremailarray.length; i++) {
                    if (validateEmail(stremailarray[i])) {
                        validemailid[validemailcount] = stremailarray[i];
                        validemailcount++;
                        //                        alert(strarray[i] + 'is Valid.' + validcount)
                    }
                    else {
                        invalidemailid[invalidemailcount] = stremailarray[i];
                        invalidemailcount++;
                        //                        alert(strarray[i] + 'Is Invalid.' + invalidcount)
                    }
                }
                $('#<%=txt_CopyEmailid.ClientID %>').val(validemailid);
                //                $('#invalidMobnums').text(invalidmobnum);
                if (invalidemailid.length > 0 && invalidemailcount != 0) {
                    $('.errormail').css('display', 'block');
                    $('.errormail').html("Please Enter Valid Email Ids Seperated By Comma!!! Invalid Email Ids Are " + invalidemailid.join(','));
                }
                else {
                    $('.errormail').html("");
                }
            }
            else {
                $('#<%=txt_CopyEmailid.ClientID %>').val("");
                $('.errormail').html("");
            }
        }
        function smskeypress(e) {
            if (e.which == 44) { }
            else {
                if (e.which != 8 && e.which != 0 && e.which != 13 && (e.which < 48 || e.which > 57)) {
                    //display error message
                    $('.errorsms').css('color', 'red');
                    $('.errorsms').css('display', 'block');
                    $('.errorsms').html("Its Allows Numbers And Comma Only").show().fadeOut("slow");
                    return false;
                }
            }
        }
        function InitEvents() {
            $('#<%=txt_CopySmsMobNo.ClientID %>').focusin(smsfocusin);
            $('#<%=txt_CopyEmailid.ClientID %>').focusin(emailfocusin);
            $('#<%=txt_CopySmsMobNo.ClientID %>').focusout(smsfocusout);
            $('#<%=txt_CopyEmailid.ClientID %>').focusout(emailfocusout);
            $('#<%=txt_CopySmsMobNo.ClientID %>').keypress(smskeypress);
        }
        function validatePhone(phoneText) {
            var filter = /^[0-9]{10}$/;
            if (filter.test(phoneText)) {
                return true;
            }
            else {
                return false;
            }
        }
        function validateEmail(sEmail) {
            var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            if (filter.test(sEmail)) {
                return true;
            }
            else {
                return false;
            }
        }
        $(document).ready(InitEvents);
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitEvents);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <center>
        <div style="width: 980px;">
            <center>
                <asp:Panel ID="Panel1" runat="server" Style="background: #0095E8; width: 980px">
                    <center>
                        <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="White" Text="Security Settings"></asp:Label>
                    </center>
                </asp:Panel>
                <asp:Panel ID="Panel2" runat="server">
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButton ID="rdb_ind" runat="server" Text="Individual" Font-Bold="True" Font-Names="Book Antiqua"
                                    Checked="true" GroupName="Report" AutoPostBack="True" OnCheckedChanged="rdb_ind_CheckedChanged" />
                            </td>
                            <td>
                                <asp:RadioButton ID="rdb_grp" runat="server" Text="Group" Font-Bold="True" Font-Names="Book Antiqua"
                                    GroupName="Report" AutoPostBack="True" OnCheckedChanged="rdb_grp_CheckedChanged" />
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text=" Select the College " Font-Bold="True"
                                    Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollege" Width="200px" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text=" Select the User " Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UPDuser" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtuser" runat="server" Height="19px" ReadOnly="true" Font-Bold="True"
                                            ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Width="113px"
                                            OnTextChanged="txtuser_TextChanged" CssClass="Dropdown_Txt_Box">- - Select - -</asp:TextBox>
                                        <asp:Panel ID="puser" runat="server" CssClass=" multxtpanel multxtpanleheight" Height="273px"
                                            ScrollBars="Vertical" Width="185px">
                                            <asp:CheckBox ID="chk_alluser" runat="server" Text="SelectAll" AutoPostBack="true"
                                                Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                                OnCheckedChanged="chk_alluser_CheckedChanged" />
                                            <asp:CheckBoxList ID="ddluser" runat="server" Font-Size="Small" AutoPostBack="True"
                                                Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Width="184px" Height="50px"
                                                OnSelectedIndexChanged="ddluser_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtuser"
                                            PopupControlID="puser" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:LinkButton ID="btnGo" runat="server" Font-Bold="True" Width="50px" Text="Go"
                                    Font-Names="Book Antiqua" Font-Size="Small" ForeColor="Black" OnClick="btnGo_Click"
                                    CausesValidation="False" Style="display: inline-block; color: Black; font-family: Book Antiqua; font-size: small; font-weight: bold; width: 50px; text-align: center; border-style: solid; border-width: 1px; border-color: gray; background-color: none; border-radius: 4px 4px 4px 4px; text-decoration: none;"></asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="linkuser" runat="server" Text="New User Creation" Font-Size="Medium"
                                    Font-Names="Book Antiqua" Font-Bold="True" PostBackUrl="~/UserCreation.aspx"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Label ID="errmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Small" ForeColor="Red"></asp:Label>
                <br />
            </center>
            <center>
                <table style="background: #0095E8; width: 980px;">
                    <tr>
                        <td>
                            <asp:Button ID="btnWebRights_0" runat="server" Text="Web Form Rights" CssClass="textbox"
                                BackColor="#EB162C" ForeColor="White" OnClick="btnWebRights_0_Click" />
                            <asp:Button ID="btnSettings_1" runat="server" Text="Settings" CssClass="textbox"
                                BackColor="#A537D1" ForeColor="White" OnClick="btnSettings_1_Click" />
                            <asp:Button ID="btnFineAmt_2" runat="server" Text="Fine Amount" CssClass="textbox"
                                BackColor="#A537D1" ForeColor="White" OnClick="btnFineAmt_2_Click" />
                            <asp:Button ID="btnFinancePrint_3" runat="server" Text="Finance Print Settings" CssClass="textbox"
                                BackColor="#A537D1" ForeColor="White" OnClick="btnFinancePrint_3_Click" />
                            <asp:Button ID="btnCOE_4" runat="server" Text="COE" CssClass="textbox" BackColor="#A537D1"
                                ForeColor="White" OnClick="btnCOE_4_Click" />
                            <asp:Button ID="btnAttendance_5" runat="server" Text="Attendance" CssClass="textbox"
                                BackColor="#A537D1" ForeColor="White" OnClick="btnAttendance_5_Click" />
                            <asp:Button ID="btnTransRemind_6" runat="server" Text="Transport Reminder" CssClass="textbox"
                                BackColor="#A537D1" ForeColor="White" OnClick="btnTransRemind_6_Click" />
                            <asp:Button ID="btnSMSTemp_7" runat="server" Text="SMS Template" CssClass="textbox"
                                BackColor="#A537D1" ForeColor="White" OnClick="btnSMSTemp_7_Click" />
                            <asp:Button ID="btnAdmissionProc_8" runat="server" Text="Admission Process" CssClass="textbox"
                                BackColor="#A537D1" ForeColor="White" OnClick="btnAdmissionProc_8_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnHRSettings_9" runat="server" Text="HR Settings" CssClass="textbox"
                                BackColor="#A537D1" ForeColor="White" OnClick="btnHRSettings_9_Click" />
                            <asp:Button ID="btnUserDegRights_10" runat="server" Text="User Degree Rights" CssClass="textbox"
                                BackColor="#A537D1" ForeColor="White" OnClick="btnUserDegRights_10_Click" />
                            <asp:Button ID="btnWebsitePayment_11" runat="server" Text="Online Application" CssClass="textbox"
                                BackColor="#A537D1" ForeColor="White" OnClick="btnWebsitePayment_11_Click" />
                            <%--Kowshika--%>
                            <asp:Button ID="btninvigilation_12" runat="server" Text="Invigilation" CssClass="textbox"
                                BackColor="#A537D1" ForeColor="White" OnClick="btninvigilation_12_Click" />

                            <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                ForeColor="White" Font-Bold="true" OnClick="btnsave_Click" Style="margin-left: 500px; width: 80px;" />
                            <asp:Button ID="btnSettings" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                ForeColor="White" Font-Bold="true" OnClick="btnSettings_Click" Style="margin-left: 500px; width: 80px;" />
                            <asp:Button ID="btnFine" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                ForeColor="White" Font-Bold="true" OnClick="btnFine_Click" Style="margin-left: 500px; width: 80px;" />
                            <asp:Button ID="btnFinnance" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                ForeColor="White" Font-Bold="true" OnClick="btnFinnance_Click" Style="margin-left: 500px; width: 80px;" />
                            <asp:Button ID="btnsave_coe" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                ForeColor="White" Font-Bold="true" OnClick="btnsave_coe_Click" Style="margin-left: 500px; width: 80px;" />
                            <asp:Button ID="btnAttendance" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                ForeColor="White" Font-Bold="true" OnClick="btnAttendance_Click" Style="margin-left: 500px; width: 80px;" />
                            <asp:Button ID="btnTransport" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                ForeColor="White" Font-Bold="true" OnClick="btnTransport_Click" Style="margin-left: 500px; width: 80px;" />
                            <asp:Button ID="btnSMS" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                ForeColor="White" Font-Bold="true" OnClick="btnSMS_Click" Style="margin-left: 500px; width: 80px;" />
                            <asp:Button ID="btnAdmission" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                ForeColor="White" Font-Bold="true" OnClick="btnAdmission_Click" Style="margin-left: 500px; width: 80px;" />
                            <asp:Button ID="btnHR" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                ForeColor="White" Font-Bold="true" OnClick="btnHR_Click" Style="margin-left: 500px; width: 80px;" />
                            <asp:Button ID="btnDegree" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                ForeColor="White" Font-Bold="true" OnClick="btnDegree_Click" Style="margin-left: 500px; width: 80px;" />
                            <asp:Button ID="btnPayment" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                ForeColor="White" Font-Bold="true" OnClick="btnPayment_Click" Style="margin-left: 500px; width: 80px;" />
                            <asp:Button ID="btnInvig" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                ForeColor="White" Font-Bold="true" OnClick="btnInvig_Click" Style="margin-left: 500px; width: 80px;" />
                        </td>
                    </tr>

                </table>
            </center>
            <asp:DropDownList ID="TabContainer1" runat="server" Style="display: none;">
                <asp:ListItem Selected="True" Value="0"></asp:ListItem>
                <asp:ListItem Value="1"></asp:ListItem>
                <asp:ListItem Value="2"></asp:ListItem>
                <asp:ListItem Value="3"></asp:ListItem>
                <asp:ListItem Value="4"></asp:ListItem>
                <asp:ListItem Value="5"></asp:ListItem>
                <asp:ListItem Value="6"></asp:ListItem>
                <asp:ListItem Value="7"></asp:ListItem>
                <asp:ListItem Value="8"></asp:ListItem>
                <asp:ListItem Value="9"></asp:ListItem>
                <asp:ListItem Value="10"></asp:ListItem>
                <asp:ListItem Value="11"></asp:ListItem>
                <asp:ListItem Value="12"></asp:ListItem>
            </asp:DropDownList>
            <%--WebForm Rights Tab 1--%>
            <center>
                <div id="div1WebFormRights" runat="server" style="width: 980px; height: 400px; background: white; border: 1px solid #0095E8; overflow: auto;">
                    <table>
                        <tr>
                            <td>
                                <fieldset style="height: 84px; width: 140px;">
                                    <legend>Type</legend>
                                    <asp:RadioButton ID="rb_clg" Text="College" GroupName="school" runat="server" /><br />
                                    <asp:RadioButton ID="rb_sch" Text="School" GroupName="school" runat="server" />
                                </fieldset>
                            </td>
                            <td>
                                <fieldset style="height: 126px; width: 137px;">
                                    <legend>Student Filter </legend>
                                    <asp:CheckBox ID="Chkrollno" runat="server" Text="Roll No" />
                                    <br />
                                    <asp:CheckBox ID="chkregno" runat="server" Text="Register No" />
                                    <br />
                                    <asp:CheckBox ID="chkadmno" runat="server" Text="Admission No" />
                                    <br />
                                    <asp:CheckBox ID="chkstud" runat="server" Text="Student Type" />
                                    <br />
                                    <asp:CheckBox ID="chkApplicationNo" runat="server" Text="Application No" />
                                    <br />
                                    <%--magseh 23.6.18--%>
                                    <asp:CheckBox ID="Chkhostelid" runat="server" Text="Hostel Id" />
                                </fieldset>
                            </td>
                            <td>
                                <fieldset style="height: 84px; width: 150px;">
                                    <legend>Attendance </legend>
                                    <asp:RadioButton ID="rbgeneral" runat="server" Text="General" GroupName="section"
                                        AutoPostBack="True" OnCheckedChanged="rbgeneral_CheckedChanged" />
                                    <br />
                                    <asp:CheckBox ID="daywise" runat="server" Text="Day Wise" />
                                    <br />
                                    <asp:CheckBox ID="hourwise" runat="server" Text="Hour Wise" />
                                    <br />
                                    <asp:RadioButton ID="rbabsentees" runat="server" Text="Absentees" GroupName="section"
                                        OnCheckedChanged="rbabsentees_CheckedChanged" Visible="False" AutoPostBack="True" /><br />
                                    <asp:RadioButton ID="rbadmno" runat="server" Text="Admission No" Visible="False"
                                        GroupName="absentees" Enabled="False" /><br />
                                    <asp:RadioButton ID="rbrollno" runat="server" Text="RollNo" Visible="False" GroupName="absentees"
                                        Enabled="False" /><br />
                                    <asp:RadioButton ID="rbregno" runat="server" Text="RegisterNo" Visible="False" GroupName="absentees"
                                        Enabled="False" /><br />
                                    <br />
                                </fieldset>
                            </td>
                            <td>
                                <fieldset style="height: 84px; width: 140px;">
                                    <legend>Staff Register </legend>
                                    <asp:RadioButton ID="rblesson" Text="As Per Lesson" GroupName="staffregister" runat="server" /><br />
                                    <asp:RadioButton ID="rbgeneralstaff" Text="General" GroupName="staffregister" runat="server" />
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 84px; width: 140px; padding-top: 0px;">
                                    <legend>Reports Display </legend>
                                    <asp:RadioButton ID="rptind" Text="Individual" GroupName="RptDisplay" runat="server" /><br />
                                    <asp:RadioButton ID="rptgen" Text="General" GroupName="RptDisplay" runat="server" />
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: 108px; width: 210px;">
                                    <legend>Header Settings </legend>
                                    <asp:CheckBox ID="chkdayssch" runat="server" Text="Days Scholor" /><asp:CheckBox
                                        ID="chkregular" runat="server" Text="Regular" /><br />
                                    <asp:CheckBox ID="chklateral" runat="server" Text="Lateral" />
                                    <asp:CheckBox ID="chktrans" runat="server" Text="Transfer" />
                                    <br />
                                    <asp:CheckBox ID="chkmale" runat="server" Text="Male" />
                                    <asp:CheckBox ID="Chkfemale" runat="server" Text="Female" /><br />
                                    <asp:CheckBox ID="Chkhostel" runat="server" Text="Hostel" />
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="width: 233px; height: 80px;">
                                    <legend>Financial Year</legend>
                                    <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="endtext" runat="server"
                                        Format="dd/MM/yyyy" Enabled="True">
                                    </asp:CalendarExtender>
                                    <asp:Label ID="startdate" runat="server" Text="Starting Date"></asp:Label>
                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="starttext" runat="server"
                                        Format="dd/MM/yyyy" Enabled="True">
                                    </asp:CalendarExtender>
                                    <asp:TextBox ID="starttext" runat="server" Height="14px" Width="103px"></asp:TextBox><br />
                                    <br />
                                    <asp:Label ID="enddate" runat="server" Text="Ending Date"></asp:Label>
                                    <asp:TextBox ID="endtext" runat="server" Height="14px" Width="100px"></asp:TextBox>
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 50px; width: 215px;">
                                    <legend>COE </legend>
                                    <asp:Label ID="lbldummy" runat="server" Text="Starting DummyNo"></asp:Label>
                                    <asp:TextBox ID="txtdummy" runat="server" Style="width: 70px"></asp:TextBox><asp:FilteredTextBoxExtender
                                        ID="filter1" runat="server" FilterType="Numbers" TargetControlID="txtdummy" Enabled="True">
                                    </asp:FilteredTextBoxExtender>
                                </fieldset>
                                <fieldset style="height: 40px; width: 215px;">
                                    <legend>Schedule Lock</legend>
                                    <asp:CheckBox ID="chklock" runat="server" Text="Lock Alternate Schedule" />
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="chkbx_printmaster" runat="server" Text="Print Master Setting Visible"
                                    Visible="False" />
                                <fieldset style="height: 48px; width: 280px;">
                                    <legend>Academic Year</legend>
                                    <asp:Label ID="lblacefromyear" runat="server" Text="From Year" Font-Size="Medium"
                                        Font-Names="Book Antiqua"></asp:Label>
                                    <asp:DropDownList ID="ddlacefromyear" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CausesValidation="True">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblacetoyear" runat="server" Text="To Year" Font-Bold="True" Font-Size="Medium"
                                        Font-Names="Book Antiqua"></asp:Label>
                                    <asp:DropDownList ID="ddlacetoyear" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CausesValidation="True">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblfromyearerror" runat="server" CssClass="font" Visible="False" ForeColor="Red"></asp:Label>
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 48px; width: 176px;">
                                    <legend>Mail Password</legend>
                                    <asp:CheckBox ID="chkpassword" runat="server" Text="Send Password To Mail" />
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 60px; width: 215px;">
                                    <legend>Send Sms Minimum Absent Days</legend>
                                    <asp:Label ID="lblsmsdays" runat="server" Text="Minimum Days" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:TextBox ID="txtminimumabsentdays" runat="server" Width="60px" MaxLength="2"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterType="Numbers"
                                        TargetControlID="txtminimumabsentdays" Enabled="True" />
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: 35px; width: 280px">
                                    <legend>Roll No Suffix Length</legend>
                                    <asp:Label ID="lblrolllenth" runat="server" Text="Roll No Suffix Length" Font-Size="Medium"
                                        Font-Names="Book Antiqua"></asp:Label>
                                    <asp:TextBox ID="txtrollsuffixlength" runat="server" Width="60px" MaxLength="1" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterType="Numbers"
                                        TargetControlID="txtrollsuffixlength" Enabled="True" />
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 35px; width: 176px;">
                                    <legend>Cam Date Lock</legend>
                                    <asp:CheckBox ID="chkcaldateentry" runat="server" Text="Lock Cam Date Entry" />
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 40px; width: 215px;">
                                    <legend>Cam Entry Attendance</legend>
                                    <asp:CheckBox ID="chkcamattendance" runat="server" Text="Mark Attendance" Font-Names="Book Antiqua"
                                        Font-Size="Medium" />
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: 60px; width: 200px">
                                    <legend>Subject Chooser Type</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblsubjectchooser" runat="server" Text="Subject Type" Font-Names="Book Antiqua"
                                                    Font-Size="Medium">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtsubjecttype" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                    ReadOnly="true" Width="113px" Style="font-family: 'Book Antiqua'" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                <asp:Panel ID="psubjecttype" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                    Height="273px" Width="150px" Style="display: none;">
                                                    <asp:CheckBox ID="chksubjecttype" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnCheckedChanged="chksubjecttype_CheckedChanged" Text="Select All"
                                                        AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="chklssubjecttype" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                        OnSelectedIndexChanged="chklssubjecttype_SelectedIndexChanged" Font-Bold="True"
                                                        Font-Names="Book Antiqua">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="pcesubjecttype" runat="server" TargetControlID="txtsubjecttype"
                                                    DynamicServicePath="" PopupControlID="psubjecttype" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 55px; width: 250px">
                                    <legend>Student Staff Selector</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkstaustaffreport" runat="server" Text="Include in Attendance"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="12px" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlsetbatch" runat="server" Width="100px" Font-Size="Medium"
                                                    AutoPostBack="true" Font-Bold="True" Font-Names="Book Antiqua">
                                                </asp:DropDownList>
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 100px; width: 230px">
                                    <legend>Leave Apply And Approval</legend>
                                    <asp:CheckBox ID="chkaltenateleaveapply" runat="server" Text="Alternate" Font-Bold="True"
                                        Font-Names="Book Antiqua" />
                                    <br />
                                    <asp:RadioButtonList ID="rblalterPeriod" runat="server" RepeatDirection="Vertical">
                                        <asp:ListItem Text="Periods For Leave Apply" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Staff For Leave Apply" Value="2"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <fieldset style="height: 40px; width: 450px">
                                    <legend>CAM Calculation Lock</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbluserlist" runat="server" Text="User Name" Font-Names="Book Antiqua"
                                                    Font-Size="Medium">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="Cbcamcalculationlock" runat="server" Text="Cam Calculation Lock"
                                                    Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_camlockusername" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                    ReadOnly="true" Width="113px" Style="top: 6px; left: 300px; position: absolute; font-family: 'Book Antiqua'"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                <asp:Panel ID="Panel3" runat="server" CssClass="MultipleSelectionDDL" Height="273px"
                                                    Width="165px" Style="top: 30px; left: 110px; position: absolute; display: none;">
                                                    <asp:CheckBox ID="cbcamlockuser" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnCheckedChanged="cbcamlockuser_CheckedChanged" Text="Select All"
                                                        AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="cblcamuserlock" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                        OnSelectedIndexChanged="cblcamuserlock_SelectedIndexChanged" Font-Bold="True"
                                                        Font-Names="Book Antiqua">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_camlockusername"
                                                    DynamicServicePath="" PopupControlID="Panel3" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 78px; width: 215px">
                                    <legend>Staff Leave Report Staff user setting</legend>
                                    <asp:CheckBox ID="chkstaffleavereport" runat="server" Text="Staff Leave Report Visible Department Wise"
                                        Font-Bold="True" Font-Names="Book Antiqua" />
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: 40px; width: 230px">
                                    <legend>Black Box</legend>
                                    <asp:CheckBox ID="chkblackboxacademic" runat="server" Text="Academic Department Only"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 40px; width: 278px">
                                    <legend>Cam Entry</legend>
                                    <asp:CheckBox ID="Chkcamstaff" runat="server" Text="Staff Subject Based On Time Table"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 40px; width: 215px;">
                                    <asp:CheckBox ID="chkelevonlyvisible" runat="server" Text="Elective Subject Only Show in Subject Allotment"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: 40px; width: 347px;">
                                    <legend>School</legend>
                                    <asp:CheckBox ID="CheckBoxschool" runat="server" Text="Report Card Through CAM Calculation"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                </fieldset>
                            </td>
                            <td colspan="3">
                                <fieldset style="height: 40px; width: 260px;">
                                    <legend>Bank Format Fixed Digit</legend>
                                    <asp:Label ID="Label33" runat="server" Text="Bank Format Fixed Digit" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:TextBox ID="txtbankdegit" runat="server" Width="60px" MaxLength="2" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" FilterType="Numbers"
                                        TargetControlID="txtbankdegit" Enabled="True" />
                                </fieldset>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: 40px; width: 365px;">
                                    <legend>DayOrder Wise Period Attendance Settings</legend>
                                    <asp:CheckBox ID="cbDayOrder" runat="server" Text="Use DayOrder Wise Period Attendace Schedule"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 60px; width: 300px">
                                    <legend>Cam Re-Test Mark Entry Settings</legend>
                                    <asp:CheckBox ID="cbRetestSettings" runat="server" Text="Re-Test Mark Entry Based On Optional Minimum Marks"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 80px; width: 210px">
                                    <legend>CAM Calculation Grade Settings</legend>
                                    <asp:CheckBox ID="chkShowGradeInCamCalulationDetails" runat="server" Text="Show Grade In CAM Calulation Details"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: 40px; width: 365px;">
                                    <legend>SMS Settings</legend>
                                    <asp:RadioButtonList ID="rblSmsIndividualCommon" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Selected="True">Common</asp:ListItem>
                                        <asp:ListItem>Individual</asp:ListItem>
                                    </asp:RadioButtonList>
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 50px; width: 300px;">
                                    <legend>Permission Time Settings</legend>Hour
                                    <asp:DropDownList ID="ddlHour" runat="server" CssClass="textbox1 ddlheight">
                                        <asp:ListItem Selected="True" Text="00"></asp:ListItem>
                                        <asp:ListItem Text="01"></asp:ListItem>
                                        <asp:ListItem Text="02"></asp:ListItem>
                                        <asp:ListItem Text="03"></asp:ListItem>
                                        <asp:ListItem Text="04"></asp:ListItem>
                                        <asp:ListItem Text="05"></asp:ListItem>
                                        <asp:ListItem Text="06"></asp:ListItem>
                                        <asp:ListItem Text="07"></asp:ListItem>
                                        <asp:ListItem Text="08"></asp:ListItem>
                                        <asp:ListItem Text="09"></asp:ListItem>
                                        <asp:ListItem Text="10"></asp:ListItem>
                                        <asp:ListItem Text="11"></asp:ListItem>
                                        <asp:ListItem Text="12"></asp:ListItem>
                                    </asp:DropDownList>
                                    Minutes
                                    <asp:DropDownList ID="ddlMin" runat="server" CssClass="textbox1 ddlheight">
                                    </asp:DropDownList>
                                </fieldset>
                            </td>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: 48px; width: 365px;">
                                    <legend>Common college Name</legend>
                                    <asp:TextBox ID="txt_common_collegename" CssClass="textbox txtheight5" runat="server"
                                        placeholder="Common College Name"></asp:TextBox>
                                    <asp:CheckBox ID="chkUseCommonCol" runat="server" Text="Apply" />
                                    <br />
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 48px; width: 365px;">
                                    <legend>Individual Staff Login Attendance</legend>
                                    <asp:CheckBox ID="CbNewStaffAttendance" runat="server" Text="Individual Staff Login Attendance New" />
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 48px; width: 207px;">
                                    <legend>IRun Master Student Import</legend>
                                    <asp:CheckBox ID="cbdegapp" runat="server" Text="Update Degree Code in Applyn" />
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
            <%--Settings Tab--%>
            <center>
                <div id="div2Settings" runat="server" style="width: 980px; height: 400px; background: white; border: 1px solid #0095E8; overflow: auto;"
                    visible="false">
                    <table>
                        <tr>
                            <td colspan="6">
                                <asp:Panel ID="Paneltree" runat="server" Height="300px" ScrollBars="Vertical" Width="800px"
                                    BorderWidth="1px">
                                    <asp:TreeView ID="TVset" runat="server" ShowCheckBoxes="All" Height="295px" Width="780px"
                                        Font-Names="Book Antiqua" Font-Size="Medium">
                                    </asp:TreeView>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Panel ID="divtree" runat="server" Height="300px" ScrollBars="Vertical" Width="800px"
                                    BorderWidth="1px">
                                    <asp:TreeView ID="TVStudentlog" runat="server" ShowCheckBoxes="All" Height="295px"
                                        Width="780px" Font-Names="Book Antiqua" Font-Size="Medium">
                                    </asp:TreeView>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <table style="border: 1px solid black; margin-left: 15px;">
                                    <tr>
                                        <td>
                                            <h3 style="color: Black; font-weight: bold;">Staff App Tab Rights</h3>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBoxList ID="chkStaffAppTabRights" runat="server" RepeatColumns="4" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="StaffAppTab_">Student Strength</asp:ListItem>
                                                <asp:ListItem Value="StaffAppTab_">Staff Strength</asp:ListItem>
                                                <asp:ListItem Value="StaffAppTab_">Student Attendance</asp:ListItem>
                                                <asp:ListItem Value="StaffAppTab_">Staff Attendance</asp:ListItem>
                                                <asp:ListItem Value="StaffAppTab_">Black Box</asp:ListItem>
                                                <asp:ListItem Value="StaffAppTab_">Attendance Completion Time</asp:ListItem>
                                                <asp:ListItem Value="StaffAppTab_">Fees Collection</asp:ListItem>
                                                <asp:ListItem Value="StaffAppTab_">Deposit</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <table style="border: 1px solid black; margin-left: 15px;">
                                    <tr>
                                        <td>
                                            <h3 style="color: Black; font-weight: bold;">Student App Tab Rights</h3>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBoxList ID="chkStudentAppTabRights" runat="server" RepeatColumns="4" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="StudentAppTab_">Request</asp:ListItem>
                                                <asp:ListItem Value="StudentAppTab_">Report</asp:ListItem>
                                                <asp:ListItem Value="StudentAppTab_">Attendance</asp:ListItem>
                                                <asp:ListItem Value="StudentAppTab_">Payment</asp:ListItem>
                                                <asp:ListItem Value="StudentAppTab_">PaymentOffline</asp:ListItem>
                                                <asp:ListItem Value="StudentAppTab_">Certificate</asp:ListItem>
                                                <asp:ListItem Value="StudentAppTab_">Xerox</asp:ListItem>
                                                <asp:ListItem Value="StudentAppTab_">Revaluation</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
            <%--Fine Amount Tab--%>
            <center>
                <div id="div3FineAmount" runat="server" style="width: 980px; height: 400px; background: white; border: 1px solid #0095E8; overflow: auto;"
                    visible="false">
                    <table>
                        <tr>
                            <td colspan="3">
                                <farpoint:fpspread id="fine_spread" runat="server" bordercolor="Black" borderstyle="Solid"
                                    borderwidth="1px" height="180" width="420" verticalscrollbarpolicy="Always" horizontalscrollbarpolicy="Always"
                                    showheaderselection="false">
                                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                        ButtonShadowColor="ControlDark">
                                    </CommandBar>
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </farpoint:fpspread>
                                <br />
                                <center>
                                    <asp:LinkButton ID="save_btn" runat="server" Font-Bold="True" Width="50px" Text="Save"
                                        Font-Names="Book Antiqua" Font-Size="Small" ForeColor="Black" OnClick="save_btn_Click"
                                        CausesValidation="False" Style="display: inline-block; color: Black; font-family: Book Antiqua; font-size: small; font-weight: bold; width: 50px; text-align: center; border-style: solid; border-width: 1px; border-color: gray; background-color: none; border-radius: 4px 4px 4px 4px; text-decoration: none;"></asp:LinkButton>
                                    <asp:LinkButton ID="delete_btn" runat="server" Font-Bold="True" Width="50px" Text="Delete"
                                        Font-Names="Book Antiqua" Font-Size="Small" ForeColor="Black" OnClick="delete_btn_Click"
                                        CausesValidation="False" Style="display: inline-block; color: Black; font-family: Book Antiqua; font-size: small; font-weight: bold; width: 50px; text-align: center; border-style: solid; border-width: 1px; border-color: gray; background-color: none; border-radius: 4px 4px 4px 4px; text-decoration: none;"></asp:LinkButton>
                                </center>
                            </td>
                            <td colspan="3">
                                <fieldset style="width: 470px; height: 175px; border-radius: 4px 4px 4px 4px;" class="cursor">
                                    <legend>Day Wise Absent Fine Amount</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblflabfine" runat="server" Text="Morning Absent Fine Amount"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtflabfine" runat="server" Width="100px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" TargetControlID="txtflabfine"
                                                    FilterType="Numbers,Custom" ValidChars="." />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblslabfine" runat="server" Text="Evening Absent Fine Amount"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtslabfine" runat="server" Width="100px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txtslabfine"
                                                    FilterType="Numbers,Custom" ValidChars="." />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:CheckBox ID="chkfinelesslave" runat="server" Text="Fine Amount Not for Leave Days" />
                                    <br />
                                    <asp:Button ID="btnabfine" runat="server" Text="Save" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btnabfine_Click" />
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <center>
                                    <fieldset style="height: 40px; width: 870px; height: 1825px; border-radius: 4px 4px 4px 4px;">
                                        <legend>Hostel / Inventory Settings </legend>
                                        <table>
                                            <tr>
                                                <td>
                                                    <fieldset style="height: 40px; width: 309px; height: 55px; border-radius: 4px 4px 4px 4px;"
                                                        class="cursor">
                                                        <legend>Hostel / Inventory Item Rights</legend>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="Cb_hostelitem" Text="Hostel Items" CssClass="cursor" runat="server"
                                                                        AutoPostBack="true" />
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Cb_otheritems" Text="Inventory Items" CssClass="cursor" runat="server"
                                                                        AutoPostBack="true" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                                <td>
                                                    <fieldset style="height: 55px; border-radius: 4px; width: 198px;" class="cursor">
                                                        <legend>Allow Additional Items</legend>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="Cb_Allowadditional" Text="Allow Additional Items" CssClass="cursor"
                                                                        runat="server" AutoPostBack="true" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                                <td>
                                                    <fieldset style="height: 55px; border-radius: 4px; width: 198px;" class="cursor">
                                                        <legend>Allow Return Item</legend>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="cballowretunitem" Text="Right for Return Item" CssClass="cursor"
                                                                        runat="server" AutoPostBack="true" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                        <fieldset style="height: 40px; width: 790px; height: 95px; border-radius: 4px 4px 4px 4px;"
                                            class="cursor">
                                            <legend>Letter Tab Rights</legend>
                                            <table cellspacing="5px;">
                                                <tr>
                                                    <td>
                                                        <asp:CheckBoxList ID="cbletter" runat="server" Font-Size="Medium" AutoPostBack="false"
                                                            Font-Names="Book Antiqua" RepeatDirection="Horizontal">
                                                        </asp:CheckBoxList>
                                                    </td>
                                                    <tr>
                                                        <td>
                                                            <asp:UpdatePanel ID="uup" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:Label ID="lbl_tapal_Header" Text="Header" runat="server"></asp:Label>
                                                                    <asp:DropDownList ID="ddl_tapal_header" runat="server" CssClass="ddlheight5 textbox textbox1"
                                                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_tapal_header_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <asp:Label ID="lbl_tapal_ledger" Text="Ledger" runat="server"></asp:Label>
                                                                    <asp:DropDownList ID="ddl_tapal_ledger" runat="server" CssClass="ddlheight5 textbox textbox1"
                                                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_tapal_ledger_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    </td>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                            </table>
                                        </fieldset>
                                        <fieldset style="height: 40px; width: 790px; height: 85px; border-radius: 4px 4px 4px 4px;"
                                            class="cursor">
                                            <legend>Gate Pass Tab Rights</legend>
                                            <table cellspacing="5px;">
                                                <tr>
                                                    <td>
                                                        <asp:CheckBoxList ID="cbl_gatepass" runat="server" Font-Size="Medium" AutoPostBack="false"
                                                            Font-Names="Book Antiqua" RepeatDirection="Horizontal">
                                                        </asp:CheckBoxList>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="cb_gatepass_hostler" runat="server" Text="Hostler" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="cb_gatepass_DayScholar" runat="server" Text="Day Scholar" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                        <table>
                                            <tr>
                                                <td>
                                                    <fieldset style="height: 40px; width: 600px; height: 55px; border-radius: 4px 4px 4px 4px;"
                                                        class="cursor">
                                                        <legend>SMS Rights</legend>
                                                        <table cellspacing="5px;">
                                                            <tr>
                                                                <td>
                                                                    <span>Request Type</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddl_requesttype" CssClass="ddlheight2 textbox textbox1" runat="server">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="cb_request" runat="server" Text="Request" />
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="cb_approval" runat="server" Text="Approval" />
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="cb_exit" runat="server" Text="Exit" />
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="cb_reject" runat="server" Text="Reject" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                                <td>
                                                    <fieldset style="height: 40px; width: 232px; height: 55px; border-radius: 4px 4px 4px 4px;"
                                                        class="cursor">
                                                        <legend>SMS Mobile Rights</legend>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="cb_mother" runat="server" Text="Mother" />
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="cb_father" runat="server" Text="Father" />
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="cb_student" runat="server" Text="Student" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                        <fieldset style="height: 60px; width: 870px; height: 200px; border-radius: 4px 4px 4px 4px;"
                                            class="cursor">
                                            <legend>Request Tab Rights</legend>
                                            <table cellspacing="5px;">
                                                <tr>
                                                    <td colspan="7">
                                                        <asp:CheckBoxList ID="cbl_request" runat="server" Font-Size="Medium" AutoPostBack="false"
                                                            Font-Names="Book Antiqua" RepeatDirection="Horizontal">
                                                        </asp:CheckBoxList>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table>
                                                <tr>
                                                    <td colspan="4">
                                                        <fieldset style="height: 40px; width: 180px; height: 45px; border-radius: 4px 4px 4px 4px;"
                                                            class="cursor">
                                                            <legend>Gatepass Rights</legend>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:RadioButton ID="rdb_gatepass_staff" Text="Staff" runat="server" GroupName="gstaff" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rdb_gatepass_dept" Text="Department" runat="server" GroupName="gstaff" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                        <fieldset style="height: 40px; width: 226px; margin-left: 220px; margin-top: -66px; height: 45px; border-radius: 4px 4px 4px 4px;"
                                                            class="cursor">
                                                            <legend>Gatepass Staff Permission</legend>
                                                            <table>
                                                                <tr>
                                                                    <td>Count
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txt_staff_per_count" runat="server" CssClass="textbox textbox txtheight"
                                                                            MaxLength="2"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="ftext_cname" runat="server" TargetControlID="txt_staff_per_count"
                                                                            FilterType="numbers" ValidChars="">
                                                                        </asp:FilteredTextBoxExtender>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                        <%--16.03.17 barath--%>
                                                        <fieldset style="height: 40px; width: 300px; height: 45px; border-radius: 4px 4px 4px 4px; margin-left: 485px; margin-top: -66px;"
                                                            class="cursor">
                                                            <legend>Gatepass Request Type</legend>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:RadioButton ID="rdb_withreq" Text="With Request" runat="server" GroupName="with" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rdb_withoutreq" Text="With Out Request" runat="server" GroupName="with" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <fieldset style="height: 53px; width: 850px; border-radius: 4px 4px 4px 4px;" class="cursor">
                                                            <legend>Leave Approval Permission</legend>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:RadioButton ID="rdo_leaveindiv" Text="Request Based On Hierarchy" runat="server"
                                                                            GroupName="reqleave" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rdo_leavedirect" Text="Single Person Requested for All Staff & Approved By Hierarchy"
                                                                            runat="server" GroupName="reqleave" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rdo_leavewithout" Text="Request & Approval By Same Person" runat="server"
                                                                            GroupName="reqleave" />
                                                                    </td>
                                                                </tr>

                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>

                                        <fieldset style="height: 53px; width: 850px; border-radius: 4px 4px 4px 4px;" class="cursor">
                                            <legend>Leave Delete Permission</legend>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdb_delwoHierarchy" Text="Selected Date Delete" runat="server"
                                                            GroupName="reqdel" />
                                                    </td>
                                                    <td>

                                                        <asp:RadioButton ID="rdb_delwHierarchy" Text="Delete Based On Request" runat="server"
                                                            GroupName="reqdel" />
                                                    </td>

                                                </tr>

                                            </table>
                                        </fieldset>


                                        <fieldset style="height: 40px; width: 818px; height: 65px; border-radius: 4px 4px 4px 4px;"
                                            class="cursor">
                                            <legend>Request Hierarchy Rights</legend>
                                            <table cellspacing="5px;">
                                                <tr>
                                                    <td>
                                                        <asp:CheckBoxList ID="cb_reqHierarchy" runat="server" Font-Size="Medium" AutoPostBack="false"
                                                            RepeatColumns="7" Font-Names="Book Antiqua" RepeatDirection="Horizontal">
                                                        </asp:CheckBoxList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                        <br />
                                        <table>
                                            <tr>
                                                <td>
                                                    <fieldset style="height: 40px; width: 278px; height: 65px; border-radius: 4px 4px 4px 4px;"
                                                        class="cursor">
                                                        <legend>Service Plan</legend>
                                                        <table cellspacing="5px;">
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rdb_serviceplan" Text="Service Plan" runat="server" GroupName="re" />
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdb_serviceallitem" Text="All Item Service Plan" runat="server"
                                                                        GroupName="re" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                                <td>
                                                    <fieldset style="height: 40px; width: 226px; height: 65px; border-radius: 4px 4px 4px 4px;"
                                                        class="cursor">
                                                        <legend>Daily Consumption Setting</legend>
                                                        <table cellspacing="5px;">
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="cb_reconsumption" Text="Allow Reconsumption" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                                <td>
                                                    <fieldset style="height: 40px; width: 226px; height: 65px; border-radius: 4px 4px 4px 4px;"
                                                        class="cursor">
                                                        <legend>Mess Rights</legend>
                                                        <table cellspacing="5px;">
                                                            <tr>
                                                                <td>
                                                                    <span>Mess</span>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="upp1" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox ID="txt_messname" runat="server" CssClass="textbox textbox1 txtheight1">--Select--</asp:TextBox>
                                                                            <asp:Panel ID="p1" runat="server" Height="150px" Width="160px" Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;">
                                                                                <asp:CheckBox ID="cb_hos" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_mess_CheckedChange" />
                                                                                <asp:CheckBoxList ID="cbl_hos" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_mess_SelectedIndexChange">
                                                                                </asp:CheckBoxList>
                                                                            </asp:Panel>
                                                                            <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_messname"
                                                                                PopupControlID="p1" Position="Bottom">
                                                                            </asp:PopupControlExtender>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                        <table>
                                            <tr>
                                                <td>
                                                    <fieldset style="height: 70px; width: 572px; height: 65px; border-radius: 4px 4px 4px 4px;"
                                                        class="cursor">
                                                        <legend>Hrs Code Settings</legend>
                                                        <table cellspacing="5px;">
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBoxList ID="cb_hrscodesetting" runat="server" Font-Size="Medium" AutoPostBack="false"
                                                                        Font-Names="Book Antiqua" RepeatDirection="Horizontal">
                                                                    </asp:CheckBoxList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                                <td>
                                                    <fieldset style="height: 40px; width: 157px; height: 65px; border-radius: 4px 4px 4px 4px;"
                                                        class="cursor">
                                                        <legend>Mess Bill Calculation</legend>
                                                        <table cellspacing="5px;">
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="cb_messaffectedfinance" runat="server" Text=" Include Finance" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                        <fieldset style="height: 70px; width: 800px; height: 90px; border-radius: 4px 4px 4px 4px;"
                                            class="cursor">
                                            <legend>Staff Manager Tab Rights</legend>
                                            <table cellspacing="5px;">
                                                <tr>
                                                    <td>
                                                        <asp:CheckBoxList ID="smtabrights" runat="server" Font-Size="Medium" AutoPostBack="false"
                                                            Font-Names="Book Antiqua" RepeatDirection="Horizontal" RepeatColumns="8">
                                                        </asp:CheckBoxList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                        <table>
                                            <tr>
                                                <td>
                                                    <fieldset style="height: 50px; width: 332px; height: 65px; border-radius: 4px 4px 4px 4px;"
                                                        class="cursor">
                                                        <legend>Daily Consumption RPU / Sailing prize Settings</legend>
                                                        <table cellspacing="5px;">
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rdo_rpu" Text="Rate per unit" runat="server" GroupName="ds" />
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdo_sailing" Text="Sailing prize" runat="server" GroupName="ds" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                                <td>
                                                    <fieldset style="height: 40px; width: 212px; height: 65px; border-radius: 4px 4px 4px 4px;"
                                                        class="cursor">
                                                        <legend>Store Rights</legend>
                                                        <table cellspacing="5px;">
                                                            <tr>
                                                                <td>
                                                                    <span>Store Name</span>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                                        <ContentTemplate>
                                                                            <div style="position: relative;">
                                                                                <asp:TextBox ID="txt_Store" runat="server" CssClass="textbox textbox1 txtheight1">--Select--</asp:TextBox>
                                                                                <asp:Panel ID="Panel12" runat="server" Height="150px" Width="160px" Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px; margin-top: 0px; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;">
                                                                                    <asp:CheckBox ID="cb_store" runat="server" Text="Select All" AutoPostBack="true"
                                                                                        OnCheckedChanged="cb_store_all_CheckedChange" />
                                                                                    <asp:CheckBoxList ID="cbl_store" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_store_SelectedIndexChange">
                                                                                    </asp:CheckBoxList>
                                                                                </asp:Panel>
                                                                                <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_Store"
                                                                                    PopupControlID="Panel12" Position="Bottom">
                                                                                </asp:PopupControlExtender>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                        <table>
                                            <tr>
                                                <td>
                                                    <fieldset style="height: 40px; width: 300px; height: 65px; border-radius: 4px 4px 4px 4px;"
                                                        class="cursor">
                                                        <legend>Alternate Subject Rights</legend>
                                                        <table cellspacing="5px;">
                                                            <tr>
                                                                <td>
                                                                    <span>Subject Type</span>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                        <ContentTemplate>
                                                                            <div style="position: relative;">
                                                                                <asp:TextBox ID="txtaltsub" runat="server" CssClass="textbox textbox1 txtheight1">--Select--</asp:TextBox>
                                                                                <asp:Panel ID="Panel11" runat="server" Height="150px" Width="160px" Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px; margin-top: 0px; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;">
                                                                                    <asp:CheckBox ID="cbaltsub" runat="server" Text="Select All" AutoPostBack="true"
                                                                                        OnCheckedChanged="cbaltsub_CheckedChange" />
                                                                                    <asp:CheckBoxList ID="cblaltsub" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblaltsub_SelectedIndexChange">
                                                                                    </asp:CheckBoxList>
                                                                                </asp:Panel>
                                                                                <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txtaltsub"
                                                                                    PopupControlID="Panel11" Position="Bottom">
                                                                                </asp:PopupControlExtender>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                                <td>
                                                    <fieldset style="height: 40px; width: 385px; height: 65px; border-radius: 4px 4px 4px 4px;"
                                                        class="cursor">
                                                        <legend>Hostel Student Admitted Sms Rights</legend>
                                                        <asp:CheckBox ID="cb_hostelsms" runat="server" Text="Send Admit sms Rights" />
                                                        <asp:CheckBox ID="cb_hostelvacatedsms" runat="server" Text="Send Vacated sms Rights" />
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%--barath 15.03.17--%>
                                                    <fieldset style="width: 130px; height: 50px; border-radius: 4px 4px 4px 4px;" class="cursor">
                                                        <legend>Print Settings</legend>
                                                        <asp:CheckBoxList ID="cbl_excelpdf" runat="server" RepeatColumns="2">
                                                            <asp:ListItem Value="E">Excel</asp:ListItem>
                                                            <asp:ListItem Value="P">Pdf</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                    </fieldset>
                                                    <%--barath 29.03.17--%>
                                                    <td>
                                                        <fieldset style="height: 140px; width: 300px;">
                                                            <legend>Hostel Admission Form Fee</legend>
                                                            <table>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:CheckBox ID="cb_hosteladmissionformfee" runat="server" Text="Hostel Admission Form Fee" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label45" runat="server" Text="Header"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddl_admissionH" runat="server" CssClass="textbox textbox1 ddlheight5"
                                                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_admissionH_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label46" runat="server" Text="Ledger"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddl_admissionL" runat="server" CssClass="textbox textbox1 ddlheight5">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label47" runat="server" Text="Amount"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txt_admissionfee" runat="server" placeholder="0.00" CssClass="textbox txtheight"
                                                                            Style="text-align: right; width: 80px; height: 15px;" BackColor="#EFF8D5" MaxLength="15">
                                                                        </asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" FilterType="Numbers,Custom"
                                                                            ValidChars="." TargetControlID="txt_admissionfee">
                                                                        </asp:FilteredTextBoxExtender>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan='2'>
                                                    <fieldset style="height: 220px; overflow: auto; width: 850px; border-radius: 4px 4px 4px 4px;"
                                                        class="cursor">
                                                        <legend>Tc Format Settings</legend>
                                                        <table cellspacing="4px;">
                                                            <%--<tr>
                                                    <td>
                                                        <asp:CheckBoxList ID="Cbl_tcformate" runat="server" Font-Size="Medium" AutoPostBack="false"
                                                            Font-Names="Book Antiqua" RepeatDirection="Horizontal" RepeatColumns="5">
                                                        </asp:CheckBoxList>
                                                    </td>
                                                </tr>--%>
                                                            <tr>
                                                                <asp:GridView ID="certificateformat_grid" runat="server" AutoGenerateColumns="false"
                                                                    Width="300px" Style="margin-top: 10px; font-size: small;" HeaderStyle-BackColor="#0CA6CA"
                                                                    HeaderStyle-ForeColor="White">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="S.No">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Certificate Name">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl_certificatename" runat="server" Text='<%# Eval("CertificateName") %>'></asp:Label>
                                                                                <asp:Label ID="lbl_certificateId" runat="server" Visible="false" Text='<%# Eval("Certificate_ID") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Format">
                                                                            <ItemTemplate>
                                                                                <asp:DropDownList ID="ddlFormattype" Width="80px" runat="server">
                                                                                </asp:DropDownList>

                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                                <asp:Button ID="formatsave" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                                                    ForeColor="White" Font-Bold="true" OnClick="formatsave_Click" Style="margin-left: 220px; width: 80px;" />
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <fieldset style="height: 35px; overflow: auto; width: 225px; border-radius: 4px 4px 4px 4px;"
                                                        class="cursor">
                                                        <legend>Approval leave Cancel Rights</legend>
                                                        <asp:CheckBox ID="cbApprovalLeave" runat="server" Text="Permission" />
                                                    </fieldset>
                                                </td>
                                                <td>
                                                    <fieldset style="height: 35px; overflow: auto; width: 303px; border-radius: 4px 4px 4px 4px;"
                                                        class="cursor">
                                                        <legend>With Request Cancel Based On Hierarchy </legend>
                                                        <asp:CheckBox ID="cb_requestforcancel" runat="server" Text="Request Cancel Setting" />
                                                    </fieldset>


                                                </td>

                                                <td>
                                                    <%--poomalar 29.11.17--%>
                                                    <fieldset style="height: 35px; overflow: auto; width: 225px; border-radius: 4px 4px 4px 4px;"
                                                        class="cursor">
                                                        <legend style="height: 5">Future Attendance</legend>
                                                        <asp:CheckBox ID="cb_allowfuture" runat="server" Text="Allow" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="medium" Style="margin: 0px; padding: 0px;" />
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <fieldset style="height: 50px; overflow: auto; width: 225px; border-radius: 4px 4px 4px 4px;"
                                                        class="cursor">
                                                        <legend>Send Sms While Request For Leave</legend>
                                                        <asp:CheckBox ID="cb_sendsmswhilerequest" runat="server" Text="Sms Rights" />
                                                    </fieldset>

                                                </td>
                                                <td>
                                                    <fieldset style="height: 35px; overflow: auto; width: 300px; border-radius: 4px 4px 4px 4px;"
                                                        class="cursor">
                                                        <legend>Change LeaveType While Approve</legend>
                                                        <asp:CheckBox ID="cbchangeleavetype" runat="server" Text="Leave Type Change Permission" />
                                                    </fieldset>
                                                </td>


                                            </tr>
                                        </table>
                                    </fieldset>
                                </center>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Label ID="lblerror" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
            <%--Finance Print Settings Tab--%>
            <center>
                <div id="div4FinPrintSet" runat="server" style="width: 980px; height: 400px; background: white; border: 1px solid #0095E8; overflow: auto;"
                    visible="false">
                    <table>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: 250px; width: 313px;">
                                    <legend>Finance Year Setting</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_finname" runat="server" Text="Finance Year"></asp:Label>
                                            </td>
                                            <td>
                                                <div style="position: relative;">
                                                    <asp:TextBox ID="txt_finname" runat="server" CssClass="textbox txtheight2" Style="width: 200px"
                                                        ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="pnl_str" runat="server" CssClass="multxtpanel " Style="width: 204px; border-color: HighlightText; height: 150px;">
                                                        <asp:CheckBox ID="chk_fin" runat="server" Width="204px" Text="Select All" AutoPostBack="true"
                                                            OnCheckedChanged="chk_fin_OnCheckedChanged" />
                                                        <asp:CheckBoxList ID="chkl_fin" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_fin_OnSelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <td colspan="4">
                                <fieldset style="height: 250px; width: 570px;">
                                    <legend>Header And Ledger Setting</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label39" runat="server" Text="Header"></asp:Label>
                                            </td>
                                            <td>
                                                <div style="position: relative;">
                                                    <asp:TextBox ID="txt_head" runat="server" CssClass="textbox txtheight2" Style="width: 204px;"
                                                        ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel " Style="width: 211px; border-color: HighlightText; height: 150px;">
                                                        <asp:CheckBox ID="chk_head" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                            OnCheckedChanged="chk_head_OnCheckedChanged" />
                                                        <asp:CheckBoxList ID="chkl_head" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_head_OnSelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                </div>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label40" runat="server" Style="margin-left: 57px;" Text="Ledger"></asp:Label>
                                            </td>
                                            <td>
                                                <div style="position: relative;">
                                                    <asp:TextBox ID="txt_ledg" runat="server" CssClass="textbox txtheight2" Style="width: 182px;"
                                                        ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="Panel5" runat="server" CssClass="multxtpanel " Style="width: 186px; border-color: HighlightText; height: 150px;">
                                                        <asp:CheckBox ID="chk_ledg" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                            OnCheckedChanged="chk_ledg_OnCheckedChanged" />
                                                        <asp:CheckBoxList ID="chkl_ledg" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkl_ledg_OnSelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <fieldset style="height: 855px; width: 935px;">
                                    <table>
                                        <tr>
                                            <td colspan="4">
                                                <fieldset style="width: 900px; height: 200px;">
                                                    <legend>
                                                        <asp:CheckBox ID="cb_AllowOtCol" runat="server" AutoPostBack="false" OnCheckedChanged="cb_otCol_CheckedChanged" />Allow
                                                        Other Colleges For Users</legend>
                                                    <div style="width: 200px; height: 150px; float: left;">
                                                        College
                                                        <asp:DropDownList ID="ddl_otColleges" runat="server" CssClass="textbox ddlheight2"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_otCol_CheckedChanged">
                                                        </asp:DropDownList>
                                                        <br />
                                                        <br />
                                                    </div>
                                                    <div style="width: 220px; height: 150px; float: left;">
                                                        <div style="position: relative;">
                                                            Finance Year
                                                            <asp:TextBox ID="txt_OtColfin" runat="server" CssClass="textbox txtheight2" Style="width: 110px;"
                                                                ReadOnly="true">Finance Year</asp:TextBox>
                                                            <asp:Panel ID="Panel6" runat="server" CssClass="multxtpanel " Style="width: 204px; border-color: HighlightText; height: 140px;">
                                                                <asp:CheckBox ID="chk_finOt" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                    OnCheckedChanged="chk_finOt_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="chkl_finOt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkl_finOt_OnIndexChange">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                        </div>
                                                    </div>
                                                    <div style="width: 220px; height: 150px; float: left;">
                                                        <div style="position: relative;">
                                                            Header
                                                            <asp:TextBox ID="txt_OtColhdr" runat="server" CssClass="textbox txtheight2" Style="width: 140px;"
                                                                ReadOnly="true">Header</asp:TextBox>
                                                            <asp:Panel ID="Panel7" runat="server" CssClass="multxtpanel " Style="width: 211px; border-color: HighlightText; height: 140px;">
                                                                <asp:CheckBox ID="chk_headOt" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                    OnCheckedChanged="chk_headOt_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="chkl_headOt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkl_headOt_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                        </div>
                                                    </div>
                                                    <div style="width: 220px; height: 150px; float: left;">
                                                        <div style="position: relative;">
                                                            Ledger<asp:TextBox ID="txt_OtColLdg" runat="server" CssClass="textbox txtheight2"
                                                                Style="width: 140px;" ReadOnly="true">Ledger</asp:TextBox>
                                                            <asp:Panel ID="Panel9" runat="server" CssClass="multxtpanel " Style="width: 211px; border-color: HighlightText; height: 140px;">
                                                                <asp:CheckBox ID="chk_ledgOt" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                    OnCheckedChanged="chk_ledgOt_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="chkl_ledgOt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkl_ledgOt_Indexchanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                        </div>
                                                    </div>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div>
                                                    <fieldset style="width: 250px; height: 207px;">
                                                        <legend>Fine Ledger Setting</legend>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label59" runat="server" Text="Batch"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox ID="txtObatch" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                                            <asp:Panel ID="panel16" runat="server" CssClass="multxtpanel" Style="width: 121px; height: 200px;">
                                                                                <asp:CheckBox ID="cbObatch" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                                    OnCheckedChanged="cbObatch_OnCheckedChanged" />
                                                                                <asp:CheckBoxList ID="cblObatch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblObatch_OnSelectedIndexChanged">
                                                                                </asp:CheckBoxList>
                                                                            </asp:Panel>
                                                                            <asp:PopupControlExtender ID="PopupControlExtender13" runat="server" TargetControlID="txtObatch"
                                                                                PopupControlID="panel16" Position="Bottom">
                                                                            </asp:PopupControlExtender>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label60" runat="server" Text="Degree"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox ID="txtOdegree" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                                            <asp:Panel ID="panel17" runat="server" CssClass="multxtpanel" Style="width: 150px; height: 200px;">
                                                                                <asp:CheckBox ID="cbOdegree" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                                    OnCheckedChanged="cbOdegree_OnCheckedChanged" />
                                                                                <asp:CheckBoxList ID="cblOdegree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblOdegree_OnSelectedIndexChanged">
                                                                                </asp:CheckBoxList>
                                                                            </asp:Panel>
                                                                            <asp:PopupControlExtender ID="PopupControlExtender14" runat="server" TargetControlID="txtOdegree"
                                                                                PopupControlID="panel17" Position="Bottom">
                                                                            </asp:PopupControlExtender>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label61" runat="server" Text="Department"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox ID="txtOdept" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                                            <asp:Panel ID="panel18" runat="server" CssClass="multxtpanel" Style="width: 250px; height: 300px;">
                                                                                <asp:CheckBox ID="cbOdept" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                                    OnCheckedChanged="cbOdept_OnCheckedChanged" />
                                                                                <asp:CheckBoxList ID="cblOdept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblOdept_OnSelectedIndexChanged">
                                                                                </asp:CheckBoxList>
                                                                            </asp:Panel>
                                                                            <asp:PopupControlExtender ID="PopupControlExtender15" runat="server" TargetControlID="txtOdept"
                                                                                PopupControlID="panel18" Position="Bottom">
                                                                            </asp:PopupControlExtender>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblhd" runat="server" Text="Header"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddl_otFineHeader" runat="server" CssClass="textbox ddlheight2"
                                                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_otFineHeader_OnSelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label62" runat="server" Text="Ledger"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddl_otFineLedger" runat="server" CssClass="textbox ddlheight2">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lbl_FineLedger2" runat="server" Text="Fine Ledger"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddl_Fineledger2" runat="server" CssClass="textbox ddlheight2">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>


                                                        </table>
                                                    </fieldset>
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <fieldset style="width: 250px; height: 50px;">
                                                        <legend>Transport Ledger Setting</legend>
                                                        <asp:DropDownList ID="ddl_otTransHeader" runat="server" CssClass="textbox ddlheight2"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_otTransHeader_OnSelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="ddl_otTransLedger" runat="server" CssClass="textbox ddlheight2">
                                                        </asp:DropDownList>
                                                    </fieldset>
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <fieldset style="width: 284px; height: 180px;">
                                                        <legend>Re-Admission Fees Settings</legend>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label63" runat="server" Text="Batch"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox ID="txtORbatch" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                                            <asp:Panel ID="panel19" runat="server" CssClass="multxtpanel" Style="width: 121px; height: 200px;">
                                                                                <asp:CheckBox ID="cbORbatch" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                                    OnCheckedChanged="cbORbatch_OnCheckedChanged" />
                                                                                <asp:CheckBoxList ID="cblORbatch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblORbatch_OnSelectedIndexChanged">
                                                                                </asp:CheckBoxList>
                                                                            </asp:Panel>
                                                                            <asp:PopupControlExtender ID="PopupControlExtender16" runat="server" TargetControlID="txtORbatch"
                                                                                PopupControlID="panel19" Position="Bottom">
                                                                            </asp:PopupControlExtender>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label64" runat="server" Text="Degree"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox ID="txtORdegree" runat="server" Style="height: 20px; width: 100px;"
                                                                                ReadOnly="true">--Select--</asp:TextBox>
                                                                            <asp:Panel ID="panel20" runat="server" CssClass="multxtpanel" Style="width: 150px; height: 200px;">
                                                                                <asp:CheckBox ID="cbORdegree" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                                    OnCheckedChanged="cbORdegree_OnCheckedChanged" />
                                                                                <asp:CheckBoxList ID="cblORdegree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblORdegree_OnSelectedIndexChanged">
                                                                                </asp:CheckBoxList>
                                                                            </asp:Panel>
                                                                            <asp:PopupControlExtender ID="PopupControlExtender17" runat="server" TargetControlID="txtORdegree"
                                                                                PopupControlID="panel20" Position="Bottom">
                                                                            </asp:PopupControlExtender>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label65" runat="server" Text="Department"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox ID="txtORdept" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                                            <asp:Panel ID="panel21" runat="server" CssClass="multxtpanel" Style="width: 250px; height: 300px;">
                                                                                <asp:CheckBox ID="cbORDept" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                                    OnCheckedChanged="cbORDept_OnCheckedChanged" />
                                                                                <asp:CheckBoxList ID="cblORdept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblORdept_OnSelectedIndexChanged">
                                                                                </asp:CheckBoxList>
                                                                            </asp:Panel>
                                                                            <asp:PopupControlExtender ID="PopupControlExtender18" runat="server" TargetControlID="txtORdept"
                                                                                PopupControlID="panel21" Position="Bottom">
                                                                            </asp:PopupControlExtender>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblhdr" runat="server" Text="Header"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlreadmhdsnd" runat="server" CssClass="textbox ddlheight2"
                                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlreadmhdsnd_OnSelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label66" runat="server" Text="Ledger"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlreadmldsnd" runat="server" CssClass="textbox ddlheight2">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </div>
                                            </td>
                                            <td colspan="2">
                                                <br />
                                                <br />
                                                <br />

                                            </td>
                                        </tr>
                                        <tr>
                                            <%--added by sudhagar 19.06.2017 journal entry setting other college--%>
                                            <td>
                                                <div>
                                                    <fieldset style="width: 250px; height: 50px;">
                                                        <legend>Journal Fees Settings</legend>
                                                        <asp:DropDownList ID="ddlOtherJrHed" runat="server" CssClass="textbox ddlheight2"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlOtherJrHed_OnSelected">
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="ddlOtherJrLed" runat="server" CssClass="textbox ddlheight2">
                                                        </asp:DropDownList>
                                                    </fieldset>
                                                </div>
                                            </td>
                                            <td colspan="2">
                                                <div>
                                                    <fieldset style="width: 500px; height: 50px;">
                                                        <legend>Transfer / Refund Settings</legend>
                                                        <asp:CheckBoxList ID="cbl_transferrefund" runat="server" RepeatColumns="5">
                                                            <asp:ListItem Value="1">Transfer</asp:ListItem>
                                                            <asp:ListItem Value="2">Discontinue</asp:ListItem>
                                                            <asp:ListItem Value="3">Refund</asp:ListItem>
                                                            <asp:ListItem Value="4">Journal</asp:ListItem>
                                                            <asp:ListItem Value="5">ProlongAbsent</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                    </fieldset>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:CheckBox ID="chk_yr" runat="server" AutoPostBack="true" Text="Display Current Sem/Year in Journal" />&nbsp&nbsp
                                                <asp:CheckBox ID="chk_stud" runat="server" AutoPostBack="true" Text="Student Receipt" />&nbsp&nbsp
                                                <asp:CheckBox ID="chk_duplicate" runat="server" AutoPostBack="true" Text="Duplicate Receipt" />&nbsp&nbsp
                                                <asp:CheckBox ID="chk_print" runat="server" AutoPostBack="true" Text="PrePrint" />&nbsp&nbsp
                                                <asp:CheckBox ID="chk_editrpt" runat="server" AutoPostBack="true" Text="Edit Receipt Date" />
                                                <asp:CheckBox ID="chk_delRcpt" runat="server" AutoPostBack="true" Text="Delete Receipt" />&nbsp&nbsp
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:CheckBox ID="chk_canrpt" runat="server" AutoPostBack="true" Text="Cancel Receipt" />&nbsp&nbsp
                                                <asp:CheckBox ID="chk_lsdate" runat="server" AutoPostBack="true" Text="Lock Start Date" />&nbsp&nbsp
                                                <asp:CheckBox ID="chk_fees" runat="server" AutoPostBack="true" Text="Fees(Yearwise)"
                                                    Visible="false" />&nbsp&nbsp
                                                <asp:CheckBox ID="chk_paid" runat="server" AutoPostBack="true" Text="Yet to be paid Selected date" />&nbsp&nbsp&nbsp
                                                <asp:CheckBox ID="chk_bank" runat="server" AutoPostBack="true" Text="Bank Statement Page Settings" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chk_chall" runat="server" AutoPostBack="true" Text="Challan Page Settings" />
                                                &nbsp&nbsp
                                                <asp:CheckBox ID="chk_challhed" runat="server" AutoPostBack="true" Text="Selected Header in Challan Print" />
                                                <asp:CheckBox ID="chk_transfee" runat="server" AutoPostBack="true" Text="Transport Fees" />
                                            </td>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chk_semyear" runat="server" Text="SemesterandYear" />
                                                <asp:CheckBox ID="chk_dd" runat="server" Text="Automatically clear DD" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:CheckBox ID="cb_autoClrCheque" runat="server" Text="Automatically clear Cheque" />
                                                <asp:CheckBox ID="cballledger" runat="server" Text="Show All Ledger" />
                                                <asp:CheckBox ID="cbcontra" runat="server" Text="Cash Deposit Cumulative" />
                                                <asp:CheckBox ID="cbclrddcnl" runat="server" Text="Clear DD to Bounce" />
                                                <asp:CheckBox ID="cbchlrcpt" runat="server" Text="Challan Receipt" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:CheckBox ID="cbeditrcptno" runat="server" Text="Edit Receipt No" />
                                                <%-- </td>
                                            <td>--%>
                                                <asp:CheckBox ID="cbincDueAmt" runat="server" Text="Include Due Amount" />
                                                <asp:CheckBox ID="cbdeduct" runat="server" Text="Include Concession in Journal" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%--  <fieldset style="width: 217px; height: auto;">--%>
                                                <%-- <legend>Fees Sem/Year/Term Wise</legend>--%>
                                                <%-- <asp:RadioButton ID="rdbfeesem" runat="server" Text="Semester" GroupName="fe" />
                                                    <asp:RadioButton ID="rdbfeeyear" runat="server" Text="Year" GroupName="fe" />
                                                    <asp:RadioButton ID="rdbfeeterm" runat="server" Text="Term" GroupName="fe" />
                                                </fieldset>--%>
                                                <asp:CheckBoxList ID="cbl_Feecategory" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="0">Semester</asp:ListItem>
                                                    <asp:ListItem Value="1">Year</asp:ListItem>
                                                    <asp:ListItem Value="2">Term</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </td>
                                            <td colspan="3">
                                                <fieldset style="height: auto; width: 298px;">
                                                    <legend>Fine Type</legend>
                                                    <asp:CheckBox ID="cbFineTypeSnd" runat="server" Text="Use Ledgerwise Fine Amount" />
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <fieldset style="width: 300px">
                                                    <legend>Receipt Tab Rights</legend>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="cbRcptStudO" runat="server" Text="Student" />
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="cbRcptStaffO" runat="server" Text="Staff" />
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="cbRcptVendorO" runat="server" Text="Vendor" />
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="cbRcptOtherO" runat="server" Text="Other" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                            <td colspan="3">
                                                <fieldset style="width: 300px">
                                                    <legend>Payment Tab Rights</legend>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="cbPayStudO" runat="server" Text="Student" />
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="cbPayStaffO" runat="server" Text="Staff" />
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="cbPayVendorO" runat="server" Text="Vendor" />
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="cbPayOtherO" runat="server" Text="Other" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td colspan="3">

                                                <asp:Button ID="btnOtCollSave" Font-Bold="true" runat="server" OnClick="btnOtCollSave_OnClick"
                                                    Text="Other College Setting Save" BackColor="DarkTurquoise" Width="200px" Height="34px" />

                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <table>
                                    <tr>
                                        <td>
                                            <fieldset style="width: 250px;">
                                                <legend>Challan/Receipt and Default</legend>
                                                <asp:CheckBoxList ID="cbl_RcptChln" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" RepeatDirection="Horizontal">
                                                    <asp:ListItem>Receipt</asp:ListItem>
                                                    <asp:ListItem>Challan</asp:ListItem>
                                                </asp:CheckBoxList>
                                                <asp:RadioButtonList ID="rbl_ChlRcptPriority" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True">Receipt</asp:ListItem>
                                                    <asp:ListItem>Challan - Priority</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <fieldset style="width: 300px;">
                                                <legend>Header Type</legend>
                                                <asp:CheckBoxList ID="cbl_typeOfHdr" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" RepeatDirection="Horizontal">
                                                    <asp:ListItem>Group Header</asp:ListItem>
                                                    <asp:ListItem>Header</asp:ListItem>
                                                    <asp:ListItem>Ledger</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <fieldset style="width: 216px;">
                                                <legend>Header Wise</legend>
                                                <asp:CheckBoxList ID="cbl_HdrwiseChlnRcpt" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" RepeatDirection="Horizontal">
                                                    <asp:ListItem>Challan</asp:ListItem>
                                                    <asp:ListItem>Receipt</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <fieldset style="width: 600px;">
                                                <legend>Preferred Unique ID</legend>
                                                <asp:CheckBoxList ID="cbl_RollRegAdmNo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" RepeatDirection="Horizontal">
                                                    <asp:ListItem>Roll No</asp:ListItem>
                                                    <asp:ListItem>Reg No</asp:ListItem>
                                                    <asp:ListItem>Admission No</asp:ListItem>
                                                    <asp:ListItem>Application No</asp:ListItem>
                                                    <asp:ListItem>SmartCard No</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <fieldset style="width: 216px;">
                                                <legend>Excess Type</legend>
                                                <asp:CheckBoxList ID="cbl_excessType" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" RepeatDirection="Horizontal">
                                                    <asp:ListItem>Common</asp:ListItem>
                                                    <asp:ListItem>Ledgerwise</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <fieldset style="width: 350px;">
                                                <legend>Do you want to Check Mac Address?</legend>
                                                <asp:CheckBox ID="chkmacadd" runat="server" Text="Mac Address Check" AutoPostBack="true"
                                                    OnCheckedChanged="chkmacadd_OnCheckedChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" RepeatDirection="Horizontal" />
                                            </fieldset>
                                        </td>
                                        <td>
                                            <fieldset id="fieldrights" runat="server" style="width: 350px;" visible="false">
                                                <legend>Rights For Mac Address</legend>
                                                <asp:CheckBox ID="chkrhtsmac" runat="server" Text="Mac Address Rights" AutoPostBack="true"
                                                    OnCheckedChanged="chkrhtsmac_OnCheckedChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" RepeatDirection="Horizontal" />
                                                <asp:TextBox ID="txtmac" runat="server" Visible="false"></asp:TextBox>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <fieldset style="width: 250px; height: 223px;">
                                                    <legend>Fine Ledger Setting</legend>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblbatch" runat="server" Text="Batch"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="UP_batch" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txt_batch" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                                        <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel" Style="width: 121px; height: 200px;">
                                                                            <asp:CheckBox ID="cb_batch" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                                OnCheckedChanged="cb_batch_OnCheckedChanged" />
                                                                            <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_OnSelectedIndexChanged">
                                                                            </asp:CheckBoxList>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="pce_batch" runat="server" TargetControlID="txt_batch"
                                                                            PopupControlID="panel_batch" Position="Bottom">
                                                                        </asp:PopupControlExtender>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="UP_degree" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txt_degree" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                                        <asp:Panel ID="panel_degree" runat="server" CssClass="multxtpanel" Style="width: 150px; height: 200px;">
                                                                            <asp:CheckBox ID="cb_degree" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                                OnCheckedChanged="cb_degree_OnCheckedChanged" />
                                                                            <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_OnSelectedIndexChanged">
                                                                            </asp:CheckBoxList>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="pce_degree" runat="server" TargetControlID="txt_degree"
                                                                            PopupControlID="panel_degree" Position="Bottom">
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
                                                                <asp:UpdatePanel ID="Up_dept" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txt_dept" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                                        <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel" Style="width: 250px; height: 300px;">
                                                                            <asp:CheckBox ID="cb_dept" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                                OnCheckedChanged="cb_dept_OnCheckedChanged" />
                                                                            <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_OnSelectedIndexChanged">
                                                                            </asp:CheckBoxList>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="pce_dept" runat="server" TargetControlID="txt_dept"
                                                                            PopupControlID="panel_dept" Position="Bottom">
                                                                        </asp:PopupControlExtender>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label52" runat="server" Text="Header"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddl_hdrFine" runat="server" CssClass="textbox ddlheight1" AutoPostBack="true"
                                                                    Width="174px" OnSelectedIndexChanged="ddl_hdrFine_OnSelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label53" runat="server" Text="Ledger"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddl_lgrFine" runat="server" CssClass="textbox ddlheight1" Width="175px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl_fineLedger" runat="server" Text="Fine Ledger"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddl_fineLedger" runat="server" CssClass="textbox ddlheight1" Width="175px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </div>
                                        </td>
                                        <td>
                                            <div>
                                                <fieldset style="width: 216px; height: 80px;">
                                                    <legend>Transport Ledger Setting</legend>
                                                    <asp:DropDownList ID="ddlhdrtrans" runat="server" CssClass="textbox ddlheight1" Width="250px"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlhdrtrans_OnSelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="ddllgrtrans" runat="server" CssClass="textbox ddlheight1" Width="235px">
                                                    </asp:DropDownList>
                                                </fieldset>
                                            </div>
                                        </td>
                                        <td>
                                            <fieldset style="width: 223px; height: 80px;">
                                                <legend>Reciept/Challan No Rights</legend>
                                                <asp:CheckBox ID="chkreciept" runat="server" Text="Reciept No" Font-Size="Medium" />
                                                <asp:CheckBox ID="chkchallan" runat="server" Text="Challan No" Font-Size="Medium" />
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--added by sudhagar 28-09-2016--%>
                                        <td>
                                            <div style="position: relative;">
                                                <fieldset style="width: 250px; height: 180px;">
                                                    <legend>Re-Admission Fees Settings</legend>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label54" runat="server" Text="Batch"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txtreadmbatch" runat="server" Style="height: 20px; width: 100px;"
                                                                            ReadOnly="true">--Select--</asp:TextBox>
                                                                        <asp:Panel ID="panel13" runat="server" CssClass="multxtpanel" Style="width: 121px; height: 200px;">
                                                                            <asp:CheckBox ID="cbreadmbatch" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                                OnCheckedChanged="cbreadmbatch_OnCheckedChanged" />
                                                                            <asp:CheckBoxList ID="cblreadmbatch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblreadmbatch_OnSelectedIndexChanged">
                                                                            </asp:CheckBoxList>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txtreadmbatch"
                                                                            PopupControlID="panel13" Position="Bottom">
                                                                        </asp:PopupControlExtender>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label55" runat="server" Text="Degree"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txtreadmdegree" runat="server" Style="height: 20px; width: 100px;"
                                                                            ReadOnly="true">--Select--</asp:TextBox>
                                                                        <asp:Panel ID="panel14" runat="server" CssClass="multxtpanel" Style="width: 150px; height: 200px;">
                                                                            <asp:CheckBox ID="cbreadmdegree" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                                OnCheckedChanged="cbreadmdegree_OnCheckedChanged" />
                                                                            <asp:CheckBoxList ID="cblreadmdegree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblreadmdegree_OnSelectedIndexChanged">
                                                                            </asp:CheckBoxList>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txtreadmdegree"
                                                                            PopupControlID="panel14" Position="Bottom">
                                                                        </asp:PopupControlExtender>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label56" runat="server" Text="Department"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txtreadmdept" runat="server" Style="height: 20px; width: 100px;"
                                                                            ReadOnly="true">--Select--</asp:TextBox>
                                                                        <asp:Panel ID="panel15" runat="server" CssClass="multxtpanel" Style="width: 250px; height: 300px; position: relative;">
                                                                            <asp:CheckBox ID="cbreadmdept" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                                OnCheckedChanged="cbreadmdept_OnCheckedChanged" />
                                                                            <asp:CheckBoxList ID="cblreadmdept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblreadmdept_OnSelectedIndexChanged">
                                                                            </asp:CheckBoxList>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txtreadmdept"
                                                                            PopupControlID="panel15" Position="Bottom">
                                                                        </asp:PopupControlExtender>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label57" runat="server" Text="Header"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlreadmfinehed" runat="server" CssClass="textbox ddlheight1"
                                                                    AutoPostBack="true" Width="250px" OnSelectedIndexChanged="ddlreadmfinehed_OnSelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label58" runat="server" Text="Ledger"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlreadmfineled" runat="server" CssClass="textbox ddlheight1"
                                                                    Width="235px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </div>
                                        </td>
                                        <%--added by sudhagar 19.06.2017 journal entry setting--%>
                                        <td>
                                            <div>
                                                <fieldset style="width: 250px; height: 80px;">
                                                    <legend>Journal Fees Settings</legend>
                                                    <asp:DropDownList ID="ddlMainJrHed" runat="server" CssClass="textbox ddlheight1"
                                                        AutoPostBack="true" Width="250px" OnSelectedIndexChanged="ddlMainJrHed_OnSelected">
                                                    </asp:DropDownList>
                                                    <br />
                                                    <asp:DropDownList ID="ddlMainJrLed" runat="server" CssClass="textbox ddlheight1"
                                                        Width="235px">
                                                    </asp:DropDownList>
                                                </fieldset>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <fieldset style="width: 280px;">
                                                <legend>Payment Rights</legend>
                                                <asp:CheckBox ID="chkpety" runat="server" Text="Pety Cash" Font-Size="Medium" />
                                                <asp:CheckBox ID="chkbank" runat="server" Text="Bank" Font-Size="Medium" />
                                            </fieldset>
                                        </td>
                                        <td>
                                            <fieldset style="width: 300px;">
                                                <legend>Stream/Shift</legend>
                                                <asp:RadioButton ID="rb_stream" runat="server" Checked="true" Text="Stream" Font-Size="Medium"
                                                    GroupName="same" />
                                                <asp:RadioButton ID="rb_shift" runat="server" Text="Shift" Font-Size="Medium" GroupName="same" />
                                                <asp:CheckBox ID="cb_StreamShift" runat="server" Text="Applicable" />
                                            </fieldset>
                                        </td>
                                        <td>
                                            <fieldset style="width: 216px;" colspan="2">
                                                <legend>Scholarship</legend>
                                                <asp:CheckBoxList ID="cblScholarship" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True">Common</asp:ListItem>
                                                    <asp:ListItem>Ledgerwise</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%-- <td>
                                    <fieldset style="width: 300px;" colspan="2">
                                        <legend>Scholarship</legend>
                                        <asp:CheckBoxList ID="cblScholarship" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" RepeatDirection="Horizontal">
                                            <asp:ListItem Selected="True">Common</asp:ListItem>
                                            <asp:ListItem>Ledgerwise</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </fieldset>
                                </td>--%>
                                        <td>
                                            <fieldset style="width: 280px;">
                                                <legend>Consession And Refund Rights</legend>
                                                <asp:RadioButton ID="rbwithdept" runat="server" Checked="true" Text="Department"
                                                    GroupName="rbdept" />
                                                <asp:RadioButton ID="rbwithoutdept" runat="server" Text="Without Department" GroupName="rbdept" />
                                            </fieldset>
                                        </td>
                                        <td>
                                            <fieldset style="width: 369px; height: 95px;">
                                                <legend>Student Login Fees Rights</legend>
                                                <asp:RadioButton ID="rb_detl" runat="server" Checked="true" Text="Show Header Ledger With Allot Paid Balance Amount"
                                                    GroupName="rbstud" />
                                                <br />
                                                <asp:RadioButton ID="rb_withledger" runat="server" Text="Show Header Ledger With Paid Status"
                                                    GroupName="rbstud" />
                                                <br />
                                                <asp:RadioButton ID="rb_withoutledger" runat="server" Text="Paid Unpaid Partially Paid Status Only"
                                                    GroupName="rbstud" />
                                                <br />
                                            </fieldset>
                                        </td>
                                        <td>
                                            <fieldset style="width: 216px; height: 88px;">
                                                <legend>Show Balance</legend>
                                                <asp:RadioButtonList ID="rblShowBal" runat="server" RepeatDirection="Vertical">
                                                    <asp:ListItem Selected="True">Current Ledgers</asp:ListItem>
                                                    <asp:ListItem>Total Ledgers</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <div>
                                                <fieldset style="width: 650px; height: 100px;">
                                                    <legend>Fees Settings </legend>
                                                    <table>
                                                        <tr>
                                                            <td colspan="10">
                                                                <asp:RadioButtonList ID="rbfeesmode" runat="server" RepeatDirection="Horizontal">
                                                                    <asp:ListItem Text="Online Application Fees" Value="1" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="Registration Fees" Value="2"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                                <%--AutoPostBack="true" OnSelectedIndexChanged="rbfeesmode_Selected"--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Stream
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlstream" runat="server" Enabled="false" CssClass="textbox ddlstyle ddlheight3"
                                                                    OnSelectedIndexChanged="ddlstream_SelectedIndexChanged" Width="60px" AutoPostBack="true">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td colspan="2">Education Level
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddledlevel" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                    OnSelectedIndexChanged="ddledlevel_SelectedIndexChanged" AutoPostBack="true"
                                                                    Width="60px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>Ledger
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlledger" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                    Width="80px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>Amount
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtamt" runat="server" Style="height: 20px; width: 60px;"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnappsave" runat="server" Text="Save" OnClick="btnappsave_OnClick"
                                                                    Style="height: 30px; width: 80px;" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:Label ID="lbloutput" runat="server" Visible="false" Style="color: Red;"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <%-- Stream
                                            <asp:DropDownList ID="ddlstream" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                OnSelectedIndexChanged="ddlstream_SelectedIndexChanged" Width="60px" AutoPostBack="true">
                                            </asp:DropDownList>
                                            Education Level
                                            <asp:DropDownList ID="ddledlevel" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                OnSelectedIndexChanged="ddledlevel_SelectedIndexChanged" Width="60px" AutoPostBack="true">
                                            </asp:DropDownList>
                                            Ledger
                                            <asp:DropDownList ID="ddlledger" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                OnSelectedIndexChanged="ddlledger_SelectedIndexChanged" Width="80px" AutoPostBack="true">
                                            </asp:DropDownList>
                                            Amount
                                            <asp:TextBox ID="txtamt" runat="server" Style="height: 20px; width: 60px;"></asp:TextBox>
                                            <asp:Button ID="btnappsave" runat="server" Text="Save" OnClick="btnappsave_OnClick"
                                                Style="height: 30px; width: 80px;" />
                                            <asp:Label ID="lbloutput" runat="server" Visible="false" Style="color: Red;"></asp:Label>--%>
                                                </fieldset>
                                            </div>
                                        </td>
                                        <td>
                                            <fieldset style="width: 200px; height: 100px;">
                                                <legend>Admission Challan Bank </legend>
                                                <asp:DropDownList ID="ddlAdmissionBank" runat="server" CssClass="textbox ddlheight1"
                                                    Width="250px">
                                                </asp:DropDownList>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <fieldset style="height: 130px; width: 170px;">
                                                            <legend>Send Sms Rights</legend>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="cbsmsrights" runat="server" Style="font-family: Book Antiqua; font-weight: bold;"
                                                                            Text="SMS Rights" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <asp:CheckBoxList ID="cblsmsrights" runat="server">
                                                                        <asp:ListItem Text="Student" Value="1"></asp:ListItem>
                                                                        <asp:ListItem Text="Father" Value="2"></asp:ListItem>
                                                                        <asp:ListItem Text="Mother" Value="3"></asp:ListItem>
                                                                    </asp:CheckBoxList>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                    <td>
                                                        <fieldset style="height: 50px; width: 340px;">
                                                            <legend>Allow Duplicate Receipt for Bank Import</legend>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="cbAllowDupRcptBnkImpt" runat="server" Text="Allow" Checked="false" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                    <td>
                                                        <fieldset style="height: 50px; width: 200px;">
                                                            <legend>Students from Admission</legend>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlStudAppAdmShort" runat="server" CssClass="textbox ddlheight1"
                                                                            Width="150px">
                                                                            <asp:ListItem>Applied</asp:ListItem>
                                                                            <asp:ListItem>Shortlisted</asp:ListItem>
                                                                            <asp:ListItem>Wait To Admit</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <fieldset style="height: 110px; width: 300px;">
                                                            <legend>Day Scholar Student Mess Setting</legend>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lbl_studmessheader" runat="server" Text="Header"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddl_studmessheader" runat="server" CssClass="textbox textbox1 ddlheight5"
                                                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_studmessheader_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lbl_studmessledger" runat="server" Text="Ledger"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddl_studmessledger" runat="server" CssClass="textbox textbox1 ddlheight5">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblmessAmt" runat="server" Text="Amount"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txt_messAmount" runat="server" placeholder="0.00" onblur="checkFloatValue(this);"
                                                                            CssClass="textbox txtheight" Style="text-align: right; width: 80px; height: 15px;"
                                                                            BackColor="#81F7D8" MaxLength="15">
                                                                        </asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="fteMeAmt" runat="server" FilterType="Numbers,Custom"
                                                                            ValidChars="." TargetControlID="txt_messAmount">
                                                                        </asp:FilteredTextBoxExtender>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                    <td>
                                                        <fieldset style="height: 110px; width: 250px;">
                                                            <legend>Department Print Settings</legend>
                                                            <asp:CheckBox ID="cbdeptacr" runat="server" Text="Include Department Acronym" />
                                                        </fieldset>
                                                    </td>
                                                    <td>
                                                        <fieldset style="height: 110px; width: 250px;">
                                                            <legend>User Based Report Settings</legend>
                                                            <asp:CheckBox ID="cbuserrpt" runat="server" Text="Include User Based Report" />
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <fieldset style="height: auto; width: 250px;">
                                                            <legend>Fine Type</legend>
                                                            <asp:CheckBox ID="chkFineType" runat="server" Text="Use Ledgerwise Fine Amount" />
                                                        </fieldset>
                                                        <fieldset style="height: auto; width: 250px;">
                                                            <legend>Show Scheme Settings</legend>
                                                            <asp:CheckBox ID="chkShowSchemeSettings" runat="server" Text="Show" />
                                                        </fieldset>
                                                        <fieldset style="height: auto; width: 250px;">
                                                            <legend>Students Display Position in Journal</legend>
                                                            <asp:CheckBox ID="chkStudDispPosJour" runat="server" Text="Base screen" />
                                                        </fieldset>
                                                    </td>
                                                    <td>
                                                        <fieldset style="height: auto; width: 250px;">
                                                            <legend>Include Finance For Leave Request</legend>
                                                            <asp:CheckBox ID="chkIncFinLeaveRequest" runat="server" Text="Include" />
                                                        </fieldset>
                                                        <fieldset style="">
                                                            <legend>Include Discontinued and Completed Students in Journal</legend>
                                                            <asp:CheckBox ID="chkJourDisc" runat="Server" Text="Discontinued" />
                                                            <asp:CheckBox ID="chkJourCc" runat="Server" Text="Completed" />
                                                        </fieldset>
                                                    </td>
                                                    <td>
                                                        <fieldset style="height: auto; width: 250px;">
                                                            <legend>Paymode</legend>
                                                            <asp:CheckBox ID="cbinclpay" runat="server" Text="Include Paymode" />
                                                            <fieldset style="height: auto; width: 150px;">
                                                                <asp:CheckBoxList ID="cblpaymode" runat="server">
                                                                </asp:CheckBoxList>
                                                            </fieldset>
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="10">
                                                        <fieldset style="height: 250px;">
                                                            <legend>TC Header & Ledger Settings</legend>
                                                            <table>
                                                                <tr>
                                                                    <td>Institute
                                                                    </td>
                                                                    <td>Header
                                                                    </td>
                                                                    <td>Ledger
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlTcCollege" runat="server" Width="320px" CssClass="textbox ddlheight"
                                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlTcCollege_OnIndexChange">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <div style="position: relative;">
                                                                            <asp:TextBox ID="txtTcHeader" runat="server" CssClass="textbox txtheight2" Style="width: 204px;"
                                                                                ReadOnly="true">--Select--</asp:TextBox>
                                                                            <asp:Panel ID="pnlTcHeader" runat="server" CssClass="multxtpanel " Style="width: 211px; border-color: HighlightText; height: 150px;">
                                                                                <asp:CheckBox ID="cbTcHeader" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                                    OnCheckedChanged="cbTcHeader_OnCheckedChanged" />
                                                                                <asp:CheckBoxList ID="cblTcHeader" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblTcHeader_OnSelectedIndexChanged">
                                                                                </asp:CheckBoxList>
                                                                            </asp:Panel>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <div style="position: relative;">
                                                                            <asp:TextBox ID="txtTcLedger" runat="server" CssClass="textbox txtheight2" Style="width: 204px;"
                                                                                ReadOnly="true">--Select--</asp:TextBox>
                                                                            <asp:Panel ID="pnlTcLedger" runat="server" CssClass="multxtpanel " Style="width: 211px; border-color: HighlightText; height: 150px;">
                                                                                <asp:CheckBox ID="cbTcLedger" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                                    OnCheckedChanged="cbTcLedger_OnCheckedChanged" />
                                                                                <asp:CheckBoxList ID="cblTcLedger" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblTcLedger_OnSelectedIndexChanged">
                                                                                </asp:CheckBoxList>
                                                                            </asp:Panel>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnTcSave" runat="server" CssClass=" textbox textbox1" Text="Save"
                                                                            ForeColor="White" BackColor="Brown" OnClick="btnTcSave_OnClick" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <fieldset style="width: 300px">
                                                            <legend>Receipt Tab Rights</legend>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="cbRcptStud" runat="server" Text="Student" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="cbRcptStaff" runat="server" Text="Staff" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="cbRcptVendor" runat="server" Text="Vendor" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="cbRcptOther" runat="server" Text="Other" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                    <td>
                                                        <fieldset style="width: 300px">
                                                            <legend>Payment Tab Rights</legend>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="cbPayStud" runat="server" Text="Student" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="cbPayStaff" runat="server" Text="Staff" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="cbPayVendor" runat="server" Text="Vendor" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="cbPayOther" runat="server" Text="Other" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                    <%--saranya--%>
                                                    <td>
                                                        <fieldset style="width: 250px">
                                                            <legend>Receipt Cancel SMS/EMAIL Rights</legend>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="cbSms" runat="server" Text="Sms" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="cbMail" runat="server" Text="Email" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <fieldset style="width: 320px">
                                                            <legend>ReceiptNo Rights For Online Payment</legend>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="ChkRecNoGen" runat="server" Text="General Receipt No" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="ChkRecNoOnl" runat="server" Text="Online Receipt No" />
                                                                    </td>

                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <fieldset style="width: 120px">
                                                            <legend>Denomination Rights for ReceiptChallan</legend>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="CheckBox1" runat="server" Text="General Receipt No" />
                                                                    </td>
                                                                    
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <table>
                                    <tr>
                                        <td colspan="2">
                                            <div runat="server" id="mindiv" visible="false">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <fieldset style="height: 175px; padding-top: 0px; top: 242px; position: absolute;">
                                                                <legend>Receipt Settings</legend>
                                                                <asp:CheckBox ID="Chk_header" runat="server" Text="HeaderWise Receipt No" /><br />
                                                                <asp:CheckBox ID="Chk_priority" runat="server" Text="Priority" /><br />
                                                                <asp:CheckBox ID="Chk_cursem" runat="server" Text="Current Sem/Year" /><br />
                                                                <asp:CheckBox ID="Chk_prevmonth" runat="server" Text="Get Monthly Fee For Previous Month" /><br />
                                                                <br />
                                                            </fieldset>
                                                        </td>
                                                        <td>
                                                            <fieldset style="height: 175px; padding-top: 0px; top: 242px; position: absolute; left: 360px;">
                                                                <legend>Body Settings</legend>
                                                                <asp:CheckBox ID="chk_allot" runat="server" Text="Alloted Amount" /><br />
                                                                <asp:CheckBox ID="chk_fine" runat="server" Text="Fine" /><br />
                                                                <asp:CheckBox ID="chk_bal" runat="server" Text="Balance" /><br />
                                                                <asp:CheckBox ID="chk_bsem" runat="server" Text="Semester / Year" /><br />
                                                                <asp:CheckBox ID="chk_prev_paid" runat="server" Text="Previous Paid Amount" /><br />
                                                                <asp:CheckBox ID="chk_shw_mnt_name" runat="server" Text="Show Month Name in Receipt (For Monthly Fee)" />
                                                            </fieldset>
                                                        </td>
                                                        <td>
                                                            <fieldset style="height: 175px; top: 242px; position: absolute; left: 680px; padding-top: 0px;">
                                                                <legend>Footer Settings</legend>
                                                                <asp:CheckBox ID="chk_singlepage" runat="server" Text="Print in Single Page" /><br />
                                                                <asp:CheckBox ID="chk_studcopy" runat="server" Text="Student Copy" />
                                                                <asp:CheckBox ID="chk_officecopy" runat="server" Text="Office Copy" /><br />
                                                                <asp:CheckBox ID="chk_clgname" runat="server" Text="For College Name" /><br />
                                                                <asp:CheckBox ID="chk_Narration" runat="server" Text="Narration" /><br />
                                                                <asp:CheckBox ID="chk_authorised_sign" runat="server" Text="Authorised Signatory:" />
                                                                <asp:TextBox ID="txtauthor_name" runat="server" Width="191px"></asp:TextBox><br />
                                                                <asp:CheckBox ID="chk_cash" runat="server" Text="Cashier Signatory:" /><br />
                                                                <asp:TextBox ID="txt_cash" runat="server" Width="191px"></asp:TextBox>
                                                                <br />
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td rowspan="2">
                                                            <fieldset style="height: 288px; top: 427px; position: absolute;">
                                                                <legend>Header Settings </legend>
                                                                <asp:CheckBox ID="chk_clg" runat="server" Text="College Name" /><br />
                                                                <asp:CheckBox ID="chk_city" runat="server" Text="City/Town/Village" /><br />
                                                                <asp:CheckBox ID="chk_time" runat="server" Text="Time" />
                                                                <br />
                                                                <asp:CheckBox ID="chk_degree" runat="server" Text="Degree Acronym" />
                                                                <br />
                                                                <asp:CheckBox ID="chk_year" runat="server" Text="Year" /><br />
                                                                <asp:CheckBox ID="chk_hsem" runat="server" Text="Semester" /><br />
                                                                <asp:CheckBox ID="chk_regno" runat="server" Text="Register No" /><br />
                                                                <asp:CheckBox ID="chk_fathername" runat="server" Text="Father Name" /><br />
                                                                <asp:CheckBox ID="chk_setrollno" runat="server" Text="Set Roll No As Admission No" /><br />
                                                                <asp:CheckBox ID="chk_counterno" runat="server" Text="Counter No" /><br />
                                                                <asp:CheckBox ID="chk_countername" runat="server" Text="Counter Name" /><br />
                                                                <asp:Label ID="Label4" runat="server" Text="Receipt Valid Upto"></asp:Label>
                                                                &nbsp;&nbsp;
                                                                <asp:TextBox ID="txtrcpt_valid" runat="server"></asp:TextBox>
                                                            </fieldset>
                                                        </td>
                                                        <td colspan="2">
                                                            <fieldset style="height: 80px; width: 400px; top: 440px; position: absolute; left: 360px;">
                                                                <asp:CheckBox ID="chk_fine_allot" runat="server" Text="Automatically Allot Fine Amount For Duplicate Receipt"
                                                                    OnCheckedChanged="chk_fine_allot_CheckedChanged" AutoPostBack="True" />
                                                                <br />
                                                                <asp:TextBox ID="txt_duplicate_Fine" Visible="False" runat="server" Style="width: 70px"></asp:TextBox>
                                                                <br />
                                                                <asp:CheckBox ID="chk_onlydue" runat="server" Text="Display Only The Due Amount In Receipt" />
                                                                <br />
                                                                <asp:CheckBox ID="chk_preprint" runat="server" Text="Preprint" />
                                                                <td>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <fieldset style="height: 150px; width: 585px; top: 565px; position: absolute; left: 360px;">
                                                                <legend>Advance Adjustment Settings</legend>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label5" runat="server" Text="Select Day"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txt_advdate" runat="server" Style="width: 70px"></asp:TextBox>
                                                                            <asp:RangeValidator ID="RangeValidator1" ControlToValidate="txt_advdate" MinimumValue="1"
                                                                                MaximumValue="31" runat="server" ErrorMessage="Value Must b/w 1 to 31"></asp:RangeValidator>
                                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txt_advdate"
                                                                                FilterType="Numbers" runat="server">
                                                                            </asp:FilteredTextBoxExtender>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label6" runat="server" Text="Select Monthwise Fee Type"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <%--<asp:DropDownList ID="ddlfeetype" runat="server">
                                            </asp:DropDownList>--%>
                                                                            <asp:TextBox ID="txtmonfee" runat="server" Height="19px" ReadOnly="true" Font-Bold="True"
                                                                                ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Width="100px">- - Select - -</asp:TextBox>
                                                                            <%--OnTextChanged="txtmonfee_TextChanged" AutoPostBack="true"--%>
                                                                            <asp:DropDownExtender ID="DropDownExtender3" runat="server" DropDownControlID="pmonfee"
                                                                                DynamicServicePath="" Enabled="true" TargetControlID="txtmonfee">
                                                                            </asp:DropDownExtender>
                                                                            <asp:Panel ID="pmonfee" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                                                BorderWidth="2px" Height="220px" ScrollBars="Vertical" Width="185px">
                                                                                <asp:CheckBox ID="chkbat_selectall" runat="server" Text="SelectAll" AutoPostBack="true"
                                                                                    Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                                    OnCheckedChanged="chkbat_selectall_CheckedChanged" />
                                                                                <asp:CheckBoxList ID="ddlfeetype" runat="server" Font-Size="Small" AutoPostBack="True"
                                                                                    Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Width="184px" OnSelectedIndexChanged="ddlfeetype_SelectedIndexChanged"
                                                                                    Height="50px">
                                                                                </asp:CheckBoxList>
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label8" runat="server" Text="Select Fee Type"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <%-- <asp:DropDownList ID="ddlfeetype1" runat="server">
                                            </asp:DropDownList>--%>
                                                            <asp:TextBox ID="txtgenfee" runat="server" Height="19px" ReadOnly="true" Font-Bold="True"
                                                                ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Width="100px">- - Select - -</asp:TextBox>
                                                            <%--OnTextChanged="txtgenfee_TextChanged" AutoPostBack="true"--%>
                                                            <asp:DropDownExtender ID="DropDownExtender1" runat="server" DropDownControlID="pgenfee"
                                                                DynamicServicePath="" Enabled="true" TargetControlID="txtgenfee">
                                                            </asp:DropDownExtender>
                                                            <asp:Panel ID="pgenfee" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                                BorderWidth="2px" Height="220px" ScrollBars="Vertical" Width="185px">
                                                                <asp:CheckBox ID="chkbat_selectall1" runat="server" Text="SelectAll" AutoPostBack="true"
                                                                    Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                    OnCheckedChanged="chkbat_selectall1_CheckedChanged" />
                                                                <asp:CheckBoxList ID="ddlfeetype1" runat="server" Font-Size="Small" AutoPostBack="True"
                                                                    Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Width="184px" OnSelectedIndexChanged="ddlfeetype1_SelectedIndexChanged"
                                                                    Height="50px">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr style="display: none;">
                                        <td rowspan="2">
                                            <fieldset style="height: 100px;">
                                                <legend>Payment </legend>
                                                <asp:RadioButtonList ID="rbpay" runat="server" RepeatDirection="Vertical" Font-Bold="True"
                                                    ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium">
                                                    <asp:ListItem Text="Demand and Approval" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Direct" Value="1"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr style="display: none;">
                                        <td></td>
                                        <td rowspan="3">
                                            <%--<asp:Button ID="btnfin_save" runat="server" CssClass="style14" 
                                OnClick="btnfin_save_Click" Text="Save" />--%>
                                            <asp:LinkButton ID="btnfin_save" runat="server" Font-Bold="True" Width="50px" Text="Save"
                                                Font-Names="Book Antiqua" Font-Size="Small" ForeColor="Black" OnClick="btnfin_save_Click"
                                                CausesValidation="False" Style="display: inline-block; color: Black; font-family: Book Antiqua; font-size: small; font-weight: bold; width: 50px; text-align: center; border-style: solid; border-width: 1px; border-color: gray; background-color: none; border-radius: 4px 4px 4px 4px; text-decoration: none;"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
            <%--COE Settings Tab--%>
            <center>
                <div id="div5COE" runat="server" style="width: 980px; height: 400px; background: white; border: 1px solid #0095E8; overflow: auto;"
                    visible="false">
                    <table>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: 80px; width: 170px;">
                                    <legend>Report Display</legend>
                                    <asp:RadioButton ID="btn_originalmark" runat="server" GroupName="mark" Text="Original Mark"
                                        Font-Size="Medium" Font-Names="Book Antique" AutoPostBack="True" OnCheckedChanged="btn_originalmark_CheckedChanged" />
                                    <br />
                                    <asp:RadioButton ID="btn_convertmark" Text="Coverted Mark" Font-Size="Medium" Font-Names="Book Antique"
                                        GroupName="mark" runat="server" AutoPostBack="True" OnCheckedChanged="btn_convertmark_CheckedChanged" />
                                    <br />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblmark" runat="server" Text="Mark Value" Font-Size="Medium" Font-Names="Book Antique"></asp:Label>
                                    <asp:TextBox ID="txtmark" runat="server" Font-Size="Medium" Font-Names="Book Antique"
                                        Width="46px"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="datefilter" FilterType="Numbers" runat="server"
                                        TargetControlID="txtmark" Enabled="True">
                                    </asp:FilteredTextBoxExtender>
                                    <br />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="errlbl_cor" runat="server" Text="Please Enter Mark" Font-Size="Medium"
                                        Font-Names="Book Antique" ForeColor="Red"></asp:Label>
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 90px; width: 260px;">
                                    <legend>Mark Convertion Thoery</legend>
                                    <asp:Label ID="lbltheoryinternal" runat="server" Text="Internal"></asp:Label>
                                    <asp:TextBox ID="txttheoryinternal" runat="server" Width="60px" MaxLength="2"></asp:TextBox><br />
                                    <br />
                                    <asp:Label ID="lbltheorypractical" runat="server" Text="External"></asp:Label>
                                    <asp:TextBox ID="txttheorypractical" runat="server" Width="60px" MaxLength="2"></asp:TextBox><br />
                                    <br />
                                    <asp:Label ID="lbltheory" runat="server" CssClass="font" Visible="False" ForeColor="Red"></asp:Label>
                                    <asp:FilteredTextBoxExtender ID="filtertheoryint" runat="server" TargetControlID="txttheoryinternal"
                                        FilterType="Numbers" Enabled="True">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:FilteredTextBoxExtender ID="filtertheoryext" runat="server" TargetControlID="txttheorypractical"
                                        FilterType="Numbers" Enabled="True">
                                    </asp:FilteredTextBoxExtender>
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 90px; width: 260px;">
                                    <legend>Mark Convertion Practical</legend>
                                    <asp:Label ID="lblpracticalinternal" runat="server" Text="Internal"></asp:Label>
                                    <asp:TextBox ID="txtpracticalinternal" runat="server" Width="60px" MaxLength="2"></asp:TextBox><br />
                                    <br />
                                    <asp:Label ID="lblpracticalexternal" runat="server" Text="External"></asp:Label>
                                    <asp:TextBox ID="txtpracticalexternal" runat="server" Width="60px" MaxLength="2"></asp:TextBox><br />
                                    <br />
                                    <asp:Label ID="lblpractical" runat="server" CssClass="font" Visible="False" ForeColor="Red"></asp:Label>
                                    <asp:FilteredTextBoxExtender ID="filterpracticalint" runat="server" TargetControlID="txtpracticalinternal"
                                        FilterType="Numbers" Enabled="True">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:FilteredTextBoxExtender ID="filterpracticalext" runat="server" TargetControlID="txtpracticalexternal"
                                        FilterType="Numbers" Enabled="True">
                                    </asp:FilteredTextBoxExtender>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: 140px; width: 167px;">
                                    <legend>Grade Display</legend>
                                    <asp:Label ID="Label7" runat="server" Text="Fail Grade : " Style="height: 15px; width: 62px"
                                        Font-Names="Book Antiqua" Font-Size="Smaller"></asp:Label>
                                    <asp:TextBox ID="txt_grade" Width="30px" runat="server" Style="height: 22px" Font-Names="Book Antiqua"
                                        Font-Size="Smaller"></asp:TextBox><br />
                                    <asp:Label ID="Label9" runat="server" Style="height: 15px; width: 68px" Text="Attempts&gt;="
                                        Width="76px" Font-Size="Smaller"></asp:Label>
                                    <asp:TextBox ID="txtattempts" runat="server" MaxLength="2" Style="height: 22px" Width="30px"
                                        Font-Size="Smaller"></asp:TextBox><br />
                                    <asp:Label ID="Label10" runat="server" Text="Max. External Marks" Style="height: 22px"
                                        Font-Names="Book Antiqua" Font-Size="Smaller"></asp:Label>
                                    <asp:TextBox ID="txtmaxmarkv" runat="server" Width="30px" MaxLength="2" Style="height: 22px"
                                        Font-Names="Book Antiqua" Font-Size="Smaller"></asp:TextBox>
                                    <br />
                                    <br />
                                    <asp:Label ID="Label11" runat="server" CssClass="font" Visible="False" ForeColor="Red"
                                        Style="height: 22px"></asp:Label>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtattempts"
                                        FilterType="Numbers" Enabled="True">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtmaxmarkv"
                                        FilterType="Numbers" Enabled="True">
                                    </asp:FilteredTextBoxExtender>
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 170px; width: 340px; font-family: Book Antiqua;">
                                    <legend>Classification</legend>
                                    <asp:Button ID="Button1" runat="server" Text="Add Row" OnClick="Button1_Click1" />
                                    <asp:Label ID="Label15" runat="server" Text="Education Level: " Font-Names="Book Antiqua"
                                        Font-Size="Smaller"></asp:Label><asp:DropDownList ID="drp_edulevel" runat="server"
                                            OnSelectedIndexChanged="drp_edulevel_SelectedIndexChanged" Font-Names="Book Antiqua"
                                            Font-Size="Smaller" AutoPostBack="True">
                                        </asp:DropDownList>
                                    <br />
                                    <asp:Label ID="lblBatchYear" runat="server" Text="Batch Year: " Font-Names="Book Antiqua"
                                        Font-Size="Smaller"></asp:Label><asp:DropDownList ID="ddlBatchYear" runat="server"
                                            OnSelectedIndexChanged="ddlBatchYear_SelectedIndexChanged" Font-Names="Book Antiqua"
                                            Font-Size="Smaller" AutoPostBack="True">
                                        </asp:DropDownList>
                                    <asp:Label ID="lblerrvel" runat="server" Text="Please Enter From & To Range" Style="margin: 10 220px 0 0;"
                                        ForeColor="Red"></asp:Label>
                                    <farpoint:fpspread id="FpSpread1" runat="server" bordercolor="Black" horizontalscrollbarpolicy="Never"
                                        verticalscrollbarpolicy="AsNeeded" borderstyle="Solid" borderwidth="1px" height="100"
                                        width="330" scrollbar3dlightcolor="Yellow" scrollbararrowcolor="Brown" scrollbarbasecolor="Goldenrod"
                                        scrollbardarkshadowcolor="DarkOrange" scrollbarfacecolor="RosyBrown" scrollbarhighlightcolor="#006699"
                                        scrollbarshadowcolor="#9999FF" currentpageindex="0" designstring="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                                        editmodepermanent="True" scrollcontent="true" scrollcontentcolumns="" scrollcontentmaxheight="50"
                                        scrollcontenttime="500">
                                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonType="PushButton"
                                            ButtonHighlightColor="ControlLightLight" ButtonShadowColor="ControlDark" Visible="true">
                                            <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                                        </CommandBar>
                                        <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" />
                                        <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" />
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                        <TitleInfo BackColor="#E7EFF7" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                            Font-Size="X-Large" Font-Strikeout="False" Font-Underline="False" ForeColor=""
                                            HorizontalAlign="Center" VerticalAlign="NotSet" Text="">
                                        </TitleInfo>
                                    </farpoint:fpspread>
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 111px; width: 260px; padding-top: 10px;">
                                    <legend></legend>
                                    <asp:Label ID="Label12" runat="server" Text="Education Level: " Style="height: 15px; width: 92px"
                                        Font-Names="Book Antiqua" Font-Size="Smaller"></asp:Label>
                                    <asp:DropDownList ID="drp_edu" runat="server" OnSelectedIndexChanged="drp_edu_SelectedIndexChanged"
                                        Style="height: 22px; width: 77px" Font-Names="Book Antiqua" Font-Size="Smaller"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                    <br />
                                    <asp:Label ID="Label13" runat="server" Text="CGPA" Style="height: 15px; width: 33px"
                                        Font-Names="Book Antiqua" Font-Size="Smaller"></asp:Label>
                                    <asp:TextBox ID="txt_cgpa" runat="server" Style="height: 20px; width: 30px" Width="72px"
                                        Font-Names="Book Antiqua" Font-Size="Smaller"></asp:TextBox><br />
                                    <asp:Label ID="Label14" runat="server" Text="Classification" Font-Names="Book Antiqua"
                                        Font-Size="Smaller" Style="height: 15px; width: 72px"></asp:Label>
                                    <asp:TextBox ID="txt_clasfi" runat="server" Style="height: 18px" Width="100px" Font-Names="Book Antiqua"
                                        Font-Size="Smaller"></asp:TextBox>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <fieldset style="height: auto; width: 500px;">
                                    <legend>Exam Month & Year</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCollegeAcr" runat="server" Text="College" Style="height: auto; width: auto"
                                                    Font-Names="Book Antiqua" Font-Size="Smaller"></asp:Label></td>
                                            <td>
                                                <asp:DropDownList ID="ddlCollegeAcr" runat="server" Style="height: auto; width: 70px"
                                                    Font-Names="Book Antiqua" Font-Size="Smaller" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlCollegeAcr_IndexChange">
                                                </asp:DropDownList></td>

                                            <td>
                                                <asp:Label ID="lblCourseName" runat="server" Text="Course" Style="height: auto; width: auto"
                                                    Font-Names="Book Antiqua" Font-Size="Smaller" AssociatedControlID="txtDegree"></asp:Label>
                                            </td>
                                            <td>
                                                <div style="position: relative;">
                                                    <asp:UpdatePanel ID="upnlDegree" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtDegree" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                                                ReadOnly="true">-- Select --</asp:TextBox>
                                                            <asp:Panel ID="pnlDegree" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                                                Width="140px">
                                                                <asp:CheckBox ID="chkDegree" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                                    AutoPostBack="True" OnCheckedChanged="chkDegree_CheckedChanged" />
                                                                <asp:CheckBoxList ID="cblDegree" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="cblDegree_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="popExtDegree" runat="server" TargetControlID="txtDegree"
                                                                PopupControlID="pnlDegree" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </td>
                                            <td colspan="2">
                                                <asp:RadioButton ID="rbdApplication" Text="Application" Font-Size="Medium" Font-Names="Book Antique"
                                                    GroupName="mark" runat="server" AutoPostBack="True" />
                                            </td>
                                            <td colspan="2">
                                                <asp:RadioButton ID="rbdMarkEntry" Text="Mark" Font-Size="Medium" Font-Names="Book Antique"
                                                    GroupName="mark" runat="server" AutoPostBack="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label29" runat="server" Text="Year: " Style="height: auto; width: auto"
                                                    Font-Names="Book Antiqua" Font-Size="Smaller"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlemyear" runat="server" Style="height: auto; width: 63px"
                                                    Font-Names="Book Antiqua" Font-Size="Smaller">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label30" runat="server" Text="Month" Style="height: auto; width: auto"
                                                    Font-Names="Book Antiqua" Font-Size="Smaller"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlemmonth" runat="server" Style="height: auto; width: 63px"
                                                    Font-Names="Book Antiqua" Font-Size="Smaller">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <fieldset style="margin: 0px; padding: 0px;">
                                                    <legend>Exam Month & Year For Result</legend>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblExamYearResult" runat="server" Text="Result Exam Year: " Style="height: auto; width: auto"
                                                                    Font-Names="Book Antiqua" Font-Size="Smaller"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlExamYearResult" runat="server" Style="height: auto; width: 63px;"
                                                                    Font-Names="Book Antiqua" Font-Size="Smaller">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblExamMonthResult" runat="server" Text="Month" Style="height: auto; width: auto;"
                                                                    Font-Names="Book Antiqua" Font-Size="Smaller"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlExamMonthResult" runat="server" Style="height: auto; width: 63px;"
                                                                    Font-Names="Book Antiqua" Font-Size="Smaller">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="center">
                                                <asp:Button ID="btnexamsave" runat="server" Text="Save" Font-Names="Book Antiqua"
                                                    OnClick="btnexamsave_Click" Font-Size="Smaller" Style="height: 22px; width: 63px" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <td colspan="2">
                            <fieldset style="height: 200px; width: 300px;">
                                <legend>Evaluation</legend>
                                <asp:Label ID="Label23" runat="server" Text="No.of Evaluation ">
                                    <asp:TextBox ID="txt_ev" runat="server" MaxLength="2" Width="20"></asp:TextBox>
                                    <asp:RangeValidator ID="RangeValidator3" ControlToValidate="txt_ev" MinimumValue="1"
                                        MaximumValue="31" runat="server" ErrorMessage="Value Must b/w 1 to 3"></asp:RangeValidator>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" TargetControlID="txt_ev"
                                        FilterType="Numbers" runat="server">
                                    </asp:FilteredTextBoxExtender>
                                    <br />
                                </asp:Label><asp:Label ID="Label24" runat="server" Text="Mark Difference "></asp:Label>
                                &nbsp;
                                    <asp:TextBox ID="txt_difmark" runat="server" MaxLength="2" Width="20"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txt_difmark"
                                    FilterType="Numbers" Enabled="True">
                                </asp:FilteredTextBoxExtender>
                                <br />
                                <asp:Label ID="Label27" runat="server" Text="Moderation Mark "></asp:Label>
                                &nbsp;
                                    <asp:TextBox ID="txtmoder" runat="server" MaxLength="2" Width="20"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtmoder"
                                    FilterType="Numbers" Enabled="True">
                                </asp:FilteredTextBoxExtender>
                                <br />
                                <asp:CheckBox ID="chkModerApplicable" runat="server" Text="Applicable" />
                                <br />
                                <asp:Label ID="Label51" runat="server" Text="Edu Level"></asp:Label>
                                <asp:DropDownList ID="ddlModerApplicableEduLevel" runat="server" Width="60px" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlModerApplicableEduLevel_IndexChange">
                                </asp:DropDownList>
                                <asp:Label ID="Label48" runat="server" Text="Batch Year "></asp:Label>
                                <asp:DropDownList ID="ddlModerApplicableBatch" runat="server" Width="60px" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlModerApplicableBatch_IndexChange">
                                </asp:DropDownList>
                                <%--<asp:TextBox ID="txtModerApplicableBatch" runat="server"  MaxLength="4" Width="40"></asp:TextBox>
                                      <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="txtModerApplicableBatch"
                                        FilterType="Numbers" Enabled="True"></asp:FilteredTextBoxExtender>--%>
                                <asp:Label ID="Label49" runat="server" Text="Semester"></asp:Label>
                                <asp:DropDownList ID="ddlModerApplicableSem" runat="server" Width="30px">
                                </asp:DropDownList>
                                <%-- <asp:TextBox ID="txtModerApplicableSem" runat="server"  MaxLength="1" Width="20"></asp:TextBox>
                                      <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" TargetControlID="txtModerApplicableSem"
                                        FilterType="Numbers" Enabled="True"></asp:FilteredTextBoxExtender>--%>
                                <br />
                                <asp:Label ID="Label50" runat="server" Text="Maximum Moderation Mark"></asp:Label>
                                <asp:TextBox ID="txtModerMaxMark" runat="server" MaxLength="2" Width="20"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" TargetControlID="txtModerMaxMark"
                                    FilterType="Numbers" Enabled="True">
                                </asp:FilteredTextBoxExtender>
                            </fieldset>
                        </td>
                        <td colspan="2">
                            <fieldset style="height: 60px; width: 220px;">
                                <legend></legend>
                                <asp:Label ID="Label28" runat="server" Text="Max External Mark" Style="height: 15px; width: 143px"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                <asp:DropDownList ID="DropDownList1" runat="server" OnSelectedIndexChanged="drpmaxexternalmark"
                                    Style="height: 22px; width: 77px" Font-Names="Book Antiqua" Font-Size="Medium"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                                <asp:TextBox ID="TextBox1" runat="server" Style="height: 20px; width: 30px" Width="72px"
                                    MaxLength="3" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="TextBox1"
                                    FilterType="Numbers" Enabled="True">
                                </asp:FilteredTextBoxExtender>
                            </fieldset>
                        </td>
                        </tr>
                        <tr>
                            <td colspan="4">

                                <fieldset style="height: 75px; width: 500px;">
                                    <legend>Exam Fee Master</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlexamapptype" runat="server" Style="height: 22px; width: 147px"
                                                    Font-Names="Book Antiqua" Font-Size="Smaller" AutoPostBack="true" OnSelectedIndexChanged="ddlexamapptype_SelectedIndexChanged">
                                                    <asp:ListItem Text="Application Form"></asp:ListItem>
                                                    <asp:ListItem Text="Semester Mark Sheet"></asp:ListItem>
                                                    <asp:ListItem Text="Theory"></asp:ListItem>
                                                    <asp:ListItem Text="Practical"></asp:ListItem>
                                                    <asp:ListItem Text="Project"></asp:ListItem>
                                                    <asp:ListItem Text="Field Work"></asp:ListItem>
                                                    <asp:ListItem Text="Viva Voice"></asp:ListItem>
                                                    <asp:ListItem Text="Disseration"></asp:ListItem>
                                                    <asp:ListItem Text="Consolidate Mark Sheet"></asp:ListItem>
                                                    <asp:ListItem Text="Course Completaion"></asp:ListItem>
                                                    <asp:ListItem Text="Online Application Fee"></asp:ListItem>
                                                    <asp:ListItem Text="Arrear Theory"></asp:ListItem>
                                                    <asp:ListItem Text="Arrear Practical"></asp:ListItem>
                                                    <asp:ListItem Text="Central Valuation"></asp:ListItem>
                                                    <asp:ListItem Text="Syllabi & Curricular"></asp:ListItem>
                                                    <asp:ListItem Text="Record Maintainance"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtappfee" runat="server" Style="height: 15px; width: 50px" Font-Names="Book Antiqua"
                                                    Font-Size="Smaller">
                                                </asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtappfee"
                                                    FilterType="Numbers">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:Label ID="Label31" runat="server" Text="Header " Style="height: 15px; width: 92px"
                                                    Font-Names="Book Antiqua" Font-Size="Smaller"></asp:Label>
                                                <asp:DropDownList ID="ddleappheader" runat="server" Style="height: 22px; width: 100px"
                                                    Font-Names="Book Antiqua" Font-Size="Smaller" AutoPostBack="true" OnSelectedIndexChanged="ddleappheader_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label32" runat="server" Text="Ledger" Style="height: 15px; width: 33px"
                                                    Font-Names="Book Antiqua" Font-Size="Smaller"></asp:Label>
                                                <asp:DropDownList ID="ddleappledger" runat="server" Style="height: 22px; width: 100px"
                                                    Font-Names="Book Antiqua" Font-Size="Smaller">
                                                </asp:DropDownList>
                                                <asp:Button ID="btnexamfeessave" Text="Save" runat="server" OnClick="btnexamfeessave_Save" />
                                                <asp:Label ID="lblexamhearerr" runat="server" ForeColor="Red"></asp:Label>
                                                <asp:CheckBox ID="chksubtypeFee" runat="server" Text="Fee obtained from Subject Type"
                                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Small" />

                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 75px; width: 250px;">
                                    <legend>III Evaluation</legend>
                                    <br />
                                    <asp:CheckBox ID="cbthirdevaluation" runat="server" Text="III Evaluation" />
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: 75px; width: 300px;">
                                    <legend>Bundle Number Generation</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <span>Starting Number</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_bundnumber" runat="server" Width="100px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1452" runat="server" FilterType="Numbers"
                                                    TargetControlID="txt_bundnumber" Enabled="True">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Bundle Per Student</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_prestudentbundle" runat="server" Width="100px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" FilterType="Numbers"
                                                    TargetControlID="txt_prestudentbundle" Enabled="True">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 22px; width: 225px;">
                                    <asp:CheckBox ID="chkattmark" runat="server" Text="Attendance Link With Mark Entry"
                                        Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Small" />
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 22px; width: 225px;">
                                    <asp:CheckBox ID="chkresultcurr" runat="server" Text="Result Publish Current Paper's Only"
                                        Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Small" />
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: 44px; width: 285px;">
                                    <legend>Mark Entry Round Off</legend>
                                    <asp:Label ID="Label42" runat="server" Text="Mark Entry Round Off" Font-Bold="true"
                                        Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
                                    <asp:TextBox Font-Bold="true" ID="txtmarkentryround" runat="server" Font-Names="Book Antiqua"
                                        Width="50px" MaxLength="3" Font-Size="Small"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" FilterType="Numbers"
                                        TargetControlID="txtmarkentryround" Enabled="True">
                                    </asp:FilteredTextBoxExtender>
                                </fieldset>
                            </td>
                            <td colspan="4">
                                <fieldset style="height: 54px; width: 512px;">
                                    <legend>Folio No</legend>
                                    <asp:DropDownList runat="server" ID="dropFolio">
                                        <asp:ListItem Text="Consolidate Sheet">
                                        </asp:ListItem>
                                        <asp:ListItem Text="Grade Sheet"></asp:ListItem>
                                        <asp:ListItem Text="Intramural Sheet"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="Ibl_prefix" runat="server" Text="Prefix" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Small">
                                    </asp:Label>
                                    <asp:TextBox Font-Bold="true" ID="txt_prefix" runat="server" Font-Names="Book Antiqua"
                                        Width="80px" Font-Size="Small"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13333" runat="server" FilterType="UppercaseLetters"
                                        TargetControlID="txt_prefix" Enabled="True">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Label ID="Ibl_suffix" runat="server" Text="Suffix" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Small"></asp:Label>
                                    <asp:TextBox Font-Bold="true" ID="txt_suffix" runat="server" Font-Names="Book Antiqua"
                                        Width="80px" Font-Size="Small">
                                    </asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender122aa233" runat="server"
                                        TargetControlID="txt_suffix" FilterType="Numbers" Enabled="True">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Button ID="Btn_save" runat="server" Text="Save" Font-Size="Small" Font-Names="Book Antiqua"
                                        CausesValidation="false" OnClick="btn_folio" />
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <fieldset style="height: 54px; width: 877px;">
                                    <legend>Student Result</legend>
                                    <asp:Label ID="lblpay1" runat="server" Text="Payment of exam fees without penalty"
                                        Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Small">
                                    </asp:Label>
                                    <asp:TextBox Font-Bold="true" ID="txtpaydate1from" placeholder="From" runat="server"
                                        Font-Names="Book Antiqua" Width="80px" Font-Size="Small"></asp:TextBox>
                                    <asp:TextBox Font-Bold="true" ID="txtpaydate1to" placeholder="To" runat="server"
                                        Font-Names="Book Antiqua" Width="80px" Font-Size="Small"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender8" runat="server" TargetControlID="txtpaydate1from"
                                        Format="dd.MM.yyyy">
                                    </asp:CalendarExtender>
                                    <asp:CalendarExtender ID="CalendarExtender10" runat="server" TargetControlID="txtpaydate1to"
                                        Format="dd.MM.yyyy">
                                    </asp:CalendarExtender>
                                    <asp:Label ID="lblpay2" runat="server" Text="Payment of exam fees with penalty" Font-Bold="true"
                                        Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
                                    <asp:TextBox Font-Bold="true" ID="txtpaydate2from" placeholder="From" runat="server"
                                        Font-Names="Book Antiqua" Width="80px" Font-Size="Small">
                                    </asp:TextBox>
                                    <asp:TextBox Font-Bold="true" ID="txtpaydate2to" placeholder="To" runat="server"
                                        Font-Names="Book Antiqua" Width="80px" Font-Size="Small">
                                    </asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender9" runat="server" TargetControlID="txtpaydate2from"
                                        Format="dd.MM.yyyy">
                                    </asp:CalendarExtender>
                                    <asp:CalendarExtender ID="CalendarExtender11" runat="server" TargetControlID="txtpaydate2to"
                                        Format="dd.MM.yyyy">
                                    </asp:CalendarExtender>
                                    <asp:Button ID="btnsavepaydate" runat="server" Text="Save" Font-Size="Small" Font-Names="Book Antiqua"
                                        CausesValidation="false" OnClick="btnsavepaydate_click" />
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: 54px; width: 232px;">
                                    <legend>I.C.A Mark</legend>
                                    <asp:CheckBox ID="chk_onlycia" runat="server" Text="I.C.A Mark" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Font-Bold="True" />
                                    <br />
                                    <asp:CheckBox ID="chk_secuirty" runat="server" Text="I.C.A Secure" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Font-Bold="True" />
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 54px; width: 300px;">
                                    <legend>Minimum ICA Mark To Apply Moderation</legend>
                                    <asp:Label ID="Label34" runat="server" Text="Course" Style="height: 15px; width: 143px"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    <asp:DropDownList ID="ddlminicamode" runat="server" OnSelectedIndexChanged="ddlminicamode_SelectedIndexChanged"
                                        Style="height: 22px; width: 77px" Font-Names="Book Antiqua" Font-Size="Medium"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtminicamode" runat="server" Style="height: 17px; width: 30px"
                                        Width="72px" MaxLength="2" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="txtminicamode"
                                        FilterType="Numbers" Enabled="True">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Button ID="btnminicamoderation" Text="Save" runat="server" OnClick="btnminicamoderation_Click"
                                        Style="height: 20px; display: inline-block; color: Black; font-family: Book Antiqua; font-size: small; font-weight: bold; width: 50px; text-align: center; border-style: solid; border-width: 1px; border-color: gray; background-color: none; border-radius: 4px 4px 4px 4px; text-decoration: none;" />
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 54px; width: 244px;">
                                    <legend>Automatic Moderation Save</legend>
                                    <asp:CheckBox ID="chkmoderationupdate" runat="server" Text="Moderation Automatic Update"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="True" />
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" rowspan="3">
                                <fieldset style="height: 180px; width: 244px;">
                                    <legend>Mark Sheet Format</legend>
                                    <asp:Button ID="btnaddrowmark" runat="server" Text="Add Row" OnClick="btnaddrowmark_Click1" />
                                    <asp:Label ID="Label35" runat="server" Text="Education Level: " Font-Names="Book Antiqua"
                                        Font-Size="Smaller"></asp:Label>
                                    <asp:DropDownList ID="ddlmarksheetadd" runat="server" OnSelectedIndexChanged="ddlmarksheetaddSelectedIndexChanged"
                                        Font-Names="Book Antiqua" Font-Size="Smaller" AutoPostBack="True">
                                    </asp:DropDownList>
                                    <farpoint:fpspread id="Fpmarksheetform" runat="server" bordercolor="Black" horizontalscrollbarpolicy="Never"
                                        verticalscrollbarpolicy="AsNeeded" borderstyle="Solid" borderwidth="1px" height="100"
                                        width="330" scrollbar3dlightcolor="Yellow" scrollbararrowcolor="Brown" scrollbarbasecolor="Goldenrod"
                                        scrollbardarkshadowcolor="DarkOrange" scrollbarfacecolor="RosyBrown" scrollbarhighlightcolor="#006699"
                                        scrollbarshadowcolor="#9999FF" currentpageindex="0" designstring="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                                        editmodepermanent="True" scrollcontent="true" scrollcontentcolumns="" scrollcontentmaxheight="50"
                                        scrollcontenttime="500">
                                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonType="PushButton"
                                            ButtonHighlightColor="ControlLightLight" ButtonShadowColor="ControlDark" Visible="true">
                                            <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                                        </CommandBar>
                                        <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" />
                                        <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" />
                                        <Sheets>
                                            <FarPoint:SheetView DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Sheet&gt;&lt;Data&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;SheetName&gt;Sheet1&lt;/SheetName&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnFooter&gt;&lt;/Data&gt;&lt;Presentation&gt;&lt;ActiveSkin class=&quot;FarPoint.Web.Spread.SheetSkin&quot;&gt;&lt;Name&gt;Default&lt;/Name&gt;&lt;BackColor&gt;Empty&lt;/BackColor&gt;&lt;CellBackColor&gt;Empty&lt;/CellBackColor&gt;&lt;CellForeColor&gt;Empty&lt;/CellForeColor&gt;&lt;CellSpacing&gt;0&lt;/CellSpacing&gt;&lt;GridLines&gt;Both&lt;/GridLines&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;HeaderBackColor&gt;Empty&lt;/HeaderBackColor&gt;&lt;HeaderForeColor&gt;Empty&lt;/HeaderForeColor&gt;&lt;FlatColumnHeader&gt;False&lt;/FlatColumnHeader&gt;&lt;FooterBackColor&gt;Empty&lt;/FooterBackColor&gt;&lt;FooterForeColor&gt;Empty&lt;/FooterForeColor&gt;&lt;FlatColumnFooter&gt;False&lt;/FlatColumnFooter&gt;&lt;FlatRowHeader&gt;False&lt;/FlatRowHeader&gt;&lt;HeaderFontBold&gt;False&lt;/HeaderFontBold&gt;&lt;FooterFontBold&gt;False&lt;/FooterFontBold&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionForeColor&gt;Empty&lt;/SelectionForeColor&gt;&lt;EvenRowBackColor&gt;Empty&lt;/EvenRowBackColor&gt;&lt;OddRowBackColor&gt;Empty&lt;/OddRowBackColor&gt;&lt;ShowColumnHeader&gt;True&lt;/ShowColumnHeader&gt;&lt;ShowColumnFooter&gt;False&lt;/ShowColumnFooter&gt;&lt;ShowRowHeader&gt;True&lt;/ShowRowHeader&gt;&lt;ColumnHeaderBackground class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/ColumnHeaderBackground&gt;&lt;SheetCornerBackground class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/SheetCornerBackground&gt;&lt;HeaderGrayAreaColor&gt;#7999c2&lt;/HeaderGrayAreaColor&gt;&lt;/ActiveSkin&gt;&lt;AxisModels&gt;&lt;Column class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/Column&gt;&lt;RowHeaderColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;40&quot; orientation=&quot;Horizontal&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;Size&gt;40&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/RowHeaderColumn&gt;&lt;ColumnHeaderRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnHeaderRow&gt;&lt;ColumnFooterRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnFooterRow&gt;&lt;/AxisModels&gt;&lt;StyleModels&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;RowHeaderDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnHeaderDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;DataAreaDefault&quot;&gt;&lt;Font&gt;&lt;Name&gt;Book Antiqua&lt;/Name&gt;&lt;Names&gt;&lt;Name&gt;Book Antiqua&lt;/Name&gt;&lt;/Names&gt;&lt;Size&gt;Medium&lt;/Size&gt;&lt;Bold&gt;False&lt;/Bold&gt;&lt;Italic&gt;False&lt;/Italic&gt;&lt;Overline&gt;False&lt;/Overline&gt;&lt;Strikeout&gt;False&lt;/Strikeout&gt;&lt;Underline&gt;False&lt;/Underline&gt;&lt;/Font&gt;&lt;GdiCharSet&gt;254&lt;/GdiCharSet&gt;&lt;HorizontalAlign&gt;Center&lt;/HorizontalAlign&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Border class=&quot;FarPoint.Web.Spread.Border&quot; Size=&quot;1&quot; Style=&quot;Solid&quot;&gt;&lt;Bottom Color=&quot;ControlDark&quot; /&gt;&lt;Left Size=&quot;0&quot; /&gt;&lt;Right Color=&quot;ControlDark&quot; /&gt;&lt;Top Size=&quot;0&quot; /&gt;&lt;/Border&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnFooterDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnFooter&gt;&lt;/StyleModels&gt;&lt;MessageRowStyle class=&quot;FarPoint.Web.Spread.Appearance&quot;&gt;&lt;BackColor&gt;LightYellow&lt;/BackColor&gt;&lt;ForeColor&gt;Red&lt;/ForeColor&gt;&lt;/MessageRowStyle&gt;&lt;SheetCornerStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Border class=&quot;FarPoint.Web.Spread.Border&quot; Size=&quot;1&quot; Style=&quot;Solid&quot;&gt;&lt;Bottom Color=&quot;ControlDark&quot; /&gt;&lt;Left Size=&quot;0&quot; /&gt;&lt;Right Color=&quot;ControlDark&quot; /&gt;&lt;Top Size=&quot;0&quot; /&gt;&lt;/Border&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/SheetCornerStyle&gt;&lt;AllowLoadOnDemand&gt;false&lt;/AllowLoadOnDemand&gt;&lt;LoadRowIncrement &gt;10&lt;/LoadRowIncrement &gt;&lt;LoadInitRowCount &gt;30&lt;/LoadInitRowCount &gt;&lt;AllowVirtualScrollPaging&gt;false&lt;/AllowVirtualScrollPaging&gt;&lt;TopRow&gt;0&lt;/TopRow&gt;&lt;PreviewRowStyle class=&quot;FarPoint.Web.Spread.PreviewRowInfo&quot; /&gt;&lt;/Presentation&gt;&lt;Settings&gt;&lt;Name&gt;Sheet1&lt;/Name&gt;&lt;Categories&gt;&lt;Appearance&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionBorder class=&quot;FarPoint.Web.Spread.Border&quot; /&gt;&lt;/Appearance&gt;&lt;Behavior&gt;&lt;EditTemplateColumnCount&gt;2&lt;/EditTemplateColumnCount&gt;&lt;GroupBarText&gt;Drag a column to group by that column.&lt;/GroupBarText&gt;&lt;/Behavior&gt;&lt;Layout&gt;&lt;RowHeaderColumnCount&gt;1&lt;/RowHeaderColumnCount&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;/Layout&gt;&lt;/Categories&gt;&lt;ActiveRow&gt;0&lt;/ActiveRow&gt;&lt;ActiveColumn&gt;0&lt;/ActiveColumn&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;ColumnFooterRowCount&gt;1&lt;/ColumnFooterRowCount&gt;&lt;PrintInfo&gt;&lt;Header /&gt;&lt;Footer /&gt;&lt;ZoomFactor&gt;0&lt;/ZoomFactor&gt;&lt;FirstPageNumber&gt;1&lt;/FirstPageNumber&gt;&lt;Orientation&gt;Auto&lt;/Orientation&gt;&lt;PrintType&gt;All&lt;/PrintType&gt;&lt;PageOrder&gt;Auto&lt;/PageOrder&gt;&lt;BestFitCols&gt;False&lt;/BestFitCols&gt;&lt;BestFitRows&gt;False&lt;/BestFitRows&gt;&lt;PageStart&gt;-1&lt;/PageStart&gt;&lt;PageEnd&gt;-1&lt;/PageEnd&gt;&lt;ColStart&gt;-1&lt;/ColStart&gt;&lt;ColEnd&gt;-1&lt;/ColEnd&gt;&lt;RowStart&gt;-1&lt;/RowStart&gt;&lt;RowEnd&gt;-1&lt;/RowEnd&gt;&lt;ShowBorder&gt;True&lt;/ShowBorder&gt;&lt;ShowGrid&gt;True&lt;/ShowGrid&gt;&lt;ShowColor&gt;True&lt;/ShowColor&gt;&lt;ShowColumnHeader&gt;Inherit&lt;/ShowColumnHeader&gt;&lt;ShowRowHeader&gt;Inherit&lt;/ShowRowHeader&gt;&lt;ShowColumnFooter&gt;Inherit&lt;/ShowColumnFooter&gt;&lt;ShowColumnFooterEachPage&gt;True&lt;/ShowColumnFooterEachPage&gt;&lt;ShowTitle&gt;True&lt;/ShowTitle&gt;&lt;ShowSubtitle&gt;True&lt;/ShowSubtitle&gt;&lt;UseMax&gt;True&lt;/UseMax&gt;&lt;UseSmartPrint&gt;False&lt;/UseSmartPrint&gt;&lt;Opacity&gt;255&lt;/Opacity&gt;&lt;PrintNotes&gt;None&lt;/PrintNotes&gt;&lt;Centering&gt;None&lt;/Centering&gt;&lt;RepeatColStart&gt;-1&lt;/RepeatColStart&gt;&lt;RepeatColEnd&gt;-1&lt;/RepeatColEnd&gt;&lt;RepeatRowStart&gt;-1&lt;/RepeatRowStart&gt;&lt;RepeatRowEnd&gt;-1&lt;/RepeatRowEnd&gt;&lt;SmartPrintPagesTall&gt;1&lt;/SmartPrintPagesTall&gt;&lt;SmartPrintPagesWide&gt;1&lt;/SmartPrintPagesWide&gt;&lt;HeaderHeight&gt;-1&lt;/HeaderHeight&gt;&lt;FooterHeight&gt;-1&lt;/FooterHeight&gt;&lt;/PrintInfo&gt;&lt;TitleInfo class=&quot;FarPoint.Web.Spread.TitleInfo&quot;&gt;&lt;Style class=&quot;FarPoint.Web.Spread.StyleInfo&quot;&gt;&lt;BackColor&gt;#e7eff7&lt;/BackColor&gt;&lt;HorizontalAlign&gt;Right&lt;/HorizontalAlign&gt;&lt;/Style&gt;&lt;Value type=&quot;System.String&quot; whitespace=&quot;&quot; /&gt;&lt;/TitleInfo&gt;&lt;LayoutTemplate class=&quot;FarPoint.Web.Spread.LayoutTemplate&quot;&gt;&lt;Layout&gt;&lt;ColumnCount&gt;4&lt;/ColumnCount&gt;&lt;RowCount&gt;1&lt;/RowCount&gt;&lt;/Layout&gt;&lt;Data&gt;&lt;LayoutData class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;Cells&gt;&lt;Cell row=&quot;0&quot; column=&quot;0&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;0&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;1&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;1&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;2&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;2&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;3&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;3&lt;/Data&gt;&lt;/Cell&gt;&lt;/Cells&gt;&lt;/LayoutData&gt;&lt;/Data&gt;&lt;AxisModels&gt;&lt;LayoutColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/LayoutColumn&gt;&lt;LayoutRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot; /&gt;&lt;/Items&gt;&lt;/LayoutRow&gt;&lt;/AxisModels&gt;&lt;/LayoutTemplate&gt;&lt;LayoutMode&gt;CellLayoutMode&lt;/LayoutMode&gt;&lt;CurrentPageIndex type=&quot;System.Int32&quot;&gt;0&lt;/CurrentPageIndex&gt;&lt;/Settings&gt;&lt;/Sheet&gt;"
                                                SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                        <TitleInfo BackColor="#E7EFF7" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                            Font-Size="X-Large" Font-Strikeout="False" Font-Underline="False" ForeColor=""
                                            HorizontalAlign="Center" VerticalAlign="NotSet" Text="">
                                        </TitleInfo>
                                    </farpoint:fpspread>
                                    <asp:Button ID="btnmarkshsave" runat="server" Text="Save" OnClick="btnmarkshsave_Click1" />
                                    <asp:Button ID="btnmarkshdelete" runat="server" Text="Delete" OnClick="btnmarkshdelete_Click1" />
                                    <asp:Label ID="coemrksheeterror" runat="server" ForeColor="Red"></asp:Label>
                                </fieldset>
                            </td>
                            <td colspan="3">
                                <fieldset style="height: 40px; width: auto;">
                                    <legend>Moderation Mark</legend>
                                    <asp:Label ID="lblmobatch" runat="server" Text="Batch Year" Font-Names="Book Antiqua"
                                        Font-Size="Smaller"></asp:Label>
                                    <asp:DropDownList ID="ddlmodbatch" runat="server" OnSelectedIndexChanged="ddlmodmarkchange"
                                        Font-Names="Book Antiqua" Font-Size="Smaller" AutoPostBack="True">
                                    </asp:DropDownList>
                                    <asp:Label ID="lbl" runat="server" Text="Edu Level" Font-Names="Book Antiqua" Font-Size="Smaller">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlmodedu" runat="server" OnSelectedIndexChanged="ddlmodmarkchange"
                                        Font-Names="Book Antiqua" Font-Size="Smaller" Width="90px" AutoPostBack="True">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblmodmark" runat="server" Text="Mark" Font-Names="Book Antiqua" Font-Size="Smaller"></asp:Label>
                                    <asp:TextBox Font-Bold="true" ID="txtmodmark" runat="server" Font-Names="Book Antiqua"
                                        Font-Size="Small" MaxLength="2" Width="40px">
                                    </asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" TargetControlID="txtmodmark"
                                        FilterType="Numbers" Enabled="True">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Button ID="btnmodsave" runat="server" Text="Save" OnClick="btnmodsave_Click1" />
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <fieldset style="height: 40px; width: auto;">
                                    <legend>Fail Grade Mark</legend>
                                    <asp:Label ID="Label36" runat="server" Text="Batch Year" Font-Names="Book Antiqua"
                                        Font-Size="Smaller"></asp:Label>
                                    <asp:DropDownList ID="ddlfailbatch" runat="server" OnSelectedIndexChanged="failgradechange"
                                        Font-Names="Book Antiqua" Font-Size="Smaller" AutoPostBack="True">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label37" runat="server" Text="Edu Level" Font-Names="Book Antiqua"
                                        Font-Size="Smaller">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlfialedu" Width="90px" runat="server" OnSelectedIndexChanged="failgradechange"
                                        Font-Names="Book Antiqua" Font-Size="Smaller" AutoPostBack="True">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label38" runat="server" Text="Fail Grade" Font-Names="Book Antiqua"
                                        Font-Size="Smaller"></asp:Label>
                                    <asp:TextBox Font-Bold="true" ID="txtfailgrade" runat="server" Font-Names="Book Antiqua"
                                        Font-Size="Small" MaxLength="5" Width="40px">
                                    </asp:TextBox>
                                    <asp:Button ID="btnfailsave" runat="server" Text="Save" OnClick="btnfailsave_Click1" />
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <fieldset style="height: 40px; width: 400px;">
                                    <legend>Compulsory Subject Type</legend>
                                    <asp:Label ID="lblCompCOurse" runat="server" Text="Course" Font-Names="Book Antiqua"
                                        Font-Size="Smaller"></asp:Label>
                                    <asp:DropDownList ID="ddlCOmpCOurse" runat="server" OnSelectedIndexChanged="ddlCOmpCOurse_IndexChange"
                                        Width="60px" Font-Names="Book Antiqua" Font-Size="Smaller" AutoPostBack="True">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblCompType" runat="server" Text="Type" Font-Names="Book Antiqua"
                                        Font-Size="Smaller">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlCOmpType" runat="server" Width="100px" Font-Names="Book Antiqua"
                                        Font-Size="Smaller">
                                    </asp:DropDownList>
                                    <asp:Button ID="btnCOmpType" runat="server" Text="Save" OnClick="btnCOmpType_Click" />
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: 100px; width: 250px;">
                                    <legend>Student Exam Application</legend>
                                    <asp:CheckBoxList ID="cblStudExApp" runat="server">
                                        <asp:ListItem>Apply To Exam</asp:ListItem>
                                        <asp:ListItem>Generate Challan</asp:ListItem>
                                        <asp:ListItem>Hall Ticket</asp:ListItem>
                                    </asp:CheckBoxList>
                                    <center>
                                        <asp:Button ID="btnStudExAppSave" runat="server" Text="Save" OnClick="btnStudExAppSave_Click" />
                                    </center>
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="width: 300px; height: auto;">
                                    <legend>COE Mark Entry</legend>
                                    <table>
                                        <tr>
                                            <td colspan="3">
                                                <asp:CheckBox ID="chkIncludeCondoUnpaid" Checked="false" runat="server" Text="Include Condonation Unpaid Students in Mark Entry"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:CheckBox ID="chkIncMarkEntBatchRights" Checked="false" runat="server" Text="Include Batch Year Rights For Semester in Mark Entry"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="width: 300px; height: auto;">
                                    <legend>Diplay Dummy No in Mark Entry</legend>
                                    <table>
                                        <tr>
                                            <td colspan="3">
                                                <asp:CheckBox ID="chkShowDummy" Checked="false" runat="server" Text="Show Dummy Number in Mark Entry"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Type
                                            </td>
                                            <td colspan="2">
                                                <asp:RadioButtonList ID="rblDummyCommonSub" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True">Common</asp:ListItem>
                                                    <asp:ListItem>SubjectWise</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Mode
                                            </td>
                                            <td colspan="2">
                                                <asp:RadioButtonList ID="rblDummyMode" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True">Serial</asp:ListItem>
                                                    <asp:ListItem>Random</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: auto; width: 250px;">
                                    <legend>CAM CIA Mark Entry Lock</legend>
                                    <table>
                                        <tr>
                                            <td colspan="3">
                                                <asp:CheckBox ID="chkCIALock" Checked="false" runat="server" Text="CAM CIA Mark Entry Lock"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" />
                                                <%--CAM CIA Mark Entry Only Once--%>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: auto; width: auto;">
                                    <legend>COE Exam Time Table & Seating</legend>
                                    <table>
                                        <tr>
                                            <td colspan="3">
                                                <asp:CheckBox ID="chkCoeSeatingArrangementLock" Checked="false" runat="server" Text="COE Exam Seating Arrangement Generation Lock"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" />
                                            </td>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:CheckBox ID="chkSerial" Checked="false" runat="server" Text="Seating Arrangement Serial no order"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:CheckBox ID="chkQpapertype" Checked="false" runat="server" Text="Seating Arrangement Qpaper order"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" />
                                                </td>
                                            </tr>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: auto; width: auto;">
                                    <legend>Maximum Subject To Be Applied</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkNRNEMaxSubject" runat="server" CssClass="font" Text="Maximum Subject Applied Count For NR/NE" />
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtNRNEMaxSubject" runat="server" Width="60px" MaxLength="2"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="filterNRNEMaxSubject" runat="server" TargetControlID="txtNRNEMaxSubject"
                                                    FilterType="Numbers">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkOnlinePaymentMaxSubject" runat="server" CssClass="font" Text="Maximum Subject Applied Count For Online Payment" />
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtOnPayMaxSubject" runat="server" Width="60px" MaxLength="2"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="filterOnPayMaxSubject" runat="server" TargetControlID="txtOnPayMaxSubject"
                                                    FilterType="Numbers">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="10">
                                <fieldset style="height: 250px;">
                                    <legend>Staff Allowance Payment Finance Year,Header & Ledger Settings </legend>
                                    <table>
                                        <tr>
                                            <td>Type :
                                            </td>
                                            <td>
                                                <div style="position: relative;">
                                                    <asp:TextBox ID="txt_FianceTypeCOE" runat="server" CssClass="textbox txtheight2"
                                                        Style="width: 200px" ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="Panel24" runat="server" CssClass="multxtpanel " Style="width: 255px; border-color: HighlightText; height: 150px;">
                                                        <asp:CheckBox ID="chk_FianceTypeCOE" runat="server" Width="204px" Text="Select All"
                                                            AutoPostBack="true" OnCheckedChanged="chkFinanceTypeCOE_OnCheckedChanged" />
                                                        <asp:CheckBoxList ID="cbl_FianceTypeCOE" runat="server" AutoPostBack="True">
                                                            <%-- OnSelectedIndexChanged="cblhdOnline_OnSelectedIndexChanged" --%>
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                </div>
                                            </td>
                                            <td>Finance Year
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_FinanceYearCOE" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td>Header
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_FinanceHeaderCOE" runat="server" Width="80px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>Ledger
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_FinanceLedgerCOE" runat="server" Width="80px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Button ID="Button2" runat="server" Text="Save" CssClass="textbox textbox1 btn2"
                                                    Width="70px" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" Style="background-color: Green; color: White;"
                                                    OnClick="btnsaveFinanceCOE_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="10">
                                <fieldset style="height: 250px;">
                                    <legend>Revaluation Apply </legend>
                                    <table>
                                        <tr>
                                            <td>Exam Year :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlRevalExamYear" runat="server" OnSelectedIndexChanged="ddlRevalExamYear_OnSelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td>Exam Month :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlRevalExamMonth" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Xerox Max Subjects :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlMaxSubjectsXerox" runat="server">
                                                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                    <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                    <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                                    <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                                    <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                                    <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                    <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                    <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                    <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                    <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                    <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>Revaluation Max Subjects :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlMaxSubjectsReval" runat="server">
                                                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                    <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                    <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                                    <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                                    <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                                    <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                    <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                    <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                    <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                    <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                    <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Xerox Application Start Date :
                                            </td>
                                            <td>
                                                <asp:TextBox Font-Bold="true" ID="txtXeroxAppStartDate" runat="server" Font-Names="Book Antiqua"
                                                    Width="80px" Font-Size="Small"></asp:TextBox>
                                                <asp:CalendarExtender ID="cldXeroxAppStartDate" runat="server" TargetControlID="txtXeroxAppStartDate"
                                                    Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>Xerox Application End Date :
                                            </td>
                                            <td>
                                                <asp:TextBox Font-Bold="true" ID="txtXeroxAppEndDate" runat="server" Font-Names="Book Antiqua"
                                                    Width="80px" Font-Size="Small"></asp:TextBox>
                                                <asp:CalendarExtender ID="cldXeroxAppEndDate" runat="server" TargetControlID="txtXeroxAppEndDate"
                                                    Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                                <asp:CompareValidator ID="CompareValidator1" ValidationGroup="Date" ForeColor="Red"
                                                    runat="server" ControlToValidate="txtXeroxAppStartDate" ControlToCompare="txtXeroxAppEndDate"
                                                    Display="Dynamic" Operator="LessThan" Type="Date" ErrorMessage="Start date must be less than End date."></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Revaluation Application Start Date :
                                            </td>
                                            <td>
                                                <asp:TextBox Font-Bold="true" ID="txtRevalAppStartDate" runat="server" Font-Names="Book Antiqua"
                                                    Width="80px" Font-Size="Small"></asp:TextBox>
                                                <asp:CalendarExtender runat="server" ID="cldRevalAppStartDate" TargetControlID="txtRevalAppStartDate"
                                                    Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>Revaluation Application End Date :
                                            </td>
                                            <td>
                                                <asp:TextBox Font-Bold="true" ID="txtRevalAppEndDate" runat="server" Font-Names="Book Antiqua"
                                                    Width="80px" Font-Size="Small"></asp:TextBox>
                                                <asp:CalendarExtender ID="cldRevalAppEndDate" runat="server" TargetControlID="txtRevalAppEndDate"
                                                    Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                                <asp:CompareValidator ID="CompareValidator2" ValidationGroup="Date" Display="Dynamic"
                                                    ForeColor="Red" runat="server" ControlToValidate="txtRevalAppStartDate" ControlToCompare="txtRevalAppEndDate"
                                                    Operator="LessThan" Type="Date" ErrorMessage="Start date must be less than End date."></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <%--                <tr>      
                                            <td>
                                                 Xerox Application Charges :
                                            </td>
                                            <td>
                                                <asp:TextBox Font-Bold="true" ID="txtXeroxCharges"  runat="server"
                                                  Font-Names="Book Antiqua" Width="80px" Font-Size="Small" TextMode="Number"></asp:TextBox>
                                            </td>
                                     </tr>
                                     <tr>
                                          <td>
                                              Revaluation Application Charges :
                                          </td>
                                          <td>
                                              <asp:TextBox Font-Bold="true" ID="txtRevalCharges"  runat="server"
                                                Font-Names="Book Antiqua" Width="80px" Font-Size="Small" TextMode="Number"></asp:TextBox>
                                          </td>
                                     </tr>           
                                     <tr>
                                           <td>
                                               Finance Year
                                           </td>
                                           <td>
                                               <asp:DropDownList ID="ddl_FinanceYearCOEnew" runat="server">
                                               </asp:DropDownList>
                                           </td>
                                           <td>
                                               Header
                                           </td>
                                           <td>
                                               <asp:DropDownList ID="ddl_FinanceHeaderCOEnew" runat="server" Width="80px">
                                               </asp:DropDownList>
                                           </td>
                                           <td>
                                               Ledger
                                           </td>
                                           <td>
                                               <asp:DropDownList ID="ddl_FinanceLedgerCOEnew" runat="server" Width="80px">
                                               </asp:DropDownList>
                                           </td>
                                     </tr>
                                     
                                        --%>
                                    </table>
                                    <asp:Button ID="btnRevaluationSubmit" Text="Save" runat="server" OnClick="btnRevaluationSubmit_OnClick"
                                        CssClass="textbox textbox1 btn2" Width="70px" Font-Bold="true" Font-Size="Medium"
                                        Font-Names="Book Antiqua" Style="background-color: Green; color: White;" />
                                    <asp:Label ID="lblRevalError" runat="server" Visible="false"></asp:Label>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
            <%--Attendance Settings Tab--%>
            <center>
                <div id="div6Attendance" runat="server" style="width: 980px; height: 400px; background: white; border: 1px solid #0095E8; overflow: auto;"
                    visible="false">
                    <table>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: 200px; width: 225px;">
                                    <legend style="height: 10">Attendance</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chbSMS" runat="server" Text="SMS User Id" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chbAttendanceSms" runat="server" Text="Attendance Sms for Absent"
                                                    AutoPostBack="True" OnCheckedChanged="AttendanceChecked" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chbvoicecall" runat="server" Text="Voice Call for Absent" AutoPostBack="true"
                                                    OnCheckedChanged="chbvoicecall_OnCheckedChanged" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chbFather" runat="server" Text="Father" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chbMother" runat="server" Text="Mother" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chbStudent" runat="server" Text="Student" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chbcurrentdayaatnd" runat="server" Text="SMS For Current Date" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="btnAtSave" runat="server" Font-Bold="True" Width="50px" Text="Save"
                                                    Font-Names="Book Antiqua" Font-Size="Small" ForeColor="Black" OnClick="AttendanceSave"
                                                    CausesValidation="False" Style="display: inline-block; color: Black; font-family: Book Antiqua; font-size: small; font-weight: bold; width: 50px; text-align: center; border-style: solid; border-width: 1px; border-color: gray; background-color: none; border-radius: 4px 4px 4px 4px; text-decoration: none;"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 160px; width: 424px;">
                                    <legend style="height: 5">Select Period For SMS Send</legend>
                                    <asp:Label ID="Label22" runat="server" Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"
                                        Text="Select Options"></asp:Label>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rbhour" runat="server" Text="Hour" AutoPostBack="true" GroupName="Attend"
                                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="rbhour_CheckedChanged" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rbday" runat="server" Text="Day" AutoPostBack="true" GroupName="Attend"
                                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="rbday_CheckedChanged" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkminimumabsent" runat="server" Text="Minimun Absent Day" AutoPostBack="true"
                                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="chkminimumabsent_CheckedChanged" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                            <td>
                                                <%--sankar edited--%>
                                                <asp:TextBox ID="txtsms" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                    ReadOnly="true" Width="113px" Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"></asp:TextBox>
                                                <%--<asp:DropDownExtender ID="ddlsms" runat="server" DropDownControlID="psms" DynamicServicePath=""
                                                    Enabled="false" TargetControlID="txtsms">
                                                </asp:DropDownExtender>--%>
                                                <asp:Panel ID="psms" runat="server" Height="150px" Width="110px" CssClass="multxtpanel">
                                                    <asp:CheckBox ID="chksms" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnCheckedChanged="chksms_CheckedChanged" Text="Select All"
                                                        AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="chklistsms" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                        OnSelectedIndexChanged="chklistsms_SelectedIndexChanged" Height="200px" Font-Bold="True"
                                                        Font-Names="Book Antiqua">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="pceAt1" runat="server" Position="Bottom" TargetControlID="txtsms"
                                                    PopupControlID="psms">
                                                </asp:PopupControlExtender>
                                            </td>
                                            <td>
                                                <%-- Style="position: absolute; left: 541px;
                                                    top: 305px;"   left: 586px; position: absolute;
                                                    top: 311px;--%>
                                                <asp:Label ID="lblsmsday" runat="server" Text="Days" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                                <asp:TextBox ID="txt_smsday" runat="server" AutoPostBack="true" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold; height: 20px; width: 50px;"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="width: 172px; height: 160px;">
                                    <legend style="height: 5">Select Rights For OD Entry</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblselect" runat="server" Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Text="Select Options"></asp:Label>
                                            </td>
                                            <td>
                                                <div style="position: relative;">
                                                    <asp:UpdatePanel ID="UpdatePanel38" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtselect" runat="server" CssClass="font" Width="40px"></asp:TextBox>
                                                            <asp:DropDownExtender ID="ddlselect" runat="server" DropDownControlID="panelselecthours"
                                                                DynamicServicePath="" Enabled="True" TargetControlID="txtselect">
                                                            </asp:DropDownExtender>
                                                            <asp:Panel ID="panelselecthours" runat="server" BackColor="White" BorderColor="Black"
                                                                Width="70px" Height="100px" BorderStyle="Solid" BorderWidth="2px" ScrollBars="Vertical">
                                                                <asp:CheckBoxList ID="Chkselecthours" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                    Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Width="60px" OnSelectedIndexChanged="Chkselecthours_SelectedIndexChanged">
                                                                    <asp:ListItem>P</asp:ListItem>
                                                                    <asp:ListItem>A</asp:ListItem>
                                                                    <asp:ListItem>OD</asp:ListItem>
                                                                    <asp:ListItem>ML</asp:ListItem>
                                                                    <asp:ListItem>SOD</asp:ListItem>
                                                                    <asp:ListItem>NSS</asp:ListItem>
                                                                    <asp:ListItem>L</asp:ListItem>
                                                                    <asp:ListItem>NCC</asp:ListItem>
                                                                    <asp:ListItem>HS</asp:ListItem>
                                                                    <asp:ListItem>PP</asp:ListItem>
                                                                    <asp:ListItem>SYOD</asp:ListItem>
                                                                    <asp:ListItem>COD</asp:ListItem>
                                                                    <asp:ListItem>OOD</asp:ListItem>
                                                                    <asp:ListItem>LA</asp:ListItem>
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: 70px; width: 180px;">
                                    <legend style="height: 5">Order By&nbsp;</legend>
                                    <asp:CheckBox ID="chkrollorder" runat="server" Text="Roll No"></asp:CheckBox>
                                    <asp:CheckBox ID="chkregorder" runat="server" Text="Reg No"></asp:CheckBox><br />
                                    <asp:CheckBox ID="chknameorder" runat="server" Text="Student Name"></asp:CheckBox>
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 110px; width: 400px;">
                                    <legend style="height: 5">Batch and Sec Rights&nbsp;</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label25" runat="server" Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Text="Batch"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlattbatch" runat="server" Width="100px" Font-Size="Medium"
                                                    AutoPostBack="true" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlattbatch_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label26" runat="server" Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Text="Sec"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtattsec" runat="server" CssClass="font" Width="104px"></asp:TextBox>
                                                <asp:Panel ID="pattsec" runat="server" BackColor="White" BorderColor="Black" Height="80px"
                                                    BorderStyle="Solid" BorderWidth="2px" ScrollBars="Vertical">
                                                    <asp:CheckBox ID="chksecall" runat="server" Font-Bold="True" OnCheckedChanged="chkallsec_CheckedChanged"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="chkattsec" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" OnSelectedIndexChanged="chkattsec_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtattsec"
                                                    PopupControlID="pattsec" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnattsave" runat="server" Text="Save" Font-Size="Medium" Font-Bold="True"
                                                    Font-Names="Book Antiqua" OnClick="btnattsave_Click" CausesValidation="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5">
                                                <asp:CheckBox ID="chkCAMBasedBatchRights" runat="server" Text="CAM Entry Based On Batch and Section Rights"
                                                    Checked="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 10; width: 170px;">
                                    <legend style="height: 5">Alternate Schedule&nbsp;</legend>
                                    <asp:CheckBox ID="chkaltnatesms" Text="Send SMS" runat="server" />
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:CheckBox ID="splhr_check" runat="server" Font-Size="Medium" Font-Bold="True"
                                    ForeColor="Black" Font-Names="Book Antiqua" Text="Include Special Hour(s) In All Attendance Reports" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" rowspan="2">
                                <fieldset style="width: 287px; height: 141px;">
                                    <legend>Leave Type For Staff Attendance&nbsp;</legend>
                                    <asp:Label ID="lblleavetypestaff" runat="server" Font-Size="Medium" Font-Bold="True"
                                        Font-Names="Book Antiqua" Text="Leave Type"></asp:Label>
                                    <asp:TextBox ID="txt_staffleavetype" runat="server" ReadOnly="true" Font-Bold="True"
                                        Width="135px" Font-Names="Book Antiqua" Font-Size="medium" CssClass="Dropdown_Txt_Box">---Select---</asp:TextBox><%--Enabled="false" 26.09.17 barath--%>
                                    <asp:Panel ID="panelstaffleavetype" runat="server" Height="150px" Width="135px" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cbstaffleavetype" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="cbstaffleavetype_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblstaffleavetype" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblstaffleavetype_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_staffleavetype"
                                        PopupControlID="panelstaffleavetype" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    <asp:TextBox ID="txt_focus" runat="server" Visible="false"></asp:TextBox>
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="width: 280px; height: 57px;">
                                    <legend>Individual Student Attendace Lock</legend>
                                    <asp:CheckBox ID="cbIndualAttendance" runat="server" Text="Individual Student Attendace Lock"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="medium" />
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="width: 154px; height: 57px;">
                                    <legend>Copy Attendance</legend>
                                    <asp:CheckBox ID="chckcopy" runat="server" Text="Copy Attendance" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="medium" />
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="height: 40px; width: 300px;">
                                    <legend>Subject Allotement</legend>
                                    <asp:CheckBox ID="chkpersem" runat="server" Text="Previous Semester Subject Allotment"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="width: 190px; height: 57px;">
                                    <legend>Print Edit Option Lock</legend>
                                    <asp:CheckBox ID="cbprintedit" runat="server" Text="Print Edit Option Lock" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="medium" />
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="width: 287px; height: 55px;">
                                    <legend>Attendance For InActive Student</legend>
                                    <asp:CheckBox ID="chkdiscon" runat="server" Text="Include Discontinue" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="medium" />
                                    <br />
                                    <asp:CheckBox ID="chkdebar" runat="server" Text="Include Debar" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="medium" />
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="width: 287px; height: 55px;">
                                    <legend>Attendance Calculation</legend>
                                    <asp:CheckBox ID="chkattday" runat="server" Text="Attendance Percentage  Calculation Based on Day"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="medium" />
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="width: 190px; height: 55px;">
                                    <legend>Attendance With Out Time Table</legend>
                                    <asp:CheckBox ID="chkwottable" runat="server" Text="With Out Time Table" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="medium" />
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="width: 287px; height: 55px;">
                                    <legend>All Student Attendance</legend>
                                    <asp:CheckBox ID="cb_studentattendance" runat="server" Text="With In Name" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="medium" />
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="width: 280px; height: 55px;">
                                    <legend>Attendance Letter Web Site</legend>
                                    <asp:Label ID="Label41" runat="server" Text="Web Site Address" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:TextBox ID="txtatwebaddress" runat="server" Width="161px" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:TextBox>
                                </fieldset>
                            </td>
                            <td colspan="2" rowspan="4">
                                <fieldset style="width: 213px; height: 351px;">
                                    <legend>Attendance Lock</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label43" runat="server" Text="Lock Days" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                                <asp:TextBox ID="txtattlock" runat="server" Width="80px" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" TargetControlID="txtattlock"
                                                    FilterType="Numbers">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cbbatlock" runat="server" Text="Lock With Batch Year" OnCheckedChanged="cbbatlock_OnCheckedIndexChange" AutoPostBack="true" /></td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <center>
                                                    <asp:GridView ID="gridviewbatch" runat="server" Style="margin-bottom: 15px; margin-top: 15px; width: 135px;"
                                                        Font-Names="Times New Roman" AutoGenerateColumns="false" BackColor="#F0F8FF" Font-Bold="true">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Batch">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblbatch" runat="server" Text='<%# Eval("batch") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Lock Days">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblbatlock" runat="server" Text='<%#Eval("lockdays") %>' Visible="false"></asp:Label>
                                                                    <asp:TextBox ID="txtbatlock" runat="server" Text='<%#Eval("lockdays") %>' Width="80px" Font-Names="Book Antiqua"
                                                                        Font-Size="Medium" BackColor="AliceBlue" Font-Bold="true"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" TargetControlID="txtbatlock"
                                                                        FilterType="Numbers">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                                    </asp:GridView>
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="width: auto; height: 55px;">
                                    <legend>Subject Based on Department Rights</legend>
                                    <asp:CheckBox ID="chksubatt" runat="server" Text="Subject Based on Department Rights"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="medium" Style="margin: 0px; padding: 0px;" />
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <%--Added By Jeyaprakash on Jan 12th,2017--%>
                                <fieldset style="width: 280px; height: 55px;">
                                    <legend>Combined Class Alert Rights</legend>
                                    <asp:CheckBox ID="cblCombinedClsAlrt" runat="server" Checked="false" Text="Time Table Combined Class Alert Rights"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="medium" />
                                </fieldset>
                            </td>
                            <td colspan="4"></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="width: 350px; height: auto;">
                                    <legend>Staff Attendance Rights</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkPresentAbsent" runat="server" Text="Require Present/Absent Only"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="medium" Style="margin: 0px; padding: 0px;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkNeedReson" runat="server" Text="Require Reason in the Staff Attendance"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="medium" Style="margin: 0px; padding: 0px;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkAttendanceCount" runat="server" Text="Require Student's Attendance Count"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="medium" Style="margin: 0px; padding: 0px;" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <td colspan="4">
                                <fieldset style="width: 350px; height: auto;">
                                    <legend>OD Entry Settings</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cbOdEnteryCheck" runat="server" Text="Max No of OD Per Students"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="medium" Style="margin: 0px; padding: 0px;" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cbodlimitexceeds" runat="server" Text="Alert For OD Limit Exceeds"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="medium" Style="margin: 0px; padding: 0px;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtODCount" runat="server" Font-Bold="True" Width="100px" Font-Names="Book Antiqua"
                                                    MaxLength="4" Font-Size="medium"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="filterODCheck" runat="server" TargetControlID="txtODCount"
                                                    FilterType="Custom,Numbers" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <fieldset style="height: 80px; width: 200px;">
                                    <legend style="height: 5">Attendance Hour</legend>
                                    <asp:Label ID="lbl_att_hour" runat="server" Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"
                                        Text="Hour"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:TextBox ID="txt_attn_hour" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                        ReadOnly="true" Width="113px" Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"></asp:TextBox>
                                    <asp:Panel ID="pnl_attn_hour" runat="server" Width="110px" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_attn_hour" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnCheckedChanged="cb_attn_hour_CheckedChanged" Text="Select All"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cbl_attn_hour" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            OnSelectedIndexChanged="cbl_attn_hour_SelectedIndexChanged" Font-Bold="True"
                                            Font-Names="Book Antiqua">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender19" runat="server" Position="Bottom"
                                        TargetControlID="txt_attn_hour" PopupControlID="pnl_attn_hour">
                                    </asp:PopupControlExtender>
                                </fieldset>
                            </td>
                            <td>

                                <fieldset style="height: 80px; width: 200px;">
                                    <legend style="height: 5">Attendance Alert Message</legend>
                                    <asp:CheckBox ID="chk_AlertMsgForAttendance" runat="server" Text="Show Alert Message "
                                        Font-Bold="true" />
                                </fieldset>
                            </td>
                            <td>
                                <fieldset style="height: 136px; width: 400px;">
                                    <legend style="height: 5">CAM Course OutCome(CO) && Program OutCome(PO)</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblcriteria" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                                            ForeColor="Black" Font-Size="Medium" Text="Criteria"></asp:Label>
                                                        <asp:Button ID="btnreovecritreia" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                                            Font-Size="Medium" OnClick="btnreovecritreia_Click" Text="-" />
                                                        <asp:DropDownList ID="ddlcriteria" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddlcriteria_SelectedIndexChanged"
                                                            Width="190px">
                                                        </asp:DropDownList>
                                                        <asp:Button ID="btnnewcriteria" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                                            Font-Size="Medium" OnClick="btnnewcriteria_Click" Text="+" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>

                                                <asp:RadioButton ID="EntryFormate1" GroupName="SaveCoFormate" runat="server" Text="Formate 1" />
                                                <asp:RadioButton ID="EntryFormate2" GroupName="SaveCoFormate" runat="server" Text="Formate 2" />
                                                <asp:Button ID="btnSaveCoFormate" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                                    Font-Size="Medium" OnClick="btnSaveCoFormate_Click" Text="Save" />
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblknow" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                                            ForeColor="Black" Font-Size="Medium" Text="Knowledge"></asp:Label>
                                                        <asp:Button ID="btn_addkno" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                                            Font-Size="Medium" OnClick="btnreoveknowledge_Click" Text="-" />
                                                        <asp:DropDownList ID="ddl_knowledge" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            Font-Bold="true" AutoPostBack="true"
                                                            Width="190px">
                                                        </asp:DropDownList>
                                                        <asp:Button ID="btn_subkno" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                                            Font-Size="Medium" OnClick="btnnewknowledge_Click" Text="+" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblpo" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                                            ForeColor="Black" Font-Size="Medium" Text="PO-Settings"></asp:Label>
                                                        <asp:Button ID="btnsubpo" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                                            Font-Size="Medium" OnClick="btnsubpo_Click" Text="-" />
                                                        <asp:DropDownList ID="ddlposettings" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            Font-Bold="true" AutoPostBack="true"
                                                            Width="190px">
                                                        </asp:DropDownList>
                                                        <asp:Button ID="btnaddpo" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                                            Font-Size="Medium" OnClick="btnaddpo_Click" Text="+" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>

                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>

            <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="PCriteria" runat="server" BorderColor="Black" BackColor="AliceBlue"
                        Visible="false" BorderWidth="2px" Width="700px">
                        <div class="panelinfraction" id="Div2" style="text-align: center; font-family: MS Sans Serif; font-size: Small; font-weight: bold">
                            <br />
                            <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute; left: 200px">
                                CO Entry
                            </caption>
                            <br />
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblcriteriaadd" runat="server" Text="Criteria" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtcriteria" runat="server" Width="600px" Height="30px" TextMode="MultiLine"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btncriteraiadd" runat="server" Text="Add" OnClick="btncriteraiadd_Click"
                                            Style="left: 420px; position: absolute; height: 26px; width: 88px" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btncriteraiexit" runat="server" Text="Exit" Style="left: 510px; position: absolute; height: 26px; width: 88px"
                                            OnClick="btncriteraiexit_Click" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <asp:Label ID="lblerrcritiria" runat="server" Text="" ForeColor="Red" Font-Bold="true"
                                Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlknowledge" runat="server" BorderColor="Black" BackColor="AliceBlue"
                        Visible="false" BorderWidth="2px" Width="700px">
                        <div class="panelinfraction" id="Div4" style="text-align: center; font-family: MS Sans Serif; font-size: Small; font-weight: bold">
                            <br />
                            <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute; left: 200px">
                                Knowledge Entry
                            </caption>
                            <br />
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblknowled" runat="server" Text="Knowledge" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_knoled" runat="server" Width="600px" Height="30px" TextMode="MultiLine"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btn_knoled" runat="server" Text="Add" OnClick="btn_knoled_Click"
                                            Style="left: 420px; position: absolute; height: 26px; width: 88px" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnex_knoled" runat="server" Text="Exit" Style="left: 510px; position: absolute; height: 26px; width: 88px"
                                            OnClick="btnex_knoled_Click" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <asp:Label ID="lbl_knowled" runat="server" Text="" ForeColor="Red" Font-Bold="true"
                                Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pannel2234" runat="server" BorderColor="Black" BackColor="AliceBlue"
                        Visible="false" BorderWidth="2px" Width="700px">
                        <div class="panelinfraction" id="Div5" style="text-align: center; font-family: MS Sans Serif; font-size: Small; font-weight: bold">

                            <br />
                            <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute; left: 200px">
                                PO Entry
                            </caption>
                            <br />
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblpoentry" runat="server" Text="Program OutCome" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtpoentry" runat="server" Width="600px" Height="30px" TextMode="MultiLine"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnposave" runat="server" Text="Add" OnClick="btnposave_Click"
                                            Style="left: 420px; position: absolute; height: 26px; width: 88px" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnpoexit" runat="server" Text="Exit" Style="left: 510px; position: absolute; height: 26px; width: 88px"
                                            OnClick="btnpoexit_Click" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <asp:Label ID="lblpoalert" runat="server" Text="" ForeColor="Red" Font-Bold="true"
                                Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>


            <%--Transport Remainder Tab--%>
            <center>
                <div id="div7TransRemind" runat="server" style="width: 980px; height: 400px; background: white; border: 1px solid #0095E8; overflow: auto;"
                    visible="false">
                    <table>
                        <table>
                            <tr>
                                <td style="height: 10px;"></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label16" runat="server" Text="Days Before Reminder" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_remainder" runat="server" MaxLength="2"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_remainder"
                                        FilterType="Custom,Numbers" ValidChars="/">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10px;"></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label17" runat="server" Text="Authorised Person" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_stfone" runat="server"></asp:TextBox>
                                    <asp:Button ID="Btn_stfone" runat="server" Text="?" OnClick="Btn_stfone_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10px;"></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label18" runat="server" Text="Alternative Authorised Person" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_stftwo" runat="server"></asp:TextBox>
                                    <asp:Button ID="Btn_stftwo" runat="server" Text="?" OnClick="Btn_stftwo_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <fieldset style="height: 120px; width: 350px;">
                                        <legend>TransPort Fees AllotMent Setting</legend>
                                        <table>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:CheckBox ID="cbtransfees" runat="server" Text="Include Transport Fees" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <%-- <asp:RadioButton ID="rdo_transsem" runat="server" Text="Semester" GroupName="trans"
                                                            AutoPostBack="true" OnCheckedChanged="rdo_transsem_CheckedChanged" />
                                                        <asp:RadioButton ID="rdo_transyear" runat="server" Text="Yearly" GroupName="trans"
                                                            AutoPostBack="true" OnCheckedChanged="rdo_transyear_CheckedChanged" />
                                                        <asp:RadioButton ID="rb_transterm" runat="server" Text="Term" GroupName="trans" AutoPostBack="true"
                                                            OnCheckedChanged="rb_transterm_CheckedChanged" />
                                                        <asp:RadioButton ID="rdo_transmoth" runat="server" Text="Month" GroupName="trans"
                                                            AutoPostBack="true" OnCheckedChanged="rdo_transmoth_CheckedChanged" />--%>


                                                    <asp:CheckBox ID="Cb_transsem" runat="server" Text="Semester"
                                                        AutoPostBack="true" OnCheckedChanged="Cb_transsem_CheckedChanged" />
                                                    <asp:CheckBox ID="Cb_transyear" runat="server" Text="Yearly"
                                                        AutoPostBack="true" OnCheckedChanged="Cb_transyear_CheckedChanged" />
                                                    <asp:CheckBox ID="Cb_transterm" runat="server" Text="Term" AutoPostBack="true"
                                                        OnCheckedChanged="Cb_transterm_CheckedChanged" />
                                                    <asp:CheckBox ID="Cb_transmoth" runat="server" Text="Month"
                                                        AutoPostBack="true" OnCheckedChanged="Cb_transmoth_CheckedChanged" />


                                                    <%--<asp:CheckBoxList ID="cbl_tranfeetype" runat="server" AutoPostBack="true" OnCheckedChanged="cbl_tranfeetype_CheckedChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="0">Semester</asp:ListItem>
                                                    <asp:ListItem Value="1">Yearly</asp:ListItem>
                                                    <asp:ListItem Value="2">Term</asp:ListItem>
                                                    <asp:ListItem Value="3">Month</asp:ListItem>
                                                </asp:CheckBoxList>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table>
                                                        <tr>
                                                            <td colspan="3">
                                                                <asp:Label ID="Label44" runat="server" Text="Month"></asp:Label>
                                                                <td>
                                                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                                        <ContentTemplate>
                                                                            <%--   <div style="position: relative;">--%>
                                                                            <asp:TextBox ID="txt_monthtrans" runat="server" Style="height: 20px; width: 69px;"
                                                                                ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                                                            <%--CssClass="textbox textbox1 txtheight"--%>
                                                                            <asp:Panel ID="Panel10" runat="server" CssClass="multxtpanel" Width="180px" Height="250px">
                                                                                <%--Style="position: absolute;"--%>
                                                                                <asp:CheckBox ID="cb_monthtrans" runat="server" Text="Select All" AutoPostBack="true"
                                                                                    OnCheckedChanged="cb_monthtrans_checkedchange" />
                                                                                <asp:CheckBoxList ID="cbl_monthtrans" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_monthtrans_SelectedIndexChanged">
                                                                                </asp:CheckBoxList>
                                                                            </asp:Panel>
                                                                            <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txt_monthtrans"
                                                                                PopupControlID="Panel10" Position="Bottom">
                                                                            </asp:PopupControlExtender>
                                                                            <%-- </div>--%>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lbl_transyr" runat="server" Text="Year"></asp:Label>
                                                                <asp:DropDownList ID="ddl_transyear" Enabled="false" runat="server" CssClass="ddlheight textbox textbox1">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                        </tr>
                    </table>
                </div>
            </center>
            <center>
                <asp:Panel ID="panel8" runat="server" BorderColor="Black" BackColor="AliceBlue" BorderWidth="2px"
                    Visible="false" Style="background-color: AliceBlue; border-color: Black; border-width: 2px; border-style: solid; width: 900px; height: 440px; top: 108px; position: absolute;">
                    <div class="PopupHeaderrstud2" id="Div1" style="text-align: center; font-family: MS Sans Serif; font-size: Small; font-weight: bold">
                        <br />
                        <asp:Label ID="Label19" runat="server" Text=" Staff List" Style="width: 150px;"></asp:Label>
                        <br />
                        <br />
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblcollege" runat="server" Text="College" Style="width: 150px;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_college" runat="server" Width="150px" Style="width: 150px;">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDepartment" runat="server" Text="Department" Style="width: 150px;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddldepratstaff" runat="server" AutoPostBack="true" Width="150px"
                                                OnSelectedIndexChanged="ddldepratstaff_SelectedIndexChanged" Style="width: 150px;">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label20" runat="server" Text="Staff Type" Style="width: 150px;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_stftype" runat="server" Width="150px" OnSelectedIndexChanged="ddl_stftype_SelectedIndexChanged"
                                                AutoPostBack="true" Style="width: 150px;">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label21" runat="server" Text="Designation" Style="width: 150px;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_design" runat="server" Width="150px" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddl_design_SelectedIndexChanged" Style="width: 150px;">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblsearchby" runat="server" Text="Staff By"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlstaff" runat="server" Width="150px" OnSelectedIndexChanged="ddlstaff_SelectedIndexChanged"
                                                AutoPostBack="true">
                                                <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                                <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_search" runat="server" OnTextChanged="txt_search_TextChanged"
                                                AutoPostBack="True"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <center>
                                                <div style="width: 510px;">
                                                    <farpoint:fpspread id="fsstaff" runat="server" activesheetviewindex="0" height="300"
                                                        width="510" verticalscrollbarpolicy="AsNeeded" borderwidth="0.5" visible="False"
                                                        onupdatecommand="fsstaff_UpdateCommand" oncellclick="fsstaff_CellClick">
                                                        <CommandBar BackColor="Control" ButtonType="PushButton">
                                                            <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                                                        </CommandBar>
                                                        <Sheets>
                                                            <FarPoint:SheetView SheetName="Sheet1">
                                                            </FarPoint:SheetView>
                                                        </Sheets>
                                                    </farpoint:fpspread>
                                                </div>
                                            </center>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <center>
                                                <fieldset style="width: 160px; height: 9px; top: 388px;">
                                                    <asp:Button runat="server" ID="btnstaffadd" OnClick="btnstaffadd_Click" Width="75px" />
                                                    <asp:Button runat="server" ID="btnexitpop" Text="Exit" OnClick="exitpop_Click" Width="75px" />
                                                </fieldset>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                </asp:Panel>
            </center>
            <%--SMS Template Tab--%>
            <center>
                <div id="div8SMS" runat="server" style="width: 980px; height: 400px; background: white; border: 1px solid #0095E8; overflow: auto;"
                    visible="false">
                    <table>
                        <tr>
                            <td colspan="6">
                                <asp:Label ID="lablmainpurpose" runat="server" Text="Purpose" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:DropDownList ID="ddlmainpurpose" runat="server" Width="150px" Font-Bold="true"
                                    AutoPostBack="true" Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlmainpurpose_SelectedIndexChanged">
                                </asp:DropDownList>
                                <br />
                                <br />
                                <farpoint:fpspread id="FpSpreadsms" runat="server" bordercolor="Black" borderstyle="Solid"
                                    borderwidth="1px" verticalscrollbarpolicy="Always" horizontalscrollbarpolicy="Always">
                                    <CommandBar BackColor="White" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                        ButtonShadowColor="ControlDark" Visible="true">
                                    </CommandBar>
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </farpoint:fpspread>
                                <br />
                                <asp:Button ID="Add_Templete" runat="server" Text="Add Template" Font-Bold="true"
                                    Font-Size="Medium" Font-Names="Book Antiqua" OnClick="Add_Templete_Click" />
                                <asp:Button ID="Delete_Templete" runat="server" Text="Delete Template" Font-Bold="true"
                                    Font-Size="Medium" Font-Names="Book Antiqua" OnClick="Delete_Templete_Click" />
                                <br />
                                <center>
                                    <div id="panelsms" runat="server" visible="false" style="height: 70em; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0; left: 0;">
                                        <asp:Panel ID="myPnlSMS" runat="server" BorderColor="Black" BackColor="AliceBlue"
                                            BorderWidth="2px" Style="background-color: AliceBlue; border-color: Black; border-width: 2px; border-style: solid; margin-top: 200px; width: 520px; height: 290px;">
                                            <br />
                                            &nbsp;&nbsp;
                                            <asp:Label ID="lblpurposesms" runat="server" Text="Purpose" Font-Bold="true" Font-Size="Medium"
                                                Font-Names="Book Antiqua"> </asp:Label>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:DropDownList ID="purposeddl" runat="server" AutoPostBack="true" OnSelectedIndexChanged="purpose_IndexChanged"
                                                Font-Bold="true" Font-Size="Medium" Width="150px" Font-Names="Book Antiqua">
                                                <asp:ListItem>---Select---</asp:ListItem>
                                                <asp:ListItem>Attendance</asp:ListItem>
                                                <asp:ListItem>CAM</asp:ListItem>
                                                <asp:ListItem>Finance</asp:ListItem>
                                                <asp:ListItem>University Result</asp:ListItem>
                                                <asp:ListItem>Attendance Cummulative</asp:ListItem>
                                            </asp:DropDownList>
                                            <br />
                                            <br />
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="panelsubtype" runat="server" Height="165px" Width="200px" BorderColor="Black"
                                                            BorderWidth="1px">
                                                            <asp:ListBox ID="list1" runat="server" OnSelectedIndexChanged="list1_OnSelectedIndexChanged"
                                                                Height="165px" Width="200px">
                                                                <%-- <asp:ListItem>Your Text</asp:ListItem>
                                                                <asp:ListItem>College Name</asp:ListItem>
                                                                <asp:ListItem>Student Name</asp:ListItem>
                                                                <asp:ListItem>Roll No</asp:ListItem>
                                                                <asp:ListItem>Register No</asp:ListItem>
                                                                <asp:ListItem>Application No</asp:ListItem>
                                                                <asp:ListItem>Admission No</asp:ListItem>
                                                                <asp:ListItem>Degree</asp:ListItem>
                                                                <asp:ListItem>Section</asp:ListItem>
                                                                <asp:ListItem>Date</asp:ListItem>
                                                                <asp:ListItem>Absent</asp:ListItem>
                                                                <asp:ListItem>Conducted Hours</asp:ListItem>
                                                                <asp:ListItem>Absent hours</asp:ListItem>
                                                                <asp:ListItem>Conducted Days</asp:ListItem>
                                                                <asp:ListItem>Absent Days</asp:ListItem>
                                                                <asp:ListItem>HOD</asp:ListItem>
                                                                <asp:ListItem>Thank You</asp:ListItem>--%>
                                                            </asp:ListBox>
                                                        </asp:Panel>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Panel ID="actionpanel" runat="server" Height="100px" Width="100px">
                                                            <asp:Button ID="btnSingleAdd" runat="server" Text=">" Font-Bold="true" Font-Size="Medium"
                                                                Width="40px" Font-Names="Book Antiqua" OnClick="btnSingleAdd_Click" />
                                                            <br />
                                                            <asp:Button ID="btnMultipleAdd" runat="server" Text=">>" Font-Bold="true" Font-Size="Medium"
                                                                Width="40px" Font-Names="Book Antiqua" OnClick="btnMultipleAdd_Click" />
                                                            <br />
                                                            <asp:Button ID="btnSingleRemove" runat="server" Text="<" Font-Bold="true" Font-Size="Medium"
                                                                Width="40px" Font-Names="Book Antiqua" OnClick="btnSingleRemove_Click" />
                                                            <br />
                                                            <asp:Button ID="btnMulitpleRemove" runat="server" Text="<<" Font-Bold="true" Font-Size="Medium"
                                                                Width="40px" Font-Names="Book Antiqua" OnClick="btnMulitpleRemove_Click" />
                                                        </asp:Panel>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="templetepanel" runat="server" Height="165px" Width="200px" BorderColor="Black"
                                                            BorderWidth="1px">
                                                            <asp:ListBox ID="list2" runat="server" Height="165px" Width="200px"></asp:ListBox>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" align="right">
                                                        <asp:Button ID="btnsavetemplete" runat="server" Text="Save" Font-Bold="true" Font-Size="Medium"
                                                            Width="50px" Font-Names="Book Antiqua" OnClick="btnsavetemplete_Click" />
                                                        <asp:Button ID="btnexittemplete" runat="server" Text="Exit" Font-Bold="true" Font-Size="Medium"
                                                            Width="50px" Font-Names="Book Antiqua" OnClick="btnexittemplete_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                        </asp:Panel>
                                    </div>
                                </center>
                                <asp:Panel ID="paneledit" runat="server" Visible="false" Width="500px" BorderWidth="1px"
                                    BorderColor="LightBlue" Style="align: Center;">
                                    <asp:TextBox ID="txt_edittextbox" runat="server" Width="300px" TextMode="MultiLine"></asp:TextBox>
                                    <br />
                                    <br />
                                    <asp:Button ID="btn_save_text" runat="server" Width="40px" Text="Save" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btn_Cancel_text" runat="server" Width="40px" Text="Cancel" />
                                </asp:Panel>
                                <div style="width: 900px; height: auto; font-family: Book Antiqua; font-size: medium; font-weight: bold;">
                                    <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; width: 850px; height: auto;">Type Mobile Numbers Seperated By Commas(For Ex:9005584555,7585688585)</span><br />
                                    <asp:TextBox ID="txt_CopySmsMobNo" runat="server" Rows="7" CssClass="noresize" TextMode="MultiLine"
                                        Width="850px"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="filterExt_txtSms" runat="server" TargetControlID="txt_CopySmsMobNo"
                                        FilterType="Numbers,custom" ValidChars=",">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Label ID="spnSmsError" runat="server" class="errorsms" Visible="true" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold; position: relative; width: 80%; height: auto; color: Red;"></asp:Label>
                                </div>
                                <div id="divEmailCopy" style="width: 900px; height: auto;">
                                    <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; width: 850px; height: auto;">Type Email Id Seperated By Commas(For Ex:abc@gmail.com,ams@ymail.com)</span><br />
                                    <asp:TextBox ID="txt_CopyEmailid" runat="server" Rows="7" TextMode="MultiLine" CssClass="noresize"
                                        Width="850px"></asp:TextBox>
                                    <asp:Label ID="spnEmailError" runat="server" class="errormail" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold; height: auto; width: 80%; position: relative; color: Red;"></asp:Label>
                                </div>
                                <div id="compliaint" runat="server">
                                    <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; width: 850px; height: auto;">Grievance From Mail</span><br />
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label67" runat="server" Text="From Mail"></asp:Label>
                                                <asp:TextBox ID="txtmail" runat="server" CssClass="noresize"></asp:TextBox></td>
                                            <td>
                                                <asp:Label ID="Label68" runat="server" Text="Password"></asp:Label>
                                                <asp:TextBox ID="Txtpass" runat="server" CssClass="noresize"></asp:TextBox></td>
                                        </tr>
                                    </table>

                                    <center>
                                        <%-- <asp:Button ID="btnsend" OnClick="btnsend_Click" BackColor="#84ff00" Visible="false" Text="Send" runat="server" Style="font-family: Book Antiqua;
                font-size: medium; font-weight: bold;" />--%>
                                    </center>

                                </div>
                                <div id="Div3" runat="server">
                                    <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; width: 850px; height: auto;">Grievance To Mail</span><br />
                                    <asp:TextBox ID="txttomail" runat="server" Rows="7" TextMode="MultiLine" CssClass="noresize"
                                        Width="850px"></asp:TextBox>
                                    <center>
                                        <%-- <asp:Button ID="btnsend" OnClick="btnsend_Click" BackColor="#84ff00" Visible="false" Text="Send" runat="server" Style="font-family: Book Antiqua;
                font-size: medium; font-weight: bold;" />--%>
                                    </center>

                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
            <center>
                <div id="panel_show" runat="server" visible="false" style="height: 70em; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0; left: 0;">
                    <asp:Panel ID="myPanel" runat="server" BorderColor="Black" BackColor="AliceBlue"
                        BorderWidth="2px" Style="background-color: lightyellow; border-color: Black; border-width: 2px; border-style: solid; width: 900px; height: 110px; z-index: 1000; margin-top: 258px;">
                        <table>
                            <tr align="center">
                                <td>
                                    <span style="font-size: medium; font-family: Book Antiqua; font-weight: bold;">Edit
                                        Template</span>
                                </td>
                            </tr>
                            <tr align="center">
                                <td>
                                    <asp:TextBox ID="txt_temple_value" runat="server" Width="508px" Height="35px" TextMode="MultiLine"
                                        Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"></asp:TextBox>
                                </td>
                            </tr>
                            <tr align="center">
                                <td>
                                    <asp:Button ID="btnsavetemple" runat="server" Text="Add" Font-Bold="true" Font-Size="Medium"
                                        Font-Names="Book Antiqua" OnClick="btnsavetemple_Click" />
                                    <asp:Button ID="btncanceltemple" runat="server" Text="Exit" Font-Bold="true" Font-Size="Medium"
                                        Font-Names="Book Antiqua" OnClick="btncanceltemple_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </center>
            <%--Admision Process Tab--%>
            <center>
                <div id="div9Admission" runat="server" style="width: 980px; height: 400px; background: white; border: 1px solid #0095E8; overflow: auto;"
                    visible="false">
                    <table border="1" style="font-family: Book Antiqua; font-size: medium; border: 1px solid black;">
                        <tr>
                            <td colspan="6">
                                <center>
                                    <br />
                                    <table border="1" rules="none" style="font-family: Book Antiqua; font-size: medium; border: 1px solid black;">
                                        <tr>
                                            <td colspan="7">
                                                <span style="font-weight: bold; font-size: large;">Online Application Apply Settings</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Select College Type</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddltypecollege" Font-Bold="true" Font-Size="Medium" runat="server"
                                                    AutoPostBack="true" Font-Names="book Antiqua" Width="100px" OnSelectedIndexChanged="ddltype_college_Changed">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <span>Select Education Level</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddleduction_level" runat="server" Font-Bold="true" Font-Size="Medium"
                                                    Font-Names="book Antiqua" Width="100px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <span>From</span>
                                                <asp:TextBox ID="fromdate" runat="server" Font-Bold="true" Font-Size="Medium" Width="100px"
                                                    Font-Names="book Antiqua" onchange="return datefrom(this.value)"></asp:TextBox>
                                                <asp:CalendarExtender ID="calender1" runat="server" TargetControlID="fromdate" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <span>To</span>
                                                <asp:TextBox ID="todate" runat="server" Font-Bold="true" Font-Size="Medium" Width="100px"
                                                    Font-Names="book Antiqua" onchange="return datefunction(this.value)"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="todate"
                                                    Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:Button ID="btntypesave" runat="server" Text="Save" Font-Bold="true" Font-Size="Medium"
                                                    Font-Names="book Antiqua" OnClick="btntypesave_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <center>
                                    <br />
                                    <table border="1" rules="none" style="font-family: Book Antiqua; font-size: medium; border: 1px solid black;">
                                        <tr>
                                            <td colspan="7">
                                                <span style="font-weight: bold; font-size: large;">Application Code Generate Settings</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Select College Type</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlapplicationcode_colltype" runat="server" AutoPostBack="true"
                                                    Font-Bold="true" Font-Size="Medium" Font-Names="book Antiqua" Width="100px" OnSelectedIndexChanged="ddlapplicationcode_colltype_college_Changed">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <span>Select Education Level</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlapplicationcode_edulevel" runat="server" Font-Bold="true"
                                                    Font-Size="Medium" Font-Names="book Antiqua" Width="100px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <span>From</span>
                                                <asp:TextBox ID="fromdatecode" runat="server" Font-Bold="true" Font-Size="Medium"
                                                    Width="100px" Font-Names="book Antiqua" onchange="return datefrom1(this.value)"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="fromdatecode"
                                                    Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <span>To</span>
                                                <asp:TextBox ID="todatecode" runat="server" Font-Bold="true" Font-Size="Medium" Width="100px"
                                                    Font-Names="book Antiqua" onchange="return dateto1(this.value)"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="todatecode"
                                                    Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:Button ID="btncodegensave" runat="server" Text="Save" Font-Bold="true" Font-Size="Medium"
                                                    Font-Names="book Antiqua" OnClick="btncodegensave_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <center>
                                    <br />
                                    <table border="1" rules="none" style="font-family: Book Antiqua; font-size: medium; border: 1px solid black;">
                                        <tr>
                                            <td colspan="7">
                                                <span style="font-weight: bold; font-size: large;">Application Number Generate Settings</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Select College Type</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlapplicatonno_colltype" runat="server" AutoPostBack="true"
                                                    Font-Bold="true" Font-Size="Medium" Font-Names="book Antiqua" Width="100px" OnSelectedIndexChanged="ddlapplicatonno_colltype_changed">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <span>Select Education Level</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlapplicatonno_edulevel" runat="server" Font-Bold="true" Font-Size="Medium"
                                                    Font-Names="book Antiqua" Width="100px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <span>From</span>
                                                <asp:TextBox ID="fromdateapplication" runat="server" Font-Bold="true" Font-Size="Medium"
                                                    Width="100px" Font-Names="book Antiqua" onchange="return datefrom2(this.value)"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender6" runat="server" TargetControlID="fromdateapplication"
                                                    Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <span>To</span>
                                                <asp:TextBox ID="todateapplication" runat="server" Font-Bold="true" Font-Size="Medium"
                                                    Width="100px" Font-Names="book Antiqua" onchange="return dateto2(this.value)"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender7" runat="server" TargetControlID="todateapplication"
                                                    Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnapplnosave" runat="server" Text="Save" Font-Bold="true" Font-Size="Medium"
                                                    Font-Names="book Antiqua" OnClick="btnapplnosave_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" rowspan="3">
                                <center>
                                    <span style="font-weight: bold; font-size: large;">Selection Criteria Settings</span>
                                    <br />
                                    <asp:CheckBoxList ID="chcklist" runat="server" Font-Names="Book Antiqua" Font-Size="Medium">
                                        <asp:ListItem Value="0">Applied</asp:ListItem>
                                        <asp:ListItem Value="1">ShortList/Counselling</asp:ListItem>
                                        <asp:ListItem Value="2">Recommanded for Admission</asp:ListItem>
                                        <asp:ListItem Value="3">Admitted</asp:ListItem>
                                    </asp:CheckBoxList>
                                </center>
                            </td>
                            <td colspan="2">
                                <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Application
                                    Confirmation Details</span>
                                <br />
                                <asp:CheckBox ID="cbconfirm" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Text="Confirmation Details" />
                            </td>
                            <td colspan="2">
                                <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Admission
                                    Confirmation</span>
                                <br />
                                <asp:RadioButtonList ID="rblAdmConf" runat="server" RepeatDirection="Horizontal"
                                    OnSelectedIndexChanged="rblAdmConf_Selected" AutoPostBack="true">
                                    <asp:ListItem Selected="True">Admit</asp:ListItem>
                                    <asp:ListItem>Wait To Admit</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <td colspan="2" rowspan="2">
                            <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Application
                                And Admission No Rights</span>
                            <br />
                            <asp:CheckBox ID="chkappno" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Text="Application No Rights" />
                            <br />
                            <asp:CheckBox ID="chkadmino" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Text="Admission No Rights" />
                            <br />
                            <asp:CheckBox ID="chkappacr" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Text="Add Academic year" />
                        </td>
                        <td colspan="2">
                            <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Show Fee
                                Structure</span><br />
                            <asp:CheckBox ID="cbAdmFeeStuct" runat="server" Text="Show Fees" />
                        </td>
                        <tr>
                            <td colspan="2">
                                <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Order
                                    By</span>
                                <br />
                                <asp:CheckBox ID="orderbymarks" runat="server" Text="Percentage" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" rowspan="3">
                                <center>
                                    <span style="font-weight: bold; font-size: large;">Selection Enrollment Settings</span>
                                    <br />
                                    <asp:CheckBoxList ID="cblenroll" runat="server" Font-Names="Book Antiqua" Font-Size="Medium">
                                        <asp:ListItem Value="0">Enrollment Selection</asp:ListItem>
                                        <asp:ListItem Value="1">Enrollment Confirm</asp:ListItem>
                                        <asp:ListItem Value="2">Enrollment Setting</asp:ListItem>
                                    </asp:CheckBoxList>
                                </center>
                            </td>
                            <td colspan="2">
                                <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Application
                                    Format Setting</span>
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdb_formate1" runat="server" Text="Formate 1" GroupName="ffff" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdb_formate2" runat="server" Text="Formate 2" GroupName="ffff" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Register
                                    Student On Confirm</span>
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cbMovetoReg" runat="server" Text="Register" Checked="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Call Letter
                                    Format Setting</span>
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlCallLetter" runat="server" Width="100px">
                                                <asp:ListItem Selected="True" Value="0">Format1</asp:ListItem>
                                                <asp:ListItem Value="1">Format2</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2" rowspan="2">
                                <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Academic
                                    Details Setting</span>
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cb_acadamicdetails" runat="server" Text="Academic
                            Details" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cb_Equalnthsc" runat="server" Text="Equivalent To HSC" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cb_cutofmark" runat="server" Text="Cut Of Mark" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Application
                                    Pdf Format Setting</span>
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdb_pdfFormat1" runat="server" Text="Formate 1" GroupName="vv" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Fee Allotment</span>
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cb_generalfee" runat="server" Text="General Fee Allot" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cb_hostelfee" runat="server" Text="Hostel Fee Allot" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cb_transport" runat="server" Text="Transport Fee Allot" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cbIncFinLink" runat="server" Text="Include Finance Link" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cbapplfee" runat="server" Text="Include Application Fees" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Student
                                    Admission Register</span>
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdb_staticreg" runat="server" Text="Static Asmission Register"
                                                GroupName="reg" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdb_dynamicreg" runat="server" Text="Dynamic Asmission Register"
                                                GroupName="reg" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Student
                                    Application SMS</span>
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkAdmSms" runat="server" Text="Send SMS after application saved" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border: 1px solid black;">
                                            <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Fees Structure</span>
                                            <br />
                                            <asp:RadioButtonList ID="rbfeesstr" Enabled="false" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="True" Value="0" Text="Challan"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Receipt"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Collegewise
                                    Rights</span>
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <fieldset style="height: 45px; width: 233px;">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="cbclgappl" runat="server" Text="CollegeWise Application " />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="cbclgadm" runat="server" Text="CollegeWise Admission " />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                            <fieldset style="height: 45px; width: 233px;">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="cb_edulevelappl" runat="server" Text="Education Level Application " />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="cb_eduleveladm" runat="server" Text="Education Level Admission " />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                            <fieldset style="height: 75px; width: 233px;">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="cb_DegSeatModeApp" runat="server" Text="Degreewise Seatwise Application " />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="cb_DegSeatModeAdm" runat="server" Text="Degreewise Seatwise Admission " />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Admission
                                    Fee Editable</span>
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cbeditable" runat="server" Text="Include Editable Option" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Fee Paid
                                                Confirmation</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cbl_Feepaidconfirmation" runat="server" Text="Fee Paid Confirmation" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%--Paavai new Application Number settings Barath 10.01.18--%>
                                            <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Application
                                                Number Settings</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cbNewAppNumber" runat="server" Text="Common Application Number" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Multiple
                                    Term </span>
                                <br />
                                <table>
                                    <tr>
                                        <td colspan="2">
                                            <asp:CheckBox ID="cbincmulterm" runat="server" Text="Include Multiple Term" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblmulterm" runat="server" Text="Semester"></asp:Label>
                                            <asp:Label ID="lbl_sem" Visible="false" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td>
                                            <fieldset style="height: auto; width: 150px;">
                                                <%-- <asp:UpdatePanel ID="UpdatePanel1mul1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtmulsem" runat="server" Height="15px" 
                                                ReadOnly="true">--Select--</asp:TextBox>
                                                <%--CssClass="textbox  txtheight2"
                                            <asp:Panel ID="pnlmulsem" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                height: 150px;">--%>
                                                <%-- <asp:CheckBox ID="cbmulterm" Visible="false" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cbmulterm_CheckedChanged" />--%>
                                                <asp:CheckBoxList ID="cblmulterm" runat="server">
                                                </asp:CheckBoxList>
                                                <%--AutoPostBack="true" OnSelectedIndexChanged="cblmulterm_SelectedIndexChanged"--%>
                                            </fieldset>
                                            <%--</asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txtmulsem"
                                                PopupControlID="pnlmulsem" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                        </td>
                                    </tr>
                                    <%-- <tr>
                                <td colspan="2">
                                    <center>
                                        <asp:Button ID="btnsave_mulsem" runat="server" Text="save" Width="50px" OnClick="btnsave_mulsem_Click" />
                                    </center>
                                </td>
                            </tr>--%>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Online
                                    Transaction Details </span>
                                <br />
                                <table>
                                    <tr>
                                        <td>Merchant KEY
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMerchKey" runat="server" Width="100px" MaxLength="64"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>SALT
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMerchantSalt" runat="server" Width="100px" MaxLength="64"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <center>
                                                <asp:Button ID="btnSaveOnlineMerchant" runat="server" Text="save" Width="50px" OnClick="saveOnlineMerchantKeySalt" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Admission
                                    Number Generation On Receipt </span>
                                <br />
                                <table>
                                    <tr>
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkAdmGenOnRcpt" runat="server" Text="Generate" />
                                        </td>
                                        <td>Batch
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlAdmGenOnRcpt" runat="server" Width="60px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSaveAdmGenOnRcpt" runat="server" Text="save" Width="50px" OnClick="saveAdmGenOnRcpt" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Online
                                    Application Gender Settings </span>
                                <br />

                                <%-- <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                              
                                    <asp:TextBox ID="txtclg" runat="server" Style="height: 20px; width: 80px;" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlclg" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width:80px;
                                        height: 120px;">
                                        <asp:CheckBox ID="cbclg" runat="server" Width="80px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cbclg_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="cblclg" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblclg_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender20" runat="server" TargetControlID="txtclg"
                                        PopupControlID="pnlclg" Position="Bottom">
                                    </asp:PopupControlExtender>--%>
                                    College<asp:DropDownList ID="ddlcollegename" runat="server" Width="80px"
                                        AutoPostBack="true">
                                        <%--OnSelectedIndexChanged="ddl_collegename_selectedindexchange"--%>
                                    </asp:DropDownList>
                                Educational Level<asp:DropDownList ID="ddl_genderset" runat="server" Width="80px"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_genderEducation_selectedindexchange">
                                </asp:DropDownList>
                                <asp:CheckBoxList ID="rdb_genderset" runat="server" RepeatColumns="4">
                                    <asp:ListItem Value="0" Text="Male">
                                    </asp:ListItem>
                                    <asp:ListItem Value="1" Text="Female">
                                    </asp:ListItem>
                                    <asp:ListItem Value="2" Text="Transgender">
                                    </asp:ListItem>
                                    <asp:ListItem Value="3" Text="Both">
                                    </asp:ListItem>
                                </asp:CheckBoxList>
                                <asp:Button ID="btn_GendersetSave" runat="server" Text="Save" OnClick="btn_GendersetSave_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Online
                                    Application Print Pdf Format </span>
                                <br />
                                <table>
                                    <td colspan="5">
                                        <asp:RadioButtonList ID="rdo_appprintformate" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="0" Text="New">
                                            </asp:ListItem>
                                            <asp:ListItem Value="1" Text="PAT">
                                            </asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Application
                                    Process </span>
                                <br />
                                <table>
                                    <tr>
                                        <td colspan="5">
                                            <asp:CheckBox ID="cb_EnquiryRights" runat="server" Text="Show Enquiry" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="Radio_formate1" runat="server" Text="Formate 1" GroupName="ff" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="Radio_formate2" runat="server" Text="Formate 2" GroupName="ff" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="Radio_both" runat="server" Text="Both" GroupName="ff" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <fieldset style="height: 180px;">
                                    <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Subject
                                        Priority </span>
                                    <br />
                                    <table>
                                        <%--07.03.17 barath--%>
                                        <tr>
                                            <td>Vocational
                                                <asp:RadioButton ID="rbVocation" runat="server" Text="Yes" GroupName="vocRb" AutoPostBack="true"
                                                    OnCheckedChanged="rbVocation_Click" />
                                                <asp:RadioButton ID="rbVocation1" runat="server" Text="No" GroupName="vocRb" Checked="true"
                                                    AutoPostBack="true" OnCheckedChanged="rbVocation_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>Subject
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtsubj" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                                        <asp:Panel ID="pnlmem" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 126px; height: 250;">
                                                            <asp:CheckBox ID="cbsubj" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                OnCheckedChanged="cbsubj_OnCheckedChanged" />
                                                            <asp:CheckBoxList ID="cblsubj" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblsubj_OnSelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txtsubj"
                                                            PopupControlID="pnlmem" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <fieldset style="height: 50px;">
                                    <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Qualified
                                        examination register number check </span>
                                    <br />
                                    <asp:CheckBox ID="cbShowVerRegNo" runat="server" Text="Show" />
                                </fieldset>
                                <fieldset style="height: 50px;">
                                    <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Online
                                        fee payment Display</span>
                                    <br />
                                    <asp:RadioButton ID="chkDispGhorHead" runat="server" Text="GroupHeader wise" GroupName="group" />
                                    <asp:RadioButton ID="chkheaderwise" runat="server" Text="Header wise" GroupName="group" /><%--abarna--%>
                                    <asp:RadioButton ID="chkledgerwise" runat="server" Text="Ledger Wise" GroupName="group" /><%--abarna--%>
                                </fieldset>
                                <fieldset style="height: 50px;">
                                    <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Application
                                        View Format Setting</span>
                                    <br />
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdb_viewformat1" runat="server" Text="Format 1" GroupName="bb" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdb_viewformat2" runat="server" Text="Format 2" GroupName="bb" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
            <%--HR Settings Tab--%>
            <center>
                <div id="div10Hr" runat="server" style="width: 980px; height: 400px; background: white; border: 1px solid #0095E8; overflow: auto;"
                    visible="false">
                    <table>
                        <tr>
                            <td colspan="2" rowspan="2">
                                <fieldset style="height: 240px; width: 270px;">
                                    <legend>HR Dept Settings</legend>
                                    <table>
                                        <tr>
                                            <td>

                                                <asp:Label ID="lbl_college" runat="server" Text="College"></asp:Label><br />

                                                <div style="position: relative;">
                                                    <asp:TextBox ID="txt_college" runat="server" CssClass="textbox txtheight2" Style="width: 125px"
                                                        ReadOnly="true">--Select--</asp:TextBox>


                                                    <asp:Panel ID="pnlcollege" runat="server" CssClass="multxtpanel " Style="width: 125px; border-color: HighlightText; height: 150px;">
                                                        <asp:CheckBox ID="Cbcollege" runat="server" Width="204px" Text="Select All" AutoPostBack="true"
                                                            OnCheckedChanged="cbcollege_OnCheckedChanged" />
                                                        <asp:CheckBoxList ID="Cblcollege" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblcollege_OnSelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>


                                                </div>
                                            </td>

                                            <td>
                                                <asp:Label ID="lblhrdept" runat="server" Text="HR Department"></asp:Label><br />
                                                <div style="position: relative;">
                                                    <asp:TextBox ID="txthrdept" runat="server" CssClass="textbox txtheight2" Style="width: 250px"
                                                        ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="pnlhrdept" runat="server" CssClass="multxtpanel " Style="width: 255px; border-color: HighlightText; height: 150px;">
                                                        <asp:CheckBox ID="cbhrdept" runat="server" Width="204px" Text="Select All" AutoPostBack="true"
                                                            OnCheckedChanged="cbhrdept_OnCheckedChanged" />
                                                        <asp:CheckBoxList ID="cblhrdept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblhrdept_OnSelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                </div>

                                            </td>

                                        </tr>

                                    </table>
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 140px; width: 250px;">
                                    <legend>HR Settings</legend>
                                    <asp:CheckBoxList ID="cblhrset" runat="server">
                                        <asp:ListItem Text="LOP from Attendance" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Allow Edit in Holiday" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Staff Holiday By Staff Type" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Allow Apply Leave After Pay Process" Value="4"></asp:ListItem>
                                    </asp:CheckBoxList>
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="height: 80px; width: 300px;">
                                    <legend>Download And Mark Attendance Settings</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkfingerid" runat="server" Checked="false" Text="Include Multiple FingerID for Staff" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <fieldset style="height: 40px; width: 200px;">
                                    <legend>Staff Code Update Settings</legend>
                                    <asp:CheckBox ID="chkupdstfcode" runat="server" Text="Allow Staff Code Update" />
                                </fieldset>

                                <fieldset style="height: 40px; width: 240px;">
                                    <legend>Pay Process Lock Setting</legend>
                                    <asp:CheckBox ID="cb_payprocesslock" runat="server" Text="Lock Generated Pay Process" />
                                </fieldset>
                                <fieldset style="height: 50px; width: 400px;">
                                    <legend>Retirement Age Calculation</legend>Staff Type
                                    <asp:DropDownList ID="ddlStfType" runat="server" CssClass="textbox1 ddlheight4" OnSelectedIndexChanged="ddlStfType_Change"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                    Retire Age
                                    <asp:TextBox ID="txtRetireAge" runat="server" MaxLength="2" onblur="RetAgeChange(this);"
                                        onkeyup="RetAgeChange(this);" CssClass="textbox textbox1 txtheight" Width="50px"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="filterage" runat="server" TargetControlID="txtRetireAge"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                </fieldset>
                            </td>


                        </tr>
                        <%-- delsi1106--%>
                        <tr>
                            <td>
                                <fieldset style="height: 60px; width: 400px;">
                                    <legend>College Bank Settings</legend>
                                    <table>
                                        <tr>

                                            <td>
                                                <asp:Label ID="lblclgbank" runat="server" Text="College Bank Name " Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>

                                            </td>
                                            <td>
                                                <div style="position: relative;">
                                                    <asp:UpdatePanel ID="updatenlclgbank" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtclgbank" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                                                Style="width: 150px;">--Select--</asp:TextBox>
                                                            <asp:Panel ID="pnlclgbank" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px; position: relative; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 250px; width: 150px;">
                                                                <asp:CheckBox ID="cbclgbank" runat="server" Text="Select All" OnCheckedChanged="cbclgbank_CheckedChange"
                                                                    AutoPostBack="true" />
                                                                <asp:CheckBoxList ID="cblclgbank" runat="server" OnSelectedIndexChanged="cblclgbank_SelectedIndexChange"
                                                                    AutoPostBack="true">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender20" runat="server" TargetControlID="txtclgbank"
                                                                PopupControlID="pnlclgbank" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </td>



                                        </tr>
                                    </table>

                                </fieldset>
                            </td>
                            <td colspan="7">

                                <fieldset style="height: 40px; width: 200px;">
                                    <legend>Priority Setting</legend>
                                    <asp:CheckBox ID="cbgeneral_priority" runat="server" Text="General Priority" />
                                </fieldset>



                            </td>
                        </tr>
                        <%--delsi1106--%>
                        <tr>
                            <td colspan="6">
                                <table style="border: solid 1px gray;">
                                    <tr>
                                        <td>From Month & Year
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlfrmmon" runat="server" CssClass="textbox1 ddlheight">
                                                <asp:ListItem Selected="True" Text="Jan" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Feb" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Mar" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Apr" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="June" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="July" Value="7"></asp:ListItem>
                                                <asp:ListItem Text="Aug" Value="8"></asp:ListItem>
                                                <asp:ListItem Text="Sep" Value="9"></asp:ListItem>
                                                <asp:ListItem Text="Oct" Value="10"></asp:ListItem>
                                                <asp:ListItem Text="Nov" Value="11"></asp:ListItem>
                                                <asp:ListItem Text="Dec" Value="12"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlfrmyear" runat="server" CssClass="textbox1 ddlheight">
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="2">To Month & Year
                                            <asp:DropDownList ID="ddltomon" runat="server" CssClass="textbox1 ddlheight">
                                                <asp:ListItem Selected="True" Text="Jan" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Feb" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Mar" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Apr" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="June" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="July" Value="7"></asp:ListItem>
                                                <asp:ListItem Text="Aug" Value="8"></asp:ListItem>
                                                <asp:ListItem Text="Sep" Value="9"></asp:ListItem>
                                                <asp:ListItem Text="Oct" Value="10"></asp:ListItem>
                                                <asp:ListItem Text="Nov" Value="11"></asp:ListItem>
                                                <asp:ListItem Text="Dec" Value="12"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddltoyear" runat="server" CssClass="textbox1 ddlheight">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnAccessYear" runat="server" CssClass="textbox textbox1 btn2" Text="Save"
                                                OnClick="btnAccessYear_click" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_AllMonth" Text="Calculate All Month" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <table style="border: solid 1px gray;">
                                    <tr>
                                        <td>
                                            <span>PAN / GIR NO.</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPanNo" runat="server" CssClass="textbox textbox1 txtheight3"
                                                Width="150px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="fl" runat="server" TargetControlID="txtPanNo" FilterType="LowercaseLetters,Custom,UppercaseLetters,Numbers"
                                                ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <span>TAN</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTanNo" runat="server" CssClass="textbox textbox1 txtheight3"
                                                Width="150px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="txtTanNo"
                                                FilterType="LowercaseLetters,Custom,UppercaseLetters,Numbers" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSaveValues" runat="server" CssClass="textbox textbox1 btn2" Text="Save"
                                                OnClick="btnSaveValues_click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <%--poo 28.10.17--%>
                            <td colspan="8">
                                <table style="border: solid 1px gray;">
                                    Round Off
                                    <tr>
                                        <td>
                                            <span>Grade Pay</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtgradepay" runat="server" CssClass="textbox textbox1 txtheight3"
                                                Width="50px" MaxLength="2"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" TargetControlID="txtgradepay"
                                                FilterType="Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <span>Basic Pay</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtbasicpay" runat="server" CssClass="textbox textbox1 txtheight3"
                                                Width="50px" MaxLength="2"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" TargetControlID="txtbasicpay"
                                                FilterType="Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                                <table style="border: solid 1px gray;">
                                    <tr>
                                        <td>Rebate amount
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRebateAmt" runat="server" CssClass="textbox textbox1 txtheight3"
                                                Width="100px" MaxLength="10"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender25" runat="server" TargetControlID="txtRebateAmt"
                                                FilterType="Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>Rebate Deduct Amount
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRebateDeductAmt" runat="server" CssClass="textbox textbox1 txtheight3"
                                                Width="100px" MaxLength="10"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender26" runat="server" TargetControlID="txtRebateDeductAmt"
                                                FilterType="Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>Education Cess%
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txteducess" runat="server" CssClass="textbox textbox1 txtheight3"
                                                Width="100px" MaxLength="3"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender27" runat="server" TargetControlID="txteducess"
                                                FilterType="Numbers">
                                            </asp:FilteredTextBoxExtender>

                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_houserent" Text="View House Rent" runat="server" />

                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <table style="border: solid 1px gray;">
                                    <tr>
                                        <td>From Rs.
                                            <asp:TextBox ID="txtfrmsecamnt" runat="server" MaxLength="15" onkeyup="chkamnt(this);"
                                                CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="filterfrmsecamnt" runat="server" FilterMode="ValidChars"
                                                FilterType="Custom,Numbers" ValidChars="." TargetControlID="txtfrmsecamnt">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>to Rs.<asp:TextBox ID="txttosecamnt" runat="server" MaxLength="15" onkeyup="chkamnt(this);"
                                            OnTextChanged="txttosecamnt_change" AutoPostBack="true" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="filtertosecamnt" runat="server" FilterMode="ValidChars"
                                                FilterType="Custom,Numbers" ValidChars="." TargetControlID="txttosecamnt">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlamntorpersec" runat="server" OnSelectedIndexChanged="ddlamntorpersec_change"
                                                AutoPostBack="true" CssClass="textbox1 ddlheight">
                                                <asp:ListItem Selected="True" Text="Amount" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Percent" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtuptosec" runat="server" MaxLength="8" onkeyup="chksetsecamnt(this);"
                                                CssClass="textbox textbox1" Width="125px" Height="19px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="filtersec" runat="server" FilterMode="ValidChars"
                                                FilterType="Custom,Numbers" ValidChars="." TargetControlID="txtuptosec">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnadditset" runat="server" CssClass="textbox textbox1 btn2" Text="Add"
                                                OnClick="btnadditset_click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">Gender
                                            <asp:DropDownList ID="ddlgender" runat="server" CssClass="textbox1 ddlheight2">
                                                <asp:ListItem Selected="True" Text="Male" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Female" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="TransGender" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Senior Citizen Male" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Senior Citizen Female" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="Senior Citizen TransGender" Value="5"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_clear" runat="server" Text="Clear" OnClick="btn_clear_Click" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItCalPk" Visible="true" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <center>
                                    <asp:Button ID="btnview" runat="server" Visible="false" Text="View" OnClick="btnview_click"
                                        CssClass="textbox textbox1 btn2" />
                                </center>
                                <div id="divgrditset" runat="server" visible="false" style="border: 2px solid indigo; border-radius: 10px; height: 250px; margin-left: 27px; width: 600px;">
                                    <div style="height: 200px; overflow: auto;">
                                        <center>
                                            <asp:GridView ID="grditset" runat="server" AutoGenerateColumns="false" Visible="false"
                                                GridLines="Both" OnRowDataBound="grditset_rowbound" OnRowCommand="grditset_rowcommand">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:Label ID="lbl_sno" runat="server" Text='<%#Eval("sno") %>' />
                                                                <asp:Label ID="lbl_itCalPk" runat="server" Visible="false" Text='<%#Eval("itCalculationPK") %>' />
                                                            </center>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="From Range" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:Label ID="lbl_frmamnt" runat="server" Text='<%#Eval("itfrmamnt") %>'>
                                                                </asp:Label>
                                                            </center>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="To Range" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:Label ID="lbl_toamnt" runat="server" Text='<%#Eval("ittoamnt") %>'>
                                                                </asp:Label>
                                                            </center>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Mode" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:Label ID="lbl_itmode" runat="server" Text='<%#Eval("itmode") %>'>
                                                                </asp:Label>
                                                            </center>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amt/Per" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:Label ID="lbl_itamntorper" runat="server" Text='<%#Eval("itamntorper") %>'>
                                                                </asp:Label>
                                                            </center>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Gender" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:Label ID="lbl_gender" runat="server" Text='<%#Eval("gender") %>'>
                                                                </asp:Label>
                                                            </center>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Delete" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:Button ID="btn_del" runat="server" Text="DELETE" OnClick="btn_del_Click" />
                                                            </center>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Update" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:Button ID="btn_update" runat="server" Text="Update" OnClick="btn_update_Click" />
                                                            </center>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </center>
                                    </div>
                                    <center>
                                        <asp:Button ID="btnsaveallitset" runat="server" Text="Set IT Range Settings" CssClass="textbox textbox1 btn2"
                                            Width="220px" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnsaveallitset_Click" />
                                    </center>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <table style="border: solid 1px gray;">
                                    Professional Tax Calculation Month
                                    <tr>
                                        <td>Start Month & Year
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlfrommonthpt" runat="server" CssClass="textbox1 ddlheight">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlfromyearpt" runat="server" CssClass="textbox1 ddlheight">
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="2">End Month & Year
                                            <asp:DropDownList ID="ddltomonthpt" runat="server" CssClass="textbox1 ddlheight">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddltoyearpt" runat="server" CssClass="textbox1 ddlheight">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnsavept" runat="server" CssClass="textbox textbox1 btn2" Text="Save"
                                                OnClick="btnsavept_click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>

                        </tr>

                        <%-- Added by saranya on 12/9/2018--%>
                        <tr>
                            <td colspan="4">
                                <asp:CheckBox ID="ChkStafLeaveapply" Text="Apply Staff Leave for Past Date" runat="server" />
                                <asp:CheckBox ID="chksetMaxMonthLeave" Text="Set Maximum Monthly Leave" runat="server" />
                            </td>


                        </tr>
                        <%--poomalar 24.10.17--%>
                    </table>
                </div>
            </center>
            <%--User Degree Tab--%>
            <center>
                <div id="div11UserDeg" runat="server" style="width: 980px; height: 400px; background: white; border: 1px solid #0095E8; overflow: auto;"
                    visible="false">
                    <table>
                        <tr>
                            <td colspan="6">
                                <farpoint:fpspread id="FpCollRight" runat="server" bordercolor="Black" borderstyle="Solid"
                                    borderwidth="1px" height="180" width="420" horizontalscrollbarpolicy="Never"
                                    verticalscrollbarpolicy="Never">
                                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                        ButtonShadowColor="ControlDark">
                                    </CommandBar>
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </farpoint:fpspread>
                                <br />
                                <asp:Panel ID="Pdegreeright" runat="server" Height="300px" ScrollBars="Vertical"
                                    Width="820px" BorderWidth="1px">
                                    <asp:TreeView ID="TVDegreeRight" runat="server" ShowCheckBoxes="All" Height="295px"
                                        Width="784px" Font-Names="Book Antiqua" Font-Size="Medium">
                                    </asp:TreeView>
                                </asp:Panel>
                            </td>
                        </tr>
                        <td>
                            <asp:Button ID="btnDegreeCollSave" runat="server" CssClass="textbox textbox1 btn2" BackColor="#1B9D17" ForeColor="White" Font-Bold="true" Text="Save" OnClick="btnDegreeCollSave_click" />
                        </td>
                    </table>
                </div>
            </center>
            <%--Web Application Payment tab--%>
            <center>
                <div id="div12WebPay" runat="server" style="width: 980px; height: 400px; background: white; border: 1px solid #0095E8; overflow: auto;"
                    visible="false">
                    <table border="1" style="font-family: Book Antiqua; font-size: medium; border: 1px solid black;">
                        <tr>
                            <td colspan="2">
                                <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Online
                                    Application Login Mode</span>
                                <br />
                                <asp:CheckBoxList ID="rblOnlineAppLoginMode" runat="server" RepeatColumns="2" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="0" Selected="True">Roll No</asp:ListItem>
                                    <asp:ListItem Value="1">Register No</asp:ListItem>
                                    <asp:ListItem Value="2">Admission No</asp:ListItem>
                                    <asp:ListItem Value="3">App No</asp:ListItem>
                                    <asp:ListItem Value="4">Staff Code</asp:ListItem>
                                </asp:CheckBoxList>
                                <asp:CheckBox ID="Pdfcheck" runat="server" Text="Pdf" /><%--added by abarna--%>
                                <asp:CheckBox ID="Dob" runat="server" Text="DOB" /><%--added by abarna--%>
                                <asp:CheckBox ID="EnablePartamt" runat="server" Text="Enable PartAmount" /><%--added by abarna--%>
                            </td>
                            <td colspan="2">
                                <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Online
                                    Payment Gateway</span>
                                <br />
                                <asp:CheckBoxList ID="rblPaymentGateway" runat="server" RepeatColumns="2" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="0">Atomtech</asp:ListItem>
                                    <asp:ListItem Value="1">PayUMoney</asp:ListItem>
                                    <asp:ListItem Value="3">Bank of Baroda</asp:ListItem>
                                    <asp:ListItem Value="4">IOB</asp:ListItem>
                                    <asp:ListItem Value="5">Kotak Mahendra</asp:ListItem>
                                    <asp:ListItem Value="6">SBI</asp:ListItem>
                                    <asp:ListItem Value="7">HDFC</asp:ListItem>
                                </asp:CheckBoxList>
                                <asp:CheckBox ID="chkWorldline" runat="server" Text="WorldLine" Checked="false" />
                            </td>
                            <td>
                                <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Application
                                    Login Date Restriction</span>
                                <br />
                                <asp:CheckBox ID="chkAppLogRestrict" runat="server" Text="Restrict Days" />
                                <br />
                                No of days to restrict
                                <asp:TextBox ID="txtAppLogRestrict" runat="server" MaxLength="2" CssClass="textbox textbox1"
                                    Width="30px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="fteAppLogRestrict" runat="server" FilterMode="ValidChars"
                                    FilterType="Custom" ValidChars="0123456789" TargetControlID="txtAppLogRestrict">
                                </asp:FilteredTextBoxExtender>
                                <asp:CheckBox ID="addfee" runat="server" Text="addfee" /><%--added by abarna--%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="10">
                                <fieldset style="height: 250px;">
                                    <legend>Online Payment Finance Year,Header & Ledger Settings </legend>
                                    <table>
                                        <tr>
                                            <td>Finance Year
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlfinOnline" runat="server" CssClass="textbox1 ddlheight2">
                                                </asp:DropDownList>
                                            </td>
                                            <td>Header
                                            </td>
                                            <td>
                                                <div style="position: relative;">
                                                    <asp:TextBox ID="txthdOnline" runat="server" CssClass="textbox txtheight2" Style="width: 200px"
                                                        ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="Panel22" runat="server" CssClass="multxtpanel " Style="width: 255px; border-color: HighlightText; height: 150px;">
                                                        <asp:CheckBox ID="cbhdOnline" runat="server" Width="204px" Text="Select All" AutoPostBack="true"
                                                            OnCheckedChanged="cbhdOnline_OnCheckedChanged" />
                                                        <asp:CheckBoxList ID="cblhdOnline" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblhdOnline_OnSelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                </div>
                                            </td>
                                            <td>Ledger
                                            </td>
                                            <td>
                                                <div style="position: relative;">
                                                    <asp:TextBox ID="txtldOnline" runat="server" CssClass="textbox txtheight2" Style="width: 200px"
                                                        ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="Panel23" runat="server" CssClass="multxtpanel " Style="width: 255px; border-color: HighlightText; height: 150px;">
                                                        <asp:CheckBox ID="cbedgOnline" runat="server" Width="204px" Text="Select All" AutoPostBack="true"
                                                            OnCheckedChanged="cbedgOnline_OnCheckedChanged" />
                                                        <asp:CheckBoxList ID="cbledgOnline" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbledgOnline_OnSelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                </div>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnsaveOnline" runat="server" Text="Save" CssClass="textbox textbox1 btn2"
                                                    Width="70px" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" Style="background-color: Green; color: White;"
                                                    OnClick="btnsaveOnline_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
            <%--Invigilation--%>
            <%-- added by kowshika on 11/12/2017 --%>
            <center>
                <div id="div13Invigilation" runat="server" style="width: 980px; height: 400px; background: white; border: 1px  solid #0095E8; overflow: auto;"
                    visible="false">
                    <table style="font-family: Book Antiqua; font-size: medium; border: 1px solid black;">
                        <tr>
                            <td colspan="2">
                                <span style="font-weight: bold; font-size: large; font-family: Book Antiqua;">Invigilation</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Lblallowed" runat="server" Text="No of Maximum Allowed Session" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_allowed" runat="server"></asp:TextBox>
                                <asp:RangeValidator runat="server" ID="allow_vaildater" ControlToValidate="txt_allowed"
                                    Type="Integer" MinimumValue="1" MaximumValue="5" ErrorMessage="Please Select Upto 5" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_cycletest" Text="Cycle Test Name" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UP_cycletest" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_cycletest" runat="server" Style="height: 20px; width: 100px;"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_cycletest" runat="server" CssClass="multxtpanel" Style="width: 121px; height: auto;">
                                            <asp:CheckBox ID="cb_cycletest" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_cycletest_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_cycletest" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_cycletest_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pce_cycletest" runat="server" TargetControlID="txt_cycletest"
                                            PopupControlID="panel_cycletest" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CalendarExtender ID="CalendarExtender12" TargetControlID="txt_from" runat="server"
                                    Format="dd/MM/yyyy" Enabled="True">
                                </asp:CalendarExtender>
                                <asp:Label ID="Lblfrom" runat="server" Text="From Date"></asp:Label>
                                <asp:CalendarExtender ID="CalendarExtender13" TargetControlID="txt_to" runat="server"
                                    Format="dd/MM/yyyy" Enabled="True">
                                </asp:CalendarExtender>
                                <asp:TextBox ID="txt_from" runat="server" Height="14px" Width="103px"></asp:TextBox>
                                <asp:Label ID="Lblto" runat="server" Text="To Date"></asp:Label>
                                <asp:TextBox ID="txt_to" runat="server" Height="14px" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
            <%--Invigilation finished--%>
            <%-- Pop Alert--%>
            <center>
                <div id="imgAlert" runat="server" visible="false" style="height: 1000px; z-index: 10000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;">
                    <center>
                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px; border-radius: 10px;">
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
                                                <asp:Button ID="btn_alertclose" CssClass=" textbox textbox1 btn1" Style="height: 28px; width: 65px;"
                                                    OnClick="btn_alertclose_Click" Text="ok" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
        </div>
    </center>
</asp:Content>
