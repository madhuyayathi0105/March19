<%@ Page Title="" Language="C#" MasterPageFile="~/HostelMod/hostelsite.master" AutoEventWireup="true"
    CodeFile="Hosteladmissionsettings.aspx.cs" Inherits="Hosteladmissionsettings"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <center>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: green">Hostel Admission Process Header Settings</span>
                </div>
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_clgname" Width="100px" runat="server" Text="College"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcollege" CssClass="ddlheight5 textbox1" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_columnordertype" Text="Report Type" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="btn_addtype" runat="server" Text="+" CssClass="textbox textbox1 btn1"
                                OnClick="btn_addtype_OnClick" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_coltypeadd" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_coltypeadd_SelectedIndexChanged"
                                CssClass="textbox textbox1 ddlheight4">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btn_deltype" runat="server" Text="-" CssClass="textbox textbox1 btn1"
                                OnClick="btn_deltype_OnClick" />
                        </td>
                        <%--<td>
                            <asp:RadioButton ID="rdoformat1" runat="server" Text="Format 1" GroupName="f" />
                            <asp:RadioButton ID="rdoformat2" runat="server" Text="Format 2" GroupName="f" />
                        </td>--%>
                    </tr>
                </table>
            </center>
            <br />
            <fieldset style="border-radius: 10px; width: 900px; height: auto;">
                <legend style="font-size: larger; font-weight: bold">ColumnOrder Header Settings</legend>
                <div>
                    <asp:TextBox ID="txtcolumn" runat="server" AutoPostBack="true" TextMode="MultiLine"
                        Width="900px" Height="100px" ReadOnly="true"></asp:TextBox>
                    <br />
                    <asp:LinkButton ID="lnk_selectall" runat="server" Font-Size="Small" AutoPostBack="true"
                        Height="20px" Visible="true" Style="margin-left: 0px; margin-top: 5px;" OnClick="LinkButtonselectall_Click">Select All</asp:LinkButton>
                    <asp:LinkButton ID="lnk_columnordr" runat="server" Font-Size="Small" AutoPostBack="true"
                        Height="20px" Visible="true" Style="margin-left: 450px; margin-top: 5px;" OnClick="LinkButtonsremove_Click">Remove All</asp:LinkButton>
                    <br />
                    <asp:CheckBoxList ID="lb_selectcolumn" runat="server" AutoPostBack="true" RepeatColumns="6"
                        Font-Size="Small" OnSelectedIndexChanged="lb_selectcolumn_Selectedindexchange">
                    </asp:CheckBoxList>
                </div>
                <br />
                <asp:Label ID="lblalerterr" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                <br />
                <center>
                    <asp:Button ID="btnok" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                        Text="OK" CssClass="textbox textbox1 btn2" OnClick="btnok_click" />
                    <%-- <asp:Button ID="btnclose" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                        Text="Close" CssClass="textbox textbox1 btn2" OnClick="btnclose_click" />--%>
                </center>
            </fieldset>
        </div>
        <div id="imgdiv33" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="panel_description11" runat="server" visible="false" class="table" style="background-color: White;
                    height: 120px; width: 467px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <table>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lbl_description111" runat="server" Text="Description" Font-Bold="true"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:TextBox ID="txt_description11" runat="server" Width="400px" Style="font-family: 'Book Antiqua';
                                    margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btn_adddesc1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="textbox btn1" OnClick="btndescpopadd_Click" />
                                <asp:Button ID="btn_exitdesc1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="textbox btn1" OnClick="btndescpopexit_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
        </div>
        <div id="imgdiv2" runat="server" visible="false" style="height: 300em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 180px;
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
                                        <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                            width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
</asp:Content>
