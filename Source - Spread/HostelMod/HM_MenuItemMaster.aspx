<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="HM_MenuItemMaster.aspx.cs" Inherits="HM_MenuItemMaster" EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title></title>
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
        <script src="Styles/~/Scripts/jquery-latest.min.js" type="text/javascript"></script>
        <script type="text/javascript">
            function checkvalue() {
                var fl = 0;
                var id = document.getElementById("<%=SelectdptGrid.ClientID %>");
                var gridViewControls = id.getElementsByTagName("input");
                var len = id.rows.length;
                for (var i = 0; i < gridViewControls.length; i++) {
                    if (gridViewControls[i].name.indexOf("txt_quantity") > 1) {

                        if (gridViewControls[i].value == "") {

                            fl = 1;
                        }
                    }
                }
                if (fl == 1) {
                    alert('Please Fill All Values');

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
                idval = document.getElementById("<%=txt_hostelname1.ClientID %>").value;
                if (idval.trim() != "--Select--") {
                    id = document.getElementById("<%=txt_session1.ClientID %>").value;
                    if (id.trim() == "---Select---") {
                        id = document.getElementById("<%=txt_session1.ClientID %>");
                        id.style.borderColor = 'Red';
                        empty = "E";
                    }
                    id = document.getElementById("<%=ddl_menuname1.ClientID %>");
                    value1 = id.options[id.selectedIndex].text;
                    if (value1.trim().toUpperCase() == "SELECT") {
                        id.style.borderColor = 'Red';
                        empty = "E";
                    }
                    idval = document.getElementById("<%=txt_noofperson.ClientID %>").value;
                    if (idval.trim() == "") {
                        idval = document.getElementById("<%=txt_noofperson.ClientID %>");
                        idval.style.borderColor = 'Red';
                        empty = "E";
                    }
                    if (empty.trim() != "") {
                        return false;
                    }
                    else {
                        return true;
                    }
                }
                else {
                    idval = document.getElementById("<%=txt_hostelname1.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                }
            }

            function valid2() {
                var idval = "";
                var empty = "";
                var id = "";
                var value1 = "";
                idval = document.getElementById("<%=txt_hostelname1.ClientID %>").value;
                if (idval.trim() != "--Select--") {
                    idval.style.borderColor = 'Red';

                    id = document.getElementById("<%=txt_session1.ClientID %>").value;
                    if (id.trim() == "---Select---") {
                        value1.style.borderColor = 'Red';
                        empty = "E";
                    }
                    id = document.getElementById("<%=ddl_menuname1.ClientID %>");
                    value1 = id.options[id.selectedIndex].text;
                    if (value1.trim().toUpperCase() == "SELECT") {
                        value1.style.borderColor = 'Red';
                        empty = "E";
                    }

                    if (empty.trim() != "") {
                        return false;
                    }
                    else {
                        return true;
                    }

                }
                else {
                    empty = "E";
                }
                if (empty.trim() != "") {

                    return false;
                }
                else {
                    return true;
                }
            }



            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }

            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }

            function valid5() {
                var idval = "";
                var empty = "";
                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                }
            }

        
        </script>
    </head>
    <body>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <div>
                <asp:Label ID="lblcanteenmennuitem" runat="server" Style="color: Green;" Text="Menu Item Master"
                    CssClass="fontstyleheader"></asp:Label>
                <br />
                <br />
            </div>
            <div class="maindivstyle" style="height: 520px; width: 1000px;">
                <br />
                <table class="maintablestyle" style="width: 990px;">
                    <tr>
                        <td style="display: none;">
                            <asp:Label ID="lbl_hostelname" runat="server" Text="Mess Name"></asp:Label>
                        </td>
                        <td style="display: none;">
                            <asp:UpdatePanel ID="upp1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_hostelname" runat="server" CssClass="textbox textbox1" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                        width: 200px;">
                                        <asp:CheckBox ID="cb_hostelname" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_hostelname_CheckedChange" />
                                        <asp:CheckBoxList ID="cbl_hostelname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_hostelname_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_hostelname"
                                        PopupControlID="Panel4" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_basemessname" runat="server" Text="Mess Name"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_basemessname" runat="server" CssClass="textbox1 ddlheight2"
                                OnSelectedIndexChanged="ddl_basemessname_Selectedindexchange" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_menuname" runat="server" Text="Menu Name"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_menuname" runat="server" CssClass="textbox textbox1" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Height="150px" Width="150px">
                                        <asp:CheckBox ID="cb_menuname" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_menuname_CheckedChange" />
                                        <asp:CheckBoxList ID="cbl_menuname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_menuname_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_menuname"
                                        PopupControlID="Panel3" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_search" runat="server" Text="Search By"></asp:Label>
                        </td>
                        <%--
                    <td>
                        <asp:TextBox ID="txt_search" runat="server" CssClass="textbox textbox1"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="Getname1" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_search"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="panel">
                        </asp:AutoCompleteExtender>
                    </td>--%>
                        <td>
                            <asp:DropDownList ID="ddl_search" runat="server" CssClass="textbox1  ddlheight2"
                                Width="100px" OnSelectedIndexChanged="ddl_search_SelectedIndexChanged" AutoPostBack="True">
                                <asp:ListItem Value="0">Item Name</asp:ListItem>
                                <asp:ListItem Value="1">Menu Name</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_item" Visible="false" placeholder="Search Item Name" runat="server"
                                CssClass="textbox textbox1"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_item"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="txtsearchpan">
                            </asp:AutoCompleteExtender>
                            <asp:TextBox ID="txt_menu" Visible="false" placeholder="Search Menu Name" runat="server"
                                CssClass="textbox textbox1"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="Getmenu" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_menu"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="txtsearchpan">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:CheckBox ID="chk_option" Visible="false" runat="server" Text="Detail" />
                        </td>
                        <td>
                            <asp:Button ID="btn_go" runat="server" Text="Go" CssClass="textbox btn1" OnClick="btn_go_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btn_addnew" runat="server" Text="Add New" CssClass="textbox btn2"
                                OnClick="btn_addnew_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <div>
                    <%--style="width: 266px; float: left;"--%>
                    <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"
                        Font-Size="Medium"></asp:Label>
                </div>
                <div id="div1" runat="server" visible="false" style="width: 667px; height: 350px;"
                    class="spreadborder">
                    <br />
                    <FarPoint:FpSpread ID="Fpspread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Width="650px" Height="350px" OnCellClick="Cell_Click" OnPreRender="Fpspread1_render">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <br />
                <div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lblvalidation1" runat="server" Font-Bold="True" ForeColor="Red" Text="Please enter the report name"
                        Visible="false"></asp:Label>
                    <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" CssClass="textbox textbox1" Height="20px"
                        Width="180px" onkeypress="display()"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=". ">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox"
                        Text="Export To Excel" Width="127px" Height="30px" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        CssClass="textbox btn1 " Height="30px" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </div>
            </div>
            <%--Menu Item Master--%>
            <div id="popwindow" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 80px; margin-left: 464px"
                    OnClick="imagebtnpopclose1_Click" />
                <br />
                <br />
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; height: 450px; width: 950px;">
                    <br />
                    <div>
                        <asp:Label ID="lbl_menuitemmaster1" runat="server" Style="font-size: large; color: Green;"
                            Font-Bold="true" Text="Menu Item Master"></asp:Label>
                    </div>
                    <br />
                    <div style="width: 950px; height: 500px;">
                        <div style="float: left; width: 880px; height: 300px; margin-left: 21px;" class="spreadborder">
                            <br />
                            <asp:UpdatePanel ID="upp2" runat="server">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td style="display: none;">
                                                <asp:Label ID="lbl_hostelname1" runat="server" Text="Mess Name"></asp:Label>
                                            </td>
                                            <td style="display: none;">
                                                <asp:TextBox ID="txt_hostelname1" runat="server" CssClass="textbox textbox1 txtheight2"
                                                    onfocus="return myFunction(this)" ReadOnly="true">--Select--</asp:TextBox>
                                                <span style="color: Red;">*</span>
                                                <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                                    width: 200px" Visible="false">
                                                    <asp:CheckBox ID="cb_hostelname1" runat="server" OnCheckedChanged="cb_hostelname1_CheckedChange"
                                                        Text="Select All" AutoPostBack="true" />
                                                    <asp:CheckBoxList ID="cbl_hostelname1" runat="server" OnSelectedIndexChanged="cbl_hostelname1_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_hostelname1"
                                                    PopupControlID="Panel2" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </td>
                                            <td style="display: none;">
                                                <asp:Label ID="lbl_sessionname1" runat="server" Text="Session Name"></asp:Label>
                                            </td>
                                            <td style="display: none;">
                                                <asp:TextBox ID="txt_session1" runat="server" CssClass="textbox textbox1 txtheight2"
                                                    onfocus="return myFunction(this)" ReadOnly="true">--Select--</asp:TextBox>
                                                <span style="color: Red;">*</span>
                                                <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Style="height: 150px;
                                                    width: 150px;">
                                                    <asp:CheckBox ID="cb_session1" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cbl_session_SelectedIndexChanged" />
                                                    <asp:CheckBoxList ID="cbl_session1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_session1_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_session1"
                                                    PopupControlID="Panel1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </td>
                                            <td>
                                                <td>
                                                    <asp:Label ID="lbl_messname" runat="server" Text="Mess Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_messname" runat="server" CssClass="textbox1 ddlheight2"
                                                        OnSelectedIndexChanged="ddl_messname_Selectedindexchange" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblmenutype2" runat="server" Text="Menu Type"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_menutype" runat="server" CssClass="textbox1 ddlheight"
                                                        OnSelectedIndexChanged="ddl_menutype_OnSelectedIndexChanged" AutoPostBack="true">
                                                       <%-- <asp:ListItem Value="2">All</asp:ListItem>
                                                        <asp:ListItem Value="0">Veg</asp:ListItem>
                                                        <asp:ListItem Value="1">Non-Veg</asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                </td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_menuname1" runat="server" Text="Menu Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_menuname1" onfocus="return myFunction(this)" runat="server"
                                                    CssClass=" textbox1 ddlheight2" AutoPostBack="true">
                                                </asp:DropDownList>
                                                <span style="color: Red;">*</span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_noofperson" runat="server" Text="No of person"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_noofperson" runat="server" onfocus="return myFunction(this)"
                                                    MaxLength="5" Width="45px" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_noofperson"
                                                    FilterType="Numbers">
                                                </asp:FilteredTextBoxExtender>
                                                <span style="color: Red;">*</span>
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_addnew1" runat="server" Text="Add Items" CssClass="textbox btn2"
                                                    OnClick="btn_addnew1_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btn_addnew1" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <br />
                            <center>
                                <asp:GridView ID="SelectdptGrid" runat="server" AutoGenerateColumns="false" Width="800px"
                                    HeaderStyle-BackColor="#0CA6CA" HeaderStyle-ForeColor="White" OnRowDataBound="typegrid_OnRowDataBound"
                                    OnRowCommand="SelectdptGrid_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cb_select" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_itemcode" runat="server" Text='<%# Eval("ItemCode") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_itemname" runat="server" Text='<%# Eval("ItemName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="300px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Measure">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_itemmeasure" runat="server" Text='<%# Eval("Measure") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantity">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_quantity" runat="server" Style="text-align: center;" Text='<%# Eval("Quantity") %>'
                                                    Width="80px" CssClass="textbox"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_quantity"
                                                    FilterType="Custom,Numbers" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </center>
                            <br />
                            <center>
                            </center>
                        </div>
                        <div style="float: left; width: 900px; height: 100px; margin-top: 20px;">
                            <center>
                                <asp:Button ID="btn_update" runat="server" Text="Update" CssClass="textbox btn2"
                                    OnClientClick="return valid()" OnClick="btn_update_Click" onfocus="checkvalue()"
                                    Visible="false" />
                                <asp:Button ID="btn_delete" runat="server" Text="Delete" CssClass="textbox btn2"
                                    OnClientClick="return valid()" OnClick="btn_delete_Click" Visible="false" />
                                <asp:Button ID="btn_save1" runat="server" Text="Save" Visible="false" CssClass="textbox btn2"
                                    OnClick="btn_save1_Clcik" OnClientClick="return valid1()" onfocus="checkvalue()" />
                                <asp:Button ID="btn_additem2" runat="server" Text="Remove" CssClass="textbox btn2"
                                    OnClientClick="return valid1()" OnClick="btn_additem2_Clcik" />
                                <asp:Button ID="btn_exit2" runat="server" Text="Exit" CssClass="textbox btn2" OnClick="btn_exit2_Click" />
                            </center>
                        </div>
                    </div>
                </div>
            </div>
            <%--popwindow2--%>
            <div id="popwindow1" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 8px; margin-left: 433px;"
                    OnClick="imagebtnpopclose_Click" />
                <br />
                <div class="subdivstyle" style="background-color: White; height: 590px; width: 900px;">
                    <br />
                    <div>
                        <asp:Label ID="lbl_selectitem3" runat="server" Style="font-size: large; color: Green;"
                            Text="Select the Item" Font-Bold="true"></asp:Label>
                    </div>
                    <br />
                    <asp:UpdatePanel ID="upp4" runat="server">
                        <ContentTemplate>
                            <table class="maintablestyle">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_itemheader3" runat="server" Text="Item Header Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_itemheader3" runat="server" CssClass="textbox" ReadOnly="true"
                                            Width="106px" Height="20px">--Select--</asp:TextBox>
                                        <asp:Panel ID="p5" runat="server" CssClass="multxtpanel" Style="height: 200px; width: 160px;">
                                            <asp:CheckBox ID="cb_itemheader3" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_itemheader3_CheckedChange" />
                                            <asp:CheckBoxList ID="cbl_itemheader3" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_itemheader_SelectedIndexChange">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupExt5" runat="server" TargetControlID="txt_itemheader3"
                                            PopupControlID="p5" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_subheadername" runat="server" Text="Sub Header Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_subheadername" runat="server" Height="20px" CssClass="textbox textbox1"
                                                    ReadOnly="true" Width="120px">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel5" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                    height: 190px;">
                                                    <asp:CheckBox ID="cb_subheadername" runat="server" Width="100px" OnCheckedChanged="cb_subheadername_CheckedChange"
                                                        Text="Select All" AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="cbl_subheadername" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_subheadername_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_subheadername"
                                                    PopupControlID="Panel5" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_itemtype3" runat="server" Text="Item Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="Upp5" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_itemname3" runat="server" CssClass="textbox" ReadOnly="true"
                                                    Width="106px" Height="20px">--Select--</asp:TextBox>
                                                <asp:Panel ID="p51" runat="server" CssClass="multxtpanel" Style="height: 300px; width: 200px;">
                                                    <asp:CheckBox ID="chk_pop2itemtyp" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="chkitemtyp" />
                                                    <asp:CheckBoxList ID="chklst_pop2itemtyp" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklstitemtyp">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupExt51" runat="server" TargetControlID="txt_itemname3"
                                                    PopupControlID="p51" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span>Search By</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_type" runat="server" CssClass="textbox1 ddlstyle" Height="30px"
                                            OnSelectedIndexChanged="ddl_type_SelectedIndexChanged" AutoPostBack="True" Width="115px">
                                            <asp:ListItem Value="0">Item Name</asp:ListItem>
                                            <asp:ListItem Value="1">Item Code</asp:ListItem>
                                            <asp:ListItem Value="2">Item Header</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_searchby" Visible="false" placeholder="Search Item Name" runat="server"
                                            CssClass="textbox textbox1" Height="20px"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchby"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="txtsearchpan">
                                        </asp:AutoCompleteExtender>
                                        <asp:TextBox ID="txt_searchitemcode" Visible="false" placeholder="Search Item Code"
                                            runat="server" CssClass="textbox textbox1"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getitemcode" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchitemcode"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="txtsearchpan">
                                        </asp:AutoCompleteExtender>
                                        <asp:TextBox ID="txt_searchheadername" Visible="false" placeholder="Search Item Header"
                                            runat="server" CssClass="textbox textbox1"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getitemheader" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchheadername"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="txtsearchpan">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_go3" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go3_Click" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btn_go3" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <br />
                    <asp:Label ID="lbl_error3" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"
                        Font-Size="Medium"></asp:Label>
                    <center>
                        <span>Menu Name: </span>
                        <asp:Label ID="menulbl" runat="server" ForeColor="#0099CC
"></asp:Label></center>
                    <br />
                    <div id="div2" runat="server" visible="false" style="width: 850px; height: 318px;
                        background-color: White;" class="spreadborder">
                        <div style="width: 550px; float: left;">
                            <br />
                            <asp:DataList ID="gvdatass" runat="server" Font-Size="Medium" RepeatColumns="4" Width="500px"
                                ForeColor="#333333">
                                <AlternatingItemStyle BackColor="White" />
                                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                <ItemStyle BackColor="#E3EAEB" BorderWidth="1px" Height="0px" />
                                <ItemTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="CheckBox2" AutoPostBack="true" OnCheckedChanged="selectedmenuchk"
                                                    runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_itemname" ForeColor="Green" runat="server" Text='<%# Eval("ItemName") %>'></asp:Label>
                                                <asp:Label ID="lbl_itemcode" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("ItemCode") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="CheckBox1" Visible="false" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblitemheadername" ForeColor="Green" Visible="false" runat="server"
                                                    Text='<%# Eval("ItemHeaderName") %>'></asp:Label>
                                                <asp:Label ID="lbl_itemheadercode" ForeColor="Red" Visible="false" runat="server"
                                                    Text='<%# Eval("ItemHeaderCode") %>'></asp:Label>
                                                <asp:Label ID="lbl_measureitem" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("ItemUnit") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <SelectedItemStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                            </asp:DataList>
                        </div>
                        <div style="width: 200px; float: right;">
                            <br />
                            <asp:GridView ID="selectitemgrid" runat="server" HeaderStyle-BackColor="#0CA6CA"
                                AutoGenerateColumns="false" HeaderStyle-ForeColor="White">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="snogv" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Name">
                                        <ItemTemplate>
                                            <asp:Label ID="itemnamegv" runat="server" Text='<%# Eval("Item Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle BorderWidth="1px" Width="200px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Code" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="itemcodegv" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("ItemCode") %>'> </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:CheckBox ID="CheckBox1" Visible="false" runat="server" />--%>
                                    <asp:TemplateField HeaderText="Item Headername" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_headername" ForeColor="Green" Visible="false" runat="server" Text='<%# Eval("Header Name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Headercode" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_itemheadercode" ForeColor="Red" Visible="false" runat="server"
                                                Text='<%# Eval("Header code") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Unit" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_measureitem" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("Item unit") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:Label ID="itemcodegv" runat="server" Text='<%# Eval("item_code") %>'></asp:Label>--%>
                                    <%-- </asp:TemplateField>--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <br />
                    <center>
                        <asp:Button ID="btn_itemsave4" runat="server" Text="Save" CssClass="textbox btn2"
                            OnClientClick="return valid5()" OnClick="btn_itemsave4_Click" />
                        <asp:Button ID="btn_conexist4" runat="server" Text="Exit" CssClass="textbox btn2"
                            OnClick="btn_conexit4_Click" />
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
                                            <asp:Button ID="btn_yes" Visible="false" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btn_sureyes_Click" Text="yes" runat="server" />
                                            <asp:Button ID="btn_yes1" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btn_sureyes1_Click" Visible="false" Text="yes" runat="server" />
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
