<%@ Page Title="Question Paper Mark Entry" Language="C#" MasterPageFile="~/QuestionMOD/QuestionBankSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Individual_mark_entry.aspx.cs" Inherits="Individual_mark_entry" %>

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
                                <span class="Header">Question Paper Mark Entry</span>
                            </center>
                        </td>
                    </tr>
                </thead>
            </table>
            <center>
                <fieldset id="divMain" runat="server" style="margin-left: 0px; border-color: silver;
                    border-radius: 10px; width: 100%; height: auto; width: -moz-max-content; padding: 10px;">
                    <div style="margin-bottom: 35px;">
                        <center>
                            <table style="background-color: #0ca6ca; border: 1px solid #ccc; border-radius: 10px;
                                box-shadow: 0 0 8px #999999; height: auto; margin-left: 0px; margin-top: 8px;
                                padding: 1em; margin-left: 0px; width: 930px;" width="853px" height="120px">
                                <tr>
                                    <td colspan="7" align="right">
                                        <asp:RadioButtonList ID="rblisIntExt" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Table" OnSelectedIndexChanged="rblisIntExt_SelectedIndexChanged"
                                            Style="width: auto; height: 20px; margin: 0px; background-color: #ffccff; border-radius: 10px;
                                            border-color: #6699ee;">
                                            <asp:ListItem Selected="True" Text="Internal" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="External" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td colspan="7" align="left">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkCalculateTotalMarks" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="chkCalculateTotalMarks_CheckedChanged"
                                                        Text="Calculate Total Marks For Students" Style="width: auto; height: auto" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_clg" runat="server" Text="College" Font-Bold="True" ForeColor="Black"
                                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_collegename" runat="server" Font-Bold="True" Font-Size="Medium"
                                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged"
                                            AutoPostBack="true" Width="100px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbatch" runat="server" Text="Batch" Font-Bold="True" ForeColor="Black"
                                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Size="Medium"
                                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                            AutoPostBack="true" Width="58px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="True" Font-Size="Medium"
                                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged"
                                            AutoPostBack="true" Width="65px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" ForeColor="Black"
                                            Font-Size="Medium" Font-Names="Book Antiqua" AutoPostBack="true"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="True" Font-Size="Medium"
                                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                            AutoPostBack="true" Width="130px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" ForeColor="Black"
                                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="True" Font-Size="Medium"
                                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged"
                                            AutoPostBack="true" Width="50px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblsec" runat="server" Text="Sec" Font-Bold="True" ForeColor="Black"
                                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsec" runat="server" Font-Bold="True" Font-Size="Medium"
                                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged"
                                            AutoPostBack="true" Width="50px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Bold="True" ForeColor="Black"
                                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsubject" runat="server" Font-Bold="True" Font-Size="Medium"
                                            Font-Names="Book Antiqua" AutoPostBack="true" Width="106px" OnSelectedIndexChanged="ddlsubject_Selectchanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="14">
                                        <table>
                                            <tr>
                                                <td colspan="3">
                                                    <table id="tbl_testname" visible="true" runat="server">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl_testname" runat="server" Text="Test Name" Width="80px" Font-Bold="True"
                                                                    ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                                                <asp:DropDownList ID="ddl_testname" runat="server" Font-Bold="True" Font-Size="Medium"
                                                                    Font-Names="Book Antiqua" AutoPostBack="true" Width="114px" OnSelectedIndexChanged="ddl_testname_Selectchanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="manth_and_Year" visible="false" runat="server">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl_month" runat="server" Text="Month" Font-Bold="True" ForeColor="Black"
                                                                    Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                                                <asp:DropDownList ID="ddl_month" runat="server" Font-Bold="True" Font-Size="Medium"
                                                                    Font-Names="Book Antiqua" OnSelectedIndexChanged="ddl_month_SelectedIndexChanged"
                                                                    AutoPostBack="true" Width="48px">
                                                                </asp:DropDownList>
                                                                <asp:Label ID="lbl_year" runat="server" Text="Year" Font-Bold="True" ForeColor="Black"
                                                                    Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                                                <asp:DropDownList ID="ddl_year" runat="server" Font-Bold="True" Font-Size="Medium"
                                                                    Font-Names="Book Antiqua" OnSelectedIndexChanged="ddl_year_SelectedIndexChanged"
                                                                    AutoPostBack="true" Width="56px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td colspan="2">
                                                    <asp:Label ID="lbl_question" runat="server" Text="Question Name" Width="130px" Font-Bold="True"
                                                        ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="Txt_question" Width=" 243px" ReadOnly="true" Font-Bold="True" ForeColor="Black"
                                                                runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                                            <asp:Panel ID="Panel_question" runat="server" CssClass="multxtpanel" Height="200px"
                                                                Width="250px">
                                                                <asp:CheckBox ID="Cb_qstn" CssClass="fontCommon" runat="server" Text="Select All"
                                                                    AutoPostBack="True" OnCheckedChanged="Cb_qstn_CheckedChanged" />
                                                                <asp:CheckBoxList ID="Cbl_qstn" CssClass="fontCommon" runat="server" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="Cbl_qstn_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="Txt_question"
                                                                PopupControlID="Panel_question" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td colspan="3">
                                                    <asp:Label ID="lbl_rollnumber" runat="server" Text="Roll Number" Width="100px" Font-Bold="True"
                                                        ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="Txt_rollno" Width=" 100px" ReadOnly="true" Font-Bold="True" ForeColor="Black"
                                                                runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                                            <asp:Panel ID="Panel_rollno" runat="server" CssClass="multxtpanel" Height="200px"
                                                                Width="250px">
                                                                <asp:CheckBox ID="Cb_rollno" CssClass="fontCommon" runat="server" Text="Select All"
                                                                    AutoPostBack="True" OnCheckedChanged="Cb_rollno_CheckedChanged" />
                                                                <asp:CheckBoxList ID="Cbl_rollno" CssClass="fontCommon" runat="server" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="Cbl_rollno_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="Txt_rollno"
                                                                PopupControlID="Panel_rollno" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
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
                    </div>
                    <div style="margin: 5px; padding: 5px;">
                        <FarPoint:FpSpread ID="FpSpread1" Visible="false" Tab-ActiveTabBorderColor="Red"
                            runat="server" BorderStyle="Solid" BorderWidth="0px" ShowHeaderSelection="false"
                            CssClass="spreadborder" Style="height: auto; margin: 3px;" OnButtonCommand="FpSpread1_ButtonCommand">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <asp:Button ID="btn_save" Text="Save" Visible="false" CssClass="textbox textbox1 btn1"
                            Height="32px" Width="50px" runat="server" OnClick="btn_save_Click" Style="margin: 5px;
                            padding: 5px;" />
                    </div>
                </fieldset>
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
