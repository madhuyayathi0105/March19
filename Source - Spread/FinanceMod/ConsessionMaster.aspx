<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ConsessionMaster.aspx.cs" Inherits="ConsessionMaster" %>

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
        <script type="text/javascript">
            function Test() {
                var id = "";
                var value1 = "";
                var idval = "";
                var empty = "";


                id = document.getElementById("<%=ddl_reason.ClientID %>");
                value1 = id.options[id.selectedIndex].text;
                if (value1.trim() == "Select") {
                    empty = "E";
                    id = document.getElementById("<%=ddl_reason.ClientID %>");
                    id.style.borderColor = 'Red';
                }




                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                }
            }

            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }

            function display1() {
                document.getElementById('<%=lblerror.ClientID %>').innerHTML = "";
            }

     
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green">Online Concession and Refund Master</span></div>
            </center>
        </div>
        <center>
            <div class="maindivstyle" style="width: 1000px; height: auto;">
                <br />
                <center>
                    <div>
                        <asp:RadioButton ID="rb_concession" runat="server" Text="Concession" Width="150px"
                            GroupName="same" AutoPostBack="true" OnCheckedChanged="rb_concession_CheckedChanged" />
                        <asp:RadioButton ID="rb_refund" runat="server" Text="Refund" Width="150px" GroupName="same"
                            AutoPostBack="true" OnCheckedChanged="rb_refund_CheckedChanged" />
                    </div>
                </center>
                <br />
                <center>
                    <div>
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_collegename" Text="College" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_str" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upp_stream" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_stream" runat="server" Height="17px" CssClass="textbox textbox1 txtheight2"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="panel_stream" runat="server" Width="129px" Height="100px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_stream" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_stream_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_stream" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_stream_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popupext_stream" runat="server" TargetControlID="txt_stream"
                                                PopupControlID="panel_stream" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    Education Level
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="up_educ" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_edulevel" runat="server" Height="17px" CssClass="textbox textbox1 txtheight2"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="panel_edulevel" runat="server" Height="100px" Width="125px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_edulevel" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_edulevel_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_edulevel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_edulevel_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popupext_edulevel" runat="server" TargetControlID="txt_edulevel"
                                                PopupControlID="panel_edulevel" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    DeptWise
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanelchk" runat="server">
                                        <ContentTemplate>
                                            <asp:CheckBox ID="chkdept" runat="server" AutoPostBack="true" OnCheckedChanged="chkdept_OnCheckedChanged" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbldeg" runat="server" Text="Course"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_course" runat="server" Height="17px" CssClass="textbox  txtheight2"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                height: 150px;">
                                                <asp:CheckBox ID="cb_course" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_course_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_course" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_course_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_course"
                                                PopupControlID="Panel3" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_dept" runat="server" Height="17px" CssClass="textbox  txtheight2"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel" Style="width: 200px;
                                                height: 150px;">
                                                <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_dept_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_dept"
                                                PopupControlID="Panel4" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblsem" runat="server" Text="Semester"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox  txtheight2" Height="17px"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel_sem" runat="server" CssClass="multxtpanel" Height="171px" Width="126px">
                                                <asp:CheckBox ID="cb_sem" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_sem_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_sem"
                                                PopupControlID="Panel_sem" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    Header
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Updp_header" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_header" runat="server" Height="17px" CssClass="textbox  txtheight2"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel_header" runat="server" CssClass="multxtpanel" Height="200px"
                                                Width="138px">
                                                <asp:CheckBox ID="cb_header" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_header_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_header" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_header_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_header"
                                                PopupControlID="Panel_header" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    Ledger
                                </td>
                                <td colspan="3">
                                    <asp:UpdatePanel ID="Updp_ledger" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_ledger" runat="server" Height="17px" Width="193px" CssClass="textbox  txtheight2"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel_ledger" runat="server" CssClass="multxtpanel" Height="200px"
                                                Width="200px">
                                                <asp:CheckBox ID="cb_ledger" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_ledger_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_ledger" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_ledger_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_ledger"
                                                PopupControlID="Panel_ledger" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lbl_reas" runat="server" Visible="false" Text="Reason"></asp:Label>
                                    <asp:Button ID="btn_plus" runat="server" Visible="false" Text="+" CssClass="textbox btn"
                                        Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus_Click" />
                                    <asp:DropDownList ID="ddl_reason" runat="server" Visible="false" CssClass="textbox  ddlheight3"
                                        onfocus="return myFunction(this)">
                                    </asp:DropDownList>
                                    <asp:Button ID="btn_minus" runat="server" Visible="false" Text="-" Font-Bold="true"
                                        Font-Size="Medium" Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btnminus_Click" />
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblfyear" Text="FinanceYear" Width="85px"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlfinyear" runat="server" CssClass="textbox textbox1 ddlheight2"
                                        Style="width: 130px;">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divperamt" runat="server">
                        <asp:RadioButton ID="rb_amt" runat="server" Text="Amount" AutoPostBack="true" OnCheckedChanged="rb_amt_OnCheckedChanged"
                            GroupName="g1" />
                        <asp:RadioButton ID="rb_per" runat="server" Text="Percentage" AutoPostBack="true"
                            OnCheckedChanged="rb_per_OnCheckedChanged" GroupName="g1" />
                    </div>
                    <br />
                    <center>
                        <div id="Divspread" runat="server" visible="false" style="width: 850px;">
                            <br />
                            <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" OnButtonCommand="FpSpread1_ButtonCommand"
                                BorderWidth="0px" Style="overflow: auto;" ActiveSheetViewIndex="0" OnPreRender="FpSpread1_SelectedIndexChanged">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                    </center>
                    <br />
                    <center>
                        <asp:Label ID="Label1" runat="server" Visible="false" Text="Reason"></asp:Label>
                        <asp:DropDownList ID="ddl_enroll" runat="server" Visible="false" CssClass="textbox  ddlheight3">
                        </asp:DropDownList>
                        <asp:Button ID="btn_save" Text="Save" runat="server" Visible="false" CssClass="btn2 textbox textbox1"
                            OnClick="btn_save_click" />
                        <asp:Button ID="btn_reset" runat="server" Visible="false" CssClass="btn2 textbox textbox1"
                            Text="Reset" OnClick="btn_reset_Onclick" />
                    </center>
                    <br />
                    <%--****end of Concession***--%>
                </center>
                <center>
                    <div id="rptprint" runat="server" visible="false">
                        <asp:Label ID="lbl_validation" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                        <asp:Label ID="lbl_rptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txt_excelname" runat="server" CssClass="textbox textbox1 txtheight4"
                            Font-Bold="True" onkeypress="display()"></asp:TextBox>
                        <asp:Button ID="btn_excel" runat="server" Font-Bold="true" OnClick="btn_excel_Click"
                            CssClass="textbox" Text="Export To Excel" Width="127px" Height="30px" />
                        <asp:Button ID="btn_printmaster" runat="server" Text="Print" OnClick="btn_printmaster_Click"
                            Font-Bold="true" Width="60px" Height="30px" CssClass="textbox" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                    <%-- ****plusdiv***--%>
                    <center>
                        <div id="plusdiv" runat="server" visible="false" class="popupstyle popupheight1">
                            <center>
                                <div id="panel_addgroup" runat="server" visible="false" class="table" style="background-color: White;
                                    height: 140px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                    margin-top: 200px; border-radius: 10px;">
                                    <table style="line-height: 30px">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lbl_addgroup" runat="server" Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:TextBox ID="txt_addgroup" runat="server" Width="200px" CssClass="textbox textbox1"
                                                    onkeypress="display1()"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style="line-height: 35px">
                                                <asp:Button ID="btn_addgroup1" runat="server" Text="Add" Font-Names="Book Antiqua"
                                                    CssClass="textbox textbox1" Height="32px" Width="60px" OnClick="btn_addgroup_Click" />
                                                <asp:Button ID="btn_exitgroup1" runat="server" Text="Exit" Font-Names="Book Antiqua"
                                                    CssClass="textbox textbox1" Height="32px" Width="60px" OnClick="btn_exitaddgroup_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblerror" runat="server" Visible="false" ForeColor="red" Font-Size="Smaller"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </center>
                        </div>
                    </center>
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
                                                    <asp:Button ID="btnerrclose" CssClass="textbox textbox1" Style="height: 28px; width: 65px;"
                                                        OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                    <div id="alertdel" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="alertdelete" runat="server" class="table" style="background-color: White;
                                height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                margin-top: 200px; border-radius: 10px;">
                                <center>
                                    <br />
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lbl_del" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btn_del" Visible="false" CssClass="textbox textbox1" Style="height: 28px;
                                                        width: 65px;" OnClick="btn_del_Click" Text="Ok" runat="server" />
                                                    <asp:Button ID="btn_ok" Visible="false" CssClass="textbox textbox1" Style="height: 28px;
                                                        width: 65px;" OnClick="btn_ok_Click" Text="Ok" runat="server" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                    <%--***end of plusdiv***--%>
            </div>
        </center>
    </body>
    </html>
</asp:Content>
