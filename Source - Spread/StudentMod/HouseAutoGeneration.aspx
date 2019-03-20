<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="HouseAutoGeneration.aspx.cs" Inherits="HouseAutoGeneration" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Student Housing Generation</span></div>
                <br />
            </center>
            <table id="Table1" class="maintablestyle" runat="server">
                <tr>
                    <td>
                        <asp:Label ID="lbl_collegename" Text="College" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_collegename" runat="server" OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged"
                            Height="29px" Width="202px" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblStr" Text="Stream" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddltype" runat="server" Width="140px" Height="30px" Enabled="false"
                            OnSelectedIndexChanged="type_Change" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <span>Batch</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" Width="70px" Height="30px" AutoPostBack="true"
                            OnSelectedIndexChanged="batch_SelectedIndexChange">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <span>Education Level</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddledulevel" runat="server" Width="120px" Height="30px" AutoPostBack="true"
                            OnSelectedIndexChanged="edulevel_SelectedIndexChange">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDeg" Text="Degree" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel_Department" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_degree" runat="server" ReadOnly="true" Width="120px" CssClass="textbox textbox1 txtheight">---Select---</asp:TextBox>
                                <asp:Panel ID="paneldegree" runat="server" Height="300px" Width="180px" CssClass="multxtpanel">
                                    <asp:CheckBox ID="cbdegree" runat="server" Text="Select All" AutoPostBack="True"
                                        OnCheckedChanged="cbdegree_Changed" />
                                    <asp:CheckBoxList ID="cbldegree" runat="server" OnSelectedIndexChanged="cbldegree_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_degree"
                                    PopupControlID="paneldegree" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="cbldegree" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblBran" Text="Branch" runat="server"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_department" runat="server" ReadOnly="true" Width="177px" CssClass="textbox textbox1 txtheight">---Select---</asp:TextBox>
                                <asp:Panel ID="paneldepartment" runat="server" Height="300px" Width="180px" CssClass="multxtpanel">
                                    <asp:CheckBox ID="cbdepartment1" runat="server" Text="Select All" AutoPostBack="True"
                                        OnCheckedChanged="cbdepartment_Changed" />
                                    <asp:CheckBoxList ID="cbldepartment" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbldepartment_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_department"
                                    PopupControlID="paneldepartment" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Button ID="btngo" runat="server" Text="Go" CssClass="textbox textbox1 btn1"
                            OnClick="btngo_click" />
                        <asp:Button ID="btngenerate" runat="server" Text="Generate" CssClass="textbox textbox1 btn2"
                            OnClick="btngenerate_click" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="lbl_error" runat="server" Text="" Visible="false" Font-Size="Large"
                ForeColor="Red"></asp:Label>
            <br />
            <br />
            <div id="sp_div" runat="server">
                <FarPoint:FpSpread ID="FpSpread" runat="server" Visible="false" BorderColor="Black"
                    BorderStyle="Solid" BorderWidth="1px" Width="900px" Height="600px" Style="margin-left: 2px;"
                    class="spreadborder" ShowHeaderSelection="false">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
            </div>
            <br />
            <br />
            <br />
            <div id="rprint" runat="server" visible="false">
                <asp:Label ID="lblsmserror" Text="Please Enter Your Report Name" Font-Size="Large"
                    Visible="false" ForeColor="Red" runat="server"></asp:Label>
                <asp:Label ID="lblexcel" runat="server" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txtexcel" onkeypress="display(this)" CssClass="textbox textbox1"
                    runat="server"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcel"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btnexcel" runat="server" CssClass="textbox textbox1 btn3" Height="30px"
                    Text="Export Excel" OnClick="btnexcel_Click" />
                <asp:Button ID="btnprintmaster" runat="server" Text="Print" Width="59px" Height="30px"
                    OnClick="btnprintmaster_Click" CssClass="textbox textbox1 btn3" />
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="59px" Height="30px" OnClick="btnSave_Click"
                    CssClass="textbox textbox1 btn3" />
                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
            </div>
        </div>
        <center>
            <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <br />
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
    </center>
</asp:Content>
