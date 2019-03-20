<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="FineDetailsReport.aspx.cs" Inherits="LibraryMod_FineDetailsReport" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Fine Report</span>
            </div>
        </center>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
            <ContentTemplate>
                <center>
                    <div>
                        <table class="maintablestyle" style="height: auto; font-family: Book Antiqua; font-weight: bold; margin-left: 0px; margin-top: 10px; margin-bottom: 10px; padding: 6px;">
                            <tr>
                                <td>
                                    <asp:RadioButtonList ID="rblreport" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                        OnSelectedIndexChanged="rblreport_Selected">
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div id="divDetail" runat="server" visible="true">
                <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                    <ContentTemplate>
                        <center>
                            <div id="popdetails" runat="server">
                                <table class="maintablestyle" style="height: auto; font-family: Book Antiqua; font-weight: bold; margin-left: 0px; margin-top: 10px; margin-bottom: 10px; padding: 6px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCollege" runat="server" Text="College" CssClass="commonHeaderFont">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Width="160px" AutoPostBack="true" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbllibrary" runat="server" Text="Library" CssClass="commonHeaderFont">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlLibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Width="163px" AutoPostBack="True" OnSelectedIndexChanged="ddllibrary_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="2">
                                            <asp:CheckBox ID="cbdate" runat="server" Enabled="true" AutoPostBack="true" OnCheckedChanged="cbdate_OnCheckedChanged" />
                                            <asp:Label ID="lbl_fromdate" runat="server" Text="From: "></asp:Label>
                                            <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox txtheight2" Enabled="false"
                                                Style="height: 20px; width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txt_fromdate" runat="server"
                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                            </asp:CalendarExtender>
                                            <asp:Label ID="lbl_todate" runat="server" Text="To:" Style="margin-left: 4px;"></asp:Label>
                                            <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox txtheight2" Enabled="false"
                                                Style="height: 20px; width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender6" TargetControlID="txt_todate" runat="server"
                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbldept" runat="server" Text="Department" CssClass="commonHeaderFont">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddldept" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Width="160px" AutoPostBack="True" OnSelectedIndexChanged="ddldept_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblselect" runat="server" Text="Select For" CssClass="commonHeaderFont">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlselect" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Width="160px" AutoPostBack="True" OnSelectedIndexChanged="ddlselect_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblstatus" runat="server" Text="Status" CssClass="commonHeaderFont">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlstatus" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Width="160px" AutoPostBack="True" OnSelectedIndexChanged="ddlstatus_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td id="tdbatch" runat="server" visible="false">
                                            <asp:Label ID="lblbatch" runat="server" Text="Batch" CssClass="commonHeaderFont">
                                            </asp:Label>
                                            <asp:DropDownList ID="ddlBatch" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Width="150px" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblbook" runat="server" Text="Book Type" CssClass="commonHeaderFont">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlbook" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Width="160px" AutoPostBack="True" OnSelectedIndexChanged="ddlbook_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="lblroll" runat="server" Text="Roll No" CssClass="commonHeaderFont">
                                            </asp:Label>
                                            <asp:TextBox ID="txtroll" runat="server" Style="height: 20px; width: 151px; margin-left: 14px;"
                                                CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblsemester" runat="server" Text="Semester" CssClass="commonHeaderFont">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlsem" runat="server" Enabled="false" CssClass="textbox ddlstyle ddlheight3"
                                                Width="160px" AutoPostBack="True" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpGo" runat="server">
                                                <ContentTemplate>
                                                    <asp:ImageButton ID="btngo" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="btngoClick" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btngo" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </center>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <center>
                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                        <ContentTemplate>
                            <center>
                                <div>
                                    <asp:GridView ID="grid_Details" runat="server" ShowFooter="false" ShowHeader="false"
                                        Font-Names="Book Antiqua" OnSelectedIndexChanged="grid_Details_OnSelectedIndexChanged"
                                        Width="1100px" OnRowDataBound="grid_Details_OnRowDataBound">
                                        <%--AllowPaging="true" PageSize="500"  OnPageIndexChanging="grdManualExit_OnPageIndexChanged"--%>
                                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                    </asp:GridView>
                                </div>
                            </center>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </center>
                <center>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div id="print2" runat="server" visible="false">
                                <asp:Label ID="lblvalidation3" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    ForeColor="Red" Text="" Style="display: none;"></asp:Label>
                                <asp:Label ID="lblrptname2" runat="server" Visible="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txtexcelname2" runat="server" Visible="true" Width="180px" onkeypress="display()"
                                    CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                <asp:ImageButton ID="btnExcel2" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                                    Visible="false" OnClick="btnExcel_Click2" />
                                <asp:ImageButton ID="btnprintmasterhed2" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                                    Visible="false" OnClick="btnprintmaster_Click2" />
                                <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnExcel2" />
                            <asp:PostBackTrigger ControlID="btnprintmasterhed2" />
                        </Triggers>
                    </asp:UpdatePanel>
                </center>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <div id="divCum" runat="server" visible="false">
                <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                    <ContentTemplate>
                        <center>
                            <div id="popcumlative" runat="server">
                                <table class="maintablestyle" style="height: auto; font-family: Book Antiqua; font-weight: bold; margin-left: 0px; padding: 6px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblcollege1" runat="server" Text="College" CssClass="commonHeaderFont">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlcollege1" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                        Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlcollege1_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbllibrary1" runat="server" Text="Library" CssClass="commonHeaderFont">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddllibrary1" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                        Width="163px" AutoPostBack="True" OnSelectedIndexChanged="ddllibrary1_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_fromdate2" runat="server" Text="From:" CssClass="commonHeaderFont">
                                            </asp:Label>
                                            <asp:TextBox ID="txt_fromdate2" runat="server" CssClass="textbox txtheight2" Style="height: 20px; width: 75px;"
                                                onchange="return checkDate()"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_fromdate2" runat="server"
                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                            </asp:CalendarExtender>
                                            <asp:Label ID="lbl_todate2" runat="server" Text="To:" Style="margin-left: 4px;"></asp:Label>
                                            <asp:TextBox ID="txt_todate2" runat="server" CssClass="textbox txtheight2" Style="height: 20px; width: 75px;"
                                                onchange="return checkDate()"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="txt_todate2" runat="server"
                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblsem1" runat="server" Text="Semester" CssClass="commonHeaderFont">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlsem1" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                        Width="160px" AutoPostBack="True" OnSelectedIndexChanged="ddlsem1_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                <ContentTemplate>
                                                    <asp:RadioButtonList ID="Rblreporttype" runat="server" RepeatDirection="Horizontal"
                                                        AutoPostBack="true" OnSelectedIndexChanged="Rblreporttype_Rblreporttype">
                                                        <%----%>
                                                    </asp:RadioButtonList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanelbtn3" runat="server">
                                                <ContentTemplate>
                                                    <asp:ImageButton ID="Button1" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="btngo1Click" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </center>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <br />
                <center>
                    <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="GrdCumulative" runat="server" ShowFooter="false" ShowHeader="false"
                                Font-Names="Book Antiqua" OnSelectedIndexChanged="GrdCumulative_OnSelectedIndexChanged"
                                Width="500px">
                                <%--AllowPaging="true" PageSize="50"  OnPageIndexChanging="GridView1_OnPageIndexChanged"--%>
                                <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </center>
                <center>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div id="Div1" runat="server" visible="false">
                                <asp:Label ID="Label2" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    ForeColor="Red" Text="" Style="display: none;"></asp:Label>
                                <asp:Label ID="Label4" runat="server" Visible="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Text="Report Name"></asp:Label>
                                <asp:TextBox ID="TextBox1" runat="server" Visible="true" Width="180px" onkeypress="display()"
                                    CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                                    Visible="false" OnClick="btnExcel_Click3" />
                                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                                    Visible="false" OnClick="btnprintmaster_Click3" />
                                <NEW:NEWPrintMater runat="server" ID="NEWPrintMater1" Visible="false" />
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="ImageButton1" />
                            <asp:PostBackTrigger ControlID="ImageButton2" />
                        </Triggers>
                    </asp:UpdatePanel>
                </center>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <center>
        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
            <ContentTemplate>
                <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;">
                    <center>
                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px; border-radius: 10px;">
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
                                            <asp:UpdatePanel ID="UpdatePanelbtn2" runat="server">
                                                <ContentTemplate>
                                                    <center>
                                                        <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                            OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                                    </center>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel14" runat="server">
            <ContentTemplate>
                <table id="colour" runat="server" visible="false" style="font-family: Book Antiqua; font-weight: bold;">
                    <tr>
                        <td>
                            <fieldset id="Fieldset8" runat="server" style="width: 33px; height: 7px; background-color: greenyellow; margin-left: 156px; font-weight: bold;">
                                <asp:Label ID="Label6" runat="server" Text="Paid"></asp:Label>
                            </fieldset>
                        </td>
                        <td>
                            <fieldset id="Fieldset6" runat="server" enabled="false" style="width: 33px; height: 7px; background-color: skyblue; margin-left: 27px; font-weight: bold;">
                                <asp:Label ID="Label3" runat="server" Text="UnPaid"></asp:Label>
                            </fieldset>
                        </td>
                        <td>
                            <fieldset id="Fieldset7" runat="server" enabled="false" style="width: 33px; height: 7px; background-color: orange; margin-left: 27px; font-weight: bold;">
                                <asp:Label ID="Label7" runat="server" Text="Cancel"></asp:Label>
                            </fieldset>
                        </td>
                        <td>
                            <b>
                                <asp:Label ID="lblpaid" runat="server" Enabled="false" Text="Paid Amount:"></asp:Label>
                                <asp:Label ID="lblpaid1" Text="" Enabled="false" runat="server"></asp:Label>
                            </b>
                        </td>
                        <td>
                            <b>
                                <asp:Label ID="lblunpaid" runat="server" Enabled="false" Text="UnPaid Amount:"></asp:Label>
                                <asp:Label ID="lblunpaid1" Text="" runat="server"></asp:Label>
                            </b>
                        </td>
                        <td>
                            <b>
                                <asp:Label ID="lblcancel" runat="server" Enabled="false" Text="Cancel Amount:"></asp:Label>
                                <asp:Label ID="Label1" Text="" runat="server"></asp:Label>
                            </b>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--progressBar for GO--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpGo">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for GO--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanelbtn3">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for UpPrint--%>
    <%--<center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpPrint">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                            Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
    </center>--%>
</asp:Content>
