<%@ Page Title="" Language="C#" MasterPageFile="~/InventoryMod/inventorysite.master"
    AutoEventWireup="true" CodeFile="inv_opening_stock.aspx.cs" Inherits="inv_opening_stock" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title></title>
        <style type="text/css">
            .watermark
            {
                color: #999999;
            }
        </style>
        <%--d69bff--%>
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    </head>
    <body>
        <script type="text/javascript">
            function Test() {
                var id = "";
                var value1 = "";
                var idval = "";
                var empty = "";
                id = document.getElementById("<%=txt_openquantity1.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_openquantity1.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_quantitymeasure1.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_quantitymeasure1.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }

                id = document.getElementById("<%=txt_rateper1.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_rateper1.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }

                id = document.getElementById("<%=ddl_itemname1.ClientID %>");
                value1 = id.options[id.selectedIndex].text;
                if (value1.trim().toUpperCase() == "SELECT") {
                    empty = "E";
                    id = document.getElementById("<%=ddl_itemname1.ClientID %>");
                    id.style.borderColor = 'Red';
                }
                id = document.getElementById("<%= ddl_itemheadername.ClientID %>");
                value1 = id.options[id.selectedIndex].text;
                if (value1.trim().toUpperCase() == "SELECT") {
                    empty = "E";
                    id = document.getElementById("<%= ddl_itemheadername.ClientID %>");
                    id.style.borderColor = 'Red';
                }
                id = document.getElementById("<%= ddl_subheadername.ClientID %>");
                value1 = id.options[id.selectedIndex].text;
                if (value1.trim().toUpperCase() == "SELECT") {
                    empty = "E";
                    id = document.getElementById("<%= ddl_subheadername.ClientID %>");
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

            /*rate per unit calculatation*/
            function calculation() {
                var openstack = document.getElementById("<%=txt_openquantity1.ClientID %>").value;
                var rateperunit = document.getElementById("<%=txt_rateper1.ClientID %>").value;
                document.getElementById("<%=txt_total1.ClientID %>").value = parseFloat(parseFloat(openstack) * parseFloat(rateperunit));
            }
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }

        </script>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <span style="color: #008000;" class="fontstyleheader ">Opening Stock Entry</span>
            <br />
            <br />
        </center>
        <center>
            <div class="maindivstyle" style="height: 580px; width: 1000px;">
                <br />
                <table class="maintablestyle" style="width: 891px; height: 78px;">
                    <tr>
                        <td>
                            <asp:Label ID="lbl_itemheadername" runat="server" Text="Item Header Name"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_headername" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                        <asp:CheckBox ID="cb_headername" runat="server" OnCheckedChanged="cb_headername_CheckedChange"
                                            Text="Select All" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cbl_headername" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_headername_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txt_headername"
                                        PopupControlID="pbatch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Sub Header Name"></asp:Label>
                       </td>
                        <td colspan="1">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_subheadername" runat="server" Height="20px" CssClass="textbox textbox1"
                                        ReadOnly="true" Width="120px">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                        height: 190px;">
                                        <asp:CheckBox ID="cb_subheadername" runat="server" Width="100px" OnCheckedChanged="cb_subheadername_CheckedChange"
                                            Text="Select All" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cbl_subheadername" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_subheadername_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_subheadername"
                                        PopupControlID="Panel2" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_itemname" runat="server" Text="Item Name"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_itemname" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                        width: 150px;">
                                        <asp:CheckBox ID="cb_itemname" runat="server" OnCheckedChanged="cb_itemname_CheckedChange"
                                            Text="Select All" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cbl_itemname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_itemname_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_itemname"
                                        PopupControlID="Panel1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="Label2" runat="server" Text="Serach By"></asp:Label>
                     <%--   </td>
                        <td>--%>
                            <asp:DropDownList ID="ddl_type" runat="server" CssClass="textbox ddlstyle ddlheight1 "
                                OnSelectedIndexChanged="ddl_type_SelectedIndexChanged" AutoPostBack="True">
                                <asp:ListItem Value="0">Item Name</asp:ListItem>
                                <asp:ListItem Value="1">Item Code</asp:ListItem>
                            </asp:DropDownList>
                      <%--  </td>
                        <td>--%>
                            <asp:TextBox ID="txt_searchby" Visible="false" placeholder="Search Item name" runat="server"
                                CssClass="textbox textbox1"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchby"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="txtsearchpan">
                            </asp:AutoCompleteExtender>
                            <%--  <asp:TextBoxWatermarkExtender ID="wateritemname" runat="server" TargetControlID="txt_searchby"
                            WatermarkText="Search Item name" WatermarkCssClass="watermark textbox textbox1">
                        </asp:TextBoxWatermarkExtender>--%>
                            <asp:TextBox ID="txt_searchitemcode" Visible="false" runat="server" CssClass="textbox textbox1"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="Getitemcode" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchitemcode"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="txtsearchpan">
                            </asp:AutoCompleteExtender>
                            <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txt_searchitemcode"
                                WatermarkText="Search Item Code" WatermarkCssClass="watermark textbox textbox1">
                            </asp:TextBoxWatermarkExtender>
                        </td>
                        <%--<td>
                            <asp:CheckBox ID="cb_datewise" runat="server" Text="Date Wise" AutoPostBack="true"
                                OnCheckedChanged="cb_datewise_change" Visible="false" />
                            <asp:Label ID="lbl_fromdate" runat="server" Text="From Date" Visible="false"></asp:Label>
                        </td>

                        <td>
                            <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox textbox1 txtheight2"
                                AutoPostBack="true" OnTextChanged="txt_fromdate_Textchanged" Visible="false"></asp:TextBox>
                            <asp:CalendarExtender ID="calfromdate" TargetControlID="txt_fromdate" Format="dd/MM/yyyy"
                                runat="server" CssClass="cal_Theme1 ajax__calendar_active">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="lbl_todate" runat="server" Text="To Date" Visible="false"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox textbox1 txtheight2"
                                AutoPostBack="true" OnTextChanged="txt_todate_Textchanged" Visible="false"></asp:TextBox>
                            <asp:CalendarExtender ID="caltodate" TargetControlID="txt_todate" Format="dd/MM/yyyy"
                                runat="server" CssClass="cal_Theme1 ajax__calendar_active">
                            </asp:CalendarExtender>
                        </td>--%>
                        <%-- </tr>
                    <tr>--%>
                        <td>
                            <%-- <fieldset class="maindivstyle" style="height: 13px;">--%>
                            <asp:RadioButton ID="rb_store" runat="server" Text="Store" Checked="true" GroupName="stormess"
                                OnCheckedChanged="rb_store_OnCheckedChanged" AutoPostBack="true" />
                               
                            <asp:RadioButton ID="rb_mess" runat="server" Text="Mess" GroupName="stormess" OnCheckedChanged="rb_mess_OnCheckedChanged"
                                AutoPostBack="true" />
                             </td>
                             <td>
                            <asp:RadioButton ID="rb_dept" runat="server" Text="Department" GroupName="stormess"
                                OnCheckedChanged="rb_dept_OnCheckedChanged" AutoPostBack="true" />
                            <%--  </fieldset>--%>
                            <asp:Label ID="lbl_store" runat="server" Visible="false" Text=""></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_store" runat="server" Visible="false" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel3" runat="server" Visible="false" CssClass="multxtpanel" Style="height: 200px;
                                        width: 150px;">
                                        <asp:CheckBox ID="cb_storeb" runat="server" OnCheckedChanged="cb_storeb_oncheckedchange"
                                            Text="Select All" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cbl_storeb" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_storeb_onselectedindexchange">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_store"
                                        PopupControlID="Panel3" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_mess" Visible="false" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel4" runat="server" Visible="false" CssClass="multxtpanel" Style="height: 200px;
                                        width: 150px;">
                                        <asp:CheckBox ID="cb_messb" runat="server" OnCheckedChanged="cb_messb_oncheckedchange"
                                            Text="Select All" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cbl_messb" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_messb_selectedindexchange">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_mess"
                                        PopupControlID="Panel4" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel ID="upp6" Visible="false" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_deptname" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                    <asp:Panel ID="p6" runat="server" CssClass="multxtpanel" Style="height: 200px; width: 150px;">
                                        <asp:CheckBox ID="cb_deptname" runat="server" OnCheckedChanged="cb_deptname_oncheckedchange"
                                            Text="Select All" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cbl_deptname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_deptname_onselectedindexchange">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_deptname"
                                        PopupControlID="p6" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td colspan="5">
                            <asp:CheckBox ID="cb_direct" runat="server" Visible="false" Text="Direct Import"
                                AutoPostBack="True" OnCheckedChanged="cb_directimport_CheckedChanged" />
                            <asp:FileUpload ID="FileUpload1" runat="server" CssClass="textbox" ForeColor="White" />
                            <asp:Button ID="btn_import" runat="server" Text="Import" CssClass="textbox" OnClick="btn_import_Click"
                                Width="80px" Height="30px" />
                            <asp:Button ID="btn_go" Text="Go" runat="server" CssClass="textbox btn1" OnClick="btn_go_Click" />
                            <asp:Button ID="btn_addnew" Text="Add New" runat="server" CssClass="textbox btn2"
                                OnClick="btn_addnew_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                <br />
                <center>
                    <FarPoint:FpSpread ID="spreadimport" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Visible="false" Width="900px" Height="300px" VerticalScrollBarPolicy="Never"
                        HorizontalScrollBarPolicy="Never" Style="background: white;" CssClass="spreadborder">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" Visible="false" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" Visible="false">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </center>
                <br />
                <center>
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButton ID="rdb_store1" Visible="false" Text="Store" runat="server" AutoPostBack="true"
                                    GroupName="co" OnCheckedChanged="rdb_store1_Click" />
                                <asp:RadioButton ID="rdb_hostel1" Visible="false" Text="Mess" runat="server" AutoPostBack="true"
                                    GroupName="co" OnCheckedChanged="rdb_Hostel1_Click" />
                                <asp:RadioButton ID="rdb_dept1" Visible="false" Text="Department" runat="server"
                                    AutoPostBack="true" GroupName="co" OnCheckedChanged="rdb_dept1_Click" />
                            </td>
                            <td>
                                <asp:Label ID="lbl_storename1" Visible="false" Text="" runat="server"></asp:Label>
                                <asp:Label ID="lbl_hostelname1" Visible="false" Text="" runat="server"></asp:Label>
                                <asp:Label ID="lbl_dept1" Visible="false" Text="" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_storename1" Visible="false" Height="30" runat="server"
                                    CssClass="textbox1 ddlheight2">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddl_Hostelname1" Visible="false" Height="30" runat="server"
                                    CssClass=" textbox1 ddlheight2 ">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddl_deptname1" Visible="false" Height="30" runat="server" CssClass="textbox1 ddlheight2">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btn_save2" Text="Save" Visible="false" runat="server" CssClass="textbox btn2"
                                    OnClick="btn_save2_Click" />
                            </td>
                        </tr>
                    </table>
                </center>
                <center>
                    <div id="div1" runat="server" visible="false" style="width: 850px; height: 320px;
                        overflow: auto; border: 1px solid Gray; border-radius: 10px; background-color: White;
                        box-shadow: 0px 0px 8px #999999;">
                        <br />
                        <FarPoint:FpSpread ID="Fpspread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Width="760px" Style="height: 350px; overflow: auto; background-color: White;"
                            OnCellClick="Cell_Click" OnPreRender="Fpspread1_render">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                </center>
                <br />
                <center>
                    <div id="rptprint" runat="server" visible="false">
                        <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                            Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" Width="180px" CssClass="textbox textbox1"
                            onkeypress="display()"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,. ">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox btn1"
                            Text="Export To Excel" Width="127px" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            Width="60px" CssClass="textbox btn1" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="popwindow" runat="server" visible="false" class="popupstyle" style="height: 43em;">
                <asp:ImageButton ID="img_btn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 70px; margin-left: 243px;"
                    OnClick="img_btnpopclose_Click" />
                <br />
                <br />
                <br />
                <br />
                <center>
                    <div class="subdivstyle" style="background-color: White; height: 480px; width: 510px;">
                        <br />
                        <center>
                            <div>
                                <span style="color: #008000; font-size: large;">Opening Stock Entry</span>
                            </div>
                            <div style="text-align: right;">
                                <asp:CheckBox ID="cb_show" runat="server" AutoPostBack="true" OnCheckedChanged="cb_show_Change"
                                    Text="Show All the Items" />
                            </div>
                        </center>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_itemheadername1" Text="Item Header Name" runat="server"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddl_itemheadername" onfocus="return myFunction(this)" runat="server"
                                        CssClass="textbox textbox1 ddlheight5" AutoPostBack="true" OnSelectedIndexChanged="ddl_itemheadername1_Change">
                                    </asp:DropDownList>
                                    <span style="color: red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_subheadername" Text="Sub Header Name" runat="server"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddl_subheadername" onfocus="return myFunction(this)" runat="server"
                                        CssClass="textbox textbox1 ddlheight5" AutoPostBack="true" OnSelectedIndexChanged="ddl_subheadername_Change">
                                    </asp:DropDownList>
                                    <span style="color: red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_itemname1" Text="Item Name" runat="server"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddl_itemname1" onfocus="return myFunction(this)" runat="server"
                                        CssClass="textbox textbox1 ddlheight5" AutoPostBack="true" OnSelectedIndexChanged="ddl_itemname1_Change">
                                    </asp:DropDownList>
                                    <span style="color: red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_quantitymeasure1" Text="Quantity Measure" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_quantitymeasure1" runat="server" Enabled="false" Width="120px"
                                        CssClass="textbox textbox1" ReadOnly="true"></asp:TextBox>
                                    <span style="color: red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_openquantity1" Text="Opening Quantity" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_openquantity1" runat="server" Width="120px" CssClass="textbox textbox1"
                                        onfocus="return myFunction(this)" onchange="return calculation()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="filtertextbox" runat="server" TargetControlID="txt_openquantity1"
                                        FilterType="Numbers,Custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_rateper1" Text="Rate Per Unit" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_rateper1" runat="server" Width="120px" CssClass="textbox textbox1"
                                        onchange="return calculation()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="filter" runat="server" TargetControlID="txt_rateper1"
                                        FilterType="Numbers,Custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_sailingprize" Text="Sailing Prize" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_sailingprize" runat="server" Width="120px" CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_sailingprize"
                                        FilterType="Numbers,Custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_total1" Text="Total" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_total1" runat="server" Width="120px" CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_total1"
                                        FilterType="Numbers,Custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr style="display: none;">
                                <td>
                                    <asp:Label ID="lbl_opendate1" Text="Opening Date" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_opendate1" runat="server" Width="120px" CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:CalendarExtender ID="cal1" TargetControlID="txt_opendate1" Format="dd/MM/yyyy"
                                        runat="server" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                    <span style="color: red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rdb_store" Text="Store" runat="server" AutoPostBack="true" GroupName="co"
                                        OnCheckedChanged="rdb_store_Click" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_hostel" Text="Mess Name" runat="server" AutoPostBack="true"
                                        GroupName="co" OnCheckedChanged="rdb_Hostel_name" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_dept" Text="Department" runat="server" AutoPostBack="true"
                                        GroupName="co" OnCheckedChanged="rdb_dept_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_storename" Visible="false" Text="Store Name" runat="server"></asp:Label>
                                    <asp:Label ID="lbl_hostelname" Visible="false" Text="Mess Name" runat="server"></asp:Label>
                                    <asp:Label ID="lbl_dept" Visible="false" Text="Department" runat="server"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddl_storename" Width="140px" Visible="false" runat="server"
                                        CssClass="textbox ddlstyle ddlheight">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddl_Hostelname" Width="140px" Visible="false" runat="server"
                                        CssClass="textbox ddlstyle ddlheight">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddl_deptname" Visible="false" runat="server" CssClass="textbox ddlstyle ddlheight5">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <br />
                                    <center>
                                        <asp:Button ID="btn_update" runat="server" Text="Update" CssClass="textbox btn2"
                                            OnClick="btn_update_Click" Visible="false" OnClientClick="return Test()" />
                                        <asp:Button ID="btn_delete" runat="server" Text="Delete" CssClass="textbox btn2"
                                            OnClick="btn_delete_Click" Visible="false" OnClientClick="return Test()" />
                                        <asp:Button ID="btn_save1" Text="Save" Visible="false" runat="server" CssClass="textbox btn2"
                                            OnClientClick="return Test()" OnClick="btn_save1_Click" />
                                        <asp:Button ID="btn_exit1" Text="Exit" runat="server" CssClass="textbox btn2" OnClick="btn_exit1_Click" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="alertmessage" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                        <asp:Label ID="lbl_alerterror" Visible="false" runat="server" Text="" Style="color: Red;"
                                            Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btn_errorclose_Click" Text="OK" runat="server" />
                                            <%-- <asp:ImageButton ID="btn_errorclose" Style="height: 40px; width: 40px;" OnClick="btn_errorclose_Click"
                                                ImageUrl="~/images/okimg.jpg" runat="server" />--%>
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
                                            <asp:Button ID="btn_yes" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btn_sureyes_Click" Text="yes" runat="server" />
                                            <asp:Button ID="btn_no" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
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
        </form>
    </body>
    </html>
</asp:Content>
