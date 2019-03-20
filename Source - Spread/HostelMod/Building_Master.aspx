<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="Building_Master.aspx.cs" Inherits="Building_Master" %>

<%@ Register Src="~/Usercontrols/Input_Events.ascx" TagName="collegedeatils" TagPrefix="UC" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title></title>
        <script type="text/javascript" language="javascript">

            function display12() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
            function checktxt() {
                empty = "";
                id = document.getElementById("<%=txtexcelname.ClientID %>").value;
                if (id.trim() == "") {
                    document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "Please Enter Your Report Name";
                    empty = "E";
                }

                if (empty != "") {
                    return false;
                }
                else {

                    return true;
                }
            }
        </script>
    </head>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <body>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager3" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <br />
                    <span class="fontstyleheader" style="color: indigo;">Building Master</span></div>
                <br />
            </center>
        </div>
        <center>
            <fieldset id="maindiv" runat="server" style="width: 950px; margin-left: 10px; height: 500px;
                border-color: silver; border-radius: 10px;">
                <fieldset style="height: 36px; width: 878px; margin-left: 0px; border: 1px solid #0ca6ca;
                    border-radius: 10px;">
                    <fieldset style="border: 2px; border: 1px solid #ccc; background-color: #0ca6ca;
                        box-shadow: 0 0 8px #999999; border-radius: 10px; height: 10px; margin-left: -683px;
                        padding: 1em; width: 156px;">
                        <table style="margin-left: 0px; margin-top: -10px;">
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rdb_detail" runat="server" Checked="true" OnCheckedChanged="rdb_detail_OnCheckedChanged"
                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium;" AutoPostBack="true"
                                        Text="Entry" GroupName="a" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_report" runat="server" OnCheckedChanged="rdb_report_OnCheckedChanged"
                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium;" AutoPostBack="true"
                                        Text="Report" GroupName="a" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset style="border: 2px; border: 1px solid #ccc; background-color: #0ca6ca;
                        box-shadow: 0 0 8px #999999; margin-left: 210px; margin-top: -44px; border-radius: 10px;
                        height: 24px; width: 650px;">
                        <table style="margin-left: 0px; margin-top: -3px; width: 650px;">
                            <tr>
                                <td>
                                    <asp:Label ID="Building" runat="server" Text="Building" Style="font-weight: bold;
                                        font-family: book antiqua; margin-left: 0px; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlbuild" runat="server" Visible="false" OnSelectedIndexChanged="ddlbuild_SelectedIndexChanged"
                                        AutoPostBack="true" Style="font-weight: bold; font-family: book antiqua; width: 100px;
                                        font-size: medium;">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Visible="false">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_build" runat="server" CssClass="textbox txtheight2" Style="font-weight: bold;
                                                font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="p2" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                                border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                                box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; width: 175px;
                                                height: 200px;">
                                                <asp:CheckBox ID="cb_build" runat="server" Text="Select All" OnCheckedChanged="cb_build_CheckedChange"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_build" runat="server" OnSelectedIndexChanged="cbl_build_SelectedIndexChange"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_build"
                                                PopupControlID="p2" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_flr" runat="server" Visible="false" Text="Floor" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" Visible="false">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_flr" runat="server" CssClass="textbox txtheight2" Style="font-weight: bold;
                                                font-family: book antiqua; width: 100px; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel1" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                                position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                                width: 175px; height: 200px;">
                                                <asp:CheckBox ID="cb_flr" runat="server" Text="Select All" OnCheckedChanged="cb_flr_CheckedChange"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_flr" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_flr_SelectedIndexChange">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_flr"
                                                PopupControlID="Panel1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_rm" runat="server" Visible="false" Text="Room" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" Visible="false">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_rm" runat="server" CssClass="textbox txtheight2" Style="font-weight: bold;
                                                font-family: book antiqua; width: 100px; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel3" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                                position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                                width: 175px; height: 200px;">
                                                <asp:CheckBox ID="cb_rm" runat="server" Text="Select All" OnCheckedChanged="cb_rm_CheckedChange"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_rm" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_rm_SelectedIndexChange">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_rm"
                                                PopupControlID="Panel3" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:CheckBox ID="cb_building" runat="server" Visible="false" Style="margin-left: 0px;
                                        font-weight: bold; font-family: book antiqua; font-size: medium;" AutoPostBack="true"
                                        Text="Building" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cb_floor" runat="server" Visible="false" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;" AutoPostBack="true" Text="Floor" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cb_room" runat="server" Visible="false" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;" AutoPostBack="true" Text="Room" />
                                </td>
                                <td>
                                    <asp:Button ID="btn_go" runat="server" Text="Go" OnClick="btn_go_click" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;" />
                                </td>
                                <td>
                                    <asp:Button ID="btn_new" runat="server" Text="Add New" Visible="false" OnClick="btn_new_Click"
                                        Style="font-weight: bold; font-family: book antiqua; font-size: medium;" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </fieldset>
                </br>
                <FarPoint:FpSpread ID="FpSpread" runat="server" Visible="false" BorderColor="Black"
                    BorderStyle="Solid" BorderWidth="1px" Width="900px" Height="290px" Style="margin-left: 0px;
                    border: 2px solid #999999; background-color: White; box-shadow: 0px 0px 8px #999999;
                    /*f0f0f0*/ border-radius: 10px; overflow: auto;" class="spreadborder" ShowHeaderSelection="false">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                </br><center>
                    <asp:Button ID="btn_update" runat="server" Visible="false" Text="Update" OnClick="btn_update_click"
                        Style="font-weight: bold; font-family: book antiqua; font-size: medium;" />
                    <asp:Button ID="btn_Delete1" runat="server" Text="Delete" Visible="false" OnClick="btn_Delete1_Click"
                        Style="font-weight: bold; font-family: book antiqua; font-size: medium;" />
                    <asp:Label ID="lbl_alert" runat="server" Visible="false" Style="color: red; font-weight: bold;
                        font-family: book antiqua; font-size: medium;"></asp:Label>
                    <div id="rptprint" runat="server" visible="false">
                        <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium;" Text="" CssClass="sty" Visible="true"></asp:Label>
                        <br />
                        <asp:Label ID="lblrptname" runat="server" Style="font-weight: bold; font-family: book antiqua;
                            font-size: medium;" CssClass="sty" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" CssClass="sty" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium;" onkeypress="display12()"></asp:TextBox>
                        <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium;" CssClass="sty" OnClientClick="return checktxt()"
                            Text="Export To Excel" Width="130px" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium;" OnClick="btnprintmaster_Click"
                            CssClass="sty" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                </center>
            </fieldset>
        </center>
        <center>
            <div id="popper1" runat="server" visible="false" style="height: 1000px; z-index: 1000;
                width: 1000px; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                    width: 30px; position: absolute; margin-top: 15px; margin-left: 453px;" OnClick="ImageButton1_Click" />
                <br />
                <center>
                    <div id="pop1" runat="server" class="sty2" style="background-color: White; height: 830px;
                        width: 950px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <div>
                            <center>
                                <span style="color: Green; font-family: book antiqua; font-weight: bold; font-size: large;">
                                    Building Master</span>
                            </center>
                        </div>
                        </br> </br>
                        <asp:Label ID="lbl_nofbuild" runat="server" Visible="false" Text="Total No Of Buildings"
                            Style="font-weight: bold; font-family: book antiqua; margin-left: -374px; font-size: medium;"></asp:Label>
                        <asp:TextBox ID="txt_nofbuild" runat="server" Visible="false" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium; width: 75px;"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_nofbuild"
                            FilterType="Numbers" ValidChars="Numbers">
                        </asp:FilteredTextBoxExtender>
                        <%--<asp:CheckBox ID="cb_new" runat="server" AutoPostBack="true" Text="Add New Building" style="font-weight: bold;
                                font-family: book antiqua; margin-left:40px; font-size: medium;" OnCheckedChanged="cb_new_ChekedChange" /> --%>
                        </br> </br>
                        <asp:Label ID="lbl_buildacr" runat="server" Visible="false" Text="Building Acronym"
                            Style="font-weight: bold; font-family: book antiqua; margin-left: -39px; font-size: medium;"></asp:Label>
                        <asp:TextBox ID="txt_buildacr" runat="server" Visible="false" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium; width: 75px;"></asp:TextBox>
                        <asp:Label ID="lbl_serial" runat="server" Visible="false" Text="Serial Starts With"
                            Style="font-weight: bold; font-family: book antiqua; margin-left: 40px; font-size: medium;"></asp:Label>
                        <asp:TextBox ID="txt_serial" runat="server" Visible="false" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium; width: 75px;"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_serial"
                            FilterType="Numbers" ValidChars="Numbers">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btn_popgo" runat="server" Visible="false" Text="Go" OnClick="btn_popgo_click"
                            Style="font-weight: bold; font-family: book antiqua; font-size: medium;" />
                        </br> </br>
                        <FarPoint:FpSpread ID="FpSpread1" runat="server" OnButtonCommand="btn_spread_click"
                            BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Width="930px" Height="290px"
                            class="spreadborder" ShowHeaderSelection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        </br>
                        <asp:Button ID="btn_save" runat="server" Text="Save" Visible="false" OnClick="btn_save_click"
                            Style="font-weight: bold; font-family: book antiqua; font-size: medium;" />
                        <asp:Button ID="btn_delete" runat="server" Visible="false" OnClick="btn_Delete1_Click"
                            Text="Delete" Style="font-weight: bold; font-family: book antiqua; font-size: medium;" />
                        <asp:Label ID="lbl_err" runat="server" Visible="false" Style="color: red; font-weight: bold;
                            font-family: book antiqua; font-size: medium;"></asp:Label>
                    </div>
                </center>
            </div>
        </center>
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
                                    <asp:Label ID="lbl_alerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_errorclose" Visible="false" CssClass="textbox btn1" Style="height: 28px;
                                            width: 65px;" Text="OK" runat="server" OnClick="btn_errorclose_Click" />
                                        <asp:Button ID="btn_yes" Visible="false" CssClass="textbox btn1" Style="height: 28px;
                                            width: 65px;" Text="Yes" runat="server" OnClick="btn_yes_Click" />
                                        <asp:Button ID="btn_no" Visible="false" CssClass="textbox btn1" Style="height: 28px;
                                            width: 65px;" Text="No" runat="server" OnClick="btn_errorclose_Click" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>

         <center>
        <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="Div1" runat="server" class="table" style="background-color: White; height: 120px;
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

        <center>
            <div id="div_floor" runat="server" visible="false" style="height: 1000px; z-index: 1000;
                width: 1000px; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                    width: 30px; position: absolute; margin-top: 15px; margin-left: 290px;" OnClick="ImageButton2_Click" />
                <br />
                <center>
                    <div id="Div2" runat="server" class="sty2" style="background-color: White; height: 570px;
                        width: 640px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <div>
                            <center>
                                <span style="color: Green; font-family: book antiqua; font-weight: bold; font-size: large;">
                                    Floor Master</span>
                            </center>
                        </div>
                        </br> </br>
                        <asp:Label ID="lbl_bname" runat="server" Visible="false" Text="Building Name" Style="font-weight: bold;
                            font-family: book antiqua; margin-left: -374px; font-size: medium;"></asp:Label>
                        <asp:TextBox ID="txt_bname" runat="server" Visible="false" ReadOnly="true" Enabled="false"
                            Style="font-weight: bold; font-family: book antiqua; font-size: medium; width: 75px;"></asp:TextBox>
                        <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_nofbuild"
                                                        FilterType="Numbers" ValidChars="Numbers">
                                                    </asp:FilteredTextBoxExtender>
            <asp:CheckBox ID="cb_new" runat="server" AutoPostBack="true" Text="Add New Building" style="font-weight: bold;
                                font-family: book antiqua; margin-left:40px; font-size: medium;" OnCheckedChanged="cb_new_ChekedChange" /> --%>
                        </br> </br>
                        <asp:Label ID="lbl_totf" runat="server" Visible="false" Text="Total No Of Floors"
                            Style="font-weight: bold; font-family: book antiqua; margin-left: -162px; font-size: medium;"></asp:Label>
                        <asp:TextBox ID="txt_totf" runat="server" Visible="false" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium; width: 75px;"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_totf"
                            FilterType="Numbers" ValidChars="Numbers">
                        </asp:FilteredTextBoxExtender>
                        <asp:Label ID="lbl_facr" runat="server" Visible="false" Text="Floor Acronym" Style="font-weight: bold;
                            font-family: book antiqua; margin-left: 40px; font-size: medium;"></asp:Label>
                        <asp:TextBox ID="txt_facr" runat="server" Visible="false" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium; width: 75px;"></asp:TextBox></br></br>
                        <asp:Label ID="lbl_ssw" runat="server" Visible="false" Text="Serial Starts With"
                            Style="font-weight: bold; font-family: book antiqua; margin-left: -300px; font-size: medium;"></asp:Label>
                        <asp:TextBox ID="txt_ssw" runat="server" Visible="false" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium; width: 75px;"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_ssw"
                            FilterType="Numbers" ValidChars="Numbers">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btn_fgo" runat="server" Visible="false" Text="Go" OnClick="btn_fgo_click"
                            Style="font-weight: bold; font-family: book antiqua; margin-left: 40px; font-size: medium;" />
                        </br> </br>
                        <FarPoint:FpSpread ID="FpSpread2" runat="server" Visible="false" BorderColor="Black"
                            OnButtonCommand="btn_roomspread_click" BorderStyle="Solid" BorderWidth="1px"
                            Width="570px" Height="290px" class="spreadborder" ShowHeaderSelection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        </br>
                        <asp:Button ID="btn_flrsave" runat="server" Text="Save" Visible="false" OnClick="btn_flrsave_click"
                            Style="font-weight: bold; font-family: book antiqua; font-size: medium;" />
                        <asp:Button ID="btn_flrdelete" runat="server" Visible="false" Text="Delete" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium;" />
                        <asp:Label ID="Label5" runat="server" Visible="false" Style="color: red; font-weight: bold;
                            font-family: book antiqua; font-size: medium;"></asp:Label>
                    </div>
                </center>
            </div>
        </center>
        <div id="imgdiv3" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="pnl3" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_alertf" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_flrok" Visible="false" CssClass="textbox btn1" Style="height: 28px;
                                            width: 65px;" Text="OK" runat="server" OnClick="btn_flrok_Click" />
                                        <asp:Button ID="Button2" Visible="false" CssClass="textbox btn1" Style="height: 28px;
                                            width: 65px;" Text="Yes" runat="server" OnClick="btn_yes_Click" />
                                        <asp:Button ID="Button3" Visible="false" CssClass="textbox btn1" Style="height: 28px;
                                            width: 65px;" Text="No" runat="server" OnClick="btn_errorclose_Click" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
        <center>
            <div id="div_room" runat="server" visible="false" style="height: 1000px; z-index: 1000;
                width: 1000px; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                    width: 30px; position: absolute; margin-top: 15px; margin-left: 443px;" OnClick="ImageButton3_Click" />
                <br />
                <center>
                    <div id="Div3" runat="server" class="sty2" style="background-color: White; height: 570px;
                        width: 930px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <div>
                            <center>
                                <span style="color: Green; font-family: book antiqua; font-weight: bold; font-size: large;">
                                    Room Master</span>
                            </center>
                        </div>
                        </br> </br>
                        <asp:Label ID="lbl_rbn" runat="server" Visible="false" Text="Building Name" Style="font-weight: bold;
                            font-family: book antiqua; margin-left: -374px; font-size: medium;"></asp:Label>
                        <asp:TextBox ID="txt_rbn" runat="server" Visible="false" ReadOnly="true" Enabled="false"
                            Style="font-weight: bold; font-family: book antiqua; font-size: medium; width: 75px;"></asp:TextBox>
                        </br> </br>
                        <asp:Label ID="lbl_rflrn" runat="server" Visible="false" Text="Floor Name" Style="font-weight: bold;
                            font-family: book antiqua; margin-left: -117px; font-size: medium;"></asp:Label>
                        <asp:TextBox ID="txt_rflrn" runat="server" Visible="false" ReadOnly="true" Enabled="false"
                            Style="font-weight: bold; font-family: book antiqua; font-size: medium; width: 75px;"></asp:TextBox>
                        <asp:Label ID="lbl_rtot" runat="server" Visible="false" Text="Total No Of Rooms"
                            Style="font-weight: bold; font-family: book antiqua; margin-left: 0px; font-size: medium;"></asp:Label>
                        <asp:TextBox ID="txt_rtot" runat="server" Visible="false" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium; width: 75px;"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txt_rtot"
                            FilterType="Numbers" ValidChars="Numbers">
                        </asp:FilteredTextBoxExtender>
                        </br></br>
                        <asp:Label ID="lbl_racr" runat="server" Visible="false" Text="Room Acronym" Style="font-weight: bold;
                            font-family: book antiqua; margin-left: -82px; font-size: medium;"></asp:Label>
                        <asp:TextBox ID="txt_racr" runat="server" Visible="false" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium; width: 75px;"></asp:TextBox>
                        <asp:Label ID="lbl_ss" runat="server" Visible="false" Text="Serial Starts With" Style="font-weight: bold;
                            font-family: book antiqua; margin-left: 0px; font-size: medium;"></asp:Label>
                        <asp:TextBox ID="txt_ss" runat="server" Visible="false" Style="font-weight: bold;
                            font-family: book antiqua; margin-left: 15px; font-size: medium; width: 75px;"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_ss"
                            FilterType="Numbers" ValidChars="Numbers">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btn_roomgo" runat="server" Visible="false" Text="Go" OnClick="btn_roomgo_click"
                            Style="font-weight: bold; font-family: book antiqua; margin-left: 15px; font-size: medium;" />
                        </br> </br>
                        <FarPoint:FpSpread ID="FpSpread3" runat="server" Visible="false" BorderColor="Black"
                            BorderStyle="Solid" BorderWidth="1px" Width="890px" Height="290px" class="spreadborder"
                            ShowHeaderSelection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        </br>
                        <asp:Button ID="btn_rsave" runat="server" Text="Save" Visible="false" OnClick="btn_rsave_click"
                            Style="font-weight: bold; font-family: book antiqua; font-size: medium;" />
                        <asp:Button ID="btn_rdelete" runat="server" Visible="false" Text="Delete" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium;" />
                        <asp:Label ID="lbl_ralert" runat="server" Visible="false" Style="color: Red; font-weight: bold;
                            font-family: book antiqua; font-size: medium;"></asp:Label>
                    </div>
                </center>
            </div>
        </center>
        <div id="imgdiv4" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="pnl4" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_alertr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_roomok" Visible="false" CssClass="textbox btn1" Style="height: 28px;
                                            width: 65px;" Text="OK" runat="server" OnClick="btn_roomok_Click" />
                                        <asp:Button ID="Button4" Visible="false" CssClass="textbox btn1" Style="height: 28px;
                                            width: 65px;" Text="Yes" runat="server" OnClick="btn_yes_Click" />
                                        <asp:Button ID="Button5" Visible="false" CssClass="textbox btn1" Style="height: 28px;
                                            width: 65px;" Text="No" runat="server" OnClick="btn_errorclose_Click" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
        </fieldset>
        </form>
    </body>
    </html>
</asp:Content>
