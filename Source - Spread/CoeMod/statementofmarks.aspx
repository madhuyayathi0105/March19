<%@ Page Title="MARK SHEET / Consolidated MARK SHEET" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="statementofmarks.aspx.cs" Inherits="statementofmarks"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .cpBody
        {
            background-color: #DCE4F9;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
        }
    </style>
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lblerror').innerHTML = "";
        }   
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <asp:Label ID="Label4" runat="server" Text="MARK SHEET / Consolidated MARK SHEET"
                ForeColor="Green" CssClass="fontstyleheader"></asp:Label>
        </div>
        <div class="maintablestyle" style="color: White; font-family: Book Antiqua; height: auto;
            width: 1100px; margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;
            text-align: left;">
            <table style="width: auto; height: auto;">
                <tr>
                    <td colspan="14">
                        <asp:CheckBox ID="chk_consoli" runat="server" Text="Consolidated Mark Sheet" AutoPostBack="true"
                            OnCheckedChanged="chk_consoli_CheckedChanged" />
                        <asp:CheckBox ID="chk_finalsemmrk_sheet" runat="server" Text="Use For Final Semester Mark Sheet"
                            AutoPostBack="true" OnCheckedChanged="chk_finalsemmrk_sheet_CheckedChanged" />
                        <asp:RadioButtonList ID="rblSpecialorCertify" runat="server" Visible="false" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" OnSelectedIndexChanged="rblSpecialorCertify_SelectedIndexChanged"
                            AutoPostBack="true">
                            <asp:ListItem Selected="False" Text="Special Course" Value="0"></asp:ListItem>
                            <asp:ListItem Selected="False" Text="Certificate Course" Value="1"></asp:ListItem>
                            <asp:ListItem Selected="true" Text="None" Value="2"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:CheckBox ID="chkShowSubjectNameOnly" Visible="false" runat="server" Text="Show Subject Name Only" />
                        <asp:CheckBox ID="chkinstatnt" runat="server" Text="Instant For Supplementary" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"
                            ForeColor="White" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCollege" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Width="180px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Lblbatch" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                            ForeColor="White" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="60px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                            ForeColor="White" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="80px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="LblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                            ForeColor="White" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="95Px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblexamyear" runat="server" Text="Exam Year" Font-Bold="true" Font-Names="Book Antiqua"
                            ForeColor="White" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlYear" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Width="60px" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblexammonth" runat="server" Text="Exam Month" Font-Bold="true" Font-Names="Book Antiqua"
                            ForeColor="White" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMonth" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="60px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td colspan="2">
                    </td>
                </tr>
                <tr>
                    <td colspan="14">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblReportType" runat="server" Text="Report" Font-Bold="True" Font-Names="Book Antiqua"
                                        ForeColor="White" Font-Size="Medium"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlreporttype" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="230px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlreport_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="6">
                                    <table>
                                        <tr>
                                            <td>
                                                <div id="div2" runat="server" visible="false" style="position: relative;">
                                                    <asp:Label ID="lblRoundOFF" runat="server" Visible="true" Text="Round Off" Font-Bold="True"
                                                        Font-Names="Book Antiqua" ForeColor="White" Font-Size="Medium"></asp:Label>
                                                    <asp:TextBox ID="txtRoundOff" Width="85px" MaxLength="1" runat="server" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox  txtheight2"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FEtxtroundoff" runat="server" ValidChars="12" TargetControlID="txtRoundOff"
                                                        FilterType="Custom">
                                                    </asp:FilteredTextBoxExtender>
                                                </div>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSubject" runat="server" Visible="false" Text="Subject" Font-Bold="True"
                                                    Font-Names="Book Antiqua" ForeColor="White" Font-Size="Medium"></asp:Label>
                                                <asp:CheckBox ID="chkenrolmentno" runat="server" Enabled="true" AutoPostBack="true"
                                                    OnCheckedChanged="cbenrolmentno_OnCheckedChanged" Visible="false" Text="Include Enrolment No" />
                                                <asp:CheckBox ID="cbregulation" runat="server" Enabled="true" Visible="false" Text="2015 Regulation" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblmode" runat="server" Visible="false" Text="Mode" Font-Bold="true"
                                                    Font-Names="Book Antiqua" ForeColor="White" Font-Size="Medium" Style="margin-left: 10px"></asp:Label>
                                                <asp:TextBox ID="txtmode" Width="50px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" CssClass="textbox  txtheight2" Visible="false"></asp:TextBox>
                                            </td>
                                            <%--<td>
                                            <asp:Label ID="lblcreditpaper" runat="server" Visible="false" Text="Credits" Font-Bold="true"
                                            Font-Names="Book Antiqua" ForeColor="White" Font-Size="Medium" style="margin-left:10px"></asp:Label>
                                             <asp:TextBox ID="txtcreditpap" Width="50px"  runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" CssClass="textbox  txtheight2" Visible="false"></asp:TextBox>
                                            </td>--%>
                                            <td>
                                                <asp:DropDownList ID="ddlSubject" runat="server" Visible="false" Font-Bold="true"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" Width="100px" CssClass="arrow">
                                                </asp:DropDownList>
                                                <div id="divSubject" runat="server" visible="false" style="position: relative;">
                                                    <asp:UpdatePanel ID="upnlSubject" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtSubject" Width="85px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="pnlSubject" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                                height: 200px; overflow: auto; margin: 0px; padding: 0px; color: Black;">
                                                                <asp:CheckBox ID="chkSubject" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                    runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkSubject_CheckedChanged" />
                                                                <asp:CheckBoxList ID="cblSubject" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                                    runat="server" AutoPostBack="True" Style="width: auto; height: auto; margin: 0px;
                                                                    padding: 0px; border: 0px; color: Black;" OnSelectedIndexChanged="cblSubject_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="popExtSubject" runat="server" TargetControlID="txtSubject"
                                                                PopupControlID="pnlSubject" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </td>
                                            <%--<td>
                                                <asp:Label ID="lblSem" runat="server" Visible="false" Text="Sem" Font-Bold="True"
                                                    Font-Names="Book Antiqua" ForeColor="White" Font-Size="Medium"></asp:Label>
                                            </td>--%>
                                            <%-- <td>--%><%--
                                                <asp:DropDownList ID="ddlSem" runat="server" Visible="false" Font-Bold="true"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" Width="100px" CssClass="arrow">
                                                </asp:DropDownList>--%>
                                            <%-- <div id="div1" runat="server" visible="false" style="position: relative;">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>--%>
                                            <%-- <asp:TextBox ID="txtSem" Width="85px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Style="width: 110px;
                                                                height: 200px; overflow: auto; margin: 0px; padding: 0px; color: Black;">
                                                                <asp:CheckBox ID="chkSem" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                    runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkSem_CheckedChanged" />--%>
                                            <%-- <asp:CheckBoxList ID="cblSem" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                                    runat="server" AutoPostBack="True" Style="width: auto; height: auto; margin: 0px;
                                                                    padding: 0px; border: 0px; color: Black;" OnSelectedIndexChanged="cblSem_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>--%>
                                            <%--  <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtSem"
                                                                PopupControlID="Panel1" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </td>--%>
                                            <td>
                                                <asp:Label ID="lbldop" runat="server" Text="Date Of Publication" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Medium">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtdop" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Width="75px"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtdop" Format="dd/MM/yyyy"
                                                    runat="server">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbldoi" runat="server" Text="Date Of Issue" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtdoi" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Width="75px"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtdoi" Format="dd/MM/yyyy"
                                                    runat="server">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td colspan="4">
                                    <asp:Button ID="Buttongo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="Buttongo_Click" Text="Go" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblcreditpaper" runat="server" Visible="false" Text="Credits" Font-Bold="true"
                                        Font-Names="Book Antiqua" ForeColor="White" Font-Size="Medium" Style="margin-left: 10px"></asp:Label>
                                    <asp:TextBox ID="txtcreditpap" Width="50px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CssClass="textbox  txtheight2" Visible="false"></asp:TextBox>
                                    <asp:Label ID="lblfrmrange" runat="server" Visible="false" Text="From" Font-Bold="true"
                                        Font-Names="Book Antiqua" ForeColor="White" Font-Size="Medium" Style="margin-left: 19px"></asp:Label>
                                    <asp:TextBox ID="txtfrmrange" Width="50px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CssClass="textbox  txtheight2" Visible="false" AutoPostBack="true"></asp:TextBox>
                                    <asp:Label ID="lbltorange" runat="server" Visible="false" Text="To" Font-Bold="true"
                                        Font-Names="Book Antiqua" ForeColor="White" Font-Size="Medium"></asp:Label>
                                    <asp:TextBox ID="txttorange" Width="50px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CssClass="textbox  txtheight2" Visible="false" AutoPostBack="true"
                                        OnTextChanged="txttorange_ontextchanged"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="14">
                        <div id="divPos" runat="server" style="display: none; text-align: left;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPos" runat="server" Text="Period of Study" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPosFMon" runat="server" Text="From Month" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium">
                                        </asp:Label>
                                        <asp:DropDownList ID="ddlPosFMonth" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="90px" CssClass="arrow">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPosFYear" runat="server" Text="From Year" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium">
                                        </asp:Label>
                                        <asp:DropDownList ID="ddlPosFYear" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="65px" CssClass="arrow">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPosTMon" runat="server" Text="To Month" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium">
                                        </asp:Label>
                                        <asp:DropDownList ID="ddlPosTMonth" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="90px" CssClass="arrow">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPosTYear" runat="server" Text="To Year" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium">
                                        </asp:Label>
                                        <asp:DropDownList ID="ddlPosTYear" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="65px" CssClass="arrow">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="14">
                        <div id="divDuplicate" runat="server" visible="false">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkDuplicateMarksheet" runat="server" Text="Duplicate Mark Statement"
                                            AutoPostBack="true" OnCheckedChanged="chkDuplicateMarksheet_CheckedChanged" Style="padding-right: 8px;" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDuplicate" runat="server" Text="Duplicate Number" Font-Bold="true"
                                            Font-Names="Book Antiqua" ForeColor="White" Font-Size="Medium" Style="padding-left: 8px;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDuplicateNumber" Enabled="false" runat="server" Text="" Style="font-size: medium;
                                            font-weight: bold; color: #000000;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDuplicateDate" runat="server" Text="Duplicate Date" Font-Bold="true"
                                            Font-Names="Book Antiqua" ForeColor="White" Font-Size="Medium" Style="padding-left: 8px;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDuplicateDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="75px"></asp:TextBox>
                                        <asp:CalendarExtender ID="calExtDupDate" TargetControlID="txtDuplicateDate" Format="dd/MM/yyyy"
                                            runat="server">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblStatementFormat" runat="server" Text="Format" Font-Bold="true"
                                            Font-Names="Book Antiqua" ForeColor="White" Font-Size="Medium" Style="padding-left: 8px;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlFormats" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="90px" CssClass="arrow">
                                            <asp:ListItem Selected="True" Text="Common Format" Value="0"></asp:ListItem>
                                            <asp:ListItem Selected="False" Text="Vocational Format" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:CheckBox ID="CbCommonCredits" runat="server" Text="Pass Credits for Batch Year Wise"
                                            Checked="true" />
                                        <asp:CheckBox ID="chkWoCr" runat="server" Text="Without *" Checked="false" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <asp:Label ID="lblerror" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
        position: relative;" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
        Width="800px" Font-Bold="true" ForeColor="Red"></asp:Label>
    <center>
        <div style="margin: 0px; margin-bottom: 20px; margin-top: 20px; position: relative;">
            <table>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Button ID="btnprint" runat="server" Font-Bold="True" Style="margin: 0px; margin-bottom: 10px;
                            margin-top: 10px; position: relative;" Font-Names="Book Antiqua" Font-Size="Medium"
                            OnClick="btnprint_Click" Text="Print" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid"
                            ShowHeaderSelection="false" BorderWidth="1px" Height="350" Style="margin: 0px;
                            margin-bottom: 10px; margin-top: 10px; position: relative;" Width="680" Visible="false"
                            OnUpdateCommand="FpSpread1_UpdateCommand" HorizontalScrollBarPolicy="Never">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark" ShowPDFButton="false">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <div id="errdiv" runat="server" visible="false" style="height: 100%; z-index: 2000;
        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
        left: 0px;">
        <center>
            <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                border-radius: 10px;">
                <center>
                    <table style="height: 100px; width: 100%">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lbl_popuperr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                    Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center>
                                    <asp:Button ID="btn_errorclose" runat="server" CssClass=" textbox btn1 comm" Font-Size="Medium"
                                        Font-Bold="True" Font-Names="Book Antiqua" Style="height: 28px; width: 65px;"
                                        OnClick="btn_errorclose_Click" Text="Ok" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </center>
    </div>
</asp:Content>
