<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Bank_Stmnt_Import.aspx.cs" Inherits="Bank_Stmnt_Import" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title>Bank Import</title>
    <link rel="Shortcut Icon" href="college/Left_Logo.jpeg" />
    <link href="../Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <body>
        <script type="text/javascript">
            window.onload = window.history.forward(0);
            function otherBank(itemid) {
                var txtid = document.getElementById("<%=txt_other.ClientID %>");
                var ddlid = itemid.value;
                if (ddlid.trim().toUpperCase() == "OTHERS") {
                    txtid.style.display = "block";
                } else {
                    txtid.style.display = "none";
                }
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Bank Statement Import</span></div>
            </center>
        </div>
        <center>
            <div class="maindivstyle" style="width: 950px; height: 600px;">
                <%--maincontent--%>
                <br />
                <center>
                    <div id="tblhdr" runat="server">
                        <table class="maintablestyle">
                            <tr>
                                <td colspan="4">
                                    <table style="border-color: White; border-height: 25px;">
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rb_bank" runat="server" AutoPostBack="true" OnCheckedChanged="rb_bank_OnCheckedChanged"
                                                    Text="Bank Statement" GroupName="trans" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rb_atm" runat="server" Visible="false" AutoPostBack="true" OnCheckedChanged="rb_atm_OnCheckedChanged"
                                                    Text="ATM Statement" GroupName="trans" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Label ID="lbl_filename" runat="server" Style="top: 10px; left: 6px;" Text="File Name"></asp:Label>
                                    <asp:TextBox ID="txtfilename" runat="server" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                    <asp:FileUpload ID="FileUpload1" runat="server" Height="25px" CssClass="textbox"
                                        ForeColor="White" />
                                    Receipt Type
                                    <asp:DropDownList ID="ddlRcptType" runat="server" CssClass="textbox textbox1 ddlheight2">
                                        <asp:ListItem Selected="True">Group Header</asp:ListItem>
                                        <asp:ListItem>Header</asp:ListItem>
                                        <asp:ListItem>Ledger</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Button ID="btnimport" runat="server" CssClass="textbox textbox1 btn2" Text="Import"
                                        OnClick="btnimport_Click" />
                                    <asp:CheckBox ID="cb_bfadm" runat="server" Visible="false" Text="Before Admission" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_bankname" runat="server" Text="Bank Name" Style="float: left;
                                                    padding-top: 5px;"></asp:Label>
                                                <asp:DropDownList ID="ddl_bankname" runat="server" CssClass="textbox textbox1 ddlheight2"
                                                    onchange="return otherBank(this);" Width="180px" Style="float: left;">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txt_other" runat="server" CssClass="textbox txtheight2" onfocus="return myFunction(this)"
                                                    Placeholder="Other Bank" Style="display: none; float: left;"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftEbank" runat="server" FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom"
                                                    ValidChars=" " TargetControlID="txt_other">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cb_totfee" runat="server" Text="Total Fees" AutoPostBack="true"
                                                    OnCheckedChanged="cb_totfee_Changed" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlHdrLedger" runat="server" CssClass="textbox textbox1 ddlheight"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlHdrLedger_Indexchanged">
                                                    <asp:ListItem Selected="True">Header</asp:ListItem>
                                                    <asp:ListItem>Ledger</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="upheader" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_HeaderPop" runat="server" ReadOnly="true" Height="20px" CssClass="textbox txtheight">Header</asp:TextBox>
                                                        <asp:Panel ID="Panel1" runat="server" Width="200px" Height="170px" CssClass="multxtpanel">
                                                            <asp:CheckBox ID="cb_HeaderPop" runat="server" OnCheckedChanged="cb_HeaderPop_ChekedChange"
                                                                Text="Select All" AutoPostBack="True" />
                                                            <asp:CheckBoxList ID="cbl_HeaderPop" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_HeaderPop_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_HeaderPop"
                                                            PopupControlID="Panel1" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="upledger" Visible="false" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_Ledgerpop" runat="server" ReadOnly="true" Height="20px" CssClass="textbox txtheight">Ledger</asp:TextBox>
                                                        <asp:Panel ID="Panel2" runat="server" Width="200px" Height="170px" CssClass="multxtpanel">
                                                            <asp:CheckBox ID="cb_ledgerpop" runat="server" OnCheckedChanged="cb_ledgerpop_ChekedChange"
                                                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                            <asp:CheckBoxList ID="cbl_ledgerpop" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_ledgerpop_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_Ledgerpop"
                                                            PopupControlID="Panel2" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lb_hdrset" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Large" ForeColor="Blue" CausesValidation="False" OnClick="lb_hdr_click"
                                                    Text="Header Settings"></asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="cbexset" Enabled="false" runat="server" Text="Excess" AutoPostBack="true"
                                                                OnCheckedChanged="cbexset_Changed" />
                                                        </td>
                                                        <td>
                                                            <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtexcess" runat="server" Enabled="false" ReadOnly="true" Height="20px"
                                                                    CssClass="textbox txtheight">Ledger</asp:TextBox>
                                                                <asp:Panel ID="Panel3" runat="server" Width="200px" Height="170px" CssClass="multxtpanel">
                                                                    <asp:CheckBox ID="cbexcess" runat="server" OnCheckedChanged="cbexcess_ChekedChanged"
                                                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                                    <asp:CheckBoxList ID="cblexcess" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblexcess_SelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtexcess"
                                                                    PopupControlID="Panel3" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>--%>
                                                            <asp:DropDownList ID="ddlexcess" runat="server" Width="150px" Enabled="false" CssClass="textbox textbox1 ddlheight5">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                    <br />
                    <asp:Label ID="lblDisplayValue" runat="server" ForeColor="Green" Text="(* - Roll No or Reg No * - Total Amount * - Sem or Year * - TransDate)"></asp:Label>
                    <br />
                    <div id="div2" runat="server" visible="true" style="width: 800px; height: 350px;
                        overflow: auto; border: 1px solid Gray; border-radius: 10px; background-color: White;
                        box-shadow: 0px 0px 8px #999999;">
                        <br />
                        <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" BorderColor="Black"
                            BorderStyle="Solid" BorderWidth="1px" VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never"
                            Style="overflow: auto; background-color: White;" OnCellClick="Cell_Click" OnPreRender="Fpspread1_render">
                            <%--<CommandBar BackColor="Control" ButtonFaceColor="Control" Visible="false" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark">
                        </CommandBar>--%>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" BorderColor="Black"
                            BorderStyle="Solid" BorderWidth="1px" VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never"
                            Style="overflow: auto; background-color: White;" OnCellClick="Cell_Click" OnPreRender="Fpspread1_render">
                            <%--<CommandBar BackColor="Control" ButtonFaceColor="Control" Visible="false" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark">
                        </CommandBar>--%>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <br />
                    <br />
                    <div id="rptprint" runat="server" visible="true">
                        <asp:Button ID="btnsave" runat="server" CssClass="textbox textbox1 btn2" Text="Save"
                            OnClick="btnsave_Click" />
                        <asp:Button ID="btnexit" runat="server" CssClass="textbox textbox1 btn2" Text="Exit"
                            OnClick="btnexit_Click" />
                        <%--<asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                        Visible="false"></asp:Label>
                    <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="25px" Width="180px" CssClass="textbox textbox1"
                        onkeypress="display()"></asp:TextBox>
                    <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox textbox1"
                        Text="Export To Excel" Width="127px" Height="35px" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        Width="60px" Height="35px" CssClass="textbox textbox1" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />--%>
                    </div>
                    <div id="printId" runat="server" visible="false">
                        <asp:Button ID="btndownload" runat="server" Text="Error Download" OnClick="btndownload_Click" />
                    </div>
                </center>
                <center>
                    <div id="poppernew" runat="server" visible="false" style="height: 70em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                        left: 0;">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                            width: 30px; position: absolute; margin-top: 9px; margin-left: 430px;" OnClick="imagebtnpopclose1_Click" />
                        <br />
                        <center>
                            <div class="popsty" style="background-color: White; height: 550px; width: 900px;
                                border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                                <br />
                                <br />
                                <br />
                                <fieldset style="border-radius: 10px; width: 500px;">
                                    <legend style="font-size: larger; font-weight: bold">Application Header Settings</legend>
                                    <table class="table">
                                        <tr>
                                            <td>
                                                <asp:ListBox ID="lb_selecthdr" runat="server" SelectionMode="Multiple" Height="300px"
                                                    Width="200px"></asp:ListBox>
                                            </td>
                                            <td>
                                                <table class="table1">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnMvOneRt" runat="server" Text=">" CssClass="textbox textbox1 btn1"
                                                                OnClick="btnMvOneRt_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnMvTwoRt" runat="server" Text=">>" CssClass="textbox textbox1 btn1"
                                                                OnClick="btnMvTwoRt_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnMvOneLt" runat="server" Text="<" CssClass="textbox textbox1 btn1"
                                                                OnClick="btnMvOneLt_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnMvTwoLt" runat="server" Text="<<" CssClass="textbox textbox1 btn1"
                                                                OnClick="btnMvTwoLt_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <asp:ListBox ID="lb_hdr" runat="server" SelectionMode="Multiple" Height="300px" Width="200px">
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <center>
                                        <asp:Button ID="btnok" runat="server" Text="OK" CssClass="textbox textbox1 btn2"
                                            OnClick="btnok_click" />
                                        <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="textbox textbox1 btn2"
                                            OnClick="btnclose_click" />
                                    </center>
                                </fieldset>
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
            </div>
        </center>
        <%-- Pop Alert--%>
        <center>
            <div id="imgAlert" runat="server" visible="false" style="height: 100em; z-index: 100000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div1" runat="server" class="table" style="background-color: White; height: 120px;
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
                                            <asp:Button ID="btn_alertclose" CssClass=" textbox btn1 textbox1" Style="height: 28px;
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
    </body>
    </html>
</asp:Content>
