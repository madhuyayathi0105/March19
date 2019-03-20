<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ExamAttendance.aspx.cs" Inherits="ExamAttendance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function chagevalue(id) {

            if (id.style.backgroundImage == "") {
                id.style.backgroundImage = "url('image/Cross.PNG')";
                id.value = "1";
            }
            else if (id.style.backgroundImage == 'url("image/Cross.PNG")') {
                id.style.backgroundImage = "url('image/Tick.PNG')";

            }
            else if (id.style.backgroundImage == 'url("image/Tick.PNG")') {
                id.style.backgroundImage = "url('image/Cross.PNG')";
            }
            return false;
        }
    </script>
    <style type="text/css">
        .stylefp
        {
            cursor: pointer;
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
        .btnadnf
        {
            background-image: url('image/Tick.jpg/');
        }
    </style>
    <style type="text/css">
        .submit
        {
            width: 30px;
            height: 30px;
            background-repeat: no-repeat;
            background-image: url(image/Tick.PNG);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="font-size: large; color: Green;">Exam Attendance</span>
        <div class="maindivstyle">
            <table class="maintablestyle">
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <%-- <asp:UpdatePanel ID="UP_college" runat="server">
                            <ContentTemplate>--%>
                        <asp:TextBox ID="txt_College" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                            placeholder="College"></asp:TextBox>
                        <asp:Panel ID="panel_college" runat="server" CssClass="multxtpanel">
                            <asp:CheckBox ID="cb_College" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                OnCheckedChanged="cb_College_CheckedChanged" />
                            <asp:CheckBoxList ID="cbl_College" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_College_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="popupce_sem" runat="server" TargetControlID="txt_College"
                            PopupControlID="panel_college" Position="Bottom">
                        </asp:PopupControlExtender>
                        <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </td>
                    <td>
                        <asp:Label ID="lblyear" runat="server" Text="Year" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlYear" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblmonth" runat="server" Text="Month" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMonth" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblfrmdate" runat="server" Text="Date" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlfrmdate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" AutoPostBack="True" Width="100px" OnSelectedIndexChanged="ddlfrmdate_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblsession" runat="server" Text="Session" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsession" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" AutoPostBack="True" Width="100px" OnSelectedIndexChanged="ddlsession_SelectedIndexChanged">
                            <asp:ListItem>F.N</asp:ListItem>
                            <asp:ListItem>A.N</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlType" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" AutoPostBack="True" Width="120px" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                            <asp:ListItem>Subject Wise</asp:ListItem>
                            <asp:ListItem>Hall Wise</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <div id="divSubject" runat="server">
                            <asp:Label ID="lblSubject" runat="server" Text="Subject" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            <asp:DropDownList ID="ddlsubject" runat="server" Font-Bold="True" OnSelectedIndexChanged="ddlsubject_Change"
                                Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True" Width="400px">
                            </asp:DropDownList>
                        </div>
                        <div id="divHall" runat="server" visible="false">
                            <asp:Label ID="lblHall" runat="server" Text="Hall" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            <asp:DropDownList ID="ddlHall" runat="server" Font-Bold="True" OnSelectedIndexChanged="ddlHall_Change"
                                Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True" Width="400px">
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td id="part" colspan="9" runat="server">
                        <div id="divpart" runat="server">
                            <asp:Label ID="lblsubpart" runat="server" Text="Subject Part" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            <asp:DropDownList ID="ddlpart" runat="server" Font-Bold="True" OnSelectedIndexChanged="ddlpart_Change"
                                Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True" Width="288px">
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td id="batch" runat="server">
                        <div id="divpart1" runat="server">
                            <asp:Label ID="lblbatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" OnSelectedIndexChanged="ddlbatch_Change"
                                Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True" Width="65px">
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <asp:Button ID="btnGo" runat="server" Text="Go" CssClass="textbox btn" Width="30px"
                            OnClick="btnGo_Click" />
                        <asp:Button ID="Savebtn" runat="server" Text="Save" CssClass="textbox btn" Width="60px"
                            BackColor="#59A62B" Visible="false" OnClick="Savebtn_Click" />
                    </td>
                </tr>
            </table>
            <center>
                <asp:Panel ID="pnlHeaderFilter" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg"
                    Height="22px" Width="850px" Style="margin-top: 20px; position: relative;">
                    <asp:Label ID="lblFilter" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                        Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                    <asp:Image ID="imgFilter" runat="server" CssClass="cpimage" AlternateText="" ImageAlign="Right" />
                </asp:Panel>
            </center>
            <center>
                <asp:Panel ID="pnlColumnOrder" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg"
                    CssClass="table2" Width="850px" Style="margin-top: 5px; margin-bottom: 25px;
                    position: relative;">
                    <table>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkColumnOrderAll" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="chkColumnOrderAll_CheckedChanged" />
                            </td>
                            <td>
                                <asp:LinkButton ID="lbtnRemoveAll" runat="server" Font-Size="X-Small" Height="16px"
                                    Style="font-family: 'Book Antiqua'; color: #ffffff; font-weight: 700; font-size: small;
                                    margin-left: -599px;" Visible="false" Width="111px" OnClick="lbtnRemoveAll_Click">Remove All</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                                <asp:TextBox ID="txtOrder" Visible="false" Width="837px" TextMode="MultiLine" CssClass="noresize"
                                    AutoPostBack="true" runat="server" Enabled="false">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBoxList ID="cblColumnOrder" runat="server" Height="43px" AutoPostBack="true"
                                    Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                    RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblColumnOrder_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Selected="True">S.No</asp:ListItem>
                                    <asp:ListItem Value="1" Selected="True">Roll No</asp:ListItem>
                                    <asp:ListItem Value="2" Selected="True" Enabled="false">Register No</asp:ListItem>
                                    <asp:ListItem Value="3" Selected="True">Student Name</asp:ListItem>
                                    <asp:ListItem Value="4" Selected="True">Student Type</asp:ListItem>
                                    <asp:ListItem Value="5" Selected="True">Degree</asp:ListItem>
                                    <asp:ListItem Value="6" Selected="True">Branch</asp:ListItem>
                                    <asp:ListItem Value="7" Selected="True" Enabled="false">Attendance</asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </center>
            <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pnlColumnOrder"
                CollapseControlID="pnlHeaderFilter" ExpandControlID="pnlHeaderFilter" Collapsed="true"
                TextLabelID="lblFilter" CollapsedSize="0" ImageControlID="imgFilter" CollapsedImage="~/images/right.jpeg"
                ExpandedImage="~/images/down.jpeg">
            </asp:CollapsiblePanelExtender>
            <asp:Label ID="lblerror" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="#FF3300" Visible="False"></asp:Label>
            <center>
                <br>
                <div>
                    <FarPoint:FpSpread ID="Subjectspread" runat="server" Height="400" Width="660" CssClass="spreadborder"
                        Visible="false" ShowHeaderSelection="false" OnUpdateCommand="Subjectspread_UpdateCommand">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#FFFFFF">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
            </center>
            <br />
            <br />
            <div id="div_report" runat="server" visible="false">
                <center>
                    <asp:Button ID="btn_printmaster" runat="server" Text="Print" CssClass="textbox btn2"
                        AutoPostBack="true" OnClick="btn_printmaster_Click" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </center>
            </div>
        </div>
    </center>
</asp:Content>
