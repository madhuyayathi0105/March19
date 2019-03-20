<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="StockAnalyser.aspx.cs" Inherits="LibraryMod_StockAnalyser" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <asp:Label ID="lblHeading" runat="server" Style="margin: 0px; margin-top: 8px; margin-bottom: 8px;
                position: relative;" Text="Data Scanning As On Date - " ForeColor="Green" CssClass="fontstyleheader"></asp:Label>
    </center>
    <br />
    <center>
        <div style="width: 840px; height: 550px; background-color: lightyellow;">
            <div>
                <table id="tableBooklist" runat="server" visible="true" style="width: 835px; height: auto;
                    font-family: Book Antiqua; font-weight: bold; padding: 6px; margin: 0px; margin-bottom: 15px;
                    margin-top: 10px;">
                    <tr>
                        <td>
                            <asp:Label ID="LblCollege" runat="server" Text="College"></asp:Label>
                            <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:Label ID="LblLibrary" runat="server" Text="Library"></asp:Label>
                            <asp:DropDownList ID="ddlLibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlLibrary_OnSelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:Label ID="LblBkType" runat="server" Text="Book Type" CssClass="commonHeaderFont">
                            </asp:Label>
                            <asp:DropDownList ID="ddlBookType" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                Width="200px" AutoPostBack="True">
                                <asp:ListItem Text="Books"></asp:ListItem>
                                <%-- <asp:ListItem Text="Project Books"></asp:ListItem>--%>
                                <%-- <asp:ListItem Text="Non Book Materials"></asp:ListItem>
                        <asp:ListItem Text="Back Volume"></asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <fieldset style="width: 778px; height: 373px; border: 2px solid #000000; margin-top: 20px;">
                                <table>
                                    <tr>
                                        <td>
                                            <fieldset style="width: 127px; height: 275px; border: 2px solid #000000;">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:LinkButton ID="lnkbtn_Stock" runat="server" Text="Stock :" Font-Underline="false"
                                                                ForeColor="Black" Width="120px" OnClick="lnkbtn_Stock_OnClick"></asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="LblStockValue" runat="server" Text="0" CssClass="commonHeaderFont">
                                                            </asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:LinkButton ID="lnkbtn_Scan" runat="server" Text="To Be Scan :" Width="120px"
                                                                Font-Underline="false" ForeColor="Black" OnClick="lnkbtn_Scan_OnClick"></asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="LblTobeScanValue" runat="server" Text="0" CssClass="commonHeaderFont"
                                                                ForeColor="Blue">
                                                            </asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:LinkButton ID="lnkbtn_Bind" runat="server" Text="Binding :" Font-Underline="false"
                                                                ForeColor="Black" Width="120px" OnClick="lnkbtn_Bind_OnClick"></asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="LblBindingValue" runat="server" Text="0" CssClass="commonHeaderFont">
                                                            </asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:LinkButton ID="lnkbtn_Transfer" runat="server" Text="Transfered :" Font-Underline="false"
                                                                ForeColor="Black" Width="120px" OnClick="lnkbtn_Transfer_OnClick"></asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="LblTransferedValue" runat="server" Text="0" CssClass="commonHeaderFont">
                                                            </asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:LinkButton ID="lnkbtn_Issued" runat="server" Text="Issued :" Font-Underline="false"
                                                                ForeColor="Black" Width="120px" OnClick="lnkbtn_Issued_OnClick"></asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="LblIssuedValue" runat="server" Text="0" CssClass="commonHeaderFont">
                                                            </asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:LinkButton ID="lnkbtn_IssueVerify" runat="server" Text="Issued (Verified):"
                                                                Font-Underline="false" ForeColor="Black" Width="140px" OnClick="lnkbtn_IssueVerify_OnClick"></asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="LblIssueVerifyValue" runat="server" Text="0" CssClass="commonHeaderFont">
                                                            </asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:LinkButton ID="lnkbtn_Lost" runat="server" Text="Lost :" Font-Underline="false"
                                                                ForeColor="Black" Width="120px" OnClick="lnkbtn_Lost_OnClick"></asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="LblLostValue" runat="server" Text="0" CssClass="commonHeaderFont">
                                                            </asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:LinkButton ID="lnkbtn_Withdraw" runat="server" Text="Withdrawn :" Font-Underline="false"
                                                                ForeColor="Black" Width="120px" OnClick="lnkbtn_Withdraw_OnClick"></asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="LblWithdrawnValue" runat="server" Text="0" CssClass="commonHeaderFont">
                                                            </asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:LinkButton ID="lnkbtn_Missing" runat="server" Text="Missing :" Font-Underline="false"
                                                                ForeColor="Black" Width="120px" OnClick="lnkbtn_Missing_OnClick"></asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="LblMissingValue" runat="server" Text="0" CssClass="commonHeaderFont">
                                                            </asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LblTotal" runat="server" Text="Total :" Width="120px" CssClass="commonHeaderFont">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="LblTotalValue" runat="server" Text="0" ForeColor="Blue" CssClass="commonHeaderFont">
                                                            </asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Button ID="btnprint" runat="server" Text="Print" OnClick="btnprint_Click" Style="margin-left: 68px;" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                            <br />
                                            <asp:Label ID="LblAccNo" runat="server" Text="Acc No:"></asp:Label>
                                            <asp:TextBox ID="Txt_AccNo" runat="server" CssClass="textbox textbox1 txtheight1"
                                                AutoPostBack="true" OnTextChanged="txt_accno_OnTextChanged"></asp:TextBox>
                                        </td>
                                        <td>
                                            <fieldset id="fldstReport" runat="server" style="height: 350px; width: 550px; border: 2px solid #000000;"
                                                visible="false" class="cursor">
                                                <legend id="LedgendName" runat="server"></legend>
                                                <div id="divReport" runat="server" style="width: 550px; height: 300px; overflow: auto">
                                                    <asp:GridView ID="grdReport" Width="550px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                                        Font-Names="Book Antiqua" ShowHeader="false" toGenerateColumns="false">
                                                        <%--OnRowDataBound="grdReport_RowDataBound"--%>
                                                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                            <asp:CheckBox ID="chk_Lost" runat="server" Text="Include Previous years lost books"
                                                Visible="false" Style="margin-left: 140px;" />
                                            <fieldset id="FldsetScan" runat="server" style="height: 95px; margin-left: 140px;
                                                width: 250px; border: 2px solid #000000;" visible="false" class="cursor">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="rbNewScan" runat="server" Text="New Scan" GroupName="Rbgrp"
                                                                AutoPostBack="true" OnCheckedChanged="rbNewScan_OnCheckedChanged" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="rbContinueScan" runat="server" Text="Continue with Previous Scan"
                                                                GroupName="Rbgrp" Checked="true" AutoPostBack="true" OnCheckedChanged="rbContinueScan_OnCheckedChanged" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="BtnOk" runat="server" Text="Ok" OnClick="BtnOk_Click" Style="margin-left: 164px;" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divConPrevScan" runat="server" visible="false">
                <asp:Label ID="LblScanbk" runat="server" Text="Scanning for Books " Font-Bold="true"
                    Font-Names="book antiqua" Style="margin-left: -200px;" CssClass="commonHeaderFont">
                </asp:Label>
                <br />
                <br />
                <div id="div2" runat="server" style="width: 700px; height: 400px; overflow: auto">
                    <asp:GridView ID="GrdScanBook" Width="700px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                        Font-Names="Book Antiqua" ShowHeader="false" toGenerateColumns="false" OnRowDataBound="GrdScanBook_RowDataBound">
                        <%----%>
                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                        <Columns>
                            <asp:TemplateField HeaderText="S.No">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lbl_sno" runat="server" Style="width: auto;" Text='<%#Eval("Sno") %>'></asp:Label>
                                    </center>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="allchk" runat="server" Text="Select" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="selectchk" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <asp:Label ID="lbl_noofrec" runat="server" Text="" Style="font-family: Book Antiqua;
                    font-weight: bold; margin-left: -675px;" CssClass="commonHeaderFont"></asp:Label>
            </div>
            <div>
                <fieldset style="width: 778px; height: 25px; font-family: Book Antiqua; border: 2px solid #000000;
                    margin-top: 20px; margin-left: -10px;">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnStartScan" runat="server" Enabled="false" Width="100px" CssClass="textbox btn1 textbox1"
                                    Text="Start Scan" OnClick="btnStartScan_Click" Style="font-family: Book Antiqua;
                                    font-weight: bold; font-size: large;" />
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="btnUndoScan" runat="server" Enabled="false" Width="100px" CssClass="textbox btn1 textbox1"
                                    Text="Undo Scan" OnClick="btnUndoScan_Click" Style="font-family: Book Antiqua;
                                    font-weight: bold; font-size: large;" />
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="btnConfirm" runat="server" Enabled="false" Width="100px" CssClass="textbox btn1 textbox1"
                                    Text="Confirm" OnClick="btnConfirm_Click" Style="font-family: Book Antiqua; font-weight: bold;
                                    font-size: large;" />
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="btnComPrint" runat="server" Enabled="false" Width="100px" CssClass="textbox btn1 textbox1"
                                    Text="Print" OnClick="btnComPrint_Click" Style="font-family: Book Antiqua; font-weight: bold;
                                    font-size: large;" />
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="btnBack" runat="server" Enabled="false" Width="100px" CssClass="textbox btn1 textbox1"
                                    Text="Back" OnClick="btnBack_Click" Style="font-family: Book Antiqua; font-weight: bold;
                                    font-size: large;" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </div>
    </center>
    <%-- Popup for Yes or No--%>
    <center>
        <div id="DivYesOrNo" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="Div1" runat="server" class="table" style="background-color: White; font-family: Book Antiqua;
                    height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="LblName" runat="server" Text="Are you sure to start new scan, Last scan details are removed"
                                        Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="BtnYes" runat="server" Text="Yes" OnClick="BtnYes_Click" />
                                        <asp:Button ID="BtnNo" runat="server" Text="No" OnClick="BtnNo_Click" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <%-- Popup for Scanned Yes or No--%>
    <center>
        <div id="DivScanYes" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="Div4" runat="server" class="table" style="background-color: White; font-family: Book Antiqua;
                    height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label1" runat="server" Text="Do you want to use Scanned Text File ?"
                                        Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="BtnScanYes" runat="server" Text="Yes" OnClick="BtnScanYes_Click" />
                                        <asp:Button ID="BtnScanNo" runat="server" Text="No" OnClick="BtnScanNo_Click" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <%-- Popup for BookScanning--%>
    <center>
        <div id="DivBookScanning" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="Div5" runat="server" class="table" style="background-color: White; font-family: Book Antiqua;
                    height: 120px; width: 320px; border: 5px solid #0CA6CA; font-family: Book Antiqua;
                    border-top: 25px solid #0CA6CA; margin-top: 200px; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td>
                                    <center>
                                        <asp:Label ID="Label2" runat="server" Text="Book Scanning" Style="color: Green;"
                                            Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </center>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="LabelAcc" runat="server" Text="Acc No:" Style="margin-left: 50px;"></asp:Label>
                                    <asp:TextBox ID="txt_access" runat="server" CssClass="textbox textbox1 txtheight1"
                                        AutoPostBack="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="BtnAdd" runat="server" Text="Add" Style="font-family: Book Antiqua;"
                                            OnClick="BtnAdd_Click" />
                                        <asp:Button ID="BtnDelete" runat="server" Text="Delete" Style="font-family: Book Antiqua;"
                                            OnClick="BtnDelete_Click" />
                                        <asp:Button ID="BtnConfirmation" runat="server" Text="Confirm" Style="font-family: Book Antiqua;"
                                            OnClick="BtnConfirmation_Click" />
                                        <asp:Button ID="BtnExit" runat="server" Text="Exit" Style="font-family: Book Antiqua;"
                                            OnClick="BtnExit_Click" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <%-- Popup for OK and Cancel--%>
    <center>
        <div id="DivScanConfirm" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="Div6" runat="server" class="table" style="background-color: White; font-family: Book Antiqua;
                    height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label3" runat="server" Text="Are you sure to Confirm Scanning ? "
                                        Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="Btn_OK" runat="server" Text="Ok" OnClick="Btn_OK_Click" />
                                        <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" OnClick="Btn_Cancel_Click" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <%-- Popup for Book Status Update Yes or No--%>
    <center>
        <div id="DivBkStatus" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="Div7" runat="server" class="table" style="background-color: White; font-family: Book Antiqua;
                    height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="LblBookStatus" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="BtnBkStatusYes" runat="server" Text="Yes" OnClick="BtnBkStatusYes_Click" />
                                        <asp:Button ID="BtnBkStatusNo" runat="server" Text="No" OnClick="BtnBkStatusNo_Click" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <%-- Popup for Error Message--%>
    <center>
        <div id="DivErrorMsg" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="Div8" runat="server" class="table" style="background-color: White; font-family: Book Antiqua;
                    height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="LblErrorMsg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
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
