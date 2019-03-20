<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudentLogDetailReport.aspx.cs" Inherits="StudentLogDetailReport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <script type="text/javascript">
            function hideDiv() {
                var divid = document.getElementById('<%=imgdiv2.ClientID %>');
                divid.style.display = "none";
                return false;
            }
            $(document).ready(function () {
                $('#<%=btnpurposeexit.ClientID %>').click(function () {
                    $('#<%=divtempsecond.ClientID %>').hide();
                    return false;

                });
                $('#<%=btnplus.ClientID %>').click(function () {
                    $('#<%=txtpurposecaption.ClientID %>').val('');
                    $('#<%=divtempsecond.ClientID %>').show();
                    return false;
                });
                $('#<%=btnaddtemplate.ClientID %>').click(function () {
                    $('#<%=txtpurposemsg.ClientID %>').val('');
                    $('#<%=divempedit.ClientID %>').show();
                    return false;
                });
                $('#<%=btnexit.ClientID %>').click(function () {
                    $('#<%=divempedit.ClientID %>').hide();
                    return false;
                });

            });

            
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Student Log Details</span></div>
            </center>
        </div>
        <div>
            <center>
                <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lblclg" Text="College" Font-Names="Book Antiqua" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollege" runat="server" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                                    AutoPostBack="true" Style="height: 24px; width: 144px;">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbltype" Text="Type" runat="server" Font-Names="Book Antiqua" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddltype" runat="server" Style="height: 24px; width: 90px;"
                                    Font-Names="Book Antiqua" AutoPostBack="true" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblbatch" runat="server" Text="Batch"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UP_batch" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_batch" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                            height: auto;">
                                            <asp:CheckBox ID="cb_batch" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_batch_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pce_batch" runat="server" TargetControlID="txt_batch"
                                            PopupControlID="panel_batch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UP_degree" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_degree" runat="server" Style="height: 20px; width: 125px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_degree" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                            height: auto;">
                                            <asp:CheckBox ID="cb_degree" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_degree_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pce_degree" runat="server" TargetControlID="txt_degree"
                                            PopupControlID="panel_degree" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Up_dept" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_dept" runat="server" Style="height: 20px; width: 140px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                            height: auto;">
                                            <asp:CheckBox ID="cb_dept" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_dept_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pce_dept" runat="server" TargetControlID="txt_dept"
                                            PopupControlID="panel_dept" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblsem" runat="server" Text="Semester"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Updp_sem" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_sem" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_sem" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                            height: auto;">
                                            <asp:CheckBox ID="cb_sem" runat="server" Width="124px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_sem_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_sem"
                                            PopupControlID="panel_sem" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                Section
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Updp_sect" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_sect" runat="server" Style="height: 20px; width: 80px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_sect" runat="server" CssClass="multxtpanel" Style="width: 100px;
                                            height: auto;">
                                            <asp:CheckBox ID="cb_sect" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_sect_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_sect" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sect_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_sect"
                                            PopupControlID="panel_sect" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td colspan="3">
                                <div id="divdatewise" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_fromdate" runat="server" Text="From"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_fromdate" runat="server" Style="height: 20px; width: 75px;"
                                                    onchange="return checkDate()"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_todate" runat="server" Text="To" Style="margin-left: 4px;"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_todate" runat="server" Style="height: 20px; width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                            <td>
                                <asp:Button ID="btngo" runat="server" Text="Go" CssClass="textbox textbox1 btn1"
                                    OnClick="btngo_OnClick" OnClientClick=" return validation()" />
                            </td>
                            <%--<td>
                                <asp:CheckBox ID="cbwithDet" runat="server" Text="With Details" />
                            </td>--%>
                        </tr>
                    </table>
                </div>
                <br />
                <FarPoint:FpSpread ID="spreadDet" runat="server" Visible="false" BorderStyle="Solid"
                    BorderWidth="0px" Width="930px" Style="overflow: auto; border: 0px solid #999999;
                    border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                    class="spreadborder" OnCellClick="spreadDet_Click" OnPreRender="spreadDet_Render">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <center>
                    <div id="print" runat="server" visible="false">
                        <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            ForeColor="Red" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname" runat="server" Visible="false" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" Visible="false" Width="180px" onkeypress="display()"
                            CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                            InvalidChars="/\">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnExcel" runat="server" Visible="false" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnExcel_Click" Text="Export To Excel" Width="127px"
                            Height="32px" CssClass="textbox textbox1" />
                        <asp:Button ID="btnprintmasterhed" runat="server" Visible="false" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" Height="32px"
                            Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                        <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                    </div>
                </center>
            </center>
        </div>
        <%--student list div--%>
        <center>
            <div id="divStud" runat="server" class="popupstyle" visible="false" style="height: 48em;
                z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                top: 15px; left: 0;">
                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: -12px; margin-left: 434px;"
                    OnClick="imagebtnpopsscode_Click" />
                <center>
                    <div style="background-color: White; height: 720px; width: 900px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <center>
                            <div>
                                <span id="spname" runat="server" class="fontstyleheader" style="color: Green;">Student
                                    Details</span></div>
                        </center>
                        <table>
                            <tr>
                                <td>
                                    <FarPoint:FpSpread ID="spreadStud" runat="server" Visible="true" BorderStyle="Solid"
                                        BorderWidth="0px" Width="890px" Height="300px" Style="overflow: auto; border: 0px solid #999999;
                                        border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                        class="spreadborder" OnButtonCommand="spreadStud_Command">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </td>
                            </tr>
                            <%--sms send template add--%>
                            <tr>
                                <td colspan="2">
                                    <div id="divpur" runat="server" visible="false">
                                        <asp:Label ID="lblpurpose1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Purpose"></asp:Label>
                                        <asp:DropDownList ID="ddlpurpose" runat="server" AutoPostBack="True" Font-Bold="True"
                                            CssClass="textbox ddlstyle ddlheight3" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Width="300px" OnSelectedIndexChanged="ddlpurpose_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <FarPoint:FpSpread ID="spreadSms" runat="server" Visible="false" BorderColor="Black"
                                        BorderStyle="Solid" BorderWidth="1px" Height="150px" Width="850px" OnCellClick="FpSpread2_CellClick"
                                        OnPreRender="FpSpread2_SelectedIndexChanged">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" SelectionBackColor="#CE5D5A"
                                                SelectionForeColor="White">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div id="divbtn" runat="server" visible="false">
                                        <asp:Button ID="btnaddtemplate" runat="server" Text="Add Template" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnaddtemplate_Click" />
                                        <asp:Button ID="btndeletetemplate" runat="server" Text="Delete Template" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" OnClientClick="return confirm('Are you sure you want delete this template?');"
                                            OnClick="btndeletetemplate_Click" />
                                        <asp:Button ID="btnclear" runat="server" Width="90px" Text="Clear" Font-Size="Medium"
                                            Font-Bold="true" Font-Names="Book Antiqua" CssClass="textbox textbox1 btn1" OnClick="btnclear_Click" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtmessage" runat="server" Visible="false" TextMode="MultiLine"
                                        Height="100px" Width="500px" Style="font-family: 'Book Antiqua'" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnsendsms" runat="server" Visible="false" Width="90px" Text="Send"
                                        Font-Size="Medium" Font-Bold="true" Font-Names="Book Antiqua" CssClass="textbox textbox1 btn1"
                                        OnClick="btnsendsms_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </div>
        </center>
        <%--sms sub popup div--%>
        <center>
            <div id="divempedit" runat="server" class="popupstyle" style="height: 48em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px; display: none;">
                <center>
                    <div id="Div4" runat="server" class="table" style="background-color: White; height: 410px;
                        width: 710px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 216px;
                        border-radius: 10px;">
                        <center>
                            <asp:Panel ID="templatepanel" runat="server" BorderColor="Black" BackColor="AliceBlue"
                                BorderWidth="2px" Height="400px" Width="690px">
                                <div class="PopupHeaderrstud2" id="Div3" style="text-align: center; font-family: MS Sans Serif;
                                    font-size: Small; font-weight: bold">
                                    <table>
                                        <tr>
                                            <td colspan="4">
                                                <br />
                                                <span>Message Template</span>
                                                <br />
                                                <br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblpurpose" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" ForeColor="Black" Text="Purpose" Width="100px"></asp:Label>
                                                <asp:Button ID="btnplus" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" OnClick="btnplus_Click" Text=" + " />
                                                <asp:DropDownList ID="ddlpurposemsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Height="25px" Width="200px">
                                                </asp:DropDownList>
                                                <asp:Button ID="btnminus" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" OnClientClick="return confirm('Are you sure you want to delete this type?');"
                                                    OnClick="btnminus_Click" Text=" - " />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblerror" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" ForeColor="Red" Style="height: 21px" Width="676px"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtpurposemsg" runat="server" TextMode="MultiLine" Height="200px"
                                                    Width="680px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" OnTextChanged="txtpurposemsg_TextChanged"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td colspan="2">
                                                <center>
                                                    <asp:Button ID="btnsave" runat="server" Text="Save" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnClick="btnsave_Click" Height=" 26px" Width=" 88px" />
                                                    <asp:Button ID="btnexit" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Height=" 26px" Width=" 88px" OnClick="btnexit_Click" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="divtempsecond" runat="server" class="popupstyle" style="height: 100em; z-index: 100000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px; display: none;">
                <center>
                    <div id="Div5" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 320px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 216px;
                        border-radius: 10px;">
                        <center>
                            <asp:Panel ID="purposepanel" runat="server" BorderColor="Black" BackColor="AliceBlue"
                                BorderWidth="2px" Height="100px" Width="300px">
                                <div class="panelinfraction" id="Div2" style="text-align: center; font-family: MS Sans Serif;
                                    font-size: Small; font-weight: bold">
                                    <table>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="Label1" runat="server" Text="Purpose Type" Style="text-align: center;"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblpurposecaption" runat="server" Text="Purpose" Style="font-size: medium;
                                                    font-weight: bold; height: 22px; font-family: 'Book Antiqua'; left: 10px;"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtpurposecaption" runat="server" Style="font-size: medium; font-weight: bold;
                                                    height: 22px; font-family: 'Book Antiqua';"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnpurposeadd" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                                    height: 26px;" OnClick="btnpurposeadd_Click" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnpurposeexit" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                                    height: 26px; width: 88px;" OnClick="btnpurposeexit_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <%--alert div--%>
        <center>
            <div id="imgdiv2" runat="server" style="height: 100%; z-index: 1000; width: 100%;
                background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;
                display: none;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
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
                                            <asp:Button ID="btn_errorclose" OnClientClick="return hideDiv();" CssClass=" textbox btn1 comm"
                                                Style="height: 28px; width: 65px;" Text="ok" runat="server" />
                                            <%--OnClick="btn_errorclose_Click"--%>
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
</asp:Content>
