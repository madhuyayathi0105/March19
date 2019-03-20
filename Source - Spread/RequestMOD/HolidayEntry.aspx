<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master"
    CodeFile="HolidayEntry.aspx.cs" Inherits="HolidayEntry" EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }       
        </script>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Declare The Holiday</span></div>
        </center>
        </div>
        <div>
            <center>
                <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                    <center>
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
                                    <asp:CheckBox ID="cbdate" runat="server" Text="Date" AutoPostBack="true" OnCheckedChanged="cbdate_Changed" />
                                </td>
                                <td>
                                    <div id="divdatewise" runat="server">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_fromdate" runat="server" Text="From" Style="margin-top: 10px;
                                                        margin-left: 3px;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_fromdate" runat="server" Enabled="false" Style="height: 20px;
                                                        width: 75px; margin-left: 11px;"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_todate" runat="server" Text="To" Style="margin-left: 4px;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_todate" runat="server" Enabled="false" Style="height: 20px;
                                                        width: 75px; margin-left: 6px;"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td>
                                    <asp:CheckBox ID="cbtype" runat="server" Text="Type" AutoPostBack="true" OnCheckedChanged="cbtype_Changed" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rbhalf" runat="server" Enabled="false" Text="HalfDay" AutoPostBack="true"
                                        GroupName="day" OnCheckedChanged="rbhalf_Changed" />
                                    <asp:RadioButton ID="rbfull" runat="server" Enabled="false" Text="FullDay" AutoPostBack="true"
                                        GroupName="day" OnCheckedChanged="rbfull_Changed" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rbmnghalf" runat="server" Text="Morning" GroupName="c2" Enabled="false" />
                                    <asp:RadioButton ID="rbevehalf" runat="server" Text="Evening" GroupName="c2" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <asp:CheckBox ID="cbdegree" runat="server" Visible="false" Text="Degree" AutoPostBack="true"
                                            OnCheckedChanged="cbdegree_Changed" />
                                        <asp:CheckBox ID="cbcatg" runat="server" Visible="false" Text="Catagory" AutoPostBack="true"
                                            OnCheckedChanged="cbcatg_Changed" />
                                        <asp:CheckBox ID="cblbr" runat="server" Visible="false" Text="Library" AutoPostBack="true"
                                            OnCheckedChanged="cblbr_Changed" />
                                    </div>
                                </td>
                                <td colspan="5">
                                    <div id="divstud" runat="server" visible="false">
                                        <table>
                                            <tr>
                                                <td>
                                                    Batch
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UP_batch" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_batch" runat="server" Enabled="false" Style="height: 20px; width: 100px;"
                                                                ReadOnly="true">--Select--</asp:TextBox>
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
                                                    Degree
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UP_degree" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_degree" runat="server" Enabled="false" Style="height: 20px;
                                                                width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
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
                                                    Department
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="Up_dept" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_dept" runat="server" Enabled="false" Style="height: 20px; width: 100px;"
                                                                ReadOnly="true">--Select--</asp:TextBox>
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
                                                <td>
                                                    Semester
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="Updp_sem" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_sem" runat="server" Enabled="false" Style="height: 20px; width: 124px;"
                                                                ReadOnly="true">--Select--</asp:TextBox>
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
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="divstf" runat="server" visible="false">
                                        <table>
                                            <tr>
                                                <td>
                                                    Staff Type
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtstfcat" runat="server" Enabled="false" Style="height: 20px; width: 240px;"
                                                                ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="Panel5" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 243px;
                                                                height: 120px;">
                                                                <asp:CheckBox ID="cbstfcat" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                    OnCheckedChanged="cbstfcat_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="cblstfcat" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblstfcat_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txtstfcat"
                                                                PopupControlID="Panel5" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="divlbr" runat="server" visible="false">
                                        <table>
                                            <tr>
                                                <td>
                                                    Library
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtlbr" runat="server" Enabled="false" Style="height: 20px; width: 243px;"
                                                                ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="Panel6" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 126px;
                                                                height: 249px;">
                                                                <asp:CheckBox ID="cblbrary" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                    OnCheckedChanged="cblbrary_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="cbllbrary" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbllbrary_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txtlbr"
                                                                PopupControlID="Panel6" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="divhos" runat="server" visible="false">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbhostel" runat="server" Text="Hostel Name"></asp:Label>
                                                </td>
                                                <td id="hostel1" runat="server">
                                                    <asp:UpdatePanel ID="UP_hostel" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_hostel" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="panel_hostel" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                                                height: auto;">
                                                                <asp:CheckBox ID="cb_hostel" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                    OnCheckedChanged="cb_hostel_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="cbl_hostel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_hostel_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="pce_hostel" runat="server" TargetControlID="txt_hostel"
                                                                PopupControlID="panel_hostel" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="divmess" runat="server" visible="false">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Lbmess" runat="server" Text="Mess Name"></asp:Label>
                                                </td>
                                                <td id="mess1" runat="server">
                                                    <asp:UpdatePanel ID="UP_mess" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_mess" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="panel_mess" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                                                height: auto;">
                                                                <asp:CheckBox ID="cb_mess" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                    OnCheckedChanged="cb_mess_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="cbl_mess" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_mess_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="pce_mess" runat="server" TargetControlID="txt_mess"
                                                                PopupControlID="panel_mess" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:RadioButton ID="rbstud" runat="server" Text="Student" GroupName="g1" AutoPostBack="true"
                                        OnCheckedChanged="rbstud_Changed" />
                                    <asp:RadioButton ID="rbstaff" runat="server" Text="Staff" GroupName="g1" AutoPostBack="true"
                                        OnCheckedChanged="rbstaff_Changed" />
                                    <asp:RadioButton ID="rblbr" runat="server" Text="Library" GroupName="g1" AutoPostBack="true"
                                        OnCheckedChanged="rblbr_Changed" />
                                    <asp:RadioButton ID="rblhos" runat="server" Text="Hostel" GroupName="g1" AutoPostBack="true"
                                        OnCheckedChanged="rblhos_Changed" />
                                    <asp:RadioButton ID="rblmess" runat="server" Text="Mess" GroupName="g1" AutoPostBack="true"
                                        OnCheckedChanged="rblmess_Changed" />
                                </td>
                                <td colspan="4">
                                    <asp:Button ID="btngo" runat="server" CssClass="textbox btn2" Text="Go" OnClick="btngo_Click" />
                                    <asp:Button ID="btnaddnew" runat="server" CssClass="textbox btn2" Text="AddNew" OnClick="btnaddnew_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <div id="divspread" runat="server" visible="false" style="width: 961px; overflow: auto;
                                            background-color: White; border-radius: 10px;">
                                            <center>
                                                <FarPoint:FpSpread ID="FpSpreadbase" runat="server" Visible="true" BorderStyle="Solid"
                                                    BorderWidth="0px" Width="930px" Style="overflow: auto; border: 0px solid #999999;
                                                    border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                                    OnButtonCommand="Fpspreadbase_UpdateCommand" class="spreadborder">
                                                    <Sheets>
                                                        <FarPoint:SheetView SheetName="Sheet1">
                                                        </FarPoint:SheetView>
                                                    </Sheets>
                                                </FarPoint:FpSpread>
                                            </center>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
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
                                                <asp:Button ID="btndel" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    Text="Delete" OnClick="btndel_Click" Height="32px" Style="margin-top: 10px;"
                                                    CssClass="textbox textbox1" Width="60px" />
                                                <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                                            </div>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </center>
                </div>
            </center>
            <center>
                <div id="divaddnew" runat="server" visible="false" class="popupstyle popupheight1 ">
                    <asp:ImageButton ID="imgaddmew" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 30px; margin-left: 343px;"
                        OnClick="imgaddmew_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; height: 460px; width: 721px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <center>
                            <span class="fontstyleheader" style="color: Green;">Holidays Entry</span>
                        </center>
                        <br />
                        <center>
                            <div id="subdiv" runat="server" class="maindivstyle">
                                <center>
                                    <table>
                                        <tr>
                                            <td colspan="6">
                                                <asp:RadioButton ID="addnewrbstud" runat="server" Checked="true" Text="Student" AutoPostBack="true"
                                                    OnCheckedChanged="addnewrbstud_Changed" GroupName="s2" />
                                                <asp:RadioButton ID="addnewrbstaff" runat="server" Text="Staff" AutoPostBack="true"
                                                    OnCheckedChanged="addnewrbstaff_Changed" GroupName="s2" />
                                                <asp:RadioButton ID="addnewrblbr" runat="server" Text="Library" AutoPostBack="true"
                                                    OnCheckedChanged="addnewrblbr_Changed" GroupName="s2" />
                                                <asp:RadioButton ID="addnewrbhos" runat="server" Text="Hostel" AutoPostBack="true"
                                                    OnCheckedChanged="addnewrbhos_Changed" GroupName="s2" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="height: 339px;">
                                        <tr id="trstud" runat="server" visible="false">
                                            <td>
                                                <div>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                College
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddl_popclg1" runat="server" CssClass="textbox1 ddlstyle ddlheight3"
                                                                    OnSelectedIndexChanged="ddl_popclg1_change" Width="183px" AutoPostBack="true">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                Batch
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="addnewtxtyr" runat="server" Style="height: 20px; width: 112px;"
                                                                            ReadOnly="true">--Select--</asp:TextBox>
                                                                        <asp:Panel ID="panel7" runat="server" CssClass="multxtpanel" Style="width: 112px;
                                                                            height: 200px;">
                                                                            <asp:CheckBox ID="addnewcbyr" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                                OnCheckedChanged="addnewcbyr_OnCheckedChanged" />
                                                                            <asp:CheckBoxList ID="addnewcblyr" runat="server" AutoPostBack="true" OnSelectedIndexChanged="addnewcblyr_OnSelectedIndexChanged">
                                                                            </asp:CheckBoxList>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="addnewtxtyr"
                                                                            PopupControlID="panel7" Position="Bottom">
                                                                        </asp:PopupControlExtender>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Degree
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="addnewtxtdegree" runat="server" Style="height: 20px; width: 177px;"
                                                                            ReadOnly="true">--Select--</asp:TextBox>
                                                                        <asp:Panel ID="addnewpnldegree" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                                            Style="width: 180px; height: 120px;">
                                                                            <asp:CheckBox ID="addnewcbdegree" runat="server" Width="100px" Text="Select All"
                                                                                AutoPostBack="True" OnCheckedChanged="addnewcbdegree_OnCheckedChanged" />
                                                                            <asp:CheckBoxList ID="addnewcbldegree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="addnewcbldegree_OnSelectedIndexChanged">
                                                                            </asp:CheckBoxList>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="addnewtxtdegree"
                                                                            PopupControlID="addnewpnldegree" Position="Bottom">
                                                                        </asp:PopupControlExtender>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td>
                                                                Department
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="addnewtxtdept" runat="server" Style="height: 20px; width: 250px;"
                                                                            ReadOnly="true">--Select--</asp:TextBox>
                                                                        <asp:Panel ID="addnewpnldept" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                                            Style="width: 250px; height: 190px;">
                                                                            <asp:CheckBox ID="addnewcbdept" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                                OnCheckedChanged="addnewcbdept_OnCheckedChanged" />
                                                                            <asp:CheckBoxList ID="addnewcbldept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="addnewcbldept_OnSelectedIndexChanged">
                                                                            </asp:CheckBoxList>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="addnewtxtdept"
                                                                            PopupControlID="addnewpnldept" Position="Bottom">
                                                                        </asp:PopupControlExtender>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Semester
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="addnewtxtsem" runat="server" Style="height: 20px; width: 150px;"
                                                                            ReadOnly="true">--Select--</asp:TextBox>
                                                                        <asp:Panel ID="panel1" runat="server" CssClass="multxtpanel" Style="width: 146px;
                                                                            height: 172px;">
                                                                            <asp:CheckBox ID="addnewcbsem" runat="server" Width="124px" Text="Select All" AutoPostBack="True"
                                                                                OnCheckedChanged="addnewcbsem_OnCheckedChanged" />
                                                                            <asp:CheckBoxList ID="addnewcblsem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="addnewcblsem_sssOnSelectedIndexChanged">
                                                                            </asp:CheckBoxList>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="addnewtxtsem"
                                                                            PopupControlID="panel1" Position="Bottom">
                                                                        </asp:PopupControlExtender>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="trstf" runat="server" visible="false">
                                            <td>
                                                <div>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                College
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddl_popclg2" runat="server" CssClass="textbox1 ddlstyle ddlheight3"
                                                                    OnSelectedIndexChanged="ddl_popclg2_change" Width="183px" AutoPostBack="true">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                Staff Type
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="addnewtxtstftype" runat="server" Style="height: 20px; width: 173px;"
                                                                            ReadOnly="true">--Select--</asp:TextBox>
                                                                        <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 177px;
                                                                            height: 120px;">
                                                                            <asp:CheckBox ID="addnewcbstafftype" runat="server" Width="100px" Text="Select All"
                                                                                AutoPostBack="True" OnCheckedChanged="addnewcbstafftype_OnCheckedChanged" />
                                                                            <asp:CheckBoxList ID="addnewcblstafftype" runat="server" AutoPostBack="True" OnSelectedIndexChanged="addnewcblstafftype_OnSelectedIndexChanged">
                                                                            </asp:CheckBoxList>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="addnewtxtstftype"
                                                                            PopupControlID="Panel2" Position="Bottom">
                                                                        </asp:PopupControlExtender>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Leave Days
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="addnewtxtlvedy" runat="server" Style="height: 20px; width: 181px;"
                                                                            ReadOnly="true">--Select--</asp:TextBox>
                                                                        <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 185px;
                                                                            height: 120px;">
                                                                            <asp:CheckBox ID="addnewcblvedy" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                                OnCheckedChanged="addnewcblvedy_OnCheckedChanged" />
                                                                            <asp:CheckBoxList ID="addnewcbllvedy" runat="server" AutoPostBack="True" OnSelectedIndexChanged="addnewcbllvedy_OnSelectedIndexChanged">
                                                                            </asp:CheckBoxList>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="addnewtxtlvedy"
                                                                            PopupControlID="Panel3" Position="Bottom">
                                                                        </asp:PopupControlExtender>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="trlbr" runat="server" visible="false">
                                            <td>
                                                <div>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                College
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddl_popclg3" runat="server" CssClass="textbox1 ddlstyle ddlheight3"
                                                                    OnSelectedIndexChanged="ddl_popclg3_change" Width="183px" AutoPostBack="true">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                Library
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="addnewtxtlbr" runat="server" Style="height: 20px; width: 307px;"
                                                                            ReadOnly="true">--Select--</asp:TextBox>
                                                                        <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 309px;
                                                                            height: 120px;">
                                                                            <asp:CheckBox ID="addnewcblbr" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                                OnCheckedChanged="addnewcblbr_OnCheckedChanged" />
                                                                            <asp:CheckBoxList ID="addnewcbllbr" runat="server" AutoPostBack="True" OnSelectedIndexChanged="addnewcbllbr_OnSelectedIndexChanged">
                                                                            </asp:CheckBoxList>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="addnewtxtlbr"
                                                                            PopupControlID="Panel4" Position="Bottom">
                                                                        </asp:PopupControlExtender>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="trhos" runat="server" visible="false">
                                            <td>
                                                <div>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="chk_mess" runat="server" Text="Mess" AutoPostBack="true" OnCheckedChanged="chk_mess_Changed" />
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chk_hostel" runat="server" Text="Hostel" AutoPostBack="true" OnCheckedChanged="chk_hostel_Changed" />
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblmess" runat="server" Text="Mess Name"></asp:Label>
                                                            </td>
                                                            <td id="anmess" runat="server">
                                                                <asp:UpdatePanel ID="UPmess" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txtmess" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                                        <asp:Panel ID="panelmess" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                                                            height: auto;">
                                                                            <asp:CheckBox ID="cbmess" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                                OnCheckedChanged="cbmess_OnCheckedChanged" />
                                                                            <asp:CheckBoxList ID="cblmess" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblmess_OnSelectedIndexChanged">
                                                                            </asp:CheckBoxList>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="pcemess" runat="server" TargetControlID="txtmess" PopupControlID="panelmess"
                                                                            Position="Bottom">
                                                                        </asp:PopupControlExtender>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblhostel" runat="server" Text="Hostel Name" Visible="false"></asp:Label>
                                                            </td>
                                                            <td id="anhostel" runat="server" visible="false">
                                                                <asp:UpdatePanel ID="UPhostel" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txthostel" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                                        <asp:Panel ID="panelhostel" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                                                            height: auto;">
                                                                            <asp:CheckBox ID="cbhostel" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                                OnCheckedChanged="cbhostel_OnCheckedChanged" />
                                                                            <asp:CheckBoxList ID="cblhostel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblhostel_OnSelectedIndexChanged">
                                                                            </asp:CheckBoxList>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="pcehostel" runat="server" TargetControlID="txthostel"
                                                                            PopupControlID="panelhostel" Position="Bottom">
                                                                        </asp:PopupControlExtender>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <div id="div1" runat="server">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label1" runat="server" Text="From" Style="margin-top: 10px; margin-left: -1px;"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="addnewtxtfrdt" runat="server" Style="height: 20px; width: 75px;
                                                                    margin-left: 9px;"></asp:TextBox>
                                                                <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="addnewtxtfrdt" runat="server"
                                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label2" runat="server" Text="To" Style="margin-left: 4px;"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="addnewtxttodt" runat="server" Style="height: 20px; width: 75px;
                                                                    margin-left: 6px;"></asp:TextBox>
                                                                <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="addnewtxttodt" runat="server"
                                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="trleave" runat="server" visible="false">
                                            <td>
                                                <div>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="addnewrbhalf" runat="server" Text="Half Day" GroupName="z2"
                                                                    AutoPostBack="true" OnCheckedChanged="addnewrbhalf_Changed" />
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="addnewrbfull" runat="server" Text="Full Day" GroupName="z2"
                                                                    AutoPostBack="true" OnCheckedChanged="addnewrbfull_Changed" />
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="rbmng" runat="server" Text="Morning" Enabled="false" Checked="true"
                                                                    GroupName="y2" />
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="rbeve" runat="server" Text="Evening" Enabled="false" GroupName="y2" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <asp:Label ID="lbldescr" runat="server" Text="Description"></asp:Label>
                                                <asp:TextBox ID="txtdes" runat="server" TextMode="MultiLine" Style="width: 450px;
                                                    height: 76px;"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnsave" runat="server" CssClass="textbox btn2" Text="Save" OnClick="btnsave_Click" />
                                                <asp:Button ID="btnreset" runat="server" CssClass="textbox btn2" Text="Cancel" OnClick="btnreset_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </div>
            </center>
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
                                            <asp:Label ID="lbl_alert" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                            <asp:Label ID="Label3" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_errorclose" CssClass="textbox textbox1" Style="height: 28px;
                                                    width: 65px;" OnClick="btn_errorclose_Click" Text="Ok" runat="server" />
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
    </body>
    </html>
</asp:Content>
