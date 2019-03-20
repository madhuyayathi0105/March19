<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="BankCodeMaster.aspx.cs" Inherits="BankCodeMaster" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title>Code Master</title>
    <link rel="Shortcut Icon" href="images/palpap.jpg" />
    <style type="text/css">
        .stNew
        {
            text-transform: uppercase;
        }
    </style>
    <body>
        <script type="text/javascript" language="javascript">
            function CheckAllEmp(Checkbox) {
                var GridVwHeaderChckbox = document.getElementById("<%=gridHeaderPrev.ClientID %>");
                for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                    GridVwHeaderChckbox.rows[i].cells[1].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
                }
            }
            function CheckOK() {
                var GridVwHeaderChckbox = document.getElementById("<%=gridHeaderPrev.ClientID %>");
                for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                    if (GridVwHeaderChckbox.rows[i].cells[1].getElementsByTagName("INPUT")[0].checked) {
                        return true;
                    }
                }
                alert("No Headers Selected");
                return false;
            }
            function checkUpdate() {
                var ok = false;
                var GridVwHeaderChckbox = document.getElementById("<%=gridHeaderPrev.ClientID %>");
                for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                    if (GridVwHeaderChckbox.rows[i].cells[1].getElementsByTagName("INPUT")[0].checked) {
                        ok = true;
                    }
                }
                if (ok == false) {
                    alert("No Headers Selected");
                    return false;
                }

                var e = 0;

                for (var i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                    if (GridVwHeaderChckbox.rows[i].cells[3].getElementsByTagName("INPUT")[0].value == "") {
                        e = 1;
                    }
                    if (GridVwHeaderChckbox.rows[i].cells[4].getElementsByTagName("INPUT")[0].value == "") {
                        e = 1;
                    }
                    if (GridVwHeaderChckbox.rows[i].cells[5].getElementsByTagName("INPUT")[0].value == "") {
                        e = 1;
                    }
                    if (GridVwHeaderChckbox.rows[i].cells[6].getElementsByTagName("INPUT")[0].value == "") {
                        e = 1;
                    }
                    if (GridVwHeaderChckbox.rows[i].cells[7].getElementsByTagName("INPUT")[0].value == "") {
                        e = 1;
                    }
                    if (GridVwHeaderChckbox.rows[i].cells[8].getElementsByTagName("INPUT")[0].value == "") {
                        e = 1;
                    }
                    if (GridVwHeaderChckbox.rows[i].cells[9].getElementsByTagName("INPUT")[0].value == "") {
                        e = 1;
                    }
                    if (GridVwHeaderChckbox.rows[i].cells[10].getElementsByTagName("INPUT")[0].value == "") {
                        e = 1;
                    }
                    if (GridVwHeaderChckbox.rows[i].cells[11].getElementsByTagName("INPUT")[0].value == "") {
                        e = 1;
                    }
                }
                if (e == 1) {
                    alert('Please Fill All Values');
                    return false;
                }
                else {
                    return true;
                }
            }
        </script>
        <script type="text/javascript">
            //            function checkvalue() {
            //                var fl = 0;
            //                var onlineCol = 0;
            //                var id = document.getElementById("<%=ug_grid.ClientID %>");
            //                var gridViewControls = id.getElementsByTagName("input");
            //                var len = id.rows.length;
            //                for (var i = 0; i < gridViewControls.length; i++) {
            //                    if (gridViewControls[i].name.indexOf("txt_acronym") > 1) {
            //                        if (gridViewControls[i].value == "") {
            //                            fl = 1;
            //                            onlineCol = i;
            //                        }
            //                    }
            //                    if (gridViewControls[i].name.indexOf("txt_startno") > 1) {
            //                        if (gridViewControls[i].value == "") {
            //                            fl = 1;
            //                            onlineCol = i;
            //                        }
            //                    }
            //                    if (gridViewControls[i].name.indexOf("txt_size") > 1) {
            //                        if (gridViewControls[i].value == "") {
            //                            fl = 1;
            //                            onlineCol = i;
            //                        }
            //                    }
            //                }

            //                if (fl == 1) {
            //                    if (onlineCol != 7) {
            //                        alert('Please Fill All Values');
            //                    }
            //                    return false;
            //                }
            //                else {

            //                    return true;
            //                }
            //            }

            //            function checkvalue1() {
            //                var e = 0;
            //                var id = document.getElementById("<%=grid_header.ClientID %>");
            //                var gridViewControls = id.getElementsByTagName("input");
            //                var len = id.rows.length;
            //                for (var i = 0; i < gridViewControls.length; i++) {
            //                    if (gridViewControls[i].name.indexOf("txt_acronym") > 1) {
            //                        if (gridViewControls[i].value == "") {
            //                            e = 1;
            //                        }
            //                    }
            //                    if (gridViewControls[i].name.indexOf("txt_startno") > 1) {
            //                        if (gridViewControls[i].value == "") {
            //                            e = 1;
            //                        }
            //                    }
            //                    if (gridViewControls[i].name.indexOf("txt_size") > 1) {
            //                        if (gridViewControls[i].value == "") {
            //                            e = 1;
            //                        }
            //                    }
            //                }
            //                if (e == 1) {
            //                    alert('Please Fill All Values');
            //                    return false;
            //                }
            //                else {
            //                    return true;
            //                }
            //            }
            function checkAlphaValue(el) {
                var ex = /^[A-Za-z]*$/;
                if (ex.test(el.value) == false) {
                    el.value = "";
                } else {
                    el.value = ex.toUpperCase();
                }
            }
            function checkNumValue(el) {
                var ex = /^[0-9]*$/;
                if (ex.test(el.value) == false) {
                    el.value = "";
                }
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Code Setting</span></div>
        </center>
        <div class="maindivstyle" style="height: 650px;">
            <br />
            <div>
                <center>
                    <table class="maintablestyle" style="width: 750px; height: 40px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblcol" runat="server" Text="College Name"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcol" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlcol_OnSelectedIndexChanged"
                                    CssClass="textbox textbox1 ddlheight4">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_frmdate" Text="From Date" runat="server"></asp:Label>
                            </td>
                            <td rowspan="2">
                                <asp:TextBox ID="txt_frmdate" runat="server" CssClass="textbox textbox1" OnTextChanged="txt_frmdate_OnTextChanged"
                                    AutoPostBack="true" Width="80px"></asp:TextBox>
                                <asp:CalendarExtender ID="calfrmdate" TargetControlID="txt_frmdate" runat="server"
                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_prvdate" Text="Previous Date" runat="server"></asp:Label>
                            </td>
                            <td rowspan="2">
                                <asp:TextBox ID="txt_prvdate" runat="server" CssClass="textbox textbox1" Width="80px"
                                    Enabled="false" Visible="false"></asp:TextBox>
                                <asp:CalendarExtender ID="calvacatedate" TargetControlID="txt_prvdate" runat="server"
                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                                <asp:DropDownList ID="ddl_PrevDate" runat="server" CssClass=" textbox  ddlheight2"
                                    OnSelectedIndexChanged="ddl_PrevDate_OnSelectedIndexChange" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
            <br />
            <center>
                <asp:Label ID="txtdateerr" runat="server" Style="color: Red;" Visible="false"></asp:Label>
            </center>
            <div id="Mainpage" runat="server">
                <center>
                    <table class="table" width="970px">
                        <tr>
                            <td>
                                <asp:RadioButtonList ID="rblCommonHeader" runat="server" RepeatDirection="Horizontal"
                                    AutoPostBack="true" OnSelectedIndexChanged="rblCommonHeader_Changed">
                                    <asp:ListItem Selected="True">Common</asp:ListItem>
                                    <asp:ListItem>Headerwise</asp:ListItem>
                                </asp:RadioButtonList>
                                <center>
                                    <table runat="server" id="tdHdrBtns" visible="false">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UPdatepanel" runat="server">
                                                    <ContentTemplate>
                                                        Header
                                                        <asp:TextBox ID="txt_select" runat="server" CssClass="textbox txtheight2" ReadOnly="true">Header</asp:TextBox>
                                                        <asp:Panel ID="panel_header" runat="server" CssClass="multxtpanel">
                                                            <asp:CheckBox ID="cb_header" runat="server" Text="Select All" AutoPostBack="True"
                                                                OnCheckedChanged="cb_header_CheckedChanged" />
                                                            <asp:CheckBoxList ID="cbl_header" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_header_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="popupheader" runat="server" TargetControlID="txt_select"
                                                            PopupControlID="panel_header" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnGo" Visible="false" runat="server" CssClass="textbox textbox1 btn1"
                                                    Text="Go" OnClick="btnGo_OnClick" />
                                                <asp:Button ID="btnSaveHeader" Text="Save" runat="server" CssClass="textbox textbox1 btn2"
                                                    OnClick="btnSaveHeader_Click" OnClientClick="return checkvalue1()" />
                                            </td>
                                        </tr>
                                        <%-- <tr>
                                        <td colspan="2">
                                            <fieldset style="height: 100px; width: 250px; border-radius: 10px;">
                                                <legend title="Header Details"></legend>
                                                <table>
                                                    <tr>
                                                        <td style="text-align: right;">
                                                            Acronym :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtHdrAcr" MaxLength="20" CssClass=" textbox textbox1 txtheight1"
                                                                onkeyup="checkAlphaValue(this);"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right;">
                                                            Start Number :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtHdrStNo" CssClass=" textbox textbox1 txtheight1"
                                                                onkeyup="checkNumValue(this);" MaxLength="8"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right;">
                                                            Size :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtHdrSize" MaxLength="1" CssClass=" textbox textbox1 txtheight1"
                                                                onkeyup="checkNumValue(this);"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>--%>
                                    </table>
                                </center>
                            </td>
                            <td style="text-align: right; padding-right: 20px;" runat="server" id="tdBtns">
                                <asp:Button ID="btn_save" BackColor="#8199FD" Text="Save" runat="server" Font-Bold="true"
                                    Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox textbox1 btn2"
                                    OnClick="btn_save_Click" OnClientClick="return checkvalue()" />
                                <asp:Button ID="btn_reset" BackColor="#8199FD" Text="Reset" runat="server" Font-Bold="true"
                                    Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox textbox1 btn2"
                                    OnClick="btn_reset_Click" />
                                <asp:Button ID="btn_exit" BackColor="#8199FD" Text="Exit" runat="server" Font-Bold="true"
                                    Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox textbox1 btn2"
                                    OnClick="btn_exit_Click" />
                            </td>
                        </tr>
                        <tr runat="server" id="trCommonSet">
                            <td>
                                <fieldset style="border-radius: 10px;">
                                    <legend>New Setting</legend>
                                    <div>
                                        <asp:GridView ID="ug_grid" runat="server" AutoGenerateColumns="false" GridLines="None">
                                            <%--OnRowDataBound="grid_prev_Bound0" OnDataBound="OnDataBound0"--%>
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Code" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_itemdetails" runat="server" Text='<%#Eval("Dummy1") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Acronym" HeaderStyle-BackColor="#0CA6CA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_acronym" runat="server" CssClass="textbox textbox1 stNew txtheight"
                                                            onfocus="return myFunction2(this)" MaxLength="20"></asp:TextBox>
                                                        <%-- <asp:FilteredTextBoxExtender ID="txtextacr" runat="server" TargetControlID="txt_acronym"
                                                        FilterType="UppercaseLetters,LowercaseLetters" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Start No" HeaderStyle-BackColor="#0CA6CA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_startno" runat="server" CssClass="textbox textbox1 txtheight"
                                                            onfocus="return myFunction2(this)" MaxLength="8"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="filterextender12" runat="server" TargetControlID="txt_startno"
                                                            FilterType="Numbers" ValidChars=" ">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Size" HeaderStyle-BackColor="#0CA6CA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_size" runat="server" CssClass="textbox textbox1 txtheight" onfocus="return myFunction2(this)"
                                                            Width="20px" MaxLength="1"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="filterextender" runat="server" TargetControlID="txt_size"
                                                            FilterType="Numbers,custom" ValidChars="1,2,3,4,5,6">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </fieldset>
                            </td>
                            <td style="padding-left: 20px;">
                                <fieldset style="border-radius: 10px;">
                                    <legend>Old Setting</legend>
                                    <div>
                                        <asp:GridView ID="old_grid" runat="server" AutoGenerateColumns="false" GridLines="None">
                                            <%--OnRowDataBound="grid_prev_Bound0" OnDataBound="OnDataBound0"--%>
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Code" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblitemdetails" runat="server" Text='<%#Eval("Dummy1") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Acronym" HeaderStyle-BackColor="#0CA6CA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtacronym" runat="server" CssClass="textbox txtheight" Enabled="false"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Start No" HeaderStyle-BackColor="#0CA6CA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtstartno" runat="server" CssClass="textbox txtheight" Enabled="false"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="filterextender" runat="server" TargetControlID="txtstartno"
                                                            FilterType="Numbers" ValidChars=" ">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Size" HeaderStyle-BackColor="#0CA6CA">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_size" runat="server" CssClass="textbox txtheight" onfocus="return myFunction2(this)"
                                                            Width="20px" Enabled="false" MaxLength="1"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="filterextender11" runat="server" TargetControlID="txt_size"
                                                            FilterType="Numbers,custom" ValidChars="1,2,3,4,5,6">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                        <tr id="trHeaderSet" runat="server" visible="false">
                            <td colspan="2">
                                <center>
                                    <div id="div1" runat="server" style="height: 150px; overflow: auto;">
                                        <table>
                                            <tr>
                                                <td colspan="3">
                                                    <%--    <fieldset style="border-radius: 10px; " id="fsHSetting" runat="server">
                                <legend>Header Setting</legend>--%>
                                                    <div>
                                                        <asp:GridView ID="grid_header" runat="server" AutoGenerateColumns="false" GridLines="Both">
                                                            <%--OnRowDataBound="grid_prev_Bound0" OnDataBound="OnDataBound0"--%>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Header Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_itemdetails" runat="server" Text='<%#Eval("Dummy1") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Acronym" HeaderStyle-BackColor="#0CA6CA">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txt_acronym" runat="server" Text='<%#Eval("Dummy4") %>' CssClass="textbox textbox1 stNew txtheight"
                                                                            onfocus="return myFunction2(this)" MaxLength="20"></asp:TextBox>
                                                                        <%-- <asp:FilteredTextBoxExtender ID="txtextacr" runat="server" TargetControlID="txt_acronym"
                                                        FilterType="UppercaseLetters,LowercaseLetters" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>--%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Start No" HeaderStyle-BackColor="#0CA6CA">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txt_startno" runat="server" Text='<%#Eval("Dummy2") %>' CssClass="textbox textbox1 txtheight"
                                                                            onfocus="return myFunction2(this)" MaxLength="8"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="filterextender12" runat="server" TargetControlID="txt_startno"
                                                                            FilterType="Numbers" ValidChars=" ">
                                                                        </asp:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Size" HeaderStyle-BackColor="#0CA6CA">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txt_size" runat="server" Text='<%#Eval("Dummy3") %>' CssClass="textbox textbox1 txtheight"
                                                                            onfocus="return myFunction2(this)" Width="20px" MaxLength="1"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="filterextender" runat="server" TargetControlID="txt_size"
                                                                            FilterType="Numbers,custom" ValidChars="1,2,3,4,5,6">
                                                                        </asp:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                    <%-- </fieldset>--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </center>
                            </td>
                        </tr>
                        <tr id="trHeaderSetPrev" runat="server" visible="false">
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <center>
                                                <b>Previous Header Settings</b>
                                                <div style="height: 320px; width: 950px; overflow: auto;">
                                                    <asp:GridView ID="gridHeaderPrev" runat="server" AutoGenerateColumns="false" GridLines="Both">
                                                        <%--OnRowDataBound="grid_prev_Bound0" OnDataBound="OnDataBound0"--%>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="CheckAllEmp(this);" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="cb_Select" runat="server" Checked="false" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Header Names" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_HeaderSettingPk" runat="server" Text='<%#Eval("HeaderSettingPk") %>'
                                                                        Visible="false"></asp:Label>
                                                                    <asp:Label ID="lbl_HeaderCodes" runat="server" Text='<%#Eval("hdrCode") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lbl_itemdetails" runat="server" Text='<%#Eval("hdrNames") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Receipt Acronym" HeaderStyle-BackColor="#0CA6CA">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txt_acronym0" runat="server" Text='<%#Eval("rcptAcr") %>' CssClass="textbox textbox1 stNew txtheight"
                                                                        onfocus="return myFunction2(this)" MaxLength="20"></asp:TextBox>
                                                                    <%-- <asp:FilteredTextBoxExtender ID="txtextacr" runat="server" TargetControlID="txt_acronym"
                        FilterType="UppercaseLetters,LowercaseLetters" ValidChars=" ">
                    </asp:FilteredTextBoxExtender>--%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Receipt Start No" HeaderStyle-BackColor="#0CA6CA">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txt_startno0" runat="server" Text='<%#Eval("rcptStno") %>' CssClass="textbox textbox1 txtheight"
                                                                        onfocus="return myFunction2(this)" MaxLength="8"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="filterextender121" runat="server" TargetControlID="txt_startno0"
                                                                        FilterType="Numbers" ValidChars=" ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Receipt Size" HeaderStyle-BackColor="#0CA6CA">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:TextBox ID="txt_size0" runat="server" Text='<%#Eval("rcptSize") %>' CssClass="textbox textbox1 txtheight"
                                                                            onfocus="return myFunction2(this)" Width="20px" MaxLength="1"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="filterextender1" runat="server" TargetControlID="txt_size0"
                                                                            FilterType="Numbers,custom" ValidChars="1,2,3,4,5,6">
                                                                        </asp:FilteredTextBoxExtender>
                                                                    </center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Challan Acronym" HeaderStyle-BackColor="#0CA6CA">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txt_acronym1" runat="server" Text='<%#Eval("chlnAcr") %>' CssClass="textbox textbox1 stNew txtheight"
                                                                        onfocus="return myFunction2(this)" MaxLength="20"></asp:TextBox>
                                                                    <%-- <asp:FilteredTextBoxExtender ID="txtextacr" runat="server" TargetControlID="txt_acronym"
                        FilterType="UppercaseLetters,LowercaseLetters" ValidChars=" ">
                    </asp:FilteredTextBoxExtender>--%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Challan Start No" HeaderStyle-BackColor="#0CA6CA">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txt_startno1" runat="server" Text='<%#Eval("chlnStno") %>' CssClass="textbox textbox1 txtheight"
                                                                        onfocus="return myFunction2(this)" MaxLength="8"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="filterextender1212" runat="server" TargetControlID="txt_startno1"
                                                                        FilterType="Numbers" ValidChars=" ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Challan Size" HeaderStyle-BackColor="#0CA6CA">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:TextBox ID="txt_size1" runat="server" Text='<%#Eval("chlnSize") %>' CssClass="textbox textbox1 txtheight"
                                                                            onfocus="return myFunction2(this)" Width="20px" MaxLength="1"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="filterextender11" runat="server" TargetControlID="txt_size1"
                                                                            FilterType="Numbers,custom" ValidChars="1,2,3,4,5,6">
                                                                        </asp:FilteredTextBoxExtender>
                                                                    </center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Voucher Acronym" HeaderStyle-BackColor="#0CA6CA">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txt_acronym2" runat="server" Text='<%#Eval("voucAcr") %>' CssClass="textbox textbox1 stNew txtheight"
                                                                        onfocus="return myFunction2(this)" MaxLength="20"></asp:TextBox>
                                                                    <%-- <asp:FilteredTextBoxExtender ID="txtextacr" runat="server" TargetControlID="txt_acronym"
                        FilterType="UppercaseLetters,LowercaseLetters" ValidChars=" ">
                    </asp:FilteredTextBoxExtender>--%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Voucher Start No" HeaderStyle-BackColor="#0CA6CA">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txt_startno2" runat="server" Text='<%#Eval("voucStno") %>' CssClass="textbox textbox1 txtheight"
                                                                        onfocus="return myFunction2(this)" MaxLength="8"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="filterextender1211" runat="server" TargetControlID="txt_startno2"
                                                                        FilterType="Numbers" ValidChars=" ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Voucher Size" HeaderStyle-BackColor="#0CA6CA">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:TextBox ID="txt_size2" runat="server" Text='<%#Eval("voucSize") %>' CssClass="textbox textbox1 txtheight"
                                                                            onfocus="return myFunction2(this)" Width="20px" MaxLength="1"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="filterextender111" runat="server" TargetControlID="txt_size2"
                                                                            FilterType="Numbers,custom" ValidChars="1,2,3,4,5,6">
                                                                        </asp:FilteredTextBoxExtender>
                                                                    </center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                                <asp:Button ID="btnUpHeader" runat="server" Text="Update" OnClick="btnUpHeader_OnCLick"
                                                    CssClass="textbox textbox1" OnClientClick="return checkUpdate();" />
                                                <asp:Button ID="btnDelHeader" runat="server" Text="Delete" OnClick="btnDelHeader_OnCLick"
                                                    CssClass="textbox textbox1" OnClientClick="return CheckOK();" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
            <%--Delete Confirmation Popup --%>
            <center>
                <div id="suredivDelete" runat="server" visible="false" style="height: 100em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_sureDel" runat="server" Text="Do You Want To Delete Selected Headers?"
                                                Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_sureyesDel" CssClass=" textbox textbox1 btn1 " Style="height: 28px;
                                                    width: 65px;" OnClick="btn_sureyesDel_Click" Text="yes" runat="server" />
                                                <asp:Button ID="btn_surenoDel" CssClass=" textbox textbox1 btn1 " Style="height: 28px;
                                                    width: 65px;" OnClick="btn_surenoDel_Click" Text="no" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
            <%--Update Confirmation Popup --%>
            <center>
                <div id="suredivUpdate" runat="server" visible="false" style="height: 100em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div4" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="Label1" runat="server" Text="Do You Want To Update Selected Headers?"
                                                Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_sureyesUpd" CssClass=" textbox textbox1 btn1 " Style="height: 28px;
                                                    width: 65px;" OnClick="btn_sureyesUpd_Click" Text="yes" runat="server" />
                                                <asp:Button ID="btn_surenoUpd" CssClass=" textbox textbox1 btn1 " Style="height: 28px;
                                                    width: 65px;" OnClick="btn_surenoUpd_Click" Text="no" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
            <div id="imgdiv2" runat="server" visible="false" class="popupstyle" style="height: 50em;">
                <center>
                    <div id="panel_erralert" runat="server" class="table" style="background-color: White;
                        height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        margin-top: 200px; border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_alerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_errclose" CssClass=" textbox btn2 comm" OnClick="btn_errclose_Click"
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
    </body>
    </html>
</asp:Content>
