<%@ Page Title="Twelth Mark Range Analysis" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="MarkrangeAnalysis.aspx.cs" Inherits="MarkrangeAnalysis" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Plus Two Mark Range Analysis</title>
    <link href="../Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
    <style type="text/css">
        .maindivstylesize
        {
            height: 12000px;
            width: 1000px;
        }
        .lbl
        {
            text-align: center;
        }
        .container
        {
            width: 100%;
        }
        .col1
        {
            float: left;
            width: 50%;
        }
        .col2
        {
            float: right;
            width: 50%;
        }
        .newtextbox
        {
            border: 1px solid #c4c4c4;
            padding: 4px 4px 4px 4px;
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            box-shadow: 0px 0px 8px #d9d9d9;
            -moz-box-shadow: 0px 0px 8px #d9d9d9;
            -webkit-box-shadow: 0px 0px 8px #d9d9d9;
        }
        
        .textboxshadow:hover
        {
            outline: none;
            border: 1px solid #BAFAB8;
            box-shadow: 0px 0px 8px #BAFAB8;
            -moz-box-shadow: 0px 0px 8px #BAFAB8;
            -webkit-box-shadow: 0px 0px 8px #BAFAB8;
        }
        .textboxchng
        {
            border: 1px solid #c4c4c4;
            padding: 4px 4px 4px 4px;
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            box-shadow: 0px 0px 8px #d9d9d9;
            -moz-box-shadow: 0px 0px 8px #d9d9d9;
            -webkit-box-shadow: 0px 0px 8px #d9d9d9;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <form id="form1">
    <%--<script type="text/javascript">
        function ClearPrint() {
            var id = document.getElementById('<%=lblvalidation1.ClientID%>');
            id.innerHTML = "";
            id.visible = false;
        }
        function ClearPrint1() {
            var id = document.getElementById('<%=lbl_norec.ClientID%>');
            id.innerHTML = "";
            id.visible = false;
        }
    </script>--%>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitEvents);
    </script>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: #008000; font-size: xx-medium">Plus Two
                            Mark Range Analysis</span>
                    </div>
                </center>
                <br />
                <div class="maindivstyle maindivstylesize">
                    <br />
                    <center>
                        <table class="maintablestyle" width="500px">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lbl_clgname" Width="100px" runat="server" Text="College"></asp:Label>
                                            <asp:DropDownList ID="ddlcollege" CssClass="ddlheight4 textbox textbox1" runat="server"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_batch" Width="102px" runat="server" Text="Batch"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_batch" runat="server" Width="70px" CssClass="textbox txtheight1 textbox1"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel2" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Style="width: 120px; height: 150px;">
                                                <asp:CheckBox ID="cb_batch" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_batch_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_batch"
                                                PopupControlID="Panel2" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lbl_status" Width="100px" runat="server" Text="Status"></asp:Label>
                                            <asp:DropDownList ID="ddl_status" runat="server" Visible="true" CssClass="ddlheight4 textbox textbox1"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddl_status_SelectedIndexChanged">
                                                <asp:ListItem Value="0" Text="Applied" />
                                                <asp:ListItem Value="1" Text="Admitted" />
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </center>
                    </br>
                    <center>
                        <table style="border: solid 1px gray;">
                            <tr>
                                <td>
                                    From Range
                                    <asp:TextBox ID="txtfrmsecamnt" runat="server" Visible="true" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="filterfrmsecamnt" runat="server" FilterMode="ValidChars"
                                        FilterType="Numbers" TargetControlID="txtfrmsecamnt">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    To Range<asp:TextBox ID="txttosecamnt" runat="server" OnTextChanged="txttosecamnt_change"
                                        AutoPostBack="true" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="filtertosecamnt" runat="server" FilterMode="ValidChars"
                                        FilterType="Numbers" TargetControlID="txttosecamnt">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Button ID="btnadditset" runat="server" CssClass="textbox textbox1 btn2" Text="Add"
                                        OnClick="btnadditset_click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnsaveallitset" runat="server" Text="Generate" CssClass="textbox textbox1 btn2"
                                        Width="100px" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnsaveallitset_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="textbox textbox1 btn2"
                                        Width="70px" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnReset_Click" />
                                </td>
                                <td>
                                    <asp:ImageButton ID="imgbtn_columsetting" Visible="false" runat="server" Width="30px"
                                        Height="30px" Text="All" ImageUrl="~/Hostel Gete Images/images (1)ppp.jpg" OnClick="imgbtn_all_Click" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkNotEntered" Text="Not Entered" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <div id="divgrditset" runat="server" visible="false" style="border: 2px solid indigo;
                        border-radius: 10px; height: auto; margin-left: 27px; width: 435px;">
                        <div style="height: 200px; overflow: auto;">
                            <center>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="grditset" runat="server" AutoGenerateColumns="false" Visible="false"
                                            GridLines="Both" OnRowDataBound="grditset_rowbound" OnRowCommand="grditset_rowcommand">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_sno" runat="server" Text='<%#Eval("sno") %>' />
                                                            <asp:Label ID="lbl_itCalPk" runat="server" Visible="false" Text='<%#Eval("itCalculationPK") %>' />
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="From Range" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_frmamnt" runat="server" Text='<%#Eval("itfrmamnt") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="To Range" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_toamnt" runat="server" Text='<%#Eval("ittoamnt") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Button ID="btn_del" runat="server" Text="DELETE" OnClick="btn_del_Click" />
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <%-- <asp:TemplateField HeaderText="Update" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Button ID="btn_update" runat="server" Text="Update" OnClick="btn_update_Click" />
                                                </center>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>--%>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </center>
                        </div>
                        <%--<center>
                                <asp:Button ID="btnsaveallitset" runat="server" Text="Set IT Range Settings" CssClass="textbox textbox1 btn2"
                                    Width="220px" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnsaveallitset_Click" />
                            </center>--%>
                        </br> </br>
                    </div>
                    <center>
                        <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" BorderWidth="5px"
                            BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" Style="margin-left: -5px"
                            OnCellClick="FpSpread1_CellClick" OnPreRender="FpSpread1_SelectedIndexChanged">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </center>
                </div>
                <center>
                    <div id="imgAlert" runat="server" visible="false" style="height: 1000px; z-index: 10000;
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
                                                <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btn_alertclose" CssClass=" textbox textbox1 btn1" Style="height: 28px;
                                                        width: 65px;" OnClick="btn_alertclose_Click" Text="ok" runat="server" />
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
        </center>
    </div>
</asp:Content>
