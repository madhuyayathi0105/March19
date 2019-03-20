<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="HourWise_StaffAttnd.aspx.cs" Inherits="HourWise_StaffAttnd" EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
<%--    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>--%>
    <style>
        body
        {
            font-family: Book Antiqua;
        }
    </style>
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblsmserror.ClientID %>').innerHTML = "";
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <span class="fontstyleheader" style="color: Green">HourWise - Staff Attendance </span>
            <br />
            <div class="maindivstyle" style="height: auto; width: 950px;">
                <br />
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lblcoll" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                CssClass="textbox1 ddlheight5" OnSelectedIndexChanged="ddlcollege_change" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbldept" runat="server" Text="Department" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updDept" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtDept" runat="server" Text="--Select--" CssClass="textbox textbox1 txtheight3"></asp:TextBox>
                                    <asp:Panel ID="pnlDept" runat="server" CssClass="multxtpanel" Height="200px">
                                        <asp:CheckBox ID="cbDept" runat="server" Text="Select All" OnCheckedChanged="cbDept_Change"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="cblDept" runat="server" OnSelectedIndexChanged="cblDept_Change"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popDept" runat="server" TargetControlID="txtDept" PopupControlID="pnlDept"
                                        Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lblDesig" runat="server" Text="Designation" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updDesig" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtDesig" runat="server" Text="--Select--" CssClass="textbox textbox1 txtheight3"></asp:TextBox>
                                    <asp:Panel ID="pnlDesig" runat="server" CssClass="multxtpanel" Height="200px">
                                        <asp:CheckBox ID="cbDesig" runat="server" Text="Select All" OnCheckedChanged="cbDesig_Change"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="cblDesig" runat="server" OnSelectedIndexChanged="cblDesig_Change"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popDesig" runat="server" TargetControlID="txtDesig"
                                        PopupControlID="pnlDesig" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblFrom" runat="server" Text="From : " Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                            &nbsp;&nbsp;<asp:TextBox ID="txtFrmDt" runat="server" OnTextChanged="txtFrmDt_Change"
                                AutoPostBack="true" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                            <asp:CalendarExtender ID="calFrmDt" runat="server" TargetControlID="txtFrmDt" Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <asp:Label ID="lblTo" runat="server" Text="To : " Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                            <asp:TextBox ID="txtToDt" runat="server" OnTextChanged="txtToDt_Change" AutoPostBack="true"
                                CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                            <asp:CalendarExtender ID="calToDt" runat="server" TargetControlID="txtToDt" Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            &nbsp;<asp:Label ID="lblStfCat" runat="server" Text="Staff Category" Font-Bold="true"
                                Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updStfCat" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtStfCat" runat="server" Text="--Select--" CssClass="textbox textbox1 txtheight3"></asp:TextBox>
                                    <asp:Panel ID="pnlStfCat" runat="server" CssClass="multxtpanel" Height="200px">
                                        <asp:CheckBox ID="cbStfCat" runat="server" Text="Select All" OnCheckedChanged="cbStfCat_Change"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="cblStfCat" runat="server" OnSelectedIndexChanged="cblStfCat_Change"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtStfCat"
                                        PopupControlID="pnlStfCat" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lblStaffType" runat="server" Text="Staff Type" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updStfType" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtStfType" runat="server" Text="--Select--" CssClass="textbox textbox1 txtheight3"></asp:TextBox>
                                    <asp:Panel ID="pnlStfType" runat="server" CssClass="multxtpanel" Height="200px">
                                        <asp:CheckBox ID="cbStfType" runat="server" Text="Select All" OnCheckedChanged="cbStfType_Change"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="cblStfType" runat="server" OnSelectedIndexChanged="cblStfType_Change"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtStfType"
                                        PopupControlID="pnlStfType" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Button ID="btnGo" runat="server" Text="GO" CssClass="textbox1 btn1" OnClick="btnGo_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="lblMainErr" runat="server" Visible="false" Text="" Font-Bold="true"
                    Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Red"></asp:Label>
                <br />
                <br />
                <asp:Label ID="lblNote" runat="server" Visible="false" Style="margin-left: 72px;
                    position: relative;" Text="(Working Hours must be less than or Equal to Total Hours)"
                    Font-Bold="true" Font-Italic="true" Font-Size="Medium" Font-Names="Book Antiqua"
                    ForeColor="Green"></asp:Label>
                <asp:Button ID="btnSave" runat="server" Visible="false" Style="top: 250px; left: 849px;
                    position: absolute;" Text="Save" CssClass="textbox1 btn2" BackColor="LightGreen"
                    OnClick="btnSave_Click" />
                <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" BorderColor="Black"
                    BorderStyle="Solid" BorderWidth="1px" Width="850px" Style="height: 380px; overflow: auto;
                    background-color: White;" CssClass="spreadborder" OnButtonCommand="Fpspread1_command"
                    ShowHeaderSelection="false">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <br />
                <div id="rprint" visible="false" runat="server">
                    <asp:Label ID="lblsmserror" Text="Please Enter Your Report Name" Font-Size="Large"
                        Font-Names="Book Antiqua" Visible="false" ForeColor="Red" runat="server" Font-Bold="true"></asp:Label>
                    <asp:Label ID="lblexcel" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                    <asp:TextBox ID="txtexcel" onkeypress="display()" CssClass="textbox textbox1" runat="server"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcel"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnexcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        CssClass="textbox textbox1 btn2" Width="150px" Text="Export Excel" OnClick="btnexcel_Click" />
                    <asp:Button ID="btnprintmaster" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Text="Print" OnClick="btnprintmaster_Click" CssClass="textbox textbox1 btn2"
                        Width="100px" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </div>
                <br />
                <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                            <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_errorclose" CssClass="textbox textbox1 btn1" OnClick="btn_errorclose_Click"
                                                    Text="OK" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </div>
        </center>
    </body>
    </html>
</asp:Content>
