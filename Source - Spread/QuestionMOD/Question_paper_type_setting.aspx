<%@ Page Title="Question Paper Type Setting" Language="C#" MasterPageFile="~/QuestionMOD/QuestionBankSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Question_paper_type_setting.aspx.cs" Inherits="Question_paper_type_setting" %>

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
    <script type="text/javascript">
        function PrintDiv() {
            var panel = document.getElementById("<%=questionPaper.ClientID %>");
            var printWindow = window.open('', '', 'height=1123,width=794');
            printWindow.document.write('<html><head>');
            printWindow.document.write('<style>.collegeHeader{font-weight:bold; font-size:16px; margin-bottom:10px;} .subHead{font-weight:bold; font-size:14px; margin-bottom:10px;} </style>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                //<div id="footer" style="background-color:White;"></div> 
                // <div id="header" style="background-color:White;"></div>
                //                document.getElementById('header').style.display = 'none';
                //                document.getElementById('footer').style.display = 'none';
                printWindow.print();
            }, 500);
            return false;
        }           
    </script>
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
                                <span class="Header">Question Paper Type Setting</span>
                            </center>
                        </td>
                    </tr>
                </thead>
            </table>
            <center>
                <div class="maindivstyle" style="width: 100%; height: auto; padding:10px;">
                    <div>
                        <center>
                            <table class="maintablestyle fontCommon" width="933px" style="margin: 10px; height: auto;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_clg" runat="server" Text="College"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="fontCommon" OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged"
                                            AutoPostBack="true" Width="100px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbatch" runat="server" Text="Batch"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbatch" runat="server" CssClass="fontCommon" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                            AutoPostBack="true" Width="100px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldegree" runat="server" Text="Degree"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddldegree" runat="server" CssClass="fontCommon" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged"
                                            AutoPostBack="true" Width="130px">
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
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblsem" runat="server" Text="Sem" CssClass="fontCommon"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsem" runat="server" CssClass="fontCommon" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged"
                                            AutoPostBack="true" Width="50px">
                                        </asp:DropDownList>
                                    </td>
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
                        </center>
                        <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </div>
                    <div id="objective_check" runat="server" visible="false" style="margin:20px;">
                        <center>
                            <FarPoint:FpSpread ID="FpSpread1" Width="476" runat="server" CssClass="spreadborder"
                                Visible="true" BorderStyle="Solid" BorderWidth="0px" Style="overflow: scroll;
                                border: 0px solid #999999; border-radius: 10px; margin:15px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                ShowHeaderSelection="false" OnCellClick="FpSpread1_OnCellClick" OnPreRender="FpSpread1_Selectedindexchange">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </center>
                        <%--  <asp:CheckBox ID="chk_option" runat="server" Text="Option" AutoPostBack="True" OnCheckedChanged="chk_option_CheckedChanged" />
                        --%>
                        <center>
                            <asp:CheckBox ID="chk_answer" runat="server" Text="Answer" AutoPostBack="True" OnCheckedChanged="chk_answer_CheckedChanged"  style="margin:15px;"/>
                        </center>
                        <FarPoint:FpSpread ID="FpSpread2" runat="server" Visible="true" BorderStyle="Solid"
                            BorderWidth="0px" CssClass="spreadborder" Style="overflow: scroll; width: auto;
                            height: auto; border: 0px solid #999999; border-radius: 10px; background-color: White;
                            box-shadow: 0px 0px 8px #999999; margin:15px;" ShowHeaderSelection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <center>
                            <asp:Button ID="btn_gendrate" Width="90px" runat="server" Text="Generate " CssClass="textbox textbox1 defaultHeight"
                                OnClick="btn_gendrate_Click" style="margin:15px;"/>
                        </center>
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
            <div style="height: 1px; width: 1px; overflow: auto;">
                <div id="questionPaper" runat="server" visible="false" style="width: 794px; height: 1123px;">
                </div>
            </div>
        </div>
    </center>
</asp:Content>
