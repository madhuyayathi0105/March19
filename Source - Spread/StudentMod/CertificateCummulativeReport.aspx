<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CertificateCummulativeReport.aspx.cs" Inherits="CertificateCummulativeReport"
    EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link rel="Stylesheet" href="~/Styles/css/Commoncss.css" />
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblsmserror.ClientID %>').innerHTML = "";
            }
        </script>
        <asp:ScriptManager ID="myscript" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green; font-size: x-large;">Admission Status
                    - Certificate Received Status Report</span>
            </div>
            <div id="Div1" class="maindivstyle" runat="server">
                <br />
                <fieldset id="fldRad" runat="server" style="border: 1px solid #999999; background-color: #F0F0F0;
                    box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999; -webkit-box-shadow: 0px 0px 10px #999999;
                    border: 3px solid #D9D9D9; border-radius: 15px; width: 300px; height: 20px;">
                    <asp:RadioButton ID="rdb_cummulate" runat="server" Text="Cummulative" GroupName="rep"
                        Checked="true" AutoPostBack="true" OnCheckedChanged="rdb_cummulate_Change" />
                    <asp:RadioButton ID="rdb_individual" runat="server" Text="Individual" GroupName="rep"
                        AutoPostBack="true" OnCheckedChanged="rdb_individual_Change" />
                </fieldset>
                <br />
                <table id="Table1" class="maintablestyle" runat="server">
                    <tr>
                        <td>
                            <asp:Label ID="lblcoll" runat="server" Text="College Name"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddlcollege" runat="server" CssClass="textbox1 ddlheight6" Width="246px"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_Change">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblbatch" Text="Batch" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_batch" runat="server" CssClass="textbox1  ddlheight" Width="90px"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_batch_change">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_degree" Text="Degree" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" Visible="false" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  textbox1 txtheight3"
                                        Width="80px" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderColor="Gray" BorderStyle="Solid"
                                        BorderWidth="2px" CssClass="multxtpanel" Width="150px" Height="180px" Style="position: absolute;">
                                        <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_degree_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_degree"
                                        PopupControlID="Panel1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel ID="UpdatePanel3" Visible="false" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddl_degree" runat="server" CssClass="textbox1  ddlheight" Width="144px"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_degree_change">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_branch" Text="Branch" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="Upp5" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_branch" runat="server" CssClass="textbox textbox1 txtheight3"
                                        Width="80px" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="p4" runat="server" BackColor="White" BorderColor="Gray" BorderStyle="Solid"
                                        BorderWidth="2px" CssClass="multxtpanel" Width="200px" Height="200px" Style="position: absolute;">
                                        <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_branch_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_branch"
                                        PopupControlID="p4" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_sem" Text="Semester" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updsem" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddl_sem" runat="server" CssClass="textbox1  ddlheight" Width="80px"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            From Date
                        </td>
                        <td>
                            <asp:TextBox ID="txtfrmDt" runat="server" CssClass="textbox textbox1" Width="80px">
                            </asp:TextBox>
                            <asp:CalendarExtender ID="calfrmDt" runat="server" Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active"
                                TargetControlID="txtfrmDt">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            To Date
                        </td>
                        <td>
                            <asp:TextBox ID="txttoDt" runat="server" CssClass="textbox textbox1" Width="80px">
                            </asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active"
                                TargetControlID="txttoDt">
                            </asp:CalendarExtender>
                        </td>
                        <td id="myTab1" runat="server" visible="false">
                            <asp:Label ID="lblCertName" runat="server" Text="Certificate Name"></asp:Label>
                        </td>
                        <td id="myTab2" runat="server" visible="false">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtCertName" runat="server" CssClass="textbox textbox1 txtheight3"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlCertName" runat="server" BackColor="White" BorderColor="Gray" BorderStyle="Solid"
                                        BorderWidth="2px" CssClass="multxtpanel" Width="200px" Height="200px" Style="position: absolute;">
                                        <asp:CheckBox ID="cbCertName" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cbCertName_checkedchange" />
                                        <asp:CheckBoxList ID="cblCertName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblCertName_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtCertName"
                                        PopupControlID="pnlCertName" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Button ID="btnGo" runat="server" Text="Go" CssClass="textbox1 btn1" OnClick="btnGo_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="lblmainerr" runat="server" Text="" Visible="false" Font-Bold="true"
                    Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Red"></asp:Label>
                <br />
                <FarPoint:FpSpread ID="Fpspreadpop" runat="server" Visible="false" BorderColor="Black"
                    BorderStyle="Solid" BorderWidth="1px" Height="350px" Style="margin-left: 2px;
                    width: auto;" class="spreadborder" ShowHeaderSelection="false">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <br />
                <div id="rprint" runat="server" visible="false">
                    <asp:Label ID="lblsmserror" Text="Please Enter Your Report Name" Font-Size="Large"
                        Font-Names="Book Antiqua" Visible="false" ForeColor="Red" runat="server" Font-Bold="true"></asp:Label>
                    <asp:Label ID="lblexcel" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                    <asp:TextBox ID="txtexcel" onkeypress="display()" CssClass="textbox textbox1" runat="server"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcel"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnexcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" CssClass="textbox textbox1 btn1" Width="100px" Text="Export Excel" OnClick="btnexcel_Click" />
                    <asp:Button ID="btnprintmaster" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" CssClass="textbox textbox1 btn1" Width="75px" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </div>
                <br />
            </div>
        </center>
    </body>
    </html>
</asp:Content>
