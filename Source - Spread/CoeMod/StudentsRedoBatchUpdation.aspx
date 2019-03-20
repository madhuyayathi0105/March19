<%@ Page Title="Student Redo/Repeat Semester Registartion" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudentsRedoBatchUpdation.aspx.cs" Inherits="StudentsRedoBatchUpdation"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        function display1() {
            document.getElementById('<%#lbl_norec1.ClientID %>').innerHTML = "";
        }

        var checkedId = false;
        function OnGridHeaderSelected() {
            var id = document.getElementById("<%=gridDetails.ClientID %>");
            var len = id.rows.length;
            var i = 0;
            var checkedId = id.rows[0].getElementsByTagName("input")[0].checked;
            for (var ak = 1; ak < len; ak++) {
                if (id.rows[ak].getElementsByTagName("input")[i].type == "checkbox") {
                    if (checkedId == true) {
                        id.rows[ak].getElementsByTagName("input")[i].checked = true;
                    } else {
                        id.rows[ak].getElementsByTagName("input")[i].checked = false;
                    }
                }
            }
        }
        function OnGenerateSelectCheck() {
            var id = document.getElementById("<%=gridDetails.ClientID %>");
            var len = id.rows.length;
            var i = 0;
            for (var ak = 1; ak < len; ak++) {
                if (id.rows[ak].getElementsByTagName("input")[i].type == "checkbox") {
                    var checkedId = id.rows[ak].getElementsByTagName("input")[i].checked;
                    if (checkedId == true) {
                        return true;
                    }
                }
            }
            alert('Please select any record');
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <asp:Label ID="lblHeader" CssClass="fontstyleheader" runat="server" Text="Student Redo/Repeat Semester Registartion"
            Font-Bold="true" ForeColor="Green" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
            position: relative;"></asp:Label>
    </center>
    <center>
        <table class="maintablestyle" style="width: auto; height: auto; background-color: #0CA6CA;
            padding: 5px; margin: 0px; margin-bottom: 15px; margin-top: 10px; position: relative;">
            <tr>
                <td>
                    <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="True" Style="font-family: 'Book Antiqua';"
                        ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCollege" runat="server" CssClass="dropdown" Style="font-family: 'Book Antiqua';"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="150px" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblBatch" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                        runat="server" Text="Batch"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlBatch" runat="server" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"
                        AutoPostBack="true" Width="90px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblDegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDegree" runat="server" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged"
                        AutoPostBack="true" Width="80px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblBranch" runat="server" Text="Branch" Font-Bold="True" ForeColor="Black"
                        Font-Size="Medium" Font-Names="Book Antiqua" AutoPostBack="true"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlBranch" runat="server" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                        AutoPostBack="true" Width="150px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblSem" runat="server" Text="Sem" Font-Bold="True" ForeColor="Black"
                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSem" runat="server" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlSem_SelectedIndexChanged"
                        AutoPostBack="true" Width="50px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="10">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblSec" runat="server" Text="Section" Font-Bold="True" ForeColor="Black"
                                    Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <div style="position: relative;">
                                    <asp:UpdatePanel ID="UpnlSection" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtSection" Width=" 70px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="pnlSection" runat="server" CssClass="multxtpanel" Style="width: auto;
                                                height: 120px; overflow: auto;">
                                                <asp:CheckBox ID="chkSection" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkSection_CheckedChanged" />
                                                <asp:CheckBoxList ID="cblSection" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblSection_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popubExtSection" runat="server" TargetControlID="txtSection"
                                                PopupControlID="pnlSection" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkSearchBy" runat="server" Checked="false" Text="" AutoPostBack="true"
                                    OnCheckedChanged="chkSearchBy_CheckedChanged" />
                            </td>
                            <td colspan="4">
                                <div id="divSearch" runat="server" visible="true">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblSearchBy" runat="server" Text="Search By" Font-Bold="True" ForeColor="Black"
                                                    Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlSearchBy" runat="server" CssClass="dropdown" Style="font-family: 'Book Antiqua';"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddlSearchBy_SelectedIndexChanged">
                                                    <asp:ListItem Selected="True" Text="Roll No" Value="0"></asp:ListItem>
                                                    <asp:ListItem Selected="False" Text="Register No" Value="1"></asp:ListItem>
                                                    <asp:ListItem Selected="False" Text="Admission No" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSearch" runat="server" Text="Roll No" Font-Bold="True" ForeColor="Black"
                                                    Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSearchBy" Width=" 150px" Text="" runat="server" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox  txtheight2"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                            <td>
                                <asp:Button ID="btnGo" runat="server" Style="width: auto; height: auto;" Font-Bold="True"
                                    Font-Size="Medium" Font-Names="Book Antiqua" CssClass="textbox btn2" Text="Go"
                                    OnClick="btnGo_Click" />
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkRedoCompletion" runat="server" Text="Redo Completion" OnClick="lnkRedoCompletion_Click"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px;
            margin-bottom: 10px; margin-top: 10px; position: relative;"></asp:Label>
        <center>
            <div id="divMainContent" visible="false" runat="server" style="margin: 0px; margin-bottom: 20px;
                margin-top: 20px;">
                <div id="divAllocatePart" runat="server">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblRedoBatch" Visible="true" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" runat="server" Text="Redo Batch Year"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRedoBatch" Visible="false" Font-Size="Medium" Font-Bold="True"
                                    Font-Names="Book Antiqua" Width=" 64px" runat="server" Text="" CssClass="textbox  txtheight2"
                                    MaxLength="2"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtRedoBatch"
                                    FilterType="Numbers">
                                </asp:FilteredTextBoxExtender>
                                <asp:DropDownList ID="ddlRedoBatch" runat="server" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" Width="60px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblRedoSem" Visible="true" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" runat="server" Text="Redo Semester"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRedoSem" Visible="false" Font-Size="Medium" Font-Bold="True"
                                    Font-Names="Book Antiqua" Width=" 64px" runat="server" Text="" CssClass="textbox  txtheight2"
                                    MaxLength="2"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtRedoSem"
                                    FilterType="Numbers">
                                </asp:FilteredTextBoxExtender>
                                <%-- <asp:DropDownList ID="ddlRedoSem" runat="server" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" Width="50px">
                                </asp:DropDownList>--%>
                                <asp:Label ID="lblRedoSemester" Visible="true" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" runat="server" Text="" Style="color: Green; padding: 5px;"></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="btnSet" CssClass="textbox textbox1" Visible="true" runat="server"
                                    Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua" Style="width: auto;
                                    height: auto;" Text="Set" OnClick="btnSet_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <FarPoint:FpSpread ID="FpRedoStudentsList" autopostback="false" Width="1000px" runat="server"
                    Visible="true" BorderStyle="Solid" BorderWidth="0px" CssClass="spreadborder"
                    ShowHeaderSelection="false" OnButtonCommand="FpRedoStudentsList_Command" Style="width: 100%;
                    height: auto; margin: 0px; margin-bottom: 10px; margin-top: 10px; padding: 0px;
                    position: relative;">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <div id="divPrint1" runat="server" style="margin: 0px; margin-bottom: 20px; margin-top: 20px;
                    text-align: center;">
                    <center>
                        <table style="margin: 0px; margin-bottom: 20px; margin-top: 20px; text-align: center;">
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblrptname1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Report Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox textbox1" Height="20px"
                                        Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                        onkeypress="display1()" Font-Size="Medium"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtexcelname1"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                        InvalidChars="/\">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Button ID="btnExcel1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        OnClick="btnExcel1_Click" Font-Size="Medium" Style="width: auto; height: auto;"
                                        Text="Export To Excel" CssClass="textbox textbox1" />
                                </td>
                                <td>
                                    <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Style="width: auto;
                                        height: auto;" CssClass="textbox textbox1" />
                                    <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
                                </td>
                                <td>
                                    <asp:Button ID="btnSave" Visible="true" CssClass="textbox textbox1" runat="server"
                                        Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua" Style="width: auto;
                                        height: auto;" Text="Save" OnClick="btnSave_Click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </div>
        </center>
    </center>
    <center>
        <div id="divPopupAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%; right: 0%;">
            <center>
                <div id="divAlertContent" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblAlertMsg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <center>
                                        <asp:Button ID="btnPopAlertClose" runat="server" CssClass=" textbox btn2 comm" Font-Size="Medium"
                                            Font-Bold="True" Font-Names="Book Antiqua" Style="height: auto; width: auto;"
                                            OnClick="btnPopAlertClose_Click" Text="Ok" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <%--  ******popup window******--%>
    <center>
        <div id="popwindow" runat="server" visible="false" class="popupstyle popupheight1 ">
            <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: 15px; margin-left: 450px;"
                OnClick="imagebtnpopclose_Click" />
            <br />
            <div style="background-color: White; height: 530px; width: 950px; border: 5px solid #0CA6CA;
                border-top: 30px solid #0CA6CA; border-radius: 10px;">
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: Green;">Redo Completion</span></div>
                </center>
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lblCollege1" runat="server" Text="College"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_college1" Width="200px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_college1_OnIndexChange">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_SearchBy" runat="server" Text="Search By"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddl_searchBy" Width="120px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_searchBy_OnIndexChange">
                                <asp:ListItem Selected="True">Adm No</asp:ListItem>
                                <asp:ListItem>Student Name</asp:ListItem>
                                <asp:ListItem>Roll No</asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="txt_SearchBy" runat="server" CssClass="textbox  txtheight1" Width="190px"
                                MaxLength="45" Placeholder="Adm No">
                            </asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="fceSear" runat="server" FilterType="UppercaseLetters,LowercaseLetters, Numbers,Custom"
                                ValidChars=" ." TargetControlID="txt_SearchBy">
                            </asp:FilteredTextBoxExtender>
                            <asp:AutoCompleteExtender ID="autocomplete_rollno" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetSearch" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_SearchBy"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:Button ID="btn_go1" Text="Go" OnClick="btn_go1_Click" CssClass="textbox btn1 textbox1"
                                runat="server" />
                        </td>
                        <td>
                            <asp:Button ID="btnSaveRedoComplete" Text="Save" OnClientClick="return OnGenerateSelectCheck()"
                                OnClick="btnSaveRedoComplete_Click" CssClass="textbox btn1 textbox1" runat="server"
                                Visible="false" />
                        </td>
                    </tr>
                </table>
                <br />
                <div>
                    <asp:GridView ID="gridDetails" runat="server" AutoGenerateColumns="false" GridLines="Both"
                        Visible="false">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_serial" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="cb_selectHead" runat="server" onchange="return OnGridHeaderSelected()">
                                    </asp:CheckBox>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb_select" runat="server"></asp:CheckBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Roll No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_RollNo" runat="server" Text='<%#Eval("roll_no") %>'></asp:Label>
                                    <asp:Label ID="lbl_AppNo" runat="server" Text='<%#Eval("app_no") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reg No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_RegNo" runat="server" Text='<%#Eval("reg_no") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Admission No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_AdmNo" runat="server" Text='<%#Eval("roll_admit") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Student Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_StudName" runat="server" Text='<%#Eval("stud_name") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Batch" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Batch" runat="server" Text='<%#Eval("batch_year") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Semester" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Sem" runat="server" Text='<%#Eval("semester") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Department" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Degree" runat="server" Text='<%#Eval("Department") %>'></asp:Label>
                                    <asp:Label ID="lbl_DegreeCode" runat="server" Text='<%#Eval("degree_code") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </center>
</asp:Content>
