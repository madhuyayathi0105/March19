<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="Inward_Entry.aspx.cs" EnableEventValidation="false"
    Inherits="LibraryMod_Inward_Entry" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style>
        .autoCompletePanel
        {
            font-family: Amudham;
            font-size: 8pt;
            color: #ffcccc;
            width: 400pt;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">
        document.onkeydown = checkKeycode
        var keycode;
        function checkKeycode(e) {
            if (window.event) keycode = window.event.keyCode;
            else if (e) keycode = e.which;
            if (keycode == "13") {
                var AccNo = document.getElementById("<%=txtsearch.ClientID%>").value;
                var focusedAccNo = document.getElementById("<%=txtsearch.ClientID%>")
                if (document.activeElement === focusedAccNo) {
                    document.getElementById("<%=Btngo.ClientID%>").click();
                }
            }
        }
        function frelig1() {
            document.getElementById('<%= Btncurrency .ClientID%>').style.display = 'block';
            document.getElementById('<%=btncurry.ClientID%>').style.display = 'block';

        }
        function frelig2() {
            document.getElementById('<%= btn_pls_status .ClientID%>').style.display = 'block';
            document.getElementById('<%=btn_min_status.ClientID%>').style.display = 'block';

        }
        function frelig3() {
            document.getElementById('<%= btn_pls_att .ClientID%>').style.display = 'block';
            document.getElementById('<%=btn_min_att.ClientID%>').style.display = 'block';

        }
        function frelig4() {
            document.getElementById('<%= btn_pls_lang .ClientID%>').style.display = 'block';
            document.getElementById('<%=btn_min_lang.ClientID%>').style.display = 'block';

        }
        function frelig5() {
            document.getElementById('<%= btn_pls_mat .ClientID%>').style.display = 'block';
            document.getElementById('<%=btn_min_mat.ClientID%>').style.display = 'block';

        }
        function frelig6() {
            document.getElementById('<%= btn_pl_currn .ClientID%>').style.display = 'block';
            document.getElementById('<%=btn_min_currn.ClientID%>').style.display = 'block';

        }
        function frelig7() {
            document.getElementById('<%= btn_pls_cat .ClientID%>').style.display = 'block';
            document.getElementById('<%=btn_min_cat.ClientID%>').style.display = 'block';

        }

        function frelig8() {
            document.getElementById('<%= btn_pls_bud .ClientID%>').style.display = 'block';
            document.getElementById('<%=btn_min_bud.ClientID%>').style.display = 'block';

        }
        function frelig9() {
            document.getElementById('<%= btn_pls_pubpl .ClientID%>').style.display = 'block';
            document.getElementById('<%=btn_min_pubpl.ClientID%>').style.display = 'block';

        }
        function frelig10() {
            document.getElementById('<%= btn_pls_callno .ClientID%>').style.display = 'block';
            document.getElementById('<%=btn_min_callno.ClientID%>').style.display = 'block';
        }
        function frelig11() {
            document.getElementById('<%= btn_plu_bo .ClientID%>').style.display = 'block';
            document.getElementById('<%=btn_min_bo.ClientID%>').style.display = 'block';
        }
        function frelig12() {
            document.getElementById('<%= btn_pls_Thrid .ClientID%>').style.display = 'block';
            document.getElementById('<%=btn_min_Thrid.ClientID%>').style.display = 'block';
        }
        function valid2() {
            var idval = "";
            var empty = "";
            var id = "";
            var value1 = "";
            id = document.getElementById("<%=txt_accno.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_accno.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txt_title.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_title.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txt_depart.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_depart.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }

            id = document.getElementById("<%=txt_author.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_author.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            //            id = document.getElementById("<%=Txt_edit.ClientID %>").value;
            //            if (id.trim() == "") {
            //                id = document.getElementById("<%=Txt_edit.ClientID %>");
            //                id.style.borderColor = 'Red';
            //                empty = "E";
            //            }
            id = document.getElementById("<%=ddl_status.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddl_status.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }

            id = document.getElementById("<%=txt_publisyear.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_publisyear.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            if (empty.trim() != "") {
                return false;
            }
            else {
                return true;
            }
        }
        function valid1() {
            var idval = "";
            var empty = "";
            var id = "";
            var value1 = "";
            id = document.getElementById("<%=ddl_Library.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddl_Library.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txacc.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txacc.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }

            id = document.getElementById("<%=ddl_mat.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddl_mat.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            if (empty.trim() != "") {
                return false;
            }
            else {
                return true;
            }
        }
        function SelLedgers() {
            var chkSelAll = document.getElementById("<%=chkGridSelectAll.ClientID %>");
            var tbl = document.getElementById("<%=grdInward.ClientID %>");
            var gridViewControls = tbl.getElementsByTagName("input");

            for (var i = 0; i < (tbl.rows.length); i++) {
                var chkSelectid = document.getElementById('MainContent_grdInward_selectchk_' + i.toString());

                if (chkSelAll.checked == false) {
                    chkSelectid.checked = false;
                } else {
                    chkSelectid.checked = true;
                }
            }

        }
  
     

    </script>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Inward Entries</span></div>
        </center>
    </div>
    <center>
        <div>
            <table>
                <tr>
                    <td>
                        <center>
                            <div style="width: 1000px; font-family: Book Antiqua; font-weight: bold; height: auto">
                                <table class="maintablestyle" style="height: auto; margin-top: 10px; margin-bottom: 10px;
                                    padding: 6px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblclg" runat="server" Text="College">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UP_issue" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                        Width="130px" Height="" AutoPostBack="True" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbllibrary" runat="server" Text="Library" CssClass="commonHeaderFont">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddllibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                        Width="130px" AutoPostBack="True" OnSelectedIndexChanged="ddllibrary_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel37" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBox ID="chkredate" runat="server" AutoPostBack="true" OnCheckedChanged="chkredate_CheckedChanged" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            From Date
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtfromdate" runat="server" Enabled="false" AutoPostBack="true"
                                                        Width="80px" CssClass="textbox txtheight2"></asp:TextBox>
                                                    <asp:CalendarExtender ID="calendetextenfordatext" TargetControlID="txtfromdate" runat="server"
                                                        Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            To Date
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txttodate" runat="server" Enabled="false" AutoPostBack="true" Width="80px"
                                                        CssClass="textbox txtheight2"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txttodate" runat="server"
                                                        Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbltype" runat="server" Text="Type" CssClass="commonHeaderFont">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddltype" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                        Width="130px" AutoPostBack="True" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblsearch" runat="server" Text="SearchBy" CssClass="commonHeaderFont">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlsearch" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                        Width="130px" AutoPostBack="True" OnSelectedIndexChanged="ddlSearchby_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td colspan="14">
                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlsearch_title" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                        Width="65px" AutoPostBack="True" Visible="false" OnSelectedIndexChanged="ddlsearch_title_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="ddlsearchchange" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                        Width="130px" AutoPostBack="True" Visible="false" OnSelectedIndexChanged="ddlSearchchange_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtsearch" AutoPostBack="true" runat="server" Width="120px" CssClass="textbox txtheight2"
                                                        Visible="false" OnTextChanged="txtsearch_OnTextChanged"></asp:TextBox>
                                                    <asp:DropDownList ID="ddlsearcbook" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                        Width="130px" AutoPostBack="True" Visible="false" OnSelectedIndexChanged="ddlsearcbook_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:CheckBox ID="chkbetween" runat="server" Text="Between" AutoPostBack="true" OnCheckedChanged="chkbetween_CheckedChanged"
                                                        Visible="false" />
                                                    <asp:TextBox ID="Txtbet1" runat="server" AutoPostBack="true" Width="50px" Height="15px"
                                                        CssClass="textbox txtheight2" Visible="false"></asp:TextBox>
                                                    <asp:TextBox ID="Txtbet2" runat="server" AutoPostBack="true" Width="50px" Height="15px"
                                                        CssClass="textbox txtheight2" Visible="false"></asp:TextBox>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="10" align="right">
                                            <asp:UpdatePanel ID="UpGoAdd" runat="server">
                                                <ContentTemplate>
                                                    <asp:ImageButton ID="Btngo" runat="server" ImageUrl="~/LibImages/Go.jpg" Style="margin-top: 10px;"
                                                        OnClick="btngo_Click" />
                                                    <asp:ImageButton ID="btnadd" runat="server" ImageUrl="~/LibImages/Add.jpg" Style="margin-top: 10px;"
                                                        OnClick="btnadd_Click" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="Btngo" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </center>
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <br />
    <div>
        <center>
            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                <ContentTemplate>
                    <span style="padding-right: 100px; margin-left: -260px; margin-top: 3px;">
                        <asp:CheckBox ID="chkGridSelectAll" runat="server" Text="SelectAll" Visible="false"
                            onchange="return SelLedgers();" Style="margin-left: -502px;" />
                    </span>
                    <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                    <asp:GridView ID="grdInward" Width="900px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                        ShowHeader="true" Font-Names="Book Antiqua" toGenerateColumns="false" AllowPaging="true"
                        PageSize="1000" OnRowDataBound="grdInward_RowDataBound" OnPageIndexChanging="grdInward_OnPageIndexChanged"
                        OnRowCreated="OnRowCreated" OnSelectedIndexChanged="SelectedIndexChanged">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="allchk" runat="server" Text="Select All" />
                                    <%--<asp:CheckBox ID="allchk" runat="server" Text="Select All" />--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="selectchk" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="grdInward" />
                </Triggers>
            </asp:UpdatePanel>
            <br />
            <br />
            <br />
            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                <ContentTemplate>
                    <div id="rptprint1" runat="server" visible="false">
                        <asp:Label ID="lblvalidation2" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                            Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname1" runat="server" Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname1" runat="server" Height="20px" Width="180px" onkeypress="display()"
                            Font-Size="Medium" CssClass="textbox txtheight2"></asp:TextBox>
                        <asp:ImageButton ID="btnExcel1" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                            OnClick="btnExcel1_Click" />
                        <asp:ImageButton ID="btnprintmaster1" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                            OnClick="btnprintmaster1_Click" />
                        <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                        <asp:ImageButton ID="btndel" runat="server" ImageUrl="~/LibImages/delete.jpg" OnClick="btndel_Click" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btndel" />
                    <asp:PostBackTrigger ControlID="btnExcel1" />
                    <asp:PostBackTrigger ControlID="btnprintmaster1" />
                </Triggers>
            </asp:UpdatePanel>
        </center>
    </div>
    <center>
        <asp:UpdatePanel ID="UpdatePanel26" runat="server">
            <ContentTemplate>
                <div id="popview" runat="server" class="popupstyle popupheight1" visible="false"
                    style="height: 300em;">
                    <asp:ImageButton ID="imagebtnpop1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 8px; margin-left: 460px;"
                        OnClick="btn_popclose_Click" />
                    <br />
                    <div style="background-color: White; height: 867px; font-family: Book Antiqua; font-weight: bold;
                        width: 960px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <span class="fontstyleheader" style="color: #008000;">Inward Entry</span>
                        </center>
                        <div>
                            <center>
                                <fieldset id="studdetail" runat="server" style="height: 770px; width: 890px;">
                                    <table width="890px">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_lib" runat="server" Text="Library"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_txt_lib" runat="server" Style="width: 185px; height: 30px;"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_txt_lib_OnSelectedIndexChanged"
                                                    CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                Ref.book
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rbl_no" Text="No" GroupName="ww" runat="server" AutoPostBack="true"
                                                    OnCheckedChanged="rbl_no_OnCheckedChanged" Checked="true"></asp:RadioButton>
                                                <asp:RadioButton ID="rbl_yes" Text="Yes" GroupName="ww" runat="server" AutoPostBack="true"
                                                    OnCheckedChanged="rbl_yes_OnCheckedChanged"></asp:RadioButton>
                                                <asp:Label ID="lbl_yes" runat="server" Text="NoOfBooks:" Visible="false"></asp:Label>
                                                <asp:TextBox ID="text_ref" runat="server" Style="width: 42px; margin-left: -6px"
                                                    CssClass="textbox txtheight2" Visible="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Entry Type
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_entrytype" runat="server" Style="width: 185px; height: 30px;"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlentrytype_SelectedIndexChanged"
                                                    CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                                <asp:CheckBox ID="IsNonBook" runat="server" Text="Non Book" AutoPostBack="true" OnCheckedChanged="chk_nonbook_CheckedChanged" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_calldes" runat="server" Text="Call Description\Class No"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanelPopDes" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddl_Description" runat="server" Style="width: 185px; height: 30px;"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_Description_SelectedIndexChanged"
                                                            CssClass="textbox ddlstyle ddlheight3">
                                                        </asp:DropDownList>
                                                        <asp:Button ID="btn_popupDes" runat="server" Text="?" Style="width: 25px; height: 30px;"
                                                            OnClick="btn_popupDes_OnClick" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                No.Of.Copies
                                            </td>
                                            <td>
                                                <fieldset style="width: 217px; height: 15px;">
                                                    <asp:RadioButton ID="rblSingle" runat="server" Text="Single" RepeatDirection="Horizontal"
                                                        AutoPostBack="true" OnCheckedChanged="rblSingle_Selected" Enabled="True" Font-Names=" Book antiqua"
                                                        Checked="true" />
                                                    <asp:RadioButton ID="rblMultiple" runat="server" Text="Multiple" RepeatDirection="Horizontal"
                                                        AutoPostBack="true" OnCheckedChanged="rblMultiple_Selected" Enabled="True" Font-Names=" Book antiqua" />
                                                    <asp:TextBox ID="TxtMultiple" runat="server" MaxLength="4" AutoPostBack="true" Style="height: 10px;
                                                        width: 50px; margin-left: 160px; margin-top: -19px;" CssClass="textbox txtheight2"
                                                        Visible="true"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="TxtMultiple"
                                                        FilterType="Numbers">
                                                    </asp:FilteredTextBoxExtender>
                                                </fieldset>
                                            </td>
                                            <td>
                                                Publication year
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_publisyear" runat="server" Style="width: 70px;" onkeypress="display(this)"
                                                    CssClass="textbox txtheight2"></asp:TextBox>
                                                <span style="color: Red;">*</span> Edition
                                                <asp:TextBox ID="Txt_edit" runat="server" AutoPostBack="true" Width="70px" Height="20px"
                                                    CssClass="textbox txtheight2"></asp:TextBox>
                                                <%--<span style="color: Red;">*</span>--%>
                                                <asp:Label ID="newcopy" runat="server" Text="No.Of.Copies:" Visible="false" Font-Size="Medium"
                                                    Font-Names="Book Antiqua"></asp:Label>
                                                <asp:TextBox ID="txt_newcopy" runat="server" AutoPostBack="true" Width="63px" Height="20px"
                                                    CssClass="textbox txtheight2" Visible="false"></asp:TextBox>
                                        </tr>
                                        <tr>
                                            <td>
                                                Copy
                                                <asp:TextBox ID="txt_copy" runat="server" AutoPostBack="true" Width="50px" Height="15px"
                                                    CssClass="textbox txtheight2" Visible="true"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblnonbook" runat="server" Text="NonbookAcc.No:" CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua" Visible="false">
                                                </asp:Label>
                                                <asp:TextBox ID="txtnonbook" runat="server" AutoPostBack="true" Width="87px" Height="15px"
                                                    CssClass="textbox txtheight2" Visible="false"></asp:TextBox>
                                                <asp:UpdatePanel ID="UpLnlNonBk" runat="server">
                                                    <ContentTemplate>
                                                        <asp:LinkButton ID="Lnknonbook" Text="NonBook" Font-Name="Book Antiqua" Font-Size="11pt"
                                                            OnClick="lnknonbook_Click" runat="server" Width="22px" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                Publication Place
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_pls_pubpl" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                                    Height="22px" Style="height: 23px; display: none; left: 656px; position: absolute;
                                                    top: 220px; width: 27px;" OnClick="btn_pls_pubpl_Click" Text="+" />
                                                <asp:DropDownList ID="ddl_publishplace" runat="server" Style="width: 185px; height: 30px;"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_publishplace_SelectedIndexChanged"
                                                    CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                                <asp:Button ID="btn_min_pubpl" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                                    Height="22px" Style="height: 23px; display: none; left: 865px; position: absolute;
                                                    top: 220px; width: 27px;" OnClick="btn_min_pubpl_Click" Text="-" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Category
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_pls_cat" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                                    Height="22px" Style="height: 23px; display: none; left: 162px; position: absolute;
                                                    top: 251px; width: 27px;" OnClick="btn_pls_cat_Click" Text="+" />
                                                <asp:DropDownList ID="ddl_Category" runat="server" Style="width: 185px; height: 30px;"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_Category_SelectedIndexChanged"
                                                    CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                                <asp:Button ID="btn_min_cat" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                                    Height="22px" Style="height: 23px; display: none; left: 371px; position: absolute;
                                                    top: 251px; width: 27px;" OnClick="btn_min_cat_Click" Text="-" />
                                            </td>
                                            <td>
                                                Book type
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_plu_bo" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                                    Style="font-family: Book Antiqua; font-size: small; height: 27px; left: 931px;
                                                    display: none; position: absolute; top: 249px; width: 27px;" OnClick="btn_plu_bo_Click"
                                                    Text="+" />
                                                <asp:DropDownList ID="ddl_booktype" runat="server" Style="width: 185px; height: 30px;"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_booktype_SelectedIndexChanged"
                                                    CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                                <asp:Button ID="btn_min_bo" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                                    Style="height: 27px; display: none; left: 1141px; position: absolute; top: 246px;
                                                    width: 27px;" OnClick="btn_min_bo_Click" Text="-" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 136px;">
                                                Date Of Accession
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_date_acc" runat="server" AutoPostBack="true" Width="95px" CssClass="textbox txtheight2"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_date_acc" runat="server"
                                                    Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                ISBN/ISSN No
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_isbn_No" runat="server" AutoPostBack="true" Width="87px" Height="20px"
                                                    CssClass="textbox txtheight2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                AccessNo
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_accno" runat="server" Enabled="true" AutoPostBack="true" Width="95px"
                                                    Height="20px" CssClass="textbox txtheight2" OnTextChanged="txt_accno_OnTextChanged"></asp:TextBox>
                                                <span style="color: Red;">*</span>
                                            </td>
                                            <td>
                                                Call No
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_pls_callno" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                                    Height="22px" Style="height: 23px; display: none; left: 656px; position: absolute;
                                                    top: 320px; width: 27px;" OnClick="btn_pls_callno_Click" Text="+" />
                                                <asp:DropDownList ID="ddl_CallNo" runat="server" Style="width: 185px; height: 30px;"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_CallNo_SelectedIndexChanged"
                                                    CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                                <asp:Button ID="btn_min_callno" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                                    Height="22px" Style="height: 23px; display: none; left: 864px; position: absolute;
                                                    top: 319px; width: 27px;" OnClick="btn_min_callno_Click" Text="-" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Title
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_title" runat="server" AutoPostBack="true" Width="171px" Height="20px"
                                                    CssClass="textbox txtheight2" OnTextChanged="txt_title_Change"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetTitle" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_title"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="multxt1panel"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                                </asp:AutoCompleteExtender>
                                                <span style="color: Red;">*</span>
                                                <asp:DropDownList ID="ddl_title_lan" runat="server" Style="width: 92px; height: 30px;"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_title_lan_SelectedIndexChanged"
                                                    CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                Status
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_pls_status" runat="server" Style="font-family: Book Antiqua;
                                                    font-size: small; display: none; height: 27px; left: 929px; position: absolute;
                                                    top: 350px; width: 27px;" OnClick="btn_pls_status_Click" Text="+" />
                                                <asp:DropDownList ID="ddl_status" runat="server" Style="width: 185px; height: 30px;"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_status_SelectedIndexChanged"
                                                    CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                                <span style="color: Red;">*</span>
                                                <asp:Button ID="btn_min_status" runat="server" Style="font-family: Book Antiqua;
                                                    font-size: small; height: 27px; display: none; left: 1141px; position: absolute;
                                                    top: 349px; width: 27px;" OnClick="btn_min_status_Click" Text="-" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Author
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_author" runat="server" AutoPostBack="true" Width="171px" Height="20px"
                                                    CssClass="textbox txtheight2"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetAuthor" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_author"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="multxt1panel"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                                </asp:AutoCompleteExtender>
                                                <span style="color: Red;">*</span>
                                                <asp:DropDownList ID="ddl_Author_lan" runat="server" Style="width: 92px; height: 30px;"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_Author_lan_SelectedIndexChanged"
                                                    CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                Attachement
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_pls_att" runat="server" Style="font-family: Book Antiqua; font-size: small;
                                                    height: 27px; display: block; left: 930px; display: none; position: absolute;
                                                    top: 384px; width: 27px;" OnClick="btn_pls_att_Click" Text="+" />
                                                <asp:DropDownList ID="ddl_atta" runat="server" Style="width: 185px; height: 30px;"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_atta_SelectedIndexChanged" CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                                <asp:Button ID="btn_min_att" runat="server" Style="font-family: Book Antiqua; font-size: small;
                                                    height: 27px; display: block; left: 1141px; display: none; position: absolute;
                                                    top: 384px; width: 27px;" OnClick="btn_min_att_Click" Text="-" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Second Author
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Txt_SedAuthor" runat="server" AutoPostBack="true" Width="274px"
                                                    Height="20px" CssClass="textbox txtheight2"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetSecAuthor" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="Txt_SedAuthor"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="multxt1panel"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Thrid Author
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_pls_Thrid" Visible="true" runat="server" Font-Names="Book Antiqua"
                                                    Font-Size="Small" Height="22px" Style="font-family: Book Antiqua; font-size: Small;
                                                    height: 22px; font-family: Book Antiqua; font-size: small; height: 30px; left: 424px;
                                                    display: block; position: absolute; top: 451px; width: 27px;" OnClick="btn_pls_Thrid_Click"
                                                    Text="+" />
                                                <asp:DropDownList ID="ddl_thridAuthor" runat="server" Style="width: 270px; height: 30px;
                                                    margin-left: 0px;" AutoPostBack="true" OnSelectedIndexChanged="ddl_thridAuthor_SelectedIndexChanged"
                                                    CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                                <asp:Button ID="btn_min_Thrid" Visible="true" runat="server" Font-Names="Book Antiqua"
                                                    Font-Size="Small" Height="22px" Style="font-family: Book Antiqua; font-size: Small;
                                                    height: 22px; font-family: Book Antiqua; display: block; font-size: small; height: 31px;
                                                    left: 725px; position: absolute; top: 451px; width: 27px;" OnClick="btn_min_Thrid_Click"
                                                    Text="-" />
                                            </td>
                                            <td>
                                                Rack No
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_Rack" runat="server" Style="width: 185px; height: 30px;"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_Rack_SelectedIndexChanged" CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Publisher
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Txt_pub" runat="server" AutoPostBack="true" Width="274px" Height="20px"
                                                    CssClass="textbox txtheight2"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetPublisher" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="Txt_pub"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="multxt1panel"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                            <td>
                                                Shelf
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_shelf" runat="server" Style="width: 185px; height: 30px;"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_shelf_SelectedIndexChanged" CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Subject
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Txt_sub" runat="server" AutoPostBack="true" Width="274px" Height="20px"
                                                    CssClass="textbox txtheight2"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetSubject" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="Txt_sub"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="multxt1panel"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                            <td>
                                                Position
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_posi" runat="server" Style="width: 185px; height: 30px;"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_posi_SelectedIndexChanged" CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Department
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_depart" runat="server" AutoPostBack="true" Width="268px" Height="20px"
                                                    CssClass="textbox txtheight2" OnTextChanged="txt_depart_Change"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="Getdeptname" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_depart"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="multxt1panel"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                                </asp:AutoCompleteExtender>
                                                <span style="color: Red;">*</span>
                                            </td>
                                            <td>
                                                Pos. Place
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_posplace" runat="server" Style="width: 185px; height: 30px;"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_posplace_SelectedIndexChanged"
                                                    CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Currency Type
                                            </td>
                                            <td>
                                                <asp:Button ID="Btncurrency" runat="server" Style="font-family: Book Antiqua; font-size: small;
                                                    height: 27px; display: none; left: 425px; position: absolute; top: 588px; width: 27px;"
                                                    OnClick="btcurrencyplus_Click" Text="+" />
                                                <asp:DropDownList ID="ddl_curren" runat="server" Style="width: 185px; height: 30px;"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_curren_SelectedIndexChanged"
                                                    CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                                <asp:Button ID="btncurry" runat="server" Style="font-family: Book Antiqua; font-size: small;
                                                    height: 27px; display: none; left: 635px; position: absolute; top: 588px; width: 27px;"
                                                    OnClick="btncurrymin_Click" Text="-" />
                                            </td>
                                            <td>
                                                Book Size/Pages
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_bookSz" runat="server" AutoPostBack="true" Width="174px" Height="20px"
                                                    CssClass="textbox txtheight2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Currency Value
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_curval" runat="server" AutoPostBack="true" Width="92px" Height="20px"
                                                    CssClass="textbox txtheight2"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_curval"
                                                    FilterType="Numbers,Custom" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                                Discount
                                                <asp:TextBox ID="txt_Offer" runat="server" AutoPostBack="true" OnTextChanged="txt_Offer_OnTextChanged"
                                                    Width="92px" Height="20px" CssClass="textbox txtheight2"></asp:TextBox>%
                                                <asp:FilteredTextBoxExtender ID="filterextendertxt_Offer" runat="server" TargetControlID="txt_Offer"
                                                    FilterType="Numbers,Custom" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                Remarks
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_remarks" runat="server" AutoPostBack="true" Width="174px" Height="20px"
                                                    CssClass="textbox txtheight2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Price
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Price" runat="server" AutoPostBack="true" Width="90px" Height="20px"
                                                    CssClass="textbox txtheight2"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_Price"
                                                    FilterType="Numbers,Custom" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                Budget Head
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_pls_bud" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                                    Height="22px" Style="height: 23px; display: none; left: 162px; position: absolute;
                                                    top: 287px; width: 27px;" OnClick="btn_pls_bud_Click" Text="+" />
                                                <asp:DropDownList ID="ddl_Budget" runat="server" Style="width: 184px; height: 30px;"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_Budget_SelectedIndexChanged"
                                                    CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                                <asp:Button ID="btn_min_bud" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                                    Height="22px" Style="height: 23px; display: none; left: 371px; position: absolute;
                                                    top: 287px; width: 27px;" OnClick="btn_min_bud_Click" Text="-" />
                                                <%-- BNo
                                                <asp:TextBox ID="txt_bno" runat="server" AutoPostBack="true" Width="50px" Height="20px"
                                                    CssClass="textbox txtheight2"></asp:TextBox>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Supplier
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlsupp" runat="server" AutoPostBack="true" CssClass="textbox ddlstyle ddlheight3"
                                                    Style="width: 200px; height: 30px;">
                                                </asp:DropDownList>
                                                <%--<asp:TextBox ID="txt_supplier" runat="server" AutoPostBack="true" Width="174px" Height="20px"
                                                    CssClass="textbox txtheight2"></asp:TextBox>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Bill/Invoice No & Date
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_billno" runat="server" AutoPostBack="true" Width="75px" Height="20px"
                                                    CssClass="textbox txtheight2"></asp:TextBox>
                                                <asp:CheckBox ID="chk_date" runat="server" AutoPostBack="true" OnCheckedChanged="chk_date_CheckedChanged" />
                                                <asp:TextBox ID="Txtbilldate" runat="server" AutoPostBack="true" Width="80px" Height="20px"
                                                    CssClass="textbox txtheight2" Enabled="false"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="Txtbilldate" runat="server"
                                                    Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Language
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_pls_lang" runat="server" Style="font-family: Book Antiqua; font-size: small;
                                                    height: 27px; display: none; left: 405px; position: absolute; top: 725px; width: 27px;"
                                                    OnClick="btn_pls_lang_Click" Text="+" />
                                                <asp:DropDownList ID="ddl_language" runat="server" Style="width: 185px; height: 30px;"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_language_SelectedIndexChanged"
                                                    CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                                <asp:Button ID="btn_min_lang" runat="server" Style="font-family: Book Antiqua; font-size: small;
                                                    height: 27px; display: none; left: 614px; position: absolute; top: 725px; width: 27px;"
                                                    OnClick="btn_min_lang_Click" Text="-" />
                                            </td>
                                        </tr>
                                        <tr>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpLnkSts" runat="server">
                                                    <ContentTemplate>
                                                        <asp:LinkButton ID="link_status" Text="Status" Font-Name="Book Antiqua" Font-Size="11pt"
                                                            OnClick="link_status_Click" runat="server" Width="22px" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpLnkAdd" runat="server">
                                                    <ContentTemplate>
                                                        <asp:LinkButton ID="link_addtion" Text="Additional Details" Font-Name="Book Antiqua"
                                                            Font-Size="11pt" OnClick="link_addtion_Click" runat="server" Width="150px" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <fieldset style="left: 825px; top: 696px; width: 241px; height: 130px; position: absolute;">
                                                    <asp:Image ID="imgstudp" runat="server" Style="width: 105px; height: 105px; position: absolute;
                                                        left: 15px; top: 4px;" />
                                                    <asp:UpdatePanel ID="UpdatePanel39" runat="server" UpdateMode="conditional">
                                                        <ContentTemplate>
                                                            <asp:FileUpload ID="fulstudp" runat="server" Style="position: absolute; left: 5px;
                                                                top: 130px; background-color: #0CA6CA;" />
                                                            <asp:ImageButton ID="BtnsaveStud" runat="server" Font-Bold="true" ImageUrl="~/LibImages/click.jpg"
                                                                OnClick="BtnsaveStud_Click" Style="position: absolute; left: 175px; top: 130px;" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="BtnsaveStud" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:HyperLink ID="HyperLink1" NavigateUrl="https://catalog.loc.gov/" runat="server"
                                                    Font-Name="Book Antiqua" Font-Size="11pt">www.loc.gov</asp:HyperLink>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanelButtonEvents" runat="server">
                                                    <ContentTemplate>
                                                        <asp:ImageButton ID="btn_Save" runat="server" Font-Bold="true" ImageUrl="~/LibImages/save.jpg"
                                                            OnClick="btn_Save_Click" OnClientClick="return valid2()" />
                                                        <asp:ImageButton ID="btn_Delete" runat="server" Font-Bold="true" ImageUrl="~/LibImages/delete.jpg"
                                                            OnClick="btn_Delete_Click" Visible="false" />
                                                        <asp:ImageButton ID="btn_Exit" runat="server" Font-Bold="true" ImageUrl="~/LibImages/save (2).jpg"
                                                            OnClick="btn_Exit_Click" />
                                                        <asp:Button ID="BtnClear" runat="server" Text="Clear" OnClick="BtnClear_OnClick"
                                                            Font-Bold="true" Style="background-color: LightGreen; font-weight: bold; height: 26px;
                                                            width: 83px;" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </center>
                        </div>
                        <br />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%-- ------------Start  Non Book Popup--------------------------------------%>
    <asp:UpdatePanel ID="UpdatePanel27" runat="server">
        <ContentTemplate>
            <center>
                <div id="DivNonBookpopup" runat="server" class="popupstyle popupheight1" visible="false"
                    style="height: 300em; font-family: Book Antiqua; font-weight: bold;">
                    <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 7px; margin-left: 413px;"
                        OnClick="btn_DivNonBookpopup_popclose_Click" />
                    <br />
                    <div style="background-color: White; height: 600px; width: 715px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px; margin-left: 150px">
                        <br />
                        <center>
                            <span class="fontstyleheader" style="color: #008000;">Non Book Materials Entry</span>
                        </center>
                        <div>
                            <center>
                                <fieldset id="Fieldset1" runat="server" style="height: 500px; width: 600px;">
                                    <table width="650px">
                                        <tr>
                                            <td>
                                                Library
                                                <asp:DropDownList ID="ddl_Library" runat="server" Style="width: 180px; height: 30px;
                                                    margin-left: 48px" AutoPostBack="true" OnSelectedIndexChanged="ddl_Library_SelectedIndexChanged"
                                                    CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                                <span style="color: Red;">*</span>
                                            </td>
                                            <td colspan="2">
                                                <fieldset style="width: 217px; height: 10px;">
                                                    <asp:RadioButton ID="rbl_non_Single" runat="server" Text="Single" RepeatDirection="Horizontal"
                                                        AutoPostBack="true" OnCheckedChanged="rbl_non_Single_Selected" Enabled="True"
                                                        Font-Names=" Book antiqua" Checked="true" />
                                                    <asp:RadioButton ID="rbl_non_mul" runat="server" Text="Multiple" RepeatDirection="Horizontal"
                                                        AutoPostBack="true" OnCheckedChanged="rbl_non_mul_Selected" Enabled="True" Font-Names=" Book antiqua" />
                                                    <asp:TextBox ID="txcopy" runat="server" AutoPostBack="true" Width="50px" Height="10px"
                                                        CssClass="textbox txtheight2" Visible="false"></asp:TextBox>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                AccessNo
                                                <asp:TextBox ID="txacc" runat="server" Style="width: 87px; height: 20px; margin-left: 33px"
                                                    CssClass="textbox txtheight2"></asp:TextBox>
                                                <span style="color: Red;">*</span>
                                            </td>
                                            <td>
                                                <%-- <asp:LinkButton ID="Link_Book_Type" Text="BookType" Font-Name="Book Antiqua" Font-Size="11pt"
                                        OnClick="Link_Book_Type_Click" runat="server" Width="22px" />--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                MaterialName
                                                <asp:Button ID="btn_pls_mat" runat="server" Style="font-family: Book Antiqua; font-size: small;
                                                    height: 27px; display: none; left: 518px; position: absolute; top: 192px; width: 27px;"
                                                    OnClick="btn_pls_mat_Click" Text="+" />
                                                <asp:DropDownList ID="ddl_mat" runat="server" Style="width: 178px; height: 30px;"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_mat_SelectedIndexChanged" CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                                <span style="color: Red;">*</span>
                                                <asp:Button ID="btn_min_mat" runat="server" Style="font-family: Book Antiqua; font-size: small;
                                                    height: 27px; display: none; left: 718px; position: absolute; top: 192px; width: 27px;"
                                                    OnClick="btn_min_mat_Click" Text="-" />
                                            </td>
                                            <td>
                                                Budget Head
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_non_budget" runat="server" Style="width: 185px; height: 30px;"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_non_budget_SelectedIndexChanged"
                                                    CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Journal Acc.No
                                                <asp:TextBox ID="txt_jour" runat="server" AutoPostBack="true" Style="width: 135px;
                                                    height: 20px; margin-right: 33px; margin-left: -6px;" CssClass="textbox txtheight2"></asp:TextBox>
                                                <asp:Button ID="btn_jour_popup" runat="server" Text="?" Style="width: 25px; height: 30px;
                                                    margin-left: -30px;" OnClick="btn_jour_popup_OnClick" />
                                            </td>
                                            <td>
                                                Book Access No
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtbook_accno" runat="server" AutoPostBack="true" Style="width: 147px;
                                                    height: 20px;" CssClass="textbox txtheight2"></asp:TextBox>
                                                <asp:Button ID="btn_book_accnopopup" runat="server" Text="?" Style="width: 25px;
                                                    height: 30px;" OnClick="btn_book_accnopopup_OnClick" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Title
                                                <asp:TextBox ID="txtitle" runat="server" Style="width: 173px; height: 20px; margin-left: 71px"
                                                    CssClass="textbox txtheight2"></asp:TextBox>
                                            </td>
                                            <td>
                                                Department
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddDepart" runat="server" Style="width: 185px; height: 30px;"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddDepart_SelectedIndexChanged" CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Author
                                                <asp:TextBox ID="txauthor" runat="server" Style="width: 173px; height: 20px; margin-left: 53px"
                                                    CssClass="textbox txtheight2"></asp:TextBox>
                                            </td>
                                            <td>
                                                Month&Year
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_monYear" runat="server" Style="width: 97px; height: 30px;"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_monYear_SelectedIndexChanged"
                                                    CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtyear" runat="server" Style="width: 75px; height: 20px;" CssClass="textbox txtheight2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Publisher
                                                <asp:TextBox ID="txpublish" runat="server" Style="width: 173px; height: 20px; margin-left: 34px"
                                                    CssClass="textbox txtheight2"></asp:TextBox>
                                            </td>
                                            <td>
                                                Date Of Accession
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txDate_Acc" runat="server" AutoPostBack="true" Width="80px" CssClass="textbox txtheight2"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txDate_Acc" runat="server"
                                                    Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Currency Type
                                                <asp:Button ID="btn_pl_currn" runat="server" Style="font-family: Book Antiqua; font-size: small;
                                                    height: 27px; display: none; left: 520px; position: absolute; top: 382px; width: 27px;"
                                                    OnClick="btn_pl_currn_Click" Text="+" />
                                                <asp:DropDownList ID="ddcurrency" runat="server" Style="width: 181px; height: 30px;"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddcurrency_SelectedIndexChanged"
                                                    CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                                <asp:Button ID="btn_min_currn" runat="server" Style="font-family: Book Antiqua; font-size: small;
                                                    height: 27px; display: none; left: 723px; position: absolute; top: 382px; width: 27px;"
                                                    OnClick="btn_min_currn_Click" Text="-" />
                                            </td>
                                            <td>
                                                Status
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="dd_sts" runat="server" Style="width: 185px; height: 30px;"
                                                    AutoPostBack="true" OnSelectedIndexChanged="dd_sts_SelectedIndexChanged" CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Currency Value
                                                <asp:TextBox ID="txcurrval" runat="server" Style="width: 108px; height: 20px; margin-left: -5px"
                                                    CssClass="textbox txtheight2"></asp:TextBox>
                                            </td>
                                            <td>
                                                Volume No
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtvol" runat="server" Style="width: 90px; height: 20px; margin-left: 0px"
                                                    CssClass="textbox txtheight2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Total Price
                                                <asp:TextBox ID="txttolprice" runat="server" Style="width: 108px; height: 20px; margin-left: 33px"
                                                    CssClass="textbox txtheight2"></asp:TextBox>
                                            </td>
                                            <td>
                                                Issue No
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtissueno" runat="server" Style="width: 90px; height: 20px; margin-left: 2px"
                                                    CssClass="textbox txtheight2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Contents Of Parts
                                            </td>
                                            <td>
                                                ISBN
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtisbn" runat="server" Style="width: 90px; height: 20px; margin-left: 1px"
                                                    CssClass="textbox txtheight2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <textarea id="textarea_contentpart" runat="server" cols="35" rows="3"> </textarea>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpNonBkSave" runat="server">
                                                    <ContentTemplate>
                                                        <asp:ImageButton ID="btn_save_Non_book" runat="server" Font-Bold="true" ImageUrl="~/LibImages/save.jpg"
                                                            OnClick="btn_save_Non_book_Click" />
                                                        <asp:ImageButton ID="btn_Exit_Non" runat="server" Font-Bold="true" ImageUrl="~/LibImages/save (2).jpg"
                                                            OnClick="btn_Exit_Non_book_Click" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Running Time
                                                <asp:TextBox ID="txt_time" runat="server" Style="width: 90px; height: 20px; margin-left: 1px"
                                                    CssClass="textbox txtheight2"></asp:TextBox>Min.
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </center>
                        </div>
                        <br />
                    </div>
                </div>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--  -----------------End Non Book Popup--------------------------------%>
    <%-- ------------Start  Additonal Detalis(Book) Popup--------------------------------------%>
    <asp:UpdatePanel ID="UpdatePanel28" runat="server">
        <ContentTemplate>
            <center>
                <div id="DivAddDetailsBookPopup" runat="server" class="popupstyle popupheight1" visible="false"
                    style="height: 300em;">
                    <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 7px; margin-left: 345px;"
                        OnClick="btn_DivadddetailsBookpopup_popclose_Click" />
                    <br />
                    <div style="background-color: White; font-family: Book Antiqua; font-weight: bold;
                        height: 680px; width: 512px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA;
                        border-radius: 10px; margin-left: 213px">
                        <br />
                        <center>
                            <span class="fontstyleheader" style="color: #008000;">Additional Book Details</span>
                        </center>
                        <div>
                            <center>
                                <fieldset id="Fieldset2" runat="server" style="height: 580px; width: 350px;">
                                    <table width="450px">
                                        <tr>
                                            <td>
                                                Sub Title
                                                <asp:TextBox ID="txsubtitle" runat="server" AutoPostBack="true" Style="width: 290px;
                                                    height: 20px; margin-left: 54px" CssClass="textbox txtheight2" MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Volume Title
                                                <asp:TextBox ID="txvolti" runat="server" AutoPostBack="true" Style="width: 288px;
                                                    height: 20px; margin-left: 28px" CssClass="textbox txtheight2" MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Volume
                                                <asp:TextBox ID="txvo" runat="server" AutoPostBack="true" Style="width: 190px; height: 20px;
                                                    margin-left: 66px" CssClass="textbox txtheight2" MaxLength="10"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txvo"
                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Volume Price
                                                <asp:TextBox ID="txvolpr" runat="server" AutoPostBack="true" Style="width: 190px;
                                                    height: 20px; margin-left: 26px" CssClass="textbox txtheight2" MaxLength="10"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txvolpr"
                                                    FilterType="Numbers">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Keyword1
                                                <asp:TextBox ID="txkey1" runat="server" AutoPostBack="true" Style="width: 290px;
                                                    height: 20px; margin-left: 50px" CssClass="textbox txtheight2" MaxLength="15"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txvo"
                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Keyword2
                                                <asp:TextBox ID="txkey2" runat="server" AutoPostBack="true" Style="width: 290px;
                                                    height: 20px; margin-left: 50px" CssClass="textbox txtheight2" MaxLength="15"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txvo"
                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Keyword3
                                                <asp:TextBox ID="txkey3" runat="server" AutoPostBack="true" Style="width: 290px;
                                                    height: 20px; margin-left: 50px" CssClass="textbox txtheight2" MaxLength="15"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txvo"
                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Collation
                                                <asp:TextBox ID="txcoll" runat="server" AutoPostBack="true" Style="width: 190px;
                                                    height: 20px; margin-left: 58px" CssClass="textbox txtheight2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Book Series
                                                <asp:TextBox ID="txtbose" runat="server" AutoPostBack="true" Style="width: 190px;
                                                    height: 20px; margin-left: 39px" CssClass="textbox txtheight2" MaxLength="15"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txvo"
                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Book Selected By
                                                <asp:TextBox ID="txtboselect" runat="server" AutoPostBack="true" Style="width: 190px;
                                                    height: 20px; margin-left: 0px" CssClass="textbox txtheight2" MaxLength="15"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txvo"
                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Book Accessed By
                                                <asp:TextBox ID="txtboacc" runat="server" AutoPostBack="true" Style="width: 190px;
                                                    height: 20px; margin-left: -4px" CssClass="textbox txtheight2" MaxLength="15"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txvo"
                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Part
                                                <asp:TextBox ID="txtpart" runat="server" AutoPostBack="true" Style="width: 100px;
                                                    height: 20px; margin-left: 99px" CssClass="textbox txtheight2" MaxLength="50"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txvo"
                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Topics
                                                <asp:TextBox ID="txttopics" runat="server" AutoPostBack="true" Style="width: 290px;
                                                    height: 20px; margin-left: 78px" CssClass="textbox txtheight2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Inward Type
                                                <asp:DropDownList ID="ddl_inward_type" runat="server" Style="width: 299px; height: 30px;
                                                    margin-left: 31px;" AutoPostBack="true" OnSelectedIndexChanged="ddl_inward_type_SelectedIndexChanged"
                                                    CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <%-- <tr>
                                <td>
                                    Tamil Title:
                                    <asp:TextBox ID="txttamil" runat="server" AutoPostBack="true" Style="width: 290px;
                                        height: 20px; margin-left: 65px" CssClass="textbox txtheight2"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Tamil Author:
                                    <asp:TextBox ID="txttamilau" runat="server" AutoPostBack="true" Style="width: 290px;
                                        height: 20px; margin-left: 65px" CssClass="textbox txtheight2"></asp:TextBox>
                                </td>
                            </tr>--%>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpAddDet" runat="server">
                                                    <ContentTemplate>
                                                        <asp:ImageButton ID="btn_Ok_Add_details" runat="server" Font-Bold="true" ImageUrl="~/LibImages/ok.jpg"
                                                            OnClick="btn_Ok_Add_details_Click" Style="width: 70px; height: 30px; margin-left: 296px;" />
                                                        <asp:ImageButton ID="btn_Ex_Add_details" runat="server" Font-Bold="true" ImageUrl="~/LibImages/save (2).jpg"
                                                            OnClick="btn_Ex_Add_details_Click" Style="width: 70px; height: 30px; margin-left: 3px;" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </center>
                        </div>
                        <br />
                    </div>
                </div>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--  -----------------End Additonal Detalis(Book) Popup--------------------------------%>
    <%-- ------------Start  Additonal Detalis(Qus Bank) Popup--------------------------------------%>
    <asp:UpdatePanel ID="UpdatePanel29" runat="server">
        <ContentTemplate>
            <center>
                <div id="Div_Question_Bank_popup" runat="server" class="popupstyle popupheight1"
                    visible="false" style="height: 300cm;">
                    <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 100px; margin-left: 385px;"
                        OnClick="btn_Question_Bank_popup_Click" />
                    <br />
                    <div style="background-color: White; height: 571px; font-family: Book Antiqua; font-weight: bold;
                        width: 550px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;
                        margin-left: 213px; margin-top: 85px;">
                        <br />
                        <center>
                            <span class="fontstyleheader" style="color: #008000;">University Question Bank Details</span>
                        </center>
                        <div>
                            <fieldset id="Fieldset3" runat="server" style="height: 475px; width: 300px;">
                                <table width="290px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSNO" runat="server" Text="S.No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSNo" runat="server" AutoPostBack="true" Style="width: 100px;
                                                height: 20px; margin-left: 0px" CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            QuestionPaperCode
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtQusPaper" runat="server" AutoPostBack="true" Style="width: 100px;
                                                height: 20px;" CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Title
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtQueTitle" runat="server" AutoPostBack="true" Style="width: 250px;
                                                height: 20px;" CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Department
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtQusDept" runat="server" AutoPostBack="true" Width="268px" Height="20px"
                                                CssClass="textbox txtheight2"></asp:TextBox><%--OnTextChanged="txtQusDept"--%>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getdeptname" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtQusDept"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="multxt1panel"
                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            PaperName
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txpagename" runat="server" AutoPostBack="true" Style="width: 250px;
                                                height: 20px;" CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Semester
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Text_sem" runat="server" AutoPostBack="true" Style="width: 100px;
                                                height: 20px;" CssClass="textbox txtheight2"></asp:TextBox>
                                            Month
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Text_mon" runat="server" AutoPostBack="true" Style="width: 100px;
                                                height: 20px; margin-left: -98px" CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Year
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Text_year" runat="server" AutoPostBack="true" Style="width: 100px;
                                                height: 20px;" CssClass="textbox txtheight2"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="ftext_mno" runat="server" TargetControlID="Text_year"
                                                FilterType="numbers" ValidChars="">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Regulation
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtQusRegu" runat="server" AutoPostBack="true" Style="width: 100px;
                                                height: 20px;" CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            AffiliationUniversity
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtQusAffUni" runat="server" AutoPostBack="true" Style="width: 100px;
                                                height: 20px;" CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Rack No
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlrack" runat="server" Style="width: 185px; height: 30px;"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlrack_SelectedIndexChanged" CssClass="textbox ddlstyle ddlheight3">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Shelf
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlself" runat="server" Style="width: 185px; height: 30px;"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlself_SelectedIndexChanged" CssClass="textbox ddlstyle ddlheight3">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Position
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlposition" runat="server" Style="width: 185px; height: 30px;"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlposition_SelectedIndexChanged"
                                                CssClass="textbox ddlstyle ddlheight3">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Pos. Place
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlplacepos" runat="server" Style="width: 185px; height: 30px;"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlplacepos_SelectedIndexChanged"
                                                CssClass="textbox ddlstyle ddlheight3">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:ImageButton ID="btnquessave1" runat="server" ImageUrl="~/LibImages/save.jpg"
                                                OnClick="btnquessave1_Click" />
                                            <asp:ImageButton ID="btn_Qus_ok" runat="server" Font-Bold="true" ImageUrl="~/LibImages/ok.jpg"
                                                OnClick="btn_Ok_Add_Qus_details_Click" />
                                            <asp:ImageButton ID="btn_Qus_exit" runat="server" Font-Bold="true" ImageUrl="~/LibImages/save (2).jpg"
                                                OnClick="btn_Ex_Add_Qus_details_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                        <br />
                    </div>
                </div>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%----------------------------------------------newspaper pop----------------------------------%>
    <asp:UpdatePanel ID="UpdatePanel38" runat="server">
        <ContentTemplate>
            <center>
                <div id="divnews_pop" runat="server" class="popupstyle popupheight1" visible="false"
                    style="height: 300cm;">
                    <asp:ImageButton ID="ImageButton9" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 100px; margin-left: 362px;"
                        OnClick="btn_newspaper_popup_Click" />
                    <br />
                    <div style="background-color: White; height: 576px; font-family: Book Antiqua; font-weight: bold;
                        width: 550px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;
                        margin-left: 213px; margin-top: 85px;">
                        <br />
                        <center>
                            <span class="fontstyleheader" style="color: #008000;">News Paper Entry</span>
                        </center>
                        <div>
                            <fieldset id="Fieldset9" runat="server" style="height: 501px; width: 300px;">
                                <table width="290px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label16" runat="server" Text="S.No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox1" runat="server" AutoPostBack="true" Style="width: 100px;
                                                height: 20px; margin-left: 0px" CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <tr>
                                            <td>
                                                Library
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddllibrary1" runat="server" Style="width: 185px; height: 30px;"
                                                    AutoPostBack="true" CssClass="textbox ddlstyle ddlheight3">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <td>
                                            NewspaperTitle
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox2" runat="server" AutoPostBack="true" Style="width: 100px;
                                                height: 20px;" CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            No.ofcopies
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox3" runat="server" AutoPostBack="true" Style="width: 250px;
                                                height: 20px;" CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Price
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox4" runat="server" AutoPostBack="true" Width="268px" Height="20px"
                                                CssClass="textbox txtheight2" OnTextChanged="TextBox4_OnTextChanged"></asp:TextBox><%--OnTextChanged="txtQusDept"--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Total
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox5" runat="server" AutoPostBack="true" Style="width: 250px;
                                                height: 20px;" CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Languages
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox6" runat="server" AutoPostBack="true" Style="width: 100px;
                                                height: 20px;" CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            SupplierName
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox7" runat="server" AutoPostBack="true" Style="width: 100px;
                                                height: 20px;" CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Address
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox8" runat="server" AutoPostBack="true" Style="width: 100px;
                                                height: 20px;" CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Place
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox9" runat="server" AutoPostBack="true" Style="width: 100px;
                                                height: 20px;" CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Rack No
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DropDownList1" runat="server" Style="width: 185px; height: 30px;"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlrack_SelectedIndexChanged" CssClass="textbox ddlstyle ddlheight3">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Shelf
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DropDownList2" runat="server" Style="width: 185px; height: 30px;"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlself_SelectedIndexChanged" CssClass="textbox ddlstyle ddlheight3">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Position
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DropDownList3" runat="server" Style="width: 185px; height: 30px;"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlposition_SelectedIndexChanged"
                                                CssClass="textbox ddlstyle ddlheight3">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Pos. Place
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DropDownList4" runat="server" Style="width: 185px; height: 30px;"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlplacepos_SelectedIndexChanged"
                                                CssClass="textbox ddlstyle ddlheight3">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:ImageButton ID="btnnewssave" runat="server" ImageUrl="~/LibImages/save.jpg"
                                                OnClick="btnnewssave_Click" />
                                            <asp:ImageButton ID="btnnewsok" runat="server" Font-Bold="true" ImageUrl="~/LibImages/ok.jpg"
                                                OnClick="btnnewsok_Click" />
                                            <asp:ImageButton ID="btnnewsexit" runat="server" Font-Bold="true" ImageUrl="~/LibImages/save (2).jpg"
                                                OnClick="btnnewsexit_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                        <br />
                    </div>
                </div>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--  ----------------------Start Status-------------------------------%>
    <asp:UpdatePanel ID="UpdatePanel30" runat="server">
        <ContentTemplate>
            <center>
                <div id="DivStatus" runat="server" class="popupstyle popupheight1" visible="false"
                    style="height: 300em;">
                    <asp:ImageButton ID="ImageButton4" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 90px; margin-left: 440px;"
                        OnClick="btn_Question_Bank_popup_Click" />
                    <br />
                    <div style="background-color: White; height: 600px; font-family: Book Antiqua; font-weight: bold;
                        width: 840px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;
                        margin-left: 80px; margin-top: 80px">
                        <br />
                        <center>
                            <span class="fontstyleheader" style="color: #008000;">Rack Status Monitor</span>
                        </center>
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <center>
                                            <div>
                                                <table class="maintablestyle" style="height: auto; margin-left: 103px; margin-top: 10px;
                                                    margin-bottom: 10px; padding: 6px; width: 350px">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label1" runat="server" Text="College" CssClass="commonHeaderFont">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlstat_college" runat="server" Style="width: 204px; height: 30px;
                                                                margin-left: 72px;" AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_sts_SelectedIndexChanged"
                                                                CssClass="textbox ddlstyle ddlheight3">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label212" runat="server" Text="Library" CssClass="commonHeaderFont">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddllibrary_sts" runat="server" Style="width: 204px; height: 30px;
                                                                margin-left: 31px;" AutoPostBack="true" OnSelectedIndexChanged="ddllibrary_sts_SelectedIndexChanged"
                                                                CssClass="textbox ddlstyle ddlheight3">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:Label ID="Label2" runat="server" Text="Rack Number" CssClass="commonHeaderFont">
                                                            </asp:Label>
                                                            <asp:DropDownList ID="ddlsts_rackno" runat="server" Style="width: 204px; height: 30px;
                                                                margin-left: 31px;" AutoPostBack="true" OnSelectedIndexChanged="ddlrack_sts_SelectedIndexChanged"
                                                                CssClass="textbox ddlstyle ddlheight3">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpRackStatus" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:ImageButton ID="btn_sts_Rack_Go" runat="server" Font-Bold="true" ImageUrl="~/LibImages/Go.jpg"
                                                                        OnClick="btn_sts_Rack_Go_Click" Style="width: 70px; height: 30px; margin-left: 31px;" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <br />
                                            <table>
                                                <tr>
                                                    <td>
                                                        <fieldset runat="server" style="width: 103px; height: 4px; background-color: Red;
                                                            margin-left: 156px;">
                                                            <asp:Label ID="Label6" runat="server" Text="CompletetyFilled"></asp:Label>
                                                        </fieldset>
                                                    </td>
                                                    <td>
                                                        <fieldset id="Fieldset6" runat="server" enabled="false" style="width: 103px; height: 4px;
                                                            background-color: Green; margin-left: 27px;">
                                                            <asp:Label ID="Label3" runat="server" Text="PartiallyFilled"></asp:Label>
                                                        </fieldset>
                                                    </td>
                                                    <td>
                                                        <fieldset id="Fieldset7" runat="server" enabled="false" style="width: 103px; height: 4px;
                                                            background-color: Yellow; margin-left: 27px;">
                                                            <asp:Label ID="Label7" runat="server" Text="No Shelf Entry"></asp:Label>
                                                        </fieldset>
                                                    </td>
                                                    <%-- <td>
                                            <asp:Label Text="SH->Shelf" runat="server" Style="font-style: italic"></asp:Label>
                                        </td>--%>
                                                </tr>
                                            </table>
                                            <table style="margin-left: 103px; margin-top: 10px;">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label4" Text="SH->Shelf" runat="server" Style="font-style: italic"></asp:Label>
                                                        <asp:Label ID="Label8" Text="AVAIL->Available Copies" runat="server" Style="font-style: italic"></asp:Label>
                                                        <asp:Label ID="Label9" Text="TOT->Maximum Copies" runat="server" Style="font-style: italic"></asp:Label>
                                                        <asp:Label ID="Label10" Text="IM->Category Of Inward Material" runat="server" Style="font-style: italic"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                            <br />
                                            <FarPoint:FpSpread ID="RackFpSpread" runat="server" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="1px" OnCellClick="RackFpSpread_CellClick" OnPreRender="RackFpSpread_SelectedIndexChanged">
                                                <Sheets>
                                                    <FarPoint:SheetView SheetName="Sheet1">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                            </FarPoint:FpSpread>
                                            <br />
                                            <br />
                                            <div id="rptprint" runat="server" visible="false">
                                                <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="lblrptname" runat="server" Font-Size="Medium" Text="Report Name"></asp:Label>
                                                <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" onkeypress="display()"
                                                    Font-Size="Medium" CssClass="textbox txtheight2"></asp:TextBox>
                                                <asp:ImageButton ID="btnExcel" runat="server" Font-Bold="true" ImageUrl="~/LibImages/export to excel.jpg"
                                                    OnClick="btnExcel_Click" />
                                                <asp:ImageButton ID="btnprintmaster" runat="server" Font-Bold="true" ImageUrl="~/LibImages/Print White.jpg"
                                                    OnClick="btnprintmaster_Click" />
                                                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                                            </div>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%-- ------------------------End Status------------------------------%>
    <%------------------------Start Call Description -------------------------------%>
    <asp:UpdatePanel ID="UpdatePanel31" runat="server">
        <ContentTemplate>
            <center>
                <div id="DivcallDes" runat="server" class="popupstyle popupheight1" visible="false"
                    style="height: 300em;">
                    <asp:ImageButton ID="ImageButton5" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 60px; margin-left: 353px;"
                        OnClick="btn_callDes_popup_Click" />
                    <br />
                    <div style="background-color: White; height: 533px; font-family: Book Antiqua; font-weight: bold;
                        width: 540px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;
                        margin-left: 213px; margin-top: 46px">
                        <br />
                        <center>
                            <span class="fontstyleheader" style="color: #008000;">Call Number Details</span>
                        </center>
                        <div>
                            <center>
                                <%--<fieldset id="Fieldset4" runat="server" style="height: 400px; width: 350px;">--%>
                                <table width="450px">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                <ContentTemplate>
                                                    Search By:
                                                    <asp:DropDownList ID="ddl_call" runat="server" Style="width: 157px; height: 30px;
                                                        margin-left: -4px;" AutoPostBack="true" OnSelectedIndexChanged="ddl_call_SelectedIndexChanged"
                                                        CssClass="textbox ddlstyle ddlheight3">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txt_callno_calldes" runat="server" Style="width: 100px; height: 20px;
                                                        margin-left: -2px" CssClass="textbox txtheight2" Visible="false"></asp:TextBox>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_call_go" Style="width: 70px; height: 30px; margin-left: 23px;"
                                                runat="server" CssClass="textbox btn2" Text="Go" OnClick="btn_call_go_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <%-- </fieldset>--%>
                            </center>
                        </div>
                        <div id="divTreeView" runat="server" align="left" style="overflow: auto; width: 400px;
                            height: 350px; border-radius: 10px; border: 1px solid Gray;">
                            <asp:HiddenField ID="HiddenField1" runat="server" Value="-1" />
                            <asp:GridView ID="GrdCallNo" Width="400px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                Font-Names="Book Antiqua" toGenerateColumns="false" OnRowDataBound="GrdCallNo_RowDataBound"
                                OnRowCreated="GrdCallNo_OnRowCreated" OnSelectedIndexChanged="GrdCallNo_SelectedIndexChanged">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                            </asp:GridView>
                        </div>
                        <asp:UpdatePanel ID="UpCallNoDet" runat="server">
                            <ContentTemplate>
                                <asp:ImageButton ID="btn_add_call" Style="width: 70px; height: 30px; margin-left: 221px;
                                    margin-top: 30px;" runat="server" Font-Bold="true" ImageUrl="~/LibImages/AddWhite.jpg"
                                    OnClick="btn_add_call_Click" />
                                <asp:ImageButton ID="btn_exit_call" Style="width: 70px; height: 30px; margin-left: 2px;"
                                    runat="server" Font-Bold="true" ImageUrl="~/LibImages/save (2).jpg" OnClick="btn_exit_call_Click"
                                    Visible="false" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                    </div>
                </div>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--------------------------End Call Des------------------------------------%>
    <asp:UpdatePanel ID="UpdatePanel32" runat="server">
        <ContentTemplate>
            <center>
                <div id="DivCallAdd" runat="server" class="popupstyle popupheight1" visible="false"
                    style="height: 300em;">
                    <asp:ImageButton ID="ImageButton6" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 40px; width: 40px; height: 30px; width: 30px; position: absolute;
                        margin-top: 138px; margin-left: 318px;" OnClick="btn_call_add_popup_Click" />
                    <br />
                    <div style="background-color: White; height: 244px; font-family: Book Antiqua; font-weight: bold;
                        width: 410px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;
                        margin-left: 271px; margin-top: 123px">
                        <br />
                        <center>
                            <span class="fontstyleheader" style="color: #008000;">Call Number Entry</span>
                        </center>
                        <div>
                            <center>
                                <fieldset id="Fieldset5" runat="server" style="height: 144px; width: 250px;">
                                    <table width="350px">
                                        <tr>
                                            <td>
                                                Call Number:
                                                <asp:TextBox ID="callno_txt" runat="server" AutoPostBack="true" Style="width: 170px;
                                                    height: 20px; margin-left: 59px" CssClass="textbox txtheight2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                CallNo Des\Class No:
                                                <asp:TextBox ID="calldes_txt" runat="server" AutoPostBack="true" Style="width: 170px;
                                                    height: 20px; margin-left: -4px" CssClass="textbox txtheight2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                        </tr>
                                        <tr>
                                        </tr>
                                        <tr>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanelCalDesEntry" runat="server">
                                                    <ContentTemplate>
                                                        <asp:ImageButton ID="btn_call_save" Style="width: 70px; height: 30px; margin-left: 107px;"
                                                            runat="server" Font-Bold="true" ImageUrl="~/LibImages/save.jpg" OnClick="btn_call_save_Click" />
                                                        <asp:ImageButton ID="btn_call_Update" Visible="false" Style="width: 70px; height: 30px;
                                                            margin-left: 107px;" runat="server" Font-Bold="true" ImageUrl="~/LibImages/update.jpg"
                                                            OnClick="btn_call_Update_Click" />
                                                        <asp:ImageButton ID="btn_call_delete" Style="width: 70px; height: 30px; margin-left: 8px;"
                                                            runat="server" Font-Bold="true" ImageUrl="~/LibImages/delete.jpg" OnClick="btn_call_delete_Click" />
                                                        <asp:ImageButton ID="btn_call_exit" Style="width: 70px; height: 30px; margin-left: 3px;"
                                                            runat="server" Font-Bold="true" ImageUrl="~/LibImages/save (2).jpg" OnClick="btn_call_exit_Click" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </center>
                        </div>
                        <br />
                    </div>
                </div>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%-------------------------Start journalaccno------------------------------------%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel33" runat="server">
            <ContentTemplate>
                <div id="popwindowjournalaccno" runat="server" class="popupstyle" visible="false"
                    style="height: 50em; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2);
                    position: absolute; top: 0; left: 0;">
                    <asp:ImageButton ID="imgbtn2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 25px; margin-left: 410px;"
                        OnClick="imagebtnpop2close_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; font-family: Book Antiqua; height: 700px; width: 840px;
                        border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <div>
                                <span style="color: Green;" class="fontstyleheader">Select the Journal</span></div>
                            <br />
                        </center>
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_acc_code" Text="Access Code" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_acc_coe" runat="server" AutoPostBack="true" Style="width: 120px;
                                        height: 20px; margin-left: -5px" CssClass="textbox txtheight2"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_search" Text="Search By" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddl_Search_By" runat="server" CssClass="textbox ddlheight2 textbox1"
                                                AutoPostBack="true" onfocus="return myFunction1(this)" OnSelectedIndexChanged="ddl_Search_By_OnSelectedIndexChanged">
                                                <asp:ListItem Value="0">All</asp:ListItem>
                                                <asp:ListItem Value="1">Journal Code</asp:ListItem>
                                                <asp:ListItem Value="2">Journal Title</asp:ListItem>
                                                <asp:ListItem Value="3">Dept Name</asp:ListItem>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_bysearch" runat="server" AutoPostBack="true" Style="width: 120px;
                                                height: 20px; margin-left: -5px" CssClass="textbox txtheight2" Visible="false"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upjournalaccno_go" runat="server">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="btn_journalaccno_go" runat="server" Font-Bold="true" ImageUrl="~/LibImages/Go.jpg"
                                                OnClick="btn_journalaccno_go_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <center>
                            <div>
                                <asp:Label ID="lblpop2error" runat="server" ForeColor="Red" Visible="false">
                                </asp:Label>
                            </div>
                        </center>
                        <br />
                        <center>
                            <div id="divGrdVNonBkAccNo" runat="server" visible="false" style="height: 500px;
                                width: 750px; overflow: auto; background-color: White; border-radius: 10px;">
                                <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                    <ContentTemplate>
                                        <asp:HiddenField ID="HiddenField2" runat="server" Value="-1" />
                                        <asp:GridView ID="GrdVNonBkAccNo" Width="750px" runat="server" ShowFooter="false"
                                            AutoGenerateColumns="true" Font-Names="Book Antiqua" toGenerateColumns="true"
                                            OnRowCreated="GrdVNonBkAccNo_OnRowCreated" OnSelectedIndexChanged="GrdVNonBkAccNo_SelectedIndexChanged">
                                            <%--AllowPaging="true" PageSize="20" OnPageIndexChanging="GrdVNonBkAccNo_OnPageIndexChanged"--%>
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="GrdVNonBkAccNo" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <%--<FarPoint:FpSpread ID="fpaccno" runat="server" Visible="false" Style="overflow: auto;
                                height: 500px; border: 0px solid #999999; border-radius: 5px; background-color: White;
                                box-shadow: 0px 0px 8px #999999;">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>--%>
                        </center>
                        <br />
                        <asp:ImageButton ID="btn_pop2exit" runat="server" Font-Bold="true" ImageUrl="~/LibImages/save (2).jpg"
                            OnClick="btn_pop2exit_Click" Visible="false" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel34" runat="server">
            <ContentTemplate>
                <div id="DivBookAccessNo" runat="server" class="popupstyle" visible="false" style="height: 50em;
                    z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                    top: 0; left: 0;">
                    <asp:ImageButton ID="ImageButton7" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 25px; margin-left: 410px;"
                        OnClick="image_DivBookAccessNoclose_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; height: 700px; width: 900px; font-family: Book Antiqua;
                        border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <div>
                                <span style="color: Green;" class="fontstyleheader">Select Access Number</span></div>
                            <br />
                        </center>
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_boaccno" Text="Access Code" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_boaccno" runat="server" AutoPostBack="true" Style="width: 120px;
                                        height: 20px; margin-left: -5px" CssClass="textbox txtheight2"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lb_Search" Text="Search By" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddl_search_book" runat="server" CssClass="textbox ddlheight2 textbox1"
                                                AutoPostBack="true" onfocus="return myFunction1(this)" OnSelectedIndexChanged="ddl_search_book_OnSelectedIndexChanged">
                                                <asp:ListItem Value="0">All</asp:ListItem>
                                                <asp:ListItem Value="1">Title</asp:ListItem>
                                                <asp:ListItem Value="2">Author</asp:ListItem>
                                                <asp:ListItem Value="3">Publisher</asp:ListItem>
                                                <asp:ListItem Value="4">Edition</asp:ListItem>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_book_search" runat="server" AutoPostBack="true" Style="width: 120px;
                                                height: 20px; margin-left: -5px" CssClass="textbox txtheight2" Visible="false"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upbook_go" runat="server">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="btn_book_go" runat="server" Font-Bold="true" ImageUrl="~/LibImages/Go.jpg"
                                                OnClick="btn_book_go_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <center>
                            <br />
                            <div>
                                <asp:Label ID="Label5" runat="server" ForeColor="Red" Visible="false">
                                </asp:Label>
                            </div>
                        </center>
                        <center>
                            <div id="divBkAccNo" runat="server" visible="false" style="width: 800px; height: 500px;
                                overflow: auto; background-color: White; border-radius: 10px;">
                                <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                    <ContentTemplate>
                                        <asp:HiddenField ID="HiddenField3" runat="server" Value="-1" />
                                        <asp:GridView ID="GrdBkAccNo" Width="800px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                            Font-Names="Book Antiqua" toGenerateColumns="true" OnRowCreated="GrdBkAccNo_OnRowCreated"
                                            OnSelectedIndexChanged="GrdBkAccNo_SelectedIndexChanged" AllowPaging="true" PageSize="5000"
                                            OnPageIndexChanging="GrdBkAccNo_OnPageIndexChanged">
                                            <%----%>
                                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="GrdBkAccNo" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <%--<FarPoint:FpSpread ID="FpSpread1" runat="server" BorderStyle="Solid" BorderWidth="0px"
                                Width="980px" Style="overflow: auto; border: 0px solid #999999; border-radius: 10px;
                                background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder"
                                DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                                EnableClientScript="true" Pager-Align="Right" Pager-ButtonType="ImageButton"
                                CommandBar-ButtonType="ImageButton" CommandBar-Visible="False" Pager-Mode="Both"
                                Pager-Position="Bottom" Pager-PageCount="10" Visible="false">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>--%>
                        </center>
                        <br />
                        <%--<asp:ImageButton ID="btn_book_ok" runat="server" Font-Bold="true" ImageUrl="~/LibImages/ok.jpg"
                            OnClick="btn_book_ok_Click" Visible="false" />--%>
                        <asp:ImageButton ID="btn_book_exit" runat="server" Font-Bold="true" ImageUrl="~/LibImages/save (2).jpg"
                            OnClick="btn_book_ok_exit" Visible="false" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel35" runat="server">
            <ContentTemplate>
                <div id="plusdiv" runat="server" visible="false" class="popupstyle popupheight1">
                    <center>
                        <div id="panel_addgroup" runat="server" visible="false" class="table" style="background-color: White;
                            height: 140px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <table style="line-height: 30px">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_addgroup" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:TextBox ID="txt_addgroup" runat="server" Width="200px" CssClass="textbox txtheight2"
                                            onkeypress="display1()"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="Filteredtxt_amount" runat="server" TargetControlID="txt_addgroup"
                                            FilterType="LowercaseLetters,UppercaseLetters,custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="line-height: 35px">
                                        <asp:ImageButton ID="btn_addgroup1" runat="server" Font-Bold="true" ImageUrl="~/LibImages/AddWhite.jpg"
                                            OnClick="btn_addgroup_Click" />
                                        <asp:ImageButton ID="btn_exitgroup1" runat="server" Font-Bold="true" ImageUrl="~/LibImages/save (2).jpg"
                                            OnClick="btn_exitaddgroup_Click" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <div>
        <center>
            <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                <ContentTemplate>
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
                                                <asp:Label ID="lbl_sure" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:ImageButton ID="btn_yes" runat="server" Font-Bold="true" ImageUrl="~/LibImages/yes.jpg"
                                                        OnClick="btn_sureyes_Click" />
                                                    <asp:ImageButton ID="btn_no" runat="server" Font-Bold="true" ImageUrl="~/LibImages/no (2).jpg"
                                                        OnClick="btn_sureno_Click" />
                                                </center>
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
    </div>
    <div>
        <center>
            <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                <ContentTemplate>
                    <div id="AddtionalDetailPopup" runat="server" visible="false" style="height: 100%;
                        z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                        top: 0; left: 0px;">
                        <center>
                            <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lbr_msg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:UpdatePanel ID="UpdatePanelAdditonalPop" runat="server">
                                                        <ContentTemplate>
                                                            <asp:ImageButton ID="btn_yes1" runat="server" Font-Bold="true" ImageUrl="~/LibImages/yes.jpg"
                                                                OnClick="btn_yes1_Click" />
                                                            <asp:ImageButton ID="btn_no1" runat="server" Font-Bold="true" ImageUrl="~/LibImages/no (2).jpg"
                                                                OnClick="btn_no1_Click" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </center>
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
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel22" runat="server">
            <ContentTemplate>
                <center>
                    <div id="Divnewspopup" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                                <asp:Label ID="lbl_news_msg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:ImageButton ID="bt_yes" runat="server" Font-Bold="true" ImageUrl="~/LibImages/yes.jpg"
                                                        OnClick="bt_yes_Click" />
                                                    <asp:ImageButton ID="bt_no" runat="server" Font-Bold="true" ImageUrl="~/LibImages/no (2).jpg"
                                                        OnClick="bt_no_Click" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div>
        <center>
            <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                <ContentTemplate>
                    <div id="Diveleterecord" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="Div5" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 476px;
                                border-radius: 10px;">
                                <center>
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lbl_Diveleterecord" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:ImageButton ID="btn_detele_yes__record" runat="server" Font-Bold="true" ImageUrl="~/LibImages/yes.jpg"
                                                        OnClick="btn_detele_yes__record_Click" />
                                                    <asp:ImageButton ID="btn_detele_no__record" runat="server" Font-Bold="true" ImageUrl="~/LibImages/no (2).jpg"
                                                        OnClick="btn_detele_no__recordClick" />
                                                </center>
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
    </div>
    <div>
        <center>
            <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                <ContentTemplate>
                    <div id="DivinwardDelete" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="Div6" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="Label11" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:ImageButton ID="btn_inwardcell_yes_Delete" runat="server" Font-Bold="true" ImageUrl="~/LibImages/yes.jpg"
                                                        OnClick="btn_inwardcell_Delete_Click" />
                                                    <asp:ImageButton ID="btn_inwardcell_no_Delete" runat="server" Font-Bold="true" ImageUrl="~/LibImages/no (2).jpg"
                                                        OnClick="btn_inwardcell_no_Delete_Click" />
                                                </center>
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
    </div>
    <div id="Divfspreadstatus" runat="server" class="popupstyle popupheight1" visible="false"
        style="height: 300em;">
        <asp:ImageButton ID="ImageButton8" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
            Style="height: 30px; width: 30px; position: absolute; margin-top: 36px; margin-left: 946px;"
            OnClick="btn_popclose5_Click" />
        <br />
        <div style="background-color: White; height: 350px; width: 915px; border: 5px solid #0CA6CA;
            border-top: 30px solid #0CA6CA; border-radius: 10px; margin-left: 30px; margin-top: 30px;">
            <br />
            <div>
                <center>
                    <fieldset id="Fieldset8" runat="server" visible="false">
                        <center>
                            <FarPoint:FpSpread ID="FpSpread3" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="1px">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </center>
                    </fieldset>
                    <br />
                    <asp:ImageButton ID="Buttonexit" runat="server" Font-Bold="true" ImageUrl="~/LibImages/save (2).jpg"
                        OnClick="Buttonexit_Click" />
                </center>
            </div>
            <br />
        </div>
    </div>
    <center>
        <asp:UpdatePanel ID="UpdatePanel19" runat="server">
            <ContentTemplate>
                <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 360px;
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
                                                <asp:ImageButton ID="btnerrclose" runat="server" Font-Bold="true" ImageUrl="~/LibImages/ok.jpg"
                                                    OnClick="btnerrclose_Click" Style="height: 28px; width: 65px;" />
                                            </center>
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
    <%--progressBar for Add--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpGoAdd">
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
    <%--progressBar for inwardEntry Popup--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanelButtonEvents">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for SureAdditionalPopup--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanelAdditonalPop">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="UpdateProgress3"
            PopupControlID="UpdateProgress3">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for CallNoPopup--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpdatePanelPopDes">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="UpdateProgress4"
            PopupControlID="UpdateProgress4">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for CallNoPopupEntry--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="UpdatePanelCalDesEntry">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender5" runat="server" TargetControlID="UpdateProgress5"
            PopupControlID="UpdateProgress5">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for NonBookLink--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="UpLnlNonBk">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender6" runat="server" TargetControlID="UpdateProgress6"
            PopupControlID="UpdateProgress6">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for NonBooksave--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="UpNonBkSave">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender7" runat="server" TargetControlID="UpdateProgress7"
            PopupControlID="UpdateProgress7">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for Status--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="UpLnkSts">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender8" runat="server" TargetControlID="UpdateProgress8"
            PopupControlID="UpdateProgress8">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for NonBooksave--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress9" runat="server" AssociatedUpdatePanelID="UpLnkAdd">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender9" runat="server" TargetControlID="UpdateProgress9"
            PopupControlID="UpdateProgress9">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for RackStatus--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress10" runat="server" AssociatedUpdatePanelID="UpRackStatus">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender10" runat="server" TargetControlID="UpdateProgress10"
            PopupControlID="UpdateProgress10">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for AdditionalDetails--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress11" runat="server" AssociatedUpdatePanelID="UpAddDet">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender11" runat="server" TargetControlID="UpdateProgress11"
            PopupControlID="UpdateProgress11">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for AdditionalDetails--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress12" runat="server" AssociatedUpdatePanelID="UpCallNoDet">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender12" runat="server" TargetControlID="UpdateProgress12"
            PopupControlID="UpdateProgress12">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for Upjournalaccno_go--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress13" runat="server" AssociatedUpdatePanelID="Upjournalaccno_go">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender13" runat="server" TargetControlID="UpdateProgress13"
            PopupControlID="UpdateProgress13">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for Upbook_go--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress14" runat="server" AssociatedUpdatePanelID="Upbook_go">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender14" runat="server" TargetControlID="UpdateProgress14"
            PopupControlID="UpdateProgress14">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
