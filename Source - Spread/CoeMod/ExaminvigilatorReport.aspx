<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ExaminvigilatorReport.aspx.cs" Inherits="ExaminvigilatorReport"
    EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .stylefp
        {
            cursor: pointer;
        }
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        .HellowWorldPopup
        {
            min-width: 600px;
            min-height: 400px;
            background: white;
        }
        .cpHeader
        {
            color: white;
            background-color: #719DDB;
            font-size: 12px;
            cursor: pointer;
            padding: 4px;
            font-style: normal;
            font-variant: normal;
            font-weight: bold;
            line-height: normal;
            font-family: "auto Trebuchet MS" , Verdana;
        }
        .cpBody
        {
            background-color: #DCE4F9;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
            width: 1000px;
        }
        .cpimage
        {
            float: right;
            vertical-align: middle;
            background-color: transparent;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin-bottom: 10px; margin-top: 10px;
            position: relative;">Exam Invigilator Report</span>
        <asp:Panel ID="Panel3" runat="server" BackColor="#0CA6CA" BorderColor="Black" BorderStyle="Solid"
            ClientIDMode="Static" Width="1000px" BorderWidth="1px" Style="margin-bottom: 66px;
            margin-top: 10px; height: auto; text-align: left;">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblyear" runat="server" Text="Year" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlYear" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                            Width="100px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblmonth" runat="server" Text="Month" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMonth" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                            Width="100px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbltype" runat="server" Text="Type" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddltype" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" AutoPostBack="True" Width="100px" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblfrmdate" runat="server" Text="From " Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtfromdate" runat="server" CssClass="font" Width="80px" OnTextChanged="txtfromdate_TextChanged"
                            AutoPostBack="true" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        <asp:CalendarExtender ID="cetxtExamToDate" runat="server" TargetControlID="txtfromdate"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Label ID="lbltodate" runat="server" Text="To" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txttodate" runat="server" CssClass="font" Width="80px" OnTextChanged="txtToDate_TextChanged"
                            AutoPostBack="true" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="Session" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownList1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" AutoPostBack="True" Width="100px" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="12">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblhallnno" runat="server" Text="Hall No" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                        <ContentTemplate>
                                            <asp:TextBox ID="tbdep" runat="server" Height="20px" Width="100px" Font-Bold="True"
                                                ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" ReadOnly="true"
                                                Style="width: 100px">- - All - -</asp:TextBox>
                                            <asp:Panel ID="Pdep" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" Height="280px" ScrollBars="Vertical" Width="170px">
                                                <asp:CheckBox ID="cbdepselectall" runat="server" AutoPostBack="True" OnCheckedChanged="Pdep_CheckedChanged"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 21px;
                                                    width: 89px" Text="Select All" />
                                                <asp:CheckBoxList ID="Chkdep" runat="server" Font-Size="Small" AutoPostBack="True"
                                                    Font-Bold="True" ForeColor="Black" OnSelectedIndexChanged="Pdep_SelectedIndexChanged"
                                                    Font-Names="Book Antiqua" Height="39px">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="tbdep"
                                                PopupControlID="Pdep" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btngo" CssClass="textbox textbox1" Style="width: auto; height: auto;"
                                        runat="server" Font-Bold="True" Text="Go" Font-Names="Book Antiqua" Font-Size="Medium"
                                        OnClick="btngo_Click" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rbstaff" runat="server" Text="Staff Wise" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" GroupName="Report" AutoPostBack="true" OnCheckedChanged="reportchange" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rbdate" runat="server" Text="Date Wise" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" GroupName="Report" AutoPostBack="true" OnCheckedChanged="reportchange" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="12">
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkautomatic" runat="server" Text="Automatic" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="chkautomatic_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:Label ID="lblstaffperstu" runat="server" Text="Staff Per Student" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtstaffperstu" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" MaxLength="3" Style="width: 50px"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtstaffperstu"
                                        FilterType="Numbers" />
                                </td>
                                <td>
                                    <asp:Label ID="lblattdinationstaff" runat="server" Text="Addtional Staff Required"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    <asp:TextBox ID="txtaddtionalstafff" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" MaxLength="3" Style="width: 50px"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtaddtionalstafff"
                                        FilterType="Numbers" />
                                    <asp:Button ID="btngenerate" CssClass="textbox textbox1" Style="width: auto; height: auto;"
                                        runat="server" Font-Bold="True" Text="Generate" Font-Names="Book Antiqua" Font-Size="Medium"
                                        OnClick="btngenerate_Click" />
                                    <asp:CheckBox ID="cb_SMS" runat="server" Text="SMS" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" AutoPostBack="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </center>
    <center>
        <%--OnPreRender="AttSpread_SelectedIndexChanged" OnCellClick="AttSpread_OnCellClicked" --%>
        <asp:Label ID="lblerr1" runat="server" Text="" Font-Bold="True" ForeColor="Red" Font-Names="Book Antiqua"
            Font-Size="Medium" Visible="false" Style="margin-bottom: 10px; margin-top: 10px;
            position: relative;"></asp:Label>
        <asp:Label ID="lblDate" runat="server" Text="Select Date" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Small" ForeColor="#FF3300" Visible="False" Style="margin-bottom: 10px;
            margin-top: 10px; position: relative;"></asp:Label>
        <FarPoint:FpSpread ID="AttSpread" runat="server" BorderColor="Black" BorderStyle="Solid"
            BorderWidth="1px" Visible="false" Style="margin-bottom: 10px; margin-top: 10px;
            position: relative;" OnButtonCommand="AttSpread_ButtonCommand" CssClass="stylefp"
            ShowHeaderSelection="false">
            <%--OnPreRender="AttSpread_SelectedIndexChanged" OnCellClick="AttSpread_OnCellClicked" --%>
            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                ButtonShadowColor="ControlDark">
            </CommandBar>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
    </center>
    <table style="margin-bottom: 10px; margin-top: 15px;">
        <tr>
            <td>
                <asp:Label ID="lbledate" runat="server" Text="Exam Date" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddledate" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="lblesession" runat="server" Text="Exam Date" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlesession" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="lblstaff" runat="server" Text="Staff" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlstaff" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Button ID="btnsave" runat="server" CssClass="textbox textbox1" Style="width: auto;
                    height: auto;" Text="Save" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                    OnClick="btnsave_Click" />
            </td>
            <td>
                <asp:Button ID="btndelete" CssClass="textbox textbox1" Style="width: auto; height: auto;"
                    runat="server" Text="Delete" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                    OnClick="btndelete_Click" />
            </td>
            <td>
                <asp:CheckBox ID="chkheadimage" runat="server" Text="Header Image" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Medium" />
            </td>
            <td>
                <asp:Button ID="btnletter" CssClass="textbox textbox1" Style="width: auto; height: auto;"
                    runat="server" Text="Letter Generate" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btnletter_Click" />
            </td>
            <td>
                <asp:Button ID="btnSendSMS" CssClass="textbox textbox1" Style="width: auto; height: auto;"
                    runat="server" Text="Send SMS" Font-Bold="true" Visible="false" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btnSendSMS_Click" />
            </td>
        </tr>
    </table>
    <table style="margin-bottom: 10px; margin-top: 15px;">
        <tr>
            <td>
                <asp:Label ID="lblexcelname" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Report Name"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtexcelname" runat="server" Visible="false" onkeypress="display()"
                    Height="20px" Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                    FilterType="Numbers" ValidChars=",/-() " />
            </td>
            <td>
                <asp:Button ID="btnExcel" CssClass="textbox textbox1" Style="width: auto; height: auto;"
                    runat="server" Visible="false" Font-Bold="true" Font-Names="Book Antiqua" OnClick="btnExcel_Click"
                    Font-Size="Medium" Text="Export To Excel" Width="127px" />
            </td>
            <td>
                <asp:Button ID="btnPrint" CssClass="textbox textbox1" Style="width: auto; height: auto;"
                    runat="server" Visible="false" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                    Text="Print" OnClick="btnPrint_Click1" />
                <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
            </td>
        </tr>
    </table>
    <center>
        <div id="divMoveStaff" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="divStaffMoving" runat="server" class="table" style="background-color: White;
                    height: auto; width: 70%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 15%; right: 15%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%; padding: 5px;">
                            <tr>
                                <td>
                                    Date :
                                </td>
                                <td>
                                    <input type="text" id="txtfromdate2" disabled="disabled" style="width: 70px;" runat="server"
                                        readonly="readonly" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    No of Staff Available :
                                </td>
                                <td>
                                    <input type="text" id="txtStaff1" disabled="disabled" style="width: 50px;" runat="server"
                                        readonly="readonly" />
                                </td>
                                <td>
                                    Staff :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlstafffrom" Width="150px" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    To
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Date :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddldateto" runat="server" OnSelectedIndexChanged="ddldateto_OnSelectedIndexChanged"
                                        Width="88px" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Session :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlsessionto" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlsessionto_OnSelectedIndexChanged"
                                        Width="55px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Hall :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlhallto" Width="60px" runat="server" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" align="center">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="BtnMovestaff" Text="Move" runat="server" Enabled="true" OnClick="BtnMovestaff_OnClick" />
                                            </td>
                                            <td>
                                                <asp:Button ID="BtnMovecncl" Text="Cancel" runat="server" Enabled="true" OnClick="BtnMovecncl_OnClick" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <%-- Add Staff --%>
    <center>
        <div id="divAddStaff" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="divAddStaffContent" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 50%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 25%; right: 25%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%; padding: 5px;">
                            <tr>
                                <td align="center">
                                    <asp:DropDownList ID="ddlAddStaffDetails" Width="250px" runat="server" Font-Bold="true"
                                        Font-Size="Medium" Font-Names="Book Antiqua">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnAddStaffSave" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnAddStaffSave_Click"
                                            Text="Save" runat="server" />
                                        <asp:Button ID="btnAddStaffClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnAddStaffClose_Click"
                                            Text="Exit" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <%-- Alert Box --%>
    <center>
        <div id="divPopAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%; padding: 5px;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                            Text="Ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <%-- Delete Staff --%>
    <center>
        <div id="divDeleteStaff" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="div2" runat="server" class="table" style="background-color: White; height: auto;
                    width: 70%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; left: 15%;
                    right: 15%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblDeleteDate" runat="server" Text="Date :" Width="55px" Font-Bold="true">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblDeleteDateVal" runat="server" Width="85px">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblDeleteSession" runat="server" Text="Session :" Width="75px" Font-Bold="true">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblDeleteSessionVal" runat="server" Width="75px">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblDeleteHall" runat="server" Text="Hall :" Width="75px" Font-Bold="true">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblDeleteHallVal" runat="server" Width="75px">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblDeleteStaff" runat="server" Text="Staff :" Width="75px" Font-Bold="true">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddldeletestaffVal" Width="150px" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                            <td colspan="7">

                            </td>
                            <td>
                            <asp:Button ID="btnDeletestaff" runat="server" OnClick="btnDeletestaff_OnClick" Text="Delete" />
                            </td>
                            <td>
                            <asp:Button ID="btnDeletestaffCancel" runat="server" OnClick="btnDeletestaffCancel_OnClick" Text="Close" />
                            </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
</asp:Content>
