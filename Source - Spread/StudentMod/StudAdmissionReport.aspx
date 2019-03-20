<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudAdmissionReport.aspx.cs" Inherits="StudentMod_StudAdmissionReport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Admission Report</title>
    <link rel="Shortcut Icon" href="~/college/Left_Logo.jpeg" />
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scriptMrgr" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <span class="fontstyleheader" style="color: Green;">Admission Report </span>
        </div>
    </center>
    <center>
        <div class="maindivstyle" style="width: 970px;">
            <center>
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lblCollege" runat="server" Text="College"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_college" Width="200px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddl_college_OnIndexChange">
                            </asp:DropDownList>
                            <asp:UpdatePanel ID="upCollege" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtCollege" runat="server" CssClass="textbox txtheight2" Width="100px"
                                        ReadOnly="true" placeholder="Semester" onfocus="return myFunction1(this)"></asp:TextBox>
                                    <asp:Panel ID="pnlCollege" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_College" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_College_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_College" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_College_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pceCollege" runat="server" TargetControlID="txtCollege"
                                        PopupControlID="pnlCollege" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <%--<td>
                            <asp:Label ID="lbl_stream" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_strm" Width="80px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddl_strm_OnIndexChange">
                            </asp:DropDownList>
                            <asp:UpdatePanel ID="upStrm" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtStrm" runat="server" CssClass="textbox txtheight2" Width="100px"
                                        ReadOnly="true" placeholder="Semester" onfocus="return myFunction1(this)"></asp:TextBox>
                                    <asp:Panel ID="pnlStrm" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_Strm" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_Strm_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_Strm" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_Strm_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pceStrm" runat="server" TargetControlID="txtStrm" PopupControlID="pnlStrm"
                                        Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>--%>
                        <td>
                            <asp:Label ID="lblEduLev" runat="server" Text="Education Level"></asp:Label>
                        </td>
                        <td>
                            <%--<asp:UpdatePanel ID="upEduLev" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlEduLev" Width="80px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlEduLev_OnIndexChange">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>--%>
                              <asp:UpdatePanel ID="upEduLev" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtEduLev" runat="server" CssClass="textbox txtheight2" Width="100px"
                                        ReadOnly="true" placeholder="EduLevel" onfocus="return myFunction1(this)"></asp:TextBox>
                                    <asp:Panel ID="pnlEduLev" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_EduLev" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_EduLev_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_EduLev" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_EduLev_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pceEduLev" runat="server" TargetControlID="txtEduLev"
                                        PopupControlID="pnlEduLev" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_batch" runat="server" Text="Batch"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_batch" Width="60px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddl_batch_OnIndexChange">
                            </asp:DropDownList>
                            <asp:UpdatePanel ID="upBatch" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_batch" runat="server" CssClass="textbox txtheight2" Width="60px"
                                        ReadOnly="true" placeholder="Semester" onfocus="return myFunction1(this)"></asp:TextBox>
                                    <asp:Panel ID="pnlBatch" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_Batch" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_Batch_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_Batch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_Batch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pceBatch" runat="server" TargetControlID="txt_batch"
                                        PopupControlID="pnlBatch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lblSeatType" runat="server" Text="Seat Type"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updSeatType" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtSeatType" runat="server" CssClass="textbox txtheight2" Width="80px"
                                        ReadOnly="true" placeholder="Semester" onfocus="return myFunction1(this)"></asp:TextBox>
                                    <asp:Panel ID="pnlSeatType" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_SeatType" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_SeatType_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_SeatType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_SeatType_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pceSeatType" runat="server" TargetControlID="txtSeatType"
                                        PopupControlID="pnlSeatType" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Sem" runat="server" Text="Semester"></asp:Label>
                        </td>
                        <td>
                            <%--  <asp:DropDownList ID="ddl_sem" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_sem_OnIndexChange">
                            </asp:DropDownList>--%>
                            <asp:UpdatePanel ID="UPpanel_sem" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox txtheight2" Width="80px"
                                        ReadOnly="true" placeholder="Semester" onfocus="return myFunction1(this)"></asp:TextBox>
                                    <asp:Panel ID="panel_sem" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_sem" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_sem_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popupce_sem" runat="server" TargetControlID="txt_sem"
                                        PopupControlID="panel_sem" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Button ID="btn_go" Text="Go" OnClick="btn_go_Click" CssClass="textbox btn1 textbox1"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="cbDateWise" runat="server" Text="" Checked="false" />
                            <asp:Label ID="lblFrom" runat="server" Text="From"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox  txtheight" Width="70px"
                                OnTextChanged="checkDate" AutoPostBack="true"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_fromdate" runat="server"
                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            To
                            <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox  txtheight" OnTextChanged="checkDate"
                                Width="70px" AutoPostBack="true"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_todate" runat="server"
                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="lblErr" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                <div>
                    <FarPoint:FpSpread ID="spreadStudList" runat="server" Visible="false" ShowHeaderSelection="false"
                        BorderWidth="0px" Width="900px" Style="overflow: auto; height: 300px; border: 0px solid #999999;
                        border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" SelectionPolicy="Single">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lbl_validation" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                        Visible="false"></asp:Label><br />
                    <asp:Label ID="lbl_rptname" runat="server" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txt_excelname" runat="server" Width="180px" onkeypress="display()"
                        CssClass="textbox textbox1 txtheight2" MaxLength="70"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="fteExcel" runat="server" TargetControlID="txt_excelname"
                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" _-">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btn_excel" runat="server" OnClick="btn_excel_Click" Text="Export To Excel"
                        Width="127px" CssClass="textbox btn2 textbox1" />
                    <asp:Button ID="btn_printmaster" runat="server" Text="Print" OnClick="btn_printmaster_Click"
                        CssClass="textbox btn2 textbox1" Width="60px" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </div>
            </center>
        </div>
    </center>
</asp:Content>
