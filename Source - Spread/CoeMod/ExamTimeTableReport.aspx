<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ExamTimeTableReport.aspx.cs" Inherits="ExamTimeTableReport"
    EnableEventValidation="false" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lblvalidation1').innerHTML = "";
        }
    </script>
    <style type="text/css">
        .font
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <asp:Label ID="Label7" CssClass="fontstyleheader" runat="server" Text="Exam Time Table Report"
            Font-Bold="True" ForeColor="Green" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
            position: relative;"></asp:Label>
        <table style="width: auto; height: auto; background-color: #0CA6CA; margin: 0px;
            margin-bottom: 10px; margin-top: 10px; position: relative;">
            <tr>
                <td colspan="9">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblMonthandYear" runat="server" Text="Month and Year" CssClass="font"
                                    Width="125px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                                    CssClass="font">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                                    CssClass="font">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:CheckBox ID="cbDate" runat="server" CssClass="font" Text="Date" TextAlign="Right"
                                    AutoPostBack="True" OnCheckedChanged="cbDate_CheckedChanged" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="font" Width="80px" OnTextChanged="txtFromDate_TextChanged"
                                    AutoPostBack="true"></asp:TextBox>
                                <asp:CalendarExtender ID="cetxtexamFromdate" runat="server" TargetControlID="txtFromDate"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:TextBox ID="txtToDate" runat="server" CssClass="font" Width="80px" OnTextChanged="txtToDate_TextChanged"
                                    AutoPostBack="true"></asp:TextBox>
                                <asp:CalendarExtender ID="cetxtExamToDate" runat="server" TargetControlID="txtToDate"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:CheckBox ID="cbBatchYear" runat="server" CssClass="font" Text="Batch/Year" TextAlign="Right"
                                    AutoPostBack="True" OnCheckedChanged="cbBatchYear_CheckedChanged" Width="100px" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlBatchYear" runat="server" CssClass="font" Width="70px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlBatchYear_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:CheckBox ID="cbCourse" runat="server" CssClass="font" Text="Course" TextAlign="Right"
                                    AutoPostBack="True" OnCheckedChanged="cbCourse_CheckedChanged" Width="80px" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCourse" runat="server" CssClass="font" Width="80px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="9">
                    <table>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cbDepartment" runat="server" CssClass="font" Text="Degree" TextAlign="Right"
                                    AutoPostBack="True" OnCheckedChanged="cbDepartment_CheckedChanged" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="font" Width="100px"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:CheckBox ID="cbSubject" runat="server" CssClass="font" Text="Subject Name" TextAlign="Right"
                                    AutoPostBack="True" OnCheckedChanged="cbSubject_CheckedChanged" Width="130px" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSubjectName" runat="server" CssClass="font" Width="150px"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlSubjectName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkindegee" Text="Include Degree Details" AutoPostBack="True" OnCheckedChanged="chkindegee_CheckedChanged"
                                    runat="server" CssClass="font" Width="200px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="9">
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButton ID="rbformat1" runat="server" Text="Format 1" CssClass="font" AutoPostBack="true"
                                    GroupName="Report" OnCheckedChanged="RadioCHange" />
                            </td>
                            <td>
                                <asp:RadioButton ID="RadioButton1" runat="server" Text="Format 2" CssClass="font"
                                    AutoPostBack="true" GroupName="Report" OnCheckedChanged="RadioCHange" />
                            </td>
                            <td>
                                <asp:Button ID="btnView" runat="server" Text="Go" CssClass="font" OnClick="btnView_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </center>
    <asp:Label ID="lblerror" runat="server" CssClass="font" ForeColor="Red" Style="margin: 0px;
        margin-bottom: 10px; margin-top: 10px; position: relative;"></asp:Label>
    <center>
        <table style="margin: 0px; margin-bottom: 15px; margin-top: 10px; position: relative;">
            <tr>
                <td>
                    <FarPoint:FpSpread ID="Fpstudents" Visible="false" runat="server" autopostback="true"
                        Style="height: auto; width: auto; margin: 0px; margin-bottom: 10px; margin-top: 10px;
                        position: relative;" Width="980px">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" AllowPage="false" GridLineColor="Gray">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblvalidation1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Report Name" Visible="false"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Visible="false"
                        onkeypress="display()"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+\}{][':;?,.">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Visible="false" OnClick="btnxl_Click" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="true" Visible="false" OnClick="btnprintmaster_Clcik" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </td>
            </tr>
        </table>
    </center>
</asp:Content>
