<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="TC Remark.aspx.cs" Inherits="StudentMod_TC_Remark" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .printclass
        {
            display: none;
        }
        .grid-view
        {
            padding: 0;
            margin: 0;
            border: 1px solid #333;
            font-family: "Verdana, Arial, Helvetica, sans-serif, Trebuchet MS";
            font-size: 0.9em;
        }
        
        .grid-view tr.header
        {
            color: white;
            background-color: #0CA6CA;
            height: 30px;
            vertical-align: middle;
            text-align: center;
            font-weight: bold;
            font-size: 20px;
        }
        
        .grid-view tr.normal
        {
            color: black;
            background-color: #FDC64E;
            height: 25px;
            vertical-align: middle;
            text-align: center;
        }
        
        .grid-view tr.alternate
        {
            color: black;
            background-color: #D59200;
            height: 25px;
            vertical-align: middle;
            text-align: center;
        }
        
        .grid-view tr.normal:hover, .grid-view tr.alternate:hover
        {
            background-color: white;
            color: black;
            font-weight: bold;
        }
        
        .grid_view_lnk_button
        {
            color: Black;
            text-decoration: none;
            font-size: large;
        }
        .lbl
        {
            font-family: Book Antiqua;
            font-size: 30px;
            font-weight: bold;
            color: Green;
            text-align: center;
            font-style: italic;
        }
        .hdtxt
        {
            font-family: Book Antiqua;
            font-size: large;
            font-weight: bold;
        }
        .FixedHeader
        {
            position: absolute;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">

        function SelLedgers() {
            var chkSelAll = document.getElementById("<%=chkGridSelectAll.ClientID %>");
            var tbl = document.getElementById("<%=grid_Details.ClientID %>");
            var gridViewControls = tbl.getElementsByTagName("input");

            for (var i = 0; i < (tbl.rows.length - 1); i++) {
                var chkSelectid = document.getElementById('MainContent_grid_Details_lbl_cb_' + i.toString());
                if (chkSelAll.checked == false) {
                    chkSelectid.checked = false;
                } else {
                    chkSelectid.checked = true;
                }
            }

        }

      
    </script>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">TC Remark </span>
            </div>
        </center>
    </div>
    <div>
        <center>
            <div id="maindiv" runat="server" class="maindivstyle" style="width: auto; height: auto">
                <div>
                    <table>
                        <tr>
                            <td>
                                <center>
                                    <div>
                                        <table class="maintablestyle">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_collegename" Text="College" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                        OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged" AutoPostBack="false">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_str1" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlstream" runat="server" AutoPostBack="false" OnSelectedIndexChanged="ddlstream_OnSelectedIndexChanged"
                                                        CssClass="textbox  ddlheight" Style="width: 108px;">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    Batch
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
                                                                height: auto;">
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
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="Up_dept" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_dept" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                                height: auto;">
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
                                                <td>
                                                    Remark
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_plus" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                                        Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus_Click" />
                                                    <asp:DropDownList ID="ddl_tccertificateissuedate" CssClass="ddlheight1 textbox1"
                                                        runat="server">
                                                    </asp:DropDownList>
                                                    <asp:Button ID="btn_minus" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                                        Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btnminus_Click" />
                                                </td>
                                                <td>
                                                    Conduct
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_plus1" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                                        Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus1_Click" />
                                                    <asp:DropDownList ID="ddl_generalconduct" CssClass="ddlheight1 textbox1" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:Button ID="btn_minus1" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                                        Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btnminus1_Click" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btngo" runat="server" CssClass="textbox btn2" Text="Go" OnClick="btngo_Click" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnsave" runat="server" CssClass="textbox btn2" Visible="false" Text="Save"
                                                        OnClick="btn_Save_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <center>
                                            <div id="plusdiv" runat="server" visible="false" class="popupstyle popupheight1">
                                                <center>
                                                    <div id="panel_addgroup" runat="server" visible="false" class="table" style="background-color: White;
                                                        height: 140px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                                        margin-top: 200px; border-radius: 10px;">
                                                        <table style="line-height: 30px">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:Label ID="lbl_addgroup" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txt_addgroup" runat="server" Width="200px" CssClass="textbox textbox1"
                                                                        onkeypress="display1()"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" style="line-height: 35px">
                                                                    <asp:Button ID="btn_addgroup1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                                        CssClass="textbox btn2" OnClick="btn_addgroup_Click" />
                                                                    <asp:Button ID="btn_exitgroup1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                                        CssClass="textbox btn2" OnClick="btn_exitaddgroup_Click" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:Label ID="lblerror" runat="server" Visible="false" ForeColor="red" Font-Size="Smaller"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </center>
                                            </div>
                                        </center>
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
                                        <%--<div id="divGrid" runat="server" visible="false"> --%>
                                        <%-- <asp:GridView ID="grid_Details" runat="server" AutoGenerateColumns="true" GridLines="Both" 
                              CssClass="grid-view" BackColor="WhiteSmoke" Style="width: auto;" >
                                  OnRowDataBound="gdattrpt_OnRowDataBound"        
                        </asp:GridView>
                                        --%>
                                    </div>
                                </center>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </center>
        <center>
            <div id="divGrid" runat="server" style="width: auto; height: auto; overflow: auto;
                ackground-color: White; border-radius: 0px;">
                <span style="padding-right: 100px; margin-left: -460px;">
                    <asp:CheckBox ID="chkGridSelectAll" runat="server" Text="SelectAll" Visible="false"
                        onchange="return SelLedgers();" />
                </span>
                <asp:GridView ID="grid_Details" runat="server" AutoGenerateColumns="false" GridLines="Both"
                    Style="width: auto" OnDataBound="grid_Details_DataBound" OnRowDataBound="grid_Details_OnRowDataBound">
                    <%--  OnRowDataBound="gridView1_OnRowDataBound" OnDataBound="Marksgrid_pg_DataBound"
                                                        OnRowCommand="gridView1_OnRowCommand"--%>
                    <Columns>
                        <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                            HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <center>
                                    <asp:Label ID="lbl_rs" runat="server" Width="60px" Text='<%#Eval("Sno") %>'></asp:Label>
                                    <asp:Label ID="lbl_sno" runat="server" Visible="false" Text='<%#Eval("appno") %>'>
                                    </asp:Label>
                                </center>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                            HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <center>
                                    <asp:CheckBox ID="lbl_cb" runat="server" Width="30px"></asp:CheckBox>
                                </center>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                            HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <center>
                                    <asp:Label ID="lbl_name" runat="server" Style="width: auto;" Text='<%#Eval("Name") %>'></asp:Label>
                                </center>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Roll No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                            HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <center>
                                    <asp:Label ID="lbl_rn" runat="server" Style="width: auto;" Text='<%#Eval("Roll No") %>'></asp:Label>
                                </center>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reg No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                            HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <center>
                                    <asp:Label ID="lbl_res" runat="server" Style="width: auto;" Text='<%#Eval("Reg No") %>'></asp:Label>
                                </center>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Admission No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                            HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <center>
                                    <asp:Label ID="lbl_an" runat="server" Style="width: auto;" Text='<%#Eval("Admission No") %>'></asp:Label>
                                </center>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remark" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <center>
                                    <asp:DropDownList ID="ddl_Remark" runat="server" CssClass="textbox ddlheight3" Width="110px"
                                        Visible="true">
                                    </asp:DropDownList>
                                    <asp:Label ID="lbl_remark" runat="server" Visible="false" Text='<%#Eval("remark") %>'>
                                    </asp:Label>
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Conduct" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <center>
                                    <asp:DropDownList ID="ddl_Conduct" runat="server" CssClass="textbox ddlheight3" Width="110px"
                                        Visible="true">
                                    </asp:DropDownList>
                                    <asp:Label ID="lbl_conduct" runat="server" Visible="false" Text='<%#Eval("conduct") %>'>
                                    </asp:Label>
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </center>
    </div>
</asp:Content>
