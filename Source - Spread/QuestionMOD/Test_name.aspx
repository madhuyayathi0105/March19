<%@ Page Title="Internal And External Test Creation" Language="C#" MasterPageFile="~/QuestionMOD/QuestionBankSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Test_name.aspx.cs" Inherits="Test_name" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .Header
        {
            font-weight: bold;
            text-align: center;
            font-size: 22px;
            color: Green;
            margin-top: 20px;
            margin-bottom: 20px;
            line-height: 3em;
        }
        .fontCommon
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
            color: #000000;
        }
        .defaultHeight
        {
            width: auto;
            height: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div style="width: 100%; height: auto;">
            <table>
                <thead>
                    <tr>
                        <td colspan="3">
                            <center>
                                <span class="Header">Internal And External Test Creation</span>
                            </center>
                        </td>
                    </tr>
                </thead>
            </table>
            <center>
                <div class="maindivstyle" style="width: 100%; height: auto; width: -moz-max-content;">
                    <center>
                        <asp:RadioButton ID="rb_internel" Width="88px" Visible="true" Font-Bold="true" runat="server"
                            GroupName="same1" Text="Internal" OnCheckedChanged="rb_internel_CheckedChanged"
                            AutoPostBack="true" Checked="true"></asp:RadioButton>
                        <asp:RadioButton ID="rb_external" runat="server" Visible="true" Font-Bold="true"
                            Width="100px" GroupName="same1" Text="External" OnCheckedChanged="rb_external_CheckedChanged"
                            AutoPostBack="true"></asp:RadioButton>
                        <asp:RadioButton ID="rb_General" runat="server" Visible="true" Font-Bold="true" Width="100px"
                            GroupName="same1" Text="General" OnCheckedChanged="rb_General_CheckedChanged"
                            AutoPostBack="true"></asp:RadioButton>
                    </center>
                    <div>
                        <center>
                            <table class="maintablestyle" width="633px" style="margin: 10px; height: auto;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_clg" runat="server" Text="College" CssClass="fontCommon"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="fontCommon" OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged"
                                            AutoPostBack="true" Width="100px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbatch" runat="server" Text="Batch" CssClass="fontCommon"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbatch" runat="server" CssClass="fontCommon" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                            AutoPostBack="true" Width="58px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldegree" runat="server" Text="Degree" CssClass="fontCommon"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddldegree" runat="server" CssClass="fontCommon" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged"
                                            AutoPostBack="true" Width="65px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbranch" runat="server" Text="Branch" CssClass="fontCommon" AutoPostBack="true"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbranch" runat="server" CssClass="fontCommon" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                            AutoPostBack="true" Width="130px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" ForeColor="Black"
                                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsem" runat="server" CssClass="fontCommon" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged"
                                            AutoPostBack="true" Width="50px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="10">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblsec" runat="server" Text="Sec" CssClass="fontCommon"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlsec" runat="server" CssClass="fontCommon" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged"
                                                        AutoPostBack="true" Width="50px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblsubject" runat="server" Text="Subject" CssClass="fontCommon"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlsubject" runat="server" CssClass="fontCommon" AutoPostBack="true"
                                                        Width="130px" OnSelectedIndexChanged="ddlsubject_Selectchanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_go" runat="server" Visible="true" Width="44px" Height="26px"
                                                        CssClass="textbox textbox1" Text="Go" Font-Bold="true" OnClick="btn_go_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </center>
                        <br />
                        <br />
                        <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </div>
                    <br />
                    <div id="divMainContent" runat="server">
                        <FarPoint:FpSpread ID="FpSpread1" Visible="false" Tab-ActiveTabBorderColor="AliceBlue"
                            runat="server" BorderStyle="Solid" BorderWidth="0px" ShowHeaderSelection="false"
                            CssClass="spreadborder" Style="height: auto; width: 667px;" OnButtonCommand="FpSpread1_ButtonCommand">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <br />
                        <asp:Button ID="btn_add" Text="ADD" Visible="false" CssClass="textbox textbox1 btn1"
                            Height="32px" Width="50px" runat="server" OnClick="btn_add_Click" />
                        <asp:Button ID="btn_save" Text="Save" Visible="false" CssClass="textbox textbox1 btn1"
                            Height="32px" Width="50px" runat="server" OnClick="btn_save_Click" /><br />
                        <br />
                    </div>
                </div>
            </center>
            <div id="imgdiv3" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div3" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_alert" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_errorclose1" CssClass="textbox textbox1" Style="height: 28px;
                                                width: 65px;" OnClick="btn_errorclose1_Click" Text="Ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
            <center>
                <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                            <asp:Label ID="lbl_alert1" runat="server" Text="" Style="color: Red;" Font-Bold="true"
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
        </div>
    </center>
</asp:Content>
