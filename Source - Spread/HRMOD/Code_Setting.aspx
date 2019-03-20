<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="Code_Setting.aspx.cs" Inherits="Code_Setting" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Src="~/Usercontrols/Commonfilter.ascx" TagName="Search" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <style type="text/css">
        .stnew
        {
            text-transform: uppercase;
        }
        
        .btn_class1
        {
            font-weight: bold;
            margin-left: 0px;
            font-family: book antiqua;
            font-size: medium;
            background-color: #6699ee;
            border-radius: 6px;
            height: 30px;
            font-weight: bold;
            border: 1px ridge #DB7C08;
            text-shadow: 0px -2px 1px #E4C31F,0px -1px 1px #E4C31d;
            -moz-border-radius: 20px 0px 20px 0px;
            -webkit-border-radius: 20px 0px 20px 0px;
            border-radius: 20px 0px 20px 0px;
            color: Blue;
            -moz-box-shadow: 6px 0px 0px 0px #3399ff;
            -webkit-box-shadow: 6px 0px 0px 0px #9932CC;
            box-shadow: 6px 0px 0px 0px #BA55D3;
            background: #3399ff;
            background: -webkit-gradient(linear,left top,left bottom,from(#3399ff),to(#3399ff));
            background: -webkit-linear-gradient(top,#3399ff 0%,#3399ff 100%);
            background: -moz-linear-gradient(top,#3399ff 0%,#3399ff 100%);
            background: -o-linear-gradient(top,#3399ff 0%,#3399ff 100%);
            background: -ms-linear-gradient(top,#3399ff 0%,#3399ff 100%);
            background: linear-gradient(top,#3399ff 0%,#3399ff 100%);
            -moz-transition: all 0.6s ease-out;
            -webkit-transition: all 0.6s ease-out;
            -o-transition: all 0.6s ease-out;
            transition: all 0.6s ease-out;
        }
        .btn_class2
        {
            font-weight: bold;
            height: 30px;
            font-family: book antiqua;
            font-size: medium;
            background-color: #6699ee;
            border-radius: 6px;
            border: 1px ridge #DB7C08;
            text-shadow: 0px -2px 1px #E4C31F,0px -1px 1px #E4C31d;
            -moz-border-radius: 20px 0px 20px 0px;
            -webkit-border-radius: 20px 0px 20px 0px;
            border-radius: 20px 0px 20px 0px;
            color: Blue;
            -moz-box-shadow: 6px 0px 0px 0px #DBB408;
            -webkit-box-shadow: 6px 0px 0px 0px #DBB408;
            box-shadow: 6px 0px 0px 0px #BA55D3;
            background: #EED51C;
            background: -webkit-gradient(linear,left top,left bottom,from(#EED51C),to(#E8C84B));
            background: -webkit-linear-gradient(top,#EED51C 0%,#E8C84B 100%);
            background: -moz-linear-gradient(top,#EED51C 0%,#EED51C 100%);
            background: -o-linear-gradient(top,#EED51C 0%,#E8C84B 100%);
            background: -ms-linear-gradient(top,#3399ff 0%,#3399ff 100%);
            background: linear-gradient(top,#EED51C 0%,#E8C84B 100%);
            -moz-transition: all 0.6s ease-out;
            -webkit-transition: all 0.6s ease-out;
            -o-transition: all 0.6s ease-out;
            transition: all 0.6s ease-out;
        }
    </style>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Code Setting</span>
                </div>
            </center>
        </div>
        <center>
            <fieldset id="maindiv" runat="server" style="width: 1000px; height: 700px; border-color: silver;
                border-radius: 10px; background-color: #F5F5F5">
                <center>
                    <fieldset>
                        <legend style="font-weight: bold; font-family: Book Antiqua; font-size: medium;">Staff
                            Code Settings </legend>
                        <fieldset style="width: 590px; height: 25px; margin-left: -352px; border-radius: 10px;
                            background-color: #0CA6CA;">
                            <asp:RadioButton ID="rdb_scode" runat="server" Text="Staff Code" AutoPostBack="true"
                                OnCheckedChanged="rdb_scode_Change" GroupName="cs" Checked="true" Font-Bold="true"
                                Font-Names="Book Antiqua" Font-Size="Medium" />
                            <asp:RadioButton ID="rdb_appno" Text="Application Number" AutoPostBack="true" OnCheckedChanged="rdb_appno_Change"
                                runat="server" GroupName="cs" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" />
                            <asp:RadioButton ID="rdb_desigcode" Text="Designation Code" AutoPostBack="true" OnCheckedChanged="rdb_desigcode_Change"
                                runat="server" GroupName="cs" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                            <asp:RadioButton ID="rdb_catcode" Text="Category Code" AutoPostBack="true" OnCheckedChanged="rdb_catcode_Change"
                                runat="server" GroupName="cs" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" />
                        </fieldset>
                        <fieldset style="width: 880px; height: 70px; margin-left: -64px; border-radius: 10px;
                            background-color: #0CA6CA;">
                            <table style="margin-left: 0px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblcoll" runat="server" Text="College" Style="margin-left: 0px;" Font-Bold="true"
                                            Font-Names="Book Antiqua"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlcoll" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlcoll_Change"
                                            Style="margin-left: 0px;" Width="193px" CssClass="textbox1 ddlheight3">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_clg" runat="server" AutoPostBack="true" Font-Bold="true" Font-Names="Book Antiqua"
                                            Style="margin-left: 0px;" Font-Size="Medium" Text="College Acronym" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_dept" runat="server" AutoPostBack="true" Font-Bold="true" Font-Names="Book Antiqua"
                                            Style="margin-left: 0px;" Font-Size="Medium" Text="Department Acronym" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_general" runat="server" AutoPostBack="true" OnCheckedChanged="cb_general_OnCheckedChanged"
                                            Font-Bold="true" Font-Names="Book Antiqua" Style="margin-left: 0px;" Font-Size="Medium"
                                            Text="General Acronym" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_general" runat="server" Enabled="false" MaxLength="3" CssClass="textbox txtheight2 stnew"
                                            Style="font-weight: bold; width: 110px; margin-left: 0px; font-family: book antiqua;
                                            font-size: medium;"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table style="margin-left: -436px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_startno" runat="server" Text="Starting No" Style="font-weight: bold;
                                            font-family: book antiqua; margin-left: 0px; font-size: medium;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_startingno" runat="server" MaxLength="3" CssClass="textbox txtheight2"
                                            Style="font-weight: bold; width: 60px; margin-left: 10px; font-family: book antiqua;
                                            font-size: medium;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender59" runat="server" TargetControlID="txt_startingno"
                                            FilterType="Custom,Numbers" ValidChars="" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Serial Size" Style="font-weight: bold;
                                            font-family: book antiqua; margin-left: 0px; font-size: medium;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_serial" runat="server" MaxLength="1" CssClass="textbox txtheight2"
                                            Style="font-weight: bold; width: 110px; margin-left: 0px; font-family: book antiqua;
                                            font-size: medium;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender58" runat="server" TargetControlID="txt_serial"
                                            FilterType="Custom" ValidChars="0,1,2,3,4,5,6" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_go" Text="Go" runat="server" OnClick="btn_go_OnClick" Style="font-weight: bold;
                                            margin-left: 6px; font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </fieldset>
                </center>
                <br />
                <center>
                    <asp:Label ID="alert" runat="server" Visible="false" Text="" Style="font-weight: bold;
                        color: Red; font-family: book antiqua; margin-left: 0px; font-size: medium;"></asp:Label>
                </center>
                <center>
                    <fieldset style="width: 950px; margin-left: -22px; background-color: #EEE8AA;">
                        <div style="height: 200px; margin-left: -572px; width: 250px;">
                            <asp:ListBox ID="lbl_select" runat="server" SelectionMode="Multiple" Height="200px"
                                Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-top: 2px;
                                margin-left: -134px;" Width="250px"></asp:ListBox>
                        </div>
                        <table style="margin-left: -330px; margin-top: -180px;">
                            <tr>
                                <td>
                                    <asp:Button ID="btn_right" Text=">" runat="server" OnClick="btn_right_OnClick" Style="font-weight: bold;
                                        margin-left: 6px; background-color: #0CA6CA; width: 50px; font-family: book antiqua;
                                        font-size: medium; border-radius: 4px;" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btn_rightfwd" Text=">>" runat="server" OnClick="btn_rightfwd_OnClick"
                                        Style="font-weight: bold; margin-left: 6px; background-color: #0CA6CA; width: 50px;
                                        font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btn_left" Text="<" runat="server" OnClick="btn_left_OnClick" Style="font-weight: bold;
                                        margin-left: 6px; background-color: #0CA6CA; width: 50px; font-family: book antiqua;
                                        font-size: medium; border-radius: 4px;" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btn_leftfwd" Text="<<" runat="server" OnClick="btn_leftfwd_OnClick"
                                        Style="font-weight: bold; margin-left: 6px; background-color: #0CA6CA; width: 50px;
                                        font-family: book antiqua; font-size: medium; border-radius: 4px;" />
                                </td>
                            </tr>
                        </table>
                        <div style="height: 200px; margin-left: 238px; margin-top: -168px; width: 250px;">
                            <asp:ListBox ID="lbl_disp" runat="server" SelectionMode="Multiple" Height="200px"
                                Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-top: 24px;
                                margin-left: -176px;" Width="250px"></asp:ListBox>
                        </div>
                        <fieldset id="preiview" runat="server" visible="false" style="height: 135px; margin-left: 643px;
                            margin-top: -176px; width: 275px;">
                            <center>
                                <fieldset style="border-radius: 10px; height: 10px; background-color: #0CA6CA; width: 120px;">
                                    <table style="margin-top: -6px;">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text="Sample Code" Style="font-weight: bold;
                                                    font-family: book antiqua; margin-left: 0px; font-size: large;"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_acr" runat="server" Text="Acronym" Style="font-weight: bold; font-family: book antiqua;
                                                margin-left: 0px; font-size: medium;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_acr" runat="server" OnSelectedIndexChanged="ddl_acr_OnSelectedIndexChanged"
                                                Width="120px" AutoPostBack="true" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium" CssClass="textbox ddlstyle ddlheight3">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_format" runat="server" Text="Code Format" Style="font-weight: bold;
                                                font-family: book antiqua; margin-left: 0px; font-size: medium;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_preview" runat="server" Text="" ReadOnly="true" Enabled="false"
                                                Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-top: 0px;"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Button ID="btn_close" Text="Close" runat="server" OnClick="btn_close_OnClick"
                                                Style="font-weight: bold; margin-left: 205px; background-color: #0CA6CA; font-family: book antiqua;
                                                font-size: medium; border-radius: 4px;" />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </fieldset>
                        <br />
                        <table style="margin-left: 630px; margin-top: -10px;">
                            <tr>
                                <td>
                                    <asp:Button ID="btn_preview" Text="Preview" runat="server" OnClick="btn_preview_OnClick"
                                        Style="font-weight: bold; margin-left: 6px; background-color: #0CA6CA; font-family: book antiqua;
                                        font-size: medium; border-radius: 4px;" />
                                    <asp:Button ID="btn_save" Text="Save" runat="server" OnClick="btn_save_OnClick" Style="font-weight: bold;
                                        margin-left: 6px; background-color: #0CA6CA; font-family: book antiqua; font-size: medium;
                                        border-radius: 4px;" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <br />
                    <center>
                        <asp:Label ID="lbl_err" runat="server" Visible="false" Text="" Style="font-weight: bold;
                            font-family: book antiqua; margin-left: 0px; font-size: medium; color: Red;">
                        </asp:Label>
                    </center>
                </center>
                <center>
                    <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 20px;
                        left: 0px;">
                        <center>
                            <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 90px;
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
                                                        Text="Ok" runat="server" OnClick="btnerrclose_Click" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
            </fieldset>
        </center>
    </body>
    </html>
</asp:Content>
