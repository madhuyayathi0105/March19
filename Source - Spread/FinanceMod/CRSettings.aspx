<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CRSettings.aspx.cs" Inherits="CRSettings" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title>Settings</title>
    <link rel="Shortcut Icon" href="college/Left_Logo.jpeg" />
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green">Receipt / Challan Print Setting
                </span>
            </div>
        </center>
        <center>
            <div class="maindivstyle" style="height: 770px">
                <div id="div_check" runat="server" style="height: 40px; padding-top: 5px;">
                    <%--<fieldset style="border-radius: 10px; background-color: White;">--%>
                    <asp:Label ID="lbl_college" runat="server" Text="College"></asp:Label>
                    <asp:DropDownList ID="ddl_college" runat="server" CssClass="textbox  ddlheight5"
                        AutoPostBack="true" OnSelectedIndexChanged="ddl_college_OnselectChange">
                    </asp:DropDownList>
                    <asp:RadioButton ID="rdo_receipt" runat="server" RepeatDirection="Horizontal" GroupName="same"
                        Text="Receipt" Width="100px" AutoPostBack="true" Checked="true" OnCheckedChanged="rdo_receipt_OnCheckedChanged">
                    </asp:RadioButton>
                    <asp:RadioButton ID="rdo_challan" runat="server" RepeatDirection="Horizontal" GroupName="same"
                        Text="Challan" Width="100px" AutoPostBack="true" OnCheckedChanged="rdo_challan_OnCheckedChanged">
                    </asp:RadioButton>
                    <%-- </fieldset>--%>
                </div>
                <div id="div_receipt" runat="server" visible="false">
                    <center>
                        <fieldset style="height: 664px; width: 900px; border-radius: 10px;">
                            <div>
                                <table cellpadding="5" style="width: 900px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_header" runat="server" Text="Header:"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cb_colgname" runat="server" Text="College Name" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_addr1" runat="server" Text="Address1" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_addr2" runat="server" Text="Address2" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_addr3" runat="server" Text="Address3" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_dist" runat="server" Text="District" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_state" runat="server" Text="State" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_univ" runat="server" Text="University" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_time" runat="server" Text="Time" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cb_degree" runat="server" Text="Degree Acronym" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_sem" runat="server" Text="Semester" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_rollno" runat="server" Text="Roll No" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_regno" runat="server" Text="Reg No" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_appno" runat="server" Text="Admin No" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_llogo" runat="server" Text="Left Logo" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_rlogo" runat="server" Text="Right Logo" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_year" runat="server" Text="Year" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cb_seattype" runat="server" Text="Seat Type" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_boarding" runat="server" Text="Boarding" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_studname" runat="server" Text="Student's Name" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_fname" runat="server" Text="Father's Name" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_mname" runat="server" Text="Mother's Name" />
                                        </td>
                                        <td colspan="2">
                                            <asp:CheckBox ID="cb_degname" runat="server" Text="Degree Name" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:CheckBox ID="cb_adminno" runat="server" Text="Set Roll No as Admission No" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_validity" runat="server" Text="Valid Date" />
                                        </td>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="Updp_date" runat="server">
                                                <ContentTemplate>
                                                    <asp:Label ID="valid" runat="server" Text="Receipt Valid Upto:"></asp:Label>
                                                    <asp:TextBox ID="txt_valid" runat="server" CssClass="textbox  textbox1 txtheight"
                                                        Height="15PX"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_valid" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cb_mobile" runat="server" Text="Mobile No" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_mail" runat="server" Text="Email" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_website" runat="server" Text="Website" />
                                        </td>
                                        <td colspan ="2">
                                            <asp:CheckBox ID="cb_CommonClgname" runat="server" Text="Common CollegeName"  />
                                        </td>
                                         <td colspan ="2">
                                            <asp:CheckBox ID="cb_hostelname" runat="server" Text="Display HostelName"  />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div>
                                <table style="line-height: 30px; width: 800px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_body" runat="server" Text="Body:"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--                       <td>
                                        <asp:CheckBox ID="cb_allfees" runat="server" Text="Show All Fees" Visible="false" />
                                    </td>--%>
                                        <td colspan="2">
                                            <asp:CheckBox ID="cb_alloted" runat="server" Text="Alloted Amount" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_fine" runat="server" Text="Fine" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_balance" runat="server" Text="Balance" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_semester" runat="server" Text="Semester/Year" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:CheckBox ID="cb_previous" runat="server" Text="Previous Paid Amount" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_excess" runat="server" Text="Excess Amount" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_total" runat="server" Text="Total Details" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_fineinrow" runat="server" Text="Fine in Row" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:CheckBox ID="cb_totalcolumn" runat="server" Text="Total With Selected Column" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_concession" runat="server" Text="Concession" />
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="Label2" runat="server" Text="Concession:"></asp:Label>
                                            <asp:TextBox ID="txt_concession" runat="server" CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div>
                                <table style="line-height: 30px; width: 800px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblGrid" runat="server" Text="Hide Grid Column:"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cbFeeAmount" runat="server" Text="Fee Amount" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cbDedAmount" runat="server" Text="Deduction" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cbTotAmount" runat="server" Text="Total" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cbPaidAmount" runat="server" Text="Paid" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cbShowBalOnly" runat="server" Text="Show Balance Only" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cbCurSem" runat="server" Text="Current Semester" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="showdatetime" runat="server" Text="Show Date/Time" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div>
                                <table style="line-height: 30px; width: 800px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_footer" runat="server" Text="Footer:"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cb_student" runat="server" Text="Student Copy" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_office" runat="server" Text="Office Copy" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_transport" runat="server" Text="Transport Copy" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_narration" runat="server" Text="Narration" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_deduction" runat="server" Text="Deduction" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_forcolgname" runat="server" Text="For College Name" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:CheckBox ID="cb_authsign" runat="server" Text="Authorised Signatory:" />
                                            <asp:TextBox ID="txt_authsign" runat="server" MaxLength="50" CssClass="textbox txtheight2"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="fil1" runat="server" TargetControlID="txt_authsign"
                                                FilterType="UppercaseLetters, LowercaseLetters,Custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_acayear" runat="server" Text="Academic Year" />
                                            <%--added by abarna--%>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_exclude" runat="server" Text="Exclude Copy" />
                                            <%--added by abarna--%>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_collectedby" runat="server" Text="Collected By" />
                                            <%--added by abarna--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:CheckBox ID="socopypage" runat="server" Text="Students & Office Copy in One Page" />
                                        </td>
                                        <td colspan="2">
                                            <asp:CheckBox ID="cb_modecash" runat="server" Text="Display Mode With Cash" />
                                        </td>
                                        <td colspan="2">
                                            <asp:CheckBox ID="cb_Username" runat="server" Text="User Name" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:CheckBox ID="cb_sign" runat="server" Text="Signature:" />
                                            <asp:FileUpload ID="FileUpload1" runat="server" CssClass=" textbox textbox1" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div>
                                <table style="line-height: 30px; width: 800px;">
                                    <tr>
                                        <td>
                                            Receipt Format
                                            <asp:DropDownList ID="rbl_ReceiptFormat" runat="server" CssClass="textbox textbox1 ddlheight2"
                                                AutoPostBack="true" OnSelectedIndexChanged="ReceiptFormat_OnselectChange">
                                                <asp:ListItem Selected="True">Format1</asp:ListItem>
                                                <asp:ListItem>Format2</asp:ListItem>
                                                <asp:ListItem>Format3</asp:ListItem>
                                                <asp:ListItem>Format4</asp:ListItem>
                                                <asp:ListItem>Format5</asp:ListItem>
                                                <asp:ListItem>Format6</asp:ListItem>
                                                <asp:ListItem>Format7</asp:ListItem>
                                                <asp:ListItem>Format8</asp:ListItem>
                                                <asp:ListItem>Format9</asp:ListItem>
                                                <asp:ListItem>Format10</asp:ListItem>
                                                <asp:ListItem>Format11</asp:ListItem>
                                                <asp:ListItem>Format12</asp:ListItem>
                                                <asp:ListItem>Format13</asp:ListItem>
                                                <asp:ListItem>Format14</asp:ListItem>
                                                <asp:ListItem>Format15</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <%--abarna--%>
                                            <asp:Label ID="print" runat="server" Text="Page Size" Visible="false"></asp:Label>
                                            <asp:DropDownList ID="printformat" runat="server" CssClass="textbox textbox1 ddlheight2"
                                                Visible="false" OnSelectedIndexChanged="printformat_OnselectChange" AutoPostBack="true">
                                                <asp:ListItem Selected="True">A4 Sheet</asp:ListItem>
                                                <asp:ListItem>10*6</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtheight" runat="server" Visible="false" CssClass="textbox textbox1 ddlheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Challan Format
                                            <asp:RadioButtonList ID="rbl_ChallanFormat" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="True">Format1</asp:ListItem>
                                                <asp:ListItem>Format2</asp:ListItem>
                                                <asp:ListItem>Format3</asp:ListItem>
                                                <asp:ListItem>Format4</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </center>
                    <center>
                        <div>
                            <asp:Button ID="btn_save" runat="server" BackColor="#8199FD" Text="Save" CssClass="textbox btn2  textbox1"
                                OnClick="btn_save_Click" />
                            <asp:Button ID="btn_exit" runat="server" Text="Exit" CssClass="textbox btn2  textbox1"
                                OnClick="btn_exit_Click" Visible="false" /></div>
                    </center>
                </div>
                <div id="div_challan" runat="server" visible="false">
                    <div id="div_challanprint" runat="server">
                        <div id="Div1" runat="server" style="margin-left: 600px;">
                            <asp:LinkButton ID="lnkChlPageSet" runat="server" Visible="true" OnClick="lnkChlPageSet_Click"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Black"
                                CausesValidation="False">Print Settings
                            </asp:LinkButton>&nbsp;&nbsp;
                            <asp:LinkButton ID="lbtn_hdrsettings" runat="server" Visible="true" OnClick="lbtn_hdrsettings_Click"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Black"
                                CausesValidation="False">Header Settings
                            </asp:LinkButton>
                        </div>
                        <center>
                            <table width="700px">
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_title" runat="server" Text="Title"></asp:Label>
                                                    <asp:Button ID="btnplus1" runat="server" Text="+" CssClass="textbox btn textbox1"
                                                        Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus1_OnClick" />
                                                    <asp:DropDownList ID="ddl_title" runat="server" CssClass="textbox ddlheight2">
                                                    </asp:DropDownList>
                                                    <asp:Button ID="btnminus1" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                                        Font-Names="Book Antiqua" CssClass="textbox btn textbox1" OnClick="btnminus1_OnClick" />
                                                    <asp:Label ID="lbl_stream" runat="server" Visible="false" Text=""></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="Updp_strm" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_strm" runat="server" CssClass="textbox txtheight1" ReadOnly="true"
                                                                onfocus="return myFunction1(this)"></asp:TextBox>
                                                            <asp:Panel ID="panel_strm" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                                Width="150px">
                                                                <asp:CheckBox ID="cb_strm" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_strm_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="cbl_strm" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_strm_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="Popupce_strm" runat="server" TargetControlID="txt_strm"
                                                                PopupControlID="panel_strm" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_degree2" runat="server" ReadOnly="true" Height="20px" CssClass="textbox txtheight1">Degree</asp:TextBox>
                                                            <asp:Panel ID="pdegree" runat="server" Width="120px" Height="170px" CssClass="multxtpanel">
                                                                <asp:CheckBox ID="cb_degree2" runat="server" OnCheckedChanged="cb_degree2_ChekedChange"
                                                                    Text="Select All" AutoPostBack="True" />
                                                                <asp:CheckBoxList ID="cbl_degree2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree2_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_degree2"
                                                                PopupControlID="pdegree" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="updp_dept" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox txtheight1" ReadOnly="true"
                                                                onfocus="return myFunction1(this)">Department</asp:TextBox>
                                                            <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                                <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_dept_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="Popupce_dept" runat="server" TargetControlID="txt_dept"
                                                                PopupControlID="panel_dept" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <div>
                                                        <asp:Button ID="btnSaveChallan" runat="server" BackColor="#8199FD" Text="Save" CssClass="textbox btn2  textbox1"
                                                            OnClick="btn_saveChallan_Click" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_slcthdr" runat="server" Text="Select Header"></asp:Label><br />
                                                    <asp:ListBox ID="lb_selecthdr" runat="server" Height="150px" Width="300px"></asp:ListBox>
                                                </td>
                                                <td>
                                                    <br />
                                                    <asp:Button ID="btnMvOneRt" runat="server" Text=">" CssClass="textbox textbox1 btn"
                                                        OnClick="btnMvOneRt_Click" /><br />
                                                    <asp:Button ID="btnMvTwoRt" runat="server" Text=">>" CssClass="textbox textbox1 btn"
                                                        OnClick="btnMvTwoRt_Click" /><br />
                                                    <asp:Button ID="btnMvOneLt" runat="server" Text="<" CssClass="textbox textbox1 btn"
                                                        OnClick="btnMvOneLt_Click" /><br />
                                                    <asp:Button ID="btnMvTwoLt" runat="server" Text="<<" CssClass="textbox textbox1 btn"
                                                        OnClick="btnMvTwoLt_Click" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_slctedhdr" runat="server" Text="Selected Headers"></asp:Label><br />
                                                    <asp:ListBox ID="lb_hdr" runat="server" Height="150px" Width="300px"></asp:ListBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                        <center>
                                            <span style="color: red; font-size: 18px; font-weight: bold;">Remove Headers From Group
                                                and Department</span></center>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:CheckBox ID="cb_GHwise" runat="server" OnCheckedChanged="cb_GHwise_OncheckedChanged"
                                                Text="GroupHeader" AutoPostBack="true" />
                                            <asp:CheckBox ID="cb_Degwise" runat="server" OnCheckedChanged="cb_Degwise_OncheckedChanged"
                                                Text="Degreewise" AutoPostBack="true" /></center>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="width: 700px; height: 250px; overflow: auto;">
                                            <center>
                                                <asp:GridView ID="gridGHreport" BackColor="white" runat="server" AutoGenerateColumns="false"
                                                    GridLines="Both">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Group Header" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_ghdr" runat="server" Text='<%#Eval("GroupHeader") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Header" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_hdr" runat="server" Text='<%#Eval("Header") %>'></asp:Label>
                                                                <asp:Label ID="lbl_hdrPk" Visible="false" runat="server" Text='<%#Eval("HeaderPk") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Remove" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnEdit_GH" BackColor="SandyBrown" runat="server" CssClass="textbox textbox1 btn2"
                                                                    Text="Remove" OnClick="btn_modifyClickGH" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <asp:GridView ID="gridDegReport" BackColor="white" runat="server" AutoGenerateColumns="false"
                                                    GridLines="Both">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Degree" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_deg" runat="server" Text='<%#Eval("Degree") %>'></asp:Label>
                                                                <asp:Label ID="lbldegCode" Visible="false" runat="server" Text='<%#Eval("DegCode") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Department" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_dept" runat="server" Text='<%#Eval("Department") %>'></asp:Label>
                                                                <asp:Label ID="lbldeptCode" Visible="false" runat="server" Text='<%#Eval("DeptCode") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Group Header" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_ghdr" runat="server" Text='<%#Eval("GroupHeader") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Semester/ Year" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_hdr" runat="server" Text='<%#Eval("TextVal") %>'></asp:Label>
                                                                <asp:Label ID="lbl_hdrPk" Visible="false" runat="server" Text='<%#Eval("TextValCode") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Remove" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnEdit_GH" BackColor="SandyBrown" runat="server" CssClass="textbox textbox1 btn2"
                                                                    Text="Remove" OnClick="btn_modifyClickDeg" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </center>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                    <%------popup window1-----%>
                    <%-- ***********imgdiv*******--%>
                    <div id="imgdiv3" runat="server" visible="false" style="height: 100%; z-index: 100000000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="panel_description" runat="server" visible="false" class="table" style="background-color: White;
                                height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                margin-top: 200px; border-radius: 10px;">
                                <table>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_description11" runat="server" Text="Add New Title" Font-Bold="true"
                                                Font-Size="Large" ForeColor="Green"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:TextBox ID="txt_description11" runat="server" Width="200px" Style="font-family: 'Book Antiqua';
                                                margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="btn_adddesc1" runat="server" Text="Add" Font-Names="Book Antiqua"
                                                Font-Size="Medium" CssClass="textbox btn1 textbox1" OnClick="btndescpopadd_Click" />
                                            <asp:Button ID="btn_exitdesc1" runat="server" Text="Exit" Font-Names="Book Antiqua"
                                                Font-Size="Medium" CssClass="textbox btn1 textbox1" OnClick="btndescpopexit_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </center>
                    </div>
                    <%--************--%>
                    <%--  **********end of popup**********--%>
                </div>
                <div id="pop_hdrsettings" runat="server" visible="false" style="height: 50em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0;">
                    <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 76px; margin-left: 285px;"
                        OnClick="imagebtnpopclose_Click" />
                    <br />
                    <br />
                    <br />
                    <br />
                    <div style="background-color: White; height: 365px; width: 596px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <span class="fontstyleheader" style="color: Green;">Header Settings</span>
                        </center>
                        <br />
                        <div id="div_hdrsettings" runat="server">
                            <table style="line-height: 37px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_hdrname" runat="server" Text="Header Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="Updp_hdrname" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_hdrname" runat="server" CssClass="textbox txtheight4" ReadOnly="true"
                                                    onfocus="return myFunction1(this)"></asp:TextBox>
                                                <asp:Panel ID="panel_hdrname" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                    <asp:CheckBox ID="cb_hdrname" runat="server" AutoPostBack="true" OnCheckedChanged="cb_hdrname_OnCheckedChanged"
                                                        Text="Select All" />
                                                    <asp:CheckBoxList ID="cbl_hdrname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_hdrname_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="Popupce_hdrname" runat="server" TargetControlID="txt_hdrname"
                                                    PopupControlID="panel_hdrname" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_strm" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td>
                                        <%--<asp:DropDownList ID="ddl_strm" runat="server" CssClass="textbox ddlheight4">
                                    </asp:DropDownList>--%>
                                        <asp:UpdatePanel ID="UpdatePanelst" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_strm1" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                    onfocus="return myFunction1(this)"></asp:TextBox>
                                                <asp:Panel ID="panel_strm1" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                    Width="150px">
                                                    <asp:CheckBox ID="cb_strm1" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_strm1_OnCheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_strm1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_strm1_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtenderst1" runat="server" TargetControlID="txt_strm1"
                                                    PopupControlID="panel_strm1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_disp" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_dispstream" runat="server" CssClass="textbox txtheight4"></asp:TextBox>
                                        <%-- <asp:Button ID="btnplusDisp" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                        Font-Size="Medium" Font-Names="Book Antiqua" Width="" Height="" OnClick="btnplusDisp_OnClick" />
                                    <asp:DropDownList ID="ddl_disp" Height="35px" Width="200px" runat="server" CssClass="textbox ddlheight2">
                                    </asp:DropDownList>
                                    <asp:Button ID="btnminusDisp" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                        Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btnminusDisp_OnClick" />--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_grphdr" runat="server" Text="Group Header"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_grphdr" runat="server" CssClass="textbox txtheight4"></asp:TextBox>
                                        <%-- <asp:Button ID="btnplus" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                        Font-Size="Medium" Font-Names="Book Antiqua" Width="" Height="" OnClick="btnplus_OnClick" />
                                    <asp:DropDownList ID="ddl_grphdr" runat="server" Width="200px" Font-Names="Book Antiqua"
                                        CssClass="textbox ddlheight2"  Font-Size="Medium" Height="35px">
                                    </asp:DropDownList>
                                    <asp:Button ID="btnminus" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                        Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btnminus_OnClick" />--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_accno" runat="server" Text="Bank / Account No"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_collegebank" runat="server" CssClass="textbox ddlheight2">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div>
                                <asp:Button ID="btn_save1" runat="server" Text="Save" CssClass="textbox btn2 textbox1"
                                    OnClick="btn_save1_Click" />
                                <asp:Button ID="btn_exit1" runat="server" Text="Exit" CssClass="textbox btn2  textbox1"
                                    OnClick="imagebtnpopclose_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <%--Challan Print settings popup--%>
                <div id="divChlanPrintSet" runat="server" visible="false" style="height: 50em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0;">
                    <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 16px; margin-left: 385px;"
                        OnClick="ImageButtonChal_Click" />
                    <br />
                    <div style="background-color: White; height: 500px; width: 800px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <center>
                            <span class="fontstyleheader" style="color: Green;">Challan Print Settings</span>
                        </center>
                        <div class="table" style="width: 700px; height: 400px;">
                            <table style="width: 700px;">
                                <tr>
                                    <td>
                                        Display 'Term' for Semester or Year
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cblTermDisp" runat="server" Text="Select" RepeatDirection="Horizontal" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Fee Counter Name
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_ChlCounter" runat="server" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Particulars
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_ChlParticulars" runat="server" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Use Degree Acronym in Challan
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkChlanDegAcr" runat="server" Text="Select" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Show IFSC in Challan
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkUseIfsc" runat="server" Text="Select" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Unique ID to use for Smartcard Number
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblSmartNodisplay" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Selected="True">RollNo</asp:ListItem>
                                            <asp:ListItem>RegNo</asp:ListItem>
                                            <asp:ListItem>AdmisssionNo</asp:ListItem>
                                            <asp:ListItem>ApplicationNo</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Display denomination in Challan
                                    </td>
                                    <td>
                                        <asp:CheckBoxList ID="cblDenom" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem>College Copy</asp:ListItem>
                                            <asp:ListItem>Bank Copy</asp:ListItem>
                                            <asp:ListItem>Student Copy</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Academic Year
                                    </td>
                                    <td>
                                        <asp:Label ID="lblacefromyear" runat="server" Text="From Year" Font-Size="Medium"
                                            Font-Names="Book Antiqua"></asp:Label>
                                        <asp:DropDownList ID="ddlacefromyear" AutoPostBack="true" OnSelectedIndexChanged="ddlacefromyear_Indexchange"
                                            runat="server" Font-Names="Book Antiqua" Font-Size="Medium">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblacetoyear" runat="server" Text="To Year" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                        <asp:DropDownList ID="ddlacetoyear" runat="server" Font-Names="Book Antiqua" Font-Size="Medium">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Show Ledgerwise Fees in Challan
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbShowledgerwise" runat="server" Text="Select" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Challan Office Footer
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtChallanOfficeFooter" runat="server" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Institution Display
                                        <asp:DropDownList ID="ddl_title1" runat="server" CssClass="textbox ddlheight2" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddl_title1_IndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInstituteHideValue" runat="server" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Include Hostel Name
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txthostelname" runat="server" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cbincshift" runat="server" Text="Include Shift" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <center>
                            <asp:Button ID="btnSavePrint" CssClass=" textbox textbox1 btn2 " OnClick="btnSavePrint_Click"
                                Text="Save" runat="server" /></center>
                    </div>
                </div>
                <%--Delete Confirmation Popup --%>
                <center>
                    <div id="surediv" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="Div3" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lbl_sure" runat="server" Text="Do You Want To Delete Selected Title?"
                                                    Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btn_yes" CssClass=" textbox textbox1 btn1 " Style="height: 28px;
                                                        width: 65px;" OnClick="btn_sureyes_Click" Text="yes" runat="server" />
                                                    <asp:Button ID="btn_no" CssClass=" textbox textbox1 btn1 " Style="height: 28px; width: 65px;"
                                                        OnClick="btn_sureno_Click" Text="no" runat="server" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
                <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 10000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="panel_erroralert" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_erroralert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_erroralert" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                                    width: 65px;" OnClick="btnerrclose_Click" Text="OK" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
                <%-- ***********imgdiv*******--%>
                <%--************--%>
            </div>
        </center>
    </body>
    </html>
</asp:Content>
