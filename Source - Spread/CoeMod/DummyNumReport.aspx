<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="DummyNumReport.aspx.cs" Inherits="CoeMod_DummyNumReport" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scrptmngr" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">
        function OnViewSelectCheck() {
            var monId = document.getElementById("<%=ddlMonth.ClientID %>").value.trim();
            var yearId = document.getElementById("<%=ddlYear.ClientID %>").value.trim();
            if (monId != '' && yearId != '' && monId != '0' && yearId != '0') {
                var genType = document.getElementById("<%=ddlGenType.ClientID %>").value.trim();

                if (genType != "Common") {
                    var subVal = document.getElementById("<%=ddlsubject.ClientID %>").value.trim();
                    if (subVal != '' && subVal != '0') {
                        return true;
                    } else {
                        alert('Please Select Subject');
                        return false;
                    }
                }
                return true;
            }
            alert('Please Select Month & Year');
            return false;
        }
        function excelReport() {
            var val = document.getElementById("<%=txt_excelname.ClientID %>").value.trim();
            if (val == '' || val == '0') {
                alert('Please Enter Report Name');
                return false;
            }
            return true;

        }
    </script>
    <center>
        <span class="fontstyleheader" style="font-size: large; color: Green;">Dummy Number Report</span>
        <div class="maindivstyle">
            <table class="maintablestyle">
                <tr>
                    <td>
                        <%-- <asp:UpdatePanel ID="UP_college" runat="server">
                            <ContentTemplate>--%>
                        <asp:TextBox ID="txt_College" runat="server" CssClass="textbox txtheight1" ReadOnly="true"
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
                        <asp:Label ID="lblMonthandYear" runat="server" Text="Year & Month"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="textbox ddlheight" Width="60px"
                            OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" AutoPostBack="True" TabIndex="2">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlMonth" runat="server" CssClass="textbox ddlheight" Width="60px"
                            OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblGenType" runat="server" Text="Type"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlGenType" runat="server" CssClass="textbox ddlheight" Width="100px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlGenType_IndexChange">
                            <asp:ListItem>Common</asp:ListItem>
                            <asp:ListItem>Subject Wise</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblGenMethod" runat="server" Text="Mode"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlGenMethod" runat="server" CssClass="textbox ddlheight" Width="90px"
                            AutoPostBack="true">
                            <asp:ListItem>Serial</asp:ListItem>
                            <asp:ListItem>Random</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnViewReport" runat="server" CssClass="textbox btn" Width="100px"
                            Text="View Report" OnClick="btnViewReport_Click" OnClientClick="return OnViewSelectCheck();" />
                    </td>
                </tr>
                <tr id="trSubjectDet" runat="server" visible="false">
                    <td colspan="10">
                        <asp:Label ID="lblExDate" runat="server" Text="Exam Date"></asp:Label>
                        <asp:DropDownList ID="ddlExDate" runat="server" CssClass="textbox ddlheight" Width="100px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlExdate_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Label ID="lblsession" runat="server" Text="Session"></asp:Label>
                        <asp:DropDownList ID="ddlsession" runat="server" CssClass="textbox ddlheight" AutoPostBack="True"
                            Width="70px" OnSelectedIndexChanged="ddlsession_SelectedIndexChanged">
                            <asp:ListItem>Both</asp:ListItem>
                            <asp:ListItem>F.N</asp:ListItem>
                            <asp:ListItem>A.N</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lblsubject" runat="server" Text="Subject"></asp:Label>
                        <asp:DropDownList ID="ddlsubject" runat="server" CssClass="textbox ddlheight" Width="400px"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkIsDept" runat="server" Text="Degreewise" AutoPostBack="true"
                            OnCheckedChanged="chkIsDept_CheckedChange" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_stream" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddl_strm" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_strm_OnIndexChange">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lbl_degree2" runat="server" Text="Degree"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_degree2" runat="server" ReadOnly="true" Height="20px" CssClass="textbox txtheight">--Select--</asp:TextBox>
                                <asp:Panel ID="pdegree" runat="server" Width="150px" Height="170px" CssClass="multxtpanel">
                                    <asp:CheckBox ID="cb_degree2" runat="server" OnCheckedChanged="cb_degree2_ChekedChange"
                                        Text="Select All" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="cbl_degree2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree2_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_degree2"
                                    PopupControlID="pdegree" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lbl_branch2" runat="server" Text="Branch"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_branch2" runat="server" ReadOnly="true" Height="20px" CssClass="textbox textbox1 txtheight">--Select--</asp:TextBox>
                                <asp:Panel ID="pbranch" runat="server" Width="250px" Height="200px" CssClass="multxtpanel">
                                    <asp:CheckBox ID="cb_branch1" runat="server" OnCheckedChanged="cb_branch1_ChekedChange"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="cbl_branch1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_branch1_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_branch2"
                                    PopupControlID="pbranch" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td colspan="3">
                    </td>
                </tr>
            </table>
            <br />
            <FarPoint:FpSpread ID="spreadReport" runat="server" Visible="false" ShowHeaderSelection="false"
                BorderWidth="0px" Width="900px" Style="overflow: auto; height: 400px; border: 0px solid #999999;
                border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#F7BE81" SelectionPolicy="Single">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            <br>
            <div id="rptprint" runat="server" visible="false">
                <asp:Label ID="lbl_rptname" runat="server" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txt_excelname" runat="server" Width="180px" onkeypress="display()"
                    CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                <asp:Button ID="btn_excel" runat="server" OnClick="btn_excel_Click" Text="Export To Excel"
                    Width="127px" CssClass="textbox btn2 textbox1" OnClientClick="return excelReport();" />
                <asp:Button ID="btn_printmaster" runat="server" Text="Print" OnClick="btn_printmaster_Click"
                    CssClass="textbox btn2 textbox1" Width="60px" />
                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
            </div>
        </div>
    </center>
</asp:Content>
