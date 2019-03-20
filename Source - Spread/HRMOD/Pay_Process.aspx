<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="Pay_Process.aspx.cs" Inherits="Pay_Process" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <%--<script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>--%>
    <script type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
    <style>
        body
        {
            font-family: Book Antiqua;
            font-size: 14px;
        }
        .myLink:hover
        {
            color: Red;
        }
    </style>
    <body>
        <script type="text/javascript">
            function onchk() {
                var txtid = document.getElementById("<%=txt_fpf.ClientID %>").value;
                if (txtid.trim() != "") {
                    if (parseFloat(txtid) > 100) {
                        document.getElementById('<%=txt_fpf.ClientID %>').value = "";
                    }
                }
            }

            function onchang() {
                var txtid = document.getElementById("<%=txt_totaldays.ClientID %>").value;
                if (txtid.trim() != "") {
                    if (parseFloat(txtid) > 31) {
                        document.getElementById('<%=txt_totaldays.ClientID %>').value = "";
                        alert("Days Exceed From Month!");
                    }
                }
            }

            function ValidDays(id) {
                var idVal = id.value;
                var ex = /^[0-9]+\.?[0-9]*$/;
                if (ex.test(idVal) == false) {
                    id.value = "";
                }
                if (parseFloat(id.value) > 31) {
                    id.value = "";
                    alert("Days Exceed From Month!");
                }
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <center>
                    <br />
                    <div>
                        <span class="fontstyleheader" style="color: Green">Pay Process Calculation </span>
                    </div>
                </center>
                <center>
                    <div class="maindivstyle" style="height: 520px; width: 1000px;">
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
                                <%-- <td>
                                    <asp:Label ID="lbl_fromyear" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Text="From Year"></asp:Label>
                                    <asp:DropDownList ID="ddl_fromyr" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        CssClass="textbox1 ddlheight1">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_toyear" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Text="To Year"></asp:Label>
                                    <asp:DropDownList ID="ddl_toyr" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        CssClass="textbox1 ddlheight1">
                                    </asp:DropDownList>
                                </td>--%>
                                <td>
                                    <asp:RadioButton ID="rdb_common1" Visible="false" Text="Common" runat="server" AutoPostBack="true"
                                        GroupName="co1" OnCheckedChanged="rdb_common1_CheckedChange" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_indivual1" Text="Individual" runat="server" Visible="false"
                                        AutoPostBack="true" GroupName="co1" OnCheckedChanged="rdb_rdb_indivual1_CheckedChange" />
                                </td>
                                <td>
                                    <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Font-Bold="true" Font-Names="Book Antiqua"
                                        Text="Go" OnClick="btn_go_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                        <br />
                        <FarPoint:FpSpread ID="Fpspread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Width="665px" Style="height: 380px; overflow: auto; background-color: White;"
                            OnButtonCommand="btnType_Click" ShowHeaderSelection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <br />
                    </div>
                </center>
                <center>
                    <div id="divshow" runat="server" visible="false" style="height: 39em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 30px;
                        left: 0;">
                        <asp:ImageButton ID="imgshow" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 16px; margin-left: 463px;"
                            OnClick="imgshow_Click" />
                        <br />
                        <div class="subdivstyle" style="background-color: White; overflow: auto; width: 970px;
                            height: 750px;" align="center">
                            <br />
                            <center>
                                <div>
                                    <span class="fontstyleheader" style="color: Green">Individual Pay Process Calculation
                                    </span>
                                </div>
                            </center>
                            <br />
                            <div align="left" class="spreadborder" style="overflow: auto; width: 880px; height: 600px;
                                border-radius: 10px; border: 1px solid Gray;">
                                <center>
                                    <br />
                                    <center>
                                        <asp:Label ID="lblshow" runat="server" Text="" Font-Bold="true" Font-Size="Large"
                                            ForeColor="Green"></asp:Label>
                                    </center>
                                    <br />
                                    <table id="Table1" class="maintablestyle" runat="server">
                                        <tr>
                                            <td>
                                                Department
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="upddept" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_dept" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                                            Style="width: 135px;">--Select--</asp:TextBox>
                                                        <asp:Panel ID="p1" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                                            border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                                            box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                                            <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" OnCheckedChanged="cb_dept_CheckedChange"
                                                                AutoPostBack="true" />
                                                            <asp:CheckBoxList ID="cbl_dept" runat="server" OnSelectedIndexChanged="cbl_dept_SelectedIndexChange"
                                                                AutoPostBack="true">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_dept"
                                                            PopupControlID="p1" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                Designation
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtDesignation" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                            Style="width: 135px;">--Select--</asp:TextBox>
                                                        <asp:Panel ID="Panel3" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                            Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                                            position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                                            height: 200px;">
                                                            <asp:CheckBox ID="CbDesignation" runat="server" Text="Select All" OnCheckedChanged="CbDesignCheckedChange"
                                                                AutoPostBack="true" />
                                                            <asp:CheckBoxList ID="CblDesignation" runat="server" OnSelectedIndexChanged="CblDesignationSelectedIndexChange"
                                                                AutoPostBack="true">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txtDesignation"
                                                            PopupControlID="Panel3" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                Staff Type
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_stype" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                                            Style="width: 135px; margin-left: 0px;">--Select--</asp:TextBox>
                                                        <asp:Panel ID="P4" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                                            border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                                            box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                                            <asp:CheckBox ID="cb_stype" runat="server" Text="Select All" OnCheckedChanged="cb_stype_CheckedChange"
                                                                AutoPostBack="true" />
                                                            <asp:CheckBoxList ID="cbl_stype" runat="server" OnSelectedIndexChanged="cbl_stype_SelectedIndexChange"
                                                                AutoPostBack="true">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_stype"
                                                            PopupControlID="P4" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Staff Category
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_scat" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                                            Style="width: 135px; margin-left: 0px;">--Select--</asp:TextBox>
                                                        <asp:Panel ID="Panel1" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                            Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                                            position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                                            height: 200px;">
                                                            <asp:CheckBox ID="cb_scat" runat="server" Text="Select All" OnCheckedChanged="cb_scat_CheckedChange"
                                                                AutoPostBack="true" />
                                                            <asp:CheckBoxList ID="cbl_scat" runat="server" OnSelectedIndexChanged="cbl_scat_SelectedIndexChange"
                                                                AutoPostBack="true">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_scat"
                                                            PopupControlID="Panel1" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                Staff Name/ Staff Code
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_staffNameCode" runat="server" CssClass="textbox1 ddlheight2"
                                                    OnSelectedIndexChanged="ddl_staffNameCode_change" AutoPostBack="true" Font-Bold="true"
                                                    Font-Names="Book Antiqua" Font-Size="Medium">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStaffName" runat="server" MaxLength="100" AutoPostBack="true"
                                                    Visible="false" CssClass="textbox txtheight2 txtcapitalize" Style="width: 150px;
                                                    font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtStaffName"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="txtsearchpan">
                                                </asp:AutoCompleteExtender>
                                                <asp:TextBox ID="txtStaffcode" runat="server" MaxLength="100" AutoPostBack="true"
                                                    Visible="false" CssClass="textbox txtheight2 txtcapitalize" Style="width: 150px;
                                                    font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtStaffcode"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="txtsearchpan">
                                                </asp:AutoCompleteExtender>
                                                <%--<asp:UpdatePanel ID="UpdatePanel7" runat="server">delsi
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtStaffName" Width=" 135px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                                        <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                                            <asp:CheckBox ID="cbStaff_Name" runat="server" Text="Select All" AutoPostBack="True"
                                                                OnCheckedChanged="cbStaff_NameCheckedChanged" />
                                                            <asp:CheckBoxList ID="cblStaff_Name" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblstaffname_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txtStaffName"
                                                            PopupControlID="Panel4" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>--%>
                                            </td>
                                            <%--<td>
                                                Staff Code
                                            </td>--%>
                                            <%--<td>--%>
                                            <%--<asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtStaffcode" runat="server" CssClass="textbox txtheight1" ReadOnly="true"
                                                            Style="width: 120px;">--Select--</asp:TextBox>
                                                        <asp:Panel ID="Panel5" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                            Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                                            position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                                            height: 200px; width: 200px;">
                                                            <asp:CheckBox ID="cbStaff_Code" runat="server" Text="Select All" OnCheckedChanged="cbStaff_CodeCheckedChange"
                                                                AutoPostBack="true" />
                                                            <asp:CheckBoxList ID="cblStaff_Code" runat="server" OnSelectedIndexChanged="cblStaff_CodeSelectedIndexChange"
                                                                AutoPostBack="true">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender13" runat="server" TargetControlID="txtStaffcode"
                                                            PopupControlID="Panel5" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>--%>
                                            <%--</td>--%>
                                            <td>
                                                <asp:Button ID="btnshow" runat="server" Text="GO" CssClass="textbox1 btn2" OnClick="btnshow_click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <asp:Label ID="lblshowerr" runat="server" Text="" Visible="false" ForeColor="Red"
                                        Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                    <br />
                                    <br />
                                    <FarPoint:FpSpread ID="fpspreadshow" runat="server" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="1px" Width="800px" CssClass="spreadborder" Style="height: 380px;
                                        overflow: auto; background-color: White;" OnButtonCommand="fpspreadshow_Command"
                                        ShowHeaderSelection="false">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </center>
                            </div>
                            <br />
                            <asp:Button ID="btnindgen" runat="server" Text="Generate" Visible="false" CssClass="textbox1 btn2"
                                OnClick="btnindgen_click" />
                            <asp:Button ID="btninddel" runat="server" Text="Delete" Visible="false" CssClass="textbox1 btn2"
                                OnClick="btninddel_click" />
                            <asp:Button ID="btnindexit" runat="server" Text="Exit" CssClass="textbox1 btn2" OnClick="btnindexit_click" />
                        </div>
                    </div>
                </center>
                <center>
                    <div id="Div2" runat="server" visible="false" style="height: 39em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 70px;
                        left: 0;">
                        <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 16px; margin-left: 420px;"
                            OnClick="imagebtnpopclose_addnew_Click" />
                        <br />
                        <div class="subdivstyle" style="background-color: White; overflow: auto; width: 883px;
                            height: 559px;" align="center">
                            <br />
                            <%--  <center>
                                <div>
                                    <span class="fontstyleheader" style="color: Green">Pay Process Calculation </span>
                                </div>
                            </center>--%>
                            <center>
                                <asp:Label ID="lbl_showmonyear" runat="server" Text="" Font-Bold="true" Font-Size="Large"
                                    ForeColor="Green"></asp:Label>
                            </center>
                            <br />
                            <div align="left" style="overflow: auto; width: 793px; height: 414px; border-radius: 10px;
                                border: 1px solid Gray;" class="spreadborder">
                                <center>
                                    <br />
                                    <table id="tblcommon" runat="server">
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkdept" runat="server" Text="Department" AutoPostBack="true" OnCheckedChanged="chkdept_change" />
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="upddeptcom" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtdeptcom" runat="server" ReadOnly="true" Enabled="false" CssClass="textbox txtheight2"
                                                            Style="width: 135px;">--Select--</asp:TextBox>
                                                        <asp:Panel ID="pnldeptcom" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                            Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                                            position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                                            height: 200px;">
                                                            <asp:CheckBox ID="cbdeptcom" runat="server" Text="Select All" OnCheckedChanged="cbdeptcom_CheckedChange"
                                                                AutoPostBack="true" />
                                                            <asp:CheckBoxList ID="cbldeptcom" runat="server" OnSelectedIndexChanged="cbldeptcom_SelectedIndexChange"
                                                                AutoPostBack="true">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtdeptcom"
                                                            PopupControlID="pnldeptcom" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cbDesig" runat="server" Text="Designation" AutoPostBack="true"
                                                    OnCheckedChanged="cbDesigChange" Checked="false" />
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_desig" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                            Style="width: 135px;">--Select--</asp:TextBox>
                                                        <asp:Panel ID="P2" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                                            border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                                            box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                                            <asp:CheckBox ID="cb_desig" runat="server" Text="Select All" OnCheckedChanged="cb_desig_CheckedChange"
                                                                AutoPostBack="true" />
                                                            <asp:CheckBoxList ID="cbl_desig" runat="server" OnSelectedIndexChanged="cbl_desig_SelectedIndexChange"
                                                                AutoPostBack="true">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_desig"
                                                            PopupControlID="P2" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkstftype" runat="server" Text="Staff Type" AutoPostBack="true"
                                                    OnCheckedChanged="chkstftype_change" />
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="updstfcom" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtstftypecom" runat="server" ReadOnly="true" Enabled="false" CssClass="textbox txtheight2"
                                                            Style="width: 125px; margin-left: 0px;">--Select--</asp:TextBox>
                                                        <asp:Panel ID="pnlstftypecom" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                            Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                                            position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                                            height: 200px;">
                                                            <asp:CheckBox ID="cbstftypecom" runat="server" Text="Select All" OnCheckedChanged="cbstftypecom_CheckedChange"
                                                                AutoPostBack="true" />
                                                            <asp:CheckBoxList ID="cblstftypecom" runat="server" OnSelectedIndexChanged="cblstftypecom_SelectedIndexChange"
                                                                AutoPostBack="true">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtstftypecom"
                                                            PopupControlID="pnlstftypecom" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkstfcat" runat="server" Text="Staff Category" AutoPostBack="true"
                                                    OnCheckedChanged="chkstfcat_change" />
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="updstfcatcom" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtscatcom" runat="server" ReadOnly="true" Enabled="false" CssClass="textbox txtheight2"
                                                            Style="width: 135px; margin-left: 0px;">--Select--</asp:TextBox>
                                                        <asp:Panel ID="pnlscatcom" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                            Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                                            position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                                            height: 200px;">
                                                            <asp:CheckBox ID="cbscatcom" runat="server" Text="Select All" OnCheckedChanged="cbscatcom_CheckedChange"
                                                                AutoPostBack="true" />
                                                            <asp:CheckBoxList ID="cblscatcom" runat="server" OnSelectedIndexChanged="cblscatcom_SelectedIndexChange"
                                                                AutoPostBack="true">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txtscatcom"
                                                            PopupControlID="pnlscatcom" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <%--<td>
                                                <asp:CheckBox ID="cbStaffName" runat="server" Text="Staff Name" AutoPostBack="true"
                                                    OnCheckedChanged="cbStaffNameOnchange" />
                                            </td>--%>
                                            <td>
                                                <asp:DropDownList ID="ddlsearchappstf" runat="server" CssClass="textbox1 ddlheight2"
                                                    OnSelectedIndexChanged="ddlsearchappstf_change" AutoPostBack="true" Font-Bold="true"
                                                    Font-Names="Book Antiqua" Font-Size="Medium">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_staffname" runat="server" MaxLength="100" AutoPostBack="true"
                                                    OnTextChanged="txt_staffname_change" CssClass="textbox txtheight2 txtcapitalize"
                                                    Style="width: 137px; font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender13" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffname"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="txtsearchpan">
                                                </asp:AutoCompleteExtender>
                                                <asp:TextBox ID="txt_StaffCode" runat="server" MaxLength="100" AutoPostBack="true"
                                                    OnTextChanged="txt_staffcode_change" CssClass="textbox txtheight2 txtcapitalize"
                                                    Style="width: 137px; font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_StaffCode"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="txtsearchpan">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lnkview" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    ForeColor="Blue" CausesValidation="False" OnClick="view_click">View Details</asp:LinkButton>
                                            </td>
                                            <%--<td>
                                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_staffname" Width=" 135px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                                        <asp:Panel ID="Panel_staffname" runat="server" CssClass="multxtpanel" Height="200px"
                                                            Width="200px">
                                                            <asp:CheckBox ID="cb_staffname" runat="server" Text="Select All" AutoPostBack="True"
                                                                OnCheckedChanged="cb_staffname_CheckedChanged" />
                                                            <asp:CheckBoxList ID="cbl_staffname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_staffname_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txt_staffname"
                                                            PopupControlID="Panel_staffname" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>--%>
                                            <%--<td>
                                                <asp:CheckBox ID="Cb_StaffCode" runat="server" Text="Staff Code" AutoPostBack="true"
                                                    OnCheckedChanged="Cb_StaffCodeOnchange" />
                                            </td>--%>
                                            <%--<td>
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_StaffCode" runat="server" CssClass="textbox txtheight1" ReadOnly="true"
                                                            Style="width: 120px;">--Select--</asp:TextBox>
                                                        <asp:Panel ID="Panel2" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                            Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                                            position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                                            height: 200px; width: 200px;">
                                                            <asp:CheckBox ID="CbStaffCode" runat="server" Text="Select All" OnCheckedChanged="CbStaffCodeCheckedChange"
                                                                AutoPostBack="true" />
                                                            <asp:CheckBoxList ID="CblStaffCode" runat="server" OnSelectedIndexChanged="CblStaffCodeSelectedIndexChange"
                                                                AutoPostBack="true">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_StaffCode"
                                                            PopupControlID="Panel2" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>--%>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cb_lopfrom_atn" runat="server" Text="LOP From Attendance" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cb_lop_fromgross" runat="server" Text="LOP From Gross" AutoPostBack="true"
                                                    OnCheckedChanged="cb_lop_fromgross_CheckedChanged" />
                                            </td>
                                            <td colspan="3">
                                                <div align="left" style="overflow: auto; width: 450px; height: 50px; border-radius: 3px;
                                                    border: 1px solid Gray;" class="spreadborder">
                                                    <asp:CheckBox ID="cb_Lopfrom_basic" runat="server" Text="LOP From Basic &nbsp &nbsp   "
                                                        AutoPostBack="true" OnCheckedChanged="cb_Lopfrom_basic_CheckedChanged" />
                                                    <asp:CheckBox ID="cb_Lopfrom_payband" runat="server" Text="LOP from Payband &nbsp &nbsp"
                                                        AutoPostBack="true" OnCheckedChanged="cb_Lopfrom_payband_CheckedChanged" />
                                                    <asp:CheckBox ID="cb_lopgradpay" runat="server" Text="LOP from Gradepay" AutoPostBack="true"
                                                        OnCheckedChanged="cb_lopgradpay_CheckedChanged" />
                                                    <%--poomalar 25.10.17--%>
                                                    <asp:CheckBox ID="cb_ptgrosslop" runat="server" Text="PT GrossLOP" AutoPostBack="true"
                                                        OnCheckedChanged="cb_ptgrosslop_CheckedChanged" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_Totalsalary" runat="server" Text="Total Salary Days"></asp:Label>
                                            </td>
                                            <td>
                                                <div align="left" style="overflow: auto; width: 250px; height: 30px; border-radius: 3px;
                                                    border: 1px solid Gray;" class="spreadborder">
                                                    <asp:RadioButton ID="rdb_month" Text="Month Wise" GroupName="month" OnCheckedChanged="rdb_month_CheckedChanged"
                                                        Checked="true" runat="server" AutoPostBack="true" />
                                                    <asp:RadioButton ID="rdb_Days" Text="Days" GroupName="month" OnCheckedChanged="rdb_Days_CheckedChanged"
                                                        runat="server" AutoPostBack="true" />
                                                </div>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_totaldays" Enabled="false" Width=" 46px" runat="server" MaxLength="2"
                                                    onkeyup="return onchang()" onblur="return onchang()" CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_totaldays"
                                                    FilterType="Numbers">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cb_absentcalculation" runat="server" AutoPostBack="true" OnCheckedChanged="cb_absentcalculation_CheckedChanged"
                                                    Text="Absent Calculation" />
                                                <asp:TextBox ID="txt_absent" Width=" 36px" Enabled="false" MaxLength="5" runat="server"
                                                    CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_absent"
                                                    FilterType="Numbers,custom" ValidChars=" .">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td colspan="3">
                                                <asp:CheckBox ID="cb_NA_RL" runat="server" AutoPostBack="true" Text="For NA & RL take attendance from current month" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cb_LOP_to_PF" runat="server" Text="LOP & PF for greater then "
                                                    AutoPostBack="true" OnCheckedChanged="cb_LOP_to_PF_CheckedChanged" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_days" Width=" 46px" runat="server" Enabled="false" MaxLength="3"
                                                    CssClass="textbox  textbox1  txtheight2"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_days"
                                                    FilterType="Numbers">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:Label ID="lbl_days" runat="server" Text=" Days"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chk_fpf" runat="server" AutoPostBack="true" OnCheckedChanged="chk_fpf_CheckedChanged" />
                                                <asp:Label ID="lbl_FPF" runat="server" Text=" FPF Calculation %"></asp:Label>
                                                <asp:TextBox ID="txt_fpf" Width=" 36px" runat="server" onkeyup="return onchk()" onblur="return onchk()"
                                                    Enabled="false" MaxLength="3" CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_fpf"
                                                    FilterType="Numbers,custom" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td colspan="3">
                                                <asp:Label ID="lbl_age_less" runat="server" Text=" Age Less than "></asp:Label>
                                                <asp:TextBox ID="txt_age_val" Width=" 36px" runat="server" Enabled="false" MaxLength="2"
                                                    CssClass="textbox textbox1 txtheight2"></asp:TextBox>&nbsp &nbsp
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_age_val"
                                                    FilterType="Numbers,custom" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:Label ID="lbl_max_amount" runat="server" Text=" Maximum Amount"></asp:Label>
                                                <asp:TextBox ID="txt_max_amount" Width=" 136px" runat="server" Enabled="false" MaxLength="12"
                                                    CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txt_max_amount"
                                                    FilterType="Numbers,custom" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cb_auto_deduct" runat="server" Text=" Auto Deduct" />
                                            </td>
                                            <td colspan="2">
                                                <asp:CheckBox ID="cb_unpaid_leave" runat="server" Text=" For Unpaid Leave take attendence from current month" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cb_max_PER" runat="server" Text=" Maximum PER/LA" AutoPostBack="true"
                                                    OnCheckedChanged="cb_max_PER_CheckedChanged" />
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="lbl_after" runat="server" Text=" After"></asp:Label>
                                                <asp:TextBox ID="txt_after" Width=" 61px" Enabled="false" runat="server" MaxLength="3"
                                                    CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txt_after"
                                                    FilterType="Numbers">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:Label ID="lbl_PER_LA" runat="server" Text=" (PER &LA) Calculate LOP"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cb_formulate_days" runat="server" Enabled="false" AutoPostBack="true"
                                                    OnCheckedChanged="cb_formulate_days_CheckedChanged" Text=" For multiple days" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cbincitcalc" runat="server" Text="Include IT Calculation" />
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lbLeaveSettings" runat="server" Text="Leave Settings" Font-Bold="true"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" CssClass="myLink" OnClick="lbLeave_Click"></asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td colspan="3">
                                                <asp:GridView ID="grid_multiple_days" runat="server" Visible="false" GridLines="Both"
                                                    AutoGenerateColumns="false" OnDataBound="grid_multiple_days_DataBound" Width="100px"
                                                    ShowHeaderWhenEmpty="true">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="From" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="300px">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:TextBox ID="txt_from" runat="server" onblur="return addmarkss(this)" CssClass="  textbox txtheight1"
                                                                        Height="17px" OnTextChanged="txt_from_OnTextChanged" Width="30px" Text='<%#Eval("From") %>'></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_from"
                                                                        FilterType="Numbers" ValidChars=" ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </center>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="To" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="300px">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:TextBox ID="txt_To" runat="server" onblur="return addmarkss(this)" CssClass="  textbox txtheight1"
                                                                        Height="17px" OnTextChanged="txt_To_OnTextChanged" Width="30px" Text='<%#Eval("To") %>'></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_To"
                                                                        FilterType="Numbers" ValidChars=" ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </center>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="LOP Days" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="300px">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:TextBox ID="txt_LOP_days" runat="server" onblur="return addmarkss(this)" CssClass="  textbox txtheight1"
                                                                        Height="17px" OnTextChanged="txt_LOP_days_OnTextChanged" Width="30px" Text='<%#Eval("LOPDays") %>'></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_LOP_days"
                                                                        FilterType="Numbers" ValidChars=" ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </center>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="right" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="ChkHourWise" runat="server" Text="Hour Wise Attendance" OnCheckedChanged="ChkHourWise_Change"
                                                    AutoPostBack="true" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblStfCat" runat="server" Visible="false" Font-Names="Book Antiqua"
                                                    Text="Staff Category"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel1" Visible="false" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtHrStfCat" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                                            Style="width: 135px; left: -190px; position: relative; font-family: book antiqua;
                                                            font-size: medium;">--Select--</asp:TextBox>
                                                        <asp:Panel ID="pnlHrStfCat" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                            Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                                            position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                                            height: 200px;">
                                                            <asp:CheckBox ID="cbHrStfCat" runat="server" Text="Select All" OnCheckedChanged="cbHrStfCat_CheckedChange"
                                                                AutoPostBack="true" />
                                                            <asp:CheckBoxList ID="cblHrStfCat" runat="server" OnSelectedIndexChanged="cblHrStfCat_SelectedIndexChange"
                                                                AutoPostBack="true">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txtHrStfCat"
                                                            PopupControlID="pnlHrStfCat" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <%--poomalar 23.10.17--%>
                                            <td>
                                                <asp:CheckBox ID="chk_desigwise" runat="server" Text="Designation Wise" OnCheckedChanged="ChkdesigWise_Change"
                                                    AutoPostBack="true" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chk_staffwise" runat="server" Text="Staff Wise" OnCheckedChanged="ChkstaffWise_Change"
                                                    AutoPostBack="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chk_CLCalc" runat="server" Text="CL Calculation" Checked="false"
                                                    OnCheckedChanged="chk_CLCalc_Change" AutoPostBack="true" />
                                            </td>
                                            <td id="tdFrmYr" runat="server" visible="false">
                                                From Month Year
                                                <asp:DropDownList ID="ddlFromMonth" runat="server" CssClass="textbox1 ddlheight"
                                                    OnSelectedIndexChanged="ddlFromMonth_Change" AutoPostBack="true">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlFromYear" runat="server" OnSelectedIndexChanged="ddlfromyear_selectchange"
                                                    AutoPostBack="true" CssClass="textbox1 ddlheight">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="tdToYr" runat="server" visible="false">
                                                To Month Year
                                                <asp:DropDownList ID="ddlToMonth" runat="server" CssClass="textbox1 ddlheight" OnSelectedIndexChanged="ddlToMonth_Change"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlToYear" runat="server" CssClass="textbox1 ddlheight">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cb_pfCalculation" runat="server" Text="PF/EPF With LOP" AutoPostBack="true"
                                                    OnCheckedChanged="cb_pfCalculation_CheckedChanged" />
                                            </td>
                                            <td colspan="">
                                                <asp:CheckBox ID="cb_permonth" runat="server" Text="Lop and Gross Zero for LOP Staff" Checked="false"
                                                    AutoPostBack="true" OnCheckedChanged="cb_permonth_CheckedChanged" />
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtloppermonth" runat="server" ReadOnly="true" Enabled="false" CssClass="textbox txtheight2"
                                                            Style="width: 135px; margin-left: -45px;">--Select--</asp:TextBox>
                                                        <asp:Panel ID="Pnlloppermonth" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                            Style="background: White; border-color: Gray; border-style: Solid; width:135px; border-width: 2px;
                                                            position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                                            height: 200px;">
                                                            <asp:CheckBox ID="cb_loppermonth" runat="server" Text="Select All" OnCheckedChanged="cb_loppermonth_CheckedChange"
                                                                AutoPostBack="true" />
                                                            <asp:CheckBoxList ID="cbl_loppermonth" runat="server" OnSelectedIndexChanged="cb_loppermonth_SelectedIndexChange"
                                                                AutoPostBack="true">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txtloppermonth"
                                                            PopupControlID="Pnlloppermonth" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                                <br />
                            </div>
                            <br />
                            <center>
                                <asp:Button ID="btn_Generate" runat="server" CssClass="textbox textbox1 btn2" Text="Generate"
                                    OnClick="btn_Generate_Click" />
                                <asp:Button ID="btn_addnew_exit" runat="server" CssClass="textbox textbox1 btn2"
                                    Text="Exit" OnClick="btn_addnew_exit_Click" />
                                <asp:Button ID="Button1" runat="server" Visible="false" CssClass="textbox textbox1 btn2"
                                    Text="select" OnClick="Button1_Click" />
                            </center>
                        </div>
                    </div>
                </center>
            </div>
            <%--delsi1004--%>
            <center>
                <div id="divview" runat="server" visible="false" style="height: 120em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="div1" runat="server" class="table" style="background-color: White; height: 500px;
                            width: 840px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 30px;
                            border-radius: 10px;">
                            <asp:ImageButton ID="imgbtn3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                Style="height: 30px; width: 30px; position: absolute; margin-top: -32px; margin-left: 400px;"
                                OnClick="imagebtnorder_Click" />
                            <center>
                                <center>
                                    <span class="fontstyleheader" style="color: Green;">Staff Details</span>
                                </center>
                                <center>
                                    <div id="staffdetail" runat="server" visible="false" style="width: 820px; height: 620px;">
                                        <center>
                                            <center>
                                                <asp:Label ID="lblerr1" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                                <FarPoint:FpSpread ID="FpSpreadstaff" runat="server" Visible="false" Style="position: relative;"
                                                    ShowHeaderSelection="false" ActiveSheetViewIndex="0" OnUpdateCommand="FpSpreadstaff_Command">
                                                    <Sheets>
                                                        <FarPoint:SheetView SheetName="Sheet1">
                                                        </FarPoint:SheetView>
                                                    </Sheets>
                                                </FarPoint:FpSpread>
                                            </center>
                                            <br />
                                            <div id="Div3" runat="server" visible="true">
                                                <asp:Button ID="btnok1" runat="server" OnClick="btnok1_Click" CssClass="textbox textbox1 btn2"
                                                    Text="Ok" />
                                                <asp:Button ID="btnexitstud" runat="server" OnClick="btnexitstud_Click" CssClass="textbox textbox1 btn2"
                                                    Text="Exit" />
                                            </div>
                                        </center>
                                    </div>
                                </center>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
            <center>
                <div id="divLeaveSet" runat="server" visible="false" style="height: 70em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <asp:ImageButton ID="imgLeaveclose" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 198px; margin-left: 203px;"
                        OnClick="imgLeaveclose_Click" />
                    <center>
                        <div id="divLev" runat="server" class="table" style="background-color: White; height: auto;
                            width: 450px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <br />
                                <span class="fontstyleheader" style="color: Green;">Leave Settings</span>
                                <br />
                                <table style="height: auto; width: 100%">
                                    <tr>
                                        <td>
                                            Permission Days
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPer" runat="server" MaxLength="3" onblur="ValidDays(this);" onkeyup="ValidDays(this);"
                                                CssClass="textbox textbox1 txtheight" Width="50px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="filterPer" runat="server" FilterType="Custom,Numbers"
                                                ValidChars="." TargetControlID="txtPer">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            Leave Type
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlLeaveType" runat="server" CssClass="textbox1 ddlheight">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Late Days
                                        </td>
                                        <td colspan="2">
                                            <asp:GridView ID="grdLateDays" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                                Width="150px" ShowHeaderWhenEmpty="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:Label ID="lblSno" runat="server" CssClass="grid_view_lnk_button" Text='<%#Container.DisplayIndex+1 %>'>
                                                                </asp:Label>
                                                            </center>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Late Days" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:TextBox ID="txt_Latedays" runat="server" MaxLength="3" CssClass="textbox txtheight1"
                                                                    onblur="ValidDays(this);" onkeyup="ValidDays(this);" Height="17px" Width="50px"
                                                                    Text='<%#Eval("LateDays") %>'></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="filterLate" runat="server" TargetControlID="txt_Latedays"
                                                                    FilterType="Custom,Numbers" ValidChars=".">
                                                                </asp:FilteredTextBoxExtender>
                                                            </center>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="LOP Days" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:TextBox ID="txt_LOPdays" runat="server" MaxLength="3" CssClass="textbox txtheight1"
                                                                    onblur="ValidDays(this);" onkeyup="ValidDays(this);" Height="17px" Width="50px"
                                                                    Text='<%#Eval("LOPDays") %>'></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="filterLOP" runat="server" TargetControlID="txt_LOPdays"
                                                                    FilterType="Custom,Numbers" ValidChars=".">
                                                                </asp:FilteredTextBoxExtender>
                                                            </center>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnAddRows" runat="server" Text="Add" Width="50px" CssClass="textbox1 btn1"
                                                OnClick="btnAddRows_Click" />
                                            <asp:Button ID="btnRemoveRows" runat="server" Text="Remove" CssClass="textbox1 btn2"
                                                OnClick="btnRemoveRows_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Absent Days
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtAbs_Days" runat="server" MaxLength="3" onblur="ValidDays(this);"
                                                onkeyup="ValidDays(this);" CssClass="textbox textbox1 txtheight" Width="50px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" FilterType="Custom,Numbers"
                                                ValidChars="." TargetControlID="txtAbs_Days">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:CheckBox ID="chkIncAllLop" runat="server" Text="Include All LOP" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <asp:Button ID="btnGrdsave" runat="server" Text="Save" CssClass="textbox1 btn2" OnClick="btnGrdsave_Click" />
                                <asp:Button ID="btnGrdExt" runat="server" Text="Exit" CssClass="textbox1 btn2" OnClick="btnGrdExt_Click" />
                                <br />
                                <br />
                            </center>
                        </div>
                    </center>
                </div>
                <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: auto;
                            width: 380px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <br />
                                <table style="height: auto; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" OnClick="btnerrclose_Click"
                                                    Text="Ok" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
        </center>
    </body>
    </html>
</asp:Content>
