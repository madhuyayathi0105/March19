<%@ Page Title="Nominal Roll" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Nominal_Roll.aspx.cs" Inherits="Nominal_Roll"
    EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_errmsg').innerHTML = "";
        }
        function display1() {
            document.getElementById('<%#lbl_norec1.ClientID %>').innerHTML = "";
        }
    </script>
    <style type="text/css">
        .style0
        {
            height: 25px;
            width: 80px;
        }
        .style1
        {
            width: 100%;
        }
        .style2
        {
            height: 25px;
            width: 135px;
        }
        .style3
        {
            height: 25px;
            width: 84px;
        }
        .style4
        {
            height: 25px;
            width: 100px;
        }
        .Dropdown_Txt_Box
        {
            text-align: left;
        }
        .style5
        {
            width: 800px;
        }
        .style6
        {
            height: 35px;
            width: 90px;
        }
    </style>
    <script type="text/javascript">

        function validation() {
            var id = document.getElementById("<%=rdbtnsubject.ClientID %>");
            var error = "";
            var ddlfst = document.getElementById("<%=txtcourse.ClientID %>");
            var ddlsecond = document.getElementById("<%=txtsub.ClientID %>");
            var ddlFormat = document.getElementById("<%=ddltype.ClientID %>");
            var go = document.getElementById("<%=ButtonGo.ClientID %>")
            if (id.checked == true) {

                if (ddlFormat.options[ddlFormat.selectedIndex].value == "3") {

                    if ((ddlsecond.value == "--Select--")) {
                        error += "Please Select Subject \n";
                    }
                }
                else if (ddlFormat.options[ddlFormat.selectedIndex].Value != "3") {

                    if ((ddlfst.value == "--Select--") && (ddlsecond.value == "--Select--")) {
                        error += "Please Select From Course and Subject \n";
                    }
                }
                if (error != "") {
                    alert(error);
                    return false;
                }
                else {
                    return true;
                }
            }
            else {
                return true;
            }
        }

        function valid(id) {
            if (id.checked == true) {
                var ddlfst = document.getElementById("<%=txtcourse.ClientID %>");
                var ddlsecond = document.getElementById("<%=txtsub.ClientID %>");
                var ddlsecond1 = document.getElementById("<%=ddldegree.ClientID %>");
                var ddlsecond2 = document.getElementById("<%=lblSubjectName.ClientID %>");
                ddlfst.style.display = "none";
                ddlsecond.style.display = "none";
                ddlsecond1.style.display = "block";
                ddlsecond2.style.display = "none";
            }
        }

        function valid1(id) {
            if (id.checked == true) {
                var ddlfst = document.getElementById("<%=txtcourse.ClientID %>");
                var ddlsecond = document.getElementById("<%=txtsub.ClientID %>");
                var ddlsecond1 = document.getElementById("<%=ddldegree.ClientID %>");
                var ddlsecond2 = document.getElementById("<%=lblSubjectName.ClientID %>");
                ddlfst.style.display = "block";
                ddlsecond.style.display = "block";
                ddlsecond1.style.display = "none";
                ddlsecond2.style.display = "block";
            }
        }   
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="width: 100%; height: auto; margin: 0px; padding: 0px;">
        <center>
            <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
                margin-top: 10px;">Nominal Roll</span>
        </center>
        <center>
            <table class="maintablestyle" style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                <tr>
                    <td>
                        <span style="font-family: Book Antiqua; font-size: large; color: Black; font-weight: 700;">
                            ExamMonth</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlExamMonth" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            CssClass="style0" AutoPostBack="true" OnSelectedIndexChanged="ddlExamMonth_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <span style="font-family: Book Antiqua; font-size: large; color: Black; font-weight: 700;">
                            Year</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlExamYear" runat="server" AutoPostBack="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" CssClass="style3" OnSelectedIndexChanged="ddlExamYear_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblExamDate" runat="server" Text="Date" Style="font-family: Book Antiqua;
                            font-size: large; color: Black; font-weight: 700;"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlExamDate" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtExamDate" Width="80px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlExamDate" runat="server" CssClass="multxtpanel" Style="width: 130px;
                                        height: 280px; overflow: auto; margin: 0px; padding: 0px;">
                                        <asp:CheckBox ID="chkExamDate" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                            runat="server" Text="Select All" AutoPostBack="True" Style="width: 100%; height: auto;
                                            margin: 0px; padding: 0px; border: 0px;" OnCheckedChanged="chkExamDate_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblExamDate" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                            runat="server" AutoPostBack="True" Style="width: 100%; height: auto; margin: 0px;
                                            padding: 0px; border: 0px;" OnSelectedIndexChanged="cblExamDate_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtExamDate" runat="server" TargetControlID="txtExamDate"
                                        PopupControlID="pnlExamDate" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    <%-- <asp:DropDownList ID="ddlDate" Visible="false" runat="server" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CssClass="style4" AutoPostBack="true" OnSelectedIndexChanged="SubjectName">
                                    </asp:DropDownList>--%>
                                    <asp:DropDownList ID="ddlExamDate" Visible="false" Width="60px" runat="server" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" CssClass="arrow" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlExamDate_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblExamSession" Text="Session" Style="font-family: Book Antiqua;
                            font-size: large; color: Black; font-weight: 700;"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlExamSession" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtExamSession" Width="80px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlExamSession" runat="server" CssClass="multxtpanel" Style="width: 130px;
                                        height: 100px; overflow: auto; margin: 0px; padding: 0px;">
                                        <asp:CheckBox ID="chkExamSession" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                            runat="server" Text="Select All" AutoPostBack="True" Style="width: 100%; height: auto;
                                            margin: 0px; padding: 0px; border: 0px;" OnCheckedChanged="chkExamSession_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblExamSession" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                            runat="server" AutoPostBack="True" Style="width: 100%; height: auto; margin: 0px;
                                            padding: 0px; border: 0px;" OnSelectedIndexChanged="cblExamSession_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtExamSession" runat="server" TargetControlID="txtExamSession"
                                        PopupControlID="pnlExamSession" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    <%-- <asp:DropDownList ID="ddlSession" Visible="false" runat="server" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CssClass="style3" AutoPostBack="true" OnSelectedIndexChanged="SubjectName1">
                                    </asp:DropDownList>--%>
                                    <asp:DropDownList ID="ddlExamSession" Visible="false" Width="60px" runat="server"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CssClass="arrow"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlExamSession_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:RadioButton ID="rdbtnstudent" runat="server" Text="Student Wise" AutoPostBack="true"
                            OnCheckedChanged="rdbtnstudent_change" Style="font-family: Book Antiqua; font-size: large;
                            color: Black; font-weight: 700;" GroupName="same" />
                    </td>
                    <td>
                        <asp:RadioButton ID="rdbtnsubject" runat="server" Text="Subject Wise" AutoPostBack="true"
                            OnCheckedChanged="redbtnsubject_change" Style="font-family: Book Antiqua; font-size: large;
                            color: Black; font-weight: 700;" GroupName="same" />
                    </td>
                </tr>
                <tr>
                    <td colspan="10">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCollege" runat="server" Text="College" Style="font-family: Book Antiqua;
                                        font-size: large; color: Black; font-weight: 700;">
                                    </asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative; margin: 0px; padding: 0px;">
                                        <asp:UpdatePanel ID="upnlCollege" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtCollege" Width=" 100px" runat="server" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" CssClass="Dropdown_Txt_Box" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="pnlCollege" runat="server" CssClass="multxtpanel" Style="width: 280px;
                                                    height: auto;">
                                                    <asp:CheckBox ID="chkCollege" Font-Names="Book Antiqua" Font-Size="Medium" runat="server"
                                                        Text="Select All" AutoPostBack="True" OnCheckedChanged="chkCollege_CheckedChanged"
                                                        Style="width: 100%; height: auto;" />
                                                    <asp:CheckBoxList ID="cblCollege" Font-Size="Medium" Font-Names="Book Antiqua" runat="server"
                                                        AutoPostBack="True" OnSelectedIndexChanged="cblCollege_SelectedIndexChanged"
                                                        Style="width: 100%; height: auto;">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="popupExtCollege" runat="server" TargetControlID="txtCollege"
                                                    PopupControlID="pnlCollege" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td>
                                    <div id="divStudentWise" visible="false" runat="server">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblBatch" runat="server" Text="Batch" Style="font-family: Book Antiqua;
                                                        font-size: large; color: Black; font-weight: 700;">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <div id="divBatch" runat="server" style="position: relative;">
                                                        <asp:UpdatePanel ID="upnlBatch" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtBatch" runat="server" CssClass="Dropdown_Txt_Box" Style="width: 70px;">--Select--</asp:TextBox>
                                                                <asp:Panel ID="pnlBatch" runat="server" CssClass="multxtpanel" Style="height: 300px;
                                                                    width: 98px;">
                                                                    <asp:CheckBox ID="chkBatch" runat="server" Font-Names="Book Antiqua" Font-Size="medium"
                                                                        AutoPostBack="true" Text="Select All" OnCheckedChanged="chkBatch_CheckedChanged" />
                                                                    <asp:CheckBoxList ID="cblBatch" runat="server" Font-Names="Book Antiqua" Font-Size="medium"
                                                                        AutoPostBack="true" OnSelectedIndexChanged="cblBatch_selectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="popExtBatch" runat="server" TargetControlID="txtBatch"
                                                                    PopupControlID="pnlBatch" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                                <asp:DropDownList ID="ddlBatch" runat="server" Font-Names="Book Antiqua" AutoPostBack="true"
                                                                    OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged" Font-Size="Medium" Visible="false"
                                                                    Style="width: 70px;">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="lblCourse" runat="server" Text="Course" Style="font-family: Book Antiqua;
                                        font-size: large; color: Black; font-weight: 700;">
                                    </asp:Label>
                                </td>
                                <td>
                                    <div id="divCourse" runat="server" style="position: relative;">
                                        <asp:UpdatePanel ID="up31" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtcourse" runat="server" CssClass="Dropdown_Txt_Box" Style="width: 135px;">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel14" runat="server" CssClass="multxtpanel" Style="height: 300px;
                                                    width: 190px;">
                                                    <asp:CheckBox ID="chkcourse" runat="server" Font-Names="Book Antiqua" Font-Size="medium"
                                                        AutoPostBack="true" Text="Select All" OnCheckedChanged="chkcourse_CheckedChanged" />
                                                    <asp:CheckBoxList ID="chklistcourse" runat="server" Font-Names="Book Antiqua" Font-Size="medium"
                                                        AutoPostBack="true" OnSelectedIndexChanged="chklistcourse_selectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="popctrl" runat="server" TargetControlID="txtcourse"
                                                    PopupControlID="panel14" Position="Bottom">
                                                </asp:PopupControlExtender>
                                                <asp:DropDownList ID="ddldegree" runat="server" Font-Names="Book Antiqua" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddldegree_SelectedIndexChanged" Font-Size="Medium" CssClass="style3"
                                                    Visible="false" Style="width: 135px;">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="lblSubjectName" Text="SubjectName" runat="server" Style="font-family: Book Antiqua;
                                        font-size: large; color: Black; font-weight: 700;"></asp:Label>
                                </td>
                                <td>
                                    <div id="divSubjectName" runat="server" style="position: relative;">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtsub" runat="server" Style="text-align: left;" CssClass="Dropdown_Txt_Box">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel1" runat="server" CssClass="multxtpanel" Width="220px" Height="300px">
                                                    <asp:CheckBox ID="chksub" runat="server" Font-Names="Book Antiqua" Font-Size="medium"
                                                        AutoPostBack="true" Text="Select All" OnCheckedChanged="chksub_CheckedChanged" />
                                                    <asp:CheckBoxList ID="chklistsub" runat="server" CssClass="style2" Font-Names="Book Antiqua"
                                                        Font-Size="medium" AutoPostBack="true" OnSelectedIndexChanged="chklistsub_selectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtsub"
                                                    PopupControlID="panel1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td>
                                    <asp:Button ID="ButtonGo" CssClass="textbox textbox1" runat="server" Font-Bold="true"
                                        Style="width: auto; height: auto;" Text="Go" Font-Names="Book Antiqua" Font-Size="Medium"
                                        OnClientClick="return validation()" ForeColor="#000000" OnClick="ButtonGo_Click" />
                                </td>
                                <td>
                                    <asp:Label ID="lblFormat" Text="Type" runat="server" Style="font-family: Book Antiqua;
                                        font-size: large; color: Black; font-weight: 700;"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddltype" runat="server" Font-Names="Book Antiqua" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddltype_SelectedIndexChanged" Font-Size="Medium" CssClass="style3"
                                        Style="width: 135px;">
                                        <asp:ListItem Selected="True" Value="0">Format 1</asp:ListItem>
                                        <asp:ListItem Selected="False" Value="1">Format 2</asp:ListItem>
                                        <asp:ListItem Selected="False" Value="2">Format 3</asp:ListItem>
                                        <asp:ListItem Selected="False" Value="3">Format 4</asp:ListItem>
                                        <asp:ListItem Selected="False" Value="4">Format 5</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                <asp:Label ID="Label1" Text="Size" runat="server" Style="font-family: Book Antiqua;
                                        font-size: large; color: Black; font-weight: 700;"></asp:Label>
                                        <asp:TextBox ID="txtSize" runat="server"></asp:TextBox>        
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"  
ControlToValidate="txtSize" ErrorMessage="*"  
ValidationExpression="[0-9]{2}"></asp:RegularExpressionValidator>                        
                                        </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="10">
                        <table>
                            <tr>
                                <td>
                                    <div id="divHall" runat="server" visible="false">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblHall" runat="server" Text="Hall No" Style="font-family: Book Antiqua;
                                                        font-size: large; color: Black; font-weight: 700;">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <div style="position: relative; margin: 0px; padding: 0px;">
                                                        <asp:UpdatePanel ID="upnlHall" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtHall" Width=" 100px" runat="server" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" CssClass="Dropdown_Txt_Box" ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="pnlHall" runat="server" CssClass="multxtpanel" Style="width: 180px;
                                                                    height: 300px;">
                                                                    <asp:CheckBox ID="chkHall" Font-Names="Book Antiqua" Font-Size="Medium" runat="server"
                                                                        Text="Select All" AutoPostBack="True" OnCheckedChanged="chkHall_CheckedChanged"
                                                                        Style="width: 100%; height: auto;" />
                                                                    <asp:CheckBoxList ID="cblHall" Font-Size="Medium" Font-Names="Book Antiqua" runat="server"
                                                                        AutoPostBack="True" OnSelectedIndexChanged="cblHall_SelectedIndexChanged" Style="width: 100%;
                                                                        height: auto;">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="popupExtHall" runat="server" TargetControlID="txtHall"
                                                                    PopupControlID="pnlHall" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkconsolidate" runat="server" Text="Consolidated" Font-Size="Medium"
                                        Font-Names="Book Antiqua" Font-Bold="true" AutoPostBack="true" OnCheckedChanged="chkconsolidate_CheckedChanged"
                                        Width="130px" Style="color: Black;" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIncludeDepartmentWise" Visible="false" runat="server" Text="Include Department Wise"
                                        Checked="true" Font-Bold="true" Font-Size="Medium" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkNeedSubjectTotal" Visible="false" runat="server" Text="Need Subject Total"
                                        Checked="true" Font-Bold="true" Font-Size="Medium" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkWithoutRegularArrear" Visible="false" runat="server" Text="Without Regular/Arrear"
                                        Checked="true" Font-Bold="true" Font-Size="Medium" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblnorec" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="Medium"
                Font-Names="Book Antiqua" Font-Bold="true" Style="margin: 0px; margin-bottom: 10px;
                margin-top: 10px;"></asp:Label>
        </center>
        <center>
            <table>
                <tr>
                    <td colspan="3" align="center">
                        <FarPoint:FpSpread ID="FSNominee" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Height="245px" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px;"
                            OnUpdateCommand="FSNominee_UpdateCommand" Width="950px" Visible="False" VerticalScrollBarPolicy="Never"
                            ActiveSheetViewIndex="0" ShowHeaderSelection="false">
                            <CommandBar BackColor="Control" ShowPDFButton="true" ButtonType="PushButton">
                                <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Sheet&gt;&lt;Data&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;SheetName&gt;Sheet1&lt;/SheetName&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnFooter&gt;&lt;/Data&gt;&lt;Presentation&gt;&lt;AxisModels&gt;&lt;Column class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/Column&gt;&lt;RowHeaderColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;40&quot; orientation=&quot;Horizontal&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;Size&gt;40&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/RowHeaderColumn&gt;&lt;ColumnHeaderRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnHeaderRow&gt;&lt;ColumnFooterRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnFooterRow&gt;&lt;/AxisModels&gt;&lt;StyleModels&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;RowHeaderDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnHeaderDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;DataAreaDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnFooterDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnFooter&gt;&lt;/StyleModels&gt;&lt;MessageRowStyle class=&quot;FarPoint.Web.Spread.Appearance&quot;&gt;&lt;BackColor&gt;LightYellow&lt;/BackColor&gt;&lt;ForeColor&gt;Red&lt;/ForeColor&gt;&lt;/MessageRowStyle&gt;&lt;SheetCornerStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/SheetCornerStyle&gt;&lt;AllowLoadOnDemand&gt;false&lt;/AllowLoadOnDemand&gt;&lt;LoadRowIncrement &gt;10&lt;/LoadRowIncrement &gt;&lt;LoadInitRowCount &gt;30&lt;/LoadInitRowCount &gt;&lt;AllowVirtualScrollPaging&gt;false&lt;/AllowVirtualScrollPaging&gt;&lt;TopRow&gt;0&lt;/TopRow&gt;&lt;PreviewRowStyle class=&quot;FarPoint.Web.Spread.PreviewRowInfo&quot; /&gt;&lt;/Presentation&gt;&lt;Settings&gt;&lt;Name&gt;Sheet1&lt;/Name&gt;&lt;Categories&gt;&lt;Appearance&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionBorder class=&quot;FarPoint.Web.Spread.Border&quot; /&gt;&lt;/Appearance&gt;&lt;Behavior&gt;&lt;EditTemplateColumnCount&gt;2&lt;/EditTemplateColumnCount&gt;&lt;AutoPostBack&gt;True&lt;/AutoPostBack&gt;&lt;GroupBarText&gt;Drag a column to group by that column.&lt;/GroupBarText&gt;&lt;/Behavior&gt;&lt;Layout&gt;&lt;RowHeaderColumnCount&gt;1&lt;/RowHeaderColumnCount&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;/Layout&gt;&lt;/Categories&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;ColumnFooterRowCount&gt;1&lt;/ColumnFooterRowCount&gt;&lt;PrintInfo&gt;&lt;Header /&gt;&lt;Footer /&gt;&lt;ZoomFactor&gt;0&lt;/ZoomFactor&gt;&lt;FirstPageNumber&gt;1&lt;/FirstPageNumber&gt;&lt;Orientation&gt;Auto&lt;/Orientation&gt;&lt;PrintType&gt;All&lt;/PrintType&gt;&lt;PageOrder&gt;Auto&lt;/PageOrder&gt;&lt;BestFitCols&gt;False&lt;/BestFitCols&gt;&lt;BestFitRows&gt;False&lt;/BestFitRows&gt;&lt;PageStart&gt;-1&lt;/PageStart&gt;&lt;PageEnd&gt;-1&lt;/PageEnd&gt;&lt;ColStart&gt;-1&lt;/ColStart&gt;&lt;ColEnd&gt;-1&lt;/ColEnd&gt;&lt;RowStart&gt;-1&lt;/RowStart&gt;&lt;RowEnd&gt;-1&lt;/RowEnd&gt;&lt;ShowBorder&gt;True&lt;/ShowBorder&gt;&lt;ShowGrid&gt;True&lt;/ShowGrid&gt;&lt;ShowColor&gt;True&lt;/ShowColor&gt;&lt;ShowColumnHeader&gt;Inherit&lt;/ShowColumnHeader&gt;&lt;ShowRowHeader&gt;Inherit&lt;/ShowRowHeader&gt;&lt;ShowColumnFooter&gt;Inherit&lt;/ShowColumnFooter&gt;&lt;ShowColumnFooterEachPage&gt;True&lt;/ShowColumnFooterEachPage&gt;&lt;ShowTitle&gt;True&lt;/ShowTitle&gt;&lt;ShowSubtitle&gt;True&lt;/ShowSubtitle&gt;&lt;UseMax&gt;True&lt;/UseMax&gt;&lt;UseSmartPrint&gt;False&lt;/UseSmartPrint&gt;&lt;Opacity&gt;255&lt;/Opacity&gt;&lt;PrintNotes&gt;None&lt;/PrintNotes&gt;&lt;Centering&gt;None&lt;/Centering&gt;&lt;RepeatColStart&gt;-1&lt;/RepeatColStart&gt;&lt;RepeatColEnd&gt;-1&lt;/RepeatColEnd&gt;&lt;RepeatRowStart&gt;-1&lt;/RepeatRowStart&gt;&lt;RepeatRowEnd&gt;-1&lt;/RepeatRowEnd&gt;&lt;SmartPrintPagesTall&gt;1&lt;/SmartPrintPagesTall&gt;&lt;SmartPrintPagesWide&gt;1&lt;/SmartPrintPagesWide&gt;&lt;HeaderHeight&gt;-1&lt;/HeaderHeight&gt;&lt;FooterHeight&gt;-1&lt;/FooterHeight&gt;&lt;/PrintInfo&gt;&lt;TitleInfo class=&quot;FarPoint.Web.Spread.TitleInfo&quot;&gt;&lt;Style class=&quot;FarPoint.Web.Spread.StyleInfo&quot;&gt;&lt;BackColor&gt;#e7eff7&lt;/BackColor&gt;&lt;HorizontalAlign&gt;Right&lt;/HorizontalAlign&gt;&lt;/Style&gt;&lt;/TitleInfo&gt;&lt;LayoutTemplate class=&quot;FarPoint.Web.Spread.LayoutTemplate&quot;&gt;&lt;Layout&gt;&lt;ColumnCount&gt;4&lt;/ColumnCount&gt;&lt;RowCount&gt;1&lt;/RowCount&gt;&lt;/Layout&gt;&lt;Data&gt;&lt;LayoutData class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;Cells&gt;&lt;Cell row=&quot;0&quot; column=&quot;0&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;0&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;1&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;1&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;2&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;2&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;3&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;3&lt;/Data&gt;&lt;/Cell&gt;&lt;/Cells&gt;&lt;/LayoutData&gt;&lt;/Data&gt;&lt;AxisModels&gt;&lt;LayoutColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/LayoutColumn&gt;&lt;LayoutRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot; /&gt;&lt;/Items&gt;&lt;/LayoutRow&gt;&lt;/AxisModels&gt;&lt;/LayoutTemplate&gt;&lt;LayoutMode&gt;CellLayoutMode&lt;/LayoutMode&gt;&lt;CurrentPageIndex type=&quot;System.Int32&quot;&gt;0&lt;/CurrentPageIndex&gt;&lt;/Settings&gt;&lt;/Sheet&gt;"
                                    EditTemplateColumnCount="2" GridLineColor="#D0D7E5" GroupBarText="Drag a column to group by that column."
                                    SelectionBackColor="#EAECF5">
                                </FarPoint:SheetView>
                            </Sheets>
                            <TitleInfo BackColor="#E7EFF7" Font-Size="X-Large" ForeColor="" HorizontalAlign="Center"
                                VerticalAlign="NotSet" />
                        </FarPoint:FpSpread>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btngen" Visible="false" CssClass="textbox textbox1" Style="width: auto;
                            height: auto;" runat="server" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                            Text="Generate" OnClick="btngen_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnprintpdf" Visible="false" CssClass="textbox textbox1" Style="width: auto;
                            height: auto;" runat="server" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                            Text="Print PDF" OnClick="btnprintpdf_Click" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </td>
                    <td>
                        <asp:Label ID="lblerror" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
            <div id="divPhasing" runat="server" visible="false" style="margin: 0px; margin-bottom: 10px;
                margin-top: 10px;">
                <table>
                    <tr>
                        <td>
                            <FarPoint:FpSpread ID="FpPhasing" AutoPostBack="false" Width="900px" runat="server"
                                Visible="true" BorderStyle="Solid" BorderWidth="0px" CssClass="spreadborder"
                                OnButtonCommand="FpPhasing_OnUpdateCommand" Style="width: 100%; height: auto;
                                margin: 0px; margin-bottom: 10px; margin-top: 10px;" ShowHeaderSelection="false">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <center>
                                <div id="rptprint1" runat="server" visible="false" style="margin: 20px;">
                                    <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                                    <asp:Label ID="lblrptname1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Report Name"></asp:Label>
                                    <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox textbox1" Height="20px"
                                        Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                        onkeypress="display1()" Font-Size="Medium"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtexcelname1"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                        InvalidChars="/\">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Button ID="btnExcel1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        OnClick="btnExcel1_Click" Font-Size="Medium" Text="Export To Excel" CssClass="textbox textbox1"
                                        Style="width: auto; height: auto;" />
                                    <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" CssClass="textbox textbox1"
                                        Style="width: auto; height: auto;" />
                                    <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
                                    <asp:Button ID="btnPrintPhasing" runat="server" Text="Phasing Sheet" OnClick="btnPrintPhasing_Click"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" CssClass="textbox textbox1"
                                        Style="width: auto; height: auto;" />
                                    <asp:Button ID="btnQPaperPacking" runat="server" Text="QPaper Packing" OnClick="btnQPaperPacking_Click"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" CssClass="textbox textbox1"
                                        Style="width: auto; height: auto;" />
                                </div>
                            </center>
                        </td>
                    </tr>
                </table>
            </div>
        </center>
        <%-- Pop Alert--%>
        <center>
            <div id="imgAlert" runat="server" visible="false" style="height: 100em; z-index: 100000;
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
                                        <asp:Label ID="lblAlert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btnPopupClose" CssClass=" textbox btn2 textbox1" Style="height: 28px;
                                                width: 65px;" OnClick="btnPopupClose_Click" Text="Ok" runat="server" />
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
</asp:Content>
