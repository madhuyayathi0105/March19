<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="moderation Report.aspx.cs" Inherits="moderation_Report" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function display1() {
            document.getElementById('<%#lbl_norec1.ClientID %>').innerHTML = "";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="Script1" runat="server">
    </asp:ScriptManager>
    <center>
        <div style="width: 960px; height: 30px; margin: 0 auto; text-align: right;">
            <center>
                <span class="fontstyleheader" style="color: Green;">Moderation Analysis Report</span>
            </center>
        </div>
        <div id="div1" runat="server" style="width: auto; height: auto; padding: 10px; margin: 0px;
            position: relative; margin-top: 10px; margin-bottom: 10px;">
            <table id="First" runat="server" style="width: auto; padding: 10px; height: auto;
                margin: 0px; -webkit-border-radius: 10px; -moz-border-radius: 10px; background-color: #0CA6CA;">
                <tr>
                    <td colspan="8">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblclg" runat="server" Text="College" font-name="Book Antiqua" Font-Size="Medium"
                                        Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcollege" runat="server" AutoPostBack="true" Style="width: 230px;"
                                        Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" Height="25px" OnSelectedIndexChanged="ddlcollege_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblyr" runat="server" Text="Year" font-name="Book Antiqua" Font-Size="Medium"
                                        Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlyear" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                                        Font-Bold="true" Font-Size="Medium" Height="25px" OnSelectedIndexChanged="ddlyear_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblmon" runat="server" Text="Exam Month" font-name="Book Antiqua"
                                        Font-Size="Medium" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlmonth" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                                        Font-Bold="true" Font-Size="Medium" Height="25px" OnSelectedIndexChanged="ddlmonth_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:CheckBox ID="checkregular" runat="server" AutoPostBack="true" OnCheckedChanged="checkregular_OnCheckedChanged"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Text="Regular" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="Checkarrear" runat="server" AutoPostBack="true" OnCheckedChanged="Checkarrear_OnCheckedChanged"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Text="Arrear" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblbatch" runat="server" Text="Batch" font-name="Book Antiqua" Font-Size="Medium"
                                        Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlbatch" runat="server" AutoPostBack="true" Style="width: 59px;"
                                        Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" Height="25px" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbldeg" runat="server" Text="Degree" font-name="Book Antiqua" Font-Size="Medium"
                                        Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddldegree" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                                        Font-Bold="true" Font-Size="Medium" Height="25px" OnSelectedIndexChanged="ddldegree_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbldept" runat="server" Text="Dept" font-name="Book Antiqua" Font-Size="Medium"
                                        Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddldept" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                                        Font-Bold="true" Font-Size="Medium" Height="25px" Width="180px" OnSelectedIndexChanged="ddldept_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblsem" runat="server" Text="Sem" font-name="Book Antiqua" Font-Size="Medium"
                                        Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlsem" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                                        Font-Bold="true" Font-Size="Medium" Height="25px" OnSelectedIndexChanged="ddlsem_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblsubject" runat="server" Text="Subject" font-name="Book Antiqua"
                                        Font-Size="Medium" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="updsubject" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtsubject" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                                Width="110px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium">---Select---</asp:TextBox>
                                            <asp:Panel ID="psubject" runat="server" CssClass="MultipleSelectionDDL" BackColor="White"
                                                BorderColor="Black" BorderStyle="Solid" Height="200" ScrollBars="Auto">
                                                <asp:CheckBox ID="chksubject" runat="server" Font-Bold="True" OnCheckedChanged="chksubject_ChekedChange"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                <asp:CheckBoxList ID="chklssubject" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklssubject_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtsubject"
                                                PopupControlID="psubject" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btngo" CssClass="btnapprove2" runat="server" Font-Bold="true" Style="height: 27px"
                                        Text="Go" Font-Size="Medium" Font-Names="Book Antiqua" OnClientClick="return validation()"
                                        OnClick="btngo_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCollegeHeaderName" runat="server" Text="College Header Name" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Font-Bold="True"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCollegeHeader" Text="" Font-Bold="true" Width="250px" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkShowNoteDescription" runat="server" Text="Show Note Description"
                                        Font-Names="Book Antiqua" Checked="true" Font-Size="Medium" Font-Bold="True" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkSubjectNameWithSubjectCode" runat="server" Text="Show Subject Name with Subject Code"
                                        Font-Names="Book Antiqua" Checked="false" Font-Size="Medium" Font-Bold="True" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblReportName" runat="server" Text="Report Name" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Font-Bold="True"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReportName" Text="" Font-Bold="true" Width="250px" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkReportWithStream" runat="server" Text="Report Name with Stream"
                                        Font-Names="Book Antiqua" Checked="false" Font-Size="Medium" Font-Bold="True" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <asp:Label ID="lblmsg" runat="server" Text="No Record Found" ForeColor="Red" Font-Bold="true"
        Visible="false" Font-Size="Medium" Font-Names="Book Antiqua" Style="margin: 0px;
        margin-bottom: 10px; margin-top: 10px; position: relative;"></asp:Label>
    <center>
        <FarPoint:FpSpread ID="FpSpread2" runat="server" Width="800px" BorderWidth="1px"
            BorderColor="White" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
            position: relative;">
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
        <div style="margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">
            <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Text="Report Name"></asp:Label>
            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Visible="false"
                onkeypress="display1()"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+\}{][':;?,.">
            </asp:FilteredTextBoxExtender>
            <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Visible="false" OnClick="btnxl_Click" />
            <asp:Button ID="btnprintmaster" runat="server" Text="Print" Font-Names="Book Antiqua"
                Font-Size="Medium" Font-Bold="true" Visible="false" OnClick="btnprintmaster_Clcik" />
            <insproplus:printmaster runat="server" id="Printcontrol" visible="false" />
        </div>
    </center>
    <center>
        <div id="divPopupAlert" runat="server" visible="false" style="height: 200%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="divAlert" runat="server" class="table" style="background-color: White; height: auto;
                    width: 507px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: auto; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblPopAlertMsg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnPopAlertClose" CssClass=" textbox btn1 comm" Font-Bold="true"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 28px; width: 65px;"
                                            OnClick="btnPopAlertClose_Click" Text="Ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
</asp:Content>
