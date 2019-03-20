<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CondonationFeesStatus.aspx.cs" Inherits="CondonationFeesStatus" %>
  

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
        </script>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Condonation Fees Status</span></div>
            </center>
        </div>
        <div>
            <center>
                <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblstr" runat="server" Text="Type"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlstream" runat="server" Enabled="false" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlstream_OnSelectedIndexChanged" CssClass="textbox  ddlheight"
                                    Style="width: 108px;">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <span>Batch</span>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UP_batch" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_batch" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                            height: 200px;">
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
                                        <asp:TextBox ID="txt_degree" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_degree" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                            height: 200px;">
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
                                        <asp:TextBox ID="txt_dept" runat="server" Style="height: 20px; width: 176px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                            height: 300px;">
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
                                        <asp:Panel ID="panel_sem" runat="server" CssClass="multxtpanel" Style="width: 124px;
                                            height: 172px;">
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
                                            height: 147px;">
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
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Header" Style="width: 50px;"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtheader" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnl_studhed" runat="server" CssClass="multxtpanel multxtpanleheight"
                                            Style="width: 164px; height: 194px;">
                                            <asp:CheckBox ID="cbheader" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cbheader_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cblheader" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblheader_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txtheader"
                                            PopupControlID="pnl_studhed" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Ledger"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtledger" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnl_studled" runat="server" CssClass="multxtpanel multxtpanleheight"
                                            Style="width: 166px; height: 194px;">
                                            <asp:CheckBox ID="cbledger" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cbledger_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cblledger" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblledger_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txtledger"
                                            PopupControlID="pnl_studled" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                PaymentMode
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upd_paid" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_paid" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnl_paid" runat="server" CssClass="multxtpanel multxtpanleheight"
                                            Style="width: 126px; height: 120px;">
                                            <asp:CheckBox ID="chk_paid" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="chk_paid_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="chkl_paid" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_paid_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_paid"
                                            PopupControlID="pnl_paid" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblfyear" Text="FinanceYear" Width="85px"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtfyear" Style="height: 20px; width: 123px;" CssClass="Dropdown_Txt_Box"
                                            runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                        <asp:Panel ID="Pfyear" runat="server" CssClass="multxtpanel" Width="178px">
                                            <asp:CheckBox ID="chkfyear" runat="server" Text="Select All" OnCheckedChanged="chkfyear_changed"
                                                AutoPostBack="True" />
                                            <asp:CheckBoxList ID="chklsfyear" runat="server" OnSelectedIndexChanged="chklsfyear_selected"
                                                AutoPostBack="True">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txtfyear"
                                            PopupControlID="Pfyear" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td colspan="3">
                                <div id="divdatewise" visible="true" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cbdatewise" runat="server" Checked="true" AutoPostBack="true" OnCheckedChanged="cbdatewise_OnCheckedChanged" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_fromdate" runat="server" Text="From"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_fromdate" runat="server" Style="height: 20px; width: 66px;"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_todate" runat="server" Text="To" Style="margin-left: 4px;"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_todate" runat="server" Style="height: 20px; width: 66px;"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                            <td colspan="5">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="checkdicon" runat="server" Text="Student Catagory" AutoPostBack="true"
                                                OnCheckedChanged="checkdicon_Changed" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Style="width: 200px;" />
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtinclude" Enabled="false" Font-Size="Medium" Font-Names="Book Antiqua"
                                                        Style="height: 20px; width: 164px;" CssClass="Dropdown_Txt_Box" runat="server"
                                                        ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="pnlinclude" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                        Width="172px">
                                                        <asp:CheckBox ID="cbinclude" runat="server" Text="Select All" Font-Size="Medium"
                                                            Font-Names="Book Antiqua" OnCheckedChanged="cbinclude_OnCheckedChanged" AutoPostBack="True" />
                                                        <asp:CheckBoxList ID="cblinclude" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            OnSelectedIndexChanged="cblinclude_OnSelectedIndexChanged" AutoPostBack="True">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txtinclude"
                                                        PopupControlID="pnlinclude" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cbCurSem" runat="server" Text="Current Semester" Checked="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <%-- <td>
                            <asp:Button ID="btn_search" runat="server" CssClass="textbox btn2" Text="Search"
                                OnClick="btnsearch_Click" />
                        </td>--%>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:CheckBox ID="cbbefore" runat="server" Text="Before Admission" />
                                <asp:LinkButton ID="lnkSetting" runat="server" Text="Setting" OnClick="lnkSetting_Click"></asp:LinkButton>
                            </td>
                            <td>
                                <asp:Button ID="btn_search" runat="server" CssClass="textbox btn2" Text="Search"
                                    OnClick="btnsearch_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <div id="divspread" runat="server" visible="false" style="width: 961px; overflow: auto;
                        background-color: White; border-radius: 10px;">
                        <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="true" BorderStyle="Solid"
                            BorderWidth="0px" Width="930px" Style="overflow: auto; border: 0px solid #999999;
                            border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                            class="spreadborder">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <br />
                    <br />
                    <center>
                        <div id="print" runat="server" visible="false">
                            <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                ForeColor="Red" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display(this)"
                                CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                InvalidChars="/\">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnExcel" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                OnClick="btnExcel_Click" Text="Export To Excel" Width="127px" Height="32px" CssClass="textbox textbox1" />
                            <asp:Button ID="btnprintmasterhed" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Text="Print" OnClick="btnprintmaster_Click" Height="32px" Style="margin-top: 10px;"
                                CssClass="textbox textbox1" Width="60px" />
                            <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                        </div>
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
                                        <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
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
        <center>
            <div id="divSetting" runat="server" visible="false" class="popupstyle popupheight1 ">
                <asp:ImageButton ID="imgSetting" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                    width: 30px; position: absolute; margin-top: 9px; margin-left: 112px;" OnClick="imgSetting_Click" />
                <br />
                <center>
                    <div style="background-color: White; height: 363px; width: 257px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px; overflow: auto;">
                        <br />
                        <span style="color: Green; font-size: large; font-weight: bold;">Feecategory Settings</span>
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnAddRow" runat="server" Text="Add New" OnClick="btnAddRow_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="gdSetting" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                        OnDataBound="gdSetting_OnDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sno" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lblsno" runat="server" Text='<%#Eval("Sno") %>'></asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Batch" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:DropDownList ID="ddlBatch" runat="server" CssClass="textbox1 ddlheight1">
                                                        </asp:DropDownList>
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Feecategory" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:DropDownList ID="ddlFeecat" runat="server" CssClass="textbox1 ddlheight1">
                                                        </asp:DropDownList>
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnRowOK" runat="server" Text="OK" OnClick="btnRowOK_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </div>
        </center>
    </body>
    </html>
</asp:Content>
