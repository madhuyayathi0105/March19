<%@ Page Title="Condonation Reports" Language="C#" AutoEventWireup="true" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    CodeFile="CondonationReports.aspx.cs" Inherits="CondonationReports" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Printcontrol" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        function display1() {
            document.getElementById('<%=lbl_norec1.ClientID %>').innerHTML = "";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="position: relative; margin: 0px; margin-bottom: 25px; width: 100%; height: auto;">
        <center>
            <div style="margin-top: 10px; margin-bottom: 10px;">
                <span class="fontstyleheader" style="color: Green">Condonation Reports</span>
            </div>
        </center>
        <center>
            <div id="divSearch" runat="server" visible="true" style="color: black; font-family: Book Antiqua;
                height: auto; width: 100%; margin-bottom: 10px; padding-bottom: 10px;">
                <table class="maintablestyle" id="tblsearch" runat="server">
                    <tr>
                        <td>
                            <div id="divNormal" runat="server" visible="true">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCollege" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Font-Bold="true" Width="250px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <%--<asp:DropDownList ID="ddlBatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Width="80px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged">
                                            </asp:DropDownList>--%>
                                            <%--added by Deepali on 30.3.18--%>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtbatch" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                        ReadOnly="true" Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                    <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel" Width="125px">
                                                        <asp:CheckBox ID="chkbatch" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chkbatch_CheckedChanged"
                                                            Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                        <asp:CheckBoxList ID="chklsbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                            Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtbatch"
                                                        PopupControlID="pbatch" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlDegree" runat="server" Visible="false" Font-Bold="true"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Width="120px" AutoPostBack="true"
                                                CssClass="arrow" OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:UpdatePanel ID="UpnlDegree" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtDegree" Width=" 100px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                                                    <asp:Panel ID="pnlDegree" runat="server" CssClass="multxtpanel" Height="200px">
                                                        <asp:CheckBox ID="chkDegree" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                            runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkDegree_CheckedChanged" />
                                                        <asp:CheckBoxList ID="cblDegree" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblDegree_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popupExtDegree" runat="server" TargetControlID="txtDegree"
                                                        PopupControlID="pnlDegree" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" AssociatedControlID=""></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlBranch" Visible="false" runat="server" Font-Bold="true"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Width="120Px" AutoPostBack="true"
                                                CssClass="arrow" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:UpdatePanel ID="upnlBranch" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtBranch" Width=" 100px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                                                    <asp:Panel ID="pnlBranch" runat="server" CssClass="multxtpanel" Height="200px">
                                                        <asp:CheckBox ID="chkBranch" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                            runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                        <asp:CheckBoxList ID="cblBranch" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popupExtBranch" runat="server" TargetControlID="txtBranch"
                                                        PopupControlID="pnlBranch" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" AssociatedControlID=""></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlSem" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Width="60Px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlSem_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblReport" runat="server" Text="Report" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" AssociatedControlID=""></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upnlReport" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtReport" Width=" 100px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                                                    <asp:Panel ID="pnlReport" runat="server" CssClass="multxtpanel" Height="113px">
                                                        <asp:CheckBox ID="chkReport" Checked="true" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkReport_CheckedChanged" />
                                                        <asp:CheckBoxList ID="cblReport" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblReport_SelectedIndexChanged">
                                                            <asp:ListItem Selected="True" Text="Eligible" Value="1"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="Condonation" Value="2"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="Not Eligible" Value="3"></asp:ListItem>
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popupExtReport" runat="server" TargetControlID="txtReport"
                                                        PopupControlID="pnlReport" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnGo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnClick="btnGo_Click" Text="Go" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </center>
        <br />
        <br />
        <asp:Label ID="lblErrmsg" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
            Font-Bold="true" ForeColor="Red" Text="" Visible="false"></asp:Label>
        <center>
            <div id="divCondonation" runat="server" visible="false">
                <FarPoint:FpSpread ID="FpCondonation" runat="server" AutoPostBack="false" Width="1200px"
                    Visible="true" BorderStyle="Solid" BorderWidth="0px" CssClass="spreadborder"
                    ShowHeaderSelection="false" Style="width: 100%; height: auto;">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <center>
                    <div id="rptprint1" runat="server" visible="false" style="margin: 20px;">
                        <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox textbox1" Height="20px"
                            Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                            onkeypress="display1()" Font-Size="Medium"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtexcelname1"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                            InvalidChars="/\">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnExcel1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            OnClick="btnExcel1_Click" Font-Size="Medium" Text="Export To Excel" Width="127px"
                            Height="35px" CssClass="textbox textbox1" />
                        <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="35px"
                            CssClass="textbox textbox1" />
                        <Insproplus:Printcontrol runat="server" ID="Printcontrol1" Visible="false" />
                        <asp:Button ID="btnCondonationReport" runat="server" Text="Save Condonation" OnClick="btnCondonationReport_Click"
                            Visible="false" Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true"
                            Height="35px" CssClass="textbox textbox1" />
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="divPopupAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="pnlPopupAlert" runat="server" class="table" style="background-color: White;
                        height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        margin-top: 200px; border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblPopupAlert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btnPopupClose" CssClass=" textbox textbox1 btn1" Style="height: 28px;
                                                width: 65px;" OnClick="btnPopupClose_Click" Text="Ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
    </div>
</asp:Content>
