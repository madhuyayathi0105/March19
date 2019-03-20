<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="FinanceYearWiseCollectionReport.aspx.cs" Inherits="FinanceYearWiseCollectionReport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Receipt / Challan</title>
    <link rel="Shortcut Icon" href="../college/Left_Logo.jpeg" />
    <link href="../Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <span class="fontstyleheader" style="color: Green;">FinanceYearWiseCollection Report
            </span>
        </div>
    </center>
    <%--  Design Started By abarna 17/1/2018 --%>
    <center>
        <div class="maindivstyle" style="width: 980px;">
            <center>
                <%--Row0 --%>
                <div style="padding-left: 10px; padding-top: 5px; clear: both;">
                    <div class="mainbatch">
                        <%--<table  style="border-radius: 10px; background-color: White; height: 25px; float: left;
                            border-style: solid; border-width: 1px;">--%>
                        <table class="maintablestyle" style="float: left; height: 25px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_college" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox ddlstyle ddlheight5"
                                        OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <%--  <asp:Label ID="lbltype" Font-Bold="true" Style="position: absolute; left: 285px;
                                        top: 11px; height: 60px;" Font-Size="Medium" ForeColor="white" Font-Names="Book Antiqua"
                                        runat="server" Text="Type"></asp:Label>--%>
                                    <asp:Label ID="lbltype" runat="server" Text="Type"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txttype" Style="left: 340px; top: 11px; right: 250px;" Width="100px"
                                        runat="server" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="PType" runat="server" CssClass="multxtpanel" Width="114px" Style="height: auto;">
                                        <asp:CheckBox ID="chktype" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chktype_batchchanged" />
                                        <asp:CheckBoxList ID="chklstype" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chklstype_batchselected">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txttype"
                                        PopupControlID="PType" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lblbatch" runat="server" Text="Batch"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UP_batch" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_batch" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                                height: auto;">
                                                <asp:CheckBox ID="cb_batch" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_batch_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="pce_batch" runat="server" TargetControlID="txt_batch"
                                                PopupControlID="panel_batch" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UP_degree" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_degree" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_degree" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                height: 200px;">
                                                <asp:CheckBox ID="cb_degree" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_degree_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="pce_degree" runat="server" TargetControlID="txt_degree"
                                                PopupControlID="panel_degree" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Up_dept" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_dept" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                height: 300px;">
                                                <asp:CheckBox ID="cb_dept" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_dept_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="pce_dept" runat="server" TargetControlID="txt_dept"
                                                PopupControlID="panel_dept" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <%--<%--    <td>
                                    <asp:Label runat="server" ID="lblacctype" Text="A/c Type" Font-Names="Book Antiqua"
                                        Style="font-family: Book Antiqua; width: 103px"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlacctype" runat="server" Style="font-family: Book Antiqua;
                                        height: auto;" OnSelectedIndexChanged="ddlacctype_change" AutoPostBack="true">
                                        <%--  <asp:ListItem>---Select---</asp:ListItem>--%>
                            <%--   <asp:ListItem>Group Header</asp:ListItem>
                                        <asp:ListItem>Header</asp:ListItem>
                                        <asp:ListItem>Ledger</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>--%>
                            <%--   <asp:Label runat="server" ID="Label1" Text="A/c Header" Font-Names="Book Antiqua"
                                        Style="font-family: Book Antiqua; width: 118px;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtheader" runat="server" Height="30px" CssClass="Dropdown_Txt_Box"
                                                ReadOnly="true" Width="127px" Style="height: 20px; font-family: 'Book Antiqua';"
                                                Enabled="false">---Select---</asp:TextBox>
                                            <asp:Panel ID="paccheader" runat="server" Style="height: auto; width: 250px;" CssClass="multxtpanel multxtpanleheight">
                                                <asp:CheckBox ID="cbheader" runat="server" Font-Names="Book Antiqua" OnCheckedChanged="cbheader_OnCheckedChanged"
                                                    Text="Select All" AutoPostBack="True" />
                                                <asp:CheckBoxList ID="cblheader" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblheader_SelectedIndexChanged"
                                                    Font-Names="Book Antiqua">
                                                </asp:CheckBoxList>
                                                <asp:TreeView ID="treeledger" runat="server" SelectedNodeStyle-ForeColor="Red" HoverNodeStyle-BackColor="Black"
                                                    Width="450px" Font-Names="Book Antiqua" ForeColor="Black" ShowCheckBoxes="All">
                                                </asp:TreeView>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtheader"--%><%--
                                                PopupControlID="paccheader" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                            --%>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_header1" runat="server" Width="60px" Text="Header"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Updp_header1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_header1" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel_header1" runat="server" CssClass="multxtpanel" Height="200px"
                                                Width="200px">
                                                <asp:CheckBox ID="cb_header1" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_header1_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_header1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_header1_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender20" runat="server" TargetControlID="txt_header1"
                                                PopupControlID="Panel_header1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_ledger1" runat="server" Text="Ledger"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Updp_ledger1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_ledger1" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel_ledger1" runat="server" CssClass="multxtpanel" Height="200px"
                                                Width="200px">
                                                <asp:CheckBox ID="cb_ledger1" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_ledger1_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_ledger1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_ledger1_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender21" runat="server" TargetControlID="txt_ledger1"
                                                PopupControlID="Panel_ledger1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblfyear" Text="FinanceYear" Width="85px"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtfyear" Style="height: 20px; width: 125px;" CssClass="Dropdown_Txt_Box"
                                                runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                            <asp:Panel ID="Pfyear" runat="server" CssClass="multxtpanel" Width="178px">
                                                <asp:CheckBox ID="chkfyear" runat="server" Text="Select All" OnCheckedChanged="chkfyear_changed"
                                                    AutoPostBack="True" />
                                                <asp:CheckBoxList ID="chklsfyear" runat="server" OnSelectedIndexChanged="chklsfyear_selected"
                                                    AutoPostBack="True">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txtfyear"
                                                PopupControlID="Pfyear" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblsem" runat="server" Text="Semester"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Updp_sem" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_sem" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_sem" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                height: 202px;">
                                                <asp:CheckBox ID="cb_sem" runat="server" Width="124px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_sem_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtendergt" runat="server" TargetControlID="txt_sem"
                                                PopupControlID="panel_sem" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_seat" runat="server" Text="Seat Type"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_seat" runat="server" Style="left: 857px; top: 49px;" runat="server"
                                        ReadOnly="true" Width="105px">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel5" runat="server" CssClass="multxtpanel" Style="width: 191px;
                                        height: auto;">
                                        <asp:CheckBox ID="cb_seat" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_seat_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_seat" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_seat_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_seat"
                                        PopupControlID="Panel5" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </td>
                                <td>
                                    <asp:Button ID="btnGo" OnClick="btnGo_Click" runat="server" Text="Go" Style="font-family: Book Antiqua;
                                        font-weight: 700;" /><%----%>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </center>
            <br />
        </div>
    </center>
    <div>
        <center>
            <FarPoint:FpSpread ID="spreadDet" runat="server" Visible="false" BorderStyle="Solid"
                BorderWidth="0px" Style="overflow: auto; border: 0px solid #999999; border-radius: 10px;
                background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="Green">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
        </center>
    </div>
    <center>
        <div id="rptprint2" runat="server" visible="false">
            <asp:Label ID="lbl_norec2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
            <asp:Label ID="lblrptname2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Text="Report Name"></asp:Label>
            <asp:TextBox ID="txtexcelname2" runat="server" CssClass="textbox textbox1" Height="20px"
                Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                onkeypress="display2()" Font-Size="Medium"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txtexcelname2"
                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                InvalidChars="/\">
            </asp:FilteredTextBoxExtender>
            <asp:Button ID="btnExcel2" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                OnClick="btnExcel2_Click" Font-Size="Medium" Text="Export To Excel" Width="127px"
                Height="35px" CssClass="textbox textbox1" />
            <asp:Button ID="btnprintmaster2" runat="server" Text="Print" OnClick="btnprintmaster2_Click"
                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="35px"
                CssClass="textbox textbox1" />
            <Insproplus:printmaster runat="server" ID="Printcontrol2" Visible="false" />
        </div>
    </center>
</asp:Content>
