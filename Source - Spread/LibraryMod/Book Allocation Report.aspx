<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="Book Allocation Report.aspx.cs" Inherits="LibraryMod_Book_Allocation_Report" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <script type="text/javascript">


        function SelLedgers() {
            var chkSelAll = document.getElementById("<%=chkGridSelectAll.ClientID %>");
            var tbl = document.getElementById("<%=gridview1.ClientID %>");
            var gridViewControls = tbl.getElementsByTagName("input");

            for (var i = 1; i < (tbl.rows.length - 1); i++) {
                var chkSelectid = document.getElementById('MainContent_gridview1_selectchk_' + i.toString());

                if (chkSelAll.checked == false) {
                    chkSelectid.checked = false;
                } else {
                    chkSelectid.checked = true;
                }
            }

        }
    </script>
    <center>
        <span class="fontstyleheader" style="color: green; margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative;">Book Allocation Report</span></center>
    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
        <ContentTemplate>
            <br />
            <center>
                <table class="maintablestyle" style="margin: 0px; margin-bottom: 0px; margin-top: 8px;
                    position: relative; font-family: Book Antiqua; font-weight: bold" width="850px">
                    <tr>
                        <td colspan="11">
                            <fieldset style="width: 919px; height: 35px;">
                                <asp:RadioButton ID="rdbrack" runat="server" Text="Allocated Books" GroupName="Move"
                                    AutoPostBack="true" OnCheckedChanged="rdbrack_CheckedChange" />
                                <asp:RadioButton ID="rdbrackto" runat="server" Text="Not Allocated Books To Rack"
                                    GroupName="Move" AutoPostBack="true" Checked="true" OnCheckedChanged="rdbrack_CheckedChange" />
                                <asp:RadioButton ID="rdbLibrary" runat="server" Text="Transfer To Library" GroupName="Move"
                                    AutoPostBack="true" OnCheckedChanged="rdbrack_CheckedChange" />
                                <asp:RadioButton ID="rdbtrans" runat="server" Text="Transfer To Department" GroupName="Move"
                                    AutoPostBack="true" OnCheckedChanged="rdbrack_CheckedChange" />
                                <asp:RadioButton ID="rdbissue" runat="server" Text=" Issue To Department" GroupName="Move"
                                    AutoPostBack="true" OnCheckedChanged="rdbrack_CheckedChange" />
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Lblcollege" runat="server" Text="College" CssClass="commonHeaderFont"
                                Visible="true"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox1  ddlheight1"
                                        Visible="true" Width="140px" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="Transfrom" runat="server" Text="Transfer From" CssClass="commonHeaderFont"
                                Visible="true"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddltransfrom" runat="server" CssClass="textbox1  ddlheight1"
                                        Visible="true" Width="140px" AutoPostBack="True">
                                        <%--OnSelectedIndexChanged="ddltransfrom_SelectedIndexChanged"--%>
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                <ContentTemplate>
                                    <asp:CheckBox ID="Cboldsearch" runat="server" Text="" AutoPostBack="True" OnCheckedChanged="Cboldsearch_CheckedChange" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="From" CssClass="commonHeaderFont" Visible="true"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_from" runat="server" CssClass="textbox  txtheight" AutoPostBack="true"
                                        Visible="true" Enabled="false"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_from" runat="server"
                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="To" CssClass="commonHeaderFont" Visible="true"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="Txtto" runat="server" CssClass="textbox  txtheight" AutoPostBack="true"
                                        Visible="true" Enabled="false"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="Txtto" runat="server"
                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <tr>
                            <%--<td>
                                <asp:Label ID="Lbltransto" runat="server" Text="Transfer To" CssClass="commonHeaderFont"
                                    Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddltransto" runat="server" CssClass="textbox1  ddlheight1"
                                            Visible="false" Width="140px" AutoPostBack="True">
                                            <%--OnSelectedIndexChanged="ddltransfrom_SelectedIndexChanged"
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
                            <td>
                                <asp:Label ID="lblbooktype" runat="server" Text="Book Type" CssClass="commonHeaderFont"
                                    Visible="true"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddltype" runat="server" CssClass="textbox1  ddlheight1" Visible="true"
                                            Width="140px" AutoPostBack="True">
                                            <%--OnSelectedIndexChanged="ddltransfrom_SelectedIndexChanged"--%>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <center>
                                    <asp:UpdatePanel ID="UpGo" runat="server">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="Button1" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="Go_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </center>
                            </td>
                        </tr>
                    </tr>
                </table>
                <br />
                <br />
                <center>
                    <span style="padding-right: 100px; margin-left: 442px; margin-top: 3px;">
                        <asp:CheckBox ID="chkGridSelectAll" runat="server" Text="SelectAll" Visible="false"
                            onchange="return SelLedgers();" />
                    </span>
                </center>
                <asp:GridView ID="gridview1" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                    Font-Names="book antiqua" togeneratecolumns="true" AllowPaging="true" PageSize="500"
                    ShowHeader="true" OnSelectedIndexChanged="gridview1_onselectedindexchanged"
                    OnPageIndexChanging="gridview1_onpageindexchanged" Width="1182px" OnRowDataBound="gridview1_RowDataBound">
                    <Columns>
                      
                       
                    </Columns>
                    <HeaderStyle BackColor="#0ca6ca" ForeColor="white" />
                </asp:GridView>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    <center>
        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
            <ContentTemplate>
                <div id="rptprint" runat="server" visible="true">
                    <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                        Visible="false"></asp:Label>
                    <asp:Label ID="lblrptname" runat="server" Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" onkeypress="display()"
                        Font-Size="Medium" CssClass="textbox textbox1"></asp:TextBox>
                    <asp:ImageButton ID="btnExcel" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                        OnClick="btnExcel_Click" />
                    <asp:ImageButton ID="btnprintmaster" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                        OnClick="btnprintmaster_Click" />
                    <NEW:NEWPrintMater runat="server" ID="Printcontrol1" Visible="false" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExcel" />
                <asp:PostBackTrigger ControlID="btnprintmaster" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
            <ContentTemplate>
                <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 100px;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 50px;
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
                                            <asp:UpdatePanel ID="UpdatePanelbtn2" runat="server">
                                                <ContentTemplate>
                                                    <center>
                                                        <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                            OnClick="btnerrclose_Click" Text="ok" runat="server" />
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
    <%--progressBar for Go--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpGo">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
