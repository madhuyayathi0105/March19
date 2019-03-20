<%@ Page Title="" Language="C#" MasterPageFile="~/InventoryMod/inventorysite.master"
    AutoEventWireup="true" CodeFile="Item_master.aspx.cs" Inherits="Item_master" %>

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
        <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
        <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
        <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
        <script type="text/javascript">
            function change1(id) {
                var value1 = id.value;
                if (value1.trim().toUpperCase() == "OTHERS") {
                    var idval = document.getElementById("<%=txt_itemheadername1.ClientID %>");
                    idval.style.display = "block";
                }
                else {
                    var idval = document.getElementById("<%=txt_itemheadername1.ClientID %>");
                    idval.style.display = "none";
                }
            }
            function change3(id) {
                var value1 = id.value;
                if (value1.trim().toUpperCase() == "OTHERS") {
                    var idval = document.getElementById("<%=txt_subheader.ClientID %>");
                    idval.style.display = "block";
                }
                else {
                    var idval = document.getElementById("<%=txt_subheader.ClientID %>");
                    idval.style.display = "none";
                }

            }
            function change2(id) {
                var value1 = id.value;
                if (value1.trim().toUpperCase() == "OTHERS") {
                    var idval = document.getElementById("<%=txt_unit1.ClientID %>");
                    idval.style.display = "block";
                }
                else {
                    var idval = document.getElementById("<%=txt_unit1.ClientID %>");
                    idval.style.display = "none";
                }

            }
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }

            function valid() {
                var id = "";
                var value1 = "";
                var idval = "";
                var empty = "";
                var id1 = "";
                id = document.getElementById("<%=ddl_itemheadername1.ClientID %>");
                value1 = id.options[id.selectedIndex].text;
                if (value1.trim().toUpperCase() == "OTHERS") {
                    idval = document.getElementById("<%=txt_itemheadername1.ClientID %>").value;
                    if (idval.trim() == "") {
                        idval = document.getElementById("<%=txt_itemheadername1.ClientID %>");
                        idval.style.borderColor = 'Red';
                        empty = "E";
                    }
                }
                else if (value1.trim().toUpperCase() == "SELECT") {
                    idval = document.getElementById("<%=ddl_itemheadername1.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_itemname1.ClientID %>").value;
                if (id.trim() == "") {
                    empty = "E";
                    id = document.getElementById("<%=txt_itemname1.ClientID %>");
                    id.style.borderColor = 'Red';
                }
                id = document.getElementById("<%=ddl_unit1.ClientID %>");
                value1 = id.options[id.selectedIndex].text;
                if (value1.trim().toUpperCase() == "OTHERS") {
                    idval = document.getElementById("<%=txt_unit1.ClientID %>").value;
                    if (idval.trim() == "") {
                        empty = "E";
                        idval = document.getElementById("<%=txt_unit1.ClientID %>");
                        idval.style.borderColor = 'Red';
                    }
                }
                else if (value1.trim().toUpperCase() == "SELECT") {
                    empty = "E";
                    idval = document.getElementById("<%=ddl_unit1.ClientID %>");
                    idval.style.borderColor = 'Red';
                }
                //                id1 = document.getElementById("<%=ddlpopdefaultstore.ClientID %>").value;
                //                if (id1.trim() == "Select") {
                //                    empty = "E";
                //                    id1 = document.getElementById("<%=ddlpopdefaultstore.ClientID %>");
                //                    id1.style.borderColor = 'Red';
                //                }
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

            function get(txt1, idval, value1) {
                idval = document.getElementById("<%=ddl_itemheadername1.ClientID %>").value;
                value1 = document.getElementById("<%=ddl_subheader.ClientID %>").value;
                $.ajax({
                    type: "POST",
                    url: "Item_master.aspx/CheckUserName",
                    data: '{StoreName: "' + txt1 + '",ItemHeadName: "' + idval + '",ItemSubHeadName: "' + value1 + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnSuccess,
                    failure: function (response) {
                        alert(response);
                    }
                });
            }
            function OnSuccess(response) {
                var mesg = $("#msg1")[0];
                switch (response.d) {
                    case "0":
                        mesg.style.color = "green";
                        mesg.innerHTML = "Item Name not exist";
                        break;
                    case "1":
                        mesg.style.color = "green";
                        document.getElementById('<%=txt_itemname1.ClientID %>').value = "";
                        mesg.innerHTML = "Item Name available";
                        break;
                    case "2":
                        mesg.style.color = "red";
                        mesg.innerHTML = "Please Enter Item Name";
                        break;
                    case "error":
                        mesg.style.color = "red";
                        mesg.innerHTML = "Error occurred";
                        break;
                }
            }

        </script>
    </head>
    <body>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <br />
                <asp:Label ID="Label1" runat="server" Style="color: Green;" Text="Item Master" CssClass="fontstyleheader"></asp:Label>
                <br />
                <br />
            </center>
        </div>
        <center>
            <div class="maindivstyle" style="height: 530px; width: 1000px;">
                <br />
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lbl_inventoryusername" runat="server" Text="Inventory User Name:"
                                Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_itemheadername" runat="server" Text="Item Header Name"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_headername" runat="server" Height="20px" CssClass="textbox textbox1"
                                        ReadOnly="true" Width="120px">--Select--</asp:TextBox>
                                    <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                        height: 190px;">
                                        <asp:CheckBox ID="cb_headername" runat="server" Width="100px" OnCheckedChanged="cb_headername_CheckedChange"
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
                            <asp:Label ID="lbl_subheadername" runat="server" Text="Sub Header/Brand Name"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_subheadername" runat="server" Height="20px" CssClass="textbox textbox1"
                                        ReadOnly="true" Width="136px">--Select--</asp:TextBox>
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
                                    <asp:TextBox ID="txt_itemname" runat="server" Height="20px" CssClass="textbox textbox1"
                                        ReadOnly="true" Width="120px">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Style="height: 245px;
                                        width: 171px;">
                                        <asp:CheckBox ID="cb_itemname" runat="server" Width="100px" OnCheckedChanged="cb_itemname_CheckedChange"
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
                        <td>
                            <asp:Label ID="search" runat="server" Text="Search By"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_type" runat="server" CssClass="textbox1  ddlheight3" OnSelectedIndexChanged="ddl_type_SelectedIndexChanged"
                                AutoPostBack="True">
                                <asp:ListItem Value="0">Item Name</asp:ListItem>
                                <asp:ListItem Value="1">Item Code</asp:ListItem>
                                <asp:ListItem Value="2">Item Header</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_searchby" Visible="false" placeholder="Search Item Name" runat="server"
                                CssClass="textbox textbox1  txtheight3"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchby"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="txtsearchpan">
                            </asp:AutoCompleteExtender>
                            <asp:TextBox ID="txt_searchitemcode" Visible="false" placeholder="Search Item Code"
                                runat="server" CssClass="textbox textbox1 txtheight3"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="Getitemcode" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchitemcode"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="txtsearchpan">
                            </asp:AutoCompleteExtender>
                            <asp:TextBox ID="txt_searchheadername" Visible="false" placeholder="Search Header Name"
                                runat="server" CssClass="textbox textbox1 txtheight3"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="Getitemheader" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchheadername"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="txtsearchpan">
                            </asp:AutoCompleteExtender>
                        </td>
                        <%--<td colspan="5">
                        <asp:CheckBox ID="cb_direct" runat="server" Text="Direct Import" AutoPostBack="True"
                            OnCheckedChanged="cb_directimport_CheckedChanged" />
                        <asp:FileUpload ID="FileUpload1" runat="server" CssClass="textbox" ForeColor="White" />
                        <asp:Button ID="btn_import" runat="server" Text="Import" CssClass="textbox" OnClick="btn_import_Click"
                            Width="50px" Height="30px" />
                    </td>--%>
                        <td>
                            <asp:RadioButton ID="rdb_saftyitemonly" Text="Safty Item Only" Style="display: none;"
                                runat="server" GroupName="same" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rdb_btnall" Text="All" runat="server" Style="display: none;"
                                GroupName="same" />
                        </td>
                        <td>
                            <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                            <asp:Button ID="btn_addnew" runat="server" Text="Add New" CssClass="textbox btn2"
                                OnClick="btn_addnew_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <center>
                    <div>
                        <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                    </div>
                </center>
                <div id="div1" runat="server" visible="false" style="width: 767px; height: 350px;
                    box-shadow: 0px 0px 8px #999999;" class="reportdivstyle">
                    <br />
                    <FarPoint:FpSpread ID="Fpspread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Width="950px" Height="350px" OnCellClick="Cell_Click" OnPreRender="Fpspread1_render">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <br />
                <div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                        Visible="false"></asp:Label>
                    <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display()"
                        CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Export To Excel"
                        Width="127px" CssClass="textbox btn1" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        CssClass="textbox btn1" Width="60px" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </div>
            </div>
            <div id="poperrjs" runat="server" visible="false" class="popupstyle" style="height: 45em;">
                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 11px; margin-left: 439px;"
                    OnClick="imagebtnpopclose1_Click" />
                <br />
                <%--            <br />--%>
                <div class="subdivstyle" style="background-color: White; height: 590px; width: 900px;">
                    <br />
                    <center>
                        <asp:Label ID="lbl_itementry" runat="server" Style="color: Green;" Text="Item Entry"
                            Font-Bold="true" CssClass="fontstyleheader"></asp:Label>
                    </center>
                    <br />
                    <div align="center" style="overflow: auto; width: 860px; height: 456px; border-radius: 10px;
                        border: 1px solid Gray;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_itemheadername1" Width="135px" runat="server" Text="Item Header Name"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddl_itemheadername1" Width="140px" CssClass="textbox textbox1 ddlheight4"
                                        runat="server" Style="float: left;" onfocus="return myFunction(this)" onchange="change1(this)">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_itemheadername1" CssClass="textbox textbox1" onfocus="return myFunction(this)"
                                        runat="server" Style="width: 200px; display: none; float: left;"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_itemheadername1"
                                        FilterType="UppercaseLetters,lowercaseletters,custom" ValidChars=" &-">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: Red; float: left;">*</span>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_itemcode1" runat="server" Text="Item Code"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_itemcode1" Width="120px" CssClass="textbox textbox1" Enabled="false"
                                        runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_itemsubheader" Width="135px" runat="server" Text="Sub Header/Brand Name"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddl_subheader" Width="140px" CssClass="textbox textbox1 ddlheight4"
                                        runat="server" onfocus="return myFunction(this)" Style="float: left;" onchange="change3(this)">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_subheader" CssClass="textbox textbox1" onfocus="return myFunction(this)"
                                        runat="server" Style="width: 200px; display: none; float: left;"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txt_subheader"
                                        FilterType="UppercaseLetters,lowercaseletters,custom" ValidChars=" &-">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: Red; float: left;">*</span>
                                </td>
                                <td>
                                    <%--<span style="color: Red; float: left;">*</span>--%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <asp:Label ID="lbl_itemacronym1" runat="server" Width="130px" Text="Item Acronym"></asp:Label>
                                    <asp:TextBox ID="txt_itemacronym1" CssClass="textbox textbox1 txtheight3" runat="server"
                                        Style="text-transform: uppercase"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_itemacronym1"
                                        FilterType="UppercaseLetters,lowercaseletters,custom" ValidChars="">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Label ID="lbl_itemname1" Width="80px" runat="server" Text="Item Name"></asp:Label>
                                    <asp:TextBox ID="txt_itemname1" CssClass="textbox textbox1 txtheight3" runat="server"
                                        onfocus="return myFunction(this)" onblur="return get(this.value)"></asp:TextBox>
                                    <%--<asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_itemname1"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txt_itemname1">
                                </asp:AutoCompleteExtender>--%>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_itemname1"
                                        FilterType="UppercaseLetters,lowercaseletters,Numbers,custom" ValidChars=" -_()[]{}';:/\<>,.!@$%^&()_+}{][';,.">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: Red;">*</span> <span style="" id="msg1"></span>
                                    <asp:CheckBox ID="itemtamil" runat="server" AutoPostBack="true" Text="Item In Tamil"
                                        OnCheckedChanged="itemtamil_OnCheckedChanged" />
                                    <asp:TextBox ID="Textitemtamil" runat="server" AutoPostBack="true" Width="171px"
                                        Height="20px" CssClass="textbox txtheight2" Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                            <%--<tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <tr style="display: none;">
                            <td>
                                <asp:Label ID="lblpopitemnametamil" runat="server" Text="Item Name in Tamil"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_itemname1tamil" Width="135px" CssClass="textbox textbox1" runat="server"></asp:TextBox>
                            </td>
                        </tr>--%>
                            <tr>
                                <td colspan="8">
                                    <asp:Label ID="lbl_popmodel" Width="130px" runat="server" Text="Model"></asp:Label>
                                    <asp:TextBox ID="txt_model1" CssClass="textbox textbox1 txtheight3" runat="server"></asp:TextBox>
                                    <asp:Label ID="lbl_size1" Width="80px" runat="server" Text="Size"></asp:Label>
                                    <asp:TextBox ID="txt_size1" Width="135px" CssClass="textbox textbox1 txtheight3"
                                        runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_size1"
                                        FilterType="UppercaseLetters,lowercaseletters,custom,numbers" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Label ID="lbl_unit1" runat="server" Text="Unit"></asp:Label>
                                    <asp:Button ID="btn_plus" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                        Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus_Click" />
                                    <asp:DropDownList ID="ddl_unit1" onfocus="return myFunction(this)" runat="server"
                                        CssClass="textbox textbox1" Width="80px" Height="30px">
                                    </asp:DropDownList>
                                    <asp:Button ID="btn_minus" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                        Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btnminus_Click" />
                                    <asp:TextBox ID="txt_unit1" Width="80px" onfocus="return myFunction(this)" Style="display: none;"
                                        CssClass="textbox textbox1" runat="server"></asp:TextBox>
                                    <span style="color: Red;">*</span>
                                    <asp:Label ID="Label2" runat="server" Width="100px" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Text=" Validity/Days"></asp:Label>
                                    <asp:TextBox ID="txt_validity" MaxLength="2" runat="server" CssClass="textbox textbox1"
                                        Height="20px" Width="50px"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="txt_validity"
                                        FilterType="Numbers,custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_department1" runat="server" Text="Department"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <fieldset style="height: 200px; width: 500px" class="spreadborder">
                                        <legend>Department
                                            <asp:Button ID="btn_addeptpartment" runat="server" Text="?" CssClass="textbox btn"
                                                OnClick="btn_addpartment_Click" />
                                        </legend>
                                        <asp:Panel ID="Panelbind" runat="server" ScrollBars="Auto" Style="height: 151px;">
                                            <asp:GridView ID="SelectdptGrid" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#0CA6CA"
                                                HeaderStyle-ForeColor="White">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="DeptCode">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_deptcode" runat="server" Text='<%# Eval("DeptCode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="DeptName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_deptname" runat="server" Text='<%# Eval("DeptName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="300px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:Label ID="lblpopspecialinstruction" runat="server" Text="Special Instruction"></asp:Label>
                                    <asp:TextBox ID="txtmulpopspecialinstruction" TextMode="MultiLine" Width="135px"
                                        runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtmulpopspecialinstruction"
                                        FilterType="UppercaseLetters,lowercaseletters,custom,numbers" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Label ID="lblpopspecfication" runat="server" Text="Specification"></asp:Label>
                                    <asp:TextBox ID="txtmulpopspecification" TextMode="MultiLine" Width="135px" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtmulpopspecification"
                                        FilterType="UppercaseLetters,lowercaseletters,custom,numbers" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Label ID="lblpopdefaultstore" runat="server" Text="Default Store"></asp:Label>
                                    <asp:DropDownList ID="ddlpopdefaultstore" CssClass="textbox textbox1" runat="server"
                                        Width="180px" Height="30px" onfocus="return myFunction(this)">
                                    </asp:DropDownList>
                                    <%-- <span style="color: Red;">*</span>--%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1">
                                    <asp:CheckBox ID="chkpophostel" runat="server" Text="For Hostel" />
                                    <asp:CheckBox ID="chkpopundersaftycondition" runat="server" Visible="false" Text="Under Safty Condition" />
                                </td>
                                <td colspan="2">
                                    <asp:RadioButton ID="rdobtnpopconsumaties" runat="server" Text="Consumables" Checked="true"
                                        GroupName="consumaties" />
                                    <asp:RadioButton ID="rdobtnpopnonconsumaties" runat="server" Text="Non Consumables"
                                        GroupName="consumaties" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <div>
                        <center>
                            <asp:Button ID="btnupdate" runat="server" Text="Update" CssClass="textbox btn2" OnClientClick="return valid()"
                                OnClick="btnupdate_Click" Visible="false" />
                            <asp:Button ID="btndelete" runat="server" Text="Delete" CssClass="textbox btn2" OnClientClick="return valid()"
                                OnClick="btndelete_Click" Visible="false" />
                            <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="textbox btn2" OnClientClick="return valid()"
                                OnClick="btnsave_Click" Visible="false" />
                            <asp:Button ID="btnexit" runat="server" Text="Exit" CssClass="textbox btn2" OnClick="btnexit_Click" />
                        </center>
                    </div>
                    <br />
                </div>
            </div>
            <div id="Newdiv" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 51px; margin-left: 331px;"
                    OnClick="imagebtnpopclose_Click" />
                <br />
                <br />
                <br />
                <center>
                    <div class="subdivstyle" style="height: 480px; width: 682px; background-color: White;">
                        <br />
                        <center>
                            <asp:Label ID="Label3" runat="server" Style="font-size: large; color: Green;" Text="Department Name"
                                Font-Bold="true"></asp:Label>
                        </center>
                        <br />
                        <div>
                            <asp:RadioButton ID="rdb_academic" runat="server" AutoPostBack="true" OnCheckedChanged="rdb_academic_CheckedChanged"
                                Text="Academic" GroupName="Dept" />
                            <asp:RadioButton ID="rdb_nonacademic" runat="server" Text="Non-Academic" AutoPostBack="true"
                                OnCheckedChanged="rdb_nonacademic_CheckedChanged" GroupName="Dept" />
                        </div>
                        <br />
                        <div class="reportdivstyle table" style="width: 514px; height: 300px;">
                            <asp:GridView ID="dptgrid" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#0CA6CA"
                                HeaderStyle-ForeColor="White">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cb_check" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DeptCode">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_deptcode" runat="server" Text='<%# Eval("DeptCode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DeptName">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_deptname" runat="server" Text='<%# Eval("DeptName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="300px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />
                        <asp:CheckBox ID="cb_selectall" runat="server" Text="Select All" AutoPostBack="true"
                            OnCheckedChanged="cbselectAll_CheckedChange" Style="margin-left: -156px; position: absolute;" />
                        <asp:Button ID="btn_deptsave" runat="server" Text="Save" CssClass="textbox btn2"
                            OnClick="btn_deptsave_Click" />
                        <asp:Button ID="btn_deptexit" runat="server" Text="Exit" CssClass="textbox btn2"
                            OnClick="btn_deptexit_Click" />
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
            <center>
                <%--    <div id="importdiv" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 51px; margin-left: 331px;"
                    OnClick="imagebtnpopclose_Click" />
                <br />
                <br />
                <br />
                <center>
                    <div class="subdivstyle" style="height: 480px; width: 682px; background-color: White;">
                        <table>
                            <tr>
                                <td>
                                </td>
                            </tr>
                        </table>
                        <center>
                            <div>
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
                            </div>
                        </center>
                    </div>
                </center>
            </div>--%>
            </center>
            <center>
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
                                        <asp:TextBox ID="txt_addgroup" runat="server" Width="200px" CssClass="textbox textbox1"
                                            onkeypress="display1()"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="line-height: 35px">
                                        <asp:Button ID="btn_addgroup1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                            CssClass="textbox btn2" OnClick="btn_addgroup_Click" />
                                        <asp:Button ID="btn_exitgroup1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                            CssClass="textbox btn2" OnClick="btn_exitaddgroup_Click" />
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
        </form>
    </body>
    </html>
</asp:Content>
