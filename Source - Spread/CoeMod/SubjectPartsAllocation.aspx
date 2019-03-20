<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="SubjectPartsAllocation.aspx.cs" EnableEventValidation="false"
    Inherits="SubjectPartsAllocation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style11
        {
            width: 68px;
            height: 2px;
        }
        .style14
        {
            height: 2px;
            width: 73px;
        }
        .style33
        {
            height: 2px;
            width: 65px;
        }
        .style34
        {
            height: 2px;
        }
        .style35
        {
            height: 2px;
            width: 138px;
        }
        .style36
        {
            height: 2px;
            width: 54px;
        }
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        .HellowWorldPopup
        {
            min-width: 600px;
            min-height: 400px;
            background: white;
        }
        .style37
        {
            height: 21px;
            width: 174px;
        }
        .style38
        {
            height: 21px;
            width: 171px;
        }
        .style39
        {
            height: 21px;
            width: 35px;
        }
        .style40
        {
            height: 27px;
            width: 44px;
        }
        .style41
        {
            height: 33px;
            width: 172px;
        }
        .style42
        {
            width: 34px;
            height: 25px;
        }
        .style43
        {
            height: 19px;
            width: 168px;
        }
        .style44
        {
            height: 21px;
            width: 126px;
        }
        .style45
        {
            height: 22px;
            width: 55px;
        }
        .style46
        {
        }
        .style47
        {
            height: 21px;
        }
        .style48
        {
            width: 34px;
        }
        .style49
        {
            height: 21px;
            width: 303px;
        }
        .style50
        {
            height: 21px;
            width: 329px;
        }
        .style51
        {
            width: 1169px;
        }
        .style52
        {
            height: 73px;
            width: 1017px;
        }
        .style53
        {
            width: 10px;
        }
        .style54
        {
            width: 179px;
            height: 21px;
        }
        .style55
        {
            height: 21px;
            width: 76px;
        }
    </style>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <script type="text/javascript">
        function display1() {
            document.getElementById('<%#lbl_norec1.ClientID %>').innerHTML = "";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <asp:Label ID="lbl" CssClass="fontstyleheader" runat="server" Text="Subject's Part Allocation"
            Font-Bold="true" ForeColor="Green"></asp:Label>
    </center>
    <center>
        <table class="maintablestyle" style="width: auto; height: auto; background-color: #0CA6CA;
            padding: 5px; margin: 0px; margin-bottom: 15px; margin-top: 10px; position: relative;">
            <tr>
                <td colspan="5">
                    <asp:RadioButtonList ID="rblCourse" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="rbCourse_SelectedIndexChanged"
                        RepeatDirection="Horizontal">
                    </asp:RadioButtonList>
                </td>
                <td colspan="5">
                    <asp:RadioButtonList ID="rblOptions" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="rblOptional_SelectedIndexChanged"
                        RepeatDirection="Horizontal">
                        <asp:ListItem Selected="true" Text="Subject Wise" Value="0"></asp:ListItem>
                        <asp:ListItem Selected="False" Text="Subject Type Wise" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Lblbatch" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="80px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="LblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="110px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblsem" runat="server" Text="Semester" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="10">
                    <table>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkEnableSubjectType" AutoPostBack="true" runat="server" Text=""
                                    Checked="true" OnCheckedChanged="chkEnableSubjectType_CheckedChanged" />
                            </td>
                            <td>
                                <asp:Label ID="lbl_subject_type" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                    runat="server" Text="Subject Types"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_SubType" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="100px" Height="30px" CssClass="arrow" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddl_SubType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkEnableSubject" runat="server" AutoPostBack="true" Text="" Checked="true"
                                    OnCheckedChanged="chkEnableSubject_CheckedChanged" />
                            </td>
                            <td>
                                <asp:Label ID="lblSubject" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                    runat="server" Text="Subject"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Updp_Degree" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_Subject" Width=" 120px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel_Subjects" runat="server" CssClass="multxtpanel" Height="300px">
                                            <asp:CheckBox ID="cb_Subjects" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_Subjects_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_Subjects" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_Subjects_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_Subject"
                                            PopupControlID="Panel_Subjects" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_noofpart" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" runat="server" Text="No. of Parts"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_noofpart" Visible="false" Font-Size="Medium" Font-Bold="True"
                                    Font-Names="Book Antiqua" Width=" 64px" runat="server" Text="" CssClass="textbox  txtheight2"
                                    MaxLength="2" OnTextChanged="txt_noofpart_TextChanged" AutoPostBack="true"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_noofpart"
                                    FilterType="Numbers">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_Parts" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" runat="server" Text="Parts"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_Parts" Visible="false" runat="server" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" CssClass="arrow" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddl_Parts_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnView" CssClass="textbox textbox1" runat="server" Font-Bold="True"
                                    Font-Size="Medium" Font-Names="Book Antiqua" Style="width: auto; height: auto;"
                                    Text="View" OnClick="btnView_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnAllocate" CssClass="textbox textbox1" Visible="false" runat="server"
                                    Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua" Style="width: auto;
                                    height: auto;" Text="Allocate" OnClick="btnAllocate_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </center>
    <asp:Label ID="errmsg" runat="server" Text="" ForeColor="Red" Visible="False" Font-Bold="True"
        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
    <center>
        <div id="divSubjectDetails" visible="false" runat="server" style="margin: 0px; margin-bottom: 20px;
            margin-top: 20px;">
            <div id="divAllocatePart" runat="server">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblSelectPart" Visible="true" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" runat="server" Text="Part Number"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSelectPart" Visible="true" Font-Size="Medium" Font-Bold="True"
                                Font-Names="Book Antiqua" Width=" 64px" runat="server" Text="" CssClass="textbox  txtheight2"
                                MaxLength="2"></asp:TextBox>
                            <%--OnTextChanged="txtSelectPart_TextChanged" AutoPostBack="true"--%>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtSelectPart"
                                FilterType="Numbers">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:Button ID="btnSetPart" CssClass="textbox textbox1" Visible="true" runat="server"
                                Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua" Style="width: auto;
                                height: auto;" Text="Set Part To Selected Subjects" OnClick="btnSetPart_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <FarPoint:FpSpread ID="FpSubjectsList" autopostback="false" Width="1000px" runat="server"
                Visible="true" BorderStyle="Solid" BorderWidth="0px" CssClass="spreadborder"
                ShowHeaderSelection="false" Style="width: 100%; height: auto; margin: 0px; margin-bottom: 10px;
                margin-top: 10px; padding: 0px;">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            <div id="divPrint1" runat="server" style="margin: 0px; margin-bottom: 20px; margin-top: 20px;">
                <table>
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
                            <asp:Button ID="btnSavePart" Visible="true" CssClass="textbox textbox1" runat="server"
                                Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua" Style="width: auto;
                                height: auto;" Text="Save Subject Part Allocation" OnClick="btnSavePart_Click" />
                        </td>
                        <%-- <td>
             <asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
            </td>--%>
                    </tr>
                </table>
            </div>
        </div>
    </center>
    <div id="divPopUpAlert" runat="server" visible="false" style="height: 58em; z-index: 2000;
        width: 100%; top: 0%; right: 0%; left: 0%; position: absolute; background-color: rgba(54, 25, 25, .2);">
        <center>
            <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                width: 238px; border: 5px solid #0CA6CA; border-radius: 10px; margin-top: 200px;">
                <center>
                    <table style="height: 100px; width: 100%">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblPopAlert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                    Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center>
                                    <asp:Button ID="btnPopAlertClose" runat="server" CssClass=" textbox btn1 comm" Font-Size="Medium"
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
</asp:Content>
