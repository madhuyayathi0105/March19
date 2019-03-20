<%@ Page Title="COE Exam Mark Entry III" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ExamValidatorMarksNew.aspx.cs" Inherits="ExamValidatorMarksNew" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <asp:Panel ID="header_Panel" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg"
            Style="width: 1240px; height: 21px">
            <center>
                <asp:Label ID="lbl_Header" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="White" Text="COE Exam Mark Entry III"></asp:Label>
            </center>
        </asp:Panel>
        <table>
            <tr>
                <td colspan="10">
                    <table>
                        <tr>
                          <td>
                                <asp:Label ID="lblCollege" runat="server" Text="College" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCollege"  Width="100px" runat="server"
                                    Font-Bold="True" Font-Names="Book Antiqua"  AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblmonthYear1" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Year And Month"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlYear1" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                    OnSelectedIndexChanged="ddlYear1_SelectedIndexChanged" Font-Size="Medium" Width="60px"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMonth1" runat="server" CssClass="font" OnSelectedIndexChanged="ddlMonth1_SelectedIndexChanged"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="60px" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <div id="UpdatePanel24" runat="server">
                                    <asp:Label ID="lblBundleNo" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Bundle No"></asp:Label>
                                    <asp:TextBox ID="txtbundleno" runat="server" placeholder="Bundle No" CssClass="textbox  txtheight2"
                                        OnTextChanged="txtBundleNo_TextChanged"></asp:TextBox>
                                    <%--OnTextChanged="txtroll_staff_Changed"--%>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getbundleno" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtbundleno"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </div>
                            </td>
                            <td>
                                <asp:Label ID="lbltype" Text="Stream" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddltype" runat="server" Width="128px" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldegree1" runat="server" CssClass="font" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Width="101px" OnSelectedIndexChanged="ddldegree1_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Branch"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbranch1" runat="server" CssClass="font" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Width="160px" OnSelectedIndexChanged="ddlbranch1_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
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
                                <asp:CheckBox ID="chkmarkbased" runat="server" Width="198px" Text="With Out Application"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true"
                                    OnCheckedChanged="chksubwise_CheckedChanged" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chk_onlycia" runat="server" Enabled="false" Text="I.C.A Mark" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True" AutoPostBack="true" OnCheckedChanged="chksubwise_CheckedChanged" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chksubwise" runat="server" Text="Subject Wise" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True" AutoPostBack="true" OnCheckedChanged="chksubwise_CheckedChanged" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chkicaretake" runat="server" Text="ICA Repeat/Retake" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True" AutoPostBack="true" OnCheckedChanged="chksubwise_CheckedChanged" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chkonlyica" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Only ICA" AutoPostBack="true" OnCheckedChanged="chksubwise_CheckedChanged" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chkbundleno" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Bundle No" AutoPostBack="True" OnCheckedChanged="chkbundleno_CheckedChanged" />
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
                                <asp:Label ID="Label3" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Sem"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsem1" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="90px" OnSelectedIndexChanged="ddlsem1_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblsubtype" runat="server" Text="Subject" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsubtype" AutoPostBack="true" Width="200px" runat="server"
                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlsubtype_SelectedIndexChanged"
                                    Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSubject" AutoPostBack="true" Width="407px" runat="server"
                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged"
                                    Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnviewre" runat="server" Text="View Report" OnClick="btnviewre_Click"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="True" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="width: 1240px;
            height: 20px;">
        </asp:Panel>
    </center>
    <asp:Label ID="lblerr1" runat="server" Font-Bold="True" ForeColor="Red" Font-Names="Book Antiqua"
        Font-Size="Medium" Visible="false"></asp:Label>
    <center>
        <table width="950px" style="margin-top: 10px; margin-bottom: 10px; position: relative;">
            <tr>
                <td>
                    <asp:RadioButton ID="rbeval" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Valuation" GroupName="Import" />
                    <asp:RadioButton ID="rbcia" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="I.C.A" GroupName="Import" />
                    <asp:FileUpload runat="server" ID="fpmarkexcel" />
                    <asp:Button ID="btnexcelimport" runat="server" Font-Bold="true" OnClick="btnexcelimport_click"
                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Import" />
                </td>
                <td colspan="2" style="text-align: right;">
                    <asp:CheckBox ID="chkincluevel2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Maroon" Text="For Single Valuation Only" />
                </td>
            </tr>
            <tr>
                <td align="right" colspan="3">
                    <asp:Button ID="btnsave1" runat="server" Font-Bold="true" OnClick="btnsavel1_click"
                        Font-Names="Book Antiqua" Font-Size="Medium" Visible="false" Text="Save" />
                    <asp:Button ID="btnprintt" runat="server" Font-Bold="true" OnClick="btnprintt_print"
                        Font-Names="Book Antiqua" Font-Size="Medium" Visible="false" Text="Print" />
                    <asp:Button ID="btnreset" runat="server" Font-Bold="true" OnClick="btnreset_print"
                        Font-Names="Book Antiqua" Font-Size="Medium" Visible="false" Text="Reset" />
                    <asp:CheckBox runat="server" ID="chkmoderation" Font-Names="Book Antiqua" Font-Bold="true"
                        Font-Size="Medium" Text="Apply Moderation" />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="3">
                    <asp:Label ID="lblaane" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Maroon" Text="Note:Please Enter If  AB: Absent, NR: Not Registered, NE:Not Entered, M: Mal Practice, LT: Discontinue"></asp:Label>
                </td>
            </tr>
        </table>
        <FarPoint:FpSpread ID="fpmarkimport" runat="server" BorderColor="Black" BorderStyle="Solid"
            BorderWidth="1px" Visible="false" VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never"
            CssClass="stylefp">
            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                ButtonShadowColor="ControlDark">
            </CommandBar>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
        <FarPoint:FpSpread ID="fpspread" runat="server" BorderColor="Black" BorderStyle="Solid"
            EnableClientScript="true" BorderWidth="1px" Visible="true" VerticalScrollBarPolicy="Never"
            HorizontalScrollBarPolicy="Never" CssClass="stylefp">
            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                ButtonShadowColor="ControlDark">
            </CommandBar>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" GridLineColor="Black"
                    GroupBarText="Drag a column to group by that column." SelectionBackColor="#CE5D5A"
                    SelectionForeColor="White">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
    </center>
    <center>
        <div id="imgdiv2" runat="server" visible="false" style="height: 200%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="pnl2" runat="server" class="table" style="background-color: White; height: auto;
                    width: 507px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: auto; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Style="height: 28px; width: 65px;" OnClick="btn_errorclose_Click"
                                            Text="Ok" runat="server" />
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
