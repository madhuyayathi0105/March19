<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudentMentorReport.aspx.cs" Inherits="StudentMod_StudentMentorReport" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <script type="text/javascript">
        function display() {
            document.getElementById('<%=lbl_validation.ClientID %>').innerHTML = "";
        }
        function autoComplete3_OnClientPopulating(sender, args) {
            var SEARCHTYPE = document.getElementById("<%=ddl_searchtype.ClientID %>").value;
            //var SEARCHTYPE = skillsSelect.options[skillsSelect.selectedIndex].value;
            sender.set_contextKey(SEARCHTYPE);
        }
        function myFunction(x) {
            x.style.borderColor = "#c4c4c4";
        }
        function PrintPanel() {
            var panel = document.getElementById("<%=divStudentMentor.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=1191');
            printWindow.document.write('<html');
            printWindow.document.write('<head>');
            printWindow.document.write('</head><body >');
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
        function CounsellingPrintPanel() {
            var panel = document.getElementById("<%=divCounsellingReport.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=1191');
            printWindow.document.write('<html');
            printWindow.document.write('<head>');
            printWindow.document.write('</head><body >');
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
    <style tyle="text/css">
        @media print
        {
            divCounsellingReport
            {
                display: block;
            }
            divStudentMentor
            {
                display: block;
            }
            .noprint
            {
                display: none;
            }
        }
        @media screen,print
        {
        
        }
        @page
        {
            size: A4;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    <center>
                        <span class="fontstyleheader" style="color: Green;">Student Mentor Report</span>
                        <br />
                        <div class="maindivstyle" style="width: 990px; height: 850px;">
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        Student Search
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_searchtype" runat="server" CssClass="textbox  ddlheight2"
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
                                    <td>
                                        Staff Search
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlstaff" runat="server" CssClass="textbox  ddlheight2" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlstaff_OnSelectedIndexChanged">
                                            <asp:ListItem>Staff Name</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtstaff" runat="server" CssClass="textbox textbox1 txtheight1"
                                            Width="135px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtstaff"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=".-/ ">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtstaff"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_Search" runat="server" Text="Go" CssClass="textbox textbox1 btn1"
                                            OnClick="btn_Search_OnClick" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                            <div id="divStudentMentor" runat="server" style="display: table; margin: 0px; height: auto;
                                margin-bottom: 20px; margin-top: 10px; position: relative; width: auto; text-align: left;">
                                <asp:HiddenField ID="HiddenFieldStudentMentor" runat="server" Value="-1" />
                                <asp:GridView ID="GrdStudentMentor" Width="950px" runat="server" ShowFooter="false"
                                    Font-Names="Book Antiqua" ShowHeader="false" OnRowDataBound="GrdStudentMentor_OnRowDataBound"
                                    OnRowCreated="GrdStudentMentor_OnRowCreated" OnSelectedIndexChanged="GrdStudentMentor_SelectedIndexChanged">
                                    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                </asp:GridView>
                            </div>
                            <br />
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <ContentTemplate>
                                    <div id="rptprint" runat="server" visible="false">
                                        <asp:Label ID="lbl_validation" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                            Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_rptname" runat="server" Text="Report Name"></asp:Label>
                                        <asp:TextBox ID="txt_excelname" runat="server" CssClass="textbox textbox1 txtheight4"
                                            onkeypress="display()"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txt_excelname"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Button ID="btn_excel" runat="server" OnClick="btn_excel_Click" CssClass="textbox"
                                            Text="Export To Excel" Width="127px" Height="30px" />
                                        <asp:Button ID="btn_printmaster" runat="server" Text="Print" OnClick="btn_printmaster_Click"
                                            Width="60px" Height="30px" CssClass="textbox" />
                                        <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                                        <asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btn_excel" />
                                    <asp:PostBackTrigger ControlID="btn_printmaster" />
                                    <asp:PostBackTrigger ControlID="btnPrint" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <%---------------------Added by saranya on 9/10/2018--------------------%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div id="CounsellingPopup" runat="server" class="popupstyle popupheight1" visible="false"
                    style="height: 300em;">
                    <asp:ImageButton ID="imagebtnpop1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 15px; margin-left: 430px;"
                        OnClick="btn_popclose_Click" />
                    <br />
                    <div style="background-color: White; height: 800px; font-family: Book Antiqua; width: 900px;
                        border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <center>
                            <span class="fontstyleheader" style="color: #008000;">Counselling</span>
                        </center>
                        <br />
                        <div>
                            <center>
                                <table width="880px" style="font-weight: bold;">
                                    <tr>
                                        <td style="width: 200px;">
                                            <asp:Label ID="lbl_CounsDate" runat="server" Text="Counselling Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtDate" runat="server" AutoPostBack="true" Width="100px" Height="20px"
                                                CssClass="textbox txtheight2" OnTextChanged="TxtDate_OnTextChanged"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="TxtDate" runat="server"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTypeOfDes" runat="server" Text="Type Of Description"></asp:Label>
                                            <asp:Button ID="btnAddDesc" runat="server" Font-Size="Medium" OnClick="btnAddDesc_OnClick"
                                                Style="font-size: Medium; font-size: Medium; position: absolute; margin-top: -6px;
                                                height: 32px;" Text="+" />
                                            <asp:DropDownList ID="ddl_Description" runat="server" AutoPostBack="true" Style="font-size: Medium;
                                                font-family: Book Antiqua; font-size: Medium; position: absolute; margin-left: 34px;
                                                margin-top: -5px;" CssClass="textbox ddlstyle ddlheight3">
                                                <asp:ListItem Text="--SELECT--" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Button ID="btnDeleteDesc" runat="server" Font-Size="Medium" OnClick="btnDeleteDesc_OnClick"
                                                Style="font-size: Medium; font-size: Medium; position: absolute; margin-left: 169px;
                                                margin-top: -6px; height: 32px;" Text="-" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="LblCounselGiven" runat="server" Text="Points Addressing / Counselling Given"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="TxtCounseGiven" runat="server" TextMode="MultiLine" AutoPostBack="true"
                                                Width="620px" Height="80px" CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="LblGrievance" runat="server" Text="Student Suggestion / Grievance"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="TxtGrievance" runat="server" TextMode="MultiLine" AutoPostBack="true"
                                                Width="620px" Height="80px" CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="right">
                                            <asp:UpdatePanel ID="UpCounselBtn" runat="server">
                                                <ContentTemplate>
                                                    <asp:Button ID="BtnSave" runat="server" Enabled="false" Text="Save" Width="80px"
                                                        CssClass="textbox textbox1 btn1" OnClick="BtnSave_OnClick" />
                                                    <asp:Button ID="BtnUpdate" runat="server" Enabled="false" Text="Update" Width="80px"
                                                        CssClass="textbox textbox1 btn1" OnClick="BtnUpdate_OnClick" />
                                                    <asp:Button ID="BtnDelete" runat="server" Enabled="false" Text="Delete" Width="80px"
                                                        CssClass="textbox textbox1 btn1" OnClick="BtnDelete_OnClick" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="BtnSave" />
                                                    <asp:PostBackTrigger ControlID="BtnUpdate" />
                                                    <asp:PostBackTrigger ControlID="BtnDelete" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                        <br />
                        <div id="divCounsellingReport" runat="server" visible="false" style="overflow: auto;"
                            width="800px">
                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                <ContentTemplate>
                                    <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                                    <asp:GridView ID="grdcounselling" Width="800px" runat="server" ShowFooter="false"
                                        AutoGenerateColumns="true" Font-Names="Book Antiqua" toGenerateColumns="true"
                                        OnRowCreated="grdcounselling_OnRowCreated" OnSelectedIndexChanged=" grdcounselling_SelectedIndexChanged">
                                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="grdcounselling" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <br />
                        <asp:Button ID="BtncounsellingPrint" Visible="false" runat="server" Text="Direct Print"
                            OnClientClick="return CounsellingPrintPanel();" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
            <asp:PostBackTrigger ControlID="imagebtnpop1" /></Triggers>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div id="plusdiv" runat="server" visible="false" class="popupstyle popupheight1">
                    <center>
                        <div id="panel_addDesc" runat="server" visible="false" class="table" style="background-color: White;
                            height: 140px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <table style="line-height: 30px">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_addDesc" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:TextBox ID="txt_addDesc" runat="server" Width="200px" CssClass="textbox textbox1"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="line-height: 35px">
                                        <asp:Button ID="btn_addDesc" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1 btn2" OnClick="btn_addDesc_Click" />
                                        <asp:Button ID="btn_exitDesc" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1 btn2" OnClick="btn_exitDesc_Click" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--Delete Confirmation Popup for description --%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <div id="surediv" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_sure" runat="server" Text="Do You Want To Delete Selected Description?"
                                                Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_yes" CssClass=" textbox textbox1 btn1 " Style="height: 28px;
                                                    width: 65px;" OnClick="btn_sureyes_Click" Text="yes" runat="server" />
                                                <asp:Button ID="btn_no" CssClass=" textbox textbox1 btn1 " Style="height: 28px; width: 65px;"
                                                    OnClick="btn_sureno_Click" Text="no" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--Delete Confirmation Popup for Counselling --%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
                <div id="SureDivCounselling" runat="server" visible="false" style="height: 100%;
                    z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                    top: 0; left: 0px;">
                    <center>
                        <div id="Div3" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="Label1" runat="server" Text="Do You Want To Delete Selected Record?"
                                                Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnSureYesDelCouns" CssClass=" textbox textbox1 btn1 " Style="height: 28px;
                                                    width: 65px;" OnClick="btnSureYesDelCouns_Click" Text="yes" runat="server" />
                                                <asp:Button ID="btnSureNoDelCoun" CssClass=" textbox textbox1 btn1 " Style="height: 28px;
                                                    width: 65px;" OnClick="btnSureNoDelCouns_Click" Text="no" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
        <ContentTemplate>
            <div id="imgdivMessage" runat="server" visible="false" style="height: 100%; z-index: 100000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="panel_erroralert" runat="server" class="table" style="background-color: White;
                        height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        margin-top: 200px; border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_erroralert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_erroralert" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                                width: 65px;" OnClick="btnerrexit_Click" Text="OK" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--progressBar for Upbook_go--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpCounselBtn">
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
    <%---------------------------------------------------------------------%>
</asp:Content>
