<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ScholarshipReport.aspx.cs" Inherits="ScholarshipReport" %>

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
            function cbColCheck() {
                var cbcol = document.getElementById('<%=cb_column.ClientID%>');
                var cblcol = document.getElementById('<%=cblcolumnorder.ClientID%>');
                var cbltag = cblcol.getElementsByTagName("input");
                if (cbcol.checked == true) {
                    for (var col = 0; col < cbltag.length; col++) {
                        cbltag[col].checked = true;
                    }
                }
                else {
                    for (var col = 0; col < cbltag.length; col++) {
                        cbltag[col].checked = false;
                    }
                }
            }
            function cblColCheck() {
                var count = 0;
                var cbcol = document.getElementById('<%=cb_column.ClientID%>');
                var cblcol = document.getElementById('<%=cblcolumnorder.ClientID%>');
                var cbltag = cblcol.getElementsByTagName("input");
                for (var i = 0; i < cbltag.length; i++) {
                    if (cbltag[i].checked == true) {
                        count += 1;
                    }
                }
                if (cbltag.length == count) {
                    cbcol.checked = true;
                }
                else {
                    cbcol.checked = false;
                }
            }

            function txtrollChange() {
                var rollno = document.getElementById('<%=txt_roll.ClientID%>');
                var name = document.getElementById('<%=txt_name.ClientID%>');
                if (rollno.value != "") {
                    name.value = "";
                }
            }

            function txtnameChange() {
                var rollno = document.getElementById('<%=txt_roll.ClientID%>');
                var name = document.getElementById('<%=txt_name.ClientID%>');
                if (name.value != "") {
                    rollno.value = "";
                }

            }
        </script>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Scholarship Report</span></div>
            </center>
        </div>
        <div>
            <center>
                <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                    <div>
                        <table>
                            <tr>
                                <td>
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
                                                <asp:Label ID="lbl_str1" runat="server" Text="Stream"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlstream" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlstream_OnSelectedIndexChanged"
                                                    CssClass="textbox  ddlheight" Style="width: 108px;">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                Batch
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
                                                        <asp:TextBox ID="txt_dept" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
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
                                                        <asp:TextBox ID="txt_sect" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                        <asp:Panel ID="panel_sect" runat="server" CssClass="multxtpanel" Style="width: 100px;
                                                            height: 100px;">
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
                                                        <asp:TextBox ID="txt_studhed" runat="server" Style="height: 20px; width: 100px;"
                                                            ReadOnly="true">--Select--</asp:TextBox>
                                                        <asp:Panel ID="pnl_studhed" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                            Style="width: 126px; height: 120px;">
                                                            <asp:CheckBox ID="chk_studhed" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                OnCheckedChanged="chk_studhed_OnCheckedChanged" />
                                                            <asp:CheckBoxList ID="chkl_studhed" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_studhed_OnSelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_studhed"
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
                                                        <asp:TextBox ID="txt_studled" runat="server" Style="height: 20px; width: 100px;"
                                                            ReadOnly="true">--Select--</asp:TextBox>
                                                        <asp:Panel ID="pnl_studled" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                            Style="width: 126px; height: 120px;">
                                                            <asp:CheckBox ID="chk_studled" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                OnCheckedChanged="chk_studled_OnCheckedChanged" />
                                                            <asp:CheckBoxList ID="chkl_studled" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_studled_OnSelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_studled"
                                                            PopupControlID="pnl_studled" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" Text="Type of Scholarship"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtschol" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                        <asp:Panel ID="pnlschol" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                            Style="width: 126px; height: 120px;">
                                                            <asp:CheckBox ID="cbschol" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                OnCheckedChanged="cbschol_OnCheckedChanged" />
                                                            <asp:CheckBoxList ID="cblschol" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblschol_OnSelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtschol"
                                                            PopupControlID="pnlschol" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="lblfyear" Text="Finance Year"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtfyear" Style="height: 20px; width: 150px;" CssClass="Dropdown_Txt_Box"
                                                            runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                                        <asp:Panel ID="Pfyear" runat="server" CssClass="multxtpanel" Width="200px">
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
                                            <td colspan="7">
                                                <fieldset style="margin-left: -1px; width: 240px; height: 20px; margin-top: 0px;">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                Student
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="rbl_rollno" runat="server" CssClass="textbox  ddlheight" Style="margin-top: -3px;
                                                                    margin-left: 5px;" AutoPostBack="true" OnSelectedIndexChanged="rbl_rollno_OnSelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_roll" runat="server" Style="margin-top: -3px; margin-left: 5px;
                                                                    height: 20px; width: 130px;" onchange="return txtrollChange()" placeholder="Search"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="ftext_rollno" runat="server" TargetControlID="txt_roll"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:AutoCompleteExtender ID="acext_rollno" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_roll"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="panelbackground">
                                                                </asp:AutoCompleteExtender>
                                                            </td>
                                                            <td>
                                                                Name
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_name" runat="server" CssClass="txtheight5 txtcaps" Style="width: 231px;"
                                                                    onchange="return txtnameChange()">
                                                                </asp:TextBox>
                                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_name"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="panelbackground">
                                                                </asp:AutoCompleteExtender>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" TargetControlID="txt_name"
                                                                    FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers" ValidChars=" .-@,">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnsearch" runat="server" Text="Search" CssClass="textbox btn2" OnClick="btnsearch_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="cbdeptacr" runat="server" Text="Department Acronym" />
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <asp:Label ID="lbloutput" runat="server" Visible="false"></asp:Label>
                                        <center>
                                            <div>
                                                <center>
                                                    <asp:Panel ID="pnlheader" runat="server" CssClass="cpHeader" Visible="false" Height="22px"
                                                        Width="146px" BackColor="#0CA6CA" Style="margin-top: -0.1%; margin-left: -853px;">
                                                        <asp:Label ID="lblcoltext" Text="Column Order" runat="server" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                                    </asp:Panel>
                                                </center>
                                            </div>
                                            <br />
                                            <div>
                                                <center>
                                                    <asp:Panel ID="pnlcolorder" runat="server" CssClass="maintablestyle" Visible="false"
                                                        Width="930px">
                                                        <div id="divcolumn" runat="server" style="height: 87px; width: 930px;">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="cb_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium" Text="Select All" onchange="return cbColCheck()" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" Width="850px"
                                                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                                                            RepeatDirection="Horizontal" onclick=" return cblColCheck()">
                                                                        </asp:CheckBoxList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </asp:Panel>
                                                </center>
                                                <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pnlcolorder"
                                                    CollapseControlID="pnlheader" ExpandControlID="pnlheader" Collapsed="true" TextLabelID="lblcoltext"
                                                    CollapsedSize="0" ImageControlID="Imagefilter">
                                                </asp:CollapsiblePanelExtender>
                                            </div>
                                            <br />
                                            <center>
                                                <div id="divspread" runat="server" visible="false" style="width: 900px; height: 450px;
                                                    background-color: White; border-radius: 10px;">
                                                    <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="true" BorderStyle="Solid"
                                                        BorderWidth="0px" Width="930px" Style="overflow: auto; border: 0px solid #999999;
                                                        border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                                        class="spreadborder" OnButtonCommand="FpSpread1_OnButtonCommand">
                                                        <Sheets>
                                                            <FarPoint:SheetView SheetName="sheet1">
                                                            </FarPoint:SheetView>
                                                        </Sheets>
                                                    </FarPoint:FpSpread>
                                                </div>
                                                <br />
                                            </center>
                                        </center>
                                    </div>
                                    <asp:Button ID="btnview" runat="server" Text="View" Visible="false" CssClass="textbox btn2"
                                        OnClick="btnview_Click" Style="margin-left: 450px;" Font-Bold="true" Font-Size="small" />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <div id="divdetail" runat="server" visible="false" style="width: 961px; overflow: auto;
                                            background-color: White; border-radius: 10px;">
                                            <FarPoint:FpSpread ID="FpSpread2" runat="server" Visible="true" BorderStyle="Solid"
                                                BorderWidth="0px" Width="930px" Style="overflow: auto; border: 0px solid #999999;
                                                border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                                class="spreadborder">
                                                <Sheets>
                                                    <FarPoint:SheetView SheetName="sheet1">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                            </FarPoint:FpSpread>
                                        </div>
                                        <br />
                                    </center>
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
                                            <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display()"
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
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </center>
        </div>
        <div>
            <center>
                <div id="error" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="diverror" runat="server" class="table" style="background-color: White; height: 120px;
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
        </div>
    </body>
    </html>
</asp:Content>
