<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Examvalidatormark.aspx.cs" Inherits="Examvalidatormark" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div class="maindivstyle">
            <span style="color: Green; font-size: large;" class=" fontstyleheader">Exam Mark Subject
                Wise Report</span>
            <table class="maintablestyle">
                <tr>
                    <td>
                        <asp:Label ID="lblmonthYear1" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Year And Month"></asp:Label>
                        <asp:DropDownList ID="ddlYear1" runat="server" CssClass="textbox ddlheight" OnSelectedIndexChanged="ddlYear1_SelectedIndexChanged"
                            Width="60px" AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlMonth1" runat="server" CssClass="textbox ddlheight" OnSelectedIndexChanged="ddlMonth1_SelectedIndexChanged"
                            Width="60px" AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:Label ID="lbltype" Text="Stream" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList ID="ddltype" runat="server" Width="128px" CssClass="textbox ddlheight"
                            AutoPostBack="true" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Label ID="Label1" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList ID="ddldegree1" runat="server" CssClass="textbox ddlheight" Width="100px"
                            OnSelectedIndexChanged="ddldegree1_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:Label ID="Label2" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Branch"></asp:Label>
                        <asp:DropDownList ID="ddlbranch1" runat="server" CssClass="textbox ddlheight" Width="160px"
                            OnSelectedIndexChanged="ddlbranch1_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkmarkbased" runat="server" Width="198px" Text="With Out Application"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true"
                            OnCheckedChanged="chksubwise_CheckedChanged" />
                        <asp:CheckBox ID="chk_onlycia" runat="server" Enabled="false" Text="I.C.A Mark" Font-Names="Book Antiqua"
                            Font-Size="Medium" Font-Bold="True" AutoPostBack="true" OnCheckedChanged="chksubwise_CheckedChanged" />
                        <asp:CheckBox ID="chksubwise" runat="server" Text="Subject Wise" Font-Names="Book Antiqua"
                            Font-Size="Medium" Font-Bold="True" AutoPostBack="true" OnCheckedChanged="chksubwise_CheckedChanged" />
                        <asp:CheckBox ID="chkicaretake" runat="server" Text="ICA Repeat/Retake" Font-Names="Book Antiqua"
                            Font-Size="Medium" Font-Bold="True" AutoPostBack="true" OnCheckedChanged="chksubwise_CheckedChanged" />
                        <asp:CheckBox ID="chkonlyica" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Only ICA" AutoPostBack="true" OnCheckedChanged="chksubwise_CheckedChanged" />
                        <asp:CheckBox ID="ChkBundlewise" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="BundleNo" AutoPostBack="true" OnCheckedChanged="chkBundleNo_CheckedChanged" />
                    </td>
                   
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Sem"></asp:Label>
                        <asp:DropDownList ID="ddlsem1" runat="server" CssClass="textbox ddlheight" Width="90px"
                            OnSelectedIndexChanged="ddlsem1_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:Label ID="lblsubtype" runat="server" Text="Subject" Font-Names="Book Antiqua"
                            Font-Size="Medium" Font-Bold="True"></asp:Label>
                        <asp:DropDownList ID="ddlsubtype" runat="server" AutoPostBack="true" CssClass="textbox ddlheight"
                            Width="200px" OnSelectedIndexChanged="ddlsubtype_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Names="Book Antiqua"
                            Font-Size="Medium" Font-Bold="True"></asp:Label>
                        <asp:DropDownList ID="ddlSubject" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged"
                            CssClass="textbox ddlheight" Width="380px">
                        </asp:DropDownList>
                        <asp:Button ID="btnviewre" runat="server" Text="View Report" OnClick="btnviewre_Click"
                            CssClass="textbox btn" Width="120px" />
                         
                    </td>
                    <td>                <%--<asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_Bundle" runat="server" CssClass="textbox textbox1 txtheight3" ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel11" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_Bundle" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_Bundle_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_bundle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_Bundle_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender20" runat="server" TargetControlID="txt_Bundle"
                                                PopupControlID="Panel11" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                      <div ID="UpdatePanel24" runat="server">
                         <asp:Label ID="lblBundleNo" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Bundle No"></asp:Label>

                            <asp:TextBox ID="txtBundleNo" runat="server" placeholder="Bundle No" CssClass="textbox  txtheight2"></asp:TextBox>  <%--OnTextChanged="txtroll_staff_Changed" AutoPostBack="true"--%>

                         <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="Getbundleno" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtBundleNo"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="panelbackground">
                        </asp:AutoCompleteExtender>
                    </div>
                                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="lblerr1" runat="server" Font-Bold="True" ForeColor="Red" Font-Names="Book Antiqua"
                Font-Size="Medium" Visible="false"></asp:Label>
            <br />
            <table width="950px">
                <tr>
                    <td>
                        <asp:RadioButton ID="rbeval" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Valuation" GroupName="Import" />
                        <asp:RadioButton ID="rbcia" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="I.C.A" GroupName="Import" />
                        <asp:FileUpload runat="server" ID="fpmarkexcel" CssClass="textbox" Height="35px"
                            Width="200px" />
                        <asp:Button ID="btnexcelimport" runat="server" OnClick="btnexcelimport_click" Text="Import"
                            CssClass="textbox btn" Width="80px" />
                    </td>
                    <td style="text-align: right;" colspan="2">
                        <asp:CheckBox ID="chkincluevel2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Maroon" Text="For Single Valuation Only" />
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="3">
                        <asp:Button ID="btnsave1" runat="server" OnClick="btnsavel1_click" CssClass="textbox btn"
                            Width="60px" Visible="false" Text="Save" />
                        <asp:Button ID="btnprintt" runat="server" OnClick="btnprintt_print" CssClass="textbox btn"
                            Width="60px" Visible="false" Text="Print" />
                        <asp:Button ID="btnreset" runat="server" OnClick="btnreset_print" CssClass="textbox btn"
                            Width="60px" Visible="false" Text="Reset" />
                        <asp:CheckBox runat="server" ID="chkmoderation" CssClass="textbox btn" Width="60px"
                            Text="Apply Moderation" />
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="3">
                        <asp:Label ID="lblaane" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Maroon" Text="Note:Please Enter If  AB: Absent, NR: Not Registered, NE:Not Entered, M: Mal Practice, LT: Discontinue"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
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
            <br />
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
                                                <br />
                                                <asp:Button ID="btn_errorclose" CssClass="textbox btn" Width="40px" OnClick="btn_errorclose_Click"
                                                    Text="Ok" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                            <br />
                            <br />
                        </div>
                    </center>
                </div>
            </center>
        </div>
    </center>
</asp:Content>
